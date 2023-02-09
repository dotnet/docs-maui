---
title: "Rectangle"
description: "The .NET MAUI Rectangle class can be used to draw rectangles."
ms.date: 01/12/2022
---

# Rectangle

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Rectangle> class derives from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, and can be used to draw rectangles and squares. For information on the properties that the <xref:Microsoft.Maui.Controls.Shapes.Rectangle> class inherits from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, see [.NET MAUI Shapes](index.md).

<xref:Microsoft.Maui.Controls.Shapes.Rectangle> defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.Rectangle.RadiusX>, of type `double`, which is the x-axis radius that's used to round the corners of the rectangle. The default value of this property is 0.0.
- <xref:Microsoft.Maui.Controls.Shapes.Rectangle.RadiusY>, of type `double`, which is the y-axis radius that's used to round the corners of the rectangle. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.Shapes.Rectangle> class sets the <xref:Microsoft.Maui.Controls.Shapes.Shape.Aspect> property, inherited from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, to `Stretch.Fill`. For more information about the <xref:Microsoft.Maui.Controls.Shapes.Shape.Aspect> property, see [Stretch shapes](index.md#stretch-shapes).

## Create a Rectangle

To draw a rectangle, create a <xref:Microsoft.Maui.Controls.Shapes.Rectangle> object and sets its <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties. To paint the inside of the rectangle, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object. To give the rectangle an outline, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Stroke> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object. The <xref:Microsoft.Maui.Controls.Shapes.Shape.StrokeThickness> property specifies the thickness of the rectangle outline. For more information about <xref:Microsoft.Maui.Controls.Brush> objects, see [Brushes](~/user-interface/brushes/index.md).

To give the rectangle rounded corners, set its <xref:Microsoft.Maui.Controls.Shapes.Rectangle.RadiusX> and <xref:Microsoft.Maui.Controls.Shapes.Rectangle.RadiusY> properties. These properties set the x-axis and y-axis radii that's used to round the corners of the rectangle.

> [!NOTE]
> There's also a <xref:Microsoft.Maui.Controls.Shapes.RoundRectangle> class, that has a `CornerRadius` <xref:Microsoft.Maui.Controls.BindableProperty>, which can be used to draw rectangles with rounded corners.

To draw a square, make the <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties of the <xref:Microsoft.Maui.Controls.Shapes.Rectangle> object equal.

The following XAML example shows how to draw a filled rectangle:

```xaml
<Rectangle Fill="Red"
           WidthRequest="150"
           HeightRequest="50"
           HorizontalOptions="Start" />
```

In this example, a red filled rectangle with dimensions 150x50 (device-independent units) is drawn:

:::image type="content" source="media/rectangle/filled.png" alt-text="Filled rectangle.":::

The following XAML example shows how to draw a filled rectangle, with rounded corners:

```xaml
<Rectangle Fill="Blue"
           Stroke="Black"
           StrokeThickness="3"
           RadiusX="50"
           RadiusY="10"
           WidthRequest="200"
           HeightRequest="100"
           HorizontalOptions="Start" />
```

In this example, a blue filled rectangle with rounded corners is drawn:

:::image type="content" source="media/rectangle/rounded.png" alt-text="Rectangle with rounded corners.":::

For information about drawing a dashed rectangle, see [Draw dashed shapes](index.md#draw-dashed-shapes).
