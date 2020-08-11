using RendleLabs.LegacyWorkspaceLoader;
using Xunit;

namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    public class ProjectLineParserTests
    {
        [Fact]
        public void CanParseLine()
        {
            const string testLine = "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"Hotel\", \"Hotel\\Hotel.csproj\", \"{0AB9EB14-38A1-40FD-B093-B756E9679FE5}\"";
            
            var target = new ProjectLineParser(@"D:\Hotel");
            Assert.True(target.TryParseLine(testLine, out var project));
            Assert.Equal("Hotel", project.Name);
            Assert.Equal(@"D:\Hotel\Hotel\Hotel.csproj", project.Path);
        }
    }
}
