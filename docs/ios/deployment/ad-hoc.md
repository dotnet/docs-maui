---
title: "Provision a .NET MAUI iOS app for ad-hoc distribution"
description: "Learn how to provision a .NET MAUI iOS app for ad-hoc distribution."
ms.date: 01/17/2023
---

# Provision an iOS app for ad-hoc distribution

Ad-hoc distribution is primarily used for testing apps within a wide group of people, and is available for the Apple Developer Program and the Apple Developer Enterprise Program. Another use case for ad-hoc distribution is distribution within a company when iTunes Connect isn't an option.

Ad-hoc distribution has the advantage of not requiring App Store approval, with apps being installed over-the-air from a web server, or via iTunes. However, it's limited to 100 devices per membership year, for both development and distribution, and the devices must be added to Apple's developer portal. For more information on adding devices, see [Add a device](~/ios/device-provisioning/manual.md#add-a-device).

Distributing an iOS app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) iOS app, you'll need to build a *Distribution Provisioning Profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. A ad-hoc distribution provisioning profile contains an App ID, a distribution certificate, and a list of the devices that can install the app. You can use the same App ID that you used when deploying your app to a device for testing. However, you will need to create a distribution certificate to identify yourself or your organization, if you don't already have one.

The process for creating an ad-hoc distribution provisioning profile is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a distribution certificate. For more information, see [Create a distribution certificate](#create-a-distribution-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

[!INCLUDE [Create a distribution certificate](../includes/distribution-certificate.md)]

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI iOS app to be digitally signed for release, so that it can be installed on an iOS device. A distribution provisioning profile contains an App ID and a distribution certificate.

### Add devices

### Create an App ID

An App ID is required to identify the app that you are distributing. An App ID is similar to a reverse-DNS string, that uniquely identifies an app, and should be identical to the bundle identifier for your app. You can use the same App ID that you used when deploying your app to a device for testing.

There are two types of App ID:

- Wildcard. A wildcard App ID allows you to use a single App ID to match multiple apps, and typically takes the form `com.domainname.*`. A wildcard App ID can be used to distribute multiple apps, and should be used for apps that do not enable app-specific capabilities.
- Explicit. An explicit App ID is unique to a single app, and typically takes the form `com.domainname.myid`. An explicit App ID allows the distribution of one app, with a matching bundle identifier.. Explicit App IDs are typically used for apps that enable app-specific capabilities such as Apple Pay, or Game Center.

For more information about capabilities, see [Capabilities](~/ios/capabilities.md).

To create a new App ID:

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
    > The Bundle ID you enter must correspond to the **Bundle identifier** in the *Info.plist* file in your app project.
    >
    > The bundle identifier for a .NET MAUI app is stored in the .csproj file as the **Application ID** property:
    > - In Visual Studio, in **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.
    > - In Visual Studio for Mac, in the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > App Info** tab. The **Application ID** field lists the bundle identifier.
    >
    > When the value of the **Application ID** field is updated, the value of the **Bundle identifier** in the **Info.plist** will be automatically updated.

1. On the **Register an App ID** page, select any capabilities that the app uses. Any capabilities must be configured both on this page and in the *Entitlements.plist* file in your app project. For more information see [Capabilities](~/ios/capabilities.md) and [Entitlements](~/ios/entitlements.md).
1. On the **Register an App ID** page, click the **Continue** button.
1. On the **Confirm your App ID** page, click the **Register** button.

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device.

To create a distribution provisioning profile:

1. In your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **App Store** radio button before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/register-provisioning-profile.png" alt-text="Register a provisioning profile for app store distribution.":::

1. In the **Generate a Provisioning Profile** page, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/provisioning/select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/provisioning/generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Visual Studio.

## Download provisioning profiles in Visual Studio

After creating a distribution provisioning profile in Apple's developer portal, Visual Studio can download it so that it's available for signing your app.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button.
1. In the **Details** dialog, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio to refresh the list.
1. In the **Details** dialog, click the **Download All Profiles** button.

The development provisioning profile will then be available for use.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select a team and click the **View Details...** button.
1. In the **Details** window, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio for Mac to refresh the list.
1. In the **Details** dialog, click the **Download All Profiles** button.

The development provisioning profile will then be available for use.

---

<!-- TODO: Next button to the doc where they actually use the distribution provisioning profile to generate the app package. -->
