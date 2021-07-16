---
title: ".NET MAUI Graphics: Draw objects"
description: ".NET MAUI "
ms.date: 07/16/2021
---

# .NET MAUI Graphics: Draw objects

The .NET Multi-platform App UI (MAUI) 2D graphics library, in the `Microsoft.Maui.Graphics` namespace, enables you to draw objects to a drawing canvas. This library exposes the drawing canvas as an `ICanvas` object.

The .NET MAUI `GraphicsView` control exposes an `ICanvas` object, on which properties can be set and methods invoked to draw objects. For more information about the `GraphicsView`, see [.NET MAUI GraphicsView](~/user-interface/controls/graphicsview.md).

## Draw a line

Lines can be drawn on an `ICanvas` using the `DrawLine` method, which takes four `float` arguments that represent the start and end points of the line.

The following example shows how to draw a line:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 6;
canvas.DrawLine(10, 50, 90, 100);
```

In this example, a red diagonal line is drawn from (10,50) to (90,100):

:::image type="content" source="draw-images/line.png" alt-text="Screenshot of a red line.":::

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

Ellipses and circles can be drawn on an `ICanvas` using the `DrawEllipse` method, which takes `x`, `y`, `width`, and `height` arguments.

The following example shows how to draw an ellipse:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.DrawEllipse(200, 50, 150, 50);
```

In this example, a red ellipse with dimensions 150x50 (device-independent) is drawn at (200,50):

:::image type="content" source="draw-images/ellipse.png" alt-text="Screenshot of a red ellipse.":::

To draw a circle, make the `width` and `height` arguments to the `DrawEllipse` method equal:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.DrawEllipse(200, 50, 150, 150);
```

In this example, a red ellipse with dimensions 150x150 (device-independent) is drawn at (200,50):

:::image type="content" source="draw-images/circle.png" alt-text="Screenshot of a red circle.":::

A filled ellipse can be drawn with the `FillEllipse` method, which also takes the same four `float` arguments:

```csharp
canvas.FillColor = Colors.Red;
canvas.FillEllipse(200, 50, 150, 50);
```

In this example, a red filled ellipse with dimensions 150x50 (device-independent) is drawn at (200,50):

:::image type="content" source="draw-images/filled-ellipse.png" alt-text="Screenshot of a red filled ellipse.":::

> [!NOTE]
> The `FillColor` property of the `ICanvas` object must be set to a `Color` before invoking the `FillEllipse` method.

For information about drawing a dashed ellipse, see [Draw dashed shapes](#draw-dashed-shapes).

## Draw a rectangle

## Draw a rounded rectangle

## Draw an arc

## Draw a path

## Draw a shadow

## Draw dashed shapes

## Control line ends

## Control line joins
