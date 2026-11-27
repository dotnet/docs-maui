---
title: "File system helpers"
description: "Learn how to use the .NET MAUI IFileSystem interface in the Microsoft.Maui.Storage namespace. This interface contains helper methods that access the application's cache and data directories, and helps open files in the app package."
ms.date: 11/27/2026
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "FileSystem"]
ai-usage: ai-assisted
---

# File system helpers

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IFileSystem` interface. This interface provides helper methods that access the app's cache and data directories, and helps access files in the app package.

The default implementation of the `IFileSystem` interface is available through the `FileSystem.Current` property. Both the `IFileSystem` interface and `FileSystem` class are contained in the `Microsoft.Maui.Storage` namespace.

## Using file system helpers

Each operating system will have unique paths to the app cache and app data directories. The `IFileSystem` interface provides a cross-platform API for accessing these directory paths.

### Cache directory

Use the `CacheDirectory` property to get the app's directory for **cache data**. Use the cache directory for data that needs to persist longer than temporary data but that your app doesn't need to run, because the operating system might delete files in this directory. Store data here only if your app can re-create or download it again, and check that a file exists before you read it.

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_cache":::

### App data directory

Use the `AppDataDirectory` property to get the app's top-level directory for data that your app owns and needs to keep, such as a local database, settings files, or other files that your app manages on behalf of the user. Files in this directory are stored in app-specific storage, and your app doesn't need to request storage permissions to read and write them. This directory isn't intended for documents that users browse and manage themselves, for example with a file manager.

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_appdata":::

The app data directory isn't a synced folder. Depending on the platform, the app's configuration, and the user's device settings, files in this directory might be included in a device backup that's restored later, for example when the user sets up a new device. A backup isn't the same as file syncing: it doesn't keep files up to date across devices. A backup might be stored in a cloud service. For example, Android Auto Backup stores app data in a private folder in the user's Google Drive account. However, backed-up files don't appear as documents that users can browse in iCloud Drive or Google Drive. For more information, see [Backup and restore](#backup-and-restore).

> [!NOTE]
> `Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)` doesn't necessarily return the same directory as `FileSystem.AppDataDirectory`. For example, on iOS and Mac Catalyst, `LocalApplicationData` returns the app's `Documents` directory, while `AppDataDirectory` returns the app's `Library` directory. On Windows, `AppDataDirectory` returns a folder that's specific to your app. If you change the API that your app uses to build file paths, existing files stay in their original location, so you need to move them to the new location yourself.

### Choose a storage location

The following table summarizes where to store common types of app data:

| Data | Recommended location |
| --- | --- |
| Data that your app owns and needs to keep, such as a local database. | `FileSystem.AppDataDirectory` |
| Data that your app can re-create or download again, and that it can afford to have the system delete at any time, such as thumbnails. | `FileSystem.CacheDirectory` |
| Small key/value settings. | [Preferences](preferences.md) |
| Small secrets, such as tokens. | [Secure storage](secure-storage.md) |
| Files that users create and want to open in other apps, such as exports. | Let the user send the file to another app, for example with [Share](../data/share.md). Users can't browse files in the app data directory. |

## Bundled files

To open a file that is bundled into the app package, use the `OpenAppPackageFileAsync` method and pass the file name. This method returns a read-only <xref:System.IO.Stream> representing the file contents. The following example demonstrates using a method to read the text contents of a file:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_readtxtfile":::

The following example demonstrates opening a bundled font file from the app package:

```csharp
await using var myFont = await FileSystem.OpenAppPackageFileAsync("MyFont.ttf");
```

### Copy a bundled file to the app data folder

You can't modify an app's bundled file. But you can copy a bundled file to the [cache directory](#cache-directory) or [app data directory](#app-data-directory). The following example uses `OpenAppPackageFileAsync` to copy a bundled file to the app data folder:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="filesys_copyfile":::

## Backup and restore

Each platform decides whether files in the app data and cache directories are included in backups. .NET MAUI doesn't provide a cross-platform API to control backup, so you need to use platform configuration or platform APIs. Before you exclude a file from backup, consider whether the user can re-create or download it again on a restored device. If they can't, keep it in the backup.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Apps that target and run on Android 6.0 (API level 23) or higher participate in [Auto Backup](https://developer.android.com/identity/data/autobackup) by default. The .NET MAUI app template also sets `android:allowBackup="true"` in _Platforms/Android/AndroidManifest.xml_. Auto Backup includes files in `FileSystem.AppDataDirectory` by default, and excludes files in `FileSystem.CacheDirectory`. Backups occur only when the user has enabled backup on the device and other conditions are met, and cloud backup data is limited to 25 MB per app. For more information, see [Back up user data with Auto Backup](https://developer.android.com/identity/data/autobackup) on developer.android.com.

To exclude files from backup, place them in a dedicated folder in `FileSystem.AppDataDirectory` and exclude that folder with XML backup rules:

1. Create a _Platforms/Android/Resources/xml/data_extraction_rules.xml_ file. Android uses these rules when your app targets Android 12 (API level 31) or higher and runs on Android 12 or higher. The `<cloud-backup>` section applies to cloud backups and the `<device-transfer>` section applies to device-to-device transfers, so exclude the folder in both sections:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <data-extraction-rules>
      <cloud-backup>
        <exclude domain="file" path="nobackup/" />
      </cloud-backup>
      <device-transfer>
        <exclude domain="file" path="nobackup/" />
      </device-transfer>
    </data-extraction-rules>
    ```

1. Create a _Platforms/Android/Resources/xml/backup_rules.xml_ file. Android uses these rules on devices running Android 11 (API level 30) or lower, and on newer devices when your app targets API level 30 or lower:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <full-backup-content>
      <exclude domain="file" path="nobackup/" />
    </full-backup-content>
    ```

1. In _Platforms/Android/AndroidManifest.xml_, add the `android:fullBackupContent` and `android:dataExtractionRules` attributes to the `<application>` element to reference both files. The following example shows the `<application>` element from the .NET MAUI app template with these attributes added:

    ```xml
    <application android:allowBackup="true"
                 android:icon="@mipmap/appicon"
                 android:roundIcon="@mipmap/appicon_round"
                 android:supportsRtl="true"
                 android:fullBackupContent="@xml/backup_rules"
                 android:dataExtractionRules="@xml/data_extraction_rules"></application>
    ```

1. Store the files that you want to exclude in the folder:

    ```csharp
    string folder = Path.Combine(FileSystem.Current.AppDataDirectory, "nobackup");
    Directory.CreateDirectory(folder);
    string databasePath = Path.Combine(folder, "app.db3");
    ```

The `file` domain refers to `FileSystem.AppDataDirectory`. Paths in backup rules don't support wildcards, and a rule for a folder applies to all the files in that folder and its subfolders. If you exclude a SQLite database, its rollback journal (`-journal`), write-ahead log (`-wal`), and shared memory (`-shm`) files are stored next to the database file, so excluding the folder that contains the database also excludes these files.

To disable backup for your app, set `android:allowBackup="false"`. However, for apps that target Android 12 (API level 31) or higher, on devices from some manufacturers this setting disables cloud backup but doesn't disable device-to-device transfers. For more information, see [Behavior changes: Apps targeting Android 12](https://developer.android.com/about/versions/12/behavior-changes-12#backup-restore) on developer.android.com.

# [iOS/Mac Catalyst](#tab/macios)

On iOS and iPadOS, the contents of the app's `Library` directory, which is returned by `FileSystem.AppDataDirectory`, are included in device backups, such as iCloud Backup, except for the `Library/Caches` directory, which is returned by `FileSystem.CacheDirectory`. The system can delete files in `Library/Caches`, so don't use this directory as a way to keep data that your app needs out of a backup. iCloud Backup isn't the same as iCloud Drive: files in `FileSystem.AppDataDirectory` aren't synced with iCloud Drive, and they don't appear in the Files app.

If your app can download or re-create a file or folder, you can exclude it from backups by setting its `isExcludedFromBackup` resource value. To exclude a group of related files, such as a SQLite database and its related files, store them in a folder and set the value on the folder. In .NET for iOS, you can use the `NSFileManager.SetSkipBackupAttribute` method. The following example uses the same folder name as the Android example, so that shared code can use one path on both platforms:

```csharp
string folder = Path.Combine(FileSystem.Current.AppDataDirectory, "nobackup");
Directory.CreateDirectory(folder);
#if IOS
Foundation.NSError? error = Foundation.NSFileManager.SetSkipBackupAttribute(folder, true);
if (error is not null)
{
    // Handle the error
}
#endif
string databasePath = Path.Combine(folder, "app.db3");
```

Apple notes that certain file operations can reset this value. If you set it on a file, set it again each time you save the file. If you set it on a folder, consider setting it again each time your app starts, for example right after you call `Directory.CreateDirectory`. The value is guidance for the system about which items it can exclude, rather than a guarantee that the items never appear in a backup or on a restored device. For more information, see [Optimizing your app's data for iCloud Backup](https://developer.apple.com/documentation/foundation/optimizing-your-app-s-data-for-icloud-backup) on developer.apple.com.

The information and code in this tab apply to backups of iOS and iPadOS devices. They don't describe how app data is backed up on macOS, so the example only sets the value in iOS builds.

# [Windows](#tab/windows)

For packaged apps, `FileSystem.AppDataDirectory` returns the [`ApplicationData.LocalFolder`](/uwp/api/windows.storage.applicationdata.localfolder) directory, which Windows documents as backed up to the cloud. `FileSystem.CacheDirectory` returns the [`ApplicationData.LocalCacheFolder`](/uwp/api/windows.storage.applicationdata.localcachefolder) directory, which Windows documents as not included in backup and restore.

For unpackaged apps, `FileSystem.AppDataDirectory` and `FileSystem.CacheDirectory` return regular folders in the user's local app data folder. The `ApplicationData` backup behavior for packaged apps doesn't apply to these folders.

.NET MAUI doesn't provide a way to exclude individual files in `FileSystem.AppDataDirectory` from backup on Windows. In a packaged app, store files that your app can re-create or download again in `FileSystem.CacheDirectory`, which isn't included in backup and restore.

-----
<!-- markdownlint-enable MD025 -->

## Platform differences

This section describes the platform-specific differences with the file system helpers.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

- `FileSystem.CacheDirectory`\
Returns the [CacheDir](https://developer.android.com/reference/android/content/Context.html#getCacheDir()) of the current context. Android can delete files in this directory when the device is low on storage, and Auto Backup excludes them.

- `FileSystem.AppDataDirectory`\
Returns the [FilesDir](https://developer.android.com/reference/android/content/Context.html#getFilesDir()) of the current context. On Android 6.0 (API level 23) and higher, files in this directory are included in [Auto Backup](https://developer.android.com/identity/data/autobackup) by default. For more information, see [Backup and restore](#backup-and-restore).

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

  The `FileSystem.OpenPackageFileAsync` method can't get the length of the stream on Android by accessing the `Result.Length` property. Instead, you have to read the whole stream and count how many bytes there are to get the size of the asset.

# [iOS/Mac Catalyst](#tab/macios)

- `FileSystem.CacheDirectory`\
Returns the [Library/Caches](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory. On iOS, the system can delete files in this directory, and they aren't included in device backups.

- `FileSystem.AppDataDirectory`\
Returns the [Library](https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html) directory. On iOS, files in this directory, except for the `Library/Caches` directory, are included in device backups. For more information, see [Backup and restore](#backup-and-restore).

  > [!IMPORTANT]
  > The Application ID, which is part of the directory name, changes on every build so you have to retrieve the correct ID each time you build your app for a Simulator or device.
  >
  > On iOS, the app sandbox path contains an application GUID segment that changes across clean builds and app reinstalls. Never hard-code or persist absolute sandbox paths — they will break after an update or reinstall. Always reconstruct paths at runtime using `FileSystem.Current.AppDataDirectory`:
  >
  > ```csharp
  > // Correct: always construct the path at runtime
  > string path = Path.Combine(FileSystem.Current.AppDataDirectory, "myfile.db");
  >
  > // Incorrect: never persist or hard-code the absolute path —
  > // the GUID segment changes on clean builds and reinstalls
  > // e.g. "/var/mobile/Containers/Data/Application/SOME-GUID/Library/myfile.db"
  > ```

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

# [Windows](#tab/windows)

- `FileSystem.CacheDirectory`\
For packaged apps, returns the [`LocalCacheFolder`](/uwp/api/windows.storage.applicationdata.localcachefolder) directory. For unpackaged apps, returns a `Cache` folder in an app-specific folder, based on the app's publisher and name, in the user's local app data folder.

- `FileSystem.AppDataDirectory`\
For packaged apps, returns the [`LocalFolder`](/uwp/api/windows.storage.applicationdata.localfolder) directory. For unpackaged apps, returns a `Data` folder in an app-specific folder, based on the app's publisher and name, in the user's local app data folder. For more information, see [Backup and restore](#backup-and-restore).

- `FileSystem.OpenAppPackageFileAsync`\
Files that were added to the project with the **Build Action** of **MauiAsset** can be opened with this method. .NET MAUI projects will process any file in the _Resources\Raw_ folder as a **MauiAsset**.

> [!IMPORTANT]
> On Windows, packaged apps operate over a virtual file system where read and write operations are relocated to mapped locations. Therefore, files might not be located where expected. For more information, see [Common file system operations](/windows/msix/desktop/desktop-to-uwp-behind-the-scenes#common-file-system-operations) and [Flexible virtualization](/windows/msix/desktop/flexible-virtualization).

-----
<!-- markdownlint-enable MD025 -->
