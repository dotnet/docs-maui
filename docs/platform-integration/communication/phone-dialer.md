---
title: "Phone dialer"
description: "Learn how to open the phone dialer to a specific number, in .NET MAUI. The PhoneDialer class in the Microsoft.Maui.ApplicationModel.Communication namespace is used to open the phone dialer."
ms.date: 08/25/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.Communication"]
---

# Phone Dialer

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `IPhoneDialer` interface. This interface enables an application to open a phone number in the dialer. The `IPhoneDialer` interface is exposed through the `PhoneDialer.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `PhoneDialer` and `IPhoneDialer` types are available in the `Microsoft.Maui.ApplicationModel.Communication` namespace.

## Get started

To access the phone dialer functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with queries that use Android's [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes the `manifest` node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.DIAL" />
    <data android:scheme="tel"/>
  </intent>
</queries>
```

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Open the phone dialer

The phone dialer functionality works by calling the `Open` method with a phone number. When the phone dialer is opened, .NET MAUI will automatically attempt to format the number based on the country code, if specified.

:::code language="csharp" source="../snippets/shared_1/CommsPage.xaml.cs" id="phone_dial":::
