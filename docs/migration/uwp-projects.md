---
title: "Xamarin.Forms UWP project migration"
description: "Learn how to migrate a Xamarin.Forms UWP project to a WinUI 3 project."
ms.date: 12/20/2023
---

# Xamarin.Forms UWP project migration

A .NET 8 project for a .NET MAUI WinUI 3 app is similar to the following example:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType> <!-- in Xamarin.Forms this was AppContainerExe -->
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <RootNamespace>MyApp.WinUI</RootNamespace>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <Platforms>x86;x64;ARM64</Platforms>
    <RuntimeIdentifiers>win10-x86;win10-x64;win10-arm64</RuntimeIdentifiers>
    <UseWinUI>true</UseWinUI>
    <EnableMsixTooling>true</EnableMsixTooling>
    <UseMaui>true</UseMaui>
    <!-- We do not want XAML files to be processed as .NET MAUI XAML -->
    <EnableDefaultMauiItems>false</EnableDefaultMauiItems>
  </PropertyGroup>
</Project>
```

> [!IMPORTANT]
> You must add `<UseMaui>true</UseMaui>` to your project file to enable .NET MAUI support. In addition, ensure you've added `<EnableDefaultMauiItems>false</EnableDefaultMauiItems>` to the project file. This will stop you receiving build errors about the `InitializeComponent` method already being defined.

## Changes to MSBuild properties

While upgrading your project, it's recommended to remove the following UWP MSBuild properties from your project file:

- `WindowsXamlEnableOverview`
- `AppxPackageSigningEnabled`
- `GenerateAssemblyInfo`

You'll also need to ensure that the platform architectures in the target project are specified with the following .NET runtime identifiers:

```xml
<PropertyGroup>
   <!-- Used in .NET MAUI WinUI projects -->
   <Platforms>x86;x64;ARM64</Platforms>
</PropertyGroup>
```

For more information about runtime identifiers, see [.NET RID Catalog](/dotnet/core/rid-catalog).

## Add package references

[!INCLUDE [Add package references](includes/net8-maui-nugets.md)]

## Namespace changes

There are differences in the names of namespaces between UWP and WinUI 3. In many cases it's as easy as changing a namespace name and then your code will compile. For example, you'll need to replace the `Windows.UI.Xaml` namespace with the `Microsoft.UI.Xaml` namespace. Similarly, you'll need to replace the `Windows.UI.Xaml.Controls` namespace with the `Microsoft.UI.Xaml.Controls` namespace.

Other times, the mapping takes a bit more work, and in rare cases requires a change in approach. For more information, see [Mapping UWP APIs and libraries to the Windows App SDK](/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/api-mapping-table).

## API changes

You'll need to address any API changes that may affect your app. For example, some types, methods, and properties may have been renamed, deprecated or removed. For information on what's supported when upgrading your UWP project to WinUI 3, see [What's supported when migrating from UWP to WinUI 3](/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/what-is-supported). For information about mapping UWP features and APIs to WinUI 3, see [Mapping UWP features to the Windows App SDK](/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/feature-mapping-table) and [Mapping UWP APIs and libraries to the Windows App SDK](/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/api-mapping-table).

Your project can be made compatible with earlier OS versions by setting the `$(SupportedOSPlatformVersion)` property in your project file:

```xml
<PropertyGroup>
   <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.19041.0</SupportedOSPlatformVersion>
</PropertyGroup>
```

The `$(SupportedOSPlatformVersion)` property indicates the minimum OS version required to run your app or library. If you don't explicitly specify this minimum runtime OS version in your project, it defaults to the platform version from the Target Framework Moniker (TFM).

If your project only targets Windows, it's sufficient to omit the platform checking condition and set the `$(SupportedOSPlatformVersion)` property directly:

```xml
<PropertyGroup>
   <SupportedOSPlatformVersion>10.0.19041.0</SupportedOSPlatformVersion>
</PropertyGroup>
```

For more information about the `$(SupportedOSPlatformVersion)` property, see [Support older OS versions](/dotnet/standard/frameworks#support-older-os-versions).

For an app to run correctly on an older OS version, it can't call APIs that don't exist on that version of the OS. However, you can add guards around calls to newer APIs so they are only called when running on a version of the OS that supports them. This can be achieved with the <xref:System.OperatingSystem.IsWindowsVersionAtLeast%2A> method:

```csharp
if (OperatingSystem.IsWindowsVersionAtLeast(10))
{
    // Use the API here
}
```

## Remove files

[!INCLUDE [Remove files](includes/uwp-remove-files.md)]

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

## Update files

[!INCLUDE [Update files](includes/uwp-update-files.md)]

These files are required to bootstrap your .NET MAUI WinUI 3 project.

## Changes to Package.appxmanifest

[!INCLUDE [Changes to Package.appxmanifest](includes/uwp-change-package-manifest.md)]

## Runtime behavior

There are behavioral changes to the `String.IndexOf()` method in .NET 5+ on different platforms. For more information, see [.NET globalization and ICU](/dotnet/standard/globalization-localization/globalization-icu).

## See also

- [Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app](multi-project-to-multi-project.md)
- [Manually upgrade a Xamarin.Forms app to a single project .NET MAUI app](multi-project-to-single-project.md)
