using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Xunit;

namespace RendleLabs.LegacyWorkspaceLoader.IntegrationTests
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
            var loader = new WorkspaceLoader();

            var workspace = await loader.LoadAsync(@"D:\RendleLabs\LegacyWorkspaceLoader\data\Hotel\Hotel.sln");
            
            Assert.NotNull(workspace.CurrentSolution);

            var projects = workspace.CurrentSolution.Projects.ToArray();
            Assert.Equal(5, projects.Length);

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

            var featureProject = projects.FirstOrDefault(p => p.Name == "Hotel.Feature");
            Assert.NotNull(featureProject);

            compilation = await featureProject.GetCompilationAsync();
            Assert.NotNull(compilation);

            var featureSymbol = compilation.GetTypeByMetadataName("Hotel.Feature.HotelFeature");
            Assert.NotNull(featureSymbol);
        }
    }
}