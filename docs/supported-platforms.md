---
title: "Supported platforms for .NET MAUI apps"
description: ".NET MAUI supports developing apps for Android, iOS, Mac Catalyst, and Windows."
ms.date: 04/13/2022
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
| [Accelerometer](sensors.md#accelerometer)       | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [App Actions](app-actions.md)                   | ✔️     | ✔️  | ✔️     | ❌     | ❌    |
| [App Information](app-information.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [App Theme](app-theme.md)                       | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Barometer](sensors.md#barometer)               | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Battery](battery.md)                           | ✔️     | ✔️  | ✔️     | ⚠️    | ✔️   |
| [Clipboard](clipboard.md)                       | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
| [Color Converters](color-converters.md)         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Compass](sensors.md#compass)                   | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Connectivity](connectivity.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Contacts](contacts.md)                         | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Detect Shake](sensors.md#shake)                | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Device Display Information](device-display.md) | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
| [Device Information](device-information.md)     | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Email](email.md)                               | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [File Picker](file-picker.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [File System Helpers](file-system-helpers.md)   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Flashlight](flashlight.md)                     | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Geocoding](geocoding.md)                       | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Geolocation](geolocation.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Gyroscope](sensors.md#gyroscope)               | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Haptic Feedback](haptic-feedback.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Launcher](launcher.md)                         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Magnetometer](sensors.md#magnetometer)         | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [MainThread](main-thread.md)                    | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Maps](maps.md)                                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Media Picker](media-picker.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ⚠️   |
| [Open Browser](open-browser.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Orientation Sensor](sensors.md#orientation)    | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Permissions](permissions.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Phone Dialer](phone-dialer.md)                 | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Platform Extensions](platform-extensions.md)   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Preferences](preferences.md)                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Screenshot](screenshot.md)                     | ✔️     | ✔️  | ✔️     | ❌     | ❌    |
| [Secure Storage](secure-storage.md)             | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Share](share.md)                               | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [SMS](sms.md)                                   | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Text-to-Speech](text-to-speech.md)             | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Unit Converters](unit-converters.md)           | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Version Tracking](version-tracking.md)         | ✔️     | ✔️  | ✔️     | ✔️    | ✔️   |
| [Vibrate](vibrate.md)                           | ✔️     | ✔️  | ✔️     | ✔️    | ❌    |
| [Web Authenticator](web-authenticator.md)       | ✔️     | ✔️  | ✔️     | ❌     | ✔️   |
