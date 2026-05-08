---
title: "Apple platform management"
description: "Learn how to use the .NET MAUI CLI to manage Xcode installations, simulator runtimes, and iOS simulators for .NET MAUI development."
ms.date: 05/08/2026
---

# Apple platform management

The .NET MAUI CLI provides commands for managing Apple development tools, including Xcode installations, simulator runtimes, and iOS simulators. These commands are available on macOS only.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Xcode management

### List installed Xcode versions

Use the `maui apple xcode list` command to discover all Xcode installations on your machine:

```dotnetcli
maui apple xcode list
```

The output includes the version, build number, path, and whether each installation is the currently selected Xcode:

```
Version | Build    | Path                          | Selected
--------+----------+-------------------------------+---------
26.4    | 17E192   | /Applications/Xcode.app       | ✓
26.3    | 17D61a   | /Applications/Xcode-26.3.app  |
```

## Runtime management

### List simulator runtimes

Use the `maui apple runtime list` command to see the installed simulator runtimes:

```dotnetcli
maui apple runtime list
```

Filter by platform with the `--platform` option:

```dotnetcli
maui apple runtime list --platform iOS
```

The `--platform` option accepts `iOS`, `tvOS`, `watchOS`, and `visionOS`.

The output shows each runtime's name, platform, version, availability, and whether it's bundled with Xcode:

```
Name                | Platform | Version | Available | Bundled
--------------------+----------+---------+-----------+--------
iOS 26.4            | iOS      | 26.4    | ✓         | Yes
iOS 26.0            | iOS      | 26.0    | ✓         | No
watchOS 26.4        | watchOS  | 26.4    | ✓         | Yes
```

## Simulator management

The .NET MAUI CLI provides commands to list, start, stop, and delete iOS simulators.

### List simulators

```dotnetcli
maui apple simulator list
```

The output shows each simulator's name, UDID, OS version, state, and availability:

```
Name                  | UDID                                 | OS        | State    | Available
----------------------+--------------------------------------+-----------+----------+----------
iPhone 17 Pro         | A1B2C3D4-E5F6-7890-ABCD-EF1234567890 | iOS 26.4  | Shutdown | ✓
iPad Air (M4)         | B2C3D4E5-F6A7-8901-BCDE-F12345678901 | iOS 26.4  | Booted   | ✓
```

### Start a simulator

Boot a simulator by name or UDID:

```dotnetcli
maui apple simulator start "iPhone 17 Pro"
```

```dotnetcli
maui apple simulator start A1B2C3D4-E5F6-7890-ABCD-EF1234567890
```

The simulator launches in the background so you can continue using the terminal.

### Stop a simulator

Shut down a running simulator by name or UDID:

```dotnetcli
maui apple simulator stop "iPhone 17 Pro"
```

To stop all running simulators:

```dotnetcli
maui apple simulator stop all
```

### Delete a simulator

Permanently remove a simulator and its associated data:

```dotnetcli
maui apple simulator delete "iPhone 17 Pro"
```

### Create a simulator

Creates a new simulator device from a device-type identifier.

**macOS only.**

```dotnetcli
maui apple simulator create (device-type) [--name (name)] [--runtime (runtime)] [--if-not-exists] [--json]
```

| Argument / Option | Required | Description |
|---|---|---|
| `(device-type)` | ✅ | Device type identifier, e.g. `com.apple.CoreSimulator.SimDeviceType.iPhone-15` |
| `--name` | ❌ | Custom display name for the simulator. Defaults to a name derived from `(device-type)`. |
| `--runtime` | ❌ | Runtime identifier, e.g. `com.apple.CoreSimulator.SimRuntime.iOS-17-2`. When specified, the derived default name includes the runtime version. |
| `--if-not-exists` | ❌ | If a simulator with the same name already exists, return its UDID instead of failing. Useful for idempotent scripting. |
| `--json` | ❌ | Emit machine-readable JSON output. |

**Examples:**

```bash
# Create an iPhone 15 simulator with a generated name
maui apple simulator create com.apple.CoreSimulator.SimDeviceType.iPhone-15

# Create with a custom name and a specific runtime
maui apple simulator create com.apple.CoreSimulator.SimDeviceType.iPhone-15 \
  --name "My Test iPhone" \
  --runtime com.apple.CoreSimulator.SimRuntime.iOS-17-2

# Idempotent create (returns existing UDID if name already exists)
maui apple simulator create com.apple.CoreSimulator.SimDeviceType.iPhone-15 \
  --name "CI iPhone" --if-not-exists --json
```

**JSON output (`--json`):**

```json
{
  "udid": "AABBCCDD-1234-5678-ABCD-000000000001",
  "name": "My Test iPhone",
  "device_type": "com.apple.CoreSimulator.SimDeviceType.iPhone-15",
  "runtime": "com.apple.CoreSimulator.SimRuntime.iOS-17-2"
}
```

`runtime` is omitted from the JSON when not specified.

**Error codes:**

| Code | Meaning |
|---|---|
| `E2207` | Simulator creation failed |
| `E2204` | A named simulator already exists and `--if-not-exists` was not specified |

### Erase a simulator

Erases (resets) a simulator device to factory state. The simulator must be shut down before erasing.

**macOS only.**

```dotnetcli
maui apple simulator erase (name-or-udid) [--json]
```

| Argument / Option | Required | Description |
|---|---|---|
| `(name-or-udid)` | ✅ | Simulator name or UDID |
| `--json` | ❌ | Emit machine-readable JSON output |

**Examples:**

```bash
# Erase by name
maui apple simulator erase "My Test iPhone"

# Erase by UDID
maui apple simulator erase AABBCCDD-1234-5678-ABCD-000000000001

# Erase and confirm in JSON
maui apple simulator erase "My Test iPhone" --json
```

**JSON output (`--json`):**

```json
{
  "target": "My Test iPhone",
  "erased": true
}
```

> [!NOTE]
> The simulator must be in `Shutdown` state before erasing. If it is booted, stop it first:
>
> ```bash
> maui apple simulator stop (name-or-udid)
> ```

**Error codes:**

| Code | Meaning |
|---|---|
| `E2208` | Erase failed (check simulator state) |
| `E2204` | Simulator not found |

## Use in CI pipelines

In automated environments, combine Apple commands with the `--json` and `--ci` flags for non-interactive, machine-readable output:

```dotnetcli
maui apple simulator list --json --ci
maui apple xcode list --json --ci
```

Use `--json` when you need to parse command output programmatically:

```dotnetcli
maui apple simulator list --json | jq '.[] | select(.isBooted == true)'
```

## See also

- [.NET MAUI CLI overview](index.md)
- [Environment diagnostics](environment-diagnostics.md)
- [Device management](device-management.md)
