---
title: "Graphics"
description: ".NET MAUI includes cross-platform 2D graphics functionality that targets iOS, Android, Windows, macOS, Tizen, and Linux."
ms.date: 09/30/2024
---

# Graphics

[![Browse sample.](../../media/code-sample.png) Browse the sample](https://github.com/dotnet/maui-samples/tree/main/9.0/UserInterface/Views/GraphicsViewDemos)

.NET Multi-platform App UI (.NET MAUI) provides a cross-platform graphics canvas on which 2D graphics can be drawn using types from the <xref:Microsoft.Maui.Graphics> namespace. This canvas supports drawing and painting shapes and images, compositing operations, and graphical object transforms.

There are many similarities between the functionality provided by <xref:Microsoft.Maui.Graphics>, and the functionality provided by .NET MAUI shapes and brushes. However, each is aimed at different scenarios:

- <xref:Microsoft.Maui.Graphics> functionality must be consumed on a drawing canvas, enables performant graphics to be drawn, and provides a convenient approach for writing graphics-based controls. For example, a control that replicates the GitHub contribution profile can be more easily implemented using <xref:Microsoft.Maui.Graphics> than by using .NET MAUI shapes.
- .NET MAUI shapes can be consumed directly on a page, and brushes can be consumed by all controls. This functionality is provided to help you produce an attractive UI.

For more information about .NET MAUI shapes, see [Shapes](../controls/shapes/index.md).

<!-- ## Platform abstractions

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

By default, .NET MAUI uses the native graphics capabilities of each platform. -->

## Draw graphics

In .NET MAUI, the <xref:Microsoft.Maui.Controls.GraphicsView> enables consumption of <xref:Microsoft.Maui.Graphics> functionality. <xref:Microsoft.Maui.Controls.GraphicsView> defines the `Drawable` property, of type `IDrawable`, which specifies the content that will be drawn by the control. To specify the content that will be drawn you must create an object that derives from `IDrawable`, and implement its `Draw` method:

```csharp
namespace MyMauiApp
{
    public class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Drawing code goes here
        }      
    }
}
```

The `Draw` method has <xref:Microsoft.Maui.Graphics.ICanvas> and `RectF` arguments. The <xref:Microsoft.Maui.Graphics.ICanvas> argument is the drawing canvas on which you draw graphical objects. The `RectF` argument is a `struct` that contains data about the size and location of the drawing canvas.

In XAML, the `IDrawable` object can be declared as a resource and then consumed by a <xref:Microsoft.Maui.Controls.GraphicsView> by specifying its key as the value of the `Drawable` property:

```xaml
<ContentPage xmlns=http://schemas.microsoft.com/dotnet/2021/maui
             xmlns:x=http://schemas.microsoft.com/winfx/2009/xaml
             xmlns:drawable="clr-namespace:MyMauiApp"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Resources>
        <drawable:GraphicsDrawable x:Key="drawable" />
    </ContentPage.Resources>
    <VerticalStackLayout>
        <GraphicsView Drawable="{StaticResource drawable}"
                      HeightRequest="300"
                      WidthRequest="400" />
    </VerticalStackLayout>
</ContentPage>
```

For more information about the <xref:Microsoft.Maui.Controls.GraphicsView>, see [GraphicsView](../controls/graphicsview.md).

## Drawing canvas

The <xref:Microsoft.Maui.Controls.GraphicsView> control provides access to an <xref:Microsoft.Maui.Graphics.ICanvas> object, via its `IDrawable` object, on which properties can be set and methods invoked to draw graphical objects. For information about drawing on an <xref:Microsoft.Maui.Graphics.ICanvas>, see [Draw graphical objects](draw.md).

<xref:Microsoft.Maui.Graphics.ICanvas> defines the following properties that affect the appearance of objects that are drawn on the canvas:

<!-- Todo: Font properties being renamed. Some property types may change -->

- <xref:Microsoft.Maui.Graphics.ICanvas.Alpha>, of type `float`, indicates the opacity of an object.
- <xref:Microsoft.Maui.Graphics.ICanvas.Antialias>, of type `bool`, specifies whether anti-aliasing is enabled.
- <xref:Microsoft.Maui.Graphics.ICanvas.BlendMode>, of type <xref:Microsoft.Maui.Graphics.BlendMode>, defines the blend mode, which determines what happens when an object is rendered on top of an existing object.
- <xref:Microsoft.Maui.Graphics.ICanvas.DisplayScale>, of type `float`, represents the scaling factor to scale the UI by on a canvas.
- <xref:Microsoft.Maui.Graphics.ICanvas.FillColor>, of type <xref:Microsoft.Maui.Graphics.Color>, indicates the color used to paint an object's interior.
- <xref:Microsoft.Maui.Graphics.ICanvas.Font>, of type <xref:Microsoft.Maui.Graphics.IFont>, defines the font when drawing text.
- <xref:Microsoft.Maui.Graphics.ICanvas.FontColor>, of type <xref:Microsoft.Maui.Graphics.Color>, specifies the font color when drawing text.
- <xref:Microsoft.Maui.Graphics.ICanvas.FontSize>, of type `float`, defines the size of the font when drawing text.
- <xref:Microsoft.Maui.Graphics.ICanvas.MiterLimit>, of type `float`, specifies the limit of the miter length of line joins in an object.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeColor>, of type <xref:Microsoft.Maui.Graphics.Color>, indicates the color used to paint an object's outline.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeDashOffset>, of type `float`, specifies the distance within the dash pattern where a dash begins.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeDashPattern>, of type `float[]`, specifies the pattern of dashes and gaps that are used to outline an object.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeLineCap>, of type <xref:Microsoft.Maui.Graphics.LineCap>, describes the shape at the start and end of a line.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeLineJoin>, of type <xref:Microsoft.Maui.Graphics.LineJoin>, specifies the type of join that is used at the vertices of a shape.
- <xref:Microsoft.Maui.Graphics.ICanvas.StrokeSize>, of type `float`, indicates the width of an object's outline.

By default, an <xref:Microsoft.Maui.Graphics.ICanvas> sets <xref:Microsoft.Maui.Graphics.ICanvas.StrokeSize> to 1, <xref:Microsoft.Maui.Graphics.ICanvas.StrokeColor> to black, <xref:Microsoft.Maui.Graphics.ICanvas.StrokeLineJoin> to `LineJoin.Miter`, and <xref:Microsoft.Maui.Graphics.ICanvas.StrokeLineCap> to `LineJoin.Cap`.

### Drawing canvas state

The drawing canvas on each platform has the ability to maintain its state. This enables you to persist the current graphics state, and restore it when required.

However, not all elements of the canvas are elements of the graphics state. The graphics state does not include drawing objects, such as paths, and paint objects, such as gradients. Typical elements of the graphics state on each platform include stroke and fill data, and font data.

The graphics state of each <xref:Microsoft.Maui.Graphics.ICanvas> can be manipulated with the following methods:

- <xref:Microsoft.Maui.Graphics.ICanvas.SaveState%2A>, which saves the current graphics state.
- <xref:Microsoft.Maui.Graphics.ICanvas.RestoreState%2A>, which sets the graphics state to the most recently saved state.
- <xref:Microsoft.Maui.Graphics.ICanvas.ResetState%2A>, which resets the graphics state to its default values.

> [!NOTE]
> The state that's persisted by these methods is platform dependent.
