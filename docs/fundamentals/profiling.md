---
title: "Performance Profiling"
description: "Learn how to profile the performance of your .NET MAUI app."
ms.date: 08/06/2026
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
> measurements. `Debug` builds use the interpreter (`UseInterpreter=true`)
> for C# hot reload support, which significantly impacts performance
> and produces unrealistic results.

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
- `dotnet-dsrouter` forwards the diagnostic connection from the remote
  device or emulator to a local port on your machine
- The diagnostic tools connect to this local port to collect profiling
  data

::: moniker range="<=net-maui-10.0"

On Android, iOS, and Mac Catalyst, the Mono diagnostics component is
included in the application package. The component is enabled when you
set one of the `Diagnostic*` MSBuild properties.

::: moniker-end

::: moniker range=">=net-maui-11.0"

Starting in .NET 11, CoreCLR is the default runtime on all .NET MAUI
platforms. Its diagnostics support is part of the runtime, so a separate
Mono diagnostics component isn't required.

::: moniker-end

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

- **`EnableDiagnostics`**: When `true`, includes the Mono diagnostic
  component in the application package. This is implicitly set when
  setting any of the `Diagnostic*` MSBuild properties. This property
  applies to apps that use the Mono runtime.

When using CoreCLR, diagnostics are built into the runtime and
`EnableDiagnostics` isn't required.

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

1. Build your application for `Release` with [ReadyToRun
   enabled][r2r]:

   ```sh
   dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true
   ```

2. Launch PerfView and select `Collect` > `Collect`.

3. In the **Command** field, filter on your app's executable (for
   example, `hellomaui.exe`).

4. Click **Start Collection**, then manually launch your app.

5. Click **Stop Collection** once your app has completed the operation
   you want to profile.

6. Open **CPU Stacks** to view timing information, or use the **Flame
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

```sh
dotnet publish -f net10.0-windows10.0.19041.0 -c Release -p:PublishReadyToRun=true -p:WindowsPackageType=None
dotnet trace collect --format speedscope -- bin\Release\net10.0-windows10.0.19041.0\win10-x64\publish\YourApp.exe
```

## Profiling on iOS and Mac Catalyst with Instruments

For iOS and Mac Catalyst applications, Apple's Instruments tool
provides native profiling with detailed insights into app launch time
and performance.

### Using Instruments for App Launch Profiling

1. Build your app for `Release` with symbols preserved:

   ```sh
   dotnet build -c Release -f net10.0-ios -p:NoSymbolStrip=true
   ```

   The `NoSymbolStrip=true` property keeps native symbols in the
   executable, making stack traces in Instruments much more helpful.

2. Install the app on your device:

   ```sh
   dotnet build -t:Run -c Release -f net10.0-ios -p:NoSymbolStrip=true
   ```

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

## Profiling Memory Usage

Memory profiling can help you distinguish a memory leak from high
allocation rates, deliberate caching, and temporary memory growth. A
.NET MAUI app on iOS or Mac Catalyst uses multiple kinds of memory:

- **Managed memory** contains objects controlled by the .NET garbage
  collector (GC).
- **Native memory** contains Objective-C objects, native buffers,
  graphics resources, and allocations made by platform frameworks.
- **Virtual memory** includes mapped files and other regions that
  contribute to the app's memory footprint.

No single tool shows every relationship between these kinds of memory.
Use `dotnet-gcdump` to inspect managed objects and their roots. Use
Xcode Instruments to inspect the process footprint, native allocations,
and native retain cycles.

> [!IMPORTANT]
> A growing process footprint doesn't necessarily indicate a leak.
> Garbage collection, allocator reuse, caches, image decoding, and
> autorelease pools can all delay a reduction in the footprint. Look
> for persistent growth after repeating the same operation and returning
> the app to the same state.

### Create a repeatable test

Before collecting snapshots, define a small scenario that reliably
causes memory to grow. For example:

1. Start the app and wait for startup work to finish.
2. Navigate to a details page and exercise the feature under test.
3. Navigate back and wait for animations and asynchronous work to
   finish.
4. Repeat the same operation at least five times.
5. Record memory after each iteration.

Run one or two warm-up iterations before recording a baseline. A cache
may grow during the first iteration and then stabilize. A leak typically
produces a stair-step pattern where retained memory increases after
every iteration.

Profile a `Release` build, but temporarily disable trimming if it
prevents the diagnostic code or types that you're investigating from
being present. Restore the app's normal settings when you verify the
fix.

### Check whether an object can be collected

A weak reference is a quick way to test whether a page, viewmodel, or
handler remains alive after it should have been released:

```csharp
static WeakReference<MyDetailsPage> CreatePageReference()
{
    var page = new MyDetailsPage();

    // Exercise the page, navigate away, and perform the same cleanup
    // that the app performs in the scenario under investigation.
    return new WeakReference<MyDetailsPage>(page);
}

static void CheckPageCollected()
{
    WeakReference<MyDetailsPage> pageReference = CreatePageReference();

    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    System.Diagnostics.Debug.WriteLine(
        pageReference.TryGetTarget(out _)
            ? "MyDetailsPage is still alive"
            : "MyDetailsPage was collected");
}
```

Create the object in a separate method when possible. Local variables
can remain live until the end of a method, even after their last apparent
use, and produce a false positive.

> [!WARNING]
> Forced garbage collections alter app behavior and can cause long
> pauses. Use them only to make a diagnostic check deterministic, and
> remove them from production code.

### Collecting Memory Dumps

Use `dotnet-gcdump` to capture a graph of the managed GC heap. A GC dump
contains managed type counts, object sizes, and references from roots.
It doesn't contain native allocations or the complete process memory.

On iOS, build and run the app with `DiagnosticSuspend=false`, as
described in [Building Your Application for
Profiling](#building-your-application-for-profiling). Then capture a
baseline GC dump:

```sh
dotnet-gcdump collect --dsrouter ios-sim -o before.gcdump
```

For a physical iOS device, use the `ios` profile:

```sh
dotnet-gcdump collect --dsrouter ios -o before.gcdump
```

Exercise the repeatable scenario several times without restarting the
app, and capture another dump:

```sh
dotnet-gcdump collect --dsrouter ios-sim -o after.gcdump
```

Use `--dsrouter ios` instead when capturing `after.gcdump` from a
physical iOS device.

Use `--dsrouter android` or `--dsrouter android-emu` to run the same
workflow on Android.

For Mac Catalyst and macOS, you can run the executable inside the app
bundle as a macOS process and use a Unix domain socket as the diagnostic
port. In one terminal, set `DOTNET_DiagnosticPorts` and start the app:

```sh
DOTNET_DiagnosticPorts="$TMPDIR/maui-gcdump.socket,suspend" \
  /path/to/MyApp.app/Contents/MacOS/MyApp
```

In another terminal, collect the dump:

```sh
dotnet-gcdump collect \
  --diagnostic-port "$TMPDIR/maui-gcdump.socket" \
  -o before.gcdump
```

Use the same diagnostic port to capture `after.gcdump` after exercising
the repeatable scenario. The app and `dotnet-gcdump` must use the same
`TMPDIR`.

> [!NOTE]
> The App Sandbox can prevent a Mac Catalyst or macOS development build
> from opening the diagnostic connection. If necessary, use a
> profiling-only entitlements file without the
> `com.apple.security.app-sandbox` entitlement. Don't distribute this
> build.

> [!WARNING]
> Collecting a GC dump triggers a full garbage collection and can
> suspend the app for a significant time when the managed heap is large.

[perfview]: https://github.com/microsoft/perfview

### Analyzing Memory Dumps

Use `dotnet-gcdump report` on any supported development platform to
compare type statistics:

```sh
dotnet-gcdump report before.gcdump
dotnet-gcdump report after.gcdump
```

To inspect reference paths and compare snapshots graphically, open both
`*.gcdump` files in Visual Studio on Windows or in PerfView. The
`*.gcdump` graphical viewers aren't available on macOS, but dumps
captured on a Mac can be copied to a Windows computer for analysis.

When you compare the dumps:

- Sort by the increase in object count and total size.
- Find types associated with the repeated operation, such as the page,
  viewmodel, handler, image, or collection item.
- Inspect paths to roots to determine why an object remains reachable.
- Repeat the capture after more iterations to confirm that the count
  continues to grow rather than stabilizing.

Common managed roots include static fields, singleton services,
long-running tasks, timers, event publishers, notification callbacks,
and collections that aren't cleared. A managed reference cycle by
itself isn't a leak: the GC can reclaim a cycle when no root can reach
it.

For more information about GC dumps, see the [`dotnet-gcdump`
documentation][dotnet-gcdump].

## Diagnose memory leaks on iOS and Mac Catalyst

Managed heap analysis is only one half of an investigation on Apple
platforms. UIKit, Foundation, Core Animation, image APIs, and third-party
native libraries allocate outside the .NET GC heap. Objects that derive
from <xref:Foundation.NSObject> also have both managed and native
representations.

### Profile native memory with Instruments

Use the **Allocations** and **Leaks** instruments to investigate native
memory:

1. Build and deploy a `Release` build with native symbols preserved:

   ::: moniker range="<=net-maui-10.0"

   ```sh
   dotnet build -t:Run -c Release -f net10.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

   ::: moniker range=">=net-maui-11.0"

   ```sh
   dotnet run -c Release -f net11.0-ios -p:NoSymbolStrip=true
   ```

   ::: moniker-end

   Use the corresponding `maccatalyst` target framework to profile a
   Mac Catalyst app.

   > [!NOTE]
   > Mac Catalyst and macOS apps must include the
   > `com.apple.security.get-task-allow` entitlement for Instruments to
   > attach to the process. Enable this entitlement only in the build
   > configuration used for profiling.

2. Open Xcode, select **Xcode** > **Open Developer Tool** >
   **Instruments**, and choose the **Allocations** template.
3. Select the app and device, and start recording.
4. Run the warm-up iterations and select **Mark Generation**.
5. Exercise the repeatable scenario, return to the baseline screen, and
   mark another generation. Repeat this step several times.
6. Stop recording. Inspect allocations that were created after each
   generation and are still living. Sort by persistent bytes and object
   count, and examine allocation stacks for types that grow in every
   generation.

Add the **Leaks** instrument to scan for unreachable native
allocations. A clean Leaks result doesn't prove that the app has no
memory leak. The Leaks instrument doesn't identify abandoned memory
that is still reachable, such as objects retained by an unintended
cache or callback. It also doesn't show managed GC reference paths.

Allocation stack recording adds overhead, but it helps identify the
native or managed-to-native call that created an allocation. For more
information, see [Analyze heap memory][apple-heap-memory] and [Gathering
information about memory use][apple-memory-use].

[apple-heap-memory]: https://developer.apple.com/videos/play/wwdc2024/10173/
[apple-memory-use]: https://developer.apple.com/documentation/xcode/gathering-information-about-memory-use

### Correlate managed and native results

Compare what each tool reports:

- If `dotnet-gcdump` shows an increasing number of pages, viewmodels, or
  handlers, follow their managed paths to roots.
- If the managed heap stabilizes but Instruments shows persistent
  growth, investigate native types, images, graphics resources, and
  buffers.
- If an <xref:Foundation.NSObject>-derived managed type grows and its
  managed root is unclear, inspect both tools. A native object can retain
  the native peer, which in turn keeps the managed object alive.

### Narrow down the cause

Once you've identified a leak, reduce the page or feature until the
growth stops:

1. Remove the XAML content. Does the leak still occur?
2. Remove application code and subscriptions. Does the leak still
   occur?
3. Test on multiple platforms. Does the leak only occur on one
   platform?

An empty <xref:Microsoft.Maui.Controls.ContentPage> should become
collectable after navigation has released it. Restore groups of controls
and application code incrementally to isolate the source of the leak.

### Common Leak Patterns

#### C# events

An event publisher holds a strong reference to each subscribed delegate,
and the delegate usually holds a strong reference to its target. If the
publisher outlives the subscriber, the subscription can therefore keep
the subscriber and its object graph alive. For example, an event
subscription from a page to a long-lived publisher, such as a
<xref:Microsoft.Maui.Controls.Style> stored in
<xref:Microsoft.Maui.Controls.Application.Resources>, can retain the
entire page.

A managed reference cycle doesn't prevent collection when no root can
reach it. The leak occurs when a rooted, long-lived publisher provides a
path to a subscriber that should no longer be reachable.

To prevent this pattern, unsubscribe when the subscriber is no longer
needed, or use <xref:Microsoft.Maui.WeakEventManager> when weak event
semantics are appropriate. Also cancel timers and remove native
notification observer tokens that can retain their callbacks.

- **Unbounded collections and caches**: Static collections, navigation
  history, and caches can retain otherwise unused object graphs. Bound
  their size and remove stale entries.
- **Long-running asynchronous work**: Tasks, callbacks, and cancellation
  registrations can capture a page or viewmodel. Cancel the operation
  and release registrations when leaving the page.
- **Handlers and platform views**: Custom handlers can retain controls,
  delegates, or native views after a page is removed. Disconnect manual
  subscriptions and dispose native resources that the handler owns.
- **Managed/native strong-reference cycles**: Objective-C uses reference
  counting while .NET uses tracing garbage collection. A native
  container can retain an `NSObject` peer while the corresponding
  managed object retains the container. Break back references, use a
  weak reference where appropriate, remove native children from their
  containers, and dispose objects you own.

For an example of a managed/native strong-reference cycle and ways to
break it, see [Avoid strong circular references on iOS and Mac
Catalyst](~/deployment/performance.md#avoid-strong-circular-references-on-ios-and-mac-catalyst).

> [!CAUTION]
> Don't dispose an `NSObject` merely because it implements
> <xref:System.IDisposable>. Dispose wrappers for native resources that
> your code owns and no longer uses. Disposing a shared or framework-owned
> object can cause later native calls to fail.

### Runtime differences in .NET 10 and .NET 11

::: moniker range="<=net-maui-10.0"

.NET MAUI apps on Android, iOS, and Mac Catalyst use the Mono runtime by
default in .NET 10. `dotnet-gcdump` supports the Mono managed heap and
is the recommended snapshot workflow.

Mono emits low-level GC and allocation events through the
`Microsoft-DotNETRuntimeMonoProfiler` EventPipe provider. CoreCLR
`Microsoft-Windows-DotNETRuntime` GC profiles don't produce equivalent
data on Mono. The Mono profiler provider is disabled by default in
modern .NET versions and requires explicit `MONO_DIAGNOSTICS`
configuration for low-level allocation tracing. This configuration
isn't required for `dotnet-gcdump`.

For more information, see [MonoVM diagnostics
tracing][mono-diagnostics].

::: moniker-end

::: moniker range=">=net-maui-11.0"

CoreCLR is the only supported runtime for .NET 11 mobile apps, including
Android, iOS, Mac Catalyst, and tvOS. This unifies the runtime
diagnostics implementation across desktop, server, and mobile .NET. The
`dotnet-gcdump` and `dotnet-dsrouter` workflow remains the same, but:

- The diagnostics server and EventPipe support are built into CoreCLR,
  so the Mono diagnostics component isn't required.
- CoreCLR uses the standard .NET runtime EventPipe providers. Mono
  profiler provider names, keyword masks, and `MONO_DIAGNOSTICS`
  settings don't apply.
- CoreCLR provides more complete runtime events and managed root
  information than the Mono-specific event mapping.
- CoreCLR's GC replaces Mono's SGen GC. Heap sizes, collection timing,
  and counter values can therefore differ from the same app on .NET 10.

If an app requires the Mono runtime, it must continue to target .NET 10.
Setting `<UseMonoRuntime>true</UseMonoRuntime>` for a .NET 11 mobile
target produces build error `NETSDK1242`. Establish a new memory
baseline after moving an app to CoreCLR rather than comparing absolute
heap values with its .NET 10 Mono baseline.

CoreCLR doesn't make native allocations visible in a managed GC dump:
Instruments is still required for native heap and retain-cycle
analysis. The CoreCLR transition also doesn't make `dotnet-dump`
attach over the mobile `dotnet-dsrouter` transport; use
`dotnet-gcdump` for a managed heap graph on mobile.

For more information, see [NETSDK1242][netsdk1242] and the [.NET 11
Preview 4 .NET MAUI release notes][maui-net11-preview4].

::: moniker-end

[mono-diagnostics]: https://github.com/dotnet/runtime/blob/main/docs/design/mono/diagnostics-tracing.md
[netsdk1242]: /dotnet/core/tools/sdk-errors/netsdk1242
[maui-net11-preview4]: https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview4/dotnetmaui.md

### Native AOT limitations

Native AOT apps don't support managed heap analysis. Reproduce and
diagnose the managed retention problem in a non-Native AOT build, then
verify the fix in the published Native AOT app. Instruments can still
profile supported native aspects of a Native AOT process, but it can't
replace a managed heap graph.

For more information, see [Native AOT diagnostic support on iOS and Mac
Catalyst](~/deployment/nativeaot.md#native-aot-diagnostic-support-on-ios-and-mac-catalyst).

### Prevent regressions

After fixing a leak:

- Repeat the original scenario for more iterations and confirm that
  managed object counts and native persistent bytes stabilize.
- Add a test that creates the affected page, view, or handler, stores a
  weak reference, releases it, waits for pending finalizers, and asserts
  that the object is collected.
- Test on each affected platform. A managed root may reproduce
  everywhere, while a managed/native ownership cycle may only reproduce
  on iOS or Mac Catalyst.
- Keep profiling code, forced collections, and diagnostics-enabled app
  packages out of production builds.

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

- [.NET MAUI Profiling Wiki][maui-profiling] - Comprehensive wiki with
  advanced scenarios and troubleshooting
- [Android Tracing Guide][android-tracing] - Detailed Android-specific
  profiling instructions
- [iOS/macOS Profiling Wiki][macios-profiling] - Platform-specific
  guidance for Apple platforms
- [.NET Diagnostic Tools Documentation][dotnet-diagnostics] - Official
  documentation for `dotnet-trace`, `dotnet-dsrouter`, and
  `dotnet-gcdump`
- [PerfView User's Guide][perfview-guide] - In-depth guide to using
  PerfView for Windows profiling

[maui-profiling]: https://github.com/dotnet/maui/wiki/Profiling-.NET-MAUI-Apps
[android-tracing]: https://github.com/dotnet/android/blob/main/Documentation/guides/tracing.md
[macios-profiling]: https://github.com/dotnet/macios/wiki/Profiling
[dotnet-diagnostics]: /dotnet/core/diagnostics/
[perfview-guide]: https://github.com/microsoft/perfview/blob/main/documentation/Markdown/GettingStarted.md
