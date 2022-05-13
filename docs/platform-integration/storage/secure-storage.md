---
title: "Secure storage"
description: "Learn how to use the .NET MAUI SecureStorage class, which helps securely store simple key/value pairs. This article discusses how to use the class, platform implementation specifics, and its limitations."
ms.date: 08/27/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "SecureStorage"]
#acrolinx score 95
---

# Secure storage

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `ISecureStorage` interface. This interface helps securely store simple key/value pairs. The `ISecureStorage` interface is exposed through the `SecureStorage.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `SecureStorage` and `ISecureStorage` types are available in the `Microsoft.Maui.Storage` namespace.

## Get started

To access the **SecureStorage** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

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

01. Create a new XML file named _auto_backup_rules.xml_ in the _Resources/xml_ directory with the build action of **AndroidResource**. Set the following content that includes all shared preferences except for `SecureStorage`:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <full-backup-content>
        <include domain="sharedpref" path="."/>
        <exclude domain="sharedpref" path="${applicationId}.mauiessentials.xml"/>
    </full-backup-content>
    ```

# [iOS](#tab/ios)

When developing on the **iOS simulator**, enable the **Keychain** entitlement and add a keychain access group for the application's bundle identifier.

Open the _Entitlements.plist_ in the project and find the **Keychain** entitlement and enable it. This will automatically add the application's identifier as a group.

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

The [Android KeyStore](https://developer.android.com/training/articles/keystore.html) is used to store the cipher key used to encrypt the value before it's saved into a [Shared Preferences](https://developer.android.com/training/data-storage/shared-preferences.html) with a filename of _[YOUR-APP-PACKAGE-ID].xamarinessentials_. The value-key used in the shared preferences file is an **MD5** hash of the key passed into the `SecureStorage` APIs.

- **API Level 23 and Higher**

  On newer API levels, an **AES** key is obtained from the Android KeyStore and used with an **AES/GCM/NoPadding** cipher to encrypt the value before it's stored in the shared preferences file.

- **API Level 22 and Lower**

  On older API levels, the Android KeyStore only supports storing **RSA** keys, which is used with an **RSA/ECB/PKCS1Padding** cipher to encrypt an **AES** key, which is randomly generated at runtime. The **AES** key is stored in the shared preferences file under the key _SecureStorageKey_, but only if one wasn't already generated.

**SecureStorage** uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation. If a device upgrades from API level 22 or lower, to API level 23 and higher, API Level 22 encryption is used unless the app is uninstalled or **RemoveAll** is called.

# [iOS](#tab/ios)

[KeyChain](xref:Security.SecKeyChain) is used to store values securely on iOS devices. The `SecRecord` used to store the value has a `Service` value set to _[YOUR-APP-BUNDLE-ID].xamarinessentials_.

In some cases, KeyChain data is synchronized with iCloud, and uninstalling the application may not remove the secure values from user devices.

# [Windows](#tab/windows)

[DataProtectionProvider](/uwp/api/windows.security.cryptography.dataprotection.dataprotectionprovider) is used to encrypt values securely on Windows devices.

Encrypted values are stored in `ApplicationData.Current.LocalSettings`, inside a container with a name of _[YOUR-APP-ID].xamarinessentials_.

**SecureStorage** uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation. It also uses `LocalSettings`, which has a restriction that a setting name length may be 255 characters at the most. Each setting can be up to 8K bytes in size, and each composite setting can be up to 64 K bytes in size.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## Limitations

Performance may be impacted if you store large amounts of text, as the API was designed to store small amounts of text.
