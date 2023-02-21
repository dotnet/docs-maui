---
ms.date: 02/21/2023
ms.topic: include
---

1. In the Visual Studio for Mac toolbar, use the **Build Configuration** drop-down to change from the debug configuration to the release configuration:

    :::image type="content" source="../deployment/media/publish/vsmac/release-configuration.png" alt-text="Select the release configuration in Visual Studio for Mac.":::

1. In the Visual Studio for Mac toolbar, use the **Debug Target** drop-down to select **iOS Build Only Device > Generic Device**:

    :::image type="content" source="../deployment/media/publish/vsmac/select-build-device.png" alt-text="Select your build device in Visual Studio for Mac.":::

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > iOS > Bundle Signing** tab and ensure that **Signing Identity** is set to **Distribution (Automatic)**, and **Provisioning profile** is set to **Automatic** or the distribution channel for your app (such as **App Store (Automatic)**):

    :::image type="content" source="../deployment/media/publish/vsmac/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio for Mac.":::

    These settings will ensure that Visual Studio for Mac will select the correct provisioning profile based on the bundle identifier in *Info.plist* (which is identical to the value of the **Application ID** property in your .csproj file). Alternatively, set the **Signing identity** to the appropriate distribution certificate and **Provisioning profile** to the profile you created in the Apple developer portal.

    Select the **OK** button to close the **Project Properties** window.

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Archive for Publishing**:

    :::image type="content" source="../deployment/media/publish/vsmac/archive-menu-item.png" alt-text="Select the archive menu item in Visual Studio for Mac.":::

    Visual Studio for Mac will begin to archive your app bundle. The archiving process signs the app with the certificate and provisioning profiles that you specified in the **iOS Bundle Signing** tab, for the selected solution configuration. When the archiving process successfully completes, the **Archive** window will be opened.

1. In the **Archive** window, ensure your archive is selected and then select the **Sign and Distribute...** button to begin the process of packaging your app for distribution:

    :::image type="content" source="../deployment/media/publish/vsmac/archive-window.png" alt-text="Screenshot of the archive window in Visual Studio once archiving is complete.":::

    The **Sign and Distribute** window will appear.
