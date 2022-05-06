---
title: "App Information"
description: "Describes the AppInfo class in the Microsoft.Maui.ApplicationModel namespace, which provides information about your application. For example, it exposes the app name and version."
ms.date: 05/05/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# App Information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IAppInfo` interface, which provides information about your application. The `IAppInfo` interface is exposed through the `AppInfo.Current` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `AppInfo` class is available in the `Microsoft.Maui.ApplicationModel` namespace.

## Read the app information

There are four properties exposed by the `IAppInfo` interface:

- `IAppInfo.Name` &mdash; The application name
- `IAppInfo.PackageName` &mdash; The package name or application identifier, such as `com.microsoft.myapp`.
- `IAppInfo.VersionString` &mdash; The application version, such as `1.0.0`.
- `IAppInfo.BuildString` &mdash; The build number of the version, such as `1000`.

The following code example demonstrates accessing these properties:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="read_info":::

## Display app settings

The `IAppInfo` class can also display a page of settings maintained by the operating system for the application:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="show_settings":::

This settings page allows the user to change application permissions and perform other platform-specific tasks.

## Platform implementation specifics

This section describes platform-specific implementation details related to the `IAppInfo` interface.

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
