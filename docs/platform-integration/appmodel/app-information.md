---
title: "App Information"
description: "Describes the IAppInfo interface in the Microsoft.Maui.ApplicationModel namespace, which provides information about your application. For example, it exposes the app name and version."
ms.date: 05/05/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# App information

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IAppInfo` interface, which provides information about your application. The `IAppInfo` interface is exposed through the `AppInfo.Current` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `AppInfo` and `IAppInfo` types are available in the `Microsoft.Maui.ApplicationModel` namespace.

## Read the app information

There are four properties exposed by the `IAppInfo` interface:

- `IAppInfo.Name` &mdash; The application name
- `IAppInfo.PackageName` &mdash; The package name or application identifier, such as `com.microsoft.myapp`.
- `IAppInfo.VersionString` &mdash; The application version, such as `1.0.0`.
- `IAppInfo.BuildString` &mdash; The build number of the version, such as `1000`.

The following code example demonstrates accessing these properties:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="read_info":::

## Read the current theme

The `RequestedTheme` property provides the current requested theme by the system for your application. One of the following values is returned:

- `Unspecified`
- `Light`
- `Dark`

`Unspecified` is returned when the operating system doesn't have a specific user interface style. An example of this is on devices running versions of iOS older than 13.0.

The following code example demonstrates reading the theme:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="read_theme":::

## Display app settings

The `IAppInfo` class can also display a page of settings maintained by the operating system for the application:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="show_settings":::

This settings page allows the user to change application permissions and perform other platform-specific tasks.

## Platform implementation specifics

This section describes platform-specific implementation details related to the `IAppInfo` interface.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

App information is taken from the _AndroidManifest.xml_ for the following fields:

- **Build** &mdash; `android:versionCode` in `manifest` node
- **Name** &mdash; `android:label` in the `application` node
- **PackageName** &mdash; `package` in the `manifest` node
- **VersionString** &mdash; `android:versionName` in the `application` node

### Requested theme

Android uses configuration modes to specify the type of theme to request from the user. Based on the version of Android, it can be changed by the user or may be changed when battery saver mode is enabled.

You can read more on the official [Android documentation for Dark Theme](https://developer.android.com/guide/topics/ui/look-and-feel/darktheme).

# [iOS](#tab/ios)

App information is taken from the _Info.plist_ for the following fields:

- **Build** &mdash; `CFBundleVersion`
- **Name** &mdash; `CFBundleDisplayName` if set, else `CFBundleName`
- **PackageName** &mdash; `CFBundleIdentifier`
- **VersionString** &mdash; `CFBundleShortVersionString`

### Requested theme

_Unspecified_ is always returned on versions of iOS older than 13.0

# [Windows](#tab/windows)

App information is taken from the _Package.appxmanifest_ for the following fields:

- **Build** &mdash; Uses the `Build` from the `Version` on the `Identity` node
- **Name** &mdash; `DisplayName` on the `Properties` node
- **PackageName** &mdash; `Name` on the `Identity` node
- **VersionString** &mdash; `Version` on the `Identity` node

### Requested theme

Code that accesses the `IAppInfo.RequestedTheme` property must be called on the UI thread or an exception will be thrown.

Windows applications respect the `RequestedTheme` property setting in the Windows _App.xaml_. If it's set to a specific theme, this API always returns this setting. To use the dynamic theme of the OS, remove this property from your application. When your app is run, it returns the theme set by the user in Windows settings: **Settings** > **Personalization** > **Colors** > **Choose your default app mode**.

<!-- TODO: You can read more on the [Windows Requested Theme Documentation](/uwp/api/windows.ui.xaml.application.requestedtheme). -->

--------------

<!-- markdownlint-enable MD025 -->
