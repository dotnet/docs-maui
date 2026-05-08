---
title: "Device management"
description: "Learn how to use the .NET MAUI CLI to list connected devices and emulators across all platforms."
ms.date: 04/03/2026
---

# Device management

The `maui device list` command discovers and lists connected devices and emulators across all platforms. It provides a unified view of available deployment targets for your .NET MAUI apps.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## List connected devices

Run the following command to list all connected devices and running emulators:

```dotnetcli
maui device list
```

The output includes:

- Physical Android devices connected via USB or Wi-Fi.
- Running Android emulators.
- Apple simulators, including booted and available devices (macOS).
- Connected iOS devices (macOS).

Each entry shows the device name, platform, identifier, and connection status.

## JSON output

For scripting and CI scenarios, use the `--json` flag to get machine-readable output:

```dotnetcli
maui device list --json
```

The JSON output can be piped to other tools for automated device selection or reporting:

```dotnetcli
maui device list --json | jq '.devices[] | select(.platform == "Android")'
```

## See also

- [.NET MAUI CLI overview](index.md)
