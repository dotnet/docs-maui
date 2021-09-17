---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 09/16/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview versions of both Visual Studio 2022 and .NET 6. Additionally, you'll need to install the platform SDKs for Android, iOS, macOS, tvOS, and Mac Catalyst.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

> [!NOTE]
> If you previously used these instructions to install .NET MAUI, note the absence of the `maui-check` tool. This tool is no longer required to get .NET MAUI working in Visual Studio.

## Visual Studio workloads

To create .NET MAUI apps, you'll need [Visual Studio 2022 Preview 4 or later](https://visualstudio.microsoft.com/vs/preview/vs2022/). Either install Visual Studio, or modify the installation and check the following **workloads**:

- ASP.NET and web development (required for Blazor Desktop and the `BlazorWebView` control)
- Mobile development with .NET
- .NET Desktop Development
- Desktop development with C++
- Universal Windows Platform development

  :::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

Under the **Installation** > **Mobile development with .NET** section, check the **.NET MAUI** optional workload:

:::image type="content" source="installation-images/vs-optional.png" alt-text="Visual Studio installer enabling the .NET MAUI optional component.":::

## More requirements

.NET MAUI requires the Android 31 SDK for development. To develop .NET MAUI apps, you must install the following items:

- Microsoft Build of OpenJDK

  You may notice that Visual Studio installs a version of Microsoft OpenJDK, however, you need to install the Microsoft OpenJDK 11, available from the [OpenJDK](https://www.microsoft.com/openjdk) page. After you install this version of the OpenJDK, Visual Studio will automatically pick it up. if it doesn't set it in the **Java Development Kit Location** field when configuring the Android 31 SDK in the next part.

- Android 31 SDK

  The Android 31 SDK can be installed through Visual Studio. These steps are critical in getting Visual Studio to recognize the Android SDK and to creating a .NET MAUI app.

  01. Open Visual Studio.
  01. If the **Open Recent/Get Started** window opens, choose **Continue without code**.
  01. In the main Visual Studio window, use the menu to choose **Tools** > **Options...**. The options window is displayed.
  01. In the search bar, type `android`. Select the **Xamarin** > **Android Settings** category.
  01. The **Android SDK Location** should point to a default of _C:\Program Files (x86)\Android\android-sdk_. It probably has a red `X` indicating it's not configured correctly. This is normal.
  01. Select the **OK** button to close the window. Visual Studio will now try to load the Android SDKs.

      :::image type="content" source="installation-images/vs-android-settings.png" alt-text="Visual Studio android settings for .NET MAUI.":::
  
  Visual Studio should now be able to load the Android SDK Manager. The next step is to install the Android 31 SDK:

  01. In the main Visual Studio window, use the menu to choose **Tools** > **Android** > **Android SDK Manager...**
  01. In the Android SDK Manager, check the **Android 12.0 API Level 31** item.
  01. Select the **Apply Changes** button.

      :::image type="content" source="installation-images/vs-android-sdk.png" alt-text="Visual Studio android SDK manager window for .NET MAUI.":::

For these changes to take effect, you may need to restart Visual Studio.

## Windows (WinUI 3)

To create apps that target Windows UI Library (WinUI) 3, install the following Visual Studio extension:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

  For more information about the required workloads and components for WinUI 3 development, see [Required workloads and components](/windows/apps/project-reunion/set-up-your-development-environment#required-workloads-and-components).

> [!IMPORTANT]
> You **must** uncomment the Windows `TargetFrameworks` from your _.csproj_ file in order to enable windows development.

## Blazor and WebView

To use the `WebView` or `BlazorWebView` controls on Windows, install the **WebView2 Runtime**:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)
