---
ms.topic: include
ms.date: 12/21/2023
---

The following changes must be made to your migrated project's *Package.appxmanifest* file:

1. Set the application entry point to `$targetentrypoint$`. For more information, see [Target entry point](https://github.com/mattleibow/MultiHeadMauiTemplates/blob/6e7cb786ed18756749a617d303df46130eab45d9/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI/Package.appxmanifest#L34).
2. Add the `runFullTrust` capability. For more information, see [Run full trust capability](https://github.com/mattleibow/MultiHeadMauiTemplates/blob/6e7cb786ed18756749a617d303df46130eab45d9/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI/Package.appxmanifest#L48).
3. Add the `Windows.Universal` and `Windows.Desktop` target device families. For more information, see [Universal target device family](https://github.com/mattleibow/MultiHeadMauiTemplates/blob/6e7cb786ed18756749a617d303df46130eab45d9/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI/Package.appxmanifest#L23) and [Desktop target device family](https://github.com/mattleibow/MultiHeadMauiTemplates/blob/6e7cb786ed18756749a617d303df46130eab45d9/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI/Package.appxmanifest#L24).

Making these changes fixes common deployment errors for your app on Windows.

For an example of a compliant *Package.appxmanifest* file, see [*Package.appxmanifest*](https://github.com/mattleibow/MultiHeadMauiTemplates/blob/main/sample/MauiMultiHeadApp/MauiMultiHeadApp.WinUI/Package.appxmanifest).
