---
title: "Geometries"
description: ".NET MAUI geometry classes enable you to describe the geometry of a 2D shape."
ms.date: 01/12/2022
---

# Geometries

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Shapes.Geometry> class, and the classes that derive from it, enable you to describe the geometry of a 2D shape. <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects can be simple, such as rectangles and circles, or composite, created from two or more geometry objects. In addition, more complex geometries can be created that include arcs and curves.

The <xref:Microsoft.Maui.Controls.Shapes.Geometry> class is the parent class for several classes that define different categories of geometries:

- <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry>, which represents the geometry of an ellipse or circle.
- <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup>, which represents a container that can combine multiple geometry objects into a single object.
- <xref:Microsoft.Maui.Controls.Shapes.LineGeometry>, which represents the geometry of a line.
- <xref:Microsoft.Maui.Controls.Shapes.PathGeometry>, which represents the geometry of a complex shape that can be composed of arcs, curves, ellipses, lines, and rectangles.
- <xref:Microsoft.Maui.Controls.Shapes.RectangleGeometry>, which represents the geometry of a rectangle or square.

> [!NOTE]
> There's also a <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry> class that derives from the <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> class. For more information, see [RoundRectangleGeometry](#roundrectanglegeometry).

The <xref:Microsoft.Maui.Controls.Shapes.Geometry> and <xref:Microsoft.Maui.Controls.Shapes.Shape> classes seem similar, in that they both describe 2D shapes, but have an important difference. The <xref:Microsoft.Maui.Controls.Shapes.Geometry> class derives from the <xref:Microsoft.Maui.Controls.BindableObject> class, while the <xref:Microsoft.Maui.Controls.Shapes.Shape> class derives from the <xref:Microsoft.Maui.Controls.View> class. Therefore, <xref:Microsoft.Maui.Controls.Shapes.Shape> objects can render themselves and participate in the layout system, while <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects cannot. While <xref:Microsoft.Maui.Controls.Shapes.Shape> objects are more readily usable than <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects, <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects are more versatile. While a <xref:Microsoft.Maui.Controls.Shapes.Shape> object is used to render 2D graphics, a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object can be used to define the geometric region for 2D graphics, and define a region for clipping.

The following classes have properties that can be set to <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects:

- The <xref:Microsoft.Maui.Controls.Shapes.Path> class uses a <xref:Microsoft.Maui.Controls.Shapes.Geometry> to describe its contents. You can render a <xref:Microsoft.Maui.Controls.Shapes.Geometry> by setting the `Path.Data` property to a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object, and setting the <xref:Microsoft.Maui.Controls.Shapes.Path> object's <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> and <xref:Microsoft.Maui.Controls.Shapes.Shape.Stroke> properties.
- The <xref:Microsoft.Maui.Controls.VisualElement> class has a <xref:Microsoft.Maui.Controls.VisualElement.Clip> property, of type <xref:Microsoft.Maui.Controls.Shapes.Geometry>, that defines the outline of the contents of an element. When the <xref:Microsoft.Maui.Controls.VisualElement.Clip> property is set to a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object, only the area that is within the region of the <xref:Microsoft.Maui.Controls.Shapes.Geometry> will be visible. For more information, see [Clip with a Geometry](#clip-with-a-geometry).

The classes that derive from the <xref:Microsoft.Maui.Controls.Shapes.Geometry> class can be grouped into three categories: simple geometries, path geometries, and composite geometries.

## Simple geometries

The simple geometry classes are <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry>, <xref:Microsoft.Maui.Controls.Shapes.LineGeometry>, and <xref:Microsoft.Maui.Controls.Shapes.RectangleGeometry>. They are used to create basic geometric shapes, such as circles, lines, and rectangles. These same shapes, as well as more complex shapes, can be created using a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> or by combining geometry objects together, but these classes provide a simpler approach for producing these basic geometric shapes.

### EllipseGeometry

An ellipse geometry represents the geometry or an ellipse or circle, and is defined by a center point, an x-radius, and a y-radius.

The <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.Center>, of type `Point`, which represents the center point of the geometry.
- <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.RadiusX>, of type `double`, which represents the x-radius value of the geometry. The default value of this property is 0.0.
- <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.RadiusY>, of type `double`, which represents the y-radius value of the geometry. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The following example shows how to create and render an <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Fill="Blue"
      Stroke="Red">
  <Path.Data>
    <EllipseGeometry Center="50,50"
                     RadiusX="50"
                     RadiusY="50" />
  </Path.Data>
</Path>
```

In this example, the center of the <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> is set to (50,50) and the x-radius and y-radius are both set to 50. This creates a red circle with a diameter of 100 device-independent units, whose interior is painted blue:

:::image type="content" source="media/geometry/ellipse.png" alt-text="EllipseGeometry.":::

### LineGeometry

A line geometry represents the geometry of a line, and is defined by specifying the start point of the line and the end point.

The <xref:Microsoft.Maui.Controls.Shapes.LineGeometry> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.LineGeometry.StartPoint>, of type `Point`, which represents the start point of the line.
- <xref:Microsoft.Maui.Controls.Shapes.LineGeometry.EndPoint>, of type `Point`, which represents the end point of the line.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.LineGeometry> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
  <Path.Data>
    <LineGeometry StartPoint="10,20"
                  EndPoint="100,130" />
  </Path.Data>
</Path>
```

In this example, a <xref:Microsoft.Maui.Controls.Shapes.LineGeometry> is drawn from (10,20) to (100,130):

:::image type="content" source="media/geometry/line.png" alt-text="LineGeometry.":::

> [!NOTE]
> Setting the <xref:Microsoft.Maui.Controls.Shapes.Shape.Fill> property of a <xref:Microsoft.Maui.Controls.Shapes.Path> that renders a <xref:Microsoft.Maui.Controls.Shapes.LineGeometry> will have no effect, because a line has no interior.

### RectangleGeometry

A rectangle geometry represents the geometry of a rectangle or square, and is defined with a `Rect` structure that specifies its relative position and its height and width.

The <xref:Microsoft.Maui.Controls.Shapes.RectangleGeometry> class defines the `Rect` property, of type `Rect`, which represents the dimensions of the rectangle. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.RectangleGeometry> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Fill="Blue"
      Stroke="Red">
  <Path.Data>
    <RectangleGeometry Rect="10,10,150,100" />
  </Path.Data>
</Path>
```

The position and dimensions of the rectangle are defined by a `Rect` structure. In this example, the position is (10,10), the width is 150, and the height is 100 device-independent units:

:::image type="content" source="media/geometry/rectangle.png" alt-text="RectangleGeometry.":::

## Path geometries

A path geometry describes a complex shape that can be composed of arcs, curves, ellipses, lines, and rectangles.

The <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.PathGeometry.Figures>, of type <xref:Microsoft.Maui.Controls.Shapes.PathFigureCollection>, which represents the collection of <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects that describe the path's contents.
- <xref:Microsoft.Maui.Controls.Shapes.PathGeometry.FillRule>, of type <xref:Microsoft.Maui.Controls.Shapes.FillRule>, which determines how the intersecting areas contained in the geometry are combined. The default value of this property is `FillRule.EvenOdd`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

For more information about the <xref:Microsoft.Maui.Controls.Shapes.FillRule> enumeration, see [.NET MAUI Shapes: Fill rules](fillrules.md).

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.PathGeometry.Figures> property is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> class, and so does not need to be explicitly set from XAML.

A <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> is made up of a collection of <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects, with each <xref:Microsoft.Maui.Controls.Shapes.PathFigure> describing a shape in the geometry. Each <xref:Microsoft.Maui.Controls.Shapes.PathFigure> is itself comprised of one or more <xref:Microsoft.Maui.Controls.Shapes.PathSegment> objects, each of which describes a segment of the shape. There are many types of segments:

- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment>, which creates an elliptical arc between two points.
- <xref:Microsoft.Maui.Controls.Shapes.BezierSegment>, which creates a cubic Bezier curve between two points.
- <xref:Microsoft.Maui.Controls.Shapes.LineSegment>, which creates a line between two points.
- <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment>, which creates a series of cubic Bezier curves.
- <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment>, which creates a series of lines.
- <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment>, which creates a series of quadratic Bezier curves.
- <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment>, which creates a quadratic Bezier curve.

All the above classes derive from the abstract <xref:Microsoft.Maui.Controls.Shapes.PathSegment> class.

The segments within a <xref:Microsoft.Maui.Controls.Shapes.PathFigure> are combined into a single geometric shape with the end point of each segment being the start point of the next segment. The <xref:Microsoft.Maui.Controls.Shapes.PathFigure.StartPoint> property of a <xref:Microsoft.Maui.Controls.Shapes.PathFigure> specifies the point from which the first segment is drawn. Each subsequent segment starts at the end point of the previous segment. For example, a vertical line from `10,50` to `10,150` can be defined by setting the <xref:Microsoft.Maui.Controls.Shapes.PathFigure.StartPoint> property to `10,50` and creating a <xref:Microsoft.Maui.Controls.Shapes.LineSegment> with a `Point` property setting of `10,150`:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,50">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <LineSegment Point="10,150" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

More complex geometries can be created by using a combination of <xref:Microsoft.Maui.Controls.Shapes.PathSegment> objects, and by using multiple <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects within a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry>.

### Create an ArcSegment

An <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> creates an elliptical arc between two points. An elliptical arc is defined by its start and end points, x- and y-radius, x-axis rotation factor, a value indicating whether the arc should be greater than 180 degrees, and a value describing the direction in which the arc is drawn.

The <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment.Point>, of type `Point`, which represents the endpoint of the elliptical arc. The default value of this property is (0,0).
- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment.Size>, of type `Size`, which represents the x- and y-radius of the arc. The default value of this property is (0,0).
- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment.RotationAngle>, of type `double`, which represents the amount in degrees by which the ellipse is rotated around the x-axis. The default value of this property is 0.
- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment.SweepDirection>, of type <xref:Microsoft.Maui.Controls.SweepDirection>, which specifies the direction in which the arc is drawn. The default value of this property is `SweepDirection.CounterClockwise`.
- <xref:Microsoft.Maui.Controls.Shapes.ArcSegment.IsLargeArc>, of type `bool`, which indicates whether the arc should be greater than 180 degrees. The default value of this property is `false`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> class does not contain a property for the starting point of the arc. It only defines the end point of the arc it represents. The start point of the arc is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> is added.

The <xref:Microsoft.Maui.Controls.SweepDirection> enumeration defines the following members:

- <xref:Microsoft.Maui.Controls.SweepDirection.CounterClockwise>, which specifies that arcs are drawn in a counter clockwise direction.
- <xref:Microsoft.Maui.Controls.SweepDirection.Clockwise>, which specifies that arcs are drawn in a clockwise direction.

The following example shows how to create and render an <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,10">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <ArcSegment Size="100,50"
                                            RotationAngle="45"
                                            IsLargeArc="True"
                                            SweepDirection="CounterClockwise"
                                            Point="200,100" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, an elliptical arc is drawn from (10,10) to (200,100).

### Create a BezierSegment

A <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> creates a cubic Bezier curve between two points. A cubic Bezier curve is defined by four points: a start point, an end point, and two control points.

The <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.BezierSegment.Point1>, of type `Point`, which represents the first control point of the curve. The default value of this property is (0,0).
- <xref:Microsoft.Maui.Controls.Shapes.BezierSegment.Point2>, of type `Point`, which represents the second control point of the curve. The default value of this property is (0,0).
- <xref:Microsoft.Maui.Controls.Shapes.BezierSegment.Point3>, of type `Point`, which represents the end point of the curve. The default value of this property is (0,0).

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> class does not contain a property for the starting point of the curve. The start point of the curve is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> is added.

The two control points of a cubic Bezier curve behave like magnets, attracting portions of what would otherwise be a straight line toward themselves and producing a curve. The first control point affects the start portion of the curve. The second control point affects the end portion of the curve. The curve doesn't necessarily pass through either of the control points. Instead, each control point moves its portion of the line toward itself, but not through itself.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,10">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <BezierSegment Point1="100,0"
                                               Point2="200,200"
                                               Point3="300,10" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, a cubic Bezier curve is drawn from (10,10) to (300,10). The curve has two control points at (100,0) and (200,200):

:::image type="content" source="media/geometry/beziersegment.png" alt-text="Line graphic shows a Bezier curve.":::

### Create a LineSegment

A <xref:Microsoft.Maui.Controls.Shapes.LineSegment> creates a line between two points.

The <xref:Microsoft.Maui.Controls.Shapes.LineSegment> class defines the `Point` property, of type `Point`, which represents the end point of the line segment. The default value of this property is (0,0), and it's backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.LineSegment> class does not contain a property for the starting point of the line. It only defines the end point. The start point of the line is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.LineSegment> is added.

The following example shows how to create and render <xref:Microsoft.Maui.Controls.Shapes.LineSegment> objects in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

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

In this example, a line segment is drawn from (10,100) to (100,100), and from (100,100) to (100,50). In addition, the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> is closed because its `IsClosed` property is set to `true`. This results in a triangle being drawn:

:::image type="content" source="media/geometry/linesegments.png" alt-text="Line graphic shows a triangle.":::

### Create a PolyBezierSegment

A <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> creates one or more cubic Bezier curves.

The <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> class defines the `Points` property, of type <xref:Microsoft.Maui.Controls.PointCollection>, which represents the points that define the <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment>. A <xref:Microsoft.Maui.Controls.PointCollection> is an `ObservableCollection` of `Point` objects. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> class does not contain a property for the starting point of the curve. The start point of the curve is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> is added.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,10">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <PolyBezierSegment Points="0,0 100,0 150,100 150,0 200,0 300,10" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.PolyBezierSegment> specifies two cubic Bezier curves. The first curve is from (10,10) to (150,100) with a control point of (0,0), and another control point of (100,0). The second curve is from (150,100) to (300,10) with a control point of (150,0) and another control point of (200,0):

:::image type="content" source="media/geometry/polybeziersegment.png" alt-text="Line graphic shows two connected Bezier curves.":::

### Create a PolyLineSegment

A <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> creates one or more line segments.

The <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> class defines the `Points` property, of type <xref:Microsoft.Maui.Controls.PointCollection>, which represents the points that define the <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment>. A <xref:Microsoft.Maui.Controls.PointCollection> is an `ObservableCollection` of `Point` objects. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> class does not contain a property for the starting point of the line. The start point of the line is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> is added.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigure StartPoint="10,10">
                    <PathFigure.Segments>
                        <PolyLineSegment Points="50,10 50,50" />
                    </PathFigure.Segments>
                </PathFigure>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.PolyLineSegment> specifies two lines. The first line is from (10,10) to (50,10), and the second line is from (50,10) to (50,50):

:::image type="content" source="media/geometry/polylinesegment.png" alt-text="Line graphic shows two lines at a right angle.":::

### Create a PolyQuadraticBezierSegment

A <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> creates one or more quadratic Bezier curves.

The <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> class defines the `Points` property, of type <xref:Microsoft.Maui.Controls.PointCollection>, which represents the points that define the <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment>. A <xref:Microsoft.Maui.Controls.PointCollection> is an `ObservableCollection` of `Point` objects. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> class does not contain a property for the starting point of the curve. The start point of the curve is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> is added.

The following example shows to create and render a <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,10">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <PolyQuadraticBezierSegment Points="100,100 150,50 0,100 15,200" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.PolyQuadraticBezierSegment> specifies two Bezier curves. The first curve is from (10,10) to (150,50) with a control point at (100,100). The second curve is from (100,100) to (15,200) with a control point at (0,100):

:::image type="content" source="media/geometry/polyquadraticbeziersegment.png" alt-text="Line graphic shows two connected overlapping Bezier curves.":::

### Create a QuadraticBezierSegment

A <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment> creates a quadratic Bezier curve between two points.

The <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment.Point1>, of type `Point`, which represents the control point of the curve. The default value of this property is (0,0).
- <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment.Point2>, of type `Point`, which represents the end point of the curve. The default value of this property is (0,0).

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment> class does not contain a property for the starting point of the curve. The start point of the curve is the current point of the <xref:Microsoft.Maui.Controls.Shapes.PathFigure> to which the <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment> is added.

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.QuadraticBezierSegment> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigureCollection>
                    <PathFigure StartPoint="10,10">
                        <PathFigure.Segments>
                            <PathSegmentCollection>
                                <QuadraticBezierSegment Point1="200,200"
                                                        Point2="300,10" />
                            </PathSegmentCollection>
                        </PathFigure.Segments>
                    </PathFigure>
                </PathFigureCollection>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, a quadratic Bezier curve is drawn from (10,10) to (300,10). The curve has a control point at (200,200):

:::image type="content" source="media/geometry/quadraticbeziersegment.png" alt-text="Line graphic shows a quadratic Bezier curve.":::

### Create complex geometries

More complex geometries can be created by using a combination of <xref:Microsoft.Maui.Controls.Shapes.PathSegment> objects. The following example creates a shape using a <xref:Microsoft.Maui.Controls.Shapes.BezierSegment>, a <xref:Microsoft.Maui.Controls.Shapes.LineSegment>, and an <xref:Microsoft.Maui.Controls.Shapes.ArcSegment>:

```xaml
<Path Stroke="Black">
    <Path.Data>
        <PathGeometry>
            <PathGeometry.Figures>
                <PathFigure StartPoint="10,50">
                    <PathFigure.Segments>
                        <BezierSegment Point1="100,0"
                                       Point2="200,200"
                                       Point3="300,100"/>
                        <LineSegment Point="400,100" />
                        <ArcSegment Size="50,50"
                                    RotationAngle="45"
                                    IsLargeArc="True"
                                    SweepDirection="Clockwise"
                                    Point="200,100"/>
                    </PathFigure.Segments>
                </PathFigure>
            </PathGeometry.Figures>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, a <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> is first defined using four points. The example then adds a <xref:Microsoft.Maui.Controls.Shapes.LineSegment>, which is drawn between the end point of the <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> to the point specified by the <xref:Microsoft.Maui.Controls.Shapes.LineSegment>. Finally, an <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> is drawn from the end point of the <xref:Microsoft.Maui.Controls.Shapes.LineSegment> to the point specified by the <xref:Microsoft.Maui.Controls.Shapes.ArcSegment>.

Even more complex geometries can be created by using multiple <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects within a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry>. The following example creates a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> from seven <xref:Microsoft.Maui.Controls.Shapes.PathFigure> objects, some of which contain multiple <xref:Microsoft.Maui.Controls.Shapes.PathSegment> objects:

```xaml
<Path Stroke="Red"
      StrokeThickness="12"
      StrokeLineJoin="Round">
    <Path.Data>
        <PathGeometry>
            <!-- H -->
            <PathFigure StartPoint="0,0">
                <LineSegment Point="0,100" />
            </PathFigure>
            <PathFigure StartPoint="0,50">
                <LineSegment Point="50,50" />
            </PathFigure>
            <PathFigure StartPoint="50,0">
                <LineSegment Point="50,100" />
            </PathFigure>

            <!-- E -->
            <PathFigure StartPoint="125, 0">
                <BezierSegment Point1="60, -10"
                               Point2="60, 60"
                               Point3="125, 50" />
                <BezierSegment Point1="60, 40"
                               Point2="60, 110"
                               Point3="125, 100" />
            </PathFigure>

            <!-- L -->
            <PathFigure StartPoint="150, 0">
                <LineSegment Point="150, 100" />
                <LineSegment Point="200, 100" />
            </PathFigure>

            <!-- L -->
            <PathFigure StartPoint="225, 0">
                <LineSegment Point="225, 100" />
                <LineSegment Point="275, 100" />
            </PathFigure>

            <!-- O -->
            <PathFigure StartPoint="300, 50">
                <ArcSegment Size="25, 50"
                            Point="300, 49.9"
                            IsLargeArc="True" />
            </PathFigure>
        </PathGeometry>
    </Path.Data>
</Path>
```

In this example, the word "Hello" is drawn using a combination of <xref:Microsoft.Maui.Controls.Shapes.LineSegment> and <xref:Microsoft.Maui.Controls.Shapes.BezierSegment> objects, along with a single <xref:Microsoft.Maui.Controls.Shapes.ArcSegment> object:

:::image type="content" source="media/geometry/multiple-pathfigures.png" alt-text="Multiple PathFigure objects.":::

## Composite geometries

Composite geometry objects can be created using a <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup>. The <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> class creates a composite geometry from one or more <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects. Any number of <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects can be added to a <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup>.

The <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> class defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup.Children>, of type <xref:Microsoft.Maui.Controls.Shapes.GeometryCollection>, which specifies the objects that define the <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup>. A <xref:Microsoft.Maui.Controls.Shapes.GeometryCollection> is an `ObservableCollection` of <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects.
- <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup.FillRule>, of type <xref:Microsoft.Maui.Controls.Shapes.FillRule>, which specifies how the intersecting areas in the <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> are combined. The default value of this property is `FillRule.EvenOdd`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The `Children` property is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> class, and so does not need to be explicitly set from XAML.

For more information about the <xref:Microsoft.Maui.Controls.Shapes.FillRule> enumeration, see [Fill rules](fillrules.md).

To draw a composite geometry, set the required <xref:Microsoft.Maui.Controls.Shapes.Geometry> objects as the children of a <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup>, and display them with a <xref:Microsoft.Maui.Controls.Shapes.Path> object. The following XAML shows an example of this:

```xaml
<Path Stroke="Green"
      StrokeThickness="2"
      Fill="Orange">
    <Path.Data>
        <GeometryGroup>
            <EllipseGeometry RadiusX="100"
                             RadiusY="100"
                             Center="150,150" />
            <EllipseGeometry RadiusX="100"
                             RadiusY="100"
                             Center="250,150" />
            <EllipseGeometry RadiusX="100"
                             RadiusY="100"
                             Center="150,250" />
            <EllipseGeometry RadiusX="100"
                             RadiusY="100"
                             Center="250,250" />
        </GeometryGroup>
    </Path.Data>
</Path>
```

In this example, four <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> objects with identical x-radius and y-radius coordinates, but with different center coordinates, are combined. This creates four overlapping circles, whose interiors are filled orange due to the default <xref:Microsoft.Maui.Controls.Shapes.FillRule.EvenOdd> fill rule:

:::image type="content" source="media/geometry/geometrygroup.png" alt-text="Line graphic shows four overlapping circles with regions filled.":::

### RoundRectangleGeometry

A round rectangle geometry represents the geometry of a rectangle, or square, with rounded corners, and is defined by a corner radius and a `Rect` structure that specifies its relative position and its height and width.

The <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry> class, which derives from the <xref:Microsoft.Maui.Controls.Shapes.GeometryGroup> class, defines the following properties:

- <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry.CornerRadius>, of type `CornerRadius`, which is the corner radius of the geometry.
- <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry.Rect>, of type `Rect`, which represents the dimensions of the rectangle.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> The fill rule used by the <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry> is `FillRule.Nonzero`. For more information about fill rules, see [Fill rules](fillrules.md).

The following example shows how to create and render a <xref:Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry> in a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Fill="Blue"
      Stroke="Red">
    <Path.Data>
        <RoundRectangleGeometry CornerRadius="5"
                                Rect="10,10,150,100" />
    </Path.Data>
</Path>
```

The position and dimensions of the rectangle are defined by a `Rect` structure. In this example, the position is (10,10), the width is 150, and the height is 100 device-independent units. In addition, the rectangle corners are rounded with a radius of 5 device-independent units.

## Clip with a Geometry

The <xref:Microsoft.Maui.Controls.VisualElement> class has a <xref:Microsoft.Maui.Controls.VisualElement.Clip> property, of type <xref:Microsoft.Maui.Controls.Shapes.Geometry>, that defines the outline of the contents of an element. When the <xref:Microsoft.Maui.Controls.VisualElement.Clip> property is set to a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object, only the area that is within the region of the <xref:Microsoft.Maui.Controls.Shapes.Geometry> will be visible.

The following example shows how to use a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object as the clip region for an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="monkeyface.png">
    <Image.Clip>
        <EllipseGeometry RadiusX="100"
                         RadiusY="100"
                         Center="180,180" />
    </Image.Clip>
</Image>
```

In this example, an <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> with <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.RadiusX> and <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.RadiusY> values of 100, and a <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry.Center> value of (180,180) is set to the <xref:Microsoft.Maui.Controls.VisualElement.Clip> property of an <xref:Microsoft.Maui.Controls.Image>. Only the part of the image that is within the area of the ellipse will be displayed:

:::image type="content" source="media/geometry/clip-ellipsegeometry.png" alt-text="Clip an Image with an EllipseGeometry.":::

> [!NOTE]
> Simple geometries, path geometries, and composite geometries can all be used to clip <xref:Microsoft.Maui.Controls.VisualElement> objects.

## Other features

The <xref:Microsoft.Maui.Controls.Shapes.GeometryHelper> class provides the following helper methods:

- <xref:Microsoft.Maui.Controls.Shapes.GeometryHelper.FlattenGeometry%2A>, which flattens a <xref:Microsoft.Maui.Controls.Shapes.Geometry> into a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry>.
- <xref:Microsoft.Maui.Controls.Shapes.GeometryHelper.FlattenCubicBezier%2A>, which flattens a cubic Bezier curve into a `List<Point>` collection.
- <xref:Microsoft.Maui.Controls.Shapes.GeometryHelper.FlattenQuadraticBezier%2A>, which flattens a quadratic Bezier curve into a `List<Point>` collection.
- <xref:Microsoft.Maui.Controls.Shapes.GeometryHelper.FlattenArc%2A>, which flattens an elliptical arc into a `List<Point>` collection.
