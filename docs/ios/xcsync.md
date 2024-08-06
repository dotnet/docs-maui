---
title: Xcode Sync
description: Xcsync is designed to provide .NET developers with improved support for editing Apple specific files
author: haritha-mohan, mcumming
ms.author: harithamohan, mcumming
ms.date: 08/05/2024
monikerRange: ">=net-maui-9.0"
---
# Xcode Sync (Xcsync)

Xcode Sync (Xcsync) is a tool that enables developers to leverage Xcode for managing Apple specific files with .NET projects. The tool generates a temporary Xcode project from a .NET project and synchronizes changes to the Xcode files back to the .NET project.

Supported file types include:

- Asset Catalog
- Plist
- Storyboard
- Xib

The tool has 2 commands: generate and sync. Use generate for tracking your changes from your .NET to Xcode and sync for tracking your changes from Xcode to .NET.

## Synopsis

### xcsync-generate

```dotnetcli
dotnet build /t:xcsync-generate /p:<XCSYNC_PROPERTY>=<Value>

Parameters:
  <XCSYNC_PROPERTY> ::= xcSyncProjectFile | xcSyncXcodeFolder | xcSyncTargetFrameworkMoniker | xcSyncVerbosity

  <xcSyncProjectFile> ::=<PROJECT>
  <xcSyncXcodeFolder> ::=<TARGET_XCODE_DIRECTORY>
  <xcSyncTargetFrameworkMoniker> ::=<FRAMEWORK>
  <xcSyncVerbosity> ::=<LEVEL>
```

### xcsync-sync

```dotnetcli
dotnet build /t:xcsync-sync /p:<XCSYNC_PROPERTY>=<Value>

Parameters:
  <XCSYNC_PROPERTY> ::= xcSyncProjectFile | xcSyncXcodeFolder | xcSyncTargetFrameworkMoniker | xcSyncVerbosity

  <xcSyncProjectFile> ::=<PROJECT>
  <xcSyncXcodeFolder> ::=<TARGET_XCODE_DIRECTORY>
  <xcSyncTargetFrameworkMoniker> ::=<FRAMEWORK>
  <xcSyncVerbosity> ::=<LEVEL>
```

### Arguments

- **`/p:<xcSyncProjectFile>=<PROJECT>`**
  The project file to build. This is **required**. Allowed values are MAUI project files or a project file that targets a supported platform (iOS, tvOS, macOS, MacCatalyst).

### Options

These are optional; if not specified, the default value will be used.

- **`/p:<xcSyncTargetFrameworkMoniker>=<FRAMEWORK>`**
  Invoke the tool for a specific framework. The framework must be defined in the project file. Examples: `net9.0-ios`, `net9.0-maccatalyst`. **Required** if the .NET project supports multiple target frameworks (for example, a standard MAUI project). If a single platform project, the default value will be the single target framework specified in the project file.
- **`/p:<xcSyncXcodeFolder>=<TARGET_XCODE_DIRECTORY>`**
  Directory in which to place the generated Xcode project. Default path is `./obj/xcode`
- **`/p:<xcSyncVerbosity>=<LEVEL>`**
  Sets verbosity level of the command. Allowed values are `Detailed`, `Diagnostic`, `Minimal`, `Normal`, `Quiet`. Default value is `Normal`.

### Examples

- Generate and open a Xcode project for a MAUI project that supports the `net9.0-ios` [TFM](https://learn.microsoft.com/en-us/dotnet/standard/frameworks).

  ```dotnetcli
  dotnet build /t:xcsync-generate --project "path/to/maui.csproj" -tfm "net9.0-ios -o"
  ```

- Sync changes from a generated Xcode project in the default location (./obj/xcode) back to a .NET macOS project

  ```dotnetcli
  dotnet build /t:xcsync-sync --project "path/to/macOS.csproj"
  ```
