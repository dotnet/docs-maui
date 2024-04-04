---
title: ".NET MAUI GraphicsView"
description: "The .NET MAUI GraphicsView is a graphics canvas on which 2D graphics can be drawn using types from the Microsoft.Maui.Graphics namespace."
ms.date: 04/19/2022
---

# GraphicsView

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-graphicsview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.GraphicsView> is a graphics canvas on which 2D graphics can be drawn using types from the <xref:Microsoft.Maui.Graphics> namespace. For more information about <xref:Microsoft.Maui.Graphics>, see [Graphics](~/user-interface/graphics/index.md).

<xref:Microsoft.Maui.Controls.GraphicsView> defines the `Drawable` property, of type `IDrawable`, which specifies the content that will be drawn. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty>, which means it can be the target of data binding, and styled.

<xref:Microsoft.Maui.Controls.GraphicsView> defines the following events:

- `StartHoverInteraction`, with `TouchEventArgs`, which is raised when a pointer enters the hit test area of the <xref:Microsoft.Maui.Controls.GraphicsView>.
- `MoveHoverInteraction`, with `TouchEventArgs`, which is raised when a pointer moves while the pointer remains within the hit test area of the <xref:Microsoft.Maui.Controls.GraphicsView>.
- `EndHoverInteraction`, which is raised when a pointer leaves the hit test area of the <xref:Microsoft.Maui.Controls.GraphicsView>.
- `StartInteraction`, with `TouchEventArgs`, which is raised when the <xref:Microsoft.Maui.Controls.GraphicsView> is pressed.
- `DragInteraction`, with `TouchEventArgs`, which is raised when the <xref:Microsoft.Maui.Controls.GraphicsView> is dragged.
- `EndInteraction`, with `TouchEventArgs`, which is raised when the press that raised the `StartInteraction` event is released.
- `CancelInteraction`, which is raised when the press that made contact with the <xref:Microsoft.Maui.Controls.GraphicsView> loses contact.

## Create a GraphicsView

A <xref:Microsoft.Maui.Controls.GraphicsView> must define an `IDrawable` object that specifies the content that will be drawn on the control. This can be achieved by creating an object that derives from `IDrawable`, and by implementing its `Draw` method:

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

The `Draw` method has <xref:Microsoft.Maui.Graphics.ICanvas> and `RectF` arguments. The <xref:Microsoft.Maui.Graphics.ICanvas> argument is the drawing canvas on which you draw graphical objects. The `RectF` argument is a `struct` that contains data about the size and location of the drawing canvas. For more information about drawing on an <xref:Microsoft.Maui.Graphics.ICanvas>, see [Draw graphical objects](~/user-interface/graphics/draw.md).

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

## Position and size graphical objects

The location and size of the <xref:Microsoft.Maui.Graphics.ICanvas> on a page can be determined by examining properties of the `RectF` argument in the `Draw` method.

The `RectF` struct defines the following properties:

- `Bottom`, of type `float`, which represents the y-coordinate of the bottom edge of the canvas.
- `Center`, of type `PointF`, which specifies the coordinates of the center of the canvas.
- `Height`, of type `float`, which defines the height of the canvas.
- `IsEmpty`, of type `bool`, which indicates whether the canvas has a zero size and location.
- `Left`, of type `float`, which represents the x-coordinate of the left edge of the canvas.
- `Location`, of type `PointF`, which defines the coordinates of the upper-left corner of the canvas.
- `Right`, of type `float`, which represents the x-coordinate of the right edge of the canvas.
- `Size`, of type `SizeF`, which defines the width and height of the canvas.
- `Top`, of type `float`, which represents the y-coordinate of the top edge of the canvas.
- `Width`, of type `float`, which defines the width of the canvas.
- `X`, of type `float`, which defines the x-coordinate of the upper-left corner of the canvas.
- `Y`, of type `float`, which defines the y-coordinate of the upper-left corner of the canvas.

These properties can be used to position and size graphical objects on the <xref:Microsoft.Maui.Graphics.ICanvas>. For example, graphical objects can be placed at the center of the `Canvas` by using the `Center.X` and `Center.Y` values as arguments to a drawing method. For information about drawing on an <xref:Microsoft.Maui.Graphics.ICanvas>, see [Draw graphical objects](~/user-interface/graphics/draw.md).

## Invalidate the canvas

<xref:Microsoft.Maui.Controls.GraphicsView> has an `Invalidate` method that informs the canvas that it needs to redraw itself. This method must be invoked on a <xref:Microsoft.Maui.Controls.GraphicsView> instance:

```csharp
graphicsView.Invalidate();
```

.NET MAUI automatically invalidates the <xref:Microsoft.Maui.Controls.GraphicsView> as needed by the UI. For example, when the element is first shown, comes into view, or is revealed by moving an element from on top of it, it's redrawn. The only time you need to call `Invalidate` is when you want to force the <xref:Microsoft.Maui.Controls.GraphicsView> to redraw itself, such as if you have changed its content while it's still visible.

<!-- TODO: Not currently supported

## Convert the drawable to an image

Graphical objects that are drawn on a <xref:Microsoft.Maui.Controls.GraphicsView> can be converted to an image by the `ToImage` method, which is available in the <xref:Microsoft.Maui.Graphics> namespace. This method requires `width` and `height` arguments, of type `float`, that specify the dimensions of the image.

The `ToImage` method operates on an `IDrawable` object, which is exposed by the `GraphicsView.Drawable` property. Therefore, to call the `ToImage` method on a <xref:Microsoft.Maui.Controls.GraphicsView>, the <xref:Microsoft.Maui.Controls.GraphicsView> must be named with the `x:Name` attribute:

```xaml
<GraphicsView x:Name="graphicsView"
              Drawable="{StaticResource drawable}"
              HeightRequest="300"
              WidthRequest="400" />
```

In code, the `Drawable` property of the <xref:Microsoft.Maui.Controls.GraphicsView> object can then be accessed, and the `ToImage` method called:

```csharp
IImage image = graphicsView.Drawable.ToImage(400, 500);
```

For information about image handling in <xref:Microsoft.Maui.Graphics>, see [Images](~/user-interface/graphics/images.md). -->
