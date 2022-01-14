---
title: "Brushes"
description: "The .NET MAUI Brush class is an abstract class that paints an area with its output."
ms.date: 01/11/2022
---

# Brushes

A .NET Multi-platform App UI (.NET MAUI) brush enables you to paint an area, such as the background of a control, using different approaches.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `Brush` class is an abstract class that paints an area with its output. Classes that derive from `Brush` describe different ways of painting an area. The following list describes the different brush types available in .NET MAUI:

- `SolidColorBrush`, which paints an area with a solid color. For more information, see [Solid color brushes](solidcolor.md).
- `LinearGradientBrush`, which paints an area with a linear gradient. For more information, see [Linear gradient brushes](lineargradient.md).
- `RadialGradientBrush`, which paints an area with a radial gradient. For more information, see [Radial gradient brushes](radialgradient.md).

Instances of these brush types can be assigned to the `Stroke` and `Fill` properties of a `Shape`, the `Stoke` property of a `Border`, the `Brush` property of a `Shadow`, and the `Background` property of a `VisualElement`.

> [!NOTE]
> The `VisualElement.Background` property enables brushes to be used as the background in any control.

The `Brush` class also has an `IsNullOrEmpty` method that returns a `bool` that represents whether the brush is defined or not.
