---
title: "Use the CLI to publish Unpackaged apps for Windows"
description: "Learn how to package and publish an unpackaged Windows .NET MAUI app with the dotnet publish command."
ms.date: 10/12/2022
---

# Publish an unpackaged .NET MAUI app for Windows with the CLI

> [!div class="op_single_selector"]
>
> - [Publish Packaged / MSIX](publish-cli.md)
> - [Publish Unpackaged / EXE](publish-unpackaged-cli.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for Windows, you can publish the app and its dependencies to a folder for deployment to another system.

<!-- markdownlint-enable MD044 -->

## Configure the project build settings

The project file is a good place to put Windows-specific build settings. You may not want to put some settings into the project file, such as passwords. The settings described in this section can be passed on the command line with the `-p:name=value` format. If the setting is already defined in the project file, a setting passed on the command line overrides the project setting.

Add the following `<PropertyGroup>` node to your project file. This property group is only processed when the target framework is Windows and the configuration is set to `Release`. This config section runs whenever a build or publish in `Release` mode.

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows' and '$(RuntimeIdentifierOverride)' != ''">
    <RuntimeIdentifier>$(RuntimeIdentifierOverride)</RuntimeIdentifier>
</PropertyGroup>
```

The `<PropertyGroup>` in the example is required to work around a bug in the Windows SDK. For more information about the bug, see [WindowsAppSDK Issue #2940](https://github.com/microsoft/WindowsAppSDK/issues/2940).

## Publish

To publish your app, open the **Developer Command Prompt for VS 2022** terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                               |
|------------------------------|-------------------------------------------------------------------------------------|
| `-f` | The target framework, which is `net7.0-windows{version}`. This value is a Windows TFM, such as `net7.0-windows10.0.19041.0`. Ensure that this value is identical to the value in the `<TargetFrameworks>` node in your *.csproj* file.           |
| `-c`                 | The build configuration, which is `Release`.                                   |
| `-p:RuntimeIdentifierOverride=win10-x64`<br>- or -<br>`-p:RuntimeIdentifierOverride=win10-x86` | Avoids the bug detailed in [WindowsAppSDK Issue #2940](https://github.com/microsoft/WindowsAppSDK/issues/2940). Choose the `-x64` or `-x86` version of the parameter based on your target platform.

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

For example:

```console
dotnet publish -f net7.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64
```

Publishing builds and packages the app, copying the signed package to the _bin\\Release\\net7.0-windows10.0.19041.0\\win10-x64\\AppPackages\\\<appname>\\_ folder. \<appname> is a folder named after both your project and version. In this folder, there's an _msix_ file, and that's the app package.

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).
