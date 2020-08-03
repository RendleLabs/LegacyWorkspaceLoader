using System;
using System.Collections.Generic;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class ProjectSource
    {
        public ProjectSource(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }
        public string Path { get; }
        
        public List<DocumentSource> Documents { get; } = new List<DocumentSource>();
        
        public HashSet<string> References { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        public HashSet<string> ProjectReferences { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}