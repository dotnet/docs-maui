---
title: "Flashlight"
description: "Learn how to use the .NET MAUI IFlashlight interface in the Microsoft.Maui.Devices namespace. This interface provides the ability to turn on or off the device's camera flash, to emulate a flashlight."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Flashlight

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IFlashlight` interface. With this interface, you can toggle the device's camera flash on and off, to emulate a flashlight.

The default implementation of the `IFlashlight` interface is available through the `Flashlight.Default` property. Both the `IFlashlight` interface and `Flashlight` class are contained in the `Microsoft.Maui.Devices` namespace.

The `` class is available in the `Microsoft.Maui.Devices` namespace.

## Get started

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

<!-- TODO unsupported right now
  \- or -

- Use the Android project properties:

  <!-- TODO: Check on this value
  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the **FLASHLIGHT** and **CAMERA** permissions. This will automatically update the _AndroidManifest.xml_ file.

-->

By adding these permissions, [Google Play will automatically filter out devices](https://developer.android.com/guide/topics/manifest/uses-feature-element.html#permissions-features) without specific hardware. You can get around this by adding the following to your _AssemblyInfo.cs_ file in your Android project:

```csharp
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
```

# [iOS\macOS](#tab/ios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Use Flashlight

The flashlight can be turned on and off through the `TurnOnAsync` and `TurnOffAsync` methods. The following code example ties the flashlight's on or off state to a `Switch` control:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="flashlight":::

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

### [iOS\macOS](#tab/ios)

The `AVCaptureDevice` API is used to turn on and off the Torch and Flash mode of the device.

### [Windows](#tab/windows)

The <xref:Windows.Devices.Lights.Lamp> API is used to turn on or off the first detected lamp on the back of the device.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
