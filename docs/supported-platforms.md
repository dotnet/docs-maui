---
title: "Supported platforms for .NET MAUI apps"
description: ".NET MAUI supports developing apps for Android, iOS, Mac Catalyst, and Windows."
ms.date: 04/29/2022
---

# Supported platforms for .NET MAUI apps

.NET Multi-platform App UI (.NET MAUI) apps can be written for the following platforms:

- Android 5.0 (API 21) or higher. For .NET MAUI Blazor apps, Android 6 (API 23) or higher is required.
- iOS 10 or higher. For .NET MAUI Blazor apps, iOS 11 or higher is required.
- macOS 10.13 or higher, using Mac Catalyst.
- Windows 11 and Windows 10 version 1809 or higher, using [Windows UI Library (WinUI) 3](/windows/apps/winui/winui3/).

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

.NET MAUI apps for Android, iOS, and Windows can be built in Visual Studio. However, a networked Mac is required for iOS development. <!--From .NET MAUI Preview 6, the minimum required version of Xcode is 13.0 Beta 1.-->

<!-- .NET MAUI apps for Android, iOS, and macOS can be built in Visual Studio for Mac.-->

## Additional platform support

.NET MAUI supports additional platforms beyond Android, iOS, macOS and Windows:

- Tizen, supported by Samsung.
- Linux, supported by the community.

<!-- ## Android platform support

You should have the latest Android SDK Tools and Android API platform installed. You can update to the latest versions using the Android SDK Manager.

Additionally, the target/compile version for Android projects **must** be set to *Use latest installed platform*. However the minimum version can be set to API 21 so you can continue to support devices that use Android 5.0 and newer. -->

## Device feature support

The table below describes the .NET MAUI Essential features that are supported by each platform:

- ✔️ - Full support
- ⚠️ - Limited support
- ❌ - Not supported

| Feature                                         | Android | iOS | Windows | Tizen | macOS |
|-------------------------------------------------|:-------:|:---:|:-------:|:-----:|:-----:|
| [Accelerometer](platform-integration/sensors/index.md#accelerometer)       | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [App Actions](platform-integration/appmodel/app-actions.md)                   | ✔️     | ✔️  | ✔️     | ❌     | ❌    |
| [App Information](platform-integration/appmodel/app-information.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [App Theme](platform-integration/appmodel/app-theme.md)                       | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Barometer](platform-integration/sensors/index.md#barometer)               | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Battery](platform-integration/device/battery.md)                           | ✔️     | ✔️  | ✔️     | ⚠️    | ✔️   |
| [Clipboard](platform-integration/data/clipboard.md)                       | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
| Color Converters         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Compass](platform-integration/sensors/index.md#compass)                   | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Connectivity](platform-integration/networking.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Contacts](platform-integration/communication/contacts.md)                         | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Shake](platform-integration/sensors/index.md#shake)                | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Device display information](platform-integration/device/device-display.md) | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
| [Device information](platform-integration/device/device-information.md)     | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Email](platform-integration/communication/email.md)                              | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [File Picker](platform-integration/storage/file-picker.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [File system helpers](platform-integration/storage/file-system-helpers.md)   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Flashlight](platform-integration/device/flashlight.md)                     | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Geocoding](platform-integration/sensors/geocoding.md)                       | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Geolocation](platform-integration/sensors/geolocation.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Gyroscope](platform-integration/sensors/index.md#gyroscope)               | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Haptic Feedback](platform-integration/device/haptic-feedback.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Launcher](platform-integration/appmodel/launcher.md)                         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Magnetometer](platform-integration/sensors/index.md#magnetometer)         | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [MainThread](platform-integration/appmodel/main-thread.md)                    | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Map](platform-integration/appmodel/maps.md)                                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Media Picker](platform-integration/device-media/media-picker.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ⚠️   |
| [Browser](platform-integration/appmodel/open-browser.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Orientation Sensor](platform-integration/sensors/index.md#orientation)    | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Permissions](platform-integration/appmodel/permissions.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Phone Dialer](platform-integration/communication/phone-dialer.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Platform Extensions](platform-integration/appmodel/platform-extensions.md)   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Preferences](platform-integration/storage/preferences.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Screenshot](platform-integration/device-media/screenshot.md)                     | ✔️     | ✔️  | ✔️     | ❌     | ❌    |
| [Secure storage](platform-integration/storage/secure-storage.md)             | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Share](platform-integration/data/share.md)                               | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [SMS](platform-integration/communication/sms.md)                                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Text-to-Speech](platform-integration/device-media/text-to-speech.md)             | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Unit Converters](platform-integration/device-media/unit-converters.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Version Tracking](platform-integration/appmodel/version-tracking.md)         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Vibration](platform-integration/device/vibrate.md)                           | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Web Authenticator](platform-integration/authentication.md)       | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
