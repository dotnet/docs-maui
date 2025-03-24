---
ms.topic: include
ms.date: 03/24/2025
---

> [!IMPORTANT]
> This section only applies to Android.

In some scenarios on Android, such as when a file is in private storage, it can be copied into the app cache which is then shared via an Android `FileProvider`. However, this can [unintentionally expose the entire cache and app data](https://developer.android.com/privacy-and-security/risks/file-providers) to an attacker. This can be prevented by adding a file provider file paths override file to your app, and ensuring that files are copied to the location specified in this file prior to sharing.

To add a file provider file paths override file to your app, add a file named *microsoft_maui_essentials_fileprovider_file_paths.xml* to the *Platforms\Android\Resources\xml* folder in your app. Therefore, the full relative file name to the project should be *Platforms\Android\Resources\xml\microsoft_maui_essentials_fileprovider_file_paths.xml*. Then, add XML to the file for your required paths:

```xml
 <?xml version="1.0" encoding="UTF-8" ?>
 <paths>
    <external-path name="external_files" path="sharing-root" />
    <cache-path name="internal_cache" path="sharing-root" />
    <external-cache-path name="external_cache" path="sharing-root" />  
 </paths>
```

For more information about file provider paths, see [FileProvider](https://developer.android.com/reference/androidx/core/content/FileProvider) on developer.android.com.

Prior to sharing a file, you should ensure it's first written to the *sharing-root* folder in one of the locations from the override file:

```cs
// Write into the specific sub-directory
var dir = Path.Combine(FileSystem.CacheDirectory, "sharing-root");  
Directory.CreateDirectory(dir);
var file = Path.Combine(dir, "mydata.txt");
await File.WriteAllTextAsync(file, $"My data: {count}");

// Share the file
await Launcher.OpenAsync(new OpenFileRequest
{
   Title = "My data",
   File = new ReadOnlyFile(file),
});
```

You can verify that the file is being shared correctly if the shared URI excludes the sharing root directory. For example, if you share the file *\<CacheDirectory\>/sharing-root/mydata.txt* and the shared URI is `content://com.companyname.overwritefileproviderpaths.fileProvider/internal_cache/sharing-root/mydata.txt` then the file provider isn't using the correct path. If the shared URI is `content://com.companyname.overwritefileproviderpaths.fileProvider/internal_cache/mydata.txt` then the file provider is using the correct path.

> [!WARNING]
> When sharing a file, if you receive an `Java.Lang.IllegalArgumentException`, with a message similar to "Failed to find configured root that contains /data/data/com.companyname.overwritefileproviderpaths/cache/some-non-sharing-path/mydata.txt", you are most likely sharing a file that's outside of the sharing-root.
