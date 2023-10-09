---
title: What's new in .NET MAUI for .NET 8
description: Learn about the new features introduced in .NET MAUI for .NET 8.
ms.date: 10/02/2023
---

# What's new in .NET MAUI for .NET 8

.NET 8 is the successor to .NET 7 and focuses on being unified, modern, simple, and *fast*. Due to working with underlying external dependencies, such as Xcode or Android SDK Tools, the .NET Multi-platform App UI (.NET MAUI) support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

This article lists the new features of .NET MAUI for .NET 8 and provides links to more detailed information on each.

For information about what's new in .NET 8, see [What's new in .NET 8](/dotnet/core/whats-new/dotnet-8).

## New functionality

.NET MAUI for .NET 8 addresses top feedback issues and introduces the following new functionality:

- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> gains a `StartPath` property, a `TryDispatchAsync` method, and enhanced logging capabilities. For more information, see [Host a Blazor web app in a .NET MAUI app using BlazorWebView](~/user-interface/controls/blazorwebview.md).
- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> gains `PointerPressedCommand`, `PointerPressedCommandParameter`, `PointerReleasedCommand`, `PointerReleasedCommandParameter` properties, and `PointerPressed` and `PointerReleased` events. For more information, see [Recognize a pointer gesture](~/fundamentals/gestures/pointer.md).
- <xref:Microsoft.Maui.Devices.Flashlight> gains a `IsSupportedAsync` method that determines whether a flashlight is available on the device. For more information, see [Flashlight](~/platform-integration/device/flashlight.md).
- <xref:Microsoft.Maui.Devices.Sensors.SensorSpeed> intervals have been unified across all platforms. For more information, see [Accessing device sensors](~/platform-integration/device/sensors.md).
- <xref:Microsoft.Maui.Controls.SolidColorBrush.Color> is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.SolidColorBrush> class, and therefore does not need to be explicitly set from XAML.

The following members have been deprecated:

- The [`AutomationProperties.Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty), [`AutomationProperties.HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty), and [`AutomationProperties.LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty) attached properties have been deprecated.

The following members have been removed:

- The `Application.Properties` property and `Application.SavePropertiesAsync` method have been removed. To migrate your app properties data to .NET MAUI, see [Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences](~/migration/app-properties.md).
- The `PhoneDialer.Current` property has been removed. Use `PhoneDialer.Default` instead.
- `OpenGLView` has been removed.

<!-- - The <xref:Microsoft.Maui.Controls.Maps.Map> control is a cross-platform view for displaying and annotating maps. The <xref:Microsoft.Maui.Controls.Maps.Map> control uses the native map control on each platform, and is provided by the [Microsoft.Maui.Controls.Maps NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Maps/). For more information, see [Map](~/user-interface/controls/map.md).
- The <xref:Microsoft.Maui.Controls.Foldable.TwoPaneView> control is a container control for foldable devices that provides two views that size and position content in the available space, either side-by-side or top-to-bottom. This control is provided by the [Microsoft.Maui.Controls.Foldable NuGet package](https://www.nuget.org/packages/Microsoft.Maui.Controls.Foldable/).
- The <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> class performs pointer gesture recognition and detects when the mouse pointer enters, exits, and moves within a view. For more information, see [Recognize a pointer gesture](~/fundamentals/gestures/pointer.md). .NET MAUI also defines a `PointerOver` visual state that can change the visual appearance of a view when it has a mouse cursor hovering over it. For more information, see [Visual states](~/user-interface/visual-states.md).
- The `Window` class defines additional properties and enables the window to be positioned and sized on desktop platforms. For more information, see [.NET MAUI Windows](~/fundamentals/windows.md).
- Tooltips can be displayed for a view, when the user rests a pointer on the view. For more information, see [Display tooltips](~/user-interface/tooltips.md).
- Context menus can be added to any control that derives from <xref:Microsoft.Maui.Controls.Element>, on Mac Catalyst and Windows. For more information, see [Display a context menu](~/user-interface/context-menu.md).
- Menu bars can contain separators, which are horizontal lines that separate items in the menu. For more information, see [Display a menu bar](~/user-interface/menu-bar.md).
- Additional .NET MAUI delegates are invoked in response to iOS lifecycle events being raised. For more information, see [App lifecycle](~/fundamentals/app-lifecycle.md#ios).
- The underlying native control for the <xref:Microsoft.Maui.Controls.WebView> on iOS and Mac Catalyst can be configured with a `WKWebViewConfiguration` object. For more information, see [Configure the native WebView on iOS and Mac Catalyst](~/user-interface/controls/webview.md#configure-the-native-webview-on-ios-and-mac-catalyst).
WebView config on iOS/Mac

In addition, `MessagingCenter` has been deprecated and replaced with `WeakReferenceMessenger` in the [CommunityToolkit.Mvvm NuGet package](https://www.nuget.org/packages/CommunityToolkit.Mvvm). -->

<!-- ## Performance

Performance is a key focus of .NET MAUI in .NET 7:

- On Android, startup performance has improved.
- On iOS, application size has been reduced.
- The rendering path for views has been optimized.
- Many issues have been addressed that impacted the smoothness of scrolling in a <xref:Microsoft.Maui.Controls.CollectionView>.

For more information, see [.NET 7 Performance Improvements in .NET MAUI](https://devblogs.microsoft.com/dotnet/dotnet-7-performance-improvements-in-dotnet-maui/). -->

<!-- ## Upgrading from .NET 7

To upgrade your projects from .NET 6 to .NET 7, open your *.csproj* file and change the Target Framework Monikers (TFMs) from 6 to 7. The following example shows the TFMs for a .NET 6 project:

```xml
<TargetFrameworks>net6.0-ios;net6.0-android;net6.0-maccatalyst;net6.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows')) and '$(MSBuildRuntimeType)' == 'Full'">$(TargetFrameworks);net6.0-windows10.0.19041</TargetFrameworks>
```

The following example shows the TFMs for a .NET 7 project:

```xml
<TargetFrameworks>net7.0-ios;net7.0-android;net7.0-maccatalyst;net7.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows')) and '$(MSBuildRuntimeType)' == 'Full'">$(TargetFrameworks);net7.0-windows10.0.19041</TargetFrameworks>
``` -->

<!-- ## See also

- [Release notes for .NET MAUI in .NET 7](https://github.com/dotnet/maui/releases/tag/7.0.49)
- [Release notes for .NET iOS, tvOS, macOS, and Mac Catalyst](https://github.com/xamarin/xamarin-macios/wiki/.NET-7-release-notes)
- [Release notes for .NET Android](https://github.com/xamarin/xamarin-android/releases/tag/33.0.4) -->
