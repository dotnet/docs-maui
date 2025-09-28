---
title: "Basic animation"
description: "The .NET MAUI ViewExtensions class, in the Microsoft.Maui.Controls namespace, provides extension methods that can be used to create and cancel basic animations."
ms.date: 04/03/2025
---

# Basic animation

::: moniker range="<=net-maui-9.0"

[!INCLUDE [Basic animation](../includes/basic-animation-dotnet9.md)]

::: moniker-end

::: moniker range=">=net-maui-10.0"

[!INCLUDE [Basic animation](../includes/basic-animation-dotnet10.md)]

::: moniker-end

## Canceling animations

The <xref:Microsoft.Maui.Controls.ViewExtensions.CancelAnimations%2A> extension method is used to cancel any animations, such as rotation, scaling, translation, and fading, that are running on a specific <xref:Microsoft.Maui.Controls.VisualElement>.

```csharp
image.CancelAnimations();
```

In this example, all animations that are running on the <xref:Microsoft.Maui.Controls.Image> instance are immediately canceled.
