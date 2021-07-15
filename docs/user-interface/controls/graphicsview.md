---
title: ".NET MAUI GraphicsView"
description: "The .NET MAUI GraphicsView is a graphics canvas on which the types from the cross-platform Microsoft.Maui.Graphics library can be drawn."
ms.date: 07/12/2021
---

# .NET MAUI GraphicsView

The .NET Multi-platform App UI (MAUI) `GraphicsView` is a graphics canvas on which the types from the cross-platform `Microsoft.Maui.Graphics` library can be drawn.

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

The `IDrawable` object can then be consumed by the `GraphicsView` by declaring it as a resource, and consuming it using its key:

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
                      WidthRequest="500" />
    </StackLayout>
</ContentPage>
```

## Draw shapes

### Line

### Ellipse

### Arc

### Rectangle

### RoundedRectangle

### Path

## Fill shapes

### Ellipse

### Arc

### Rectangle

### RoundedRectangle

### Path

## Draw dashed shapes

## Draw text

## Control line ends

## Control line joins
