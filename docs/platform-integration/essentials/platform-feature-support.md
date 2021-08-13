---
title: "Essentials platform and feature support matrix"
description: "Learn about what features of .NET MAUI Essentials each platform supports."
ms.date: 08/11/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Platform support

This article describes which features of .NET Multi-platform App UI (.NET MAUI) Essentials are supported on the following platforms:

| Platform | Version                    |
|----------|----------------------------|
| Android  | 4.4 (API 19) or higher     |
| iOS      | 10.0 or higher             |
| Tizen    | 4.0 or higher              |
| tvOS     | 10.0 or higher             |
| watchOS  | 4.0 or higher              |
| UWP      | 10.0.16299.0 or higher     |
| macOS    | 10.12.6 (Sierra) or higher |

> [!NOTE]
>
> - **Tizen** is officially supported by the Samsung development team.
> - **tvOS** & **watchOS** have limited API coverage, please see the feature guide for more information.
> - **macOS** support is in preview.

## Feature support

The table below describes the .NET MAUI Essential features that are supported by each platform:

Icon Guide:

- ✔️ - Full support
- ⚠️ - Limited support
- ❌ - Not supported

| Feature                                         | Android | iOS | UWP | watchOS | tvOS | Tizen | macOS |
|-------------------------------------------------|:-------:|:---:|:---:|:-------:|:----:|:-----:|:-----:|
| [Accelerometer](sensors.md#accelerometer)       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [App Actions](app-actions.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ❌    |
| [App Information](app-information.md)           | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [App Theme](app-theme.md)                       | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Barometer](sensors.md#barometer)               | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Battery](battery.md)                           | ✔️     | ✔️  | ✔️ | ⚠️      | ❌   | ⚠️    | ✔️   |
| [Clipboard](clipboard.md)                       | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Color Converters](color-converters.md)         | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Compass](sensors.md#compass)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Connectivity](connectivity.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ✔️  | ✔️    | ✔️   |
| [Contacts](contacts.md)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Detect Shake](sensors.md#shake)                | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ❌    |
| [Device Display Information](device-display.md) | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ❌     | ✔️   |
| [Device Information](device-information.md)     | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Email](email.md)                               | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File Picker](file-picker.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [File System Helpers](file-system-helpers.md)   | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Flashlight](flashlight.md)                     | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ❌    |
| [Geocoding](geocoding.md)                       | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Geolocation](geolocation.md)                   | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Gyroscope](sensors.md#gyroscope)               | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [Haptic Feedback](haptic-feedback.md)           | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Launcher](launcher.md)                         | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Magnetometer](sensors.md#magnetometer)         | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
| [MainThread](main-thread.md)                    | ✔️     | ✔️  | ✔️ | ✔️      | ✔️  | ✔️    | ✔️   |
| [Maps](maps.md)                                 | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ✔️   |
| [Media Picker](media-picker.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ⚠️   |
| [Open Browser](open-browser.md)                 | ✔️     | ✔️  | ✔️ | ❌       | ❌   | ✔️    | ✔️   |
| [Orientation Sensor](sensors.md#orientation)    | ✔️     | ✔️  | ✔️ | ✔️      | ❌   | ✔️    | ❌    |
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
