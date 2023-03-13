---
ms.date: 03/13/2023
ms.topic: include
---

## Create a distribution certificate

The CSR allows you to generate a distribution certificate, which confirms your identity. The distribution certificate must be created using the Apple ID for your Apple Developer Account:

1. In a web browser, login to your [Apple Developer Account](https://developer.apple.com/account/).
1. In your Apple Developer Account, select the **Certificates, IDs & Profiles** tab.
1. On the **Certificates, Identifiers & Profiles** page, click the **+** button to create a new certificate.
1. On the **Create a New Certificate** page, select the **iOS Distribution (App Store and Ad Hoc)** radio button before clicking the **Continue** button:

    :::image type="content" source="../deployment/media/ios-app-development.png" alt-text="Create a new certificate.":::

1. On the **Create a New Certificate** page, click **Choose File**:

    :::image type="content" source="../deployment/media/choose-certificate.png" alt-text="Upload your certificate signing request.":::

1. In the **Choose Files to Upload** dialog, select the certificate request file (a file with a `.certSigningRequest` file extension) and then click **Upload**.
1. On the **Create a New Certificate** page, click the **Continue** button:

    :::image type="content" source="../deployment/media/chosen-certificate.png" alt-text="Continue to generate your distribution certificate.":::

1. On the **Download Your Certificate** page, click the **Download** button:

    :::image type="content" source="../deployment/media/download-certificate.png" alt-text="Download your distribution certificate.":::

    The certificate file (a file with a `.cer` extension) will be downloaded to your chosen location.

1. On your Mac, double-click the downloaded certificate file to install the certificate to your keychain. The certificate appears in the **My Certificates** category in **Keychain Access**, and begins with **iPhone Distribution**:

    :::image type="content" source="../deployment/media/keychain-access.png" alt-text="Keychain Access showing distribution certificate.":::

    > [!NOTE]
    > Make a note of the full distribution certificate name in Keychain Access. It will be required when signing your app.
