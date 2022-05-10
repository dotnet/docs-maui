---
title: "File system helpers"
description: "Learn how to use the .NET MAUI FileSystem class in the Microsoft.Maui.Essentials namespace. This class contains helper methods that access the application's cache and data directories, and helps open files in the app package."
ms.date: 08/18/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# File system helpers

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `FileSystem` class. This class provides helper methods that access the app's cache and data directories, and helps access files in the app package.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

## Using file system helpers

Each operating system will have unique paths to the app cache and app data directories. The `FileSystem` class provides a cross-platform API for accessing these directory paths.

### Cache directory

To get the application's directory to store **cache data**. Cache data can be used for any data that needs to persist longer than temporary data, but shouldn't be data that is required to operate the app, as the operating system may clear this storage.

```csharp
string cacheDir = FileSystem.CacheDirectory;
```

### App data directory

To get the app's top-level directory for any files that aren't user data files. These files are backed up with the operating system syncing framework.

```csharp
string  mainDir = FileSystem.AppDataDirectory;
```

## Bundled files

To open a file that is bundled into the app package, use the `FileSystem.OpenAppPackageFileAsync` method and pass the file name. This method returns an <xref:System.IO.Stream> representing the file contents. The following example demonstrates using a method to read the text contents of a file:

```csharp
public async Task<string> ReadTextFile(string filePath)
{
    using Stream fileStream = await FileSystem.OpenAppPackageFileAsync(filePath);
    using StreamReader reader = new StreamReader(fileStream);

    return await reader.ReadToEndAsync();
}
```

## Platform differences

This section describes the platform-specific differences with the file system helpers.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

- `FileSystem.CacheDirectory`\
Returns the [CacheDir](https://developer.android.com/reference/android/content/Context.html#getCacheDir) of the current context.

- `FileSystem.AppDataDirectory`\
Returns the [FilesDir](https://developer.android.com/reference/android/content/Context.html#getFilesDir) of the current context, which are backed up using [Auto Backup](https://developer.android.com/guide/topics/data/autobackup.html) starting on API 23 and above.

- `FileSystem.OpenAppPackageFileAsync`\
Add any file into the _Assets_ folder in the Android project, and mark the **Build Action** as **AndroidAsset** to use it with `OpenAppPackageFileAsync`.

# [iOS](#tab/ios)

- `FileSystem.CacheDirectory`\
Returns the [Library/Caches](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory.

- `FileSystem.AppDataDirectory`\
Returns the [Library](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory that is backed up by iTunes and iCloud.

> [!IMPORTANT]
> In the iOS Simulator, the Application ID (which is part of the directory name) changes on every build so you have to retrieve the correct ID each time you build your application for the Simulator.

- `FileSystem.OpenAppPackageFileAsync`\
Add any file into the _Resources_ folder in the iOS project, and mark the **Build Action** as **BundledResource** to use it with `OpenAppPackageFileAsync`.

# [Windows](#tab/windows)

- `FileSystem.CacheDirectory`\
Returns the [LocalCacheFolder](/uwp/api/windows.storage.applicationdata.localcachefolder#Windows_Storage_ApplicationData_LocalCacheFolder) directory..

- `FileSystem.AppDataDirectory`\
Returns the [LocalFolder](/uwp/api/windows.storage.applicationdata.localfolder#Windows_Storage_ApplicationData_LocalFolder) directory that is backed up to the cloud.

- `FileSystem.OpenAppPackageFileAsync`\
Add any file into the root in the Windows project, and mark the **Build Action** as **Content** to use it with `OpenAppPackageFileAsync`.

-----
<!-- markdownlint-enable MD025 -->
