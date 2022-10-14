---
title: "Open the browser"
description: "The IBrowser interface in the Microsoft.Maui.ApplicationModel namespace enables an application to open a web link in the optimized system preferred browser or the external browser."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Browser

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IBrowser` interface. This interface enables an application to open a web link in the system-preferred browser or the external browser.

The default implementation of the `IBrowser` interface is available through the `Browser.Default` property. Both the `IBrowser` interface and `Browser` class are contained in the `Microsoft.Maui.ApplicationModel` namespace.

## Get started

To access the browser functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with queries that use Android's [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes the `manifest` node:

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

# [iOS\macOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Open the browser

The browser is opened by calling the `Browser.OpenAsync` method with the `Uri` and the type of `BrowserLaunchMode`. The following code example demonstrates opening the browser:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="browser_open":::

This method returns after the browser is launched, not after it was closed by the user. `Browser.OpenAsync` returns a `bool` value to indicate if the browser was successfully launched.

## Customization

When using the system-preferred browser, there are several customization options available for iOS and Android. These options include a `TitleMode` (Android only) and preferred color for the `Toolbar` (iOS and Android) and `Controls` (iOS only) that appear.

Specify these options using `BrowserLaunchOptions` when you call `OpenAsync`.

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="browser_open_custom":::

## Platform differences

This section describes the platform-specific differences with the browser API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
### [Android](#tab/android)

The `BrowserLaunchOptions.LaunchMode` determines how the browser is launched:

- `SystemPreferred`

  [Custom Tabs](https://developer.chrome.com/multidevice/android/customtabs) will try to be used to load the URI and keep navigation awareness.

- `External`

  An `Intent` is used to request the URI be opened through the system's normal browser.

# [iOS\macOS](#tab/ios)

The `BrowserLaunchOptions.LaunchMode` determines how the browser is launched:

- `SystemPreferred`

  [SFSafariViewController](xref:SafariServices.SFSafariViewController) is used to load the URI and keep navigation awareness.

- `External`

  The standard `OpenUrl` on the main application is used to launch the default browser outside of the application.

# [Windows](#tab/windows)

The user's default browser will always be launched regardless of the `BrowserLaunchMode`.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
