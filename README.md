# LegacyWorkspaceLoader

Library to create Roslyn AdhocWorkspaces from old-style .NET solutions and projects.

![.NET Core](https://github.com/RendleLabs/LegacyWorkspaceLoader/workflows/.NET%20Core/badge.svg)
[![NuGet version (RendleLabs.LegacyWorkspaceLoader)](https://img.shields.io/nuget/v/RendleLabs.LegacyWorkspaceLoader.svg?style=flat-square)](https://www.nuget.org/packages/RendleLabs.LegacyWorkspaceLoader/)

## Usage

To create an [AdhocWorkspace](https://docs.microsoft.com/dotnet/api/microsoft.codeanalysis.adhocworkspace?view=roslyn-dotnet) from an on-disk
solution file:

```csharp
var loader = new WorkspaceLoader();
var workspace = await loader.LoadAsync(@"D:\Code\My.sln");
```

The workspace should support compilations, semantic models and all that
goodness.

## Why?

You can't use `MSBuildWorkspace` to load old-style .NET 4.x projects in a .NET Core 3.1
application, because it tries to resolve various build targets and it can't load the assemblies
for them, so you get errors like this:

```text
Msbuild failed when processing the file 'D:\ReCode\Samples\Hotel\Hotel\Hotel.csproj' with message: 
C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\Microsoft.Common.CurrentVersion.targets: (1489, 5): 
The "AssignProjectConfiguration" task could not be instantiated from 
"Microsoft.Build.Tasks.Core, Version=15.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a".
Method not found: 'Byte[] System.AppDomainSetup.GetConfigurationBytes()'.
```

What you can do is just parse the Solution and Project files and build an
[AdhocWorkspace](https://docs.microsoft.com/dotnet/api/microsoft.codeanalysis.adhocworkspace?view=roslyn-dotnet)
with the code documents, and use that instead.

So that's what LegacyWorkspaceLoader does.

## Notes

If your solution contains a mix of old-style and new-style project files,
the new-style ones will be loaded using the .NET Core variant of
`MSBuildWorkspace` and then "cloned" into the `AdhocWorkspace`.
