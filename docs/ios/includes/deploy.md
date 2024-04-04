---
ms.topic: include
ms.date: 11/30/2022
---

## Deploy to device

After configuring provisioning in your .NET MAUI app project, the app can be deployed to a device.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, ensure that the IDE is paired to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).
1. Ensure that your iOS device is connected to your Mac build host via USB or WiFi. For more information about wireless deployment, see [Wireless deployment for .NET MAUI iOS apps](~/ios/wireless-deployment.md).
1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Remote Devices** and then the device that's connected to your Mac build host:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vs/select-remote-device.png" alt-text="Select your remote device in Visual Studio.":::

1. In the Visual Studio toolbar, press the green Start button to launch the app on your remote device:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vs/chosen-debug-target.png" alt-text="Visual Studio iOS device debug target choice.":::

> [!NOTE]
> An alternative approach to deploying a .NET MAUI iOS app to a device is to use hot restart. Hot restart enables you to deploy a .NET MAUI app to a 64-bit local iOS device, from Visual Studio, without requiring a Mac build host. For more information, see [Deploy an iOS app using hot restart](~/ios/hot-restart.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In the **Solution Window**, double-click the **Info.plist** file from the **Platforms > iOS** folder of your .NET MAUI app project to open it in the editor.
1. In the **Info.plist** editor, change from the **Source** view to the **Application** view:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vsmac/source-view.png" alt-text="Screenshot of Info.plist editor source view in Visual Studio for Mac.":::

1. In the **Deployment Info** section of the **Application** view, set the deployment target to the minimum supported iOS version for your app, using the **Deployment target** drop-down:

    :::image type="content" source="../device-provisioning/media/automatic-provisioning/vsmac/deployment-target.png" alt-text="Screenshot of deployment target in Info.plist editor in Visual Studio for Mac.":::

    > [!IMPORTANT]
    > The deployment target value must be identical to the **Minimum iOS version** value that's defined in the **Project Properties** dialog under the **Build > Target Platforms** tab.

1. Ensure that your iOS device is connected to your Mac via USB or WiFi. For more information about wireless deployment, see [Wireless deployment for .NET MAUI iOS apps](~/ios/wireless-deployment.md).
1. In the Visual Studio for Mac toolbar, ensure that the debug target is set to your connected iOS device, and then press the **Play** button to launch the app on your device.

---
