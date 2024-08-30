---
ms.date: 08/28/2024
ms.topic: include
---

## Create a distribution certificate

A distribution certificate is used to confirm your identity. Before creating a distribution certificate, you should ensure that you've added your Apple Developer Account to Visual Studio. For more information, see [Apple account management](~/ios/apple-account-management.md).

You only need to create a distribution certificate if you don't already one. The distribution certificate must be created using the Apple ID for your Apple Developer Account.

To create a distribution certificate in Visual Studio:

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select a team and click the **View Details...** button.
1. In the **Details** dialog, click **Create Certificate** and select **iOS Distribution**. A new signing identity will be created and will sync with Apple provided that you have the correct permissions.

> [!IMPORTANT]
> The private key and certificate that make up your signing identity will also be exported to **Keychain Access** on your Mac build host, provided that the IDE is paired to it. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

### Understanding certificate key pairs

A distribution profile contains certificates, their associated keys, and any provisioning profiles associated with your Apple Developer Account. There are two versions of a distribution profile — one exists in your Apple Developer Account, and the other lives on a local machine. The difference between the two is the type of keys they contain: the profile in your Apple Developer Account contains all of the public keys associated with your certificates, while the copy on your local machine contains all of the private keys. For certificates to be valid, the key pairs must match.

> [!WARNING]
> Losing the certificate and associated keys can be incredibly disruptive, as it will require revoking existing certificates and re-creating provisioning profiles.
