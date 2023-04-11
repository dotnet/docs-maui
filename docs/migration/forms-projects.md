---
title: "Manually upgrade a Xamarin.Forms app to .NET MAUI"
description: "Learn how to manually upgrade a Xamarin.Forms project to .NET MAUI."
ms.date: 3/02/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net7.0-android", "net7.0-ios" ]
---

# Manually upgrade a Xamarin.Forms app to .NET MAUI

Upgrading a Xamarin.Forms app to a .NET Multi-platform App UI (.NET MAUI) app follows the same steps as a Xamarin.Android and Xamarin.iOS project, with additional steps to take advantage of changes in .NET MAUI.

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

To simplify the upgrade process, we recommend creating a new .NET MAUI library project of the same name as your Xamarin.Forms library project, and then copying in your code. This is the approach outlined below.

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

## Namespace changes

Namespaces have changed in the move from Xamarin.Forms to .NET MAUI, and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, perform a find and replace for the following namespaces:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms namespace | .NET MAUI namespace(s) |
> | --- | --- |
> | <xref:Xamarin.Forms> | <xref:Microsoft.Maui> and <xref:Microsoft.Maui.Controls> |
> | <xref:Xamarin.Forms.DualScreen> | <xref:Microsoft.Maui.Controls.Foldable> |
> | <xref:Xamarin.Forms.Maps> | <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Maps> |
> | <xref:Xamarin.Forms.PlatformConfiguration> | <xref:Microsoft.Maui.Controls.PlatformConfiguration> |
> | <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat> |
> | <xref:Xamarin.Forms.PlatformConfiguration.TizenSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.WindowsSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific> |
> | <xref:Xamarin.Forms.PlatformConfiguration.iOSSpecific> | <xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific> |
> | <xref:Xamarin.Forms.Shapes> | <xref:Microsoft.Maui.Controls.Shapes> |
> | <xref:Xamarin.Forms.StyleSheets> | <xref:Microsoft.Maui.Controls.StyleSheets> |
> | <xref:Xamarin.Forms.Xaml> | <xref:Microsoft.Maui.Controls.Xaml> |

.NET MAUI projects make use of implicit `global using` directives. This enables you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to replace them with the equivalent .NET MAUI namespaces.

In addition, the default XAML namespace has changed from `http://xamarin.com/schemas/2014/forms` in Xamarin.Forms to `http://schemas.microsoft.com/dotnet/2021/maui` in .NET MAUI. Therefore, you should replace all occurrences of `xmlns="http://xamarin.com/schemas/2014/forms"` with `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"`.

## API changes

Some APIs have changed in the move from Xamarin.Forms to .NET MAUI. This is multiple reasons including removing duplicate functionality caused by Xamarin.Essentials becoming part of .NET MAUI, and ensuring that APIs follow .NET naming guidelines. The following sections discuss these changes.

### Color changes

In Xamarin.Forms, the <xref:Xamarin.Forms.Color?displayProperty=fullName> struct lets you construct <xref:Microsoft.Maui.Graphics.Color> objects using `double` values, and provides named colors, such as <xref:Xamarin.Forms.Color.AliceBlue?displayProperty=fullName>. In .NET MAUI, this functionality has been separated into the <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, and the <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class.

The <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class, in the <xref:Microsoft.Maui.Graphics> namespace, lets you construct <xref:Microsoft.Maui.Graphics.Color> objects using `float` values, `byte` values, and `int` values. The <xref:Microsoft.Maui.Graphics.Colors?displayProperty=fullName> class, which is also in the <xref:Microsoft.Maui.Graphics> namespace, largely provides the same named colors.

The following table shows the API changes between the <xref:Xamarin.Forms.Color?displayProperty=fullName> struct and the <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> class:

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

In addition, all of the numeric values in a <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> are `float`, rather than `double` as used in <xref:Xamarin.Forms.Color?displayProperty=fullName>.

> [!NOTE]
> Unlike Xamarin.Forms, a <xref:Microsoft.Maui.Graphics.Color?displayProperty=fullName> doesn't have an implicit conversion to <xref:System.Drawing.Color?displayProperty=fullName>.

### Layout changes

The following table lists the layout APIs that have been removed in the move from Xamarin.Forms to .NET MAUI:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | <xref:Xamarin.Forms.AbsoluteLayout.IAbsoluteList`1.Add%2A?displayProperty=fullName> |  | The `Add` overload that accepts 3 arguments isn't present in .NET MAUI. |
> | <xref:Xamarin.Forms.Grid.IGridList`1.Add%2A?displayProperty=fullName> |  | The `Add` overload that accepts 5 arguments isn't present in .NET MAUI. |
> | <xref:Xamarin.Forms.Grid.IGridList`1.AddHorizontal%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Grid.IGridList`1.AddVertical%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.RelativeLayout?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Compatibility.RelativeLayout?displayProperty=fullName> | In .NET MAUI, `RelativeLayout` only exists as a compatibility control for users migrating from Xamarin.Forms. Use <xref:Microsoft.Maui.Controls.Grid> instead, or add the `xmlns` for the compatibility namespace. |

In addition, adding children to a layout in code in Xamarin.Forms is accomplished by adding the children to the layout's `Children` collection:

```csharp
Grid grid = new Grid();
grid.Children.Add(new new Label { Text = "Hello world" });
```

In .NET MAUI, the <xref:Microsoft.Maui.Controls.Layout.Children> collection is for internal use by .NET MAUI and shouldn't be manipulated directly. Therefore, in code children should be added directly to the layout:

```csharp
Grid grid = new Grid();
grid.Add(new new Label { Text = "Hello world" });
```

> [!IMPORTANT]
> Any `Add` layout extension methods, such as <xref:Microsoft.Maui.Controls.GridExtensions.Add%2A?displayProperty=nameWithType>, are invoked on the layout rather than the layouts <xref:Microsoft.Maui.Controls.Layout.Children> collection.

You may notice when running your upgraded .NET MAUI app that layout behavior is different. For more information, see [Layout behavior changes from Xamarin.Forms](layouts.md).

### Custom layout changes

The process for creating a custom layout in Xamarin.Forms involves creating a class that derives from `Layout<View>`, and overriding the <xref:Xamarin.Forms.VisualElement.OnMeasure%2A> and <xref:Xamarin.Forms.Layout.LayoutChildren%2A> methods. For more information, see [Create a custom layout in Xamarin.Forms](/xamarin/xamarin-forms/user-interface/layouts/custom).

The process for creating a custom layout in .NET MAUI involves creating an <xref:Microsoft.Maui.Layouts.ILayoutManager> implementation, and overriding the <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> methods:

- The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> override should call <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> on each <xref:Microsoft.Maui.IView> in the layout, and should return the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> override should determine where each <xref:Microsoft.Maui.IView> should be placed within the given bounds, and should call <xref:Microsoft.Maui.IView.Arrange%2A> on each <xref:Microsoft.Maui.IView> with its appropriate bounds. The return value should be the actual size of the layout.

For more information, see [Custom layout examples](https://github.com/hartez/CustomLayoutExamples).

<!-- TODO: Replace the link above with one to a custom layout doc, once the content is written -->

### Device changes

Xamarin.Forms has a <xref:Xamarin.Forms.Device?displayProperty=fullName> class that helps you to interact with the device and platform the app is running on. The equivalent class in .NET MAUI, <xref:Microsoft.Maui.Controls.Device?displayProperty=fullName>, is deprecated and its functionality is replaced by multiple types.

The following table shows the .NET MAUI replacements for the functionality in the <xref:Xamarin.Forms.Device?displayProperty=fullName> class:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | <xref:Xamarin.Forms.Device.Android?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.Android?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Device.iOS?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.iOS?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.GTK?displayProperty=fullName>  |  | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Device.macOS?displayProperty=fullName>  | | No .NET MAUI equivalent. Instead, use <xref:Microsoft.Maui.Devices.DevicePlatform.MacCatalyst?displayProperty=fullName>. |
> | <xref:Xamarin.Forms.Device.Tizen?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.Tizen?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.UWP?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DevicePlatform.WinUI?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.WPF?displayProperty=fullName>  |  |  No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Device.Flags?displayProperty=fullName>  | | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Device.FlowDirection?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.AppInfo.RequestedLayoutDirection?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.Idiom?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Idiom?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.IsInvokeRequired?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.Dispatcher.IsDispatchRequired?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.OS?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.RuntimePlatform?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.BeginInvokeOnMainThread%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread%2A?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.GetMainThreadSynchronizationContextAsync%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.MainThread.GetMainThreadSynchronizationContextAsync%2A?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.GetNamedColor%2A?displayProperty=fullName>  | | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Device.GetNamedSize%2A?displayProperty=fullName>  | | No .NET MAUI equivalent.|
> | <xref:Xamarin.Forms.Device.Invalidate%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Controls.VisualElement.InvalidateMeasure%2A?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.InvokeOnMainThreadAsync%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync%2A?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.OnPlatform%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Devices.DeviceInfo.Platform?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.OpenUri%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.ApplicationModel.Launcher.OpenAsync%2A?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.SetFlags%2A?displayProperty=fullName>  | | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Device.SetFlowDirection%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Controls.Window.FlowDirection?displayProperty=fullName> | |
> | <xref:Xamarin.Forms.Device.StartTimer%2A?displayProperty=fullName>  | <xref:Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer%2A?displayProperty=fullName> or <xref:Microsoft.Maui.Dispatching.Dispatcher.DispatchDelayed%2A?displayProperty=fullName> | |

### Map changes

In Xamarin.Forms, the <xref:Xamarin.Forms.Maps.Map> control and associated types are in the <xref:Xamarin.Forms.Maps?displayProperty=fullName> namespace. In .NET MAUI, this functionality has moved to the <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Maps> namespaces. Some properties have been renamed and some types have been replaced with equivalent types from Xamarin.Essentials.

The following table shows the .NET MAUI replacements for the functionality in the <xref:Xamarin.Forms.Maps> namespace:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comment |
> | ----------------- | ------------- | ------- |
> | <xref:Xamarin.Forms.Maps.Map.HasScrollEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsScrollEnabled?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.Map.HasZoomEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsZoomEnabled?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.Map.TrafficEnabled?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Map.IsTrafficEnabled?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.Map.MoveToLastRegionOnLayoutChange%2A?displayProperty=fullName> |  | No .NET MAUI equivalent. |
> | <xref:Xamarin.Forms.Maps.Pin.Id?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Pin.MarkerId?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.Pin.Position?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.Pin.Location?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.MapClickedEventArgs.Position?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Maps.MapClickedEventArgs.Location?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Maps.Position?displayProperty=fullName> | <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> | Members of type <xref:Xamarin.Forms.Maps.Position?displayProperty=fullName> have changed to the <xref:Microsoft.Maui.Devices.Sensors.Location?displayProperty=fullName> type. |
> | <xref:Xamarin.Forms.Maps.Geocoder?displayProperty=fullName> | <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> | Members of type <xref:Xamarin.Forms.Maps.Geocoder?displayProperty=fullName> have changed to the <xref:Microsoft.Maui.Devices.Sensors.Geocoding?displayProperty=fullName> type. |

.NET MAUI has two `Map` types - <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> and <xref:Microsoft.Maui.ApplicationModel.Map?displayProperty=fullName>. Because the <xref:Microsoft.Maui.ApplicationModel> namespace is one of .NET MAUI's `global using` directives, when using the <xref:Microsoft.Maui.Controls.Maps.Map?displayProperty=fullName> control from code you'll have to fully qualify your `Map` usage or use a [using alias](/dotnet/csharp/language-reference/keywords/using-directive#using-alias).

In XAML, an `xmlns` namespace definition should be added for the <xref:Microsoft.Maui.Controls.Maps.Map> control. While this isn't required, it prevents a collision between the `Polygon` and `Polyline` types, which exist in both the <xref:Microsoft.Maui.Controls.Maps> and <xref:Microsoft.Maui.Controls.Shapes> namespaces. For more information, see [Display a map](~/user-interface/controls/map.md#display-a-map).

### Other changes

A small number of other APIs have been consolidated in the move from Xamarin.Forms to .NET MAUI. The following table shows these changes:

> [!div class="mx-tdBreakAll"]
> | Xamarin.Forms API | .NET MAUI API | Comments |
> | ----------------- | ------------- | -------- |
> | <xref:Xamarin.Forms.Application.Properties?displayProperty=fullName> | <xref:Microsoft.Maui.Storage.Preferences?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Button.Image?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Button.ImageSource?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Frame.OutlineColor?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Frame.BorderColor?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.IQueryAttributable.ApplyQueryAttributes%2A?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.IQueryAttributable.ApplyQueryAttributes%2A?displayProperty=fullName> | In Xamarin.Forms, the `ApplyQueryAttributes` method accepts an `IDictionary<string, string>` argument. In .NET MAUI, the `ApplyQueryAttributes` method accepts an `IDictionary<string, object>` argument.  |
> | <xref:Xamarin.Forms.MenuItem.Icon?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource?displayProperty=fullName> | <xref:Xamarin.Forms.MenuItem.Icon?displayProperty=fullName> is the base class for <xref:Xamarin.Forms.ToolbarItem?displayProperty=fullName>, and so `ToolbarItem.Icon` becomes `ToolbarItem.IconImageSource`. |
> | <xref:Xamarin.Forms.OrientationStateTrigger.Orientation?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.OrientationStateTrigger.Orientation?displayProperty=fullName> | In Xamarin.Forms, the `OrientationStateTrigger.Orientation` property is of type <xref:Xamarin.Forms.Internals.DeviceOrientation>. In .NET MAUI, the `OrientationStateTrigger.Orientation` property is of type <xref:Microsoft.Maui.Devices.DisplayOrientation>. |
> | <xref:Xamarin.Forms.OSAppTheme?displayProperty=fullName> | <xref:Microsoft.Maui.ApplicationModel.AppTheme?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.Span.ForegroundColor?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.Span.TextColor?displayProperty=fullName> |  |
> | <xref:Xamarin.Forms.ToolbarItem.Name?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> | <xref:Microsoft.Maui.Controls.MenuItem.Text?displayProperty=fullName> is the base class for <xref:Microsoft.Maui.Controls.ToolbarItem?displayProperty=fullName>, and so `ToolbarItem.Name` becomes `ToolbarItem.Text`. |

### Native forms changes

[Native forms](/xamarin/xamarin-forms/platform/native-forms) in Xamarin.Forms has become native embedding in .NET MAUI, and uses a different initialization approach and different extension methods to convert cross-platform controls to their native types. For more information, see [Native embedding](~/platform-integration/native-embedding.md).

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

### AssemblyInfo changes

Properties that are typically set in an *AssemblyInfo.cs* file are now available in your SDK-style project. We recommend migrating them from *AssemblyInfo.cs* to your project file in every project, and removing the *AssemblyInfo.cs* file.

<!-- Todo: Feels like more is required here -->

Optionally, you can keep the *AssemblyInfo.cs* file and set the `GenerateAssemblyInfo` property in your project file to `false`:

```xml
<PropertyGroup>
  <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
</PropertyGroup>
```

For more information about the `GenerateAssemblyInfo` property, see [GenerateAssemblyInfo](/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo).

## Update app dependencies

Xamarin.Forms NuGet packages are not compatible with .NET 6+ unless they have been recompiled using .NET target framework monikers (TFMs). You can confirm a package is .NET 6+ compatible by looking at the **Frameworks** tab on [NuGet](https://nuget.org) for the package you're using, and checking that it lists one of the compatible frameworks shown in the following table:

| Compatible frameworks | Incompatible frameworks |
| --- | --- |
| net6.0-android, net7.0-android | monoandroid, monoandroid10.0 |
| net6.0-ios, net7.0-ios | monotouch, xamarinios, xamarinios10 |
| net6.0-maccatalyst, net7.0-maccatalyst |  |
| net6.0-windows, net7.0-windows | uap10.0.16299 |

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed below are still compatible with .NET 6+.

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the `net6` or newer frameworks above, regardless of also including incompatible frameworks, then the package is compatible. Compatible NuGet packages can be added to your .NET MAUI library project using the NuGet package manager in Visual Studio.

If you can't find a .NET 6+ compatible version of a NuGet package you should:

- Recompile the package with .NET TFMs, if you own the code.
- Look for a preview release of a .NET 6+ version of the package.
- Replace the dependency with a .NET 6+ compatible alternative.

## Compile and troubleshoot

Once your dependencies are resolved, you should build your project. Any errors will guide you towards next steps.

<!-- markdownlint-disable MD032 -->
> [!TIP]
> - Delete all *bin* and *obj* folders from all projects before opening and building projects in Visual Studio, particularly when changing .NET versions.
> - Delete the *Resource.designer.cs* generated file from the Android project.
<!-- markdownlint-enable MD032 -->

The following table provides guidance for overcoming common build or runtime issues:

| Issue | Tip |
| ----- | --- |
| `Xamarin.*` namespace doesn't exist. | Update the namespace to its .NET MAUI equivalent. For more information, see [Namespace changes](#namespace-changes). |
| API doesn't exist. | Update the API usage to its .NET MAUI equivalent. For more information, see [API changes](#api-changes). |
| App won't deploy. | Ensure that the required platform project is set to deploy in Visual Studio's Configuration Manager. |
| App won't launch. | Update each platform project's entry point class, and the app entry point. For more information, see [Boostrap your migrated app](#bootstrap-your-migrated-app). |
| <xref:Microsoft.Maui.Controls.CollectionView> doesn't scroll. | Check the container layout and the measured size of the <xref:Microsoft.Maui.Controls.CollectionView>. By default the control will take up as much space as the container allows. A <xref:Microsoft.Maui.Controls.Grid> will constrain children at its own size. However a <xref:Microsoft.Maui.Controls.StackLayout> will enable children to take up space beyond its bounds. |
| <xref:Microsoft.Maui.Controls.BoxView> not appearing. | The default size of a <xref:Microsoft.Maui.Controls.BoxView> in Xamarin.Forms is 40x40. The default size of a <xref:Microsoft.Maui.Controls.BoxView> in .NET MAUI is 0x0. Set `WidthRequest` and `HeightRequest` to 40. |
| Layout is missing padding, margin, or spacing. | Add default values to your project based on the .NET MAUI style resource. For more information, see [Default value changes from Xamarin.Forms](layouts.md#default-layout-value-changes-from-xamarinforms). |
| Custom layout doesn't work. | Custom layout code needs updating to work in .NET MAUI. For more information, see [Custom layout changes](#custom-layout-changes). |
| Custom renderer doesn't work. | Renderer code needs updating to work in .NET MAUI. For more information, see [Use custom renderers in .NET MAUI](custom-renderers.md). |
| Effect doesn't work. | Effect code needs updating to work in .NET MAUI. For more information, see [Use effects in .NET MAUI](effects.md). |

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
