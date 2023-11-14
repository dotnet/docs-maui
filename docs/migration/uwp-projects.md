---
title: "Xamarin.UWP project migration to .NET MAUI WinUI"
description: "Learn how to migrate a Xamarin.UWP project to a .NET WinUI project."
ms.date: 11/13/2023
content_well_notification: 
  - AI-contribution
---

# Xamarin.UWP project migration to .NET MAUI WinUI

## Prerequisites

- Visual Studio 2023 Preview or later with and ensure that .NET Multi-platform App UI development is selected.
- The latest version of the Windows App SDK extension for Visual Studio.
- A Xamarin.Forms UWP app that targets Windows 10 version 1809 or later.

## Project Changes

A .NET 8 project for a .NET MAUI WinUI app is similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType> <!-- in Xamarin.Forms this was AppContainerExe -->
    <UseWinUI>true</UseWinUI>
    <UseMaui>true</UseMaui>
    <EnableMsixTooling>true</EnableMsixTooling>
    <Platforms>x86;x64;ARM64</Platforms>
  </PropertyGroup>
</Project>
```

The `<OutputType>` property was previously AppContainerExe in Xamarin.UWP propjects. For a library project, omit the `$(OutputType)` property completely or specify `Library` as the property value.

## Changes to MSBuild properties

Platform properties should be replaced in favor of the following .NET runtime identifiers:

```xml
<PropertyGroup>
  <!-- Used in .NET MAUI WinUI projects -->
   <Platforms>x86;x64;ARM64</Platforms>
</PropertyGroup>
```

For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

## API Changes

To safely use recent or older APIs, you can declare a `SupportedOSPlatformVersion` in your project or use the <xref:System.OperatingSystem.IsWindowsVersionAtLeast%2A> API at runtime:

```csharp
if (OperatingSystem.IsWindowsVersionAtLeast(10))
{
    // Use the API here
}
```

Address any API changes that may affect your application.  For example, some methods and properties may have been renamed, deprecated or removed.

### Namespace Changes

Update the namespaces in your code files, following the changes between the following two pieces of documentation: [UWP Namespaces](/uwp/api/) and [WinUI Namespaces](/windows/winui/api/).

For example, you will need to replace `Windows.UI.Xaml` with `Microsoft.UI.Xaml`, and `Windows.UI.Xaml.Controls` with `Microsoft.UI.Xaml.Controls`.

### File Changes

#### Deprecated files

Migrate your business logic from the following files into other files and remove them:

- MainPage.xaml/MainPage.xaml.cs
- AssemblyInfo.cs
  - See [AssemblyInfo changes](includes/assemblyinfo-changes.md)
- Default.rd.xml
These files are no longer needed for .NET MAUI WinUI application.

#### Files to add

Use this [.NET MAUI Multi Head Application](https://github.com/mattleibow/MauiMultiHeadProject/tree/main/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI) project as a sample for the following files when adding them to your upgraded project:

- MauiProgram.cs
- App.xaml/ App.xaml.cs
  - The old Xamarin.UWP project starts with an app.xaml/.cs file. Migrate your business logic over to the Maui version of the file. There exists more resources [here](https://github.com/dotnet/maui/wiki/Migrating-from-Xamarin.Forms-to-.NET-MAUI) for manually moving resources from Xamarin.Forms projects to NET MAUI projects.
- launchSettings.json
These files are required to get your .NET MAUI WinUI application up and running.

#### Package.appxmanifest changes

1. Set the application entry point to `$targetentrypoint$`
2. Add the `runFullTrust` capability
3. Add the `Windows.Universal` and `Windows.Desktop` target device families

```
> [!NOTE]
> This step is crucial if you are unable to deploy or seeing deploy errors (not build errors) or are unable to select the deploy checkbox in the Configuration manager. Adding the targetDevice and the other updated to Package.appxmanifest fixes the most common errors. 
```

## Runtime behavior

There are behavioral changes to the `String.IndexOf()` method in .NET 5+ on different platforms. For more information, see [.NET globalization and ICU](/dotnet/standard/globalization-localization/globalization-icu).

## Next steps

Build and test your application to identify any UI differences between Xamarin.Forms and .NET MAUI.

You can now enjoy the benefits that .NET MAUI offers such as improved performance and modernized UI controls.

## See also

- [Upgrade into a Multi Project Application](/dotnet/maui/migration/multi-project-to-multi-project)
- [Upgrade into a Single Project](/dotnet/maui/migration/multi-project-to-multi-project)
