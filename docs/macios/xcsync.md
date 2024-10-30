---
title: xcsync
description: Learn how to use xcsync, to provide .NET developers with improved support for editing Apple specific files.
author: haritha-mohan
ms.author: harithamohan
ms.date: 10/30/2024
monikerRange: ">=net-maui-9.0"
no-loc: [ "xcsync" ]
---
# xcsync

## Name

`xcsync`

## Description

xcsync is a global .NET tool that enables developers to leverage Xcode for managing Apple specific files with .NET projects. The tool generates a temporary Xcode project from a .NET project and synchronizes changes to the Xcode files back to the .NET project.

Supported file types include:

- Asset catalog
- Plist
- Storyboard
- Xib

## Subcommands

### generate

#### Synopsis

```dotnetcli
xcsync generate [options]
xcsync generate -h|--help
```

#### Description

Use `generate` to create an Xcode project from a .NET project

#### Options

- **`-p|--project <PROJECT>`**

    The project file to build. Supported project types are .NET MAUI projects or any .NET project that targets a supported platform (iOS, tvOS, macOS, MacCatalyst).
- **`-tfm|--target-framework-moniker <FRAMEWORK>`**

    Invoke the tool for a specific framework. The framework must be defined in the project file. Examples: `net9.0-ios`, `net9.0-maccatalyst`. **Required** if the .NET project supports multiple target frameworks (for example, a standard MAUI project). If a single platform project, the default value will be the single target framework specified in the project file.
- **`-t|--target <TARGET_XCODE_DIRECTORY>`**

    The directory in which to place the generated Xcode project. Default value is `./obj/xcode`.
- **`-f|--force`**

    Forces the overwrite of an existing Xcode project. Default value is `False`.
- **`-o|--open`**

    Opens the generated project in Xcode. Default value is `False`.
- **`-v|--verbosity <LEVEL>`**

    Sets the verbosity level of the command. Allowed values are `Detailed`, `Diagnostic`, `Minimal`, `Normal`, `Quiet`. Default value is `Normal`.
- **`-h|--help`**
  
    Shows help and usage information

### sync

#### Description

Use `sync` to bring changes in the Xcode project back to the .NET project.

#### Synopsis

```dotnetcli
xcsync sync [options]
xcsync sync -h|--help
```

#### Options

- **`-p|--project <PROJECT>`**

    The project file to build. Supported project types are .NET MAUI projects or any .NET project that targets a supported platform (iOS, tvOS, macOS, MacCatalyst).
- **`-tfm|--target-framework-moniker <FRAMEWORK>`**

    Invoke the tool for a specific framework. The framework must be defined in the project file. Examples: `net9.0-ios`, `net9.0-maccatalyst`. **Required** if the .NET project supports multiple target frameworks (for example, a standard MAUI project). If a single platform project, the default value will be the single target framework specified in the project file.
- **`-t|--target <TARGET_XCODE_DIRECTORY>`**

    Path to the Xcode project to sync from. Default value is `./obj/xcode`.
- **`-v|--verbosity <LEVEL>`**

    Sets the verbosity level of the command. Allowed values are `Detailed`, `Diagnostic`, `Minimal`, `Normal`, `Quiet`. Default value is `Normal`.
- **`-h|--help`**
  
    Shows help and usage information

## Examples

- Generate and open an Xcode project for a .NET MAUI project that uses the project file in the current directory, which supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    xcsync generate -tfm net9.0-ios
    ```

- Generate and open an Xcode project for a .NET MAUI project that supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    xcsync generate -p path/to/maui.csproj -tfm net9.0-ios
    ```

- Sync changes from a generated Xcode project in the default location (*./obj/Xcode*) back to a .NET MAUI project that supports the `net9.0-ios` [TFM](/dotnet/standard/frameworks):

    ```dotnetcli
    xcsync sync -p path/to/maui.csproj -tfm net9.0-ios
    ```
