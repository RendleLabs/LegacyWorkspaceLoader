using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RendleLabs.LegacyWorkspaceLoader
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
            
            var newStyleProjectParser = new NewStyleProjectParser();
            await newStyleProjectParser.ParseAsync(projectSource);
            return projectSource;
        }
    }
}