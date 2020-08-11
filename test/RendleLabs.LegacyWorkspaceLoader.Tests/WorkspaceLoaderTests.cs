using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using RendleLabs.LegacyWorkspaceLoader;
using Xunit;

namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    public class WorkspaceLoaderTests
    {
        static WorkspaceLoaderTests()
        {
            MSBuildLocator.RegisterDefaults();
        }
        
        [Fact]
        public async Task CanLoadSolution()
        {
            var fileSystem = Mocks.MockFileSystem();

            var loader = new WorkspaceLoader(fileSystem);

            var workspace = await loader.LoadAsync(MockHotel.Solution.FilePath);
            
            Assert.NotNull(workspace.CurrentSolution);

            var projects = workspace.CurrentSolution.Projects.ToArray();
            Assert.Equal(3, projects.Length);

            var hotelProject = projects.FirstOrDefault(p => p.Name == "Hotel");
            Assert.NotNull(hotelProject);

            var compilation = await hotelProject.GetCompilationAsync();
            Assert.NotNull(compilation);

            var hotelServiceSymbol = compilation.GetTypeByMetadataName("Hotel.IHotelService");
            Assert.NotNull(hotelServiceSymbol);

            var attribute = hotelServiceSymbol.GetAttributes().FirstOrDefault()?.AttributeClass;
            Assert.NotNull(attribute);
            Assert.Equal("ServiceContractAttribute", attribute.Name);
            Assert.Equal("System.ServiceModel", attribute.ContainingNamespace.ToString());
        }
    }
}