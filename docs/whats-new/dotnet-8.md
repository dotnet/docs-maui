---
title: What's new in .NET MAUI for .NET 8
description: Learn about the new features introduced in .NET MAUI for .NET 8.
ms.date: 10/10/2023
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
- The position at which a drag or drop gesture occurred can be obtained by calling the `GetPosition` method on a <xref:Microsoft.Maui.Controls.DragEventArgs>, <xref:Microsoft.Maui.Controls.DragStartingEventArgs>, or <xref:Microsoft.Maui.Controls.DropEventArgs> object. For more information, see [Recognize a drag and drop gesture](~/fundamentals/gestures/drag-and-drop.md).
- <xref:Microsoft.Maui.Controls.WebView> gains a `UserAgent` property. For more information, see [WebView](~/user-interface/controls/webview.md).
- Inline media playback of HTML5 video, including autoplay and picture in picture, has been enabled by default for the <xref:Microsoft.Maui.Controls.WebView> on iOS. For more information, see [Set media playback preferences on iOS and Mac Catalyst](~/user-interface/controls/webview.md#set-media-playback-preferences-on-ios-and-mac-catalyst).
- Resources defined in a <xref:Microsoft.Maui.Controls.ResourceDictionary> can also be consumed in an [`AppThemeBinding`](xref:Microsoft.Maui.Controls.Xaml.AppThemeBindingExtension) with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. For more information, see [Define and consume theme resources](~/user-interface/system-theme-changes.md#define-and-consume-theme-resources).
- The `Grid.Add` overload that accepts 5 arguments has been added back to .NET MAUI. However, this method is deprecated and is only present to aid migrations from Xamarin.Forms.
- <xref:Microsoft.Maui.Controls.Grid> gains an `AddWithSpan` extension method that adds a view to the `Grid` at the specified row and column with the specified row and column spans.
- On iOS, `MauiUIApplicationDelegate` gains a `PerformFetch` method that can be overridden or consumed via the `iOSLifecycle.PerformFetch` delegate. For more information, see [iOS platform lifecycle events](~/fundamentals/app-lifecycle.md#ios).
- The <xref:Microsoft.Maui.Controls.TapGestureRecognizer> class gains the ability to handle secondary taps on Android. For more information, see [Recognize a tap gesture](~/fundamentals/gestures/tap.md).
- The <xref:Microsoft.Maui.Controls.PointerEventArgs> object that accompanies the pointer events raised by the <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> class gains a `PlatformArgs` property of type `PlatformPointerEventArgs`. This property provides access to the platform-specific arguments for a pointer gesture event. For more information, see [Recognize a pointer gesture](~/fundamentals/gestures/pointer.md).
- The <xref:Microsoft.Maui.Controls.DragStartingEventArgs>, <xref:Microsoft.Maui.Controls.DragEventArgs>, <xref:Microsoft.Maui.Controls.DropEventArgs>, and <xref:Microsoft.Maui.Controls.DropCompletedEventArgs> objects that accompany drag and drop gesture events each gain a `PlatformArgs` property. This property provides access to the platform-specific arguments for a drag or drop event. For more information, see [Recognize a drag and drop gesture](~/fundamentals/gestures/drag-and-drop.md).
- For troubleshooting purposes, resource generation can be disabled. For more information, see [Disable image packaging](~/troubleshooting.md#disable-image-packaging), [Disable splash screen packaging](~/troubleshooting.md#disable-splash-screen-packaging), [Disable font packaging](~/troubleshooting.md#disable-font-packaging), and [Disable asset file packaging](~/troubleshooting.md#disable-asset-file-packaging).
- For troubleshooting purposes, a blank splash screen can be generated. For more information, see [Generate a blank splash screen](~/troubleshooting.md#generate-a-blank-splash-screen).
- Resizeter checks for duplicate image filenames. For more information, see [Duplicate image filename errors](~/troubleshooting.md#duplicate-image-filename-errors).
- Controls that support text input gain extension methods that support hiding and showing the soft input keyboard. For more information, see [Hide and show the soft input keyboard](~/user-interface/controls/entry.md#hide-and-show-the-soft-input-keyboard).
- The `x:ClassModifier` attribute can be specified on XAML classes, to control the access level for a generated class in an assembly. For more information, see [Class modifiers](~/xaml/class-modifiers.md).
- The <xref:Microsoft.Maui.ApplicationModel.Permissions> class gains the `Bluetooth` permission, which is an Android 12 permission for looking for Bluetooth devices, making the current device discoverable to other Bluetooth devices, and communicating with already-paired Bluetooth devices. For more information, see [Permissions](~/platform-integration/appmodel/permissions.md).
- The <xref:Microsoft.Maui.ApplicationModel.Permissions> class gains the `NearbyWifiDevices` permission, which is an Android 13 permission for accessing nearby WiFi devices. For more information, see [Permissions](~/platform-integration/appmodel/permissions.md).

The following types or members have been deprecated:

- <xref:Microsoft.Maui.Controls.ClickGestureRecognizer> has been deprecated.
- The [`AutomationProperties.Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty), [`AutomationProperties.HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty), and [`AutomationProperties.LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty) attached properties have been deprecated.
- The <xref:Microsoft.Maui.Controls.VisualElement.FocusChangeRequested> has been deprecated. Instead, use the <xref:Microsoft.Maui.IView.Focus> method to attempt to set focus on the view.

The following types or members have been removed:

- The `Application.Properties` property and `Application.SavePropertiesAsync` method have been removed. To migrate your app properties data to .NET MAUI, see [Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences](~/migration/app-properties.md).
- The `PhoneDialer.Current` property has been removed. Use `PhoneDialer.Default` instead.
- `OpenGLView` has been removed.

The following behavior has changed from the previous release:

- Image caching is disabled on Android when loading an image from a stream with <xref:Microsoft.Maui.Controls.ImageSource.FromStream%2A>. This is due to the lack of data from which to create a reasonable cache key.

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
