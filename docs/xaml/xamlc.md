---
title: "XAML compilation"
description: ".NET MAUI XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC)."
ms.date: 05/12/2026
---

# XAML Processing

.NET Multi-platform App UI (.NET MAUI) XAML can be processed, and inflated into a tree of objects in different ways explained here. As of .NET 10, default inflation is runtime for debug builds, XamlC (XamlCompilation) for Release. We encourage you to try source generation and use it if it works for you. This will become the future in new projects, then in all projects, soon.

## XAML Compilation

XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC). XAML compilation offers a number of benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It removes some of the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly by no longer including .xaml files.

XAML compilation is enabled by default in .NET MAUI apps. For apps built using the debug configuration, XAML compilation provides compile-time validation of XAML, but does not convert the XAML to IL in the assembly. Instead, XAML files are included as embedded resources in the app package, and evaluated at runtime. For apps built using the release configuration, XAML compilation provides compile-time validation of XAML, and converts the XAML to IL that's written to the assembly. However, XAML compilation behavior can be overridden in both configurations with the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute> class.

> [!IMPORTANT]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## XAML Runtime Inflation

XAML can be inflated at Runtime using reflection. It has advantages, like allowing Hot Reload scenario, shortening build times, allow the report of diagnostics to IDE. However typically this method should be avoided because it is also the slowest and syntax errors are only caught at runtime.

## XAML Source Generation

Starting with .NET 10, XAML can be transformed into C# code at compilaiton time. It provides the following benefits:

- Consistency: same generated code used in Debug and Release
- Speed: inflation times on device are 10000% (100 times) faster in Debug, and 25% faster on Release. The volume of allocation is reduced in the same proportion
- Debug: you can see the generated code, you can break and debug it.

This is the recommended way going further. It will be enabled by default in the future.

### Enable Source Generation, and per file settings

We no longer recommend using `[XamlCompilation]` attribute to enable or disable per file compilation.

You can enable XAML source generation at the project level, by setting the `MauiXamlInflator` value to `SourceGen` in your csproj file as shown here:

```xml
<MauiXamlInflator>SourceGen</MauiXamlInflator>
```

This will use source generation for both Release and Debug configurations, for all files.

You can revert to the default per file (or use wildcards) or force another inflator

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" />        <!-- enable sourcegen on a single file. prefer setting it at project level -->
    <MauiXaml Update="Controls\**.xaml" Inflator="Default" />     <!-- revert to defaults for all XAML in Controls. as of .NET 10, default is XamlC for Release, Runtime for Debug -->
    <MauiXaml Update="Controls\**.xaml" Inflator="Runtime" />     <!-- force runtime inflation. if you have to do this, it probably indicates a bug in both XamlC and sourcegen, please report -->
</ItemGroup>
```

There other metadata you can set to instruct xaml sourcegenerator

```xml
<ItemGroup>
    <MauiXaml Update="MyFile.xaml" Inflator="SourceGen" NoWarn="0612;0618" />   <!-- prevent the compiler to fail if the xaml use deprecated API -->
</ItemGroup>
```

:::moniker range=">=net-maui-11.0"

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
