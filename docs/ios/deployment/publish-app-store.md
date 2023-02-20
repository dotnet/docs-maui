---
title: "App store publish"
description: "Learn how to publish an iOS .NET MAUI app to the App Store."
ms.date: 02/20/2023
---

# Publish an app store app

When you try to publish the archive, you'll need to specify a certificate and provisioning profile again to resign the app before publishing it, and the type of those should match the selected distribution channel.

You must have created a record for the app you want to upload to the App Store, in App Store Connect. This record includes all the information about the app as it will appear in the App Store and all of the information needed to manage the app through the distribution process. For more information, see [Create an app record](https://developer.apple.com/help/app-store-connect/create-an-app-record/add-a-new-app) on developer.apple.com.

In addition, when uploading your app to the app store, you'll need to create an app-specific password. For information about generating an app-specific password, see [Sign in to apps with your Apple ID using app-specific passwords](https://support.apple.com/HT204397) on support.apple.com.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Publish](../includes/publish.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **App Store** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-app-store.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-app-store.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

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

[Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12) can be used to help identify errors with app packages that stop successful submission to the App Store.
