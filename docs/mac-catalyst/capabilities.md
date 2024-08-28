---
title: "Mac Catalyst capabilities"
description: "Learn how to add capabilities to your .NET MAUI Mac Catalyst app's provisioning profile, to code sign your app."
ms.date: 08/28/2024
---

# Mac Catalyst capabilities

On Mac Catalyst .NET Multi-platform App UI (.NET MAUI) apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. Apple provides *capabilities*, also known as *app services*, as a means of extending functionality and widening the scope of what Mac Catalyst apps can do. For more information about capabilities, see [Capabilities](https://developer.apple.com/documentation/xcode/capabilities) on developer.apple.com.

Capabilities are added to your app's provisioning profile, and are used when code signing your app. The provisioning profile must contain an App ID, that matches your app's bundle identifier, with the required capabilities enabled. The provisioning profile should be created in your Apple Developer Account.

Capabilities are closely related to the concept of entitlements. They both request the expansion of the sandbox your app runs in, to give it additional capabilities. Entitlements are typically added when developing your app, while capabilities are typically added when code signing your app for distribution. For more information about entitlements, see [Entitlements](entitlements.md).

## Add capabilities in your Apple Developer Account

Capabilities can be added to your app's provisioning profile in your Apple Developer Account. This is a multi-step process that requires creating a signing certificate, App ID, and provisioning profile.

When adding a new capability to your provisioning profile, you should also add the same capability to your app's *Entitlements.plist* file and ensure that the app consumes this file. For more information, see [Entitlements](entitlements.md). It may also be necessary to set privacy keys in *Info.plist*, for certain capabilities.

## Create a signing certificate

Creating a provisioning profile requires you to have previously created a signing certificate. The type of signing certificate depends on the intended distribution channel for your app:

- If you intend to distribute your app via the Mac App Store, see [Create a distribution certificate](~/mac-catalyst/deployment/publish-app-store.md#create-a-distribution-certificate) in [Publish a Mac Catalyst app for Mac App Store distribution](~/mac-catalyst/deployment/publish-app-store.md).
- If you intend to distribute your app outside the Mac App Store, see [Create a developer ID application certificate](~/mac-catalyst/deployment/publish-outside-app-store.md#create-a-developer-id-application-certificate) in [Publish a Mac Catalyst app for distribution outside the Mac App Store](~/mac-catalyst/deployment/publish-outside-app-store.md).
- If you intend to distribute your app to a limited number of users on registered devices, outside the Mac App Store, see [Create a development certificate](~/mac-catalyst/deployment/publish-ad-hoc.md#create-a-development-certificate) in [Publish a Mac Catalyst app for ad-hoc distribution](~/mac-catalyst/deployment/publish-ad-hoc.md).

[!INCLUDE [Create an App ID](../macios/includes/capabilities-create-app-id.md)]

## Create a provisioning profile

Once you've created an App ID you must create a provisioning profile for the App ID. The provisioning profile you create will also depend on the intended distribution channel for your app:

- If you intend to distribute your app via the Mac App Store, see [Create a provisioning profile](~/mac-catalyst/deployment/publish-app-store.md#create-a-provisioning-profile) in [Publish a Mac Catalyst app for Mac App Store distribution](~/mac-catalyst/deployment/publish-app-store.md).
- If you intend to distribute your app outside the Mac App Store, see [Create a provisioning profile](~/mac-catalyst/deployment/publish-outside-app-store.md#create-a-provisioning-profile) in [Publish a Mac Catalyst app for distribution outside the Mac App Store](~/mac-catalyst/deployment/publish-outside-app-store.md).
- If you intend to distribute your app to a limited number of users on registered devices, outside the Mac App Store, see [Create a provisioning profile](~/mac-catalyst/deployment/publish-ad-hoc.md#create-a-provisioning-profile) in [Publish a Mac Catalyst app for ad-hoc distribution](~/mac-catalyst/deployment/publish-ad-hoc.md).

## Download provisioning profiles

[!INCLUDE [Download provisioning profile in Xcode](includes/download-profiles.md)]

## Define build properties in your project file

The project file for your app should be updated to use the signing certificate, provisioning profile, and entitlements file. This can be achieved by adding the following build properties to a `<PropertyGroup>` in your project file:

| Property                    | Value                                                                                           |
|-----------------------------|-------------------------------------------------------------------------------------------------|
| `<CodesignKey>`             | The name of the code signing key. Set to the name of your distribution certificate, as displayed in Keychain Access. |
| `<CodesignEntitlements>`    | The path to the entitlements file that specifies the entitlements the app requires. Set to `Platforms\MacCatalyst\Entitlements.plist`. |
| `<CodesignProvision>`       | The provisioning profile to use when signing the app bundle. |
| `<EnableCodeSigning>`       | Set to `true` so that code signing is enabled.                                                  |

The following example shows a typical property group for building and signing your Mac Catalyst app for Mac App Store distribution:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Release|net8.0-maccatalyst|AnyCPU'">
  <EnableCodeSigning>True</EnableCodeSigning>
  <CodesignKey>Apple Distribution: John Smith (AY2GDE9QM7)</CodesignKey>
  <CodesignProvision>MyMauiApp</CodesignProvision>
  <CodesignEntitlements>Platforms\MacCatalyst\Entitlements.plist</CodesignEntitlements>
</PropertyGroup>
```

## Troubleshoot

The following list details the common issues that can cause issues when developing a .NET MAUI Mac Catalyst app that uses capabilities:

[!INCLUDE [Troubleshooting capabilities](../macios/includes/troubleshooting-capabilities.md)]
