---
title: "Device Display Information"
description: "Learn how to use the .NET MAUIDeviceDisplay class in the Microsoft.Maui.Essentials namespace, which provides screen metrics for the device on which the app is running."
ms.date: 08/17/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Device display information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `DeviceDisplay` class to read information about the device's screen metrics. This class can be used to request the screen stay awake while the app is running.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Main display info

The `DeviceDisplay.MainDisplayInfo` property returns information about the screen and orientation.

```csharp
// Get Metrics
DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

// Orientation (Landscape, Portrait, Square, Unknown)
DisplayOrientation orientation = mainDisplayInfo.Orientation;

// Rotation (0, 90, 180, 270)
DisplayRotation rotation = mainDisplayInfo.Rotation;

// Width (in pixels)
double width = mainDisplayInfo.Width;

// Height (in pixels)
double height = mainDisplayInfo.Height;

// Screen density
double density = mainDisplayInfo.Density;
```

The `DeviceDisplay` class also provides the `MainDisplayInfoChanged` event that is raised when any screen metric changes:

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
        DisplayInfo displayInfo = e.DisplayInfo;
    }
}
```

## Keep Screen On

The `DeviceDisplay` class has a property named `KeepScreenOn` which when set to `true`, which prevents the device's display from turning off or locking.

```csharp
public void ToggleScreenLock() =>
    DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
```

## Platform differences

This section describes the platform-specific differences with the device display.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform differences.

# [iOS](#tab/ios)

- Accessing `DeviceDisplay` must be done on the UI thread or else an exception will be thrown. You can use the [`MainThread.BeginInvokeOnMainThread`](main-thread.md) method to run that code on the UI thread.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->

## API

- [DeviceDisplay source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/DeviceDisplay)
<!-- - [DeviceDisplay API documentation](xref:Microsoft.Maui.Essentials.DeviceDisplay)-->
