---
title: "Device management (experimental)"
description: "Learn how to use the .NET MAUI CLI to list connected devices and emulators across all platforms."
ms.date: 04/03/2026
---

# Device management (experimental)

The `maui device list` command discovers and lists connected devices and emulators across all platforms. It provides a unified view of available deployment targets for your .NET MAUI apps.

> [!IMPORTANT]
> The .NET MAUI CLI is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. The package is provided as-is with no guarantees of support, servicing, or updates.

## List connected devices

Run the following command to list all connected devices and running emulators:

```dotnetcli
maui device list
```

The output includes:

- Physical Android devices connected via USB or Wi-Fi.
- Running Android emulators.
- iOS simulators (macOS).
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
