---
title: "Android Emulator Setup"
description: "The Android Emulator can be run in a variety of configurations to simulate different devices. This article explains how to prepare the Android Emulator for testing your .NET MAUI app."
ms.date: 10/11/2021
---

# Android emulator setup

The Android Emulator can be run in a variety of configurations to simulate different devices. Each configuration is called a _virtual device_. When you deploy and test your app on the emulator, you select a pre-configured or custom virtual device that simulates a physical Android device such as a Nexus or Pixel phone.

The following articles are available to help manage and setup your Android emulator:

- [Hardware Acceleration for Emulator Performance](hardware-acceleration.md)

  How to prepare your computer for maximum Android Emulator performance by using either Hyper-V or HAXM virtualization technology. Because the Android Emulator can be prohibitively slow without hardware acceleration, we recommend that you enable hardware acceleration on your computer before you use the emulator.

- [Managing Virtual Devices with the Android Device Manager](device-manager.md)

  How to use the Android Device Manager to create and customize virtual devices.

- [Editing Android Virtual Device Properties](~/android/get-started/installation/android-emulator/device-properties.md)

  How to use the Android Device Manager to edit the profile properties of a virtual device.

- [Android Emulator Troubleshooting](~/android/get-started/installation/android-emulator/troubleshooting.md)

  In this article, the most common warning messages and issues that occur while running the Android Emulator are described, along with workarounds and tips.

After you have configured the Android Emulator, see [Debugging on the Android Emulator](~/android/deploy-test/debugging/debug-on-emulator.md) for information about how to launch the emulator and use it for testing and debugging your app.
