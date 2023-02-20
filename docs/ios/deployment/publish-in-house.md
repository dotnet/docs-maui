---
title: "In-house publish"
description: "Learn how to publish an iOS .NET MAUI app using in-house distribution."
ms.date: 02/20/2023
---

# Publish an in-house app

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Publish](../includes/publish.md)]

<!-- markdownlint-disable MD029 -->
7. In the **Distribute - Select Channel** dialog, select the **Enterprise** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-enterprise.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select your signing identity and provisioning profile:

    :::image type="content" source="media/publish/vs/distribution-signing-identity-enterprise.png" alt-text="Screenshot of selecting a signing identity in the distribution dialog.":::

    > [!NOTE]
    > Your signing identity and provisioning profile should match the selected distribution channel.

1. In the **Distribute - Signing Identity** dialog, select the **Save As** button. The **Save As** button will re-sign your app and publish it to an *.ipa* file on your file system.

In-house apps can be distributed via a secure website, or via Mobile Device Management (MDM). Both of these approaches require the app to be prepared for distribution, which includes the preparation of a manifest. For more information, see [Distribute proprietary in-house apps to Apple devices](https://support.apple.com/guide/deployment/depce7cefc4d/web) on support.apple.com.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

TEXT GOES HERE.

---
