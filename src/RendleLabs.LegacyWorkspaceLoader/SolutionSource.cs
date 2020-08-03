using System.Collections.Generic;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class SolutionSource
    {
        public SolutionSource(string filePath)
        {
            FilePath = filePath;
            Projects = new List<ProjectSource>();
        }
        
        public string FilePath { get; }
        public List<ProjectSource> Projects { get; }
    }
}