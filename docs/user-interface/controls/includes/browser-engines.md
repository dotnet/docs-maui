---
ms.topic: include
ms.date: 12/19/2024
---

## Browser engines

<xref:Microsoft.Maui.Controls.WebView> uses different browser engines on each platform to render web content:

- **Windows**: Uses WebView2, which is based on the Microsoft Edge (Chromium) browser engine. This provides modern web standards support and consistent behavior with the Edge browser.
- **Android**: Uses `android.webkit.WebView`, which is based on the Chromium browser engine. The specific version depends on the Android WebView system component installed on the device.
- **iOS and Mac Catalyst**: Uses `WKWebView`, which is based on the Safari WebKit browser engine. This is the same engine used by the Safari browser on iOS and macOS.

These platform-specific implementations mean that web content may render differently between platforms, and some platform-specific web APIs may only be available on certain platforms. When developing cross-platform apps, test your web content on all target platforms to ensure consistent behavior.
