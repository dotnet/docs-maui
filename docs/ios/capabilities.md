---
title: "Capabilities"
description: "Learn how to use capabilities in a .NET MAUI iOS app to XXXX."
ms.date: 01/10/2022
---

# Capabilities

In iOS, apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. Apple provides *capabilities*, also known as *app services*, as a means of extending functionality and widening the scope of what iOS apps can do. Capabilities allow you to add a deeper integration with platform features to your app, such as integration with Siri. For more information about capabilities, see [Capabilities](https://developer.apple.com/documentation/xcode/capabilities).

To use capabilities, the app must have a valid provisioning profile that contains an App ID with the required service enabled. The provisioning profile can either be created automatically in Visual Studio or Visual Studio for Mac, or manually in the Apple developer portal.


## VS/VSMac

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE

---

## Apple developer portal

Adding an app service using Apple's developer portal is a two step process that requires creating an App ID and then using that App ID to create a provisioning profile.

### Create an App ID with an app service



### Create a provisioning profile


Once a capability has been enabled in the Apple developer portal, you should also add the required entitlements to your app. For more information, see [Entitlements](entitlements.md).

## Troubleshoot

The following list details the common issues that can cause issues when developing a .NET MAUI iOS app with an app service enabled:

- Ensure that the correct ID has been created and registered in the **Certificates, IDs & Profiles** section of Apple's developer portal.
- Ensure that the service has been added to the app's ID and that the service is configured using the correct strings.
- Ensure that the provisioning profiles and App IDs have been installed on your development machine and that the app's *Info.plist* file is using the correct App ID.
- Ensure that the app's *Entitlements.plist* file has the correct services enabled.
- Ensure that the appropriate privacy-keys are set in *Info.plist*.
- Ensure that the app consumes the *Entitlements.plist* file. For more information, see [Consume entitlements](entitlements.md#consume-entitlements).

---

https://learn.microsoft.com/en-us/xamarin/ios/deploy-test/provisioning/capabilities/?tabs=macos
https://developer.apple.com/documentation/xcode/adding-capabilities-to-your-app

## Next Steps

Once a Capability has been enabled on the server side, there is still work that needs to be done to allow your app to use the functionality. The list below describes additional steps that may need to be taken:

- Use the framework namespace in your app.
- Add the required entitlements to your App. Information on the entitlements required and how to add them is detailed in the Introduction to Entitlements guide.
