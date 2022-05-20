---
title: "Version tracking"
description: "Learn how to use the .NET MAUI VersionTracking class, which lets you check the applications version and build numbers along with seeing additional information."
ms.date: 05/10/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Version tracking

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IVersionTracking` interface. This class lets you check the applications version and build numbers along with seeing additional information such as if it's the first time the application launched. The `IVersionTracking` interface is exposed through the `VersionTracking.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `VersionTracking` and `IVersionTracking` types are available in the `Microsoft.Maui.ApplicationModel` namespace.

## Get started

To enable version tracking in your app, add the `ConfigureEssentials` step to the `CreateMauiApp` bootstrap code. The app startup code is configured in the _MauiProgram.cs_ file. Call the `UseVersionTracking` method to enable version tracking.

:::code language="csharp" source="../snippets/shared_1/MauiProgram.cs" id="bootstrap_versiontracking" highlight="12-15":::

For more information, see [Configure fonts, services, and handlers at startup](../../fundamentals/app-startup.md).

## Check the version

The `IVersionTracking` interface provides many properties that describe the current version of the app and how it relates to the previous version. The following example writes the tracking information to labels on the page:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="version_read":::

The first time the app is run after version tracking is enabled, the `IsFirstLaunchEver` property will return `true`. If you add version tracking in a newer version of an already released app, `IsFirstLaunchEver` may incorrectly report `true`. This property always returns `true` the first time version tracking is enabled and the user runs the app. You can't fully rely on this property if users have upgraded from older versions that weren't tracking the version.

## Platform differences

All version information is stored using the [Preferences](../storage/preferences.md) API, and is stored with a filename of _[YOUR-APP-PACKAGE-ID].microsoft.maui.essentials.versiontracking_ and follows the same data persistence outlined in the [Preferences](../storage/preferences.md#persistence) documentation.
