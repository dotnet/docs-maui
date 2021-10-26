---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 10/12/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview versions of Visual Studio 2022 and .NET 6.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Visual Studio

To create .NET MAUI apps, you'll need [Visual Studio 2022 Preview 5](https://visualstudio.microsoft.com/vs/preview/vs2022/). Either install Visual Studio, or modify your installation, and install the following **workloads**:

- Mobile development with .NET
- Universal Windows Platform development
- ASP.NET and web development (required for .NET MAUI Blazor apps)

  :::image type="content" source="media/installation/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

In the **Installation details** > **Mobile development with .NET** section of the installation window, check the **.NET MAUI (Preview)** optional workload:

:::image type="content" source="media/installation/vs-optional.png" alt-text="Visual Studio installer enabling the .NET MAUI optional workload.":::

## Windows (WinUI 3)

To create apps that target Windows UI Library (WinUI) 3, install the following Visual Studio extension:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

> [!IMPORTANT]
> You **must** uncomment the Windows `TargetFrameworks` from your _.csproj_ file in order to enable windows development.

## Next steps

To learn how to create and run your first .NET MAUI app, continue to build your first app.

> [!div class="nextstepaction"]
> [Build your first app](first-app.md)
