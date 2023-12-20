---
title: "File picker"
description: "Learn how to use the .NET MAUI IFilePicker interface in the Microsoft.Maui.Storage namespace, which lets a user choose one or more files from the device."
ms.date: 05/18/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "FilePicker"]
---

# File picker

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IFilePicker` interface. With the `IFilePicker` interface, you can prompt the user to pick one or more files from the device.

The default implementation of the `IFilePicker` interface is available through the `FilePicker.Default` property. Both the `IFilePicker` interface and `FilePicker` class are contained in the `Microsoft.Maui.Storage` namespace.

## Get started

To access the **FilePicker** functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If your app targets Android 12 or lower, you must request the `READ_EXTERNAL_STORAGE` permission. If your app targets Android 13 or higher and needs access to files that other apps have created, you must request one or more of the following granular permissions instead of the `READ_EXTERNAL_STORAGE` permission:

- `READ_MEDIA_IMAGES`
- `READ_MEDIA_VIDEO`
- `READ_MEDIA_AUDIO`

These permissions can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage, MaxSdkVersion = 32)]
  [assembly: UsesPermission(Android.Manifest.Permission.ReadMediaAudio)]
  [assembly: UsesPermission(Android.Manifest.Permission.ReadMediaImages)]
  [assembly: UsesPermission(Android.Manifest.Permission.ReadMediaVideo)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" android:maxSdkVersion="32" />
  <!-- Required only if your app needs to access images or photos that other apps created -->
  <uses-permission android:name="android.permission.READ_MEDIA_IMAGES" />
  <!-- Required only if your app needs to access videos that other apps created -->
  <uses-permission android:name="android.permission.READ_MEDIA_VIDEO" />
  <!-- Required only if your app needs to access audio files that other apps created -->
  <uses-permission android:name="android.permission.READ_MEDIA_AUDIO" />    
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the permissions listed above. This will automatically update the *AndroidManifest.xml* file.

# [iOS/Mac Catalyst](#tab/macios)

Enable iCloud capabilities. For more information, see [Capabilities](~/ios/capabilities.md).

Mac Catalyst apps that are released to the Mac App Store require Apple's App Sandbox to be enabled. The App Sandbox restricts access to system resources and user data in Mac apps, to contain damage if an app becomes compromised. For more information about enabling the App Sandbox, see [Add entitlements](~/mac-catalyst/deployment/publish-app-store.md#add-entitlements).

A consequence of enabling the App Sandbox for Mac Catalyst apps is that the file picker won't open. This is because the first time a user launches a sandboxed Mac Catalyst app, the system creates a container folder that the app has exclusive read-write access to. The system also restricts the app's file system access to its container. While the container includes symbolic links to common user folders, they're considered sensitive folders that require that your app includes specific entitlements before it grants access to these locations. Therefore, you must add the following entitlements to your app to use the file picker in Mac Catalyst apps that are published to the Mac App Store:

```xml
<key>com.apple.security.assets.movies.read-only</key>
<true/>
<key>com.apple.security.assets.music.read-only</key>
<true/>
<key>com.apple.security.assets.pictures.read-only</key>
<true/>
<key>com.apple.security.files.downloads.read-only</key>
<true/>
<key>com.apple.security.files.user-selected.read-only</key>
<true/>
<key>com.apple.security.personal-information.photos-library</key>
<true/>
```

For more information about adding entitlements to Mac Catalyst apps, see [Mac Catalyst entitlements](~/mac-catalyst/entitlements.md). For more information about managed file access, see [Enable managed file access](https://developer.apple.com/documentation/xcode/configuring-the-macos-app-sandbox/#Enable-managed-file-access) on developer.apple.com.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

> [!IMPORTANT]
> All methods must be called on the UI thread because permission checks and requests are automatically handled by .NET MAUI.

## Pick a file

The `PickAsync` method prompts the user to pick a file from the device. Use the `PickOptions` type to specify the title and file types allowed with the picker. The following example demonstrates opening the picker and processing the selected image:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="file_pick":::

Default file types are provided with `FilePickerFileType.Images`, `FilePickerFileType.Png`, and `FilePickerFilerType.Videos`. You can specify custom file types per platform, by creating an instance of the `FilePickerFileType` class. The constructor of this class takes a dictionary that is keyed by the `DevicePlatform` type to identify the platform. The value of the dictionary key is a collection of strings representing the file types. For example here's how you would specify specific comic file types:

:::code language="csharp" source="../snippets/shared_1/Storage.cs" id="file_types":::

Searching for files based on the file type may be different from one platform to the other. For more information, see [Platform differences](#platform-differences).

## Pick multiple files

If you want the user to pick multiple files, call the `FilePicker.PickMultipleAsync` method. This method also takes a `PickOptions` parameter to specify additional information. The results are the same as `PickAsync`, but instead of the `FileResult` type returned, an `IEnumerable<FileResult>` type is returned with all of the selected files.

[!INCLUDE [tip-file-result](../includes/tip-file-result.md)]

## Platform differences

This section describes the platform-specific differences with the file picker.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `PickOptions.PickerTitle` is displayed on the initial prompt to the user, but not in the picker dialog itself.

When filtering files by type, use the file's MIME type. For a list of MIME types, see [Mozilla - Common MIME types](https://developer.mozilla.org/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types).

# [iOS/Mac Catalyst](#tab/macios)

When filtering by file type, use Uniform Type Identifiers (UTType) values, specifically the identifier value. For more information, see [System-Declared Uniform Type Identifiers (Apple developer archive)](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/UTIRef/Articles/System-DeclaredUniformTypeIdentifiers.html) and [System-declared uniform type identifiers](https://developer.apple.com/documentation/uniformtypeidentifiers/system-declared_uniform_type_identifiers).

- **iOS**

  The `PickOptions.PickerTitle` isn't displayed to the user.

- **macOS**

  The `PickOptions.PickerTitle` is displayed to the user.

# [Windows](#tab/windows)

The `PickOptions.PickerTitle` isn't displayed to the user.

When filtering by file type, use the file extension, including the `.` character. For example, you can filter to GIF files with `.gif`.

-----
