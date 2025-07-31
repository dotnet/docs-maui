---
title: "Automatic provisioning for .NET MAUI iOS apps"
description: "Learn how to use automatic provisioning to request development certificates and profiles for .NET MAUI iOS apps."
ms.date: 08/28/2024
ms.custom: sfi-image-nochange
---

# Automatic provisioning for iOS apps

Automatic provisioning is the recommended approach for deploying a .NET Multi-platform App UI (.NET MAUI) iOS app to a device. With this approach, Visual Studio automatically creates and manages signing identifies, App IDs, and provisioning profiles. Before starting the automatic provisioning process, you should ensure that you've added your Apple Developer Account to Visual Studio. For more information, see [Apple account management](~/ios/apple-account-management.md).

Once you've added your Apple Developer Account to Visual Studio, you can use any associated team. Certificates, App IDs, and profiles can then be created against the team. The team ID is also used to create a prefix for the App ID that will be included in the provisioning profile, which enables Apple to verify that an app can be deployed to a device.

> [!IMPORTANT]
> Before you begin, ensure that you've accepted any user license agreements in your [Apple Developer Account](https://developer.apple.com/account/) and [App Store Connect](https://appstoreconnect.apple.com/).

## Enable automatic provisioning

Once you've added your Apple Developer Account to Visual Studio, you need to enable automatic provisioning for the .NET MAUI app project:

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **iOS > Bundle Signing** tab and ensure that **Automatic Provisioning** is selected in the **Scheme** drop-down:

    :::image type="content" source="media/automatic-provisioning/vs/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio.":::

1. In the **Bundle Signing** settings, click the **Configure Automatic Provisioning** hyperlink.

1. In the **Configure Automatic Provisioning** dialog, select your team. Visual Studio will attempt to automatically provision your project and will indicate if the process completed successfully:

    :::image type="content" source="media/automatic-provisioning/vs/automatic-provisioning-configured.png" alt-text="Screenshot of the automatic provisioning dialog when it's correctly configured.":::

    If the automatic provisioning fails the **Configure Automatic Provisioning** dialog will display the reason for the error.

1. In the **Configure Automatic Provisioning** dialog, click the **Ok** button to dismiss the dialog.

[!INCLUDE [Deploy the app to your device](~/ios/includes/deploy.md)]

## Run automatic provisioning

When automatic provisioning is enabled, Visual Studio will re-run the automatic provisioning process if necessary when any of the following occur:

- An iOS device is plugged into your Mac. This automatically checks to see if the device is registered in your Apple Developer Account. If it isn't, it will add it and generate a new provisioning profile that contains it.
- The Bundle ID of your app is changed. This updates the App ID and so a new provisioning profile containing this App ID is created.
- A supported capability is enabled in the *Entitlements.plist* file. This capability will be added to the App ID and a new provisioning profile with the updated App ID is generated. Not all capabilities are currently supported. For more information about capabilities, see [Capabilities](~/ios/capabilities.md).

## Wildcard App IDs

By default automatic provisioning will attempt to create and use a wildcard App ID and provisioning profile instead of an explicit App ID based on the app's bundle identifier. Wildcard App IDs reduce the number of profiles and IDs to maintain in your Apple Developer Account. For more information about wildcard App IDs, see [Create a development provisioning profile](manual-provisioning.md#create-a-development-provisioning-profile).

In some cases, an app's entitlements require an explicit App ID. The following entitlements do not support wildcard App IDs:

- App Groups
- Associated Domains
- Apple Pay
- Game Center
- HealthKit
- HomeKit
- Hotspot
- In-App purchase
- Multipath
- NFC
- Personal VPN
- Push Notifications
- Wireless Accessory Configuration

If your app uses one of these entitlements, Visual Studio will attempt to create an explicit App ID.

## Troubleshoot

It may take several hours for a new Apple Developer Account to be approved. You won't be able to enable automatic provisioning until the account has been approved.

If the automatic provisioning process fails with the error message `Authentication Service Is Unavailable`, sign in to either [App Store Connect](https://appstoreconnect.apple.com/) or your [Apple Developer account](https://appleid.apple.com/account) to check that you have accepted the latest service agreements.

### Certificate can't be found in local keychain

If you use multiple machines for development, you may receive the following error message when attempting to configure automatic provisioning on a machine: "There was an error while trying to automatically provision the project: 'Certificate: Apple Development: Create via API (Key ID)' already exists but cannot be found in the local Keychain. It may have been created on another development machine. Import the certificate and private key from that machine or revoke it and try again to automatically create a new one.'":

:::image type="content" source="media/automatic-provisioning/vs/automatic-provisioning-failure.png" alt-text="Screenshot of automatic provisioning failure when the certificate can't be found.":::

This can occur because automatic provisioning doesn't remove the need to manually copy certificates between machines, when you're using multiple machines for development. This is because the private key that creates a certificate only exists on the machine that created the certificate.

To discover if a required certificate is missing from your development machine, in Visual Studio go to **Tools > Options > Xamarin > Apple Accounts**. Then, in the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button:

:::image type="content" source="media/automatic-provisioning/vs/certificate-missing.png" alt-text="Screenshot of Details window when the certificate isn't in the keychain.":::

If the required certificate isn't installed on the machine, the **Details** window will show a "Not in Keychain" status for the certificate. In this scenario, the specific certificate must be exported from the machine that created it, in .p12 format, and then imported into Visual Studio with the **Import Certificate** button.

<!-- markdownlint-disable MD032 -->
> [!NOTE]
> - To copy a certificate from a Mac to another Mac, export the certificate from Keychain Access on the Mac that created the certificate and then import it into Keychain Access on the other Mac.
> - To copy a certificate from a Mac to a Windows machine, export the certificate from Keychain Access on the Mac and then on the Windows machine import it into Visual Studio with the **Import Certificate** button.
> - It's not possible to copy a certificate that was created by Visual Studio on a Windows machine, to another machine, because it's password protected.
<!-- markdownlint-enable MD032 -->

After the certificate has been imported, Visual Studio will show its status as "Valid":

:::image type="content" source="media/automatic-provisioning/vs/certificate-added.png" alt-text="Screenshot of Details window when the certificate has been added to the keychain.":::

It should then be possible for Visual Studio to automatically provision your project.
