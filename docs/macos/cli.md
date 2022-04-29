---
title: "Build a Mac Catalyst app on macOS with .NET CLI"
description: "Learn how to create and run a .NET MAUI app on Mac Catalyst using .NET CLI on macOS."
ms.date: 04/29/2022
---

# Build a Mac Catalyst app with .NET CLI

In this tutorial, you'll learn how to create and run a .NET Multi-platform App UI (.NET MAUI) app on Mac Catalyst using .NET Command Line Interface (CLI):

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

1. To create .NET MAUI apps, you'll need to download and run the [installer](https://github.com/dotnet/installer/blob/main/README.md#installers-and-binaries) for the latest .NET 6 runtime. You'll also need to download and install the latest version of [Xcode 13](https://xcodereleases.com), which is also available from the App Store app on your Mac.

1. On your Mac, open **Terminal** and check that you have the latest .NET 6 runtime installed:

    ```zsh
    dotnet --version
    ```

1. In **Terminal**, install the latest public build of .NET MAUI:

    ```zsh
    sudo dotnet workload install maui --source https://api.nuget.org/v3/index.json
    ```

    This command will install the latest released version of .NET MAUI, including the required platform SDKs.

1. In **Terminal**, create a new .NET MAUI app using .NET CLI:

    ```zsh
    dotnet new maui -n "MyMauiApp"
    ```

1. In **Terminal**, change directory to *MyMauiApp*, and build and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

    The `dotnet build` command will restore the project the dependencies, build the app, and launch it.

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/cli/maccatalyst-running-app.png" alt-text=".NET MAUI app running on Mac.":::
