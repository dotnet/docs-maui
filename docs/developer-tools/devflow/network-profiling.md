---
title: "Network monitoring and profiling (experimental)"
description: "Learn how to monitor HTTP network traffic and profile application performance in .NET MAUI apps with DevFlow."
ms.date: 04/03/2026
---

# Network monitoring and profiling (experimental)

DevFlow provides tools for monitoring HTTP network traffic and profiling application performance, including CPU usage, memory allocation, GC events, and jank detection.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## Network monitoring

DevFlow intercepts HTTP requests and responses from the running app through the DevFlow agent. The agent captures request and response details including URLs, headers, status codes, timing, and payload sizes.

The CLI provides a TUI (text-based UI) for real-time monitoring of network traffic. This is useful for debugging API calls, verifying request payloads, and identifying slow or failing network requests during development.

## Performance profiling

DevFlow collects performance profiling data from the running app, including:

- **CPU usage** — Monitor CPU utilization over time to identify expensive operations.
- **Memory allocation** — Track memory allocations to detect leaks or excessive allocation patterns.
- **GC events** — Observe garbage collection events, including frequency, generation, and pause duration.
- **Jank detection** — Identify UI thread delays that cause visible stuttering or dropped frames.

Profiling data can be queried through the CLI or through [MCP tools](mcp-server.md) for AI-assisted analysis.

> [!NOTE]
> Detailed profiling documentation may evolve as the feature matures. Check the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository for the latest capabilities.

## See also

- [DevFlow overview](index.md)
