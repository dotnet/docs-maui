---
title: "Flashlight"
description: "Learn how to use the .NET MAUI IFlashlight interface in the Microsoft.Maui.Devices namespace. This interface provides the ability to turn on or off the device's camera flash, to emulate a flashlight."
ms.date: 10/03/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Flashlight

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.IFlashlight> interface. With this interface, you can toggle the device's camera flash on and off, to emulate a flashlight.

The default implementation of the `IFlashlight` interface is available through the <xref:Microsoft.Maui.Devices.Flashlight.Default?displayProperty=nameWithType> property. Both the `IFlashlight` interface and `Flashlight` class are contained in the `Microsoft.Maui.Devices` namespace.

## Get started

To access the flashlight functionality the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

There are two permissions to configure in your project: `Flashlight` and `Camera`. These permissions can be set in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]
  [assembly: UsesPermission(Android.Manifest.Permission.Camera)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.FLASHLIGHT" />
  <uses-permission android:name="android.permission.CAMERA" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the **FLASHLIGHT** and **CAMERA** permissions. This will automatically update the *AndroidManifest.xml* file.

If you set these permissions, [Google Play will automatically filter out devices](https://developer.android.com/guide/topics/manifest/uses-feature-element.html#permissions-features) without specific hardware. You can get around this filtering by adding the following assembly attributes to the _Platforms/Android/MainApplication.cs_ file after `using` directives:

```csharp
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
```

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Use Flashlight

The flashlight can be turned on and off through the <xref:Microsoft.Maui.Devices.IFlashlight.TurnOnAsync> and <xref:Microsoft.Maui.Devices.IFlashlight.TurnOffAsync> methods. The following code example ties the flashlight's on or off state to a <xref:Microsoft.Maui.Controls.Switch> control:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="flashlight":::

::: moniker range=">=net-maui-8.0"

In addition, the <xref:Microsoft.Maui.Devices.Flashlight.IsSupportedAsync%2A> method can be invoked to check if a flashlight is available on the device, prior to calling the <xref:Microsoft.Maui.Devices.IFlashlight.TurnOnAsync> method.

::: moniker-end

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

### [iOS/Mac Catalyst](#tab/macios)

The `AVCaptureDevice` API is used to turn on and off the Torch and Flash mode of the device.

### [Windows](#tab/windows)

The <xref:Windows.Devices.Lights.Lamp> API is used to turn on or off the first detected lamp on the back of the device.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
