---
title: "AgentClient API reference"
description: "Reference for the AgentClient class in the Microsoft.Maui.DevFlow.Driver namespace for programmatic interaction with DevFlow agents from .NET code."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# AgentClient API reference

The `AgentClient` class in the `Microsoft.Maui.DevFlow.Driver` namespace provides a programmatic .NET API for interacting with DevFlow agents. Use it to automate tests, integrate with CI/CD pipelines, or build custom tooling.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Package

Add the driver package to your test or automation project:

```xml
<PackageReference Include="Microsoft.Maui.DevFlow.Driver" />
```

`AgentClient` ships in the `Microsoft.Maui.DevFlow.Client` package, which `Microsoft.Maui.DevFlow.Driver` references. If you only need `AgentClient` and its protocol types, for example in a .NET Framework test harness, you can reference `Microsoft.Maui.DevFlow.Client` directly. It targets `netstandard2.0` and `net8.0`. In both packages, the namespace is `Microsoft.Maui.DevFlow.Driver`.

## Create a client

```csharp
public AgentClient(string host = "localhost", int port = 9223)
```

Creates a client for the DevFlow agent at the specified host and HTTP port. `AgentClient` implements `IDisposable`, so dispose it when you're done:

```csharp
using Microsoft.Maui.DevFlow.Driver;

using var client = new AgentClient("localhost", 9223);
```

## Transient retry configuration

On unstable transports such as Android ADB port forwards, `AgentClient` can retry requests that fail with a transient transport error. Retries are off by default, so calls fail fast unless you opt in.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `TransientFailureRetryCount` | `int` | `0` | Number of additional attempts after a transient transport failure. `0` keeps the fail-fast behavior. |
| `RetryMutatingRequests` | `bool` | `true` | Whether retries also apply to requests that change app state, such as taps, fills, navigation, and property changes. Set it to `false` to retry only requests that read state. It has no effect while `TransientFailureRetryCount` is `0`. |
| `TransientFailureRetryDelay` | `TimeSpan` | 250 ms | Base delay between attempts. The delay grows linearly with each attempt (base × attempt number, up to 5 × base). Set it to `TimeSpan.Zero` to retry immediately. |

`AgentClient` checks the exception and its inner exceptions, and treats the following failures as transient:

- A `SocketException` or `IOException`.
- On .NET Framework, a `WebException` with a connection-level status such as `ConnectFailure`, `ConnectionClosed`, `ReceiveFailure`, or `SendFailure`.
- A `TaskCanceledException` that wraps an inner exception other than `TimeoutException`.

These settings don't retry HTTP error responses. A plain `HttpClient` timeout or a cancellation that you request also isn't retried.

> [!WARNING]
> Retrying a request that changes app state can duplicate its side effect if the agent receives the request but the response is lost. For example, a tap might fire twice or a Shell navigation might push the same route twice. Unless your scenario tolerates this, set `RetryMutatingRequests` to `false` when you enable retries.

The following example enables retries for Android integration tests while limiting them to read requests:

```csharp
using var client = new AgentClient("localhost", agentPort)
{
    TransientFailureRetryCount = 3,
    RetryMutatingRequests = false,
    TransientFailureRetryDelay = TimeSpan.FromMilliseconds(500)
};
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

### ScreenshotAsync

```csharp
public Task<byte[]?> ScreenshotAsync(
    int? window = null,
    string? elementId = null,
    string? selector = null,
    int? maxWidth = null,
    string? scale = null)
```

Captures a PNG screenshot of the running app, or of a specific element when you pass `elementId` or a CSS `selector`. Returns the PNG bytes, or `null` if the capture fails. To find out why a capture failed, use `ScreenshotResultAsync` instead.

### ScreenshotResultAsync

```csharp
public Task<ScreenshotResult> ScreenshotResultAsync(
    int? window = null,
    string? elementId = null,
    string? selector = null,
    int? maxWidth = null,
    string? scale = null)
```

Captures a PNG screenshot and returns a `ScreenshotResult` that describes success or failure. It takes the same parameters as `ScreenshotAsync`. Unlike `ScreenshotAsync`, it doesn't collapse every failure into `null`, and it doesn't throw when the agent doesn't support screenshots. Instead, it returns a failed result with `Reason` set to `not_supported`.

Other overloads also accept capture epoch, registry generation, and `ScreenshotResolutionOptions` arguments.

The `ScreenshotResult` class includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `Success` | `bool` | `true` if the capture succeeded. |
| `Data` | `byte[]?` | PNG bytes when `Success` is `true`. |
| `Error` | `string?` | Human-readable error message when the capture failed. It can be `null` if the agent didn't provide one, for example after a transport failure. |
| `Reason` | `string?` | Machine-readable cause, when available. For example, `window-not-visible`, `not_supported`, or `screenshot-too-large`. |
| `Retryable` | `bool` | `true` if retrying after you correct the reported condition might succeed. |
| `Suggestions` | `IReadOnlyList<string>?` | Actionable suggestions from the agent, when available. |

On success, `ScreenshotResult` also reports capture metadata such as `PixelWidth`, `PixelHeight`, `CaptureScale`, and `Format`.

The following example saves a screenshot or reports why it failed:

```csharp
var result = await client.ScreenshotResultAsync();
if (result.Success)
{
    File.WriteAllBytes("screenshot.png", result.Data!);
}
else
{
    Console.WriteLine($"Screenshot failed: {result.Error ?? "unknown error"} ({result.Reason})");
    if (result.Retryable && result.Suggestions is { Count: > 0 })
    {
        foreach (var suggestion in result.Suggestions)
            Console.WriteLine($"- {suggestion}");
    }
}
```

> [!NOTE]
> On macOS (AppKit), DevFlow captures the app window without activating it or changing keyboard focus, so screenshots work when the app isn't frontmost or is covered by other windows. If a capture fails because the window is hidden or minimized, the result has `Reason` set to `window-not-visible` and `Retryable` set to `true`. Restore the window and retry. If native window capture is required, make sure the **Screen & System Audio Recording** permission is granted in **System Settings** > **Privacy & Security**.

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

## See also

- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [MCP tools reference](mcp-tools.md)
