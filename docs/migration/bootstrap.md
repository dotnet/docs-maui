---
title: "Bootstrap your migrated app"
description: "Learn how to modify your converted Xamarin.Forms projects to bootstrap the app on .NET MAUI."
ms.date: 1/31/2023
---

# Bootstrap your migrated app

When manually updating a Xamarin.Forms to .NET Multi-platform App UI (.NET MAUI) you will need to update each platform project's entry point class, and then configure the bootstrapping of the .NET MAUI app.

## Android project configuration

In your .NET MAUI Android project, update the `MainApplication` class to match the code below:

```csharp
using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
...

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

Then, update your android manifest file to specify that the `minSdKVersion` is 32, which is the minimum Android SDK version required by .NET MAUI. This can be achieved by modifying `<uses-sdk />` node, which is a child of the `<manifest>` node:

```xml
<uses-sdk android:minSdkVersion="21" android:targetSdkVersion="31" />
```

## iOS project configuration

In your .NET MAUI iOS project, update the `AppDelegate` class to inherit from `MauiUIApplicationDelegate`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Foundation;
using UIKit;
...

namespace YOUR_NAMESPACE_HERE.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
```

Then, update **Info.plist** so that `MinimumOSVersion` is 11.0, which is the minimum version required by .NET MAUI.

## MauiProgram configuration

.NET MAUI apps have a single cross-platform app entry point. Each platform entry point calls a `CreateMauiApp` method on the static `MauiProgram` class, and returns a `MauiApp`.

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

## AssemblyInfo changes

Properties that are typically set in an *AssemblyInfo.cs** file are now available in your SDK-style project. We recommend migrating them to your project file in every project, and removing the *AssemblyInfo.cs* file.

Optionally, you may keep the *AssemblyInfo.cs* file and set the `GenerateAssemblyInfo` property in your project file to `false`:

```xml
<PropertyGroup>
  <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
</PropertyGroup>
```

For more information about the `GenerateAssemblyInfo` property, see [GenerateAssemblyInfo](/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo).

## Next step

> [!div class="nextstepaction"]
> [Upgrade or replace incompatible dependencies with .NET 6+ versions](dependencies.md)
