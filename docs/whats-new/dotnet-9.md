---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 02/14/2024
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is product quality across layout, control features, and reliability of tooling experiences such as setup, build, deploy, hot reload, debug, and diagnostics. To learn more about the product quality improvements in .NET MAUI 9 Preview 1, see the following release notes:

- [Release notes for .NET MAUI 9 Preview 1](https://github.com/dotnet/maui/releases/tag/untagged-e764fa1780e2dd618900)
- [Release notes for .NET iOS, tvOS, macOS, and Mac Catalyst](https://github.com/xamarin/xamarin-macios/wiki/.NET-8-release-notes)
- [Release notes for .NET Android 9 Preview 1](https://github.com/xamarin/xamarin-android/releases/tag/untagged-a2a308a67c16cf7fe691)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 8 and .NET 9, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

For information about what's new in .NET 9, see [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9).

<!--

## Upgrade from .NET 8 to .NET 9

To upgrade your projects from .NET 8 to .NET 9, install .NET 9 with the [standalone installer](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) and the `dotnet workload install maui` command.

Then, open your *.csproj* file and change the Target Framework Monikers (TFMs) from 8 to 9. If you're using a TFM such as `net8.0-ios13.6` be sure to match the platform version or remove it entirely. The following example shows the TFMs for a .NET 8 project:

The following example shows the TFMs for a .NET 8 project:

```xml
<TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
```

The following example shows the TFMs for a .NET 8 project:

```xml
<TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst;net9.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net9.0-windows10.0.19041.0</TargetFrameworks>
```

Explicit package references should also be added to your *.csproj* file for the following .NET MAUI NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="VERSION_GOES_HERE" />
    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="VERSION_GOES_HERE" />
</ItemGroup>
```

The `$(MauiVersion)` variable is referenced from the version of .NET MAUI you've installed. You can override this by adding the `$(MauiVersion)` build property to your *.csproj* file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
        <UseMaui>True</UseMaui>
        <MauiVersion>8.0.3</MauiVersion>
    </PropertyGroup>
</Project>
```

This can be useful when using ad-hoc builds from the [nightly feed](https://github.com/dotnet/maui/wiki/Nightly-Builds) or builds downloaded from pull requests.

Prior to building your upgraded app for the first time, delete the `bin` and `obj` folders.

-->

## See also

- [Announcing .NET 9 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-9-preview-1)
