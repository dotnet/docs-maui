---
title: "Email"
description: "Learn how to use the .NET MAUI IEmail interface in the Microsoft.Maui.ApplicationModel.Communication namespace to open the default email application. The subject, body, and recipients of an email can be set."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.Communication"]
---

# Email

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IEmail` interface to open the default email app. When the email app is loaded, it can be set to create a new email with the specified recipients, subject, and body.

The default implementation of the `IEmail` interface is available through the `Email.Default` property. Both the `IEmail` interface and `Email` class are contained in the `Microsoft.Maui.ApplicationModel.Communication` namespace.

## Get started

To access the email functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with queries that use Android's [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes in the `manifest` node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.SENDTO" />
    <data android:scheme="mailto" />
  </intent>
</queries>
```

# [iOS\macOS](#tab/ios)

Apple requires that you define the schemes you want to use. Add the `LSApplicationQueriesSchemes` key and schemes to the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files:

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
  <string>mailto</string>
</array>
```

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Using Email

The Email functionality works by providing the email information as an argument to the `ComposeAsync` method. In this example, the `EmailMessage` type is used to represent the email information:

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="email_compose":::

## File attachments

When creating the email provided to the email client, you can add file attachments. The file type (MIME) is automatically detected, so you don't need to specify it. Some mail clients may restrict the types of files you send, or possibly prevent attachments altogether.

Use the `EmailMessage.Attachments` collection to manage the files attached to an email.

The following example demonstrates adding an image file to the email attachments.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="email_picture" highlight="18":::

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
## Platform Differences

# [Android](#tab/android)

Not all email clients for Android support `EmailBodyFormat.Html`, since there is no way to detect this, we recommend using `EmailBodyFormat.PlainText` when sending emails.

# [iOS\macOS](#tab/ios)

Both `EmailBodyFormat.Html` and `EmailBodyFormat.PlainText` are supported.

> [!WARNING]
> To use the email API on iOS, you must run it on a physical device. Otherwise, an exception is thrown.

# [Windows](#tab/windows)

Only supports `EmailBodyFormat.PlainText` as the `EmailMessage.BodyFormat`. Attempting to send an `Html` email throws the exception: `FeatureNotSupportedException`.

Not all email clients support sending attachments. <!-- For more information, see [Sending emails](/windows/uwp/contacts-and-calendar/sending-email).-->

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
