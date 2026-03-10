---
title: ".NET MAUI CLI reference"
description: "Learn how to use the maui CLI tool to check your development environment, manage Android and Apple SDKs, and set up emulators and simulators for .NET MAUI development."
ms.date: 03/10/2026
monikerRange: ">=net-maui-11.0"
---

# .NET MAUI CLI reference

The `maui` CLI is a unified command-line tool for managing your .NET MAUI development environment. It provides environment health checks, automated fixes, and device management across Android, iOS, Mac Catalyst, and Windows.

The CLI is designed for three consumers in priority order: AI agents, CI/CD pipelines, and developers. Every command supports structured `--json` output for machine consumption.

## Installation

The `maui` CLI is included with the .NET MAUI workload starting in .NET 11. After installing the .NET 11 SDK and the MAUI workload, the `maui` command is available globally:

```dotnetcli
dotnet workload install maui
maui --version
```

## Global options

All commands support the following options:

| Option | Description |
|--------|-------------|
| `--json` | Output results as structured JSON |
| `--verbose` | Enable verbose logging |
| `--dry-run` | Show what would be done without making changes |
| `--interactive` | Control interactive prompts (auto-detects CI) |

## Environment health check

Use `maui doctor` to check your development environment for common issues:

```dotnetcli
maui doctor
```

To automatically fix detected issues:

```dotnetcli
maui doctor --fix
```

To check a specific platform:

```dotnetcli
maui doctor --platform android
```

## Android commands

The `maui android` commands manage your Android development environment, wrapping native tools like `sdkmanager` and `avdmanager`.

### Set up Android environment

Install JDK, Android SDK, and recommended packages in a single command:

```dotnetcli
maui android install --accept-licenses
```

### JDK management

| Command | Description |
|---------|-------------|
| `maui android jdk check` | Check JDK installation status |
| `maui android jdk install` | Install OpenJDK (default: 21) |
| `maui android jdk list` | List installed JDK versions |

### SDK management

| Command | Description |
|---------|-------------|
| `maui android sdk list` | List installed SDK packages |
| `maui android sdk list --available` | Show available packages |
| `maui android sdk install <packages>` | Install SDK package(s) |
| `maui android sdk accept-licenses` | Accept all SDK licenses |

### Emulator management

| Command | Description |
|---------|-------------|
| `maui android emulator list` | List emulators |
| `maui android emulator create <name>` | Create an emulator |
| `maui android emulator start <name>` | Start an emulator |
| `maui android emulator stop <name>` | Stop an emulator |
| `maui android emulator delete <name>` | Delete an emulator |

## Apple commands (macOS only)

The `maui apple` commands manage your Apple development environment, wrapping native tools like `xcrun simctl` and `xcode-select`.

### Set up Apple environment

Verify Xcode, accept the license, and install an iOS runtime:

```dotnetcli
maui apple install --accept-license --runtime 18.5
```

### Xcode management

| Command | Description |
|---------|-------------|
| `maui apple xcode check` | Check Xcode installation and license |
| `maui apple xcode list` | List Xcode installations |
| `maui apple xcode select <path>` | Switch active Xcode |
| `maui apple xcode accept-license` | Accept Xcode license |

### Runtime management

| Command | Description |
|---------|-------------|
| `maui apple runtime check` | Check runtime installation |
| `maui apple runtime list` | List installed runtimes |
| `maui apple runtime list --available` | List downloadable runtimes |
| `maui apple runtime install <version>` | Install an iOS runtime |

### Simulator management

| Command | Description |
|---------|-------------|
| `maui apple simulator list` | List simulators |
| `maui apple simulator create <name> <device> <runtime>` | Create a simulator |
| `maui apple simulator start <name>` | Start a simulator |
| `maui apple simulator stop <name>` | Stop a simulator |
| `maui apple simulator delete <name>` | Delete a simulator |

## Windows commands (Windows only)

| Command | Description |
|---------|-------------|
| `maui windows sdk list` | List Windows SDK installations |
| `maui windows developer-mode status` | Check Developer Mode status |
| `maui windows developer-mode enable` | Guide to enable Developer Mode |

## JSON output

All commands support `--json` for structured output suitable for CI/CD pipelines and AI agent consumption:

```dotnetcli
maui doctor --json
```

```json
{
  "platform": "android",
  "status": "warning",
  "checks": [
    {
      "name": "JDK",
      "status": "ok",
      "version": "21.0.4"
    },
    {
      "name": "Android SDK",
      "status": "warning",
      "message": "SDK licenses not accepted",
      "fix": "maui android sdk accept-licenses"
    }
  ]
}
```

Each error includes a code, category, and fix command, enabling automated remediation.

## See also

- [Installation](installation.md)
- [Troubleshooting known issues](~/troubleshooting.md)
