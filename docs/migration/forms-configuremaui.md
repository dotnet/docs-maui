---
title: "Upgrading Xamarin.Forms - Configure .NET MAUI"
description: "Updating your application files and creating MauiProgram."
ms.date: 1/31/2023
---

# Configure .NET MAUI

When manually upgrading from Xamarin.Forms to .NET MAUI you will update each platform project's application file, and then configure the bootstrapping of the .NET MAUI applications.

## Android project configuration

In the .NET MAUI Android project, update your `MainApplication.cs` to match the code below. Add a new file if necessary.

```csharp
using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using ArtAuction;

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

Update the `MainActivity.cs` to inherit from `MauiAppCompatActivity`. Refer to the code below.

```csharp
using System;
using Microsoft.Maui;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace ArtAuction.Droid
{
    [Activity(Label = "ArtAuction", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}
```

Update the `AndroidManifest` to `android:targetSdkVersion="31"` which is the minimum Android SDK version required by .NET MAUI.

## iOS project configuration

In the .NET MAUI iOS project, update `AppDelegate.cs` to inherit from `MauiUIApplicationDelegate`. Refer to the code below.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Foundation;
using UIKit;
using ArtAuction;

namespace ArtAuction.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
```

Update `Info.plist` `MinimumOSVersion` to `15.2` which is the minimum version required by .NET MAUI.

## MauiProgram configuration

.NET MAUI uses the host builder pattern to initialize an application in a single place. This pattern replaces `Xamarin.Forms.Init`.

Add a new file called `MauiProgram.cs` and implement `MauiAppBuilder` by referencing the sample code below.

```csharp
namespace ArtAuction;

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

## AssemblyInfo changes

Most properties set in an `AssemblyInfo.cs` file are now available in your SDK Style csproj. We recommend migrating those to your `csproj` in every project, and removing any `AssemblyInfo.cs`.

Optionally, you may keep the `AssemblyInfo.cs` file and disable `GenerateAssemblyInfo` in the csproj file.

```xml
<PropertyGroup>
  <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
</PropertyGroup>
```

> [!div class="nextstepaction"]
> [Upgrade or replace incompatible dependencies with .NET 6+ versions](dependencies.md)

## See also

- [MainProgram and app startup documentation](/dotnet/maui/fundamentals/app-startup)
- [MSBuild and project files - GenerateAssembly Info](/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo)
