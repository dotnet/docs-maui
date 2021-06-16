---
title: "Build your first .NET MAUI app in Visual Studio"
description: "How to create and run your first .NET MAUI app."
ms.date: 06/09/2021
---

# Build your first .NET MAUI app

In this tutorial, you'll learn how to create your first .NET Multi-platform App UI (MAUI) app, and run it in an Android emulator.

## Prerequisites

- Visual Studio 2019 (build 16.11 Preview 1 or greater) or Visual Studio 2022 (any edition).
- An environment that has been configured for .NET MAUI development, using the maui-check tool. For more information, see [Installation](installation.md).
- A configured Android emulator. For more information about creating an Android emulator, see [Android emulator setup](/xamarin/android/get-started/installation/android-emulator/).

## Get started with Visual Studio 2019

1. Launch VS2019 build 16.11 (Preview 1 or greater), and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="first-app-images/new-solution.png" alt-text="New solution":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App** template, and click the **Next** button:

    :::image type="content" source="first-app-images/new-project.png" alt-text="Choose a template":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="first-app-images/configure-project.png" alt-text="Configure the project":::

    Wait for the project to be created. Note that Visual Studio hasn't restored the required .NET 6 dependencies. You'll restore them in a later step.

    :::image type="content" source="first-app-images/broken-dependencies.png" alt-text="Broken dependencies":::

1. In the .NET CLI, create a *nuget.config* file in the root folder of the project and add the `maui-preview` feed:

    ```dotnetcli
    dotnet new nugetconfig
    dotnet nuget add source -n maui-preview https://aka.ms/maui-preview/index.json
    ```

1. In the .NET CLI, restore the dependencies for the newly created project:

    ```dotnetcli
    dotnet restore
    ```

    In Visual Studio, note that the dependencies have now been restored:

    :::image type="content" source="first-app-images/restored-dependencies.png" alt-text="Restored dependencies":::

1. In the Visual Studio toolbar, select the drop-down next to the **Start** button (the triangular button that resembles a Play button), select **Android Emulator**, and then select the emulator you'd like to deploy the app to:

    :::image type="content" source="first-app-images/select-android-emulator.png" alt-text="Select your Android emulator":::

1. In the Visual Studio toolbar, press the **Start** button to launch the app in your chosen Android emulator.

1. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    [![App running in the Android emulator](first-app-images/running-app.png)](first-app-images/running-app-large.png#lightbox)
