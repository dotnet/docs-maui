---
title: "Media Picker"
description: "The MediaPicker class in Microsoft.Maui.Essentials lets a user pick or take a photo or video on the device."
ms.date: 01/04/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Media Picker

The `MediaPicker` class lets a user pick or take a photo or video on the device.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

To access the **MediaPicker** functionality the following platform specific setup is required.

# [Android](#tab/android)

The following permissions are required and must be configured in the Android project. This can be added in the following ways:

Open the **AssemblyInfo.cs** file under the **Properties** folder and add:

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

OR Update Android Manifest:

Open the **AndroidManifest.xml** file under the **Properties** folder and add the following inside of the **manifest** node.

```xml
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.CAMERA" />
```

Or right click on the Android project and open the project's properties. Under **Android Manifest** find the **Required permissions:** area and check the these permissions. This will automatically update the **AndroidManifest.xml** file.

# [iOS](#tab/ios)

In your `Info.plist` add the following keys:

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

Ensure that you update the `<string>` in each to a description specific for your app as it will be shown to your users.

# [Windows](#tab/windows)

In the `Package.appxmanifest` under **Capabilities** ensure that `Microphone` and `Webcam` capabilities are checked.

-----

## Using Media Picker

The `MediaPicker` class has the following methods that all return a `FileResult` that can be used to get the files location or read it as a `Stream`.

* `PickPhotoAsync`: Opens the media browser to select a photo.
* `CapturePhotoAsync`: Opens the camera to take a photo.
* `PickVideoAsync`: Opens the media browser to select a video.
* `CaptureVideoAsync`: Opens the camera to take a video.

Each method optionally takes in a `MediaPickerOptions` parameter that allows the `Title` to be set on some operating systems that is displayed to the users.

> [!TIP]
> All methods must be called on the UI thread because permission checks and requests are automatically handled by Xamarin.Essentials.

## General Usage

```csharp
async Task TakePhotoAsync()
{
    try
    {
        var photo = await MediaPicker.CapturePhotoAsync();
        await LoadPhotoAsync(photo);
        Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
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

async Task LoadPhotoAsync(FileResult photo)
{
    // canceled
    if (photo == null)
    {
        PhotoPath = null;
        return;
    }
    // save the file into local storage
    var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
    using (var stream = await photo.OpenReadAsync())
    using (var newStream = File.OpenWrite(newFile))
        await stream.CopyToAsync(newStream);

    PhotoPath = newFile;
}
```
[!INCLUDE [tip-file-result](includes/tip-file-result.md)]

## API

- [MediaPicker source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MediaPicker)
<!-- - [MediaPicker API documentation](xref:Microsoft.Maui.Essentials.MediaPicker)-->
