using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace RendleLabs.LegacyWorkspaceLoader
{
    public class NewStyleProjectParser
    {
        public async Task ParseAsync(ProjectSource projectSource)
        {
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(projectSource.Path);
            
            ReadDocuments(projectSource, project);
            
            ReadReferences(projectSource, project);

            ReadProjectReferences(projectSource, project, workspace);
        }

        private static void ReadProjectReferences(ProjectSource projectSource, Project project, MSBuildWorkspace workspace)
        {
            foreach (var projectReference in project.ProjectReferences)
            {
                var reffedProject = workspace.CurrentSolution.GetProject(projectReference.ProjectId);
                if (!(reffedProject?.FilePath is null))
                {
                    projectSource.ProjectReferences.Add(reffedProject.FilePath);
                }
            }
        }

        private static void ReadReferences(ProjectSource projectSource, Project project)
        {
            foreach (var metadataReference in project.MetadataReferences.OfType<PortableExecutableReference>())
            {
                if (!string.IsNullOrWhiteSpace(metadataReference.FilePath))
                {
                    projectSource.References.Add(metadataReference.FilePath);
                }
            }
        }

        private static void ReadDocuments(ProjectSource projectSource, Project project)
        {
            foreach (var document in project.Documents.Where(d => d.SupportsSemanticModel))
            {
                if (!string.IsNullOrWhiteSpace(document.FilePath))
                {
                    projectSource.Documents.Add(new DocumentSource(document.Name, document.FilePath));
                }
            }
        }
    }
}