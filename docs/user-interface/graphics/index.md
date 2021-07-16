---
title: ".NET MAUI Graphics"
description: ".NET MAUI includes a cross-platform 2D graphics library that targets iOS, Android, Windows, macOS, Tizen, and Linux."
ms.date: 07/16/2021
---

# .NET MAUI Graphics

.NET Multi-platform App UI (MAUI) includes a cross-platform 2D graphics library that targets iOS, Android, Windows, macOS, Tizen, and Linux. With this library, you can use a common API to target multiple graphics abstractions, enabling you to share your drawing code between platforms, or mix and match graphics implementations with a single app.

The library, which is contained in the `Microsoft.Maui.Graphics` namespace, includes a drawing canvas, which supports drawing shapes, paths, gradients, text, images, and shadows.

In .NET MAUI, the `GraphicsView` can be used to draw 2D graphics using this library. For more information about the `GraphicsView`, see [.NET MAUI GraphicsView](~/user-interface/controls/graphicsview.md).

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
