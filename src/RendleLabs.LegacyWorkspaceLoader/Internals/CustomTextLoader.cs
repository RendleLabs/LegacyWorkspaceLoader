using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace RendleLabs.LegacyWorkspaceLoader.Internals
{
    public class CustomTextLoader : TextLoader
    {
        private readonly string _path;
        private readonly IFileSystem _fileSystem;

        public CustomTextLoader(string path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }

        public override async Task<TextAndVersion> LoadTextAndVersionAsync(Workspace workspace, DocumentId documentId, CancellationToken cancellationToken)
        {
            var text = await GetSourceText();
            var info = _fileSystem.FileInfo.FromFileName(_path);

            return TextAndVersion.Create(text, VersionStamp.Create(info.LastWriteTimeUtc));
        }

        private async Task<SourceText> GetSourceText()
        {
            var text = await _fileSystem.File.ReadAllTextAsync(_path);
            return SourceText.From(text);
        }
    }
}