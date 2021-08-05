---
title: "Essentials: App Theme"
description: "Describes the AppInfo class in the Microsoft.Maui.Essentials namespace and how it can be used to detect the requested app theme."
ms.date: 08/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials", "RequestedTheme"]
---

# App Theme

The `RequestedTheme` API is part of the [`AppInfo`](app-information.md) class and provides information as to what theme is requested by the system for your running app.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using RequestedTheme

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Read the theme setting

The requested app theme can be detected with the following code:

```csharp
AppTheme appTheme = AppInfo.RequestedTheme;
```

This class provides the current requested theme by the system for your application. One of the following values is returned:

- Unspecified
- Light
- Dark

_Unspecified_ is returned when the operating system doesn't have a specific user interface style. An example of this is on devices running versions of iOS older than 13.0.

## Platform specifics

This section describes platform-specific implementation details related to the `AppInfo.RequestedTheme` property.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

Android uses configuration modes to specify the type of theme to request from the user. Based on the version of Android, it can be changed by the user or may be changed when battery saver mode is enabled.

You can read more on the official [Android documentation for Dark Theme](https://developer.android.com/guide/topics/ui/look-and-feel/darktheme).

# [iOS](#tab/ios)

_Unspecified_ is always returned on versions of iOS older than 13.0

# [Windows](#tab/windows)

Code that accesses the `AppInfo.RequestedTheme` property must be called on the UI thread or an exception will be thrown.

Windows applications respect the **RequestedTheme** property setting in the Windows _App.xaml_. If it's set to a specific theme, this API always returns this setting. To use the dynamic theme of the OS, remove this property from your application. When your app is run, it returns the theme set by the user in Windows settings: **Settings** > **Personalization** > **Colors** > **Choose your default app mode**.

<!-- TODO: You can read more on the [UWP Requested Theme Documentation](/uwp/api/windows.ui.xaml.application.requestedtheme). -->

--------------

<!-- markdownlint-enable MD025 -->

## API

- [AppInfo source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/AppInfo)
<!-- - [AppInfo API documentation](xref:Microsoft.Maui.Essentials.AppInfo)-->
