---
title: "Publish an unsigned .NET MAUI Mac Catalyst app"
description: "Learn how to package and publish an unsigned .NET MAUI Mac Catalyst app."
ms.date: 03/23/2023
---

# Publish an unsigned .NET MAUI Mac Catalyst app

> [!div class="op_single_selector"]
>
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish outside the app store](publish-outside-app-store.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)

To publish an unsigned .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app, open a terminal and navigate to the folder for your app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                               |
|------------------------------|-----------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net8.0-maccatalyst`.                        |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                        |
| `-p:MtouchLink`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                           |
| `-p:CreatePackage`           | An optional parameter that controls whether to create an .app or a .pkg. Use `false` for an *.app*. |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

Additional build parameters can be specified on the command line. The following table lists some of the common parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-p:ApplicationTitle` | The user-visible name for the app. |
| `-p:ApplicationId` | The unique identifier for the app, such as `com.companyname.mymauiapp`. |
| `-p:ApplicationVersion` | The version of the build that identifies an iteration of the app. |
| `-p:ApplicationDisplayVersion` | The version number of the app. |
| `-p:RuntimeIdentifier` | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |

For example, use the following command to create an *.app*:

```dotnetcli
dotnet publish -f net8.0-maccatalyst -c Release -p:CreatePackage=false
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Use the following command to create a *.pkg*:

```dotnetcli
dotnet publish -f net8.0-maccatalyst -c Release
```

Publishing builds the app, and then copies the *.app* to the *bin/Release/net8.0-maccatalyst/* folder or the *.pkg* to the *bin/Release/net8.0-maccatalyst/publish/* folder. If you publish the app using only a single architecture, the *.app* will be published to the *bin/Release/net8.0-maccatalyst/{architecture}/* folder while the *.pkg* will be published to the *bin/Release/net8.0-maccatalyst/{architecture}/publish/* folder.

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

## Run the unsigned app

By default, *.app* and *.pkg* files that are downloaded from the internet can't be run by double-clicking on them. For more information, see [Open a Mac app from an unidentified developer](https://support.apple.com/en-gb/guide/mac-help/mh40616/mac) on support.apple.com.

> [!WARNING]
> Sometimes macOS will update any existing app when installing a *.pkg*, which might not be the app in the */Applications* directory.
> This means that if you try to test a *.pkg* locally, macOS might update the existing app in the project's *bin* directory, instead of
> the app in the */Applications* directory. The solution is to delete the project's *bin* directory before installing any *.pkg* files
> (after copying the *.pkg* elsewhere).

## See also

- [Open apps safely on your Mac](https://support.apple.com/en-gb/HT202491)
