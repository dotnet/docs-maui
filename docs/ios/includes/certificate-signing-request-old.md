---
ms.date: 01/19/2023
ms.topic: include
---

## Create a certificate signing request

To create a distribution certificate, you'll first need to create a certificate signing request (CSR) in Keychain Access on a Mac. You only need to create a certificate signing request if you don't already have a distribution certificate.

To create a CSR:

1. On your Mac, launch Keychain Access.
1. In Keychain Access, select the **Keychain Access > Certificate Assistant > Request a Certificate from a Certificate Authority...** menu item.
1. In the **Certificate Assistant** dialog, enter an email address in the **User Email Address** field.
1. In the **Certificate Assistant** dialog, enter a name for the key in the **Common Name** field.
1. In the **Certificate Assistant** dialog, leave the **CA Email Address** field empty.
1. In the **Certificate Assistant** dialog, choose the **Saved to disk** radio button and click **Continue**:

    :::image type="content" source="../deployment/media/provisioning/certificate-assistant.png" alt-text="Certificate assistant dialog.":::

1. Save the certificate signing request to a known location.
1. Close Keychain Access.
