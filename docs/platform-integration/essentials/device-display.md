---
title: "Device Display Information"
description: "Describes the DeviceDisplay class in Microsoft.Maui.Essentials, which provides screen metrics for the device on which the application is running."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Device Display Information

The `DeviceDisplay` class provides information about the device's screen metrics the application is running on and can request to keep the screen from falling asleep when the application is running.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using DeviceDisplay

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Main Display Info

In addition to basic device information the `DeviceDisplay` class contains information about the device's screen and orientation.

```csharp
// Get Metrics
var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

// Orientation (Landscape, Portrait, Square, Unknown)
var orientation = mainDisplayInfo.Orientation;

// Rotation (0, 90, 180, 270)
var rotation = mainDisplayInfo.Rotation;

// Width (in pixels)
var width = mainDisplayInfo.Width;

// Height (in pixels)
var height = mainDisplayInfo.Height;

// Screen density
var density = mainDisplayInfo.Density;
```

The `DeviceDisplay` class also exposes an event that can be subscribed to that is triggered whenever any screen metric changes:

```csharp
public class DisplayInfoTest
{
    public DisplayInfoTest()
    {
        // Subscribe to changes of screen metrics
        DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
    }

    void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs  e)
    {
        // Process changes
        var displayInfo = e.DisplayInfo;
    }
}
```

## Keep Screen On

The `DeviceDisplay` class exposes a `bool` property called `KeepScreenOn` that can be set to attempt to keep the device's display from turning off or locking.

```csharp
public class KeepScreenOnTest
{
    public void ToggleScreenLock()
    {
        DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
    }
}
```

## Platform differences

# [Android](#tab/android)

No differences.

# [iOS](#tab/ios)

- Accessing `DeviceDisplay` must be done on the UI thread or else an exception will be thrown. You can use the [`MainThread.BeginInvokeOnMainThread`](main-thread.md) method to run that code on the UI thread.

# [Windows](#tab/windows)

No differences.

--------------

## API

- [DeviceDisplay source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/DeviceDisplay)
<!-- - [DeviceDisplay API documentation](xref:Microsoft.Maui.Essentials.DeviceDisplay)-->
