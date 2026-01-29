---
title: "Publish a .NET MAUI Mac Catalyst app for ad-hoc distribution"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for ad-hoc distribution to a limited number of registered devices."
ms.date: 03/20/2023
ms.custom: sfi-image-nochange
---

# Publish a Mac Catalyst app for ad-hoc distribution

> [!div class="op_single_selector"]
>
> - [Publish an unsigned app](publish-unsigned.md)
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish outside the app store](publish-outside-app-store.md)

When distributing Mac Catalyst apps outside the Mac App Store, you can also choose to distribute your app to a limited number of users on registered devices. This is known as *ad-hoc* distribution, and is primarily used for testing apps within a group of people. However, it's limited to 100 devices per membership year, and the devices must be added to your Apple Developer Account. Members of the Apple Developer Program and the Apple Developer Enterprise Program can use this distribution approach.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the Mac App Store with ad-hoc distribution, you'll need to build a *development provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. An ad-hoc development provisioning profile contains an App ID, a development certificate, and a list of the devices that can install the app. You'll need to create a development certificate to identify yourself or your organization, if you don't already have one.

The process for provisioning a .NET MAUI Mac Catalyst app for ad-hoc distribution is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a development certificate. For more information, see [Create a development certificate](#create-a-development-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Add devices to your Apple Developer Account. For more information, see [Add devices](#add-devices).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).
1. Download your provisioning profile. For more information, see [Download your provisioning profile in Xcode](#download-your-provisioning-profile-in-xcode).

> [!IMPORTANT]
> This article shows provisioning for ad-hoc distribution using an Apple Developer Account that's part of the Apple Developer Program. It can also be used for an Apple Developer Account that's part of the Apple Developer Enterprise Program, as the steps required are largely identical.

Then, once provisioning is complete you should prepare your app for publishing, and then publish it with the following process:

1. Optionally add entitlements to your app. For more information, see [Add entitlements](#add-entitlements).
1. Update the app's *Info.plist* file. For more information, see [Update Info.plist](#update-infoplist).
1. Publish your app using the command line. For more information, see [Publish using the command line](#publish-using-the-command-line).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

## Create a development certificate

The CSR allows you to generate a development certificate, which confirms your identity. The development certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Apple Development** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/apple-developer-certificate.png" alt-text="Create an Apple development certificate.":::

1. On the **Create a New Certificate** page, select **Choose File**:

    :::image type="content" source="media/publish-ad-hoc/choose-certificate.png" alt-text="Upload your certificate signing request.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-ad-hoc/download-developer-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Apple Development**:

    :::image type="content" source="media/publish-ad-hoc/keychain-access-developer-certificate.png" alt-text="Keychain Access showing development certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a development profile

An ad-hoc development provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed, so that it can be installed on specific Macs. An ad-hoc development provisioning profile contains an App ID, a development certificate, and a list of the devices that can install the app.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

### Add devices

When creating a provisioning profile for ad-hoc distribution, the profile must include which devices can run the app. Before selecting the devices to be added to the provisioning profile you must first add devices to your Apple Developer Account. This can be achieved with the following steps:

1. Select the **Apple > About this Mac** menu item.
1. In the **Overview** tab, select the **System Report...** button.
1. In the **System Report**, select the **Hardware** expander to see the hardware overview. The report displays the universally unique identifier (UUID) as **Hardware UUID** in macOS 10.15 and earlier, or **Provisioning UDID** in macOS 11.0 and later.
1. Select the **Hardware UUID** or **Provisioning UDID** value and copy it to the clipboard.
1. In a web browser, go to the [Devices](https://developer.apple.com/account/resources/devices/list) section of your Apple Developer Account and click the **+** button.
1. In the **Register a New Device** page, set the **Platform** to **macOS** and provide a name for the new device. Then paste the identifier from the clipboard into the **Device ID (UUID)** field, and click the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/add-device.png" alt-text="Register a device by naming it and entering its unique device identifier.":::

1. In the **Register a New Device** page, review the information and then click the **Register** button.

Repeat the above steps for any Macs that you want to deploy your .NET MAUI Mac Catalyst app to using ad-hoc distribution.

### Create a provisioning profile

Once the App ID has been created, you should create a development provisioning profile. This profile enables the app to be digitally signed so that it can be installed on specific Macs.

To create a provisioning profile for ad-hoc distribution:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **macOS App Development** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/register-provisioning-profile.png" alt-text="Register a provisioning profile for ad-hoc distribution.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the check box that corresponds to your development certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish-ad-hoc/select-certificate.png" alt-text="Select your development certificate.":::

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

## Add entitlements

Apple's App Sandbox restricts access to system resources and user data in Mac apps, to contain damage if an app becomes compromised. It must be enabled for Mac Catalyst apps that are distributed through the Mac App Store, and can optionally be enabled for Mac Catalyst apps that are distributed outside the Mac App Store.

[!INCLUDE [Add entitlements](../includes/add-entitlements.md)]

## Update Info.plist

Before distributing your app, you should update its *Info.plist* file with additional information.

> [!NOTE]
> While it's not a requirement to update your app's *Info.plist* file when distributing it for testing, these updates will be required when distributing the final tested version of your app. Therefore, it's best practice to perform these updates.

[!INCLUDE [Update Info.plist](../includes/update-info-plist.md)]

### Declare your app's use of encryption

If your app uses encryption, and you plan to distribute it outside the United States or Canada, it's subject to US export compliance requirements. You can provide information about you app's use of encryption in its *Info.plist* file.

[!INCLUDE [Declare your app's use of encryption](../includes/encryption.md)]

## Publish using the command line

To publish your Mac Catalyst app from the command line on a Mac, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net8.0-maccatalyst`.                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |
| `-p:MtouchLink`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                       |
| `-p:CreatePackage`           | Set to `true` so that a package (*.pkg*) is created for the app at the end of the build.        |
| `-p:EnableCodeSigning`       | Set to `true` so that code signing is enabled.                                                  |
| `-p:CodesignKey`             | The name of the code signing key. Set to the name of your distribution certificate, as displayed in Keychain Access. |
| `-p:CodesignProvision`       | The provisioning profile to use when signing the app bundle. |
| `-p:CodesignEntitlements`    | The path to the entitlements file that specifies the entitlements the app requires. Set to `Platforms\MacCatalyst\Entitlements.plist`. |
| `-p:RuntimeIdentifier`       | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |
| `-p:UseHardenedRuntime`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the Mac App Store. |

[!INCLUDE [Additional build parameters](../includes/additional-build-parameters.md)]

For example, use the following command to build and sign a *.pkg* on a Mac, for ad-hoc distribution to users on registered devices:

```dotnetcli
dotnet publish -f net8.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true  -p:CodesignKey="Apple Development: John Smith (AY2GDE9QM7)" -p:CodesignProvision="MyMauiApp (Ad-hoc)" -p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.plist" -p:UseHardenedRuntime=true
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Publishing builds, signs, and packages the app, and then copies the *.pkg* to the *bin/Release/net8.0-maccatalyst/publish/* folder. If you publish the app using only a single architecture, it will be published to the *bin/Release/net8.0-maccatalyst/{architecture}/publish/* folder.

During the signing process it maybe necessary to enter your login password and allow `codesign` to run:

:::image type="content" source="media/codesign.png" alt-text="Allow codesign to sign your app on your Mac.":::

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

<!-- Todo: It's possible to re-sign an existing app bundle with a different certificate to change the distribution channel -->

## Define build properties in your project file

An alternative to specifying build parameters on the command line is to specify them in your project file in a `<PropertyGroup>`. The following table lists some of the common build properties:

| Property                     | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `<ApplicationTitle>` | The user-visible name for the app. |
| `<ApplicationId>` | The unique identifier for the app, such as `com.companyname.mymauiapp`. |
| `<ApplicationVersion>` | The version of the build that identifies an iteration of the app. |
| `<ApplicationDisplayVersion>` | The version number of the app. |
| `<CodesignKey>`             | The name of the code signing key. Set to the name of your distribution certificate, as displayed in Keychain Access. |
| `<CodesignEntitlements>`    | The path to the entitlements file that specifies the entitlements the app requires. Set to `Platforms\MacCatalyst\Entitlements.plist`. |
| `<CodesignProvision>`       | The provisioning profile to use when signing the app bundle. |
| `<CreatePackage>`           | Set to `true` so that a package (*.pkg*) is created for the app at the end of the build.        |
| `<EnableCodeSigning>`       | Set to `true` so that code signing is enabled.                                                  |
| `<MtouchLink>`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                       |
| `<RuntimeIdentifier>` | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |
| `<UseHardenedRuntime>`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the Mac App Store. |

For a full list of build properties, see [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties).

> [!IMPORTANT]
> Values for these build properties don't have to be provided in the project file. They can also be provided on the command line when you publish the app. This enables you to omit specific values from your project file.

The following example shows a typical property group for building and signing your Mac Catalyst app for ad-hoc distribution to users on registered devices:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Release|net8.0-maccatalyst|AnyCPU'">
  <MtouchLink>SdkOnly</MtouchLink>
  <EnableCodeSigning>True</EnableCodeSigning>
  <CreatePackage>true</CreatePackage>
  <CodesignKey>Apple Development: John Smith (AY2GDE9QM7)</CodesignKey>
  <CodesignProvision>MyMauiApp (Ad-hoc)</CodesignProvision>
  <CodesignEntitlements>Platforms\MacCatalyst\Entitlements.plist</CodesignEntitlements>
  <UseHardenedRuntime>true</UseHardenedRuntime>
</PropertyGroup>
```

This example `<PropertyGroup>` adds a condition check, preventing the settings from being processed unless the condition check passes. The condition check looks for two items:

1. The build configuration is set to `Release`.
1. The target framework is set to something containing the text `net8.0-maccatalyst`.
1. The platform is set to `AnyCPU`.

If any of these conditions fail, the settings aren't processed. More importantly, the `<CodesignKey>` and `<CodesignProvision>` settings aren't set, preventing the app from being signed.

After adding the above property group, the app can be published from the command line on a Mac by opening a terminal and navigating to the folder for your .NET MAUI app project. Then, run the following command:

```dotnetcli
dotnet build -f net8.0-maccatalyst -c Release
```

Publishing builds, signs, and packages the app, and then copies the *.pkg* to the *bin/Release/net8.0-maccatalyst/publish/* folder.

## Distribute your app for testing

The *.pkg* can be distributed to users on registered devices, where it can be run by double-clicking on the *.pkg* file to install the app.

For information about safely opening apps on a Mac, see [Open apps safely on your Mac](https://support.apple.com/HT202491) on support.apple.com.

> [!WARNING]
> Sometimes macOS will update any existing app when installing a *.pkg*, which might not be the app in the */Applications* directory.
> This means that if you try to test a *.pkg* locally, macOS might update the existing app in the project's *bin* directory, instead of
> the app in the */Applications* directory. The solution is to delete the project's *bin* directory before installing any *.pkg* files
> (after copying the *.pkg* elsewhere).

## See also

- [Distributing your app to registered devices](https://developer.apple.com/documentation/xcode/distributing-your-app-to-registered-devices) on developer.apple.com
- [Preparing your app for distribution](https://developer.apple.com/documentation/xcode/preparing-your-app-for-distribution) on developer.apple.com
- [Hardened Runtime](https://developer.apple.com/documentation/security/hardened_runtime) on developer.apple.com
