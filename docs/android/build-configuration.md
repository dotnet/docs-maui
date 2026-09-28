---
title: "Android build settings and diagnostics in .NET MAUI"
description: "Learn which .NET for Android build settings apply to Mono, CoreCLR, and NativeAOT in a .NET MAUI app."
ms.date: 09/28/2026
---

# Android build settings and diagnostics in .NET MAUI

.NET MAUI Android apps use the .NET for Android build system. The runtime
selected for an app determines which Android build settings apply. For the
complete list of settings, see the .NET for Android [build properties](/dotnet/android/building-apps/build-properties),
[build items](/dotnet/android/building-apps/build-items), and
[build targets](/dotnet/android/building-apps/build-targets) references.
For an overview of the runtimes and compilation strategies on all .NET MAUI
platforms, see [Runtimes and compilation](~/deployment/runtimes-compilation.md).

::: moniker range="<=net-maui-10.0"

## Mono AOT settings through .NET 10

Mono is the default runtime for Android apps targeting .NET 10 or earlier.
In supported Mono Release builds, `RunAOTCompilation` defaults to `true`,
while it defaults to `false` for Debug builds. When Mono AOT is enabled
and `AndroidEnableProfiledAot` isn't specified, profiled AOT defaults to
`true`. To use a custom Mono AOT profile, add an `AndroidAotProfile` item
containing a `.aprof` file. Set `AndroidUseDefaultAotProfile` to `false`
only if you need to exclude the default profile. These are Mono profiles,
not CoreCLR MIBC profiles or dynamic PGO inputs.

Other Mono-specific Android settings include:

| Setting | Mono behavior |
| --- | --- |
| `AndroidAotAdditionalArguments` | Comma-separated options inside the Mono AOT compiler's `--aot=...` argument. |
| `AndroidExtraAotOptions` | Semicolon-separated standalone arguments passed to the Mono AOT compiler process. |
| `EnableLLVM` | Uses LLVM when Mono AOT is enabled; requires the Android NDK. |
| `AndroidAotEnableLazyLoad` | Delays loading Mono AOT assemblies; defaults to `true` when Mono AOT is enabled and debug symbols aren't included. |
| `AndroidUseInterpreter` | Enables the Mono interpreter; defaults to `true` for Debug builds that use Mono and `false` otherwise. |
| `AndroidEnableSGenConcurrent` | Enables Mono's concurrent garbage collector; defaults to `true`. |

The `BuildAndStartAotProfiling` and `FinishAotProfiling` targets collect
Mono AOT profiling data through `aprofutil`. The `AndroidAotProfilerPort`
defaults to `9999`, and `AndroidAotCustomProfilePath` defaults to
`custom.aprof`. These targets and settings don't configure CoreCLR or
NativeAOT profiling.

The deprecated `AotAssemblies` property warns with
[XA1029](/dotnet/android/messages/xa1029) whenever it's specified, even
when set to `false`. Remove it and, if you need to configure Mono AOT,
use `RunAOTCompilation` instead. `BundleAssemblies=true` warns with
[XA1035](/dotnet/android/messages/xa1035) and no longer affects the
build. For the former packaging behavior in supported Mono projects, use
`AndroidUseAssemblyStore` with `AndroidEnableAssemblyCompression` if needed.

::: moniker-end

::: moniker range=">=net-maui-11.0"

## Move an Android app to .NET 11

CoreCLR is the default runtime for Android apps targeting .NET 11 or later.
NativeAOT is an experimental, opt-in publishing alternative on Android.
Mono isn't supported on these targets: `UseMonoRuntime=true` produces
[NETSDK1242](/dotnet/core/tools/sdk-errors/netsdk1242). ReadyToRun for
CoreCLR and NativeAOT are separate compilation models, not replacements
for Mono AOT properties.

When updating an Android app project, remove these legacy settings rather
than copying them into a .NET 11 property group:

| Setting | What to do |
| --- | --- |
| `AotAssemblies` | Remove it, even if set to `false`; it emits [XA1029](/dotnet/android/messages/xa1029). If set to `true` while `RunAOTCompilation` is unset or `true`, it also forwards to that property and stops the build with [XA1044](/dotnet/android/messages/xa1044). |
| `RunAOTCompilation`, `EnableLLVM` | Remove or restrict to supported .NET 10-and-earlier Mono targets. Setting either to `true` for CoreCLR or NativeAOT resets it to `false` and stops the build with [XA1044](/dotnet/android/messages/xa1044); it isn't silently ignored. |
| `BundleAssemblies` | Remove it. A value of `true` emits [XA1035](/dotnet/android/messages/xa1035), but doesn't affect the build. CoreCLR controls packaged assembly-store behavior; don't add replacement assembly-store or compression settings. |

Other Mono AOT options, including `AndroidEnableProfiledAot`,
`AndroidUseDefaultAotProfile`, `AndroidAotProfile` items,
`AndroidAotAdditionalArguments`, `AndroidExtraAotOptions`,
`AndroidAotEnableLazyLoad`, `AndroidUseInterpreter`,
`AndroidEnableSGenConcurrent`, and the AOT profiling targets, don't
configure CoreCLR or NativeAOT. Remove them unless a project also
targets a supported .NET 10-or-earlier Mono runtime and scopes them to
that target. The Mono `.aprof` profile isn't a CoreCLR MIBC profile or
a dynamic PGO input.

For CoreCLR Release builds, .NET MAUI enables composite partial
ReadyToRun by default. If your app benefits from full ReadyToRun,
you can set `MauiEnableFullReadyToRun=true`; measure the increase in
package size against the startup benefit. Don't set `RunAOTCompilation`
to configure ReadyToRun. For more information, see
[Runtimes and compilation](~/deployment/runtimes-compilation.md#readytorun-r2r).

::: moniker-end

## Android build diagnostics

The following .NET for Android diagnostics can also appear while building
a .NET MAUI Android app:

| Diagnostic | Resolution |
| --- | --- |
| [XA0119](/dotnet/android/messages/xa0119), [XA1030](/dotnet/android/messages/xa1030) | Don't enable Mono AOT in Debug or without trimming on supported Mono targets. Remove `RunAOTCompilation` from CoreCLR and NativeAOT projects rather than trying to use Mono AOT there. |
| [XA1044](/dotnet/android/messages/xa1044) | Remove `RunAOTCompilation` or `EnableLLVM` from non-Mono builds; the build stops, even though it resets the unsupported property to `false`. |
| [XA1049](/dotnet/android/messages/xa1049) | Don't set `AndroidEnableMarshalMethods` and `PublishReadyToRun` to `true` together. |
| [XA4256](/dotnet/android/messages/xa4256) | An `AndroidLibrary` item specifies multiple artifacts in its `JavaArtifact` metadata. Specify exactly one `group-id:artifact-id:version` artifact per item; use separate items for additional artifacts. |
| [XA4257](/dotnet/android/messages/xa4257) | A Java peer type refers to an unresolved type in its managed base type, implemented interface, or generic parameter constraint. If the app uses this type, align the versions of the affected Android binding packages. Otherwise the unused peer is omitted from the trimmable type map and requires no action. |

For other messages, see the [.NET for Android message reference](/dotnet/android/messages).
