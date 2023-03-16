---
title: "Publish a .NET MAUI Mac Catalyst app for distribution outside the App Store"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for distribution outside the App Store."
ms.date: 03/16/2023
---

# Publish a Mac Catalyst app for distribution outside the App Store

An alternative to distributing Mac Catalyst apps through the App Store is to distribute them outside the App Store. With this approach, your Mac Catalyst app can be hosted on a server for download.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the App Store, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. A distribution provisioning profile contains an App ID and a developer ID application certificate. You'll need to create a developer ID application certificate to identify yourself or your organization, if you don't already have one. In addition, you'll need to create developer ID installer certificate to sign your app's installer package.

The process for provisioning a Mac Catalyst app for distribution outside the App Store is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a developer ID application certificate. For more information, see [Create a developer ID application certificate](#create-a-developer-id-application-certificate).
1. Create a developer ID installer certificate. For more information, see [Create a developer ID installer certificate](#create-a-developer-id-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
<!-- 1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id). IS THIS REALLY REQUIRED???
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile). -->

After provisioning and publishing your app, it must be notarized SOMETHING ABOUT GATEKEEPER HERE.

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

## Create a developer ID application certificate

The CSR allows you to generate a developer ID certificate, which confirms your identity. The developer ID certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Developer ID Application** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/developer-id-application-certificate.png" alt-text="Create a Developer ID Application certificate.":::

1. On the **Create a New Certificate** page, select the **G2 Sub-CA** radio button, and then select **Choose File**:

    :::image type="content" source="media/publish-outside-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for a Developer ID Application certificate.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-outside-app-store/download-developer-id-application-certificate.png" alt-text="Download your Developer ID Application certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Developer ID Application**:

    :::image type="content" source="media/publish-outside-app-store/keychain-access-developer-id-application-certificate.png" alt-text="Keychain Access showing Developer ID Application certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a developer ID installer certificate

The CSR allows you to generate a developer ID installer certificate, which is required to sign your app's installer package. The installer certificate must be created using the Apple ID for your Apple Developer Account:

1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Developer ID Installer** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/developer-id-installer-certificate.png" alt-text="Create a Developer ID Installer certificate.":::

1. On the **Create a New Certificate** page, select the **G2 Sub-CA** radio button, and then select **Choose File**:

    :::image type="content" source="media/publish-outside-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for a Developer ID Installer certificate.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/chosen-certificate.png" alt-text="Continue to generate your installer certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-outside-app-store/download-installer-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Developer ID Installer**:

    :::image type="content" source="media/publish-outside-app-store/keychain-access-installer-certificate.png" alt-text="Keychain Access showing installer certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed for release, so that it can be installed on another Mac. A distribution provisioning profile contains an App ID and a developer ID application certificate.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

<!-- TODO: Does the App ID need configuring to use the Mac Catalyst entitlement?
[!INCLUDE [Configure the App ID](../includes/configure-app-id.md)] -->

### Create a provisioning profile

Once the App ID has been created and configured, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on Macs.

To create a provisioning profile for distribution outside the App Store:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **Developer ID** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/register-provisioning-profile.png" alt-text="Register a provisioning profile distribution outside the App Store.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/select-app-id.png" alt-text="Select your App ID.":::

    > [!NOTE]
    > The App ID will be in the **Enabled App IDs with an associated application identifier** section.

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish-outside-app-store/generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Xcode.

[!INCLUDE [Download certificates and provisioning profiles in Xcode](../includes/download-profiles.md)]
