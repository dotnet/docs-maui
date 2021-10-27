---
title: ".NET MAUI installation"
description: "Installation instructions for .NET MAUI."
ms.date: 10/20/2021
---

# Installation

To create .NET Multi-platform App UI (.NET MAUI) apps, you need the latest preview versions of Visual Studio 2022 and .NET 6.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Visual Studio

To create .NET MAUI apps, you'll need Visual Studio 17.1 Preview 1. Either install Visual Studio, or modify your installation, and install the following **workloads**:

- Mobile development with .NET

  :::image type="content" source="media/installation/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

## Windows (WinUI 3)

To create apps that target Windows UI Library (WinUI) 3, install the following Visual Studio extension:

- [Single-project MSIX Packaging Tools](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

> [!IMPORTANT]
> You **must** uncomment the Windows `TargetFrameworks` from your _.csproj_ file in order to enable Windows development.

## Next steps

To learn how to create and run your first .NET MAUI app, continue to build your first app.

> [!div class="nextstepaction"]
> [Build your first app](first-app.md)
