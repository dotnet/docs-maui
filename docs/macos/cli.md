---
title: "Build a Mac Catalyst app on macOS with .NET CLI"
description: "Learn how to create and run a .NET MAUI app on Mac Catalyst using .NET CLI on macOS."
ms.date: 04/29/2022
---

# Build a Mac Catalyst app with .NET CLI

In this tutorial, you'll learn how to create and run a .NET Multi-platform App UI (.NET MAUI) app on Mac Catalyst using .NET Command Line Interface (CLI) on macOS:

[!INCLUDE [Install .NET MAUI on macOS](~/includes/install-create-macos.md)]

<!-- markdownlint-disable MD029 -->

::: moniker range="=net-maui-6.0"

5. In **Terminal**, change directory to *MyMauiApp*, and build and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

    The `dotnet build` command will restore the project dependencies, build the app, and launch it.

::: moniker-end

::: moniker range="=net-maui-7.0"

5. In **Terminal**, change directory to *MyMauiApp*, and build and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net7.0-maccatalyst
    ```

    The `dotnet build` command will restore the project dependencies, build the app, and launch it.

::: moniker-end

    <!-- markdownlint-enable MD029 -->
    If you see a build error and a warning that the Xcode app bundle could not be found, you may need to run the following command:

    ```zsh
    xcode-select --reset
    ```

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/cli/maccatalyst-running-app.png" alt-text=".NET MAUI app running on Mac.":::
