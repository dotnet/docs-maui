---
title: "Manual provisioning for .NET MAUI iOS apps"
description: "Learn how to use manual provisioning to create development certificates and profiles for .NET MAUI iOS apps."
ms.date: 11/29/2022
---

# Manual provisioning for iOS apps

Manual provisioning is an approach for deploying a .NET Multi-platform App UI (.NET MAUI) iOS app to a device, where you have full control over the provisioning data that gets created. With this approach, you manually create and manage signing identities, App IDs, and provisioning profiles in your [Apple Developer Account](https://developer.apple.com/account), and then have to download this data into Visual Studio. Before starting the manual provisioning process, you should ensure that you've added your Apple Developer Account to Visual Studio. For more information, see [Apple account management](~/ios/apple-account-management.md).

Development teams, certificates, and profiles can all be managed by accessing the [Certificates, Identifiers & Profiles](https://developer.apple.com/account/resources/certificates/list) section of your Apple Developer Account.

## Create a signing certificate

With manual provisioning, the first step in setting up a development device is to create a signing certificate, which consists of a development certificate and a private key.

Development certificates and their associated keys establish your identity with Apple, and associate you with specific devices and profiles for development. This is analogous to adding your digital signature to your apps. Apple uses these certificates to ensure that you can only deploy your apps to specific devices.

> [!IMPORTANT]
> You can only have two iOS development certificates at any one time. If you need to create any more, you will need to revoke an existing one. Any machine using a revoked certificate will not be able to sign their app.

Once you've added your Apple Developer Account to Visual Studio, you need to generate a signing certificate.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and then select **View Details**.
1. In the **Details** dialog, select **Create Certificate** > **iOS Development**. A new signing identity will be created and will sync with Apple if you have the correct permissions.

> [!IMPORTANT]
> The private key and certificate that make up your signing identity will also be exported to **Keychain Access** on your Mac build host, provided that the IDE is paired to it. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select a team and then select **View Details**.
1. In the **Details** window, select **Create Certificate** and then select **Apple Development** or **iOS Development**. A new signing identity will be created and will sync with Apple if you have the correct permissions.

---

### Understanding certificate key pairs

A developer profile contains certificates, their associated keys, and any provisioning profiles associated with the Apple Developer Account. There are two versions of a developer profile—one exists in your Apple Developer Account, and the other lives on a local machine. The difference between the two is the type of keys they contain: the profile in your Apple Developer Account contains all of the public keys associated with your certificates, while the copy on your local machine contains all of the private keys. For certificates to be valid, the key pairs must match.

> [!WARNING]
> Losing the certificate and associated keys can be incredibly disruptive, as it will require revoking existing certificates and re-provisioning any associated devices. After successfully setting up a development certificate, export a backup copy and store it in a safe place. For more information on how to do this, see [Export developer accounts](https://help.apple.com/xcode/mac/current/#/dev8a2822e0b) on help.apple.com.

## Provision an iOS device for development

After creating a signing certificate, you must set up a development provisioning profile so that it's possible to deploy your app to a device. The device must be running a version of iOS that's supported by Xcode.

[!INCLUDE [Add a device](../includes/add-a-device.md)]

### Create an App ID

After adding a device to your Apple Developer Account, you should create an *App ID*. An App ID is similar to a reverse-DNS string, that uniquely identifies an app, and should be identical to the bundle identifier for your app.

<!-- markdownlint-disable MD032 -->
> [!IMPORTANT]
> The bundle identifier for a .NET MAUI app is stored in the project file as the **Application ID** property:
> - In Visual Studio, in **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.
> - In Visual Studio for Mac, in the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > App Info** tab. The **Application ID** field lists the bundle identifier.
>
> When the value of the **Application ID** field is updated, the value of the **Bundle identifier** in the **Info.plist** will be automatically updated.
<!-- markdownlint-enable MD032 -->

There are two types of App ID:

- Wildcard. A wildcard App ID allows you to use a single App ID to match multiple apps, and typically takes the form `com.domainname.*`. A wildcard App ID can be used to install most apps to a device, and should be used for apps that do not enable app-specific capabilities.
- Explicit. An explicit App ID is unique to a single app, and typically takes the form `com.domainname.myid`. An explicit App ID allows the installation of one app, with a matching bundle identifier, to a device. Explicit App IDs are typically used for apps that enable app-specific capabilities such as Apple Pay, or Game Center.

The recommended approach is to create a *wildcard App ID*, unless your app uses app-specific capabilities. For more information about capabilities, see [Capabilities](~/ios/capabilities.md).

A wildcard App ID can be created with the following steps:

1. In a web browser, go to the [Identifiers](https://developer.apple.com/account/resources/identifiers/list) section of your Apple Developer Account and click the **+** button.
1. In the **Register a new identifier** page, select **App IDs** and click the **Continue** button.
1. In the **Register a new identifier** page, select the **App** type and click the **Continue** button.
1. In the **Register an App ID** page, provide a **Description** and set the **Bundle ID** to **Wildcard**. Then, enter an App ID in the format `com.domainname.*` and click the **Continue** button:

    :::image type="content" source="media/manual-provisioning/register-app-id.png" alt-text="Screenshot of new App ID registration page with required fields populated.":::

1. In the **Confirm your App ID** page, review the information and then click the **Register** button.

### Create a development provisioning profile

Once the App ID has been created, you should create a development provisioning profile. A provisioning profile contains information on what app (or apps, if it's a wildcard App ID) the profile relates to, who can use the profile (depending on the developer certificates that are added), and what devices can install the app.

A development provisioning profile can be created with the following steps:

1. In a web browser, go to the [Profiles](https://developer.apple.com/account/resources/profiles/list) section of your Apple Developer Account and click the **+** button.
1. In the **Register a New Provisioning Profile** page, in the **Development** section, select **iOS App Development** and click the **Continue** button:

    :::image type="content" source="media/manual-provisioning/provisioning-profile-ios.png" alt-text="Screenshot of creating a provisioning profile for iOS app development.":::

1. In the **Generate a Provisioning Profile** page, select the **App ID** to use and then click the **Continue** button.

    :::image type="content" source="media/manual-provisioning/provisioning-profile-appid.png" alt-text="Screenshot of adding an App ID to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, select the certificates to include in the provisioning profile and then click the **Continue** button.

    :::image type="content" source="media/manual-provisioning/provisioning-profile-certificates.png" alt-text="Screenshot of adding a certificate to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, select the devices that the app will be installed on and then click the **Continue** button.

    :::image type="content" source="media/manual-provisioning/provisioning-profile-devices.png" alt-text="Screenshot of adding a device to a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, provide a provisioning profile name and then click the **Generate** button.

    :::image type="content" source="media/manual-provisioning/provisioning-profile-name.png" alt-text="Screenshot of naming a provisioning profile.":::

1. In the **Generate a Provisioning Profile** page, you can optionally click the **Download** button to download the provisioning profile.

## Download provisioning profiles in Visual Studio

After creating a development provisioning profile in your Apple Developer Account, Visual Studio can download it so that it's available for signing your app.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button.
1. In the **Details** dialog, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio to refresh the list.
1. In the **Details** dialog, click the **Download All Profiles** button.

The provisioning profiles will be downloaded on Windows, and exported to your Mac build host if the IDE is paired to it. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select a team and click the **View Details...** button.
1. In the **Details** window, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio for Mac to refresh the list.
1. In the **Details** dialog, click the **Download All Profiles** button.

The development provisioning profile will then be available for use.

---

## Enable manual provisioning

After manually creating the development provisioning profile, and installing it in Visual Studio, your .NET MAUI app project should be configured to use manual provisioning.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab and ensure that the value of the **Application ID** field corresponds to the format of the App ID you created earlier.
1. In the project properties, navigate to the **iOS Bundle Signing** tab and ensure that **Manual Provisioning** is selected in the **Scheme** drop-down:

    :::image type="content" source="media/manual-provisioning/vs/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio.":::

1. In the **Bundle Signing** properties, select your **Signing identity** and **Provisioning profile**, or set both to **Automatic**. When **Signing identity** and **Provisioning profile** are both set to **Automatic**, Visual Studio will select the signing identity and provisioning profile based on the **Bundle identifier** in **Info.plist** (which is identical to the value of the **Application ID** property in your project file).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > App Info** tab and ensure that the value of the **Application ID** field corresponds to the format of the App ID you created earlier.
1. In the **Solution Window**, double-click the **Info.plist** file from the **Platforms > iOS** folder of your .NET MAUI app project to open it in the editor.
1. In the **Info.plist** editor, change from the **Source** view to the **Application** view:

    :::image type="content" source="media/manual-provisioning/vsmac/source-view.png" alt-text="Screenshot of source view in Info.plist editor in Visual Studio for Mac.":::

1. In the **Signing** section of the **Application** view, select **Manual Provisioning**:

    :::image type="content" source="media/manual-provisioning/vsmac/manual-provisioning.png" alt-text="Screenshot of manual provisioning selected as the signing scheme in Visual Studio for Mac.":::

1. In the **Signing** section of the **Application** view, click the **Bundle Signing Options...** button.
1. In the **Project Properties** window, select your **Signing Identity** and **Provisioning profile**, or leave both as **Automatic**:

    :::image type="content" source="media/manual-provisioning/vsmac/bundle-signing.png" alt-text="Screenshot of bundle signing options in Visual Studio for Mac.":::

    When **Signing Identity** and **Provisioning profile** are both set to **Automatic**, Visual Studio for Mac will select the signing identity and provisioning profile based on the **Bundle identifier** in **Info.plist** (which is identical to the value of the **Application ID** property in your project file).

1. In the **Project Properties** window, click the **OK** button.
1. In Visual Studio for Mac, close the **Info.plist** file.

---

[!INCLUDE [Deploy the app to your device](~/ios/includes/deploy.md)]

## Provisioning for application services

Apple provides a selection of application services, known as capabilities, that can be activated for a .NET MAUI iOS app. These capabilities must be configured in both your provisioning profile, when the **App ID** is created, and in the *Entitlements.plist* file that should be added to the .NET MAUI app project. For more information about capabilities, see [Entitlements](~/ios/entitlements.md) and [Capabilities](~/ios/capabilities.md).
