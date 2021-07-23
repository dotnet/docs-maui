---
title: ".NET MAUI Graphics: Paint objects"
description: "The .NET MAUI Paint class is an abstract class that paints an area with its output."
ms.date: 07/15/2021
---

# .NET MAUI Graphics: Paint objects

.NET Multi-platform App UI (MAUI) includes the ability to paint an area of objects drawn with types from the `Microsoft.Maui.Graphics`, using different approaches.

The `Paint` class is an abstract class that paints an area with its output. Classes that derive from `Paint` describe different ways of painting an area. The following list describes the different paint types available in .NET MAUI:

- `SolidPaint`, which paints an area with a solid color. For more information, see [Solid color paint](solidcolor.md).
- `GradientPaint`, which paints an area with a gradient. For more information, see [Gradient paint](gradient.md).
- `ImagePaint`, which paints an area with an image. For more information, see [Image paint](image.md).
- `PatternPaint`, which paints an area with a pattern. For more information, see [Pattern paint](pattern.md).

Instances of these types can painted on an `ICanvas`, typically by using the `SetFillPaint` method to set the paint as the fill of a shape.
