---
ms.topic: include
ms.date: 03/23/2023
ms.custom: sfi-image-nochange
---

## Create an installer certificate

The CSR allows you to generate an installer certificate, which is required to sign your app's installer package for submission to the Mac App Store. The installer certificate must be created using the Apple ID for your Apple Developer Account:

1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, select the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **Mac Installer Distribution** radio button before selecting the **Continue** button:

    :::image type="content" source="../deployment/media/publish-app-store/mac-installer-certificate.png" alt-text="Create a Mac Installer distribution certificate.":::

1. On the **Create a New Certificate** page, select **Choose File**:

    :::image type="content" source="../deployment/media/publish-app-store/choose-certificate.png" alt-text="Upload your certificate signing request for the Mac installer certificate.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file you previously created (a file with a `.certSigningRequest` file extension) and then select **Upload**.
1. On the **Create a New Certificate** page, select the **Continue** button:

    :::image type="content" source="../deployment/media/publish-app-store/chosen-certificate.png" alt-text="Continue to generate your installer certificate.":::

1. On the **Download Your Certificate** page, select the **Download** button:

    :::image type="content" source="../deployment/media/publish-app-store/download-installer-certificate.png" alt-text="Download your Mac installer certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **3rd Party Mac Developer Installer**:

    :::image type="content" source="../deployment/media/publish-app-store/keychain-access-installer-certificate.png" alt-text="Keychain Access showing installer certificate.":::

    > [!NOTE]
    > Make a note of the full certificate name in Keychain Access. It will be required when signing your app.
