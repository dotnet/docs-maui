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

The workflow for profiling your .NET MAUI application depends on
whether you're measuring startup time or profiling runtime operations.
The key difference is the `-p:DiagnosticSuspend` MSBuild property,
which controls whether your application waits for the profiler to
connect before starting.

### Profiling Startup Time

To capture accurate startup time measurements, you need to suspend
application startup until the profiler is ready. This ensures you
capture the entire startup sequence from the very beginning.

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
   `-p:DiagnosticSuspend=true`:

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
   `dotnet-trace` to connect. Once connected, `dotnet-trace` will
   begin recording, and your application will start normally.

4. Allow your application to fully start and reach the initial screen.

5. Press `<Enter>` in the `dotnet-trace` terminal to stop recording.
   The trace file will be saved to the current directory.

> [!TIP]
> Use `dotnet build -t:Run` instead of `dotnet run` for better build
> progress visibility, especially for `Release` builds which can take
> several seconds.

### Profiling Runtime Operations

To profile specific operations during runtime, such as button taps,
navigation, or scrolling performance, you should use
`-p:DiagnosticSuspend=false` and connect the profiler after the
application has launched.

1. Build and deploy your application with `-p:DiagnosticSuspend=false`:

   ```sh
   dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
   ```

2. Navigate to the area of your application you want to profile.

3. Start `dotnet-trace` with the `--dsrouter` option:

   ```sh
   dotnet-trace collect --dsrouter android --format speedscope
   ```

4. Perform the operation you want to profile (such as tapping a button,
   navigating to a page, or scrolling through a list).

5. Press `<Enter>` to stop the trace.

This approach produces a more focused trace file that's easier to
analyze, as it only contains the specific operation you're
investigating rather than the entire application lifecycle.

### Understanding Diagnostic Properties

The MSBuild properties used for profiling control how your application
communicates with the diagnostic tools:

- **`DiagnosticAddress`**: The IP address where `dotnet-dsrouter` is
  listening. Use `10.0.2.2` for Android emulators (this is the host
  machine's loopback address from the emulator's perspective), and
  `127.0.0.1` for physical devices and iOS.

- **`DiagnosticPort`**: The port number for the diagnostic connection
  (default is `9000`).

- **`DiagnosticSuspend`**: When `true`, the application waits for the
  profiler to connect before starting. When `false`, the application
  starts immediately and the profiler can connect later.

- **`DiagnosticListenMode`**: Set to `connect` for Android (the app
  connects to `dotnet-dsrouter`), or `listen` for iOS (the app listens
  for `dotnet-dsrouter` to connect to it).

> [!IMPORTANT]
> Applications built with these diagnostic properties should only be
> used for development and testing. Never release builds with
> diagnostic components enabled to production, as they include
> additional components and can expose diagnostic endpoints.

## Measuring Memory Usage or Leaks

Memory leaks in .NET MAUI applications can manifest as steadily
increasing memory usage, especially during repeated navigation or
interactions. On mobile platforms, this can eventually cause the OS to
terminate your application due to excessive memory consumption.

### Collecting Memory Snapshots with `dotnet-gcdump`

The `dotnet-gcdump` tool creates snapshots of all managed (C#) objects
in memory at a given point in time. This allows you to inspect what
objects exist, how many instances there are, and what's holding
references to them.

To collect a memory dump from a mobile application, use the same
`--dsrouter` workflow as `dotnet-trace`:

```sh
dotnet-gcdump collect --dsrouter android
```

For other platforms, use `--dsrouter android-emu`, `--dsrouter ios`,
or `--dsrouter ios-sim` as appropriate.

Unlike CPU tracing, you typically want to use
`-p:DiagnosticSuspend=false` when collecting memory dumps, as
suspending the application startup isn't necessary:

```sh
dotnet build -t:Run -c Release -f net10.0-android -p:DiagnosticAddress=127.0.0.1 -p:DiagnosticPort=9000 -p:DiagnosticSuspend=false -p:DiagnosticListenMode=connect
```

Once `dotnet-gcdump` connects, it will create a `*.gcdump` file in the
current directory. You can open this file in Visual Studio on Windows
or [PerfView][perfview] to explore the memory contents.

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

### Determining if a Leak Exists

The symptom of a memory leak might be:

1. Navigate from the main page to a details page
2. Navigate back
3. Navigate to the details page again
4. Memory grows consistently with each cycle

To determine if a page is actually leaking:

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
> They don't occur on Android or Windows.

### Best Practices

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

[adb-logcat]: /xamarin/android/deploy-test/debugging/android-debug-log
[maui-memory-leaks]: https://github.com/dotnet/maui/wiki/Memory-Leaks
