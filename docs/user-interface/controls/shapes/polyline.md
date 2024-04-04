---
title: "Polyline"
description: "The .NET MAUI Polyline class can be used to draw a series of connected straight lines."
ms.date: 01/12/2022
---

# Polyline

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Polyline> class derives from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, and can be used to draw a series of connected straight lines. A polyline is similar to a polygon, except the last point in a polyline is not connected to the first point. For information on the properties that the <xref:Microsoft.Maui.Controls.Shapes.Polyline> class inherits from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, see [Shapes](index.md).

<xref:Microsoft.Maui.Controls.Shapes.Polyline> defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.Polyline.Points>, of type <xref:Microsoft.Maui.Controls.PointCollection>, which is a collection of `Point` structures that describe the vertex points of the polyline.
- <xref:Microsoft.Maui.Controls.Shapes.Polyline.FillRule>, of type <xref:Microsoft.Maui.Controls.Shapes.FillRule>, which specifies how the intersecting areas in the polyline are combined. The default value of this property is `FillRule.EvenOdd`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The `PointsCollection` type is an `ObservableCollection` of `Point` objects. The `Point` structure defines `X` and `Y` properties, of type `double`, that represent an x- and y-coordinate pair in 2D space. Therefore, the `Points` property should be set to a list of x-coordinate and y-coordinate pairs that describe the polyline vertex points, delimited by a single comma and/or one or more spaces. For example, "40,10 70,80" and "40 10, 70 80" are both valid.

For more information about the <xref:Microsoft.Maui.Controls.Shapes.FillRule> enumeration, see [Fill rules](fillrules.md).

## Create a Polyline

To draw a polyline, create a <xref:Microsoft.Maui.Controls.Shapes.Polyline> object and set its `Points` property to the vertices of a shape. To give the polyline an outline, set its <xref:Microsoft.Maui.Controls.Shapes.Shape.Stroke> property to a <xref:Microsoft.Maui.Controls.Brush>-derived object. The <xref:Microsoft.Maui.Controls.Shapes.Shape.StrokeThickness> property specifies the thickness of the polyline outline. For more information about <xref:Microsoft.Maui.Controls.Brush> objects, see [Brushes](~/user-interface/brushes/index.md).

> [!IMPORTANT]
> If you set the <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> property of a <xref:Microsoft.Maui.Controls.Shapes.Polyline> to a <xref:Microsoft.Maui.Controls.Brush>-derived object, the interior space of the polyline is painted, even if the start point and end point do not intersect.

The following XAML example shows how to draw a polyline:

```xaml
<Polyline Points="0,0 10,30 15,0 18,60 23,30 35,30 40,0 43,60 48,30 100,30"
          Stroke="Red" />
```

In this example, a red polyline is drawn:

:::image type="content" source="media/polyline/stroke.png" alt-text="Polyline.":::

The following XAML example shows how to draw a dashed polyline:

```xaml
<Polyline Points="0,0 10,30 15,0 18,60 23,30 35,30 40,0 43,60 48,30 100,30"
          Stroke="Red"
          StrokeThickness="2"
          StrokeDashArray="1,1"
          StrokeDashOffset="6" />
```

In this example, the polyline is dashed:

:::image type="content" source="media/polyline/dashed.png" alt-text="Dashed polyline.":::

For more information about drawing a dashed polyline, see [Draw dashed shapes](index.md#draw-dashed-shapes).

The following XAML example shows a polyline that uses the default fill rule:

```xaml
<Polyline Points="0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48"
          Fill="Blue"
          Stroke="Red"
          StrokeThickness="3" />
```

In this example, the fill behavior of the polyline is determined using the <xref:Microsoft.Maui.Controls.Shapes.FillRule.EvenOdd> fill rule.

:::image type="content" source="media/polyline/evenodd.png" alt-text="EvenOdd polyline.":::

The following XAML example shows a polyline that uses the <xref:Microsoft.Maui.Controls.Shapes.FillRule.Nonzero> fill rule:

```xaml
<Polyline Points="0 48, 0 144, 96 150, 100 0, 192 0, 192 96, 50 96, 48 192, 150 200 144 48"
          Fill="Black"
          FillRule="Nonzero"
          Stroke="Yellow"
          StrokeThickness="3" />
```

:::image type="content" source="media/polyline/nonzero.png" alt-text="Nonzero polyline.":::

In this example, the fill behavior of the polyline is determined using the <xref:Microsoft.Maui.Controls.Shapes.FillRule.Nonzero> fill rule.
