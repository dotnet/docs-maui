---
title: "Device information"
description: "Learn how to use the .NET MAUI DeviceInfo class in the Microsoft.Maui.Devices namespace, which provides information about the device the app is running on."
ms.date: 05/03/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Device information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `DeviceInfo` class to read information about the device the app is running on.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `DeviceInfo` class is available in the `Microsoft.Maui.Devices` namespace.

## Read device info

Information about your device's is accessed by the default implementation of the `IDeviceInfo` interface, which is available by accessing the `Microsoft.Maui.Devices.DeviceInfo.Current` property. The `IDeviceInfo` interface provides many properties that describe the device, such as the manufacturer and idiom. The following example demonstrates reading the device info properties:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="read_info":::

## Get the device platform

The `IDeviceInfo.Platform` property represents the operating system the app is running on. The `DevicePlatform` type provides a property for each operating system:

- `DevicePlatform.iOS`
- `DevicePlatform.Android`
- `DevicePlatform.macOS`
- `DevicePlatform.UWP` (Windows)
- `DevicePlatform.tvOS`
- `DevicePlatform.watchOS`
- `DevicePlatform.Tizen`
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

# [iOS](#tab/ios)

iOS doesn't expose an API for developers to get the model of the specific iOS device. Instead, a hardware identifier is returned, like _iPhone10,6_, which refers to the iPhone X. A mapping of these identifiers isn't provided by Apple, but can be found on the internet such as at [The iPhone Wiki](https://www.theiphonewiki.com/wiki/Models) and [Get iOS Model](https://github.com/dannycabrera/Get-iOS-Model) websites.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD025 -->

## API

- [DeviceInfo source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/DeviceInfo)
<!-- - [DeviceInfo API documentation](xref:Microsoft.Maui.Essentials.DeviceInfo)-->
