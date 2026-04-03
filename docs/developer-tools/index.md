---
title: ".NET MAUI developer tools overview (experimental)"
description: "Learn about experimental developer tools for .NET MAUI from the dotnet/maui-labs repository, including the .NET MAUI CLI and DevFlow toolkit."
ms.date: 04/03/2026
---

# .NET MAUI developer tools overview (experimental)

The [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository hosts experimental packages and tooling for .NET MAUI development. These tools extend the .NET MAUI development experience with command-line automation, device management, in-app debugging, and AI-assisted workflows.

> [!IMPORTANT]
> The .NET MAUI developer tools described in this section are **experimental** packages from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. They are **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

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

The following table lists the experimental packages available from the dotnet/maui-labs repository:

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

These experimental tools are community-supported. To report issues or request features, file an issue in the [dotnet/maui-labs issue tracker](https://github.com/dotnet/maui-labs/issues). For additional guidance on support options, see the repository's [SUPPORT.md](https://github.com/dotnet/maui-labs/blob/main/SUPPORT.md).
