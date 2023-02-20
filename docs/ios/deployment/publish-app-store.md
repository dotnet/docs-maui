---
title: "App store publish"
description: "Learn how to publish an iOS .NET MAUI app to the App Store."
ms.date: 01/24/2023
---

# Publish an app store app

When you try to publish the archive, you'll need to specify a certificate and provisioning profile again to resign the app before publishing it, and the type of those should match the selected distribution channel.

You must have created a record for the app you want to upload to the App Store, in App Store Connect. This record includes all the information about the app as it will appear in the App Store and all of the information needed to manage the app through the distribution process. For more information, see [Create an app record](https://developer.apple.com/help/app-store-connect/create-an-app-record/add-a-new-app) on developer.apple.com.

In addition, when uploading your app to the app store, you'll need to create an app-specific password. For information about generating an app-specific password, see [Sign in to apps with your Apple ID using app-specific passwords](https://support.apple.com/HT204397) on support.apple.com.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, ensure that the IDE is paired to a Mac Build host. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **iOS Remote Devices** and then the device that's connected to your Mac build host via a USB cable or WiFi:

    :::image type="content" source="media/publish/vs/select-remote-device.png" alt-text="Select your remote device in Visual Studio.":::

    For more information about connecting to a device via WiFi, see [Wireless deployment for .NET MAUI iOS apps](../ios/wireless-deployment.md).

1. In the Visual Studio toolbar, use the **Solutions Configuration** drop-down to change from the debug configuration to the release configuration:

    :::image type="content" source="media/publish/vs/release-configuration.png" alt-text="Select the release configuration in Visual Studio.":::

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **iOS Bundle Signing** tab and ensure that **Scheme** is set to **Manual Provisioning**, **Signing identity** is set to **Distribution (Automatic)**, and **Provisioning profile** is set to **Automatic**:

    :::image type="content" source="media/publish/vs/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio.":::

    These settings will ensure that Visual Studio will select the correct provisioning profile based on the bundle identifier in *Info.plist* (which is identical to the value of the **Application ID** property in your .csproj file). Alternatively, set the **Signing identity** to the appropriate distribution certificate and **Provisioning profile** to the profile you created in the Apple developer portal.

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Publish...**:

    :::image type="content" source="media/publish/vs/publish-menu-item.png" alt-text="Select the publish menu item in Visual Studio.":::

    The **Archive Manager** will open and Visual Studio will begin to archive your app bundle:

    :::image type="content" source="media/publish/vs/archive-manager.png" alt-text="Screenshot of the archive manager in Visual Studio.":::

    The archiving process signs the app with the certificate and provisioning profiles that you specified in the **iOS Bundle Signing** tab, for the selected solution configuration.

1. In the **Archive Manager**, once archiving has successfully completed, click the **Distribute ...** button to begin the process of signing and packaging your app for distribution:

    :::image type="content" source="media/publish/vs/archive-manager-distribute.png" alt-text="Screenshot of the archive manager in Visual Studio once archiving is complete.":::

    The **Distribute - Select Channel** dialog will appear.

1. In the **Distribute - Select Channel** dialog, select the **App Store** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

1. In the **Distribute - Signing Identity** dialog, select either the **Save As** button or the **Upload to Store** button. The **Save As** button will publish your app to an *.ipa* file on your file system for later upload to the App Store through an app such as [Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12). The **Upload to Store** button will publish your app to an *.ipa* file and then upload it to the App Store.

    1. If you select the **Upload to Store** button, the **Upload to store credentials** dialog will appear after Visual Studio has created the *.ipa* file. In the **Upload to store credentials** dialog enter your Apple ID and app-specific password and select the **OK** button:

        :::image type="content" source="media/publish/vs/upload-to-app-store.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

      Visual Studio will validate your app package and upload it to the App Store, provided you've created an application record in App Store Connect.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE.

---

## Troubleshoot

[Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12) can be used to help identify errors with app packages that stop the successful submission of them to the App Store.
