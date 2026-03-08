---
title: "Publish a .NET MAUI Mac Catalyst app for distribution outside the Mac App Store"
description: "Learn how to provision, publish, and notarize a .NET MAUI Mac Catalyst app for distribution outside the Mac App Store."
ms.date: 03/23/2023
ms.custom: sfi-image-nochange
---

# Publish a Mac Catalyst app for distribution outside the Mac App Store

> [!div class="op_single_selector"]
>
> - [Publish an unsigned app](publish-unsigned.md)
> - [Publish for app store distribution](publish-app-store.md)
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)

An alternative to distributing Mac Catalyst apps through the Mac App Store is to distribute them outside the Mac App Store. With this approach, your Mac Catalyst app can be hosted at a location of your choosing for download. Members of the Apple Developer Program and the Apple Developer Enterprise Program can use this distribution approach.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the Mac App Store, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. A distribution provisioning profile contains an App ID and a developer ID application certificate. You'll need to create a developer ID application certificate to identify yourself or your organization, if you don't already have one. In addition, you'll need to create a developer ID installer certificate to sign your app's installer package.

The process for provisioning a .NET MAUI Mac Catalyst app for distribution outside the Mac App Store is as follows:

1. Create two certificate signing requests. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a developer ID application certificate. For more information, see [Create a developer ID application certificate](#create-a-developer-id-application-certificate).
1. Create a developer ID installer certificate. For more information, see [Create a developer ID installer certificate](#create-a-developer-id-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile).
1. Download your provisioning profile. For more information, see [Download your provisioning profile in Xcode](#download-your-provisioning-profile-in-xcode).

> [!IMPORTANT]
> This article shows provisioning for distribution outside the Mac App Store using an Apple Developer Account that's part of the Apple Developer Program. It can also be used for an Apple Developer Account that's part of the Apple Developer Enterprise Program, as the steps required are largely identical.

Then, once provisioning is complete you should prepare your app for publishing, publish it, and notarize it with the following process:

1. Optionally add entitlements to your app. For more information, see [Add entitlements](#add-entitlements).
1. Update the app's *Info.plist* file. For more information, see [Update Info.plist](#update-infoplist).
1. Disable code signature verification for your app. For more information, see [Disable code signature verification](#disable-code-signature-verification).
1. Publish your app using the command line. For more information, see [Publish using the command line](#publish-using-the-command-line).
1. Notarize your app. For more information, see [Notarize your app](#notarize-your-app).

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

Repeat this process to create a second certificate signing request. The first CSR will be used to create a developer ID application certificate. The second will be used to create a developer ID installer certificate.

> [!IMPORTANT]
> You can't use the same certificate signing request to create a developer ID application certificate and a developer ID installer certificate.

## Create a developer ID application certificate

The CSR allows you to generate a developer ID certificate, which confirms your identity. The developer ID certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Developer ID Application** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/developer-id-application-certificate.png" alt-text="Create a Developer ID Application certificate.":::

1. On the **Create a New Certificate** page, select the **G2 Sub-CA** radio button, and then select **Choose File**:

    :::image type="content" source="media/publish-outside-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for a Developer ID Application certificate.":::

1. In the **Choose Files to Upload** dialog, select the first certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-outside-app-store/download-developer-id-application-certificate.png" alt-text="Download your Developer ID Application certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Developer ID Application**:

    :::image type="content" source="media/publish-outside-app-store/keychain-access-developer-id-application-certificate.png" alt-text="Keychain Access showing Developer ID Application certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a developer ID installer certificate

The CSR allows you to generate a developer ID installer certificate, which is required to sign your app's installer package. The installer certificate must be created using the Apple ID for your Apple Developer Account:

1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Developer ID Installer** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/developer-id-installer-certificate.png" alt-text="Create a Developer ID Installer certificate.":::

1. On the **Create a New Certificate** page, select the **G2 Sub-CA** radio button, and then select **Choose File**:

    :::image type="content" source="media/publish-outside-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for a Developer ID Installer certificate.":::

1. In the **Choose Files to Upload** dialog, select the second certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/chosen-certificate.png" alt-text="Continue to generate your installer certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="media/publish-outside-app-store/download-installer-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **Developer ID Installer**:

    :::image type="content" source="media/publish-outside-app-store/keychain-access-installer-certificate.png" alt-text="Keychain Access showing installer certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.

## Create a distribution profile

A distribution provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed for release, so that it can be installed on another Mac. A provisioning profile for distribution outside the Mac App Store contains an App ID and a developer ID application certificate.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

### Create a provisioning profile

Once the App ID has been created, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on Macs.

To create a provisioning profile for distribution outside the Mac App Store:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **Developer ID** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/register-provisioning-profile.png" alt-text="Register a provisioning profile distribution outside the App Store.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/select-app-id.png" alt-text="Select your App ID.":::

1. In the **Generate a Provisioning Profile** page, select the radio button that corresponds to your distribution certificate before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/select-certificate.png" alt-text="Select your distribution certificate.":::

1. In the **Generate a Provisioning Profile** page, enter a name for the provisioning profile before clicking the **Generate** button:

    :::image type="content" source="media/publish-outside-app-store/generate-profile.png" alt-text="Generate the provisioning profile.":::

    > [!NOTE]
    > Make a note of the provisioning profile name, as it will be required when signing your app.

1. In the **Generate a Provisioning Profile** page, optionally click the **Download** button to download your provisioning profile.

    > [!NOTE]
    > It's not necessary to download your provisioning profile now. Instead, you will do this in Xcode.

[!INCLUDE [Download certificates and provisioning profiles in Xcode](../includes/download-profiles.md)]

## Add entitlements

Apple's App Sandbox restricts access to system resources and user data in Mac apps, to contain damage if an app becomes compromised. It can optionally be enabled for Mac Catalyst apps that are distributed outside the Mac App Store.

[!INCLUDE [Add entitlements](../includes/add-entitlements.md)]

## Update Info.plist

Before publishing your app, you should update its *Info.plist* file with additional information.

[!INCLUDE [Update Info.plist](../includes/update-info-plist.md)]

### Declare your app's use of encryption

If your app uses encryption, and you plan to distribute it outside the United States or Canada, it's subject to US export compliance requirements. You can provide information about you app's use of encryption in its *Info.plist* file.

[!INCLUDE [Declare your app's use of encryption](../includes/encryption.md)]

<!-- Todo remove once this bug is fixed: https://github.com/xamarin/xamarin-macios/issues/17829 -->
## Disable code signature verification

Currently, when you attempt to publish a .NET MAUI Mac Catalyst app for distribution outside the Mac App Store, provided you've met the provisioning requirements, you'll receive an error about `codesign` exiting with code 3:

```
/usr/local/share/dotnet/packs/Microsoft.MacCatalyst.Sdk/16.2.1040/tools/msbuild/iOS/Xamarin.Shared.targets(1930,3): error MSB6006: "codesign" exited with code 3. [/Users/JohnDoe/Projects/MyMauiApp/MyMauiApp/MyMauiApp.csproj::TargetFramework=net8.0-maccatalyst]
```

While `codesign` succeeds in signing your app, the `_CodesignVerify` target fails to verify the code signature:

```
test-requirement: code failed to satisfy specified code requirement(s)
```

Because of this failure, a *.pkg* file isn't produced.

Therefore, it's currently necessary to add the following build target to the end of your project file to disable verification of the code signature:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  ...
  <Target Name="_SkipCodesignVerify" BeforeTargets="_CodesignVerify" AfterTargets="_CodesignAppBundle">
    <PropertyGroup>
      <_RequireCodeSigning>false</_RequireCodeSigning>
    </PropertyGroup>
  </Target>
</Project>
```

## Publish using the command line

To publish your Mac Catalyst app from the command line on a Mac, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net8.0-maccatalyst`.                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |
| `-p:MtouchLink`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                       |
| `-p:CreatePackage`           | Set to `true` so that a package (*.pkg*) is created for the app at the end of the build.        |
| `-p:EnableCodeSigning`       | Set to `true` so that code signing is enabled.                                                  |
| `-p:EnablePackageSigning`    | Set to `true` so that the package that's created gets signed.                                   |
| `-p:CodesignKey`             | The name of the code signing key. Set to the name of your distribution certificate, as displayed in Keychain Access. |
| `-p:CodesignProvision`       | The provisioning profile to use when signing the app bundle. |
| `-p:CodesignEntitlements`    | The path to the entitlements file that specifies the entitlements the app requires. Set to `Platforms\MacCatalyst\Entitlements.plist`. |
| `-p:PackageSigningKey`       | The package signing key to use when signing the package. Set to the name of your installer certificate, as displayed in Keychain Access. |
| `-p:RuntimeIdentifier`       | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |
| `-p:UseHardenedRuntime`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the Mac App Store. |

[!INCLUDE [Additional build parameters](../includes/additional-build-parameters.md)]

For example, use the following command to build and sign a *.pkg* on a Mac, for distribution outside the Mac App Store:

```dotnetcli
dotnet publish -f net8.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: John Smith (AY2GDE9QM7)" -p:CodesignProvision="MyMauiApp (Non-App Store)" -p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.plist" -p:PackageSigningKey="Developer ID Installer: John Smith (AY2GDE9QM7)" -p:UseHardenedRuntime=true
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

[!INCLUDE [Publishing output](../includes/publishing-output.md)]

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
| `<EnablePackageSigning>`    | Set to `true` so that the package that's created gets signed.                                   |
| `<MtouchLink>`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                       |
| `<PackageSigningKey>`       | The package signing key to use when signing the package. Set to the name of your installer certificate, as displayed in Keychain Access. |
| `<RuntimeIdentifier>` | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |
| `<UseHardenedRuntime>`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the Mac App Store. |

For a full list of build properties, see [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties).

> [!IMPORTANT]
> Values for these build properties don't have to be provided in the project file. They can also be provided on the command line when you publish the app. This enables you to omit specific values from your project file.

The following example shows a typical property group for building and signing your Mac Catalyst app for distribution outside the Mac App Store:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Release|net8.0-maccatalyst|AnyCPU'">
  <MtouchLink>SdkOnly</MtouchLink>
  <EnableCodeSigning>True</EnableCodeSigning>
  <EnablePackageSigning>true</EnablePackageSigning>
  <CreatePackage>true</CreatePackage>
  <CodesignKey>Developer ID Application: John Smith (AY2GDE9QM7)</CodesignKey>
  <CodesignProvision>MyMauiApp (Non-App Store)</CodesignProvision>
  <CodesignEntitlements>Platforms\MacCatalyst\Entitlements.plist</CodesignEntitlements>
  <PackageSigningKey>Developer ID Installer: John Smith (AY2GDE9QM7)</PackageSigningKey>
  <UseHardenedRuntime>true</UseHardenedRuntime>
</PropertyGroup>
```

[!INCLUDE [Publishing based on a property group](../includes/publishing-property-group.md)]

## Notarize your app

macOS includes a technology called *Gatekeeper*, which helps to ensure that only trusted software runs on a Mac. When a user downloads and opens an installer package, or an app, Gatekeeper verifies that the software is from an identified developer. It does this by checking for a Developer ID certificate, and checking that the software is notarized by Apple to be free of known malicious content and hasn't been altered. Therefore, after provisioning and publishing your app you should submit it to Apple to be notarized. Apple's notary service automatically scans your developer ID-signed app and performs security checks. When notarization succeeds, your software is assigned a ticket to let Gatekeeper know that it's been notarized.

Apps can be submitted to Apple's notary service with the `notarytool` command line tool:

```zsh
xcrun notarytool submit {my_package_filename} --wait --apple-id {my_apple_id} --password {my_app_specific_password} --team-id {my_team_id}
```

An example of using the `notarytool` tool to sign a .NET MAUI Mac Catalyst *.pkg* file is shown below:

```zsh
xcrun notarytool submit MyMauiApp-1.0.pkg --wait --apple-id johm.smith@provider.com --password fqtk-cprr-gqdt-bvyo --team-id AY2GDE9QM7
```

The `wait` flag tells `notarytool` to exit only after the notary service finishes processing the submission, therefore removing the need to poll the service for its status. The `apple-id`, `password`, and `team-id` arguments are used to supply your App Store Connect credentials. Because App Store Connect requires two-factor authentication, you must create an app-specific password for `notarytool`. For information about creating an app-specific password, see [Sign in to apps with your Apple ID using app-specific passwords](https://support.apple.com/en-us/HT204397) on developer.apple.com.

After you submit your app to Apple, the notarization process typically takes less than an hour.

> [!NOTE]
> Notarization completes for most apps within 5 minutes. For information about avoiding long response times from the notary service, see [Avoid long notarization response times and size limits](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution/customizing_the_notarization_workflow?) on developer.apple.com.

Provided that notarization succeeds, the notary service generates a ticket for the signed installer package and the app bundle inside it.

For more information about notarization, see [Notarizing macOS software before distribution](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution). For other approaches to submitting your app to Apple's notary service, see [Upload your app to the notarization service](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution/customizing_the_notarization_workflow) on developer.apple.com. For information about resolving common notarization issues, see [Resolving common notarization issues](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution/resolving_common_notarization_issues).

### Staple the ticket to your app

Notarization produces a ticket for the signed installer package and the app bundle inside it, that tell Gatekeeper that your app is notarized. Once notarization completes, when users attempt to run your app on macOS 10.14 or later, Gatekeeper locates the ticket online.

After notarization has succeeded, you should attach the ticket to your app using the `stapler` tool. This ensures that Gatekeeper can find the ticket even when a network connection isn't available. Run the following command to attach the ticket to your *.pkg*:

```zsh
xcrun stapler staple {filename}.pkg
```

The `stapler` tool retrieves the ticket and attaches it to the installer package. You'll receive a message telling you that the staple and validate action worked, provided that stapling succeeds.

### Validate notarization

If you want to validate notarization, you can do so with the following command:

```zsh
xcrun stapler validate mypackage.pkg
```

## Distribute your app

The signed and notarized *.pkg* can be safely distributed outside the Mac App Store at a location of your choosing, such as a web server.

For information about safely opening apps on a Mac, see [Open apps safely on your Mac](https://support.apple.com/HT202491) on support.apple.com.

> [!WARNING]
> Sometimes macOS will update any existing app when installing a *.pkg*, which might not be the app in the */Applications* directory.
> This means that if you try to test a *.pkg* locally, macOS might update the existing app in the project's *bin* directory, instead of
> the app in the */Applications* directory. The solution is to delete the project's *bin* directory before installing any *.pkg* files
> (after copying the *.pkg* elsewhere).

## See also

- [Preparing your app for distribution](https://developer.apple.com/documentation/xcode/preparing-your-app-for-distribution) on developer.apple.com
- [Hardened Runtime](https://developer.apple.com/documentation/security/hardened_runtime) on developer.apple.com
- [Customizing the notarization workflow](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution/customizing_the_notarization_workflow) on developer.apple.com
