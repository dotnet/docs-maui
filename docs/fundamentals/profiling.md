---
title: "Performance Profiling"
description: "Learn how to profile the performance of your .NET MAUI app."
ms.date: 09/11/2026
---

# Performance Profiling

Performance profiling is the process of measuring the performance of
an application to identify areas for improvement. .NET MAUI and client
applications, in general, are interested in:

- **Startup time**: The time it takes for the application to start and
  display the first screen.
- **CPU usage**: If specific methods are consuming too much CPU time:
  either through many calls or long-running operations.
- **Memory usage**: If many allocations are made beyond reason or if
  there are memory leaks.

The techniques and tools for improving these metrics are different,
which we plan to demystify in this guide. The tools used to profile
.NET MAUI applications can also vary depending on the platform. This
guide covers Android, iOS, Mac Catalyst, and Windows profiling
approaches.

> [!IMPORTANT]
> Always profile `Release` builds for accurate performance
> measurements.

::: moniker range="<=net-maui-10.0"
> `Debug` builds use the interpreter (`UseInterpreter=true`) for C#
> hot reload support, which significantly impacts performance and
> produces unrealistic results.
::: moniker-end

::: moniker range=">=net-maui-11.0"
> .NET MAUI applications normally use CoreCLR on Android, iOS, Mac
> Catalyst, and Windows in .NET 11 and later. NativeAOT has different
> diagnostics limitations; see [NativeAOT deployment](~/deployment/nativeaot.md).
> For runtime selection, see [Runtime and compilation](~/deployment/runtimes-compilation.md).
::: moniker-end

## Prerequisites

### Installing Diagnostic Tools

To profile .NET MAUI applications with the .NET diagnostic tools, you
need to install the following .NET global tools. The router is used
for the Android and iOS TCP workflows; Mac Catalyst and Windows use
their local diagnostic endpoints.

- [`dotnet-trace`][dotnet-trace] - Collects CPU traces and performance
  data
- [`dotnet-dsrouter`][dotnet-dsrouter] - Forwards diagnostic
  connections from remote devices to your local machine
- [`dotnet-gcdump`][dotnet-gcdump] - Collects memory dumps for
  analyzing managed memory usage

You can install these tools using the following commands:

```sh
$ dotnet tool install -g dotnet-trace
You can invoke the tool using the following command: dotnet-trace
Tool 'dotnet-trace' was successfully installed.
$ dotnet tool install -g dotnet-dsrouter
You can invoke the tool using the following command: dotnet-dsrouter
Tool 'dotnet-dsrouter' was successfully installed.
$ dotnet tool install -g dotnet-gcdump
You can invoke the tool using the following command: dotnet-gcdump
Tool 'dotnet-gcdump' was successfully installed.
```

> [!NOTE]
> ::: moniker range="<=net-maui-10.0"
> You need at least version 9.0.652701 of all the diagnostic tools to
> use the features described in this guide. Check
> [dotnet-trace](https://www.nuget.org/packages/dotnet-trace/),
> [dotnet-dsrouter](https://www.nuget.org/packages/dotnet-dsrouter/),
> and [dotnet-gcdump](https://www.nuget.org/packages/dotnet-gcdump/)
> on NuGet for the latest versions.
> ::: moniker-end
>
> ::: moniker range=">=net-maui-11.0"
> You need at least version [10.0.731102](https://www.nuget.org/packages/dotnet-gcdump/10.0.731102 "10.0.731102")
> of all the diagnostic tools to use the features described in this
> guide. Check
> [dotnet-trace](https://www.nuget.org/packages/dotnet-trace/),
> [dotnet-dsrouter](https://www.nuget.org/packages/dotnet-dsrouter/),
> and [dotnet-gcdump](https://www.nuget.org/packages/dotnet-gcdump/)
> on NuGet for the latest versions.
> ::: moniker-end

The `--dsrouter` option in `dotnet-trace` and `dotnet-gcdump`
automatically launches and manages `dotnet-dsrouter` as a subprocess.
If the integrated option cannot find the router, install
`dotnet-dsrouter` globally or place it alongside the diagnostic tool.

> [!WARNING]
> Diagnostic TCP endpoints are development and test interfaces. Keep
> them on loopback or use device forwarding, and never expose them to
> an untrusted network. The endpoints are unauthenticated and
> unencrypted.

See the .NET Conf session, [.NET Diagnostic Tooling with
AI][dotnetconf], for a live demo of using these tools.

[dotnet-trace]: /dotnet/core/diagnostics/dotnet-trace
[dotnet-dsrouter]: /dotnet/core/diagnostics/dotnet-dsrouter
[dotnet-gcdump]: /dotnet/core/diagnostics/dotnet-gcdump
[dotnetconf]: https://youtu.be/HLNYCwgk5fU

### How the Tools Work Together

::: moniker range="<=net-maui-10.0"
To use these diagnostic tools on iOS and Android, several components
work together:

- The .NET global tools (`dotnet-trace`, `dotnet-gcdump`,
  `dotnet-dsrouter`) run on your development machine.
- The Mono diagnostic component
  (`libmono-component-diagnostics_tracing.so`) is included in your
  application package.
- `dotnet-dsrouter` forwards the diagnostic connection from the remote
  device or emulator to a local port on your machine.
- The diagnostic tools connect to this local port to collect profiling
  data.
::: moniker-end

::: moniker range=">=net-maui-11.0"
The .NET global tools run on your development machine. EventPipe and
the diagnostic server are CoreCLR runtime components.

Android and iOS applications use the configured TCP diagnostic port.
`dotnet-dsrouter` bridges that port to the local diagnostic endpoint
used by `dotnet-trace` or `dotnet-gcdump`. Use the `android-emu` or
`android` router mode for Android, and the `ios-sim` or `ios` router
mode for iOS. Mac Catalyst uses a direct local CoreCLR Unix domain
socket endpoint and does not use the iOS TCP or `--dsrouter` workflow.
Windows uses the normal local CoreCLR diagnostic endpoint and does not
require `dotnet-dsrouter`.
::: moniker-end

The `--dsrouter` option in `dotnet-trace` and `dotnet-gcdump`
automatically handles the complexity of starting `dotnet-dsrouter` and
coordinating the connection where a router is required.

## Building Your Application for Profiling

To enable profiling, your application must be built with the
diagnostic-port settings required by the target platform.

### Understanding Diagnostic Properties

::: moniker range="<=net-maui-10.0"
The following MSBuild properties control how your application
communicates with the diagnostic tools:

- **`DiagnosticAddress`**: The IP address where `dotnet-dsrouter` is
  listening. Use `10.0.2.2` for Android emulators (this is the host
  machine's loopback address from the emulator's perspective), and
  `127.0.0.1` for physical devices and iOS.

- **`DiagnosticPort`**: The port number for the diagnostic connection
  (default is `9000`).

- **`DiagnosticSuspend`**: When `true`, the application waits for the
  profiler to connect before starting. When `false`, the application
  starts immediately and the profiler can connect later. Use `true`
  for startup profiling, `false` for runtime profiling and memory
  dumps.

- **`DiagnosticListenMode`**: Set to `connect` for Android (the app
  connects to `dotnet-dsrouter`), or `listen` for iOS (the app listens
  for `dotnet-dsrouter` to connect to it).

- **`EnableDiagnostics`**: When `true`, includes the Mono diagnostic
  component in the application package. This is implicitly set when
  setting any of the `Diagnostic*` MSBuild properties. This property
  works on Android, iOS, and Mac Catalyst.

> [!NOTE]
> The diagnostic component is intended for development and testing
> builds only.
::: moniker-end

::: moniker range=">=net-maui-11.0"
CoreCLR includes EventPipe and the diagnostic server. The MSBuild
`EnableDiagnostics` property controls SDK diagnostics configuration.
The Android and iOS SDKs use `EnableDiagnostics` and the
`Diagnostic*` properties to preserve diagnostic providers in optimized
builds and package their configured diagnostic ports. Setting a
`Diagnostic*` property enables the SDK diagnostics configuration. Mac
Catalyst uses the local CoreCLR diagnostic endpoint, not the mobile
`Diagnostic*` port configuration. Where applicable,
`EnableDiagnostics` still preserves diagnostic providers in optimized
builds.

`DOTNET_EnableDiagnostics` is a different setting: it is a runtime
environment variable. Setting `DOTNET_EnableDiagnostics=0` disables
the runtime diagnostic server and related diagnostics. Do not confuse
this runtime variable with the `EnableDiagnostics` MSBuild property.

The following properties configure `DOTNET_DiagnosticPorts`, the
runtime diagnostic-port environment variable:

- **`DiagnosticConfiguration`**: Supplies the complete
  `DOTNET_DiagnosticPorts` value when advanced configuration is
  required.
- **`DiagnosticAddress`**: Supplies the address used by the
  diagnostic-port configuration. Use `10.0.2.2` for an Android
  emulator and `127.0.0.1` for Android device forwarding or iOS.
- **`DiagnosticPort`**: Supplies the TCP port number, such as `9000`.
  The app and router must use the same free port.
- **`DiagnosticSuspend`**: When `true`, the runtime waits for the
  diagnostic connection before startup continues. Use it for startup
  tracing. When `false`, the application starts immediately and a
  tool can attach later.
- **`DiagnosticListenMode`**: Uses `connect` for Android, where the
  app connects to the router. The .NET 11 iOS SDK uses `listen`,
  where the app listens for the router. Keep the iOS simulator and
  physical iOS device workflows separate.

`DiagnosticConfiguration` can supply the complete runtime
configuration directly:

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticConfiguration=10.0.2.2:9000,connect,suspend
```

Use the individual properties in the examples below when they make
the platform topology easier to read.
::: moniker-end

### Build Command Examples

When you run `dotnet-trace` or `dotnet-gcdump` with the `--dsrouter`
option, the tool displays instructions for building your application.
For example:

::: moniker range="<=net-maui-10.0"
**For Android emulators:**

```sh
dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

**For Android devices:**

```sh
dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

**For iOS devices and simulators:**

```sh
dotnet build -t:Run -c Release -f net10.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
```

> [!NOTE]
> Use `-f net10.0-android` or `-f net10.0-ios` for projects with
> multiple target frameworks in `$(TargetFrameworks)`.

> [!IMPORTANT]
> Applications built with these diagnostic properties should only be
> used for development and testing. Never release builds with
> diagnostic components enabled to production, as they can expose
> endpoints with deeper insights into your application's code.
::: moniker-end

::: moniker range=">=net-maui-11.0"
**For an Android emulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

**For a physical Android device:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

**For an iOS simulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
```

**For a physical iOS device:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
dotnet-trace collect --dsrouter ios
```

Run these commands from a macOS development host. The `ios` router
mode uses the USB-connected physical iOS device.

**For Mac Catalyst:**

```sh
dotnet build -t:Run -c Release -f net11.0-maccatalyst
dotnet-trace ps
dotnet-trace collect -p <pid>
```

Mac Catalyst uses the direct local CoreCLR EventPipe diagnostic
endpoint. Do not use the iOS TCP settings or `dotnet-dsrouter` for Mac
Catalyst. Instruments remains an alternative for native and system
profiling.

**For Windows:**

Use the normal local CoreCLR diagnostic endpoint. Windows does not
require `DiagnosticConfiguration`, the mobile `Diagnostic*` properties,
or `dotnet-dsrouter`.

> [!IMPORTANT]
> Applications built with diagnostic-port settings should only be used
> for development and testing. Keep diagnostic endpoints on loopback or
> behind device forwarding. They are unauthenticated and unencrypted.
::: moniker-end

## Profiling CPU Usage

The `dotnet-trace` tool collects CPU sampling information in formats
like `.nettrace` and `.speedscope.json`. These traces show you the
time spent in each method, helping you identify performance
bottlenecks in your application.

The workflow for CPU profiling depends on whether you're measuring
startup time or profiling runtime operations. The key difference is
the `-p:DiagnosticSuspend` MSBuild property.

### Profiling Startup Time

To capture accurate startup time measurements, suspend application
startup until the profiler is ready. This ensures you capture the
entire startup sequence from the very beginning.

::: moniker range="<=net-maui-10.0"

1. In one terminal, start `dotnet-trace` with the `--dsrouter` option:

   ```sh
   dotnet-trace collect --dsrouter android-emu --format speedscope
   ```

   Or for a physical Android device:

   ```sh
   dotnet-trace collect --dsrouter android --format speedscope
   ```

   For iOS devices and simulators, use `--dsrouter ios` or `--dsrouter ios-sim` respectively.

2. In another terminal, build and deploy your application with
   `-p:DiagnosticSuspend=true` to pause at startup:

   **For Android emulators:**

   ```sh
   dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
   ```

   **For Android devices:**

   ```sh
   dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
   ```

   **For iOS (devices and simulators):**

   ```sh
   dotnet build -t:Run -c Release -f net10.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=listen
   ```

3. Your application will pause at the splash screen, waiting for
   `dotnet-trace` to connect. Once connected, the application will
   start and `dotnet-trace` will begin recording.

4. Allow your application to fully start and reach the initial screen.

5. Press `<Enter>` in the `dotnet-trace` terminal to stop recording.

The trace file will be saved to the current directory. Use the `-o`
option to specify a different output directory.

::: moniker-end

::: moniker range=">=net-maui-11.0"
Start the collector before starting the application. For each target,
run the collector command in one terminal and the build command in
another terminal.

**Android emulator:**

```sh
dotnet-trace collect --dsrouter android-emu --format speedscope
```

In another terminal, run:

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
```

**Physical Android device:**

```sh
dotnet-trace collect --dsrouter android --format speedscope
```

In another terminal, run:

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
```

**iOS simulator:**

```sh
dotnet-trace collect --dsrouter ios-sim --format speedscope
```

In another terminal, run:

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=listen
```

**Physical iOS device:**

```sh
dotnet-trace collect --dsrouter ios --format speedscope
```

In another terminal, run:

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=listen
```

Run the physical iOS workflow from a macOS development host.

**Mac Catalyst:**

Mac Catalyst uses the direct local CoreCLR diagnostic endpoint rather
than the mobile TCP and `Diagnostic*` configuration. Start the
application, then find its process and attach from another terminal:

```sh
dotnet build -t:Run -c Release -f net11.0-maccatalyst
```

In another terminal, run:

```sh
dotnet-trace ps
dotnet-trace collect -p <pid> --format speedscope
```

This direct-attach workflow begins after the application launches; do
not use `dotnet-dsrouter` for Mac Catalyst.

For Android and iOS, the application pauses at the splash screen until
the diagnostic tool connects. After the connection is established, the
application starts and the trace is recorded. Allow the application to
reach its initial screen, then press `<Enter>` in the `dotnet-trace`
terminal to stop recording.

::: moniker-end

### Profiling Runtime Operations

To profile specific operations during runtime (such as button taps,
navigation, or scrolling), use `-p:DiagnosticSuspend=false` and
connect the profiler after the application has launched.

::: moniker range="<=net-maui-10.0"

1. Build and deploy your application with `-p:DiagnosticSuspend=false`:

   ```sh
   dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
   ```

2. Navigate to the area of your application you want to profile.

3. Start `dotnet-trace` with the `--dsrouter` option:

   ```sh
   dotnet-trace collect --dsrouter android --format speedscope
   ```

4. Perform the operation you want to profile.

5. Press `<Enter>` to stop the trace.

::: moniker-end

::: moniker range=">=net-maui-11.0"
Build and deploy the application with `DiagnosticSuspend=false`, then
start the appropriate collector after the application has launched.
The app and router must use the same address, port, and listen mode.

**Android emulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
dotnet-trace collect --dsrouter android-emu --format speedscope
```

**Physical Android device:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
dotnet-trace collect --dsrouter android --format speedscope
```

**iOS simulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
dotnet-trace collect --dsrouter ios-sim --format speedscope
```

**Physical iOS device:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
dotnet-trace collect --dsrouter ios --format speedscope
```

Run this command from a macOS development host.

**Mac Catalyst:**

Use the direct local CoreCLR EventPipe diagnostic endpoint:

```sh
dotnet-trace ps
dotnet-trace collect -p <pid> --format speedscope
```

Do not use the iOS TCP or `--dsrouter` commands.

**Windows:**

Use `dotnet-trace` against the normal local CoreCLR diagnostic endpoint.

After the collector connects for any of these platforms, perform the
operation you want to profile, then press `<Enter>` in the
`dotnet-trace` terminal to stop the trace.
::: moniker-end

This approach produces a more focused trace file containing only the
specific operation you're investigating.

### Understanding Trace Output

When `dotnet-trace` is collecting a trace, you'll see output similar
to:

```
Process        : $HOME/.dotnet/tools/dotnet-dsrouter
Output File    : /tmp/hellomaui-app-trace
[00:00:00:35]    Recording trace 1.7997   (MB)
Press <Enter> or <Ctrl+C> to exit...
```

After pressing `<Enter>`, the trace is finalized:

```
Stopping the trace. This may take up to minutes depending on the application being traced.

Trace completed.
Writing:    hellomaui-app-trace.speedscope.json
```

### Viewing Trace Files

The `--format` argument controls the output format:

- **`nettrace`** (default): Can be viewed in PerfView or Visual Studio
  on Windows
- **`speedscope`**: JSON format that can be viewed on any platform at
  [https://speedscope.app/][speedscope]

For cross-platform analysis, use `--format speedscope`:

```sh
dotnet-trace collect --dsrouter android --format speedscope
```

[speedscope]: https://speedscope.app/

## Profiling on Windows

While the cross-platform `dotnet-trace` tool works on Windows, the
platform offers additional native profiling options that may be more
convenient.

### Using Visual Studio Performance Profiler

The Visual Studio Performance Profiler provides integrated profiling
for .NET applications. See the [Visual Studio profiling feature
tour][prof-overview] for comprehensive guidance.

[prof-overview]: /visualstudio/profiling/profiling-feature-tour?pivots=programming-language-dotnet

### Using PerfView

[PerfView][perfview] is a powerful, free performance analysis tool for
Windows that can profile .NET MAUI applications with minimal setup.

To profile with PerfView:

Build your application for `Release` with [ReadyToRun enabled][r2r]:

::: moniker range="<=net-maui-10.0"

```sh
dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true
```

::: moniker-end

::: moniker range=">=net-maui-11.0"

```sh
dotnet publish -f net11.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true
```

::: moniker-end

1. Launch PerfView and select `Collect` > `Collect`.

2. In the **Command** field, filter on your app's executable (for
   example, `hellomaui.exe`).

3. Click **Start Collection**, then manually launch your app.

4. Click **Stop Collection** once your app has completed the operation
   you want to profile.

5. Open **CPU Stacks** to view timing information, or use the **Flame
   Graph** tab for a graphical view.

You can also save the PerfView data in SpeedScope format (`File` >
`Save View As`) to view it at [https://speedscope.app/][speedscope]
for cross-platform analysis.

[r2r]: /dotnet/core/deploying/ready-to-run

### Measuring Windows Startup Time with PerfView

To measure precise startup times on Windows, you can use PerfView to
capture [Event Tracing for Windows (ETW)][etw] events:

1. In PerfView, open `Collect` > `Collect` and expand **Advanced
   Options**.

2. Configure the following:
   - Enable **Kernel Base**
   - Add `Microsoft-Windows-XAML:0x44:Informational` to **Additional
     Providers**

3. Click **Start Collection**, then launch and close your app 3-5
   times.

4. Click **Stop Collection**.

5. Open the **Events** report and calculate startup time by finding:
   - The `Windows Kernel/Process/Start` event for your app (note the
     `Time MSec` value)
   - The first `Microsoft-Windows-XAML/Frame/Stop` event for the same
     process ID
   - Subtract the start time from the stop time to get startup
     duration

Run the app multiple times and average the results for more accurate
measurements.

[etw]: /windows-hardware/drivers/devtest/event-tracing-for-windows--etw-

### Using dotnet-trace on Windows

For unpackaged Windows applications, you can use `dotnet-trace`
directly:

::: moniker range="<=net-maui-10.0"

```sh
dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true -p:WindowsPackageType=None
dotnet-trace collect --format speedscope -- bin\Release\net10.0-windows10.0.19041.0\win10-x64\publish\YourApp.exe
```

::: moniker-end

::: moniker range=">=net-maui-11.0"

```sh
dotnet publish -f net11.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true -p:WindowsPackageType=None
dotnet-trace collect --format speedscope -- bin\Release\net11.0-windows10.0.19041.0\win10-x64\publish\YourApp.exe
```

::: moniker-end

This child-process form is suitable for an unpackaged Windows
application. For a packaged MSIX application, activate the app first
and attach to its process instead of using the child-process command.
Windows does not require `dotnet-dsrouter` for either workflow.

## Profiling on iOS and Mac Catalyst with Instruments

For iOS and Mac Catalyst applications, Apple's Instruments tool
provides native profiling with detailed insights into app launch time
and performance.

### Using Instruments for App Launch Profiling

1. Build your app for `Release` with symbols preserved:

   ::: moniker range="<=net-maui-10.0"

   ```sh
   dotnet build -c Release -f net10.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

   ::: moniker range=">=net-maui-11.0"

   ```sh
   dotnet build -c Release -f net11.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

   The `NoSymbolStrip=true` property keeps native symbols in the
   executable, making stack traces in Instruments much more helpful.

2. Install the app on your device:

   ::: moniker range="<=net-maui-10.0"

   ```sh
   dotnet build -t:Run -c Release -f net10.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

   ::: moniker range=">=net-maui-11.0"

   ```sh
   dotnet build -t:Run -c Release -f net11.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

3. Launch Instruments (from Xcode or by running `open -a Instruments`
   in Terminal).

4. Select your iOS device at the top.

5. Select your app from the list of installed applications.

6. Choose the **App Launch** instrument template.

7. Click **Choose**, then click the **Record** button to start
   profiling.

8. The app will launch automatically. Stop the recording once the app
   has fully started.

9. In the results, select the **App Lifecycle** row to see the
   lifecycle timeline. The last row in the bottom table shows the time
   when the app completed launching (for example, `Currently running
   in the foreground...`).

For more information about using Instruments, see Apple's
documentation on [Reducing Your App's Launch Time][apple-launch].

[apple-launch]: https://developer.apple.com/documentation/xcode/reducing-your-app-s-launch-time

::: moniker range=">=net-maui-11.0"
For Mac Catalyst, Instruments is an alternative for native and system
profiling. Managed EventPipe profiling uses the direct local CoreCLR
diagnostic endpoint for the `net11.0-maccatalyst` target. Do not use
the iOS TCP settings or `dotnet-dsrouter` commands for Mac Catalyst.
::: moniker-end

## Profiling Memory Usage

Memory profiling helps you identify memory leaks and understand memory
allocation patterns in your application. Use `dotnet-gcdump` to create
snapshots of managed memory.

::: moniker range=">=net-maui-11.0"
The CoreCLR EventPipe and diagnostic-port instructions in this section
do not apply to NativeAOT applications. See [NativeAOT
deployment](~/deployment/nativeaot.md) for its diagnostics limitations.
::: moniker-end

### Collecting Memory Dumps

::: moniker range="<=net-maui-10.0"
To collect a memory dump, use the same `--dsrouter` workflow as
`dotnet-trace`:

```sh
dotnet-gcdump collect --dsrouter android
```

Use `--dsrouter android-emu`, `--dsrouter ios`, or `--dsrouter
ios-sim` for other targets.

Unlike CPU tracing, memory dumps do not require suspending application
startup. Build your application with `-p:DiagnosticSuspend=false`:

```sh
dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

::: moniker-end

::: moniker range=">=net-maui-11.0"
For Android and iOS, use the `--dsrouter` workflow. Mac Catalyst uses
the direct local CoreCLR endpoint.

**Android emulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
dotnet-gcdump collect --dsrouter android-emu
```

**Physical Android device:**

```sh
dotnet build -t:Run -c Release -f net11.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
dotnet-gcdump collect --dsrouter android
```

**iOS simulator:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
dotnet-gcdump collect --dsrouter ios-sim
```

**Physical iOS device:**

```sh
dotnet build -t:Run -c Release -f net11.0-ios -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=listen
dotnet-gcdump collect --dsrouter ios
```

Run the physical iOS workflow from a macOS development host.

**Mac Catalyst:**

Use the direct local CoreCLR endpoint instead of `--dsrouter`:

```sh
dotnet-trace ps
dotnet-gcdump collect -p <pid>
```

::: moniker-end

Once `dotnet-gcdump` connects, it creates a `*.gcdump` file in the
current directory. You can open this file in Visual Studio on Windows
or [PerfView][perfview].

[perfview]: https://github.com/microsoft/perfview

### Analyzing Memory Dumps

When you open a `*.gcdump` file in Visual Studio, you can:

- View every managed object in memory
- See the total count and size of each type
- Inspect the reference tree to understand what's keeping objects alive
- Compare multiple snapshots to identify growing allocations

Visual Studio's Memory Usage diagnostic tool (`Debug` > `Windows` >
`Diagnostic Tools`) also allows you to take snapshots while debugging,
though you should disable XAML hot reload for accurate results.

> [!TIP]
> Consider taking memory snapshots of `Release` builds, as code paths
> can be significantly different when XAML compilation, AOT
> compilation, and trimming are enabled.

## Diagnosing Memory Leaks

Memory leaks in .NET MAUI applications manifest as steadily increasing
memory usage, especially during repeated navigation or interactions.
On mobile platforms, this can eventually cause the OS to terminate
your application due to excessive memory consumption.

### Symptoms of Memory Leaks

A typical symptom of a memory leak might be:

1. Navigate from the main page to a details page
2. Navigate back
3. Navigate to the details page again
4. Memory grows consistently with each cycle

### Determining if a Leak Exists

To determine if a page is actually leaking, use finalizers with
logging and forced garbage collection during debugging.

1. Add a finalizer with logging to the page class:

   ```csharp
   ~MyDetailsPage() => System.Diagnostics.Debug.WriteLine("~MyDetailsPage() finalized");
   ```

2. Force garbage collection in strategic places (for debugging only):

   ```csharp
   public MyDetailsPage()
   {
       GC.Collect(); // For debugging purposes only
       GC.WaitForPendingFinalizers();
       InitializeComponent();
   }
   ```

3. Test a `Release` build and watch the console output using [adb
   logcat][adb-logcat] (Android) or device logs (iOS).

If the finalizer runs when navigating away from the page, the page is
being collected correctly. If the finalizer never runs, the page is
leaking--something is holding a reference to it indefinitely.

> [!WARNING]
> Remove `GC.Collect()` calls after debugging. They're only for
> diagnosing issues and should never be in production code.

[adb-logcat]: /xamarin/android/deploy-test/debugging/android-debug-log

### Narrowing Down the Cause

Once you've identified a leak, narrow down the cause:

1. Comment out all XAML content. Does the leak still occur?
2. Comment out all C# code in code-behind. Does the leak still occur?
3. Test on multiple platforms. Does it only happen on one platform?

Generally, an empty `ContentPage` should not leak. By systematically
removing code, you can identify which control or code pattern is
causing the problem.

### Common Leak Patterns

#### C# Events

C# events can create circular references that prevent garbage
collection. Consider a scenario where a child object subscribes to a
parent's event, but the parent also holds a reference to the child.
Both objects can end up living forever.

If the event source outlives the subscriber (like a `Style` in
`Application.Resources`), this can cause entire pages to leak.

**Solution**: Use `WeakEventManager` for events in .NET MAUI controls,
or unsubscribe from events when the object is no longer needed.

#### iOS and Mac Catalyst Circular References

On iOS and Mac Catalyst, circular references between C# objects and
native objects can cause leaks because C# objects that subclass
`NSObject` exist in both the garbage-collected .NET world and the
reference-counted Objective-C world.

Example of a problematic pattern:

```csharp
class MyView : UIView
{
    public MyView()
    {
        var picker = new UIDatePicker();
        AddSubview(picker); // MyView -> UIDatePicker
        picker.ValueChanged += OnValueChanged; // UIDatePicker -> MyView via event handler
    }

    void OnValueChanged(object? sender, EventArgs e) { }
}
```

**Solutions**:

1. Make event handlers `static`:

   ```csharp
   static void OnValueChanged(object? sender, EventArgs e) { }
   ```

2. Use a proxy object that doesn't inherit from `NSObject`:

   ```csharp
   class MyView : UIView
   {
       readonly Proxy _proxy = new();

       public MyView()
       {
           var picker = new UIDatePicker();
           AddSubview(picker);
           picker.ValueChanged += _proxy.OnValueChanged;
       }

       class Proxy
       {
           public void OnValueChanged(object? sender, EventArgs e) { }
       }
   }
   ```

> [!NOTE]
> These circular reference issues are specific to iOS and Mac Catalyst.
> They do not normally occur on Android or Windows.

### Best Practices for Avoiding Leaks

- **Test `Release` builds**: Memory behavior can differ significantly
  from `Debug` builds due to optimizations, trimming, and AOT
  compilation.

- **Use finalizers when investigating**: Add finalizers with logging to
  key objects to quickly identify if they're being collected.

- **Unsubscribe from events**: Always unsubscribe from events when
  objects are disposed or no longer needed.

- **Be cautious with events on long-lived objects**: Avoid having
  long-lived objects (like those in `Application.Resources`) hold
  references to short-lived objects (like pages or views).

- **Profile regularly**: Make memory profiling part of your regular
  testing process, especially after adding new features or making
  significant changes.

For more detailed information about memory leak patterns and
techniques, see the [.NET MAUI Memory Leaks wiki][maui-memory-leaks].

[maui-memory-leaks]: https://github.com/dotnet/maui/wiki/Memory-Leaks

## Alternative Profiling Approaches

### Android ActivityManager Startup Logs

Android automatically logs startup time information through the
ActivityManager. You can view these logs using `adb logcat`:

```sh
adb logcat | grep "ActivityManager"
```

When your app starts, you'll see messages like:

```
ActivityManager: Displayed com.android.myexample/.StartupTiming: +3s534ms
```

This shows the time it took for your activity to be displayed. This is
a quick way to measure startup time without any additional tooling or
code changes.

For more information about Android app launch time and optimization
techniques, see the [Android documentation on app startup
time][android-launch].

[android-launch]: https://developer.android.com/topic/performance/vitals/launch-time

### Logging-Based Startup Measurement

For a lightweight approach to measuring startup time across all
platforms, you can log messages at specific points in your application
and measure the time between them:

1. Add a log message when your main page loads:

   ```csharp
   Loaded += (sender, e) => Dispatcher.Dispatch(() => 
       Console.WriteLine("loaded"));
   ```

2. Use a tool like the [measure-startup][measure-startup] sample to
   launch your app and measure the time until the log message appears.

3. On Android, you can filter `adb logcat` output to watch for
   specific messages:

   ```sh
   adb logcat | grep "loaded"
   ```

This approach works across all platforms and is useful for continuous
integration scenarios or quick checks.

[measure-startup]: https://github.com/jonathanpeppers/measure-startup

## Additional Resources

- [EventPipe][eventpipe] - Runtime tracing infrastructure
- [Diagnostic ports][diagnostic-port] - Runtime endpoint configuration
- [.NET MAUI Profiling Wiki][maui-profiling] - Additional advanced
  scenarios and troubleshooting
- [Android Tracing Guide][android-tracing] - Detailed Android-specific
  profiling instructions
- [iOS/macOS Profiling Wiki][macios-profiling] - Platform-specific
  guidance for iOS, Mac Catalyst, and macOS tooling
- [.NET Diagnostic Tools Documentation][dotnet-diagnostics] - Official
  documentation for `dotnet-trace`, `dotnet-dsrouter`, and
  `dotnet-gcdump`
- [PerfView User's Guide][perfview-guide] - In-depth guide to using
  PerfView for Windows profiling

[maui-profiling]: https://github.com/dotnet/maui/wiki/Profiling-.NET-MAUI-Apps
[android-tracing]: https://github.com/dotnet/android/blob/main/Documentation/guides/tracing.md
[macios-profiling]: https://github.com/dotnet/macios/wiki/Profiling
[eventpipe]: /dotnet/core/diagnostics/eventpipe
[diagnostic-port]: /dotnet/core/diagnostics/diagnostic-port
[dotnet-diagnostics]: /dotnet/core/diagnostics/
[perfview-guide]: https://github.com/microsoft/perfview/blob/main/documentation/Markdown/GettingStarted.md
