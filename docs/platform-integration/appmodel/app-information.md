---
title: "App Information"
description: "Describes the IAppInfo interface in the Microsoft.Maui.ApplicationModel namespace, which provides information about your application. For example, it exposes the app name and version."
ms.date: 08/07/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# App information

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.IAppInfo> interface, which provides information about your application.

The default implementation of the `IAppInfo` interface is available through the <xref:Microsoft.Maui.ApplicationModel.AppInfo.Current?displayProperty=nameWithType> property. Both the `IAppInfo` interface and `AppInfo` class are contained in the `Microsoft.Maui.ApplicationModel` namespace.

## Read the app information

The `IAppInfo` interface exposes the following properties:

- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.Name> &mdash; The application name.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.PackageName> &mdash; The package name or application identifier, such as `com.microsoft.myapp`.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.VersionString> &mdash; The application version, such as `1.0.0`.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.Version> &mdash; The application version, as a <xref:System.Version> object.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.BuildString> &mdash; The build number of the version, such as `1000`.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.RequestedTheme> &mdash; The detected theme of the system or application.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.PackagingModel> &mdash; The packaging model of the application.
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.RequestedLayoutDirection> &mdash; The requested layout direction of the system or application.

The following code example demonstrates accessing some of these properties:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="read_info":::

## Get the current theme

The <xref:Microsoft.Maui.ApplicationModel.IAppInfo.RequestedTheme> property provides the current requested theme by the system for your application. One of the following values is returned:

- <xref:Microsoft.Maui.ApplicationModel.AppTheme.Unspecified>
- <xref:Microsoft.Maui.ApplicationModel.AppTheme.Light>
- <xref:Microsoft.Maui.ApplicationModel.AppTheme.Dark>

`Unspecified` is returned when the operating system doesn't have a specific user interface style. An example of this is on devices running versions of iOS older than 13.0.

The following code example demonstrates getting the theme:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="read_theme":::

## Get the layout direction

The <xref:Microsoft.Maui.ApplicationModel.IAppInfo.RequestedLayoutDirection> property provides the current layout direction used by the system for your application. One of the following values is returned:

- <xref:Microsoft.Maui.ApplicationModel.LayoutDirection.Unknown>
- <xref:Microsoft.Maui.ApplicationModel.LayoutDirection.LeftToRight>
- <xref:Microsoft.Maui.ApplicationModel.LayoutDirection.RightToLeft>

`Unknown` is returned when the layout direction is unknown.

The following code example demonstrates getting the layout direction:

```csharp
LayoutDirection layoutDirection = AppInfo.Current.RequestedLayoutDirection;
```

## Display app settings

The <xref:Microsoft.Maui.ApplicationModel.IAppInfo> class can also display a page of settings maintained by the operating system for the application:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="show_settings":::

This settings page allows the user to change application permissions and perform other platform-specific tasks.

## Platform implementation specifics

This section describes platform-specific implementation details related to the <xref:Microsoft.Maui.ApplicationModel.IAppInfo> interface.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

App information is taken from the _AndroidManifest.xml_ for the following fields:

- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.BuildString> &mdash; `android:versionCode` in `manifest` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.Name> &mdash; `android:label` in the `application` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.PackageName> &mdash; `package` in the `manifest` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.VersionString> &mdash; `android:versionName` in the `manifest` node

### Requested theme

Android uses configuration modes to specify the type of theme to request from the user. Based on the version of Android, it can be changed by the user or may be changed when battery saver mode is enabled.

You can read more on the official [Android documentation for Dark Theme](https://developer.android.com/guide/topics/ui/look-and-feel/darktheme).

# [iOS/Mac Catalyst](#tab/macios)

App information is taken from the _Info.plist_ for the following fields:

- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.BuildString> &mdash; `CFBundleVersion`
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.Name> &mdash; `CFBundleDisplayName` if set, else `CFBundleName`
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.PackageName> &mdash; `CFBundleIdentifier`
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.VersionString> &mdash; `CFBundleShortVersionString`

### Requested theme

_Unspecified_ is always returned on versions of iOS older than 13.0

# [Windows](#tab/windows)

App information is taken from the _Package.appxmanifest_ for the following fields:

- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.BuildString> &mdash; Uses the `Build` from the `Version` on the `Identity` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.Name> &mdash; `DisplayName` on the `Properties` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.PackageName> &mdash; `Name` on the `Identity` node
- <xref:Microsoft.Maui.ApplicationModel.IAppInfo.VersionString> &mdash; `Version` on the `Identity` node

### Requested theme

Code that accesses the `IAppInfo.RequestedTheme` property must be called on the UI thread or an exception will be thrown.

Windows applications respect the `RequestedTheme` property setting in the Windows _App.xaml_. If it's set to a specific theme, this API always returns this setting. To use the dynamic theme of the OS, remove this property from your application. When your app is run, it returns the theme set by the user in Windows settings: **Settings** > **Personalization** > **Colors** > **Choose your default app mode**.

<!-- TODO: You can read more on the [Windows Requested Theme Documentation](/uwp/api/windows.ui.xaml.application.requestedtheme). -->

--------------

<!-- markdownlint-enable MD025 -->
