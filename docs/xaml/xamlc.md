---
title: "XAML compilation"
description: ".NET MAUI XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC)."
ms.date: 01/26/2022
---

# XAML compilation

.NET Multi-platform App UI (.NET MAUI) XAML is compiled directly into intermediate language (IL) with the XAML compiler (XAMLC). XAML compilation offers a number of a benefits:

- It performs compile-time checking of XAML, notifying you of any errors.
- It removes some of the load and instantiation time for XAML elements.
- It helps to reduce the file size of the final assembly by no longer including .xaml files.

XAML compilation is enabled by default in .NET MAUI apps that are built in release configuration, and disabled by default in apps that are built in debug configuration. However, these defaults can be overridden with the `XamlCompilationAttribute` class.

## Enable compilation

XAML compilation can be enabled by passing `XamlCompilationOptions.Compile` to the `XamlCompilationAttribute`:

```csharp
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
```

In this example, XAML compilation is enabled for all of the XAML contained within the assembly, with XAML errors being reported at compile-time rather than run-time.

> [!TIP]
> While the `XamlCompilationAttribute` can be placed anywhere, a good place to put it is in **AssemblyInfo.cs**.

XAML compilation can also be enabled at the class level:

```csharp
[XamlCompilation (XamlCompilationOptions.Compile)]
public class MyPage : ContentPage
{
    ...
}
```

In this example, XAML compilation is enabled only for the `MyPage` class.

> [!NOTE]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled Bindings](~/fundamentals/data-binding/compiled-bindings.md).

## Disable compilation

XAML compilation can be disabled by passing `XamlCompilationOptions.Skip` to the `XamlCompilationAttribute`:

```csharp
[assembly: XamlCompilation(XamlCompilationOptions.Skip)]
```

In this example, XAML compilation is disabled within the assembly, with XAML errors being reported at run-time rather than compile-time.

XAML compilation can also be disabled at the class level:

```csharp
[XamlCompilation (XamlCompilationOptions.Skip)]
public class MyPage : ContentPage
{
    ...
}
```

In this example, XAML compilation is disabled only for the `MyPage` class.
