---
title: "Device Display Information"
description: "Learn how to use the .NET MAUI DeviceDisplay class in the Microsoft.Maui.Devices namespace, which provides screen metrics for the device on which the app is running."
ms.date: 05/03/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Device display information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `DeviceDisplay` class to read information about the device's screen metrics. This class can be used to request the screen stays awake while the app is running.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `DeviceDisplay` class is available in the `Microsoft.Maui.Devices` namespace.

## Main display info

Information about your device's screen is accessed by the default implementation of the `IDeviceDisplay` interface, which is available by accessing the `Microsoft.Maui.Devices.DeviceDisplay.Current` property.

The `IDeviceDisplay.MainDisplayInfo` property returns information about the screen and orientation. The following code example uses the `Loaded` event of a page to read information about the current screen:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="main_display":::

The `IDeviceDisplay` interface also provides the `MainDisplayInfoChanged` event that is raised when any screen metric changes, such as when the device orientation changes from `DisplayOrientation.Landscape` to `DisplayOrientation.Portrait`.

## Keep the screen on

You can also prevent the device from locking or the screen turning off by setting the `IDeviceDisplay.KeepScreenOn` property to `true`. The following code example toggles the screen lock whenever the switch control is pressed:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="always_on":::

## Platform differences

This section describes the platform-specific differences with the device display.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform differences.

# [iOS](#tab/ios)

- Accessing `DeviceDisplay` must be done on the UI thread or else an exception will be thrown. You can use the [`MainThread.BeginInvokeOnMainThread`](../appmodel/main-thread.md) method to run that code on the UI thread.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->

## API

- [DeviceDisplay source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/DeviceDisplay)
<!-- - [DeviceDisplay API documentation](xref:Microsoft.Maui.Essentials.DeviceDisplay)-->
