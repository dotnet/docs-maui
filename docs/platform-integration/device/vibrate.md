---
title: "Vibration"
description: "Learn how to use the .NET MAUI Vibration class, which lets you start and stop the vibrate functionality for a desired amount of time."
ms.date: 05/23/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Devices"]
---

# Vibration

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `Vibration` class. This class lets you start and stop the vibrate functionality for a desired amount of time.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `HapticFeedback` class is available in the `Microsoft.Maui.Devices` namespace.

## Get started

To access the Vibration functionality, the following platform specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

The `VIBRATE` permission is required, and must be configured in the Android project. This can be added in the following ways:

- Add the assembly-based permission:

  Open the _AssemblyInfo.cs_ file under the **Properties** folder and add:

  ```csharp
  [assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
  ```

  \- or -

- Update the Android Manifest:

  Open the _AndroidManifest.xml_ file under the **Properties** folder and add the following in the `manifest` node:

  ```xml
  <uses-permission android:name="android.permission.VIBRATE" />
  ```

<!-- TODO not yet supported
  \- or -

- Use the Android project properties:

  Right-click on the Android project and open the project's properties. Under _Android Manifest_ find the **Required permissions:** area and check the appropriate permissions. This will automatically update the _AndroidManifest.xml_ file.
-->

# [iOS](#tab/ios)

No additional setup required.

# [Windows](#tab/windows)

No additional setup required.

-----

## Vibrate the device

Haptic feedback is accessed through default implementation of the `IVibration` interface, available from the `Microsoft.Maui.Devices.Vibration.Default` property. The vibration functionality can be requested for a set amount of time or the default of 500 milliseconds. The following code example randomly vibrates the device between one and seven seconds:

:::code language="csharp" source="../snippets/shared_1/DeviceDetailsPage.xaml.cs" id="vibrate":::

## Platform differences

This section describes the platform-specific differences with the vibration API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
### [Android](#tab/android)

No platform differences.

# [iOS](#tab/ios)

- Only vibrates when device is set to "Vibrate on ring".
- Always vibrates for 500 milliseconds.
- Not possible to cancel vibration.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
