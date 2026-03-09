---
title: "Runtimes and compilation in .NET MAUI"
description: "Learn about the runtimes and compilation strategies used by .NET MAUI apps, including Mono, CoreCLR, NativeAOT, ReadyToRun, and the Mono interpreter."
ms.date: 03/09/2026
---

# Runtimes and compilation in .NET MAUI

.NET Multi-platform App UI (.NET MAUI) apps can run on different .NET runtimes and use different compilation strategies depending on the target platform, build configuration, and deployment model. This article explains the key terms, how they relate to each other, and what .NET MAUI uses by default on each platform.

## Runtimes

A .NET *runtime* is the execution environment that manages your app's memory, type system, garbage collection, and code execution. .NET MAUI apps can run on one of the following runtimes:

### Mono

[Mono](https://www.mono-project.com/docs/) is the cross-platform .NET runtime that has historically powered Xamarin apps and now powers .NET MAUI apps on Android, iOS, and Mac Catalyst. Mono is designed for a small footprint and supports both just-in-time (JIT) compilation and ahead-of-time (AOT) compilation. Mono is the default runtime for .NET MAUI apps on mobile and Mac Catalyst platforms.

### CoreCLR

CoreCLR is the .NET Common Language Runtime used by .NET on desktop and server platforms. It's the runtime that powers ASP.NET Core, console apps, and Windows desktop apps. CoreCLR features a highly optimizing JIT compiler, tiered compilation, and a full set of runtime diagnostics. In .NET MAUI, CoreCLR is the runtime used on Windows.

> [!NOTE]
> In .NET 10, CoreCLR is available as an experimental option for Android. In .NET 11, CoreCLR becomes the default runtime for Android `Release` builds and is available as an experimental option for iOS. For more information, see [CoreCLR on Android and iOS](#coreclr-on-android-and-ios).

### NativeAOT runtime

When you publish with [Native AOT](nativeaot.md), your app doesn't run on Mono or CoreCLR. Instead, it runs on a minimal NativeAOT runtime that's statically linked into your native binary. This runtime includes a garbage collector and type system but no JIT compiler — all code is compiled to native machine code at build time.

## Compilation strategies

A *compilation strategy* determines how your C# code is turned into machine code that the processor can execute. .NET MAUI uses several compilation strategies, depending on the runtime and deployment scenario.

### JIT (Just-in-Time) compilation

JIT compilation translates Microsoft Intermediate Language (MSIL) into native machine code *at runtime*, as methods are called for the first time. JIT compilation is used during development because it supports fast build-deploy-debug cycles and features like Edit and Continue.

- **Used by**: CoreCLR (Windows, Android experimental), Mono (debug builds on Android)
- **Advantages**: Fast build times, full runtime diagnostics, dynamic code generation support
- **Disadvantages**: Slower startup because code is compiled on the device at runtime

> [!NOTE]
> iOS and Mac Catalyst device builds can't use JIT compilation due to Apple's security restrictions on dynamically generated code. However, JIT is used in the iOS simulator and on x64 Mac Catalyst during development.

### Mono AOT (Ahead-of-Time) compilation

Mono AOT compilation pre-compiles your MSIL into native code at build time using Mono's AOT compiler. This is the default compilation mode for .NET MAUI release builds on iOS, Mac Catalyst, and Android. Mono AOT is a technology carried forward from Xamarin.

- **Used by**: Mono runtime (iOS, Mac Catalyst, Android release builds)
- **Advantages**: Faster startup than JIT, required for platforms that restrict dynamic code generation
- **Disadvantages**: Larger app size than JIT-only, limited support for some dynamic features

Mono AOT is not the same as NativeAOT. With Mono AOT, your app still includes the Mono runtime and can optionally fall back to the Mono interpreter for code that wasn't AOT-compiled. NativeAOT compiles *all* code ahead of time and doesn't include a JIT or interpreter.

### NativeAOT (Native Ahead-of-Time) compilation

NativeAOT compiles your entire app, all its dependencies, and a minimal runtime into a single native binary at build time. There's no JIT compiler or interpreter at runtime. NativeAOT performs full trimming and static analysis of your code, which produces the smallest app sizes and fastest startup times, but places the most restrictions on what code patterns you can use.

- **Used by**: NativeAOT runtime (iOS and Mac Catalyst stable in .NET 9+, Android experimental)
- **Advantages**: Smallest app size, fastest startup, single native binary
- **Disadvantages**: No dynamic code generation (`System.Reflection.Emit`), no dynamic loading, requires all code to be trim-safe and AOT-compatible
- **MSBuild property**: `<PublishAot>true</PublishAot>`

For more information, see [Native AOT deployment on iOS and Mac Catalyst](nativeaot.md) and [Native AOT deployment](/dotnet/core/deploying/native-aot).

### ReadyToRun (R2R)

ReadyToRun is a form of AOT compilation for CoreCLR that pre-compiles your assemblies into a format that includes *both* the original MSIL and a native code representation. At startup, the runtime uses the pre-compiled native code instead of JIT-compiling from MSIL, which improves startup time. For methods that are called frequently, tiered compilation eventually replaces the ReadyToRun code with fully optimized JIT-compiled code.

- **Used by**: CoreCLR (Windows, Android, and iOS release builds when using CoreCLR)
- **Advantages**: Improved startup time while retaining full JIT compatibility
- **Disadvantages**: Larger assembly sizes (assemblies contain both MSIL and native code)
- **MSBuild property**: `<PublishReadyToRun>true</PublishReadyToRun>`

ReadyToRun is enabled by default for .NET MAUI apps on Windows in `Release` mode, and for Android apps that use CoreCLR in `Release` mode. The R2R images are packed inside your `.dll` files, which is why you'll notice assemblies increase in size.

For more information, see [ReadyToRun compilation](/dotnet/core/deploying/ready-to-run).

### Mono interpreter

The Mono interpreter enables your app to interpret MSIL at runtime without generating native code dynamically. This lets your app use dynamic features such as generics instantiations and limited reflection on platforms that don't allow JIT compilation, such as iOS devices. The interpreter is used alongside Mono AOT — most code is AOT-compiled, and the interpreter handles the portions that couldn't be compiled ahead of time.

- **Used by**: Mono runtime (enabled by default in debug builds on iOS and Mac Catalyst)
- **Advantages**: Supports dynamic features that AOT can't handle, can reduce app size
- **Disadvantages**: Interpreted code runs slower than compiled code
- **MSBuild property**: `<UseInterpreter>true</UseInterpreter>`

For more information, see [Mono interpreter on iOS and Mac Catalyst](~/macios/interpreter.md).

## What .NET MAUI uses by default

The following table summarizes which runtime and compilation strategy .NET MAUI uses on each platform, by build configuration:

| Platform | Debug | Release |
|---|---|---|
| **Android** | Mono + JIT | Mono + Mono AOT |
| **iOS** | Mono + JIT (simulator) / Mono + AOT + interpreter (device) | Mono + Mono AOT |
| **Mac Catalyst** | Mono + JIT (x64) / Mono + AOT + interpreter (ARM64) | Mono + Mono AOT |
| **Windows** | CoreCLR + JIT | CoreCLR + JIT + ReadyToRun |

> [!NOTE]
> When you opt in to CoreCLR on Android or iOS by setting `<UseMonoRuntime>false</UseMonoRuntime>` in your project file, `Release` builds also use ReadyToRun by default.

> [!TIP]
> You can opt in to NativeAOT on iOS and Mac Catalyst by setting `<PublishAot>true</PublishAot>` in your project file. For more information, see [Native AOT deployment on iOS and Mac Catalyst](nativeaot.md).

## Comparison

The following table compares the compilation strategies available for .NET MAUI apps:

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

The following benchmarks are from a `dotnet new maui` app and are hardware-dependent. They illustrate the relative differences between runtimes and are not absolute guarantees.

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

## CoreCLR on Android and iOS

Starting in .NET 10, you can opt in to running your Android app on the CoreCLR runtime instead of Mono. In .NET 11, CoreCLR is also available as an experimental option for iOS.

```xml
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">
    <UseMonoRuntime>false</UseMonoRuntime>
</PropertyGroup>
```

CoreCLR brings improved compatibility with the rest of .NET and shorter startup times. In .NET 10, CoreCLR on Android is an experimental feature and isn't intended for production use. In .NET 11, CoreCLR becomes the default runtime for Android `Release` builds, and is available as an experimental option for iOS.

To opt out of CoreCLR and continue using Mono in .NET 11 and later:

```xml
<PropertyGroup>
    <UseMonoRuntime>true</UseMonoRuntime>
</PropertyGroup>
```

### Inside an Android APK

The contents of your APK differ depending on which runtime and compilation strategy your app uses:

**Mono (default):**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libmonosgen-2.0.so` — Mono runtime
- `lib/arm64-v8a/libmonodroid.so` — Android-specific glue code for starting the runtime
- `lib/arm64-v8a/libassemblies.arm64-v8a.blob.so` — Compressed, packed MSIL assemblies
- `lib/arm64-v8a/libaot-*.dll.so` — Mono AOT native images (release builds)

**CoreCLR (experimental):**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libcoreclr.so` — CoreCLR runtime
- `lib/arm64-v8a/libclrjit.so` — CoreCLR JIT compiler
- `lib/arm64-v8a/libmonodroid.so` — Android-specific glue code for starting the runtime
- `lib/arm64-v8a/libassemblies.arm64-v8a.so` — Compressed, packed MSIL with ReadyToRun images

**NativeAOT (experimental):**

- `classes.dex` — Java/Kotlin code
- `lib/arm64-v8a/libhellomaui.so` — Single native library containing the runtime, all managed code, and glue code

## Trimming

Trimming is a build step that removes unused code from your app to reduce its size. Trimming is relevant to all compilation strategies but is *required* for NativeAOT. .NET MAUI uses the ILLink trimmer, which analyzes your code and removes types, methods, and fields that aren't statically referenced.

.NET MAUI uses `TrimMode=partial` by default, which trims framework assemblies but not your code or NuGet references. You can opt in to full trimming:

```xml
<PropertyGroup>
    <TrimMode>full</TrimMode>
</PropertyGroup>
```

For more information, see [Trim a .NET MAUI app](trimming.md).

## MSBuild property reference

The following table summarizes the MSBuild properties that control runtime and compilation behavior:

| Property | Description | Default |
|---|---|---|
| `PublishAot` | Enable NativeAOT compilation. | `false` |
| `PublishReadyToRun` | Enable ReadyToRun pre-compilation for CoreCLR. | `true` (Windows Release) |
| `PublishTrimmed` | Enable ILLink trimming. | `true` (Release) |
| `TrimMode` | Set trimming aggressiveness (`partial` or `full`). | `partial` |
| `UseInterpreter` | Enable the Mono interpreter. | `true` (iOS/Mac Catalyst Debug) |
| `UseMonoRuntime` | Use Mono instead of CoreCLR on Android. | `true` (.NET 10), `false` (.NET 11) |

## See also

- [Native AOT deployment on iOS and Mac Catalyst](nativeaot.md)
- [Mono interpreter on iOS and Mac Catalyst](~/macios/interpreter.md)
- [Trim a .NET MAUI app](trimming.md)
- [Improve app performance](performance.md)
- [Native AOT deployment overview](/dotnet/core/deploying/native-aot)
- [ReadyToRun compilation](/dotnet/core/deploying/ready-to-run)
- [.NET glossary](/dotnet/standard/glossary)
