---
title: ".NET MAUI Graphics"
description: ".NET MAUI includes a cross-platform 2D graphics library that targets iOS, Android, Windows, macOS, Tizen, and Linux."
ms.date: 07/23/2021
---

# .NET MAUI Graphics

<!-- Sample link goes here -->

.NET Multi-platform App UI (MAUI) includes a cross-platform 2D graphics library that targets iOS, Android, Windows, macOS, Tizen, and Linux. With this library, you can use a common API to target multiple graphics abstractions, enabling you to share your drawing code between platforms, or mix and match graphics implementations within a single app.

The library, which is contained in the `Microsoft.Maui.Graphics` namespace, contains a drawing canvas, which supports drawing shapes, paths, gradients, text, images, and shadows.

There are many similarities between the functionality provided by `Microsoft.Maui.Graphics`, and the functionality provided by .NET MAUI shapes and brushes. However, each is aimed at different scenarios:

- `Microsoft.Maui.Graphics` functionality must be consumed on a drawing canvas, enables performant graphics to be drawn, and provides a convenient approach for writing graphics-based controls. For example, a control that replicates the GitHub contribution profile can be more easily implemented using `Microsoft.Maui.Graphics` than by using .NET MAUI shapes.
- .NET MAUI shapes can be consumed directly on a page, and brushes can be consumed by all controls. This functionality is provided to help you produce an attractive UI.

## Platform abstractions

The following table lists the graphics abstractions that are supported on each platform:

| Platform | Graphics abstractions |
| -- | -- |
| .NET MAUI | Platform support as shown per platform below. |
| .NET for iOS | CoreGraphics, SkiaSharp |
| .NET for Android | Android.Graphics, SkiaSharp |
| .NET for macOS | CoreGraphics, SkiaSharp |
| Windows Presentation Foundation | SharpDX, XAML, GDI, SkiaSharp |
| Universal Windows Platform | SharpDX, Win2D, XAML, SkiaSharp |
| Windows Forms | SharpDX, GDI, SkiaSharp |
| Tizen | SkiaSharp |
| Linux | SkiaSharp |

By default, .NET MAUI uses the native graphics capabilities of each platform.

For examples of using different graphics abstractions, see [Microsoft.Maui.Graphics samples](https://github.com/dotnet/Microsoft.Maui.Graphics/tree/main/samples).

## Drawing canvas

In .NET MAUI, the `GraphicsView` enables consumption of the `Microsoft.Maui.Graphics` library, via a drawing canvas that's exposed as an `ICanvas` object. For more information about the `GraphicsView`, see [.NET MAUI GraphicsView](~/user-interface/controls/graphicsview.md).

`ICanvas` defines the following properties, that effect the appearance of object's that are drawn on the canvas:

- `Alpha`, of type `float`, indicates the opacity of an object.
- `Antialias`, of type `bool`, specifies whether anti-aliasing is enabled.
- `BlendMode`, of type `BlendMode`, defines the blend mode, which determines what happens when an object is rendered on top of an existing object.
- `DisplayScale`, of type `float`, represents the scaling factor to scale the UI by on a canvas.
- `FillColor`, of type `Color`, indicates the color used to paint an object's interior.
- `FontColor`, of type `Color`, specifies the font color when drawing text.
- `FontName`, of type `string`, defines the font when drawing text.
- `FontSize`, of type `float`, defines the size of the font when drawing text.
- `MiterLimit`, of type `float`, specifies the limit of the miter length of line joins in an object.
- `StrokeColor`, of type `Color`, indicates the color used to paint an object's outline.
- `StrokeDashPattern`, of type `float[]`, specifies the pattern of dashes and gaps that are used to outline an object.
- `StrokeLineCap`, of type `LineCap`, describes the shape at the start and end of a line.
- `StrokeLineJoin`, of type `LineJoin`, specifies the type of join that is used at the vertices of a shape.
- `StrokeSize`, of type `float`, indicates the width of an object's outline.

By default, an `ICanvas` sets `StrokeSize` to 1, `StrokeColor` to black, `StrokeLineJoin` to `LineJoin.Miter`, and `StrokeLineCap` to `LineJoin.Cap`.

### Drawing canvas state

The drawing canvas on each platform has the ability to maintain its state. This enables you to persist the current graphics state, and restore it when required.

However, not all elements of the canvas are elements of the graphics state. The graphics state does not include drawing objects, such as paths, and paint objects, such as gradients. Typical elements of the graphics state on each platform include stroke and fill data, and font data.

The graphics state of each `ICanvas` can be manipulated with the following methods:

- `SaveState`, which saves the current graphics state.
- `RestoreState`, which sets the graphics state to the most recently saved state.
- `ResetState`, which resets the graphics state to its default values.

> [!NOTE]
> The state that's persisted by these methods is platform dependent.
