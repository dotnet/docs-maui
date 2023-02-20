---
title: "Ad-hoc publish"
description: "Learn how to publish an iOS .NET MAUI app using ad-hoc distribution."
ms.date: 02/20/2023
---

# Publish an ad-hoc app

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Publish](../includes/publish.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **Ad Hoc** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-ad-hoc.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-ad-hoc.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

    > [!NOTE]
    > Your signing identity and provisioning profile should match the selected distribution channel.

1. In the **Distribute - Signing Identity** dialog, select the **Save As** button. The **Save As** button will re-sign your app and publish it to an *.ipa* file on your file system.

The app can then be distributed using [Apple Configurator](https://apps.apple.com/app/id1037126344). For more information, see [Apple Configurator user guide](https://support.apple.com/guide/apple-configurator-mac/welcome/mac) on support.apple.com

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE.

---
