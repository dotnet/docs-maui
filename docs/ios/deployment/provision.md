---
title: "Provision a .NET MAUI iOS app for app store distribution"
description: "Learn how to provision a .NET MAUI iOS app for app store distribution."
ms.date: 05/09/2022
---

# Provision an iOS app for app store distribution

Distributing a .NET Multi-platform App UI (.NET MAUI) app on iOS requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To publish a .NET MAUI iOS app, you'll need to build a *Distribution Provisioning Profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. A distribution provisioning profile contains an app ID and a distribution certificate.

The process for creating a distribution provisioning profile is as follows:

1. Create a certificate signing request.
1. Create a distribution certificate.
1. Create an App ID.
1. Create a provisioning profile.

## Create a certificate signing request

To create a distribution certificate, you'll first need to create a certificate signing request (CSR) in Keychain Access on a Mac:

1. On your Mac, launch Keychain Access.
1. In Keychain Access, select the **Keychain Access > Certificate Assistant > Request a Certificate from a Certificate Authority...** menu item.
1. In the **Certificate Assistant** dialog, enter an email address in the **User Email Address** field.
1. In the **Certificate Assistant** dialog, enter a name for the key in the **Common Name** field.
1. In the **Certificate Assistant** dialog, leave the **CA Email Address** field empty.
1. In the **Certificate Assistant** dialog, choose the **Saved to disk** radio button and click **Continue**:

    :::image type="content" source="media/provisioning/certificate-assistant.png" alt-text="Certificate assistant dialog.":::

1. Save the certificate signing request to a known location.
1. Close Keychain Access.

## Create a distribution certificate

The CSR allows you to generate a distribution certificate, which confirms your identity. The distribution certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, click the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **iOS Distribution (App Store and Ad Hoc)** radio button before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/ios-app-development.png" alt-text="Create a new certificate.":::

1. On the **Create a New Certificate** page, click **Choose File**:

    :::image type="content" source="media/provisioning/choose-certificate.png" alt-text="Upload your certificate signing request.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file (a file with a `.certSigningRequest` file extension) and then click **Upload**.
1. On the **Create a New Certificate** page, click the **Continue** button:

    :::image type="content" source="media/provisioning/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, click the **Download** button:

    :::image type="content" source="media/provisioning/download-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **iPhone Distribution**:

    :::image type="content" source="media/provisioning/keychain-access.png" alt-text="Keychain Access showing distribution certificate.":::

    > [!NOTE]
    > Make a note of the full distribution certificate name in Keychain Access. It will be required when signing your app.

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release, so that it can be installed on an iOS device. A distribution provisioning profile contains an app ID and a distribution certificate.

### Create an App ID

An App ID is required to identify the app that you are distributing:

1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **Identifiers** tab.
1. On the **Identifiers** page, click the **+** button to create a new App ID.
1. On the **Register a new identifier** page, select the **App IDs** radio button before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/create-app-id.png" alt-text="Create an App ID.":::

1. On the **Register a new identifier** page, select **App** before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/register-identifier.png" alt-text="Register an App ID.":::

1. On the **Register an App ID** page, enter a description, and select either the **Explicit** or **Wildcard** Bundle ID radio button. Then, enter the Bundle ID for your app in reverse DS format:

    :::image type="content" source="media/provisioning/specify-bundle-id.png" alt-text="Specify the bundle identifier for the app.":::

    > [!IMPORTANT]
    > The Bundle ID you enter must correspond to the Bundle ID defined in the **Info.plist** file in your app project.

1. On the **Register an App ID** page, select any capabilities that the app uses. Any capabilities must be configured both on this page and in the **Entitlements.plist** file in your app project.
1. On the **Register an App ID** page, click the **Continue** button.
1. On the **Confirm your App ID** page, click the **Register** button.

### Create a provisioning profile

Once you've created a distribution certificate and an App ID you'll be able to create a provisioning profile:

1. In your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **App Store** radio button before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/register-provisioning-profile.png" alt-text="Register a provisioning profile for app store distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/provisioning/generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this later from Xcode.
