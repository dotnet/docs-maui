---
title: "Secure storage"
description: "Learn how to use the .NET MAUI ISecureStorage interface, which helps securely store simple key/value pairs. This article discusses how to use the ISecureStorage, platform implementation specifics, and its limitations."
ms.date: 12/16/2024
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "SecureStorage"]
#acrolinx score 95
---

# Secure storage

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `ISecureStorage` interface. This interface helps securely store simple key/value pairs.

The default implementation of the `ISecureStorage` interface is available through the `SecureStorage.Default` property. Both the `ISecureStorage` interface and `SecureStorage` class are contained in the `Microsoft.Maui.Storage` namespace.

## Get started

To access the **SecureStorage** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

<!-- .NET MAUI secure storage is only supported with Android 6.0 (API level 23) and later. -->
<!-- The above is true when using v1.0.0 of AndroidX.Security.Crypto. But MAUI is currently using v1.1.0-alpha03, which supports API21+. -->

[Auto Backup for Apps](https://developer.android.com/guide/topics/data/autobackup) is a feature of Android 6.0 (API level 23) and later that backs up user's app data (shared preferences, files in the app's internal storage, and other specific files). Data is restored when an app is reinstalled or installed on a new device. This can affect `SecureStorage`, which utilizes share preferences that are backed up and can't be decrypted when the restore occurs. .NET MAUI automatically handles this case by removing the key so it can be reset. Alternatively, you can disable Auto Backup.

### Enable or disable backup

You can choose to disable Auto Backup for your entire application by setting `android:allowBackup` to false in the _AndroidManifest.xml_ file. This approach is only recommended if you plan on restoring data in another way.

```xml
<manifest ... >
    ...
    <application android:allowBackup="false" ... >
        ...
    </application>
</manifest>
```

### Selective backup

Auto Backup can be configured to disable specific content from backing up. You can create a custom rule set to exclude `SecureStore` items from being backed up.

01. Set the `android:fullBackupContent` attribute in your _AndroidManifest.xml_:

    ```xml
    <application ...
        android:fullBackupContent="@xml/auto_backup_rules">
    </application>
    ```

01. Create a new XML file named _auto_backup_rules.xml_ in the _Platforms/Android/Resources/xml_ directory with the build action of **AndroidResource**. Set the following content that includes all shared preferences except for `SecureStorage`:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <full-backup-content>
        <include domain="sharedpref" path="."/>
        <exclude domain="sharedpref" path="${applicationId}.microsoft.maui.essentials.preferences.xml"/>
    </full-backup-content>
    ```

# [iOS/Mac Catalyst](#tab/macios)

When developing on the **iOS simulator**, enable the **Keychain** entitlement and add a keychain access group for the application's bundle identifier.

Create or open the _Entitlements.plist_ in the project and find the **Keychain** entitlement and enable it. This will automatically add the application's identifier as a group. For more information about editing the _Entitlements.plist_ file, see [Entitlements](../../ios/entitlements.md).

In the project properties, under **iOS Bundle Signing** set the **Custom Entitlements** to **Entitlements.plist**.

> [!TIP]
> When deploying to an iOS device, this entitlement isn't required and should be removed.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Use secure storage

The following code examples demonstrate how to use secure storage.

> [!TIP]
> It's possible that an exception is thrown when calling `GetAsync` or `SetAsync`. This can be caused by a device not supporting secure storage, encryption keys changing, or corruption of data. it's best to handle this by removing and adding the setting back if possible.

### Write a value

To save a value for a given _key_ in secure storage:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="secstorage_set":::

### Read a value

To retrieve a value from secure storage:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="secstorage_get":::

> [!TIP]
> If there isn't a value associated with the key, `GetAsync` returns `null`.

### Remove a value

To remove a specific value, remove the key:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="secstorage_remove":::

To remove all values, use the `RemoveAll` method:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="secstorage_remove_all":::

## Platform differences

This section describes the platform-specific differences with the secure storage API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

`SecureStorage` uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation, with a filename of _[YOUR-APP-PACKAGE-ID].microsoft.maui.essentials.preferences_. However, data is encrypted with the Android [`EncryptedSharedPreferences`](https://developer.android.com/reference/androidx/security/crypto/EncryptedSharedPreferences) class, from the Android Security library, which wraps the `SharedPreferences` class and automatically encrypts keys and values using a two-scheme approach:

- Keys are deterministically encrypted, so that the key can be encrypted and properly looked up.
- Values are non-deterministically encrypted using AES-256 GCM.

For more information about the Android Security library, see [Work with data more securely](https://developer.android.com/topic/security/data) on developer.android.com.

# [iOS/Mac Catalyst](#tab/macios)

[KeyChain](xref:Security.SecKeyChain) is used to store values securely on iOS devices. The `SecRecord` used to store the value has a `Service` value set to _[YOUR-APP-BUNDLE-ID].microsoft.maui.essentials.preferences_.

In some cases, KeyChain data is synchronized with iCloud, and uninstalling the application may not remove the secure values from user devices.

# [Windows](#tab/windows)

`DataProtectionProvider` is used to encrypt values securely on Windows devices. <!-- (/uwp/api/windows.security.cryptography.dataprotection.dataprotectionprovider) -->

In packaged apps, encrypted values are stored in `ApplicationData.Current.LocalSettings`, inside a container with a name of _[YOUR-APP-ID].microsoft.maui.essentials.preferences_. `SecureStorage` uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation. It also uses `LocalSettings`, which has a restriction that a setting name length may be 255 characters at the most. Each setting can be up to 8K bytes in size, and each composite setting can be up to 64 K bytes in size.

In unpackaged apps, encrypted values are stored in JSON format in `securestorage.dat` inside the app's data folder.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## Limitations

Performance may be impacted if you store large amounts of text, as the API was designed to store small amounts of text.
