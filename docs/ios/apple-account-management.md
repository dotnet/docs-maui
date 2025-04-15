---
title: "Apple account management"
description: "Learn how to use Apple account management features in Visual Studio."
ms.date: 11/08/2024
---

# Apple account management

When developing a .NET Multi-platform App UI (.NET MAUI) iOS app it's essential to test it by deploying it to a device, prior to uploading it to the App Store for distribution. Both of these tasks require you or someone else in your organization to enroll in one of Apple's Developer Programs:

1. Enrolling in Apple's [AppStoreConnect Program](https://developer.apple.com/programs/enroll) allows you to publish your iOS applications to Apple's public AppStore.
1. Enrolling in Apple's [Enterprise Program](https://developer.apple.com/programs/enterprise/) allows you to publish your iOS applications to a private "AppStore" that's fully controlled by your organization.

The Apple account management interface in Visual Studio enables you to view signing certificates and provisioning profiles, create new signing certificates, and download existing provisioning profiles.

## Accepting Apple's licensing agreement

Every year, Apple requires you to review and agree to their licensing agreement. To do this, sign-in to your [Apple Developer Account](https://developer.apple.com/account/) and agree to any licensing agreement that's presented to you.

## Generating an API Key

Before you can add an Apple Developer Account to Visual Studio, you'll need to generate an API Key.

### Generating an AppStoreConnect API Key

If you've enrolled in Apple's **AppStoreConnect Program**, you'll need to:

1. Sign-in to your [Apple Developer Account](https://appstoreconnect.apple.com).
1. Select **Users and Access**.
1. Select the **Integrations** tab.
1. Select the **Team Keys** tab.
1. Press the **+** button.
1. Enter a descriptive name in the **Name** field.
1. Enter **Admin** in the **Access** field.
1. Press **OK**.

> [!NOTE]
> Visual Studio currently only supports **Team Keys** and doesn't support **Individual Keys**.
>
> Only **Admin** keys will be able to register new Bundle IDs and generate new provisioning profiles. **Developer** keys won't be able to perform these tasks.
>
> For more information about Apple's AppStoreConnect API, visit Apple's [AppStoreConnect API documentation](https://developer.apple.com/documentation/appstoreconnectapi).

### Generating an Enterprise API Key

If you've enrolled in Apple's **Enterprise Program**, you'll need to:

1. Sign-in to your [Apple Developer Account](https://developer.apple.com/account).
1. Select **Users and Access** located under the **Services** section.
1. Select the **Integrations** tab.
1. Press the **+** button.
1. Enter a descriptive name in the **Name** field.
1. Enter **Admin** in the **Access** field.
1. Press **OK**.

> [!NOTE]
> Only **Admin** keys will be able to register new Bundle IDs and generate new provisioning profiles. **Developer** keys won't be able to perform these tasks.
>
> For more information about how to generate an Enterprise API Key, visit Apple's [Enterprise Program API documentation](https://developer.apple.com/documentation/enterpriseprogramapi).

## Add an Apple Developer Account

To add your Apple account to Visual Studio:

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts** and click on the **Add** button:

    :::image type="content" source="media/apple-account-management/vs/add-account.png" alt-text="Add an Apple Developer Account to Visual Studio.":::

1. Provide a descriptive **Name** for your API Key and copy the **Issuer ID** and **Key ID** values from Apple's website into the appropriate text boxes. If you haven't already done so, download the **Private Key** from Apple's website to a safe location and then use the **Browse...** button to select the location of the downloaded private key file:

    :::image type="content" source="media/apple-account-management/vs/api-key-dialog.png" alt-text="Enter your API Key information.":::

## View signing certificates and provisioning profiles

When you select an Apple Developer Account and a Team name, the **View Details...** button becomes enabled. Clicking it opens the team details dialog that displays a list of signing identifies and provisioning profiles that are installed on your machine. This dialog organizes the signing identities by type, with the **Status** column advising you if the certificate is:

- **Valid**. The signing identity (both the certificate and the private key) is installed on your machine and has not expired.
- **Not in Keychain**. Apple holds a valid signing identity. To install this on your machine, it must be exported from another machine. You cannot download the signing identity from your Apple Developer Account as it doesn't contain the private key.
- **Private key is missing**. A certificate with no private key is installed in the keychain.
- **Expired**. The certificate is expired. You should remove this from your keychain.

## Create a signing certificate

To create a new signing identity, in the team details dialog, click **Create Certificate** to open the drop-down menu and select the [certificate type](https://help.apple.com/xcode/mac/current/#/dev80c6204ec) that you want to create. If you have the correct permissions a new signing identity will appear after a few seconds.

If an option in the drop-down is greyed out and unselected, it means that you don't have the correct team permissions to create this type of certificate.

## Download provisioning profiles

The team details dialog also displays a list of all provisioning profiles associated with your Apple Developer Account. You can download all provisioning profiles to your local machine by clicking the **Download all Profiles** button.
