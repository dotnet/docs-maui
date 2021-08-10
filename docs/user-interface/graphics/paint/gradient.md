---
title: "Paint gradients"
description: "The .NET MAUI graphics library includes a GradientPaint class, which is an abstract class that describes a gradient, composed of gradient stops."
ms.date: 07/15/2021
---

# Paint gradients

<!-- Sample link goes here -->

The .NET Multi-platform App UI (.NET MAUI) graphics library includes a `GradientPaint` class that's derived from the `Paint` class, and is an abstract base class that describes a gradient, which is composed of gradient steps. A `GradientPaint` paints a graphical object with multiple colors that blend into each other along an axis. Classes that derive from `GradientPaint` describe different ways of interpreting gradients stops, and the .NET MAUI graphics library provides the following gradient paints:

- `LinearGradientPaint`, which paints an object with a linear gradient. For more information, see [Paint a linear gradient](lineargradient.md).
- `RadialGradientPaint`, which paints an object with a radial gradient. For more information, see [Paint a radial gradient](radialgradient.md).

The `GradientPaint` class defines the `GradientStops` property, of type `GradientStop`, which represents the brush's gradient stops, each of which specifies a color and an offset along the gradient axis.

## Gradient stops

Gradient stops are the building blocks of a gradient, and specify the colors in the gradient and their location along the gradient axis. Gradient stops are specified using `GradientStop` objects.

The `GradientStop` class defines the following properties:

- `Color`, of type `Color`, which represents the color of the gradient stop.
- `Offset`, of type `float`, which represents the location of the gradient stop within the gradient vector. Valid values are in the range 0.0-1.0. The closer this value is to 0, the closer the color is to the start of the gradient. Similarly, the closer this value is to 1, the closer the color is to the end of the gradient.

> [!IMPORTANT]
> The coordinate system used by gradients is relative to a bounding box for the graphical object. 0 indicates 0 percent of the bounding box, and 1 indicates 100 percent of the bounding box. Therefore, (0.5,0.5) describes a point in the middle of the bounding box, and (1,1) describes a point at the bottom right of the bounding box.

Gradient stops can be added to a `GradientPoint` object with the `AddOffset` method.

The following example creates a diagonal `LinearGradientPaint` with four colors:

```csharp
LinearGradientPaint linearGradientPaint = new LinearGradientPaint
{
    StartColor = Colors.Yellow,
    EndColor = Colors.Green,
    StartPoint = new Point(0, 0),
    EndPoint = new Point(1, 1)
};

linearGradientPaint.AddOffset(0.25f, Colors.Red);
linearGradientPaint.AddOffset(0.75f, Colors.Blue);

RectangleF linearRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(linearGradientPaint, linearRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(linearRectangle, 12);                                                     
```

The color of each point between gradient stops is interpolated as a combination of the color specified by the two bounding gradient stops. The following diagram shows the gradient stops from the previous example:

:::image type="content" source="gradient-images/gradient-stops.png" alt-text="Screenshot of a rounded rectangle, filled with a diagonal LinearGradientPaint." border="false":::

In this diagram, the circles mark the position of gradient stops, and the dashed line shows the gradient axis. The first gradient stop specifies the color yellow at an offset of 0.0. The second gradient stop specifies the color red at an offset of 0.25. The points between these two gradient stops gradually change from yellow to red as you move from left to right along the gradient axis. The third gradient stop specifies the color blue at an offset of 0.75. The points between the second and third gradient stops gradually change from red to blue. The fourth gradient stop specifies the color lime green at offset of 1.0. The points between the third and fourth gradient stops gradually change from blue to lime green.
