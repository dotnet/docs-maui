---
title: "Deployment"
description: "Learn how to deploy and publish .NET MAUI apps to Android, iOS, macOS, and Windows."
ms.date: 09/22/2022
---

# Deployment

.NET Multi-platform App UI (.NET MAUI) uses a single-project system to manage the configuration of your cross-platform app. This configuration includes properties and settings that control building and packaging the app on each platform. For more information, see [Project configuration for .NET MAUI apps](visual-studio-properties.md).

## Android

You can debug and test your apps on the Android Emulator, which can be run in a variety of configurations to simulate different devices. Each configuration is called a *virtual device*. When you deploy and test your apps on the emulator, you select a pre-configured or custom virtual device that simulates a physical Android device such as a Pixel phone. For more information, see [Debug on the Android Emulator](~/android/emulator/debug-on-emulator.md).

While the Android emulator enables you to rapidly develop and test your apps, you'll also want to test your apps on a real Android device. To run on a device, you'll need to enable developer mode on the device and connect it to your development machine. For more information, see [Set up Android device for debugging](~/android/device/setup.md).

When distributing a .NET MAUI app for Android, you generate an *apk* (Android Package) or an *aab* (Android App Bundle) file. The *apk* is used for installing your app to an Android device, and the *aab* is used to publish your app to an Android store. With just a few configuration changes to your project, your app can be packaged for distribution. For more information, see [Publish a .NET MAUI app for Android](~/android/deployment/overview.md).

## iOS

Building native iOS apps using .NET MAUI requires access to Apple's build tools, which only run on a Mac. Because of this, Visual Studio 2022 must connect to a network-accessible Mac to build .NET MAUI iOS apps. Visual Studio 2022's Pair to Mac feature discovers, connects to, authenticates with, and remembers Mac build hosts so that you can work productively on Windows. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

When combined with Pair to Mac, the remote iOS Simulator for Windows allows you to test your apps on an iOS simulator displayed in Windows alongside Visual Studio 2022. For more information, see [Remote iOS Simulator for Windows](~/ios/remote-simulator.md).

Alternatively, hot restart enables you to quickly deploy iOS apps to a 64-bit local device, from Visual Studio 2022, without requiring a Mac build host. It also removes the need for a full package rebuild by pushing new changes to the existing app bundle that's already present on your locally connected iOS device. It supports changes to code files, resources, and project references, enabling you to quickly test changes to your apps during their development. For more information, see [Deploy an iOS app to a local device using hot restart](hot-restart.md).

When distributing a .NET MAUI app for iOS, you generate an *.ipa* file. An *.ipa* file is an iOS app archive file that stores an iOS app. Distributing a .NET MAUI app on iOS requires that the app is provisioned using a provisioning profile. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism. For more information about provisioning, see [Provision an iOS app for app store distribution](~/ios/deployment/provision.md). For more information about publishing an .NET MAUI app for iOS, see [Publish a .NET MAUI app for iOS](~/ios/deployment/overview.md).

## macOS

When distributing your .NET MAUI app for macOS, you generate an *.app* or a *.pkg* file. An *.app* file is a self-contained app that can be run without installation, whereas a *.pkg* is an app packaged in an installer. For more information, see [Publish a .NET MAUI app for macOS](~/macos/deployment/overview.md).

## Windows

You can test and debug your apps on a local Windows machine, which requires you to enable Developer Mode. For more information, see [Deploy and debug your .NET MAUI app on Windows](~/windows/setup.md).

When distributing a .NET MAUI app for Windows, you can publish the app and its dependencies to a folder for deployment to another system. You can also package the app into an MSIX package, which has numerous benefits for the users installing your app. For more information, see [Publish a .NET MAUI app for Windows](~/windows/deployment/overview.md).
