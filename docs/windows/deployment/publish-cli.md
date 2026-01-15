---
title: "Use the CLI to publish packaged apps for Windows"
description: "Learn how to package and publish a packaged Windows .NET MAUI app with the dotnet publish command."
ms.date: 01/15/2026
ms.custom: sfi-image-nochange
---

# Publish a packaged .NET MAUI app for Windows with the CLI

> [!div class="op_single_selector"]
>
> - [Publish an unpackaged app using the command line](publish-unpackaged-cli.md)
> - [Publish a packaged app using Visual Studio](publish-visual-studio-folder.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for Windows, you can publish the app and its dependencies to a folder for deployment to another system. You can also package the app into an MSIX package, which has numerous benefits for the users installing your app. For more information about the benefits of MSIX, see [What is MSIX?](/windows/msix/overview)

## Create a signing certificate

You must use a signing certificate for use in publishing your app. This certificate is used to sign the MSIX package. The following steps demonstrate how to create and install a self-signed certificate with PowerShell:

> [!NOTE]
> When you create and use a self-signed certificate only users who install and trust your certificate can run your app. This is easy to implement for testing but it may prevent additional users from installing your app. When you are ready to publish your app we recommend that you use a certificate issued by a trusted source. This system of centralized trust helps to ensure that the app ecosystem has levels of verification to protect users from malicious actors.

01. Open a PowerShell terminal and navigate to the directory with your project.
01. Use the [`New-SelfSignedCertificate`](/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2019-ps&preserve-view=true) command to generate a self-signed certificate.

    The `<PublisherName>` value is displayed to the user when they install your app, supply your own value and omit the `< >` characters. You can set the `FriendlyName` parameter to any string of text you want.

    ```powershell
    New-SelfSignedCertificate -Type Custom `
                              -Subject "CN=<PublisherName>" `
                              -KeyUsage DigitalSignature `
                              -FriendlyName "My temp dev cert" `
                              -CertStoreLocation "Cert:\CurrentUser\My" `
                              -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
    ```

01. Use the following PowerShell command to query the certificate store for the certificate that was created:

    ```powershell
    Get-ChildItem "Cert:\CurrentUser\My" | Format-Table Thumbprint, Subject, FriendlyName
    ```

    You should see results similar to the following output:

    ```text
    Thumbprint                               Subject                                  FriendlyName
    ----------                               -------                                  ------------
    DE8B962E7BF797CB48CCF66C8BCACE65C6585E2F CN=1f23fa36-2a2f-475e-a69e-3a14fe56ed4
    A6CA34FD0BA6B439787391F51C87B1AD0C9E7FAE CN=someone@microsoft.com
    94D93DBC97D4F7E4364A215F15C6ACFEFC71E569 CN=localhost                             ASP.NET Core HTTPS development certificate
    F14211566DACE867DA0BF9C2F9C47C01E3CF1D9B CN=john
    568027317BE8EE5E6AACDE5079D2DE76EC46EB88 CN=e1f823e2-4674-03d2-aaad-21ab23ad84ae
    DC602EE83C95FEDF280835980E22306067EFCA96 CN=John Smith, OU=MSE, OU=Users, DC=com
    07AD38F3B646F5AAC16F2F2570CAE40F4842BBE0 CN=Contoso                               My temp dev cert
    ```

01. The **Thumbprint** of your certificate will be used later, so copy it to your clipboard. It's the **Thumbprint** value whose entry matches the **Subject** and **FriendlyName** of your certificate.

For more information, see [Create a certificate for package signing](/windows/msix/package/create-certificate-package-signing).

<!-- markdownlint-disable MD044 -->
<!-- The pfx command line options don't seem to work yet -->
<!--
### Optionally export a PFX file

You can use the thumbprint of the certificate to sign your package later, or you export the certificate to a Personal Information Exchange (PFX) file. The PFX file is secured with a password and can be referenced by your project. If you want to do signing from an automated build pipeline, you will need to have this file. To export the certificate as a PFX file, do the following:

01. In the same PowerShell terminal session in the [Create a signing certificate](#create-a-signing-certificate) section, create a security password and assign it to the `$password` variable:

    Replace `my-password` with your own plain-text password.

    ```powershell
    $password = ConvertTo-SecureString -String "my-password" -Force -AsPlainText
    ```

01. Export the certificate with the [`Export-PfxCertificate`](/powershell/module/pki/export-pfxcertificate?view=windowsserver2019-ps) command. The `-cert` parameter is set to the path to the certificate store and the thumbprint of your certificate.

    ```powershell
    Export-PfxCertificate -cert "Cert:\CurrentUser\My\07AD38F3B646F5AAC16F2F2570CAE40F4842BBE0" -FilePath .\app.pfx -Password $password
    ```
-->
<!-- markdownlint-enable MD044 -->

## Configure the project build settings

The project file is a good place to put Windows-specific build settings. You may not want to put some settings into the project file, such as passwords. The settings described in this section can be passed on the command line with the `-p:name=value` format. If the setting is already defined in the project file, a setting passed on the command line overrides the project setting.

Add the following `<PropertyGroup>` node to your project file. This property group is only processed when the target framework is Windows and the configuration is set to `Release`. This config section runs whenever a build or publish in `Release` mode.

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows' and '$(Configuration)' == 'Release'">
    <AppxPackageSigningEnabled>true</AppxPackageSigningEnabled>
    <PackageCertificateThumbprint>AA11BB22CC33DD44EE55FF66AA77BB88CC99DD00</PackageCertificateThumbprint>
</PropertyGroup>
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows' and '$(RuntimeIdentifierOverride)' != ''">
    <RuntimeIdentifier>$(RuntimeIdentifierOverride)</RuntimeIdentifier>
</PropertyGroup>
```

<!-- Place in PropertyGroup above once pfx export works: <PackageCertificateKeyFile>myCert.pfx</PackageCertificateKeyFile> <!-- Optional if you want to use the exported PFX file -->

Replace the `<PackageCertificateThumbprint>` property value with the certificate thumbprint you previously generated. Alternatively, you can remove this setting from the project file and provide it on the command line. For example: `-p:PackageCertificateThumbprint=AA11BB22CC33DD44EE55FF66AA77BB88CC99DD00`.

The second `<PropertyGroup>` in the example is required to work around a bug in the Windows SDK. For more information about the bug, see [WindowsAppSDK Issue #3337](https://github.com/microsoft/WindowsAppSDK/issues/3337).

## Publish

::: moniker range="<=net-maui-9.0"

To publish your app, open a **Developer Command Prompt for Visual Studio** terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                               |
|------------------------------|-------------------------------------------------------------------------------------|
| `-f` | The target framework, which is `net8.0-windows{version}`. This value is a Windows TFM, such as `net8.0-windows10.0.19041.0`. Ensure that this value is identical to the value in the `<TargetFrameworks>` node in your *.csproj* file.           |
| `-c`                 | The build configuration, which is `Release`.                                   |
| `-p:RuntimeIdentifierOverride=win10-x64`<br>- or -<br>`-p:RuntimeIdentifierOverride=win10-x86` | Avoids the bug detailed in [WindowsAppSDK Issue #3337](https://github.com/microsoft/WindowsAppSDK/issues/3337). Choose the `-x64` or `-x86` version of the parameter based on your target platform. |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

For example:

```console
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Publishing builds and packages the app, copying the signed package to the _bin\\Release\\net8.0-windows10.0.19041.0\\win10-x64\\AppPackages\\\<appname>\\_ folder. \<appname> is a folder named after both your project and version. In this folder, there's an _msix_ file, and that's the app package.

::: moniker-end

::: moniker range=">=net-maui-10.0"

To publish your app, open a **Developer Command Prompt for Visual Studio** terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                               |
|------------------------------|-------------------------------------------------------------------------------------|
| `-f` | The target framework, which is `net10.0-windows{version}`. This value is a Windows TFM, such as `net10.0-windows10.0.19041.0`. Ensure that this value is identical to the value in the `<TargetFrameworks>` node in your *.csproj* file.           |
| `-c`                 | The build configuration, which is `Release`.                                   |
| `-p:RuntimeIdentifierOverride=win-x64`<br>- or -<br>`-p:RuntimeIdentifierOverride=win-x86` | Avoids the bug detailed in [WindowsAppSDK Issue #3337](https://github.com/microsoft/WindowsAppSDK/issues/3337). Choose the `-x64` or `-x86` version of the parameter based on your target platform. |

> [!IMPORTANT]
> Starting with .NET 10, version-specific Windows RuntimeIdentifiers such as `win10-x64` and `win10-x86` are no longer supported. You must use portable RuntimeIdentifiers such as `win-x64` and `win-x86`. For more information, see [.NET Runtime Identifier (RID) catalog](/dotnet/core/rid-catalog) and [NETSDK1083](/dotnet/core/tools/sdk-errors/netsdk1083).

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

For example:

```console
dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win-x64
```

[!INCLUDE [dotnet publish in .NET 8](~/includes/dotnet-publish-net8.md)]

Publishing builds and packages the app, copying the signed package to the _bin\\Release\\net10.0-windows10.0.19041.0\\win-x64\\AppPackages\\\<appname>\\_ folder. \<appname> is a folder named after both your project and version. In this folder, there's an _msix_ file, and that's the app package.

::: moniker-end

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

## Installing the app

To install the app, it must be signed with a certificate that you already trust. If it isn't, Windows won't let you install the app. You'll be presented with a dialog similar to the following, with the Install button disabled:

:::image type="content" source="media/publish-cli/install-untrusted.png" alt-text="Installing an untrusted app.":::

Notice that in the previous image, the Publisher was "unknown."

To trust the certificate of app package, perform the following steps:

01. Right-click on the _.msix_ file and choose **Properties**.
01. Select the **Digital Signatures** tab.
01. Choose the certificate then press **Details**.

    :::image type="content" source="media/publish-cli/properties-digital-signatures.png" alt-text="Properties pane of an MSIX file with the digital signatures tab selected.":::

01. Select **View Certificate**.
01. Select **Install Certificate...**.
01. Choose **Local Machine** then select **Next**.

    If you're prompted by User Account Control to **Do you want to allow this app to make changes to your device?**, select **Yes**.

01. In the **Certificate Import Wizard** window, select **Place all certificates in the following store**.
01. Select **Browse...** and then choose the **Trusted People** store. Select **OK** to close the dialog.

    :::image type="content" source="media/publish-cli/certificate-import.png" alt-text="Certificate import wizard window is shown while selecting the Trusted People store.":::

01. Select **Next** and then **Finish**. You should see a dialog that says: **The import was successful**.

    :::image type="content" source="media/publish-cli/certificate-import-success.png" alt-text="Certificate import wizard window with a successful import message.":::

01. Select **OK** on any window opened as part of this process, to close them all.

Now, try opening the package file again to install the app. You should see a dialog similar to the following, with the Publisher correctly displayed:

:::image type="content" source="media/publish-cli/install-trusted.png" alt-text="Installing a trusted app.":::

Select the **Install** button if you would like to install the app.

## Current limitations

The following list describes the current limitations with publishing and packaging:

- The published app doesn't work if you try to run it directly with the executable file out of the publish folder.
- The way to run the app is to first install it through the packaged _MSIX_ file.
