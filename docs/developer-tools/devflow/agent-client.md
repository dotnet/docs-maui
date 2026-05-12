---
title: "AgentClient API reference"
description: "Reference for the AgentClient class in Microsoft.Maui.DevFlow.Driver for programmatic interaction with DevFlow agents from .NET code."
ms.date: 05/08/2026
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

## See also

- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [MCP tools reference](mcp-tools.md)
