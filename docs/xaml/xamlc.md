---
title: "XAML compilation"
description: ".NET MAUI XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC)."
ms.date: 02/24/2022
---

# XAML compilation

.NET Multi-platform App UI (.NET MAUI) XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC). XAML compilation offers a number of benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It removes some of the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly by no longer including .xaml files.

XAML compilation is enabled by default in .NET MAUI apps. For apps built using the debug configuration, XAML compilation provides compile-time validation of XAML, but does not convert the XAML to IL in the assembly. Instead, XAML files are included as embedded resources in the app package, and evaluated at runtime. For apps built using the release configuration, XAML compilation provides compile-time validation of XAML, and converts the XAML to IL that's written to the assembly. However, XAML compilation behavior can be overridden in both configurations with the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute> class.

> [!IMPORTANT]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## Disable XAML compilation

XAML compilation can be disabled by passing `XamlCompilationOptions.Skip` to the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute>:

```csharp
[assembly: XamlCompilation(XamlCompilationOptions.Skip)]
```

In this example, XAML compilation is disabled within the assembly, with XAML errors being reported at runtime rather than compile-time.

XAML compilation can also be disabled at the type level:

```csharp
[XamlCompilation (XamlCompilationOptions.Skip)]
public partial class MyPage : ContentPage
{
    ...
}
```

In this example, XAML compilation is disabled only for the `MyPage` class.

> [!WARNING]
> Disabling XAML compilation is not recommended because XAML is then parsed and interpreted at runtime, which will reduce app performance.

## Enable XAML compilation

Because XAML compilation is enabled by default in .NET MAUI apps, removing any `XamlCompilation(XamlCompilationOptions.Skip)` statements will ensure that XAML compilation is enabled.

Alternatively, XAML compilation can be forcibly enabled by passing `XamlCompilationOptions.Compile` to the <xref:Microsoft.Maui.Controls.Xaml.XamlCompilationAttribute>:

```csharp
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
```

In this example, XAML compilation is enabled for all of the XAML contained within the assembly, with XAML errors being reported at compile-time rather than runtime.

XAML compilation can also be enabled at the type level:

```csharp
[XamlCompilation (XamlCompilationOptions.Compile)]
public partial class MyPage : ContentPage
{
    ...
}
```

In this example, XAML compilation is enabled only for the `MyPage` class.
