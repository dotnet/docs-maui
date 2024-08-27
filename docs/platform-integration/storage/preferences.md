---
title: "Preferences"
description: "Learn how to read and write application preferences, in .NET MAUI. The IPreferences interface can save and load application preferences in a key/value store."
ms.date: 08/27/2024
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "Preferences"]
---

# Preferences

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IPreferences` interface. This interface helps store app preferences in a key/value store.

The default implementation of the `IPreferences` interface is available through the `Preferences.Default` property. Both the `IPreferences` interface and `Preferences` class are contained in the `Microsoft.Maui.Storage` namespace.

## Storage types

Preferences are stored with a <xref:System.String> key. The value of a preference must be one of the following data types:

- <xref:System.Boolean>
- <xref:System.Double>
- <xref:System.Int32>
- <xref:System.Single>
- <xref:System.Int64>
- <xref:System.String>
- <xref:System.DateTime>

Values of `DateTime` are stored in a 64-bit binary (long integer) format using two methods defined by the `DateTime` class:

- The [`ToBinary`](xref:System.DateTime.ToBinary) method is used to encode the `DateTime` value.
- The [`FromBinary`](xref:System.DateTime.FromBinary(System.Int64)) method decodes the value.

See the documentation of these methods for adjustments that might be made to decoded values when a `DateTime` is stored that isn't a Coordinated Universal Time (UTC) value.

## Set preferences

Preferences are set by calling the `Preferences.Set` method, providing the key and value:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="prefs_set":::

## Get preferences

To retrieve a value from preferences you pass the key of the preference, followed by the default value when the key doesn't exist:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="prefs_defaults":::

Alternatively, to retrieve a value from preferences you pass the key of the preference, followed by the default value and its type when the key doesn't exist:

```csharp
long value = Preferences.Get("master_date", (long)0);
```

## Check for a key

It may be useful to check if a key exists in the preferences or not. Even though `Get` has you set a default value when the key doesn't exist, there may be cases where the key existed, but the value of the key matched the default value. So you can't rely on the default value as an indicator that the key doesn't exist. Use the `ContainsKey` method to determine if a key exists:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="prefs_containskey":::

## Remove one or all keys

Use the `Remove` method to remove a specific key from preferences:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="prefs_remove":::

To remove all keys, use the `Clear` method:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="prefs_clear":::

## Shared keys

The preferences stored by your app are only visible to your app. However, you can also create a **shared** preference that can be used by other extensions or a watch app. When you set, remove, or retrieve a preference, an optional string parameter can be supplied to specify the name of the container the preference is stored in.

The following methods take a string parameter named `sharedName`:

- `Preferences.Set`
- `Preferences.Get`
- `Preferences.Remove`
- `Preferences.Clear`

> [!IMPORTANT]
> Please read the platform implementation specifics, as shared preferences have behavior-specific implementations

## Integrate with system settings

Preferences are stored natively, which allows you to integrate your settings into the native system settings. Follow the platform documentation to integrate with the platform:

- Apple: [Implementing an iOS Settings Bundle](https://developer.apple.com/library/content/documentation/Cocoa/Conceptual/UserDefaults/Preferences/Preferences.html)
- Android: [Getting Started with Settings Screens](https://developer.android.com/guide/topics/ui/settings.html)

## Platform differences

This section describes the platform-specific differences with the preferences API.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

All data is stored into [Shared Preferences](https://developer.android.com/training/data-storage/shared-preferences.html). If no `sharedName` is specified, the default **Shared Preferences** are used. Otherwise, the name is used to get a **private Shared Preferences** with the specified name.

# [iOS/Mac Catalyst](#tab/macios)

**NSUserDefaults** <!-- TODO link (../ios/app-fundamentals/user-defaults.md) --> is used to store values on iOS devices. If no `sharedName` is specified, the `StandardUserDefaults` are used. Otherwise, the name is used to create a new `NSUserDefaults` with the specified name used for the `NSUserDefaultsType.SuiteName`.

# [Windows](#tab/windows)

`ApplicationDataContainer` is used to store the values on the device. If no `sharedName` is specified, the `LocalSettings` are used. Otherwise the name is used to create a new container inside of `LocalSettings`. <!-- (/uwp/api/windows.storage.applicationdatacontainer) -->

`LocalSettings` restricts the preference key names to 255 characters or less. Each preference value can be up to 8K bytes in size, and each composite setting can be up to 64 K bytes in size.

-----
<!-- markdownlint-enable MD025 -->

## Persistence

Uninstalling the application causes all _preferences_ to be removed, except when the app runs on Android 6.0 (API level 23) or later, while using the [Auto Backup](https://developer.android.com/guide/topics/data/autobackup) feature. This feature is on by default and preserves app data, including **Shared Preferences**, which is what the **Preferences** API uses. You can disable this by following Google's [Auto Backup documentation](https://developer.android.com/guide/topics/data/autobackup).

## Limitations

Performance may be impacted if you store large amounts of text, as the API was designed to store small amounts of text.
