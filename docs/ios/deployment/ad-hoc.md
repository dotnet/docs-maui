---
title: "Provision a .NET MAUI iOS app for ad-hoc distribution"
description: "Learn how to provision a .NET MAUI iOS app for ad-hoc distribution."
ms.date: 01/17/2023
---

# Provision an iOS app for ad-hoc distribution

Ad-hoc distribution is primarily used for testing apps within a wide group of people, and is available for the Apple Developer Program and the Apple Developer Enterprise Program. Another use case for ad-hoc distribution is distribution within a company when iTunes Connect isn't an option.

Ad-hoc distribution has the advantage of not requiring App Store approval, with apps being installed over-the-air from a web server, or via iTunes. However, it's limited to 100 devices per membership year, for both development and distribution, and the devices must be added to Apple's developer portal.

Distributing an iOS app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) iOS app, you'll need to build a *Distribution Provisioning Profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. A ad-hoc distribution provisioning profile contains an App ID, a distribution certificate, and a list of the devices that can install the app. You can use the same App ID that you used when deploying your app to a device for testing. However, you will need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an ad-hoc distribution provisioning profile is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Add devices.
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

## Create a distribution profile

An ad-hoc distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release, so that it can be installed on specific iOS devices. An ad-hoc distribution provisioning profile contains an App ID, a distribution certificate, and a list of the devices that can install the app. The devices you wish to support must be running a version of iOS that's supported by Xcode.

[!INCLUDE [Add a device](../includes/add-a-device.md)]

[!INCLUDE [Create an App ID](../includes/app-id.md)]

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device.

To create a distribution provisioning profile:

1. In your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **App Store** radio button before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/adhoc-register-provisioning-profile.png" alt-text="Register a provisioning profile for app store distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/adhoc-select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, select the devices that the app will be installed on and then click the **Continue** button.

    :::image type="content" source="media/provisioning/provisioning-profile-devices.png" alt-text="Screenshot of adding a device to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/provisioning/adhoc-generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Visual Studio.

[!INCLUDE [Download provisioning profiles in Visual Studio](../includes/download-profiles.md)]

<!-- TODO: Next button to the doc where they actually use the distribution provisioning profile to generate the app package. -->
