---
title: "Platform features"
description: ".NET MAUI provides cross-platform APIs to access platform functionality that includes access to sensors, storing data, and checking network connectivity."
ms.date: 06/27/2022
---

# Platform features

Each platform that .NET Multi-platform App UI (.NET MAUI) supports offers unique operating system and platform APIs that you can access from C#. .NET MAUI provides cross-platform APIs to access much of this platform functionality, which includes access to sensors, accessing information about the device an app is running on, checking network connectivity, storing data securely, and initiating browser-based authentication flows.

.NET MAUI separates these cross-platform APIs into different areas of functionality.

## Application model

.NET MAUI provides the following functionality in the `Microsoft.Maui.ApplicationModel` namespace:

| Functionality | Description |
| ------- | ----------- |
| App actions | The `AppActions` class enables you to create and respond to app shortcuts, which provide additional ways of starting your app. For more information, see [App actions](appmodel/app-actions.md). |
| App information | The `AppInfo` class provides access to basic app information, which includes the app name and version, and the current active theme for the device. For more information, see [App information](appmodel/app-information.md). |
| Browser | The `Browser` class enables an app to open a web link in an in-app browser, or the system browser. For more information, see [Browser](appmodel/open-browser.md). |
| Launcher | The `Launcher` class enables an app to open a URI, and is often used when deep linking into another app's custom URI schemes. For more information, see [Launcher](appmodel/launcher.md). |
| Main thread | The `MainThread` class enables you to run code on the UI thread. For more information, see [Main thread](appmodel/main-thread.md). |
| Maps | The `Map` class enables an app to open the system map app to a specific location or place mark. For more information, see [Maps](appmodel/maps.md). |
| Permissions | The `Permissions` class enables you to check and request permissions at run-time. For more information, see [Permissions](appmodel/permissions.md). |
| Version tracking | The `VersionTracking` class enables you to check the app's version and build numbers, and determine if it's the first time the app has been launched. For more information, see [Version tracking](appmodel/version-tracking.md). |

## Communication

.NET MAUI provides the following functionality in the `Microsoft.Maui.ApplicationModel.Communication` namespace:

| Functionality | Description |
| ------- | ----------- |
| Contacts | The `Contacts` class enables an app to select a contact and read information about it. For more information, see [Contacts](communication/contacts.md). |
| Email | The `Email` class can be used to open the default email app, and can create a new email with the specified recipients, subject, and body. For more information, see [Email](communication/email.md). |
| Networking | The `Connectivity` class, in the `Microsoft.Maui.Networking` namespace, enables you to inspect the network accessibility of the device your app is running on. For more information, see [Connectivity](communication/networking.md). |
| Phone dialer | The `PhoneDialer` class enables an app to open a phone number in the dialer. For more information, see [Phone dialer](communication/phone-dialer.md). |
| SMS (messaging) | The `Sms` class can be used to open the default SMS app and preload it with a recipient and message. For more information, see [SMS](communication/sms.md). |
| Web authenticator | The `WebAuthenticator` class, in the `Microsoft.Maui.Authentication` namespace, enables you to start a browser-based authentication flow, which listens for a callback to a specific URL registered to the app. For more information, see [Web authenticator](communication/authentication.md). |

## Device features

.NET MAUI provides the following functionality in the `Microsoft.Maui.Devices` namespace:

| Functionality | Description |
| ------- | ----------- |
| Battery | The `Battery` class enables an app to check the device's battery information, and monitor the battery for changes. For more information, see [Battery](device/battery.md). |
| Device display | The `DeviceDisplay` class enables an app to read information about the device's screen metrics. For more information, see [Device display](device/display.md). |
| Device information | The `DeviceInfo` class enables an app to read information about the device the app is running on. For more information, see [Device information](device/information.md). |
| Device sensors | Types in the `Microsoft.Maui.Devices.Sensors` namespace provide access to the device's accelerometer, barometer, compass, gyroscope, magnetometer, and orientation sensor. For more information, see [Device sensors](device/sensors.md). |
| Flashlight | The `FlashLight` class can toggle the device's camera flash on and off, to emulate a flashlight. For more information, see [Flashlight](device/flashlight.md). |
| Geocoding | The `Geocoding` class, in the `Microsoft.Maui.Devices.Sensors` namespace, provides APIs to geocode a place mark to a positional coordinate, and reverse geocode a coordinate to a place mark. For more information, see [Geocoding](device/geocoding.md). |
| Geolocation | The `Geolocation` class, in the `Microsoft.Maui.Devices.Sensors` namespace, provides APIs to retrieve the device's current geolocation coordinates. For more information, see [Geolocation](device/geolocation.md). |
| Haptic feedback | The `HapticFeedback` class control's haptic feedback on a device, which is generally manifested as a gentle vibration sensation to give a response to the user. For more information, see [Haptic feedback](device/haptic-feedback.md). |
| Vibration | The `Vibration` class enables you to start and stop the vibrate functionality for a desired amount of time. For more information, see [Vibration](device/vibrate.md). |

## Media

.NET MAUI provides the following functionality in the `Microsoft.Maui.Media` namespace:

| Functionality | Description |
| ------- | ----------- |
| Media picker | The `MediaPicker` class enables you to prompt the user to pick or take a photo or video on the device. For more information, see [Media picker](device-media/picker.md). |
| Screenshot | The `Screenshot` class enables you to capture the current displayed screen of the app. For more information, see [Screenshot](device-media/screenshot.md). |
| Text-to-speech | The `TextToSpeech` class enables an app to utilize the built-in text-to-speech engines to speak text from the device. For more information, see [Text-to-Speech](device-media/text-to-speech.md). |
| Unit converters | The `UnitConverters` class provides unit converters to help you convert from one unit of measurement to another. For more information, see [Unit converters](device-media/unit-converters.md). |

## Sharing

.NET MAUI provides the following functionality in the `Microsoft.Maui.ApplicationModel.DataTransfer` namespace:

| Functionality | Description |
| ------- | ----------- |
| Clipboard | The `Clipboard` class enables an app copy and paste text to and from the system clipboard. For more information, see [Clipboard](data/clipboard.md). |
| Share files and text | The `Share` class provides an API to send data, such as text or web links, to the device's share function. For more information, see [Share](data/share.md). |

## Storage

.NET MAUI provides the following functionality in the `Microsoft.Maui.Storage` namespace:

| Functionality | Description |
| ------- | ----------- |
| File picker | The `FilePicker` class enables you to prompt the user to pick one or more files from the device. For more information, see [File picker](storage/file-picker.md). |
| File system helpers | The `FileSystem` class provides helper methods that access the app's cache and data folders, and helps access files that are stored in the app package. For more information, see [File system helpers](storage/file-system-helpers.md). |
| Preferences | The `Preferences` class helps to store app preferences in a key/value store. For more information, see [Preferences](storage/preferences.md). |
| Secure storage | The `SecureStorage` class helps to securely store simple key/value pairs. For more information, see [Secure storage](storage/secure-storage.md). |

## Access platform APIs

.NET MAUI platform-specifics allow you to consume specific functionality that's only available on a specific platform. For more information, see [Android platform-specifics](~/android/platform-specifics/index.md), [iOS platform-specifics](~/ios/platform-specifics/index.md), and [Windows platform-specifics](~/windows/platform-specifics/index.md).

In situations where .NET MAUI doesn't provide any APIs for accessing specific platform APIs, you can write your own code to access the required platform APIs. For more information, see [Invoke platform code](invoke-platform-code.md).
