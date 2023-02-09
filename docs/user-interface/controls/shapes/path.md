---
title: "Path"
description: "The .NET MAUI Path class can be used to draw curves and complex shapes."
ms.date: 01/12/2022
---

# Path

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Path> class derives from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, and can be used to draw curves and complex shapes. These curves and shapes are often described using <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects. For information on the properties that the <xref:Microsoft.Maui.Controls.Shapes.Path> class inherits from the <xref:Microsoft.Maui.Controls.Shapes.Shape> class, see [Shapes](index.md).

<xref:Microsoft.Maui.Controls.Shapes.Path> defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.Path.Data>, of type <xref:Microsoft.Maui.Controls.Shapes.Geometry>, which specifies the shape to be drawn.
- <xref:Microsoft.Maui.Controls.Shapes.Path.RenderTransform>, of type <xref:Microsoft.Maui.Controls.Shapes.Transform>, which represents the transform that is applied to the geometry of a path prior to it being drawn.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

For more information about transforms, see [Path Transforms](path-transforms.md).

## Create a Path

To draw a path, create a <xref:Microsoft.Maui.Controls.Shapes.Path> object and set its <xref:Microsoft.Maui.Controls.Shapes.Path.Data> property. There are two techniques for setting the <xref:Microsoft.Maui.Controls.Shapes.Path.Data> property:

- You can set a string value for <xref:Microsoft.Maui.Controls.Shapes.Path.Data> in XAML, using path markup syntax. With this approach, the `Path.Data` value is consuming a serialization format for graphics. Typically, you don't edit this string value by hand after it's created. Instead, you use design tools to manipulate the data, and export it as a string fragment that's consumable by the <xref:Microsoft.Maui.Controls.Shapes.Path.Data> property.
- You can set the <xref:Microsoft.Maui.Controls.Shapes.Path.Data> property to a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object. This can be a specific <xref:Microsoft.Maui.Controls.Shapes.Geometry> object, or a <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> which acts as a container that can combine multiple geometry objects into a single object.

### Create a Path with path markup syntax

The following XAML example shows how to draw a triangle using path markup syntax:

```xaml
<Path Data="M 10,100 L 100,100 100,50Z"
      Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Start" />
```

The <xref:Microsoft.Maui.Controls.Shapes.Path.Data> string begins with the move command, indicated by `M`, which establishes an absolute start point for the path. `L` is the line command, which creates a straight line from the start point to the specified end point. `Z` is the close command, which creates a line that connects the current point to the starting point. The result is a triangle:

:::image type="content" source="media/path/triangle.png" alt-text="Path triangle.":::

For more information about path markup syntax, see [Path markup syntax](path-markup-syntax.md).

### Create a Path with Geometry objects

Curves and shapes can be described using <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects, which are used to set the <xref:Microsoft.Maui.Controls.Shapes.Path> object's <xref:Microsoft.Maui.Controls.Shapes.Path.Data> property. There are a variety of <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects to choose from. The <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry>, <xref:Microsoft.Maui.Controls.Shapes.LineGeometry>, and <xref:Microsoft.Maui.Controls.Shapes.RectangleGeometry> classes describe relatively simple shapes. To create more complex shapes or create curves, use a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry>.

<xref:Microsoft.Maui.Controls.Shapes.PathGeometry> objects are comprised of one or more <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects. Each <xref:Microsoft.Maui.Controls.Shapes.PathFigure> object represents a different shape. Each <xref:Microsoft.Maui.Controls.Shapes.PathFigure> object is itself comprised of one or more <xref:Microsoft.Maui.Controls.Shapes.PathSegment> objects, each representing a connection portion of the shape. Segment types include the following the <xref:Microsoft.Maui.Controls.Shapes.LineSegment>, <xref:Microsoft.Maui.Controls.Shapes.BezierSegment>, and <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> classes.

The following XAML example shows how to draw a triangle using a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> object:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Start">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure IsClosed="True"
                                StartPoint="10,100">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <LineSegment Point="100,100" />
                                <LineSegment Point="100,50" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, the start point of the triangle is (10,100). A line segment is drawn from (10,100) to (100,100), and from (100,100) to (100,50). Then the figures first and last segments are connected, because the `PathFigure.IsClosed` property is set to `true`. The result is a triangle:

:::image type="content" source="media/path/triangle.png" alt-text="Path triangle.":::

For more information about geometries, see [Geometries](geometries.md).
