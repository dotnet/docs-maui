---
author: adegeo
ms.author: adegeo
ms.date: 12/15/2022
ms.topic: include
---

If you followed the [previous tutorial series](../../notes-app/index.yml) before December 14, 2022, you may need to upgrade your app to .NET 7. If your notes app is already .NET 7, move on to the next step.

To upgrade your app to .NET 7:

01. Open the _Notes.csproj_ file in Visual Studio, Visual Studio Code, or any other text editor.
01. Find the `Project/PropertyGroup/TargetFrameworks` element and change it to the following:

    ```xml
    <TargetFrameworks>net7.0-android;net7.0-ios;net7.0-maccatalyst</TargetFrameworks>
    ```

01. Find the `Project/PropertyGroup/TargetFrameworks` element with a `condition` specifying Windows, and change it to the following:

    ```xml
    <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net7.0-windows10.0.19041.0</TargetFrameworks>
    ```

01. Change the minimum operating system versions required for iOS and Mac. Replace the two `SupportedOSPlatformVersion` elements with the following:

    ```xml
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">11.0</SupportedOSPlatformVersion>
    <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">13.1</SupportedOSPlatformVersion>
    ```

01. Save the project file.
01. Close Visual Studio if you opened the project with Visual Studio.
01. Delete the _./bin_ and _./obj_ folders in the same folder as the _Notes.csproj_ file.
