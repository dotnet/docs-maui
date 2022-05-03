---
title: "Build a Mac Catalyst app on macOS with .NET CLI"
description: "Learn how to create and run a .NET MAUI app on Mac Catalyst using .NET CLI on macOS."
ms.date: 04/29/2022
---

# Build a Mac Catalyst app with .NET CLI

In this tutorial, you'll learn how to create and run a .NET Multi-platform App UI (.NET MAUI) app on Mac Catalyst using .NET Command Line Interface (CLI) on macOS:

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

[!INCLUDE [Install .NET MAUI on macOS](~/includes/install-create-macos.md)]

<!-- markdownlint-disable MD029 -->
5. In **Terminal**, change directory to *MyMauiApp*, and build and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

    The `dotnet build` command will restore the project the dependencies, build the app, and launch it.

    <!-- markdownlint-enable MD029 -->

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/cli/maccatalyst-running-app.png" alt-text=".NET MAUI app running on Mac.":::
