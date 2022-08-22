---
title: "Deployment"
description: "Learn how to deploy and publish .NET MAUI apps to Android, iOS, macOS, and Windows."
ms.date: 08/19/2022
---

# Deployment

.NET Multi-platform App UI (.NET MAUI) uses a single-project system to manage the configuration of your cross-platform app. This configuration includes properties and settings that control building and packaging the app on each platform. For more information, see [Project configuration for .NET MAUI apps](visual-studio-properties.md).

## Publishing

When distributing your .NET MAUI app for Android, you generate an *apk* (Android Package) or an *aab* (Android App Bundle) file. The *apk* is used for installing your app to an Android device, and the *aab* is used to publish your app to an Android store. With just a few configuration changes to your project, your app can be packaged for distribution. For more information, see [Publish a .NET MAUI app for Android](~/android/deployment/overview.md).

When distributing your .NET MAUI app for iOS, you generate an *.ipa* file. An *.ipa* file is an iOS app archive file that stores an iOS app. Distributing a .NET MAUI app on iOS requires that the app is provisioned using a provisioning profile. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism. For more information about provisioning, see [Provision an iOS app for app store distribution](~/ios/deployment/provision.md). For more information about publishing an .NET MAUI app for iOS, see [Publish a .NET MAUI app for iOS](~/ios/deployment/overview.md).

When distributing your .NET MAUI app for macOS, you generate an *.app* or a *.pkg* file. An *.app* file is a self-contained app that can be run without installation, whereas a *.pkg* is an app packaged in an installer. For more information, see [Publish a .NET MAUI app for macOS](~/macos/deployment/overview.md).

When distributing your .NET MAUI app for Windows, you can publish the app and its dependencies to a folder for deployment to another system. You can also package the app into an MSIX package, which has numerous benefits for the users installing your app. For more information, see [Publish a .NET MAUI app for Windows](~/windows/deployment/overview.md).
