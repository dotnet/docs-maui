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

For more information about dashed lines, see [Draw dashed shapes](#draw-dashed-shapes).

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

For information about drawing a dashed ellipse, see [Draw dashed shapes](#draw-dashed-shapes).

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

For information about drawing a dashed rectangle, see [Draw dashed shapes](#draw-dashed-shapes).

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

For information about drawing a dashed rounded rectangle, see [Draw dashed shapes](#draw-dashed-shapes).

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

For information about drawing a dashed arc, see [Draw dashed shapes](#draw-dashed-shapes).

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

## Draw a shadow

## Draw dashed shapes

## Control line ends

## Control line joins
