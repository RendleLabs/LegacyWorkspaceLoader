using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    public static class Mocks
    {
        public static IFileSystem MockFileSystem()
        {
            var file = MockFile();

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.File.Returns(file);
            return fileSystem;
        }

        private static IFile MockFile()
        {
            var file = Substitute.For<IFile>();
            
            MockSolutionPaths(file);

            MockProjectPaths(file);

            return file;
        }

        private static void MockProjectPaths(IFile file)
        {
            MockHotelProjectPaths(file);

            MockHotelDataProjectPaths(file);

            MockHotelDatabaseProjectPaths(file);
            
            MockHotelFeatureProjectPaths(file);
        }

        private static void MockHotelProjectPaths(IFile file)
        {
            file.Exists(MockHotel.Hotel.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.Hotel.FilePath).Returns(MockHotel.Hotel.Csproj);
            
            file.Exists(MockHotel.Hotel.HotelServiceSvc.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.Hotel.HotelServiceSvc.FilePath).Returns(MockHotel.Hotel.HotelServiceSvc.Code);
            
            file.Exists(MockHotel.Hotel.IHotelService.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.Hotel.IHotelService.FilePath).Returns(MockHotel.Hotel.IHotelService.Code);
            
            MockAssemblyInfo(file, MockHotel.Hotel.Directory);
        }

        private static void MockHotelDataProjectPaths(IFile file)
        {
            file.Exists(MockHotel.HotelData.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelData.FilePath).Returns(MockHotel.HotelData.Csproj);
            
            file.Exists(MockHotel.HotelData.Room.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelData.Room.FilePath).Returns(MockHotel.HotelData.Room.Code);
            
            MockAssemblyInfo(file, MockHotel.HotelData.Directory);
        }

        private static void MockHotelDatabaseProjectPaths(IFile file)
        {
            file.Exists(MockHotel.HotelDatabase.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelDatabase.FilePath).Returns(MockHotel.HotelDatabase.Csproj);
            
            file.Exists(MockHotel.HotelDatabase.RoomData.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelDatabase.RoomData.FilePath).Returns(MockHotel.HotelDatabase.RoomData.Code);

            MockAssemblyInfo(file, MockHotel.HotelDatabase.Directory);
        }

        private static void MockHotelFeatureProjectPaths(IFile file)
        {
            file.Exists(MockHotel.HotelFeature.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelFeature.FilePath).Returns(MockHotel.HotelFeature.Csproj);
            
            file.Exists(MockHotel.HotelFeature.Feature.FilePath).Returns(true);
            file.ReadAllTextAsync(MockHotel.HotelFeature.Feature.FilePath).Returns(MockHotel.HotelFeature.Feature.Code);
        }

        private static void MockSolutionPaths(IFile file)
        {
            file.Exists(MockHotel.Solution.FilePath).Returns(true);
            var solutionLines = MockHotel.Solution.Sln.Split('\n').Select(l => l.Trim()).ToArray();
            file.ReadAllLinesAsync(MockHotel.Solution.FilePath).Returns(solutionLines);
        }

        private static void MockAssemblyInfo(IFile file, string directory)
        {
            var assemblyInfo = Path.Combine(directory, "Properties", "AssemblyInfo.cs");
            file.Exists(assemblyInfo).Returns(true);
            file.ReadAllTextAsync(assemblyInfo).Returns(MockHotel.AssemblyInfoCs);
        }
    }
}