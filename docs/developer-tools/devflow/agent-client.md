---
title: "AgentClient API reference"
description: "Reference for the AgentClient class in Microsoft.Maui.DevFlow.Driver for programmatic interaction with DevFlow agents from .NET code."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# AgentClient API reference

The `AgentClient` class in the `Microsoft.Maui.DevFlow.Driver` namespace provides a programmatic .NET API for interacting with DevFlow agents. Use it to automate tests, integrate with CI/CD pipelines, or build custom tooling.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Package

Add the driver package to your test or automation project. The package is currently available only as a prerelease, and it requires .NET 9 or later:

```dotnetcli
dotnet add package Microsoft.Maui.DevFlow.Driver --prerelease
```

`AgentClient` ships in the `Microsoft.Maui.DevFlow.Client` package, which `Microsoft.Maui.DevFlow.Driver` references. If you only need `AgentClient` and its protocol types, for example in a .NET Framework test harness, you can reference `Microsoft.Maui.DevFlow.Client` directly. It targets `netstandard2.0` and `net8.0`. In both packages, the namespace is `Microsoft.Maui.DevFlow.Driver`.

## Create a client

```csharp
public AgentClient(string host = "localhost", int port = 9223)
```

Creates a client for the DevFlow agent at the specified host and HTTP port. When the DevFlow broker is running, it assigns each agent a port from the 10223–10899 range, and `9223` is used only as a fallback. Run `maui devflow list` to find the port of your app's agent. For more information, see [DevFlow broker](broker.md).

`AgentClient` implements `IDisposable`, so dispose it when you're done:

```csharp
using Microsoft.Maui.DevFlow.Driver;

int agentPort = 10223; // The port reported by `maui devflow list`.
using var client = new AgentClient("localhost", agentPort);
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

These settings don't retry HTTP error responses or request timeouts.

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

## Check the agent status

Use the following methods to confirm that the agent is reachable and to find out which features it supports.

### GetStatusAsync

```csharp
public Task<AgentStatus?> GetStatusAsync(int? window = null)
```

Returns the status of the connected agent, or `null` if the agent can't be reached. Use it to confirm that the agent is running before you send other requests.

The `AgentStatus` class includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `Running` | `bool` | `true` if the agent reports that it's running. |
| `Platform` | `string?` | The device platform, for example `Android` or `iOS`. |
| `AppName` | `string?` | The name of the app that hosts the agent. |
| `Version` | `string?` | The agent version. |
| `Route` | `string?` | The current Shell route, when available. |
| `Capabilities` | `AgentCapabilities?` | The features that the agent supports on the current platform. |

### AgentCapabilities

The `AgentCapabilities` class, returned by `AgentStatus.Capabilities`, describes which features the agent supports on the current platform. Each property is a `bool` that's `true` when the feature is available.

| Property | Description |
|----------|-------------|
| `Ui` | Visual tree inspection and UI interaction. |
| `Screenshots` | Screenshot capture. |
| `WebView` | WebView inspection through the Chrome DevTools Protocol. |
| `Network` | HTTP request capture. |
| `Logs` | App log retrieval. |
| `Sensors` | Device sensor data. |
| `Storage` | App storage inspection, such as preferences. |
| `Profiler` | Performance profiling. |
| `Jobs` | Background job inspection (Android, iOS, Mac Catalyst). |
| `Menus` | App menu inspection. |
| `Theme` | App theme inspection and switching. |
| `SyntheticTouch` | Gestures delivered as synthesized touches to views that handle raw touches, such as `GraphicsView`. Only available on iOS and Mac Catalyst when the app enables it. |
| `PointerStream` | Continuous pointer sessions. |

The following example checks whether screenshots are supported before capturing one:

```csharp
var status = await client.GetStatusAsync();
if (status?.Capabilities?.Screenshots == true)
{
    var png = await client.ScreenshotAsync();
}
```

### GetCapabilitiesAsync

```csharp
public Task<JsonElement> GetCapabilitiesAsync()
```

Returns the agent's raw capabilities document as a `JsonElement`, including details about registered extensions. If the agent can't be reached, it returns a default `JsonElement` whose `ValueKind` is `JsonValueKind.Undefined`. For typed feature flags, use `GetStatusAsync` and read `AgentStatus.Capabilities`.

## Find elements

UI actions such as `TapAsync` and `FillAsync` target elements by their DevFlow element ID, which is the `Id` property of an `ElementInfo`. For an element that sets an `AutomationId`, the element ID is usually the `AutomationId`. However, the agent adds a suffix when several elements share an `AutomationId`, and adds a window scope in secondary windows. Elements without an `AutomationId` get generated IDs. To act on an element reliably, query for it first and pass its `Id`.

### GetTreeAsync

```csharp
public Task<List<ElementInfo>> GetTreeAsync(int maxDepth = 0, int? window = null)
```

Returns the visual tree of the running app as a list of root elements. Each `ElementInfo` exposes its descendants through its `Children` property. Pass a `maxDepth` greater than `0` to limit the depth of the tree. Another overload accepts an `includeNative` argument that controls whether native view details are included. If the agent can't be reached, the method returns an empty list.

### QueryAsync

```csharp
public Task<List<ElementInfo>> QueryAsync(
    string? type = null,
    string? automationId = null,
    string? text = null)
```

Returns the elements that match all of the criteria that you specify. `type` matches the element's short or full type name, and `automationId` matches the element's `AutomationId`. Both are exact, case-insensitive matches. `text` matches elements whose text contains the specified value, ignoring case. If you don't specify any criteria, or the agent can't be reached, the method returns an empty list. To query with a CSS selector, use `QueryCssAsync(string selector)` instead.

The `ElementInfo` class includes properties such as `Id`, `Type`, `AutomationId`, `Text`, `IsVisible`, `IsEnabled`, `Bounds`, and `Children`.

## Capture screenshots

Use the following methods to capture the app's window or a single element as a PNG image.

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

Captures a PNG screenshot and returns a `ScreenshotResult` that describes success or failure. It takes the same parameters as `ScreenshotAsync`, but instead of collapsing every failure into `null`, it reports the cause of the failure. For example, when the agent doesn't support screenshots, it returns a failed result with `Reason` set to `not_supported`.

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
> On macOS (AppKit), DevFlow captures the app window without activating it or changing keyboard focus, so screenshots work when the app isn't frontmost or is covered by other windows. If a capture fails because the window is hidden or minimized, the result has `Reason` set to `window-not-visible` and `Retryable` set to `true`. Restore the window and retry. If native window capture is required, grant the **Screen & System Audio Recording** permission in **System Settings** > **Privacy & Security**.

## Interact with the app

The following methods return `true` if the agent reports that the action succeeded. They return `false` if the agent reports a failure, returns an HTTP error, or can't be reached. To find out why an action failed, use `TapResultAsync` or `FillResultAsync`, which return an `ActionResult` with `Success`, `Reason`, `Error`, and `Retryable` properties.

Because these methods change app state, they can throw the following exceptions, both of which derive from `InvalidOperationException`:

- `MutationLeaseException`, when `AutoAcquireMutationLease` is `true` (the default) and another client holds the agent's mutation lease.
- `NotSupportedByAgentException`, when the agent doesn't support the action on the current platform.

### TapAsync

```csharp
public Task<bool> TapAsync(string elementId)
```

Taps the element with the specified DevFlow element ID.

### FillAsync

```csharp
public Task<bool> FillAsync(string elementId, string text)
```

Fills text into the element, such as an `Entry` or `Editor`, with the specified DevFlow element ID.

### NavigateAsync

```csharp
public Task<bool> NavigateAsync(string route)
```

Navigates to the specified Shell route.

The following example finds elements by `AutomationId`, fills in a form, and submits it:

```csharp
var username = (await client.QueryAsync(automationId: "UsernameEntry")).FirstOrDefault();
var login = (await client.QueryAsync(automationId: "LoginButton")).FirstOrDefault();

if (username is null || login is null)
    throw new InvalidOperationException("Login form not found.");

if (!await client.FillAsync(username.Id, "testuser"))
    throw new InvalidOperationException("Couldn't fill the username.");

if (!await client.TapAsync(login.Id))
    throw new InvalidOperationException("Couldn't tap the login button.");
```

## Read logs and network traffic

Use the following methods to read the app's logs and the HTTP requests that it makes.

### GetLogsAsync

```csharp
public Task<string> GetLogsAsync(int limit = 100, int skip = 0, string? source = null)
```

Retrieves app log entries from the agent and returns the agent's JSON response as a string. Use `limit` and `skip` to page through entries, and `source` to filter by log source, such as `native` or `webview`. Unlike most `AgentClient` read methods, it throws an exception, such as `HttpRequestException`, if the agent can't be reached or returns an HTTP error.

### GetNetworkRequestsAsync

```csharp
public Task<List<NetworkRequest>> GetNetworkRequestsAsync(
    int limit = 100,
    string? host = null,
    string? method = null)
```

Returns captured HTTP requests, optionally filtered by host and HTTP method. Each `NetworkRequest` includes properties such as `Id`, `Method`, `Url`, `StatusCode`, `DurationMs`, and `Error`. If the agent can't be reached, the method returns an empty list. To get the headers and bodies of a specific request, call `GetNetworkRequestDetailAsync(string id)`. To clear captured requests, call `ClearNetworkRequestsAsync()`.

## Run background jobs

Use the following methods to inspect and trigger the app's platform background jobs.

### GetJobsAsync

```csharp
public Task<JsonElement> GetJobsAsync()
```

Returns a JSON representation of the platform's background jobs. See [`maui_jobs_list`](mcp-tools.md#maui_jobs_list) for the response schema.

**Platform support:** Android, iOS, Mac Catalyst.

### RunJobAsync

```csharp
public Task<JsonElement> RunJobAsync(string identifier, string? type = null)
```

Triggers a background job by identifier and returns the agent's JSON response. If the agent can't be reached, it returns a default `JsonElement`.

| Parameter | Description |
|-----------|-------------|
| `identifier` | The platform job identifier from `GetJobsAsync()`. |
| `type` | iOS only. Either `"processing"` or `"refresh"`. Optional. When omitted, the agent resolves the type from pending requests. |

**Platform support:** iOS, Mac Catalyst. (Android returns `success: false`.)

## See also

- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [MCP tools reference](mcp-tools.md)
