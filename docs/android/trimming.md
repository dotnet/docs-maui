---
title: "Trimming a .NET MAUI Android app"
description: "Learn about the .NET for Android trimmer which is used to eliminate unused code from a .NET MAUI Android app in order to reduce its size."
ms.date: 10/21/2024
no-loc: [ ILLink ]
monikerRange: ">=net-maui-9.0"
---

# Trimming a .NET MAUI Android app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Trimmer behavior

The linker enables you to trim your .NET MAUI Android apps. When trimming is enabled, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.

Trimming behavior can be configured for each build configuration of your app. By default, trimming is disabled for debug builds and enabled for release builds. This behavior can be changed with the `$(PublishTrimmed)` build property:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Debug'">
  <PublishTrimmed>True</PublishTrimmed>
</PropertyGroup>
```

> [!WARNING]
> Enabling trimming for your app's debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.

## Trimming granularity

Trimming granularity can be controlled by setting the `$(TrimMode)` build property to either `partial` or `full`:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <TrimMode>partial</TrimMode>
</PropertyGroup>
```

By default, release builds default to the `full` trim mode. The `partial` trim mode is used to only trim assemblies that have opted in to trimming with the `$(TrimmableAsssembly)` MSBuild item:

```xml
<ItemGroup>
  <TrimmableAssembly Include="MyAssembly" />
</ItemGroup>
```

This is equivalent to setting `[AssemblyMetadata("IsTrimmable", "True")]` when building the assembly.

[!INCLUDE [Trimming feature switches](../includes/feature-switches.md)]

[!INCLUDE [Control the trimmer](../includes/linker-control.md)]

## See also

- [Trim self-contained deployments and executables](/dotnet/core/deploying/trimming/trim-self-contained)
