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
| <xref:Foundation.NSFileManager.SetAttributes%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateDirectory%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.CreateFile%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSFileManager.GetAttributes%2A?displayProperty=nameWithType> | | |
| <xref:Foundation.NSDictionary.ToFileAttributes%2A?displayProperty=nameWithType> | | |
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
- <xref:Foundation.NSFileManager.GetFileSystemAttributes%2A?displayProperty=nameWithType>

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

| .NET API | Internal usage | CoreClr usage | Mono usage |
| -------- | -------------- | ------------- | ---------- |
| [System.Diagnostics.FileVersionInfo](https://learn.microsoft.com/dotnet/api/System.Diagnostics.FileVersionInfo) | [Interop.Sys.LStat](https://source.dot.net/#System.Private.CoreLib/src/libraries/Common/src/Interop/Unix/System.Native/Interop.Stat.cs,65) | SystemNative_LStat | g_file_test
| [System.IO.Compression.ZipFile.CreateFromDirectory](https://learn.microsoft.com/dotnet/api/System.IO.Compression.ZipFile.CreateFromDirectory) | [Interop.Sys.Stat](https://source.dot.net/#System.Private.CoreLib/src/libraries/Common/src/Interop/Unix/System.Native/Interop.Stat.cs,62) | SystemNative_Stat | mono_file_map_size
| [System.IO.Directory.CreateDirectory(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.CreateDirectory) | [Interop.Sys.FStat](https://source.dot.net/#System.Private.CoreLib/src/libraries/Common/src/Interop/Unix/System.Native/Interop.Stat.cs,59) | SystemNative_FStat |
| [System.IO.Directory.CreateDirectory(string, UnixFileMode)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.CreateDirectory) | [System.Runtime.Loader.AssemblyLoadContext.ResolveSatelliteAssembly](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Runtime/Loader/AssemblyLoadContext.cs,763)
| [System.IO.Directory.Delete(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.Delete)
| [System.IO.Directory.Exists(string?)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.Exists)
| [System.IO.Directory.GetCreationTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetCreationTime)
| [System.IO.Directory.GetCreationTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetCreationTimeUtc)
| [System.IO.Directory.GetLastAccessTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetLastAccessTime)
| [System.IO.Directory.GetLastAccessTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetLastAccessTimeUtc)
| [System.IO.Directory.GetLastWriteTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetLastWriteTime)
| [System.IO.Directory.GetLastWriteTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.GetLastWriteTimeUtc)
| [System.IO.Directory.Move(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.Directory.Move)
| [System.IO.DirectoryInfo.Delete(string?)](https://learn.microsoft.com/dotnet/api/System.IO.DirectoryInfo.Delete)
| [System.IO.DirectoryInfo.MoveTo(string)](https://learn.microsoft.com/dotnet/api/System.IO.DirectoryInfo.MoveTo)
| [System.IO.Enumeration.FileSystemEntry.Attributes](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.Attributes)
| [System.IO.Enumeration.FileSystemEntry.CreationTime](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.CreationTime)
| [System.IO.Enumeration.FileSystemEntry.CreationTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.CreationTimeUtc)
| [System.IO.Enumeration.FileSystemEntry.IsHidden](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.IsHidden)
| [System.IO.Enumeration.FileSystemEntry.LastAccessTime](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.Attributes)
| [System.IO.Enumeration.FileSystemEntry.LastAccessTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.LastAccessTimeUtc)
| [System.IO.Enumeration.FileSystemEntry.LastWriteTime](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.LastWriteTime)
| [System.IO.Enumeration.FileSystemEntry.LastWriteTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.LastWriteTimeUtc)
| [System.IO.Enumeration.FileSystemEntry.Length](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.Length)
| [System.IO.Enumeration.FileSystemEntry.ToFileSystemInfo()](https://learn.microsoft.com/dotnet/api/System.IO.Enumeration.FileSystemEntry.ToFileSystemInfo)
| [System.IO.File.Copy(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.File.Copy)
| [System.IO.File.Copy(string, string, boolean)](https://learn.microsoft.com/dotnet/api/System.IO.File.Copy)
| [System.IO.File.Delete(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.Delete)
| [System.IO.File.Exists(string?)](https://learn.microsoft.com/dotnet/api/System.IO.File.Exists)
| [System.IO.File.GetAttributes(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetAttributes)
| [System.IO.File.GetAttributes(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetAttributes)
| [System.IO.File.GetCreationTime(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetCreationTime)
| [System.IO.File.GetCreationTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetCreationTime)
| [System.IO.File.GetCreationTimeUtc(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetCreationTimeUtc)
| [System.IO.File.GetCreationTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetCreationTimeUtc)
| [System.IO.File.GetLastAccessTime(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastAccessTime)
| [System.IO.File.GetLastAccessTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastAccessTime)
| [System.IO.File.GetLastAccessTimeUtc(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastAccessTimeUtc)
| [System.IO.File.GetLastAccessTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastAccessTimeUtc)
| [System.IO.File.GetLastWriteTime(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastWriteTime)
| [System.IO.File.GetLastWriteTime(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastWriteTime)
| [System.IO.File.GetLastWriteTimeUtc(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastWriteTimeUtc)
| [System.IO.File.GetLastWriteTimeUtc(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetLastWriteTimeUtc)
| [System.IO.File.GetUnixFileMode(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetUnixFileMode)
| [System.IO.File.GetUnixFileMode(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.GetUnixFileMode)
| [System.IO.File.Move(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.File.Move)
| [System.IO.File.Move(string, string, boolean)](https://learn.microsoft.com/dotnet/api/System.IO.File.Move)
| [System.IO.File.OpenHandle(string, FileMode, FileAccess, FileShare, FileOptions, long)](https://learn.microsoft.com/dotnet/api/System.IO.File.OpenHandle)
| [System.IO.File.Replace(string, string, string)](https://learn.microsoft.com/dotnet/api/System.IO.File.Replace)
| [System.IO.File.Replace(string, string, string, boolean)](https://learn.microsoft.com/dotnet/api/System.IO.File.Replace)
| [System.IO.File.ReadAllBytes(string)](https://learn.microsoft.com/dotnet/api/System.IO.File.ReadAllBytes)
| [System.IO.File.ReadAllBytesAsync(string, CancellationToken)](https://learn.microsoft.com/dotnet/api/System.IO.File.ReadAllBytesAsync)
| [System.IO.FileInfo.Delete()](https://learn.microsoft.com/dotnet/api/System.IO.FileInfo.Delete)
| [System.IO.FileInfo.MoveTo(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.FileInfo.MoveTo)
| [System.IO.FileInfo.MoveTo(string, string, boolean)](https://learn.microsoft.com/dotnet/api/System.IO.FileInfo.MoveTo)
| [System.IO.FileInfo.Replace(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.FileInfo.Replace)
| [System.IO.FileInfo.Replace(string, string, boolean)](https://learn.microsoft.com/dotnet/api/System.IO.FileInfo.Replace)
| [System.IO.FileSystemInfo.Attributes](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.Attributes)
| [System.IO.FileSystemInfo.CreationTime](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.CreationTime)
| [System.IO.FileSystemInfo.CreationTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.CreationTimeUtc)
| [System.IO.FileSystemInfo.LastAccessTime](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.Attributes)
| [System.IO.FileSystemInfo.LastAccessTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.LastAccessTimeUtc)
| [System.IO.FileSystemInfo.LastWriteTime](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.LastWriteTime)
| [System.IO.FileSystemInfo.LastWriteTimeUtc](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.LastWriteTimeUtc)
| [System.IO.FileSystemInfo.Length](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.Length)
| [System.IO.FileSystemInfo.Refresh()](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.Refresh)
| [System.IO.FileSystemInfo.UnixFileMode](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemInfo.UnixFileMode)
| [System.IO.FileSystemWatcher](https://learn.microsoft.com/dotnet/api/System.IO.FileSystemWatcher)
| [System.IO.IsolatedStorage.IsolatedStorageFile.MoveDirectory(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.IsolatedStorage.IsolatedStorageFile.MoveDirectory)
| [System.IO.IsolatedStorage.IsolatedStorageFile.MoveFile(string, string)](https://learn.microsoft.com/dotnet/api/System.IO.IsolatedStorage.IsolatedStorageFile.MoveFile)
| [System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(string)](https://learn.microsoft.com/dotnet/api/System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile)
| [System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(string, FileMode)](https://learn.microsoft.com/dotnet/api/System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile)
| [System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(string, FileMode, string?)](https://learn.microsoft.com/dotnet/api/System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile)
| [System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(string, FileMode, string?, long)](https://learn.microsoft.com/dotnet/api/System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile)
| [System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(string, FileMode, string?, long, MemoryMappedFileAccess)](https://learn.microsoft.com/dotnet/api/System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile)
| [System.IO.Path.Exists(string?)](https://learn.microsoft.com/dotnet/api/System.IO.Path.Exists)
| [System.IO.Pipes.AnonymousPipeClientStream](https://learn.microsoft.com/dotnet/api/System.IO.Pipes.AnonymousPipeClientStream)
| [System.IO.Pipes.AnonymousPipeServerStream](https://learn.microsoft.com/dotnet/api/System.IO.Pipes.AnonymousPipeServerStream)
| [System.IO.Pipes.NamedPipeClientStream](https://learn.microsoft.com/dotnet/api/System.IO.Pipes.NamedPipeClientStream)
| [System.IO.Pipes.NamedPipeServerStream](https://learn.microsoft.com/dotnet/api/System.IO.Pipes.NamedPipeServerStream)
| [System.IO.RandomAccess.GetLength(SafeFileHandle)](https://learn.microsoft.com/dotnet/api/System.IO.RandomAccess.GetLength)
| [System.Formats.Tar.TarWriter.WriteEntry(TarEntry)](https://learn.microsoft.com/dotnet/api/System.Formats.Tar.TarWriter.WriteEntry)
| [System.Formats.Tar.TarWriter.WriteEntry(string, string)](https://learn.microsoft.com/dotnet/api/System.Formats.Tar.TarWriter.WriteEntry)
| [System.Formats.Tar.TarWriter.WriteEntryAsync(TarEntry, CancellationToken)](https://learn.microsoft.com/dotnet/api/System.Formats.Tar.TarWriter.WriteEntryAsync)
| [System.Formats.Tar.TarWriter.WriteEntryAsync(string, string, CancellationToken)](https://learn.microsoft.com/dotnet/api/System.Formats.Tar.TarWriter.WriteEntryAsync)
| [System.Net.Sockets.Socket.SendPacketsAsync(SocketAsyncEventArgs)](https://learn.microsoft.com/dotnet/api/System.Net.Sockets.Socket.SendPacketsAsync)
| [System.TimeZoneInfo.Local](https://learn.microsoft.com/dotnet/api/System.TimeZoneInfo.Local)

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

| .NET API | Internal usage | CoreClr usage | Mono usage |
| -------- | -------------- | ------------- | ---------- |
| [System.Environment.TickCount](https://learn.microsoft.com/dotnet/api/System.Environment.TickCount) | | | mono_msec_boottime
| [System.Environment.TickCount64](https://learn.microsoft.com/dotnet/api/System.Environment.TickCount64) | | | mono_domain_finalize
| | | | mono_join_uninterrupted
| | | | mono_msec_ticks
| | | | mono_100ns_ticks
| | | | threads_wait_pending_joinable_threads
| | | | current_time

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

| .NET API | Internal usage | CoreClr usage | Mono usage |
| -------- | -------------- | ------------- | ---------- |
| <xref:System.IO.DriveInfo.AvailableFreeSpace?displayProperty=nameWithType> | [`Interop.Sys.TryGetFileSystemType`](https://source.dot.net/#System.Private.CoreLib/src/libraries/Common/src/Interop/Unix/System.Native/Interop.UnixFileSystemTypes.cs,155) | `SystemNative_GetFileSystemType` | |
| <xref:System.IO.DriveInfo.DriveFormat?displayProperty=nameWithType> | [`Interop.Sys.GetSpaceInfoForMountPoint`](https://source.dot.net/#System.IO.FileSystem.DriveInfo/src/libraries/Common/src/Interop/Unix/System.Native/Interop.MountPoints.FormatInfo.cs,34) | `SystemNative_GetSpaceInfoForMountPoint` | |
| <xref:System.IO.DriveInfo.DriveType?displayProperty=nameWithType> | [`Interop.Sys.GetFormatInfoForMountPoint`](https://source.dot.net/#System.IO.FileSystem.DriveInfo/src/libraries/Common/src/Interop/Unix/System.Native/Interop.MountPoints.FormatInfo.cs,37) | `SystemNative_GetFormatInfoForMountPoint` |
| <xref:System.IO.DriveInfo.TotalFreeSpace?displayProperty=nameWithType> | | | |
| <xref:System.IO.DriveInfo.TotalSize?displayProperty=nameWithType> | | | |
| <xref:System.IO.File.Copy(String, String)?displayProperty=nameWithType> | | | |
| <xref:System.IO.File.Copy(String, String, Boolean)?displayProperty=nameWithType> | | | |
| <xref:System.IO.File.OpenHandle(String, FileMode, FileAccess, FileShare, FileOptions, Int64)?displayProperty=nameWithType> | | | |
| <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(String)?displayProperty=nameWithType> | | | |
| <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(String, FileMode)?displayProperty=nameWithType> | | | |
| <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(String, FileMode, String)?displayProperty=nameWithType> | | | |
| <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(String, FileMode, String, Int64)?displayProperty=nameWithType> | | | |
| <xref:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(String, FileMode, String, Int64, MemoryMappedFileAccess)?displayProperty=nameWithType> | | | |
| <xref:System.TimeZoneInfo.Local?displayProperty=nameWithType> | | | |
| <xref:System.Net.Sockets.Socket.SendPacketsAsync(SocketAsyncEventArgs)?displayProperty=nameWithType> | | | |

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
