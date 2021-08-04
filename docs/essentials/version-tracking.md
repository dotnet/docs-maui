---
title: "Xamarin.Essentials: Version Tracking"
description: "The VersionTracking class in Xamarin.Essentials lets you check the applications version and build numbers along with seeing additional information such as if it is the first time the application launched ever or for the current version, get the previous build information, and more."
author: jamesmontemagno
ms.author: jamont
ms.date: 05/28/2019
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Version Tracking

The **VersionTracking** class lets you check the applications version and build numbers along with seeing additional information such as if it is the first time the application launched ever or for the current version, get the previous build information, and more.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Version Tracking

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

The first time you use the **VersionTracking** class it will start tracking the current version. You must call `Track` early only in your application each time it is loaded to ensure the current version information is tracked:

```csharp
VersionTracking.Track();
```

After the initial `Track` is called version information can be read:

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

## Platform Implementation Specifics

All version information is stored using the [Preferences](preferences.md) API in Xamarin.Essentials and is stored with a filename of **[YOUR-APP-PACKAGE-ID].xamarinessentials.versiontracking** and follows the same data persistence outlined in the [Preferences](preferences.md#persistence) documentation.

## API

- [Version Tracking source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/VersionTracking)
- [Version Tracking API documentation](xref:Xamarin.Essentials.VersionTracking)
