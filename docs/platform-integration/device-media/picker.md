---
title: "Media picker for photos and videos"
description: "Learn how to use the IMediaPicker interface in the Microsoft.Maui.Media namespace, to prompt the user to select or take a photo or video"
ms.date: 04/18/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "MediaPicker"]
---

# Media picker for photos and videos

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Media.IMediaPicker> interface. This interface lets a user pick or take a photo or video on the device.

The default implementation of the `IMediaPicker` interface is available through the <xref:Microsoft.Maui.Media.MediaPicker.Default?displayProperty=nameWithType> property. Both the `IMediaPicker` interface and `MediaPicker` class are contained in the `Microsoft.Maui.Media` namespace.

## Get started

To access the media picker functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `CAMERA` permission is required and must be configured in the Android project. In addition:

- If your app targets Android 12 or lower, you must request the `READ_EXTERNAL_STORAGE` and `WRITE_EXTERNAL_STORAGE` permissions.
- If your app targets Android 13 or higher and needs access to media files that other apps have created, you must request one or more of the following granular media permissions instead of the `READ_EXTERNAL_STORAGE` permission:

  - `READ_MEDIA_IMAGES`
  - `READ_MEDIA_VIDEO`
  - `READ_MEDIA_AUDIO`

These permissions can be added in the following ways:

- Add the assembly-based permissions:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  :::code language="csharp" source="../snippets/shared_1/Platforms/Android/MainApplication.cs" id="media_picker":::

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.CAMERA" />
  <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" android:maxSdkVersion="32" />
  <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" android:maxSdkVersion="32" />    
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

If your project's Target Android version is set to **Android 11 (R API 30)** or higher, you must update your _Android Manifest_ with queries that use Android's [package visibility requirements](https://developer.android.com/preview/privacy/package-visibility).

In the _Platforms/Android/AndroidManifest.xml_ file, add the following `queries/intent` nodes in the `manifest` node:

```xml
<queries>
  <intent>
    <action android:name="android.media.action.IMAGE_CAPTURE" />
  </intent>
</queries>
```

# [iOS/Mac Catalyst](#tab/macios)

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

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Using media picker

The <xref:Microsoft.Maui.Media.IMediaPicker> interface has the following methods that all return a <xref:Microsoft.Maui.Storage.FileResult>, which can be used to get the file's location or read it.

- <xref:Microsoft.Maui.Media.MediaPicker.PickPhotoAsync%2A>\
Opens the media browser to select a photo.

- <xref:Microsoft.Maui.Media.MediaPicker.CapturePhotoAsync%2A>\
Opens the camera to take a photo.

- <xref:Microsoft.Maui.Media.MediaPicker.PickVideoAsync%2A>\
Opens the media browser to select a video.

- <xref:Microsoft.Maui.Media.MediaPicker.CaptureVideoAsync%2A>\
Opens the camera to take a video.

Each method optionally takes in a <xref:Microsoft.Maui.Media.MediaPickerOptions> parameter type that allows the <xref:Microsoft.Maui.Media.MediaPickerOptions.Title> to be set on some operating systems, which is displayed to the user.

> [!IMPORTANT]
> All methods must be called on the UI thread because permission checks and requests are automatically handled by .NET MAUI.

## Take a photo

Call the <xref:Microsoft.Maui.Media.IMediaPicker.CapturePhotoAsync%2A> method to open the camera and let the user take a photo. If the user takes a photo, the return value of the method will be a non-null value. The following code sample uses the media picker to take a photo and save it to the cache directory:

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="photo_take_and_save":::

[!INCLUDE [tip-file-result](../includes/tip-file-result.md)]
