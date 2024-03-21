---
title: "Apple privacy manifest"
description: Learn how to include a privacy manifest in your .NET MAUI app on iOS.
ms.date: 03/19/2024
---

# Apple privacy manifest

Apple has a privacy policy for apps that target iOS or tvOS on the App Store. It requires the app to include a privacy manifest in the app bundle, that lists the types of data your .NET MAUI app or any third-party SDKs and packages collect, and the reasons for using any required reason APIs. If your use of the required reason APIs, or third-party SDKs, isn’t declared in the privacy manifest, your app might be rejected by the App Store. For more information about privacy manifests, see [Privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) on developer.apple.com.

Depending on whether you’re using .NET MAUI to develop an app or providing binding packages to use with .NET MAUI apps, the requirement for providing a privacy manifest might differ.

## Privacy manifest for .NET MAUI apps  

All .NET MAUI apps that target devices running iOS, iPadOS or tvOS require a privacy manifest in the app bundle. For more information, see [Add required entries to the privacy manifest](#add-required-entries-to-the-privacy-manifest).

You'll also need to review your own code, any native code, and data collection and tracking practices and update the privacy manifest accordingly:

- If your app or SDK collects data about the person using the app, you'll need to describe the data use in a privacy manifest. For information, see [Describing data use in privacy manifests](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_data_use_in_privacy_manifests) on developer.apple.com.
- If your app or SDK includes .NET APIs that call Apple's required reason APIs, then you must assess your use of each of these APIs and declare the reasons for using them. For more information about the required reasons APIs, see [Describing use of required reason API](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) on developer.apple.com.

> [!NOTE]
> If your app includes any third-party SDKs or packages, then these third-party components must include their own privacy manifests separately.

For more information about creating a privacy manifest, see [Create a privacy manifest](#create-a-privacy-manifest).

> [!IMPORTANT]
> The above guidelines are provided for your convenience. It’s important that you review Apple’s documentation on [privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) before creating a privacy manifest for your project.

## Privacy manifest for binding projects

If you are binding project owner, and you are binding a xcframework, then the xcframework provider will need to include the `PrivacyInfo.xcprivacy` file as part of the xcframework. Otherwise, there are two options, provide documentation for package consumers to create the `PrivacyInfo.xcprivacy` file properly or change the bindings to bind an xcframework that has the `PrivacyInfo.xcprivacy` file included. It is currently not possible for binding project authors to include a `PrivacyInfo.xcprivacy` file outside an xcframework that will be recognized by Apple when submitting an app.

## Create a privacy manifest

To add a privacy manifest to your .NET MAUI app project, add a new XML file named *PrivacyInfo.xcprivacy* to the *Platforms/iOS* folder of your app project. Ensure that the *PrivacyInfo.xcprivacy* file doesn't have an *.xml* extension. Then, add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict/>
</plist>
```

Then, edit your .NET MAUI app project file (*.csproj) and add the following build item for iOS at the bottom of the root `<Project>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
    <BundleResource Include="Platforms\iOS\PrivacyInfo.xcprivacy" LogicalName="PrivacyInfo.xcprivacy" />
</ItemGroup>
```

This will ensure that the privacy manifest is packaged into the iOS app at the root of the bundle.

## Add required entries to the privacy manifest

All .NET MAUI apps that target devices running iOS or iPadOS require a privacy manifest in the app bundle. This is due to the .NET runtime and Base Class Library (BCL) using required reason APIs that aren't removed regardless of the linker mode. The three API categories and their associated reasons that must be in the privacy manifest in a .NET MAUI app are shown in the following table:

| API category | Reason | Link |
| ------------ | ------ | ---- |
| `NSPrivacyAccessedAPICategoryFileTimestamp` | `C617.1` | [File timestamp APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278393) |
| `NSPrivacyAccessedAPICategorySystemBootTime` | `35F9.1` | [System boot time APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278394) |
| `NSPrivacyAccessedAPICategoryDiskSpace` | `E174.1` | [Disk space APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278397) |

To add these entries to your privacy manifest, open the *PrivacyInfo.xcprivacy* file in a text editor and add the `NSPrivacyAccessAPITypes` key, where each required reason API category use will subsequently be added:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
    </array>
</dict>
</plist>
```

Then, add the `NSPrivacyAccessedAPICategoryFileTimestamp` category with reason `C617.1`, `NSPrivacyAccessedAPICategorySystemBootTime` category with reason `35F9.1`, and `NSPrivacyAccessedAPICategoryDiskSpace` category with reason `E174.1`, to the `NSPrivacyAccessedAPITypes` array in the privacy manifest:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryFileTimestamp</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>C617.1</string>
            </array>
        </dict>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategorySystemBootTime</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>35F9.1</string>
            </array>
        </dict>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryDiskSpace</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>E174.1</string>
            </array>
        </dict>       
    </array>
</dict>
</plist>
```
These entries are the minimum you may need for your app. If you use any of the [Required Reasons API's](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) in a way that isn't covered by codes provided, then you will need to add additional reason codes to support your usage of the API. The [See Also](#see-also) section has additional resources for more detail on API usages in the .NET Runtime, .NET Base Class Libaries, .NET for iOS, and .NET MAUI that might cause you to need additional reason codes.

If your .NET MAUI app uses the [Preferences](~/platform-integration/storage/preferences.md) API or you use the <xref:Foundation.NSUserDefaults> API directly, you must include the reasons for use in your privacy manifest. Use the string `NSPrivacyAccessedAPICategoryUserDefaults` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK directly or indirectly, via .NET MAUI [Preferences](~/platform-integration/storage/preferences.md) API, uses the <xref:Foundation.NSUserDefaults> API, your *PrivacyInfo.xcprivacy* file should contain an additional `dict` element in the `NSPrivacyAccessedAPITypes` key's array as shown below:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryFileTimestamp</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>C617.1</string>
            </array>
        </dict>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategorySystemBootTime</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>35F9.1</string>
            </array>
        </dict>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryDiskSpace</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>E174.1</string>
            </array>
        </dict>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryUserDefaults</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>CA92.1</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

You will need to provide one or more reason codes from [User defaults APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278401) 

Add one or more of the reson codes below to indicate the correct usage:
* CA92.1 - To access user defaults in just your app.
* 1C8F.1 - To access user defaults from apps, app extensinos, and App Clips that are members of the same App Group.
* C56D.1 - To access user defaults from an SDK.
* AC6B.1 - To access user defaults to read the com.apple.configuration.managed or com.apple.feedback.managed key.

> [!IMPORTANT]
> An app's *PrivacyInfo.xcprivacy* file may need to be updated if you modify the code in your app. This includes adding a NuGet package or binding project to your app that calls into any of Apple’s required reason APIs.

## See also
To learn more about how the .NET Runtime, .NET Base Class Libaries, .NET for iOS, and .NET MAUI use the links below:
- [Required reasons API usage in .NET MAUI and Xamarin.Forms](https://github.com/xamarin/xamarin-macios/blob/main/docs/required-reasons-dotnet-maui.md)
- [Required reasons API usage in .NET for iOS, tvOS, and Xamarin.iOS](https://github.com/xamarin/xamarin-macios/blob/main/docs/required-reasons-macios.md)
- [Required reasons API usage in .NET, Mono and the BCL](https://github.com/xamarin/xamarin-macios/blob/main/docs/required-reasons-bcl.md)
