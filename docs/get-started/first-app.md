---
title: "Build your first .NET MAUI app in Visual Studio"
description: "How to create and run your first .NET MAUI app."
ms.date: 06/17/2021
---

# Build your first .NET MAUI app

In this tutorial, you'll learn how to create your first .NET Multi-platform App UI (MAUI) app, and run it in an Android emulator.

> [!NOTE]
> Visual Studio for Mac support will arrive in a future release.

## Prerequisites

- An environment that has been configured for .NET MAUI development, using the maui-check tool. For more information, see [Install .NET 6 Preview 5](installation.md#install-net-6-preview-5).
- Visual Studio 2019 (build 16.11 Preview 2 or greater) or Visual Studio 2022 (any edition), with the required workloads. For more information, see [Installation](installation.md).
- A configured Android emulator. For more information about creating an Android emulator, see [Android emulator setup](/xamarin/android/get-started/installation/android-emulator/).

## Get started with Visual Studio 2019

1. Launch VS2019 build 16.11 (Preview 2 or greater), and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="first-app-images/new-solution.png" alt-text="New solution":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App** template, and click the **Next** button:

    :::image type="content" source="first-app-images/new-project.png" alt-text="Choose a template":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="first-app-images/configure-project.png" alt-text="Configure the project":::

    Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="first-app-images/restored-dependencies.png" alt-text="Restored dependencies":::

1. In the Visual Studio toolbar, select the drop-down next to the **Start** button (the triangular button that resembles a Play button), select **Android Emulator**, and then select the emulator you'd like to deploy the app to:

    :::image type="content" source="first-app-images/select-android-emulator.png" alt-text="Select your Android emulator":::

1. In the Visual Studio toolbar, press the **Start** button to launch the app in your chosen Android emulator.

1. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    [![App running in the Android emulator](first-app-images/running-app.png)](first-app-images/running-app-large.png#lightbox)
