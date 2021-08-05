---
title: "Xamarin.Essentials: Phone Dialer"
description: "The PhoneDialer class in Xamarin.Essentials enables an application to open a phone number in the dialer"
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 07/02/2019
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Phone Dialer

The **PhoneDialer** class enables an application to open a phone number in the dialer.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your Android Manifest with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.DIAL" />
    <data android:scheme="tel"/>
  </intent>
</queries>
```

# [iOS](#tab/ios)

No additional setup required.

# [UWP](#tab/uwp)

No platform differences.

-----

## Using Phone Dialer

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The Phone Dialer functionality works by calling the `Open` method with a phone number to open the dialer with. When `Open` is requested the API will automatically attempt to format the number based on the country code if specified.

```csharp
public class PhoneDialerTest
{
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
}
```

## API

- [Phone Dialer source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/PhoneDialer)
- [Phone Dialer API documentation](xref:Xamarin.Essentials.PhoneDialer)
