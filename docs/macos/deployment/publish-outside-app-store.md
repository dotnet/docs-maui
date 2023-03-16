---
title: "Publish a .NET MAUI Mac Catalyst app for distribution outside the App Store"
description: "Learn how to provision and publish a .NET MAUI Mac Catalyst app for distribution outside the App Store."
ms.date: 03/16/2023
---

# Publish a Mac Catalyst app for distribution outside the App Store

An alternative to distributing Mac Catalyst apps through the App Store is to distribute them outside the App Store. With this approach, your Mac Catalyst app can be hosted on a server for download.

Distributing a Mac Catalyst app requires that the app is provisioned using a *provisioning profile*. Provisioning profiles are files that contain code signing information, as well as the identity of the app and its intended distribution mechanism.

To distribute a .NET Multi-platform App UI (.NET MAUI) Mac Catalyst app outside the App Store, you'll need to build a *distribution provisioning profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on Macs. A distribution provisioning profile contains an App ID and a developer ID application certificate. You'll need to create a developer ID application certificate to identify yourself or your organization, if you don't already have one. In addition, you'll need to create developer ID installer certificate to sign your app's installer package.

The process for provisioning a Mac Catalyst app for distribution outside the App Store is as follows:

1. Create a certificate signing request. For more information, see [Create a certificate signing request](#create-a-certificate-signing-request).
1. Create a developer ID application certificate. For more information, see [Create a developer ID application certificate](#create-a-developer-id-application-certificate).
1. Create a developer ID installer certificate. For more information, see [Create a developer ID installer certificate](#create-a-developer-id-installer-certificate).
1. Create an App ID. For more information, see [Create an App ID](#create-an-app-id).
<!-- 1. Configure the App ID. For more information, see [Configure the App ID](#configure-the-app-id). IS THIS REALLY REQUIRED???
1. Create a provisioning profile. For more information, see [Create a provisioning profile](#create-a-provisioning-profile). -->

After provisioning and publishing your app, it must be notarized SOMETHING ABOUT GATEKEEPER HERE. MORE WAFFLE HERE ABOUT WHAT YOU DO AFTER PROVISIONING AND PUBLISHING.

[!INCLUDE [Create a certificate signing request](../includes/certificate-signing-request.md)]

## Create a developer ID application certificate

The CSR allows you to generate a developer ID certificate, which confirms your identity. The developer ID certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Developer ID Application** radio button before selecting the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/developer-id-application-certificate.png" alt-text="Create a Developer ID Application certificate.":::

1. On the **Create a New Certificate** page, select the **G2 Sub-CA** radio button, and then select **Choose File**:

    :::image type="content" source="media/publish-outside-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for a Developer ID Application certificate.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
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

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
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

A distribution provisioning profile enables your .NET MAUI Mac Catalyst app to be digitally signed for release, so that it can be installed on another Mac. A distribution provisioning profile contains an App ID and a developer ID application certificate.

[!INCLUDE [Create an App ID](../includes/create-app-id.md)]

<!-- TODO: Does the App ID need configuring to use the Mac Catalyst entitlement?
[!INCLUDE [Configure the App ID](../includes/configure-app-id.md)] -->

### Create a provisioning profile

Once the App ID has been created and configured, you should create a distribution provisioning profile. This profile enables the app to be digitally signed for release so that it can be installed on Macs.

To create a provisioning profile for distribution outside the App Store:

1. In the Certificates, Identifiers & Profiles page of your Apple Developer Account, select the **Profiles** tab.
1. In the **Profiles** tab, click the **+** button to create a new profile.
1. In the **Register a New Provisioning Profile** page, select the **Developer ID** radio button before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/register-provisioning-profile.png" alt-text="Register a provisioning profile distribution outside the App Store.":::

1. In the **Generate a Provisioning Profile** page, select the **Mac Catalyst** radio button. Then, in the **App ID** drop-down, select the App ID that you previously created before clicking the **Continue** button:

    :::image type="content" source="media/publish-outside-app-store/select-app-id.png" alt-text="Select your App ID.":::

    > [!NOTE]
    > The App ID will be in the **Enabled App IDs with an associated application identifier** section.

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

Apple's App Sandbox restricts access to system resources and user data in macOS apps, to contain damage if an app becomes compromised. It can optionally be enabled for Mac Catalyst apps that are distributed outsidethe App Store. This can be accomplished by adding an *Entitlements.plist* file to the *Platforms/MacCatalyst* folder of your .NET MAUI app project:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
	<dict>
		<key>com.apple.security.app-sandbox</key>
		<true/>
	</dict>
</plist>
```

The App Sandbox entitlement is defined using the `com.apple.security.app-sandbox` key, of type `Boolean`. For information about App Sandbox, see [Protecting user data with App Sandbox](https://developer.apple.com/documentation/security/app_sandbox/protecting_user_data_with_app_sandbox) on developer.apple.com. For information about the App Sandbox entitlement, see [App Sandbox Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_app-sandbox).

If you use the App Sandbox and your app opens outgoing network connections, you'll also need to add the `com.apple.security.network.client` key, of type `Boolean`, to your *Entitlements.plist* file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
	<dict>
		<key>com.apple.security.app-sandbox</key>
		<true/>
		<key>com.apple.security.network.client</key>
		<true/>
	</dict>
</plist>
```

For information about the outgoing network connections entitlement, see [com.apple.security.network.client](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_network_client) on developer.apple.com.

## Update Info.plist

Before publishing your app, you should update its *Info.plist* file with additional information.

### Specify the user interface idiom

A Mac Catalyst app can run in the iPad or Mac user interface idiom:

- The iPad user interface idiom tells macOS to scale the app's user interface to match the Mac display environment while preserving iPad-like appearance.
- The Mac user interface idiom doesn't scale the app's user interface to match the Mac display environment. Some controls change their size and appearance, and interacting with them feels identical to interacting with `AppKit` controls.

By default, .NET MAUI Mac Catalyst apps use the iPad user interface idiom. If this is your desired behavior, ensure that the app's *Info.plist* file only specifies 2 as the value of the `UIDeviceFamily` key:

```xml
<key>UIDeviceFamily</key>
<array>
  <integer>2</integer>
</array>
```

To adopt the Mac user interface idiom, update the app's *Info.plist* file to specify 6 as the value of the `UIDeviceFamily` key:

```xml
<key>UIDeviceFamily</key>
<array>
  <integer>6</integer>
</array>
```

For more information about Mac Catalyst user interface idioms, see [Choosing a user interface idiom for your Mac app](https://developer.apple.com/documentation/uikit/mac_catalyst/choosing_a_user_interface_idiom_for_your_mac_app?language=objc).

### Set the default language and region for the app

Set the `CFBundleDevelopmentRegion` key in your app's *Info.plist* to a `String` that represents the localization native development region:

```xml
<key>CFBundleDevelopmentRegion</key>
<string>en</string>
```

The value of the key should be a language designator, with an optional region designator. For more information, see [CFBundleDevelopmentRegion](https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundledevelopmentregion) on developer.apple.com.

### Set the copyright key

Set the `NSHumanReadableCopyright` key in your app's *Info.plist* to a `String` that represents the human-readable copyright notice for the app:

```xml
<key>NSHumanReadableCopyright</key>
<string>My Maui App © 2023</string>
```

For more information, see [NSHumanReadableCopyright](https://developer.apple.com/documentation/bundleresources/information_property_list/nshumanreadablecopyright) on developer.apple.com.

### Declare your app's use of encryption

If your app uses encryption, and you plan to distribute it outside the United States or Canada, it's subject to US export compliance requirements.


Every time you submit a version of your app to App Store Connect, it undergoes an encryption export regulations compliance review. To avoid App Store Connect asking you questions to guide you through the review, you can provide the required information in your app's *Info.plist* file.

To declare your app's use of encryption, add the `ITSAppUsesNonExemptEncryption` key to your app's *Info.plist* with a `Boolean` value that indicates whether it uses encryption:

```xml
<key>ITSAppUsesNonExemptEncryption</key>
<false/>
```

For more information, see [Complying with Encryption Export Regulations](https://developer.apple.com/documentation/security/complying_with_encryption_export_regulations) on developer.apple.com.

<!-- Todo once this bug is fixed:
## Configure your project file

Currently, when you attempt to publish a .NET MAUI Mac Catalyst app for distribution outside the App Store, provided you've met the provisioning requirements, you'll receive an error about `codesign` exiting with code 3:

```
/usr/local/share/dotnet/packs/Microsoft.MacCatalyst.Sdk/16.2.1040/tools/msbuild/iOS/Xamarin.Shared.targets(1930,3): error MSB6006: "codesign" exited with code 3. [/Users/davidbritch/Projects/MyMauiApp/MyMauiApp/MyMauiApp.csproj::TargetFramework=net7.0-maccatalyst]
```

While `codesign` succeeds in signing your app, it fails to verify the code signature which in turn stops the production of the *.pkg* file.

```
test-requirement: code failed to satisfy specified code requirement(s)
```

Therefore, it's currently necessary to add the following build target to your project file to disable verification of the code signature:

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
-->

## Publish using the command line

To publish your Mac Catalyst app from the command line on a Mac, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-maccatalyst` or `net7.0-maccatalyst`.                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |
| `/p:MtouchLink`              | The link mode for the project, which can be `None`, `SdkOnly`, or `Full`.                       |
| `/p:CreatePackage`           | Set to `true` so that a package (*.pkg*) is created for the app at the end of the build.        |
| `/p:EnableCodeSigning`       | Set to `true` so that code signing is enabled.                                                  |
| `/p:EnablePackageSigning`    | Set to `true` so that the package that's created gets signed.                                   |
| `/p:CodesignKey`             | The name of the code signing key. Set to the name of your distribution certificate, as displayed in Keychain Access. |
| `/p:CodesignProvision`       | The provisioning profile to use when signing the app bundle. |
| `/p:CodesignEntitlements`    | The path to the entitlements file that specifies the entitlements the app requires. Set to `Platforms\MacCatalyst\Entitlements.plist`. |
| `/p:PackageSigningKey`       | The package signing key to use when signing the package. Set to the name of your installer certificate, as displayed in Keychain Access. |
| `/p:UseHardenedRuntime`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the App Store. |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

Additional build parameters can be specified on the command line, if they aren't provided in a `<PropertyGroup>` in your project file. The following table lists some of the common parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `/p:ApplicationTitle` | The user-visible name for the app. |
| `/p:ApplicationId` | The unique identifier for the app, such as `com.companyname.mymauiapp`. |
| `/p:ApplicationVersion` | The version of the build that identifies an iteration of the app. |
| `/p:ApplicationDisplayVersion` | The version number of the app. |
| `/p:RuntimeIdentifier` | The runtime identifier (RID) for the project. Release builds of .NET MAUI Mac Catalyst apps default to using `maccatalyst-x64` and `maccatalyst-arm64` as runtime identifiers, to support universal apps. To support only a single architecture, specify `maccatalyst-x64` or `maccatalyst-arm64`. |

For a full list of build properties, see [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties).

> [!IMPORTANT]
> Values for all of these parameters don't have to be provided on the command line. They can also be provided in the project file. When a parameter is provided on the command line and in the project file, the command line parameter takes precedence. For more information about providing build properties in your project file, see [Define build properties in your project file](#define-build-properties-in-your-project-file).

For example, use the following command to build and sign a *.pkg* on a Mac, for distribution outside the App Store:

```dotnetcli
dotnet publish -f:net7.0-maccatalyst -c:Release /p:MtouchLink=SdkOnly /p:CreatePackage=true /p:EnableCodeSigning=true /p:EnablePackageSigning=true /p:CodesignKey="Developer ID Application: John Smith (AY2GDE9QM7)" /p:CodesignProvision="MyMauiApp (Non-App Store)" /p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.plist" /p:PackageSigningKey="Developer ID Installer: John Smith (AY2GDE9QM7)" /p:UseHardenedRuntime=true
```

Publishing builds, signs, and packages the app, and then copies the *.pkg* to the *bin/Release/net7.0-maccatalyst/publish/* folder. If you publish the app using only a single architecture, it will be published to the *bin/Release/net7.0maccatalyst/{arechitecture}/publish/* folder.

During the signing process it maybe necessary to enter your login password and allow `codesign` and `productbuild` to run:

:::image type="content" source="media/codesign.png" alt-text="Allow codesign to sign your app on your Mac.":::
:::image type="content" source="media/productbuild.png" alt-text="Allow productbuild to sign your app on your Mac.":::

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

<!-- Todo: It's possible to re-sign an existing app bundle with a different certificate to change the distribution channel -->

## Define build properties in your project file

An alternative to specifying build parameters on the command line is to specify them in your project file in a `<PropertyGroup>`. The following table lists some of the common build properties:

| Parameter                    | Value                                                                                           |
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
| `<UseHardenedRuntime>`      | Set to `true` to enable the hardened runtime, which is required for Mac Catalyst apps that are distributed outside of the App Store. |

For a full list of build properties, see [Project file properties](https://github.com/xamarin/xamarin-macios/wiki/Project-file-properties).

> [!IMPORTANT]
> Values for these build properties don't have to be provided in the project file. They can also be provided on the command line when you publish the app. This enables you to omit specific values from your project file.

The following example shows a typical property group for building and signing your Mac Catalyst app, for distribution outside the App Store, with its provisioning profile:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Release|net7.0-maccatalyst|AnyCPU'">
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

This example `<PropertyGroup>` adds a condition check, preventing the settings from being processed unless the condition check passes. The condition check looks for two items:

1. The build configuration is set to `Release`.
1. The target framework is set to something containing the text `net7.0-maccatalyst`.
1. The platform is set to `AnyCPU`.

If any of these conditions fail, the settings aren't processed. More importantly, the `<CodesignKey>`, `<CodesignProvision>`, and `<PackageSigningKey>` settings aren't set, preventing the app from being signed.

After adding the above property group, the app can be published from the command line on a Mac by opening a terminal and navigating to the folder for your .NET MAUI app project. Then, run the following command:

```dotnetcli
dotnet build -f:net7.0-maccatalyst -c:Release
```

Publishing builds, signs, and packages the app, and then copies the *.pkg* to the *bin/Release/net7.0-maccatalyst/publish/* folder.

## Notarization

## See also

- [Preparing your app for distribution](https://developer.apple.com/documentation/xcode/preparing-your-app-for-distribution) on developer.apple.com
- [Hardened Runtime](https://developer.apple.com/documentation/security/hardened_runtime) on developer.apple.com
