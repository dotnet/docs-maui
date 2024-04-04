---
title: "WebView zoom on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that enables zoom on a WebView."
ms.date: 04/05/2022
---

# WebView zoom on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific enables pinch-to-zoom and a zoom control on a <xref:Microsoft.Maui.Controls.WebView>. It's consumed in XAML by setting the `WebView.EnableZoomControls` and `WebView.DisplayZoomControls` bindable properties to `boolean` values:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <WebView Source="https://www.microsoft.com"
             android:WebView.EnableZoomControls="true"
             android:WebView.DisplayZoomControls="true" />
</ContentPage>
```

The `WebView.EnableZoomControls` bindable property controls whether pinch-to-zoom is enabled on the <xref:Microsoft.Maui.Controls.WebView>, and the `WebView.DisplayZoomControls` bindable property controls whether zoom controls are overlaid on the <xref:Microsoft.Maui.Controls.WebView>.

Alternatively, the platform-specific can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
    .EnableZoomControls(true)
    .DisplayZoomControls(true);
```

The `WebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `WebView.EnableZoomControls` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to control whether pinch-to-zoom is enabled on the <xref:Microsoft.Maui.Controls.WebView>. The `WebView.DisplayZoomControls` method, in the same namespace, is used to control whether zoom controls are overlaid on the <xref:Microsoft.Maui.Controls.WebView>. In addition, the `WebView.ZoomControlsEnabled` and `WebView.ZoomControlsDisplayed` methods can be used to return whether pinch-to-zoom and zoom controls are enabled, respectively.

The result is that pinch-to-zoom can be enabled on a <xref:Microsoft.Maui.Controls.WebView>, and zoom controls can be overlaid on the <xref:Microsoft.Maui.Controls.WebView>:

:::image type="content" source="media/webview-zoom-controls/webview-zoom.png" alt-text="Screenshot of zoomed WebView on Android.":::

> [!IMPORTANT]
> Zoom controls must be both enabled and displayed, via the respective bindable properties or methods, to be overlaid on a <xref:Microsoft.Maui.Controls.WebView>.
