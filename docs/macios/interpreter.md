---
title: "Mono interpreter on iOS and Mac Catalyst"
description: Learn how to enable the Mono interpreter, which lets you use dynamic code generation in your .NET MAUI iOS and ARM64-based Mac Catalyst release builds.
ms.date: 10/23/2024
---

# Mono interpreter on iOS and Mac Catalyst

When you compile a .NET Multi-Platform App UI (.NET MAUI) app for iOS or Mac Catalyst, the compiler turns your app code into Microsoft Intermediate Language (MSIL). When you run the iOS app in the simulator, or the Mac Catalyst app, the .NET Common Language Runtime (CLR) compiles the MSIL using a Just in Time (JIT) compiler. At runtime the MSIL is compiled into native code, which can run on the correct architecture for your app.

However, there is a security restriction on iOS, set by Apple, which disallows the execution of dynamically generated code on a device. Similarly, the execution of dynamically generated code is disallowed in iOS apps running on the ARM64 architecture in the simulator, and on Mac Catalyst apps running on the ARM64 architecture. To meet this restriction, iOS and Mac Catalyst apps use an Ahead of Time (AOT) compiler to compile the managed code. This produces a native iOS binary that can be deployed to Apple devices, or a native Mac Catalyst binary.

AOT provides benefits through a reduction in startup time, and various other performance optimizations. However, it also restricts certain features from being used in your app:

- There's limited generics support. Not every possible generic instantiation can be determined at compile time. Many of the iOS-specific issues encountered in .NET MAUI release builds are due to this limitation.
- Dynamic code generation isn't allowed. This means that `System.Relection.Emit` is unavailable, there's no support for `System.Runtime.Remoting`, and some uses of the C# [dynamic](/dotnet/csharp/advanced-topics/interop/using-type-dynamic) type aren't permitted.

When an AOT restriction occurs, a `System.ExecutionEngineException` will be thrown with a message of "Attempting to JIT compile method while running in aot-only mode".

The Mono interpreter overcomes these restrictions while abiding by platform restrictions. It enables you to interpret some parts of your app at runtime, while AOT compiling the rest. However, there are some potential drawbacks to using the interpreter in a production app:

- While the app size usually shrinks significantly when the interpreter is enabled, in certain cases the app size can increase.
- App execution speed will be slower because interpreted code runs more slowly than AOT compiled code. This execution speed reduction can range from unmeasurable to unacceptable, so performance testing should be performed.
- Native stack traces in crash reports become less useful, because they'll contain generic frames from the interpreter that don't mention the code that's executing. However, managed stack traces won't change.

The interpreter is enabled by default for .NET MAUI debug builds, and can be enabled for release builds.

> [!TIP]
> If your .NET MAUI iOS app or ARM64-based Mac Catalyst app works correctly as a debug build but then crashes as a release build, try enabling the interpreter for your app's release build. It may be that your app, or one of its libraries, uses a feature that requires the interpreter.

## Enable the interpreter

The Mono interpreter can be enabled in iOS release builds by setting the `$(UseInterpreter)` MSBuild property to `true` in your .NET MAUI app's project file:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <UseInterpreter>true</UseInterpreter>
</PropertyGroup>
```

The interpreter can also be enabled for Mac Catalyst release builds on ARM64:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)' == 'maccatalyst-arm64' and '$(Configuration)' == 'Release'">
    <UseInterpreter>true</UseInterpreter>
</PropertyGroup>
```

> [!WARNING]
> Don't enable the interpreter for release builds on Android because it disables JIT compilation.

On iOS and Mac Catalyst, the interpreter can also be enabled with the `$(MtouchInterpreter)` MSBuild property. This property optionally takes a comma-separated list of assemblies to be interpreted. In addition, `all` can be used to specify all assemblies, and when prefixed with a minus sign, an assembly will be AOT compiled. This enables you to:

- Interpret all assemblies by specifying `all` or AOT compile everything by specifying `-all`.
- Interpret individual assemblies by specifying **MyAssembly** or AOT compile individual assemblies by specifying **-MyAssembly**.
- Mix and match to interpret some assemblies and AOT compile other assemblies.

> [!WARNING]
> The interpreter isn't compatible with Native AOT deployment, and therefore the `$(UseInterpreter)` and `$(MtouchInterpreter)` MSBuild properties have no effect when using Native AOT.

The following example shows how to interpret all assemblies except **System.Xml.dll**:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <!-- Interpret everything, except System.Xml.dll -->
    <MtouchInterpreter>all,-System.Xml</MtouchInterpreter>
</PropertyGroup>
```

The following example shows how to AOT compile all assemblies except **System.Numerics.dll**:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <!-- AOT everything, except System.Numerics.dll, which will be interpreted -->
    <MtouchInterpreter>-all,System.Numerics</MtouchInterpreter>
</PropertyGroup>
```

> [!IMPORTANT]
> A stack frame executed by the interpreter won't provide useful information. However, because the interpreter can be disabled on a per-assembly basis, it's possible to have stack frames from some assemblies accurately depicted in crash reports.

Alternatively, use the following example to AOT compile all assemblies, while still allowing the interpreter to perform dynamic code generation:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <MtouchInterpreter>-all</MtouchInterpreter>
</PropertyGroup>
```

Another common scenario where the interpreter is sometimes required is a .NET MAUI Mac Catalyst app running on the ARM64 architecture, which can throw an exception on launch. This launch exception can often be fixed by enabling the interpreter:

```xml
<PropertyGroup Condition="'$(RuntimeIdentifier)' == 'maccatalyst-arm64' and '$(Configuration)' == 'Release'">
    <MtouchInterpreter>-all,MyAssembly</MtouchInterpreter>
</PropertyGroup>
```
