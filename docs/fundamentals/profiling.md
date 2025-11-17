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

## Prerequisites

### Installing Diagnostic Tools

To profile .NET MAUI applications on iOS and Android, you need to
install the following .NET global tools:

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
> You need at least version 9.0.652701 of all the diagnostic tools to
> use the features described in this guide. Check
> [dotnet-trace](https://www.nuget.org/packages/dotnet-trace/),
> [dotnet-dsrouter](https://www.nuget.org/packages/dotnet-dsrouter/),
> and [dotnet-gcdump](https://www.nuget.org/packages/dotnet-gcdump/)
> on NuGet for the latest versions.

Starting with version 9.0.652701, both `dotnet-trace` and
`dotnet-gcdump` include a `--dsrouter` option that automatically
launches and manages `dotnet-dsrouter` as a subprocess. This
eliminates the need to run `dotnet-dsrouter` separately, significantly
simplifying the profiling workflow.

See the .NET Conf session, [.NET Diagnostic Tooling with
AI][dotnetconf], for a live demo of using these tools.

[dotnet-trace]: /dotnet/core/diagnostics/dotnet-trace
[dotnet-dsrouter]: /dotnet/core/diagnostics/dotnet-dsrouter
[dotnet-gcdump]: /dotnet/core/diagnostics/dotnet-gcdump
[dotnetconf]: https://youtu.be/HLNYCwgk5fU

### How the Tools Work Together

To use these diagnostic tools on iOS and Android, several components
work together:

- The .NET global tools (`dotnet-trace`, `dotnet-gcdump`,
  `dotnet-dsrouter`) run on your development machine
- The Mono diagnostic component
  (`libmono-component-diagnostics_tracing.so`) is included in your
  application package
- `dotnet-dsrouter` forwards the diagnostic connection from the remote
  device or emulator to a local port on your machine
- The diagnostic tools connect to this local port to collect profiling
  data

The `--dsrouter` option in `dotnet-trace` and `dotnet-gcdump`
automatically handles the complexity of starting `dotnet-dsrouter` and
coordinating the connection.

## Building Your Application for Profiling

To enable profiling, your application must be built with special
MSBuild properties that include the diagnostic components and
configure the connection to the profiling tools.

### Understanding Diagnostic Properties

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

### Build Command Examples

When you run `dotnet-trace` or `dotnet-gcdump` with the `--dsrouter`
option, the tool displays instructions for building your application.
For example:

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

### Profiling Runtime Operations

To profile specific operations during runtime (such as button taps,
navigation, or scrolling), use `-p:DiagnosticSuspend=false` and
connect the profiler after the application has launched.

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

## Profiling Memory Usage

Memory profiling helps you identify memory leaks and understand memory
allocation patterns in your application. Use `dotnet-gcdump` to create
snapshots of managed memory.

### Collecting Memory Dumps

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
