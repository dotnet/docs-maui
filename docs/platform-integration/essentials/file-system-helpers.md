---
title: "File System Helpers"
description: "The FileSystem class in Microsoft.Maui.Essentials contains a series of helpers to find the application's cache and data directories and open files inside of the app package."
ms.date: 11/04/2018
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# File System Helpers

The `FileSystem` class contains a series of helpers to find the application's cache and data directories and open files inside of the app package.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using File System Helpers

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To get the application's directory to store **cache data**. Cache data can be used for any data that needs to persist longer than temporary data, but should not be data that is required to properly operate, as the OS dictates when this storage is cleared.

```csharp
var cacheDir = FileSystem.CacheDirectory;
```

To get the application's top-level directory for any files that are not user data files. These files are backed up with the operating system syncing framework. See Platform Implementation Specifics below.

```csharp
var mainDir = FileSystem.AppDataDirectory;
```

To open a file that is bundled into the application package:

```csharp
 using (var stream = await FileSystem.OpenAppPackageFileAsync(templateFileName))
 {
    using (var reader = new StreamReader(stream))
    {
        var fileContents = await reader.ReadToEndAsync();
    }
 }
```

## Platform implementation specifics

# [Android](#tab/android)

- **CacheDirectory** – Returns the [CacheDir](https://developer.android.com/reference/android/content/Context.html#getCacheDir) of the current context.
- **AppDataDirectory** – Returns the [FilesDir](https://developer.android.com/reference/android/content/Context.html#getFilesDir) of the current context and are backed up using [Auto Backup](https://developer.android.com/guide/topics/data/autobackup.html) starting on API 23 and above.

Add any file into the **Assets** folder in the Android project and mark the Build Action as **AndroidAsset** to use it with `OpenAppPackageFileAsync`.

# [iOS](#tab/ios)

- **CacheDirectory** – Returns the [Library/Caches](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory.
- **AppDataDirectory** – Returns the [Library](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory that is backed up by iTunes and iCloud.

> [!IMPORTANT]
> In the iOS Simulator, the Application ID (which is part of the directory name) changes on every build so you have to retrieve the correct ID each time you build your application for the Simulator.

Add any file into the **Resources** folder in the iOS project and mark the Build Action as **BundledResource** to use it with `OpenAppPackageFileAsync`.

# [Windows](#tab/windows)

- **CacheDirectory** – Returns the [LocalCacheFolder](/uwp/api/windows.storage.applicationdata.localcachefolder#Windows_Storage_ApplicationData_LocalCacheFolder) directory..
- **AppDataDirectory** – Returns the [LocalFolder](/uwp/api/windows.storage.applicationdata.localfolder#Windows_Storage_ApplicationData_LocalFolder) directory that is backed up to the cloud.

Add any file into the root in the UWP project and mark the Build Action as **Content** to use it with `OpenAppPackageFileAsync`.

--------------

## API

- [File System Helpers source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/FileSystem)
<!-- - [File System API documentation](xref:Microsoft.Maui.Essentials.FileSystem)-->
