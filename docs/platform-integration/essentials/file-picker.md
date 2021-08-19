---
title: "File picker"
description: "Learn how to use the .NET MAUIFilePicker class in the Microsoft.Maui.Essentials namespace, which lets a user choose one or more files from the device."
ms.date: 08/17/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# File picker

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `FilePicker` class. With the `FilePicker` class, you can prompt the user to pick one or more files from the device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To access the **FilePicker** functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `ReadExternalStorage` permission is required and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
  ```

  \- or -

- Use the Android project properties:

  <!-- TODO: Check on this value -->
  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **ReadExternalStorage** permission. This will automatically update the _AndroidManifest.xml_ file.

# [iOS](#tab/ios)

To enable iCloud capabilities in the file picker, follow these [directions](../ios/platform/document-picker.md#enabling-icloud-in-maui).

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

> [!IMPORTANT]
> All methods must be called on the UI thread because permission checks and requests are automatically handled by .NET MAUI Essentials.

## Pick file

The `FilePicker.PickAsync` method prompts the user to pick a file from the device. Use the `PickOptions` type to specify the title and file types allowed with the picker. The following example demonstrates opening the picker and processing the selected image:

```csharp
async Task<FileResult> PickAndShow(PickOptions options)
{
    try
    {
        var result = await FilePicker.PickAsync(options);
        if (result != null)
        {
            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = await result.OpenReadAsync();
                var image = ImageSource.FromStream(() => stream);
            }
        }

        return result;
    }
    catch (Exception ex)
    {
        // The user canceled or something went wrong
    }

    return null;
}
```

Default file types are provided with `FilePickerFileType.Images`, `FilePickerFileType.Png`, and `FilePickerFilerType.Videos`. You can specify custom file types per platform, by creating an instance of the `FilePickerFileType` class. The constructor of this class takes a dictionary that is keyed by the `DevicePlatform` type to identify the platform. The value of the dictionary key is a collection of strings representing the file types. For example here is how you would specify specific comic file types:

```csharp
var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // or general UTType values
                        { DevicePlatform.Android, new[] { "application/comics" } },
                        { DevicePlatform.UWP, new[] { ".cbr", ".cbz" } },
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // or general UTType values
                    });

PickOptions options = new()
{
    PickerTitle = "Please select a comic file",
    FileTypes = customFileType,
};
```

## Pick multiple files

If you want the user to pick multiple files, call the `FilePicker.PickMultipleAsync` method. This method also takes a `PickOptions` parameter to specify additional information. The results are the same as `PickAsync`, but instead of the `FileResult` type returned, an `IEnumerable<FileResult>` type is returned with all of the selected files.

[!INCLUDE [tip-file-result](includes/tip-file-result.md)]

## API

- [FilePicker source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/FilePicker)
<!-- - [FilePicker API documentation](xref:Microsoft.Maui.Essentials.FilePicker)-->
