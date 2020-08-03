using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class ProjectLineParser
    {
        private readonly string _solutionDirectory;

        public ProjectLineParser(string solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
        }

        public bool TryParseLine(ReadOnlySpan<char> line, [NotNullWhen(true)] out ProjectSource? project)
        {
            project = null;

            if (!TrySkipPastChar(ref line, '=')) return false;
            
            if (!TryGetNextQuotedString(ref line, out var projectName)) return false;

            if (!TryGetNextQuotedString(ref line, out var projectFilePath)) return false;
            
            project = new ProjectSource(projectName, Path.GetFullPath(projectFilePath, _solutionDirectory));
            return true;
        }

        private static bool TryGetNextQuotedString(ref ReadOnlySpan<char> line, [NotNullWhen(true)] out string? value)
        {
            value = null;

            if (!TrySkipPastChar(ref line, '"')) return false;

            if (!TryReadToChar(ref line, '"', out var valueSpan)) return false;
            
            value = new string(valueSpan);
            
            return true;
        }
        
        private static bool TrySkipPastChar(ref ReadOnlySpan<char> line, char ch)
        {
            var index = line.IndexOf(ch);
            if (index < 0) return false;
            line = line.Slice(index + 1);
            return true;
        }

        private static bool TryReadToChar(ref ReadOnlySpan<char> line, char ch, out ReadOnlySpan<char> value)
        {
            var index = line.IndexOf(ch);
            if (index < 0)
            {
                value = default;
                return false;
            }

            value = line.Slice(0, index);
            line = line.Slice(index + 1);
            return true;
        }
    }
}
