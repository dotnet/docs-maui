---
title: "Media picker"
description: "Learn how to use the MediaPicker class in the Microsoft.Maui.Media namespace, to prompt the user to select or take a photo or video."
ms.date: 05/23/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "MediaPicker"]
---

# Media picker

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IMediaPicker` interface. This interface lets a user pick or take a photo or video on the device. The `IMediaPicker` interface is exposed through the `MediaPicker.Default` property.

The `MediaPicker` and `IMediaPicker` types are available in the `Microsoft.Maui.Media` namespace.

## Get started

To access the media picker functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `CAMERA`, `WRITE_EXTERNAL_STORAGE`, `READ_EXTERNAL_STORAGE` permissions are required, and must be configured in the Android project. These can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  :::code language="csharp" source="../snippets/shared_1/Platforms/Android/MainApplication.cs" id="media_picker":::

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
  <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
  <uses-permission android:name="android.permission.CAMERA" />
  ```
<!-- NOT SUPPORTED
  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the appropriate permissions. This will automatically update the _AndroidManifest.xml_ file.
-->

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with queries that use Android's [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes the `manifest` node:

```xml
<queries>
  <intent>
    <action android:name="android.media.action.IMAGE_CAPTURE" />
  </intent>
</queries>
```

# [iOS\macOS](#tab/ios)

In the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files, add the following keys and values:

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

<!-- NOT SUPPORTED>
In the `Package.appxmanifest` under **Capabilities** ensure that `Microphone` and `Webcam` capabilities are checked.
-->

In the **Solution Explorer** pane, right-click on the _Platforms/Windows/Package.appxmanifest_ file, and select **View Code**. Under the `<Capabilities>` node, add `<DeviceCapability Name="microphone"/>` and `<DeviceCapability Name="webcam"/>` elements.

-----
<!-- markdownlint-enable MD025 -->

## Using media picker

The `IMediaPicker` interface has the following methods that all return a `FileResult`, which can be used to get the file's location or read it.

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

Call the `CapturePhotoAsync` method to open the camera and let the user take a photo. If the user takes a photo, the return value of the method will be a non-null value. The following code sample uses the media picker to take a photo and save it to the cache directory:

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="photo_take_and_save":::

[!INCLUDE [tip-file-result](../includes/tip-file-result.md)]
