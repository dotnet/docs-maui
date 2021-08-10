---
title: "Paint a solid color"
description: "The .NET MAUI graphics library includes a SolidPaint class, that paints a graphical object with a solid color."
ms.date: 07/15/2021
---

# Paint a solid color

<!-- Sample link goes here -->

The .NET Multi-platform App UI (.NET MAUI) graphics library includes a `SolidPaint` class that's derived from the `Paint` class, which s used to paint a graphical object with a solid color.

The `SolidPaint` class defines a `Color` property, of type `Color`, which represents the color of the paint. The class also has an `IsTransparent` property that returns a `bool` that represents whether the color has an alpha value of less than 1.

## Create a SolidPaint object

The color of a `SolidPaint` object is typically specified through its constructor, using a `Color` argument:

```csharp
SolidPaint solidPaint = new SolidPaint(Colors.Silver);

RectangleF solidRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```

The `SolidPaint` object is specified as the first argument to the `SetFillPaint` method. Therefore, a filled rounded rectangle is painted with a silver `SolidPaint` object:

:::image type="content" source="solidcolor-images/solidpaint.png" alt-text="Screenshot of a rounded rectangle, filled with a silver SolidPaint object.":::

Alternatively, the color can be specified with the `Color` property:

```csharp
SolidPaint solidPaint = new SolidPaint
{
    Color = Colors.Silver
};

RectangleF solidRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```
