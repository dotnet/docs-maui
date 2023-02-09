---
title: ".NET MAUI Shapes: Line"
description: "The .NET MAUI Line class can be used to draw lines."
ms.date: 01/12/2022
---

# Line

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Line> class derives from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, and can be used to draw lines. For information on the properties that the <xref:Microsoft.Maui.Controls.Shapes.Line> class inherits from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, see [Shapes](index.md).

<xref:Microsoft.Maui.Controls.Shapes.Line> defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.Line.X1>, of type double, indicates the x-coordinate of the start point of the line. The default value of this property is 0.0.
- <xref:Microsoft.Maui.Controls.Shapes.Line.Y1>, of type double, indicates the y-coordinate of the start point of the line. The default value of this property is 0.0.
- <xref:Microsoft.Maui.Controls.Shapes.Line.X2>, of type double, indicates the x-coordinate of the end point of the line. The default value of this property is 0.0.
- <xref:Microsoft.Maui.Controls.Shapes.Line.Y2>, of type double, indicates the y-coordinate of the end point of the line. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

For information about controlling how line ends are drawn, see [Control line ends](index.md#control-line-ends).

## Create a Line

To draw a line, create a <xref:Microsoft.Maui.Controls.Shapes.Line> object and set its `X1` and `Y1` properties to its start point, and its `X2` and `Y2` properties to its end point. In addition, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Stroke> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object because a line without a stroke is invisible. For more information about <xref:Microsoft.Maui.Controls.Brush> objects, see [Brushes](~/user-interface/brushes/index.md).

> [!NOTE]
> Setting the <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> property of a <xref:Microsoft.Maui.Controls.Shapes.Line> has no effect, because a line has no interior.

The following XAML example shows how to draw a line:

```xaml
<Line X1="40"
      Y1="0"
      X2="0"
      Y2="120"
      Stroke="Red" />
```

In this example, a red diagonal line is drawn from (40,0) to (0,120):

:::image type="content" source="media/line/line.png" alt-text="Diagonal line.":::

Because the <xref:Microsoft.Maui.Controls.Shapes.Line.X1>, <xref:Microsoft.Maui.Controls.Shapes.Line.Y1>, <xref:Microsoft.Maui.Controls.Shapes.Line.X2>, and <xref:Microsoft.Maui.Controls.Shapes.Line.Y2> properties have default values of 0, it's possible to draw some lines with minimal syntax:

```xaml
<Line Stroke="Red"
      X2="200" />
```

In this example, a horizontal line that's 200 device-independent units long is defined. Because the other properties are 0 by default, a line is drawn from (0,0) to (200,0).

The following XAML example shows how to draw a dashed line:

```xaml
<Line X1="40"
      Y1="0"
      X2="0"
      Y2="120"
      Stroke="DarkBlue"
      StrokeDashArray="1,1"
      StrokeDashOffset="6" />
```

In this example, a dark blue dashed diagonal line is drawn from (40,0) to (0,120):

:::image type="content" source="media/line/dashed-line.png" alt-text="Dashed line.":::

For more information about drawing a dashed line, see [Draw dashed shapes](index.md#draw-dashed-shapes).
