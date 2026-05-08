---
title: "MCP server for AI agents"
description: "Learn how DevFlow exposes a Model Context Protocol (MCP) server with 50+ tools for AI agent integration with running .NET MAUI apps."
ms.date: 05/08/2026
---

# MCP server for AI agents

DevFlow includes a Model Context Protocol (MCP) server that exposes 50+ structured tools for AI agent integration. This allows AI assistants like Claude, GitHub Copilot, and others to inspect, interact with, and debug running MAUI applications.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## What is MCP

Model Context Protocol (MCP) is a standard for AI tool integration. It defines a structured way for AI agents to discover and call tools exposed by a server. The DevFlow MCP server exposes MAUI app capabilities as structured tools that AI agents can call, enabling AI-assisted development, testing, and debugging workflows.

## Start the MCP server

Use the `mcp` command to start the MCP server:

```console
maui devflow mcp
```

This starts a stdio-based MCP server. AI agents connect to the server through stdio transport, discovering available tools and calling them to interact with the running MAUI app.

## Available tools

The MCP server exposes tools organized into the following categories:

| Category | Description |
|----------|-------------|
| Agent | Agent status and connected agent listing |
| Assert | UI assertions for testing |
| CDP | Chrome DevTools Protocol for Blazor WebViews |
| Interaction | Tap, fill, scroll UI elements |
| Logs | Application log retrieval |
| Navigation | Shell and page navigation |
| Network | HTTP request/response monitoring |
| Platform | Platform and device information |
| Preferences | App preferences read/write |
| Property | Element property inspection and mutation |
| Storage | File storage root discovery and sandboxed file management |
| Query | CSS selector-based element queries |
| Recording | Interaction recording |
| Screenshot | App screenshot capture |
| Sensor | Device sensor data |
| Tree | Visual tree inspection |

## Configure with AI agents

The MCP server uses stdio transport, which is the standard transport mechanism supported by most AI agent frameworks. Add the DevFlow MCP server to your AI tool's configuration:

```json
{
  "mcpServers": {
    "maui-devflow": {
      "command": "maui",
      "args": ["devflow", "mcp"]
    }
  }
}
```

Refer to your AI tool's documentation for the specific configuration file location and format.

## Example workflow

An AI agent connected to the DevFlow MCP server can perform end-to-end automation tasks. For example:

1. **Inspect** the visual tree to understand the current UI state.
1. **Query** for a specific button using a CSS selector.
1. **Tap** the button to trigger an action.
1. **Take a screenshot** to capture the resulting UI state.
1. **Assert** that the expected changes occurred.

All of these steps are performed through MCP tool calls, enabling fully automated AI-driven testing and debugging.

## Storage tools

The following tools enable AI agents to inspect and manage sandboxed app files on device or simulator.

| Tool | Purpose |
|------|---------|
| `maui_storage_roots` | List file storage roots advertised by the app. Call this before specifying a non-default root. |
| `maui_files_list` | List files and directories under an advertised app storage root. Optionally specify a subdirectory path. |
| `maui_files_download` | Download a file from an advertised app storage root. Returns the file content as base64. |
| `maui_files_upload` | Upload a file to an advertised app storage root. Content must be base64-encoded. Parent directories are created automatically. |
| `maui_files_delete` | Delete a file from an advertised app storage root. |

### Tool parameters

**`maui_storage_roots`**

| Parameter | Type | Description |
|-----------|------|-------------|
| `agentPort` | `int?` | Agent HTTP port (optional if only one agent connected) |

**`maui_files_list`**

| Parameter | Type | Description |
|-----------|------|-------------|
| `path` | `string?` | Subdirectory path relative to the selected storage root (optional, lists root if omitted) |
| `root` | `string?` | Storage root id (default: `appData`; call `maui_storage_roots` to discover supported roots) |
| `agentPort` | `int?` | Agent HTTP port (optional if only one agent connected) |

**`maui_files_download`**

| Parameter | Type | Description |
|-----------|------|-------------|
| `path` | `string` | File path relative to the selected storage root |
| `root` | `string?` | Storage root id (default: `appData`) |
| `agentPort` | `int?` | Agent HTTP port (optional if only one agent connected) |

**`maui_files_upload`**

| Parameter | Type | Description |
|-----------|------|-------------|
| `path` | `string` | File path relative to the selected storage root |
| `contentBase64` | `string` | File content encoded as base64 |
| `root` | `string?` | Storage root id (default: `appData`) |
| `agentPort` | `int?` | Agent HTTP port (optional if only one agent connected) |

**`maui_files_delete`**

| Parameter | Type | Description |
|-----------|------|-------------|
| `path` | `string` | File path relative to the selected storage root |
| `root` | `string?` | Storage root id (default: `appData`) |
| `agentPort` | `int?` | Agent HTTP port (optional if only one agent connected) |

## See also

- [DevFlow overview](index.md)
- [Visual tree inspection and screenshots](visual-tree-screenshots.md)
- [Element interaction and automation](element-interaction.md)
- [File storage access](storage.md)
