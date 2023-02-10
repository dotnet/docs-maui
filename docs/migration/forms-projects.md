---
title: "Manually upgrade a Xamarin.Forms app to .NET MAUI"
description: "Learn how to manually upgrade a Xamarin.Forms project to .NET MAUI."
ms.date: 1/31/2023
---

# Manually upgrade a Xamarin.Forms app to .NET MAUI

Updating a Xamarin.Forms app to be a .NET Multi-platform App UI (.NET MAUI) app follows the same steps as a Xamarin.Android and Xamarin.iOS project, with additional steps to take advantage of changes in .NET MAUI.

SDK-style projects are the same project format used by all other .NET workloads, and compared to many Xamarin projects are much less verbose. For information about updating your app projects, see [Update Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md). This article describes how to manually migrate a Xamarin.Forms library project to .NET.

To migrate a Xamarin.Forms library project to .NET, you must:

> [!div class="checklist"]
>
> - Update to SDK-style projects.
> - Update namespaces.
> - Address any API changes.
> - Configure .NET MAUI.
> - Upgrade or replace incompatible dependencies with .NET 6+ versions.
> - Compile and test your app.

To simplify the upgrade, we recommend creating a new .NET project of the same type and name as your Xamarin.Forms project, and then copying in your code. This is the approach outlined below.

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

In your app projects, add a reference to this new library project. Then copy your Xamarin.Forms library project files into the .NET MAUI library project.

## Namespace changes

Namespaces have changed in the move from Xamarin.Forms to .NET Multi-platform App UI (.NET MAUI), and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, do a find and replace for the following namespace changes:

| Xamarin.Forms namespace | .NET MAUI namespace(s) |
| --- | --- |
| `xmlns="http://xamarin.com/schemas/2014/forms"` | `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"` |
| `using Xamarin.Forms` | `using Microsoft.Maui` and `using Microsoft.Maui.Controls` |
| `using Xamarin.Forms.Xaml` | `using Microsoft.Maui.Controls.Xaml` |

The .NET MAUI class library project makes use of implicit `global using` directives. This enables you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to resolve the types from that namespace.

## API changes

TEXT GOES HERE

### Colors

`Color` and `Colors` are separate types in .NET MAUI, and can be found in the `Microsoft.Maui.Graphics` namespace:

- `Color` - the type used to express a color
- `Colors` - holds static references to default colors by name, and extension methods to convert types.
- `Color.Default` doesn't exist. A `Color` defaults to `null`.

Default colors on views are `null`.

For more information about color changes, see [Microsoft.Maui.Graphics.Color vs Xamarin.Forms.Color](https://gist.github.com/hartez/593fc3fb87035a3aedc91657e9c15ab3).

### Other API changes

- `Shape`, and it's derivatives, are in `Microsoft.Maui.Controls.Shapes` namespace.
- `Frame.BorderColor=Accent` doesn't exist. Instead, use an explicit color.
- `ToolbarItem.Icon` doesn't exist. Instead, use `ToolbarItem.IconImageSource`.
- `Button.Image` doesn't exist. Instead, use `Button.ImageSource`.
- `Span.ForegroundColor` doesn't exist. Instead, use `Span.TextColor`.
- `OSTheme` doesn't exist. Instead, use `AppTheme`.

## Bootstrap your migrated app

When manually updating a Xamarin.Forms to .NET Multi-platform App UI (.NET MAUI) you will need to update each platform project's entry point class, and then configure the bootstrapping of the .NET MAUI app.

### Android project configuration

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

### iOS project configuration

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

### MauiProgram configuration

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

### AssemblyInfo changes

Properties that are typically set in an *AssemblyInfo.cs** file are now available in your SDK-style project. We recommend migrating them to your project file in every project, and removing the *AssemblyInfo.cs* file.

Optionally, you may keep the *AssemblyInfo.cs* file and set the `GenerateAssemblyInfo` property in your project file to `false`:

```xml
<PropertyGroup>
  <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
</PropertyGroup>
```

For more information about the `GenerateAssemblyInfo` property, see [GenerateAssemblyInfo](/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo).

## Update app dependencies

NuGet packages and other library dependencies that depend upon target frameworks prior to .NET 6 are generally not compatible with .NET 6+. When migrating your Xamarin projects, you should identify which packages have `net6-` or later compatibility.

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed below are still compatible with .NET 6+.

| Compatible frameworks | Incompatible frameworks |
|:--|:--|
| net6.0-android | monoandroid, monoandroid10.0 |
| net6.0-ios | monotouch, xamarinios, xamarinios10 |
| net6.0-macos | monomac, xamarinmac, xamarinmac20 |
| net6.0-maccatalyst |  |
| net6.0-tvos | xamarintvos |
| | xamarinwatchos |
| net6.0-windows | uap10.0.16299 |

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the net6 or newer frameworks above, regardless of also including incompatible frameworks, then the package is compatible.

You should replace any incompatible packages with compatible alternatives.

When upgrading a Xamarin.Forms apps to .NET Multi-platform App UI (.NET MAUI), you may encounter build or runtime errors that need to be addressed.

## Troubleshooting

<!-- markdownlint-disable MD032 -->
> [!TIP]
> - Delete all *bin* and *obj* folders from all projects before opening and building projects in Visual Studio, particularly when changing .NET versions.
> - Delete the 'ResourceDesigner' generated file from the Android project.
<!-- markdownlint-enable MD032 -->

The following table provides guidance for overcoming common build or runtime errors:

| Issue | Tip |
| ----- | --- |
| Layout is missing padding, margin, or spacing. | Add default values to your project based on the .NET MAUI style resource. For more information, see [Default value changes from Xamarin.Forms](layouts.md#default-layout-value-changes-from-xamarinforms). |
| `Color.Red` and similar can't be found | Named colors are now in `Microsoft.Maui.Graphics.Colors`. |
| `Color` and `Colors` can't be found | `Color` and `Colors` are now in the `Microsoft.Maui.Graphics` namespace. |
| `Color.Default` doesn't exist. | `Color` defaults to `null`. |
| `Frame.BorderColor=Accent` doesn't exist | Use an explicit color. |
| `ToolbarItem.Icon` doesn't exist. | Use `ToolbarItem.IconImageSource`. |
| `Button.Image` doesn't exist. | Use `Button.ImageSource`. |
| `Span.ForegroundColor` doesn't exist. | Use `Span.TextColor`. |
| `OSTheme` doesn't exist. | Use `AppTheme`. |
| `Xamarin.Essentials` namespace doesn't exist. | Remove the namespace and resolve types individually. |
| `Xamarin.Forms` namespace doesn't exist. | Replace with the `Microsoft.Maui` or `Microsoft.Maui.Controls` namespace, depending on the type used. |
| `Xamarin.Forms.Xaml` namespace doesn't. | Replace with the `Microsoft.Maui.Controls.Xaml` namespace. |
| `CollectionView` doesn't scroll. | Check the container layout and the measured size of the `CollectionView`. By default the control will take up as much space as the container allows. A <xref:Microsoft.Maui.Controls.Grid> will constrain children at its own size. However a <xref:Microsoft.Maui.Controls.StackLayout> will enable children to take up space beyond its bounds. |

For more information about color changes, see [Microsoft.Maui.Graphics.Color vs Xamarin.Forms.Color](https://gist.github.com/hartez/593fc3fb87035a3aedc91657e9c15ab3).

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
