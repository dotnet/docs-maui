---
title: "Device information"
description: "Learn how to use the .NET MAUI IDeviceInfo interface in the Microsoft.Maui.Devices namespace, which provides information about the device the app is running on."
ms.date: 12/20/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Device information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IDeviceInfo` interface to read information about the device the app is running on.

The default implementation of the `IDeviceInfo` interface is available through the `DeviceInfo.Current` property. Both the `IDeviceInfo` interface and `DeviceInfo` class are contained in the `Microsoft.Maui.Devices` namespace.

## Read device info

The `IDeviceInfo` interface provides many properties that describe the device, such as the manufacturer and idiom. The following example demonstrates reading the device info properties:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="read_info":::

## Get the device platform

The `IDeviceInfo.Platform` property represents the operating system the app is running on. The `DevicePlatform` type provides a property for each operating system:

- `DevicePlatform.Android`
- `DevicePlatform.iOS`
- `DevicePlatform.macOS`
- `DevicePlatform.MacCatalyst`
- `DevicePlatform.tvOS`
- `DevicePlatform.Tizen`
- `DevicePlatform.WinUI`
- `DevicePlatform.watchOS`
- `DevicePlatform.Unknown`

The following example demonstrates checking if the `DeviceInfo.Platform` property matches the `Android` operating system:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="check_for_android":::

## Get the device type

The `DeviceInfo.Idiom` property represents the type of device the app is running on, such as a desktop computer or a tablet. The `DeviceIdiom` type provides a property for each type of device:

- `DeviceIdiom.Phone`
- `DeviceIdiom.Tablet`
- `DeviceIdiom.Desktop`
- `DeviceIdiom.TV`
- `DeviceIdiom.Watch`
- `DeviceIdiom.Unknown`

The following example demonstrates comparing the `DeviceInfo.Idiom` value to a `DeviceIdiom` property:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="idiom":::

## Device type

`IDeviceInfo.DeviceType` property an enumeration to determine if the application is running on a physical or virtual device. A virtual device is a simulator or emulator.

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="device_type":::

## Platform differences

This section describes the platform-specific differences with the device information.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No platform differences.

# [iOS\macOS](#tab/ios)

To access the user-assigned device name through the `DeviceInfo.Name` property in iOS 16 and later, rather than the generic device name, your app must meet certain criteria and be assigned the `com.apple.developer.device-information.user-assigned-device-name` entitlement. For more information, see [`com.apple.developer.device-information.user-assigned-device-name`](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_device-information_user-assigned-device-name?language=objc) on developer.apple.com.

iOS doesn't expose an API for developers to get the model of the specific iOS device. Instead, a hardware identifier is returned, like _iPhone10,6_, which refers to the iPhone X. A mapping of these identifiers isn't provided by Apple, but can be found on the internet such as at [The iPhone Wiki](https://www.theiphonewiki.com/wiki/Models) and [Get iOS Model](https://github.com/dannycabrera/Get-iOS-Model) websites.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->
