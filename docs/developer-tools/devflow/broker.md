---
title: "DevFlow broker architecture (experimental)"
description: "Learn about the DevFlow broker daemon that coordinates port assignment and agent discovery across multiple running .NET MAUI apps."
ms.date: 04/03/2026
---

# DevFlow broker architecture (experimental)

The DevFlow broker is a lightweight background daemon that coordinates port assignment and agent discovery across multiple running MAUI apps. It eliminates port collisions when debugging several MAUI apps (or the same app on different platforms) simultaneously.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## Overview

The broker is a thin registry, not a command proxy. Its key responsibilities are:

- Assigns unique ports to each MAUI agent from a shared pool (10223–10899).
- Tracks running agents so the CLI can discover them without manual `--agent-port` flags.
- Detects disconnections instantly via persistent WebSocket connections.
- Starts and stops automatically.

```
┌─────────────────────────────────┐
│        Broker Daemon            │
│     (port 19223, well-known)    │
│                                 │
│  Agent Registry (in-memory)     │
│  WebSocket /ws/agent ← agents  │
│  HTTP /api/agents   ← CLI      │
│                                 │
│  Auto-exit after 5 min idle     │
└───┬─────────────┬──────────────┘
    │             │
    │ Agent       │ CLI
    │ (WebSocket) │ (HTTP)
```

The CLI discovers an agent's port from the broker, then connects **directly** to the agent's HTTP server. There is no overhead on the inspection hot path.

## Agent registration

When a MAUI app starts with `AddMauiDevFlowAgent()`, the following registration sequence occurs:

1. The agent connects to the broker at `ws://localhost:19223/ws/agent`.
1. The agent sends a registration message containing the project path, target framework moniker (TFM), platform, and app name.
1. The broker assigns a free port from the pool (10223–10899) by performing a TCP bind test.
1. The agent starts its HTTP server on the assigned port.
1. The WebSocket connection stays open as a liveness signal.

## CLI discovery

When you run a CLI command, the following discovery sequence occurs:

1. The CLI ensures the broker is running.
1. The CLI queries `/api/agents` to find the right agent.
1. Auto-resolution follows this priority:
   - `--agent-port` flag (explicit override)
   - Project file (`.csproj`) + TFM match
   - Project file match only
   - Single agent (if only one is registered)
   - Print the full agent list and fall back to manual selection

## Agent identity

Each agent's identity is computed as:

```
ID = SHA256(csproj_path + "|" + TFM)[:12]
```

The same app running on different platforms receives different IDs because the TFM differs. Restarting an app replaces the old registration for the same identity.

## Broker lifecycle

- **Auto-start**: The broker starts transparently via `EnsureBrokerRunningAsync()`, which reads the `~/.mauidevflow/broker.json` state file to determine whether a broker process is already active.
- **Auto-exit**: The broker exits after 5 minutes with zero connected agents and no CLI requests.
- **Manual commands**:

  ```bash
  maui devflow broker start
  maui devflow broker stop
  maui devflow broker status
  maui devflow broker log
  ```

## List connected agents

Run `maui devflow list` to display a table of all registered agents:

| Column | Description |
|--------|-------------|
| ID | The 12-character agent identity hash |
| App | The application name |
| Platform | The target platform (Android, iOS, Mac Catalyst, Windows) |
| TFM | The target framework moniker |
| Port | The assigned HTTP port |
| Uptime | How long the agent has been connected |

## Graceful fallback

The broker is optional. When it's unavailable, both the agent and the CLI fall back through a chain of alternatives:

- **Agent fallback**: broker-assigned port → assembly metadata port → default port 9223.
- **CLI fallback**: broker query → `.mauidevflow` config file → default port 9223.
- The `--agent-port` flag always overrides the entire chain.

## Platform connectivity

| Platform | Agent → Broker | CLI → Agent |
|----------|---------------|-------------|
| Mac Catalyst | `localhost:19223` direct | `localhost:{port}` direct |
| Windows | `localhost:19223` direct | `localhost:{port}` direct |
| Linux/GTK | `localhost:19223` direct | `localhost:{port}` direct |
| iOS Simulator | Shares host network | `localhost:{port}` direct |
| Android Emulator | Requires `adb reverse` | Requires `adb reverse` |

For Android emulators, configure port forwarding for both the broker and the agent:

```bash
adb reverse tcp:19223 tcp:19223
adb reverse tcp:{port} tcp:{port}
```

Replace `{port}` with the assigned agent port.

## Troubleshooting

| Issue | Possible cause | Resolution |
|-------|---------------|------------|
| Broker won't start | Port 19223 is already in use | Stop the process using port 19223, or remove the stale `~/.mauidevflow/broker.json` state file and retry. |
| Agent not appearing | Broker not running, app not started, or firewall blocking the connection | Verify the broker is running with `maui devflow broker status`. Ensure the app includes `AddMauiDevFlowAgent()` and is running. Check firewall rules for the assigned port. |
| CLI can't connect to agent | Port mismatch, agent crashed, or missing ADB reverse on Android | Confirm the agent is listed in `maui devflow list`. For Android emulators, run the `adb reverse` commands. |
| Broker exits unexpectedly | Idle timeout (normal behavior) | The broker auto-exits after 5 minutes with no agents and no CLI requests. Check `maui devflow broker log` for details. |

## See also

- [DevFlow overview](index.md)
