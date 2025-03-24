---
title: "Email"
description: "Learn how to use the .NET MAUI IEmail interface in the Microsoft.Maui.ApplicationModel.Communication namespace to open the default email application. The subject, body, and recipients of an email can be set."
ms.date: 03/24/2025
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.Communication"]
---

# Email

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.Communication.IEmail> interface to open the default email app. When the email app is loaded, it can be set to create a new email with the specified recipients, subject, and body.

The default implementation of the `IEmail` interface is available through the <xref:Microsoft.Maui.ApplicationModel.Communication.Email.Default?displayProperty=nameWithType> property. Both the `IEmail` interface and `Email` class are contained in the `Microsoft.Maui.ApplicationModel.Communication` namespace.

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

# [iOS/Mac Catalyst](#tab/macios)

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

The Email functionality works by providing the email information as an argument to the <xref:Microsoft.Maui.ApplicationModel.Communication.Email.ComposeAsync%2A> method. In this example, the <xref:Microsoft.Maui.ApplicationModel.Communication.EmailMessage> type is used to represent the email information:

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="email_compose":::

## File attachments

When creating the email provided to the email client, you can add file attachments. The file type (MIME) is automatically detected, so you don't need to specify it. Some mail clients may restrict the types of files you send, or possibly prevent attachments altogether.

Use the <xref:Microsoft.Maui.ApplicationModel.Communication.EmailMessage.Attachments?displayProperty=nameWithType> collection to manage the files attached to an email.

The following example demonstrates adding an image file to the email attachments.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="email_picture" highlight="18":::

### Control file locations

[!INCLUDE [android-fileproviderpaths](../includes/android-fileproviderpaths.md)]

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
## Platform Differences

# [Android](#tab/android)

Not all email clients for Android support <xref:Microsoft.Maui.ApplicationModel.Communication.EmailBodyFormat.Html?displayProperty=nameWithType>, since there is no way to detect this, we recommend using <xref:Microsoft.Maui.ApplicationModel.Communication.EmailBodyFormat.PlainText?displayProperty=nameWithType> when sending emails.

# [iOS/Mac Catalyst](#tab/macios)

Both <xref:Microsoft.Maui.ApplicationModel.Communication.EmailBodyFormat.Html?displayProperty=nameWithType> and <xref:Microsoft.Maui.ApplicationModel.Communication.EmailBodyFormat.PlainText?displayProperty=nameWithType> are supported.

> [!WARNING]
> To use the email API on iOS, you must run it on a physical device. Otherwise, an exception is thrown.

# [Windows](#tab/windows)

Only supports <xref:Microsoft.Maui.ApplicationModel.Communication.EmailBodyFormat.PlainText?displayProperty=nameWithType> as the <xref:Microsoft.Maui.ApplicationModel.Communication.EmailMessage.BodyFormat?displayProperty=nameWithType>. Attempting to send an `Html` email throws the exception: <xref:Microsoft.Maui.ApplicationModel.FeatureNotSupportedException>.

Not all email clients support sending attachments. <!-- For more information, see [Sending emails](/windows/uwp/contacts-and-calendar/sending-email).-->

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
