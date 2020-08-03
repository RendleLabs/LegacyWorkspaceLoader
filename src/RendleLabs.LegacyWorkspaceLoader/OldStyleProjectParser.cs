using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RendleLabs.AdhocWorkspaceLoader
{
    public class OldStyleProjectParser
    {
        private readonly XElement _xml;
        private readonly string _filePath;
        private readonly XName _itemGroupName;
        private readonly XName _referenceName;
        private readonly XName _projectReferenceName;
        private readonly XName _compileName;
        private readonly XName _hintPathName;
        private readonly string _projectDirectory;
        private readonly string _projectName;

        private OldStyleProjectParser(XElement xml, string filePath, string projectDirectory, string projectName,
            XName itemGroupName, XName referenceName, XName projectReferenceName, XName compileName, XName hintPathName)
        {
            _xml = xml;
            _filePath = filePath;
            _projectDirectory = projectDirectory;
            _projectName = projectName;
            _itemGroupName = itemGroupName;
            _referenceName = referenceName;
            _projectReferenceName = projectReferenceName;
            _compileName = compileName;
            _hintPathName = hintPathName;
        }

        public static OldStyleProjectParser Create(XElement xml, string filePath)
        {
            var xmlns = xml.Attribute("xmlns")?.Value;
            if (xmlns is null) throw new InvalidOperationException("XML contains no 'xmlns' attribute");
            
            var itemGroupName = XName.Get("ItemGroup", xmlns);
            var referenceName = XName.Get("Reference", xmlns);
            var projectReferenceName = XName.Get("ProjectReference", xmlns);
            var compileName = XName.Get("Compile", xmlns);
            var hintPathName = XName.Get("HintPath", xmlns);
            
            var projectDirectory = Path.GetDirectoryName(filePath);
            var projectName = Path.GetFileNameWithoutExtension(filePath);
            
            return new OldStyleProjectParser(xml, filePath, projectDirectory, projectName,
                itemGroupName, referenceName, projectReferenceName, compileName, hintPathName);
        }

        public ProjectSource Parse()
        {
            var project = new ProjectSource(_projectName, _filePath);
            Parse(project);
            return project;
        }

        public void Parse(ProjectSource projectSource)
        {
            projectSource.References.Add("mscorlib");
            projectSource.References.AddRange(ParseReferences());
            projectSource.ProjectReferences.AddRange(ParseProjectReferences());
            projectSource.Documents.AddRange(ParseCompileItems());
        }

        private IEnumerable<DocumentSource> ParseCompileItems()
        {
            var documents = _xml
                .Elements(_itemGroupName)
                .Elements(_compileName)
                .Select(e => e.Attribute("Include")?.Value)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => Path.GetFullPath(s, _projectDirectory))
                .Select(s => new DocumentSource(Path.GetFileName(s), s));
            return documents;
        }

        private IEnumerable<string> ParseProjectReferences()
        {
            var projectReferences = _xml
                .Elements(_itemGroupName)
                .Elements(_projectReferenceName)
                .Select(e => e.Attribute("Include")?.Value)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => Path.GetFullPath(s, _projectDirectory));
            
            return projectReferences;
        }

        private IEnumerable<string> ParseReferences()
        {
            var references = _xml
                .Elements(_itemGroupName)
                .Elements(_referenceName);

            foreach (var reference in references)
            {
                if (reference.Element(_hintPathName) is XElement hintPath)
                {
                    if (!string.IsNullOrWhiteSpace(hintPath.Value))
                    {
                        yield return Path.GetFullPath(hintPath.Value, _projectDirectory);
                    }
                }

                var include = reference.Attribute("Include")?.Value;
                if (!string.IsNullOrWhiteSpace(include))
                {
                    int comma = include.IndexOf(',');
                    if (comma < 0)
                    {
                        yield return include;
                    }
                    else
                    {
                        yield return include.Substring(0, comma);
                    }
                }
            }
        }
    }
}