---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 08/07/2024
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is to improve product quality. This includes expanding test coverage, end to end scenario testing, and bug fixing. For more information about the product quality improvements in .NET MAUI 9 Preview, see the following release notes:

- [.NET MAUI 9 Preview 6](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.6.24327.7)
- [.NET MAUI 9 Preview 5](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.5.24307.10)
- [.NET MAUI 9 Preview 4](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.4.10690)
- [.NET MAUI 9 Preview 3](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.3.10457)
- [.NET MAUI 9 Preview 2](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.2.10293)
- [.NET MAUI 9 Preview 1](https://github.com/dotnet/maui/releases/tag/9.0.100-preview.1.9973)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 9, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

## Blazor Hybrid

.NET MAUI 9 Preview 5 adds a **.NET MAUI Blazor Hybrid and Web App** project template to Visual Studio that creates a solution with a .NET MAUI Blazor Hybrid app with a Blazor Web app, which share common code in a Razor class library project.

The template can also be used from `dotnew new`:

```dotnetcli
dotnet new maui-blazor-web -n AllTheTargets
```

## Control enhancements

.NET MAUI in .NET 9 also includes control enhancements.

### Soft keyboard input support

.NET MAUI 9 Preview 4 adds new soft keyboard input support for `Password`, `Date`, and `Time`. These can be enabled on <xref:Microsoft.Maui.Controls.Editor> and <xref:Microsoft.Maui.Controls.Entry> controls:

```xaml
<Entry Keyboard="Date" />
```

### TimePicker

<xref:Microsoft.Maui.Controls.TimePicker> gains a <xref:Microsoft.Maui.Controls.TimePicker.TimeSelected> event, which is raised when the selected time changes. The <xref:Microsoft.Maui.Controls.TimeChangedEventArgs> object that accompanies the `TimeSelected` event has `NewTime` and `OldTime` properties, which specify the new and old time, respectively.

## Multi-window support

.NET MAUI 9 Preview 7 adds the ability to bring a specific window to the front on Mac Catalyst and Windows with the `Application.Current.ActivateWindow` method:

```csharp
Application.Current!.ActivateWindow(windowToActivate);
```

## Android

.NET for Android 9 Preview, which adds support for API 35, includes work to reduce build times, and to improve the trimability of apps to reduce size and improve performance. For more information about .NET for Android 9 Preview, see the following release notes:

- [.NET for Android 9 Preview 6](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.6.340)
- [.NET for Android 9 Preview 5](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.5.308)
- [.NET for Android 9 Preview 4](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.4.272)
- [.NET for Android 9 Preview 3](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.3.231)
- [.NET for Android 9 Preview 2](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.2.189)
- [.NET for Android 9 Preview 1](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.1.151)

### Asset packs

.NET for Android 9 Preview 3 introduces the ability to place assets into a separate package, known as an *asset pack*. This enables you to upload games and apps that would normally be larger than the basic package size allowed by Google Play. By putting these assets into a separate package you gain the ability to upload a package which is up to 2Gb in size, rather than the basic package size of 200Mb.

> [!IMPORTANT]
> Asset packs can only contain assets. In the case of .NET for Android this means items that have the `AndroidAsset` build action.

.NET MAUI apps define assets via the `MauiAsset` build action. An asset pack can be specified via the `AssetPack` attribute:

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

Asset packs can have different delivery options, which control when your assets will install on the device:

- Install time packs are installed at the same time as the app. This pack type can be up to 1Gb in size, but you can only have one of them. This delivery type is specified with `InstallTime` metadata.
- Fast follow packs will install at some point shortly after the app has finished installing. The app will be able to start while this type of pack is being installed so you should check it has finished installing before trying to use the assets. This kind of asset pack can be up to 512Mb in size. This delivery type is specified with `FastFollow` metadata.
- On demand packs will never be downloaded to the device unless the app specifically requests it. The total size of all your asset packs can't exceed 2Gb, and you can have up to 50 separate asset packs. This delivery type is specified with `OnDemand` metadata.

In .NET MAUI apps, the delivery type can be specified with the `DeliveryType` attribute on a `MauiAsset`:

```xml
<MauiAsset Update="Resources\Raw\myvideo.mp4" AssetPack="myassetpack" DeliveryType="FastFollow" />
```

For more information about Android asset packs, see [Android Asset Packs](https://github.com/xamarin/xamarin-android/blob/main/Documentation/guides/AndroidAssetPacks.md).

### Android 15 beta support

.NET for Android Preview 4 adds .NET bindings for the first beta of Android 15 (API 35) codenamed "Vanilla Ice Cream". To build for these APIs, update the target framework of your project:

```xml
<TargetFramework>net9.0-android35</TargetFramework>
```

.NET for Android Preview 5 extends these bindings to Android 15 beta 2, with improvements for startup performance and app size.

### LLVM marshalled methods

Low-level Virtual Machine (LLVM) marshalled methods are now enabled by default in .NET for Android Preview 5 in non-Blazor apps. This has resulted in a [~10% improvement in performance in a test app](https://github.com/xamarin/xamarin-android/pull/8925).

LLVM marshalled methods can be disabled in your project file (*.csproj*):

```xml
<PropertyGroup Condition="'$(TargetFramework)' == 'net9.0-android'">
    <AndroidEnableLLVM>false</AndroidEnableLLVM>
    <AndroidEnableLLVMOptimizations>false</AndroidEnableLLVMOptimizations>
</PropertyGroup>
```

### Trimming enhancements

.NET for Android Preview 5 includes fixes for when using full trimming to reduce app size. Full trimming is usually only enabled for release builds of your app, and can be configured in your project file (*.csproj*):

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release' And '$(TargetFramework)' == 'net9.0-android'">
    <TrimMode>Full</TrimMode>
</PropertyGroup>
```

## iOS

.NET 9 Preview on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 15.2 for the following platform versions:

- iOS: 17.2
- tvOS: 17.2
- Mac Catalyst: 17.2
- macOS: 14.2

For more information about .NET 9 Preview on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 9.0.1xx Preview 6](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview6-9714)
- [.NET 9.0.1xx Preview 5](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview5-9639)
- [.NET 9.0.1xx Preview 4](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview4-9523)
- [.NET 9.0.1xx Preview 3](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview3-9429)
- [.NET 9.0.1xx Preview 2](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview2-9244)
- [.NET 9.0.1xx Preview 1](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview1-9088)

### Bindings

.NET for iOS 9 Preview 3 introduces the ability to multi-target versions of .NET for iOS bindings. For example, a library project may need to build for two distinct iOS versions:

```xml
<TargetFrameworks>net9.0-ios17.0;net9.0-ios17.2</TargetFrameworks>
```

This will produce two libraries, one using iOS 17.0 bindings, and one using iOS 17.2 bindings.

> [!IMPORTANT]
> An app project should always target the latest iOS SDK.

### Native AOT for iOS & Mac Catalyst

In .NET for iOS 9 Preview 4, native Ahead of Time (AOT) compilation for iOS and Mac Catalyst takes advantage of full trimming to reduce your app's package size and startup performance. This is a publishing feature that you can use when you're ready to ship your app.

> [!IMPORTANT]
> Your app and it's dependencies must be fully trimmable in order to utilize this feature.

## See also

- [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9/overview).
- [Our Vision for .NET 9](https://devblogs.microsoft.com/dotnet/our-vision-for-dotnet-9/)
