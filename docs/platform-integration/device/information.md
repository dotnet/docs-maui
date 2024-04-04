---
title: "Device information"
description: "Learn how to use the .NET MAUI IDeviceInfo interface in the Microsoft.Maui.Devices namespace, which provides information about the device the app is running on."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Device information

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.IDeviceInfo> interface to read information about the device the app is running on.

The default implementation of the `IDeviceInfo` interface is available through the <xref:Microsoft.Maui.Devices.DeviceInfo.Current?displayProperty=nameWithType> property. Both the `IDeviceInfo` interface and `DeviceInfo` class are contained in the `Microsoft.Maui.Devices` namespace.

## Read device info

The `IDeviceInfo` interface provides many properties that describe the device, such as the manufacturer and idiom. The following example demonstrates reading the device info properties:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="read_info":::

## Get the device platform

The `IDeviceInfo.Platform` property represents the operating system the app is running on. The <xref:Microsoft.Maui.Devices.DevicePlatform> type provides a property for each operating system:

- <xref:Microsoft.Maui.Devices.DevicePlatform.Android?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.iOS?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.macOS?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.MacCatalyst?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.tvOS?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.Tizen?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.WinUI?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.watchOS?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DevicePlatform.Unknown?displayProperty=nameWithType>

The following example demonstrates checking if the <xref:Microsoft.Maui.Devices.IDeviceInfo.Platform?displayProperty=nameWithType> property matches the `Android` operating system:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="check_for_android":::

## Get the device type

The <xref:Microsoft.Maui.Devices.IDeviceInfo.Idiom?displayProperty=nameWithType> property represents the type of device the app is running on, such as a desktop computer or a tablet. The <xref:Microsoft.Maui.Devices.DeviceIdiom> type provides a property for each type of device:

- <xref:Microsoft.Maui.Devices.DeviceIdiom.Phone?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DeviceIdiom.Tablet?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DeviceIdiom.Desktop?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DeviceIdiom.TV?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DeviceIdiom.Watch?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Devices.DeviceIdiom.Unknown?displayProperty=nameWithType>

The following example demonstrates comparing the `IDeviceInfo.Idiom` value to a `DeviceIdiom` property:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="idiom":::

## Device type

<xref:Microsoft.Maui.Devices.IDeviceInfo.DeviceType?displayProperty=nameWithType> property an enumeration to determine if the application is running on a physical or virtual device. A virtual device is a simulator or emulator.

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="device_type":::

## Platform differences

This section describes the platform-specific differences with the device information.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform differences.

# [iOS/Mac Catalyst](#tab/macios)

To access the user-assigned device name through the <xref:Microsoft.Maui.Devices.IDeviceInfo.Name?displayProperty=nameWithType> property in iOS 16 and later, rather than the generic device name, your app must meet certain criteria and be assigned the `com.apple.developer.device-information.user-assigned-device-name` entitlement. For more information, see [`com.apple.developer.device-information.user-assigned-device-name`](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_device-information_user-assigned-device-name?language=objc) on developer.apple.com.

iOS doesn't expose an API for developers to get the model of the specific iOS device. Instead, a hardware identifier is returned, like _iPhone10,6_, which refers to the iPhone X. A mapping of these identifiers isn't provided by Apple, but can be found on the internet such as at [The iPhone Wiki](https://www.theiphonewiki.com/wiki/Models) and [Get iOS Model](https://github.com/dannycabrera/Get-iOS-Model) websites.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->
