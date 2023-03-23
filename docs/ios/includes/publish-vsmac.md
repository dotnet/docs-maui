---
ms.date: 02/24/2023
ms.topic: include
---

1. In the Visual Studio for Mac toolbar, use the **Build Configuration** drop-down to change from the debug configuration to the release configuration:

    :::image type="content" source="../deployment/media/publish/vsmac/release-configuration.png" alt-text="Select the release configuration in Visual Studio for Mac.":::

1. In the Visual Studio for Mac toolbar, use the **Debug Target** drop-down to select **iOS Build Only Device > Generic Device**:

    :::image type="content" source="../deployment/media/publish/vsmac/select-build-device.png" alt-text="Select your build device in Visual Studio for Mac.":::

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > iOS > Bundle Signing** tab and ensure that **Signing Identity** is set to **Distribution (Automatic)**, and **Provisioning profile** is set to **Automatic** or the distribution channel for your app (such as **App Store (Automatic)**):

    :::image type="content" source="../deployment/media/publish/vsmac/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio for Mac.":::

    These settings will ensure that Visual Studio for Mac will select the correct provisioning profile based on the bundle identifier in *Info.plist* (which is identical to the value of the **Application ID** property in your project file). Alternatively, set the **Signing identity** to the appropriate distribution certificate and **Provisioning profile** to the profile you created in your Apple Developer Account.

    Select the **OK** button to close the **Project Properties** window.

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Archive for Publishing**:

    :::image type="content" source="../deployment/media/publish/vsmac/archive-menu-item.png" alt-text="Select the archive menu item in Visual Studio for Mac.":::

    Visual Studio for Mac will begin to archive your app bundle, and progress can be monitored in the **Archive** window. The archiving process signs the app with the certificate and provisioning profiles that you specified in the **iOS Bundle Signing** tab, for the selected solution configuration. When the archiving process successfully completes, the **Archives** tab will be opened.
