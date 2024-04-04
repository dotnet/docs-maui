---
title: "Brushes"
description: "The .NET MAUI Brush class is an abstract class that paints an area with its output."
ms.date: 01/11/2022
---

# Brushes

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-brushes)

A .NET Multi-platform App UI (.NET MAUI) brush enables you to paint an area, such as the background of a control, using different approaches.

The <xref:Microsoft.Maui.Controls.Brush> class is an abstract class that paints an area with its output. Classes that derive from <xref:Microsoft.Maui.Controls.Brush> describe different ways of painting an area. The following list describes the different brush types available in .NET MAUI:

- <xref:Microsoft.Maui.Controls.SolidColorBrush>, which paints an area with a solid color. For more information, see [Solid color brushes](solidcolor.md).
- <xref:Microsoft.Maui.Controls.LinearGradientBrush>, which paints an area with a linear gradient. For more information, see [Linear gradient brushes](lineargradient.md).
- <xref:Microsoft.Maui.Controls.RadialGradientBrush>, which paints an area with a radial gradient. For more information, see [Radial gradient brushes](radialgradient.md).

Instances of these brush types can be assigned to the `Stroke` and `Fill` properties of a <xref:Microsoft.Maui.Controls.Shapes.Shape>, the `Stroke` property of a <xref:Microsoft.Maui.Controls.Border>, the `Brush` property of a `Shadow`, and the `Background` property of a <xref:Microsoft.Maui.Controls.VisualElement>.

> [!NOTE]
> The `VisualElement.Background` property enables brushes to be used as the background in any control.

The <xref:Microsoft.Maui.Controls.Brush> class also has an `IsNullOrEmpty` method that returns a `bool` that represents whether the brush is defined or not.
