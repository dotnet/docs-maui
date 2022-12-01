---
title: "Manual provisioning for .NET MAUI iOS apps"
description: "Learn how to use manual provisioning to request development certificates and profiles for .NET MAUI iOS apps."
ms.date: 11/29/2022
---

# Manual provisioning for iOS apps

Manual provisioning involves creating and managing signing identities, app IDs, and provisioning profiles in the [Apple Developer portal](https://developer.apple.com/account), and then downloading this data into Visual Studio.

## Create a signing certificate

The first step in setting up a development device is to create a signing certificate, which consists of a development certificate and a private key.

Development certificates and associated [keys](#understanding-certificate-key-pairs) are critical for an iOS developer. They establish your identity with Apple and associate you with a given device and profile for development, which is analogous to putting your digital signature on your apps. Apple checks certificates to control access to the devices you are allowed to deploy to.

Development teams, certificates, and profiles can be managed by accessing the [Certificates, Identifiers & Profiles](https://developer.apple.com/account/resources/certificates/list) section of Apple's developer portal. Apple requires you to have a signing identity to build your code for device or simulator.  

> [!IMPORTANT]
> You can only have two iOS Development certificates at any one time. If you need to create any more, you will need to revoke an existing one. Any machine using a revoked certificate will not be able to sign their app.

Before starting the manual provisioning process, you should ensure that you've added your Apple developer account to Visual Studio. For more information, see [Apple account management](~/ios/apple-account-management.md).

Once you've added your Apple developer account to Visual Studio, you need to generate a signing certificate.

# [Visual Studio](#tab/vs)

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button.
1. In the **Details** dialog, click **Create Certificate** and select **iOS Development**. A new signing identity will be created, and will sync with Apple, provided that you have the correct permissions.

# [Visual Studio for Mac](#tab/vsmac)

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select a team and click the **View Details...** button.
1. In the **Details** window, click **Create Certificate** and select **Apple Development** or **iOS Development**. A new signing identity will be created, and will sync with Apple, provided that you have the correct permissions.

---

### Understanding certificate key pairs

A developer profile contains certificates, their associated keys, and any provisioning profiles associated with the account. There are two versions of a developer profile — one exists in Apple's developer portal, and the other lives on a local Mac. The difference between the two is the type of keys they contain: the profile on Apple's developer portal contains all of the public keys associated with your certificates, while the copy on your local Mac contains all of the private keys. For certificates to be valid, the key pairs must match.

> [!WARNING]
> Losing the certificate and associated keys can be incredibly disruptive, as it will require revoking existing certificates and re-provisioning any associated devices. After successfully setting up  development certificates, export a backup copy and store them in a safe place. For more information on how to do this, see [Export developer accounts](https://help.apple.com/xcode/mac/current/#/dev8a2822e0b) on help.apple.com.

## Provision an iOS device for development

After creating a signing certificate you must set up a provisioning profile so that it's possible to deploy an app to an Apple device. The device must be running a version of iOS that is supported by Xcode.

### Add a device

When creating a provisioning profile for development, you must state which devices can run the app. Before selecting a device to be added to a provisioning profile you must first add the device to Apple's developer portal. This can be achieved with the following steps:

1. Connect the device to be provisioned to your local Mac with a USB cable.
1. Open Xcode, and navigate to **Window > Devices and Simulators**.
1. In Xcode, select the **Devices** tab, and select the device from the list of connected devices.
1. In Xcode, copy the **Identifier** value to the clipboard:

    :::image type="content" source="media/manual-provisioning/xcode-devices.png" alt-text="Xcode devices and simulator window with the iOS identifier string location highlighted.":::

1. In a web browser, go to the [Devices](https://developer.apple.com/account/resources/devices/list) section of Apple's developer portal and click the **+** button.
1. In the **Register a New Device** page, set the correct **Platform** and provide a name for the new device. Then paste the identifier from the clipboard into the **Device ID (UDID)** field.
1. In the **Register a New Device** page, click the **Continue** button.
1. In the **Register a New Device** page, review the information and then click the **Register** button.

Repeat the above steps for any iOS device that will be used to test or debug a .NET MAUI iOS app.

### Create a development provisioning profile

After adding a device to Apple's developer portal, it's necessary to create a provisioning profile and add the device to it.

Before creating a provisioning profile, an *App ID* must be created. An App ID is a reverse-DNS style string that uniquely identifies an app. The recommended approach is to create a *wildcard App ID*, which can be used to install most apps to a device. The alternative is to use an *explicit App ID*, which only allows the installation of one app (with a matching bundle ID) to a device, and is generally used for apps that use iOS features such as Apple Pay or HealthKit. <!-- For information on creating Explicit App IDs, refer to the [Working with Capabilities](~/ios/deploy-test/provisioning/capabilities/index.md) guide.-->

#### Create an wildcard App ID

A wildcard App ID can be created with the following steps:

1. In a web browser, go to the [Identifiers](https://developer.apple.com/account/resources/identifiers/list) section of Apple's developer portal and click the **+** button.
1. In the **Register a new identifier** page, select **App IDs** and click the **Continue** button.
1. In the **Register a new identifier** page, select the **App** type and click the **Continue** button.
1. In the **Register an App ID** page, provide a **Description** and set the **Bundle ID** to **Wildcard**. Then, enter an App ID in the format `com.domainname.*`:

    :::image type="content" source="media/manual-provisioning/register-app-id.png" alt-text="Screenshot of new App ID registration page with required fields populated.":::

1. In the **Register an App ID** page, click the **Continue** button.
1. In the **Confirm your App ID** page, review the information and then click the **Register** button.

#### Create a development provisioning profile

Once the App ID has been created, the provisioning profile can be created. A provisioning profile contains information on what app (or apps, if it's a wildcard app ID) the profile relates to, who can use the profile (depending on the developer certificates that are added), and what devices can install the app.

A provisioning profile for development can be created with the following steps:

1. In a web browser, go to the [Profiles](https://developer.apple.com/account/resources/profiles/list) section of Apple's developer portal and click the **+** button.
1. In the **Register a New Provisioning Profile** page, in the **Development** section, select **iOS App Development** and click the **Continue** button.
1. In the **Generate a Provisioning Profile** page, select the **App ID** to use and then click the **Continue** button.
1. In the **Generate a Provisioning Profile** page, select the certificates to include in the provisioning profile and then click the **Continue** button.
1. In the **Generate a Provisioning Profile** page, select the devices that the app will be installed on and then click the **Continue** button.
1. In the **Generate a Provisioning Profile** page, provide a provisioning profile name and then click the **Generate** button.
1. In the **Generate a Provisioning Profile** page, you can optionally click the **Download** button to download the provisioning profile.

## Download provisioning profiles in Visual Studio

After creating a new provisioning profile in Apple's developer portal, you can use Visual Studio to download it so that it's available for signing your app.

# [Visual Studio](#tab/vs)

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button.
1. In the **Details** dialog, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio to refresh the list.
1. In the **Details** dialog, click the **Download All Profiles** button.

The new provisioning profile will then be available for use.

# [Visual Studio for Mac](#tab/vsmac)

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select a team and click the **View Details...** button.
1. In the **Details** window, click **Create Certificate** and select **Apple Development** or **iOS Development**. A new signing identity will be created, and will sync with Apple, provided that you have the correct permissions.

The new provisioning profile will then be available for use.

---

## Enable manual provisioning

After manually creating the provisioning profile, and installing it in Visual Studio, your .NET MAUI app project should be configured to use manual provisioning.

# [Visual Studio](#tab/vs)

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab and ensure that the value of the **Application ID** field corresponds to the format of the App ID you created earlier.
1. In the project properties, navigate to the **iOS Bundle Signing** tab and ensure that **Manual Provisioning** is selected in the **Scheme** drop-down:

    :::image type="content" source="media/manual-provisioning/vs/bundle-signing.png" alt-text="Screenshot of bundle signing tab for iOS in Visual Studio.":::

1. In the **Bundle Signing** properties, select your **Signing identity** and **Provisioning profile**, or set both to **Automatic**. When **Signing identity** and **Provisioning profile** are both set to **Automatic**, Visual Studio will select the signing identity and provisioning profile based on the **Bundle identifier** in **Info.plist**.

# [Visual Studio for Mac](#tab/vsmac)

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

    When **Signing Identity** and **Provisioning profile** are both set to **Automatic**, Visual Studio for Mac will select the signing identity and provisioning profile based on the **Bundle identifier** in **Info.plist**.

1. In the **Project Properties** window, click the **OK** button.
1. In Visual Studio for Mac, close the **Info.plist** file.

---

[!INCLUDE [Deploy the app to your device](~/ios/includes/deploy.md)]


## Provisioning for application services

Apple provides a selection of application services, known as capabilities, that can be activated for a .NET MAUI iOS app. These capabilities must be configured in both your provisioning profile, when the **App ID** is created, and in the **Entitlements.plist** file that should be added to the .NET MAUI app project. For more information about capabilities, see [Entitlements and capabilities](~/ios/deployment/entitlements.md), and [Capabilites]((https://developer.apple.com/documentation/xcode/capabilities) on developer.apple.com.
