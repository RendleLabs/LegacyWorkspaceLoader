using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RendleLabs.LegacyWorkspaceLoader.Internals;

namespace RendleLabs.LegacyWorkspaceLoader
{
    public class WorkspaceLoader : IWorkspaceLoader
    {
        private readonly IFileSystem _fileSystem;
        private string _frameworkDirectory = DefaultFrameworkDirectory();
        private readonly Dictionary<string, ProjectInfo> _projectInfos = new Dictionary<string, ProjectInfo>(StringComparer.OrdinalIgnoreCase);
        
        public WorkspaceLoader()
        {
            _fileSystem = new FileSystem();
        }

        internal WorkspaceLoader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void SetFrameworkDirectory(string frameworkDirectory)
        {
            _frameworkDirectory = frameworkDirectory;
        }

        public async Task<AdhocWorkspace> LoadAsync(string solutionPath)
        {
            var solution = await new SolutionLoader(solutionPath, _fileSystem).LoadAsync();
            return await BuildAsync(solution);
        }

        private async Task<AdhocWorkspace> BuildAsync(SolutionSource solutionSource)
        {
            var loader = new ProjectLoader(_fileSystem);
            
            foreach (var project in solutionSource.Projects.ToArray())
            {
                if (!await loader.LoadAsync(project))
                {
                    solutionSource.Projects.Remove(project);
                }
            }
            
            var projects = solutionSource.Projects.ToList();
            while (projects.Count > 0)
            {
                var validProjects = projects
                    .Where(p => p.ProjectReferences.All(_projectInfos.ContainsKey))
                    .ToArray();
                
                foreach (var project in validProjects)
                {
                    var info = CreateProjectInfo(project);
                    _projectInfos.Add(project.Path, info);
                    projects.Remove(project);
                }
            }
            
            var workspace = new AdhocWorkspace();

            var solutionInfo = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default, solutionSource.FilePath, _projectInfos.Values);
            workspace.AddSolution(solutionInfo);
            return workspace;
        }

        private ProjectInfo CreateProjectInfo(ProjectSource projectSource)
        {
            var projectId = ProjectId.CreateNewId();
            
            var projectReferences = projectSource.ProjectReferences
                .Select(TryGetProjectReference)
                .WhereNotNull();

            var metadataReferences = projectSource.References
                .Select(TryGetMetadataReference)
                .WhereNotNull();

            var documents = projectSource.Documents
                .SelectMany(d => CreateDocumentInfo(projectId, d))
                .WhereNotNull();

            return ProjectInfo.Create(projectId, VersionStamp.Default, projectSource.Name, projectSource.Name, LanguageNames.CSharp,
                projectSource.Path,
                projectReferences: projectReferences,
                metadataReferences: metadataReferences,
                documents: documents);
        }

        private IEnumerable<DocumentInfo> CreateDocumentInfo(ProjectId projectId, DocumentSource documentSource)
        {
            string? directoryPath = Path.GetDirectoryName(documentSource.Path);

            if(!_fileSystem.Directory.Exists(directoryPath)) yield break;

            var filePaths = _fileSystem.Directory.EnumerateFiles(directoryPath, documentSource.Name);

            foreach (var filePath in filePaths)
            {
                if (!_fileSystem.File.Exists(filePath)) continue;

                var fileName = Path.GetFileName(filePath);

                yield return DocumentInfo.Create(DocumentId.CreateNewId(projectId), fileName,
                    filePath: filePath,
                    loader: new FileTextLoader(filePath, null));
            }
        }

        private ProjectReference? TryGetProjectReference(string filePath)
        {
            if (!_projectInfos.TryGetValue(filePath, out var info)) return null;
            return new ProjectReference(info.Id);
        }

        private MetadataReference? TryGetMetadataReference(string reference)
        {
            if (!reference.Contains('\\'))
            {
                if (!reference.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    reference += ".dll";
                }
                reference = Path.Combine(_frameworkDirectory, reference);
            }

            if (!File.Exists(reference)) return null;
            
            return MetadataReference.CreateFromFile(reference, MetadataReferenceProperties.Assembly);
        }

        private static string DefaultFrameworkDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET", "Framework64", "v4.0.30319");
        }
    }
}