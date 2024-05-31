---
title: "Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app"
description: "Learn how to manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app."
ms.date: 05/31/2024
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net8.0-android", "net8.0-ios" ]
---

# Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app

Upgrading a multi-project Xamarin.Forms app to a multi-project .NET Multi-platform App UI (.NET MAUI) app follows the same steps as a Xamarin.Android and Xamarin.iOS project, with additional steps to take advantage of changes in .NET MAUI.

This article describes how to manually migrate a Xamarin.Forms library project to a .NET MAUI library project. Before you do this, you must update your Xamarin.Forms platform projects to be SDK-style projects. SDK-style projects are the same project format used by all .NET workloads, and compared to many Xamarin projects are much less verbose. For information about updating your app projects, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac projects to .NET](native-projects.md), [Xamarin.Android project migration](android-projects.md), [Xamarin Apple project migration](apple-projects.md), and [Xamarin.Forms UWP project migration](uwp-projects.md).

To migrate a Xamarin.Forms library project to a .NET MAUI library project, you must:

> [!div class="checklist"]
>
> - Update your Xamarin.Forms app to use Xamarin.Forms 5.
> - Update the app's dependencies to the latest versions.
> - Ensure the app still works.
> - Update your project file to be SDK-style.
> - Update namespaces.
> - Address any API changes.
> - Configure .NET MAUI.
> - Upgrade or replace incompatible dependencies with .NET 8 versions.
> - Compile and test your app.

To simplify the upgrade process, you should create a new .NET MAUI library project of the same name as your Xamarin.Forms library project, and then copy in your code. This is the approach outlined below.

## Update your Xamarin.Forms app

Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions.

This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.

## Create a new project

In Visual Studio, create a new .NET MAUI class library project of the same name as your Xamarin.Forms library project. This project will host the code from your Xamarin.Forms library project. Opening the project file will confirm that you have a .NET SDK-style project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net8.0;net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
        <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
        <!-- Uncomment to also build the tizen app. You will need to install tizen by following this: https://github.com/Samsung/Tizen.NET -->
        <!-- <TargetFrameworks>$(TargetFrameworks);net8.0-tizen</TargetFrameworks> -->
        <UseMaui>true</UseMaui>
        <SingleProject>true</SingleProject>
        <ImplicitUsings>enable</ImplicitUsings>

        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">11.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">13.1</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</SupportedOSPlatformVersion>
        <TargetPlatformMinVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</TargetPlatformMinVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'tizen'">6.5</SupportedOSPlatformVersion>
    </PropertyGroup>

</Project>
```

In your platform projects, add a reference to this new library project. Then copy your Xamarin.Forms library files into the .NET MAUI library project.

[!INCLUDE [Namespace changes](includes/namespace-changes.md)]

[!INCLUDE [API changes](includes/api-changes.md)]

## Bootstrap your migrated app

When manually updating a Xamarin.Forms to .NET MAUI you will need to enable .NET MAUI support in each platform project, update each platform project's entry point class, and then configure the bootstrapping of your .NET MAUI app.

### Enable .NET MAUI in platform projects

Before you update each platform project's entry point class, you must first enable .NET MAUI support. This can be achieved by setting the `$(UseMaui)` build property to `true` in each platform project:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    ...
    <UseMaui>true</UseMaui>
  </PropertyGroup>
</Project>
```

> [!IMPORTANT]
> You must add `<UseMaui>true</UseMaui>` to your project file to enable .NET MAUI support. In addition, ensure you've added `<EnableDefaultMauiItems>false</EnableDefaultMauiItems>` to your WinUI project file. This will stop you receiving build errors about the `InitializeComponent` method already being defined.

#### Add package references

In .NET 8, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

You should add the following explicit package references to an `<ItemGroup>` in each project file:

```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
```

The `$(MauiVersion)` variable is referenced from the version of .NET MAUI you've installed. You can override this by adding the `$(MauiVersion)` build property to each project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <UseMaui>True</UseMaui>
        <MauiVersion>8.0.3</MauiVersion>
    </PropertyGroup>
</Project>
```

### Android project configuration

In your .NET MAUI Android project, update the `MainApplication` class to match the code below:

```csharp
using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using YOUR_MAUI_CLASS_LIB_HERE;

namespace YOUR_NAMESPACE_HERE.Droid
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
```

Also update the `MainActivity` class to inherit from `MauiAppCompatActivity`:

```csharp
using System;
using Microsoft.Maui;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace YOUR_NAMESPACE_HERE.Droid
{
    [Activity(Label = "MyTitle", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}
```

Then, update your manifest file to specify that the `minSdKVersion` is 21, which is the minimum Android SDK version required by .NET MAUI. This can be achieved by modifying the `<uses-sdk />` node, which is a child of the `<manifest>` node:

```xml
<uses-sdk android:minSdkVersion="21" android:targetSdkVersion="32" />
```

### iOS project configuration

In your .NET MAUI iOS project, update the `AppDelegate` class to inherit from `MauiUIApplicationDelegate`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Foundation;
using UIKit;
using YOUR_MAUI_CLASS_LIB_HERE;

namespace YOUR_NAMESPACE_HERE.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
```

Then, update *Info.plist* so that `MinimumOSVersion` is 11.0, which is the minimum iOS version required by .NET MAUI.

### UWP project configuration

In your .NET MAUI WinUI 3 project, update *App.xaml* to match the code below:

```xaml
<?xml version="1.0" encoding="utf-8"?>
<maui:MauiWinUIApplication
    x:Class="YOUR_NAMESPACE_HERE.WinUI.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:maui="using:Microsoft.Maui"
    xmlns:local="using:YOUR_NAMESPACE_HERE.WinUI">
    <maui:MauiWinUIApplication.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
                <!-- Other merged dictionaries here -->
            </ResourceDictionary.MergedDictionaries>
            <!-- Other app resources here -->
        </ResourceDictionary>
    </maui:MauiWinUIApplication.Resources>
</maui:MauiWinUIApplication>
```

> [!NOTE]
> If your project included resources in your existing *App.xaml* you should migrate them to the new version of the file.

Also, update *App.xaml.cs* to match the code below:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using YOUR_MAUI_CLASS_LIB_HERE;

namespace YOUR_NAMESPACE_HERE.WinUI;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

> [!NOTE]
> If your project included business logic in *App.xaml.cs* you should migrate that logic to the new version of the file.

Then add a *launchSettings.json* file to the *Properties* folder of the project, and add the following JSON to the file:

```json
{
  "profiles": {
    "Windows Machine": {
      "commandName": "MsixPackage",
      "nativeDebugging": true
    }
  }
}
```

### App entry point

.NET MAUI apps have a single cross-platform app entry point. Each platform entry point calls a `CreateMauiApp` method on the static `MauiProgram` class, and returns a <xref:Microsoft.Maui.Hosting.MauiApp>.

Therefore, add a new class named `MauiProgram` that contains the following code:

```csharp
namespace YOUR_NAMESPACE_HERE;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        return builder.Build();
    }
}
```

> [!NOTE]
> For Xamarin.Forms UWP projects, the `App` reference in `builder.UseMauiApp<App>()` can be found in the *MainPage.xaml.cs* file.

If there are platform specific services that need to be migrated to .NET MAUI, use the <xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Type)> method to add a transient service of the specified type to the specified <xref:Microsoft.Extensions.DependencyInjection.IServiceCollection>.

> [!NOTE]
> You can quickly update your `Xamarin.Forms` namespaces to `Microsoft.Maui` by using [Quick actions in Visual Studio](upgrade-assistant.md#quick-actions-in-visual-studio), provided that you have [Upgrade Assistant](upgrade-assistant.md) installed.

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

[!INCLUDE [Update app dependencies](includes/update-app-dependencies.md)]

[!INCLUDE [Compile and troubleshoot](includes/compile-troubleshoot.md)]

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
