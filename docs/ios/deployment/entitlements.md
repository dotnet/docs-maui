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

Entitlement keys can be added via the Source panel of the Entitlements.plist editor. The required keys will normally be added when using the Entitlements.plist editor but are listed below for reference.

### Access WiFi information

This Access WiFi information entitlement enables your app to obtain information about the currently connected WiFi network.

The entitlement is defined using the `com.apple.developer.networking.wifi-info` key, of type `Boolean`:

```xml
<key>com.apple.developer.networking.wifi-info</key>
<true/>
```

For more information, see [Access WiFi Information Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_networking_wifi-info?language=objc).

### App Attest

With the App Attest entitlement, you can generate a special cryptographic key on your device and use it to validate the integrity of your app before your server provides access to sensitive data.

The entitlement is defined using the `com.apple.developer.devicecheck.appattest-environment` key, of type `String`:

```xml

```

For more information, see [App Attest Environment](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_devicecheck_appattest-environment?language=objc) on developer.apple.com.

### App groups

The app groups entitlement enables your app to access group containers shared among multiple related apps as well as perform inter-process communication between the apps.

The entitlement is defined using the `com.apple.security.application-groups` key, of type `Array` of `String`:

```xml

```

For more information, see [App Groups Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_application-groups?language=objc) on developer.apple.com.

### Apple Pay

The Apple Pay entitlement enables users to easily and securely pay for physical good and services such as groceries, clothing, tickets, and reservations using payment information stored on their device.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Associated domains

The associated domains entitlement enables your app to be associated with specific domains for specific services, such as accessing Safari, saved passwords, and activity continuation.

The entitlement is defined using the `com.apple.developer.associated-domains` key, of type `Array` of `String`:

```xml

```

For more information, see [Associated Domains Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_associated-domains?language=objc) on developer.apple.com.

### AutoFill credential provider

The AutoFill credential provider entitlement enables an app, with user permission, to provide user names and passwords for AutoFill into the app and Safari.

The entitlement is defined using the `com.apple.developer.authentication-services.autofill-credential-provider` key, of type `Boolean`:

```xml
<key>com.apple.developer.authentication-services.autofill-credential-provider</key>
<true/>
```

For more information, see [AutoFill Credential Provider Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_authentication-services_autofill-credential-provider?language=objc) on developer.apple.com.

### ClassKit

The ClassKit entitlement enables your app to privately and securely share student progress with teachers on assigned activities, such as reading a chapter in a book or taking a quiz, in school-managed environments.

The entitlement is defined using the `com.apple.developer.ClassKit-environment` key, of type `String`:

```xml

```

For more information, see [ClassKit Environment Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_classkit-environment?language=objc) on developer.apple.com.

### Communicates with drivers

The communicates with drivers entitlement enables communication between an app and DriverKit drivers.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Communication notifications

The communication notifications entitlement enables an app to send communication notifications from a person to a person or multiple people.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Data protection

The data protection entitlement enables your app to use the built-in encryption on supported devices. When you specify a file as protected, the system will store the file in an encrypted format.

The entitlement is defined using the `com.apple.developer.default-data-protection` key, of type `String`:

```xml

```

For more information, see [Data Protection Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_default-data-protection?language=objc) on developer.apple.com.

### Extended virtual addressing

The extended virtual addressing entitlement enables you to use more address space in your app.

The entitlement is defined using the `com.apple.developer.kernel.extended-virtual-addressing` key, of type `Boolean`:

```xml
<key>com.apple.developer.kernel.extended-virtual-addressing</key>
<true/>
```

For more information, see [Extended Virtual Addressing Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_kernel_extended-virtual-addressing?language=objc) on developer.apple.com.

### Family controls

The family controls entitlement enables parental controls in your app, granting access to the Managed Settings and Device Activity frameworks in the ScreenTime API. Use of Family controls requires Family Sharing for user enrolment. It prevents removal of your app and enables on-device content filters from Network Extensions.

The entitlement is defined using the `com.apple.developer.family-controls` key, of type `Boolean`:

```xml
<key>com.apple.developer.family-controls</key>
<true/>
```

For more information, see [Family Controls Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_family-controls?language=objc) on developer.apple.com.

### FileProvider testing mode

The FileProvider testing mode entitlement enables a test mode that provides the File Provider extension more control over the system's behavior during testing.

The entitlement is defined using the `com.apple.developer.fileprovider.testing-mode` key, of type `Boolean`:

```xml
<key>com.apple.developer.fileprovider.testing-mode</key>
<true/>
```

For more information, see [FileProvider Testing Mode Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_fileprovider_testing-mode?language=objc) on developer.apple.com.

### Fonts

The fonts entitlement enables your app, with user permission, to install and use custom fonts.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Group activities

The group activities entitlement enables an app to communicate with the same app on one or more other devices, to create a group activity within a FaceTime call. Group activities on FaceTime let users watch video together, listen to music together, or perform another synchronous activity.

The entitlement is defined using the `com.apple.developer.group-session` key, of type `Boolean`:

```xml
<key>com.apple.developer.group-session</key>
<true/>
```

For more information, see [Group Activities Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_group-session?language=objc) on developer.apple.com.

### HealthKit

The HealthKit entitlement enables your app to access, with user permission, personal health information.

The entitlement is defined using the `com.apple.developer.healthkit` key, of type `Boolean`:

```xml
<key>com.apple.developer.healthkit</key>
<array/>
```

For more information, see [HealthKit Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_healthkit?language=objc) on developer.apple.com.

### HomeKit

The HomeKit entitlement enables your app to interact with HomeKit accessories.

The entitlement is defined using the `com.apple.developer.homekit` key, of type `Boolean`:

```xml
<key>com.apple.developer.homekit</key>
<true/>
```

For more information, see [HomeKit Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_homekit?language=objc) on developer.apple.com.

### Hotspot configuration

The hotspot configuration entitlement entitlement enables your app to configure WiFi networks.

The entitlement is defined using the `com.apple.developer.networking.HotspotConfiguration` key, of type `Boolean`:

```xml
<key>com.apple.developer.networking.HotspotConfiguration</key>
<true/>
```

For more information, see [Hotspot Configuration Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_networking_hotspotconfiguration?language=objc) on developer.apple.com.

### iCloud

The iCloud entitlement enables your app to store data in the cloud, making it possible for users to share their data across multiple devices.

The entitlement is defined using the `` key, of type ``:

```xml

```

### In-App purchases

The In-App purchases entitlement enables you to sell items from within your app.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Increased memory limit

The increased memory limit entitlement enables your app to exceed the default app memory limit on supported devices.

The entitlement is defined using the `com.apple.developer.kernel.increased-memory-limit` key, of type `Boolean`:

```xml
<key>com.apple.developer.kernel.increased-memory-limit</key>
<true/>
```

For more information, see [Increased Memory Limit Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_kernel_increased-memory-limit?language=objc) on developer.apple.com.

### Inter-app audio

The inter-app audio entitlement enables your app to send and receive audio to/from other apps that have Inter-app audio enabled.

The entitlement is defined using the `inter-app-audio` key, of type `Boolean`:

```xml
<key>inter-app-audio</key>
<true/>
```

For more information, see [Inter-App Audio Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/inter-app-audio?language=objc) on developer.apple.com.

> [!IMPORTANT]
> This entitlement is deprecated in iOS 13 and is unavailable when running iPads apps in macOS.

### Keychain sharing

The Keychain sharing entitlement enables multiple apps written by the same team to share passwords.

The entitlement is defined using the `keychain-access-groups` key, of type `Array` of `String`:

```xml

```

For more information, see [Keychain Access Groups entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/keychain-access-groups?language=objc) on developer.apple.com.

### MDM managed associated domains

The Mobile Development Management (MDM) managed associated domains entitlement enables MDM to supplement the Associated Domains that are included with your app with values such as server names that are unique for an environment.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Multipath

The Multipath entitlement enables your app to use multipath protocols such as Multipath TCP, which will seamlessly handover traffic from one interface to another.

The entitlement is defined using the `com.apple.developer.networking.multipath` key, of type `Boolean`:

```xml
<key>com.apple.developer.networking.multipath</key>
<true/>
```

For more information, see [Multipath Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_networking_multipath?language=objc) on developer.apple.com.

### Near field communication tag reader

The near field communication tag reader entitlement enables an app to read NFC Data Exchanged Format (NDEF) Near Field Communication (NFC) tags.

The entitlement is defined using the `com.apple.developer.nfc.readersession.formats` key, of type `Array` of `String`:

```xml

```

For more information, see [Near Field Communication Tag Reader Session Formats Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_nfc_readersession_formats?language=objc) on developer.apple.com.

### Network extensions

The network extensions entitlement enables you to create app extensions that extend and customize the network capabilities of your device.

The entitlement is defined using the `com.apple.developer.networking.networkextension` key, of type `Array` of `String`:

```xml

```

For more information, see [Network Extensions Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_networking_networkextension?language=objc) on developer.apple.com.

### Personal VPN

The personal VPN entitlement enables your app to use custom VPN connections.

The entitlement is defined using the `com.apple.developer.networking.vpn.api` key, of type `Array` of `String`:

```xml

```

For more information, see [Personal VPN Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_networking_vpn_api?language=objc) on developer.apple.com.

### Push notifications

The push notifications entitlement enables your app to receive push notifications.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Push to talk

The push to talk entitlement enables your app to report Push to Talk channels to the system so that it can handle transmitting and receiving background audio.

The entitlement is defined using the `com.apple.developer.push-to-talk` key, of type `Boolean`:

```xml
<key>com.apple.developer.push-to-talk</key>
<true/>
```

For more information, see [Push to Talk Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_push-to-talk?language=objc) on developer.apple.com.

### Shared with You

The shared with you entitlement enables an app to claim links shared in Messages conversations and for them to be surfaced to it via the Shared with You framework.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Sign in with Apple

The sign in with Apple entitlement enables users to authenticate with their Apple ID.

The entitlement is defined using the `com.apple.developer.applesignin` key, of type `Array` of `String`:

```xml

```

For more information, see [Sign in with Apple Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_applesignin?language=objc) on developer.apple.com.

### Siri

The Siri entitlement enables your app to handle Siri requests.

The entitlement is defined using the `com.apple.developer.siri` key, of type `Boolean`:

```xml
<key>com.apple.developer.siri</key>
<true/>
```

For more information, see [Siri Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_siri?language=objc) on developer.apple.com.

### Time sensitive notifications

The time sensitive notifications entitlement enables an app to handle time sensitive notifications. Time sensitive notifications deliver information that demands immediate attention and directly calls on the individual to take action the moment the notification is received. Time Sensitive alerts are always delivered immediately, are surfaced above other notifications, and are allowed to break through Focus and Do Not Disturb.

The entitlement is defined using the `` key, of type ``:

```xml

```

### Wallet

The wallet entitlement enables your app to manage passes, tickets, gift cards, and loyalty cards. It supports a variety of bar code formats.

The entitlement is defined using the `` key, of type ``:

```xml

```

### WeatherKit

The WeatherKit entitlement enables an app to receive and process current and forecasted weather information.

The entitlement is defined using the `com.apple.developer.weatherkit` key, of type `Boolean`:

```xml
<key>com.apple.developer.weatherkit</key>
<true/>
```

For more information, see [WeatherKit Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_developer_weatherkit?language=objc) on developer.apple.com.

### Wireless accessory configuration

The wireless accessory configuration entitlement enables your app to configure WiFi accessories.

The entitlement is defined using the `com.apple.external-accessory.wireless-configuration` key, of type `Boolean`:

```xml
<key>com.apple.external-accessory.wireless-configuration</key>
<true/>
```

For more information, see [Wireless Accessory Configuration Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_external-accessory_wireless-configuration?language=objc) on developer.apple.com.

---

https://learn.microsoft.com/en-gb/dotnet/maui/ios/deployment/entitlements?view=net-maui-7.0
https://learn.microsoft.com/en-us/xamarin/ios/deploy-test/provisioning/entitlements?tabs=macos
https://developer.apple.com/documentation/bundleresources/entitlements?language=objc
