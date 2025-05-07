---
title: "Install Visual Studio 2022 and Visual Studio Code to develop cross-platform apps using .NET MAUI"
description: "Learn how to install Visual Studio 2022 and Visual Studio Code with the .NET MAUI extension to develop native, cross-platform apps using .NET MAUI."
ms.date: 01/06/2025
monikerRange: ">=net-maui-9.0"
---

# Installation

Developing native, cross-platform .NET Multi-platform App UI (.NET MAUI) apps requires Visual Studio 2022 17.12 or greater, or the latest Visual Studio Code with the .NET MAUI extension.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/visual-studio)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET MAUI apps on Windows, install Visual Studio 2022 17.12 or greater by following the [installation](#installation) steps.

## Prerequisites

- Visual Studio 2022 17.12 or greater. For information about supported operating systems, hardware, supported languages, and additional requirements and guidance, see [Visual Studio 2022 System Requirements](/visualstudio/releases/2022/system-requirements).

To build, sign, and deploy .NET MAUI apps for iOS and Mac Catalyst, you'll also need:

- A Mac that is compatible with Xcode. For more information, see Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/).
- A specific version of Xcode, which depends on the version of .NET MAUI that you're using. For information, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions).
- An [Apple ID](https://appleid.apple.com/account) and paid [Apple Developer Program](https://developer.apple.com/programs) enrollment. An Apple ID is required to deploy apps to devices, and to submit apps to the Apple Store.

Alternatively, to deploy debug builds of your app directly from Windows to your iOS device with [hot restart](~/ios/hot-restart.md), you'll need:

- An [Apple Developer account](https://appleid.apple.com/account) and paid [Apple Developer Program](https://developer.apple.com/programs) enrollment.

## Installation

1. To create .NET MAUI apps, you'll need to download the latest version of Visual Studio 2022:

    - [Download Visual Studio 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2305)

    - [Download Visual Studio 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2305)

    - [Download Visual Studio 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2305)

1. Either install Visual Studio, or modify your existing installation through the Visual Studio installer, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/installation/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio Code](#tab/visual-studio-code)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET MAUI apps on Windows, macOS, or Linux, install the latest Visual Studio Code by following the [installation](#install-visual-studio-code) steps.

## Prerequisites

To build, sign, and deploy .NET MAUI apps for iOS and Mac Catalyst, you'll need:

- A Mac that is compatible with Xcode. For more information, see Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/).
- A specific version of Xcode, which depends on the version of .NET MAUI that you're using. For information, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions).
- An [Apple ID](https://appleid.apple.com/account) and paid [Apple Developer Program](https://developer.apple.com/programs) enrollment. An Apple ID is required to deploy apps to devices, and to submit apps to the Apple Store.

## Install Visual Studio Code

1. To create .NET MAUI apps, you'll need to download the latest version of Visual Studio Code:

    - [Download Visual Studio Code](https://code.visualstudio.com)

1. Install Visual Studio Code. For detailed instructions on how to install Visual Studio Code, see [Visual Studio Code on Windows](https://code.visualstudio.com/docs/setup/windows), [Visual Studio Code on macOS](https://code.visualstudio.com/docs/setup/mac), and [Visual Studio Code on Linux](https://code.visualstudio.com/docs/setup/linux).

## Install the .NET MAUI extension

Before you can create .NET MAUI apps in Visual Studio Code you'll need to install the .NET MAUI extension:

1. Launch Visual Studio Code.
1. In Visual Studio Code, navigate to the **Extensions** tab and search for ".NET MAUI". Then select the [.NET MAUI](https://aka.ms/mauidevkit-marketplace) extension and install it by pressing the **Install** button:

    :::image type="content" source="media/installation/vscode/maui-extension.png" alt-text="Screenshot of the Visual Studio Code extension pane showing the .NET MAUI extension.":::

    The .NET MAUI extension automatically installs the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) and [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extensions, which are required for the .NET MAUI extension to run. For more information about C# Dev Kit, see [C# Dev Kit for Visual Studio Code](/visualstudio/subscriptions/vs-c-sharp-dev-kit).

## Connect your account to C# Dev Kit

Using C# Dev Kit requires you to sign in with a Microsoft account that has an active Visual Studio subscription:

1. In the **Welcome** tab for getting started with .NET MAUI, press the **Connect** button:

    :::image type="content" source="media/installation/vscode/connect-account.png" alt-text="Screenshot of the Visual Studio Code connect account to c# dev kit button.":::

    Follow the prompts to sign into your Microsoft account. For more information, see [Signing in to C# Dev Kit](https://code.visualstudio.com/docs/csharp/signing-in#_sign-in-with-a-microsoft-or-organizational-account).

For more information about C# Dev Kit licensing, see [C# Dev Kit FAQ](https://code.visualstudio.com/docs/csharp/cs-dev-kit-faq).

## Set up your .NET environment

You'll need the .NET SDK installed on your machine to develop .NET MAUI apps. If you don't have the .NET SDK installed on your machine, the preferred approach to installing it on Windows is through the Visual Studio Installer. For more information, see [Installation](installation.md?tabs=visual-studio).

Alternatively, to manually install the .NET SDK:

1. Download the [.NET installer](https://aka.ms/dotnet-extensionpack-sdk).
1. Install the .NET SDK by running the .NET installer. For more information, see [Install .NET on Windows, Linux, and macOS](/dotnet/core/install/).

    > [!TIP]
    > On Linux, you can install the .NET SDK using the [scripted installation instructions](/dotnet/core/install/linux-scripted-manual#scripted-install).

To verify that the .NET SDK is installed:

1. Open a terminal.
1. In the terminal, run the following command:

    ```dotnetcli
    dotnet --version
    ```

    You should see the version of the .NET SDK that you've installed.

    > [!NOTE]
    > It may be necessary to restart your machine before verifying that the .NET SDK is installed.

## Set up your .NET MAUI environment

You'll need the .NET MAUI SDK installed on your machine to develop .NET MAUI apps. If you don't have the .NET MAUI SDK installed on your machine, the preferred approach to installing it on Windows is through the Visual Studio Installer. For more information, see [Installation](installation.md?tabs=visual-studio).

Alternatively, to manually install the .NET MAUI SDK:

1. Open a terminal.
1. In the terminal on Windows, run the following command:

    ```dotnetcli
    dotnet workload install maui
    ```

    In the terminal on macOS, run the following command:

    ```dotnetcli
    sudo dotnet workload install maui
    ```

    In the terminal on Linux, run the following command:

    ```dotnetcli
    dotnet workload install maui-android
    ```

To verify that the .NET MAUI SDK is installed:

1. Open a terminal.
1. In the terminal, run the following command:

    ```dotnetcli
    dotnet workload list
    ```

    On Windows and macOS, you should see the `maui` workload ID listed alongside the installed version. However, if you've installed it through the Visual Studio Installer on Windows the following workload IDs are listed:

    ```
    android
    maui-windows
    maccatalyst
    ios
    ```

    On Linux, you should see the `maui-android` workload ID listed alongside the installed version.

## Set up target platforms

To build and debug a .NET MAUI app, you'll need to have a valid target platform relative to your development machine's operating system. The following table lists the supported target platforms on each operating system:

| Your Operating System | Supported Target Platforms |
|---|---|
| Windows | Windows, Android |
| macOS | Android, iOS, macOS |
| Linux | Android |

Building a .NET MAUI app for Android, and for Apple platforms, requires you to perform additional set up and configuration.

### Android

To process to set up your machine for .NET MAUI development on Android with Visual Studio Code is:

- Download and install the Java SDK For more information, see [Download and install the Java JDK](#download-and-install-the-java-sdk).
- Download and install the Android SDK. For more information, see [Download and install the Android SDK](#download-and-install-the-android-sdk).
- Download and install an Android emulator. For more information, see [Download and install an Android emulator](#download-and-install-an-android-emulator).

#### Download and install the Java SDK

To download and install the Java SDK, and configure Visual Studio Code to use it:

1. Download and install [Microsoft OpenJDK 17](/java/openjdk/download). For information about installing the OpenJDK, see [Install the Microsoft Build of OpenJDK](/java/openjdk/install).

    Alternatively, rather than manually downloading and installing the Java SDK, you can use the `InstallAndroidDependencies` build target to install the Java SDK (and the Android SDK). For more information, see [Using the InstallAndroidDependencies target](#using-the-installandroiddependencies-target).

    > [!IMPORTANT]
    > Ensure that you note the location that the OpenJDK is installed to, as this is required in the next step.

1. Ensure that you've configured the path to OpenJDK via one of the following approaches:
    1. Set the `JAVA_HOME` environment variable to define the Java SDK path for your machine. This is the recommended approach, which defines the Java SDK path at the machine level.

        > [!NOTE]
        > If you install the OpenJDK on Windows via MSI, you can opt into the installer setting the `JAVA_HOME` environmental variable.

    1. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> on Windows, or <kbd>CMD+SHIFT+P</kbd> on macOS, and then select **.NET MAUI: Configure Android**, followed by **Select Java SDK location** to set the Java SDK path at the user/workspace level.
    1. Configure the Java SDK path in your .csproj file by setting the `$(JavaSdkDirectory)` MSBuild property to the OpenJDK path. This will define the Java SDK path at the project level.

#### Download and install the Android SDK

To download and install the Android SDK, and configure Visual Studio Code to use it:

1. Download and install the Android SDK via one of the following approaches:
    1. Download and install the Android SDK by creating a new .NET MAUI project and then use the `InstallAndroidDependencies` build target, which helps set up your Android environment. This is the recommended approach. For more information, see [Using the InstallAndroidDependencies target](#using-the-installandroiddependencies-target).
    1. Download and install the Android SDK on Windows by [installing the .NET MAUI development workload in Visual Studio](installation.md?tabs=visual-studio), and then [creating and running a .NET MAUI app on Android](first-app.md?pivots=devices-android&tabs=visual-studio). This process will ensure that the Android SDK and an Android emulator are installed.
    1. Download and install the Android SDK through Android Studio. For more information, see [Install Android Studio](https://developer.android.com/studio/install) on developer.android.com.
    1. Download and install the Android SDK through your preferred package manager on Linux.

1. Ensure that you've configured the path to the Android SDK via one of the following approaches:
    1. Set the `ANDROID_HOME` environment variable to define the Android SDK path for your machine. This is the recommended approach, which defines the Android SDK path at the machine level.
    1. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> on Windows, or <kbd>CMD+SHIFT+P</kbd> on macOS, and then select **.NET MAUI: Configure Android**, followed by **Select Android SDK location** to set the Android SDK path at the user/workspace level.
    1. Configure the Android SDK path in your .csproj file by setting the `$(AndroidSdkDirectory)` MSBuild property to the Android SDK path. This will define the Android SDK path at the project level.

1. In Visual Studio Code, verify that your Android environment is configured correctly by pressing <kbd>CTRL+SHIFT+P</kbd> on Windows, or <kbd>CMD+SHIFT+P</kbd> on macOS, and then selecting **.NET MAUI: Configure Android**, followed by **Refresh Android environment**. Any detected errors must be addressed:
    - In the command palette, select **.NET MAUI: Configure Android** followed by both **Select Android SDK location** and **Select Android SDK location** and validate that they correctly point to installations of each. On Windows, if you install the SDKs via Visual Studio, OpenJDK will be located at *C:\Program Files\Microsoft* and the Android SDK will be located at *C:\Program Files (x86)\Android\android-sdk*.
    - Ensure that your Android SDK folder has sub-folders such as *build-tools*, *cmdline-tools*, and *platform-tools*.
    - Ensure that your OpenJDK folder has sub-folders such as *bin*, *lib*, and more.
    - Ensure that the `ANDROID_HOME` environment variable is set to your Android SDK path.
    - Ensure that the `JAVA_HOME` environment variable is set to the your Java SDK path.
    - If Android licenses haven't been accepted, in an elevated terminal navigate to your Android SDK's *cmdline-tools/{version}/bin* folder and run `sdkmanager --licenses` and then follow the CLI prompts.

##### Using the InstallAndroidDependencies target

The recommended approach to installing the required dependencies for your .NET MAUI project on Android is to run the [InstallAndroidDependencies](/dotnet/android/building-apps/build-targets#installandroiddependencies) MSBuild target. This target will install the Android SDK for you, if it isn't already installed.

In a terminal, create a new .NET MAUI project:

```dotnetcli
dotnet new maui -n "MyMauiApp"
```

In a terminal, change directory to *MyMauiApp*, and build the app while specifying the `InstallAndroidDependencies` build target:

```dotnetcli
cd MyMauiApp
dotnet build -t:InstallAndroidDependencies -f:net9.0-android -p:AndroidSdkDirectory="/path/to/sdk" -p:AcceptAndroidSDKLicenses=True
```

> [!NOTE]
> The `InstallAndroidDependencies` MSBuild target can also install the Java SDK if the `JavaSdkDirectory` MSBuild property is provided.

In the command above:

- `-p:AndroidSdkDirectory="/path/to/sdk"` installs or updates Android dependencies to the specified absolute path. Suggested paths are *%LOCALAPPDATA%/Android/Sdk* on Windows, and *$HOME/Library/Android/sdk* on macOS.
- `-p:AcceptAndroidSDKLicenses=True` accepts the required Android licenses for development.
- (optional) `-p:JavaSdkDirectory="/path/to/sdk"` installs the Java SDK to the specified absolute path.

Try to avoid using paths that contain spaces or non-ASCII characters.

#### Download and install an Android emulator

To download and install an Android emulator on which to run your apps:

1. In a terminal, navigate to the *{YOUR_ANDROID_SDK_FOLDER}/cmdline-tools/{version}/bin/*.
1. In a terminal, use the `sdkmanager` command to download and install an Android emulator:

    On Windows, run the following commands:

    ```console
    sdkmanager --install emulator
    sdkmanager --install "system-images;android-35;google_apis;x86_64"
    ```

    On macOS, run the following commands:

    ```console
    ./sdkmanager --install emulator
    ./sdkmanager --install "system-images;android-35;google_apis;arm64-v8a"
    ```

    > [!NOTE]
    > The above command assumes an Apple Silicon Mac. For an Intel Mac, replace `arm64-v8a` with `x86_64`.

    For more information about the `sdkmanager` command, see [sdkmanager](https://developer.android.com/tools/sdkmanager) on developer.android.com.

1. In a terminal, use the `avdmanager` command to create a new Android emulator:

    On Windows, run the following commands:

    ```console
    avdmanager create avd -n MyAndroidVirtualDevice-API35 -k "system-images;android-35;google_apis;x86_64"
    ```

    On macOS, run the following commands:

    ```console
    ./avdmanager create avd -n MyAndroidVirtualDevice-API35 -k "system-images;android-35;google_apis;arm64-v8a"
    ```

    > [!NOTE]
    > The above command assumes an Apple Silicon Mac. For an Intel Mac, replace `arm64-v8a` with `x86_64`.

    For more information about the `avdmanager` command, see [avdmanager](https://developer.android.com/tools/avdmanager) on developer.android.com.

### iOS and macOS

To set up your Mac for .NET MAUI development on iOS and Mac Catalyst with Visual Studio Code:

1. Install the version of Xcode that's required by the version of .NET MAUI that you're using. For information, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions). The latest stable Xcode release can be downloaded from the [Apple App Store](https://apps.apple.com/us/app/xcode/id497799835?mt=12).
1. In a terminal, run the following command to acquire the Xcode command line tools:

    ```console
    xcode-select --install
    ```

1. Launch Xcode and accept any license agreements. If simulators don't start installing, navigate to **Xcode > Settings > Components** and install your chosen simulator runtimes.
1. In Visual Studio Code, verify that your Apple environment is configured correctly by pressing <kbd>CMD+SHIFT+P</kbd> and then selecting **.NET MAUI: Configure Apple**, followed by **Refresh Apple environment**. Any detected errors must be addressed:
    - Ensure you've ran `xcode-select --install` in a terminal.
    - If you receive an error that Xcode hasn't been found, run `xcode-select -p` in a terminal and check that it returns a path to your Xcode installation.
    - Open Xcode to ensure it loads correctly, and then navigate to **Xcode > Settings > Location** and check that the **Command Line Tools** field is pointing to the correct Xcode installation.

## Troubleshooting

If you encounter issues when installing the .NET MAUI extension in Visual Studio Code, more information about the issues can be found by navigating to the **Output** window (<kbd>CTRL+SHIFT+U</kbd> on Windows or <kbd>CMD+SHIFT+U</kbd> on macOS) and selecting **.NET MAUI** in the drop-down.

## Provide feedback

To provide feedback about the .NET MAUI extension from inside Visual Studio Code, navigate to the **Help > Report Issue** dialog. Then, ensure you select "Bug Report" as the value of the **This is a** drop-down, "A VS Code extension" as the value of the **For** drop-down, and ".NET MAUI" as the value of the **Extension** drop-down:

:::image type="content" source="media/installation/vscode/report-issue.png" alt-text="Picture of the report issue dialog in Visual Studio Code":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio Code (Experimental)](#tab/visual-studio-code-experimental)
<!-- markdownlint-enable MD025 -->

## Prerequisites

To build, sign, and deploy .NET MAUI apps for iOS and Mac Catalyst, you'll need:

- A Mac that is compatible with Xcode. For more information, see Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/).
- A specific version of Xcode, which depends on the version of .NET MAUI that you're using. For information, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions).
- An [Apple ID](https://appleid.apple.com/account) and paid [Apple Developer Program](https://developer.apple.com/programs) enrollment. An Apple ID is required to deploy apps to devices, and to submit apps to the Apple Store.

To start developing native, cross-platform .NET MAUI apps on Windows, macOS, or Linux, install the latest Visual Studio Code by following the [installation](#install-visual-studio-code) steps.

To build and debug a .NET MAUI app, you'll need to have a valid target platform relative to your development machine's operating system. The following table lists the supported target platforms on each operating system:

| Your Operating System | Supported Target Platforms |
|---|---|
| Windows | Windows, Android |
| macOS | Android, iOS, macOS |
| Linux | Android |

## Install Visual Studio Code

1. To create .NET MAUI apps, you'll need to download the latest version of Visual Studio Code:

    - [Download Visual Studio Code](https://code.visualstudio.com)

1. Install Visual Studio Code. For detailed instructions on how to install Visual Studio Code, see [Visual Studio Code on Windows](https://code.visualstudio.com/docs/setup/windows), [Visual Studio Code on macOS](https://code.visualstudio.com/docs/setup/mac), and [Visual Studio Code on Linux](https://code.visualstudio.com/docs/setup/linux).

## Install the .NET MAUI extension

Before you can create .NET MAUI apps in Visual Studio Code you'll need to install the .NET MAUI extension:

1. Launch Visual Studio Code.
1. In Visual Studio Code, navigate to the **Extensions** tab and search for ".NET MAUI". Then select the [.NET MAUI](https://aka.ms/mauidevkit-marketplace) extension and install it by pressing the **Install** button:

    :::image type="content" source="media/installation/vscode/maui-extension.png" alt-text="Screenshot of the Visual Studio Code extension pane showing the .NET MAUI extension.":::

    The .NET MAUI extension automatically installs the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) and [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) extensions, which are required for the .NET MAUI extension to run. For more information about C# Dev Kit, see [C# Dev Kit for Visual Studio Code](/visualstudio/subscriptions/vs-c-sharp-dev-kit).

## Follow the steps in the walkthrough

Once you install the .NET MAUI extension, you will be welcomed by the .NET MAUI walkthrough, "Get Started with .NET MAUI".

> [!IMPORTANT]
> To access the improved setup experience, the **Maui > Configuration: Improved Setup Experience** [setting must be turned on](#access-the-improved-setup-experience).

Click through and follow the prompts at each step to fully configure your .NET MAUI environment and target platforms:

1. [Connect your account to the C# Dev Kit extension](#connect-your-account-to-c-dev-kit-1)
1. [Set up your .NET environment](#set-up-your-net-environment-1)
1. [Set up your .NET MAUI environment](#set-up-your-net-maui-environment-1)
1. [Set up your iOS and macOS environment](#set-up-your-ios-and-macos-environment)
1. [Set up your Android environment](#set-up-your-android-environment)

> [!NOTE]
> You will need to be on a Mac to see and follow the **Set up your iOS and macOS environment** step.

The **Set up your Android environment** step will set up everything you need to be successful with Android as a target platform, and the **Set up your iOS and macOS environment** step will set up everything you need to be successful with iOS and Mac as target platforms.

This walkthrough can also be accessed from the command palette. Select **Welcome: Open Walkthrough...** followed by **Get Started with .NET MAUI**.

### Access the improved setup experience

1. In Visual Studio Code, press <kbd>CTRL+SHIFT+P</kbd> on Windows, or <kbd>CMD+SHIFT+P</kbd> on macOS, select **Preferences: Open Settings (UI)**.

    :::image type="content" source="media/installation/vscode/settings.png" alt-text="Screenshot of the Visual Studio Code settings menu option.":::

1. Navigate to Extensions > .NET MAUI and check on the **Maui > Configuration: Improved Setup Experience** setting.

    :::image type="content" source="media/installation/vscode/experimental-setting-improved-setup.png" alt-text="Screenshot of the Visual Studio Code improved dotnet maui setup experimental setting.":::

1. Return to the **Get Started with .NET MAUI** walkthrough. Ensure that you are able to see **Set up your Android environment** among other steps in the walkthrough.

    :::image type="content" source="media/installation/vscode/walkthrough.png" alt-text="Screenshot of the Visual Studio Code extension pane showing the .NET MAUI walkthrough.":::

### Connect your account to C# Dev Kit

Using C# Dev Kit requires you to sign in with a Microsoft account that has an active Visual Studio subscription:

1. In the **Welcome** tab for getting started with .NET MAUI, press the **Connect** button:

    :::image type="content" source="media/installation/vscode/connect-account.png" alt-text="Screenshot of the Visual Studio Code connect account to c# dev kit button.":::

    Follow the prompts to sign into your Microsoft account. For more information, see [Signing in to C# Dev Kit](https://code.visualstudio.com/docs/csharp/signing-in#_sign-in-with-a-microsoft-or-organizational-account).

For more information about C# Dev Kit licensing, see [C# Dev Kit FAQ](https://code.visualstudio.com/docs/csharp/cs-dev-kit-faq).

### Set up your .NET environment

You'll need the .NET SDK installed on your machine to develop .NET MAUI apps. If you don't have the .NET SDK installed on your machine, the preferred approach to installing it on Windows is through the Visual Studio Installer. For more information, see [Installation](installation.md?tabs=visual-studio).

Alternatively, to manually install the .NET SDK:

1. Download the [.NET installer](https://aka.ms/dotnet-extensionpack-sdk).
1. Install the .NET SDK by running the .NET installer. For more information, see [Install .NET on Windows, Linux, and macOS](/dotnet/core/install/).

    > [!TIP]
    > On Linux, you can install the .NET SDK using the [scripted installation instructions](/dotnet/core/install/linux-scripted-manual#scripted-install).

To verify that the .NET SDK is installed:

1. Open a terminal.
1. In the terminal, run the following command:

    ```dotnetcli
    dotnet --version
    ```

    You should see the version of the .NET SDK that you've installed.

    > [!NOTE]
    > It may be necessary to restart your machine before verifying that the .NET SDK is installed.

### Set up your .NET MAUI environment

You'll need the .NET MAUI SDK installed on your machine to develop .NET MAUI apps. If you don't have the .NET MAUI SDK installed on your machine, the preferred approach to installing it on Windows is through the Visual Studio Installer. For more information, see [Installation](installation.md?tabs=visual-studio).

Alternatively, to manually install the .NET MAUI SDK:

1. Open a terminal.
1. In the terminal on Windows, run the following command:

    ```dotnetcli
    dotnet workload install maui
    ```

    In the terminal on macOS, run the following command:

    ```dotnetcli
    sudo dotnet workload install maui
    ```

    In the terminal on Linux, run the following command:

    ```dotnetcli
    dotnet workload install maui-android
    ```

To verify that the .NET MAUI SDK is installed:

1. Open a terminal.
1. In the terminal, run the following command:

    ```dotnetcli
    dotnet workload list
    ```

    On Windows and macOS, you should see the `maui` workload ID listed alongside the installed version. However, if you've installed it through the Visual Studio Installer on Windows the following workload IDs are listed:

    ```
    android
    maui-windows
    maccatalyst
    ios
    ```

    On Linux, you should see the `maui-android` workload ID listed alongside the installed version.

### Set up your Android environment

To set up your Android development environment, simply click on the **Configure Android Environment** button from the **Set up your Android Environment** walkthrough step to enter the full Android acquisition flow.

:::image type="content" source="media/installation/vscode/walkthrough-step-android.png" alt-text="Screenshot of the Visual Studio Code Configure Android Environment button.":::

The Android acquisition flow will analyze your Android environment and offer to install all missing components:

* **Android SDK and Java SDK**: Android SDK and Java SDK components are required.

  * The Android acquisition flow will prompt you to install these components directly, or to select a preexisting installation you may already have.

    :::image type="content" source="media/installation/vscode/android-installation-popup.png" alt-text="Screenshot of the Visual Studio Code Android installation popup.":::

  * To configure your SDK and JDK installations further, see instructions on how to [install and configure Android manually](?tabs=visual-studio-code#android).

* **Android emulator**: Installing an Android emulator is recommended, and required in the absence of a physical device.

  * The Android acquisition flow will prompt you to install a default emulator directly.

    :::image type="content" source="media/installation/vscode/android-avd-installation-popup.png" alt-text="Screenshot of the Visual Studio Code Android Virtual Device installation popup.":::

  * To configure emulators further, follow the instructions to [install an Android emulator manually](?tabs=visual-studio-code#download-and-install-an-android-emulator).

> [!IMPORTANT]
> Android licenses will need to be manually reviewed and accepted in Terminal. When prompted, review each license. To accept, type 'y', and press 'Enter'.

When your Android environment has been properly configured, you will be notified, "Your Android environment for .NET MAUI is all set!"

:::image type="content" source="media/installation/vscode/android-setup-success.png" alt-text="Screenshot of the Android environment set up success message.":::

View the Output pane for more details on your Android environment status.

### Set up your iOS and macOS environment

To set up your Mac for .NET MAUI development on iOS and Mac Catalyst with Visual Studio Code:

1. Install the version of Xcode that's required by the version of .NET MAUI that you're using. For information, see [Release versions](https://github.com/dotnet/maui/wiki/Release-Versions). The latest stable Xcode release can be downloaded from the [Apple App Store](https://apps.apple.com/us/app/xcode/id497799835?mt=12).
1. In a terminal, run the following command to acquire the Xcode command line tools:

    ```console
    xcode-select --install
    ```

1. Launch Xcode and accept any license agreements. If simulators don't start installing, navigate to **Xcode > Settings > Components** and install your chosen simulator runtimes.
1. In Visual Studio Code, verify that your Xcode environment is configured simply by clicking **Configure iOS and macOS Environment** from the **Set up your iOS and macOS Environment** walkthrough step.

:::image type="content" source="media/installation/vscode/walkthrough-step-ios.png" alt-text="Screenshot of the Visual Studio Code Configure iOS and macOS Environment button.":::

Any detected errors must be addressed:

- Ensure you've ran `xcode-select --install` in a terminal.
- If you receive an error that Xcode hasn't been found, run `xcode-select -p` in a terminal and check that it returns a path to your Xcode installation.
- Open Xcode to ensure it loads correctly, and then navigate to **Xcode > Settings > Location** and check that the **Command Line Tools** field is pointing to the correct Xcode installation.

When your Xcode environment has been properly configured, you will be notified, "Your iOS and macOS environment for .NET MAUI is all set!"

:::image type="content" source="media/installation/vscode/ios-setup-success.png" alt-text="Screenshot of the iOS and macOS environment set up success message.":::

## Provide feedback

To provide feedback about the .NET MAUI extension from inside Visual Studio Code, navigate to the **Help > Report Issue** dialog. Then, ensure you select "Bug Report" as the value of the **This is a** drop-down, "A VS Code extension" as the value of the **For** drop-down, and ".NET MAUI" as the value of the **Extension** drop-down:

:::image type="content" source="media/installation/vscode/report-issue.png" alt-text="Picture of the report issue dialog in Visual Studio Code":::

---

## Next steps

To learn how to create and run your first .NET MAUI app in Visual Studio 2022 on Windows, or Visual Studio Code, click the button below.

> [!div class="nextstepaction"]
> [Build your first app](first-app.md)
