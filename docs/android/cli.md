---
title: "Build an Android app with .NET CLI"
description: "Learn how to create and run a .NET MAUI app on Android using .NET CLI."
ms.date: 01/21/2025
no-loc: [ "MY_EMULATOR_NAME" ]
---

# Build an Android app with .NET CLI

In this tutorial, you'll learn how to create and run a .NET Multi-platform App UI (.NET MAUI) app on Android using .NET Command Line Interface (CLI):

1. To create .NET MAUI apps, you'll need to download and run the [installer](https://github.com/dotnet/installer/blob/main/README.md#installers-and-binaries) for the latest .NET runtime.

2. In a terminal, check that you have the latest .NET runtime installed:

    ```dotnetcli
    dotnet --version
    ```

3. In a terminal, install the latest public build of .NET MAUI:

    ```dotnetcli
    dotnet workload install maui --source https://api.nuget.org/v3/index.json
    ```

    This command will install the latest released version of .NET MAUI, including the required platform SDKs.

4. In a terminal, create a new .NET MAUI app using .NET CLI:

    ```dotnetcli
    dotnet new maui -n "MyMauiApp"
    ```

<!-- markdownlint-disable MD029 -->
5. In a terminal, change directory to *MyMauiApp*, and build and run the app:

    ```dotnetcli
    cd MyMauiApp
    dotnet build -t:Run -f net8.0-android
    ```

    The `dotnet build` command will restore the project dependencies, build the app, and launch it on the default Android emulator or connected device.

6. In the emulator or device, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    > [!NOTE]
    > If no Android emulator is running and no device is connected, the build will fail. Make sure you have an Android emulator running or a device connected via USB debugging before running the command.

<!-- markdownlint-enable MD029 -->

## Launch the app on a specific emulator

A .NET MAUI Android app can be launched on a specific Android emulator by providing its name or identifier. This is useful when you have multiple emulators running simultaneously:

1. First, list all available Android Virtual Devices (AVDs) using the `emulator` command:

    On Windows, run the following command:

    ```console
    %ANDROID_SDK_ROOT%\emulator\emulator -list-avds
    ```

    On macOS/Linux, run the following command:

    ```console
    $ANDROID_SDK_ROOT/emulator/emulator -list-avds
    ```

    This command displays a list of all configured AVDs, for example:

    ```console
    Pixel_7_API_35
    Pixel_Tablet_API_35
    MyAndroidVirtualDevice-API35
    ```

2. Alternatively, you can list running emulators with their ports:

    On Windows, run the following command:

    ```console
    adb devices
    ```

    On macOS/Linux, run the following command:

    ```console
    adb devices
    ```

    This command shows running emulators and connected devices:

    ```console
    List of devices attached
    emulator-5554   device
    emulator-5556   device
    ```

<!-- markdownlint-disable MD029 -->
3. To run your app on a specific emulator, use the `AndroidDeviceUsb` MSBuild property with the emulator's identifier. If using an AVD name, first start the emulator:

    ```console
    %ANDROID_SDK_ROOT%\emulator\emulator -avd MY_EMULATOR_NAME
    ```

    Or on macOS/Linux:

    ```console
    $ANDROID_SDK_ROOT/emulator/emulator -avd MY_EMULATOR_NAME
    ```

4. Then build and run your app targeting the specific emulator using its ADB identifier:

    ```dotnetcli
    dotnet build -t:Run -f net8.0-android -p:AndroidDeviceUsb=emulator-5554
    ```

    Replace `emulator-5554` with the actual identifier of your target emulator from the `adb devices` output.

5. In your chosen emulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

<!-- markdownlint-enable MD029 -->

## Launch the app on a device

A .NET MAUI Android app can be launched on a physical Android device connected via USB. The device must have USB debugging enabled:

1. Connect your Android device to your computer via USB cable.
2. Enable **Developer Options** and **USB Debugging** on your device. For more information, see [Set up Android device for debugging](~/android/device/setup.md).
3. Verify your device is recognized by running:

    ```console
    adb devices
    ```

    You should see your device listed with a unique identifier:

    ```console
    List of devices attached
    1A2B3C4D5E6F    device
    ```

<!-- markdownlint-disable MD029 -->
4. Build and run your app on the connected device by specifying the device identifier:

    ```dotnetcli
    dotnet build -t:Run -f net8.0-android -p:AndroidDeviceUsb=1A2B3C4D5E6F
    ```

    Replace `1A2B3C4D5E6F` with your device's actual identifier from the `adb devices` output.

5. If you have only one device or emulator connected, you can omit the `AndroidDeviceUsb` property:

    ```dotnetcli
    dotnet build -t:Run -f net8.0-android
    ```

<!-- markdownlint-enable MD029 -->

## Additional CLI options

You can customize the build and run process with additional MSBuild properties:

- **Debug vs Release**: Use the `-c` (configuration) parameter:

    ```dotnetcli
    dotnet build -t:Run -f net8.0-android -c Release
    ```

- **Specific architecture**: Use the `RuntimeIdentifier` property for device deployment:

    ```dotnetcli
    dotnet build -t:Run -f net8.0-android -p:RuntimeIdentifier=android-arm64
    ```

- **Clean and rebuild**: Use the `Rebuild` target:

    ```dotnetcli
    dotnet build -t:Rebuild -t:Run -f net8.0-android
    ```

For more information about Android deployment and debugging, see [Android deployment](~/android/deployment/index.md).
