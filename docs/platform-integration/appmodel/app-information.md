---
title: "App Information"
description: "Describes the AppInfo class in Microsoft.Maui.Essentials, which provides information about your application. For example, it exposes the app name and version."
ms.date: 08/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# App Information

The `AppInfo` class provides information about your application.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

## Using AppInfo

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

## Read the app information

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

## Display app settings

The `AppInfo` class can also display a page of settings maintained by the operating system for the application:

```csharp
// Display settings page
AppInfo.ShowSettingsUI();
```

This settings page allows the user to change application permissions and perform other platform-specific tasks.

## Platform implementation specifics

This section describes platform-specific implementation details related to the `AppInfo` type.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

App information is taken from the _AndroidManifest.xml_ for the following fields:

- **Build** – `android:versionCode` in `manifest` node
- **Name** - `android:label` in the `application` node
- **PackageName**: `package` in the `manifest` node
- **VersionString** – `android:versionName` in the `application` node

# [iOS](#tab/ios)

App information is taken from the _Info.plist_ for the following fields:

- **Build** – `CFBundleVersion`
- **Name** - `CFBundleDisplayName` if set, else `CFBundleName`
- **PackageName**: `CFBundleIdentifier`
- **VersionString** – `CFBundleShortVersionString`

# [Windows](#tab/windows)

App information is taken from the _Package.appxmanifest_ for the following fields:

- **Build** – Uses the `Build` from the `Version` on the `Identity` node
- **Name** - `DisplayName` on the `Properties` node
- **PackageName**: `Name` on the `Identity` node
- **VersionString** – `Version` on the `Identity` node

--------------

<!-- markdownlint-enable MD025 -->

## API

- [AppInfo source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/AppInfo)
<!-- - [AppInfo API documentation](xref:Microsoft.Maui.Essentials.AppInfo)-->
