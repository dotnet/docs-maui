---
title: "Media Picker"
description: "Learn how to use the MediaPicker class to prompt the user to select or take a photo or video"
ms.date: 08/25/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Media Picker

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `MediaPicker` class. This class lets a user pick or take a photo or video on the device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To access the media picker functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `CAMERA`, `WRITE_EXTERNAL_STORAGE`, `READ_EXTERNAL_STORAGE` permissions are required, and must be configured in the Android project. These can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  // Needed for Picking photo/video
  [assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
  
  // Needed for Taking photo/video
  [assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]
  [assembly: UsesPermission(Android.Manifest.Permission.Camera)]
  
  // Add these properties if you would like to filter out devices that do not have cameras, or set to false to make them optional
  [assembly: UsesFeature("android.hardware.camera", Required = true)]
  [assembly: UsesFeature("android.hardware.camera.autofocus", Required = true)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
  <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
  <uses-permission android:name="android.permission.CAMERA" />
  ```

  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the appropriate permissions. This will automatically update the _AndroidManifest.xml_ file.

# [iOS](#tab/ios)

In your _Info.plist_ file, add the following keys:

```xml
<key>NSCameraUsageDescription</key>
<string>This app needs access to the camera to take photos.</string>
<key>NSMicrophoneUsageDescription</key>
<string>This app needs access to microphone for taking videos.</string>
<key>NSPhotoLibraryAddUsageDescription</key>
<string>This app needs access to the photo gallery for picking photos and videos.</string>
<key>NSPhotoLibraryUsageDescription</key>
<string>This app needs access to photos gallery for picking photos and videos.</string>
```

Each `<string>` element represents the reason the app is requesting access to that specific permission. This text is shown to the user.

# [Windows](#tab/windows)

In the `Package.appxmanifest` under **Capabilities** ensure that `Microphone` and `Webcam` capabilities are checked.

-----
<!-- markdownlint-enable MD025 -->

## Using Media Picker

The `MediaPicker` class has the following methods that all return a `FileResult` that can be used to get the files location or read it as a `Stream`.

- `PickPhotoAsync`\
Opens the media browser to select a photo.

- `CapturePhotoAsync`\
Opens the camera to take a photo.

- `PickVideoAsync`\
Opens the media browser to select a video.

- `CaptureVideoAsync`\
Opens the camera to take a video.

Each method optionally takes in a `MediaPickerOptions` parameter type that allows the `Title` to be set on some operating systems, which is displayed to the user.

> [!IMPORTANT]
> All methods must be called on the UI thread because permission checks and requests are automatically handled by .NET MAUI.

## Take a photo

The following code sample uses the media picker to take a photo, and then calls the `LoadPhotoAsync` method to save the file:

```csharp
async Task TakePhotoAsync()
{
    try
    {
        FileResult photo = await MediaPicker.CapturePhotoAsync();

        string filePath = await LoadPhotoAsync(photo);
        Console.WriteLine($"CapturePhotoAsync COMPLETED: { filePath }");
    }
    catch (FeatureNotSupportedException fnsEx)
    {
        // Feature is not supported on the device
    }
    catch (PermissionException pEx)
    {
        // Permissions not granted
    }
    catch (Exception ex)
    {
        Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
    }
}

async Task<string> LoadPhotoAsync(FileResult photo)
{
    // canceled
    if (photo == null)
        return string.Empty;

    // save the file into local storage
    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

    using Stream sourceStream = await photo.OpenReadAsync();
    using FileStream localFileStream = File.OpenWrite(localFilePath);

    await sourceStream.CopyToAsync(localFileStream);

    return localFilePath;
}
```

[!INCLUDE [tip-file-result](includes/tip-file-result.md)]

## API

- [MediaPicker source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MediaPicker)
<!-- - [MediaPicker API documentation](xref:Microsoft.Maui.Essentials.MediaPicker)-->
