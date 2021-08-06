---
title: "Xamarin.Essentials Open Browser"
description: "The Browser class in Microsoft.Maui.Essentials enables an application to open a web link in the optimized system preferred browser or the external browser."
ms.date: 09/24/2020
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Browser

The `Browser` class enables an application to open a web link in the optimized system preferred browser or the external browser.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **Browser** functionality the following platform specific setup is required.

# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your Android Manifest with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node:

```xml
<queries>
  <intent>
    <action android:name="android.intent.action.VIEW" />
    <data android:scheme="http"/>
  </intent>
  <intent>
    <action android:name="android.intent.action.VIEW" />
    <data android:scheme="https"/>
  </intent>
</queries>
```

# [iOS](#tab/ios)

No additional setup required.

# [Windows](#tab/windows)

No platform differences.

-----

## Using Browser

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

The Browser functionality works by calling the `OpenAsync` method with the `Uri` and `BrowserLaunchMode`.

```csharp

public class BrowserTest
{
    public async Task OpenBrowser(Uri uri)
    {
        try
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch(Exception ex)
        {
            // An unexpected error occured. No browser may be installed on the device.
        }
    }
}
```

This method returns after the browser was _launched_ and not necessarily _closed_ by the user.  The `bool` result indicates whether the launching was successful or not.

## Customization

When using the system preferred browser there are several customization options available for iOS and Android. This includes a `TitleMode` (Android only), and preferred color options for the `Toolbar` (iOS and Android) and `Controls` (iOS only) that appear.

These options are specified using `BrowserLaunchOptions` when calling `OpenAsync`.

```csharp
await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
```

![Browser Options.](images/browser-options.png)

## Platform implementation specifics

# [Android](#tab/android)

The Launch Mode determines how the browser is launched:

## System Preferred

[Custom Tabs](https://developer.chrome.com/multidevice/android/customtabs) will attempted to be used to load the Uri and keep navigation awareness.

## External

An `Intent` will be used to request the Uri be opened through the systems normal browser.

# [iOS](#tab/ios)

## System Preferred

[SFSafariViewController](xref:SafariServices.SFSafariViewController) is used to load the Uri and keep navigation awareness.

## External

The standard `OpenUrl` on the main application is used to launch the default browser outside of the application.

# [Windows](#tab/windows)

The user's default browser will always be launched regardless of the `BrowserLaunchMode`.

--------------

## API

- [Browser source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Browser)
<!-- - [Browser API documentation](xref:Microsoft.Maui.Essentials.Browser)-->
