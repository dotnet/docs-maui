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

> **NOTE:** You need at least version 9.0.652701 of all the diagnostic
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

See the .NET Conf Session, [.NET Diagnostic Tooling with
AI][dotnetconf], for a live demo of using these tools.

See the [`dotnet-trace` documentation][dotnet-trace] for further
details about its usage.

[dotnet-trace]: /dotnet/core/diagnostics/dotnet-trace
[dotnet-dsrouter]: /dotnet/core/diagnostics/dotnet-dsrouter
[dotnet-gcdump]: /dotnet/core/diagnostics/dotnet-gcdump
[dotnetconf]: https://youtu.be/HLNYCwgk5fU

## Using `dotnet-trace`

Starting with version 9.0.652701, `dotnet-trace` includes a
`--dsrouter` option that eliminates the need to run `dotnet-dsrouter`
separately. This simplifies the workflow significantly.

For Android emulators:

```sh
$ dotnet-trace collect --dsrouter android-emu
For finer control over the dotnet-dsrouter options, run it separately and connect to it using -p

WARNING: dotnet-dsrouter is a development tool not intended for production environments.

How to connect current dotnet-dsrouter pid=90312 with android emulator and diagnostics tooling.
Build and run your application on android emulator such as:
[Default Tracing]
dotnet build -t:Run -c Release -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
[Startup Tracing]
dotnet build -t:Run -c Release -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-dsrouter for additional details and examples.
```

For Android devices:

```sh
$ dotnet-trace collect --dsrouter android --format speedscope
For finer control over the dotnet-dsrouter options, run it separately and connect to it using -p

WARNING: dotnet-dsrouter is a development tool not intended for production environments.

How to connect current dotnet-dsrouter pid=76412 with android device and diagnostics tooling.
Build and run your application on android device such as:
[Default Tracing]
dotnet build -t:Run -c Release -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
[Startup Tracing]
dotnet build -t:Run -c Release -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=true -p:DiagnosticListenMode=connect
See https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-dsrouter for additional details and examples.
```

For iOS, you can use `--dsrouter ios-sim` for Simulators and
`--dsrouter ios` for iOS devices. Note that `-p:DiagnosticListenMode=listen`
is the recommended value for iOS.

The `--format` argument is optional and it defaults to `nettrace`.
However, `nettrace` files can be viewed only with Perfview or Visual
Studio on Windows, while the speedscope JSON files can be viewed "on"
macOS or Linux by opening them with [https://speedscope.app/][speedscope].

### Building your Application with Diagnostics

Note the log message that `dotnet-dsrouter` prints that mentions
various `Diagnostic` MSBuild properties:

```sh
dotnet build -t:Run -c Release -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

*NOTE: `-f net10.0-android` or `-f net10.0-ios` is needed for projects with multiple `$(TargetFrameworks)`.*

For emulators, `-p:DiagnosticAddress` should specify an IP address
of 10.0.2.2:

```sh
dotnet build -t:Run -c Release -p:DiagnosticAddress=10.0.2.2 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

*NOTE: `-p:DiagnosticListenMode=listen` is the recommended value for iOS.*

`-p:DiagnosticSuspend=true` is useful as it blocks application
startup, so you can actually `dotnet-trace` startup times of the
application.

If you are wanting to collect a `gcdump` or just get things working,
try `-p:DiagnosticSuspend=false` instead. See the [`dotnet-dsrouter`
documentation][nosuspend] for further information.

Building your application with these settings, will encode the values
*into* the application. This makes the produced application include
the .NET diagnostic component(s) that will try to communicate with
`dotnet-trace` and other tools. These builds should be for
development/testing-only and not released to production.

[nosuspend]: /dotnet/core/diagnostics/dotnet-dsrouter#collect-a-trace-using-dotnet-trace-from-a-net-application-running-on-android

### Running your Application and Saving a Trace

Once the application is installed and started, `dotnet-trace --dsrouter ...`
should show something similar to:

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
the above setup for building your application.

However, you can simply use the same `--dsrouter` switch as you would
for `dotnet-trace`:

```sh
dotnet-gcdump collect --dsrouter android
```

This will create a `*.gcdump` file in the current directory. You can
open this file on Windows in Visual Studio or [PerfView][perfview].

Note that using the `-p:DiagnosticSuspend=false` MSBuild property is
useful, as it won't block application startup.

[perfview]: https://github.com/microsoft/perfview

## Measuring Startup Time or CPU Usage

For measuring startup time, build your application with the setting
mentioned: `-p:DiagnosticSuspend=true`. Upon launching your
application, you will notice it will "pause" indefinitely on the
splash screen waiting for a connection to `dotnet-trace` and/or
`dotnet-dsrouter`.

For measuring other operations, such as:

* A long operation, triggered by a `Button` tap.

* A slow navigation.

* Slow scrolling.

Use `-p:DiagnosticSuspend=false`, and connect `dotnet-trace` /
`dotnet-dsrouter` *after* the application has launched. In reasonably
quick succession:

* Get to the appropriate location in your application.

* Connect `dotnet-trace`

* Perform the steps you want to profile

* Stop `dotnet-trace`

This will more successfully get a *targeted* trace that will be easier
to read and understand what went wrong.

## Measuring Memory Usage or Leaks

TODO
