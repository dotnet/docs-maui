---
title: ".NET MAUI CLI overview"
description: "Learn about the .NET MAUI CLI, a command-line tool for environment setup, device management, and app automation."
ms.date: 05/07/2026
---

# .NET MAUI CLI overview

The .NET MAUI CLI (`maui`) is a command-line tool for development environment setup, device management, and app automation. It's distributed as a .NET global tool from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Install the CLI

Install the .NET MAUI CLI as a .NET global tool:

```bash
dotnet tool install -g Microsoft.Maui.Cli --prerelease
```

The CLI is currently in pre-release, so the `--prerelease` flag is required.

To update to the latest pre-release version:

```bash
dotnet tool update -g Microsoft.Maui.Cli --prerelease
```

## Verify installation

After installation, verify that the CLI is available by checking the version or displaying the help text:

```bash
maui version
```

```bash
maui --help
```

## Command reference

The following table lists the available `maui` CLI commands:

| Command | Description |
|---|---|
| `maui doctor` | Run environment diagnostics and auto-fix issues |
| `maui device list` | List connected devices and emulators |
| `maui version` | Display version information |
| `maui android install` | Full interactive Android environment setup |
| `maui android sdk list` | List available and installed Android SDK packages |
| `maui android sdk install` | Install Android SDK packages |
| `maui android jdk install` | Install and manage JDK versions |
| `maui android emulator create` | Create an Android emulator |
| `maui android emulator start` | Start an Android emulator |
| `maui android emulator stop` | Stop a running emulator |
| `maui android emulator delete` | Delete an emulator |
| `maui profile startup` | Profile app startup by capturing a trace from launch |
| `maui profile manual` | Launch the app and attach `dotnet-trace` on demand |
| `maui devflow` | DevFlow app automation and debugging |
| `maui devflow MAUI tree` | Dump the visual tree of a running MAUI app |
| `maui devflow MAUI screenshot` | Take a screenshot of a running MAUI app |
| `maui devflow cdp` | Blazor WebView automation via Chrome DevTools Protocol |
| `maui devflow mcp` | Start MCP server for AI agent integration |
| `maui devflow broker` | Manage the DevFlow agent broker |

For more information, see:

- [Environment diagnostics](environment-diagnostics.md)
- [Android SDK &amp; emulator management](android-management.md)
- [Device management](device-management.md)
- [App profiling](profile.md)
- [DevFlow overview](../devflow/index.md)

## Global options

The following options are available on all commands:

| Option | Description |
|---|---|
| `--json` | Output in JSON format for scripting and CI |
| `-v`, `--verbose` | Enable verbose output |
| `--dry-run` | Show what would be done without making changes |
| `--ci` | CI mode — non-interactive, fail fast on errors |

## Output formats

By default, the CLI renders human-friendly interactive output using [Spectre.Console](https://spectreconsole.net/). This includes color, tables, and progress indicators in the terminal.

For scripting and CI pipelines, use the `--json` flag to produce machine-readable JSON output instead:

```bash
# Human-friendly output
maui doctor

# JSON output for scripting
maui doctor --json
```

## Platform support

| Platform | Status |
|---|---|
| macOS | ✅ Supported |
| Windows | ✅ Supported |
| Linux | ✅ Supported |

## See also

- [Environment diagnostics](environment-diagnostics.md)
- [Android SDK &amp; emulator management](android-management.md)
- [Device management](device-management.md)
- [App profiling](profile.md)
- [DevFlow overview](../devflow/index.md)
