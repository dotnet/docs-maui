---
title: What's new in .NET MAUI for .NET 11
description: Learn about the new features introduced in .NET MAUI for .NET 11.
ms.date: 03/10/2026
---

# What's new in .NET MAUI for .NET 11

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 11 is to improve product quality. For information about what's new in each .NET MAUI in .NET 11 release, see the following release notes:

<!-- markdownlint-disable-next-line MD042 -->
- [.NET MAUI in .NET 11 Preview 1]()

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 11, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

## Feature

This is a description of the feature and essential code snippets to adopt it.

## Controls

.NET MAUI in .NET 11 includes control enhancements and deprecations.

### Map pin clustering

The <xref:Microsoft.Maui.Controls.Maps.Map> control now supports pin clustering, which automatically groups nearby pins into cluster markers at lower zoom levels. This long-requested feature is supported on Android and iOS/Mac Catalyst.

New APIs:

- `Map.IsClusteringEnabled` — enables or disables pin clustering.
- `Pin.ClusteringIdentifier` — groups pins into named clusters for independent clustering.
- `Map.ClusterClicked` — event fired when a cluster marker is tapped.
- `ClusterClickedEventArgs` — provides the list of pins in the cluster, the cluster location, and a `Handled` property to suppress default zoom behavior.

```xaml
<maps:Map IsClusteringEnabled="True"
          ClusterClicked="OnClusterClicked" />
```

For more information, see [Pin clustering](~/user-interface/controls/map.md?view=net-maui-11.0&preserve-view=true#pin-clustering).

> [!div class="nextstepaction"]
> [Explore the sample](/samples/dotnet/maui-samples/userinterface-map-clustering)

## Platform features

.NET MAUI's platform features have received some updates in .NET 11.

### `maui` CLI

.NET MAUI 11 introduces the `maui` CLI, a unified command-line tool for managing your development environment. Key commands include `maui doctor`, `maui android install`, and `maui apple install`, with structured `--json` output for CI/CD and AI agent integration.

For more information, see [.NET MAUI CLI reference](~/get-started/maui-cli.md?view=net-maui-11.0&preserve-view=true).

## .NET for Android

.NET for Android in .NET 11 makes CoreCLR the default runtime for `Release` builds, and includes work to improve performance. For more information about .NET for Android in .NET 10, see the following release notes:

- [.NET for Android 11 Preview 1](https://github.com/dotnet/android/releases/)

### Feature

## CoreCLR by Default

CoreCLR is now the default runtime for `Release` builds. This should
improve compatibility with the rest of .NET as well as shorter startup
times, with a reasonable increase to application size.

We are always working to improve performance and app size, but please
file issues with stability or concerns by filing
[issues on GitHub](https://github.com/dotnet/android/issues).

If you would like to opt out of CoreCLR, and use the Mono runtime
instead, you can still do so via:

```xml
<PropertyGroup>
  <UseMonoRuntime>true</UseMonoRuntime>
</ProperyGroup>
```

## `dotnet run`

We have enhanced the .NET CLI with [Spectre.Console](https://spectreconsole.net/) to *prompt* when a selection is needed for `dotnet run`.

So, for multi-targeted projects like .NET MAUI, it will:

* Prompt for a `$(TargetFramework)`
* Prompt for a device, emulator, simulator if there are more than one.

Console output of your application should appear directly in the terminal, and Ctrl+C will terminate the application.

![GIF of `dotnet run` selections on Windows for Android](media/dotnet-11/dotnet-run-android-preview-1.gif)

![GIF of `dotnet run` selections on macOS for iOS](media/dotnet-11/dotnet-run-ios-preview-1.gif)

## .NET for iOS

.NET 11 on iOS, tvOS, Mac Catalyst, and macOS supports the following platform versions:

- iOS: 18.2
- tvOS: 18.2
- Mac Catalyst: 18.2
- macOS: 15.2

For more information about .NET 11 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 11.0.1xx Preview 1](https://github.com/dotnet/macios/releases/)

For information about known issues, see [Known issues in .NET 11](https://github.com/dotnet/macios/wiki/Known-issues-in-.NET11).

### Feature

Description

## See also

- [.NET MAUI roadmap](https://github.com/dotnet/maui/wiki/Roadmap)
- [What's new in .NET 11](/dotnet/core/whats-new/dotnet-11/overview)
