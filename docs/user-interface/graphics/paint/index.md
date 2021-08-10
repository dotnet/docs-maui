---
title: "Paint graphical objects"
description: ".NET MAUI graphics includes a Paint class, which is an abstract class that paints a graphical object with its output."
ms.date: 07/15/2021
---

# Paint graphical objects

<!-- Sample link goes here -->

.NET Multi-platform App UI (.NET MAUI) graphics includes the ability to paint graphical objects with solid colors, gradients, repeating images, and patterns.

The `Paint` class is an abstract class that paints an object with its output. Classes that derive from `Paint` describe different ways of painting an object. The following list describes the different paint types available in .NET MAUI graphics:

- `SolidPaint`, which paints an object with a solid color. For more information, see [Solid color paint](solidcolor.md).
- `GradientPaint`, which paints an object with a gradient. For more information, see [Gradient paint](gradient.md).
- `ImagePaint`, which paints an object with an image. For more information, see [Image paint](image.md).
- `PatternPaint`, which paints an object with a pattern. For more information, see [Pattern paint](pattern.md).

Instances of these types can be painted on an `ICanvas`, typically by using the `SetFillPaint` method to set the paint as the fill of a graphical object.

The `Paint` class also defines `BackgroundColor`, and `ForegroundColor` properties, of type `Color`, that can be used to optionally define background and foreground colors for a `Paint` object.
