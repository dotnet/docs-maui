---
title: "Version tracking"
description: "Learn how to use the .NET MAUI VersionTracking class, which lets you check the applications version and build numbers along with seeing additional information."
ms.date: 05/28/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Version Tracking

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IVersionTracking` class. This class lets you check the applications version and build numbers along with seeing additional information such as if it's the first time the application launched.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Check the version

The first time you use the `VersionTracking` class, it starts tracking the current version. You must call `Track` early in your application's lifecycle, and each time it's loaded to ensure the current version information is tracked:

```csharp
VersionTracking.Track();
```

After the initial `Track` is called, version information can be read:

```csharp
// First time ever launched application
var firstLaunch = VersionTracking.IsFirstLaunchEver;

// First time launching current version
var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;

// First time launching current build
var firstLaunchBuild = VersionTracking.IsFirstLaunchForCurrentBuild;

// Current app version (2.0.0)
var currentVersion = VersionTracking.CurrentVersion;

// Current build (2)
var currentBuild = VersionTracking.CurrentBuild;

// Previous app version (1.0.0)
var previousVersion = VersionTracking.PreviousVersion;

// Previous app build (1)
var previousBuild = VersionTracking.PreviousBuild;

// First version of app installed (1.0.0)
var firstVersion = VersionTracking.FirstInstalledVersion;

// First build of app installed (1)
var firstBuild = VersionTracking.FirstInstalledBuild;

// List of versions installed (1.0.0, 2.0.0)
var versionHistory = VersionTracking.VersionHistory;

// List of builds installed (1, 2)
var buildHistory = VersionTracking.BuildHistory;
```

## Platform differences

<!-- TODO: file name contains xamarin, what is it? the secure-storage article also has this./ -->

All version information is stored using the [Preferences](../storage/preferences.md) API, and is stored with a filename of _[YOUR-APP-PACKAGE-ID].xamarinessentials_.versiontracking** and follows the same data persistence outlined in the [Preferences](../storage/preferences.md#persistence) documentation.
