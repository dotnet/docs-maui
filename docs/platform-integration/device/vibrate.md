---
title: "Vibration"
description: "Learn how to use the .NET MAUI IVibration interface, which lets you start and stop the vibrate functionality for a desired amount of time."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Vibration

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Devices.IVibration> interface. This interface lets you start and stop the vibrate functionality for a desired amount of time.

The default implementation of the `IVibration` interface is available through the <xref:Microsoft.Maui.Devices.Vibration.Default?displayProperty=nameWithType> property. Both the `IVibration` interface and `Vibration` class are contained in the `Microsoft.Maui.Devices` namespace.

## Get started

To access the Vibration functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `VIBRATE` permission is required, and must be configured in the Android project. This permission can be added in the following ways:

- Add the assembly-based permission:

  Open the _Platforms/Android/MainApplication.cs_ file and add the following assembly attributes after `using` directives:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _Platforms/Android/AndroidManifest.xml_ file and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.VIBRATE" />
  ```

  \- or -

- Update the Android Manifest in the manifest editor:

  In Visual Studio double-click on the *Platforms/Android/AndroidManifest.xml* file to open the Android manifest editor. Then, under **Required permissions** check the **VIBRATE** permission. This will automatically update the *AndroidManifest.xml* file.

# [iOS/Mac Catalyst](#tab/macios)

No setup is required.

# [Windows](#tab/windows)

No setup is required.

-----

## Vibrate the device

The vibration functionality can be requested for a set amount of time or the default of 500 milliseconds. The following code example randomly vibrates the device between one and seven seconds using the <xref:Microsoft.Maui.Devices.IVibration.Vibrate(System.TimeSpan)>:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="vibrate":::

## Platform differences

This section describes the platform-specific differences with the vibration API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
### [Android](#tab/android)

No platform differences.

# [iOS/Mac Catalyst](#tab/macios)

- Only vibrates when device is set to "Vibrate on ring".
- Always vibrates for 500 milliseconds.
- Not possible to cancel vibration.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
