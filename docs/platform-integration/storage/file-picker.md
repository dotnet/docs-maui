---
title: "File picker"
description: "Learn how to use the .NET MAUI IFilePicker interface in the Microsoft.Maui.Storage namespace, which lets a user choose one or more files from the device."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Storage", "FilePicker"]
---

# File picker

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IFilePicker` interface. With the `IFilePicker` interface, you can prompt the user to pick one or more files from the device.

The default implementation of the `IFilePicker` interface is available through the `FilePicker.Default` property. Both the `IFilePicker` interface and `FilePicker` class are contained in the `Microsoft.Maui.Storage` namespace.

## Get started

To access the **FilePicker** functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `ReadExternalStorage` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
  ```
<!-- NOT SUPPORTED
  \- or -

- Use the Android project properties:

  TODO: Check on this value

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **ReadExternalStorage** permission. This will automatically update the _AndroidManifest.xml_ file.
-->

# [iOS/Mac Catalyst](#tab/macios)

Enable iCloud capabilities. For more information, see [Capabilities](~/ios/capabilities.md).

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
