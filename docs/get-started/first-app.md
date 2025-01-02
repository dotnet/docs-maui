---
title: "Build your first .NET MAUI app"
description: "Learn how to create and run your first .NET MAUI app in Visual Studio 2022 on Windows, or Visual Studio Code with the .NET MAUI extension"
ms.date: 01/02/2025
zone_pivot_groups: devices-platforms
monikerRange: ">=net-maui-9.0"
---

# Build your first app

In this tutorial, you'll learn how to create and run your first .NET Multi-platform App UI (.NET MAUI) app in Visual Studio 2022 on Windows or Visual Studio Code on Windows, macOS, or Linux. This will help to ensure that your development environment is correctly set up.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vswin)
<!-- markdownlint-enable MD025 -->

## Prerequisites

- Visual Studio 2022 17.12 or greater, with the .NET Multi-platform App UI workload installed. For more information, see [Installation](installation.md?tabs=vswin).

## Create an app

:::zone pivot="devices-android"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 and run it on an Android emulator:

:::zone-end

:::zone pivot="devices-ios"

Developing .NET MAUI apps for iOS on Windows requires a Mac build host. If you don't specifically need to target iOS and don't have a Mac, consider getting started with Android or Windows instead.

In this tutorial, you'll create your first .NET MAUI app in Visual Studio and run it on an iOS simulator:

:::zone-end

:::zone pivot="devices-windows"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio 2022 and run it on Windows:

:::zone-end

:::zone pivot="devices-android, devices-ios, devices-windows"

01. Launch Visual Studio 2022. In the start window, click **Create a new project** to create a new project:

    :::image type="content" source="media/first-app/vs/new-solution.png" alt-text="New solution.":::

01. In the **Create a new project** window, select **MAUI** in the **All project types** drop-down, select the **.NET MAUI App** template, and click the **Next** button:

    :::image type="content" source="media/first-app/vs/new-project.png" alt-text="Choose a template.":::

01. In the **Configure your new project** window, name your project, choose a suitable location for it, and click the **Next** button:

    :::image type="content" source="media/first-app/vs/configure-project.png" alt-text="Configure the project.":::

01. In the **Additional information** window, choose the version of .NET that you'd like to target and click the **Create** button:

    :::image type="content" source="media/first-app/vs/additional-information.png" alt-text="Additional information.":::

01. Wait for the project to be created and its dependencies to be restored:

    :::image type="content" source="media/first-app/vs/restored-dependencies.png" alt-text="Restored dependencies.":::

:::zone-end

:::zone pivot="devices-android"

<!-- markdownlint-disable MD029 -->
06. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **Android Emulators** and then the **Android Emulator** entry:

    :::image type="content" source="media/first-app/vs/android-debug-target.png" alt-text="Select the Android Emulator debugging target for .NET MAUI.":::

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio toolbar, press the **Android Emulator** button:

    :::image type="content" source="media/first-app/vs/android-emulator-button.png" alt-text="Android emulator button.":::

    Visual Studio will start the process of installing the default Android SDK and Android Emulator.

01. In the **Android SDK - License Agreement** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license1.png" alt-text="First Android SDK License Agreement window.":::

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license-uac.png" alt-text="Android SDK license user account control dialog.":::

    Wait for Visual Studio to download the default Android SDK and Android Emulator.

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/vs/android-device-manager-uac.png" alt-text="Android Device Manager user account control dialog.":::

01. In the **Android SDK Platform is missing** dialog, press the **Install** button:

    :::image type="content" source="media/first-app/vs/android-sdk-platform-missing.png" alt-text="Android SDK platform is missing dialog.":::

01. In the **Android SDK - License Agreement** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license2.png" alt-text="Second Android SDK License Agreement window.":::

    Wait for Visual Studio to install the Android SDK components.

01. In the **New Device** window, press the **Create** button:

    :::image type="content" source="media/first-app/vs/new-android-device.png" alt-text="New Android Device window.":::

    Wait for Visual Studio to download, unzip, and create an Android emulator.

01. Close the **Android Device Manager** window:

    :::image type="content" source="media/first-app/vs/android-device-manager.png" alt-text="Android Device Manager window.":::

01. In the Visual Studio toolbar, press the **Pixel 7 - API 35 (Android 15.0 - API 35)** button:

    :::image type="content" source="media/first-app/vs/pixel7-api-35.png" alt-text="Pixel 7 API 35 emulator button.":::

01. In the **Android SDK - License Agreement** window, press the **Accept** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license3.png" alt-text="Third Android SDK License Agreement window.":::

01. In the **User Account Control** dialog, press the **Yes** button:

    :::image type="content" source="media/first-app/vs/android-sdk-license-uac.png" alt-text="Android SDK license user account control dialog.":::

    Wait for Visual Studio to install to download the Android SDK.

01. In the Visual Studio toolbar, press the **Pixel 7 - API 35 (Android 15.0 - API 35)** button to build and run the app:

    :::image type="content" source="media/first-app/vs/pixel7-api-35.png" alt-text="Pixel 5 API 35 emulator button.":::

    Visual Studio will start the Android emulator, build the app, and deploy the app to the emulator.

    > [!WARNING]
    > Hardware acceleration must be enabled to maximize Android emulator performance. Failure to do this will result in the emulator running very slowly. For more information, see [How to enable hardware acceleration with Android emulators (Hyper-V & AEHD)](~/android/emulator/hardware-acceleration.md).

01. In the running app in the Android emulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/vs/android-running-app.png" alt-text="App running in the Android emulator.":::

## Troubleshooting

If your app fails to compile, review [Troubleshooting known issues](../troubleshooting.md), which may have a solution to your problem. If the problem is related to the Android emulator, see [Android emulator troubleshooting](../android/emulator/troubleshooting.md).

:::zone-end

:::zone pivot="devices-windows"

<!-- markdownlint-disable MD029 -->
06. In the Visual Studio toolbar, press the **Windows Machine** button to build and run the app:

    :::image type="content" source="media/first-app/vs/windows-run-button.png" alt-text="Run .NET MAUI app in Visual Studio button.":::
    <!-- markdownlint-enable MD029 -->

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vs/windows-running-app.png" alt-text=".NET MAUI app running on Windows.":::

## Troubleshooting

If your app fails to compile, review [Troubleshooting known issues](../troubleshooting.md), which may have a solution to your problem.

:::zone-end

:::zone pivot="devices-ios"

<!-- markdownlint-disable MD029 -->
06. In Visual Studio, pair the IDE to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

    <!-- markdownlint-enable MD029 -->

01. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Simulators** and then a specific iOS simulator:

    :::image type="content" source="media/first-app/vs/ios-debug-target.png" alt-text="Visual Studio iOS simulators debug targets.":::

01. In the Visual Studio toolbar, press the Start button for your chosen iOS simulator to build and run your app:

    :::image type="content" source="media/first-app/vs/ios-chosen-debug-target.png" alt-text="Visual Studio iOS simulator debug target choice.":::

    Visual Studio will build the app, start the remote iOS Simulator for Windows, and deploy the app to the remote simulator. For more information about the remote iOS Simulator for Windows, see [Remote iOS Simulator for Windows](~/ios/remote-simulator.md).

01. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/first-app/vs/ios-running-app.png" alt-text=".NET MAUI app running in iOS Simulator on a Mac.":::

## Troubleshooting

If your app fails to compile, review [Troubleshooting known issues](../troubleshooting.md), which may have a solution to your problem.

:::zone-end

:::zone pivot="devices-maccatalyst"

.NET MAUI apps that target Mac Catalyst can only be launched and debugged using Visual Studio Code.

:::zone-end

<!-- markdownlint-disable MD025 -->
# [Visual Studio Code](#tab/visual-studio-code)
<!-- markdownlint-enable MD025 -->

## Prerequisites

- Visual Studio Code, with the .NET MAUI extension installed and configured:
    - Your Microsoft account connected to C# Dev Kit.
    - The .NET SDK installed.
    - The .NET MAUI SDK installed.
    - Xcode installed on your Mac, including simulator runtimes and the Xcode command line tools, if targeting Apple platforms.
    - Microsoft OpenJDK, the Android SDK, and an Android emulator installed on your machine, if targeting Android.    

For more information, see [Installation](installation.md?tabs=visual-studio-code).

## Create an app

:::zone pivot="devices-android"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio Code and run it on an Android emulator:

:::zone-end

:::zone pivot="devices-ios"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio Code on a Mac, and run it on an iOS simulator:

:::zone-end

:::zone pivot="devices-maccatalyst"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio Code on a Mac, and run it on macOS:

:::zone-end

:::zone pivot="devices-windows"

In this tutorial, you'll create your first .NET MAUI app in Visual Studio Code on Windows, and run it on Windows:

:::zone-end

1. Launch Visual Studio Code. In the **Explorer**, press **Create .NET Project**:

    :::image type="content" source="media/first-app/vscode/create-new-project.png" alt-text="Screenshot of the create new project button in Visual Studio Code.":::

    Alternatively, select <kbd>CTRL+SHIFT+P</kbd> (or <kbd>CTRL+SHIFT+P</kbd> on macOS) and then the **.NET: New Project...** command.

1. In the command palette, select the **.NET MAUI App** template:

    :::image type="content" source="media/first-app/vscode/select-project-template.png" alt-text="Screenshot of selecting the .NET MAUI App template in the command palette in Visual Studio Code.":::

1. In the **Project Location** dialog, select the location where you'd like the new project to be created.
1. In the command palette, enter a name for your new project and press <kbd>ENTER</kdb>:

    :::image type="content" source="media/first-app/vscode/enter-project-name.png" alt-text="Screenshot of entering a project name in the command palette in Visual Studio Code.":::

1. In the command palette, press **Create project**:

    :::image type="content" source="media/first-app/vscode/create-project.png" alt-text="Screenshot of creating a new project in the command palette in Visual Studio Code.":::

    Wait for the project to be created, accepting the folder as a trusted location if required.

1. In the **Explorer**, expand the root node of your project and then open a C# file such as *MainPage.xaml.cs*:

    :::image type="content" source="media/first-app/vscode/mainpage-xaml-cs-open.png" alt-text="Screenshot of a C# file opened in Visual Studio Code.":::

:::zone pivot="devices-android"

<!-- markdownlint-disable MD029 -->
7. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> (or <kbd>CTRL+SHIFT+P</kbd> on macOS) and then select **.NET MAUI: Configure Android**, followed by **Refresh Android environment** to ensure that your Android environment is configured correctly. Any errors must be addressed.
    <!-- markdownlint-enable MD029 -->

1. In the status bar at the bottom of Visual Studio Code, press on the curly brackets symbol **{ }** and ensure that the **Debug Target** is set to a specific Android emulator:

    :::image type="content" source="media/first-app/vscode/android-debug-target.png" alt-text="Screenshot of the debug target in Visual Studio Code set to an Android emulator.":::

    You can also set the debug target by pressing <kbd>CMD+SHIFT+P</kbd> and selecting **.NET MAUI: Pick Android Device** from the command palette.

1. Build and run the app on Android by pressing <kbd>F5</kbd> or by pressing the **Run** button in the upper right corner of Visual Studio Code:

    :::image type="content" source="media/first-app/vscode/mac-run-button.png" alt-text="Screenshot of the run button in Visual Studio Code on macOS.":::

    If you're asked to select a debugger in the command palette, select **C#** and then the launch configuration for your project.

1. In the running app in your chosen Android emulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vscode/android-running-app.png" alt-text="Screenshot of the app running on Android.":::

:::zone-end

:::zone pivot="devices-ios"

<!-- markdownlint-disable MD029 -->
7. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> and then select **.NET MAUI: Configure Apple**, followed by **Refresh Apple environment** to ensure that your iOS environment is configured correctly. Any errors must be addressed.
    <!-- markdownlint-enable MD029 -->

1. In the status bar at the bottom of Visual Studio Code, press on the curly brackets symbol **{ }** and ensure that the **Debug Target** is set to a specific iOS simulator:

    :::image type="content" source="media/first-app/vscode/ios-debug-target.png" alt-text="Screenshot of the debug target in Visual Studio Code set to an iOS simulator.":::

    You can also set the debug target by pressing <kbd>CMD+SHIFT+P</kbd> and selecting **.NET MAUI: Pick iOS Device** from the command palette.

1. Build and run the app on iOS by pressing <kbd>F5</kbd> or by pressing the **Run** button in the upper right corner of Visual Studio Code:

    :::image type="content" source="media/first-app/vscode/mac-run-button.png" alt-text="Screenshot of the run button in Visual Studio Code on macOS.":::

    If you're asked to select a debugger in the command palette, select **C#** and then the launch configuration for your project.

1. In the running app in your chosen iOS simulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vscode/ios-running-app.png" alt-text="Screenshot of the app running on iOS.":::

:::zone-end

:::zone pivot="devices-maccatalyst"

<!-- markdownlint-disable MD029 -->
7. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> and then select **.NET MAUI: Configure Apple**, followed by **Refresh Apple environment** to ensure that your macOS environment is configured correctly. Any errors must be addressed.
    <!-- markdownlint-enable MD029 -->

1. In the status bar at the bottom of Visual Studio Code, press on the curly brackets symbol **{ }** and ensure that the **Debug Target** is set to your Mac:

    :::image type="content" source="media/first-app/vscode/mac-debug-target.png" alt-text="Screenshot of the debug target in Visual Studio Code set to macOS.":::

    You can also set the debug target by pressing <kbd>CMD+SHIFT+P</kbd> and selecting **.NET MAUI: Pick macOS Device** from the command palette.

1. Build and run the app on macOS by pressing <kbd>F5</kbd> or by pressing the **Run** button in the upper right corner of Visual Studio Code:

    :::image type="content" source="media/first-app/vscode/mac-run-button.png" alt-text="Screenshot of the run button in Visual Studio Code on macOS.":::

    If you're asked to select a debugger in the command palette, select **C#** and then the launch configuration for your project.

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vscode/mac-running-app.png" alt-text="Screenshot of the app running on macOS.":::

:::zone-end

:::zone pivot="devices-windows"

<!-- markdownlint-disable MD029 -->
7. In the status bar at the bottom of Visual Studio Code, press on the curly brackets symbol **{ }** and ensure that the **Debug Target** is set to Windows:

    :::image type="content" source="media/first-app/vscode/windows-debug-target.png" alt-text="Screenshot of the debug target in Visual Studio Code set to Windows.":::

    You can also set the debug target by pressing <kbd>CTRL+SHIFT+P</kdb> and selecting **.NET MAUI: Pick Windows Device** from the command palette.
    <!-- markdownlint-enable MD029 -->

1. Build and run the app on Windows by pressing <kbd>F5</kbd> or by pressing the **Run** button in the upper right corner of Visual Studio Code:

    :::image type="content" source="media/first-app/vscode/windows-run-button.png" alt-text="Screenshot of the run button in Visual Studio Code on Windows.":::

    If you're asked to select a debugger in the command palette, select **C#** and then the launch configuration for your project.

1. In the running app, press the **Click me** button several times and observe that the count of the number of button clicks is incremented:

    :::image type="content" source="media/first-app/vscode/windows-running-app.png" alt-text="Screenshot of the app running in Windows.":::

:::zone-end

## Troubleshooting

If your app fails to build and deploy, review [Troubleshooting known issues](~/troubleshooting.md), which may have a solution to your problem.

---

## Next steps

In this tutorial, you've learned how to create and run your first .NET Multi-platform App UI (.NET MAUI) app.

To learn the fundamentals of building an app with .NET MAUI, see [Create a .NET MAUI app](~/tutorials/notes-app/index.yml). Alternatively, for a full .NET MAUI training course, see [Build mobile and desktop apps with .NET MAUI](/training/paths/build-apps-with-dotnet-maui).
