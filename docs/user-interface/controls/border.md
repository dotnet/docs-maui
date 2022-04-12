---
title: "Border"
description: "Learn how to use the .NET MAUI Border class, which is a container control that draws a border, background, or both, around another control."
ms.date: 04/12/2022
---

# Border

<!-- Sample link, if any, goes here -->

The .NET Multi-platform App UI (.NET MAUI) `Border` is a container control that draws a border, background, or both, around another control. A `Border` can only contain one child object. If you want to put a border around multiple objects, wrap them in a container object such as a layout. <!-- For more information about layouts, see [](). -->

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

`Border` defines the following properties:

- `Content`, of type `IView`, represents the content to display in the border. This property is the `ContentProperty` of the `Border` class, and therefore does not need to be explicitly set from XAML.
- `Padding`, of type `Thickness`, represents the distance between the border and its child element.
- `StrokeShape`, of type `IShape`, describes the shape of the border.
- `Stroke`, of type `Brush`, indicates the brush used to paint the border.
- `StrokeThickness`, of type `double`, indicates the width of the border. The default value of this property is 1.0.
- `StrokeDashArray`, of type `DoubleCollection`, which represents a collection of `double` values that indicate the pattern of dashes and gaps that make up the border.
- `StrokeDashOffset`, of type `double`, specifies the distance within the dash pattern where a dash begins. The default value of this property is 0.0.
- `StrokeLineCap`, of type `PenLineCap`, describes the shape at the start and end of its line. The default value of this property is `PenLineCap.Flat`.
- `StrokeLineJoin`, of type `PenLineJoin`, specifies the type of join that is used at the vertices of the stroke shape. The default value of this property is `PenLineJoin.Miter`.
- `StrokeMiterLimit`, of type `double`, specifies the limit on the ratio of the miter length to half the stroke thickness. The default value of this property is 10.0.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

<!-- For more information about the properties that control the shape and stroke of the border, see [Shapes](). -->

## Create a Border

To draw a border, create a `Border` object and set its properties to define its appearance. Then, set its child to the control to which the border should be added.

The following XAML example shows how to draw a border around a `Label`:

```xaml
<Border Stroke="#C49B33"
        StrokeThickness="4"
        Background="#2B0B98"
        Padding="16,8"
        HorizontalOptions="Center">
    <Border.StrokeShape>
        <RoundRectangle CornerRadius="40,0,0,40" />
    </Border.StrokeShape>
    <Label Text=".NET MAUI"
           TextColor="White"
           FontSize="18"
           FontAttributes="Bold" />
</Border>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
...

Border border = new Border
{
    Stroke = Color.FromArgb("#C49B33"),
    Background = Color.FromArgb("#2B0B98"),
    StrokeThickness = 4,
    Padding = new Thickness(16, 8),
    HorizontalOptions = LayoutOptions.Center,
    StrokeShape = new RoundRectangle
    {
        CornerRadius = new CornerRadius(40, 0, 0, 40)
    },
    Content = new Label
    {
        Text = ".NET MAUI",
        TextColor = Colors.White,
        FontSize = 18,
        FontAttributes = FontAttributes.Bold
    }
};
```

In this example, a border with rounded top-left and bottom-right corners is drawn around a `Label`. The border shape is defined as a `RoundRectangle` object, whose `CornerRadius` property is set to a `Thickness` value that enables independent control of each corner of the rectangle:

:::image type="content" source="media/border/border.png" alt-text="Border around a Label screenshot.":::

<!-- Todo (potentially): .NET MAUI may add a markup extension for setting the stroke shape directly, rather than having to instantiate one. See https://github.com/dotnet/maui/pull/3256 -->

Because the `Stroke` property is of type `Brush`, borders can also be drawn using gradients:

```xaml
<Border StrokeThickness="4"
        Background="#2B0B98"
        Padding="16,8"
        HorizontalOptions="Center">
    <Border.StrokeShape>
        <RoundRectangle CornerRadius="40,0,0,40" />
    </Border.StrokeShape>
    <Border.Stroke>
        <LinearGradientBrush EndPoint="0,1">
            <GradientStop Color="Orange"
                          Offset="0.1" />
            <GradientStop Color="Brown"
                          Offset="1.0" />
        </LinearGradientBrush>
    </Border.Stroke>
    <Label Text=".NET MAUI"
           TextColor="White"
           FontSize="18"
           FontAttributes="Bold" />
</Border>
```

The equivalent C# code is:

```csharp
using Microsoft.Maui.Controls.Shapes;
using GradientStop = Microsoft.Maui.Controls.GradientStop;
...

Border gradientBorder = new Border
{
    StrokeThickness = 4,
    Background = Color.FromArgb("#2B0B98"),
    Padding = new Thickness(16, 8),
    HorizontalOptions = LayoutOptions.Center,
    StrokeShape = new RoundRectangle
    {
        CornerRadius = new CornerRadius(40, 0, 0, 40)
    },
    Stroke = new LinearGradientBrush
    {
        EndPoint = new Point(0, 1),
        GradientStops = new GradientStopCollection
        {
            new GradientStop { Color = Colors.Orange, Offset = 0.1f },
            new GradientStop { Color = Colors.Brown, Offset = 1.0f }
        },
    },
    Content = new Label
    {
        Text = ".NET MAUI",
        TextColor = Colors.White,
        FontSize = 18,
        FontAttributes = FontAttributes.Bold
    }
};
```

In this example, a border that uses a linear gradient is drawn around a `Label`:

:::image type="content" source="media/border/linear-gradient-border.png" alt-text="Linear gradient border around a Label screenshot.":::
