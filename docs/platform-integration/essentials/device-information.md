---
title: "Device information"
description: "Learn how to use the .NET MAUI DeviceInfo class in the Microsoft.Maui.Essentials namespace, which provides information about the device the app is running on."
ms.date: 08/17/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Device information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `DeviceInfo` class to read information about the device the app is running on.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Read device info

The following information is exposed through the API:

```csharp
// Device Model (SMG-950U, iPhone10,6)
string device = DeviceInfo.Model;

// Manufacturer (Samsung)
string manufacturer = DeviceInfo.Manufacturer;

// Device Name (My iPhone)
string deviceName = DeviceInfo.Name;

// Operating System Version Number (7.0)
string version = DeviceInfo.VersionString;

// Platform (Android)
DevicePlatform platform = DeviceInfo.Platform;

// Idiom (Phone)
DeviceIdiom idiom = DeviceInfo.Idiom;

// Device Type (Physical)
DeviceType deviceType = DeviceInfo.DeviceType;
```

## Get the device platform

The `DeviceInfo.Platform` property represents the operating system the app is running on. The `DevicePlatform` type provides a property for each operating system:

- `DevicePlatform.iOS`
- `DevicePlatform.Android`
- `DevicePlatform.UWP` (Windows)
- `DevicePlatform.Unknown`
- `DevicePlatform.Tizen`

The following example demonstrates comparing the `DeviceInfo.Platform` value to a `DevicePlatform` property:

```csharp
public void WritePlatform()
{
    if (DeviceInfo.Platform == DevicePlatform.Android)
        Console.WriteLine("The current OS is Android");
    else if (DeviceInfo.Platform == DevicePlatform.iOS)
        Console.WriteLine("The current OS is iOS");
    else if (DeviceInfo.Platform == DevicePlatform.UWP)
        Console.WriteLine("The current OS is Windows");
    else if (DeviceInfo.Platform == DevicePlatform.Tizen)
        Console.WriteLine("The current OS is Tizen");
}
```

## Get the device type

The `DeviceInfo.Idiom` property represents the type of device the app is running on, such as a desktop computer or a tablet. The `DeviceIdiom` type provides a property for each type of device:

- `DeviceIdiom.Phone`
- `DeviceIdiom.Tablet`
- `DeviceIdiom.Desktop`
- `DeviceIdiom.TV`
- `DeviceIdiom.Watch`
- `DeviceIdiom.Unknown`

The following example demonstrates comparing the `DeviceInfo.Idiom` value to a `DeviceIdiom` property:

```csharp
public void WriteIdiom()
{
    if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
        Console.WriteLine("The current device is a desktop");
    else if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        Console.WriteLine("The current device is a phone");
    else if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
        Console.WriteLine("The current device is a Tablet");
}
```

## Device type

`DeviceInfo.DeviceType` correlates an enumeration to determine if the application is running on a physical or virtual device. A virtual device is a simulator or emulator.

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
