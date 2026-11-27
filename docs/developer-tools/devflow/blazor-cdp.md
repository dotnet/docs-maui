---
title: "Blazor WebView debugging with CDP"
description: "Learn how to debug Blazor Hybrid content in .NET MAUI apps using the DevFlow CDP bridge and Chrome DevTools Protocol."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Blazor WebView debugging with CDP

DevFlow includes a Blazor WebView CDP (Chrome DevTools Protocol) bridge that enables browser-style debugging of Blazor Hybrid content in MAUI apps. The bridge is powered by Chobitsu and works across platforms.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## How it works

The `Microsoft.Maui.DevFlow.Blazor` package injects a CDP bridge into `BlazorWebView`. This allows Chrome DevTools-style inspection of the web content without requiring browser-specific debugging setup. The bridge communicates using the Chrome DevTools Protocol, giving you access to DOM inspection, JavaScript evaluation, navigation, input, screenshots, and console output for Blazor Hybrid content.

> [!NOTE]
> The CDP bridge doesn't capture browser `fetch` or `XMLHttpRequest` traffic from WebView content. DevFlow network monitoring captures native .NET HTTP traffic only. For more information, see [Network monitoring and profiling](network-profiling.md).

## Setup

Add the `Microsoft.Maui.DevFlow.Blazor` NuGet package to your project:

```xml
<PackageReference Include="Microsoft.Maui.DevFlow.Blazor" />
```

Register the Blazor CDP bridge in `MauiProgram.cs` alongside the DevFlow agent:

```csharp
var builder = MauiApp.CreateBuilder();
builder.UseMauiApp<App>();
builder.Services.AddMauiBlazorWebView();

#if DEBUG
// Register the DevFlow agent
builder.AddMauiDevFlowAgent();
// Register the Blazor CDP bridge
builder.AddMauiBlazorDevFlowTools();
#endif
```

## Use the CDP bridge

Once the app is running with the Blazor CDP bridge registered, use the `maui devflow webview` command group to inspect and automate the web content. The `cdp` command name is an alias for `webview`, so `maui devflow cdp` accepts the same subcommands and options.

Check that the bridge is connected:

```console
maui devflow webview status
```

### Target a WebView

When your app contains more than one `BlazorWebView`, list the registered WebViews:

```console
maui devflow webview webviews
```

Each entry includes its registry `index`, `automationId`, and `elementId`. The `id` value is the AutomationId when one is set, otherwise the element ID, otherwise the index.

Use the `--webview` option, or its `-w` short form, to choose which WebView a command targets. The option accepts the WebView's index, AutomationId, or element ID, and the agent checks them in that order:

1. If the value is a number, the agent looks for the WebView with that registry index. If no WebView has that index, it continues with the next checks. As a result, a numeric AutomationId such as `1` matches the WebView at index 1 first.
1. The agent looks for a WebView whose AutomationId matches the value, ignoring case.
1. The agent looks for a WebView whose element ID matches the value, ignoring case.

If more than one WebView has a matching AutomationId or element ID, the agent uses the most recently registered one. If nothing matches, the command fails with a `WebView '<value>' not found` error.

If you omit `--webview`, the agent selects a default WebView in this order:

1. A ready WebView on the current page.
1. Any WebView on the current page, even if it isn't ready yet.
1. The most recently registered ready WebView.
1. The most recently registered WebView.

The agent identifies WebViews on the current page by their AutomationId, so the first two steps only apply to WebViews that have an AutomationId. Because the default can change as pages and WebViews load, specify the target explicitly when your app has more than one WebView, especially in scripts.

```console
maui devflow webview --webview Right DOM querySelector "#name"
maui devflow webview -w 1 snapshot
maui devflow cdp -w Right Runtime evaluate "document.title"
```

> [!TIP]
> Set the `AutomationId` property on each `BlazorWebView` so that you can target it by a stable name instead of its registry index. Registry indexes follow the order in which WebViews register, which can differ between app launches. An AutomationId is also required for the agent to recognize a WebView as being on the current page when it selects a default. Avoid purely numeric AutomationIds, because numeric values are matched as indexes first.

### WebView commands

The following commands are available under `maui devflow webview`. Every command except `webviews` accepts `--webview`.

| Command | Description |
|---------|-------------|
| `status` | Check the CDP connection status. |
| `webviews` | List the registered WebViews. |
| `source` | Get the page HTML source. |
| `snapshot` | Get a simplified DOM snapshot with element references. |
| `Browser getVersion` | Get browser version information. |
| `Runtime evaluate <expression>` | Evaluate a JavaScript expression. |
| `DOM getDocument` | Get the document root node. |
| `DOM querySelector <selector>` | Find the first element that matches a CSS selector. |
| `DOM querySelectorAll <selector>` | Find all elements that match a CSS selector. |
| `DOM getOuterHTML <selector>` | Get the HTML of the element that matches a CSS selector. |
| `Page navigate <url>` | Navigate to a URL. |
| `Page reload` | Reload the page. |
| `Page captureScreenshot` | Capture a page screenshot as base64. |
| `Input dispatchClickEvent <selector>` | Click the element that matches a CSS selector. |
| `Input fill <selector> <text>` | Set the value of the matching form field and dispatch `input` and `change` events. |
| `Input insertText <text>` | Insert text at the cursor in the focused element. |

For example, fill in a form field and submit it in the WebView with the `Right` AutomationId:

```console
maui devflow webview -w Right Input fill "#name" "Hello"
maui devflow webview -w Right Input insertText " world"
maui devflow webview -w Right Input dispatchClickEvent "#submit"
```

These commands operate on the DOM inside the selected WebView. They're separate from native UI commands such as `maui devflow ui query --selector`, which query the .NET MAUI visual tree.

### Error handling

WebView commands report failures as errors instead of successful results. When a command fails, the CLI writes the error to standard error and exits with a nonzero exit code. Errors include the original JavaScript exception description, CDP and HTTP error messages, or the raw response when the bridge returns an invalid response. For example, when no element matches the selector, `Input fill` and `Input dispatchClickEvent` fail with an `Element not found` error.

In scripts, check the exit code of each command rather than parsing its output:

```bash
if ! maui devflow webview -w Right Input fill "#name" "Hello"; then
    echo "Couldn't fill the field" >&2
    exit 1
fi
```

The `maui_cdp_evaluate` and `maui_cdp_source` MCP tools report the same failures as tool errors. The `maui_cdp_source` tool also reports an error when the WebView returns no HTML source.

### Agent HTTP API

The CLI sends the selected WebView to the agent as the `webview` query parameter. WebView endpoints under `/api/v1/webview/` accept this parameter. Most of them also accept `contextId` as an alias, but `POST /api/v1/webview/evaluate` accepts only `webview`. When you supply both, `webview` takes precedence.

The agent's port is usually assigned by the [DevFlow broker](broker.md), so don't assume the default port 9223. Run `maui devflow list` to see each connected agent and its port. The following example looks up the port for an app named `MyApp` and then reads the HTML source of the WebView whose `AutomationId` is `Right`:

```bash
PORT=$(maui devflow list --json | jq -r '.[] | select(.appName == "MyApp") | .port')
curl "http://localhost:$PORT/api/v1/webview/source?webview=Right"
```

For the list of WebView endpoints, see [DevFlow agent HTTP API reference](agent-api.md#webview-endpoints).

## Platform-specific setup

For platform-specific WebView debugging configuration, see:

- [Android setup](setup-android.md)
- [Apple platforms setup](setup-apple.md)
- [Windows setup](setup-windows.md)

## See also

- [DevFlow overview](index.md)
- [DevFlow agent HTTP API reference](agent-api.md)
- [MCP tools reference](mcp-tools.md)
