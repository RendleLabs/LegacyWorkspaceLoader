namespace RendleLabs.LegacyWorkspaceLoader
{
    public class DocumentSource
    {
        public DocumentSource(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }
        public string Path { get; }
    }
}