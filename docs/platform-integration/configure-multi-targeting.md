---
title: "Configure .NET MAUI multi-targeting"
description: "Learn how to configure multi-targeting in a .NET MAUI app, based on your own filename and folder criteria."
ms.date: 08/19/2022
---

# Configure multi-targeting

.NET Multi-platform App UI (.NET MAUI) apps use multi-targeting to target multiple platforms from a single project.

The project for a .NET MAUI app contains a _Platforms_ folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/configure-multi-targeting/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each target platform contain platform-specific code that starts the app on each platform, plus any additional platform code you add. At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the _Platforms_ > _Android_ folder will be built into the app package, but the files in the other _Platforms_ folders won't be.

In addition to this default multi-targeting approach, .NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into sub-folders of the _Platforms_ folder.

## Configure filename-based multi-targeting

A standard multi-targeting pattern is to include the platform as an extension in the filename for the platform code. For example, _MyService.Android.cs_ would represent an Android-specific implementation of the `MyService` class. The build system can be configured to use this pattern by adding the following XML to your .NET MAUI app project (.csproj) file as children of the `<Project>` node:

```xml
<!-- Android -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-android')) != true">
  <Compile Remove="**\*.Android.cs" />
  <None Include="**\*.Android.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Both iOS and Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true AND $(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\*.MaciOS.cs" />
  <None Include="**\*.MaciOS.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- iOS -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true">
  <Compile Remove="**\*.iOS.cs" />
  <None Include="**\*.iOS.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\*.MacCatalyst.cs" />
  <None Include="**\*.MacCatalyst.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Windows -->
<ItemGroup Condition="$(TargetFramework.Contains('-windows')) != true">
  <Compile Remove="**\*.Windows.cs" />
  <None Include="**\*.Windows.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>
```

This XML configures the build system to remove platform-based filename patterns under specific conditions:

- Don't compile C# code whose filename ends with _.Android.cs_, if you aren't building for Android.
- Don't compile C# code whose filename ends with _.MaciOS.cs_, if you aren't building for iOS and Mac Catalyst.
- Don't compile C# code whose filename ends with _.iOS.cs_, if you aren't building for iOS.
- Don't compile C# code whose filename ends with _.MacCatalyst.cs_, if you aren't building for Mac Catalyst.
- Don't compile C# code whose filename ends with _.Windows.cs_, if you aren't building for Windows.

> [!IMPORTANT]
> Filename-based multi-targeting can be combined with folder-based multi-targeting. For more information, see [Combine filename and folder multi-targeting](#combine-filename-and-folder-multi-targeting).

## Configure folder-based multi-targeting

Another standard multi-targeting pattern is to include the platform as a folder name. For example, a folder named _Android_ would contain Android-specific code. The build system can be configured to use this pattern by adding the following XML to your .NET MAUI app project (.csproj) file as children of the `<Project>` node:

```xml
<!-- Android -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-android')) != true">
  <Compile Remove="**\Android\**\*.cs" />
  <None Include="**\Android\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Both iOS and Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true AND $(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\MaciOS\**\*.cs" />
  <None Include="**\MaciOS\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- iOS -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true">
  <Compile Remove="**\iOS\**\*.cs" />
  <None Include="**\iOS\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\MacCatalyst\**\*.cs" />
  <None Include="**\MacCatalyst\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Windows -->
<ItemGroup Condition="$(TargetFramework.Contains('-windows')) != true">
  <Compile Remove="**\Windows\**\*.cs" />
  <None Include="**\Windows\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>
```

This XML configures the build system to remove platform-based folder patterns under specific conditions:

- Don't compile C# code that's located in the _Android_ folder, or sub-folder of the _Android_ folder, if you aren't building for Android.
- Don't compile C# code that's located in the _MaciOS_ folder, or sub-folder of the _MaciOS_ folder, if you aren't building for iOS and Mac Catalyst.
- Don't compile C# code that's located in the _iOS_ folder, or sub-folder of the _iOS_ folder, if you aren't building for iOS.
- Don't compile C# code that's located in the _MacCatalyst_ folder, or sub-folder of the _MacCatalyst_ folder, if you aren't building for Mac Catalyst.
- Don't compile C# code that's located in the _Windows_ folder, or sub-folder of the _Windows_ folder, if you aren't building for Windows.

> [!IMPORTANT]
> Folder-based multi-targeting can be combined with filename-based multi-targeting. For more information, see [Combine filename and folder multi-targeting](#combine-filename-and-folder-multi-targeting).

## Combine filename and folder multi-targeting

Filename-based multi-targeting can be combined with folder-based multi-targeting if required. The build system can be configured to use this pattern by adding the following XML to your .NET MAUI app project (.csproj) file as children of the `<Project>` node:

```xml
<!-- Android -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-android')) != true">
  <Compile Remove="**\*.Android.cs" />
  <None Include="**\*.Android.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\Android\**\*.cs" />
  <None Include="**\Android\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />  
</ItemGroup>

<!-- Both iOS and Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true AND $(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\*.MaciOS.cs" />
  <None Include="**\*.MaciOS.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\MaciOS\**\*.cs" />
  <None Include="**\MaciOS\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- iOS -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true">
  <Compile Remove="**\*.iOS.cs" />
  <None Include="**\*.iOS.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\iOS\**\*.cs" />
  <None Include="**\iOS\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />  
</ItemGroup>

<!-- Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\*.MacCatalyst.cs" />
  <None Include="**\*.MacCatalyst.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\MacCatalyst\**\*.cs" />
  <None Include="**\MacCatalyst\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Windows -->
<ItemGroup Condition="$(TargetFramework.Contains('-windows')) != true">
  <Compile Remove="**\*.Windows.cs" />
  <None Include="**\*.Windows.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\Windows\**\*.cs" />
  <None Include="**\Windows\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />  
</ItemGroup>
```

This XML configures the build system to remove platform-based filename and folder patterns under specific conditions:

- Don't compile C# code whose filename ends with _.Android.cs_, or that's located in the _Android_ folder or sub-folder of the _Android_ folder, if you aren't building for Android.
- Don't compile C# code whose filename ends with _.MaciOS.cs_, or that's located in the _MaciOS_ folder or sub-folder of the _MaciOS_ folder, if you aren't building for iOS and Mac Catalyst.
- Don't compile C# code whose filename ends with _.iOS.cs_, or that's located in the _iOS_ folder or sub-folder of the _iOS_ folder, if you aren't building for iOS.
- Don't compile C# code whose filename ends with _.MacCatalyst.cs_, or that's located in the _MacCatalyst_ folder or sub-folder of the _MacCatalyst_ folder, if you aren't building for Mac Catalyst.
- Don't compile C# code whose filename ends with _.Windows.cs_, or that's located in the _Windows_ folder or sub-folder of the _Windows_ folder, if you aren't building for Windows.
