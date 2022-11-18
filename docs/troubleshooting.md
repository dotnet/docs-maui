---
title: "Troubleshoot known issues"
description: "Describes the known issues and troubleshooting you can do to resolve these issues for a .NET Multi-platform App UI (.NET MAUI) app."
ms.date: 11/18/2022
---

# Troubleshooting known issues

This article describes some of the known issues with .NET MAUI, and how you can solve or work around them. The [.NET MAUI code repository](https://github.com/dotnet/maui/wiki/Known-Issues) also details some known issues.

## Templates are missing

If you've installed Visual Studio 2022 and the **.NET Multi-platform App UI development** workload, but the .NET MAUI templates are missing, you most likely have a conflict with a .NET 7 preview version. To see if .NET 7 is being resolved as your current version of .NET, perform the following steps:

01. Open a terminal.
01. Run the `dotnet --version` command.

    If the result starts with `7.0` and includes the value `-preview`, you're running in the context of a .NET 7 preview release. Use one of the fixes below.

    If the result starts with `7.0` and includes the value `-rc`, you're running in the context of a .NET 7 release candidate, which fully supports .NET MAUI, and includes .NET MAUI templates. You may just need to install the .NET MAUI workload. Review the [Installation instructions](get-started/installation.md) and ensure that the **.NET Multi-platform App UI development** workload is enabled.

> [!TIP]
> You can see all versions of .NET installed with the `dotnet --info` command.

### Fix: Uninstall or upgrade .NET 7 preview

You may have a preview release of .NET 7 on your computer, which doesn't contain any .NET MAUI templates. However, the latest .NET 7 release candidate does contain .NET MAUI templates. You have two options:

- Remove .NET 7. Once .NET 7 is officially released, install it.

  \- or -

- Remove .NET 7 and install the latest .NET 7 release candidate.

> [!IMPORTANT]
> The latest Visual Studio 2022 17.4 previews include the .NET 7 release candidate and fully support .NET MAUI.

### Fix: Use a global.json config file

Use a `global.json` config file in the folder where you'll create the project. This config file can force the context of .NET 6, which contains .NET MAUI templates:

01. Open a terminal and navigate to a folder where you want to create a project.
01. Run the following command: `dotnet new globaljson --sdk-version 6.0.0 --roll-forward major`
01. Run `dotnet new maui --list` to show a list of projects you can create. For example, you may see the following output:

    ```dotnetcli
    dotnet new maui --list

    These templates matched your input: 'maui'

    Template Name                        Short Name        Language  Tags
    -----------------------------------  ----------------  --------  ---------------------------------------------------------
    .NET MAUI App                        maui              [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/Windows/Tizen
    .NET MAUI Blazor App                 maui-blazor       [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/Windows/Tizen/Blazor
    .NET MAUI Class Library              mauilib           [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/Windows/Tizen
    .NET MAUI ContentPage (C#)           maui-page-csharp  [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/WinUI/Tizen/Xaml/Code
    .NET MAUI ContentPage (XAML)         maui-page-xaml    [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/WinUI/Tizen/Xaml/Code
    .NET MAUI ContentView (C#)           maui-view-csharp  [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/WinUI/Tizen/Xaml/Code
    .NET MAUI ContentView (XAML)         maui-view-xaml    [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/WinUI/Tizen/Xaml/Code
    .NET MAUI ResourceDictionary (XAML)  maui-dict-xaml    [C#]      MAUI/Android/iOS/macOS/Mac Catalyst/WinUI/Xaml/Code
    ```

01. Next, create a new .NET MAUI project with the `dotnet new` command, using either `maui` or `maui-blazor` as the project type:

    ```dotnetcli
    dotnet new maui
    ```

01. Open the project in Visual Studio.

## ERROR Platform version is not present.

Visual Studio may not be resolving the required workloads if you try to compile a project and receive an error similar to the following text:

::: moniker range="=net-maui-6.0"

> Platform version is not present for one or more target frameworks, even though they have specified a platform: net6.0-android, net6.0-ios, net6.0-maccatalyst

::: moniker-end

::: moniker range="=net-maui-7.0"

> Platform version is not present for one or more target frameworks, even though they have specified a platform: net7.0-android, net7.0-ios, net7.0-maccatalyst

::: moniker-end

This problem is generally caused by one of the following scenarios:

- You have a .NET 7 preview installed, which doesn't support .NET MAUI.

  There are a few options to fix this issue, you can either upgrade to a .NET 7 release candidate or remove the .NET 7 preview. Review the steps in the [Templates are missing](#templates-are-missing) section.

- You have both an x86 and x64 SDK installed, and the x86 version is being used.

  Visual Studio and .NET MAUI require the x64 .NET SDK. If your operating system has a system-wide `PATH` variable that is resolving the x86 SDK first, you'll need to fix that by either removing the x86 .NET SDK from the `PATH` variable, or promoting the x64 .NET SDK so that it resolves first. For more information on troubleshooting x86 vs x64 SDK resolution, see [Install .NET on Windows - Troubleshooting](/dotnet/core/install/windows#it-was-not-possible-to-find-any-installed-net-core-sdks).

## The WINDOWS `#if` directive is broken

The `WINDOWS` definition doesn't resolve correctly in the latest release of .NET MAUI. To work around this issue, add the following entry to the `<PropertyGroup>` element of your project file.

```xml
<DefineConstants Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">$(DefineConstants);WINDOWS</DefineConstants>
```

The definitions that identify a specific version of Windows will still be missing.

## ERROR Type or namespace 'Default' does not exist

When using the [`Contacts` API](platform-integration/communication/contacts.md), you may see the following error related to iOS and macOS:

```
The type or namespace name 'Default' does not exist in the namespace 'Contacts' (are you missing an assembly reference?)
```

The iOS and macOS platforms contain a root namespace named `Contacts`. This conflict causes a conflict for those platforms with the `Microsoft.Maui.ApplicationModel.Communication` namespace, which contains a `Contacts` type. The `Microsoft.Maui.ApplicationModel.Communication` namespace is automatically imported by the `<ImplicitUsings>` setting in the project file.

To write code that also compiles for iOS and macOS, fully qualify the `Contacts` type. Alternatively, provide a `using` directive at the top of the code file to map the `Communication` namespace:

```csharp
using Communication = Microsoft.Maui.ApplicationModel.Communication;

// Code that uses the namespace:
var contact = await Communication.Contacts.Default.PickContactAsync();
```

## Xcode is not currently installed or could not be found

After installing the Xcode command line tools, using `xcode-select --install`, Visual Studio for Mac might show a "Xcode is not currently installed or could not be found" message when attempting to build .NET MAUI apps that target iOS or Mac Catalyst. In this scenario, check that you also have Xcode installed from the App Store. Then, launch Xcode and go to **Xcode > Preferences > Locations > Command Line Tools** and check if the drop-down is empty. If it is empty, select the drop-down and then select the location of the Xcode command line tools. Then close Xcode and restart Visual Studio for Mac.

## Could not find a valid Xcode app bundle

If you receive the error "Could not find a valid Xcode app bundle at '/Library/Developer/CommandLineTools'", when attempting to build .NET MAUI apps that target iOS or Mac Catalyst, you should try the solution described in [Xcode is not currently installed or could not be found](#xcode-is-not-currently-installed-or-could-not-be-found). If you're still unable to access the **Xcode > Preferences > Locations > Command Line Tools** drop-down, run the following command:

```zsh
sudo xcode-select --reset
```
