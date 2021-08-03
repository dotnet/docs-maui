---
title: "Xamarin.Essentials: SMS"
description: "The Sms class in Xamarin.Essentials enables an application to open the default SMS application with a specified message to send to a recipient."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 09/24/2020
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: SMS

The **Sms** class enables an application to open the default SMS application with a specified message to send to a recipient.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **Sms** functionality the following platform specific setup is required.

# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your Android Manifest with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.VIEW" />
    <data android:scheme="smsto"/>
  </intent>
</queries>
```

# [iOS](#tab/ios)

No additional setup required.

# [UWP](#tab/uwp)

No platform differences.

-----

## Using Sms

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The SMS functionality works by calling the `ComposeAsync` method an `SmsMessage` that contains the message's recipient and the body of the message, both of which are optional.

```csharp
public class SmsTest
{
    public async Task SendSms(string messageText, string recipient)
    {
        try
        {
            var message = new SmsMessage(messageText, new []{ recipient });
            await Sms.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException ex)
        {
            // Sms is not supported on this device.
        }
        catch (Exception ex)
        {
            // Other error has occurred.
        }
    }
}
```

Additionally, you can pass in multiple receipients to a `SmsMessage`:

```csharp
public class SmsTest
{
    public async Task SendSms(string messageText, string[] recipients)
    {
        try
        {
            var message = new SmsMessage(messageText, recipients);
            await Sms.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException ex)
        {
            // Sms is not supported on this device.
        }
        catch (Exception ex)
        {
            // Other error has occurred.
        }
    }
}
```

## API

- [Sms source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Sms)
- [Sms API documentation](xref:Xamarin.Essentials.Sms)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/SMS-XamarinEssentials-API-of-the-Week/player]

[!INCLUDE [xamarin-show-essentials](includes/xamarin-show-essentials.md)]
