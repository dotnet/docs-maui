---
title: ".NET MAUI Graphics: Paint"
description: "The .NET MAUI Paint class is an abstract class that paints an area with its output."
ms.date: 07/15/2021
---

# .NET MAUI Graphics: Paint

.NET Multi-platform App UI (MAUI) includes the ability to paint an area of objects drawn with types from the `Microsoft.Maui.Graphics`, using different approaches.

The `Paint` class is an abstract class that paints an area with its output. Classes that derive from `Paint` describe different ways of painting an area. The following list describes the different paint types available in .NET MAUI:

- `SolidPaint`
- `GradientPaint`
- `ImagePaint`
- `PatternPaint`

Instances of these types can painted on an `ICanvas`, typically by using the `SetFillPaint` method to set the paint as the fill of a particular shape.
