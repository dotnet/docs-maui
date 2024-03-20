---
title: "Publish a .NET MAUI iOS app for App Store distribution"
description: "Learn how to provision and publish a .NET MAUI iOS app for App Store distribution."
ms.date: 02/24/2023
---

# Publish an iOS app for App Store distribution

> [!div class="op_single_selector"]
>
> - [Publish for in-house distribution](publish-in-house.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)
> - [Publish using the command line](publish-cli.md)

The most common approach to distributing iOS apps to users is through the App Store. Apps are submitted to the App Store through an online tool called *App Store Connect*. Only developers who belong to the Apple Developer Program have access to this tool. Members of the Apple Developer Enterprise Program do not have access. All apps submitted to the App Store require approval from Apple.

Distributing an iOS app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) iOS app, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. A distribution provisioning profile contains an App ID and a distribution certificate. You can use the same App ID that you used when deploying your app to a device for testing. However, you'll need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an App Store distribution provisioning profile is as follows:

1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

> [!IMPORTANT]
> Apple has a privacy policy for apps that target iOS on the App Store. It requires the app to include a privacy manifest file in the app bundle, that lists the types of data your .NET MAUI app, or any third-party SDKs and packages collect, and the reasons for using certain required reason APIs. If your use of the required reason APIs, or third-party SDKs, isn’t declared in the privacy manifest, your app might be rejected by the App Store. For more information, see [Apple privacy manifest](~/ios/privacy-manifest.md).

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release, so that it can be installed on an iOS device. A distribution provisioning profile contains an App ID and a distribution certificate.

[!INCLUDE [Create an App ID](../includes/app-id.md)]

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device.

To create a provisioning profile for App Store distribution:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **App Store** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish/appstore-register-provisioning-profile.png" alt-text="Register a provisioning profile for app store distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish/appstore-select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish/appstore-generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Visual Studio.

[!INCLUDE [Download provisioning profiles in Visual Studio](../includes/download-profiles.md)]

## Publish the app

Visual Studio can publish a .NET MAUI iOS app for App Store distribution and upload it to the App Store. However, before you can upload an app to the App Store you must:

- Create a record for the app you want to upload to the App Store in App Store Connect. This record includes all the information about the app as it will appear in the App Store and all of the information needed to manage the app through the distribution process. For more information, see [Create an app record](https://developer.apple.com/help/app-store-connect/create-an-app-record/add-a-new-app) on developer.apple.com.
- Create an app-specific password. For information about generating an app-specific password, see [Sign in to apps with your Apple ID using app-specific passwords](https://support.apple.com/HT204397) on support.apple.com.

For information about publishing an iOS app using the Command Line Interface (CLI), see [Publish an iOS app using the command line](publish-cli.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Publish](../includes/publish-vs.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **App Store** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-app-store.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-app-store.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

    > [!NOTE]
    > You should use the signing identity and provisioning profile that were created for your app and the selected distribution channel.

1. In the **Distribute - Signing Identity** dialog, select either the **Save As** button or the **Upload to Store** button. The **Save As** button will enable you to enter a filename, before re-signing your app and publishing it to an *.ipa* file on your file system for later upload to the App Store through an app such as [Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12). The **Upload to Store** button will re-sign your app and publish it to an *.ipa* file, before uploading it to the App Store.

    1. If you select the **Upload to Store** button, the **Upload to store credentials** dialog will appear after Visual Studio has created the *.ipa* file. In the **Upload to store credentials** dialog enter your Apple ID and app-specific password and select the **OK** button:

        :::image type="content" source="media/publish/vs/upload-to-app-store.png" alt-text="Screenshot of entering your app-specific password to upload the app to the App Store.":::

      Visual Studio will validate your app package and upload it to the App Store, provided you've created an application record in App Store Connect.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

[!INCLUDE [Publish](../includes/publish-vsmac.md)]

<!-- markdownlint-disable MD029 -->
5. In the **Archives** tab, ensure your archive is selected and then select the **Sign and Distribute...** button to begin the process of packaging your app for distribution:

    :::image type="content" source="../deployment/media/publish/vsmac/archive-tab-individual.png" alt-text="Screenshot of the archive tab in Visual Studio for Mac once archiving is complete.":::

    The **Sign and Distribute** window will appear.
    <!-- markdownlint-enable MD029 -->

1. In the **Sign and Distribute** window, select the **App Store** distribution channel, and then select the **Next** button:

    :::image type="content" source="media/publish/vsmac/distribution-select-channel-app-store.png" alt-text="Screenshot of selecting a distribution channel in the VSMac distribution dialog.":::

1. In the **Sign and Distribute** window, select either the **Upload** radio button or the **Export** radio button. The **Export** radio button will publish your app to an *.ipa* file on your file system for later upload to the App Store through an app such as [Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12). The **Upload** radio button will publish your app to an *.ipa* file, before uploading it to the App Store. Then select the **Next** button.

    :::image type="content" source="media/publish/vsMAC/distribution-select-destination-app-store.png" alt-text="Screenshot of selecting a distribution destination in the distribution dialog.":::

1. In the **Sign and Distribute** window, select the signing identity and provisioning profile for your app, and then select the **Next** button:

    :::image type="content" source="media/publish/vsmac/distribution-signing-identity-app-store.png" alt-text="Screenshot of selecting a signing identity in the VSMac distribution dialog.":::

    > [!NOTE]
    > You should use the signing identity and provisioning profile that were created for your app and the selected distribution channel.

1. In the **Sign and Distribute** window, if you chose to upload to the App Store, enter your Apple ID and app-specific password and select the **Next** button:

    :::image type="content" source="media/publish/vsmac/upload-to-app-store.png" alt-text="Screenshot of entering your app-specific password to upload the app to the App Store from VSMac.":::

1. In the **Sign and Distribute** window, select the **Publish** button:

    :::image type="content" source="media/publish/vsmac/distribution-publish-app-store.png" alt-text="Screenshot of publishing an iOS app using app store distribution.":::

1. In the **Output IPA file** dialog, choose a location and filename for your IPA file, and select the **Save** button:

    :::image type="content" source="media/publish/vsmac/distribution-save-ipa-ad-hoc.png" alt-text="Screenshot of saving an IPA file using ad hoc distribution.":::

    Your app will then be published to an *.ipa* file on your file system. If you chose to upload to the App Store, Visual Studio for Mac will validate your app package and upload it to the App Store, provided you've created an application record in App Store Connect. Visual Studio for Mac will notify you once publishing is complete:

    :::image type="content" source="media/publish/vsmac/distribution-publish-complete-app-store.png" alt-text="Screenshot of publishing being complete.":::

1. In the **Sign and Distribute** window, select the **Close** button.

---

## Troubleshoot

[Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12) can be used to help identify errors with app packages that stop successful submission to the App Store.
