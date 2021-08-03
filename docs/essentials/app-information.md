---
title: "Xamarin.Essentials: App Information"
description: "This document describes the AppInfo class in Xamarin.Essentials, which provides information about your application. For example, it exposes the app name and version."
author: jamesmontemagno
ms.author: jamont
ms.date: 01/29/2019
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: App Information

The **AppInfo** class provides information about your application.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using AppInfo

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

## Obtaining Application Information:

The following information is exposed through the API:

```csharp
// Application Name
var appName = AppInfo.Name;

// Package Name/Application Identifier (com.microsoft.testapp)
var packageName = AppInfo.PackageName;

// Application Version (1.0.0)
var version = AppInfo.VersionString;

// Application Build Number (1)
var build = AppInfo.BuildString;
```

## Displaying Application Settings

The **AppInfo** class can also display a page of settings maintained by the operating system for the application:

```csharp
// Display settings page
AppInfo.ShowSettingsUI();
```

This settings page allows the user to change application permissions and perform other platform-specific tasks.

## Platform Implementation Specifics

# [Android](#tab/android)

App information is taken from the `AndroidManifest.xml` for the following fields:

- **Build** – `android:versionCode` in `manifest` node
- **Name** - `android:label` in the `application` node
- **PackageName**: `package` in the `manifest` node
- **VersionString** – `android:versionName` in the `application` node

# [iOS](#tab/ios)

App information is taken from the `Info.plist` for the following fields:

- **Build** – `CFBundleVersion`
- **Name** - `CFBundleDisplayName` if set, else `CFBundleName`
- **PackageName**: `CFBundleIdentifier`
- **VersionString** – `CFBundleShortVersionString`

# [UWP](#tab/uwp)

App information is taken from the `Package.appxmanifest` for the following fields:

- **Build** – Uses the `Build` from the `Version` on the `Identity` node
- **Name** - `DisplayName` on the `Properties` node
- **PackageName**: `Name` on the `Identity` node
- **VersionString** – `Version` on the `Identity` node

--------------

## API

- [AppInfo source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/AppInfo)
- [AppInfo API documentation](xref:Xamarin.Essentials.AppInfo)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/App-Information-Essential-API-of-the-Week/player]

[!INCLUDE [xamarin-show-essentials](includes/xamarin-show-essentials.md)]
