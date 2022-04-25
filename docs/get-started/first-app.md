---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app on Android."
ms.date: 04/25/2022
ms.custom: updateeachrelease
zone_pivot_groups: devices-set-one
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

:::zone pivot="devices-android"

Visual Studio 2022 for Mac, with .NET MAUI support, is currently available as a [preview release](https://visualstudio.microsoft.com/vs/mac/preview/).

## Get started with Visual Studio 2022 17.2 (Preview)

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.2 Preview, and run it on an Android emulator:

1. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.2 Preview:

    :::image type="content" source="media/first-app/download-community-preview.png" alt-text="Download Visual Studio 2022 Community Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-professional-preview.png" alt-text="Download Visual Studio 2022 Professional Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-enterprise-preview.png" alt-text="Download Visual Studio 2022 Enterprise Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    Either install Visual Studio, or modify your installation, and install the Mobile development with .NET workload:

    :::image type="content" source="media/first-app/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

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

    > [!IMPORTANT]
    > Hardware acceleration must be enabled to maximize Android emulator performance. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/emulator/hardware-acceleration.md).

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
    > Hardware acceleration must be enabled to maximize Android emulator performance. Failure to do this will result in the emulator running very slowly. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/emulator/hardware-acceleration.md).

1. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/running-app.png" alt-text="App running in the Android emulator." lightbox="media/first-app/running-app-large.png":::

:::zone-end

:::zone pivot="devices-windows"

## Get started with Visual Studio 2022 17.2 (Preview)

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.2 Preview, and run it on Windows:

1. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.2 Preview:

    :::image type="content" source="media/first-app/download-community-preview.png" alt-text="Download Visual Studio 2022 Community Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    :::image type="content" source="media/first-app/download-professional-preview.png" alt-text="Download Visual Studio 2022 Professional Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    :::image type="content" source="media/first-app/download-enterprise-preview.png" alt-text="Download Visual Studio 2022 Enterprise Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    Either install Visual Studio, or modify your installation, and install the Mobile development with .NET workload:

    :::image type="content" source="media/first-app/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

1. Launch Visual Studio 2022 17.2 Preview, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="media/first-app/new-solution.png" alt-text="New solution.":::

1. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App (Preview)** template, and click the **Next** button:

    :::image type="content" source="media/first-app/new-project.png" alt-text="Choose a template.":::

1. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="media/first-app/configure-project.png" alt-text="Configure the project.":::

1. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="media/first-app/restored-dependencies.png" alt-text="Restored dependencies.":::

    > [!NOTE]
    > Even though you're going to run your app directly on Windows, you may be prompted to install the Android SDK, if this is the first time you've created a .NET MAUI app.
    >
    > The default build target for a new .NET MAUI project is Android. If you want to also target and run on Android in addition to Windows, navigate to the start of this article and select Android. Then, complete this tutorial which will teach you how to install the Android SDK and create a virtual Android device. Afterwards, switch this article back to Windows and continue where you left off.

1. In the Visual Studio toolbar, use the **Debug Target** drop down to select **Framework** and then the **net6.0-windows** entry.

    :::image type="content" source="media/first-app/windows-debug-target.png" alt-text="Select the Windows Machine debugging target for .NET MAUI.":::

1. Press the **Windows Machine** button to deploy the app.

    :::image type="content" source="media/first-app/windows-run-button.png" alt-text="Run .NET MAUI app in Visual Studio button.":::

    If you've not enabled Developer Mode, the Settings app should open to the appropriate page. Turn on **Developer Mode** and accept the disclaimer.

    :::image type="content" source="media/first-app/windows-developer-mode-win11.png" alt-text="Developer Mode toggle on the Windows 11 settings app.":::

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/windows-running-app.png" alt-text=".NET MAUI app running on Windows.":::

:::zone-end

:::zone pivot="devices-maccatalyst"

Visual Studio 2022 for Mac, with .NET MAUI support, is currently available as a [preview release](https://visualstudio.microsoft.com/vs/mac/preview/).

## Get started with .NET CLI

In this tutorial, you'll create your first .NET MAUI app with .NET Command Line Interface (CLI), and run it on a Mac:

1. To create .NET MAUI apps, you'll need to download and run the [installer](https://github.com/dotnet/installer/blob/main/README.md#installers-and-binaries) for the latest .NET 6 runtime.

    You'll also need to download and install the latest version of [Xcode 13](https://xcodereleases.com), which is also available from the App Store app on your Mac.

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

1. In **Terminal**, change directory to the *MyMauiApp* directory, and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

    The `dotnet build` command will restore the project the dependencies, build the app, and launch it.

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.    

    :::image type="content" source="media/first-app/maccatalyst-running-app.png" alt-text=".NET MAUI app running on Mac.":::

:::zone-end

<!-- ## Build and debug iOS apps

If, while connecting Visual Studio to your Mac through Xamarin Mac Agent (XMA), you are prompted to install a different version of the SDK, you can ignore the prompt since it refers to a legacy version of XMA.

> [!NOTE]
> Visual Studio 2022 can only currently deploy .NET MAUI iOS apps to the iOS simulator, and not to physical devices.

::: zone-end
::: zone pivot="dotnet-cli"

Visual Studio 2022 for Mac, with .NET MAUI support, is currently available as a [preview release](https://visualstudio.microsoft.com/vs/mac/preview/).

## Get started with .NET command-line interface

In this tutorial, you'll create and run your first .NET MAUI app using the .NET command-line interface (CLI):

01. In the .NET CLI, create a new .NET MAUI app:

    ```dotnetcli
    dotnet new maui -n HelloMauiPreview
    ```

01. In the .NET CLI, change directory to the newly created project:

    ```dotnetcli
    cd HelloMauiPreview
    ```

01. In the .NET CLI, change directory to the single project and restore its dependencies:

    ```dotnetcli
    cd HelloMauiPreview
    dotnet restore
    ```

01. In the .NET CLI, build and launch the app on your chosen platform:

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
