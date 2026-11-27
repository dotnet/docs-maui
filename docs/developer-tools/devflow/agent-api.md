---
title: "DevFlow agent HTTP API reference"
description: "Reference for the DevFlow agent HTTP API endpoints for inspecting and interacting with running .NET MAUI apps."
ms.date: 11/27/2026
ai-usage: ai-assisted
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

Network endpoints capture HTTP traffic from the app's .NET code, such as requests sent with `HttpClient`. They don't capture `fetch` or `XMLHttpRequest` calls made by JavaScript running in a `BlazorWebView`.

> [!NOTE]
> Earlier previews exposed `/api/v1/webview/network` as an alias of `/api/v1/network/requests`. This endpoint has been removed and now returns `404 Not Found`. Use `/api/v1/network/requests` instead.

## WebView endpoints

WebView endpoints interact with Blazor WebViews that are registered with the DevFlow Blazor bridge. Most WebView endpoints accept an optional `webview` query parameter that targets a WebView by index, `AutomationId`, or element ID. A numeric value is matched against the registry index before `AutomationId`. When you omit it, the agent selects a default WebView, preferring WebViews on the current page and then the most recently registered one. The default can change as pages and WebViews load. For the full matching and default-selection order, see [Target a WebView](blazor-cdp.md#target-a-webview). When your app has more than one WebView, specify the target explicitly. Except for `POST /api/v1/webview/evaluate`, these endpoints also accept `contextId` as an alias of `webview`. If you specify both, `webview` takes precedence.

### GET /api/v1/webview/contexts

Lists the registered WebViews. Each entry includes `id`, `index`, `automationId`, `elementId`, `url`, `title`, `ready`, `isReady`, and `active`. The `id` value is the `AutomationId` if one is set, otherwise the element ID, otherwise the index.

### POST /api/v1/webview/evaluate

Evaluates a JavaScript expression in the target WebView.

### GET /api/v1/webview/source

Returns the HTML source of the target WebView. `GET /api/v1/webview/dom` is an alias.

### POST /api/v1/webview/dom/query

Queries the DOM of the target WebView with a CSS selector.

### POST /api/v1/webview/navigate

Navigates the target WebView to a URL.

### POST /api/v1/webview/input/click

Clicks the element that matches a CSS selector in the target WebView.

### POST /api/v1/webview/input/fill

Fills text into the element that matches a CSS selector in the target WebView.

### POST /api/v1/webview/input/text

Inserts text at the current focus in the target WebView.

### GET /api/v1/webview/screenshot

Captures a screenshot of the target WebView.

### GET /api/v1/webview/console

Returns WebView console log entries. This endpoint is an alias of `GET /api/v1/logs` filtered to WebView logs, and it ignores the `webview` and `contextId` parameters.

The following example looks up the agent port for an app named `MyApp` and then reads the HTML source of the WebView whose `AutomationId` is `Right`. The [DevFlow broker](broker.md) usually assigns the agent port, so use `maui devflow list` to find it instead of assuming the default port 9223:

```bash
PORT=$(maui devflow list --json | jq -r '.[] | select(.appName == "MyApp") | .port')
curl "http://localhost:$PORT/api/v1/webview/source?webview=Right"
```

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
- [Blazor WebView debugging](blazor-cdp.md)
