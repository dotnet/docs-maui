---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app in Visual Studio 2022 on Windows."
ms.date: 05/04/2022
ms.custom: updateeachrelease
zone_pivot_groups: devices-deployment
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app in Visual Studio 2022 on Windows.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

:::zone pivot="devices-android,devices-ios"

Visual Studio for Mac support will arrive in a future release.

:::zone-end

## Get started with Visual Studio 2022 17.2 (Preview)

:::zone pivot="devices-android"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.2 Preview, and run it on an Android emulator:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.2 Preview:

    :::image type="content" source="media/first-app/download-community-preview.png" alt-text="Download Visual Studio 2022 Community Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-professional-preview.png" alt-text="Download Visual Studio 2022 Professional Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-enterprise-preview.png" alt-text="Download Visual Studio 2022 Enterprise Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    Either install Visual Studio, or modify your installation, and install the Mobile development with .NET workload:

    :::image type="content" source="media/first-app/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone-end

:::zone pivot="devices-ios"

01. In this tutorial, you'll create your first .NET MAUI app in Visual Studio 17.2 Preview, and run it on an iOS simulator:

    :::image type="content" source="media/first-app/download-community-preview.png" alt-text="Download Visual Studio 2022 Community Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-professional-preview.png" alt-text="Download Visual Studio 2022 Professional Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    :::image type="content" source="media/first-app/download-enterprise-preview.png" alt-text="Download Visual Studio 2022 Enterprise Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303":::

    Either install Visual Studio, or modify your installation, and install the Mobile development with .NET workload:

    :::image type="content" source="media/first-app/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone end

:::zone pivot="devices-windows"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.2 Preview, and run it on Windows:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.2 Preview:

    :::image type="content" source="media/first-app/download-community-preview.png" alt-text="Download Visual Studio 2022 Community Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    :::image type="content" source="media/first-app/download-professional-preview.png" alt-text="Download Visual Studio 2022 Professional Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    :::image type="content" source="media/first-app/download-enterprise-preview.png" alt-text="Download Visual Studio 2022 Enterprise Preview" link="https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Preview&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302":::

    Either install Visual Studio, or modify your installation, and install the Mobile development with .NET workload:

    :::image type="content" source="media/first-app/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone-end

<!-- markdownlint-disable MD029 -->
02. Launch Visual Studio 2022 17.2 Preview, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="media/first-app/new-solution.png" alt-text="New solution.":::

    <!-- markdownlint-enable MD029 -->

01. In the **Create a new project** window, select **MAUI** in the **Project type** drop-down, select the **.NET MAUI App (Preview)** template, and click the **Next** button:

    :::image type="content" source="media/first-app/new-project.png" alt-text="Choose a template.":::

01. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Create** button:

    :::image type="content" source="media/first-app/configure-project.png" alt-text="Configure the project.":::

01. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="media/first-app/restored-dependencies.png" alt-text="Restored dependencies.":::

    :::zone pivot="devices-windows,devices-ios"

    > [!NOTE]
    > If this is the first time you've created a .NET MAUI app, you may be prompted to install the Android SDK. This is because the default build target a new .NET MAUI app is Android.
    >
    > If you want to also target and run on Android, navigate to the start of this article and select Android. Then, complete this tutorial which will teach you how to install the Android SDK and create a virtual Android device..

    :::zone-end

01. In the **Android SDK License Acceptance** window, press the **Accept** button:

    :::image type="content" source="media/first-app/android-sdk-license.png" alt-text="Android SDK License Acceptance window.":::

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/user-account-control.png" alt-text="User Account Control dialog.":::

    Wait for Visual Studio to download the Android SDK and Android Emulator.

:::zone pivot="devices-android"

<!-- markdownlint-disable MD029 -->
08. In the Visual Studio toolbar, press the **Android Emulator** button to build the app:

    :::image type="content" source="media/first-app/android-emulator-button.png" alt-text="Android emulator button.":::

    > [!IMPORTANT]
    > Hardware acceleration must be enabled to maximize Android emulator performance. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/emulator/hardware-acceleration.md).

    <!-- markdownlint-enable MD029 -->

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/user-account-control.png" alt-text="User Account Control dialog.":::

01. In the **New Device** window, press the **Create** button:

    :::image type="content" source="media/first-app/new-android-device.png" alt-text="New Android Device window.":::

01. In the **License Acceptance** window, press the **Accept** button:

    :::image type="content" source="media/first-app/license-acceptance.png" alt-text="License Acceptance window.":::

    Wait for Visual Studio to download, unzip, and create an Android emulator.

01. Close the **Android Device Manager** window:

    :::image type="content" source="media/first-app/android-device-manager.png" alt-text="Android Device Manager window.":::

01. In the Visual Studio toolbar, press the **Pixel 5 - API 30 (Android 11.0 - API 30)** button to build and run the app:

    :::image type="content" source="media/first-app/pixel5-api30.png" alt-text="Pixel 5 API 30 emulator button.":::

    Visual Studio will start the Android emulator, build the app, and deploy the app to the emulator.

    > [!WARNING]
    > Hardware acceleration must be enabled to maximize Android emulator performance. Failure to do this will result in the emulator running very slowly. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/emulator/hardware-acceleration.md).

01. In the running app in the Android emulator, press the **CLICK ME** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/running-app.png" alt-text="App running in the Android emulator." lightbox="media/first-app/running-app-large.png":::

:::zone-end

:::zone pivot="devices-windows"

<!-- markdownlint-disable MD029 -->
08. In the Visual Studio toolbar, use the **Debug Target** drop down to select **Framework** and then the **net6.0-windows** entry.

    :::image type="content" source="media/first-app/windows-debug-target.png" alt-text="Select the Windows Machine debugging target for .NET MAUI.":::

    <!-- markdownlint-enable MD029 -->

01. Press the **Windows Machine** button to deploy the app.

    :::image type="content" source="media/first-app/windows-run-button.png" alt-text="Run .NET MAUI app in Visual Studio button.":::

    If you've not enabled Developer Mode, the Settings app should open to the appropriate page. Turn on **Developer Mode** and accept the disclaimer.

    :::image type="content" source="media/first-app/windows-developer-mode-win11.png" alt-text="Developer Mode toggle on the Windows 11 settings app.":::

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/windows-running-app.png" alt-text=".NET MAUI app running on Windows.":::

:::zone-end

:::zone pivot="devices-ios"

<!-- markdownlint-disable MD029 -->
08. In Visual Studio, pair the IDE to a Mac Build host. For more information, see [Pair to Mac for iOS development](pair-to-mac.md).
    <!-- markdownlint-enable MD029 -->

01. In Visual Studio, navigate to **Tools > Options > Xamarin > iOS Settings** and disable the **Remote Simulator to Windows** checkbox:

     :::image type="content" source="media/first-app/ios-disable-remote-simulator.png" alt-text="Disable the remote simulator in the iOS settings.":::

    For more about the remote iOS Simulator for Windows, see [Remote iOS Simulator for Windows](~/ios/remote-simulator.md).

01. In the Visual Studio toolbar, use the **Debug Target** drop down to select **iOS Simulators** and then a specific iOS simulator:

     :::image type="content" source="media/first-app/ios-debug-target.png" alt-text="Visual Studio iOS simulators debug targets.":::

01. In the Visual Studio toolbar, press the green Start button for your chosen iOS simulator:

   :::image type="content" source="media/remote-simulator/ios-chosen-debug-target.png" alt-text="Visual Studio iOS simulator debug target choice.":::

   Visual Studio will build the app, start the iOS simulator on the paired Mac, and deploy the app to the simulator.

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

   :::image type="content" source="media/first-app/ios-running-app.png" alt-text=".NET MAUI app running in iOS Simulator on a Mac.":::

:::zone end
