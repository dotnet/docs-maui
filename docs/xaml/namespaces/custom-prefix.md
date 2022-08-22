---
title: "Custom namespace prefixes in .NET MAUI"
description: "The .NET MAUI XmlnsPrefixAttribute class can be used by control authors to specify a recommended prefix to associate with a XAML namespace, for XAML usage."
ms.date: 01/26/2022
---

# Custom namespace prefixes

The NET Multi-platform App UI (.NET MAUI) `XmlnsPrefixAttribute` class can be used by control authors to specify a recommended prefix to associate with a XAML namespace, for XAML usage. The prefix is useful when supporting object tree serialization to XAML, or when interacting with a design environment that has XAML editing features. For example:

- XAML text editors could use the `XmlnsPrefixAttribute` as a hint for an initial XAML namespace `xmlns` mapping.
- XAML design environments could use the `XmlnsPrefixAttribute` to add mappings to the XAML when dragging objects out of a toolbox and onto a visual design surface.

Recommended namespace prefixes should be defined at the assembly level with the `XmlnsPrefixAttribute` constructor, which takes two arguments: a string that specifies the identifier of a XAML namespace, and a string that specifies a recommended prefix:

```csharp
[assembly: XmlnsPrefix("http://schemas.microsoft.com/dotnet/2021/maui", "maui")]
```

Prefixes should use short strings, because the prefix is typically applied to all serialized elements that come from the XAML namespace. Therefore, the prefix string length can have a noticeable effect on the size of the serialized XAML output.

> [!NOTE]
> More than one `XmlnsPrefixAttribute` can be applied to an assembly. For example, if you have an assembly that defines types for more than one XAML namespace, you could define different prefix values for each XAML namespace.
