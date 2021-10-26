---
title: "Paint a radial gradient"
description: ".NET MAUI graphics includes a RadialGradientPaint class, that paints a graphical object with a radial gradient."
ms.date: 07/15/2021
---

# Paint a radial gradient

<!-- Sample link goes here -->

.NET Multi-platform App UI (.NET MAUI) graphics includes a `RadialGradientPaint` class that's derived from the `GradientPaint` class, which paints a graphical object with a radial gradient. A radial gradient blends two or more colors across a circle. `GradientStop` objects are used to specify the colors in the gradient and their positions. For more information about `GradientStop` objects, see [Paint gradients](gradient.md).

The `RadialGradientPaint` class defines the following properties:

- `Center`, of type `Point`, which represents the center point of the circle for the radial gradient. The class constructor initializes this property to (0.5,0.5).
- `Radius`, of type `double`, which represents the radius of the circle for the radial gradient. The class constructor initializes this property to 0.5.

## Create a RadialGradientPaint object

A radial gradient's gradient stops are positioned along a gradient axis defined by a circle. The gradient axis radiates from the center of the circle to its circumference. The position and size of the circle can be changed using the `Center` and `Radius` properties. The circle defines the end point of the gradient. Therefore, a gradient stop at 1.0 defines the color at the circle's circumference. A gradient stop at 0.0 defines the color at the center of the circle.

To create a radial gradient, create a `RadialGradientPaint` object and set its `StartColor` and `EndColor` properties. Then, set its `Center` and `Radius` properties.

The following example shows how to create a diagonal `LinearGradientPaint`:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue
    // Center is already (0.5,0.5)
    // Radius is already 0.5
};

RectangleF radialRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the center of the rectangle:

:::image type="content" source="media/radialgradient/center.png" alt-text="Screenshot of a rounded rectangle, filled with a centered radial gradient.":::

The following example moves the center of the radial gradient to the top-left corner of the rectangle:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue,
    Center = new Point(0.0, 0.0)
    // Radius is already 0.5
};

RectangleF radialRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the top-left of the rectangle:

:::image type="content" source="media/radialgradient/top-left.png" alt-text="Screenshot of a rounded rectangle, filled with a top-left radial gradient.":::

The following example moves the center of the radial gradient to the bottom-right corner of the rectangle:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue,
    Center = new Point(1.0, 1.0)
    // Radius is already 0.5
};

RectangleF radialRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the bottom-right of the rectangle:

:::image type="content" source="media/radialgradient/bottom-right.png" alt-text="Screenshot of a rounded rectangle, filled with a bottom-right radial gradient.":::
