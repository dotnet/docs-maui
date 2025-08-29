---
title: "Performance Profiling"
description: "Learn how to profile the performance of your .NET MAUI app."
ms.date: 06/20/2025
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
- **Scrolling responsiveness**: If your application scrolls smoothly
  and responds to user input without lag.

The techniques and tools for improving these metrics are different,
which we plan to demystify in this guide. The tools used to profile
.NET MAUI applications can also vary depending on the platform. This
guide will focus heavily on Android and Apple platforms, while
profiling on Windows is most easily done using the [tools in Visual
Studio][prof-overview].

[prof-overview]: /visualstudio/profiling/profiling-feature-tour?pivots=programming-language-dotnet

## Diagnostic Tooling for Mobile Platforms

Attaching `dotnet-trace` to a .NET MAUI application, allows you to
get profiling information in formats like `.nettrace` and
`.speedscope`. These give you CPU sampling information about the time
spent in each method in your application. This is quite useful for
finding *where* time is spent in the startup or general performance of
your .NET MAUI applications.

To use `dotnet-trace` on iOS and Android, the following
tools/components work together to make this happen:

* [`dotnet-trace`][dotnet-trace] itself is a .NET global tool.

* [`dotnet-dsrouter`][dotnet-dsrouter] is a .NET global tool that
  forwards a connection from a remote Android or iOS device or
  emulator to a local port on your development machine.

* [`dotnet-gcdump`][dotnet-gcdump] is a .NET global tool that can be
  used to collect memory dumps of .NET applications.

* The Mono Diagnostic component, `libmono-component-diagnostics_tracing.so`,
  is included in the application and is used to collect the trace data.

> **NOTE:** You need at least version 9.0.621003 of all the diagnostic
> tools to use the features described in this guide. Check
> [dotnet-trace](https://www.nuget.org/packages/dotnet-trace/),
> [dotnet-dsrouter](https://www.nuget.org/packages/dotnet-dsrouter/),
> and [dotnet-gcdump](https://www.nuget.org/packages/dotnet-gcdump/)
> on NuGet for the latest versions.

Generally, you can install the required tooling such as:

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

See the [`dotnet-trace` documentation][dotnet-trace] for further details about its usage.

[dotnet-trace]: /dotnet/core/diagnostics/dotnet-trace
[dotnet-dsrouter]: /dotnet/core/diagnostics/dotnet-dsrouter
[dotnet-gcdump]: /dotnet/core/diagnostics/dotnet-gcdump

## Using `dotnet-trace` on Android

### Using `dotnet-trace` with the `--dsrouter` option

Starting with version 9.0.621003, `dotnet-trace` includes a
`--dsrouter` option that eliminates the need to run `dotnet-dsrouter`
separately. This simplifies the workflow significantly.

For Android emulators:

```sh
$ dotnet-trace collect --dsrouter android-emu --format speedscope
WARNING: dotnet-dsrouter is a development tool not intended for production environments.
For finer control over the dotnet-dsrouter options, run it separately and connect to it using -p

No profile or providers specified, defaulting to trace profile 'cpu-sampling'

Provider Name                           Keywords            Level               Enabled By
Microsoft-DotNETCore-SampleProfiler     0x0000F00000000000  Informational(4)    --profile
Microsoft-Windows-DotNETRuntime         0x00000014C14FCCBD  Informational(4)    --profile
```

For Android devices:

```sh
# `adb reverse` is still required when using hardware devices
$ adb reverse tcp:9000 tcp:9001
$ dotnet-trace collect --dsrouter android --format speedscope
WARNING: dotnet-dsrouter is a development tool not intended for production environments.
For finer control over the dotnet-dsrouter options, run it separately and connect to it using -p

No profile or providers specified, defaulting to trace profile 'cpu-sampling'

Provider Name                           Keywords            Level               Enabled By
Microsoft-DotNETCore-SampleProfiler     0x0000F00000000000  Informational(4)    --profile
Microsoft-Windows-DotNETRuntime         0x00000014C14FCCBD  Informational(4)    --profile
```

The `--format` argument is optional and it defaults to `nettrace`.
However, `nettrace` files can be viewed only with Perfview or Visual
Studio on Windows, while the speedscope JSON files can be viewed "on"
Unix by opening them with [https://speedscope.app/][speedscope].

### Running `dotnet-dsrouter` Separately

> **NOTE:** The following section describes the approach before
> `dotnet-trace` 9.0.621003. Running `dotnet-dsrouter` separately can
> be useful for viewing its log messages when troubleshooting.

For profiling an Android application running on an Android *emulator*:

```sh
$ dotnet-dsrouter android-emu
How to connect current dotnet-dsrouter pid=1234 with android emulator and diagnostics tooling.
Start an application on android emulator with ONE of the following environment variables set:
[Default Tracing]
DOTNET_DiagnosticPorts=10.0.2.2:9000,nosuspend,connect
[Startup Tracing]
DOTNET_DiagnosticPorts=10.0.2.2:9000,suspend,connect
Run diagnotic tool connecting application on android emulator through dotnet-dsrouter pid=1234:
dotnet-trace collect -p 1234
See https://learn.microsoft.com/dotnet/core/diagnostics/dotnet-dsrouter for additional details and examples.

info: dotnet-dsrouter-1234[0]
      Starting dotnet-dsrouter using pid=1234
info: dotnet-dsrouter-1234[0]
      Starting IPC server (dotnet-diagnostic-dsrouter-1234) <--> TCP server (127.0.0.1:9000) router.
```

For profiling an Android application running on an Android *device*:

```sh
# `adb reverse` is required when using hardware devices
$ adb reverse tcp:9000 tcp:9001
$ dotnet-dsrouter android
How to connect current dotnet-dsrouter pid=1234 with android device and diagnostics tooling.
Start an application on android device with ONE of the following environment variables set:
[Default Tracing]
DOTNET_DiagnosticPorts=127.0.0.1:9000,nosuspend,connect
[Startup Tracing]
DOTNET_DiagnosticPorts=127.0.0.1:9000,suspend,connect
Run diagnotic tool connecting application on android device through dotnet-dsrouter pid=1234:
dotnet-trace collect -p 1234
...
```

### Android System Properties

Note the log message that `dotnet-dsrouter` prints that mentions
`$DOTNET_DiagnosticPorts`. `$DOTNET_DiagnosticPorts` is an environment
variable that could be defined in an `@(AndroidEnvironment)` file, but
it is simpler to use the `debug.mono.profile` Android system property.
Android system properties can be used without rebuilding the app.

For emulators, `$DOTNET_DiagnosticPorts` should specify an IP address
of 10.0.2.2:

```sh
adb shell setprop debug.mono.profile '10.0.2.2:9000,suspend,connect'
```

For devices, `$DOTNET_DiagnosticPorts` should specify an IP address of
127.0.0.1, and the port number should be the port used used with adb
reverse, e.g:

```sh
# `adb reverse` is required when using hardware devices
$ adb reverse tcp:9000 tcp:9001
$ adb shell setprop debug.mono.profile '127.0.0.1:9000,suspend,connect'
```

`suspend` is useful as it blocks application startup, so you can
actually `dotnet-trace` startup times of the application.

If you are wanting to collect a `gcdump` or just get things working,
try `nosuspend` instead. See the [`dotnet-dsrouter`
documentation][nosuspend] for further information.

[nosuspend]: /dotnet/core/diagnostics/dotnet-dsrouter#collect-a-trace-using-dotnet-trace-from-a-net-application-running-on-android

### Running `dotnet-trace` on the Host

First, run `dotnet-trace ps` to find a list of processes:

```sh
> dotnet-trace ps
 38604  dotnet-dsrouter  C:\Users\myuser\.dotnet\tools\dotnet-dsrouter.exe  "C:\Users\myuser\.dotnet\tools\dotnet-dsrouter.exe" android-emu --verbose debug
```

`dotnet-trace` knows how to tell if a process ID is `dotnet-dsrouter` and
connect *through it* appropriately.

Using the process ID from the previous step, run `dotnet-trace collect`:

```sh
$ dotnet-trace collect -p 38604 --format speedscope
No profile or providers specified, defaulting to trace profile 'cpu-sampling'

Provider Name                           Keywords            Level               Enabled By
Microsoft-DotNETCore-SampleProfiler     0x0000F00000000000  Informational(4)    --profile 
Microsoft-Windows-DotNETRuntime         0x00000014C14FCCBD  Informational(4)    --profile 

Waiting for connection on /tmp/maui-app
Start an application with the following environment variable: DOTNET_DiagnosticPorts=/tmp/maui-app
```

The `--format` argument is optional and it defaults to `nettrace`.
However, `nettrace` files can be viewed only with Perfview or Visual
Studio on Windows, while the speedscope JSON files can be viewed "on"
Unix by opening them with [https://speedscope.app/][speedscope].

[speedscope]: https://speedscope.app/

### Running `dotnet-trace` on the Host

First, run `dotnet-trace ps` to find a list of processes:

```sh
> dotnet-trace ps
 38604  dotnet-dsrouter  C:\Users\myuser\.dotnet\tools\dotnet-dsrouter.exe  "C:\Users\myuser\.dotnet\tools\dotnet-dsrouter.exe" android-emu --verbose debug
```

`dotnet-trace` knows how to tell if a process ID is `dotnet-dsrouter` and
connect *through it* appropriately.

Using the process ID from the previous step, run `dotnet-trace collect`:

```sh
$ dotnet-trace collect -p 38604 --format speedscope
No profile or providers specified, defaulting to trace profile 'cpu-sampling'

Provider Name                           Keywords            Level               Enabled By
Microsoft-DotNETCore-SampleProfiler     0x0000F00000000000  Informational(4)    --profile 
Microsoft-Windows-DotNETRuntime         0x00000014C14FCCBD  Informational(4)    --profile 

Waiting for connection on /tmp/maui-app
Start an application with the following environment variable: DOTNET_DiagnosticPorts=/tmp/maui-app
```

The `--format` argument is optional and it defaults to `nettrace`.
However, `nettrace` files can be viewed only with Perfview or Visual
Studio on Windows, while the speedscope JSON files can be viewed "on"
Unix by opening them with [https://speedscope.app/][speedscope].

### Running the .NET for Android Application

`$(AndroidEnableProfiler)` must be set to `true` as it includes the
Mono diagnostic component in the application. This component is the
`libmono-component-diagnostics_tracing.so` native library.

```sh
dotnet build -f net9.0-android -t:Run -c Release -p:AndroidEnableProfiler=true
```

*NOTE: `-f net9.0-android` is only needed for projects with multiple `$(TargetFrameworks)`.*

Once the application is installed and started, `dotnet-trace` should show something similar to:

```
Process        : $HOME/.dotnet/tools/dotnet-dsrouter
Output File    : /tmp/hellomaui-app-trace
[00:00:00:35]    Recording trace 1.7997   (MB)
Press <Enter> or <Ctrl+C> to exit...812  (KB)
```

Once `<Enter>` is pressed, you should see:

```
Stopping the trace. This may take up to minutes depending on the application being traced.

Trace completed.
Writing:    hellomaui-app-trace.speedscope.json
```

And the output files should be found in the current directory. You can
use the `-o` switch if you would prefer to output them to a specific
directory.

## Running `dotnet-gcdump`

To get memory information from an Android application, you need all
the above setup for `adb shell`, `dsrouter`, etc. except you need to
simply use `dotnet-gcdump` instead of `dotnet-trace`:

```sh
dotnet-gcdump collect -p 38604
```

This will create a `*.gcdump` file in the current directory. You can
open this file on Windows in Visual Studio or [PerfView][perfview].

Note that using `nosuspend` in the `debug.mono.profile` property is
useful, as it won't block application startup.

[perfview]: https://github.com/microsoft/perfview

## Using `dotnet-trace` on iOS

TODO

## Measuring Startup Time, CPU Usage, or Scrolling Responsiveness

TODO

## Measuring Memory Usage or Leaks

TODO
