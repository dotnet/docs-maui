---
title: "Publish a .NET MAUI iOS app for ad-hoc distribution"
description: "Learn how to provision and publish a .NET MAUI iOS app for ad-hoc distribution."
ms.date: 08/27/2024
---

# Publish an iOS app for ad-hoc distribution

> [!div class="op_single_selector"]
>
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish for in-house distribution](publish-in-house.md)
> - [Publish using the command line](publish-cli.md)

Ad-hoc distribution is primarily used for testing apps within a wide group of people, and is available for the Apple Developer Program and the Apple Developer Enterprise Program. Another use case for ad-hoc distribution is distribution within a company when App Store Connect isn't an option.

Ad-hoc distribution has the advantage of not requiring App Store approval, with apps being installed with [Apple Configurator](https://apps.apple.com/app/id1037126344). However, it's limited to 100 devices per membership year, for both development and distribution, and the devices must be added to your Apple Developer Account.

Distributing an iOS app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) iOS app, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. An ad-hoc distribution provisioning profile contains an App ID, a distribution certificate, and a list of the devices that can install the app. You can use the same App ID that you used when deploying your app to a device for testing. However, you'll need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an ad-hoc distribution provisioning profile is as follows:

1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Add devices to your Apple Developer Account. For more information, see [Add a device](#add-a-device).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

## Create a distribution profile

An ad-hoc distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release, so that it can be installed on specific iOS devices. An ad-hoc distribution provisioning profile contains an App ID, a distribution certificate, and a list of the devices that can install the app. The devices you wish to support must be running a version of iOS that's supported by Xcode.

[!INCLUDE [Add a device](../includes/add-a-device.md)]

[!INCLUDE [Create an App ID](../includes/app-id.md)]

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device.

To create a provisioning profile for ad-hoc distribution:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **Ad Hoc** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish/adhoc-register-provisioning-profile.png" alt-text="Register a provisioning profile for ad hoc distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop-down, select the App ID that you previously created, and choose whether to create an offline profile, before clicking the **Continue** button:

    :::image type="content" source="media/publish/select-app-id-ad-hoc.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish/adhoc-select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, select the devices that the app will be installed on and then click the **Continue** button.

    :::image type="content" source="media/publish/provisioning-profile-devices.png" alt-text="Screenshot of adding a device to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish/adhoc-generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Visual Studio.

[!INCLUDE [Download provisioning profiles in Visual Studio](../includes/download-profiles.md)]

## Publish the app

Visual Studio can publish a .NET MAUI iOS app for ad-hoc distribution:

[!INCLUDE [Publish](../includes/publish-vs.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **Ad Hoc** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-ad-hoc.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-ad-hoc.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

    > [!NOTE]
    > You should use the signing identity and provisioning profile that were created for your app and the selected distribution channel.

1. In the **Distribute - Signing Identity** dialog, select the **Save As** button and enter a filename. Your app will then be re-signed and published to an *.ipa* file on your file system.

The app can then be distributed using [Apple Configurator](https://apps.apple.com/app/id1037126344). For more information, see [Apple Configurator user guide](https://support.apple.com/guide/apple-configurator-mac/welcome/mac) on support.apple.com.

For information about publishing an iOS app using the Command Line Interface (CLI), see [Publish an iOS app using the command line](publish-cli.md).
