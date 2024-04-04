---
title: "Winding modes"
description: ".NET MAUI graphics enables you to specify the fill algorithm when filling or clipping a path."
ms.date: 12/16/2021
---

# Winding modes

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-graphicsview)

.NET Multi-platform App UI (.NET MAUI) graphics provides a <xref:Microsoft.Maui.Graphics.WindingMode> enumeration that enables you to specify the fill algorithm to be used by the <xref:Microsoft.Maui.Graphics.ICanvas.FillPath%2A> method. Contours in a path can overlap, and any enclosed area can potentially be filled, but you might not want to fill all the enclosed areas. For more information about paths, see [Draw a path](draw.md#draw-a-path).

The <xref:Microsoft.Maui.Graphics.WindingMode> enumeration defines `NonZero` and `EvenOdd` members. Each member represents a different algorithm for determining whether a point is in the fill region of an enclosed area.

> [!NOTE]
> The <xref:Microsoft.Maui.Graphics.ICanvas.ClipPath%2A> method has an overload that enables a <xref:Microsoft.Maui.Graphics.WindingMode> argument to be specified. By default, this argument is set to `WindingMode.NonZero`.

## NonZero

The `NonZero` winding mode draws a hypothetical ray from the point to infinity in any direction and then examines the places where a path contour crosses the ray. The count starts at zero and is incremented each time a contour crosses the ray from left to right and decremented each time a contour crosses the ray from right to left. If the count of crossings is zero, the area isn't filled. Otherwise, the area is filled.

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

In this example, the path is drawn twice. The <xref:Microsoft.Maui.Graphics.ICanvas.FillPath%2A> method is used to fill the path with blue, while the <xref:Microsoft.Maui.Graphics.ICanvas.DrawPath%2A> method outlines the path with a red stroke. The <xref:Microsoft.Maui.Graphics.ICanvas.FillPath%2A> overload used omits the <xref:Microsoft.Maui.Graphics.WindingMode> argument, and instead automatically uses the `NonZero` winding mode. This results in all the enclosed areas of the path being filled:

:::image type="content" source="media/windingmodes/nonzero.png" alt-text="Screenshot of a five-pointed star, using the non-zero winding mode.":::

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

In this example, the path is drawn twice. The <xref:Microsoft.Maui.Graphics.ICanvas.FillPath%2A> method is used to fill the path with blue, while the <xref:Microsoft.Maui.Graphics.ICanvas.DrawPath%2A> method outlines the path with a red stroke. The <xref:Microsoft.Maui.Graphics.ICanvas.FillPath%2A> overload used specifies that the `EvenOdd` winding mode is used. This mode results in the central area of the star not being filled:

:::image type="content" source="media/windingmodes/evenodd.png" alt-text="Screenshot of a five-pointed star, using the even-odd winding mode.":::
