---
title: "Apple privacy manifest"
description: Learn how to include a privacy manifest file in your .NET MAUI app on iOS or iPadOS.
ms.date: 03/19/2024
---

# Apple privacy manifest

Apple has a privacy policy for apps that target iOS and Mac Catalyst on the App Store. It requires the app to include a privacy manifest file in the app bundle, that lists the types of data your .NET MAUI app, or any third-party SDKs and packages collect, and the reasons for using certain required reason APIs. If your use of the required reason APIs, or third-party SDKs, isn’t declared in the privacy manifest, your app might be rejected by the App Store. For more information about privacy manifest files, see [Privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) on developer.apple.com.

Depending on whether you’re using .NET MAUI to develop an app or providing Binding packages to use with .NET MAUI apps, the requirement for providing a privacy manifest file might differ.

## Privacy manifests for .NET MAUI apps  

All .NET MAUI apps that target devices running iOS or iPadOS will require a privacy manifest file in the app bundle. This is due to the .NET runtime and Base Class Library (BCL) using required reason APIs that aren't removed regardless of the linker mode. For more information, see [Add required .NET MAUI entries to the privacy manifest](#add-required-net-maui-entries-to-the-privacy-manifest).

You'll also need to review your own code, any native code, and data collection and tracking practices and update the privacy manifest file accordingly:

- If your app or SDK collects data about the person using the app, you'll need to describe the data use in a privacy manifest. For information, see [Describing data use in privacy manifests](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_data_use_in_privacy_manifests) on developer.apple.com.
- If your app or SDK includes .NET APIs that call Apple's required reason APIs, then you must assess your use of each of these APIs and declare the reasons for using them. For more information about the required reasons APIs, see [Describing use of required reason API](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api) on developer.apple.com.

> [!NOTE]
> If your app includes any third-party SDKs or packages, then these third-party components must include their own privacy manifest files separately.

For more information about creating a privacy manifest file, see [Create a privacy manifest file](#create-a-privacy-manifest-file).

> [!IMPORTANT]
> The above guidelines are provided for your convenience. It’s important that you review Apple’s documentation on [privacy manifest files](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files) before creating a privacy manifest for your project.

## Privacy manifest for Binding projects

If you are Binding project owner, and you are binding a `xcframework`, then the `xcframework` provider will need to include the privacy manifest file as part of the `xcframework`. Alternatively, you can provide documentation for package consumers to create the privacy manifest file or change the bindings to bind an `xcframework` that has the privacy manifest file included. It's not currently possible for Binding project authors to include a privacy manifest file outside an `xcframework` that will be recognized by Apple when submitting an app.

## Create a privacy manifest file

To add a privacy manifest file to your .NET MAUI app project, add a new XML file named *PrivacyInfo.xcprivacy* to the *Platforms/iOS* or *Platforms/MacCatalyst* folder of your app project. Ensure that the *PrivacyInfo.xcprivacy* file doesn't have an *.xml* extension. Add the following XML to the file:

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

Then, if required edit the .NET MAUI app project file (*.csproj) and add the following build item for iOS at the bottom of the root `<Project>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">
    <BundleResource Include="Platforms\iOS\PrivacyInfo.xcprivacy" LogicalName="PrivacyInfo.xcprivacy" />
</ItemGroup>
```

Then, if required edit the .NET MAUI app project file (*.csproj) and add the following build item for Mac Catalyst at the bottom of the root `<Project>` element:

```xml
<ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">
    <BundleResource Include="Platforms\MacCatalyst\PrivacyInfo.xcprivacy" LogicalName="PrivacyInfo.xcprivacy" />
</ItemGroup>
```

This will ensure that the privacy manifest file is packaged into the iOS or Mac Catalyst app at the root of the bundle.

## Add required .NET MAUI entries to the privacy manifest

The three API categories and their associated reasons that must be in the privacy manifest file in a .NET MAUI app are shown in the following table:

| API category | Reason | Link |
| ------------ | ------ | ---- |
| `NSPrivacyAccessedAPICategoryFileTimestamp` | `C617.1` | [File timestamp APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278393) |
| `NSPrivacyAccessedAPICategorySystemBootTime` | `35F9.1` | [System boot time APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278394) |
| `NSPrivacyAccessedAPICategoryDiskSpace` | `E174.1` | [Disk space APIs](https://developer.apple.com/documentation/bundleresources/privacy_manifest_files/describing_use_of_required_reason_api#4278397) |

If you use the <xref:Foundation.NSUserDefaults> API in your app, you'll need to add the `NSPrivacyAccessedAPICategoryUserDefaults` API category, with a reason code of `CAS92.1`.

Using a text editor add the `NSPrivacyAccessAPITypes` key, where each required reason API category usage will be added, to the privacy manifest file:

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

Add the `NSPrivacyAccessedAPICategoryFileTimestamp` category with reason `C617.1`, `NSPrivacyAccessedAPICategorySystemBootTime` category with reason `35F9.1`, and `NSPrivacyAccessedAPICategoryDiskSpace` category with reason `E174.1`, to the `NSPrivacyAccessedAPITypes` array in the privacy manifest file:

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
    </array>
</dict>
</plist>
```
For the <xref:Foundation.NSFileManager.ModificationDate?displayProperty=nameWithType> API, a reason code of `C617.1` is needed since modification dates are stored as a hash using <xref:Foundation.NSUserDefaults>, even though they aren't displayed to users.

> [!IMPORTANT]
> An app's *PrivacyInfo.xcprivacy* file will need to be updated if there are any additional API usages from additional categories or additional reasons for usage. This will include adding a NuGet package or Binding project to your app that calls into any of Apple’s required reason APIs.

## Required reasons API usage in .NET MAUI

The APIs in this section list .NET MAUI APIs that call the required reason APIs. If your app or SDK calls any of the APIs in this section, you must declare the reasons for their use in your privacy manifest file.

> [!NOTE]
> The following APIs are verified for .NET MAUI versions 8.0.0 and later.

### User defaults APIs

.NET MAUI's preferences API uses the <xref:Foundation.NSUserDefaults> API to access user defaults. For more information about the preferences API, see [Preferences](~/platform-integration/storage/preferences.md).

If your .NET MAUI app or SDK uses this API, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryUserDefaults` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK directly or indirectly uses the <xref:Foundation.NSUserDefaults> API, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The APIs in this section list .NET iOS APIs that call the required reason APIs. If your app or SDK calls any of the APIs in this section, you must declare the reasons for their use in your privacy manifest file.

> [!NOTE]
> The following APIs are verified for .NET iOS version 8.0.4 and later.

### File timestamp APIs

The following APIs directly or indirectly access file timestamps:

| Foundation APIs | UIKit APIs | AppKit APIs |
| --------------- | ---------- | ----------- |
| <xref:Foundation.NSFileManager.CreationDate?displayProperty=nameWithType> | <xref:UIKit.UIDocument.FileModificationDate?displayProperty=nameWithType> | <xref:AppKit.NSDocument.FileModificationDate?displayProperty=nameWithType> |
| <xref:Foundation.NSFileManager.ModificationDate?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.SetAttributes%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateDirectory%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateFile(System.String,Foundation.NSData,Foundation.NSDictionary)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateFile(System.String,Foundation.NSData,Foundation.NSFileAttributes)?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.GetAttributes%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSDictionary.ToFileAttributes?displayProperty=nameWithType> | | |
| <xref:Foundation.NSUrl.ContentModificationDateKey?displayProperty=nameWithType> | | |
| <xref:Foundation.NSUrl.CreationDateKey?displayProperty=nameWithType> | | |

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file.

Use the string `NSPrivacyAccessedAPICategoryFileTimestamp` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The <xref:Foundation.NSProcessInfo.SystemUptime> API directly or indirectly accesses the system boot time.

If your .NET MAUI app or SDK uses this API, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategorySystemBootTime` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses the <xref:Foundation.NSProcessInfo.SystemUptime> API, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The following <xref:Foundation> APIs directly or indirectly access the available disk space:

- <xref:Foundation.NSUrl.VolumeAvailableCapacityKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeAvailableCapacityForImportantUsageKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeAvailableCapacityForOpportunisticUsageKey?displayProperty=nameWithType>
- <xref:Foundation.NSUrl.VolumeTotalCapacityKey?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.SystemFreeSize?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.SystemSize?displayProperty=nameWithType>
- <xref:Foundation.NSFileManager.GetFileSystemAttributes%2A?displayProperty=nameWithType>

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryDiskSpace` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The `AppKit.UITextInputMode.ActiveInputModes` API directly or indirectly accesses the list of available keyboards.

If your .NET MAUI app or SDK uses this API, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryActiveKeyboards` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses the `AppKit.UITextInputMode.ActiveInputModes` API, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The following APIs directly or indirectly access user defaults:

| Foundation APIs | UIKit APIs | AppKit APIs |
| --------------- | ---------- | ----------- |
| <xref:Foundation.NSUserDefaults> |  | `AppKit.NSUserDefaultsController.NSUserDefaultsController` |
|  |  | <xref:AppKit.NSUserDefaultsController.Defaults?displayProperty=nameWithType> |
|  |  | `AppKit.NSUserDefaultsController.SharedUserDefaultsController` |

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryUserDefaults` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

## Required reasons API usage in .NET

The APIs in this section list .NET APIs that call the required reason APIs. If your app or SDK calls any of the APIs in this section, you must declare the reasons for their use in your privacy manifest file.

> [!NOTE]
> The following APIs are verified for .NET versions 8.0.0 and later.

### File timestamp APIs

The following APIs directly or indirectly access file timestamps:

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

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryFileTimestamp` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The following APIs directly or indirectly access the system boot time:

- <xref:System.Environment.TickCount?displayProperty=nameWithType>
- <xref:System.Environment.TickCount64?displayProperty=nameWithType>

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategorySystemBootTime` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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

The following APIs directly or indirectly access the available disk space:

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

If your .NET MAUI app or SDK uses any of these APIs, you must include reasons for use in a privacy manifest file. Use the string `NSPrivacyAccessedAPICategoryDiskSpace` as the value for the `NSPrivacyAccessedAPIType` key in your `NSPrivacyAccessedAPITypes` dictionary. For example, if your app or SDK uses any of the APIs listed above, your *PrivacyInfo.xcprivacy* file should contain the following `dict` element in the `NSPrivacyAccessedAPITypes` key's array:

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
