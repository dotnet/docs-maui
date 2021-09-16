---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 09/14/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview versions of both Visual Studio 2022 and .NET 6. Additionally, you'll need to install the platform SDKs for Android, iOS, macOS, tvOS, and Mac Catalyst.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

To create .NET MAUI apps in Visual Studio, you'll need the latest [Visual Studio 2022 Preview](https://visualstudio.microsoft.com/vs/preview/vs2022/) with the following workloads installed:

- Mobile development with .NET, **including the optional workload .NET MAUI**
- Universal Windows Platform development
- Desktop development with C++
- .NET Desktop Development
- ASP.NET and web development (required for .NET MAUI Blazor apps)

:::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workload.":::

In addition, you must currently install the following Visual Studio extension to create apps that target Windows UI Library (WinUI) 3:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

For more information about the required workloads and components for WinUI 3 development, see [Required workloads and components](/windows/apps/project-reunion/set-up-your-development-environment#required-workloads-and-components).

> [!IMPORTANT]
> You **must** uncomment the Windows Target Framework from your _.csproj_ file in order to enable windows development.

To use the `WebView` or `BlazorWebView` controls on Windows you need to install the WebView2 package:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)

.NET MAUI requires the Android 31 SDK for development. The Android 31 SDK can be installed through [Android Studio](https://developer.android.com/studio)

Currently .NET MAUI has a dependency on OpenJDK. OpenJDK can be installed [from the microsoft openJDK page](https://www.microsoft.com/openjdk).
