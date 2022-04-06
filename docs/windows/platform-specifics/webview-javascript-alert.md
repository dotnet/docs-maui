---
title: "WebView JavaScript alerts on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that enables a WebView to display JavaScript alerts in a Windows message dialog."
ms.date: 04/06/2022
---

# WebView JavaScript alerts on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific enables a `WebView` to display JavaScript alerts in a Windows message dialog. It's consumed in XAML by setting the `WebView.IsJavaScriptAlertEnabled` attached property to a `boolean` value:

```xaml
<ContentPage ...
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <WebView ... windows:WebView.IsJavaScriptAlertEnabled="true" />
        ...
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

var webView = new Microsoft.Maui.Controls.WebView
{
    Source = new HtmlWebViewSource
    {
        Html = @"<html><body><button onclick=""window.alert('Hello World from JavaScript');"">Click Me</button></body></html>"
    }
};
webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetIsJavaScriptAlertEnabled(true);
```

The `WebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `WebView.SetIsJavaScriptAlertEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to control whether JavaScript alerts are enabled. In addition, the `WebView.SetIsJavaScriptAlertEnabled` method can be used to toggle JavaScript alerts by calling the `IsJavaScriptAlertEnabled` method to return whether they are enabled:

```csharp
_webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetIsJavaScriptAlertEnabled(!_webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().IsJavaScriptAlertEnabled());
```

The result is that JavaScript alerts can be displayed in a Windows message dialog:

:::image type="content" source="media/webview-javascript-alert/webview-javascript-alert.png" alt-text="WebView JavaScript alert platform-specific.":::
