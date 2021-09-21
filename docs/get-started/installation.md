---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 09/21/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview versions of Visual Studio 2022 and .NET 6. Visual Studio 2022 should install .NET 6 for you, but if it's missing, download it from the [Download .NET 6.0 site](https://dotnet.microsoft.com/download/dotnet/6.0).

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Visual Studio

To create .NET MAUI apps, you'll need [Visual Studio 2022 Preview 4 or later](https://visualstudio.microsoft.com/vs/preview/vs2022/). Either install Visual Studio, or modify your installation, and install the following **workloads**:

- Mobile development with .NET
- ASP.NET and web development (required for .NET MAUI Blazor apps)

  :::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

Then, in the **Installation details** > **Mobile development with .NET** section of the installation window, check the **.NET MAUI (Preview)** optional workload:

:::image type="content" source="installation-images/vs-optional.png" alt-text="Visual Studio installer enabling the .NET MAUI optional workload.":::

## Android

.NET MAUI requires the Android 12 (API 31) SDK for development. Install the following items:

- [Microsoft Build of OpenJDK](https://www.microsoft.com/openjdk)

  While Visual Studio installs a version of Microsoft OpenJDK, you need to install Microsoft OpenJDK 11, available from the [OpenJDK page](https://www.microsoft.com/openjdk). When installing OpenJDK 11, use the default installation configuration settings. After installing OpenJDK 11, Visual Studio should automatically consume it. However, if it doesn't, set the path to the OpenJDK install in the **Tools** > **Options** > **Xamarin** > **Android Settings** > **Java Development Kit Location** field.

- Android 12 (API 31) SDK

  The Android 12 SDK must be installed so that Visual Studio can create a .NET MAUI app. This can be accomplished through Visual Studio:

  01. Open Visual Studio.
  01. If the **Open Recent/Get Started** window opens, choose **Continue without code**.
  01. In the main Visual Studio window, use the menu to choose **Tools** > **Options...**. The options window is displayed.
  01. In the search bar, type `android`. Select the **Xamarin** > **Android Settings** category.
  01. The **Android SDK Location** field should point to a default of _C:\Program Files (x86)\Android\android-sdk_. It probably has a red `X` indicating it's not configured correctly. This is normal.
  01. Select the **OK** button to close the window.

      :::image type="content" source="installation-images/vs-android-settings.png" alt-text="Visual Studio android settings for .NET MAUI.":::

  01. Visual Studio will now attempt to load the Android SDKs. This may involve accepting the Android SDK License:

      :::image type="content" source="installation-images/vs-android-sdk-license.png" alt-text="Visual Studio android SDK license.":::

      If you lack a previous Android install, Visual Studio will download the Android tooling required for you to proceed.

  Visual Studio should now be able to load the Android SDK Manager. The next step is to install the Android 12 SDK:

  01. In the main Visual Studio window, choose the **Tools** > **Android** > **Android SDK Manager...** menu item.
  01. In the Android SDK Manager, check the **Android 12.0 API Level 31** item and click the **Apply Changes** button.

      :::image type="content" source="installation-images/vs-android-sdk.png" alt-text="Visual Studio android SDK manager window for .NET MAUI.":::

  Visual Studio will then install the Android 12 SDK.

## iOS

To build and debug .NET 6 iOS apps from Visual Studio 2022 you must also install the .NET MAUI workload on your Mac build host. On your Mac, in the .NET CLI, run the following command to install the .NET MAUI workload:

```dotnetcli
dotnet workload install maui
```

In addition, you must also install the latest beta of Xcode 13 on your Mac.

## Windows (WinUI 3)

To create apps that target Windows UI Library (WinUI) 3, install the following Visual Studio extension:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

> [!IMPORTANT]
> You **must** uncomment the Windows `TargetFrameworks` from your _.csproj_ file in order to enable windows development.

To use the `WebView` or `BlazorWebView` controls on Windows, install the **WebView2 Runtime**:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)
