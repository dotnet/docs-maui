---
title: "Phone dialer"
description: "Learn how to open the phone dialer to a specific number, in .NET MAUI. The PhoneDialer class in the Microsoft.Maui.Essentials namespace is used to open the phone dialer."
ms.date: 08/25/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Phone Dialer

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `PhoneDialer` class. This class enables an application to open a phone number in the dialer.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

To access the phone dialer functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your _Android Manifest_ with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the _AndroidManifest.xml_ file in the **Properties** folder of the project, and add the following in the **manifest** node:

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

The phone dialer functionality works by calling the `PhoneDialer.Open` method, with a phone number. When the phone dialer is opened, .NET MAUI will automatically attempt to format the number based on the country code, if specified.

```csharp
public void PlacePhoneCall(string number)
{
    try
    {
        PhoneDialer.Open(number);
    }
    catch (ArgumentNullException anEx)
    {
        // Number was null or white space
    }
    catch (FeatureNotSupportedException ex)
    {
        // Phone Dialer is not supported on this device.
    }
    catch (Exception ex)
    {
        // Other error has occurred.
    }
}
```

## API

- [Phone Dialer source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/PhoneDialer)
<!-- - [Phone Dialer API documentation](xref:Microsoft.Maui.Essentials.PhoneDialer)-->
