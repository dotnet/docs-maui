---
title: "Set up a device for development"
description: "This article discusses how to enable development mode on an Android device so that you can deploy and debug a .NET MAUI application."
ms.date: 02/13/2025
ms.custom: sfi-image-nochange
---

# Set up Android device for debugging

While the [Android emulator](../emulator/debug-on-emulator.md) is a great way to rapidly develop and test your app, you'll want to test your apps on a real Android device. To run on a device, you'll need to enable developer mode on the device and connect it to your computer. For more information, see [Run apps on a hardware device](https://developer.android.com/studio/run/device) on developer.android.com.

> [!IMPORTANT]
> The steps in this article are written generically, to work on as many devices as possible. If you can't find these settings on your device, consult your device manufacturer's documentation.

## Enable developer mode on the device

A device must enable Developer mode in order to deploy and test an Android app. Developer mode is enabled by following these steps:

01. Go to the **Settings** screen.
01. Select **About phone**.
01. Select **Software information**.
01. Tap **Build Number** seven times until **You are now a developer!** is visible.

Depending on the UI your device is running, the **About phone** option may be in a different location. Consult your device documentation if you can't find **About phone**.

:::image type="content" source="media/setup/build-version-sml.png" alt-text="Developer options screen on Android." lightbox="media/setup/build-version.png":::

## Enable USB debugging

After enabling developer mode on your device, enable USB debugging by following these steps:

01. Go to the **Settings** screen.
01. Select **Developer options**.
01. Turn on the **USB debugging** option.

Depending on the UI your device is running, the **USB debugging** option may be in a different location. Consult your device documentation if you can't find **USB debugging**.

## Connect the device to the computer

The final step is to connect the device to the computer. The easiest and most reliable way is to do so over USB.

You'll receive a prompt to trust the computer on your device if you haven't used it for debugging before. You can also check **Always allow from this computer** to prevent requiring this prompt each time you connect the device.

:::image type="content" source="media/setup/trust-computer-for-usb-debugging.png" alt-text="Android trust prompt from computer to use USB debugging.":::

If your computer isn't recognizing the device when it's plugged in, try installing a driver for the device. Consult your device manufacturer's support documentation. You can also try installing the Google USB Driver through the Android SDK Manager:

:::image type="content" source="media/setup/google-usb-driver.png" alt-text="Android SKD Manager with the Google USB device driver selected.":::

## Enable WiFi debugging

It's possible to debug an android device over WiFi, without keeping the device physically connected to the computer. This technique requires more effort, but could be useful when the device is too far from the computer to remain constantly plugged-in via a cable.

### Connecting over WiFi

By default, the Android Debug Bridge (adb) is configured to communicate with an Android device via USB. It's possible to reconfigure it to use TCP/IP instead of USB. To do this, both the device and the computer must be on the same WiFi network.

> [!NOTE]
> The Google USB driver is required to perform `adb` debugging on Windows with Google devices. For more information, see [Get the Google USB Driver](https://developer.android.com/studio/run/win-usb) on developer.android.com. Windows drivers for all other devices are provided by the respective hardware manufacturer. For more information, see [Install OEM USB drivers](https://developer.android.com/studio/run/oem-usb) on developer.android.com.

First, enable Wireless debugging on your Android device:

01. Follow the steps in the [Enable developer mode on the device](#enable-developer-mode-on-the-device) section.
01. Follow the steps in the [Enable USB debugging](#enable-usb-debugging) section.
01. Go to the **Settings** screen.
01. Select **Developer options**.
01. Turn on the **Wireless debugging** option.

Depending on the UI your device is running, the **Wireless debugging** option may be in a different location. Consult your device documentation if you can't find **Wireless debugging**.

Next, use adb to connect to your device, first through a USB connection:

01. Determine the IP address of your Android device. One way to find out the IP address is to look under **Settings > Network & internet > Wi-Fi**, then tap on the WiFi network that the device is connected to, and then tap on **Advanced**. This will open a drop-down showing information about the network connection, similar to what is seen in the screenshot below:

    :::image type="content" source="media/setup/ip-settings-sml.png" alt-text="Android status screen with IP address." lightbox="media/setup/ip-settings.png":::

    On some versions of Android the IP address won't be listed there but can be found instead under **Settings > About phone > Status**.

01. In Visual Studio, open the adb command prompt by selecting the menu option: **Tools** > **Android** > **Android Adb Command Prompt...**.

01. In the command prompt, use the `adb tcpip` command to tell the device to listen to TCP/IP connections on port 5555.

    ```command
    adb tcpip 5555
    ```

01. Disconnect the USB cable from your device.

01. Connect to the device's IP address with port 5555:

    ```command
    adb connect 192.168.1.28:5555
    ```

    When this command finishes, the Android device is connected to the computer via WiFi.

    When you're finished debugging via WiFi, you can reset ADB back to USB mode with the following command:

    ```command
    adb usb
    ```

    To see the devices connected to the computer, use the `adb devices` command:

    ```command
    adb devices
    ```

### Connecting over WiFi without USB cable

> [!WARNING]
> This method is only available for devices running Android 11 (API level 30) and higher.

01. Follow the steps in the [Enable developer mode on the device](#enable-developer-mode-on-the-device) section.
01. Follow the steps in the [Enable USB debugging](#enable-usb-debugging) section.
01. Go to the **Settings** screen.
01. Select **Developer options**.
01. Turn on the **Wireless debugging** option.
01. Click on the area next to the switch to enter Wireless debugging settings.
   
:::image type="content" source="media/setup/wireless-debugging-button.png" alt-text="Button leading to Wireless debugging settings.":::

01. Select **Pair device with pairing code**. It will generate pairing code and show IP adress with appropriate port, it should look like on screenshot below:
   
:::image type="content" source="media/setup/pairing-via-code.png" alt-text="Pairing via pairing code.":::

01. Open your **terminal with access to adb**. One of the ways to do that is to go to Visual Studio and choose **Tools > Android > Android Adb Command Prompt**.
01. Pair your device using your IP adress and port.

    ```command
    adb pair 192.168.1.5:42219
    ```

01. When prompted, **enter the pairing code**.
   
After successfully pairing your device should be available as a debug target in Visual Studio. For more details on selecting debug target see [Debug an app on an Android device](#debug-an-app-on-an-android-device).

## Configure on-device developer options

The Settings app on Android includes a screen called **Developer options** where you can configure system behaviors that help you profile and debug your app performance. For more information, see [Configure on-device developer options](https://developer.android.com/studio/debug/dev-options) on developer.android.com.

## Debug an app on an Android device

To debug an app on an Android device:

1. Plug the device into your machine and select it as a debug target in Visual Studio or Visual Studio Code.
1. Run the app.

For information about selecting a debug target in Visual Studio, see [Build your first app](~/get-started/first-app.md?pivots=devices-android&tabs=vswin). For information about selecting a debug target in Visual Studio Code, see [Build your first app](~/get-started/first-app.md?pivots=devices-android&tabs=visual-studio-code).
