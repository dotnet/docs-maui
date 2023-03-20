---
title: "Publish a .NET MAUI Mac Catalyst app for ad-hoc distribution"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for ad-hoc distribution to a limited number of registered devices."
ms.date: 03/20/2023
---

# Publish a Mac Catalyst app for ad-hoc distribution

When distributing Mac Catalyst apps outside the Mac App Store, you can also choose to distribute your app to a limited number of users on registered devices. This is known as *ad-hoc* distribution, and is primarily used for testing apps within a group of people. However, it's limited to 100 devices per membership year, and the devices must be added to your Apple Developer Account. It's available for the Apple Developer Program and the Apple Developer Enterprise Program.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the Mac App Store, you'll need to build a *development provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. An ad-hoc development provisioning profile contains an App ID, a development certificate, and a list of the devices that can install the app. You'll need to create a development certificate to identify yourself or your organization, if you don't already have one.

The process for provisioning a .NET MAUI Mac Catalyst app for ad-hoc distribution is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a developer certificate. For more information, see [Create a developer certificate](#create-a-developer-certificate).
<!-- 1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id). IS THIS REALLY REQUIRED???
1. Add devices to your Apple Developer Account. For more information, see [Add a device](#add-a-device).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).
1. Download your provisioning profile. For more information, see [Download your provisioning profile in Xcode](#download-your-provisioning-profile-in-xcode).

Then, once provisioning is complete you should prepare your app for publishing, and then publish it with the following process:

1. Optionally add entitlements to your app. For more information, see [Add entitlements](#add-entitlements).
1. Update the app's *Info.plist* file. For more information, see [Update Info.plist](#update-infoplist).
1. Publish your app using the command line. For more information, see [Publish using the command line](#publish-using-the-command-line). -->

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

## Create a developer certificate

The CSR allows you to generate a developer certificate, which confirms your identity. The developer certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Apple Development** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/apple-developer-certificate.png" alt-text="Create an Apple developer certificate.":::

1. On the **Create a New Certificate** page, select **Choose File**:

    :::image type="content" source="media/publish-ad-hoc/choose-certificate.png" alt-text="Upload your certificate signing request.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-ad-hoc/download-developer-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Apple Developer**:

    :::image type="content" source="media/publish-ad-hoc/keychain-access-developer-certificate.png" alt-text="Keychain Access showing developer certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a developer profile

An ad-hoc developer provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed, so that it can be installed on specific Macs. An ad-hoc developer provisioning profile contains an App ID, a developer certificate, and a list of the devices that can install the app.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

<!-- TODO: Does the App ID need configuring to use the Mac Catalyst entitlement? -->
[!INCLUDE [Configure the App ID](../includes/configure-app-id.md)]

### Add a device

When creating a provisioning profile, the profile must include which devices can run the app. Before selecting a device to be added to a provisioning profile you must first add the device to your Apple Developer Account. This can be achieved with the following steps:

1. Select the **Apple > About this Mac** menu item.
1. In the **Overview** tab, select the **System Report...** button.
1. In the **System Report**, select the **Hardware** expander to see the hardware overview. The report displays the universally unique identifier (UUID) as **Hardware UUID** in macOS 10.15 and later, or **Provisioning UDID** in macOS 11.0 and later.
1. Select the **Hardware UUID** or **Provisioning UDID** value and copy it to the clipboard.
1. In a web browser, go to the [Devices](https://developer.apple.com/account/resources/devices/list) section of your Apple Developer Account and click the **+** button.
1. In the **Register a New Device** page, set the **Platform** to **macOS** and provide a name for the new device. Then paste the identifier from the clipboard into the **Device ID (UUID)** field, and click the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/add-device.png" alt-text="Register a device by naming it and entering its unique device identifier.":::

1. In the **Register a New Device** page, review the information and then click the **Register** button.

Repeat the above steps for any Macs that you want to deploy a .NET MAUI Mac Catalyst app to.

### Create a provisioning profile

Once the App ID has been created and configured, you should create a developer provisioning profile. This profile enables the app to be digitally signed so that it can be installed on specific Macs.

To create a provisioning profile for ad-hoc distribution:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **macOS App Development** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/register-provisioning-profile.png" alt-text="Register a provisioning profile for ad-hoc distribution.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/select-app-id.png" alt-text="Select your App ID.":::

    > [!NOTE]
    > The App ID will be in the **Enabled App IDs with an associated application identifier** section.

1. In the **Generate a Provisioning Profile** page, select the check box that corresponds to your development certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/select-certificate.png" alt-text="Select your developer certificate.":::

1. In the **Generate a Provisioning Profile** page, select the devices that the app will be installed on and then click the **Continue** button.

    :::image type="content" source="media/publish-ad-hoc/provisioning-profile-devices.png" alt-text="Screenshot of adding a device to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish-ad-hoc/generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Xcode.

[!INCLUDE [Download certificates and provisioning profiles in Xcode](../includes/download-profiles.md)]

## See also

- [Distributing your app to registered devices](https://developer.apple.com/documentation/xcode/distributing-your-app-to-registered-devices) on developer.apple.com