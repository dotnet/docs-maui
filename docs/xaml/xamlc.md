---
title: "XAML compilation"
description: ".NET MAUI XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC)."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# XAML Processing

.NET Multi-platform App UI (.NET MAUI) XAML can be processed and inflated into a tree of objects in different ways. SourceGen is the default inflator in .NET 11 and later. The `Runtime` and `XamlC` inflators are deprecated and produce warning `MAUI1001` when selected. For more information, see [XAML Source Generation](#xaml-source-generation).

::: moniker range="<=net-maui-10.0"

In .NET 10 and earlier, default inflation is Runtime for Debug builds and XamlC for Release builds.

::: moniker-end

## XAML Compilation

When you select the XamlC inflator, XAML is compiled into intermediate language (IL) with the XAML compiler (XAMLC). XAML compilation offers the following benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It removes some of the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly by no longer including .xaml files.

::: moniker range="<=net-maui-10.0"

XAML compilation is enabled by default in .NET MAUI apps. For apps built using the Debug configuration, XAML compilation provides compile-time validation but doesn't convert XAML to IL. Instead, XAML files are included as embedded resources in the app package and evaluated at runtime. For apps built using the Release configuration, XAML compilation validates XAML and converts it to IL in the assembly. You can override XAML compilation behavior in both configurations with the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute> class.

::: moniker-end

::: moniker range=">=net-maui-11.0"

In .NET 11 and later, SourceGen is the default instead of XamlC. If you explicitly select XamlC, it is deprecated and produces warning `MAUI1001`.

::: moniker-end

> [!IMPORTANT]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## XAML Runtime Inflation

XAML can be inflated at runtime using reflection. This can support existing Hot Reload workflows, shorten build times, and report diagnostics to the IDE, but it's the slowest inflator and catches syntax errors only at runtime. Runtime inflation is deprecated in .NET 11 and later and produces warning `MAUI1001`; don't select it solely for Hot Reload, because Debug builds use SourceGen with incremental Hot Reload by default.

## XAML Source Generation

Starting with .NET 10, XAML can be transformed into C# code at compilation time. SourceGen provides the following benefits:

- Consistency: same generated code used in Debug and Release
- Speed: inflation times on device are 10000% (100 times) faster in Debug, and 25% faster on Release. The volume of allocation is reduced in the same proportion
- Debug: you can see the generated code, you can break and debug it.

SourceGen is the default inflator in .NET 11 and later.

### Enable Source Generation, and per file settings

We no longer recommend using `[XamlCompilation]` attribute to enable or disable per file compilation.

To explicitly select XAML source generation at the project level, set the `MauiXamlInflator` value to `SourceGen` in your project file:

```xml
<MauiXamlInflator>SourceGen</MauiXamlInflator>
```

This uses SourceGen for all XAML files in both Release and Debug configurations.

You can set an individual file or pattern to use the project default with `Inflator="Default"`, or explicitly select an inflator:

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" />        <!-- enable sourcegen on a single file. prefer setting it at project level -->
    <MauiXaml Update="Controls\**.xaml" Inflator="Default" />     <!-- use the project default inflator for files in Controls -->
    <MauiXaml Update="Controls\**.xaml" Inflator="Runtime" />     <!-- deprecated in .NET 11 and later; produces warning MAUI1001 -->
    <MauiXaml Update="Controls\**.xaml" Inflator="XamlC" />       <!-- deprecated in .NET 11 and later; produces warning MAUI1001 -->
</ItemGroup>
```

There other metadata you can set to instruct xaml sourcegenerator

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" NoWarn="0612;0618" />   <!-- prevent the compiler to fail if the xaml use deprecated API -->
</ItemGroup>
```

:::moniker range=">=net-maui-11.0"

For short computed values and event actions, see [C# expressions in XAML](csharp-expressions.md). Expressions require the `SourceGen` inflator and preview features.

### Inline C# with the `x:Code` directive

When XAML source generation is enabled, you can use the `x:Code` directive to embed a small block of C# directly inside a XAML file. The XAML source generator extracts the code and emits it into the generated partial class for the page or view, so the inline members behave exactly as if they were declared in a code-behind file.

`x:Code` is intended for short, view-local glue such as a single event handler or a private helper — it lets you keep that code next to the markup it serves without adding a separate code-behind partial. For anything larger, prefer a dedicated code-behind file.

> [!IMPORTANT]
> `x:Code` is a preview feature. To use it, set the `EnablePreviewFeatures` MSBuild property to `true` in your project file:
>
> ```xml
> <PropertyGroup>
>   <EnablePreviewFeatures>true</EnablePreviewFeatures>
> </PropertyGroup>
> ```

The `x:Code` element must be a direct child of the root element, and the root element must have an `x:Class` attribute. Wrap the code in a CDATA section so XAML doesn't try to parse it as markup:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.MainPage">
    <x:Code>
        <![CDATA[
        using System.Diagnostics;

        void OnButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked from inline x:Code.");
        }
        ]]>
    </x:Code>

    <Button Text="Click me"
            Clicked="OnButtonClicked" />
</ContentPage>
```

`using` directives in an `x:Code` block are hoisted to the top of the generated file, while everything else is emitted as members of the page's partial class. The source generator reports the following diagnostics when `x:Code` is used incorrectly:

- `MAUIX2015` — the `x:Code` element isn't a direct child of the root.
- `MAUIX2016` — the root element doesn't have an `x:Class` attribute.

:::moniker-end
