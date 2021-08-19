---
title: "Email"
description: "Learn how to use the .NET MAUIEmail class in the Microsoft.Maui.Essentials namespace to open the default email application. The subject, body, and recipients of email can be set."
ms.date: 08/12/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Email

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Email` class to open the default email app. When the email app is loaded, it can be set to create a new email with the specified recipients, subject, and body.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

> [!WARNING]
> To use the email API on iOS, you must run it on a physical device. Otherwise, an exception is thrown.

### Platform specific setup

To access the email functionality the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's _Target Android_ version is set to **Android 11 (R API 30)**, you must update your _Android Manifest_ with queries that are used with the [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.SENDTO" />
    <data android:scheme="mailto" />
  </intent>
</queries>
```

# [iOS](#tab/ios)

In iOS 9 and greater, Apple enforces which schemes an application can query for. To query if email is a valid target, the `mailto` scheme must be specified for the `LSApplicationQueriesSchemes` key, in your _Info.plist_ file. With this scheme, your app is given permission to open the system email app.

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
  <string>mailto</string>
</array>
```

# [Windows](#tab/windows)

No additional setup required.

-----
<!-- markdownlint-enable MD025 -->

## Using Email

The Email functionality works by providing the email information as an argument to the `ComposeAsync` method. In this example, the `EmailMessage` type is used to represent the email information:

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

## File attachments

When creating the email provided to the email client, you can add file attachments. The API will automatically detect the file type (MIME), so you don't need to specify it. Some mail clients may restrict the types of files you send, or possibly prevent attachments altogether.

Use the `EmailMessage.Attachments` collection to manage the files attached to an email.

The following example demonstrates adding arbitrary text to a file, and then adding it to the email.

```csharp
var message = new EmailMessage
{
    Subject = "To my friends",
    Body = "Hello everyone!\r\n\r\nI've attached a list of my favorite memories.",
};

string fileName = "attachment.txt";
string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

File.WriteAllText(filePath, "Hello World");

message.Attachments.Add(new EmailAttachment(filePath));

await Email.ComposeAsync(message);
```

## Body type

The body of an email message can specify either `Html` or `PlainText`. However, the platform and email client may or may not restrict the type of email you can send.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

Not all email clients for Android support `Html`, since there is no way to detect this, we recommend using `PlainText` when sending emails.

# [iOS](#tab/ios)

Both `Html` and `PlainText` are supported.

# [Windows](#tab/windows)

Only supports `PlainText` as the `BodyFormat`. Attempting to send an `Html` email throws the exception: `FeatureNotSupportedException`.

Not all email clients support sending attachments. For more information, see [Sending emails](/windows/uwp/contacts-and-calendar/sending-email).

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Email source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Email)
<!-- - [Email API documentation](xref:Microsoft.Maui.Essentials.Email)-->
