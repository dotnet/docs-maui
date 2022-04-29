---
title: "Xamarin.Essentials Platform Extensions"
description: "Xamarin.Essentials provides several platform extension methods when having to work with platform types such as Rect, Size, and Point."
ms.date: 03/13/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

<!-- TODO update article -->
# Platform Extensions

Xamarin.Essentials provides several platform extension methods when having to work with platform types such as Rect, Size, and Point. This means that you can convert between the `System` version of these types for their iOS, Android, and UWP specific types.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

## Using Platform Extensions

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

All platform extensions can only be called from the iOS, Android, or UWP project.

## Android Extensions

These extensions can only be accessed from an Android project.

### Application Context & Activity

Using the platform extensions in the `Platform` class you can get access to the current `Context` or `Activity` for the running app.

```csharp

var context = Platform.AppContext;

// Current Activity or null if not initialized or not started.
var activity = Platform.CurrentActivity;
```

If there is a situation where the `Activity` is needed, but the application hasn't fully started then the `WaitForActivityAsync` method should be used.

```csharp
var activity = await Platform.WaitForActivityAsync();
```

### Activity Lifecycle

In addition to getting the current Activity, you can also register for lifecycle events.

```csharp
protected override void OnCreate(Bundle bundle)
{
    base.OnCreate(bundle);

    Xamarin.Essentials.Platform.Init(this, bundle);

    Xamarin.Essentials.Platform.ActivityStateChanged += Platform_ActivityStateChanged;
}

protected override void OnDestroy()
{
    base.OnDestroy();
    Xamarin.Essentials.Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
}

void Platform_ActivityStateChanged(object sender, Xamarin.Essentials.ActivityStateChangedEventArgs e) =>
    Toast.MakeText(this, e.State.ToString(), ToastLength.Short).Show();
```

Activity states are the following:

* Created
* Resumed
* Paused
* Destroyed
* SaveInstanceState
* Started
* Stopped

<!-- TODO Read the [Activity Lifecycle](../android/app-fundamentals/activity-lifecycle/index.md) documentation to learn more.-->

## iOS Extensions

These extensions can only be accessed from an iOS project.

### Current UIViewController

Gain access to the currently visible `UIViewController`:

```csharp
var vc = Platform.GetCurrentUIViewController();
```

This method will return `null` if unable to detect a `UIViewController`.

## Cross-platform Extensions

These extensions exist in every platform.

### Point

```csharp
var system = new System.Drawing.Point(x, y);

// Convert to CoreGraphics.CGPoint, Android.Graphics.Point, and Windows.Foundation.Point
var platform = system.ToPlatformPoint();

// Back to System.Drawing.Point
var system2 = platform.ToSystemPoint();
```

### Size

```csharp
var system = new System.Drawing.Size(width, height);

// Convert to CoreGraphics.CGSize, Android.Util.Size, and Windows.Foundation.Size
var platform = system.ToPlatformSize();

// Back to System.Drawing.Size
var system2 = platform.ToSystemSize();
```

### Rectangle

```csharp
var system = new System.Drawing.Rectangle(x, y, width, height);

// Convert to CoreGraphics.CGRect, Android.Graphics.Rect, and Windows.Foundation.Rect
var platform = system.ToPlatformRectangle();

// Back to System.Drawing.Rectangle
var system2 = platform.ToSystemRectangle();
```

## API

- [Converters source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Types/PlatformExtensions)
<!-- - [Point Converters API documentation](xref:Microsoft.Maui.Essentials.PointExtensions)-->
<!-- - [Rectangle Converters API documentation](xref:Microsoft.Maui.Essentials.RectangleExtensions)-->
<!-- - [Size Converters API documentation](xref:Microsoft.Maui.Essentials.SizeExtensions)-->
