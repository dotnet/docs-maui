---
title: "Mono interpreter on iOS and Mac Catalyst"
description: Learn how to enable the Mono interpreter, which enables you to use dynamic code generation in your .NET MAUI iOS and Mac Catalyst release builds.
ms.date: 05/24/2023
---

# Mono interpreter on iOS and Mac Catalyst

When you compile a .NET app for iOS or Mac Catalyst, the compiler turns your app code into Microsoft Intermediate Language (MSIL). When you run a Mac Catalyst app, or an iOS app in the simulator, the .NET Common Language Runtime (CLR) compiles the MSIL using a Just in Time (JIT) compiler. At runtime the MSIL is compiled into native code, which can run on the correct architecture for your app.

However, there is a security restriction on iOS, set by Apple, which disallows the execution of dynamically generated code on a device. To meet this restriction, iOS apps use an Ahead of Time (AOT) compiler to compile the managed code. This produces a native iOS binary that can be deployed to Apple devices.

AOT provides benefits through a reduction in startup time, and various other performance optimizations. However, it also restricts certain features from being used in your app:

- There's limited generics support. Not every possible generic instantiation can be determined at compile time. Many of the iOS-specific issues encountered in .NET MAUI release builds are due to this.
- Dynamic code generation isn't allowed, because the iOS kernel prevents apps from generating code dynamically. This means that:
  - `System.Relection.Emit` is unavailable.
  - There's no support for `System.Runtime.Remoting`.
  - Some uses of the C# [dynamic](/dotnet/csharp/advanced-topics/interop/using-type-dynamic) type aren't permitted.

The Mono interpreter overcomes these restrictions while abiding by platform restrictions. It enables you to interpret at runtime some parts of your app while compiling the rest AOT as usual. However, there are some drawbacks:

- While the app size usually shrinks significantly when the interpreter is enabled, in certain cases the app size could increase.
- App execution speed will be slower because interpreted code runs more slowly than AOT-compiled code. This can range from unmeasurable to unacceptable, so performance testing should be performed.
- Native stack traces in crash reports become less useful, because they'll contain generic frames from the interpreter that don't mention the code that's executing. However, managed stack traces won't change.

The interpreter is enabled by default for debug builds, and can be enabled for release builds.

> [!TIP]
> If your app works correctly as a debug build but then crashes as a release build, try enabling the interpreter for your app's release build. It may be that your app, of one of its libraries, uses a feature that requires the interpreter.

## Enable the interpreter

The Mono interpreter can be enabled by setting the `UseInterpreter` MSBuild property to `true`. For example, add the following XML to your .NET MAUI app project file to enable the interpreter for release builds on iOS:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <UseInterpreter>true</UseInterpreter>
</PropertyGroup>
```

The interpreter can be enabled for Mac Catalyst release builds on ARM64 by changing `-ios` in the above example to `-maccatalyst-arm64`.

> [!WARNING]
> Don't enable the interpreter for release builds on Android because it disables JIT compilation.

On iOS and Mac Catalyst, the interpreter can also be enabled with the `MtouchInterpreter` MSBuild property. This build property also optionally takes a comma-separated list of assemblies to be interpreted. `all` can be used to specify all assemblies, and when prefixed with a minus sign, an assembly will be AOT-compiled. This enables you to:

- Interpret all assemblies by specifying `all` or AOT compile everything by specifying `-all`.
- Interpret individual assemblies by specifying **MyAssembly.dll** or AOT compile individual assemblies by specifying **-MyAssembly.dll**.
- Mix and match to interpret some assemblies and AOT compile other assemblies.

The following example shows how to interpret all assemblies except **System.dll**:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <!-- Interpret everything, except System.Xml.dll -->
    <MtouchInterpreter>all,-System.Xml.dll</MtouchInterpreter>
</PropertyGroup>
```

The following example shows how to AOT compile all assemblies except **System.Numerics.dll**:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <!-- AOT everything, except System.Numerics.dll, which will be interpreted -->
    <MtouchInterpreter>-all,System.Numerics.dll</MtouchInterpreter>
</PropertyGroup>
```

Alternatively, use the following example to AOT compile all assemblies, while still allowing the interpreter to perform code generation:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-ios')) and '$(Configuration)' == 'Release'">
    <MtouchInterpreter>-all</MtouchInterpreter>
</PropertyGroup>
```

Another common scenario where the interpreter is required is Mac Catalyst apps running on ARM64, which can throw an exception on launch. A launch exception can often be fixed by enabling the interpreter:

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-maccatalyst-arm64')) and '$(Configuration)' == 'Release'">
    <MtouchInterpreter>-all,MyAssembly.dll</MtouchInterpreter>
</PropertyGroup>
```

Enabling the interpreter in this scenario is necessary because apps can't use JIT compilation on ARM64 because macOS doesn't allow it.
