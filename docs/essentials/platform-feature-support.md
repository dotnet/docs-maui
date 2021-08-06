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
| [Accelerometer](accelerometer.md?context=xamarin/xamarin-forms)               | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [App Actions](app-actions.md?context=xamarin/xamarin-forms)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ❌    |
| [App Information](app-information.md?context=xamarin/xamarin-forms)           | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [App Theme](app-theme.md?context=xamarin/xamarin-forms)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Barometer](barometer.md?context=xamarin/xamarin-forms)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Battery](battery.md?context=xamarin/xamarin-forms)                           | ✔️     | ✔️  | ✔️ | ⚠️      | ❌   | ⚠️    | ✔️   |
| [Clipboard](clipboard.md?context=xamarin/xamarin-forms)                       | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Color Converters](color-converters.md?context=xamarin/xamarin-forms)         | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Compass](compass.md?context=xamarin/xamarin-forms)                           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Connectivity](connectivity.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [Contacts](contacts.md?context=xamarin/xamarin-forms)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Detect Shake](detect-shake.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ❌    |
| [Device Display Information](device-display.md?context=xamarin/xamarin-forms) | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Device Information](device-information.md?context=xamarin/xamarin-forms)     | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Email](email.md?context=xamarin/xamarin-forms)                               | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File Picker](file-picker.md?context=xamarin/xamarin-forms)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File System Helpers](file-system-helpers.md?context=xamarin/xamarin-forms)   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Flashlight](flashlight.md?context=xamarin/xamarin-forms)                     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Geocoding](geocoding.md?context=xamarin/xamarin-forms)                       | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Geolocation](geolocation.md?context=xamarin/xamarin-forms)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Gyroscope](gyroscope.md?context=xamarin/xamarin-forms)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Haptic Feedback](haptic-feedback.md?context=xamarin/xamarin-forms)           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Launcher](launcher.md?context=xamarin/xamarin-forms)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Magnetometer](magnetometer.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [MainThread](main-thread.md?content=xamarin/xamarin-forms)                    | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Maps](maps.md?content=xamarin/xamarin-forms)                                 | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Media Picker](media-picker.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ⚠️   |
| [Open Browser](open-browser.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Orientation Sensor](orientation-sensor.md?context=xamarin/xamarin-forms)     | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Permissions](permissions.md?context=xamarin/xamarin-forms)                   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Phone Dialer](phone-dialer.md?context=xamarin/xamarin-forms)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Platform Extensions](platform-extensions.md?context=xamarin/xamarin-forms)   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Preferences](preferences.md?context=xamarin/xamarin-forms)                   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Screenshot](screenshot.md?context=xamarin/xamarin-forms)                     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ❌    |
| [Secure Storage](secure-storage.md?context=xamarin/xamarin-forms)             | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Share](share.md?context=xamarin/xamarin-forms)                               | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [SMS](sms.md?context=xamarin/xamarin-forms)                                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Text-to-Speech](text-to-speech.md?context=xamarin/xamarin-forms)             | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Unit Converters](unit-converters.md?context=xamarin/xamarin-forms)           | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Version Tracking](version-tracking.md?context=xamarin/xamarin-forms)         | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Vibrate](vibrate.md?context=xamarin/xamarin-forms)                           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Web Authenticator](web-authenticator.md?context=xamarin/xamarin-forms)       | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ❌     | ✔️   |
