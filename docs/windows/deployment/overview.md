---
title: "Publish a .NET MAUI app for Windows"
description: "Learn how to package and publish a Windows .NET MAUI app."
ms.date: 03/25/2022
---

# Publish a .NET MAUI App for Windows

> [!div class="op_single_selector"]
>
> - [Publish for Android](../../android/deployment/overview.md)
> - [Publish for Windows](overview.md)

When distributing your .NET MAUI app for Windows, you can publish the app and its dependencies to a folder for deployment to another system. You can also package the app into an MSIX package, which has numerous benefits for the users installing your app. For more information about the benefits of MSIX, see [What is MSIX?](/windows/msix/overview)

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The current preview of .NET MAUI only allows publishing an MSIX package. You can't yet publish a Windows executable file for distribution.

## Prerequisites

In addition to the .NET MAUI workload, you'll need to add the latest Visual C++ build tools. Open the Visual Studio 2022 installer and add the **MSVC v143 - VS2022 C++ x64/x86 build tools (latest)** individual component. For more information on how to install an individual component, see [Modify Visual Studio workloads, components, and language packs](/visualstudio/install/modify-visual-studio?view=vs-2022&preserve-view=true).

:::image type="content" source="media/overview/individual-component-small.png" alt-text="Visual Studio individual component highlighting the latest C++ build tools." lightbox="media/overview/individual-component.png":::

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
    Get-ChildItem "Cert:\CurrentUser\My" | Format-Table Subject, FriendlyName, Thumbprint
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

01. The **Thumbprint** of your certificate will be used later, copy it to your clipboard. It's the **Thumbprint** value whose entry matches the **Subject** and **FriendlyName** of your certificate.

For more information, see [Create a certificate for package signing](/windows/msix/package/create-certificate-package-signing).

<!-- The pfx command line options don't seem to work yet -->
<!--
### Optionally export a PFX file

You can use the thumbprint of the certificate to sign your package later, or you export the certificate to a Personal Information Exchange (PFX) file. The PFX file is secured with a password and can be referenced by your project. To export the certificate as a PFX file, do the following:

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

## Configure the project build settings

The project file is a good place to put Windows-specific build settings. You may not want to put some settings into the project file, such as passwords. The settings described in this section can be passed on the command line with the `/p:name=value` format. If the setting is already defined in the project file, a setting passed on the command line will override the project setting.

Add the following `<PropertyGroup>` node to your project file. This property group is only processed when the target framework is Windows and the configuration is set to `Release`. This config section runs whenever a build or publish in `Release` mode.

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-windows')) and '$(Configuration)' == 'Release'">
    <GenerateAppxPackageOnBuild>true</GenerateAppxPackageOnBuild>
    <AppxPackageSigningEnabled>true</AppxPackageSigningEnabled>
    <PackageCertificateThumbprint>A10612AF095FD8F8255F4C6691D88F79EF2B135E</PackageCertificateThumbprint>
</PropertyGroup>
```

Replace the `<PackageCertificateThumbprint>` property value with the certificate thumbprint you previously generated. Alternatively, you can remove this setting from the project file and provide it on the command line. For example: `/p:PackageCertificateThumbprint=A10612AF095FD8F8255F4C6691D88F79EF2B135E`.

The `<GenerateAppxPackageOnBuild>` set to `true` packages the app and signs it with the certificate that matches the `<PackageCertificateThumbprint>` value. Signing only happens if the `<AppxPackageSigningEnabled>` setting is `true`.

## Publish

At this time, publishing is only supported through `msbuild`, not `dotnet`.

To publish your app, open the **Developer Command Prompt for VS 2022 Preview** terminal and navigate to the project's folder. Run `msbuild` in publish mode:  

```console
msbuild /restore /t:Publish /p:TargetFramework=net6.0-windows10.0.19041 /p:configuration=release
```

The following table defines the parameters used by the previous command:

| Parameter                  | Value                                                                             |
|----------------------------|-----------------------------------------------------------------------------------|
| `/restore`                 | Restores any dependencies referenced by the project.                              |
| `/t:Publish`               | Runs the publish command.                                                         |
| `/p:TargetFramework`       | The target framework, which is a Windows TFM, such as `net6.0-windows10.0.19041`. |
| `/p:configuration=Release` | Sets the build configuration, which is `Release`.                                 |

Publishing builds and packages the app, copying the signed package to the _bin\\Release\\net6.0-windows10.0.19041\\win10-x64\\AppPackages\\\<appname>\\_ folder. Where \<appname> is a folder named after both your project and version. In this folder there's an _msix_ file, that's the app package.

## Installing the app

To install the app, it must be signed with a certificate that you already trust. If it isn't, Windows won't let you install the app. You'll be presented with a dialog similar to the following, with the Install button disabled:

:::image type="content" source="media/overview/install-untrusted.png" alt-text="Installing an untrusted app.":::

Notice that in the previous image, the Publisher was "unknown."

To trust the certificate of app package, perform the following steps:

01. Right-click on the _.msix_ file and choose **Properties**.
01. Select the **Digital Signatures** tab.
01. Choose the certificate then press **Details**.

    :::image type="content" source="media/overview/properties-digital-signatures.png" alt-text="Properties pane of an MSIX file with the digital signatures tab selected.":::

01. Select **View Certificate**.
01. Select **Install Certificate...**
01. Choose **Local Machine** then select **Next**.

    If you're prompted by User Account Control to **Do you want to allow this app to make changes to your device?**, select **Yes**.

01. In the **Certificate Import Wizard** window, select **Place all certificates in the following store**.
01. Select **Browse...** and then choose the **Trusted People** store. Select **OK** to close the dialog.

    :::image type="content" source="media/overview/certificate-import.png" alt-text="Certificate import wizard window is shown while selecting the Trusted People store.":::

01. Select **Next** and then **Finish**. You should see a dialog that says: **The import was successful**.

    :::image type="content" source="media/overview/certificate-import-success.png" alt-text="Certificate import wizard window with a successful import message.":::

01. Select **OK** on any window opened as part of this process, to close them all.

Now, try opening the package file again to install the app. You should see a dialog similar to the following, with the Publisher correctly displayed:

:::image type="content" source="media/overview/install-trusted.png" alt-text="Installing a trusted app.":::

Select the **Install** button if you would like to install the app.

## Current limitations

The following list describes the current limitations with publishing and packaging:

01. The published app doesn't work if you try to run it directly with the executable file out of the publish folder.
01. The way to run the app is to first install it through the packaged _MSIX_ file.

## .NET MAUI Blazor app considerations

Currently, .NET MAUI Blazor apps won't run when deployed to another computer. There's one more config section to add to your project to make the published app work on other computers:

```xml
<Target Name="_RemoveStaticWebAssetsDevelopmentManifest" BeforeTargets="GetCopyToOutputDirectoryItems">
    <ItemGroup>
        <ContentWithTargetPath Remove="$(StaticWebAssetDevelopmentManifestPath)" />
    </ItemGroup>
</Target>
```

## See also

- [GitHub discussion and feedback: .NET MAUI Windows target publishing/archiving](https://github.com/dotnet/maui/issues/4329)
