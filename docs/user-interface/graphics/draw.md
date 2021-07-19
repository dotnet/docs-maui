---
title: ".NET MAUI Graphics: Draw objects"
description: "The .NET MAUI 2d graphics library enables you to draw objects on a drawing canvas."
ms.date: 07/16/2021
---

# .NET MAUI Graphics: Draw objects

The .NET Multi-platform App UI (MAUI) 2D graphics library, in the `Microsoft.Maui.Graphics` namespace, enables you to draw objects on a drawing canvas. This library exposes the drawing canvas as an `ICanvas` object.

The .NET MAUI `GraphicsView` control exposes an `ICanvas` object, on which properties can be set and methods invoked to draw objects. For more information about the `GraphicsView`, see [.NET MAUI GraphicsView](~/user-interface/controls/graphicsview.md).

## Draw a line

Lines can be drawn on an `ICanvas` using the `DrawLine` method, which requires four `float` arguments that represent the start and end points of the line.

The following example shows how to draw a line:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 6;
canvas.DrawLine(10, 50, 90, 100);
```

In this example, a red diagonal line is drawn from (10,50) to (90,100):

:::image type="content" source="draw-images/line.png" alt-text="Screenshot of a red line.":::

> [!NOTE]
> There's also a `DrawLine` overload that takes two `PointF` arguments.

The following example shows how to draw a dashed line:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.StrokeDashPattern = new float[] { 2, 2 };
canvas.DrawLine(10, 50, 90, 100);
```

In this example, a red dashed diagonal line is drawn from (10,50) to (90,100):

:::image type="content" source="draw-images/dashed-line.png" alt-text="Screenshot of a dashed red line.":::

For more information about dashed lines, see [Draw dashed objects](#draw-dashed-objects).

## Draw an ellipse

Ellipses and circles can be drawn on an `ICanvas` using the `DrawEllipse` method, which requires `x`, `y`, `width`, and `height` arguments.

The following example shows how to draw an ellipse:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.DrawEllipse(200, 50, 150, 50);
```

In this example, a red ellipse with dimensions 150x50 (device-independent units) is drawn at (200,50):

:::image type="content" source="draw-images/ellipse.png" alt-text="Screenshot of a red ellipse.":::

To draw a circle, make the `width` and `height` arguments to the `DrawEllipse` method equal:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.DrawEllipse(200, 50, 150, 150);
```

In this example, a red circle with dimensions 150x150 (device-independent units) is drawn at (200,50):

:::image type="content" source="draw-images/circle.png" alt-text="Screenshot of a red circle.":::

For information about drawing a dashed ellipse, see [Draw dashed objects](#draw-dashed-objects).

A filled ellipse can be drawn with the `FillEllipse` method, which also requires `x`, `y`, `width`, and `height` arguments:

```csharp
canvas.FillColor = Colors.Red;
canvas.FillEllipse(200, 50, 150, 50);
```

In this example, a red filled ellipse with dimensions 150x50 (device-independent units) is drawn at (200,50):

:::image type="content" source="draw-images/filled-ellipse.png" alt-text="Screenshot of a red filled ellipse.":::

The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillEllipse` method.

> [!NOTE]
> There are `DrawEllipse` and `FillEllipse` overloads that take `Rectangle` and `RectangleF` arguments. In addition, there are also `DrawCircle` and `FillCircle` overloads.

## Draw a rectangle

Rectangles and squares can be drawn on an `ICanvas` using the `DrawRectangle` method, which requires `x`, `y`, `width`, and `height` arguments.

The following example shows how to draw a rectangle:

```csharp
canvas.StrokeColor = Colors.DarkBlue;
canvas.StrokeSize = 4;
canvas.DrawRectangle(50, 100, 100, 50);
```

In this example, a dark blue rectangle with dimensions 100x50 (device-independent units) is drawn at (50,100):

:::image type="content" source="draw-images/rectangle.png" alt-text="Screenshot of a dark blue rectangle.":::

To draw a square, make the `width` and `height` arguments to the `DrawRectangle` method equal:

```csharp
canvas.StrokeColor = Colors.DarkBlue;
canvas.StrokeSize = 4;
canvas.DrawRectangle(50, 100, 100, 100);
```

In this example, a dark blue square with dimensions 100x100 (device-independent units) is drawn at (50,100):

:::image type="content" source="draw-images/square.png" alt-text="Screenshot of a dark blue square.":::

For information about drawing a dashed rectangle, see [Draw dashed objects](#draw-dashed-objects).

A filled rectangle can be drawn with the `FillRectangle` method, which also requires `x`, `y`, `width`, and `height` arguments:

```csharp
canvas.FillColor = Colors.DarkBlue;
canvas.FillRectangle(50, 100, 100, 50);
```

In this example, a dark blue filled rectangle with dimensions 100x50 (device-independent units) is drawn at (50,100):

:::image type="content" source="draw-images/filled-rectangle.png" alt-text="Screenshot of a dark blue filled rectangle.":::

The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillRectangle` method.

> [!NOTE]
> There are `DrawRectangle` and `FillRectangle` overloads that take `Rectangle` and `RectangleF` arguments.

## Draw a rounded rectangle

Rounded rectangles and squares can be drawn on an `ICanvas` using the `DrawRoundedRectangle` method, which requires `x`, `y`, `width`, `height`, and `cornerRadius` arguments. The `cornerRadius` argument specifies the radius used to round the corners of the rectangle.

The following example shows how to draw a rounded rectangle:

```csharp
canvas.StrokeColor = Colors.Green;
canvas.StrokeSize = 4;
canvas.DrawRoundedRectangle(50, 100, 100, 50, 12);
```

In this example, a green rectangle with rounded corners and dimensions 100x50 (device-independent units) is drawn at (50,100):

:::image type="content" source="draw-images/rounded-rectangle.png" alt-text="Screenshot of a green rounded rectangle.":::

For information about drawing a dashed rounded rectangle, see [Draw dashed objects](#draw-dashed-objects).

A filled rounded rectangle can be drawn with the `FillRoundedRectangle` method, which also requires requires `x`, `y`, `width`, `height`, and `cornerRadius` arguments.

```csharp
canvas.FillColor = Colors.Green;
canvas.FillRoundedRectangle(50, 100, 100, 50, 12);
```

In this example, a green filled rectangle with rounded corners and dimensions 100x50 (device-independent units) is drawn at (50,100):

:::image type="content" source="draw-images/filled-rounded-rectangle.png" alt-text="Screenshot of a green filled rounded rectangle.":::

The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillRoundedRectangle` method.

> [!NOTE]
> There are `DrawRoundedRectangle` and `FillRoundedRectangle` overloads that take `Rectangle` and `RectangleF` arguments, and overloads that enable the radius of each corner to be separately specified.

## Draw an arc

Arcs can be drawn on an `ICanvas` using the `DrawArc` method, which requires `x`, `y`, `width`, `height`, `startAngle`, `endAngle`, `clockwise`, and `closed` arguments. The `startAngle` argument specifies the angle from the x-axis to the starting point of the arc. The `endAngle` argument specifies the angle from from the x-axis to the end point of the arc. The `clockwise` argument specifies the direction in which the arc is drawn, and the `closed` argument specifies whether the end point of the arc will be connected to the start point.

The following example shows how to draw an arc:

```csharp
canvas.StrokeColor = Colors.Teal;
canvas.StrokeSize = 4;
canvas.DrawArc(50, 50, 100, 100, 0, 180, true, false);
```

In this example, a teal arc of dimensions 100x100 (device-independent units) is drawn at (50,50). The arc is drawn in a clockwise direction from 0 to 180 degrees, and isn't closed:

:::image type="content" source="draw-images/arc.png" alt-text="Screenshot of a teal arc.":::

For information about drawing a dashed arc, see [Draw dashed objects](#draw-dashed-objects).

A filled arc can be drawn with the `FillArc` method, which requires `x`, `y`, `width`, `height`, `startAngle`, `endAngle`, and `clockwise` arguments:

```csharp
canvas.FillColor = Colors.Teal;
canvas.FillArc(50, 50, 100, 100, 0, 180, true);
```

In this example, a filled teal arc of dimensions 100x100 (device-independent units) is drawn at (50,50). The arc is drawn in a clockwise direction from 0 to 180 degrees, and is closed automatically:

:::image type="content" source="draw-images/filled-arc.png" alt-text="Screenshot of a filled teal arc.":::

The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillArc` method.

> [!NOTE]
> There are `DrawArc` and `FillArc` overloads that take `Rectangle` and `RectangleF` arguments.

## Draw a path

Paths are used to draw curves and complex shapes and can be drawn on an `ICanvas` using the `DrawPath` method, which requires a `PathF` argument. The `PathF` class a series of methods that enable the shape of the path to be manipulated,

The following example shows how to draw a path:

```csharp
PathF path = new PathF();
path.MoveTo(40, 10);
path.LineTo(70, 80);
path.LineTo(10, 50);
path.Close();
canvas.StrokeColor = Colors.Green;
canvas.StrokeSize = 6;
canvas.DrawPath(path);
```

In this example, a closed green triangle is drawn:

:::image type="content" source="draw-images/path.png" alt-text="Screenshot of a closed green triangle.":::

A filled path can be drawn with the `FillPath`, which also requires a `PathF` argument:

```csharp
PathF path = new PathF();
path.MoveTo(40, 10);
path.LineTo(70, 80);
path.LineTo(10, 50);
canvas.FillColor = Colors.SlateBlue;
canvas.FillPath(path);
```

In this example, a filled slate blue triangle is drawn:

:::image type="content" source="draw-images/filled-path.png" alt-text="Screenshot of a filled slate blue triangle.":::

The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillPath` method.

> [!NOTE]
> There are `DrawPath` and `FillPath` overloads that take `PathF` arguments.

## Draw an image

## Draw a string

## Draw text

## Draw a shadow

Objects drawn on an `ICanvas` can have a shadow applied using the `SetShadow` method, which takes `SizeF`, `float`, and `Color` arguments:

- `offset`, of type `SizeF`, specifies an offset for the shadow, which represents the position of a light source that creates the shadow.
- `blur`, of type `float`, represents the amount of blur to apply to the shadow.
- `color`, of type `Color`, defines the color of the shadow.

The following examples show how to add shadows to objects:

```csharp
canvas.StrokeColor = Colors.Black;
canvas.StrokeSize = 4;
canvas.SetShadow(new SizeF(10, 10), 2, Colors.Grey);
canvas.DrawRectangle(100, 50, 90, 100);

canvas.SetShadow(new SizeF(10, 10), 4, Colors.Grey);
canvas.DrawEllipse(200, 50, 90, 100);

canvas.SetShadow(new SizeF(10, 10), 6, Colors.Grey);
canvas.DrawRoundedRectangle(300, 50, 90, 100, 25);
```

In these examples, shadows whose light sources are in identical sources are added to different objects, with differing amounts of blur:

:::image type="content" source="draw-images/shadow.png" alt-text="Screenshot of a objects drawn with shadows.":::

## Draw dashed objects

`ICanvas` objects have a `StrokeDashPattern` property, of type `float[]`. This property is an array of `float` values that indicate the pattern of dashes and gaps that are to be used when during an object. Each `float` in the array specifies the length of a dash or gap. The first item in the array specifies the length of a dash, while the second item in the array specifies the length of a gap. Therefore, `float` values with an even index value specify dashes, while `float` values with an odd index value specify gaps.

The following example shows how to draw a dashed square, using a regular dash::

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.StrokeDashPattern = new float[] { 2, 2 };
canvas.DrawRectangle(100, 50, 90, 100);
```

In this example, a square with a regular dashed stroke is drawn:

:::image type="content" source="draw-images/dashed-square1.png" alt-text="Screenshot of a regular dashed square.":::

The following example shows how to draw a dashed square, using an irregular dash:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.StrokeDashPattern = new float[] { 4, 4, 1, 4 };
canvas.DrawRectangle(100, 50, 90, 100);
```

In this example, a square with an irregular dashed stroke is drawn:

:::image type="content" source="draw-images/dashed-square2.png" alt-text="Screenshot of an irregular dashed square.":::

## Control line ends

A line has three parts: start cap, line body, and end cap. The start and end caps describe the shape at the start and end of a line.

`ICanvas` objects have a `StrokeLineCap` property, of type `LineCap`, that describes the shape at the start and end of a line. The `LineCap` enumeration defines the following members:

- `Butt`, which represents a line with a square end, drawn to extend to the exact endpoint of the line. This is the default value of the `StrokeLineCap` property.
- `Round`, which represents a line with a rounded end.
- `Square`, which represents a line with a square end, drawn to extend beyond the endpoint to a distance equal to half the line width.

The following example shows how to set the `StrokeLineCap` property:


```csharp
canvas.StrokeSize = 10;
canvas.StrokeColor = Colors.Red;
canvas.StrokeLineCap = LineCap.Round;
canvas.DrawLine(100, 160, 300, 160);
```

In this example, the red line is rounded at the start and end of the line:

:::image type="content" source="draw-images/linecap.png" alt-text="Screenshot of three lines with different line caps.":::

## Control line joins

`ICanvas` objects have a `StrokeLineJoin` property, of type `LineJoin`, that specifies the type of join that is used at the vertices of an object. The `LineJoin` enumeration defines the following members:

- `Miter`, which represents angular vertices that produce a sharp or clipped corner. This is the default value of the `StrokeLineJoin` property.
- `Round`, which represents rounded vertices that produce a circular arc at the corner.
- `Bevel`, which represents beveled vertices that produce a diagonal corner.

> [!NOTE]
> When the `StrokeLineJoin` property is set to `Miter`, the `MiterLimit` property can be set to a `float` to limit the miter length of line joins in the object.

The following example shows how to set the `StrokeLineJoin` property:

```csharp
canvas.StrokeSize = 20;
canvas.StrokeColor = Colors.Blue;
canvas.StrokeLineJoin = LineJoin.Round;
//canvas.MiterLimit = 2;
PathF path = new PathF();
path.MoveTo(20, 20);
path.LineTo(250, 50);
path.LineTo(20, 120);
canvas.DrawPath(path);
```

In this example, the blue `PathF` object has rounded joins at its vertices:

:::image type="content" source="draw-images/linejoin.png" alt-text="Screenshot of the effect of the three different LineJoin enumeration members.":::
