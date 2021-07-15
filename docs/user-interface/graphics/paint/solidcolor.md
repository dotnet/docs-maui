---
title: ".NET MAUI Paint: Solid colors"
description: "The .NET MAUI SolidPaint class paints an area with a solid color."
ms.date: 07/15/2021
---

# .NET MAUI Paint: Solid colors

The .NET Multi-platform App UI (MAUI) `SolidPaint` class derives from the `Paint` class, and is used to paint an area with a solid color.

The `SolidPaint` class defines a `Color` property, of type `Color`, which represents the color of the paint. The class also has an `IsTransparent` method that returns a `bool` that represents whether the color has an alpha value of less than 1.

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
