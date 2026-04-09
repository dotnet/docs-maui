---
title: ".NET MAUI developer tools overview"
description: "Learn about developer tools for improving your productivity using .NET MAUI, including the .NET MAUI CLI and DevFlow toolkit."
ms.date: 04/03/2026
---

# .NET MAUI developer tools overview

The .NET MAUI command line (CLI) tool and DevFlow toolkit enhance your development experience with automation, device management, in-app debugging, AI-assisted workflows, and much more. They are well suited for use with GitHub Copilot and AI assisted coding agents.

> [!IMPORTANT]
> The .NET MAUI developer tools described in this section are experimental and will change between releases.

## .NET MAUI CLI

The .NET MAUI CLI is a command-line tool for environment setup, device management, Android SDK and JDK and emulator management, and app automation. Install it as a .NET global tool:

```dotnetcli
dotnet tool install -g Microsoft.Maui.Cli --prerelease
```

Key capabilities include:

- **`maui doctor`** &ndash; Diagnose and repair your .NET MAUI development environment.
- **Device listing** &ndash; Enumerate connected physical devices and running emulators/simulators.
- **Android SDK management** &ndash; Install and manage Android SDKs, JDKs, and emulator images.
- **DevFlow integration** &ndash; Launch and coordinate DevFlow sessions from the command line.

For more information, see [.NET MAUI CLI overview](cli/index.md).

## DevFlow

DevFlow is a comprehensive testing, automation, and debugging toolkit for .NET MAUI apps. It consists of three components: an in-app agent that runs inside your app, a CLI that provides 50+ commands for interacting with the agent, and an MCP server that enables AI integration.

Key capabilities include:

- **Visual tree inspection** &ndash; Browse and query the live UI element hierarchy.
- **Screenshots** &ndash; Capture screenshots of your running app.
- **Element interaction** &ndash; Tap, swipe, type, and interact with UI elements programmatically.
- **Blazor WebView debugging** &ndash; Inspect and automate Blazor WebView content via CDP.
- **Network monitoring** &ndash; Observe HTTP traffic from your app.
- **Performance profiling** &ndash; Measure rendering and startup performance.

For more information, see [DevFlow overview](devflow/index.md).

## Packages

The following table lists the packages available from the dotnet/maui-labs repository:

| Package | Description |
|---|---|
| `Microsoft.Maui.Cli` | CLI global tool (`maui`) |
| `Microsoft.Maui.DevFlow.Agent` | In-app agent for .NET MAUI automation |
| `Microsoft.Maui.DevFlow.Agent.Core` | Platform-agnostic agent core |
| `Microsoft.Maui.DevFlow.Agent.Gtk` | GTK/Linux agent |
| `Microsoft.Maui.DevFlow.Blazor` | Blazor WebView CDP bridge |
| `Microsoft.Maui.DevFlow.Blazor.Gtk` | WebKitGTK CDP bridge |
| `Microsoft.Maui.DevFlow.Driver` | Platform driver library |
| `Microsoft.Maui.DevFlow.Logging` | Buffered JSONL file logger |

## Source code

The source code for these tools is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository on GitHub.

## Support and feedback

These experimental tools are community-supported. To report issues or request features, file an issue in the [dotnet/maui-labs issue tracker](https://github.com/dotnet/maui-labs/issues).
