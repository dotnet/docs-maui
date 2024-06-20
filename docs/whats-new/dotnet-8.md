---
title: What's new in .NET MAUI for .NET 8
description: Learn about the new features introduced in .NET MAUI for .NET 8.
ms.date: 11/01/2023
---

# What's new in .NET MAUI for .NET 8

The focus of .NET MAUI in .NET 8 is quality. In .NET 8, 1618 pull requests were merged that closed 689 issues. These includes changes from the .NET MAUI team as well as the .NET MAUI community. These changes should result in a significant increase in quality in .NET 8.

> [!IMPORTANT]
> Due to working with underlying external dependencies, such as Xcode or Android SDK Tools, the .NET Multi-platform App UI (.NET MAUI) support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 8, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

This article lists the new features of .NET MAUI for .NET 8 and provides links to more detailed information on each.

For information about what's new in .NET 8, see [What's new in .NET 8](/dotnet/core/whats-new/dotnet-8).

## New functionality

While the focus of this release of .NET MAUI is quality, there's also some new functionality that enables new scenarios in your apps.

### Controls

- Controls that support text input gain extension methods that support hiding and showing the soft input keyboard. For more information, see [Hide and show the soft input keyboard](~/user-interface/controls/entry.md#hide-and-show-the-soft-input-keyboard).
- The <xref:Microsoft.Maui.Controls.ContentPage> class gains a <xref:Microsoft.Maui.Controls.ContentPage.HideSoftInputOnTapped> property, which indicates whether tapping anywhere on the page will cause the soft input keyboard to hide if it's visible. For more information, see [ContentPage](~/user-interface/pages/contentpage.md).
- <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> gains a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.StartPath> property, a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView.TryDispatchAsync%2A> method, and enhanced logging capabilities. For more information, see [Host a Blazor web app in a .NET MAUI app using BlazorWebView](~/user-interface/controls/blazorwebview.md).
- <xref:Microsoft.Maui.Controls.WebView> gains a <xref:Microsoft.Maui.Controls.WebView.UserAgent> property. For more information, see [WebView](~/user-interface/controls/webview.md).
- Inline media playback of HTML5 video, including autoplay and picture in picture, has been enabled by default for the <xref:Microsoft.Maui.Controls.WebView> on iOS. For more information, see [Set media playback preferences on iOS and Mac Catalyst](~/user-interface/controls/webview.md?pivots=devices-ios#set-media-playback-preferences-on-ios-and-mac-catalyst).
- The `Grid.Add` overload that accepts 5 arguments has been added back to .NET MAUI. However, this method is deprecated and is only present to aid migrations from Xamarin.Forms.
- <xref:Microsoft.Maui.Controls.Grid> gains an <xref:Microsoft.Maui.Controls.GridExtensions.AddWithSpan%2A> extension method that adds a view to the <xref:Microsoft.Maui.Controls.Grid> at the specified row and column with the specified row and column spans.

### Desktop

- Menu bar items and context menu items can be invoked through keyboard shortcuts known as keyboard accelerators. For more information, see [Keyboard accelerators](~/user-interface/keyboard-accelerators.md).
- Windows apps can be published as unpackaged apps. For more information, see [Publish an unpackaged .NET MAUI app for Windows with the CLI](~/windows/deployment/publish-unpackaged-cli.md).

### Gesture recognizers

- <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> gains <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressedCommand>, <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressedCommandParameter>, <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleasedCommand>, <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleasedCommandParameter> properties, and <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerPressed> and <xref:Microsoft.Maui.Controls.PointerGestureRecognizer.PointerReleased> events. For more information, see [Recognize a pointer gesture](~/fundamentals/gestures/pointer.md).
- The <xref:Microsoft.Maui.Controls.PointerEventArgs> object that accompanies the pointer events raised by the <xref:Microsoft.Maui.Controls.PointerGestureRecognizer> class gains a <xref:Microsoft.Maui.Controls.PointerEventArgs.PlatformArgs> property of type <xref:Microsoft.Maui.Controls.PlatformPointerEventArgs>. This property provides access to the platform-specific arguments for a pointer gesture event. For more information, see [Recognize a pointer gesture](~/fundamentals/gestures/pointer.md).
- The <xref:Microsoft.Maui.Controls.DragStartingEventArgs>, <xref:Microsoft.Maui.Controls.DragEventArgs>, <xref:Microsoft.Maui.Controls.DropEventArgs>, and <xref:Microsoft.Maui.Controls.DropCompletedEventArgs> objects that accompany drag and drop gesture events each gain a `PlatformArgs` property. This property provides access to the platform-specific arguments for a drag or drop event. For more information, see [Recognize a drag and drop gesture](~/fundamentals/gestures/drag-and-drop.md).
- The position at which a drag or drop gesture occurred can be obtained by calling the <xref:Microsoft.Maui.Controls.DragEventArgs.GetPosition%2A> method on a <xref:Microsoft.Maui.Controls.DragEventArgs>, <xref:Microsoft.Maui.Controls.DragStartingEventArgs>, or <xref:Microsoft.Maui.Controls.DropEventArgs> object. For more information, see [Recognize a drag and drop gesture](~/fundamentals/gestures/drag-and-drop.md).
- The <xref:Microsoft.Maui.Controls.TapGestureRecognizer> class gains the ability to handle secondary taps on Android. For more information, see [Recognize a tap gesture](~/fundamentals/gestures/tap.md).

### Navigation

- Shell navigation gains a <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> overload that enables you to pass single use navigation data, that's cleared after navigation has occurred, as a <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object. For more information, see [Pass single use object-based navigation data](~/fundamentals/shell/navigation.md#pass-single-use-object-based-navigation-data).

### Platform integration

- The <xref:Microsoft.Maui.Devices.Sensors.Geolocation> class can listen for location changes when app's are in the foreground. For more information, see [Listen for location changes](~/platform-integration/device/geolocation.md#listen-for-location-changes).
- <xref:Microsoft.Maui.Devices.Flashlight> gains a <xref:Microsoft.Maui.Devices.Flashlight.IsSupportedAsync%2A> method that determines whether a flashlight is available on the device. For more information, see [Flashlight](~/platform-integration/device/flashlight.md).
- <xref:Microsoft.Maui.Devices.Sensors.SensorSpeed> intervals have been unified across all platforms. For more information, see [Accessing device sensors](~/platform-integration/device/sensors.md).
- The <xref:Microsoft.Maui.ApplicationModel.Permissions> class gains the <xref:Microsoft.Maui.ApplicationModel.Permissions.Bluetooth> permission, which is an Android 12 permission for looking for Bluetooth devices, making the current device discoverable to other Bluetooth devices, and communicating with already-paired Bluetooth devices. For more information, see [Permissions](~/platform-integration/appmodel/permissions.md).
- The <xref:Microsoft.Maui.ApplicationModel.Permissions> class gains the <xref:Microsoft.Maui.ApplicationModel.Permissions.NearbyWifiDevices> permission, which is an Android 13 permission for accessing nearby WiFi devices. For more information, see [Permissions](~/platform-integration/appmodel/permissions.md).

### XAML

- The `x:ClassModifier` attribute can be specified on XAML classes, to control the access level for a generated class in an assembly. For more information, see [Class modifiers](~/xaml/class-modifiers.md).
- Resources defined in a <xref:Microsoft.Maui.Controls.ResourceDictionary> can also be consumed in an [`AppThemeBinding`](xref:Microsoft.Maui.Controls.Xaml.AppThemeBindingExtension) with the [`DynamicResource`](xref:Microsoft.Maui.Controls.Xaml.DynamicResourceExtension) markup extension. For more information, see [Define and consume theme resources](~/user-interface/system-theme-changes.md#define-and-consume-theme-resources).
- <xref:Microsoft.Maui.Controls.SolidColorBrush.Color> is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.SolidColorBrush> class, and therefore does not need to be explicitly set from XAML.

### Troubleshooting

- For troubleshooting purposes, resource generation can be disabled. For more information, see [Disable image packaging](~/troubleshooting.md#disable-image-packaging), [Disable splash screen packaging](~/troubleshooting.md#disable-splash-screen-packaging), [Disable font packaging](~/troubleshooting.md#disable-font-packaging), and [Disable asset file packaging](~/troubleshooting.md#disable-asset-file-packaging).
- For troubleshooting purposes, a blank splash screen can be generated. For more information, see [Generate a blank splash screen](~/troubleshooting.md#generate-a-blank-splash-screen).
- Resizeter checks for duplicate image filenames. For more information, see [Duplicate image filename errors](~/troubleshooting.md#duplicate-image-filename-errors).

### Miscellaneous

- Window management can be decoupled from the `App` class. For more information, see [Decouple window management from the App class](~/fundamentals/windows.md#decouple-window-management-from-the-app-class).
- Several system fonts can be easily consumed in Android apps. For more information, see [Consume fonts](~/user-interface/fonts.md#consume-fonts).
- On iOS, `MauiUIApplicationDelegate` gains a `PerformFetch` method that can be overridden or consumed via the `iOSLifecycle.PerformFetch` delegate. For more information, see [iOS platform lifecycle events](~/fundamentals/app-lifecycle.md#ios).

## Type deprecation and removal

The following types or members have been deprecated:

- <xref:Microsoft.Maui.Controls.ClickGestureRecognizer> has been deprecated.
- The [`AutomationProperties.Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty), [`AutomationProperties.HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty), and [`AutomationProperties.LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty) attached properties have been deprecated.
- The <xref:Microsoft.Maui.Controls.VisualElement.FocusChangeRequested> has been deprecated. Instead, use the <xref:Microsoft.Maui.IView.Focus> method to attempt to set focus on the view.

The following types or members have been removed:

- The `Application.Properties` property and `Application.SavePropertiesAsync` method have been removed. To migrate your app properties data to .NET MAUI, see [Migrate data from the Xamarin.Forms app properties dictionary to .NET MAUI preferences](~/migration/app-properties.md).
- The `PhoneDialer.Current` property has been removed. Use `PhoneDialer.Default` instead.
- `OpenGLView` has been removed.

## Behavior changes

The following behavior has changed from the previous release:

- Use of the <xref:Microsoft.Maui.Controls.Maps.Map> control from XAML now requires the following `xmlns` namespace declaration: `xmlns:maps="http://schemas.microsoft.com/dotnet/2021/maui/maps"`.
- Image caching is disabled on Android when loading an image from a stream with the [`ImageSource.FromStream`](xref:Microsoft.Maui.Controls.ImageSource.FromStream%2A) method. This is due to the lack of data from which to create a reasonable cache key.
- On iOS, pages automatically scroll when the soft input keyboard would cover a text entry field, so that the field is above the soft input keyboard. The `KeyboardAutoManagerScroll.Disconnect` method, in the `Microsoft.Maui.Platform` namespace, can be called to disable this default behavior. The `KeyboardAutoManagerScroll.Connect` method can be called to re-enable the behavior after it's been disabled.
- How the color of a tab is set in a Shell app has changed on some platforms. For more information, see [Tab appearance](~/fundamentals/shell/tabs.md#tab-appearance).
- It's not required to specify a value for the `$(ApplicationIdGuid)` build property in your app's project file. This is because .NET MAUI Windows apps no longer require a GUID as an app ID, and instead use the value of the `$(ApplicationId)` build property as the app ID. Therefore, the same reverse domain format app ID is now used across all platforms, such as com.mycompany.myapp.
- .NET MAUI Mac Catalyst apps are no longer limited to 50 menu items on the menu bar.
- The `PlatformImage.FromStream` method, in the `Microsoft.Maui.Graphics` namespace, can now be used to load images on Windows instead of having to use the `W2DImageLoadingService` class.
- On Android, animations respect the system animation settings. For more information, see [Basic animation](~/user-interface/animation/basic.md).

## Performance

There are plenty of performance changes in .NET MAUI 8. These changes can be classified into five areas:

- New features
  - [`AndroidStripILAfterAOT`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#androidstripilafteraot)
  - [`AndroidEnableMarshalMethods`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#androidenablemarshalmethods)
  - [NativeAOT on iOS](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#nativeaot-on-ios)
- Build and inner loop performance
  - [Filter Android `ps -A` output with `grep`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#filter-android-ps--a-output-with-grep)
  - [Port WindowsAppSDK usage of `vcmeta.dll` to C#](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#port-windowsappsdk-usage-of-vcmetadll-to-c)
  - [Improvements to remote iOS builds on Windows](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improvements-to-remote-ios-builds-on-windows)
  - [Improvements to Android inner-loop](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improvements-to-android-inner-loop)
  - [XAML Compilation no longer uses `LoadInSeparateAppDomain`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#xaml-compilation-no-longer-uses-loadinseparateappdomain)
- Performance or app size improvements
  - [Structs and `IEquatable` in .NET MAUI](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#structs-and-iequatable-in-net-maui)
  - [Fix performance issue in `{AppThemeBinding}`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#fix-performance-issue-in-appthemebinding)
  - [Address `CA1307` and `CA1309` for performance](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#address-ca1307-and-ca1309-for-performance)
  - [Address `CA1311` for performance](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#address-ca1311-for-performance)
  - [Remove unused `ViewAttachedToWindow` event on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#remove-unused-viewattachedtowindow-event-on-android)
  - [Remove unneeded `System.Reflection` for `{Binding}`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#remove-unneeded-systemreflection-for-binding)
  - [Use `StringComparer.Ordinal` for `Dictionary` and `HashSet`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#use-stringcomparerordinal-for-dictionary-and-hashset)
  - [Reduce Java interop in `MauiDrawable` on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#reduce-java-interop-in-mauidrawable-on-android)
  - [Improve layout performance of `Label` on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improve-layout-performance-of-label-on-android)
  - [Reduce Java interop calls for controls in .NET MAUI](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#reduce-java-interop-calls-for-controls-in-net-maui)
  - [Improve performance of `Entry.MaxLength` on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improve-performance-of-entrymaxlength-on-android)
  - [Improve memory usage of `CollectionView` on Windows](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#improve-memory-usage-of-collectionview-on-windows)
  - [Use `UnmanagedCallersOnlyAttribute` on Apple platforms](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#use-unmanagedcallersonlyattribute-on-apple-platforms)
  - [Faster Java interop for strings on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#faster-java-interop-for-strings-on-android)
  - [Faster Java interop for C# events on Android](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#faster-java-interop-for-c-events-on-android)
  - [Use Function Pointers for JNI](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#use-function-pointers-for-jni)
  - [Removed `Xamarin.AndroidX.Legacy.Support.V4`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#removed-xamarinandroidxlegacysupportv4)
  - [Deduplication of generics on iOS and macOS](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#deduplication-of-generics-on-ios-and-macos)
  - [Fix `System.Linq.Expressions` implementation on iOS-like platforms](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#fix-systemlinqexpressions-implementation-on-ios-like-platforms)
  - [Set `DynamicCodeSupport=false` for iOS and Catalyst](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#set-dynamiccodesupportfalse-for-ios-and-catalyst)
- Memory leaks
  - [Memory Leaks and Quality](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#memory-leaks-and-quality)
  - [Diagnosing leaks in .NET MAUI](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#diagnosing-leaks-in-net-maui)
  - [Patterns that cause leaks: C# events](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#patterns-that-cause-leaks-c-events)
  - [Circular references on Apple platforms](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#circular-references-on-apple-platforms)
  - [Roslyn analyzer for Apple platforms](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#roslyn-analyzer-for-apple-platforms)
- Tooling and documentation
  - [Simplified `dotnet-trace` and `dotnet-dsrouter`](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#simplified-dotnet-trace-and-dotnet-dsrouter)
  - [`dotnet-gcdump` Support for Mobile](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/#dotnet-gcdump-support-for-mobile)

For more information, see [.NET 8 Performance Improvements in .NET MAUI](https://devblogs.microsoft.com/dotnet/dotnet-8-performance-improvements-in-dotnet-maui/).

## Upgrade from .NET 7 to .NET 8

To upgrade your projects from .NET 7 to .NET 8, install .NET 8 and the .NET MAUI workload with [Visual Studio 17.8+](https://visualstudio.microsoft.com/vs/), or with the [standalone installer](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and the `dotnet workload install maui` command.

Then, open your *.csproj* file and change the Target Framework Monikers (TFMs) from 7 to 8. If you're using a TFM such as `net7.0-ios13.6` be sure to match the platform version or remove it entirely. The following example shows the TFMs for a .NET 7 project:

```xml
<TargetFrameworks>net7.0-android;net7.0-ios;net7.0-maccatalyst;net7.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net7.0-windows10.0.19041.0</TargetFrameworks>
```

The following example shows the TFMs for a .NET 8 project:

```xml
<TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
```

Explicit package references should also be added to your *.csproj* file for the following .NET MAUI NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
    <PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
</ItemGroup>
```

The `$(MauiVersion)` variable is referenced from the version of .NET MAUI you've installed. You can override this by adding the `$(MauiVersion)` build property to your *.csproj* file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
        <UseMaui>True</UseMaui>
        <MauiVersion>8.0.3</MauiVersion>
    </PropertyGroup>
</Project>
```

This can be useful when using ad-hoc builds from the [nightly feed](https://github.com/dotnet/maui/wiki/Nightly-Builds) or builds downloaded from pull requests.

In addition, the `$(ApplicationIdGuid)` build property can be removed from your *.csproj* file in .NET 8. For more information, see [Behavior changes](#behavior-changes).

Prior to building your upgraded app for the first time, delete the `bin` and `obj` folders.

> [!NOTE]
> The project template for a .NET MAUI app in .NET 8 enables the nullable context for the project with the `$(Nullable)` build property. For more information, see [Nullable](/dotnet/csharp/language-reference/compiler-options/language#nullable).

## See also

- [Release notes for .NET MAUI](https://github.com/dotnet/maui/releases/tag/8.0.3)
- [Release notes for .NET for iOS, tvOS, macOS, and Mac Catalyst](https://github.com/xamarin/xamarin-macios/wiki/.NET-8-release-notes)
- [Release notes for .NET for Android](https://github.com/xamarin/xamarin-android/releases/tag/34.0.43)
