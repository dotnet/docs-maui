---
title: "Publish a .NET MAUI app for macOS"
description: "Learn how to package and publish a macOS .NET MAUI app."
ms.date: 04/25/2022
---

# Publish a .NET MAUI app for macOS

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for macOS](overview.md)
> - [Publish for Windows](../../windows/deployment/overview.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for macOS, you generate an *.app* or a *.pkg* file. An *.app* file is a self-contained app that can be run without installation, whereas a *.pkg* is an app packaged in an installer.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Publish an unsigned app

At this time, publishing is only supported through the .NET command line interface.

To publish your app, open a terminal and navigate to the project's folder. Run the `dotnet build` command, providing the following parameters:

<!-- dotnet publish doesn't work at the time of writing -->

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-maccatalyst`.                                            |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |
| `/p:CreatePackage`           | An optional parameter that controls whether to create an .app or a .pkg. Use `true` for a .pkg. |

For example, use the following command to create an *.app*:

```console
dotnet build -f:net6.0-maccatalyst -c:Release
```

Use the following command to create a *.pkg*:

```console
dotnet build -f:net6.0-maccatalyst -c:Release /p:CreatePackage=true
```

Publishing builds the app, and then copies the *.app* or *.pkg* to the *bin/Release/net6.0-maccatalyst/maccatalyst-x64* folder.

## Run the unsigned app

By default, *.app* and *.pkg* files that are downloaded from the internet can't be run by double-clicking on them. For more information, see [Open a Mac app from an unidentified developer](https://support.apple.com/en-gb/guide/mac-help/mh40616/mac).

To ensure that a *.pkg* installs the app to your *Applications* folder, copy the *.pkg* to outside of your build artifacts folder and delete the *bin* and *obj* folders before double-clicking on the *.pkg*.

## See also

- [GitHub discussion and feedback: .NET MAUI macOS target publishing/archiving](https://github.com/dotnet/maui/issues/5399)
- [Open apps safely on your Mac](https://support.apple.com/en-gb/HT202491)
