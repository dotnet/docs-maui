---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 08/12/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview of .NET 6 with .NET MAUI and the platform SDKs for Android, iOS, macOS, tvOS, and Mac Catalyst.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

To create .NET MAUI apps in Visual Studio, you'll also need the latest [Visual Studio 2022 Preview](https://visualstudio.microsoft.com/vs/preview/vs2022/) with the following workloads installed:

- Mobile development with .NET
- Universal Windows Platform development
- Desktop development with C++
- .NET Desktop Development
- ASP.NET and web development (required for Blazor Desktop and the `BlazorWebView` control)

:::image type="content" source="installation-images/vs-workloads.png" alt-text="Visual Studio workload.":::

In addition, you must currently install the following Visual Studio extension to create apps that target Windows UI Library (WinUI) 3:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

For more information about the required workloads and components for WinUI 3 development, see [Required workloads and components](/windows/apps/project-reunion/set-up-your-development-environment#required-workloads-and-components).

To use the `WebView` or `BlazorWebView` controls on Windows you need to install the WebView2 package:

- [Microsoft Edge WebView2 installer](https://developer.microsoft.com/microsoft-edge/webview2/)

## Install latest .NET 6 preview

<!-- In the .NET CLI, run the following command to install the .NET MAUI workloads:

```dotnetcli
dotnet workload install maui
``` -->

To verify your development environment, and install any missing components, use the [maui-check](https://github.com/Redth/dotnet-maui-check) utility. For acquiring and installing .NET SDKs, `maui-check` uses the same workload commands described in the [release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-maui.md). Install the `maui-check` utility using the following .NET CLI command:

```dotnetcli
dotnet tool install -g redth.net.maui.check
```

If you already have a previous version of `maui-check` installed, update it to the latest version with the following .NET CLI command:

```dotnetcli
dotnet tool update -g redth.net.maui.check
```

Then, run `maui-check`:

```dotnetcli
maui-check
```

If any tools and SDKs required by .NET MAUI are missing, `maui-check` will install them. It may be necessary to run `maui-check` more than once to ensure that your environment has the latest tools and SDKs required by .NET MAUI.
