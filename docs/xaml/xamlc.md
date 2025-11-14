---
title: "XAML processing"
description: ".NET MAUI XAML is processed and inflated at build time or runtime using different inflators, including source generation."
ms.date: 11/14/2024
---

# XAML processing

.NET Multi-platform App UI (.NET MAUI) XAML is processed and inflated into the corresponding user interface. XAML processing offers a number of benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It improves the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly.

## XAML inflators

.NET MAUI provides different XAML inflators that control how XAML is processed and converted into user interface elements:

- **Default inflator** - Processes XAML at build time and embeds it as resources in the assembly. XAML is inflated at runtime.
- **Source generation inflator** - Generates C# code at build time from XAML, which is then compiled into the assembly. This provides the best performance and smallest app size.

## Using source generation (recommended)

Source generation is the recommended approach for processing XAML in .NET MAUI applications. It generates C# code from your XAML at build time, resulting in better performance and smaller app sizes.

To enable source generation for your entire project, add the following property to your project file:

```xml
<PropertyGroup>
  <MauiXamlInflator>SourceGen</MauiXamlInflator>
</PropertyGroup>
```

With source generation enabled, your XAML files are processed at build time and converted to C# code, which is then compiled into your application. This eliminates the need for runtime XAML parsing and improves application startup performance.

> [!TIP]
> Source generation is particularly beneficial for release builds where performance and app size are critical. We recommend trying source generation in your projects to take advantage of these benefits.

> [!IMPORTANT]
> Compiled bindings can be enabled to further improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## Per-file inflator selection

In rare cases where you need to use a different inflator for specific XAML files, you can set the `Inflator` metadata on individual `MauiXaml` items in your project file:

```xml
<ItemGroup>
  <MauiXaml Update="Views\MySpecialPage.xaml">
    <Inflator>Default</Inflator>
  </MauiXaml>
</ItemGroup>
```

This allows you to use the default inflator for specific files while using source generation for the rest of your project.

> [!NOTE]
> Per-file inflator selection should only be used in exceptional cases where specific XAML files have compatibility issues with source generation.

## Legacy XamlCompilation attribute

The `XamlCompilationAttribute` class was used in earlier versions to control XAML compilation behavior. This attribute is now obsolete and should not be used in new projects. Instead, rely on the default XAML processing behavior or use the `MauiXamlInflator` property to enable source generation.

If your project contains code like this:

```csharp
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
```

or

```csharp
[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class MyPage : ContentPage
{
    ...
}
```

You can safely remove these attributes and trust the default XAML processing behavior, which is optimized for .NET MAUI applications.
