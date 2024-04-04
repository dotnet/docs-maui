---
title: "Wireless deployment for .NET MAUI iOS apps"
description: "Learn how to wirelessly deploy your .NET MAUI iOS apps to a provisioned device that's paired to Xcode."
ms.date: 12/19/2022
---

# Wireless deployment for .NET MAUI iOS apps

Rather than having to use a USB cable to connect an iOS device to your Mac to deploy and debug a .NET Multi-platform App UI (.NET MAUI) app, Visual Studio and Visual Studio for Mac can deploy .NET MAUI iOS apps to devices wirelessly, and debug them wirelessly. To do this you must pair your iOS device with Xcode on your Mac. Once paired, the device can be selected from the device target list in Visual Studio and Visual Studio for Mac.

> [!IMPORTANT]
> .NET MAUI iOS apps must have been provisioned before they can be deployed to a device for testing and debugging. For more information, see [Device provisioning for iOS](~/ios/device-provisioning/index.md).

## Pair an iOS device

Perform the following steps to pair an iOS device to Xcode on your Mac:

1. Ensure that your iOS device is connected to the same wireless network as your Mac.
1. Plug your iOS device into your Mac using a USB cable.

    > [!NOTE]
    > The first time you connect an iOS device to your Mac, you'll need to tap the **Trust** button in the **Trust This Computer** dialog on your device.

1. Open **Xcode** and click **Window > Devices and Simulators**. In the window that appears, click **Devices**.
1. In the **Devices and Simulators** window, in the left column, select your device. Then in the detail area select the **Connect via network** checkbox:

    :::image type="content" source="media/wireless-deployment/xcode.png" alt-text="Screenshot of Devices and Simulators window showing Connect via network option.":::

    Xcode pairs with the iOS device.

1. In the **Devices and Simulators** window, in the left column, a network icon will appear next to a connected device that's paired:

    :::image type="content" source="media/wireless-deployment/xcode-paired.png" alt-text="Screenshot of Devices and Simulators window showing a paired iOS device.":::

1. Disconnect the USB cable and check that the device remains paired.

Xcode will retain the pairing settings, so the device shouldn't need to be paired again.

## Unpair an iOS device

Perform the following steps to unpair an iOS device from Xcode on your Mac:

1. Ensure that your iOS device is connected to the same wireless network as your Mac.
1. Open **Xcode** and click **Window > Devices and Simulators**. In the window that appears, click **Devices**.
1. In the **Devices and Simulators** window, in the left column, select your paired device. Then right-click the device and select the **Unpair Device** item.

    :::image type="content" source="media/wireless-deployment/xcode-unpair.png" alt-text="Screenshot of Devices and Simulators window showing a paired iOS device about to be unpaired.":::

## Deploy to device

After wirelessly pairing your device to Xcode, provisioned .NET MAUI iOS apps can be wirelessly deployed to the device.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. Ensure that your iOS device is wirelessly paired to your Mac build host. For more information, see [Pair an iOS device](#pair-an-ios-device).
1. In Visual Studio, ensure that the IDE is paired to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).
1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Remote Devices** and then the device that's connected to your Mac build host:

    :::image type="content" source="media/wireless-deployment/vs/select-remote-device.png" alt-text="Select your remote device in Visual Studio.":::

1. In the Visual Studio toolbar, press the green Start button to launch the app on your remote device:

    :::image type="content" source="media/wireless-deployment/vs/chosen-debug-target.png" alt-text="Visual Studio iOS device debug target choice.":::

> [!NOTE]
> An alternative approach to deploying a .NET MAUI iOS app to a device is to use hot restart. Hot restart enables you to deploy a .NET MAUI app to a 64-bit local iOS device, from Visual Studio, without requiring a Mac build host. For more information, see [Deploy an iOS app using hot restart](~/ios/hot-restart.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. Ensure that your iOS device is wirelessly paired to your Mac build host. For more information, see [Pair an iOS device](#pair-an-ios-device).
1. In the Visual Studio for Mac toolbar, ensure that the debug target is set to your connected iOS device:

    :::image type="content" source="media/wireless-deployment/vsmac/select-device.png" alt-text="Select your remote device in Visual Studio for Mac.":::

1. In the Visual Studio for Mac toolbar, press the **Play** button to launch the app on your device:

    :::image type="content" source="media/wireless-deployment/vsmac/chosen-debug-target.png" alt-text="Visual Studio for Mac iOS device debug target choice.":::

---

## Troubleshoot

- Ensure that your iOS device is connected to the same network as your Mac.
- Ensure that your device is provisioned. For more information about provisioning, see [Device provisioning for iOS](~/ios/device-provisioning/index.md).
- Verify that Xcode can see the device:
  - In Xcode, choose **Window > Devices and Simulators**, and in the window that appears click **Devices**. The device should appear under **Connected**.
- Ping the device:
  - Find the device's IP address. On the device open **Settings**, tap **Wi-Fi**, and then tap the information button next to the network that's active.
  - On a Mac, open **Terminal** and type `ping` followed by the device's IP address. Provided that your Mac can see the device, you'll receive output similar to:

    ```zsh
    PING 192.168.1.107 (192.168.1.107): 56 data bytes
    64 bytes from 192.168.1.107: icmp_seq=0 ttl=64 time=121.015 ms
    64 bytes from 192.168.1.107: icmp_seq=1 ttl=64 time=28.387 ms
    64 bytes from 192.168.1.107: icmp_seq=2 ttl=64 time=49.890 ms
    64 bytes from 192.168.1.107: icmp_seq=3 ttl=64 time=72.283 ms
    ```

    If there's an error, the output will be `Request timeout for icmp_seq 0`. If you can't ping the device, then the Internet Control Message Protocol (ICMP) is blocked or there's another connectivity issue.
- Ensure that port 62078 is open.
- Connect the device to the network using an Ethernet cable:
  - Use the Lightning to USB Camera Adapter and a USB to Ethernet adapter.
- Re-pair the iOS device:
  - Unpair the device. For more information, see [Unpair an iOS device](#unpair-an-ios-device).
  - Pair the iOS device with Xcode. For more information, see [Pair an iOS device](#pair-an-ios-device).
