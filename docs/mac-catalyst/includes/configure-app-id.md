---
ms.topic: include
ms.date: 03/13/2023
---

### Configure the App ID

By default, a Mac Catalyst app uses the same bundle ID as an iOS app so you can offer the apps together as a universal purchase on the Mac App Store. Alternatively, you can specify a unique bundle ID to offer the app as a separate product.

To configure the App ID:

1. In your Apple Developer Account, navigate to **Certificates, IDs & Profiles**.
1. On the **Certificates, Identifiers & Profiles** page, select the **Identifiers** tab.
1. On the **Identifiers** page, select the App ID you just created.
1. On the **Edit your App ID Configuration** page, scroll to the bottom of the page and enable the **Mac Catalyst** capability check-box. Then select the **Configure** button:

    :::image type="content" source="../deployment/media/publish-app-store/catalyst-capability.png" alt-text="Enable the Mac Catalyst capability.":::

1. In the **Configure Bundle ID for Mac Catalyst** popup, select the **Use existing Mac App ID** radio button. In the **App ID** drop-down, select either the App ID for your Mac Catalyst's partner iOS app, or the the App ID you've created if you're offering the Mac Catalyst app as a separate product. Then, select the **Save button**:

    :::image type="content" source="../deployment/media/publish-app-store/configure-bundle-id.png" alt-text="Configure the Bundle ID for Mac Catalyst.":::

1. In the **Edit your App ID Configuration** page, select the **Save** button:

    :::image type="content" source="../deployment/media/publish-app-store/save-catalyst-configuration.png" alt-text="Save the Mac Catalyst configuration.":::

1. In the **Modify App Capabilities** popup, select the **Confirm** button:

    :::image type="content" source="../deployment/media/publish-app-store/modify-app-capabilities.png" alt-text="Modify the app capabilities.":::
