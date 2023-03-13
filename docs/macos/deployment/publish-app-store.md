---
title: "Publish a .NET MAUI Mac Catalyst app for App Store distribution"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for App Store distribution."
ms.date: 03/13/2023
---

# Publish a Mac Catalyst app for App Store distribution

The most common approach to distributing Mac Catalyst apps to users is through the App Store. Apps are submitted to the App Store through a portal called *App Store Connect*. Only developers who belong to the Apple Developer Program have access to this portal. Members of the Apple Developer Enterprise Program do not have access. All apps submitted to the App Store require approval from Apple.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. A distribution provisioning profile contains an App ID and a distribution certificate. You can use the same App ID that you used when deploying your app to a device for testing. However, you will need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an App Store distribution provisioning profile is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Create an installer certificate. For more information, see [Create an installer certificate](#create-an-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

[!INCLUDE [Create an installer certificate](../includes/installer-certificate.md)]

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed for release, so that it can be installed on another Mac A distribution provisioning profile contains an App ID and a distribution certificate.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

[!INCLUDE [Configure the App ID](../includes/configure-app-id.md)]

### Create a provisioning profile

Once the App ID has been created and configured, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on Macs.

To create a provisioning profile for App Store distribution:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **Mac App Store** radio button before clicking the **Continue** button:

    :::image type="content" source="media/deployment/appstore-register-provisioning-profile.png" alt-text="Register a provisioning profile for app store distribution.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/deployment/select-app-id.png" alt-text="Select your App ID.":::

    > [!NOTE]
    > The App ID will be in the **Enabled App IDs with an associated application identifier** section.

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/deployment/appstore-select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/deployment/appstore-generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Xcode.

[!INCLUDE [Download certificates and provisioning profiles in Xcode](../includes/download-profiles.md)]

## Add entitlements

## Add code signing data to your app project

## Publish
