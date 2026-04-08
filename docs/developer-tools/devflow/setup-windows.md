---
title: "DevFlow Windows setup"
description: "Learn how to configure Windows for DevFlow Blazor WebView2 debugging using Edge DevTools Protocol."
ms.date: 04/03/2026
---

# DevFlow Windows setup

This guide covers how to configure Windows for DevFlow Blazor WebView2 debugging using Edge DevTools Protocol.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Prerequisites

- **EdgeDriver**: Must match the installed Microsoft Edge version. Download from [Microsoft Edge Developer](https://developer.microsoft.com/microsoft-edge/tools/webdriver/).
- **WebView2 Runtime**: Usually pre-installed on Windows 10 and Windows 11. If missing, download from [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/).

## Enable remote debugging

The WebView2 control must be configured to enable remote debugging. Choose one of the following options:

### Option 1: Via BlazorWebViewHandler mapper

Configure the handler in your `Platforms/Windows/App.xaml.cs` or `MauiProgram.cs`:

```csharp
#if DEBUG && WINDOWS
Microsoft.Maui.Handlers.BlazorWebViewHandler.Mapper.AppendToMapping(
    "WebView2Debugging", async (handler, view) =>
    {
        await handler.PlatformView.EnsureCoreWebView2Async();

        // This has no effect if the WebView2 is already created.
        // Set the environment variable (Option 2) for earlier initialization.
        handler.PlatformView.CoreWebView2.OpenDevToolsWindow();
    });
#endif
```

To configure the remote debugging port during WebView2 environment creation, set the environment options in `Platforms/Windows/App.xaml.cs`:

```csharp
#if DEBUG
Microsoft.Maui.Handlers.BlazorWebViewHandler.Mapper.AppendToMapping(
    "WebView2RemoteDebugging", async (handler, view) =>
    {
        var environment = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(
            browserExecutableFolder: null,
            userDataFolder: null,
            options: new Microsoft.Web.WebView2.Core.CoreWebView2EnvironmentOptions(
                "--remote-debugging-port=9222"));

        await handler.PlatformView.EnsureCoreWebView2Async(environment);
    });
#endif
```

### Option 2: Environment variable

Set the `WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS` environment variable before the app starts:

```powershell
$env:WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS = "--remote-debugging-port=9222"
```

Or set it permanently via **System Properties** > **Environment Variables**.

> [!NOTE]
> The environment variable must be set before the WebView2 control is initialized. Setting it after the app starts has no effect.

## Verify the debug endpoint

After the app is running with remote debugging enabled, verify the endpoint is accessible:

```bash
curl http://localhost:9222/json
```

You should see a JSON response listing the available debugging targets. Alternatively, open `edge://inspect` in Microsoft Edge and verify that the WebView2 target appears.

## Troubleshooting

| Issue | Possible cause | Resolution |
|-------|---------------|------------|
| EdgeDriver version mismatch | EdgeDriver version doesn't match installed Edge | Check your Edge version at `edge://version` and download the matching EdgeDriver. |
| Port 9222 already in use | Another process is using the debugging port | Close other Edge or Chrome instances that may be using port 9222, or choose a different port. |
| WebView2 not created with debug port | Environment options applied too late | Use the environment variable approach (Option 2) to ensure the flag is set before WebView2 initialization. |
| WebView2 Runtime not found | Runtime not installed on the machine | Download and install the [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/). |

## See also

- [DevFlow overview](index.md)
- [DevFlow broker architecture](broker.md)
