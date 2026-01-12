---
title: "Material 3 design on Android"
description: "Learn how to enable Material 3 design on Android in .NET MAUI apps by setting the UseMaterial3 build property to apply modern Material Design theming and components."
ms.date: 01/12/2026
---

# Material 3 design on Android

Material 3 (also known as Material You) is the latest evolution of Google's Material Design system, offering a more personalized and adaptive user interface. In .NET Multi-platform App UI (.NET MAUI), Material 3 design is available on the Android platform but is not enabled by default.

## Overview

Material 3 introduces several improvements over Material 2, including:

- Dynamic color schemes that adapt to user preferences and system themes
- Updated component designs with refined shapes and elevations
- Enhanced accessibility features
- More flexible customization options

Without enabling Material 3, your .NET MAUI Android app will continue to use Material 2 styles, which may not provide the latest design patterns and user experience enhancements.

## Enable Material 3

To enable Material 3 design on Android, set the `UseMaterial3` build property to `true` in your project file (.csproj).

Add the following property to a `<PropertyGroup>` in your project file:

```xml
<PropertyGroup>
  <UseMaterial3>true</UseMaterial3>
</PropertyGroup>
```

### Full example

The following example shows a complete project file with the `UseMaterial3` property enabled:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst</TargetFrameworks>
    <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net9.0-windows10.0.19041.0</TargetFrameworks>
    
    <OutputType>Exe</OutputType>
    <RootNamespace>MyMauiApp</RootNamespace>
    <UseMaui>true</UseMaui>
    <SingleProject>true</SingleProject>
    <ImplicitUsings>enable</ImplicitUsings>
    
    <!-- Enable Material 3 design on Android -->
    <UseMaterial3>true</UseMaterial3>
    
    <!-- Display name -->
    <ApplicationTitle>MyMauiApp</ApplicationTitle>
    
    <!-- App Identifier -->
    <ApplicationId>com.companyname.mymauiapp</ApplicationId>
    
    <!-- Versions -->
    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
    <ApplicationVersion>1</ApplicationVersion>
    
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">15.0</SupportedOSPlatformVersion>
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">15.0</SupportedOSPlatformVersion>
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</SupportedOSPlatformVersion>
    <TargetPlatformMinVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</TargetPlatformMinVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
  </ItemGroup>

</Project>
```

### Platform-specific configuration

If you want to enable Material 3 only for specific configurations (for example, only in Release builds), you can use conditional property groups:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)'=='Release|net9.0-android'">
  <UseMaterial3>true</UseMaterial3>
</PropertyGroup>
```

## Platform availability

The `UseMaterial3` build property is specific to the Android platform. It has no effect on other platforms such as iOS, macOS, or Windows. On these platforms, .NET MAUI apps use the native design systems and controls specific to each platform.

> [!NOTE]
> Material 3 is only available on Android. The `UseMaterial3` property is ignored on other platforms.

## Considerations

When enabling Material 3 in your .NET MAUI Android app, consider the following:

- **Visual changes**: Enabling Material 3 will change the appearance of UI controls throughout your app. Test your app's UI thoroughly to ensure the new styles work well with your design.
- **Dynamic theming**: Material 3 supports dynamic color schemes based on the user's wallpaper and preferences. Ensure your app's custom colors and themes work well with this feature.
- **Backward compatibility**: Material 3 requires Android 5.0 (API level 21) or higher, which is the minimum version supported by .NET MAUI.
- **Default behavior**: If the `UseMaterial3` property is not set or is set to `false`, your app will use Material 2 design by default.

## Related links

- [Theme an app](theming.md)
- [Respond to system theme changes](system-theme-changes.md)
- [Material Design 3 (Google)](https://m3.material.io/)
