---
title: "Flashlight"
description: "Learn how to use the .NET MAUI Flashlight class in the Microsoft.Maui.Essentials namespace. This class provides the ability to turn on or off the device's camera flash, to emulate a flashlight."
ms.date: 08/19/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Flashlight

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Flashlight` class. With this class, you can toggle the device's camera flash on and off, to emulate a flashlight.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

To access the flashlight functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

There are two permissions to configure in your project: `Flashlight` and `Camera`. These permissions can be set in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]
  [assembly: UsesPermission(Android.Manifest.Permission.Camera)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.FLASHLIGHT" />
  <uses-permission android:name="android.permission.CAMERA" />
  ```

  \- or -

- Use the Android project properties:

  <!-- TODO: Check on this value -->
  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **FLASHLIGHT** and **CAMERA** permissions. This will automatically update the _AndroidManifest.xml_ file.

By adding these permissions, [Google Play will automatically filter out devices](https://developer.android.com/guide/topics/manifest/uses-feature-element.html#permissions-features) without specific hardware. You can get around this by adding the following to your _AssemblyInfo.cs_ file in your Android project:

```csharp
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
```

[!INCLUDE [android-permissions](includes/android-permissions.md)]

# [iOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Use Flashlight

The flashlight can be turned on and off through the `TurnOnAsync` and `TurnOffAsync` methods:

```csharp
try
{
    // Turn on
    await Flashlight.TurnOnAsync();

    // Pause for 3 seconds
    await Task.Delay(TimeSpan.FromSeconds(3));

    // Turn off
    await Flashlight.TurnOffAsync();
}
catch (FeatureNotSupportedException fnsEx)
{
    // Handle not supported on device exception
}
catch (PermissionException pEx)
{
    // Handle permission exception
}
catch (Exception ex)
{
    // Unable to turn on/off flashlight
}
```

## Platform differences

This section describes the platform-specific differences with the flashlight.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
### [Android](#tab/android)

The `Flashlight` class has been optimized based on the device's operating system.

#### API level 23 and higher

On newer API levels, [Torch Mode](https://developer.android.com/reference/android/hardware/camera2/CameraManager.html#setTorchMode) will be used to turn on or off the flash unit of the device.

#### API level 22 and lower

A camera surface texture is created to turn on or off the `FlashMode` of the camera unit.

### [iOS](#tab/ios)

The `AVCaptureDevice` API is used to turn on and off the Torch and Flash mode of the device.

### [Windows](#tab/windows)

The <xref:Windows.Devices.Lights.Lamp> API is used to turn on or off the first detected lamp on the back of the device.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Flashlight source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Flashlight)
<!-- - [Flashlight API documentation](xref:Microsoft.Maui.Essentials.Flashlight)-->
