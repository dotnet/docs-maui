---
title: "Publish a .NET MAUI iOS app for in-house distribution"
description: "Learn how to provision and publish a .NET MAUI iOS app for in-house distribution."
ms.date: 08/27/2024
ms.custom: sfi-image-nochange
---

# Publish an iOS app for in-house distribution

> [!div class="op_single_selector"]
>
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)
> - [Publish using the command line](publish-cli.md)

In-house distribution enables members of the Apple Developer Enterprise Program to distribute apps internally to other members of the same organization. This has the advantage of not requiring an App Store review, and having no limit on the number of devices on which an app can be installed. However, members of the Apple Developer Enterprise Program don't have access to App Store Connect, and therefore the licensee is responsible for distributing the app.

Distributing an iOS app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) iOS app, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. An in-house distribution provisioning profile contains an App ID and a distribution certificate. You can use the same App ID that you used when deploying your app to a device for testing. However, you'll need to create a distribution certificate to identify your organization, if you don't already have one.

The process for creating an in-house distribution provisioning profile is as follows:

1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

## Create a distribution profile

An in-house distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release. An in-house distribution provisioning profile contains an App ID and a distribution certificate.

<!-- Can't use the include file as the steps are slightly different -->

### Create an App ID

An App ID is required to identify the app that you are distributing. An App ID is similar to a reverse-DNS string, that uniquely identifies an app, and should be identical to the bundle identifier for your app. You can use the same App ID that you used when deploying your app to a device for testing.

There are two types of App ID:

- Wildcard. A wildcard App ID allows you to use a single App ID to match multiple apps, and typically takes the form `com.domainname.*`. A wildcard App ID can be used to distribute multiple apps, and should be used for apps that do not enable app-specific capabilities.
- Explicit. An explicit App ID is unique to a single app, and typically takes the form `com.domainname.myid`. An explicit App ID allows the distribution of one app, with a matching bundle identifier.. Explicit App IDs are typically used for apps that enable app-specific capabilities such as Apple Pay, or Game Center.

For more information about capabilities, see [Capabilities](~/ios/capabilities.md).

To create a new App ID:

1. In your Apple Developer Account, navigate to **Certificates, IDs & Profiles**.
1. On the **Certificates, Identifiers & Profiles** page, select the **Identifiers** tab.
1. On the **Identifiers** page, click the **+** button to create a new App ID.
1. On the **Register a new identifier** page, select the **App IDs** radio button before clicking the **Continue** button:

    :::image type="content" source="../deployment/media/publish/create-app-id.png" alt-text="Create an App ID.":::

1. On the **Register an App ID** page, enter a description, and select either the **Explicit** or **Wildcard** Bundle ID radio button. Then, enter the Bundle ID for your app in reverse DNS format:

    :::image type="content" source="../deployment/media/publish/specify-bundle-id.png" alt-text="Specify the bundle identifier for the app.":::

    <!-- markdownlint-disable MD032 -->
    > [!IMPORTANT]
    > The Bundle ID you enter must correspond to the **Bundle identifier** in the *Info.plist* file in your app project.
    >
    > The bundle identifier for a .NET MAUI app is stored in the project file as the **Application ID** property. In Visual Studio, in **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.
    >
    > When the value of the **Application ID** field is updated, the value of the **Bundle identifier** in the **Info.plist** will be automatically updated.
    <!-- markdownlint-enable MD032 -->

1. On the **Register an App ID** page, select any capabilities that the app uses. Any capabilities must be configured both on this page and in the *Entitlements.plist* file in your app project. For more information see [Capabilities](~/ios/capabilities.md) and [Entitlements](~/ios/entitlements.md).
1. On the **Register an App ID** page, click the **Continue** button.
1. On the **Register an App ID** page, enter your deployment details if you have them, prior to clicking the **Continue** button.
1. On the **Confirm your App ID** page, click the **Register** button.

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device.

To create a distribution provisioning profile:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **In House** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish/inhouse-register-provisioning-profile.png" alt-text="Register a provisioning profile for in house distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish/inhouse-select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish/inhouse-generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Visual Studio.

[!INCLUDE [Download provisioning profiles in Visual Studio](../includes/download-profiles.md)]

## Publish the app

Visual Studio can publish a .NET MAUI iOS app for in-house distribution:

[!INCLUDE [Publish](../includes/publish-vs.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **Enterprise** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-enterprise.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-enterprise.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

    > [!NOTE]
    > You should use the signing identity and provisioning profile that were created for your app and the selected distribution channel.

1. In the **Distribute - Signing Identity** dialog, select the **Save As** button and enter a filename. Your app will then be re-signed and published to an *.ipa* file on your file system.

For information about publishing an iOS app using the Command Line Interface (CLI), see [Publish an iOS app using the command line](publish-cli.md).

In-house apps can be distributed via a secure website, or via Mobile Device Management (MDM). Both of these approaches require the app to be prepared for distribution, which includes the preparation of a manifest. For more information, see [Distribute proprietary in-house apps to Apple devices](https://support.apple.com/guide/deployment/depce7cefc4d/web) on support.apple.com.
