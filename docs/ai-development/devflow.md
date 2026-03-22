---
title: AI-assisted UI debugging with MauiDevFlow
description: Learn how to use MauiDevFlow to inspect the visual tree, capture screenshots, and debug UI issues in .NET MAUI apps with AI coding assistants.
ms.topic: how-to
ms.date: 03/22/2026
no-loc: [".NET MAUI"]
---

# AI-assisted UI debugging with MauiDevFlow

MauiDevFlow is an experimental toolkit from maui-labs that provides an HTTP API and CLI for visual tree inspection, element interaction, screenshots, and more in .NET MAUI apps. It's particularly powerful when combined with AI coding assistants, enabling them to inspect your running app's UI and diagnose layout and rendering issues in real time. If you're familiar with Xcode Instruments or Chrome DevTools, MauiDevFlow provides a similar level of runtime introspection for .NET MAUI apps, with the added benefit of AI agent integration through the Model Context Protocol.

> [!WARNING]
> MauiDevFlow is experimental and its APIs may change without notice. It is intended for local development and debugging only — never include DevFlow packages in production builds. The `#if DEBUG` guard and the `Condition="'$(Configuration)' == 'Debug'"` in the project file ensure DevFlow is excluded from release builds. The source is available at [github.com/dotnet/maui-labs](https://github.com/dotnet/maui-labs).

## Who should use MauiDevFlow

MauiDevFlow is an optional, advanced tool. Consider using it when:

- You're doing **heavy UI debugging** — diagnosing layout problems, clipped elements, or unexpected rendering.
- You're working with **complex layouts** where the visual tree is deeply nested and hard to reason about from code alone.
- You're building **Blazor Hybrid** apps and need to inspect the WebView DOM alongside the native visual tree.
- Standard tools like **XAML Hot Reload** and **Live Visual Tree** aren't providing enough visibility into the issue.

If your debugging needs are met by the built-in Visual Studio tools, you don't need MauiDevFlow. It's designed for scenarios where deeper, programmatic access to the running app's UI state is required.

> [!NOTE]
> MauiDevFlow is strictly for interactive debugging during development. It's not designed for automated CI/CD test runs or headless environments.

## What MauiDevFlow provides

MauiDevFlow exposes a rich set of capabilities for inspecting and interacting with a running .NET MAUI app:

- **Visual tree inspection** — query the full element hierarchy, including layout properties, binding contexts, and rendered bounds.
- **Element interaction** — programmatically tap, fill text fields, and scroll within your running app.
- **Screenshots** — capture the current state of the app as an image, useful for verifying layout changes.
- **Network monitoring** — observe outgoing HTTP requests and responses from the app.
- **Performance profiling** — collect timing data for rendering and layout passes.
- **Blazor WebView CDP bridge** — connect to Blazor Hybrid WebViews through the Chrome DevTools Protocol for DOM inspection and JavaScript evaluation.
- **MCP server** — exposes 50+ structured tools through the [Model Context Protocol](https://modelcontextprotocol.io), allowing AI agents to call DevFlow capabilities directly.

## Platform support

| Platform | Status |
|---|---|
| Mac Catalyst | ✅ Supported |
| iOS Simulator | ✅ Supported |
| Linux/GTK | ✅ Supported |
| Android | 🔄 In progress |
| Windows | 🔄 In progress |

## Prerequisites

Before setting up MauiDevFlow, ensure you have the following:

- .NET 9 SDK or later
- A .NET MAUI project targeting a supported platform
- The .NET CLI available in your terminal

For AI assistant integration, you also need an AI coding tool that supports MCP, such as GitHub Copilot, Claude, or Cursor.

## Set up MauiDevFlow

Setting up MauiDevFlow involves three steps: adding NuGet packages to your project, registering the agent in your app startup code, and installing the CLI tool.

### Install the NuGet packages

Add the DevFlow NuGet packages to your project file. Wrapping them in a `Debug` condition ensures they're excluded from release builds:

```xml
<ItemGroup Condition="'$(Configuration)' == 'Debug'">
  <PackageReference Include="Microsoft.Maui.DevFlow.Agent" Version="*-*" />
  <PackageReference Include="Microsoft.Maui.DevFlow.Blazor" Version="*-*" />
</ItemGroup>
```

> [!TIP]
> The `*-*` version pattern pulls the latest prerelease package. Pin to a specific version if you need reproducible builds.

### Register the agent

In your `MauiProgram.cs`, register the DevFlow agent inside a `#if DEBUG` directive so it's stripped from release builds:

```csharp
var builder = MauiApp.CreateBuilder();
builder.UseMauiApp<App>();

#if DEBUG
builder.AddMauiDevFlowAgent();
#endif
```

### Install the CLI tool

Install the DevFlow CLI as a global .NET tool. The `-g` flag installs the tool globally, making it available from any directory in your terminal rather than only within a specific project:

```dotnetcli
dotnet tool install -g Microsoft.Maui.DevFlow.CLI
```

After installation, the `maui-devflow` command is available in your terminal.

### Verify the setup

Build and run your app in `Debug` configuration, then confirm that the agent is reachable:

```bash
maui-devflow agent status
```

A successful connection produces output similar to:

```
✅ Agent is running
   Version: 1.0.0-preview.3
   Platform: maccatalyst
   App: com.companyname.myapp
```

If the connection is successful, the command reports the agent version and the platform the app is running on.

## Key CLI commands

The CLI communicates with the DevFlow agent running inside your app. The following commands cover the most common workflows:

### Agent commands

```bash
maui-devflow agent status        # Check agent connection
maui-devflow agent tree          # Visual tree inspection
maui-devflow agent screenshot    # Capture screenshot
maui-devflow agent interact tap  # Interact with elements
maui-devflow agent logs          # Application ILogger output
```

### Blazor WebView (CDP) commands

```bash
maui-devflow cdp status          # Blazor WebView connection status
maui-devflow cdp snapshot        # DOM snapshot (useful for AI)
```

> [!NOTE]
> Run `maui-devflow --help` for a full list of commands and options.

## Platform-specific setup notes

Most platforms work without additional configuration, but some require extra steps.

### Android

After deploying your app to a device or emulator, set up port forwarding so the CLI can reach the agent:

```bash
adb reverse tcp:9223 tcp:9223
```

> [!IMPORTANT]
> You must run the `adb reverse` command each time you redeploy to the device or restart the emulator.

Once port forwarding is active, verify the connection:

```bash
maui-devflow agent status
```

### Mac Catalyst

No additional configuration is required. The agent is accessible over localhost once the app is running. Launch your app from Visual Studio or the terminal and use the CLI immediately.

### iOS Simulator

No additional configuration is required. The agent is accessible over localhost once the app is running in the simulator. DevFlow's agent listens on port 9223 by default.

## Using with AI assistants

MauiDevFlow is designed to work with AI coding assistants such as GitHub Copilot. When an AI agent has access to DevFlow (for example, through the MCP server), it can perform the following actions against your running app.

### Typical AI debugging workflow

A typical AI-assisted debugging session with MauiDevFlow follows this pattern:

1. You describe a visual or layout issue to the AI assistant (for example, "the login button is hidden behind the keyboard").
1. The AI agent calls `maui-devflow agent tree` to inspect the visual hierarchy and identify the element.
1. The agent calls `maui-devflow agent screenshot` to see the current rendered state.
1. The agent analyzes the tree data and screenshot, identifies the root cause, and proposes a code fix.
1. After you apply the fix, the agent takes another screenshot to verify the result.

### Inspect the visual tree

The agent can query the full element hierarchy to understand the current UI state. This is useful when the AI needs to determine why an element isn't visible, why a layout isn't rendering as expected, or which element a user is referring to.

### Take screenshots

The agent can capture a screenshot of the running app to verify layout changes visually. This allows the AI to confirm that a code change produced the intended result without requiring you to describe the screen manually.

### Read DOM snapshots for Blazor Hybrid apps

For apps using Blazor Hybrid, the agent can capture a DOM snapshot through the CDP bridge. This gives the AI full visibility into the HTML structure rendered inside the `BlazorWebView`, which is especially helpful for diagnosing CSS or component rendering issues.

### Blazor Hybrid debugging workflow

When debugging Razor component rendering issues in a Blazor Hybrid app, the AI agent can combine native visual tree inspection with CDP DOM snapshots:

1. You describe a rendering issue (for example, "my `WeatherList` Razor component shows a blank area instead of the forecast cards").
1. The agent calls `maui-devflow agent tree` to confirm the `BlazorWebView` is present and correctly sized in the native layout.
1. The agent calls `maui-devflow cdp snapshot` to capture the DOM inside the WebView, revealing the rendered HTML and CSS state of the Razor component.
1. The agent identifies the root cause in the DOM (for example, a missing CSS class, a conditional rendering block evaluating to `false`, or a failed data binding) and proposes a fix to the `.razor` or `.css` file.
1. After you apply the fix and hot reload, the agent takes another CDP snapshot to verify the component renders correctly.

### Check logs for runtime errors

The agent can read `ILogger` output from the running app. This enables the AI to correlate UI issues with runtime exceptions, binding errors, or other diagnostic messages.

### MCP server integration

The MCP server exposes 50+ structured tools that AI agents can call directly. When configured as an MCP tool provider, DevFlow gives the AI agent the ability to autonomously inspect, interact with, and diagnose issues in your running app. This transforms the AI from a code-only assistant into one that understands your app's live runtime state.

The tools are organized into the following categories:

- **Visual tree inspection** — query elements, layout properties, binding contexts, and rendered bounds.
- **Element interaction** — tap, type text, scroll, and navigate within the running app.
- **Screenshot capture** — capture the current app state as an image for visual verification.
- **Network monitoring** — observe outgoing HTTP requests and responses.
- **Performance profiling** — collect timing data for rendering and layout passes.
- **Blazor WebView / CDP commands** — inspect the DOM, evaluate JavaScript, and debug Blazor Hybrid content.
- **Application logging** — read `ILogger` output and diagnostic messages from the running app.

For the full tool reference including parameters and response formats, see the [MauiDevFlow documentation in the maui-labs repository](https://github.com/dotnet/maui-labs).

MCP is supported by a growing number of AI coding tools, including **GitHub Copilot** (in VS Code, Visual Studio, and the CLI), **Claude Code**, **Cursor**, and **Windsurf**. Any AI tool that supports the [Model Context Protocol](https://modelcontextprotocol.io) specification can use DevFlow as a tool provider.

To use DevFlow as an MCP server, point your AI tool's MCP configuration at the DevFlow CLI. The exact configuration depends on your AI tool. For example, in a `.json` configuration file:

```json
{
  "mcpServers": {
    "maui-devflow": {
      "command": "maui-devflow",
      "args": ["mcp"]
    }
  }
}
```

> [!TIP]
> For best results, describe the visual problem you're seeing and let the AI agent use DevFlow to investigate. The agent can combine tree inspection, screenshots, and log reading to diagnose issues faster than manual debugging.

## Troubleshooting

If the CLI can't connect to the agent, verify the following:

- The app is running in `Debug` configuration.
- The `AddMauiDevFlowAgent()` call is present in `MauiProgram.cs`.
- For Android, the `adb reverse` command has been run after deployment.
- No firewall or network policy is blocking localhost on port 9223.

If the CLI connects but returns empty or unexpected results:

- Ensure the page you want to inspect has fully loaded before querying the visual tree.
- For Blazor Hybrid apps, confirm that the `Microsoft.Maui.DevFlow.Blazor` package is installed and the `BlazorWebView` has finished rendering.
- Check the app's debug output for DevFlow-related error messages.

## See also

- [Best practices for AI-assisted .NET MAUI development](best-practices.md)
- [Write a copilot-instructions.md file for .NET MAUI projects](copilot-instructions.md)
- [MauiDevFlow source and documentation (github.com/dotnet/maui-labs)](https://github.com/dotnet/maui-labs)
