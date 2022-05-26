---
title: "Troubleshoot known issues"
description: "Describes the known issues and troubleshooting you can do to resolve these issues for a .NET Multi-platform App UI (.NET MAUI) app."
ms.date: 05/25/2022
---

# Troubleshooting known issues guide

This article describes some of the known issues with .NET MAUI, and how you can solve or work around them. The [.NET MAUI code repository](https://github.com/dotnet/maui/wiki/Known-Issues) also details some known issues.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Templates are missing

If you've installed Visual Studio 2022 and the **.NET Multi-platform App UI development** workload, but the .NET MAUI templates are missing, you most likely have a conflict with a .NET 7 preview version. To see if .NET 7 is being resolved as your current version of .NET, perform the following steps:

01. Open a terminal.
01. Run the `dotnet --version` command.

    If the result starts with `7.0`, you're running in the context of .NET 7. Use one of the fixes below.

> [!TIP]
> You can see all versions of .NET installed with the `dotnet --info` command.

### Fix (1)

Uninstall the .NET 7 preview.

### Fix (2)

Use a `global.json` config file in the folder where you'll create the project. This config file can force the context of .NET 6, allowing you to create a new project:

01. Open a terminal and navigate to a folder where you want to create a project.
01. Run the following command: `dotnet new globaljson --sdk-version 6.0.300`
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

Visual Studio may not be resolving the required workloads if you try to compile a project and receive an error similar to the following:

```
Platform version is not present for one or more target frameworks, even though they have specified a platform: net6.0-android, net6.0-ios, net6.0-maccatalyst
```

This may be caused by having the .NET 7 preview installed. To see if .NET 7 is being resolved as your current version of .NET, perform the following steps:

01. Open a terminal.
01. Run the `dotnet --version` command.

    If the result shows a `7.0.XXX` value, you're running in the context of .NET 7. Use one of the fixes below.

### Fix (1)

Uninstall the .NET 7 preview.

### Fix (2)

Use a `global.json` config file in the same folder as your current project. This config file can force the context of .NET 6, allowing you to keep a .NET 7 preview version installed:

01. Close Visual Studio.
01. Open a terminal and navigate to the folder where your project is located.
01. Run the following command: `dotnet new globaljson --sdk-version 6.0.300`
01. Reopen the project in Visual Studio.

## The WINDOWS `#if` directive is broken

The `WINDOWS` definition doesn't resolve correctly in the latest release of .NET MAUI. To work around this issue, add the following entry to the `<PropertyGroup>` element of your project file.

```
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
