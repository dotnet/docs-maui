---
ms.topic: include
ms.date: 03/23/2023
---

### Specify the user interface idiom

A Mac Catalyst app can run in the iPad or Mac user interface idiom:

- The iPad user interface idiom tells macOS to scale the app's user interface to match the Mac display environment while preserving iPad-like appearance.
- The Mac user interface idiom doesn't scale the app's user interface to match the Mac display environment. Some controls change their size and appearance, and interacting with them feels identical to interacting with `AppKit` controls.

By default, .NET MAUI Mac Catalyst apps use the iPad user interface idiom. If this is your desired behavior, ensure that the app's *Info.plist* file only specifies 2 as the value of the `UIDeviceFamily` key:

```xml
<key>UIDeviceFamily</key>
<array>
  <integer>2</integer>
</array>
```

To adopt the Mac user interface idiom, update the app's *Info.plist* file to specify 6 as the value of the `UIDeviceFamily` key:

```xml
<key>UIDeviceFamily</key>
<array>
  <integer>6</integer>
</array>
```

For more information about Mac Catalyst user interface idioms, see [Specify the UI idiom for your Mac Catalyst app](../user-interface-idiom.md).

### Set the default language and region for the app

Set the `CFBundleDevelopmentRegion` key in your app's *Info.plist* to a `string` that represents the localization native development region:

```xml
<key>CFBundleDevelopmentRegion</key>
<string>en</string>
```

The value of the key should be a language designator, with an optional region designator. For more information, see [CFBundleDevelopmentRegion](https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundledevelopmentregion) on developer.apple.com.

### Set the copyright key

Set the `NSHumanReadableCopyright` key in your app's *Info.plist* to a `string` that represents the human-readable copyright notice for your app:

```xml
<key>NSHumanReadableCopyright</key>
<string>MyMauiApp © 2023</string>
```

For more information, see [NSHumanReadableCopyright](https://developer.apple.com/documentation/bundleresources/information_property_list/nshumanreadablecopyright) on developer.apple.com.
