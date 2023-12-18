---
title: "Android emulator"
description: "Learn how to get started with the Android emulator, so that you can test your apps on a variety of devices and Android API levels without requiring physical devices."
ms.date: 12/18/2023
---

# Android emulator

The Android emulator, which is provided by Google, simulates Android devices on your computer so that you can test your apps on a variety of devices and Android API levels, without needing to have each physical device. Different devices are simulated via Android Virtual Devices (AVDs). When you deploy and test your app on the emulator, you select a pre-configured or custom virtual device that simulates a physical Android device such as a Pixel phone.

Alternatively, you can deploy your app to a physical device. For more information, see [Set up Android device for debugging](~/android/device/setup.md).

## Get started

The Android emulator will be installed by Visual Studio, and an AVD created, when you build your first .NET Multi-platform App UI (.NET MAUI) app for Android. For more information, see [Build your first app](~/get-started/first-app.md?pivots=devices-android).

If hardware acceleration isn't available or enabled, the emulator will run very slowly. You can significantly improve the performance of the emulator by enabling hardware acceleration and virtual device images that are appropriate for your processor architecture. For more information, see [How to enable hardware acceleration with Android emulators](hardware-accelerationl.md).

Physical Android devices are emulated by AVDs, which specify the Android version and hardware characteristics of the simulated device. To test an app, create AVDs that model each device your app is designed to run on. Each AVD function runs as an independent device with its own storage for user data, and SD card data. The emulator stores the user data, SD card data, and cache in a folder specific to the AVD. When you launch the emulator, it loads the user data and SD card data from the AVD folder. For information about managing virtual devices, see [Managing virtual devices with the Android Device Manager](device-manager.md). For information about editing the configuration of a virtual device, see [Editing Android virtual device properties](device-properties.md).

After you've created a AVD you can launch the emulator and debug an app in it. For more information, see [Debug on the Android emulator](debug-on-emulator.md). Once an app is installed on an AVD, you can run it from the device as you would run any app on a device.

For information about diagnosing emulator issues, see [Android emulator troubleshooting](troubleshooting.md).

## Use the emulator

When the Android emulator is running, you use your computer mouse pointer to mimic your finger on the touch screen, select menu items and input fields, and click buttons and controls. You can type in the emulator by using your computer keyboard or by using the onscreen keyboard that appears in the emulator.

The following table shows how to simulate gestures in the emulator:

| Gesture | Description |
| ------- | ----------- |
| Tap | Press and release the primary mouse button. |
| Double tap | Double-click and then release the primary mouse button. |
| Drag | Press and hold the primary mouse button, move the item, and then release. |
| Touch and hold | Press the primary mouse button, hold, and release. |
| Pinch | Press the CTRL key on Windows, or the Command key on macOS, to display a pinch gesture multi-touch interface. The mouse acts as the first finger, which can be moved by dragging the cursor. The second finger is across the anchor point. Pressing the left mouse button simulates touching both points, and releasing simulates removing both points. |
| Swipe | Press and hold the primary mouse button, swipe, and then release. |
| Vertical swipe | Open a vertical menu on the screen and use the mouse scroll wheel to scroll through the menu items. |

Common actions can be performed in the emulator via its panel on the right hand side. The following table lists common emulator actions:

| Action | Description |
| ------ | ----------- |
| Close | Close the emulator. |
| Minimize | Minimize the emulator window. |
| Resize | Resize the emulator. |
| Power | Click to turn the screen on/off. Click and hold to turn the device on/off. |
| Volume up | Increase the volume. |
| Volume down | Decrease the volume. |
| Rotate left | Rotate the screen 90 degrees left. |
| Rotate right | Rotate the screen 90 degrees right. |
| Take screenshot | Click to take a screenshot of the device. |
| Enter zoom mode | Click to change the cursor to the zoom icon. Click again to exit zoom mode. |
| Back | Return to the previous screen, or close an options menu, dialog, onscreen keyboard, or Notifications panel. |
| Home | Return to the Home screen. |
| Overview (recent apps) | Click to open a list of apps you've worked with recently. To open an app, tap its thumbnail.  |
| Menu | Press CTRL+M on Windows, or COMMAND+M on macOS, to simulate the Menu button. |
| More | Click for more features and settings. |

## Advanced emulator scenarios

The following table lists where to find more information about advanced Android emulator scenarios:

| Scenarios | More information |
| ------- | ---------------- |
| Use the camera | [Camera support](https://developer.android.com/studio/run/emulator-use-camera) on developer.android.com |
| Install and add files | [Install and add files](https://developer.android.com/studio/run/emulator-install-add-files) on developer.android.com |
| Use Wi-Fi | [Wi-Fi](https://developer.android.com/studio/run/emulator-wifi) on developer.android.com |
| Use extended controls | [Extended controls, settings, and help](https://developer.android.com/studio/run/emulator-extended-controls) on developer.android.com |
| Use snapshots | [Snapshots](https://developer.android.com/studio/run/emulator-snapshots) on developer.android.com |
| Use the emulator from the command line | [Start the emulator from the command line](https://developer.android.com/studio/run/emulator-commandline) on developer.android.com |
| Send console commands | [Send emulator console commands](https://developer.android.com/studio/run/emulator-console) on developer.android.com |
| Set up networking | [Set up Android Emulator networking](https://developer.android.com/studio/run/emulator-networking) on developer.android.com |
| Configure hardware acceleration | [Configure hardware acceleration for the Android Emulator](https://developer.android.com/studio/run/emulator-acceleration) on developer.android.com |
| Android emulator tools | [Comparison of Android Emulator tools](https://developer.android.com/studio/run/emulator-comparison) on developer.android.com |

## Limitations

The Android Emulator doesn't include virtual hardware for the following:

- Bluetooth
- NFC
- SD card insert/eject
- Device-attached headphones
- USB
