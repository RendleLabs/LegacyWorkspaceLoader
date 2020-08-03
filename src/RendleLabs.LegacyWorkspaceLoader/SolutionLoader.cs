using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class SolutionLoader
    {
        private readonly string _filePath;
        private readonly IFileSystem _fileSystem;
        private readonly ProjectLineParser _projectLineParser;

        public SolutionLoader(string filePath, IFileSystem? fileSystem = null)
        {
            _filePath = filePath;
            _fileSystem = fileSystem ?? new FileSystem();
            _projectLineParser = new ProjectLineParser(Path.GetDirectoryName(filePath)!);
        }

        public SolutionSource Parse(string text)
        {
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToArray();
            
            return ParseLines("", lines);
        }
        
        public async Task<SolutionSource> LoadAsync()
        {
            if (!_fileSystem.File.Exists(_filePath))
            {
                throw new FileNotFoundException("File not found", _filePath);
            }

            var lines = await _fileSystem.File.ReadAllLinesAsync(_filePath);

            return ParseLines(_filePath, lines);
        }

        private SolutionSource ParseLines(string filePath, string[] lines)
        {
            var solution = new SolutionSource(filePath);
            
            foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                if (line.StartsWith("Project"))
                {
                    if (_projectLineParser.TryParseLine(line, out var project))
                    {
                        solution.Projects.Add(project);
                    }
                }
            }

            return solution;
        }
    }
}