namespace RendleLabs.AdhocWorkspaceLoader.Tests
{
    internal static partial class MockHotel
    {
        public static class HotelDatabase
        {
            public const string Directory = @"D:\Hotel\Hotel.Database";
            public const string FilePath = @"D:\Hotel\Hotel.Database\Hotel.Database.csproj";

            public const string Csproj = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{3AD86DE0-8AFC-4A9D-AC0B-6DC7D3CCB295}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Hotel.Database</RootNamespace>
    <AssemblyName>Hotel.Database</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Net.Http"" />
    <Reference Include=""System.Xml"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""RoomData.cs"" />
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\Hotel.Data\Hotel.Data.csproj"">
      <Project>{BD1E82C9-E6F6-4833-9164-56F3D6F57F20}</Project>
      <Name>Hotel.Data</Name>
    </ProjectReference>
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>";

            public static class RoomData
            {
                public const string FilePath = @"D:\Hotel\Hotel.Database\RoomData.cs";
                public const string Code = @"using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.Data;

namespace Hotel.Database
{
    public interface IRoomData
    {
        IEnumerable<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate);
        Room GetRoom(int number);
        Room[] GetRooms(int[] numbers);
        IEnumerable<Room> AllRooms();
    }

    public class RoomData : IRoomData
    {
        private static readonly Room[] Rooms = {

            new Room
            {
                Number = 101,
                Floor = 1,
                Price = 50m
            },

            new Room
            {
                Number = 237,
                Floor = 2,
                Price = 10m
            },

            new Room
            {
                Number = 1408,
                Floor = 14,
                Price = 1m
            },
        };

        public IEnumerable<Room> AllRooms() => Rooms;

        public IEnumerable<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate)
        {
            return Rooms.AsEnumerable();
        }

        public Room GetRoom(int number)
        {
            return Rooms.FirstOrDefault(r => r.Number == number);
        }

        public Room[] GetRooms(int[] numbers)
        {
            return Rooms.Where(r => numbers.Contains(r.Number)).ToArray();
        }
    }
}
";
            }
        }
    }
}