---
title: What's new in .NET MAUI for .NET 10
description: Learn about the new features introduced in .NET MAUI for .NET 10.
ms.date: 02/18/2025
---

# What's new in .NET MAUI for .NET 10

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 10 is to improve product quality. For more information about the product quality improvements in .NET MAUI in .NET 10, see the following release notes:

- [.NET MAUI in .NET 10 Preview 1](https://github.com/dotnet/maui/releases/tag/10.0.0-preview.1.9973)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 10, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

## Control enhancements

.NET MAUI in .NET 10 includes control enhancements.

### CollectionView and CarouselView

.NET MAUI in .NET 9 included two optional handlers on iOS and Mac Catalyst that brought performance and stability improvements to <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>.

In .NET 10, these are the default handlers for <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>.

## .NET for Android

.NET for Android in .NET 10 adds support for API 36 and JDK 21, and includes work to reduce build times and improve performance. For more information about .NET for Android in .NET 10, see the following release notes:

- [.NET for Android 10 Preview 1](https://github.com/dotnet/android/releases/tag/LINK)

### Android 16 (Baklava) beta 1 bindings

Google has released [Beta 1](https://android-developers.googleblog.com/2025/01/first-beta-android16.html) of the Android 16 (API-36) SDK.  Support has been adding for using these preview APIs.

To target the Android 16 preview API:

- Use the Android SDK Manager to download the Android 16 (Baklava) platform.
- Update your project's `TargetFramework` to `net10.0-android36`.

### Update recommended minimum supported Android API

The .NET for Android project templates have been updated to specify 24 (Nougat) as the default `$(SupportedOSPlatformVersion)` instead of 21 (Lollipop). This prevents you from hitting "desugaring" runtime crashes when using Java default interface methods.

While API 21 is still supported in .NET 10, it's recommended to update existing projects to API 24 to avoid unexpected runtime errors.

### Support for `dotnet run`

Previously, the `dotnet run` command wasn't supported for .NET for Android projects because it didn't accept parameters needed to specify which Android device or emulator to use.

In .NET 10, .NET for Android projects can be run using the `dotnet run` command:

```dotnetcli
// Run on the only attached Android physical device
dotnet run -p:AdbTarget=-d

// Run on the only running Android emulator
dotnet run -p:AdbTarget=-e

// Run on the specified Android physical device or emulator
dotnet run -p:AdbTarget="-s emulator-5554"
```

The `$(AdbTarget)` property is passed to `adb`. For more information, see [Issue shell commands](https://developer.android.com/tools/adb#shellcommands) on developer.android.com.

### Enable marshal methods by default

In .NET 9, a [new way](https://github.com/dotnet/android/pull/7351) of creating the marshal methods needed for Java calling into C# code provided startup performance improvements. However, in .NET 9 they were off by default.

In .NET 10, they are enabled by default. Problems with these marshal methods often manifest as a hang at startup. If you're getting a hang on startup on .NET 10 previews that you didn't see on .NET 9, try disabling marshal methods by setting the `$(AndroidEnableMarshalMethods)` MSBuild property to `false` in your project file:

```xml
<PropertyGroup>
    <AndroidEnableMarshalMethods>false</AndroidEnableMarshalMethods>
<PropertyGroup>
```

If this fixes the hang, please file an [issue](https://github.com/dotnet/android/issues).

### Add `ArtifactFilename` metadata for `@(AndroidMavenLibrary)` item

[`@(AndroidMavenLibrary)`](/dotnet/android/binding-libs/binding-java-libs/binding-java-maven-library) was added in .NET 9 and allows a Java library to be automatically downloaded from Maven to be bound. Generally, this library is named `{artifact.Id}-{artifact.Version}.[jar|aar]`. However, this doesn't follow a standard and could be arbitrarily different.

In .NET 10 you can add the `ArtifactFilename` metadata to the `@(AndroidMavenLibrary)` MSBuild item to allow an alternative filename:

```xml
<ItemGroup>
    <AndroidMavenLibrary Include="com.facebook.react:react-android" Version="0.76.0" ArtifactFilename="react-android-0.76.0-release.aar" />
</ItemGroup>
```

### Visual Studio design time builds no longer invoke `aapt2`

In order to speed up design time builds, `aapt2` is no longer invoked. Instead, the `.aar` files and underlying Android resources are parsed directly. This reduces the time of a design time build for some unit tests from over 2s to under 600ms.

### Building with JDK 21 is now supported

.NET for Android projects can now be built with JDK 21.

### `generator` output now avoids potential System.Reflection.Emit usage

App startup and overall performance has been optimized by removing codepaths that may hit `System.Reflection.Emit` from "Java calling into C#" codepaths.

### Fixed `InvalidCastException` when using `ApplicationAttribute.ManageSpaceActivity`

Trying to set the `ApplicationAttribute.ManageSpaceActivity` property would result in an XAGJS7007 error. This has been fixed.

## .NET for iOS

.NET 10 on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 17.0+ for the following platform versions:

- iOS: 18.2
- tvOS: 18.2
- Mac Catalyst: 18.2
- macOS: 15.2

For more information about .NET 10 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 10.0.1xx Preview 1](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-10.0.1xx-preview1-9088)

### Trimmer warnings enabled by default

Trimmer warnings were previously suppressed, because the base class library produced trimmer warnings which means that it wasn't possible for you to fix all the trimmer warnings. However, in .NET 9 all the iOS trimmer warnings were fixed, and so trimmer warnings are now enabled by default. To disable trimmer warnings, set the `$(SuppressTrimAnalysisWarnings)` MSBuild property to `true` in your project file:

```xml
<PropertyGroup>
    <SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings>
</PropertyGroup>
```

### Bundling original resources in libraries

Library projects can have different types of bundle resources, such as storyboards, xibs, property lists, images, CoreML models, and texture atlases, and they're bundled into the compiled library as embedded resources.

Processing these resources, such as compiling storyboards or xibs, or optimizing property lists and images, is done before embedding but this complicates library builds because the processing:

- Needs to run on a Mac, because compiling storyboards/xibs can only be done on a Mac.
- Needs Apple's tool chain.
- Makes it impossible to perform decision-making based on the original resources when building the app.

Therefore, opt-in support for embedding the original resource in libraries was added in .NET 9, and it's now opt-out in .NET 10. To opt out of this behavior, set the `$(BundleOriginalResources)` MSBuild property to `false` in your project file:

```xml
<PropertyGroup>
    <BundleOriginalResources>false</BundleOriginalResources>
</PropertyGroup>
```

## See also

- [.NET MAUI roadmap](https://github.com/dotnet/maui/wiki/Roadmap)
- [What's new in .NET 10](/dotnet/core/whats-new/dotnet-10/overview)
