---
title: "Entitlements and capabilities"
description: "Learn how to use entitlements and capabilities in a .NET MAUI iOS app to request access to specific system resources or user data."
ms.date: 05/09/2022
---

# Entitlements and capabilities

In iOS, apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. *Entitlements* are used to request the expansion of the sandbox to give your app additional capabilities. Any entitlements used by your app must be specified in the app's entitlements file. For more information about entitlements, see [Entitlements](https://developer.apple.com/documentation/bundleresources/entitlements).

Apple provides *capabilities*, also known as *app services*, as a means of extending functionality and widening the scope of what iOS apps can do. Capabilities allow you to add a deeper integration with platform features to your app, such as integration with Siri. For more information about capabilities, see [Capabilities](https://developer.apple.com/documentation/xcode/capabilities).

To extend the capabilities of your app, an entitlement must be provided in your app's *Entitlements.plist* file. Entitlements are a key/value pair, and generally only one entitlement is required per capability. In addition to specifying entitlements, the *Entitlements.plist* file is used to sign the app.

> [!IMPORTANT]
> An *Entitlements.plist* file isn't linked to an Apple Developer Account. Therefore, any entitlements used by an app must also be specified when creating a provisioning profile for an app.

## Add an Entitlements.plist file

To add a new entitlements file to your .NET Multi-platform App UI (.NET MAUI) app project, add a new XML file named *Entitlements.plist* to the *Platforms\\iOS\\* folder of your app project. Then add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
</dict>
</plist>
```

## Set entitlements

Entitlements can be configured in Visual Studio by double-clicking the *Entitlements.plist* file to open it in the iOS Entitlements editor:

:::image type="content" source="media/entitlements/editor.png" alt-text="Visual Studio iOS entitlements editors.":::

When a .NET MAUI iOS app uses entitlements, the *Entitlements.plist* file must be referenced from the `<CodesignEntitlements>` node within a `<PropertyGroup>`. For more information, see [Add code signing data to your app project](overview.md#add-code-signing-data-to-your-app-project).

## Key reference

| Entitlement | Description | Key | Type | Example |
| ----------- | ----------- | --- | ---- | ------- |
| Access WiFi information | This entitlement enables your app to obtain information about the currently connected WiFi network. | `com.apple.developer.networking.wifi-info` | Boolean | `<key>com.apple.developer.networking.wifi-info</key>\<true/>`  |
| App Attest | With App Attest, you can generate a special cryptographic key on your device and use it to validate the integrity of your app before your server provides access to sensitive data. |   |   |   |
| App groups | App groups enables your app to access group containers shared among multiple related apps as well as perform inter-process communication between the apps. |   |   |   |
| Apple Pay  | Apple Pay enables users to easily and securely pay for physical good and services such as groceries, clothing, tickets, and reservations using payment information stored on their device. |   |   |   |
| Associated domains | Associated domains enables your app to be associated with specific domains for specific services, such as accessing Safari, saved passwords, and activity continuation. |   |   |   |
| AutoFill credential provider | AutoFill credential provider enables an app, with user permission, to provide user names and passwords for AutoFill into the app and Safari. |   |   |   |
| ClassKit | Enabling ClassKit allows your app to privately and securely share student progress with teachers on assigned activities, such as reading a chapter in a book or taking a quiz, in school-managed environments. |   |   |   |
| Communicates with drivers | Enables communication between an app and DriverKit drivers. |   |   |   |
| Communication notifications | Enables an app to send communication notifications from a person to a person or multiple people. |   |   |   |
| Data protection | Data protection enables your app to use the built-in encryption on supported devices. When you specify a file as protected, the system will store the file in an encrypted format. |   |   |   |
| Extended virtual addressing | Extended virtual addressing enables you to use more address space in your app. |   |   |   |
| Family controls | Family controls enable parental controls in your app, granting access to the Managed Settings and Device Activity frameworks in the ScreenTime API. Use of Family controls requires Family Sharing for user enrolment. It prevents removal of your app and enables on-device content filters from Network Extensions. |   |   |   |
| FileProvider testing mode | Enables a test mode that provides the File Provider extension more control over the system's behavior during testing. |   |   |   |
| Fonts | Enables your app, with user permission, to install and use custom fonts. |   |   |   |
| Group activities | Enables an app to communicate with the same app on one or more other devices, to create a group activity within a FaceTime call. Group activities on FaceTime let users watch video together, listen to music together, or perform another synchronous activity. |   |   |   |
| HealthKit | HealthKit enables your app to access, with user permission, personal health information. |   |   |   |
| HomeKit | HomeKit enables your app to interact with HomeKit accessories. |   |   |   |
| Hotspot configuration | Enabling Hotspot configuration allows your app to configure WiFi networks. |   |   |   |
| iCloud | iCloud enables your app to store data in the cloud, making it possible for users to share their data across multiple devices. |   |   |   |
| In-App purchases | In-App purchases enables you to sell items from within your app. |   |   |   |
| Increased memory limit | Enables your app to exceed the default app memory limit on supported devices. |   |   |   |
| Inter-app audio | Enables your app to send and receive audio from other apps that have Inter-app audio enabled. |   |   |   |
| Keychain sharing | Enables multiple apps written by the same team to share passwords. |   |   |   |
| MDM managed associated domains | Enables Mobile Development Management (MDM) to supplement the Associated Domains that are included with your app with values such as server names that are unique for an environment. |   |   |   |
| Multipath | MultiPath enables your app to use multipath protocols such as Multipath TCP, which will seamlessly handover traffic from one interface to another. |   |   |   |
| Near field communication tag reader | Enables an app to read NFC Data Exchanged Format (NDEF) Near Field Communication (NFC) tags. |   |   |   |
| Network extensions | Enables you to create app extensions that extend and customize the network capabilities of your device. |   |   |   |
| Personal VPN | Enables your app to use custom VPN connections. |   |   |   |
| Push notifications | Enables your app to receive push notifications. |   |   |   |
| Push to talk | Enables your app to report Push to Talk channels to the system so that it can handle transmitting and receiving background audio. |   |   |   |
| Shared with You | Enables an app to claim links shared in Messages conversations and for them to be surfaced to it via the Shared with You framework. |   |   |   |
| Sign in with Apple | Enables users to authenticate with their Apple ID. |   |   |   |
| Siri | Enables your app to handle Siri requests. |   |   |   |
| Time sensitive notifications | Time sensitive notifications deliver information that demands immediate attention and directly calls on the individual to take action the moment the notification is received. Time Sensitive alerts are always delivered immediately, are surfaced above other notifications, and are allowed to break through Focus and Do Not Disturb. |   |   |   |
| Wallet | Wallet enables your app to manage passes, tickets, gift cards, and loyalty cards. It supports a variety of bar code formats. |   |   |   |
| WeatherKit | WeatherKit provides current and forecasted weather information. |   |   |   |
| Wireless accessory configuration | Enables your app to configure WiFi accessories. |   |   |   |

https://learn.microsoft.com/en-gb/dotnet/maui/ios/deployment/entitlements?view=net-maui-7.0
https://learn.microsoft.com/en-us/xamarin/ios/deploy-test/provisioning/entitlements?tabs=macos
https://developer.apple.com/documentation/bundleresources/entitlements?language=objc
