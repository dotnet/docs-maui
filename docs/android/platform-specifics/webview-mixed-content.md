---
title: "WebView mixed content on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that displays mixed content in a WebView."
ms.date: 04/05/2022
---

# WebView mixed content on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific controls whether a <xref:Microsoft.Maui.Controls.WebView> can display mixed content. Mixed content is content that's initially loaded over an HTTPS connection, but which loads resources (such as images, audio, video, stylesheets, scripts) over an HTTP connection. It's consumed in XAML by setting the `WebView.MixedContentMode` attached property to a value of the `MixedContentHandling` enumeration:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <WebView ... android:WebView.MixedContentMode="AlwaysAllow" />
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetMixedContentMode(MixedContentHandling.AlwaysAllow);
```

The `WebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `WebView.SetMixedContentMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to control whether mixed content can be displayed, with the `MixedContentHandling` enumeration providing three possible values:

- `AlwaysAllow` – indicates that the <xref:Microsoft.Maui.Controls.WebView> will allow an HTTPS origin to load content from an HTTP origin.
- `NeverAllow` – indicates that the <xref:Microsoft.Maui.Controls.WebView> will not allow an HTTPS origin to load content from an HTTP origin.
- `CompatibilityMode` – indicates that the <xref:Microsoft.Maui.Controls.WebView> will attempt to be compatible with the approach of the latest device web browser. Some HTTP content may be allowed to be loaded by an HTTPS origin and other types of content will be blocked. The types of content that are blocked or allowed may change with each operating system release.

The result is that a specified `MixedContentHandling` value is applied to the <xref:Microsoft.Maui.Controls.WebView>, which controls whether mixed content can be displayed:

:::image type="content" source="media/webview-mixed-content/webview-mixedcontent.png" alt-text="WebView mixed content handling platform-specific.":::
