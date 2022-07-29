---
author: adegeo
ms.author: adegeo
ms.date: 07/29/2022
ms.topic: include
---

This tutorial series is designed to demonstrate how to create a .NET Multi-platform App UI (.NET MAUI) app that doesn't use any platform-specific code. Meaning, the code you write won't be specific to Windows, Android, iOS, or macOS.

In this tutorial, you learn how to:

> [!div class="checklist"]
>
> - Create a .NET MAUI Shell app.
> - Run your app in the Android emulator.
> - Define the user interface with eXtensible Application Markup Language (XAML), and interact with XAML elements through code.
> - Create views and bind them to data.
> - Use navigation to move to and from pages.

You'll use Visual Studio 2022 to create an application with which you can enter a note and save it to device storage. The final application is shown below:

:::image type="content" source="../media/intro/screenshots1-sml.png" alt-text="Final screenshot of the notes app, listing the notes." lightbox="../media/intro/screenshots1.png"::: :::image type="content" source="../media/intro/screenshots2-sml.png" alt-text="Final screenshot of the notes app, adding a note." lightbox="../media/intro/screenshots2.png":::

You'll also use [XAML Hot Reload for .NET MAUI](../../../xaml/hot-reload.md) to see UI changes without rebuilding your application.

## Prerequisites

Before you can begin this tutorial, you'll need to install Visual Studio 2022 (latest release), with the **.NET Multi-platform App UI development** workload installed. To learn how to get the required Visual Studio version and workload, see [Build your first app](../../../get-started/first-app.md).

You can test and debug on:

- **Windows**: [Deploy and debug your .NET MAUI app on Windows](../../../windows/setup.md)
- **Android**: [Managing virtual devices with the Android Device Manager](../../../android/emulator/device-manager.md)
- **iOS**: [Remote iOS Simulator for Windows](../../../ios/remote-simulator.md)

If you're going to test and debug the app on an Android device, you'll need to configure a virtual device. For more information, see [Managing virtual devices with the Android Device Manager](../../../android/emulator/device-manager.md).

## Create a new project

The first part of this tutorial is creating a new project. This project will be named `Notes`.

01. Open Visual Studio, and press **Create a new project**. This button opens up the _new project_ wizard.

01. In the **All project types** box, select `MAUI`. This box filters the available templates to .NET MAUI.

    Next, select **.NET MAUI app** and then press **Next**. The **Configure your project** dialog is displayed.

    :::image type="content" source="../media/intro/vs-new-project.png" alt-text="Filter the new project dialog in Visual Studio to .NET MAUI.":::

01. Set the **Project Name** to `Notes` and select a **Location** where you want to save the code.

    > [!IMPORTANT]
    > You must name the project `Notes`. If you don't, the code you copy and paste from this tutorial may result in build errors.

01. Uncheck **Place solution and project in the same directory**, and then press **Next**. The **Additional information** dialog is displayed.

    :::image type="content" source="../media/intro/vs-configure-project.png" alt-text="Set the name of the .NET MAUI project to Notes in Visual Studio.":::

01. In the **Framework** box, select the latest long-term supported release, which should be .NET 6.0. Press **Create**.

    :::image type="content" source="../media/intro/vs-select-framework.png" alt-text="Create new notes app project in Visual Studio for .NET MAUI.":::

Congratulations! You now have a new project that's ready for the tutorial. In Visual Studio, the **Solution Explorer** pane should look like the following screenshot:

:::image type="content" source="../media/intro/vs-solution-explorer.png" alt-text="The Notes project as presented in the Solution Explorer of Visual Studio for a .NET MAUI app.":::

## Select the target device

You'll need to select which target you want to test and debug your app with. .NET MAUI apps are designed to run on multiple operating systems and devices. At the time this tutorial was published,

Set the **Debug Target** in the Visual Studio toolbar to the device you want to debug and test with.

The following steps demonstrate setting the **Debug Target** to Android:

:::image type="content" source="../media/intro/vs-debugtarget.png" alt-text="Selecting the Android debug target for a .NET MAUI app in Visual Studio.":::

01. Select the **Debug Target** dropdown button.
01. Select the **Android Emulators** item.
01. Select the emulator device.
