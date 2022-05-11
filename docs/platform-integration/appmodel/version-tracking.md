---
title: "Version tracking"
description: "Learn how to use the .NET MAUI VersionTracking class, which lets you check the applications version and build numbers along with seeing additional information."
ms.date: 05/10/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Version Tracking

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IVersionTracking` interface. This class lets you check the applications version and build numbers along with seeing additional information such as if it's the first time the application launched. The `IVersionTracking` interface is exposed through the `VersionTracking.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `VersionTracking` and `IVersionTracking` types are available in the `Microsoft.Maui.ApplicationModel` namespace.

## Check the version

The first time you use the `VersionTracking` class, it starts tracking the current version. You must call `Track` early in your application's lifecycle, and each time it's loaded to ensure the current version information is tracked. A good place to track your version would be in the `App` class:

:::code language="csharp" source="../snippets/shared_1/App.xaml.cs" id="version_track":::

After the initial `Track` is called, version information can be read. The following example writes the tracking information to labels on the page:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="version_read":::

## Platform differences

<!-- TODO: file name contains xamarin, what is it? the secure-storage article also has this./ -->

All version information is stored using the [Preferences](../storage/preferences.md) API, and is stored with a filename of _[YOUR-APP-PACKAGE-ID].xamarinessentials_.versiontracking** and follows the same data persistence outlined in the [Preferences](../storage/preferences.md#persistence) documentation.
