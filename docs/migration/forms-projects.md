---
title: "Xamarin.Forms project migration"
description: "Learn how to migrate a Xamarin.Forms project to .NET MAUI."
ms.date: 1/31/2023
---

# Xamarin.Forms project migration

Updating a Xamarin.Forms app to be a .NET Multi-platform App UI (.NET MAUI) app follows the same steps as a Xamarin.Android and Xamarin.iOS project, with additional steps to take advantage of changes in .NET MAUI.

SDK-style projects are the same project format used by all other .NET workloads, and compared to many Xamarin projects are much less verbose. For information about updating your app projects, see [Update Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md). This article describes how to manually migrate a Xamarin.Forms library project to .NET.

To migrate a Xamarin.Forms library project to .NET, you must:

> [!div class="checklist"]
>
> - Update to SDK-style projects.
> - Find and replace namespace changes.
> - Configure .NET MAUI.
> - Upgrade or replace incompatible dependencies with .NET 6+ versions.
> - Compile and test your app.

To simplify the upgrade, we recommend creating a new .NET project of the same type and name as your Xamarin.Forms project, and then copying in your code. This is the approach outlined below.

## Create a new project

In Visual Studio, create a new .NET MAUI class library project of the same name as your Xamarin.Forms library project. This project will host the code from your Xamarin.Forms library project. Opening the project file will confirm that you have a .NET SDK-style project:

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

        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">11.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">13.1</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">21.0</SupportedOSPlatformVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</SupportedOSPlatformVersion>
        <TargetPlatformMinVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.17763.0</TargetPlatformMinVersion>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'tizen'">6.5</SupportedOSPlatformVersion>
    </PropertyGroup>

</Project>
```

In your app projects, add a reference to this new library project. Then copy your Xamarin.Forms library project files into the .NET MAUI library project.

## Next step

> [!div class="nextstepaction"]
> [API changes](api-changes.md)
