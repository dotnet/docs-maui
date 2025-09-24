---
ms.topic: include
ms.date: 08/27/2024
---

## Deploy to device

After configuring provisioning in your .NET MAUI app project, the app can be deployed to a device with Visual Studio:

1. In Visual Studio, ensure that the IDE is paired to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).
1. Ensure that your iOS device is connected to your Mac build host via USB or WiFi. For more information about wireless deployment, see [Wireless deployment for .NET MAUI iOS apps](~/ios/wireless-deployment.md).
1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Remote Devices** and then the device that's connected to your Mac build host:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vs/select-remote-device.png" alt-text="Select your remote device in Visual Studio.":::

1. In the Visual Studio toolbar, press the green Start button to launch the app on your remote device:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vs/chosen-debug-target.png" alt-text="Visual Studio iOS device debug target choice.":::
