---
title: ".NET MAUI Graphics: Winding modes"
description: "The .NET MAUI graphics library enables you to specify the fill algorithm when filling or clipping a path."
ms.date: 07/28/2021
---

# .NET MAUI Graphics: Winding modes

<!-- Sample link goes here -->

The .NET Multi-platform App UI (MAUI) graphics library provides a `WindingMode` enumeration, which enables you to specify the fill algorithm to be used by the `FillPath` method. Contours in a path can overlap, and any enclosed area can potentially be filled, but you might not want to fill all the enclosed areas.

The `WindingMode` enumeration defines `NonZero` and `EvenOdd` members. Each member represents a different algorithm for determining whether a point is in the fill region of an enclosed area.

> [!NOTE]
> The `ClipPath` method has an overload that enables a `WindingMode` argument to be specified. By default, this argument is set to `WindingMode.NonZero`.

## NonZero

The `NonZero` winding mode draws a hypothetical ray from the point to infinity in any direction and then examines the places where a path contour crosses the ray. Starting with a count of zero, the count is incremented each time a contour crosses the ray from left to right and decremented each time a contour crosses the ray from right to left. After counting the crossings, if the result is zero then the area isn't filled. Otherwise, the area is filled.

The following example fills a five-pointed star using the `NonZero` winding mode:

```csharp
float radius = 0.45f * Math.Min(dirtyRect.Width, dirtyRect.Height);

PathF path = new PathF();
path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y - radius);

for (int i = 1; i < 5; i++)
{
    double angle = i * 4 * Math.PI / 5;
    path.LineTo(new PointF(radius * (float)Math.Sin(angle) + dirtyRect.Center.X, -radius * (float)Math.Cos(angle) + dirtyRect.Center.Y));
}
path.Close();

canvas.StrokeSize = 15;
canvas.StrokeLineJoin = LineJoin.Round;
canvas.StrokeColor = Colors.Red;
canvas.FillColor = Colors.Blue;
canvas.FillPath(path); // Overload automatically uses a NonZero winding mode
canvas.DrawPath(path);
```

In this example, the path is drawn twice. The `FillPath` method is used to fill the path with blue, while the `DrawPath` method outlines the path with a red stroke. The `FillPath` overload used omits the `WindingMode` argument, and instead automatically uses the `NonZero` winding mode. This results in all the enclosed areas of the path being filled:

:::image type="content" source="windingmodes-images/nonzero.png" alt-text="Screenshot of a five-pointed star, using the non-zero winding mode.":::

> [!NOTE]
> For many paths, the `NonZero` winding mode often fills all the enclosed areas of a path.

## EvenOdd

The `EvenOdd` winding mode draws a hypothetical ray from the point to infinity in any direction and counts the number of path contours that the ray crosses. If this number is odd, then the area is filled. Otherwise, the area isn't filled.

The following example fills a five-pointed star using the `EvenOdd` winding mode:

```csharp
float radius = 0.45f * Math.Min(dirtyRect.Width, dirtyRect.Height);

PathF path = new PathF();
path.MoveTo(dirtyRect.Center.X, dirtyRect.Center.Y - radius);

for (int i = 1; i < 5; i++)
{
    double angle = i * 4 * Math.PI / 5;
    path.LineTo(new PointF(radius * (float)Math.Sin(angle) + dirtyRect.Center.X, -radius * (float)Math.Cos(angle) + dirtyRect.Center.Y));
}
path.Close();

canvas.StrokeSize = 15;
canvas.StrokeLineJoin = LineJoin.Round;
canvas.StrokeColor = Colors.Red;
canvas.FillColor = Colors.Blue;
canvas.FillPath(path, WindingMode.EvenOdd);
canvas.DrawPath(path);
```

In this example, the path is drawn twice. The `FillPath` method is used to fill the path with blue, while the `DrawPath` method outlines the path with a red stroke. The `FillPath` overload used specifies that the `EvenOdd` winding mode is used. This results in the central area of the star not being filled:

:::image type="content" source="windingmodes-images/evenodd.png" alt-text="Screenshot of a five-pointed star, using the even-odd winding mode.":::
