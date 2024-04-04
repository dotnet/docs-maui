---
title: "Device Display Information"
description: "Learn how to use the .NET MAUI IDeviceDisplay interface in the Microsoft.Maui.Devices namespace, which provides screen metrics for the device on which the app is running."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Device display information

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.IDeviceDisplay> interface to read information about the device's screen metrics. This interface can be used to request the screen stays awake while the app is running.

The default implementation of the `IDeviceDisplay` interface is available through the <xref:Microsoft.Maui.Devices.DeviceDisplay.Current?displayProperty=nameWithType> property. Both the `IDeviceDisplay` interface and `DeviceDisplay` class are contained in the `Microsoft.Maui.Devices` namespace.

## Main display info

The <xref:Microsoft.Maui.Devices.IDeviceDisplay.MainDisplayInfo> property returns information about the screen and orientation. The following code example uses the <xref:Microsoft.Maui.Controls.VisualElement.Loaded> event of a page to read information about the current screen:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="main_display":::

The <xref:Microsoft.Maui.Devices.IDeviceDisplay> interface also provides the <xref:Microsoft.Maui.Devices.IDeviceDisplay.MainDisplayInfoChanged> event that is raised when any screen metric changes, such as when the device orientation changes from <xref:Microsoft.Maui.Devices.DisplayOrientation.Landscape?displayProperty=nameWithType> to <xref:Microsoft.Maui.Devices.DisplayOrientation.Portrait?displayProperty=nameWithType>.

## Keep the screen on

You can also prevent the device from locking or the screen turning off by setting the <xref:Microsoft.Maui.Devices.IDeviceDisplay.KeepScreenOn> property to `true`. The following code example toggles the screen lock whenever the switch control is pressed:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="always_on":::

## Platform differences

This section describes the platform-specific differences with the device display.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform differences.

# [iOS/Mac Catalyst](#tab/macios)

- Accessing <xref:Microsoft.Maui.Devices.DeviceDisplay.Current?displayProperty=nameWithType> must be done on the UI thread or else an exception will be thrown. You can use the [`MainThread.BeginInvokeOnMainThread`](../appmodel/main-thread.md) method to run that code on the UI thread.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->
