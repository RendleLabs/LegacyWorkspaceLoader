using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    public class SolutionLoaderTests
    {
        [Fact]
        public void CanParseSolution()
        {
	        var solution = new SolutionLoader(SolutionPath).Parse(MockHotel.Solution.Sln);
	        
	        Assert.Equal(4, solution.Projects.Count);

	        var expects = new (string name, string path)[]
	        {
		        ("Hotel", @"D:\Hotel\Hotel\Hotel.csproj"),
		        ("Hotel.Data", @"D:\Hotel\Hotel.Data\Hotel.Data.csproj"),
		        ("Hotel.Database", @"D:\Hotel\Hotel.Database\Hotel.Database.csproj"),
	        };

	        foreach (var expect in expects)
	        {
		        var project = solution.Projects.FirstOrDefault(p => p.Name == expect.name);
		        Assert.NotNull(project);
		        Assert.Equal(expect.path, project.Path);
	        }
        }

        [Fact]
        public async Task CanLoadSolution()
        {
            var file = Substitute.For<IFile>();
            file.Exists(SolutionPath).Returns(true);
            
            var lines = MockHotel.Solution.Sln.Split('\n').Select(l => l.Trim()).ToArray();
            file.ReadAllLinesAsync(SolutionPath).Returns(lines);
            
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.File.Returns(file);
            
            var loader = new SolutionLoader(SolutionPath, fileSystem);
            var solution = await loader.LoadAsync();
            
	        var expects = new (string name, string path)[]
	        {
		        ("Hotel", @"D:\Hotel\Hotel\Hotel.csproj"),
		        ("Hotel.Data", @"D:\Hotel\Hotel.Data\Hotel.Data.csproj"),
		        ("Hotel.Database", @"D:\Hotel\Hotel.Database\Hotel.Database.csproj"),
	        };

	        foreach (var expect in expects)
	        {
		        var project = solution.Projects.FirstOrDefault(p => p.Name == expect.name);
		        Assert.NotNull(project);
		        Assert.Equal(expect.path, project.Path);
	        }
        }

        private const string SolutionPath = @"D:\Hotel\Hotel.sln";
    }
}