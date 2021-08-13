---
title: "SMS"
description: "Learn how to use the .MET MAUI Sms class in Microsoft.Maui.Essentials to open the default SMS application. The text message can be preloaded with a message and recipient."
ms.date: 08/12/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# SMS

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Sms` class to open the default SMS app and preload it with a message and recipient.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

### Platform specific setup

To access the SMS functionality the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's _Target Android_ version is set to **Android 11 (R API 30)**, you must update your _Android Manifest_ with queries that are used with the [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following inside of the **manifest** node:

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

# [Windows](#tab/windows)

No additional setup required.

-----
<!-- markdownlint-enable MD025 -->

## Create a message

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The SMS functionality works by creating a new `SmsMessage` object, and calling the `ComposeAsync` method. You can optionally include a message and zero or more recipients.

```csharp
public partial class SmsTest
{
    public async Task SendSms(string messageText, string recipient)
    {
        try
        {
            var message = new SmsMessage(messageText, new[] { recipient });
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
public partial class SmsTest
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

- [Sms source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Sms)
<!-- - [Sms API documentation](xref:Microsoft.Maui.Essentials.Sms)-->
