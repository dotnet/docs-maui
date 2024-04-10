---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 04/11/2024
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is to improve product quality. This includes expanding test coverage, end to end scenario testing, and bug fixing. For more information about the product quality improvements in .NET MAUI 9 Preview, see the following release notes:

- [.NET MAUI 9 Preview 3](https://github.com/dotnet/maui/releases/tag/9.0.100-preview.3.10457)
- [.NET MAUI 9 Preview 2](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.2.10293)
- [.NET MAUI 9 Preview 1](https://github.com/dotnet/maui/releases/tag/9.0.100-preview.1.9973)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 9, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

For information about what's new in .NET 9, see [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9/overview).

## Android

.NET Android 9 Preview, which uses API 34 and JDK 17, includes work to reduce build times, and to improve the trimability of apps to reduce size and improve performance. For more information about .NET Android 9 Preview, see the following release notes:

- [.NET Android 9 Preview 3](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.3.189)
- [.NET Android 9 Preview 2](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.2.189)
- [.NET Android 9 Preview 1](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.1.151)

### Asset packs

.NET Android 9 Preview 3 introduces the ability to place assets into a separate package, known as an *asset pack*. This enables you to upload games and apps that would normally be larger that the basic package size allowed by Google Play. By putting these assets into a separate package you gain the ability to upload a package which is up to 2Gb in size, rather than the basic package size of 200Mb.

> [!IMPORTANT]
> Asset packs can only contain assets. In the case of .NET Android this means items which have the `AndroidAsset` build action.

.NET MAUI defines assets via the `MauiAsset` build action. An asset pack can be specified via the `AssetPack` attribute:

```xml
<MauiAsset
    Include="Resources\Raw\**"
    LogicalName="%(RecursiveDir)%(Filename)%(Extension)"
    AssetPack="myassetpack" />
```

> [!NOTE]
> The additional metadata will be ignored by other platforms.

If you have specific items you want to place in an asset pack you can use the `Update` attribute to define the `AssetPack` metadata:

```xml
<MauiAsset Update="Resources\Raw\MyLargeAsset.txt" AssetPack="myassetpack" />
```

Asset packs can can have different delivery options, which control when your assets will install on the device:

- Install time packs are installed at the same time as the app. This type pack can be up to 1Gb in size, but you can only have one of them. This delivery type is specified with `InstallTime` metadata.
- Fast follow packs will install at some point shortly after the app has finished installing. The app will be able to start while this type of pack is being installed so developers should check it has finished installing before trying to use the assets. This kind of asset pack can be up to 512Mb in size. This delivery type is specified with `FastFollow` metadata.
- On demand packs will never be downloaded to the device unless the app specifically requests it. The total size of all your asset packs cannot exceed 2Gb, and you can have up to 50 separate asset packs. This delivery type is specified with `OnDemand` metadata.

In .NET MAUI apps, the delivery type can be specified with the `DeliveryType` attribute on a `MauiAsset`:

```xml
<MauiAsset Update="Resources\Raw\myvideo.mp4" AssetPack="myassetpack" DeliveryType="FastFollow" />
```

For more information about Android asset packs, see [Android Asset Packs](https://github.com/xamarin/xamarin-android/blob/main/Documentation/guides/AndroidAssetPacks.md).

## iOS

.NET 9 Preview on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 15.2 for the following platform versions:

- iOS: 17.2
- tvOS: 17.2
- Mac Catalyst: 17.2
- macOS: 14.2

For more information about .NET 9 Preview on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 9.0.1xx Preview 3](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview3-9429)
- [.NET 9.0.1xx Preview 2](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview2-9244)
- [.NET 9.0.1xx Preview 1](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview1-9088).

### Bindings

Projects can now multi-target versions of .NET for iOS bindings. For example, a library project may need to build for two distinct versions:

```xml
<TargetFrameworks>net9.0-ios17.0;net9.0-ios17.2</TargetFrameworks>
```

This will produce two libraries, one using iOS 17.0 bindings, and one using iOS 17.2 bindings.

> [!IMPORTANT]
> An app project should always target the latest iOS SDK.

## See also

- [.NET MAUI updates in .NET 9 Preview 3](https://github.com/dotnet/core/blob/main/release-notes/9.0/preview/preview3/dotnetmaui.md).
- [Our Vision for .NET 9](https://devblogs.microsoft.com/dotnet/our-vision-for-dotnet-9/)
