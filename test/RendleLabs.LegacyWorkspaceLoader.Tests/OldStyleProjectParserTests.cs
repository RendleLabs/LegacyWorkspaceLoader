using System.Xml.Linq;
using Xunit;

namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    public class OldStyleProjectParserTests
    {
        [Fact]
        public void CanParseWcfProject()
        {
            var xml = XDocument.Parse(MockHotel.Hotel.Csproj);
            var target = OldStyleProjectParser.Create(xml.Root!, ProjectPath);
            var actual = target.Parse();
            
            Assert.Equal("Hotel", actual.Name);
            Assert.Equal(ProjectPath, actual.Path);
            
            Assert.Equal(3, actual.Documents.Count);

            foreach (var name in new[]{"HotelService.svc.cs", "IHotelService.cs"})
            {
                Assert.Contains(actual.Documents,
                    document => document.Name == name && document.Path == $@"D:\Hotel\Hotel\{name}");
            }

            Assert.Contains(actual.Documents,
                document => document.Name == "AssemblyInfo.cs" && document.Path == @"D:\Hotel\Hotel\Properties\AssemblyInfo.cs");

            Assert.Contains("System.ServiceModel", actual.References);
            Assert.Contains(@"D:\Hotel\packages\Newtonsoft.Json.12.0.3\lib\net45\Newtonsoft.Json.dll", actual.References);

            Assert.Contains(@"D:\Hotel\Hotel.Data\Hotel.Data.csproj", actual.ProjectReferences);
            Assert.Contains(@"D:\Hotel\Hotel.Database\Hotel.Database.csproj", actual.ProjectReferences);
        }

        private const string ProjectPath = @"D:\Hotel\Hotel\Hotel.csproj";
    }
}