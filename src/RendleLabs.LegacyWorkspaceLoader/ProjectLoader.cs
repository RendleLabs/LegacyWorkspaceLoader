using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class ProjectLoader
    {
        private readonly IFileSystem _fileSystem;

        public ProjectLoader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<ProjectSource> Parse(string projectFilePath, string text)
        {
            var name = Path.GetFileNameWithoutExtension(projectFilePath);
            return ParseAsync(new ProjectSource(name, projectFilePath), text);
        }

        public async Task<bool> LoadAsync(ProjectSource projectSource)
        {
            if (!_fileSystem.File.Exists(projectSource.Path)) return false;

            var text = await _fileSystem.File.ReadAllTextAsync(projectSource.Path);

            await ParseAsync(projectSource, text);

            return true;
        }

        private static async Task<ProjectSource> ParseAsync(ProjectSource projectSource, string text)
        {
            var xml = XDocument.Parse(text);

            if (xml.Root?.Attribute("xmlns")?.Value is string)
            {
                OldStyleProjectParser.Create(xml.Root, projectSource.Path).Parse(projectSource);
                return projectSource;
            }

            await ParseNewStyleProject(projectSource);
            return projectSource;
        }

        private static async Task ParseNewStyleProject(ProjectSource projectSource)
        {
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(projectSource.Path);
            
            foreach (var document in project.Documents.Where(d => d.SupportsSemanticModel))
            {
                if (!string.IsNullOrWhiteSpace(document.FilePath))
                {
                    projectSource.Documents.Add(new DocumentSource(document.Name, document.FilePath));
                }
            }
            
            foreach (var metadataReference in project.MetadataReferences.OfType<PortableExecutableReference>())
            {
                if (!string.IsNullOrWhiteSpace(metadataReference.FilePath))
                {
                    projectSource.References.Add(metadataReference.FilePath);
                }
            }

            foreach (var projectReference in project.ProjectReferences)
            {
                var reffedProject = workspace.CurrentSolution.GetProject(projectReference.ProjectId);
                if (!(reffedProject?.FilePath is null))
                {
                    projectSource.ProjectReferences.Add(reffedProject.FilePath);
                }
            }
        }
    }
}