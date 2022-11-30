1. In Visual Studio, pair the IDE to a Mac Build host if you haven't previously. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).
1. Ensure that your iOS device is connect to your Mac build host via USB or WiFi.
1. In the Visual Studio toolbar, use the **Debug Target** drop down to select **iOS Remote Devices** and then the device that's connected to your Mac build host:

    :::image type="content" source="~/ios/device-provisioning/media/automatic-provisioning/vs/select-remote-device.png" alt-text="Select your remote device in Visual Studio".":::

1. In the Visual Studio toolbar, press the green Start button to launch the app on your remote device:

    :::image type="content" source="~/ios/device-provisioning/media/automatic-provisioning/vs/chosen-debug-target.png" alt-text="Visual Studio iOS simulator debug target choice.":::

> [!NOTE]
> An alternative approach to deploying a .NET MAUI iOS app to a device is to use hot restart. Hot restart enables you to deploy a .NET MAUI app to a 64-bit local iOS device, from Visual Studio, without requiring a Mac build host. For more information, see [Deploy an iOS app using hot restart](~/deployment/hot-restart.md).
