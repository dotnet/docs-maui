---
title: "Android SDK and emulator management (experimental)"
description: "Learn how to use the .NET MAUI CLI to install and manage the Android SDK, JDK, and emulators for .NET MAUI development."
ms.date: 04/03/2026
---

# Android SDK and emulator management (experimental)

The .NET MAUI CLI provides commands for managing the Android development environment, including SDK packages, JDK installations, and emulators. These commands simplify setup and ongoing maintenance of the Android toolchain.

> [!IMPORTANT]
> The .NET MAUI CLI is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. The package is provided as-is with no guarantees of support, servicing, or updates.

## Interactive Android setup

The `maui android install` command runs a full interactive setup of the Android development environment:

```dotnetcli
maui android install
```

This command walks you through:

1. Installing and configuring the JDK.
1. Installing the Android SDK and required platform packages.
1. Accepting Android SDK licenses.
1. Optionally creating an Android emulator.

This workflow is ideal for new developers setting up their environment for the first time.

## Android SDK management

Use the `maui android sdk` commands to list available packages and install specific SDK components.

### List available and installed packages

```dotnetcli
maui android sdk list
```

The output shows available and installed packages, similar to:

```
Package                          | Installed | Available
---------------------------------+-----------+----------
platforms;android-35             | 35.0.1    | 35.0.1
platforms;android-34             |           | 34.0.4
build-tools;35.0.0               | 35.0.0    | 35.0.0
platform-tools                   | 35.0.2    | 35.0.2
emulator                         | 35.3.10   | 35.3.10
```

### Install a specific package

```dotnetcli
maui android sdk install "platforms;android-35"
```

You can install multiple packages by running the command for each one, or by specifying multiple package names if supported by your CLI version.

## JDK management

Install and configure the JDK with a single command:

```dotnetcli
maui android jdk install
```

This command handles the full JDK setup process:

- Downloads a compatible JDK version.
- Installs the JDK to the appropriate location.
- Configures the `JAVA_HOME` environment variable and updates the system path.

## Emulator management

The .NET MAUI CLI provides commands to create, start, stop, and delete Android emulators.

### Create an emulator

```dotnetcli
maui android emulator create --name MyEmulator
```

This creates a new Android emulator with default hardware settings. The command downloads the required system image if it isn't already installed.

### Start an emulator

```dotnetcli
maui android emulator start --name MyEmulator
```

Launches the specified emulator. The emulator runs in the background so you can continue using the terminal.

### Stop an emulator

```dotnetcli
maui android emulator stop --name MyEmulator
```

Gracefully shuts down the running emulator.

### Delete an emulator

```dotnetcli
maui android emulator delete --name MyEmulator
```

Permanently removes the emulator and its associated data.

## Use in CI pipelines

In automated environments, combine Android commands with the `--json` and `--ci` flags for non-interactive, machine-readable output:

```dotnetcli
maui android sdk install "platforms;android-35" --ci
maui android emulator create --name CIEmulator --ci
maui android emulator start --name CIEmulator --ci
```

Use `--json` when you need to parse command output programmatically:

```dotnetcli
maui android sdk list --json --ci
```

## See also

- [.NET MAUI CLI overview](index.md)
- [Device management](device-management.md)
