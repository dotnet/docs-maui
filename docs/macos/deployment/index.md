---
title: "Publish a .NET MAUI app for macOS"
description: "Learn how to package and publish a macOS .NET MAUI app."
ms.date: 04/25/2022
---

# Publish a .NET MAUI app for macOS

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for iOS](../../ios/deployment/index.md)
> - [Publish for macOS](overview.md)
> - [Publish for Windows](../../windows/deployment/overview.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for macOS, you generate an *.app* or a *.pkg* file. An *.app* file is a self-contained app that can be run without installation, whereas a *.pkg* is an app packaged in an installer.

> [!IMPORTANT]
> Blazor Hybrid apps require a WebView on the host platform. For more information, see [Keep the Web View current in deployed Blazor Hybrid apps](/aspnet/core/blazor/hybrid/security/security-considerations#keep-the-web-view-current-in-deployed-apps).

## Publish an unsigned app

At this time, publishing is only supported through the .NET command line interface.

To publish your app, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet build` command, providing the following parameters:

<!-- dotnet publish doesn't work at the time of writing -->

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-maccatalyst` or `net7.0-maccatalyst`.                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |
| `/p:CreatePackage`           | An optional parameter that controls whether to create an .app or a .pkg. Use `true` for a .pkg. |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

::: moniker range="=net-maui-6.0"

For example, use the following command to create an *.app*:

```console
dotnet build -f:net6.0-maccatalyst -c:Release
```

Use the following command to create a *.pkg*:

```console
dotnet build -f:net6.0-maccatalyst -c:Release /p:CreatePackage=true
```

Publishing builds the app, and then copies the *.app* or *.pkg* to the *bin/Release/net6.0-maccatalyst/maccatalyst-x64* folder.

::: moniker-end

::: moniker range="=net-maui-7.0"

For example, use the following command to create an *.app*:

```console
dotnet build -f:net7.0-maccatalyst -c:Release
```

Use the following command to create a *.pkg*:

```console
dotnet build -f:net7.0-maccatalyst -c:Release /p:CreatePackage=true
```

Publishing builds the app, and then copies the *.app* or *.pkg* to the *bin/Release/net7.0-maccatalyst/maccatalyst-x64* folder.

::: moniker-end

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

## Run the unsigned app

By default, *.app* and *.pkg* files that are downloaded from the internet can't be run by double-clicking on them. For more information, see [Open a Mac app from an unidentified developer](https://support.apple.com/en-gb/guide/mac-help/mh40616/mac).

To ensure that a *.pkg* installs the app to your *Applications* folder, copy the *.pkg* to outside of your build artifacts folder and delete the *bin* and *obj* folders before double-clicking on the *.pkg*.

## See also

- [GitHub discussion and feedback: .NET MAUI macOS target publishing/archiving](https://github.com/dotnet/maui/issues/5399)
- [Open apps safely on your Mac](https://support.apple.com/en-gb/HT202491)
