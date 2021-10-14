---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app on Android."
zone_pivot_groups: preview-platforms
ms.date: 10/14/2021
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Visual Studio for Mac support will arrive in a future release.

::: zone pivot="windows"

## Prerequisites

- The latest preview of Visual Studio 2022, with the required workloads. For more information, see [Installation](installation.md).

## Get started with Visual Studio 2022 (Preview)

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022, and run it on an Android emulator:

1. Launch Visual Studio 2022, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="first-app-images/new-solution.png" alt-text="New solution.":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App (Preview)** template, and click the **Next** button:

    :::image type="content" source="first-app-images/new-project.png" alt-text="Choose a template.":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="first-app-images/configure-project.png" alt-text="Configure the project.":::

1. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="first-app-images/restored-dependencies.png" alt-text="Restored dependencies.":::

1. In the Visual Studio toolbar, press the **Android Emulator** button to build the app:

    :::image type="content" source="first-app-images/android-emulator-button.png" alt-text="Android emulator button.":::

    > [!NOTE]
    > If the **Android Emulator** button does not appear, restart Visual Studio and reload the solution.

    > [!NOTE]
    > If Visual Studio builds the app and attempts to deploy it to Windows, restart Visual Studio a second time and reload the solution.

    The **Android SDK - License Acceptance** window will appear.

1. In the **Android SDK - License Acceptance** window, press the **Accept** button:

    :::image type="content" source="first-app-images/android-sdk-license.png" alt-text="Android SDK License Acceptance window.":::

    The **User Account Control** dialog will appear.

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="first-app-images/user-account-control.png" alt-text="User Account Control dialog.":::

    Wait for Visual Studio to download the Android SDK and Android Emulator.

1. In the Visual Studio toolbar, press the **Android Emulator** button to build the app.

    :::image type="content" source="first-app-images/android-emulator-button.png" alt-text="Android emulator button.":::

    The **User Account Control** dialog will appear.

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="first-app-images/user-account-control.png" alt-text="User Account Control dialog.":::

    The **New Device** window will appear.

1. In the **New Device** window, press the **Create** button:

    :::image type="content" source="first-app-images/new-android-device.png" alt-text="New Android Device window.":::

   The **License Acceptance** window will appear.

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

## Build and debug iOS apps

If, while connecting Visual Studio to your Mac through Xamarin Mac Agent (XMA), you are prompted to install a different version of the SDK, you can ignore the prompt since it refers to a legacy version of XMA.

> [!NOTE]
> Visual Studio 2022 can only currently deploy .NET MAUI iOS apps to the iOS simulator, and not to physical devices.

::: zone-end
::: zone pivot="dotnet-cli"

## Prerequisites

- The latest preview of Visual Studio 2022, with the required workloads. For more information, see [Installation](installation.md).
- A configured Android emulator.

## Get started with .NET command-line interface

In this tutorial, you'll create and run your first .NET MAUI app using the .NET command-line interface (CLI):

1. In the .NET CLI, create a new .NET MAUI app:

    ```dotnetcli
    dotnet new maui -n HelloMauiPreview
    ```

1. In the .NET CLI, change directory to the newly created project:

    ```dotnetcli
    cd HelloMauiPreview
    ```

1. In the .NET CLI, change directory to the single project and restore its dependencies:

    ```dotnetcli
    cd HelloMauiPreview
    dotnet restore
    ```

1. In the .NET CLI, build and launch the app on your chosen platform:

    ```dotnetcli
    dotnet build -t:Run -f net6.0-android
    dotnet build -t:Run -f net6.0-ios
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

    > [!NOTE]
    > These commands will launch the app on the default platform device, if one can be found. On Android, it's recommended to start an emulator before building and launching your app.

## Select an iOS simulator

It's possible to specify which simulator is launched and used for net6.0-ios by specifying the `_DeviceName` MSBuild property:

```dotnetcli
dotnet build -t:Run -f net6.0-ios -p:_DeviceName=:v2:udid=<UDID>
```

You can retrieve a list of possible unique device id (UDID) values by executing the `simctl list` command:

```console
/Applications/Xcode.app/Contents/Developer/usr/bin/simctl list
```

The default iOS simulator will be launched if you don't specify a UDID.

::: zone-end
