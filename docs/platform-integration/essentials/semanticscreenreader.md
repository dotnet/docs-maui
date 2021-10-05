---
title: "SemanticScreenReader"
description: "Learn how to make a screen reader read out text, using .NET MAUI. The SemanticScreenReader class in Microsoft.Maui.Essentials namespace is used to instruct the screen reader to announce the specified text."
ms.date: 10/05/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# SemanticScreenReader

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `SemanticScreenReader` class. This class lets you instruct the screen reader to announce the specified text.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Using SemanticScreenReader

To instruct the screen reader to announce text, use the `SemanticScreenReader.Announce` method, passing a `string` argument that represents the text. The following example demonstrates using this method:

```csharp
SemanticScreenReader.Announce("This is the announcement text.");
```

## Limitations

The default platform screen reader must be enabled for text to be read aloud.

<!-- Todo: insert link to relevant section of accessibility doc that discusses enabling screen readers. -->

## API

- [Screenshot source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/SemanticScreenReader)
<!-- - [Screenshot API documentation](xref:Microsoft.Maui.Essentials.SemanticScreenReader)-->
