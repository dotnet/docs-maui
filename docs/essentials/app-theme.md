---
title: "Xamarin.Essentials: App Theme"
description: "This document describes the Requested App Theme API in Xamarin.Essentials, which provides information as to what theme style is requested for the running app."
author: jamesmontemagno
ms.custom: video
ms.author: jamont
ms.date: 01/06/2020
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: App Theme

The **RequestedTheme** API is part of the [`AppInfo`](app-information.md) class and provides information as to what theme is requested for your running app by the system.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using RequestedTheme

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

## Obtaining Theme Information

The requested application theme can be detected with the following API:

```csharp
AppTheme appTheme = AppInfo.RequestedTheme;

```

This will provide the current requested theme by the system for your application. The return value will be one of the following:

* Unspecified
* Light
* Dark

Unspecified will be returned when the operating system does not have a specific user interface style to request. An example of this is on devices running versions of iOS older than 13.0.


## Platform Implementation Specifics

# [Android](#tab/android)

Android uses configuration modes to specify the type of theme to request from the user. Based on the version of Android, it can be changed by the user or is changed when battery saver mode is enabled.

You can read more on the official [Android documentation for Dark Theme](https://developer.android.com/guide/topics/ui/look-and-feel/darktheme).


# [iOS](#tab/ios)

Unspecified will always be returned on versions of iOS older than 13.0


# [UWP](#tab/uwp)

Calling `RequestedTheme` must be called on the UI thread or an exception will be thrown.

UWP applications will respect your setting in the UWP App.xaml under **RequestedTheme**. If it is set to a specific theme, Xamarin.Essentials will always return this setting. To use dynamic theme of the OS remove this node from your application and then when your app is run it will return the theme set by the user in Windows settings (**Settings > Personalization > Colors > Choose your default app mode**).

You can read more on the [UWP Requested Theme Documentation](/uwp/api/windows.ui.xaml.application.requestedtheme).

--------------

## API

- [AppInfo source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/AppInfo)
- [AppInfo API documentation](xref:Xamarin.Essentials.AppInfo)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Theme-Detection-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
