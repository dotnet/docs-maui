---
title: "Runtimes and compilation in .NET MAUI"
description: "Learn about the runtimes and compilation strategies used by .NET MAUI apps, including CoreCLR, NativeAOT, ReadyToRun, and interpreters."
ms.date: 09/10/2026
---

# Runtimes and compilation in .NET MAUI

.NET Multi-platform App UI (.NET MAUI) apps can use different runtimes and
compilation strategies depending on the target platform, build configuration,
and deployment model. This article explains the key terms and the runtime and
compilation strategies that .NET MAUI uses on each platform.

## Runtimes

A .NET *runtime* is the execution environment that manages your app's memory,
type system, garbage collection, and code execution. .NET MAUI apps use one of
the following runtimes:

::: moniker range="<=net-maui-10.0"

### Mono

[Mono](https://www.mono-project.com/docs/) is the cross-platform .NET runtime
that has historically powered Xamarin apps and .NET MAUI apps on Android, iOS,
and Mac Catalyst. Mono supports both just-in-time (JIT) compilation and
ahead-of-time (AOT) compilation. Mono is the default runtime for .NET MAUI apps
on mobile and Mac Catalyst platforms.

::: moniker-end

::: moniker range=">=net-maui-11.0"

Mono isn't supported for .NET MAUI apps that target .NET 11+.

::: moniker-end

### CoreCLR

CoreCLR is the .NET Common Language Runtime used by .NET on desktop and server
platforms. It powers ASP.NET Core, console apps, and Windows desktop apps.
CoreCLR features a highly optimizing JIT compiler, tiered compilation, and a
full set of runtime diagnostics.

::: moniker range="<=net-maui-10.0"

In .NET MAUI, CoreCLR is the runtime used on Windows. In .NET 10, CoreCLR is
also available as an experimental option for Android.

::: moniker-end

::: moniker range=">=net-maui-11.0"

In .NET 11+, CoreCLR is the runtime for .NET MAUI apps on Windows, Android,
iOS, and Mac Catalyst. NativeAOT is an opt-in alternative for publishing.

::: moniker-end

### NativeAOT

When you publish with [Native AOT](nativeaot.md), your app doesn't run on the
CoreCLR runtime. Instead, it runs on a minimal NativeAOT runtime that's
statically linked into your native binary. This runtime includes a garbage
collector and type system but no JIT compiler or interpreter. All code is
compiled to native machine code at build time.

## Compilation strategies

A *compilation strategy* determines how your C# code is turned into machine code
that the processor can execute. .NET MAUI uses several compilation strategies,
depending on the runtime and deployment scenario.

### JIT (Just-in-Time) compilation

JIT compilation translates Microsoft Intermediate Language (MSIL) into native
machine code at runtime as methods are called for the first time. JIT
compilation supports fast build-deploy-debug cycles and features such as Edit
and Continue.

::: moniker range="<=net-maui-10.0"

- **Used by**: CoreCLR on Windows and experimentally on Android; Mono in
  Android debug builds
- **Advantages**: Fast build times, full runtime diagnostics, and dynamic code
  generation support
- **Disadvantages**: Slower startup because code is compiled on the device at
  runtime

::: moniker-end

::: moniker range=">=net-maui-11.0"

- **Used by**: CoreCLR on Windows and Android
- **Advantages**: Fast build times, full runtime diagnostics, and dynamic code
  generation support
- **Disadvantages**: Slower startup when code isn't already compiled by
  ReadyToRun

Apple CoreCLR device targets don't use JIT because Apple platform restrictions
prevent dynamically generated code. They use the CoreCLR interpreter instead.

::: moniker-end

::: moniker range="<=net-maui-10.0"

### Mono AOT (Ahead-of-Time) compilation

Mono AOT compilation precompiles MSIL into native code at build time using
Mono's AOT compiler. This is the default compilation mode for .NET MAUI release
builds on iOS, Mac Catalyst, and Android.

- **Used by**: Mono runtime on iOS, Mac Catalyst, and Android
- **Advantages**: Faster startup than JIT and required for platforms that
  restrict dynamic code generation
- **Disadvantages**: Larger app size than JIT-only builds and limited support
  for some dynamic features

Mono AOT is not the same as NativeAOT. With Mono AOT, your app still includes
the Mono runtime and can optionally use the Mono interpreter for code that
wasn't AOT-compiled. NativeAOT compiles all code ahead of time and doesn't
include a JIT or interpreter. Mono also supports a Full AOT mode where no
interpreter or JIT is available. Full AOT is used by default for Mono release
builds on iOS and Mac Catalyst.

::: moniker-end

### NativeAOT (Native Ahead-of-Time) compilation

NativeAOT compiles your entire app, its dependencies, and a minimal runtime
into a native binary at build time. There is no JIT compiler or interpreter at
runtime. NativeAOT performs full trimming and static analysis, which can
produce smaller app sizes and faster startup times, but it places more
restrictions on the code patterns you can use.

::: moniker range="<=net-maui-10.0"

- **Available on**: .NET MAUI iOS and Mac Catalyst apps when explicitly
  enabled for publishing
- **Advantages**: Small app size, fast startup, and a single native binary
- **Disadvantages**: Longer build times, no dynamic code generation or dynamic
  loading, and all code must be trim-safe and AOT-compatible
- **MSBuild property**: `<PublishAot>true</PublishAot>`

::: moniker-end

::: moniker range=">=net-maui-11.0"

- **Available on**: .NET MAUI iOS and Mac Catalyst apps, and experimentally
  on Android, when explicitly enabled for publishing
- **Advantages**: Small app size, fast startup, and a single native binary
- **Disadvantages**: Longer build times, no dynamic code generation or dynamic
  loading, and all code must be trim-safe and AOT-compatible
- **MSBuild property**: `<PublishAot>true</PublishAot>`

NativeAOT is a publish-only deployment model. Use `dotnet publish` to produce
and validate a NativeAOT app. A regular debug build doesn't use NativeAOT.

::: moniker-end

For more information, see [Native AOT deployment on iOS and Mac
Catalyst](nativeaot.md) and [Native AOT deployment](/dotnet/core/deploying/native-aot).

### ReadyToRun (R2R)

ReadyToRun is a form of ahead-of-time compilation for CoreCLR that precompiles
assemblies into a format containing both the original MSIL and a native code
representation. At startup, the runtime can use the precompiled native code
instead of compiling methods from MSIL. On platforms where JIT is available,
the runtime can still JIT-compile methods that aren't precompiled and optimize
frequently used methods at runtime.

::: moniker range="<=net-maui-10.0"

- **Used by**: CoreCLR on Windows and CoreCLR Android builds that opt into the
  experimental runtime
- **Advantages**: Improved startup time while retaining JIT compatibility
- **Disadvantages**: Larger assembly sizes because assemblies contain both MSIL
  and native code
- **MSBuild property**: `<PublishReadyToRun>true</PublishReadyToRun>`

ReadyToRun is enabled by default for .NET MAUI apps on Windows in `Release`
mode and for Android apps that use CoreCLR in `Release` mode. The R2R images
are packed inside the `.dll` files.

::: moniker-end

::: moniker range=">=net-maui-11.0"

- **Used by**: CoreCLR on Windows, Android, iOS, and Mac Catalyst
- **Advantages**: Improved startup time while retaining the appropriate
  runtime compilation behavior
- **Disadvantages**: Larger application and download sizes

For Android apps, ReadyToRun is enabled by default in the `Release`
configuration. The default is composite partial ReadyToRun. Full ReadyToRun
can be enabled with `MauiEnableFullReadyToRun=true`. It can improve startup or
runtime performance, but it increases package size, so measure the result for
your app.

For iOS and Mac Catalyst apps, composite partial ReadyToRun is used in `Debug`
builds and composite full ReadyToRun is used in `Release` builds. The CoreCLR
interpreter is always enabled and executes code that isn't precompiled because
these platforms don't permit JIT compilation.

::: moniker-end

*Composite* R2R compiles assemblies together, enabling cross-assembly
optimizations. *Partial* R2R precompiles selected methods while leaving the
remaining methods for runtime compilation, while *full* R2R precompiles all
methods.

For more information, see [ReadyToRun compilation](/dotnet/core/deploying/ready-to-run).

::: moniker range="<=net-maui-10.0"

### Profile-guided optimization (PGO)

Profile-guided optimization (PGO) uses profiling data to guide the compiler
toward producing more efficient code. In the context of .NET MAUI, there are
two forms of PGO:

- **Static PGO with MIBC profiles**: .NET MAUI ships `.mibc` profile files
  containing data about methods frequently called during typical app startup
  and usage. When ReadyToRun compilation is enabled, the R2R compiler uses
  these profiles to decide which methods to precompile.
- **Dynamic PGO**: On CoreCLR, the JIT compiler can collect profile data at
  runtime and use it when methods are recompiled with more optimizations.

For more information, see [Profile-guided optimization](/dotnet/core/runtime-config/compilation#profile-guided-optimization)
and [ReadyToRun compilation](/dotnet/core/deploying/ready-to-run).

::: moniker-end

::: moniker range="<=net-maui-10.0"

### Mono interpreter

The Mono interpreter enables an app to interpret MSIL at runtime without
generating native code dynamically. It is used alongside Mono AOT on platforms
that don't allow JIT compilation, such as iOS devices.

The Mono interpreter also enables .NET Hot Reload for apps running on the Mono
runtime. `UseInterpreter` is set to `true` by default in `Debug` mode on
Android, iOS, and Mac Catalyst.

- **Used by**: Mono runtime
- **Advantages**: Supports dynamic features that AOT can't handle, enables
  .NET Hot Reload, and can reduce app size
- **Disadvantages**: Interpreted code runs slower than compiled code
- **MSBuild property**: `<UseInterpreter>true</UseInterpreter>`

For more information, see [Mono interpreter on iOS and Mac Catalyst](~/macios/interpreter.md).

::: moniker-end

::: moniker range=">=net-maui-11.0"

### CoreCLR interpreter

CoreCLR includes an interpreter on iOS and Mac Catalyst. The interpreter is
always enabled and executes code that isn't precompiled because these platforms
don't permit JIT compilation. This behavior is part of the runtime and doesn't
require an interpreter MSBuild property.

::: moniker-end

## What .NET MAUI uses by default

::: moniker range="<=net-maui-10.0"

The following table summarizes which runtime and compilation strategy .NET MAUI
uses on each platform, by build configuration:

| Platform | Debug | Release |
|---|---|---|
| **Android** | Mono + JIT + interpreter | Mono + Mono AOT |
| **iOS** | Mono + JIT (x64) / Mono + AOT + interpreter (ARM64) | Mono + Mono AOT |
| **Mac Catalyst** | Mono + JIT (x64) / Mono + AOT + interpreter (ARM64) | Mono + Mono AOT |
| **Windows** | CoreCLR + JIT | CoreCLR + JIT + ReadyToRun |

> [!NOTE]
> When you opt in to CoreCLR on Android or iOS by setting
> `<UseMonoRuntime>false</UseMonoRuntime>` in your project file, builds use
> ReadyToRun by default. On iOS and Mac Catalyst with CoreCLR, composite
> ReadyToRun is enabled for both debug and release builds.

> [!TIP]
> You can opt in to NativeAOT on iOS and Mac Catalyst by setting
> `<PublishAot>true</PublishAot>` in your project file. NativeAOT only takes
> effect when publishing.

::: moniker-end

::: moniker range=">=net-maui-11.0"

The following table summarizes the runtime and compilation strategy used by
.NET MAUI on each platform:

| Platform | Debug | Release |
|---|---|---|
| **Android** | CoreCLR + JIT | CoreCLR + composite partial ReadyToRun + JIT |
| **iOS** | CoreCLR + composite partial ReadyToRun + interpreter | CoreCLR + composite full ReadyToRun + interpreter |
| **Mac Catalyst** | CoreCLR + composite partial ReadyToRun + interpreter | CoreCLR + composite full ReadyToRun + interpreter |
| **Windows** | CoreCLR + JIT | CoreCLR + JIT + ReadyToRun |

> [!IMPORTANT]
> Don't set `UseMonoRuntime` to `true` when targeting .NET 11+. Mono isn't
> supported, and setting this property produces a build error.

> [!TIP]
> NativeAOT is enabled with `<PublishAot>true</PublishAot>` and takes effect
> when you use `dotnet publish`. NativeAOT has no JIT or interpreter and
> requires full trimming.

::: moniker-end

## Comparison

::: moniker range="<=net-maui-10.0"

The following table compares the compilation strategies available for .NET
MAUI apps:

| Strategy | JIT | Mono AOT | ReadyToRun | NativeAOT |
|---|---|---|---|---|
| **Compilation time** | At runtime | At build time | At build time | At build time |
| **Startup speed** | Slowest | Fast | Fast | Fastest |
| **Steady-state speed** | Fastest (optimized JIT) | Good | Fastest (tiered recompilation) | Good |
| **App size** | Smallest | Larger | Larger | Smallest |
| **Dynamic code** | Full support | Limited | Full support | Not supported |
| **Trimming required** | No | Partial (default) | No | Full |
| **Diagnostics** | Full | Limited | Full | Limited |

### Typical performance

The following benchmarks are from a `dotnet new maui` app and are
hardware-dependent. They illustrate relative differences between runtimes and
aren't absolute guarantees.

**Startup time (milliseconds):**

| Runtime | Android | iOS | macOS |
|---|---|---|---|
| Mono (default) | ~642 | ~275 | ~311 |
| NativeAOT | ~274 | ~130 | ~255 |

**App size on iOS (MB):**

| Deployment model | Size |
|---|---|
| Mono (default) | ~14.3 |
| Mono (full trimming) | ~11.5 |
| NativeAOT | ~5.4 |

::: moniker-end

::: moniker range=">=net-maui-11.0"

The following table compares the compilation strategies available for .NET
MAUI apps targeting .NET 11:

| Strategy | CoreCLR JIT | CoreCLR interpreter | ReadyToRun | NativeAOT |
|---|---|---|---|---|
| **Compilation time** | At runtime | At runtime | At build time | At build time |
| **Startup speed** | Slower | Slower | Fast | Fastest |
| **App size** | Smaller | Smaller | Larger | Smallest |
| **Dynamic code** | Full support | Full support | Full support (through either JIT or interpreter) | Not supported |
| **Diagnostics** | Full | Full | Limited | Limited |

::: moniker-end

## CoreCLR on Android and iOS

::: moniker range="<=net-maui-10.0"

Starting in .NET 10, you can opt in to running an Android app on CoreCLR
instead of Mono.

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">
    <UseMonoRuntime>false</UseMonoRuntime>
</PropertyGroup>
```

In .NET 10, CoreCLR on Android is an experimental feature and isn't intended
for production use.

::: moniker-end

::: moniker range=">=net-maui-11.0"

In .NET 11+, CoreCLR is the supported runtime for Android, iOS, and Mac
Catalyst apps unless NativeAOT is explicitly enabled for publishing. NativeAOT
is an opt-in alternative that doesn't use CoreCLR and remains experimental on
Android. Don't set
`UseMonoRuntime=true`; Mono isn't supported for .NET 11+, and setting the
property produces a build error.

::: moniker-end

### Inside an Android APK

::: moniker range="<=net-maui-10.0"

The contents of an APK differ depending on which runtime and compilation
strategy the app uses:

**Mono:**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libmonosgen-2.0.so` — Mono runtime
- `lib/arm64-v8a/libmonodroid.so` — Android-specific runtime startup glue
- `lib/arm64-v8a/libassemblies.arm64-v8a.blob.so` — Compressed, packed MSIL
  assemblies
- `lib/arm64-v8a/libaot-*.dll.so` — Mono AOT native images in release builds

**CoreCLR:**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libcoreclr.so` — CoreCLR runtime
- `lib/arm64-v8a/libclrjit.so` — CoreCLR JIT compiler
- `lib/arm64-v8a/libmonodroid.so` — Android-specific runtime startup glue
- `lib/arm64-v8a/libassemblies.arm64-v8a.so` — Packed MSIL with ReadyToRun
  images

::: moniker-end

::: moniker range=">=net-maui-11.0"

The contents of an Android APK differ depending on whether the app uses
CoreCLR or NativeAOT:

**CoreCLR:**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libcoreclr.so` — CoreCLR runtime
- `lib/arm64-v8a/libclrjit.so` — CoreCLR JIT compiler
- `lib/arm64-v8a/libmonodroid.so` — Android-specific runtime startup glue
- `lib/arm64-v8a/libassemblies.arm64-v8a.so` — Packed MSIL with ReadyToRun
  images

**NativeAOT on Android (experimental):**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libhellomaui.so` — Native library containing the runtime,
  managed code, and startup glue

::: moniker-end

## Trimming

Trimming is a build step that removes unused code from your app to reduce its
size. .NET MAUI uses the ILLink trimmer, which analyzes your code and removes
types, methods, and fields that aren't statically referenced.

For CoreCLR Release builds, .NET MAUI uses
`TrimMode=partial` by default, which trims framework assemblies but not your
code or NuGet references. To use full trimming, set `TrimMode` to `full`:

```xml
<PropertyGroup>
    <TrimMode>full</TrimMode>
</PropertyGroup>
```

NativeAOT automatically performs full trimming. Don't set `TrimMode` to select
a different trimming mode when using NativeAOT.

For more information, see [Trim a .NET MAUI app](trimming.md).

## MSBuild property reference

::: moniker range="<=net-maui-10.0"

The following table summarizes the MSBuild properties that control runtime and
compilation behavior:

| Property | Description | Default |
|---|---|---|
| `PublishAot` | Enable NativeAOT compilation. | `false` |
| `PublishReadyToRun` | Enable ReadyToRun precompilation for CoreCLR. | `true` (Windows Release and applicable CoreCLR builds) |
| `PublishTrimmed` | Enable ILLink trimming. | `true` (Release) |
| `TrimMode` | Set trimming aggressiveness (`partial` or `full`). | `partial` |
| `UseInterpreter` | Enable the Mono interpreter. | `true` (iOS/Mac Catalyst Debug) |
| `UseMonoRuntime` | Use Mono instead of CoreCLR. | `true` on supported Mono targets |

::: moniker-end

::: moniker range=">=net-maui-11.0"

The following table summarizes the MSBuild properties relevant to .NET 11
runtime and compilation behavior:

| Property | Description | Default |
|---|---|---|
| `PublishAot` | Enable NativeAOT compilation during `dotnet publish`. | `false` |
| `MauiEnableFullReadyToRun` | Enable full ReadyToRun for Android CoreCLR apps. | `false` |
| `PublishTrimmed` | Enable ILLink trimming. | `true` (Release) |
| `TrimMode` | Set trimming aggressiveness for non-NativeAOT builds. | `partial` |

::: moniker-end

## See also

- [Native AOT deployment on iOS and Mac Catalyst](nativeaot.md)
- [Mono interpreter on iOS and Mac Catalyst](~/macios/interpreter.md)
- [Trim a .NET MAUI app](trimming.md)
- [Improve app performance](performance.md)
- [Native AOT deployment overview](/dotnet/core/deploying/native-aot)
- [ReadyToRun compilation](/dotnet/core/deploying/ready-to-run)
- [.NET glossary](/dotnet/standard/glossary)
