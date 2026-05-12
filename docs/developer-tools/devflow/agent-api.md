---
title: "DevFlow agent HTTP API reference"
description: "Reference for the DevFlow agent HTTP API endpoints for inspecting and interacting with running .NET MAUI apps."
ms.date: 05/08/2026
---

# DevFlow agent HTTP API reference

The DevFlow agent exposes an HTTP API that you can call directly or via the CLI and MCP server. All endpoints are under `/api/v1/`.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Agent endpoints

### GET /api/v1/agent/status

Returns the agent status and capabilities.

### GET /api/v1/agent/capabilities

Returns an `AgentCapabilities` object describing which features the current platform supports.

## Visual tree endpoints

### GET /api/v1/tree

Returns the visual tree of the running app.

### GET /api/v1/tree/query

Queries elements in the visual tree using a CSS selector.

**Query parameters:** `selector` — CSS selector string.

## Element interaction endpoints

### POST /api/v1/interact/tap

Simulates a tap on a UI element.

**Request body:** `{ "automationId": "MyButton" }`

### POST /api/v1/interact/fill

Fills text into an `Entry` or `Editor` control.

**Request body:** `{ "automationId": "MyEntry", "text": "Hello" }`

### POST /api/v1/interact/scroll

Scrolls within a scrollable container.

**Request body:** `{ "automationId": "MyScrollView", "direction": "down" }`

### POST /api/v1/interact/navigate

Navigates to a Shell route or page.

**Request body:** `{ "route": "//MainPage/Details" }`

### POST /api/v1/interact/mutate

Changes an element property at runtime.

**Request body:** `{ "automationId": "MyLabel", "property": "Text", "value": "Updated" }`

## Screenshot endpoint

### GET /api/v1/screenshot

Captures a PNG screenshot of the running app.

## Logs endpoint

### GET /api/v1/logs

Retrieves application log entries.

## Network endpoints

### GET /api/v1/network/requests

Returns captured HTTP requests.

### DELETE /api/v1/network/requests

Clears captured HTTP request history.

## Preferences endpoints

### GET /api/v1/preferences/{key}

Reads an app preference value.

### PUT /api/v1/preferences/{key}

Writes an app preference value.

**Request body:** `{ "value": "myValue" }`

### DELETE /api/v1/preferences/{key}

Deletes an app preference.

## Platform endpoints

### GET /api/v1/device/info

Returns platform and device information.

### GET /api/v1/device/sensors/{sensor}

Returns sensor data for the specified sensor (for example, `accelerometer`, `gyroscope`, `location`).

### GET /api/v1/device/jobs

Returns platform background jobs known to the agent.

- **Android**: WorkManager workers (all states)
- **iOS / Mac Catalyst**: Pending BGTaskScheduler requests

Returns `501 Not Implemented` on platforms where jobs are not supported.

### POST /api/v1/device/jobs/{identifier}/run

Attempts to trigger the named background job.

- **iOS / Mac Catalyst**: Submits a `BGTaskRequest` for the identifier. Pass `type` in the request body (`"processing"` or `"refresh"`) when known.
- **Android**: Returns `success: false` — WorkManager workers cannot be reconstructed from identifiers.

**Path parameter:** `identifier` — platform job identifier returned by the list endpoint.

**Request body (optional):**

```json
{ "type": "refresh" }
```

Returns `501 Not Implemented` on platforms where job triggering is not supported.

## See also

- [DevFlow overview](index.md)
- [MCP tools reference](mcp-tools.md)
- [AgentClient API reference](agent-client.md)
