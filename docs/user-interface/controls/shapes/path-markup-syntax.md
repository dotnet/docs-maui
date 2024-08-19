---
title: ".NET MAUI Shapes: Path markup syntax"
description: ".NET MAUI Path markup syntax enables you to compactly specify path geometries in XAML."
ms.date: 08/30/2024
---

# Path markup syntax

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

.NET Multi-platform App UI (.NET MAUI) path markup syntax enables you to compactly specify path geometries in XAML.

Path markup syntax is specified as a string value to the `Path.Data` property:

```xaml
<Path Stroke="Black"
      Data="M13.908992,16.207977 L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983Z" />
```

Path markup syntax is composed of an optional `FillRule` value, and one or more figure descriptions. This syntax can be expressed as: `<Path Data="`*[fillRule]* *figureDescription* *[figureDescription]* * `" ... />`

In this syntax:

- *fillRule* is an optional <xref:Microsoft.Maui.Controls.Shapes.FillRule> that specifies whether the geometry should use the <xref:Microsoft.Maui.Controls.Shapes.FillRule.EvenOdd> or <xref:Microsoft.Maui.Controls.Shapes.FillRule.Nonzero> fill rule. `F0` is used to specify the <xref:Microsoft.Maui.Controls.Shapes.FillRule.EvenOdd> fill rule, while `F1` is used to specify the <xref:Microsoft.Maui.Controls.Shapes.FillRule.Nonzero> fill rule. For more information about fill rules, see [Fill rules](fillrules.md).
- *figureDescription* represents a figure composed of a move command, draw commands, and an optional close command. A move command specifies the start point of the figure. Draw commands describe the figure's contents, and the optional close command closes the figure.

In the example above, the path markup syntax specifies a start point using the move command (`M`), a series of straight lines using the line command (`L`), and closes the path with the close command (`Z`).

In path markup syntax, spaces are not required before or after commands. In addition, two numbers don't have to be separated by a comma or white space, but this can only be achieved when the string is unambiguous.

> [!TIP]
> Path markup syntax is compatible with Scalable Vector Graphics (SVG) image path definitions, and so it can be useful for porting graphics from SVG format.

While path markup syntax is intended for consumption in XAML, it can be converted to a <xref:Microsoft.Maui.Controls.Shapes.Geometry> object in code by invoking the `ConvertFromInvariantString` method in the <xref:Microsoft.Maui.Controls.Shapes.PathGeometryConverter> class:

```csharp
Geometry pathData = (Geometry)new PathGeometryConverter().ConvertFromInvariantString("M13.908992,16.207977 L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983Z");
```

## Move command

The move command specifies the start point of a new figure. The syntax for this command is: `M` *startPoint* or `m` *startPoint*.

In this syntax, *startPoint* is a `Point` structure that specifies the start point of a new figure. If you list multiple points after the move command, a line is drawn to those points.

`M 10,10` is an example of a valid move command.

## Draw commands

A draw command can consist of several shape commands. The following draw commands are available:

- Line (`L` or `l`).
- Horizontal line (`H` or `h`).
- Vertical line (`V` or `v`).
- Elliptical arc (`A` or `a`).
- Cubic Bezier curve (`C` or `c`).
- Quadratic Bezier curve (`Q` or `q`).
- Smooth cubic Bezier curve (`S` or `s`).
- Smooth quadratic Bezier curve (`T` or `t`).

Each draw command is specified with a case-insensitive letter. When sequentially entering more than one command of the same type, you can omit the duplicate command entry. For example `L 100,200 300,400` is equivalent to `L 100,200 L 300,400`.

### Line command

The line command creates a straight line between the current point and the specified end point. The syntax for this command is: `L` *endPoint* or `l` *endPoint*.

In this syntax, *endPoint* is a `Point` that represents the end point of the line.

`L 20,30` and `L 20 30` are examples of valid line commands.

For information about creating a straight line as a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> object, see [Create a LineSegment](geometries.md#create-a-linesegment).

### Horizontal line command

The horizontal line command creates a horizontal line between the current point and the specified x-coordinate. The syntax for this command is: `H` *x* or `h` *x*.

In this syntax, *x* is a `double` that represents the x-coordinate of the end point of the line.

`H 90` is an example of a valid horizontal line command.

### Vertical line command

The vertical line command creates a vertical line between the current point and the specified y-coordinate. The syntax for this command is: `V` *y* or `v` *y*.

In this syntax, *y* is a `double` that represents the y-coordinate of the end point of the line.

`V 90` is an example of a valid vertical line command.

### Elliptical arc command

The elliptical arc command creates an elliptical arc between the current point and the specified end point. The syntax for this command is: `A` *size* *rotationAngle* *isLargeArcFlag* *sweepDirectionFlag* *endPoint* or `a` *size* *rotationAngle* *isLargeArcFlag* *sweepDirectionFlag* *endPoint*.

In this syntax:

- `size` is a `Size` that represents the x- and y-radius of the arc.
- `rotationAngle` is a `double` that represents the rotation of the ellipse, in degrees.
- `isLargeArcFlag` should be set to 1 if the angle of the arc should be 180 degrees or greater, otherwise set it to 0.
- `sweepDirectionFlag` should be set to 1 if the arc is drawn in a positive-angle direction, otherwise set it to 0.
- `endPoint` is a `Point` to which the arc is drawn.

`A 150,150 0 1,0 150,-150` is an example of a valid elliptical arc command.

For information about creating an elliptical arc as a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> object, see [Create an ArcSegment](geometries.md#create-an-arcsegment).

### Cubic Bezier curve command

The cubic Bezier curve command creates a cubic Bezier curve between the current point and the specified end point by using the two specified control points. The syntax for this command is: `C` *controlPoint1* *controlPoint2* *endPoint* or `c` *controlPoint1* *controlPoint2* *endPoint*.

In this syntax:

- *controlPoint1* is a `Point` that represents the first control point of the curve, which determines the starting tangent of the curve.
- *controlPoint2* is a `Point` that represents the second control point of the curve, which determines the ending tangent of the curve.
- *endPoint* is a `Point` that represents the point to which the curve is drawn.

`C 100,200 200,400 300,200` is an example of a valid cubic Bezier curve command.

For information about creating a cubic Bezier curve as a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> object, see [Create a BezierSegment](geometries.md#create-a-beziersegment).

### Quadratic Bezier curve command

The quadratic Bezier curve command creates a quadratic Bezier curve between the current point and the specified end point by using the specified control point. The syntax for this command is: `Q` *controlPoint* *endPoint* or `q` *controlPoint* *endPoint*.

In this syntax:

- *controlPoint* is a `Point` that represents the control point of the curve, which determines the starting and ending tangents of the curve.
- *endPoint* is a `Point` that represents the point to which the curve is drawn.

`Q 100,200 300,200` is an example of a valid quadratic Bezier curve command.

For information about creating a quadratic Bezier curve as a <xref:Microsoft.Maui.Controls.Shapes.PathGeometry> object, see [Create a QuadraticBezierSegment](geometries.md#create-a-quadraticbeziersegment).

### Smooth cubic Bezier curve command

The smooth cubic Bezier curve command creates a cubic Bezier curve between the current point and the specified end point by using the specified control point. The syntax for this command is: `S` *controlPoint2* *endPoint* or `s`  *controlPoint2* *endPoint*.

In this syntax:

- *controlPoint2* is a `Point` that represents the second control point of the curve, which determines the ending tangent of the curve.
- *endPoint* is a `Point` that represents the point to which the curve is drawn.

The first control point is assumed to be the reflection of the second control point of the previous command, relative to the current point. If there is no previous command, or the previous command was not a cubic Bezier curve command or a smooth cubic Bezier curve command, the first control point is assumed to be coincident with the current point.

`S 100,200 200,300` is an example of a valid smooth cubic Bezier curve command.

### Smooth quadratic Bezier curve command

The smooth quadratic Bezier curve command creates a quadratic Bezier curve between the current point and the specified end point by using a control point. The syntax for this command is: `T` *endPoint* or `t` *endPoint*.

In this syntax, *endPoint* is a `Point` that represents the point to which the curve is drawn.

The control point is assumed to be the reflection of the control point of the previous command relative to the current point. If there is no previous command or if the previous command was not a quadratic Bezier curve or a smooth quadratic Bezier curve command, the control point is assumed to be coincident with the current point.

`T 100,30` is an example of a valid smooth quadratic cubic Bezier curve command.

## Close command

The close command ends the current figure and creates a line that connects the current point to the starting point of the figure. Therefore, this command creates a line-join between the last segment and the first segment of the figure.

The syntax for the close command is: `Z` or `z`.

## Additional values

Instead of a standard numerical value, you can also use the following case-sensitive special values:

- `Infinity` represents `double.PositiveInfinity`.
- `-Infinity` represents `double.NegativeInfinity`.
- `NaN` represents `double.NaN`.

In addition, you may also use case-insensitive scientific notation. Therefore, `+1.e17` is a valid value.
