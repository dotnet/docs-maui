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

5. In a terminal, change directory to *MyMauiApp*, and build and run the app:

    ```dotnetcli
    cd MyMauiApp
    dotnet build -f net10.0-android
    dotnet run -f net10.0-android
    ```

    > [!NOTE]
    > The `dotnet run` command is supported for .NET for Android projects starting in .NET 10. For .NET 9 and earlier versions, use `dotnet build -t:Run -f net9.0-android` instead.

    The `dotnet run` command will restore the project dependencies, build the app, and launch it on an available Android emulator or connected device.

6. In the emulator or device, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    > [!NOTE]
    > If no Android emulator is running and no device is connected, the run will fail. Make sure you have an Android emulator running or a device connected via USB debugging before running the command.

## Launch the app on a specific emulator

A .NET MAUI Android app can be launched on a specific Android emulator by using the `AdbTarget` MSBuild property. This is useful when you have multiple emulators running simultaneously:

1. First, list running emulators with their identifiers:

    ```console
    adb devices
    ```

    This command shows running emulators and connected devices:

    ```console
    List of devices attached
    emulator-5554   device
    emulator-5556   device
    ```

2. To run your app on a specific emulator, use the `AdbTarget` property with the `-s` flag and the emulator identifier:

    ```dotnetcli
    dotnet run -f net10.0-android -p:AdbTarget="-s emulator-5554"
    ```

    Replace `emulator-5554` with the actual identifier of your target emulator from the `adb devices` output.

3. Alternatively, you can use the `-e` flag to run on the only running emulator:

    ```dotnetcli
    dotnet run -f net10.0-android -p:AdbTarget=-e
    ```

4. In your chosen emulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

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

4. Run your app on the connected device by using the `AdbTarget` property with the `-d` flag:

    ```dotnetcli
    dotnet run -f net10.0-android -p:AdbTarget=-d
    ```

    This targets the only attached physical device.

5. Alternatively, if you have multiple devices or emulators connected, specify the device identifier with the `-s` flag:

    ```dotnetcli
    dotnet run -f net10.0-android -p:AdbTarget="-s 1A2B3C4D5E6F"
    ```

    Replace `1A2B3C4D5E6F` with your device's actual identifier from the `adb devices` output.

## Additional CLI options

You can customize the build and run process with additional MSBuild properties:

- **Debug vs Release**: Use the `-c` (configuration) parameter:

    ```dotnetcli
    dotnet run -f net10.0-android -c Release
    ```

- **Specific architecture**: Use the `RuntimeIdentifier` property for device deployment:

    ```dotnetcli
    dotnet run -f net10.0-android -p:RuntimeIdentifier=android-arm64
    ```

- **Combine with AdbTarget**: You can specify both the target device and other options:

    ```dotnetcli
    dotnet run -f net10.0-android -c Release -p:AdbTarget=-d
    ```

For more information about Android deployment and debugging, see [Android deployment](~/android/deployment/index.md).

> [!NOTE]
> The `dotnet run` command is supported for .NET for Android projects starting in .NET 10. For .NET 9 and earlier versions, use `dotnet build -t:Run` with the appropriate target framework (for example, `dotnet build -t:Run -f net9.0-android`).

> [!NOTE]
> The `$(AdbTarget)` property is passed to `adb`. For more information about adb command-line options, see [Issue shell commands](https://developer.android.com/tools/adb#shellcommands) on developer.android.com.
