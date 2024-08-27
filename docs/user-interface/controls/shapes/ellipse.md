---
title: "Ellipse"
description: "The .NET MAUI Ellipse class can be used to draw ellipses and circles."
ms.date: 08/30/2024
---

# Ellipse

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Ellipse> class derives from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, and can be used to draw ellipses and circles. For information on the properties that the <xref:Microsoft.Maui.Controls.Shapes.Ellipse> class inherits from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, see [Shapes](index.md).

The <xref:Microsoft.Maui.Controls.Shapes.Ellipse> class sets the <xref:Microsoft.Maui.Controls.Shapes.Shape.Aspect> property, inherited from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, to `Stretch.Fill`. For more information about the <xref:Microsoft.Maui.Controls.Shapes.Shape.Aspect> property, see [Stretch shapes](index.md#stretch-shapes).

## Create an Ellipse

To draw an ellipse, create an <xref:Microsoft.Maui.Controls.Shapes.Ellipse> object and set its <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties. To paint the inside of the ellipse, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object. To give the ellipse an outline, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Stroke> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object. The <xref:Microsoft.Maui.Controls.Shapes.Shape.StrokeThickness> property specifies the thickness of the ellipse outline. For more information about <xref:Microsoft.Maui.Controls.Brush> objects, see [Brushes](~/user-interface/brushes/index.md).

To draw a circle, make the <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.Shapes.Ellipse> object equal.

The following XAML example shows how to draw a filled ellipse:

```xaml
<Ellipse Fill="Red"
         WidthRequest="150"
         HeightRequest="50"
         HorizontalOptions="Start" />
```

In this example, a red filled ellipse with dimensions 150x50 (device-independent units) is drawn:

:::image type="content" source="media/ellipse/filled.png" alt-text="Filled ellipse.":::

The following XAML example shows how to draw a circle:

```xaml
<Ellipse Stroke="Red"
         StrokeThickness="4"
         WidthRequest="150"
         HeightRequest="150"
         HorizontalOptions="Start" />
```

In this example, a red circle with dimensions 150x150 (device-independent units) is drawn:

:::image type="content" source="media/ellipse/circle.png" alt-text="Unfilled circle.":::

For information about drawing a dashed ellipse, see [Draw dashed shapes](index.md#draw-dashed-shapes).
