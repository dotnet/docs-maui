---
title: "Manually upgrade a Xamarin.Forms app to .NET MAUI"
description: "Learn how to manually upgrade a Xamarin.Forms project to .NET MAUI."
ms.date: 1/31/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net7.0-android", "net7.0-ios" ]
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

Namespaces have changed in the move from Xamarin.Forms to .NET MAUI, and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, perform a find and replace for the following namespaces:

| Xamarin.Forms namespace | .NET MAUI namespace(s) |
| --- | --- |
| <xref:Xamarin.Forms> | <xref:Microsoft.Maui> and <xref:Microsoft.Maui.Controls> |
| <xref:Xamarin.Forms.DualScreen> | <xref:Microsoft.Maui.Controls.Foldable> |
| <xref:Xamarin.Forms.Maps> | <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Maps> |
| <xref:Xamarin.Forms.PlatformConfiguration> | <xref:Microsoft.Maui.Controls.PlatformConfiguration> |
| <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific> |
| <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat> |
| <xref:Xamarin.Forms.PlatformConfiguration.GTKSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific> |
| <xref:Xamarin.Forms.PlatformConfiguration.TizenSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific> |
| <xref:Xamarin.Forms.PlatformConfiguration.WindowsSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific> |
| <xref:Xamarin.Forms.PlatformConfiguration.iOSSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific> |
| <xref:Xamarin.Forms.PlatformConfiguration.macOSSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.macOSSpecific> |
| <xref:Xamarin.Forms.Shapes> | <xref:Microsoft.Maui.Controls.Shapes> |
| <xref:Xamarin.Forms.StyleSheets> | <xref:Microsoft.Maui.Controls.StyleSheets> |
| <xref:Xamarin.Forms.Xaml> | <xref:Microsoft.Maui.Controls.Xaml> |

.NET MAUI projects make use of implicit `global using` directives. This enables you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to replace them with the equivalent .NET MAUI namespaces.

In addition, the default XAML namespace has changed from `http://xamarin.com/schemas/2014/forms` in Xamarin.Forms to `http://schemas.microsoft.com/dotnet/2021/maui` in .NET MAUI. Therefore, you should replace all occurrences of `xmlns="http://xamarin.com/schemas/2014/forms"` with `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"`.

## API changes

TEXT GOES HERE

### Colors

In Xamarin.Forms, the <xref:Xamarin.Forms.Color?displayProperty=fullName> struct lets you construct `Color` objects using `double` values, and provides 240 named colors, such as <xref:Xamarin.Forms.Color.AliceBlue?displayProperty=fullName>. In .NET MAUI, this functionality has been separated into the <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, and the <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class.

The <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, in the `Microsoft.Maui.Graphics` namespace lets you construct `Color` objects using `float` values, `byte` values, and `int` values. The <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class, which is also in the `Microsoft.Maui.Graphics` namespace, provides 148 named colors.

| Xamarin.Forms API | .NET MAUI API | Comment |
| ----------------- | ------------- | ------- |
| <xref:Xamarin.Forms.Color.R?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.Red?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Color.G?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.Green?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Color.B?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.Blue?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Color.A?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.Alpha?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Color.Hue?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.GetHue%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| <xref:Xamarin.Forms.Color.Saturation?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.GetSaturation%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| <xref:Xamarin.Forms.Color.Luminosity?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.GetLuminosity%2A?displayProperty=fullName> | Xamarin.Forms property replaced with a method in .NET MAUI. |
| <xref:Xamarin.Forms.Color.Default?displayProperty=fullName> | | No .NET MAUI equivalent. Instead, <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> objects default to `null`. |
| <xref:Xamarin.Forms.Color.Accent?displayProperty=fullName> |  | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Color.FromHex%2A?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.FromArgb%2A?displayProperty=fullName> | <xref:Microsoft.Maui.Graphics.Color.FromHex%2A?displayProperty=fullName> is obsolete and will be removed in a future release. |

In addition, all of the numeric values in <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> are `float`, rather than `double` as used in <xref:Xamarin.Forms.Color?displayProperty=fullName>.

> [!NOTE]
> A <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> doesn't have an implicit conversion to <xref:System.Drawing.Color?displayProperty=fullName>, unlike in Xamarin.Forms.

### Device

Xamarin.Forms has a <xref:Xamarin.Forms.Device?displayProperty=fullName> class that helps you customize layout and functionality on a per-platform basis. The equivalent class in .NET MAUI, <xref:Microsoft.Maui.Controls.Device?displayProperty=fullName>, is deprecated. Instead, it's functionality has been replaced by multiple types.

| Xamarin.Forms API | .NET MAUI API | Comments |
| ----------------- | ------------- | -------- |
| <xref:Xamarin.Forms.Device.Android?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.Android?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Device.iOS?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.iOS?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.GTK?displayProperty=fullName>  |  | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Device.macOS?displayProperty=fullName>  | | No .NET MAUI equivalent. Instead, use <xref:Microsoft.Maui.Devices.DevicePlatform.MacCatalyst?displayProperty=fullName>. |
| <xref:Xamarin.Forms.Device.Tizen?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.Tizen?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.UWP?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.WinUI?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.WPF?displayProperty=fullName>  |  |  No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Device.Flags?displayProperty=fullName>  | | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Device.FlowDirection?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.AppInfo.RequestedLayoutDirection?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.Idiom?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Idiom?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.IsInvokeRequired?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.Dispatcher.IsDispatchRequired?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.OS?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.RuntimePlatform?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.BeginInvokeOnMainThread%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.IDispatcher.Dispatch%2A?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.GetMainThreadSynchronizationContextAsync%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.DispatcherExtensions.GetSynchronizationContextAsync%2A?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.GetNamedColor%2A?displayProperty=fullName>  | | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Device.GetNamedSize%2A?displayProperty=fullName>  | | No .NET MAUI equivalent.|
| <xref:Xamarin.Forms.Device.Invalidate%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Controls.VisualElement.InvalidateMeasure%2A?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.InvokeOnMainThreadAsync%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.DispatcherExtensions.DispatchAsync%2A?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.OnPlatform%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.OpenUri%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.Launcher.OpenAsync%2A?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.SetFlags%2A?displayProperty=fullName>  | | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Device.SetFlowDirection%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Controls.Window.FlowDirection?displayProperty=fullName> | |
| <xref:Xamarin.Forms.Device.StartTimer%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer%2A?displayProperty=fullName> or <xref:Microsoft.Maui.Dispatching.Dispatcher.DispatchDelayed%2A?displayProperty=fullName> | |

### Maps

| Xamarin.Forms API | .NET MAUI API | Comment |
| ----------------- | ------------- | ------- |
| <xref:Xamarin.Forms.Maps.Map.HasScrollEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsScrollEnabled?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Maps.Map.HasZoomEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsZoomEnabled?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Maps.Map.TrafficEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsTrafficEnabled?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Maps.Map.MoveToLastRegionOnLayoutChange%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Maps.Pin.Id?displayProperty=fullName> | | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Maps.Pin.Position?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Pin.Location?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Maps.MapClickedEventArgs.Position> | <xref:Microsoft.Maui.Controls.Maps.MapClickedEventArgs.Location> |  |
| <xref:Xamarin.Forms.Maps.Position?displayProperty=fullName> | <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> | Any members of type <xref:Xamarin.Forms.Maps.Position?displayProperty=fullName> have changed to the <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> type. |
| <xref:Xamarin.Forms.Maps.Geocoder?displayProperty=fullName> | <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> | Any members of type <xref:Xamarin.Forms.Maps.Geocoder?displayProperty=fullName> have changed to the <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> type.  |

### Others

| Xamarin.Forms API | .NET MAUI API | Comments |
| ----------------- | ------------- | -------- |
| <xref:Xamarin.Forms.OSAppTheme?displayProperty=fullName> | <xref:Microsoft.Maui.ApplicationModel.AppTheme?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.ToolbarItem.Name?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> is the base class for <xref:Microsoft.Maui.Controls.ToolbarItem?displayProperty=fullName>, and so `ToolbarItem.Name` becomes `ToolbarItem.Text`. |
| <xref:Xamarin.Forms.MenuItem.Icon?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource?displayProperty=fullName> | <xref:Xamarin.Forms.MenuItem.Icon?displayProperty=fullName> is the base class for <xref:Xamarin.Forms.ToolbarItem?displayProperty=fullName>, and so `ToolbarItem.Icon` becomes `ToolbarItem.IconImageSource`. |
| <xref:Xamarin.Forms.Span.ForegroundColor?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Span.TextColor?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Button.Image?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Button.ImageSource?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Frame.OutlineColor?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Frame.BorderColor?displayProperty=fullName> |  |
| <xref:Xamarin.Forms.Grid.IGridList`1.Add%2A?displayProperty=fullName> |  | The `Add` overload that accepts 5 arguments isn't present in .NET MAUI. |
| <xref:Xamarin.Forms.Grid.IGridList`1.AddHorizontal%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.Grid.IGridList`1.AddVertical%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
| <xref:Xamarin.Forms.IQueryAttributable.ApplyQueryAttributes%2A?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.IQueryAttributable.ApplyQueryAttributes%2A?displayProperty=fullName> | In Xamarin.Forms, the `ApplyQueryAttributes` method accepts an `IDictionary<string, string>` argument. In .NET MAUI, the `ApplyQueryAttributes` method accepts an `IDictionary<string, object>` argument.  |
| <xref:Xamarin.Forms.Application.Properties?displayProperty=fullName> | <xref:Microsoft.Maui.Storage.Preferences> |  |

### Custom layouts

The process for creating a custom layout in Xamarin.Forms involves creating a class that derives from `Layout<View>`, and overriding the <xref:Xamarin.Forms.VisualElement.OnMeasure%2A> and <xref:Xamarin.Forms.Layout.LayoutChildren%2A> methods. For more information, see [Create a custom layout in Xamarin.Forms](/xamarin/xamarin-forms/user-interface/layouts/custom).

The process for creating a custom layout in .NET MAUI involves creating an <xref:Microsoft.Maui.Layouts.ILayoutManager> implementation, and overriding the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> methods:

- The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> override should call <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> on each <xref:Microsoft.Maui.IView> in the layout, and should return the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> override should determine where each <xref:Microsoft.Maui.IView> should be placed within the given bounds, and should call <xref:Microsoft.Maui.IView.Arrange%2A> on each <xref:Microsoft.Maui.IView> with its appropriate bounds. The return value should be the actual size of the layout.

For more information, see [Custom layout examples](https://github.com/hartez/CustomLayoutExamples).

<!-- TODO: Replace the link above with one to a custom layout doc, once the content is written -->

### Native forms

[Native forms](/xamarin/xamarin-forms/platform/native-forms) in Xamarin.Forms has become Native embedding in .NET MAUI, and uses a different initialization approach and different extension methods to convert cross-platform controls to their native types. For more information, see [Native embedding](~/platform-integration/native-embedding.md)]

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

The following table provides guidance for overcoming common build or runtime issues:

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
| `BoxView` | The default size of a `BoxView` in Xamarin.Forms is 40x40. The default size of a `BoxView` in .NET MAUI is 0x0. |

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
