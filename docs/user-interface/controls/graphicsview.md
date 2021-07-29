---
title: ".NET MAUI GraphicsView"
description: "The .NET MAUI GraphicsView is a graphics canvas on which 2D graphics can be drawn using the cross-platform Microsoft.Maui.Graphics library."
ms.date: 07/28/2021
---

# .NET MAUI GraphicsView

The .NET Multi-platform App UI (MAUI) `GraphicsView` is a graphics canvas on which 2D graphics can be drawn using the `Microsoft.Maui.Graphics` library. For more information about `Microsoft.Maui.Graphics`, see [.NET MAUI Graphics](~/user-interface/graphics/index.md).

`GraphicsView` defines the `Drawable` property, of type `IDrawable`. This property is backed by a `BindableProperty`, which means it can be the target of data binding, and styled.

## Create a GraphicsView

A `GraphicsView` must define an `IDrawable` object that defines the content that will be drawn on the control. This can be achieved by creating an object that derives from `IDrawable`, and by implementing its `Draw` method:

```csharp
using Microsoft.Maui.Graphics;

namespace MyMauiApp
{
    public class GraphicsDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            // Drawing code goes here
        }      
    }
}
```

The `Draw` method has `ICanvas` and `RectangleF` arguments. The `ICanvas` argument is the drawing canvas on which you can draw graphical objects. The `RectangleF` argument is a `struct` that contains data about the size and location of the drawing canvas. For information about drawing on an `ICanvas`, see [.NET MAUI Graphics: Draw objects](~/user-interface/graphics/draw.md).

The `IDrawable` object can be consumed by the `GraphicsView` by declaring it as a resource, and consuming it using its key:

```xaml
<ContentPage xmlns=http://schemas.microsoft.com/dotnet/2021/maui
             xmlns:x=http://schemas.microsoft.com/winfx/2009/xaml
             xmlns:drawable="clr-namespace:MyMauiApp"
             x:Class="MyMauiApp.MainPage">
    <ContentPage.Resources>
        <drawable:GraphicsDrawable x:Key="drawable" />
    </ContentPage.Resources>
    <StackLayout>
        <GraphicsView Drawable="{StaticResource drawable}"
                      HeightRequest="500"
                      WidthRequest="400" />
    </StackLayout>
</ContentPage>
```

## Drawing canvas location and size

The location and size of the `ICanvas` on a page can be determined by examining properties of the `RectangleF` argument in the `Draw` method.

The `RectangleF` struct defines the following properties:

- `Bottom`, of type `float`, which represents the y-coordinate of the bottom edge of the canvas.
- `Center`, of type `PointF`. which specifies the coordinates of the center of the canvas.
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

These properties can be used to place and size graphical objects on the `ICanvas`. For example, graphical objects can be placed at the center of the `Canvas` by using the `Center.X` and `Center.Y` values as arguments to a drawing method. For information about drawing on an `ICanvas`, see [.NET MAUI Graphics: Draw objects](~/user-interface/graphics/draw.md).
