---
title: "Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app"
description: "Learn how to manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app."
ms.date: 3/02/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net7.0-android", "net7.0-ios" ]
---

# Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app

Upgrading a multi-project Xamarin.Forms app to a multi-project .NET Multi-platform App UI (.NET MAUI) app follows the same steps as a Xamarin.Android and Xamarin.iOS project, with additional steps to take advantage of changes in .NET MAUI.

This article describes how to manually migrate a Xamarin.Forms library project to a .NET MAUI library project. Before you do this, you must update your Xamarin.Forms platform projects to be SDK-style projects. SDK-style projects are the same project format used by all .NET workloads, and compared to many Xamarin projects are much less verbose. For information about updating your app projects, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md), [Xamarin.Android project migration](android-projects.md) and [Xamarin Apple project migration](apple-projects.md).

To migrate a Xamarin.Forms library project to a .NET MAUI library project, you must:

> [!div class="checklist"]
>
> - Update your project file to be SDK-style.
> - Update namespaces.
> - Address any API changes.
> - Configure .NET MAUI.
> - Upgrade or replace incompatible dependencies with .NET 6+ versions.
> - Compile and test your app.

To simplify the upgrade process, you should create a new .NET MAUI library project of the same name as your Xamarin.Forms library project, and then copy in your code. This is the approach outlined below.

## Create a new project

In Visual Studio, create a new .NET MAUI class library project of the same name as your Xamarin.Forms library project. This project will host the code from your Xamarin.Forms library project. Opening the project file will confirm that you have a .NET SDK-style project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net7.0;net7.0-android;net7.0-ios;net7.0-maccatalyst</TargetFrameworks>
        <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net7.0-windows10.0.19041.0</TargetFrameworks>
        <!-- Uncomment to also build the tizen app. You will need to install tizen by following this: https://github.com/Samsung/Tizen.NET -->
        <!-- <TargetFrameworks>$(TargetFrameworks);net7.0-tizen</TargetFrameworks> -->
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

When manually updating a Xamarin.Forms to .NET MAUI you will need to update each platform project's entry point class, and then configure the bootstrapping of the .NET MAUI app.

### Android project configuration

In your .NET MAUI Android project, update the `MainApplication` class to match the code below:

```csharp
using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

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

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

[!INCLUDE [Update app dependencies](includes/update-app-dependencies.md)]

[!INCLUDE [Compile and troubleshoot](includes/compile-troubleshoot.md)]

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
