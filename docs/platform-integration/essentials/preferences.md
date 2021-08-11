---
title: "Preferences"
description: "Describes the Preferences class in Microsoft.Maui.Essentials, which saves application preferences in a key/value store. It discusses how to use the class and the types of data that can be stored."
ms.date: 01/15/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Preferences

The `Preferences` class helps to store application preferences in a key/value store.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Preferences

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To save a value for a given _key_ in preferences:

```csharp
Preferences.Set("my_key", "my_value");
```

To retrieve a value from preferences or a default if not set:

```csharp
var myValue = Preferences.Get("my_key", "default_value");
```

To check if a given _key_ exists in preferences:

```csharp
bool hasKey = Preferences.ContainsKey("my_key");
```

To remove the _key_ from preferences:

```csharp
Preferences.Remove("my_key");
```

To remove all preferences:

```csharp
Preferences.Clear();
```

> [!TIP]
> The above methods take in an optional `string` parameter called `sharedName`. This parameter is used to create additional containers for preferences which are helpful in some use cases. One use case is when your application needs to share preferences across extensions or to a watch application. Please read the platform implementation specifics below.

## Supported Data Types

The following data types are supported in **Preferences**:

- **bool**
- **double**
- **int**
- **float**
- **long**
- **string**
- **DateTime**

## Integrate with System Settings

Preferences are stored natively, which allows you to integrate your settings into the native system settings. Follow the platform documentation and samples to integrate with the platform:

* Apple: [Implementing an iOS Settings Bundle](https://developer.apple.com/library/content/documentation/Cocoa/Conceptual/UserDefaults/Preferences/Preferences.html)
* [iOS Applicaton Preferences Sample](/samples/xamarin/ios-samples/appprefs/)
* [watchOS Settings](https://developer.xamarin.com/guides/ios/watch/working-with/settings/)
* Android: [Getting Started with Settings Screens](https://developer.android.com/guide/topics/ui/settings.html)

## Implementation Details

Values of `DateTime` are stored in a 64-bit binary (long integer) format using two methods defined by the `DateTime` class: The [`ToBinary`](xref:System.DateTime.ToBinary) method is used to encode the `DateTime` value, and the [`FromBinary`](xref:System.DateTime.FromBinary(System.Int64)) method decodes the value. See the documentation of these methods for adjustments that might be made to decoded values when a `DateTime` is stored that is not a Coordinated Universal Time (UTC) value.

## Platform implementation specifics

# [Android](#tab/android)

All data is stored into [Shared Preferences](https://developer.android.com/training/data-storage/shared-preferences.html). If no `sharedName` is specified the default shared preferences are used, otherwise the name is used to get a **private** shared preferences with the specified name.

# [iOS](#tab/ios)

[NSUserDefaults](../ios/app-fundamentals/user-defaults.md) is used to store values on iOS devices. If no `sharedName` is specified the `StandardUserDefaults` are used, else the name is used to create a new `NSUserDefaults` with the specified name used for the `NSUserDefaultsType.SuiteName`.

# [Windows](#tab/windows)

[ApplicationDataContainer](/uwp/api/windows.storage.applicationdatacontainer) is used to store the values on the device. If no `sharedName` is specified the `LocalSettings` are used, otherwise the name is used to create a new container inside of `LocalSettings`.

`LocalSettings` also has the following restriction that the name of each setting can be 255 characters in length at most. Each setting can be up to 8K bytes in size and each composite setting can be up to 64K bytes in size.

--------------

## Persistence

Uninstalling the application will cause all _Preferences_ to be removed, with the exception being apps that target and run on Android 6.0 (API level 23) or later that use [__Auto Backup__](https://developer.android.com/guide/topics/data/autobackup). This feature is on by default and preserves app data including __Shared Preferences__, which is what the **Preferences** API utilizes. You can disable this by following Google's [documentation](https://developer.android.com/guide/topics/data/autobackup).

## Limitations

When storing a string, this API is intended to store small amounts of text. Performance may be subpar if you try to use it to store large amounts of text.

## API

- [Preferences source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Preferences)
<!-- - [Preferences API documentation](xref:Microsoft.Maui.Essentials.Preferences)-->
