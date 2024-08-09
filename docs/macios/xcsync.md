---
title: Xcode sync
description: Learn how to use Xcsync, to provide .NET developers with improved support for editing Apple specific files.
author: haritha-mohan
ms.author: harithamohan
ms.date: 08/05/2024
monikerRange: ">=net-maui-9.0"
no-loc: [ "xcsync" ]
---
# Xcode sync (xcsync)

Xcode sync (xcsync) is a tool that enables developers to leverage Xcode for managing Apple specific files with .NET projects. The tool generates a temporary Xcode project from a .NET project and synchronizes changes to the Xcode files back to the .NET project.

Supported file types include:

- Asset catalog
- Plist
- Storyboard
- Xib

The tool has two commands: `generate` and `sync`. Use `generate` to create an Xcode project from a .NET project and `sync` to bring changes in the Xcode project back to the .NET project.

## Synopsis

The following examples show the syntax for the `generate` and `sync` commands.

### xcsync-generate

```dotnetcli
dotnet build /t:xcsync-generate
    /p:xcSyncProjectFile=<PROJECT>
    /p:xcSyncXcodeFolder=<TARGET_XCODE_DIRECTORY>
    /p:xcSyncTargetFrameworkMoniker=<FRAMEWORK>
    /p:xcSyncVerbosity=<LEVEL>
```

### xcsync-sync

```dotnetcli
dotnet build /t:xcsync-sync
    /p:xcSyncProjectFile=<PROJECT>
    /p:xcSyncXcodeFolder=<TARGET_XCODE_DIRECTORY>
    /p:xcSyncTargetFrameworkMoniker=<FRAMEWORK>
    /p:xcSyncVerbosity=<LEVEL>
```

### Arguments

- **`/p:xcSyncProjectFile=<PROJECT>`**

    The project file to build. Supported project types are .NET MAUI projects or any .NET project that targets a supported platform (iOS, tvOS, macOS, MacCatalyst).

### Options

The following options allow you to change the default behavior of the tool:

- **`/p:xcSyncTargetFrameworkMoniker=<FRAMEWORK>`**

    Invoke the tool for a specific framework. The framework must be defined in the project file. Examples: `net9.0-ios`, `net9.0-maccatalyst`. **Required** if the .NET project supports multiple target frameworks (for example, a standard MAUI project). If a single platform project, the default value will be the single target framework specified in the project file.

- **`/p:xcSyncXcodeFolder=<TARGET_XCODE_DIRECTORY>`**

    The directory in which to place the generated Xcode project. The default path is *./obj/xcode*.

- **`/p:xcSyncVerbosity=<LEVEL>`**

    Sets the verbosity level of the command. Allowed values are `Detailed`, `Diagnostic`, `Minimal`, `Normal`, `Quiet`. The default value is `Normal`.

### Examples

- Generate and open an Xcode project for a .NET MAUI project that uses the project file in the current directory, which supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    dotnet build /t:xcsync-generate /p:xcSyncTargetFrameworkMoniker=net9.0-ios
    ```

- Generate and open an Xcode project for a .NET MAUI project that supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    dotnet build /t:xcsync-generate /p:xcSyncProjectFile=path/to/maui.csproj /p:xcSyncTargetFrameworkMoniker=net9.0-ios
    ```

- Sync changes from a generated Xcode project in the default location (*./obj/Xcode*) back to a .NET MAUI project that supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    dotnet build /t:xcsync-sync /p:xcSyncProjectFile=path/to/maui.csproj /p:xcSyncTargetFrameworkMoniker=net9.0-ios
    ```
