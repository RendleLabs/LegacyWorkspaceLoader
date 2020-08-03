namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    internal static partial class MockHotel
    {
        public static class HotelFeature
        {
            public const string FilePath = @"D:\Hotel\Hotel.Feature\Hotel.Feature.csproj";

            public const string Csproj = @"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
  </PropertyGroup>

</Project>
";

            public static class Feature
            {
                public const string FilePath = @"D:\Hotel\Hotel.Feature\Feature.cs";

                public const string Code = @"using System;

namespace Hotel.Feature
{
    public class HotelFeature
    {
        public string Name { get; set; } = ""Foo"";
    }
}
";
            }
        }
    }
}