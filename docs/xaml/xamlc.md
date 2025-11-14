---
title: "XAML compilation"
description: ".NET MAUI XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC)."
ms.date: 11/14/2025
---

# XAML Processing

.NET Multi-platform App UI (.NET MAUI) XAML can be processed, and inflated into a tree of objects in different ways explained here. As of net10, default inflation is runtime for debug builds, XamlC (XamlCompilation) for Release. We encourage you to try source generation and use it if it works for you. This will become the future in new projects, then in all projects, soon.

## XAML Compilation

XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC). XAML compilation offers a number of benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It removes some of the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly by no longer including .xaml files.

XAML compilation is enabled by default in .NET MAUI apps. For apps built using the debug configuration, XAML compilation provides compile-time validation of XAML, but does not convert the XAML to IL in the assembly. Instead, XAML files are included as embedded resources in the app package, and evaluated at runtime. For apps built using the release configuration, XAML compilation provides compile-time validation of XAML, and converts the XAML to IL that's written to the assembly. However, XAML compilation behavior can be overridden in both configurations with the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute> class.

> [!IMPORTANT]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## XAML runtime inflation

XAML can be inflated at Runtime using reflection. It has advantages, like allowing HotReload scenario, shortening build times, allow the report of diagnostics to IDE.

## XAML Sourcegeneration

Starting with net10, XAML can be transformed into C# code at compilaiton time. It provides the following benefits:

- Consistency: same generated code used in Debug and Release
- Speed: inflation times on device are 10000% (100 times) faster in Debug, and 25% faster on Release. The volume of allocation is reduced in the same proportion
- Debug: you can see the generated code, you can break and debug it.

This is the recommended way going further. It will be enabled by default in the future.

## Enable Source Generation, and per file settings

We no longer recommend using `[XamlCompilation]` attribute to enable or disable per file compilation.

You can enable XAML source generation at the project level, by setting the `MauiXamlInflator` value to `SourceGen` in your csproj file as shown here:

```xml
<MauiXamlInflator>SourceGen</MauiXamlInflator>
```

This will use source generation for both Release and Debug configurations.

You can revert to the default per file (or use wildcards) or force another inflator

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" />        <!-- enable sourcegen on a single file. prefer setting it at project level -->
    <MauiXaml Update="Controls\**.xaml" Inflator="" />            <!-- revert to defaults for all xaml in Controls -->
    <MauiXaml Update="Controls\**.xaml" Inflator="Runtime" />     <!-- force runtime inflation. if you have to do this, it probably indicates a bug in both XamlC and sourcegen, please report -->
</ItemGroup>
```

There other metadata you can set to instruct xaml sourcegenerator

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" NoWarn="0612;0618" />   <!-- prevent the compiler to fail if the xaml use deprecated API -->
</ItemGroup>
```

