---
title: "Ad-hoc publish"
description: "Learn how to publish an iOS .NET MAUI app using ad-hoc distribution."
ms.date: 02/20/2023
---

# Publish an ad-hoc app

When you try to publish the archive, you'll need to specify a certificate and provisioning profile again to resign the app before publishing it, and the type of those should match the selected distribution channel.
 markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Publish](../includes/publish.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **Ad Hoc** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-ad-hoc.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-ad-hoc.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

1. In the **Distribute - Signing Identity** dialog, select the **Save As** button. The **Save As** button will publish your app to an *.ipa* file on your file system.

The app can then be distributed using TestFlight, over-the-air from a web server, or via iTunes.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE.

---

## Troubleshoot

[Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12) can be used to help identify errors with app packages that stop successful submission to the App Store.
