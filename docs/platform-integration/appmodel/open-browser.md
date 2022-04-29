---
title: "Open the browser"
description: "The Browser class in the Microsoft.Maui.Essentials namespace enables an application to open a web link in the optimized system preferred browser or the external browser."
ms.date: 08/25/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Browser

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Browser` class. This class enables an application to open a web link in the optimized system preferred browser or the external browser.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

To access the browser functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** you must update your _Android Manifest_ with queries that are used with the new [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

Open the _AndroidManifest.xml_ file in the **Properties** folder of the project, and add the following in the **manifest** node:

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

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Open the browser

The browser is opened by calling the `Browser.OpenAsync` method with the `Uri` and the type of `BrowserLaunchMode`. The following code example demonstrates opening the browser:

```csharp
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
```

This method returns after the browser is launched, not after it was closed by the user.  `Browser.OpenAsync` returns a `bool` value to indicate if the browser was successfully launched.

## Customization

When using the system preferred browser, there are several customization options available for iOS and Android. This includes a `TitleMode` (Android only), and preferred color options for the `Toolbar` (iOS and Android) and `Controls` (iOS only) that appear.

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

:::image type="content" source="media/open-browser/browser-options.png" alt-text="Demonstration of the browser options used with Browser.OpenAsync":::

## Platform differences

This section describes the platform-specific differences with the browser API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
### [Android](#tab/android)

The `BrowserLaunchOptions.LaunchMode` determines how the browser is launched:

- `SystemPreferred`

  [Custom Tabs](https://developer.chrome.com/multidevice/android/customtabs) will try to be used to load the Uri and keep navigation awareness.

- `External`

  An `Intent` is used to request the Uri be opened through the system's normal browser.

# [iOS](#tab/ios)

The `BrowserLaunchOptions.LaunchMode` determines how the browser is launched:

- `SystemPreferred`

  [SFSafariViewController](xref:SafariServices.SFSafariViewController) is used to load the Uri and keep navigation awareness.

- `External`

  The standard `OpenUrl` on the main application is used to launch the default browser outside of the application.

# [Windows](#tab/windows)

The user's default browser will always be launched regardless of the `BrowserLaunchMode`.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Browser source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Browser)
<!-- - [Browser API documentation](xref:Microsoft.Maui.Essentials.Browser)-->
