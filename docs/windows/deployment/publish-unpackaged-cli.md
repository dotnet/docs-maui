---
title: "Use the CLI to publish unpackaged apps for Windows"
description: "Learn how to package and publish an unpackaged Windows .NET MAUI app with the dotnet publish command."
ms.date: 11/08/2023
---

# Publish an unpackaged .NET MAUI app for Windows with the CLI

> [!div class="op_single_selector"]
>
> - [Publish a packaged app using the command line](publish-cli.md)
> - [Publish a packaged app using Visual Studio](publish-visual-studio-folder.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for Windows, you can publish the app and its dependencies to a folder for deployment to another system.

## Configure the project build settings

Add the following `<PropertyGroup>` node to your project file. This property group is only processed when the target framework is Windows and the configuration is set to `Release`. This config section runs whenever a build or publish in `Release` mode.

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows' and '$(RuntimeIdentifierOverride)' != ''">
    <RuntimeIdentifier>$(RuntimeIdentifierOverride)</RuntimeIdentifier>
</PropertyGroup>
```

The `<PropertyGroup>` in the example is required to work around a bug in the Windows App SDK. For more information about the bug, see [WindowsAppSDK Issue #3337](https://github.com/microsoft/WindowsAppSDK/issues/3337).

## Publish

To publish your app, open the **Developer Command Prompt for VS 2022** terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                               |
|------------------------------|-------------------------------------------------------------------------------------|
| `-f` | The target framework, which is `net8.0-windows{version}`. This value is a Windows TFM, such as `net8.0-windows10.0.19041.0`. Ensure that this value is identical to the value in the `<TargetFrameworks>` node in your *.csproj* file.           |
| `-c`                 | The build configuration, which is `Release`.                                   |
| `-p:RuntimeIdentifierOverride=win10-x64`<br>- or -<br>`-p:RuntimeIdentifierOverride=win10-x86` | Avoids the bug detailed in [WindowsAppSDK Issue #3337](https://github.com/microsoft/WindowsAppSDK/issues/3337). Choose the `-x64` or `-x86` version of the parameter based on your target platform. |
| `-p:WindowsPackageType` | The package type, which is `None` for unpackaged apps. |
| `-p:WindowsAppSDKSelfContained` | The deployment mode for your app, which can be framework-dependent or self-contained. This value should be `true` for self-contained apps. For more information about framework-dependent apps and self-contained apps, see [Windows App SDK deployment overview](/windows/apps/package-and-deploy/deploy-overview). |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

For example:

```console
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Publishing builds the app, copying the executable to the _bin\\Release\\net8.0-windows10.0.19041.0\\win10-x64\\publish_ folder. In this folder, there's an _exe_ file, and that's the built app. This app can be launched or the entire folder can be copied to another machine and launched there.

An important distinction from a packaged app is that this won't include the .NET runtime in the folder. This means that the app will require the .NET runtime to first be installed on the machines that will eventually run the app. To ensure the app also contains all the runtime components, the `-p:WindowsAppSDKSelfContained` argument can be provided when publishing. For example:

```console
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true
```

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).
