---
title: "Supported platforms for .NET MAUI apps"
description: ".NET MAUI supports developing apps for Android, iOS, Mac Catalyst, and Windows."
ms.date: 10/04/2024
---

# Supported platforms for .NET MAUI apps

.NET Multi-platform App UI (.NET MAUI) apps can be written for the following platforms:

::: moniker range="=net-maui-8.0"

- Android 5.0 (API 21) or higher is required.
- iOS 11 or higher is required.
- macOS 11 or higher, using Mac Catalyst.
- Windows 11 and Windows 10 version 1809 or higher, using [Windows UI Library (WinUI) 3](/windows/apps/winui/winui3/).

.NET MAUI Blazor apps have the following additional platform requirements:

- Android 7.0 (API 24) or higher is required.
- iOS 14 or higher is required.
- macOS 11 or higher, using Mac Catalyst.

::: moniker-end

::: moniker range=">=net-maui-9.0"

- Android 5.0 (API 21) or higher is required.
- iOS 12.2 or higher is required.
- macOS 12 or higher, using Mac Catalyst.
- Windows 11 and Windows 10 version 1809 or higher, using [Windows UI Library (WinUI) 3](/windows/apps/winui/winui3/).

.NET MAUI Blazor apps have the following additional platform requirements:

- Android 7.0 (API 24) or higher is required.
- iOS 14 or higher is required.
- macOS 12 or higher, using Mac Catalyst.

::: moniker-end

.NET MAUI Blazor apps also require an updated platform specific WebView control. For more information, see [Blazor supported platforms](/aspnet/core/blazor/supported-platforms).

> [!IMPORTANT]
> For information about the version of Xcode, the Android SDK and JDK, and the Windows App SDK, that's required for a specific version of .NET MAUI, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions).

.NET MAUI apps for Android, iOS, and Windows can be built in Visual Studio. However, a networked Mac is required for iOS development.

On Windows, .NET MAUI apps for Android, iOS, and Windows can also be built with Visual Studio Code when using the .NET MAUI extension. On macOS, .NET MAUI apps for Android, iOS, and macOS can be built in Visual Studio Code when using the .NET MAUI extension.

## Additional platform support

.NET MAUI also includes Tizen support, which is provided by Samsung.

<!-- ## Android platform support

You should have the latest Android SDK Tools and Android API platform installed. You can update to the latest versions using the Android SDK Manager.

Additionally, the target/compile version for Android projects **must** be set to *Use latest installed platform*. However the minimum version can be set to API 21 so you can continue to support devices that use Android 5.0 and newer. -->
