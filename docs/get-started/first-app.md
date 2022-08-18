---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app in Visual Studio 2022 on Windows, or Visual Studio 2022 for Mac."
ms.date: 08/16/2022
zone_pivot_groups: devices-deployment
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app in Visual Studio 2022 17.3 on Windows, or Visual Studio 2022 for Mac 17.4 Preview.

# [Visual Studio](#tab/vswin)

:::zone pivot="devices-android"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.3, and run it on an Android emulator:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.3:

    - [Download 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    - [Download 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    - [Download 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    Either install Visual Studio, or modify your installation, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/first-app/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone-end

:::zone pivot="devices-ios"

Developing .NET MAUI apps for iOS on Windows requires a Mac build host. If you don't specifically need to target iOS and don't have a Mac, consider getting started with Android or Windows instead.

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 17.3, and run it on an iOS simulator:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.3:

    - [Download 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2304)

    - [Download 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2304)

    - [Download 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2304)

    Either install Visual Studio, or modify your installation, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/first-app/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone-end

:::zone pivot="devices-windows"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 17.3, and run it on Windows:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 17.3:

    - [Download 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302)

    - [Download 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302)

    - [Download 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2302)

    Either install Visual Studio, or modify your installation, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/first-app/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

:::zone-end

:::zone pivot="devices-android, devices-ios, devices-windows"

<!-- markdownlint-disable MD029 -->
02. Launch Visual Studio 2022 17.3, and in the start window click **Create a new project** to create a new project:

    :::image type="content" source="media/first-app/vs/new-solution.png" alt-text="New solution.":::

    <!-- markdownlint-enable MD029 -->

01. In the **Create a new project** window, select **MAUI** in the **All project types** drop-down, select the **.NET MAUI App** template, and click the **Next** button:

    :::image type="content" source="media/first-app/vs/new-project.png" alt-text="Choose a template.":::

    > [!NOTE]
    > The .NET MAUI templates might not appear in Visual Studio if you also have .NET 7 Preview installed. For more information, see [.NET MAUI templates do not appear in Visual Studio](https://github.com/dotnet/maui/wiki/Known-Issues#net-maui-templates-do-not-appear-in-visual-studio).

01. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Next** button:

    :::image type="content" source="media/first-app/vs/configure-project.png" alt-text="Configure the project.":::

01. In the **Additional information** window, click the **Create** button:

    :::image type="content" source="media/first-app/vs/additional-information.png" alt-text="Additional information.":::

01. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="media/first-app/vs/restored-dependencies.png" alt-text="Restored dependencies.":::

:::zone-end

:::zone pivot="devices-android"

<!-- markdownlint-disable MD029 -->
07. In the Visual Studio toolbar, use the **Debug Target** drop down to select **Android Emulators** and then the **Android Emulator** entry:

    :::image type="content" source="media/first-app/vs/android-debug-target.png" alt-text="Select the Android Emulator debugging target for .NET MAUI.":::

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio toolbar, press the **Android Emulator** button:

    :::image type="content" source="media/first-app/vs/android-emulator-button.png" alt-text="Android emulator button.":::

    Visual Studio will start to install the default Android SDK and Android Emulator.

01. In the **Android SDK - License Agreement** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license1.png" alt-text="First Android SDK License Agreement window.":::

01. In the **Android SDK - License Agreement** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license2.png" alt-text="Second Android SDK License Agreement window.":::

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license-uac.png" alt-text="Android SDK license user account control dialog.":::

01. In the **License Acceptance** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-device-license.png" alt-text="Android device license window.":::

    Wait for Visual Studio to download the Android SDK and Android Emulator.

01. In the Visual Studio toolbar, press the **Android Emulator** button:

    :::image type="content" source="media/first-app/vs/android-emulator-button.png" alt-text="Android emulator button.":::

    Visual Studio will start to create a default Android emulator.

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/vs/android-device-manager-uac.png" alt-text="Android Device Manager user account control dialog.":::

01. In the **New Device** window, press the **Create** button:

    :::image type="content" source="media/first-app/vs/new-android-device.png" alt-text="New Android Device window.":::

    Wait for Visual Studio to download, unzip, and create an Android emulator.

01. Close the **Android Device Manager** window:

    :::image type="content" source="media/first-app/vs/android-device-manager.png" alt-text="Android Device Manager window.":::

01. In the Visual Studio toolbar, press the **Pixel 5 - API 30 (Android 11.0 - API 30)** button to build and run the app:

    :::image type="content" source="media/first-app/vs/pixel5-api30.png" alt-text="Pixel 5 API 30 emulator button.":::

    Visual Studio will start the Android emulator, build the app, and deploy the app to the emulator.

    > [!WARNING]
    > Hardware acceleration must be enabled to maximize Android emulator performance. Failure to do this will result in the emulator running very slowly. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & HAXM)](~/android/emulator/hardware-acceleration.md).

01. In the running app in the Android emulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/vs/running-app.png" alt-text="App running in the Android emulator." lightbox="media/first-app/vs/running-app-large.png":::

:::zone-end

:::zone pivot="devices-windows"

<!-- markdownlint-disable MD029 -->
07. In the Visual Studio toolbar, use the **Debug Target** drop down to select **Framework** and then the **net6.0-windows** entry:

    :::image type="content" source="media/first-app/vs/windows-debug-target.png" alt-text="Select the Windows Machine debugging target for .NET MAUI.":::

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio toolbar, press the **Windows Machine** button to build and run the app:

    :::image type="content" source="media/first-app/vs/windows-run-button.png" alt-text="Run .NET MAUI app in Visual Studio button.":::

    If you've not enabled Developer Mode, Visual Studio will prompt you to enable it. In the **Enable Developer Mode for Windows** dialog, click **settings for developers** to open the Settings app:

    :::image type="content" source="media/first-app/vs/windows-enable-developer-mode.png" alt-text="Enable Windows developer mode dialog.":::

    In the Settings app, turn on **Developer Mode** and accept the disclaimer:

    :::image type="content" source="media/first-app/vs/windows-developer-mode-win11.png" alt-text="Developer Mode toggle on the Windows 11 settings app.":::

    Close the Settings app, and then close the **Enable Developer Mode for Windows** dialog.

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vs/windows-running-app.png" alt-text=".NET MAUI app running on Windows.":::

:::zone-end

:::zone pivot="devices-ios"

<!-- markdownlint-disable MD029 -->
07. In Visual Studio, pair the IDE to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio toolbar, use the **Debug Target** drop down to select **iOS Simulators** and then a specific iOS simulator:

    :::image type="content" source="media/first-app/vs/ios-debug-target.png" alt-text="Visual Studio iOS simulators debug targets.":::

01. In the Visual Studio toolbar, press the Start button for your chosen iOS simulator to build and run your app:

    :::image type="content" source="media/first-app/vs/ios-chosen-debug-target.png" alt-text="Visual Studio iOS simulator debug target choice.":::

    Visual Studio will build the app, start the remote iOS Simulator for Windows, and deploy the app to the remote simulator. For more information about the remote iOS Simulator for Windows, see [Remote iOS Simulator for Windows](~/ios/remote-simulator.md).

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/vs/ios-running-app.png" alt-text=".NET MAUI app running in iOS Simulator on a Mac.":::

:::zone-end

:::zone pivot="devices-maccatalyst"

Mac Catalyst apps can only be launched and debugged using Visual Studio 2022 for Mac.

:::zone-end

# [Visual Studio for Mac](#tab/vsmac)

:::zone pivot="devices-android, devices-ios, devices-maccatalyst"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 for Mac 17.4 Preview, and run it on your chosen platform:

01. To create .NET MAUI apps, you'll need the latest Visual Studio 2022 for Mac 17.4 Preview. Either install [Visual Studio 2022 for Mac 17.4 Preview](https://aka.ms/installer/preview), or modify your existing installation, and ensure that the following workloads are installed:

    - .NET
    - .NET MAUI
    - Android
    - iOS

    :::image type="content" source="media/first-app/vsmac/maui-workloads.png" alt-text="Visual Studio for Mac .NET MAUI workloads.":::

    :::image type="content" source="media/first-app/vsmac/legacy-workloads.png" alt-text="Visual Studio for Mac legacy workloads.":::

    <!-- The legacy workloads shouldn't be required in 17.4 P2 -->

01. Launch Visual Studio 2022 for Mac 17.4 Preview, and in the start window click **New** to create a new project:

    :::image type="content" source="media/first-app/vsmac/new-project.png" alt-text="Create a new project in Visual Studio for Mac.":::

01. In the **Choose a template for your new project** dialog, select **Multiplatform > App**, select the **.NET MAUI App** template, and click the **Continue** button:

    :::image type="content" source="media/first-app/vsmac/choose-template.png" alt-text="Choose the .NET MAUI App project template.":::

01. In the **Configure your new .NET MAUI App** dialog, ensure that **.NET 6.0** is selected in the **Target framework** drop-down and click the **Continue** button:

    :::image type="content" source="media/first-app/vsmac/select-target-framework.png" alt-text="Select the target framework for your .NET MAUI app.":::

01. In the **Configure your new .NET MAUI App** dialog, enter **MyMauiApp** as the project name, select a suitable location for the project, and click the **Create** button:

    :::image type="content" source="media/first-app/vsmac/name-project.png" alt-text="Name your .NET MAUI project and specify its location.":::

01. Wait for the project to be created, and its dependencies to be restored:

    :::image type="content" source="media/first-app/vsmac/project-dependencies-restored.png" alt-text="Ensure the project dependencies have been restored.":::

:::zone-end

:::zone pivot="devices-android"

<!-- markdownlint-disable MD029 -->
07. Visual Studio 2022 for Mac 17.4 Preview will display an infobar if your Mac lacks the Android SDK components. Click the **Install** button to install the Android SDK:

    :::image type="content" source="media/first-app/vsmac/android-install-sdk.png" alt-text="Install the missing Android SDK components.":::

    <!-- markdownlint-enable MD029 -->

01. In the **Android SDK** window, select the **Download and install Android dependencies automatically** checkbox, and click the **Download and Install** button:

    :::image type="content" source="media/first-app/vsmac/android-sdk-download.png" alt-text="Download and install the Android SDK.":::

01. In the **License Acceptance** window, click the **Accept** button:

    :::image type="content" source="media/first-app/vsmac/android-sdk-license-acceptance.png" alt-text="Accept the Android SDK license.":::

    Wait for Visual Studio 2022 for Mac 17.4 Preview to download the Android SDK:

01. In the Visual Studio 2022 for Mac toolbar (CHECK IT'S CALLED THIS), change the debug target to **Android Emulator**:

    :::image type="content" source="media/first-app/vsmac/android-emulator-debug-target.png" alt-text="Set the Android emulator as the debug target.":::

01. In the Visual Studio 2022 for Mac toolbar, press the **Start** button (the triangular button that resembles a Play button) to build the app and attempt to launch it:

    :::image type="content" source="media/first-app/vsmac/android-emulator-run.png" alt-text="Build and launch the app in an Android emulator.":::

    Visual Studio 2022 for Mac will build the app, and then will create a default Android emulator.

01. In the **New Device** window, click the **Create** button:

    :::image type="content" source="media/first-app/vsmac/android-create-emulator.png" alt-text="Create a default Android emulator.":::

01. In the **License Acceptance** window, click the **Accept** button:

    :::image type="content" source="media/first-app/vsmac/android-emulator-accept-license.png" alt-text="Accept the Android emulator license.":::

    Wait for Visual Studio 2022 for Mac to download and install an Android emulator.

01. Close the **Android Device Manager** window:

    :::image type="content" source="media/first-app/vsmac/android-device-manager.png" alt-text="Close the Android Device Manager window.":::

01. In the Visual Studio 2022 for Mac toolbar, change the debug target to **Pixel 5 - API 31 (API 31)**:

    :::image type="content" source="media/first-app/vsmac/android-pixel5-debug-target.png" alt-text="Set the created Android emulator as the debug target.":::

01. In the Visual Studio 2022 for Mac toolbar, press the **Start** button (the triangular button that resembles a Play button) to launch the app inside the Android emulator:

    :::image type="content" source="media/first-app/vsmac/android-pixel5-run.png" alt-text="Launch the app in the default Android emulator.":::

    Visual Studio 2022 for Mac will start the Android emulator, build the app, and deploy the app to the emulator:

01. In the running app in the Android emulator, press the **Click me** button several times and observe that the count of the number of buttons clicks is incremented:

    :::image type="content" source="media/first-app/vsmac/android-running-app.png" alt-text=".NET MAUI app running in an Android emulator on a Mac.":::

:::zone-end

:::zone pivot="devices-ios"

<!-- markdownlint-disable MD029 -->
07. In the Visual Studio 2022 for Mac toolbar, ensure that the debug target is set to an iOS Simulator:

    :::image type="content" source="media/first-app/vsmac/ios-debug-target.png" alt-text="Ensure the debug target is set to an iOS Simulator.":::

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio 2022 for Mac toolbar, press the **Start** button (the triangular button that resembles a Play button) to launch the app in your chosen iOS simulator:

    :::image type="content" source="media/first-app/vsmac/ios-run.png" alt-text="Launch the app in your chosen iOS Simulator.":::

01. In the running app, press the **Click me** button several times and observe that the count of the number of buttons clicks is incremented:

    :::image type="content" source="media/first-app/vsmac/ios-running-app.png" alt-text=".NET MAUI app running in iOS Simulator on a Mac.":::

:::zone-end

:::zone pivot="devices-maccatalyst"

<!-- markdownlint-disable MD029 -->
07. In the Visual Studio 2022 for Mac toolbar, ensure that the debug target is set to **My Mac**:

    :::image type="content" source="media/first-app/vsmac/mac-debug-target.png" alt-text="Ensure the debug target is set to My Mac.":::

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio 2022 for Mac toolbar, press the **Start** button (the triangular button that resembles a Play button) to launch the app on your Mac:

    :::image type="content" source="media/first-app/vsmac/mac-run.png" alt-text="Launch the app on your Mac.":::

01. In the running app, press the **Click me** button several times and observe that the count of the number of buttons clicks is incremented:

    :::image type="content" source="media/first-app/vsmac/mac-running-app.png" alt-text=".NET MAUI app running on a Mac.":::

:::zone-end

:::zone pivot="devices-windows"

Windows apps can only be launched and debugged using Visual Studio 2022.

:::zone-end

---

## Next steps

In this tutorial, you've learnt how to create and run your first .NET Multi-platform App UI (.NET MAUI) app in Visual Studio 2022 17.3 on Windows.

To learn the fundamentals of building an app with .NET MAUI, and more advanced topics such as local data storage and invoking REST-based web services, see [Build mobile and desktop apps with .NET MAUI](/learn/paths/build-apps-with-dotnet-maui/).
