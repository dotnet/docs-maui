---
title: "SemanticScreenReader"
description: "Learn how to make a screen reader read out text, using .NET MAUI. The SemanticScreenReader class in Microsoft.Maui.Accessability namespace is used to instruct a screen reader to announce the specified text."
ms.date: 05/12/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Accessability"]
---

# SemanticScreenReader

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `SemanticScreenReader` class. This class lets you instruct a screen reader to announce the specified text. The `ISemanticScreenReader` interface is exposed through the `SemanticScreenReader.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `SemanticScreenReader` and `ISemanticScreenReader` types are available in the `Microsoft.Maui.Accessability` namespace.

## Using SemanticScreenReader

To instruct a screen reader to announce text, use the `SemanticScreenReader.Announce` method, passing a `string` argument that represents the text. The following example demonstrates using this method:

:::code language="csharp" source="../snippets/shared_1/ScreenReaderPage.cs" id="announce":::

## Limitations

The default platform screen reader must be enabled for text to be read aloud.

<!-- Todo: insert link to relevant section of accessibility doc that discusses enabling screen readers. -->
