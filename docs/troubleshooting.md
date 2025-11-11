---
title: "Troubleshoot known issues"
description: "Learn about .NET MAUI known issues and troubleshooting you can do to resolve these issues."
ms.date: 01/02/2025
---

# Troubleshooting known issues

This article describes some of the known issues with .NET Multi-platform App UI (.NET MAUI), and how you can solve or work around them. The [.NET MAUI repository](https://github.com/dotnet/maui/wiki/Known-Issues) also details some known issues.

## Can't locate the .NET MAUI workloads

There are two options for installing the .NET MAUI workloads:

1. Visual Studio on Windows can install *.msi* files for each workload pack.
1. `dotnet workload install` commands.

On Windows, if you run a `dotnet workload install` after installing .NET MAUI through the Visual Studio installer, Visual Studio can enter a state where it can't locate the .NET MAUI workloads. You'll receive build errors telling you to install the .NET MAUI workloads, and it's possible to enter a state where the workloads can't be repaired or reinstalled. For more information, see the GitHub issue [dotnet/sdk#22388](https://github.com/dotnet/sdk/issues/22388).

### Windows

The solution to this issue on Windows is to uninstall the .NET MAUI workloads through the CLI, uninstall any .NET SDKs in Control Panel, and uninstall the .NET MAUI workloads in Visual Studio. These uninstalls can be accomplished with the following process:

1. Run `dotnet workload uninstall maui` if you ever used the `dotnet workload install` commands.
1. Uninstall any standalone .NET SDK installers from Control Panel. These installers have names similar to `Microsoft .NET SDK 6.0.300`.
1. In every instance of Visual Studio, uninstall the .NET Multi-platform App UI development, and .NET desktop development Visual Studio workloads.

Then, check if there are additional `.msi` files that need to be uninstalled by running the following command:

```cmd
reg query HKLM\SOFTWARE\Microsoft\Windows\currentversion\uninstall\ -s -f manifest
```

This `reg query` command lists .NET 6+ SDKs that are still installed on your machine, such as:

```
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\currentversion\uninstall\{EEC1BB5F-3391-43C2-810E-42D78ADF3140}
    InstallSource    REG_SZ    C:\ProgramData\Microsoft\VisualStudio\Packages\Microsoft.MacCatalyst.Manifest-6.0.300,version=125.179.40883,chip=x64,productarch=neutral\
    DisplayName    REG_SZ    Microsoft.NET.Sdk.MacCatalyst.Manifest-6.0.300
```

If you receive similar output, you should copy the GUID for each package and uninstall the package with the `msiexec` command:

```cmd
msiexec /x {EEC1BB5F-3391-43C2-810E-42D78ADF3140} /q IGNOREDEPENDENCIES=ALL
```

Then, you should keep executing the `reg query` command until it doesn't return any results. Once there are no more results and all .NET 6+ SDKs are uninstalled, you should also consider deleting the following folders:

- `C:\Program Files\dotnet\sdk-manifests`
- `C:\Program Files\dotnet\metadata`
- `C:\Program Files\dotnet\packs`
- `C:\Program Files\dotnet\library-packs`
- `C:\Program Files\dotnet\template-packs`
- `C:\Program Files\dotnet\sdk\6.*` or `C:\Program Files\dotnet\sdk\7.*`
- `C:\Program Files\dotnet\host\fxr\6.*` or `C:\Program Files\dotnet\host\fxr\7.*`

After following this process, you should be able to reinstall .NET MAUI either through Visual Studio, or by installing your chosen .NET SDK version and running the `dotnet workload install maui` command.

<!-- Leaving here in case this situation occurs with .NET 8
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
-->

## Platform version isn't present

Visual Studio may not be resolving the required workloads if you try to compile a project and receive an error similar to the following text:

> Platform version is not present for one or more target frameworks, even though they have specified a platform: net8.0-android, net8.0-ios, net8.0-maccatalyst

This problem typically results from having an x86 and x64 SDK installed, and the x86 version is being used. Visual Studio and .NET MAUI require the x64 .NET SDK. If your operating system has a system-wide `PATH` variable that is resolving the x86 SDK first, you need to fix that by either removing the x86 .NET SDK from the `PATH` variable, or promoting the x64 .NET SDK so that it resolves first. For more information on troubleshooting x86 vs x64 SDK resolution, see [Install .NET on Windows - Troubleshooting](/dotnet/core/install/windows#it-was-not-possible-to-find-any-installed-net-core-sdks).

## Type or namespace 'Default' doesn't exist

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

## Xcode isn't currently installed or couldn't be found

After you install the Xcode command line tools using `xcode-select --install`, Visual Studio Code might show a "Xcode is not currently installed or could not be found" message when you attempt to build .NET MAUI apps that target iOS or Mac Catalyst. In this scenario, check that you also have Xcode installed from the App Store. Then, launch Xcode and go to **Xcode > Preferences > Locations > Command Line Tools** and check if the drop-down is empty. If it's empty, select the drop-down, and then select the location of the Xcode command line tools. Then close Xcode and restart Visual Studio Code.

## Couldn't find a valid Xcode app bundle

If you receive the error "Could not find a valid Xcode app bundle at '/Library/Developer/CommandLineTools'" when you attempt to build .NET MAUI apps that target iOS or Mac Catalyst, try the solution described in [Xcode isn't currently installed or couldn't be found](#xcode-isnt-currently-installed-or-couldnt-be-found). If you're still unable to access the **Xcode > Preferences > Locations > Command Line Tools** drop-down, run the following command:

```zsh
sudo xcode-select --reset
```

## Xcode version can't be located

In some scenarios, building a .NET MAUI app on iOS or Mac Catalyst may attempt to use a version of Xcode that's no longer installed on your machine. When this occurs you'll receive an error message similar to:

```
xcodebuild: error: SDK "/Applications/Xcode_14.1.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk" cannot be located.
xcrun: error: sh -c '/Applications/Xcode_14.1.app/Contents/Developer/usr/bin/xcodebuild -sdk /Applications/Xcode_14.1.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk -find dsymutil 2> /dev/null' failed with exit code 16384: (null) (errno=Invalid argument)
xcrun: error: unable to find utility "dsymutil", not a developer tool or in PATH
```

When building an app, .NET for iOS and .NET for Mac Catalyst use the following process to determine which version of Xcode to use:

1. If the `MD_APPLE_SDK_ROOT` environment variable is set, use its value. _This environment variable will be deprecated in a future release; do not use it._
1. If the *~/Library/Preferences/Xamarin/Settings.plist* or *~/Library/Preferences/maui/Settings.plist* file exists, use the value defined inside it. _These files will be deprecated in a future release; do not use them._
1. Use the value of `xcode-select -p`.
1. Use `/Applications/Xcode.app`.

The recommended approach to specify the location of Xcode on yoru machine is to:

1. Delete *~/Library/Preferences/Xamarin/Settings.plist* or *~/Library/Preferences/maui/Settings.plist* files (if they exist).
2. Either use `xcode-select --switch ...` to select the system's version of Xcode, or set the `DEVELOPER_DIR` environment variable to the path of Xcode. For more information, see [Build with a specific version of Xcode](~/ios/cli.md).

## Diagnose issues in Blazor Hybrid apps

<xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> has built-in logging that can help you diagnose problems in your Blazor Hybrid app. There are two steps to enable this logging:

1. Enable <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> and related components to log diagnostic information.
1. Configure a logger to write the log output to where you can view it.

For more information, see [Diagnosing issues in Blazor Hybrid apps](~/user-interface/controls/blazorwebview.md#diagnosing-issues).

## Disable image packaging

For troubleshooting purposes, image resource packaging can be disabled by setting the `$(EnableMauiImageProcessing)` build property to `false` in the first `<PropertyGroup>` node in your project file:

```xml
<EnableMauiImageProcessing>false</EnableMauiImageProcessing>
```

## Disable splash screen packaging

For troubleshooting purposes, splash screen resource generation can be disabled by setting the `$(EnableSplashScreenProcessing)` build property to `false` in the first `<PropertyGroup>` node in your project file:

```xml
<EnableSplashScreenProcessing>false</EnableSplashScreenProcessing>
```

## Disable font packaging

For troubleshooting purposes, font resource packaging can be disabled by setting the `$(EnableMauiFontProcessing)` build property to `false` in the first `<PropertyGroup>` node in your project file:

```xml
<EnableMauiFontProcessing>false</EnableMauiFontProcessing>
```

## Disable asset file packaging

For troubleshooting purposes, asset file resource packaging can be disabled by setting the `$(EnableMauiAssetProcessing)` build property to `false` in the first `<PropertyGroup>` node in your project file:

```xml
<EnableMauiAssetProcessing>false</EnableMauiAssetProcessing>
```

## Generate a blank splash screen

For troubleshooting purposes, a blank splash screen can be generated if you don't have a `<MauiSplashScreen>` item and you don't have a custom splash screen. This can be achieved by setting the `$(EnableBlankMauiSplashScreen)` build property to `true` in the first `<PropertyGroup>` node in your project file:

```xml
<EnableBlankMauiSplashScreen>true</EnableBlankMauiSplashScreen>
```

Generating a blank splash screen will override any custom splash screen and will cause app store rejection. However, it can be a useful approach in testing to ensure that your app UI is correct.

## Duplicate image filename errors

You may encounter build errors about duplicate image filenames:

> One or more duplicate file names were detected. All image output filenames must be unique.

This occurs for `MauiIcon` and `MauiImage` items because from .NET 8, .NET MAUI checks to ensure that there are no duplicate image resource filenames.

The error will occur when you have identical filenames in multiple folders, and in certain circumstances when you have identical filenames with different extensions in different folders. For example, the build error will occur for a PNG file at *Resources/Images/PNG/dotnet_bot.png* and an SVG file at *Resources/Images/SVG/dotnet_bot.svg* because SVG files are converted to PNG files at build time.

The error will also occur if you use the `Include` attribute on a `MauiImage` item to include all images in a folder, and then also include a specific image file:

```xml
<MauiImage Include="Resources\Images\*" />
<MauiImage Include="Resources\Images\dotnet_bot.svg" BaseSize="168,208" />
```

If you receive this build error it can be fixed by ensuring that your project file doesn't include duplicate images. To do this, change any `MauiIcon` or `MauiImage` that references a specific file to use the `Update` attribute instead of the `Include` attribute:

```xml
<MauiImage Include="Resources\Images\*" />
<MauiImage Update="Resources\Images\dotnet_bot.svg" BaseSize="168,208" />
```

For information about MSBuild item element attributes, see [Item element (MSBuild): Attributes and elements](/visualstudio/msbuild/item-element-msbuild#attributes-and-elements).
