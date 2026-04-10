---
title: What's new in .NET MAUI for .NET 11
description: Learn about the new features introduced in .NET MAUI for .NET 11.
ms.date: 07/14/2025
---

# What's new in .NET MAUI for .NET 11

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 11 is to improve product quality. For information about what's new in each .NET MAUI in .NET 11 release, see the following release notes:

- [.NET MAUI in .NET 11 Preview 1](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview1/dotnetmaui.md)
- [.NET MAUI in .NET 11 Preview 3](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview3/dotnetmaui.md)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 11, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

## Implicit XAML namespace declarations

:::moniker range=">=net-maui-11.0"

Starting in .NET 11, implicit XAML namespace declarations are enabled by default. XAML files no longer need the standard `xmlns` and `xmlns:x` declarations at the root element — the compiler injects them automatically. Existing explicit declarations still compile and can be used to disambiguate duplicate type names. For more information, see [GitHub PR #33834](https://github.com/dotnet/maui/pull/33834).

:::moniker-end

## Lazy ResourceDictionary

:::moniker range=">=net-maui-11.0"

XAML Source Generation now registers resource dictionary entries as factories, inflating each resource on demand instead of eagerly loading everything at startup. This can yield up to an ~8× improvement in resource dictionary initialization time for apps with large dictionaries. The optimization is automatic when XAML source generation is enabled — no code changes are required. For more information, see [GitHub PR #33826](https://github.com/dotnet/maui/pull/33826).

:::moniker-end

## InvalidateStyle and InvalidateVisualStates

:::moniker range=">=net-maui-11.0"

Two new APIs make it easier to reapply styles and visual states that have been mutated in place:

- `VisualElement.InvalidateStyle()` — forces a control to reapply its current <xref:Microsoft.Maui.Controls.Style>, picking up any property changes made directly on the style object.
- `VisualStateManager.InvalidateVisualStates(VisualElement)` — reapplies the current visual state group setters, useful when visual state property values change at runtime.

These methods are especially useful for Hot Reload scenarios and dynamic UI updates where styles or visual states are modified without replacing the entire style object. For more information, see [GitHub PR #34723](https://github.com/dotnet/maui/pull/34723).

```csharp
// Mutate a style in place and force the control to pick up the change
var style = myButton.Style;
style.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Red });
myButton.InvalidateStyle();

// Reapply visual states after changing a setter value
VisualStateManager.InvalidateVisualStates(myButton);
```

:::moniker-end

## Trimmable CSS

:::moniker range=">=net-maui-11.0"

.NET MAUI CSS support is now fully trimmable. If your app doesn't use CSS stylesheets, the CSS infrastructure is trimmed away during publish, reducing app size. No code changes are needed — the linker removes unused CSS types automatically. For more information, see [GitHub PR #33160](https://github.com/dotnet/maui/pull/33160).

:::moniker-end

## Controls

.NET MAUI in .NET 11 includes control enhancements and deprecations.

### LongPressGestureRecognizer

:::moniker range=">=net-maui-11.0"

.NET 11 adds a built-in <xref:Microsoft.Maui.Controls.LongPressGestureRecognizer> for handling long-press gestures. It supports a configurable press duration, a movement threshold to cancel the gesture if the user's finger moves too far, state tracking via `GestureState`, and command binding with `Command` and `CommandParameter`. For more information, see [GitHub PR #33432](https://github.com/dotnet/maui/pull/33432).

```xaml
<Image Source="dotnet_bot.png">
    <Image.GestureRecognizers>
        <LongPressGestureRecognizer Duration="500"
                                    LongPressed="OnLongPressed" />
    </Image.GestureRecognizers>
</Image>
```

```csharp
void OnLongPressed(object sender, LongPressGestureRecognizerEventArgs e)
{
    if (e.State == GestureState.Completed)
    {
        // Handle completed long press
    }
}
```

:::moniker-end

### Map

:::moniker range=">=net-maui-11.0"

The <xref:Microsoft.Maui.Controls.Maps.Map> control receives a significant set of enhancements in .NET 11 Preview 3:

#### Pin clustering

Enable pin clustering to group nearby pins at lower zoom levels. Set `IsClusteringEnabled` on the map and optionally assign a `ClusteringIdentifier` to each pin. Handle the `ClusterClicked` event to respond when a user taps a cluster.

```xaml
<maps:Map IsClusteringEnabled="True"
          ClusterClicked="OnClusterClicked" />
```

#### Custom pin icons

Pins can now display a custom image instead of the default marker by setting the `ImageSource` property:

```csharp
var pin = new Pin
{
    Label = "Custom pin",
    Location = new Location(47.6062, -122.3321),
    ImageSource = ImageSource.FromFile("custom_pin.png")
};
```

#### Custom JSON map styling (Android)

Apply a custom JSON style to the map on Android using the `MapStyle` property. This enables dark mode maps, hiding labels, or any styling supported by the Google Maps Styling API.

#### Map events and element properties

- `MapLongClicked` — fires when the user long-presses on the map.
- `Circle`, `Polygon`, and `Polyline` now raise click events (`MapElementClick`).
- `MapElement.IsVisible` and `MapElement.ZIndex` — control element visibility and draw order.
- `Pin.ShowInfoWindow()` / `Pin.HideInfoWindow()` — programmatically show or hide a pin's info window.
- `UserLocationChanged` event and `LastUserLocation` property — track the user's location in real time.

#### Animated MoveToRegion and MapSpan.FromLocations

`MoveToRegion` now supports animated transitions, and the new `MapSpan.FromLocations()` factory method creates a span that encompasses a collection of locations.

For more information, see GitHub PRs [#29101](https://github.com/dotnet/maui/pull/29101), [#33831](https://github.com/dotnet/maui/pull/33831), [#33950](https://github.com/dotnet/maui/pull/33950), [#33982](https://github.com/dotnet/maui/pull/33982), [#33985](https://github.com/dotnet/maui/pull/33985), [#33792](https://github.com/dotnet/maui/pull/33792), [#33799](https://github.com/dotnet/maui/pull/33799), [#33991](https://github.com/dotnet/maui/pull/33991), and [#33993](https://github.com/dotnet/maui/pull/33993).

:::moniker-end

## Platform features

.NET MAUI's platform features have received some updates in .NET 11.

### iOS PostNotifications permission

:::moniker range=">=net-maui-11.0"

`Permissions.PostNotifications` is now implemented on iOS, providing a cross-platform API for requesting notification authorization. Previously this permission was only functional on Android. Use it to request authorization before scheduling local notifications on iOS. For more information, see [GitHub PR #30132](https://github.com/dotnet/maui/pull/30132).

```csharp
var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
if (status == PermissionStatus.Granted)
{
    // Schedule notifications
}
```

:::moniker-end

## .NET for Android

.NET for Android in .NET 11 makes CoreCLR the default runtime for `Release` builds, and includes work to improve performance. For more information about .NET for Android in .NET 10, see the following release notes:

- [.NET for Android 11 Preview 1](https://github.com/dotnet/android/releases/)

### Feature

## CoreCLR by Default

CoreCLR is now the default runtime for `Release` builds. This should
improve compatibility with the rest of .NET as well as shorter startup
times, with a reasonable increase to application size.

We are always working to improve performance and app size, but please
file issues with stability or concerns by filing
[issues on GitHub](https://github.com/dotnet/android/issues).

If you would like to opt out of CoreCLR, and use the Mono runtime
instead, you can still do so via:

```xml
<PropertyGroup>
  <UseMonoRuntime>true</UseMonoRuntime>
</ProperyGroup>
```

## `dotnet run`

We have enhanced the .NET CLI with [Spectre.Console](https://spectreconsole.net/) to *prompt* when a selection is needed for `dotnet run`.

So, for multi-targeted projects like .NET MAUI, it will:

* Prompt for a `$(TargetFramework)`
* Prompt for a device, emulator, simulator if there are more than one.

Console output of your application should appear directly in the terminal, and Ctrl+C will terminate the application.

![GIF of `dotnet run` selections on Windows for Android](media/dotnet-11/dotnet-run-android-preview-1.gif)

![GIF of `dotnet run` selections on macOS for iOS](media/dotnet-11/dotnet-run-ios-preview-1.gif)

## .NET for iOS

.NET 11 on iOS, tvOS, Mac Catalyst, and macOS supports the following platform versions:

- iOS: 18.2
- tvOS: 18.2
- Mac Catalyst: 18.2
- macOS: 15.2

For more information about .NET 11 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 11.0.1xx Preview 1](https://github.com/dotnet/macios/releases/)

For information about known issues, see [Known issues in .NET 11](https://github.com/dotnet/macios/wiki/Known-issues-in-.NET11).

### Feature

Description

## See also

- [.NET MAUI roadmap](https://github.com/dotnet/maui/wiki/Roadmap)
- [What's new in .NET 11](/dotnet/core/whats-new/dotnet-11/overview)
