---
title: "App profiling with maui profile"
description: "Learn how to profile .NET MAUI app startup and custom scenarios using the maui profile command."
ms.date: 05/07/2026
---

# App profiling with maui profile

The `maui profile` command captures performance traces from a running .NET MAUI app using `dotnet-trace`. Two subcommands are available:

- `maui profile startup` — Launch the app suspended, then attach immediately to capture the full startup trace. The trace stops automatically when `MauiProfilingMarker.Complete()` is called in your app code.
- `maui profile manual` — Launch the app without suspending it, then attach `dotnet-trace` on demand at any point during the session.

> [!IMPORTANT]
> `maui profile` supports **Android** and **iOS simulator** only.

## maui profile startup

Launch your app in a suspended state and immediately attach `dotnet-trace` to capture a startup trace.

```bash
maui profile startup [options]
```

### Options

| Option | Description | Default |
|--------|-------------|---------|
| `--project` | Path to `.csproj` or containing directory | Current directory |
| `--framework`, `-f` | Target framework (e.g. `net10.0-android`, `net10.0-ios`) | Inferred from project |
| `--device`, `-d` | Device or simulator identifier | Only available device |
| `--output`, `-o` | Output trace file path | `(project)_(timestamp).nettrace` |
| `--format` | Output format: `nettrace`, `speedscope`, or `mibc` | `nettrace` |
| `--configuration`, `-c` | Build configuration | `Release` |
| `--platform` | Target platform (inferred from `--framework` if omitted) | Auto |
| `--duration` | Max trace duration in `hh:mm:ss`; omit to stop on marker | None |
| `--trace-profile` | dotnet-trace built-in profile name | None |
| `--no-build` | Skip build, use existing binaries | `false` |
| `--diagnostic-port` | TCP port for diagnostic connection | `9000` |
| `--stopping-event-provider-name` | EventSource name used to stop the trace | None |
| `--stopping-event-event-name` | Event name used to stop the trace | None |

### Add the profiling helper package

To stop the trace automatically at a specific point in startup, add the `Microsoft.Maui.ProfilingHelper` NuGet package to your app project:

```xml
<PackageReference Include="Microsoft.Maui.ProfilingHelper" Version="*" />
```

Then call `MauiProfilingMarker.Complete()` in your app code at the point where you want profiling to stop (for example, after the first frame renders):

```csharp
using Microsoft.Maui.ProfilingHelper;

// Call this after the point you want to stop measuring
MauiProfilingMarker.Complete();
```

### Stop the trace on the marker event

Pass the `--stopping-event-provider-name` and `--stopping-event-event-name` options to stop the trace automatically when `MauiProfilingMarker.Complete()` fires:

```bash
maui profile startup \
  --framework net10.0-android \
  --stopping-event-provider-name Microsoft.Maui.ProfilingHelper \
  --stopping-event-event-name StartupComplete
```

### Environment variables

The following environment variables are set by the profiling helper and can be used in advanced scenarios:

| Variable | Values | Effect |
|----------|--------|--------|
| `MAUI_PROFILING_HELPER` | `1` / `true` | Indicates that the app is running in a profiling session. |
| `MAUI_PROFILING_HELPER_EXIT_HOST` | host name / IP | Optional explicit host for the CLI exit-control channel. |
| `MAUI_PROFILING_HELPER_EXIT_PORT` | TCP port | Optional explicit port for the CLI exit-control channel. |

### MSBuild properties

| Property | Description |
|----------|-------------|
| `MauiProfilingHelperInject` | Enable automatic injection of the profiling helper |
| `MauiProfilingHelperEnableRuntimePgo` | Enable runtime PGO during profiling |
| `MauiProfilingHelperExitHost` | Override the CLI exit-control host |
| `MauiProfilingHelperExitPort` | Override the CLI exit-control port |
| `MauiProfilingHelperEventPipeOutputPath` | Override the EventPipe output path |

## maui profile manual

Launch a .NET MAUI app without suspending it, then attach `dotnet-trace` on demand.

```bash
maui profile manual [options]
```

### Options

| Option | Description | Default |
|--------|-------------|---------| 
| `--project` | Path to `.csproj` or containing directory | Current directory |
| `--framework`, `-f` | Target framework (e.g. `net10.0-android`, `net10.0-ios`) | Inferred from project |
| `--device`, `-d` | Device or simulator identifier | Only available device |
| `--output`, `-o` | Output trace file path | `(project)_(timestamp).nettrace` |
| `--format` | Output format: `nettrace`, `speedscope`, or `mibc` | `nettrace` |
| `--configuration`, `-c` | Build configuration | `Release` |
| `--platform` | Target platform (inferred from `--framework` if omitted) | Auto |
| `--duration` | Max trace duration in `hh:mm:ss`; omit to stop manually | None |
| `--trace-profile` | dotnet-trace built-in profile name | None |
| `--no-build` | Skip build, use existing binaries | `false` |
| `--diagnostic-port` | TCP port for diagnostic connection | `9000` |

> [!NOTE]
> `maui profile manual` supports **Android** and **iOS simulator** only.

### Workflow

1. Run `maui profile manual --framework net10.0-android`
2. The app launches without suspending.
3. Navigate to the screen or scenario you want to profile.
4. Press **Enter** to attach `dotnet-trace` and start collection.
5. Reproduce the scenario.
6. Press **Enter** again to stop the trace and finalize the output file.

### Example: Profile a specific screen on Android

```bash
maui profile manual \
  --framework net10.0-android \
  --output my-profile.nettrace
```

### Example: Collect a speedscope trace with a fixed 30-second window

```bash
maui profile manual \
  --framework net10.0-ios \
  --format speedscope \
  --duration 00:00:30
```

## See also

- [.NET MAUI CLI overview](index.md)
- [Network monitoring and profiling](../devflow/network-profiling.md)
