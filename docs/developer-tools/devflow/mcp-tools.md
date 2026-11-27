---
title: "MCP tools reference"
description: "Reference for all MCP tools exposed by the DevFlow MCP server for AI agent integration with running .NET MAUI apps."
ms.date: 05/08/2026
---

# MCP tools reference

The DevFlow MCP server exposes structured tools that AI agents can call to inspect, interact with, and debug running .NET MAUI applications. Tools are organized into categories.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Tools

### Agent tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_agent_list` | List all connected agents and their connection details. | All |
| `maui_agent_status` | Get the status and capabilities of a connected agent. | All |

### Assert tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_assert_exists` | Assert that an element matching a selector exists in the visual tree. | All |
| `maui_assert_not_exists` | Assert that no element matching a selector exists. | All |
| `maui_assert_property` | Assert that an element property matches an expected value. | All |

### CDP tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_cdp_evaluate` | Evaluate JavaScript in a Blazor WebView using Chrome DevTools Protocol. | All |
| `maui_cdp_navigate` | Navigate to a URL in a Blazor WebView. | All |

### Interaction tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_interact_tap` | Tap a UI element by automation ID. | All |
| `maui_interact_fill` | Fill text into an Entry or Editor by automation ID. | All |
| `maui_interact_scroll` | Scroll within a scrollable container by automation ID. | All |
| `maui_interact_navigate` | Navigate to a Shell route or page. | All |
| `maui_interact_mutate` | Change an element property at runtime. | All |

### Log tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_logs_get` | Retrieve application log entries from the agent. | All |

### Navigation tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_navigation_current` | Get the current Shell route or page type. | All |
| `maui_navigation_back` | Navigate back in the navigation stack. | All |

### Network tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_network_requests` | Retrieve captured HTTP requests from the agent. | All |
| `maui_network_clear` | Clear the captured HTTP request history. | All |

### Platform tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_platform_info` | Get platform and device information. | All |

### Preferences tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_preferences_get` | Read an app preference value by key. | All |
| `maui_preferences_set` | Write an app preference value by key. | All |
| `maui_preferences_delete` | Delete an app preference by key. | All |

### Property tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_property_get` | Get a property value from an element. | All |
| `maui_property_set` | Set a property value on an element. | All |

### Query tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_query_select` | Query elements using a CSS selector. | All |

### Recording tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_recording_start` | Start recording user interactions. | All |
| `maui_recording_stop` | Stop recording and retrieve interaction script. | All |

### Screenshot tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_screenshot_capture` | Capture a PNG screenshot of the running app. | All |

### Sensor tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_sensors_accelerometer` | Read accelerometer data from the device. | Android, iOS, Mac Catalyst |
| `maui_sensors_gyroscope` | Read gyroscope data from the device. | Android, iOS, Mac Catalyst |
| `maui_sensors_location` | Read GPS location from the device. | Android, iOS, Mac Catalyst |

### Background jobs tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_jobs_list` | List background jobs registered on the device (Android Workers via WorkManager, iOS BGTasks via BGTaskScheduler). | Android, iOS, Mac Catalyst |
| `maui_jobs_run` | Trigger a supported background job by identifier. Android jobs can be listed but cannot be safely re-run; iOS submits a BGTaskRequest for the given identifier. | iOS, Mac Catalyst |

### Tree tools

| Tool | Description | Platforms |
|------|-------------|-----------|
| `maui_tree_get` | Get the visual tree of the running app. | All |
| `maui_tree_find` | Find elements in the visual tree using a CSS selector. | All |

---

## Tool details

### maui_jobs_list

Lists background jobs registered on the device.

**Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `agentPort` | `int` | No | Agent HTTP port (optional if only one agent connected) |

**Platform support:** Android (WorkManager), iOS, Mac Catalyst (BGTaskScheduler)

**Example response (Android):**

```json
{
  "platform": "Android",
  "type": "WorkManager",
  "supported": true,
  "runSupported": false,
  "jobs": [
    {
      "identifier": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "tags": ["com.example.SyncWorker"],
      "state": "ENQUEUED",
      "runAttemptCount": 0
    }
  ]
}
```

**Example response (iOS):**

```json
{
  "platform": "iOS",
  "type": "BGTaskScheduler",
  "supported": true,
  "runSupported": true,
  "jobs": [
    {
      "identifier": "com.example.refresh",
      "type": "refresh",
      "earliestBeginDate": ""
    }
  ]
}
```

---

### maui_jobs_run

Triggers a supported background job by identifier.

> [!NOTE]
> Android jobs cannot be re-triggered via this tool (`runSupported: false`). Only iOS and Mac Catalyst support job triggering (submits a `BGTaskRequest` to `BGTaskScheduler`).

**Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `identifier` | `string` | Yes | Job identifier returned by `maui_jobs_list` |
| `type` | `string` | No | iOS BGTask type: `processing` or `refresh`. If omitted, resolved from pending requests when possible. |
| `agentPort` | `int` | No | Agent HTTP port (optional if only one agent connected) |

## See also

- [MCP server for AI agents](mcp-server.md)
- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [AgentClient API reference](agent-client.md)
