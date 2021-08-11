---
title: "Secure Storage"
description: "Describes the SecureStorage class in Microsoft.Maui.Essentials, which helps securely store simple key/value pairs. It discusses how to use the class, platform implementation specifics, and limitations."
ms.date: 04/02/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Secure Storage

The `SecureStorage` class helps securely store simple key/value pairs.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **SecureStorage** functionality, the following platform-specific setup is required:

# [Android](#tab/android)

> [!TIP]
> [Auto Backup for Apps](https://developer.android.com/guide/topics/data/autobackup) is a feature of Android 6.0 (API level 23) and later that backs up user's app data (shared preferences, files in the app's internal storage, and other specific files). Data is restored when an app is re-installed or installed on a new device. This can impact `SecureStorage` which utilizes share preferences that are backed up and can not be decrypted when the restore occurs. Xamarin.Essentials automatically handles this case by removing the key so it can be reset, but you can take an additional step by disabling Auto Backup.

### Enable or disable backup
You can choose to disable Auto Backup for your entire application by setting the `android:allowBackup` setting to false in the `AndroidManifest.xml` file. This approach is only recommended if you plan on restoring data in another way.

```xml
<manifest ... >
    ...
    <application android:allowBackup="false" ... >
        ...
    </application>
</manifest>
```

### Selective Backup
Auto Backup can be configured to disable specific content from backing up. You can create a custom rule set to exclude `SecureStore` items from being backed up.

1. Set the `android:fullBackupContent` attribute in your **AndroidManifest.xml**:

    ```xml
    <application ...
        android:fullBackupContent="@xml/auto_backup_rules">
    </application>
    ```

2. Create a new XML file named **auto_backup_rules.xml** in the **Resources/xml** directory with the build action of **AndroidResource**. Then set the following content that includes all shared preferences except for `SecureStorage`:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <full-backup-content>
        <include domain="sharedpref" path="."/>
        <exclude domain="sharedpref" path="${applicationId}.xamarinessentials.xml"/>
    </full-backup-content>
    ```

# [iOS](#tab/ios)

When developing on the **iOS simulator**, enable the **Keychain** entitlement and add a keychain access group for the application's bundle identifier.

Open the **Entitlements.plist** in the iOS project and find the **Keychain** entitlement and enable it. This will automatically add the application's identifier as a group.

In the project properties, under **iOS Bundle Signing** set the **Custom Entitlements** to **Entitlements.plist**.

> [!TIP]
> When deploying to an iOS device this entitlement is not required and should be removed.

# [Windows](#tab/windows)

No additional setup required.

-----

## Using Secure Storage

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To save a value for a given _key_ in secure storage:

```csharp
try
{
  await SecureStorage.SetAsync("oauth_token", "secret-oauth-token-value");
}
catch (Exception ex)
{
  // Possible that device doesn't support secure storage on device.
}
```

To retrieve a value from secure storage:

```csharp
try
{
  var oauthToken = await SecureStorage.GetAsync("oauth_token");
}
catch (Exception ex)
{
  // Possible that device doesn't support secure storage on device.
}
```

> [!NOTE]
> If there is no value associated with the requested key, `GetAsync` will
> return `null`.

To remove a specific key, call:

```csharp
SecureStorage.Remove("oauth_token");
```

To remove all keys, call:

```csharp
SecureStorage.RemoveAll();
```

> [!TIP]
> It is possible that an exception is thrown when calling `GetAsync` or `SetAsync`. This can be caused by a device not supporting secure storage, encryption keys changing, or corruption of data. It is best to handle this by removing and adding the setting back if possible.

## Platform implementation specifics

# [Android](#tab/android)

The [Android KeyStore](https://developer.android.com/training/articles/keystore.html) is used to store the cipher key used to encrypt the value before it is saved into a [Shared Preferences](https://developer.android.com/training/data-storage/shared-preferences.html) with a filename of **[YOUR-APP-PACKAGE-ID].xamarinessentials**.  The key (not a cryptographic key, the _key_ to the _value_) used in the shared preferences file is a _MD5 Hash_ of the key passed into the `SecureStorage` APIs.

**API Level 23 and Higher**

On newer API levels, an **AES** key is obtained from the Android KeyStore and used with an **AES/GCM/NoPadding** cipher to encrypt the value before it is stored in the shared preferences file.

**API Level 22 and Lower**

On older API levels, the Android KeyStore only supports storing **RSA** keys, which is used with an **RSA/ECB/PKCS1Padding** cipher to encrypt an **AES** key (randomly generated at runtime) and stored in the shared preferences file under the key _SecureStorageKey_, if one has not already been generated.

**SecureStorage** uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation. If a device upgrades from API level 22 or lower to API level 23 and higher, this type of encryption will continue to be used unless the app is uninstalled or **RemoveAll** is called.

# [iOS](#tab/ios)

[KeyChain](xref:Security.SecKeyChain) is used to store values securely on iOS devices.  The `SecRecord` used to store the value has a `Service` value set to **[YOUR-APP-BUNDLE-ID].xamarinessentials**.

In some cases KeyChain data is synchronized with iCloud, and uninstalling the application may not remove the secure values from iCloud and other devices of the user.

# [Windows](#tab/windows)

[DataProtectionProvider](/uwp/api/windows.security.cryptography.dataprotection.dataprotectionprovider) is used to encrypt values securely on UWP devices.

Encrypted values are stored in `ApplicationData.Current.LocalSettings`, inside a container with a name of **[YOUR-APP-ID].xamarinessentials**.

**SecureStorage** uses the [Preferences](preferences.md) API and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation. It also uses `LocalSettings` which has a restriction that the name of each setting can be 255 characters in length at most. Each setting can be up to 8K bytes in size and each composite setting can be up to 64K bytes in size.

-----

## Limitations

This API is intended to store small amounts of text.  Performance may be slow if you try to use it to store large amounts of text.

## API

- [SecureStorage source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/SecureStorage)
<!-- - [SecureStorage API documentation](xref:Microsoft.Maui.Essentials.SecureStorage)-->
