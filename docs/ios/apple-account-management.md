---
title: "Apple account management"
description: "Learn how to use Apple account management features in Visual Studio and Visual Studio for Mac"
ms.date: 11/15/2022
---

# Apple account management

When developing a .NET Multi-platform App UI (.NET MAUI) iOS app it's essential to test it by deploying it to a device, prior to uploading it to the App Store for distribution. Both of these tasks require you to have an [Apple ID](https://appleid.apple.com/account), and have enrolled your Apple ID in the [Apple Developer Program](https://developer.apple.com/programs).

The Apple account management interface in Visual Studio and Visual Studio for Mac enables you to add your Apple ID, and provides the ability to view information about development teams associated with the Apple ID, view signing certificates and provisioning profiles, create new signing certificates, and download existing provisioning profiles.

> [!IMPORTANT]
> Adding an Apple account that uses federated credentials isn't possible in Visual Studio.

## Add an Apple Developer Account

Before you begin, ensure that you've accepted any user license agreements in your [Apple Developer Account](https://developer.apple.com/account/) and [App Store Connect](https://appstoreconnect.apple.com/).

If you have an individual Apple Developer account, as opposed to an enterprise account, you'll also need to create an App Store Connect API key. For information about creating an App Store Connect API key, see [Creating API Keys for App Store Connect API](https://developer.apple.com/documentation/appstoreconnectapi/creating_api_keys_for_app_store_connect_api) on developer.apple.com.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**, click on the **Add** button and select **Individual Account...** or **Enterprise Account...**:

    :::image type="content" source="media/apple-account-management/vs/add-account.png" alt-text="Add an Apple Developer Account to Visual Studio.":::

1. To add an enterprise account, in the **Enterprise Account...** dialog, enter your Apple ID and password, and click the **Login** button:

    :::image type="content" source="media/apple-account-management/vs/enterprise-account.png" alt-text="Add an Enterprise Apple Developer Account to Visual Studio.":::

    Provided that your account details are valid, your Apple Developer Account will be added to Visual Studio.

1. To add an individual account, in the **Individual Account...** dialog, enter your App Store Connect API key data and click the **Add** button:

    :::image type="content" source="media/apple-account-management/vs/individual-account.png" alt-text="Add an Individual Apple Developer Account to Visual Studio.":::

    The **Name**, **Issuer ID**, and **Key ID** data can be found in [App Store Connect](https://appstoreconnect.apple.com/) by selecting **Users and Access** and then the **Keys** tab. The **Private key** can also be downloaded from this location:

    :::image type="content" source="media/apple-account-management/app-store-connect-details.png" lightbox="media/apple-account-management/app-store-connect-details-large.png" alt-text="Screenshot of Apple App Store Connect API details.":::

    Provided that your account details are valid, your Apple Developer Account will be added to Visual Studio.

1. Once your account has been added successfully, you'll see your Apple ID and any teams that your Apple ID is part of:

    :::image type="content" source="media/apple-account-management/vs/account.png" alt-text="Apple Developer Account added to Visual Studio.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**, click on the **Add** button and select **Individual Account...** or **Enterprise Account...**:

    :::image type="content" source="media/apple-account-management/vsmac/add-account.png" alt-text="Add an Apple Developer Account to Visual Studio for Mac.":::

1. To add an enterprise account, in the **Add Enterprise Account** dialog, enter your Apple ID and password, and click the **Sign In** button:

    :::image type="content" source="media/apple-account-management/vsmac/enterprise-account.png" alt-text="Add an Enterprise Apple Developer Account to Visual Studio for Mac.":::

    Provided that your account details are valid, your Apple Developer Account will be added to Visual Studio.

1. To add an individual account, in the **Add Individual Account** dialog, enter your App Store Connect API key data and click the **Add** button:

    :::image type="content" source="media/apple-account-management/vsmac/individual-account.png" alt-text="Add an Individual Apple Developer Account to Visual Studio for Mac.":::

    The **Name**, **Issuer ID**, and **Key ID** data can be found in [App Store Connect](https://appstoreconnect.apple.com/) by selecting **Users and Access** and then the **Keys** tab. The **Private key** can also be downloaded from this location:

    :::image type="content" source="media/apple-account-management/app-store-connect-details.png" lightbox="media/apple-account-management/app-store-connect-details-large.png" alt-text="Screenshot of Apple App Store Connect API details.":::

    Provided that your account details are valid, your Apple Developer Account will be added to Visual Studio.

1. Once your account has been added successfully, you'll see your Apple ID and any teams that your Apple ID is part of:

    :::image type="content" source="media/apple-account-management/vsmac/account.png" alt-text="Apple Developer Account added to Visual Studio.":::

---

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
