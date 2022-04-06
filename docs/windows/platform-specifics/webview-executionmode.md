---
title: "WebView execution mode on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that sets the thread on which a WebView hosts its content."
ms.date: 04/06/2022
---

# WebView execution mode on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific sets the thread on which a `WebView` hosts its content. It's consumed in XAML by setting the `WebView.ExecutionMode` bindable property to a `WebViewExecutionMode` enumeration value:

```xaml
<ContentPage ...
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <WebView ... windows:WebView.ExecutionMode="SeparateThread" />
        ...
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

WebView webView = new Xamarin.Forms.WebView();
webView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetExecutionMode(WebViewExecutionMode.SeparateThread);
```

The `WebView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `WebView.SetExecutionMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to set the thread on which a `WebView` hosts its content, with the `WebViewExecutionMode` enumeration providing three possible values:

- `SameThread` indicates that content is hosted on the UI thread. This is the default value for the `WebView` on Windows.
- `SeparateThread` indicates that content is hosted on a background thread.
- `SeparateProcess` indicates that content is hosted on a separate process off the app process. There isn't a separate process per `WebView` object, and so all of an app's `WebView` objects share the same separate process.

In addition, the `GetExecutionMode` method can be used to return the current `WebViewExecutionMode` for the `WebView`.
