---
title: "Xamarin.Essentials Platform & Feature Support"
description: "Xamarin.Essentials provides a single cross-platform API that works with any iOS, Android, or UWP application that can be accessed from shared code no matter how the user interface is created."
ms.date: 08/20/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Platform Support

Xamarin.Essentials supports the following platforms and operating systems:

| Platform | Version |
| --- | --- |
| Android | 4.4 (API 19) or higher |
| iOS |10.0 or higher |
| Tizen | 4.0 or higher |
| tvOS | 10.0 or higher |
| watchOS | 4.0 or higher |
| UWP | 10.0.16299.0 or higher |
| macOS | 10.12.6 (Sierra) or higher |

> [!NOTE]
>
> * Tizen is officially supported by the Samsung development team.
> * tvOS & watchOS have limited API coverage, please see the feature guide for more information.
> * macOS support is in preview.

## Feature Support

Xamarin.Essentials always tries to bring features to every platform, however sometimes there are limitations based on the device. Below is a guide of what features are supported on each platform.

Icon Guide:

* ✔️ - Full support
* ⚠️ - Limited support
* ❌ - Not supported

| Feature                                                                       | Android | iOS | UWP | watchOS | tvOS | Tizen | macOS |
|-------------------------------------------------------------------------------|:-------:|:---:|:---:|:-------:|:----:|:-----:|:-----:|
| [Accelerometer](sensors.md#accelerometer)               | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [App Actions](app-actions.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ❌    |
| [App Information](app-information.md)           | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [App Theme](app-theme.md)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Barometer](sensors.md#barometer)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Battery](battery.md)                           | ✔️     | ✔️  | ✔️ | ⚠️      | ❌   | ⚠️    | ✔️   |
| [Clipboard](clipboard.md)                       | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Color Converters](color-converters.md)         | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Compass](sensors.md#compass)                           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Connectivity](connectivity.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [Contacts](contacts.md)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Detect Shake](sensors.md#shake)                 | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ❌    |
| [Device Display Information](device-display.md) | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Device Information](device-information.md)     | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Email](email.md)                               | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File Picker](file-picker.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File System Helpers](file-system-helpers.md)   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Flashlight](flashlight.md)                     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Geocoding](geocoding.md)                       | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Geolocation](geolocation.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Gyroscope](sensors.md#gyroscope)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Haptic Feedback](haptic-feedback.md)           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Launcher](launcher.md)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Magnetometer](sensors.md#magnetometer)                 | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [MainThread](main-thread.md)                    | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Maps](maps.md)                                 | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Media Picker](media-picker.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ⚠️   |
| [Open Browser](open-browser.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Orientation Sensor](sensors.md#orientation)     | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Permissions](permissions.md)                   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Phone Dialer](phone-dialer.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Platform Extensions](platform-extensions.md)   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Preferences](preferences.md)                   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Screenshot](screenshot.md)                     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ❌    |
| [Secure Storage](secure-storage.md)             | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Share](share.md)                               | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [SMS](sms.md)                                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Text-to-Speech](text-to-speech.md)             | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Unit Converters](unit-converters.md)           | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Version Tracking](version-tracking.md)         | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Vibrate](vibrate.md)                           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Web Authenticator](web-authenticator.md)       | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ❌     | ✔️   |
