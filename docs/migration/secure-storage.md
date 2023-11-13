---
title: "Migrate from Xamarin.Essentials SecureStorage to .NET MAUI SecureStorage"
description: "Learn how to migrate your app from Xamarin.Essentials secure storage to .NET MAUI secure storage. On Android, the SecureStorage implementations differ significantly."
ms.date: 09/05/2023
---

# Migrate from Xamarin.Essentials secure storage to .NET MAUI secure storage

Xamarin.Essentials and .NET Multi-platform App UI (.NET MAUI) both have a `SecureStorage` class that helps you securely store simple key/value pairs. However, there are implementation differences between the `SecureStorage` class in Xamarin.Essentials and .NET MAUI:

| Platform | Xamarin.Essentials | .NET MAUI |
| - | ------------------ | --------- |
| Android | The Android KeyStore is used to store the cipher key used to encrypt a value before it's saved into a shared preferences object with a name of {your-app-package-id}.xamarinessentials. | Data is encrypted with the `EncryptedSharedPreferences` class, which wraps the `SharedPreferences` class, and automatically encrypts keys and values. The name used is {your-app-package-id}.microsoft.maui.essentials.preferences. |
| iOS | KeyChain is used to store values securely. The `SecRecord` used to store values has a `Service` value set to {your-app-package-id}.xamarinessentials. | KeyChain is used to store values securely. The `SecRecord` used to store values has a `Service` value set to {your-app-package-id}.microsoft.maui.essentials.preferences. |
<!-- | Windows | The `DataProtectionProvider` class is used to encrypt values securely. Encrypted values are stored in `ApplicationData.Current.LocalSettings`, inside a container with a name of {your-app-package-id}.xamarinessentials. | The `DataProtectionProvider` class is used to encrypt values securely. Encrypted values are stored in `ApplicationData.Current.LocalSettings`, inside a container with a name of {your-app-package-id}.microsoft.maui.essentials.preferences. | -->

For more information about the `SecureStorage` class in Xamarin.Essentials, see [Xamarin.Essentials: Secure storage](/xamarin/essentials/secure-storage). For more information about the `SecureStorage` class in .NET MAUI, see [Secure storage](~/platform-integration/storage/secure-storage.md).

When migrating a Xamarin.Forms app that uses the `SecureStorage` class to .NET MAUI, you must deal with these implementation differences to provide users with a smooth upgrade experience. This article describes how you can use the the `LegacySecureStorage` class and helper classes to deal with the implementation differences. The `LegacySecureStorage` class enables your .NET MAUI app on Android and iOS to read secure storage data that was created with a previous Xamarin.Forms version of your app.

## Access legacy secure storage data

The following code shows the `LegacySecureStorage` class, which provides the secure storage implementation from Xamarin.Essentials:

> [!NOTE]
> To use this code, add it to a class named `LegacySecureStorage` in your .NET MAUI app project.

```csharp
#nullable enable
#if ANDROID || IOS

namespace MigrationHelpers;

public class LegacySecureStorage
{
    internal static readonly string Alias = $"{AppInfo.PackageName}.xamarinessentials";

    public static Task<string> GetAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        string result = string.Empty;

#if ANDROID
        object locker = new object();
        string? encVal = Preferences.Get(key, null, Alias);

        if (!string.IsNullOrEmpty(encVal))
        {
            byte[] encData = Convert.FromBase64String(encVal);
            lock (locker)
            {
                AndroidKeyStore keyStore = new AndroidKeyStore(Platform.AppContext, Alias, false);
                result = keyStore.Decrypt(encData);
            }
        }
#elif IOS
        KeyChain keyChain = new KeyChain();
        result = keyChain.ValueForKey(key, Alias);
#endif
        return Task.FromResult(result);
    }

    public static bool Remove(string key)
    {
        bool result = false;

#if ANDROID
        Preferences.Remove(key, Alias);
        result = true;
#elif IOS
        KeyChain keyChain = new KeyChain();
        result = keyChain.Remove(key, Alias);
#endif
        return result;
    }

    public static void RemoveAll()
    {
#if ANDROID
        Preferences.Clear(Alias);
#elif IOS
        KeyChain keyChain = new KeyChain();
        keyChain.RemoveAll(Alias);
#endif
    }
}
#endif
```

### Android

On Android, the `LegacySecureStorage` class uses the `AndroidKeyStore` class to store the cipher key used to encrypt a value before it's saved into a shared preferences object with a name of {your-app-package-id}.xamarinessentials. The following code shows the `AndroidKeyStore` class:

> [!NOTE]
> To use this code, add it to a class named `AndroidKeyStore` in the *Platforms\Android* folder of your .NET MAUI app project.

```csharp
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Security;
using Android.Security.Keystore;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;
using System.Text;

namespace MigrationHelpers;

class AndroidKeyStore
{
    const string androidKeyStore = "AndroidKeyStore"; // this is an Android const value
    const string aesAlgorithm = "AES";
    const string cipherTransformationAsymmetric = "RSA/ECB/PKCS1Padding";
    const string cipherTransformationSymmetric = "AES/GCM/NoPadding";
    const string prefsMasterKey = "SecureStorageKey";
    const int initializationVectorLen = 12; // Android supports an IV of 12 for AES/GCM

    internal AndroidKeyStore(Context context, string keystoreAlias, bool alwaysUseAsymmetricKeyStorage)
    {
        alwaysUseAsymmetricKey = alwaysUseAsymmetricKeyStorage;
        appContext = context;
        alias = keystoreAlias;

        keyStore = KeyStore.GetInstance(androidKeyStore);
        keyStore.Load(null);
    }

    readonly Context appContext;
    readonly string alias;
    readonly bool alwaysUseAsymmetricKey;
    readonly string useSymmetricPreferenceKey = "essentials_use_symmetric";

    KeyStore keyStore;
    bool useSymmetric = false;

    ISecretKey GetKey()
    {
        // check to see if we need to get our key from past-versions or newer versions.
        // we want to use symmetric if we are >= 23 or we didn't set it previously.
        var hasApiLevel = Build.VERSION.SdkInt >= BuildVersionCodes.M;

        useSymmetric = Preferences.Get(useSymmetricPreferenceKey, hasApiLevel, alias);

        // If >= API 23 we can use the KeyStore's symmetric key
        if (useSymmetric && !alwaysUseAsymmetricKey)
            return GetSymmetricKey();

        // NOTE: KeyStore in < API 23 can only store asymmetric keys
        // specifically, only RSA/ECB/PKCS1Padding
        // So we will wrap our symmetric AES key we just generated
        // with this and save the encrypted/wrapped key out to
        // preferences for future use.
        // ECB should be fine in this case as the AES key should be
        // contained in one block.

        // Get the asymmetric key pair
        var keyPair = GetAsymmetricKeyPair();

        var existingKeyStr = Preferences.Get(prefsMasterKey, null, alias);

        if (!string.IsNullOrEmpty(existingKeyStr))
        {
            try
            {
                var wrappedKey = Convert.FromBase64String(existingKeyStr);

                var unwrappedKey = UnwrapKey(wrappedKey, keyPair.Private);
                var kp = unwrappedKey.JavaCast<ISecretKey>();

                return kp;
            }
            catch (InvalidKeyException ikEx)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to unwrap key: Invalid Key. This may be caused by system backup or upgrades. All secure storage items will now be removed. {ikEx.Message}");
            }
            catch (IllegalBlockSizeException ibsEx)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to unwrap key: Illegal Block Size. This may be caused by system backup or upgrades. All secure storage items will now be removed. {ibsEx.Message}");
            }
            catch (BadPaddingException paddingEx)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to unwrap key: Bad Padding. This may be caused by system backup or upgrades. All secure storage items will now be removed. {paddingEx.Message}");
            }
            LegacySecureStorage.RemoveAll();
        }

        var keyGenerator = KeyGenerator.GetInstance(aesAlgorithm);
        var defSymmetricKey = keyGenerator.GenerateKey();

        var newWrappedKey = WrapKey(defSymmetricKey, keyPair.Public);

        Preferences.Set(prefsMasterKey, Convert.ToBase64String(newWrappedKey), alias);

        return defSymmetricKey;
    }

    // API 23+ Only
#pragma warning disable CA1416
    ISecretKey GetSymmetricKey()
    {
        Preferences.Set(useSymmetricPreferenceKey, true, alias);

        var existingKey = keyStore.GetKey(alias, null);

        if (existingKey != null)
        {
            var existingSecretKey = existingKey.JavaCast<ISecretKey>();
            return existingSecretKey;
        }

        var keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, androidKeyStore);
        var builder = new KeyGenParameterSpec.Builder(alias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
            .SetBlockModes(KeyProperties.BlockModeGcm)
            .SetEncryptionPaddings(KeyProperties.EncryptionPaddingNone)
            .SetRandomizedEncryptionRequired(false);

        keyGenerator.Init(builder.Build());

        return keyGenerator.GenerateKey();
    }
#pragma warning restore CA1416

    KeyPair GetAsymmetricKeyPair()
    {
        // set that we generated keys on pre-m device.
        Preferences.Set(useSymmetricPreferenceKey, false, alias);

        var asymmetricAlias = $"{alias}.asymmetric";

        var privateKey = keyStore.GetKey(asymmetricAlias, null)?.JavaCast<IPrivateKey>();
        var publicKey = keyStore.GetCertificate(asymmetricAlias)?.PublicKey;

        // Return the existing key if found
        if (privateKey != null && publicKey != null)
            return new KeyPair(publicKey, privateKey);

        var originalLocale = Java.Util.Locale.Default;
        try
        {
            // Force to english for known bug in date parsing:
            // https://issuetracker.google.com/issues/37095309
            SetLocale(Java.Util.Locale.English);

            // Otherwise we create a new key
#pragma warning disable CA1416
            var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa, androidKeyStore);
#pragma warning restore CA1416

            var end = DateTime.UtcNow.AddYears(20);
            var startDate = new Java.Util.Date();
#pragma warning disable CS0618 // Type or member is obsolete
            var endDate = new Java.Util.Date(end.Year, end.Month, end.Day);
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618
            var builder = new KeyPairGeneratorSpec.Builder(Platform.AppContext)
                .SetAlias(asymmetricAlias)
                .SetSerialNumber(Java.Math.BigInteger.One)
                .SetSubject(new Javax.Security.Auth.X500.X500Principal($"CN={asymmetricAlias} CA Certificate"))
                .SetStartDate(startDate)
                .SetEndDate(endDate);

            generator.Initialize(builder.Build());
#pragma warning restore CS0618

            return generator.GenerateKeyPair();
        }
        finally
        {
            SetLocale(originalLocale);
        }
    }

    byte[] WrapKey(IKey keyToWrap, IKey withKey)
    {
        var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
        cipher.Init(CipherMode.WrapMode, withKey);
        return cipher.Wrap(keyToWrap);
    }

#pragma warning disable CA1416
    IKey UnwrapKey(byte[] wrappedData, IKey withKey)
    {
        var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
        cipher.Init(CipherMode.UnwrapMode, withKey);
        var unwrapped = cipher.Unwrap(wrappedData, KeyProperties.KeyAlgorithmAes, KeyType.SecretKey);
        return unwrapped;
    }
#pragma warning restore CA1416

    internal string Decrypt(byte[] data)
    {
        if (data.Length < initializationVectorLen)
            return null;

        var key = GetKey();

        // IV will be the first 16 bytes of the encrypted data
        var iv = new byte[initializationVectorLen];
        Buffer.BlockCopy(data, 0, iv, 0, initializationVectorLen);

        Cipher cipher;

        // Attempt to use GCMParameterSpec by default
        try
        {
            cipher = Cipher.GetInstance(cipherTransformationSymmetric);
            cipher.Init(CipherMode.DecryptMode, key, new GCMParameterSpec(128, iv));
        }
        catch (InvalidAlgorithmParameterException)
        {
            // If we encounter this error, it's likely an old bouncycastle provider version
            // is being used which does not recognize GCMParameterSpec, but should work
            // with IvParameterSpec, however we only do this as a last effort since other
            // implementations will error if you use IvParameterSpec when GCMParameterSpec
            // is recognized and expected.
            cipher = Cipher.GetInstance(cipherTransformationSymmetric);
            cipher.Init(CipherMode.DecryptMode, key, new IvParameterSpec(iv));
        }

        // Decrypt starting after the first 16 bytes from the IV
        var decryptedData = cipher.DoFinal(data, initializationVectorLen, data.Length - initializationVectorLen);

        return Encoding.UTF8.GetString(decryptedData);
    }

    internal void SetLocale(Java.Util.Locale locale)
    {
        Java.Util.Locale.Default = locale;
        var resources = appContext.Resources;
        var config = resources.Configuration;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            config.SetLocale(locale);
        else
#pragma warning disable CS0618 // Type or member is obsolete
            config.Locale = locale;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        resources.UpdateConfiguration(config, resources.DisplayMetrics);
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
```

The [Android KeyStore](https://developer.android.com/training/articles/keystore.html) is used to store the cipher key used to encrypt the value before it's saved into a [Shared Preferences](https://developer.android.com/training/data-storage/shared-preferences.html)file with a name of *{your-app-package-id}.xamarinessentials*. The key (not a cryptographic key, the *key* to the *value*) used in the shared preferences file is an *MD5 Hash* of the key passed into the `SecureStorage` APIs.

On API 23+, an **AES** key is obtained from the Android KeyStore and used with an **AES/GCM/NoPadding** cipher to encrypt the value before it's stored in the shared preferences file. On API 22 and lower, the Android KeyStore only supports storing **RSA** keys, which is used with an **RSA/ECB/PKCS1Padding** cipher to encrypt an **AES** key (randomly generated at runtime) and stored in the shared preferences file under the key _SecureStorageKey_, if one hasn't already been generated.

### iOS

On iOS, the `LegacySecureStorage` class uses the `KeyChain` class to store values securely. The `SecRecord` used to store values has a `Service` value set to {your-app-package-id}.xamarinessentials. The following code shows the `KeyChain` class:

> [!NOTE]
> To use this code, add it to a class named `KeyChain` in the *Platforms\iOS* folder of your .NET MAUI app project.

```csharp
using Foundation;
using Security;

namespace MigrationHelpers;

class KeyChain
{
    SecRecord ExistingRecordForKey(string key, string service)
    {
        return new SecRecord(SecKind.GenericPassword)
        {
            Account = key,
            Service = service
        };
    }

    internal string ValueForKey(string key, string service)
    {
        using (var record = ExistingRecordForKey(key, service))
        using (var match = SecKeyChain.QueryAsRecord(record, out var resultCode))
        {
            if (resultCode == SecStatusCode.Success)
                return NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
            else
                return null;
        }
    }

    internal bool Remove(string key, string service)
    {
        using (var record = ExistingRecordForKey(key, service))
        using (var match = SecKeyChain.QueryAsRecord(record, out var resultCode))
        {
            if (resultCode == SecStatusCode.Success)
            {
                RemoveRecord(record);
                return true;
            }
        }
        return false;
    }

    internal void RemoveAll(string service)
    {
        using (var query = new SecRecord(SecKind.GenericPassword) { Service = service })
        {
            SecKeyChain.Remove(query);
        }
    }

    bool RemoveRecord(SecRecord record)
    {
        var result = SecKeyChain.Remove(record);
        if (result != SecStatusCode.Success && result != SecStatusCode.ItemNotFound)
            throw new Exception($"Error removing record: {result}");

        return true;
    }
}
```

To use this code, you must have an *Entitlements.plist* file for your iOS app with the Keychain entitlement set:

```xml
<key>keychain-access-groups</key>
<array>
  <string>$(AppIdentifierPrefix)$(CFBundleIdentifier)</string>
</array>
```

You must also ensure that the *Entitlements.plist* file is set as the Custom Entitlements field in the Bundle Signing settings for your app. For more information, see [iOS Entitlements](~/ios/entitlements.md).

## Consume legacy secure storage data

The `LegacySecureStorage` class can be used to consume legacy secure storage data, on Android and iOS, that was created with a previous Xamarin.Forms version of your app:

```csharp
#if ANDROID || IOS
using MigrationHelpers;
...

string username = await LegacySecureStorage.GetAsync("username");
bool result = LegacySecureStorage.Remove("username");
await SecureStorage.SetAsync("username", username);
#endif
```

The example shows using the `LegacySecureStorage` class to read and remove a value from legacy secure storage, and then write the value to .NET MAUI secure storage.
