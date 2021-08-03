---
title: "Xamarin.Essentials: Email"
description: "The Email class in Xamarin.Essentials enables an application to open the default email application with a specified information including subject, body, and recipients (TO, CC, BCC)."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 09/24/2020
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Email

The **Email** class enables an application to open the default email application with a specified information including subject, body, and recipients (TO, CC, BCC).

To access the **Email** functionality the following platform specific setup is required.

# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your Android Manifest with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.SENDTO" />
    <data android:scheme="mailto" />
  </intent>
</queries>
```

# [iOS](#tab/ios)

In iOS 9 and greater, Apple enforces what schemes an application can query for. To query if email is a valid target the `mailto` scheme must be specified in the  LSApplicationQueriesSchemes in your Info.plist file.

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
  <string>mailto</string>
</array>
```

# [UWP](#tab/uwp)

No platform differences.

-----

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

> [!TIP]
> To use the Email API on iOS you must run it on a physical device, else an exception will be thrown.

## Using Email

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Email functionality works by calling the `ComposeAsync` method an `EmailMessage` that contains information about the email:

```csharp
public class EmailTest
{
    public async Task SendEmail(string subject, string body, List<string> recipients)
    {
        try
        {
            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = recipients,
                //Cc = ccRecipients,
                //Bcc = bccRecipients
            };
            await Email.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException fbsEx)
        {
            // Email is not supported on this device
        }
        catch (Exception ex)
        {
            // Some other exception occurred
        }
    }
}
```

## File Attachments

This feature enables an app to email files in email clients on the device. Xamarin.Essentials will automatically detect the file type (MIME) and request the file to be added as an attachment. Every email client is different and may only support specific file extensions, or none at all.

Here is a sample of writing text to disk and adding it as an email attachment:

```csharp
var message = new EmailMessage
{
    Subject = "Hello",
    Body = "World",
};

var fn = "Attachment.txt";
var file = Path.Combine(FileSystem.CacheDirectory, fn);
File.WriteAllText(file, "Hello World");

message.Attachments.Add(new EmailAttachment(file));

await Email.ComposeAsync(message);
```

## Platform Differences

# [Android](#tab/android)

Not all email clients for Android support `Html`, since there is no way to detect this we recommend using `PlainText` when sending emails.

# [iOS](#tab/ios)

No platform differences.

# [UWP](#tab/uwp)

Only supports `PlainText` as the `BodyFormat` attempting to send `Html` will throw a `FeatureNotSupportedException`.

Not all email clients support sending attachments. See [documentation](/windows/uwp/contacts-and-calendar/sending-email) for more information.

-----

## API

- [Email source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Email)
- [Email API documentation](xref:Xamarin.Essentials.Email)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Email-XamarinEssentials-API-of-the-Week/player]

[!INCLUDE [xamarin-show-essentials](includes/xamarin-show-essentials.md)]
