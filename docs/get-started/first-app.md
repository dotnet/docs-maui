---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app on Android."
ms.date: 10/20/2021
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Visual Studio for Mac support will arrive in a future release.

## Prerequisites

- The latest preview of Visual Studio 2022 17.1, with the required workloads. For more information, see [Installation](installation.md).

## Get started with Visual Studio 2022 17.1

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022, and run it on an Android emulator:

1. Launch Visual Studio 2022 17.1, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="first-app-images/new-solution.png" alt-text="New solution.":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App (Preview)** template, and click the **Next** button:

    :::image type="content" source="first-app-images/new-project.png" alt-text="Choose a template.":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="first-app-images/configure-project.png" alt-text="Configure the project.":::

1. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="first-app-images/restored-dependencies.png" alt-text="Restored dependencies.":::

1. In the Visual Studio toolbar, press the **Android Emulator** button to start the download of the Android SDK and emulator:

    :::image type="content" source="first-app-images/android-emulator-button.png" alt-text="Android emulator button.":::

1. In the **Android SDK License Acceptance** window, press the **Accept** button:

    :::image type="content" source="first-app-images/android-sdk-license.png" alt-text="Android SDK License Acceptance window.":::

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="first-app-images/user-account-control.png" alt-text="User Account Control dialog.":::

    Wait for Visual Studio to download the Android SDK and Android Emulator.

1. In the Visual Studio toolbar, press the **Android Emulator** button to build the app:

    :::image type="content" source="first-app-images/android-emulator-button.png" alt-text="Android emulator button.":::

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="first-app-images/user-account-control.png" alt-text="User Account Control dialog.":::

1. In the **New Device** window, press the **Create** button:

    :::image type="content" source="first-app-images/new-android-device.png" alt-text="New Android Device window.":::

1. In the **License Acceptance** window, press the **Accept** button:

    :::image type="content" source="first-app-images/license-acceptance.png" alt-text="License Acceptance window.":::

   Wait for Visual Studio to download, unzip, and create an Android emulator.

1. Close the **Android Device Manager** window:

    :::image type="content" source="first-app-images/android-device-manager.png" alt-text="Android Device Manager window.":::

1. In the Visual Studio toolbar, press the **Pixel 2 - API 28 (Android 9.0 - API 28)** button to build and run the app:

    :::image type="content" source="first-app-images/pixel2-api28.png" alt-text="Pixel 2 API 28 emulator button.":::

   Visual Studio will start the Android emulator, build the app, and deploy the app to the emulator.

1. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="first-app-images/running-app.png" alt-text="App running in the Android emulator." lightbox="first-app-images/running-app-large.png":::
