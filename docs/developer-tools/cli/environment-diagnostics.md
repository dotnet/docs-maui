---
title: "Environment diagnostics with maui doctor"
description: "Learn how to use the maui doctor command to diagnose, troubleshoot, and auto-fix .NET MAUI development environment issues."
ms.date: 04/03/2026
---

# Environment diagnostics with `maui doctor`

The `maui doctor` command runs a comprehensive set of checks on your .NET MAUI development environment, identifies issues, and can automatically fix common problems. Use it to verify that your machine is correctly set up for .NET MAUI development.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Run diagnostics

Run the following command to perform a full environment check:

```dotnetcli
maui doctor
```

The command checks for the following components and their configurations:

- .NET SDK version and installed workloads
- Android SDK packages and platform tools
- Java Development Kit (JDK) version and configuration
- Xcode version and command-line tools (macOS)
- Apple simulator runtimes and availability (macOS)
- Visual Studio installation and required components
- Environment variables and path configuration

Each check reports a pass, warning, or failure status so you can quickly identify what needs attention.

## Auto-fix issues

When `maui doctor` detects common problems, it can automatically resolve them. Typical auto-fix scenarios include:

- Installing missing Android SDK packages
- Correcting JDK configuration and environment variables
- Accepting Android SDK licenses
- Installing missing .NET workloads

Follow the interactive prompts to approve or skip individual fixes.

## Use in CI pipelines

For continuous integration environments, use the `--json` and `--ci` flags together:

```dotnetcli
maui doctor --json --ci
```

The `--ci` flag makes the command non-interactive and causes it to fail fast when a critical issue is detected, which is useful for gating CI pipelines. Combine with `--json` to produce machine-readable output that you can pipe to other tools:

```dotnetcli
maui doctor --json --ci | jq '.checks[] | select(.status == "error")'
```

## Verbose output

For detailed diagnostic information, use the `-v` flag:

```dotnetcli
maui doctor -v
```

Verbose output includes additional details about each check, such as exact file paths, version strings, and environment variable values. This level of detail is helpful when reporting issues or debugging edge cases in your environment.

## See also

- [.NET MAUI CLI overview](index.md)
