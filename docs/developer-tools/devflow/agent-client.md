---
title: "AgentClient API reference"
description: "Reference for the AgentClient class in Microsoft.Maui.DevFlow.Driver for programmatic interaction with DevFlow agents from .NET code."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# AgentClient API reference

The `AgentClient` class in the `Microsoft.Maui.DevFlow.Driver` package provides a programmatic .NET API for interacting with DevFlow agents. Use it to automate tests, integrate with CI/CD pipelines, or build custom tooling.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Package

Add the driver package to your test or automation project:

```xml
<PackageReference Include="Microsoft.Maui.DevFlow.Driver" />
```

## AgentCapabilities

The `AgentCapabilities` class describes which features the current connected platform supports.

| Property | Type | Description |
|----------|------|-------------|
| `Screenshots` | `bool` | `true` if the platform supports screenshot capture. |
| `Sensors` | `bool` | `true` if the platform supports device sensor data. |
| `Network` | `bool` | `true` if the platform supports network request capture. |
| `Profiling` | `bool` | `true` if the platform supports CPU/memory/GC profiling. |
| `Jobs` | `bool` | `true` if the platform supports background job inspection (Android, iOS, Mac Catalyst). |

## AgentClient methods

### ConnectAsync

```csharp
public Task ConnectAsync(int port)
```

Connects to the DevFlow agent on the specified HTTP port.

### GetCapabilitiesAsync

```csharp
public Task<AgentCapabilities> GetCapabilitiesAsync()
```

Returns the capabilities of the connected agent for the current platform.

### GetVisualTreeAsync

```csharp
public Task<JsonElement> GetVisualTreeAsync()
```

Returns the visual tree of the running app.

### TakeScreenshotAsync

```csharp
public Task<byte[]> TakeScreenshotAsync()
```

Captures a PNG screenshot of the running app. Returns the raw PNG bytes.

### TapAsync

```csharp
public Task TapAsync(string automationId)
```

Simulates a tap on the element with the specified `AutomationId`.

### FillAsync

```csharp
public Task FillAsync(string automationId, string text)
```

Fills text into the `Entry` or `Editor` with the specified `AutomationId`.

### NavigateAsync

```csharp
public Task NavigateAsync(string route)
```

Navigates to the specified Shell route or page.

### GetLogsAsync

```csharp
public Task<JsonElement> GetLogsAsync()
```

Retrieves application log entries from the agent.

### GetNetworkRequestsAsync

```csharp
public Task<JsonElement> GetNetworkRequestsAsync()
```

Returns captured HTTP request/response pairs.

### GetJobsAsync

```csharp
public Task<JsonElement> GetJobsAsync()
```

Returns a JSON representation of the platform's background jobs. See [`maui_jobs_list`](mcp-tools.md#maui_jobs_list) for the response schema.

**Platform support:** Android, iOS, Mac Catalyst.

---

### RunJobAsync

```csharp
public Task<JsonElement> RunJobAsync(string identifier, string? type = null)
```

Triggers a background job by identifier.

| Parameter | Description |
|-----------|-------------|
| `identifier` | Platform job identifier from `GetJobsAsync()` |
| `type` | iOS only — `"processing"` or `"refresh"`. Optional; agent resolves from pending requests when omitted. |

**Platform support:** iOS, Mac Catalyst. (Android returns `success: false`.)

## WebView methods

The following methods interact with Blazor WebViews that are registered with the DevFlow Blazor bridge. The `webviewId` and `contextId` parameters target a WebView by index, `AutomationId`, or element ID. When you pass `null`, the agent selects a default WebView: a ready WebView on the current page if there is one, otherwise the most recently registered ready WebView. When your app has more than one WebView, specify the target explicitly.

```csharp
public Task<JsonElement> GetCdpWebViewsAsync()
public Task<JsonElement> SendCdpCommandAsync(string method, JsonNode? @params = null, string? webviewId = null)
public Task<string> GetCdpSourceAsync(string? webviewId = null)
public Task<bool> NavigateWebViewAsync(string url, string? contextId = null)
public Task<bool> ClickWebViewAsync(string selector, string? contextId = null)
public Task<bool> FillWebViewAsync(string selector, string text, string? contextId = null)
public Task<bool> InsertWebViewTextAsync(string text, string? contextId = null)
```

`GetCdpWebViewsAsync` lists the registered WebViews. `SendCdpCommandAsync` sends a Chrome DevTools Protocol command, such as `Runtime.evaluate`, to the target WebView.

Except for `GetCdpWebViewsAsync`, these methods throw an exception when a WebView command fails, instead of returning an unsuccessful result:

- HTTP failures throw `HttpRequestException`, and the exception message includes the response body.
- CDP, protocol, and JavaScript failures throw `InvalidOperationException`. JavaScript exceptions include the original error description.
- Invalid JSON responses throw `JsonException`.

`GetCdpWebViewsAsync` doesn't throw on failure. Instead, it returns a `JsonElement` whose `ValueKind` is `JsonValueKind.Undefined`, so check `ValueKind` before you read the result.

`NavigateWebViewAsync`, `ClickWebViewAsync`, `FillWebViewAsync`, and `InsertWebViewTextAsync` return `true` on success. They throw instead of returning `false` when the command fails or the response doesn't report success.

The following example fills an input in the WebView whose `AutomationId` is `Right` by using a connected `AgentClient` instance named `client`, and handles a failure:

```csharp
try
{
    await client.FillWebViewAsync("#name", "Hello", contextId: "Right");
}
catch (Exception ex) when (ex is HttpRequestException or InvalidOperationException)
{
    Console.WriteLine($"WebView command failed: {ex.Message}");
}
```

## See also

- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [MCP tools reference](mcp-tools.md)
- [Blazor WebView debugging](blazor-cdp.md)
