---
title: "Apple privacy manifest"
description: Learn how to include a privacy manifest file in your .NET MAUI app on iOS or iPadOS.
ms.date: 03/18/2024
---

# Apple privacy manifest

Apple has introduced a privacy policy for including [privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) in new and updated apps that target iOS, iPadOS, and tvOS on the App Store.

The privacy manifest file, *PrivacyInfo.xcprivacy*, should list the [types of data](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_data_use_in_privacy_manifests) your .NET MAUI apps, or any third-party SDKs, and packages collect, and the reasons for using certain [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) categories.

> [!IMPORTANT]
> If your use of the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api), or third-party SDKs, isn’t declared in the privacy manifest, your app might be rejected by the App Store. For more information, see [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) on developer.apple.com.

## Prepare your app for Apple’s privacy manifest policy

You must review your native code, C# code, and data collection and tracking practices to understand if Apple’s privacy manifest policy applies to your app. Use these guidelines to decide if you need to include a privacy manifest file in your product:

- If your app includes any third-party SDKs or packages, then these third-party components must provision their own privacy manifest files separately.

    > [!NOTE]
    > It’s your responsibility to ensure that the owners of these third-party components include privacy manifest files. Microsoft isn’t responsible for any third-party privacy manifest, and their data collection and tracking practices.

- If your app includes the [C# .NET APIs](#c-net-apis-in-net-maui) that call certain APIs listed in the Apple’s [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) categories, then you must assess your product for the API usage. For assessing what constitutes as part of data collection and tracking practices, refer to Apple’s documentation on [privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files).

    > [!NOTE]
    > It’s your responsibility to assess your use of each of these APIs and declare the applicable reasons for using them.

Depending on whether you’re using [.NET MAUI to develop an app](#privacy-manifest-for-net-maui-apps) or providing [ObjectiveC or Swift Binding packages](#privacy-manifest-for-binding-projects) to use with .NET MAUI apps, the requirement for providing a privacy manifest file might differ.

> [!IMPORTANT]
> The above guidelines are provided for your convenience. It’s important that you review Apple’s documentation on [privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) before creating a privacy manifest for your project.

## Privacy manifest for .NET MAUI apps  

To determine if you need to add a privacy manifest to your .NET MAUI app you must assess if your native app code uses any of the following APIs:

- APIs listed under the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) category.
- The [C# .NET APIs](#c-net-apis-in-net-maui) in .NET MAUI.

If you use any of these APIs, or if you have disabled [linking](~/ios/linking.md), which will retain all of the [C# .NET APIs](#c-net-apis-in-net-maui), then [create a privacy manifest file](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files#4284009) and add it to your project. In the privacy manifest file, you must declare the approved reasons for using the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) or [C# .NET APIs](#c-net-apis-in-net-maui), as applicable. For more information, see [Create the privacy manifest file](#create-the-privacy-manifest-file).

> [!WARNING]
> If you don’t declare the reasons for the use of APIs, your app might be rejected by the App Store.

You must also verify if any native app code collects any type of data [categorized by Apple](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_data_use_in_privacy_manifests#4250555) and declare those data types in the privacy manifest file as applicable. Any third-party SDKs or packages used in your app must include their own separate manifest files to declare data collection and the use of any [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) with approved reasons.

It’s your responsibility to check the accuracy of the privacy manifest within your .NET MAUI app and if any third-party components included in your .NET MAUI project require any declarations in you privacy manifest. You should search these third-party components for any references to a privacy manifest declaration.

If you’re developing an app using .NET MAUI as a library, check if your native app code collects any of the following information outside of the .NET MAUI project:

- [Data collection](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_data_use_in_privacy_manifests)
- [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api)

If it does, then you must declare their usage in the privacy manifest file.

## Privacy manifest for Binding projects

If you are Binding project owner, and you are binding a xcframework, then the xcframework provider will need to include the *PrivacyInfo.xcprivacy* file as part of the xcframework. Otherwise, there are two options, provide documentation for package consumers to create the *PrivacyInfo.xcprivacy* file properly or change the bindings to bind an xcframework that has the *PrivacyInfo.xcprivacy* file included. It is currently not possible for Binding project authors to include a *PrivacyInfo.xcprivacy* file outside an xcframework that will be recognized by Apple when submitting an app.

## C# .NET APIs in .NET MAUI

All .NET MAUI apps that target devices running iOS or iPadOS will require a *PrivacyInfo.xcprivacy* file in the app bundle. This is due to the .NET runtime and BCL using [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) that are not removed regardless of the [linking](~/ios/linking.md) setting. The three API categories and their associated reasons that must be in the *PrivacyInfo.xcprivacy* file are shown in the following table:

| API category | Reason |
| ------------ | ------ |
| `NSPrivacyAccessedAPICategoryFileTimestamp` | `C617.1` |
| `NSPrivacyAccessedAPICategorySystemBootTime` | `35F9.1` |
| `NSPrivacyAccessedAPICategoryDiskSpace` | `E174.1` |

In addition, if you use the <xref:Foundation.NSUserDefaults> API in your app, you will need to add the `NSPrivacyAccessedAPICategoryUserDefaults` API category, with a reason code of `CAS92.1`.

## Create the privacy manifest file

To add a privacy manifest file to your .NET MAUI app project, add a new XML file named *PrivacyInfo.xcprivacy* to the *Platforms/iOS* or *Platforms/MacCatalyst* folder of your app project. Then add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<!--
... omitted for brevity
-->
<plist version="1.0">
<dict/>
</plist>
```

Then, edit the .NET MAUI app project file (*.csproj) and add the following build item for iOS at the bottom of the root `<Project>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
    <BundleResource Include="Platforms\iOS\PrivacyInfo.xcprivacy" LogicalName="PrivacyInfo.xcprivacy" />
</ItemGroup>
```

Alternatively, add the following build item for Mac Catalyst at the bottom of the root `<Project>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">
    <BundleResource Include="Platforms\MacCatalyst\PrivacyInfo.xcprivacy" LogicalName="PrivacyInfo.xcprivacy" />
</ItemGroup>
```

This will package the file into the iOS or Mac Catalyst app at the root of the bundle.

## Add entries to the privacy manifest

The following example shows editing a privacy manifest file for an app that uses the <xref:Foundation.NSUserDefaults>, <xref:Foundation.NSProcessInfo.SystemUptime>, and <xref:Foundation.NSFileManager.ModificationDate> APIs.

Using a text editor, add the `NSPrivacyAccessAPITypes` key, where each category usage will be added, to the privacy manifest:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<!--
... omitted for brevity
-->
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
    </array>
</dict>
</plist>
```

The .NET runtime and BCL include APIs from the [file timestamp](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278393), [system boot time](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278394), and [disk space](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278397) API categories. Therefore, add the `NSPrivacyAccessedAPICategoryFileTimestamp` category with reason `C617.1`, `NSPrivacyAccessedAPICategorySystemBootTime` category with reason `35F9.1`, and `NSPrivacyAccessedAPICategoryDiskSpace` category with reason `E174.1` to the `NSPrivacyAccessedAPITypes` array in the privacy manifest:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<!--
... omitted for brevity
-->
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

For the <xref:Foundation.NSProcessInfo.SystemUptime?displayProperty=nameWithType> API, a reason code of `35F9.1` is needed since that is the only reason code available. However, since the .NET runtime and BCL already included that category and reason, there is nothing additional to add.

For the <xref:Foundation.NSFileManager.ModificationDate?displayProperty=nameWithType> API, a reason code of `C617.1` is needed since the modification dates are stored as a hash using <xref:Foundation.NSUserDefaults>, but aren't displayed to the user. However, since the .NET runtime and BCL already included that category and reason, there is nothing additional to add.

For the <xref:Foundation.NSUserDefaults> API, a reason code of `CA92.1` is required since the data accessed is only accessible to the app itself. Therefore, the `NSPrivacyAccessedAPICategoryUserDefaults` category with a reason of `CA92.1` was added at the end of the `NSPrivacyAccessedAPITypes` array.

> [!IMPORTANT]
> Once added to your project, the *PrivacyInfo.xcprivacy* file will need to be updated if there are any additional API usages from additional categories or additional reasons for usage. This will include adding a NuGet package or Binding project that calls into any of Apple’s [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api).

## Required reasons API usage in .NET MAUI

The APIs in this section list C# .NET APIs that call the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api), organized by category. If your app, SDK, or package code, calls any of the APIs from the lists, declare the reasons for their use in your privacy manifest file following the guidelines specified in Apple’s documentation on [Required reasons APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api).

> [!NOTE]
> The following APIs are verified for .NET MAUI versions 8.0.0 and later.

### User defaults APIs

The <xref:Foundation.NSUserDefaults> API directly or indirectly accesses user defaults and require reasons for use.

Use the string `NSPrivacyAccessedAPICategoryUserDefaults` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use the <xref:Foundation.NSUserDefaults> API, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryUserDefaults</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [User defaults APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278401) need to be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

## Required reasons API usage in .NET iOS

The APIs in this section list C# .NET APIs that call the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api), organized by category. If your app, SDK, or package code, calls any of the APIs from the lists, declare the reasons for their use in your privacy manifest file following the guidelines specified in Apple’s documentation on [Required reasons APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api).

> [!NOTE]
> The following APIs are verified for .NET iOS version 8.0.4 and later.

### File timestamp APIs

The following APIs directly or indirectly access file timestamps and require reasons for use:

| Foundation APIs | UIKit APIs | AppKit APIs |
| --------------- | ---------- | ----------- |
| <xref:Foundation.NSFileManager.CreationDate?displayProperty=nameWithType> | <xref:UIKit.UIDocument.FileModificationDate?displayProperty=nameWithType> | <xref:AppKit.NSDocument.FileModificationDate?displayProperty=nameWithType> |
| <xref:Foundation.NSFileManager.ModificationDate?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.SetAttributes(Foundation.NSDictionary,System.String,Foundation.NSError)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.SetAttributes(Foundation.NSFileAttributes,System.String,Foundation.NSError)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.SetAttributes(Foundation.NSFileAttributes,SYstem.String)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateDirectory(System.String,System.Boolean,Foundation.NSDictionary,Foundation.NSError)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateDirectory(System.String,System.Boolean,Foundation.NSFileAttributes,Foundation.NSError)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateDirectory(System.String,System.Boolean,Foundation.NSFileAttributes)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateFile(System.String,Foundation.NSData,Foundation.NSDictionary)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateFile(System.String,Foundation.NSData,FOundation.NSFileAttributes)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.GetAttributes(System.String,Foundation.NSError)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.GetAttributes(System.String)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSDictionary.ToFileAttributes?displayProperty=nameWithType> | | |
| <xref:Foundation.NSUrl.ContentModificationDateKey?displayProperty=nameWithType> | | |
| <xref:Foundation.NSUrl.CreationDateKey?displayProperty=nameWithType> | | |

Use the string `NSPrivacyAccessedAPICategoryFileTimestamp` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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
                <string>...</string>
            </array>
        </dict>
	  </array>
</dict>
</plist>
```

Reason codes from [File timestamp APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278393) can be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

### System boot time APIs

The <xref:Foundation.NSProcessInfo.SystemUptime> API directly or indirectly accesses the system boot time and require reasons for use.

Use the string `NSPrivacyAccessedAPICategorySystemBootTime` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use the <xref:Foundation.NSProcessInfo.SystemUptime> API, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategorySystemBootTime</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>35F9.1</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [System boot time APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278394) can be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

### Disk space APIs

The following <xref:Foundation> APIs directly or indirectly access the available disk space and require reasons for use:

- <xref:Foundation.NSUrl.VolumeAvailableCapacityKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeAvailableCapacityForImportantUsageKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeAvailableCapacityForOpportunisticUsageKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeTotalCapacityKey?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.SystemFreeSize?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.SystemSize?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.GetFileSystemAttributes(System.String,Foundation.NSError)?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.GetFileSystemAttributes(System.String)?displayProperty=nameWithType>

Use the string `NSPrivacyAccessedAPICategoryDiskSpace` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryDiskSpace</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [Disk space APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278397) need to be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

### Active keyboard APIs

The `AppKit.UITextInputMode.ActiveInputModes` API directly or indirectly accesses the list of available keyboards and require reasons for use.

Use the string `NSPrivacyAccessedAPICategoryActiveKeyboards` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use the `AppKit.UITextInputMode.ActiveInputModes` API, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryActiveKeyboards</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [Active keyboard APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278400) need to be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

### User defaults APIs

The following APIs directly or indirectly access user defaults and require reasons for use:

| Foundation APIs | UIKit APIs | AppKit APIs |
| --------------- | ---------- | ----------- |
| <xref:Foundation.NSUserDefaults> |  | `AppKit.NSUserDefaultsController.NSUserDefaultsController` |
|  |  | <xref:AppKit.NSUserDefaultsController.Defaults?displayProperty=nameWithType> |
|  |  | `AppKit.NSUserDefaultsController.SharedUserDefaultsController` |

Use the string `NSPrivacyAccessedAPICategoryUserDefaults` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryUserDefaults</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [User defaults APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278401) need to be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

## Required reasons API usage in .NET, Mono, and the BCL

The APIs in this section list C# .NET APIs that call the [required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api), organized by category. These API usages are present in your app even if you do not explicitly call them. Therefore, you will be required to provide the API categories and reasons provided below in the *PrivacyInfo.xcprivacy* file for your app. You may have to provide additional reason codes if you use the APIs directly. For more information, see [Required reason APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api).

> [!NOTE]
> The following APIs are verified for .NET versions 8.0.0 and later.

### File timestamp APIs

The following APIs directly or indirectly access file timestamps and require reasons for use:

- <xref:System.Diagnostics.FileVersionInfo?displayProperty=nameWithType>
- <xref:System.IO.Compression.ZipFile.CreateFromDirectory%2A?displayProperty=nameWithType>
- <xref:System.IO.Directory.CreateDirectory(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.CreateDirectory(System.String,System.IO.UnixFileMode)?displayProperty=nameWithType>
- [`System.Runtime.Loader.AssemblyLoadContext.ResolveSatelliteAssembly`](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/Loader/AssemblyLoadContext.cs,763)
- <xref:System.IO.Directory.Delete(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.Exists(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetCreationTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetCreationTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetLastAccessTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetLastAccessTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetLastWriteTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.GetLastWriteTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Directory.Move(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.DirectoryInfo.Delete%2A?displayProperty=nameWithType>
- <xref:System.IO.DirectoryInfo.MoveTo(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Enumeration.FileSystemEntry.Attributes?displayProperty=nameWithType>
<!-- - <xref:System.IO.Enumeration.FileSystemEntry.CreationTime?displayProperty=nameWithType> -->
- <xref:System.IO.Enumeration.FileSystemEntry.CreationTimeUtc?displayProperty=nameWithType>
- <xref:System.IO.Enumeration.FileSystemEntry.IsHidden?displayProperty=nameWithType>
<!-- - <xref:System.IO.Enumeration.FileSystemEntry.LastAccessTime?displayProperty=nameWithType> -->
- <xref:System.IO.Enumeration.FileSystemEntry.LastAccessTimeUtc?displayProperty=nameWithType>
<!-- - <xref:System.IO.Enumeration.FileSystemEntry.LastWriteTime?displayProperty=nameWithType> -->
- <xref:System.IO.Enumeration.FileSystemEntry.LastWriteTimeUtc?displayProperty=nameWithType>
- <xref:System.IO.Enumeration.FileSystemEntry.Length?displayProperty=nameWithType>
- <xref:System.IO.Enumeration.FileSystemEntry.ToFileSystemInfo?displayProperty=nameWithType>
- <xref:System.IO.File.Copy(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Copy(System.String,System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.File.Delete(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Exists(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetAttributes(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetAttributes(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetCreationTime(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetCreationTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetCreationTimeUtc(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetCreationTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastAccessTime(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastAccessTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastAccessTimeUtc(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastAccessTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastWriteTime(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastWriteTime(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastWriteTimeUtc(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetLastWriteTimeUtc(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.GetUnixFileMode(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.IO.File.GetUnixFileMode(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Move(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Move(System.String,System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.File.OpenHandle(System.String,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare,System.IO.FileOptions,System.Int64)?displayProperty=nameWithType>
- <xref:System.IO.File.Replace(System.String,System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Replace(System.String,System.String,System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.File.ReadAllBytes(System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.ReadAllBytesAsync(System.String,System.Threading.CancellationToken)?displayProperty=nameWithType>
- <xref:System.IO.FileInfo.Delete?displayProperty=nameWithType>
- <xref:System.IO.FileInfo.MoveTo(System.String)?displayProperty=nameWithType>
- <xref:System.IO.FileInfo.MoveTo(System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.FileInfo.Replace(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.FileInfo.Replace(System.String,System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.Attributes?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.CreationTime?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.CreationTimeUtc?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.LastAccessTime?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.LastAccessTimeUtc?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.LastWriteTime?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.LastWriteTimeUtc?displayProperty=nameWithType>
<!-- - <xref:System.IO.FileSystemInfo.Length?displayProperty=nameWithType> -->
- <xref:System.IO.FileSystemInfo.Refresh?displayProperty=nameWithType>
- <xref:System.IO.FileSystemInfo.UnixFileMode?displayProperty=nameWithType>
- <xref:System.IO.FileSystemWatcher?displayProperty=nameWithType>
- <xref:System.IO.IsolatedStorage.IsolatedStorageFile.MoveDirectory(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.IsolatedStorage.IsolatedStorageFile.MoveFile(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String,System.Int64)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String,System.Int64,System.IO.MemoryMappedFiles.MemoryMappedFileAccess)?displayProperty=nameWithType>
- <xref:System.IO.Path.Exists(System.String)?displayProperty=nameWithType>
- <xref:System.IO.Pipes.AnonymousPipeClientStream?displayProperty=nameWithType>
- <xref:System.IO.Pipes.AnonymousPipeServerStream?displayProperty=nameWithType>
- <xref:System.IO.Pipes.NamedPipeClientStream?displayProperty=nameWithType>
- <xref:System.IO.Pipes.NamedPipeServerStream?displayProperty=nameWithType>
- <xref:System.IO.RandomAccess.GetLength(Microsoft.Win32.SafeHandles.SafeFileHandle)?displayProperty=nameWithType>
- <xref:System.Formats.Tar.TarWriter.WriteEntry(System.Formats.Tar.TarEntry)?displayProperty=nameWithType>
- <xref:System.Formats.Tar.TarWriter.WriteEntry(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.Formats.Tar.TarWriter.WriteEntryAsync(System.Formats.Tar.TarEntry,System.Threading.CancellationToken)?displayProperty=nameWithType>
- <xref:System.Formats.Tar.TarWriter.WriteEntryAsync(System.String,System.String,System.Threading.CancellationToken)?displayProperty=nameWithType>
- <xref:System.Net.Sockets.Socket.SendPacketsAsync(System.Net.Sockets.SocketAsyncEventArgs)?displayProperty=nameWithType>
- <xref:System.TimeZoneInfo.Local?displayProperty=nameWithType>

Use the string `NSPrivacyAccessedAPICategoryFileTimestamp` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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
                <string>C617.1,...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [File timestamp APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278393) can be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.

### System boot time APIs

The following APIs directly or indirectly access the system boot time and require reasons for use:

- <xref:System.Environment.TickCount?displayProperty=nameWithType>
- <xref:System.Environment.TickCount64?displayProperty=nameWithType>

Use the string `NSPrivacyAccessedAPICategorySystemBootTime` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategorySystemBootTime</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>35F9.1</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [File timestamp APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278394) can be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key. If you only access the system boot time from the list of APIs below, then use the `35F9.1` value in the `NSPrivacyAccessedAPITypeReasons` array.

### Disk space APIs

The following APIs directly or indirectly access the available disk space and require reasons for use:

- <xref:System.IO.DriveInfo.AvailableFreeSpace?displayProperty=nameWithType>
- <xref:System.IO.DriveInfo.DriveFormat?displayProperty=nameWithType>
- <xref:System.IO.DriveInfo.DriveType?displayProperty=nameWithType>
- <xref:System.IO.DriveInfo.TotalFreeSpace?displayProperty=nameWithType>
- <xref:System.IO.DriveInfo.TotalSize?displayProperty=nameWithType>
- <xref:System.IO.File.Copy(System.String,System.String)?displayProperty=nameWithType>
- <xref:System.IO.File.Copy(System.String,System.String,System.Boolean)?displayProperty=nameWithType>
- <xref:System.IO.File.OpenHandle(System.String,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare,System.IO.FileOptions,System.Int64)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String,System.Int64)?displayProperty=nameWithType>
- <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(System.String,System.IO.FileMode,System.String,System.Int64,System.IO.MemoryMappedFiles.MemoryMappedFileAccess)?displayProperty=nameWithType>
- <xref:System.TimeZoneInfo.Local?displayProperty=nameWithType>
- <xref:System.Net.Sockets.Socket.SendPacketsAsync(System.Net.Sockets.SocketAsyncEventArgs)?displayProperty=nameWithType>

Use the string `NSPrivacyAccessedAPICategoryDiskSpace` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if you use any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>NSPrivacyAccessedAPITypes</key>
    <array>
        <dict>
            <key>NSPrivacyAccessedAPIType</key>
            <string>NSPrivacyAccessedAPICategoryDiskSpace</string>
            <key>NSPrivacyAccessedAPITypeReasons</key>
            <array>
                <string>E174.1,...</string>
            </array>
        </dict>
    </array>
</dict>
</plist>
```

Reason codes from [Disk space APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278397) can be provided in the array for the `NSPrivacyAccessedAPITypeReasons` key.
