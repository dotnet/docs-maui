---
title: ".NET MAUI Graphics: Pattern paint"
description: "The .NET MAUI graphics library includes a PatternPaint class, that paints a graphical object with a pattern."
ms.date: 07/22/2021
---

# .NET MAUI Graphics: Pattern paint

<!-- Sample link goes here -->

The .NET Multi-platform App UI (.NET MAUI) graphics library includes a `PatternPaint` class that's derived from the `Paint` class, which is used to paint a graphical object with a pattern.

The `PatternPaint` class defines a `Pattern` property, of type `IPattern`, which represents the pattern to paint. The class also has an `IsTransparent` property that returns a `bool` that represents whether the background or foreground color of the paint has an alpha value of less than 1.

## Create a PatternPaint object

To paint an area with a pattern, create the pattern and assign it to the `Pattern` property of a `PatternPaint` object.

The following example shows how to create a pattern and fill an object with it:

```csharp
IPattern pattern;

// Create a 10x10 template for the pattern
using (PictureCanvas picture = new PictureCanvas(0, 0, 10, 10))
{
    picture.StrokeColor = Colors.Silver;
    picture.DrawLine(0, 0, 10, 10);
    picture.DrawLine(0, 10, 10, 0);
    pattern = new PicturePattern(picture.Picture, 10, 10);
}

// Fill the rectangle with the 10x10 pattern
PatternPaint patternPaint = new PatternPaint
{
    Pattern = pattern
};
canvas.SetFillPaint(patternPaint, RectangleF.Zero);
canvas.FillRectangle(10, 10, 250, 250);
```

In this example, the pattern is a 10x10 area that contains a diagonal line from (0,0) to (10,10), and a diagonal line from (0,10) to (10,0). The `Pattern` property of the `PatternPaint` object is set to the pattern, and the `PatternPaint` object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="pattern-images/patternpaint.png" alt-text="Screenshot of a rectangle, filled with a silver pattern.":::

> [!NOTE]
> A `PatternPaint` object can also be created from a `PicturePattern` object by the `AsPaint` extension method.
