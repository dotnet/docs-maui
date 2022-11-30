---
title: "Device provisioning for .NET MAUI apps on iOS"
description: "Learn how to provision an iOS device so that it can be used to test a .NET MAUI iOS app."
ms.date: 11/28/2022
---

# Device provisioning for iOS

While developing a .NET Multi-platform App UI (.NET MAUI) app it's essential to test it by deploying the app to a physical device, in addition to the simulator. Device-only bugs and performance issues can transpire when running on a device, due to hardware limitations such as memory or network connectivity. To test an app on a physical device, the device must be *provisioned*, and Apple must be informed that the device will be used for testing.

Before deploying an app to a device, you need to have an active subscription to Apple's Developer Program. Apple offers two program options:

- **Apple Developer Program**. Regardless of whether you are an individual or represent an organization, the [Apple Developer Program](https://developer.apple.com/programs/) enables you to develop, test, and distribute apps.
- **Apple Developer Enterprise Program**, which is most suited to organizations that want to develop and distribute apps in-house only. Members of the [Apple Developer Enterprise Program](https://developer.apple.com/programs/enterprise/) do not have access to iTunes Connect, and apps created cannot be published to the App Store.

To register for either of these programs, you must first have an [Apple ID](https://appleid.apple.com/). Then you can visit the [Apple Developer Program](https://developer.apple.com/programs/enroll/) to register.

Any app that runs on a device needs to include a thumbprint that contains information about the app and the developer, and iOS uses this thumbprint to make sure that the app hasn't been tampered with. This is achieved by requiring app developers to register their Apple ID as a developer, and to setup an App ID, request a certificate, and register the device on which the app will be deployed during the development process.

When deploying an app to a device, a provisioning profile is also installed on the iOS device. The provisioning profile exists to verify the information that the app was signed with at build time and is cryptographically signed by Apple. Together, the provisioning profile and thumbprint checks determine if an app can be deployed to a device by checking the following:

- Certificate – has the app been signed with a private key, which has a corresponding public key in the provisioning profile? The certificate also associates the developer with a development team.
- App ID – does the bundle identifier set in the app's **Info.plist** match the App ID in the provisioning profile?
- Device – is the device contained in the provisioning profile?

These checks ensure that everything that is created or used during the development process, including the app and device, can be traced back to an Apple Developer account.

## Provisioning your device

There are two approaches to provisioning your iOS device:

- **Automatically**. Signing identities, app IDs, and provisioning profiles will be automatically created and managed by Visual Studio. This is the recommended approach for provisioning an iOS device. For more information, see [Automatic provisioning](automatic-provisioning.md).
- **Manually**. Signing identities, app IDs, and provisioning profiles will be created and managed in the [Apple Developer portal](https://developer.apple.com/account). For more information, see [Manual provisioning](manual-provisioning.md).

## Provisioning for application services

Apple provides a selection of application services, known as capabilities, that can be activated for a .NET MAUI iOS app. These capabilities must be configured in both your provisioning profile, when the **App ID** is created, and in the **Entitlements.plist** file that should be added to the .NET MAUI app project. For more information about capabilities, see [Entitlements and capabilities](~/ios/deployment/entitlements.md), and [Capabilites]((https://developer.apple.com/documentation/xcode/capabilities) on developer.apple.com.
