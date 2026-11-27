---
title: "Check TCP port usage with maui port check"
description: "Learn how to use the maui port check command to find which process is listening on a TCP port."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Check TCP port usage with `maui port check`

The `maui port check` command reports whether a process is listening on a TCP port and, if so, which process. Use it to diagnose port conflicts before you start a development server or a DevFlow agent.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Check a port

Pass the TCP port number to check, from 1 through 65535:

```bash
maui port check 8080
```

When no process is listening on the port, the command reports that the port is free:

```console
✓ Port 8080 is free.
```

When the port is in use, the command lists each listener with its address and address family, plus its process ID and process name when they're available:

```console
ℹ Port 8080 is in use:
ℹ   PID 12345 (dotnet) :: [ipv6]
```

To free the port, stop the listed process, or configure your tool to use a different port. For example, DevFlow agents use port 9223 by default, and the DevFlow broker uses port 19223. For more information, see [DevFlow broker](../devflow/broker.md).

The command works on Windows, macOS, and Linux.

## JSON output

Use the `--json` option to produce machine-readable output:

```bash
maui port check 8080 --json
```

The following example shows the output when the port is in use:

```json
{
  "port": 8080,
  "in_use": true,
  "listeners": [
    {
      "pid": 12345,
      "process_name": "dotnet",
      "address": "::",
      "family": "ipv6",
      "state": "listen"
    }
  ]
}
```

When the port is free, `in_use` is `false` and `listeners` is an empty array.

On some macOS and Linux systems, the CLI can't identify the process that owns a listener. In that case, the terminal output shows `unknown process`, and the JSON output reports a `pid` of `0` and an empty `process_name`.

## Exit codes

| Exit code | Meaning |
|---|---|
| `0` | The port is free. |
| `1` | The port is in use. |
| `2` | The port number is out of range, or the CLI couldn't enumerate TCP listeners. |

An out-of-range port number reports error `E1004`. A failure to enumerate TCP listeners reports error `E1008`.

You can use the exit code in scripts to detect a port conflict:

```bash
maui port check 8080
case $? in
  1) echo "Port 8080 is already in use." ;;
  2) echo "The port check couldn't run." ;;
esac
```

## See also

- [.NET MAUI CLI overview](index.md)
- [DevFlow broker](../devflow/broker.md)
