---
title: "File system helpers"
description: "Learn how to use the .NET MAUI FileSystem class in the Microsoft.Maui.Storage namespace. This class contains helper methods that access the application's cache and data directories, and helps open files in the app package."
ms.date: 07/14/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "FileSystem"]
---

# File system helpers

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IFileSystem` interface. This interface provides helper methods that access the app's cache and data directories, and helps access files in the app package. The `IFileSystem` interface is exposed through the `FileSystem.Current` property.

The `FileSystem` and `IFileSystem` types are available in the `Microsoft.Maui.Storage` namespace.

## Using file system helpers

Each operating system will have unique paths to the app cache and app data directories. The `IFileSystem` interface provides a cross-platform API for accessing these directory paths.

### Cache directory

To get the application's directory to store **cache data**. Cache data can be used for any data that needs to persist longer than temporary data, but shouldn't be data that is required to operate the app, as the operating system may clear this storage.

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_cache":::

### App data directory

To get the app's top-level directory for any files that aren't user data files. These files are backed up with the operating system syncing framework.

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_appdata":::

## Bundled files

To open a file that is bundled into the app package, use the `OpenAppPackageFileAsync` method and pass the file name. This method returns a read-only <xref:System.IO.Stream> representing the file contents. The following example demonstrates using a method to read the text contents of a file:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_readtxtfile":::

## Platform differences

This section describes the platform-specific differences with the file system helpers.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

- `FileSystem.CacheDirectory`\
Returns the [CacheDir](https://developer.android.com/reference/android/content/Context.html#getCacheDir) of the current context.

- `FileSystem.AppDataDirectory`\
Returns the [FilesDir](https://developer.android.com/reference/android/content/Context.html#getFilesDir) of the current context, which are backed up using [Auto Backup](https://developer.android.com/guide/topics/data/autobackup.html) starting on API 23 and above.

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

# [iOS\macOS](#tab/ios)

- `FileSystem.CacheDirectory`\
Returns the [Library/Caches](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory.

- `FileSystem.AppDataDirectory`\
Returns the [Library](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory that is backed up by iTunes and iCloud.

  > [!IMPORTANT]
  > In the iOS Simulator, the Application ID (which is part of the directory name) changes on every build so you have to retrieve the correct ID each time you build your application for the Simulator.

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

# [Windows](#tab/windows)

- `FileSystem.CacheDirectory`\
Returns the `LocalCacheFolder` directory. <!-- (/uwp/api/windows.storage.applicationdata.localcachefolder#Windows_Storage_ApplicationData_LocalCacheFolder) -->

- `FileSystem.AppDataDirectory`\
Returns the `LocalFolder` directory that is backed up to the cloud. <!-- (/uwp/api/windows.storage.applicationdata.localfolder#Windows_Storage_ApplicationData_LocalFolder) -->

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

-----
<!-- markdownlint-enable MD025 -->
