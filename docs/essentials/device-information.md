---
title: "Device Information"
description: "Describes the DeviceInfo class in Microsoft.Maui.Essentials, which provides information about the device the application is running on."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Device Information

The `DeviceInfo` class provides information about the device the application is running on.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using DeviceInfo

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The following information is exposed through the API:

```csharp
// Device Model (SMG-950U, iPhone10,6)
var device = DeviceInfo.Model;

// Manufacturer (Samsung)
var manufacturer = DeviceInfo.Manufacturer;

// Device Name (Motz's iPhone)
var deviceName = DeviceInfo.Name;

// Operating System Version Number (7.0)
var version = DeviceInfo.VersionString;

// Platform (Android)
var platform = DeviceInfo.Platform;

// Idiom (Phone)
var idiom = DeviceInfo.Idiom;

// Device Type (Physical)
var deviceType = DeviceInfo.DeviceType;
```

## Platforms

[`DeviceInfo.Platform`](xref:Microsoft.Maui.Essentials.DeviceInfo.Platform) correlates to a constant string that maps to the operating system. The values can be checked with the `DevicePlatform` struct:

- **DevicePlatform.iOS** – iOS
- **DevicePlatform.Android** – Android
- **DevicePlatform.UWP** – UWP
- **DevicePlatform.Unknown** – Unknown

## Idioms

[`DeviceInfo.Idiom`](xref:Microsoft.Maui.Essentials.DeviceInfo.Idiom) correlates a constant string that maps to the type of device the application is running on. The values can be checked with the `DeviceIdiom` struct:

- **DeviceIdiom.Phone** – Phone
- **DeviceIdiom.Tablet** – Tablet
- **DeviceIdiom.Desktop** – Desktop
- **DeviceIdiom.TV** – TV
- **DeviceIdiom.Watch** – Watch
- **DeviceIdiom.Unknown** – Unknown

## Device Type

`DeviceInfo.DeviceType` correlates an enumeration to determine if the application is running on a physical or virtual device. A virtual device is a simulator or emulator.

## Platform implementation specifics

# [iOS](#tab/ios)

iOS does not expose an API for developers to get the model of the specific iOS device. Instead a hardware identifier is returned such as _iPhone10,6_ which refers to the iPhone X. A mapping of these identifiers are not provided by Apple, but can be found on these (non-official sources) [The iPhone Wiki](https://www.theiphonewiki.com/wiki/Models) and [Get iOS Model](https://github.com/dannycabrera/Get-iOS-Model).

--------------

## API

- [DeviceInfo source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/DeviceInfo)
<!-- - [DeviceInfo API documentation](xref:Microsoft.Maui.Essentials.DeviceInfo)-->
