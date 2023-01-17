---
title: "Publish a .NET MAUI app for iOS"
description: "Learn how to package and publish a iOS .NET MAUI app."
ms.date: 06/13/2022
---

# Publish a .NET MAUI app for iOS

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for iOS](index.md)
> - [Publish for macOS](../../macos/deployment/overview.md)
> - [Publish for Windows](../../windows/deployment/overview.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for iOS, you generate an *.ipa* file. An *.ipa* file is an iOS app archive file that stores an iOS app. With just a few configuration changes to your project, your app can be packaged for distribution.

Publishing a .NET MAUI app for iOS builds on top of Apple's provisioning process, which requires you to have:

- Created an Apple ID. For more information, see [Create Your Apple ID](https://appleid.apple.com/account).
- Enrolled your Apple ID in the Apple Developer Program, which you have to pay to join. Enrolling in the Apple Developer Program enables you to create a *provisioning profile*, which contains code signing information. For more information, see [Apple Developer Program](https://developer.apple.com/programs/).
- A Mac on which you can build your app.

The process for publishing a .NET MAUI iOS app is as follows:

1. Create a certificate signing request on your Mac.
1. Create a distribution certificate in your Apple Developer Account.
1. Create a distribution profile in your Apple Developer Account.
1. Download provisioning profiles on your Mac.
1. Add any required entitlements to your app.
1. Add the code signing data to your app project.
1. Validate package settings.
1. Connect Visual Studio 2022 to a Mac build host.
1. Publish your app using .NET CLI.

> [!IMPORTANT]
> Blazor Hybrid apps require a WebView on the host platform. For more information, see [Keep the Web View current in deployed Blazor Hybrid apps](/aspnet/core/blazor/hybrid/security/security-considerations#keep-the-web-view-current-in-deployed-apps).

## Create a certificate signing request

To sign a .NET MAUI iOS app you must first create a certificate signing request (CSR) in Keychain Access on a Mac. For more information, see [Create a certificate signing request](app-store.md#create-a-certificate-signing-request).

## Create a distribution certificate

The CSR allows you to generate a distribution certificate, which will be used to confirm your identity. The distribution certificate must be created using the Apple ID for your Apple Developer Account. For more information, see [Create a distribution certificate](app-store.md#create-a-distribution-certificate).

## Create a distribution profile

To publish a .NET MAUI iOS app, you'll need to build a *Distribution Provisioning Profile* specific to it. This profile enables the app to be digitally signed for release so that it can be installed on an iOS device. A distribution provisioning profile contains an App ID and a distribution certificate. For more information, see [Create a distribution profile](app-store.md#create-a-distribution-profile).

## Download the provisioning profile on your Mac build host

After creating the provisioning profile, it must be added to your Mac build host. The profile can be downloaded to your Mac build host with the following steps:

1. On your Mac, launch Xcode.
1. In Xcode, select the **Xcode > Preferences...** menu item.
1. In the **Preferences** dialog, select the **Accounts** tab.
1. In the **Accounts** tab, click the **+** button to add your Apple Developer Account to Xcode:

    :::image type="content" source="media/overview/accounts-dialog.png" alt-text="Xcode Accounts dialog in preferences.":::

1. In the account type popup, select **Apple ID** and then click the **Continue** button:

    :::image type="content" source="media/overview/account-type.png" alt-text="Xcode select the type of account you'd like to add popup.":::

1. In the sign in popup, enter your Apple ID and click the **Next** button.
1. In the sign in popup, enter your Apple ID password and click the **Next** button:

    :::image type="content" source="media/overview/sign-in.png" alt-text="Xcode Apple account sign-in.":::

1. In the **Accounts** tab, click the **Manage Certificates...** button to ensure that your distribution certificate has been downloaded.
1. In the **Accounts** tab, click the **Download Manual Profiles** button to download your provisioning profiles:

    :::image type="content" source="media/overview/account-details.png" alt-text="Xcode Apple Developer Program account details.":::

1. Wait for the download to complete and then close Xcode.

## Add entitlements to your app

In iOS, apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. *Entitlements* are used to request the expansion of the sandbox to give your app additional capabilities. Any entitlements used by your app must be specified in an entitlements file. For more information about entitlements, see [Entitlements](~/ios/entitlements.md).

## Add code signing data to your app project

There are project-level settings you must set to sign your iOS app with its provisioning profile. These settings are configured in a `<PropertyGroup>` node:

- `<RuntimeIdentifier>` – the runtime identifier (RID) for the project. Set to `ios-arm64`.
- `<CodesignKey>` – the name of the distribution certificate you installed into Keychain Access on your Mac build host.
- `<CodesignProvision>` – the provisioning profile name. This is the name you entered in the Apple Developer portal when creating your provisioning profile.
- `<CodesignEntitlements>` – the name of the entitlements file. Set to `Entitlements.plist`. This setting need only be specified if you're using entitlements.
- `<ArchiveOnBuild>` – a boolean value that indicates whether to build the app package. Set to `true`.
- `<TcpPort>` – the TCP port on which to communicate with your Mac build host. Set to `58181`.
- `<ServerAddress>` – the IP address of your Mac build host.
- `<ServerUser>` – the username to use when logging into your Mac build host. Use your system username rather than your full name.
- `<ServerPassword>` – the password for the username used to log into the Mac build host.
- `<_DotNetRootRemoteDirectory>` – the folder on the Mac build host that contains the .NET SDK. Set to `/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/`.

> [!IMPORTANT]
> Values for these settings don't have to be provided in the project file. They can also be provided on the command line when you publish the app. This enables you to omit specific values from your project file. For example, values for `<ServerAddress>`, `<ServerUser>`, `<ServerPassword>`, and `<_DotNetRootRemoteDirectory>` will typically be provided on the command line for security reasons. An example of this can be found in the [Publish](#publish) section.

The following example shows a typical property group for building and signing your iOS app with its provisioning profile:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
  <RuntimeIdentifier>ios-arm64</RuntimeIdentifier>
  <CodesignKey>iPhone Distribution: John Smith (AY2GDE9QM7)</CodesignKey>
  <CodesignProvision>MyMauiApp</CodesignProvision>
  <ArchiveOnBuild>true</ArchiveOnBuild>
  <TcpPort>58181</TcpPort>
</PropertyGroup>
```

This example `<PropertyGroup>` adds a condition check, preventing the settings from being processed unless the condition check passes. The condition check looks for two items:

1. The target framework is set to something containing the text `-ios`.
1. The build configuration is set to `Release`.

If either of these conditions fail, the settings aren't processed. More importantly, the `<CodesignKey>` and `<CodesignProvision>` settings aren't set, preventing the app from being signed.

## Validate package settings

Every iOS app specifies a unique package identifier and a version. These identifiers are generally set in your project file, from where they are copied to your *Info.plist* file that's located in your project folder at _Platforms\\iOS\\Info.plist_.

Your project file must declare `<ApplicationId>` and `ApplicationVersion` within a `<PropertyGroup>` node. These items should have been generated for you when the project was created. Just validate that they exist and are set to valid values:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <!-- other settings -->

    <PropertyGroup>
        <ApplicationId>com.companyname.mymauiapp</ApplicationId>
        <ApplicationVersion>1</ApplicationVersion>
    </PropertyGroup>

</Project>
```

> [!TIP]
> Some settings are available in the **Project Properties** editor in Visual Studio to change values. Right-click on the project in the **Solution Explorer** pane and choose **Properties**. For more information, see [Project configuration in .NET MAUI](../../deployment/visual-studio-properties.md).

The following table describes how each project setting maps to the *Info.plist* file:

| Project setting | Info.plist manifest field |
| --- | --- |
| `ApplicationId` | Bundle Identifier |
| `ApplicationVersion` | Build |

## Connect Visual Studio 2022 to your Mac build host

Building native iOS apps using .NET MAUI requires access to Apple's build tools, which only run on a Mac. Because of this, Visual Studio 2022 must connect to a network-accessible Mac to build .NET MAUI iOS apps. For more information, see [Pair to Mac for iOS development](../pair-to-mac.md).

> [!NOTE]
> The first time Pair to Mac logs into a Mac build host from Visual Studio 2022, it sets up SSH keys. With these keys, future logins will not require a username or password.

## Publish

At this time, publishing is only supported through the .NET command line interface.

To publish your app, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-ios` or `net7.0-ios`.                                    |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                    |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

In addition, the following common parameters can be specified on the command line if they aren't provided in a `<PropertyGroup>` in your project file:

| Parameter                    | Value                                                                                           |
|------------------------------|-------------------------------------------------------------------------------------------------|
| `/p:RuntimeIdentifier` | The runtime identifier (RID) for the project. Use `ios-arm64`. |
| `/p:CodesignKey` | The name of the distribution certificate you installed into Keychain Access on your Mac build host. |
| `/p:CodesignProvision` | The provisioning profile name. |
| `/p:CodesignEntitlements` | The name of the entitlements file. Use *Entitlements.plist*. |
| `/p:ArchiveOnBuild` | A boolean value that indicates whether to produce the archive. Use `true` to produce the *.ipa*. |
| `/p:TcpPort` | The TCP port to use to communicate with the Mac build host, which is 58181. |
| `/p:ServerAddress` | The IP address of the Mac build host. |
| `/p:ServerUser` | The username to use when logging into the Mac build host. Use your system username rather than your full name. |
| `/p:ServerPassword` | The password for the username used to log into the Mac build host. |
| `/p:_DotNetRootRemoteDirectory` | The folder on the Mac build host that contains the .NET SDK. Use `/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/`. |

> [!IMPORTANT]
> Values for these parameters don't have to be provided on the command line. They can also be provided in the project file. For more information, see [Add code signing data to your app project](#add-code-signing-data-to-your-app-project).

::: moniker range="=net-maui-6.0"

For example, use the following command to create an *.ipa*:

```console
dotnet publish -f:net6.0-ios -c:Release /p:ServerAddress={macOS build host IP address} /p:ServerUser={macOS username} /p:ServerPassword={macOS password} /p:TcpPort=58181 /p:ArchiveOnBuild=true /p:_DotNetRootRemoteDirectory=/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/
```

> [!NOTE]
> If the `ServerPassword` parameter is omitted from a command line build invocation, Pair to Mac attempts to log in to the Mac build host using the saved SSH keys.

Publishing builds the app, and then copies the *.ipa* to the *bin\\Release\\net6.0-ios\\ios-arm64\\publish* folder.

::: moniker-end

::: moniker range="=net-maui-7.0"

For example, use the following command to create an *.ipa*:

```console
dotnet publish -f:net7.0-ios -c:Release /p:ServerAddress={macOS build host IP address} /p:ServerUser={macOS username} /p:ServerPassword={macOS password} /p:TcpPort=58181 /p:ArchiveOnBuild=true /p:_DotNetRootRemoteDirectory=/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/
```

> [!NOTE]
> If the `ServerPassword` parameter is omitted from a command line build invocation, Pair to Mac attempts to log in to the Mac build host using the saved SSH keys.

Publishing builds the app, and then copies the *.ipa* to the *bin\\Release\\net7.0-ios\\ios-arm64\\publish* folder.

::: moniker-end

During the publishing process it maybe necessary to allow `codesign` to run on your paired Mac:

:::image type="content" source="media/overview/codesign.png" alt-text="Allow codesign to sign your app on your paired Mac.":::

The *.ipa* file can then be uploaded to the App Store using App Store Connect. To learn how to use App Store Connect, see [App Store Connect workflow](https://help.apple.com/app-store-connect/#/dev300c2c5bf).

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

## See also

- [GitHub discussion and feedback: .NET MAUI iOS target publishing/archiving](https://github.com/dotnet/maui/issues/4397)
