---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app on Android."
ms.date: 02/24/2021
---

<!-- zone_pivot_groups: preview-platforms -->

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Visual Studio for Mac support will arrive in a future release.

<!-- ::: zone pivot="windows" -->

## Prerequisites

- The latest preview of Visual Studio 2022 17.2 with the required workload. For more information, see [Installation](installation.md).
- Hardware acceleration must be enabled to maximize Android emulator performance. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/hardware-acceleration.md).

## Get started with Visual Studio 2022 17.2 (Preview)

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.2 Preview, and run it on an Android emulator:

1. Launch Visual Studio 2022 17.2 Preview, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="media/first-app/new-solution.png" alt-text="New solution.":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App (Preview)** template, and click the **Next** button:

    :::image type="content" source="media/first-app/new-project.png" alt-text="Choose a template.":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="media/first-app/configure-project.png" alt-text="Configure the project.":::

1. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="media/first-app/restored-dependencies.png" alt-text="Restored dependencies.":::

1. In the **Android SDK License Acceptance** window, press the **Accept** button:

    :::image type="content" source="media/first-app/android-sdk-license.png" alt-text="Android SDK License Acceptance window.":::

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/user-account-control.png" alt-text="User Account Control dialog.":::

    Wait for Visual Studio to download the Android SDK and Android Emulator.

1. In the Visual Studio toolbar, press the **Android Emulator** button to build the app:

    :::image type="content" source="media/first-app/android-emulator-button.png" alt-text="Android emulator button.":::

1. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/user-account-control.png" alt-text="User Account Control dialog.":::

1. In the **New Device** window, press the **Create** button:

    :::image type="content" source="media/first-app/new-android-device.png" alt-text="New Android Device window.":::

1. In the **License Acceptance** window, press the **Accept** button:

    :::image type="content" source="media/first-app/license-acceptance.png" alt-text="License Acceptance window.":::

   Wait for Visual Studio to download, unzip, and create an Android emulator.

1. Close the **Android Device Manager** window:

    :::image type="content" source="media/first-app/android-device-manager.png" alt-text="Android Device Manager window.":::

1. In the Visual Studio toolbar, press the **Pixel 5 - API 30 (Android 11.0 - API 30)** button to build and run the app:

    :::image type="content" source="media/first-app/pixel5-api30.png" alt-text="Pixel 5 API 30 emulator button.":::

   Visual Studio will start the Android emulator, build the app, and deploy the app to the emulator.

   > [!WARNING]
   > Hardware acceleration must be enabled to maximize Android emulator performance. Failure to do this will result in the emulator running very slowly. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/hardware-acceleration.md).

1. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/running-app.png" alt-text="App running in the Android emulator." lightbox="media/first-app/running-app-large.png":::

<!-- ## Build and debug iOS apps

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

-->
