---
title: "Upgrading Xamarin.Forms Overview"
description: "Overview for the steps taken to upgrade a project from Xamarin.Forms to .NET MAUI."
ms.date: 1/20/2023
---

# Upgrade a Xamarin.Forms app to .NET MAUI

The upgrade of a Xamarin.Forms project follows the same steps as a Xamarin.Android and Xamarin.iOS projects, with a few additional steps to take advantage of changes in .NET MAUI.

**Checklist:**

* Update to SDK Style projects
* [Find and replace namespace changes](forms-namechanges.md)
* [Configure .NET MAUI](forms-configuremaui.md)
* Upgrade or replace incompatible dependencies with .NET 6 (or newer) versions
* Compile and test your app

To simplify the upgrade, we recommend creating a new .NET project of the same type and name as your Xamarin project, and then copying in your code.

## Update to SDK Style projects

SDK Style projects are the same project format used by all other .NET workloads, and compared to Xamarin projects they are much less verbose. For details updating your application projects, refer to the [Xamarin Projects](xamarin-projects.md) guidance. This document will describe the changes specific to your Xamarin.Forms library projects.

### Create new project

From Visual Studio 2022 create a new .NET MAUI class library project to host the code from your Xamarin.Forms library project.

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net7.0;net7.0-android;net7.0-ios;net7.0-maccatalyst</TargetFrameworks>
        <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net7.0-windows10.0.19041.0</TargetFrameworks>
        <!-- Uncomment to also build the tizen app. You will need to install tizen by following this: https://github.com/Samsung/Tizen.NET -->
        <!-- <TargetFrameworks>$(TargetFrameworks);net7.0-tizen</TargetFrameworks> -->
        <UseMaui>true</UseMaui>
        <SingleProject>true</SingleProject>
        <ImplicitUsings>enable</ImplicitUsings>

        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">14.2</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">14.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</SupportedOSPlatformVersion>
        <TargetPlatformMinVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</TargetPlatformMinVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'tizen'">6.5</SupportedOSPlatformVersion>
    </PropertyGroup>

</Project>
```

In your application projects, add a reference to this library project. Copy your Xamarin.Forms library project files into this new .NET MAUI library project. Now you are ready to move on to updating your namespaces.

Next: [Find and replace namespace changes](forms-namechanges.md)

### See also

* [Upgrading Xamarin.Android, Xamarin.iOS, and Xamarin.Mac](xamarin-projects.md)
* [Default value and name changes](defaults.md)
* [Layouts reference](layouts.md)
* [Using custom renderers](using-custom-renderers.md)
