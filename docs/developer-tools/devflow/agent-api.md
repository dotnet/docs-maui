---
title: "DevFlow agent HTTP API reference"
description: "Reference for the DevFlow agent HTTP API endpoints for inspecting and interacting with running .NET MAUI apps."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# DevFlow agent HTTP API reference

The in-app DevFlow agent exposes an HTTP API for inspecting and interacting with a running .NET MAUI app. The CLI and MCP server also use this API. For agent setup, see [DevFlow overview](index.md).

> [!IMPORTANT]
> DevFlow is experimental and can change between releases. Enable the agent only in development builds because it exposes app data and can perform actions in your app.

## Connect to the agent

The agent listens on IPv4 loopback at `http://127.0.0.1:<agent-port>`. With your app running and registered with the broker, find its port:

```bash
maui devflow list
```

The broker normally assigns the agent port. Port `9223` is the agent's fallback when no configured, build-specified, or broker-assigned port is available. The broker's default port is `19223`; it isn't the agent HTTP port. For details, see [Broker architecture](broker.md).

The following examples use Bash and `curl`. Replace `10223` with your agent's port:

```bash
AGENT_PORT=10223
BASE_URL="http://127.0.0.1:${AGENT_PORT}"
```

For an Android device or emulator, the agent listens on the device's loopback interface. To reach it from the host, forward the agent port with `adb forward`:

```bash
adb forward "tcp:${AGENT_PORT}" "tcp:${AGENT_PORT}"
```

Broker registration can also require a reverse connection to the broker. For device setup, see [DevFlow Android setup](setup-android.md).

## API versioning

Agent HTTP endpoints use the `/api/v1/` prefix. WebSocket channels use `/ws/v1/`. The unversioned endpoints from the original `Redth/MauiDevFlow` project aren't compatibility aliases in the `Microsoft.Maui.DevFlow` packages.

## Agent endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/v1/agent/status` | Get the agent, app, and device status. |
| `GET` | `/api/v1/agent/capabilities` | Get features supported by the current agent and platform. |
| `POST` | `/api/v1/agent/lease` | Claim, check, renew, or release a mutation lease. |
| `POST` | `/api/v1/agent/recording` | Start, stop, or check workflow mutation recording. |

Check the agent status and capabilities before using platform-specific features:

```bash
curl --fail-with-body "$BASE_URL/api/v1/agent/status"
curl --fail-with-body "$BASE_URL/api/v1/agent/capabilities"
```

## Visual tree endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/v1/ui/tree` | Get the visual tree. |
| `GET` | `/api/v1/ui/elements` | Query elements. |
| `GET` | `/api/v1/ui/elements/{id}` | Get an element by its returned ID. |
| `GET`, `POST` | `/api/v1/ui/hit-test` | Find elements at window-logical coordinates using `x` and `y` query parameters. |
| `GET` | `/api/v1/ui/diagnostics/layout/rules` | List layout diagnostic rules. |
| `POST` | `/api/v1/ui/diagnostics/layout` | Analyze the current layout. |

For `/api/v1/ui/elements`, use the `selector`, `automationId`, `type`, or `text` query parameter. For example, query an element by its `AutomationId` with a CSS selector:

```bash
curl --fail-with-body --get \
  --data-urlencode 'selector=#MyButton' \
  "$BASE_URL/api/v1/ui/elements"
```

Replace `MyButton` with an `AutomationId` in your app. The response is an array, which is empty if nothing matches. For element actions, use an element's returned `id`, not its `AutomationId`.

> [!NOTE]
> The upstream OpenAPI specification currently describes different query parameters for `/api/v1/ui/elements` and a JSON body for `POST /api/v1/ui/hit-test`. The .NET MAUI agent uses the query parameters shown here.

The layout diagnostic routes are experimental and disabled by default. To enable them, set `EnableLayoutDiagnostics` when you register the agent in `MauiProgram.cs`:

```csharp
#if DEBUG
builder.AddMauiDevFlowAgent(options =>
{
    options.EnableLayoutDiagnostics = true;
});
#endif
```

Update your existing agent registration rather than adding a second registration. When diagnostics are disabled, the routes return HTTP 404. Support can also vary by backend.

## Element interaction endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| `POST` | `/api/v1/ui/actions/tap` | Tap an element. |
| `POST` | `/api/v1/ui/actions/fill` | Fill an input element with text. |
| `POST` | `/api/v1/ui/actions/scroll` | Scroll a view. |
| `POST` | `/api/v1/ui/actions/navigate` | Navigate to a route. |
| `GET`, `PUT` | `/api/v1/ui/elements/{id}/properties/{name}` | Read or set an element property. |

Action requests use the returned element ID in the `elementId` field when an element is required. See the [OpenAPI specification](https://github.com/dotnet/maui-labs/blob/main/docs/DevFlow/spec/openapi.yaml) for request schemas and more actions.

## Mutation lease

By default, requests that change the app require a mutation lease. Claim a lease with a unique client ID, and include that ID in the `X-DevFlow-Lease` header on protected requests. The `POST` hit-test endpoint also requires a lease by default, even though it doesn't change the app. A request without an accepted lease returns HTTP 409.

Replace `my-unique-client-id` with a unique ID for your client, and claim the lease:

```bash
LEASE_ID="my-unique-client-id"
curl --fail-with-body --header 'Content-Type: application/json' \
  --data "{\"action\":\"claim\",\"leaseId\":\"${LEASE_ID}\"}" \
  "$BASE_URL/api/v1/agent/lease"
```

Continue only if the response reports `"youHold": true`. An `"ok": true` response alone doesn't mean that the lease was granted. Don't force a takeover from another client unless you intend to interrupt it.

For example, send a hit test at window-logical coordinates `(100, 100)`:

```bash
curl --fail-with-body --request POST \
  --header "X-DevFlow-Lease: ${LEASE_ID}" \
  "$BASE_URL/api/v1/ui/hit-test?x=100&y=100"
```

Release the lease when you're finished:

```bash
curl --fail-with-body --header 'Content-Type: application/json' \
  --data "{\"action\":\"release\",\"leaseId\":\"${LEASE_ID}\"}" \
  "$BASE_URL/api/v1/agent/lease"
```

By default, a lease expires after 10 seconds without a successful mutation or a heartbeat. To keep it active, send `"action": "heartbeat"` with the same `leaseId` to `/api/v1/agent/lease`. You can check lease status with `"action": "status"`. Read-only `GET` requests don't require a lease.

Workflow recording requires a connected broker. Checking recording status doesn't require a lease, but starting or stopping recording does. Layout analysis is read-only and doesn't require a lease.

## Screenshot endpoint

`GET /api/v1/ui/screenshot` captures a PNG screenshot of the running app.

## Logs endpoint

`GET /api/v1/logs` retrieves application log entries.

## Network endpoints

The network endpoints provide access to captured HTTP requests.

### GET /api/v1/network/requests

Returns captured HTTP requests.

### GET /api/v1/network/requests/{id}

Returns details for a captured request.

### DELETE /api/v1/network/requests

Clears captured HTTP request history.

## Preferences endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| `GET`, `DELETE` | `/api/v1/storage/preferences` | List or clear app preferences. |
| `GET`, `PUT`, `DELETE` | `/api/v1/storage/preferences/{key}` | Read, write, or delete a preference. |

## Platform endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| `GET` | `/api/v1/device/info` | Get platform and device information. |
| `GET` | `/api/v1/device/sensors` | List sensors available on the device. |
| `POST` | `/api/v1/device/sensors/{name}/start` | Start a sensor. |
| `POST` | `/api/v1/device/sensors/{name}/stop` | Stop a sensor. |
| `GET` | `/api/v1/device/jobs` | List platform background jobs. |
| `POST` | `/api/v1/device/jobs/{identifier}/run` | Attempt to trigger a background job. |

Feature availability and job execution vary by platform. Check `/api/v1/agent/capabilities`; an unsupported operation can return HTTP 501. A request to trigger a job doesn't establish that the job has completed.

On Android, listing jobs requires WorkManager, and running a job through the HTTP endpoint returns `"success": false`. On iOS and Mac Catalyst, the list contains pending `BGTaskScheduler` requests. A debug-built agent can request a debugger launch, but the response doesn't verify execution or completion. A release-built agent doesn't support running jobs.

## See also

- [DevFlow overview](index.md)
- [MCP tools reference](mcp-tools.md)
- [AgentClient API reference](agent-client.md)
