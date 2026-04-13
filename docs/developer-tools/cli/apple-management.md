---
title: "Apple platform management"
description: "Learn how to use the .NET MAUI CLI to manage Xcode installations, simulator runtimes, and iOS simulators for .NET MAUI development."
ms.date: 04/13/2026
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
Version | Build   | Path                          | Selected
--------+---------+-------------------------------+---------
16.2    | 16C5032a| /Applications/Xcode.app       | ✓
15.4    | 15F31d  | /Applications/Xcode-15.4.app  |
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
iOS 18.2            | iOS      | 18.2    | ✓         | Yes
iOS 17.5            | iOS      | 17.5    | ✓         | No
watchOS 11.2        | watchOS  | 11.2    | ✓         | Yes
```

## Simulator management

The .NET MAUI CLI provides commands to list, start, stop, and delete iOS simulators.

### List simulators

```dotnetcli
maui apple simulator list
```

The output shows each simulator's name, UDID, OS version, state, and availability:

```
Name                  | UDID                                 | OS       | State    | Available
----------------------+--------------------------------------+----------+----------+----------
iPhone 16 Pro         | A1B2C3D4-E5F6-7890-ABCD-EF1234567890 | iOS 18.2 | Shutdown | ✓
iPad Air (M2)         | B2C3D4E5-F6A7-8901-BCDE-F12345678901 | iOS 18.2 | Booted   | ✓
```

### Start a simulator

Boot a simulator by name or UDID:

```dotnetcli
maui apple simulator start "iPhone 16 Pro"
```

```dotnetcli
maui apple simulator start A1B2C3D4-E5F6-7890-ABCD-EF1234567890
```

The simulator launches in the background so you can continue using the terminal.

### Stop a simulator

Shut down a running simulator by name or UDID:

```dotnetcli
maui apple simulator stop "iPhone 16 Pro"
```

To stop all running simulators:

```dotnetcli
maui apple simulator stop all
```

### Delete a simulator

Permanently remove a simulator and its associated data:

```dotnetcli
maui apple simulator delete "iPhone 16 Pro"
```

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
