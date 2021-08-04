---
title: "Xamarin.Essentials: Device Information"
description: "This document describes the DeviceInfo class in Xamarin.Essentials, which provides information about the device the application is running on."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 11/04/2018
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Device Information

The **DeviceInfo** class provides information about the device the application is running on.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using DeviceInfo

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

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

[`DeviceInfo.Platform`](xref:Xamarin.Essentials.DeviceInfo.Platform) correlates to a constant string that maps to the operating system. The values can be checked with the `DevicePlatform` struct:

- **DevicePlatform.iOS** – iOS
- **DevicePlatform.Android** – Android
- **DevicePlatform.UWP** – UWP
- **DevicePlatform.Unknown** – Unknown

## Idioms

[`DeviceInfo.Idiom`](xref:Xamarin.Essentials.DeviceInfo.Idiom) correlates a constant string that maps to the type of device the application is running on. The values can be checked with the `DeviceIdiom` struct:

- **DeviceIdiom.Phone** – Phone
- **DeviceIdiom.Tablet** – Tablet
- **DeviceIdiom.Desktop** – Desktop
- **DeviceIdiom.TV** – TV
- **DeviceIdiom.Watch** – Watch
- **DeviceIdiom.Unknown** – Unknown

## Device Type

`DeviceInfo.DeviceType` correlates an enumeration to determine if the application is running on a physical or virtual device. A virtual device is a simulator or emulator.

## Platform Implementation Specifics

# [iOS](#tab/ios)

iOS does not expose an API for developers to get the model of the specific iOS device. Instead a hardware identifier is returned such as _iPhone10,6_ which refers to the iPhone X. A mapping of these identifiers are not provided by Apple, but can be found on these (non-official sources) [The iPhone Wiki](https://www.theiphonewiki.com/wiki/Models) and [Get iOS Model](https://github.com/dannycabrera/Get-iOS-Model).

--------------

## API

- [DeviceInfo source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/DeviceInfo)
- [DeviceInfo API documentation](xref:Xamarin.Essentials.DeviceInfo)
