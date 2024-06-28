---
title: "Border"
description: "Learn how to use the .NET MAUI Border class, which is a container control that draws a border, background, or both, around another control."
ms.date: 09/29/2022
---

# Border

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Border> is a container control that draws a border, background, or both, around another control. A <xref:Microsoft.Maui.Controls.Border> can only contain one child object. If you want to put a border around multiple objects, wrap them in a container object such as a layout.  For more information about layouts, see [Layouts](~/user-interface/layouts/index.md).

<xref:Microsoft.Maui.Controls.Border> defines the following properties:

- `Content`, of type `IView`, represents the content to display in the border. This property is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.Border> class, and therefore does not need to be explicitly set from XAML.
- `Padding`, of type `Thickness`, represents the distance between the border and its child element.
- `StrokeShape`, of type `IShape`, describes the shape of the border. This property has a type converter applied to it that can convert a string to its equivalent `IShape`. Its default value is <xref:Microsoft.Maui.Controls.Shapes.Rectangle>. Therefore, a <xref:Microsoft.Maui.Controls.Border> will be rectangular by default.
- `Stroke`, of type <xref:Microsoft.Maui.Controls.Brush>, indicates the brush used to paint the border.
- `StrokeThickness`, of type `double`, indicates the width of the border. The default value of this property is 1.0.
- `StrokeDashArray`, of type `DoubleCollection`, which represents a collection of `double` values that indicate the pattern of dashes and gaps that make up the border.
- `StrokeDashOffset`, of type `double`, specifies the distance within the dash pattern where a dash begins. The default value of this property is 0.0.
- `StrokeLineCap`, of type `PenLineCap`, describes the shape at the start and end of its line. The default value of this property is `PenLineCap.Flat`.
- `StrokeLineJoin`, of type `PenLineJoin`, specifies the type of join that is used at the vertices of the stroke shape. The default value of this property is `PenLineJoin.Miter`.
- `StrokeMiterLimit`, of type `double`, specifies the limit on the ratio of the miter length to half the stroke thickness. The default value of this property is 10.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!IMPORTANT]
> When creating a border using a shape, such as a <xref:Microsoft.Maui.Controls.Shapes.Rectangle> or <xref:Microsoft.Maui.Controls.Shapes.Polygon>, only closed shapes should be used. Therefore, open shapes such as <xref:Microsoft.Maui.Controls.Shapes.Line> are unsupported.

For more information about the properties that control the shape and stroke of the border, see [Shapes](~/user-interface/controls/shapes/index.md).

## Create a Border

To draw a border, create a <xref:Microsoft.Maui.Controls.Border> object and set its properties to define its appearance. Then, set its child to the control to which the border should be added.

The following XAML example shows how to draw a border around a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Border Stroke="#C49B33"
        StrokeThickness="4"
        StrokeShape="RoundRectangle 40,0,0,40"
        Background="#2B0B98"
        Padding="16,8"
        HorizontalOptions="Center">
    <Label Text=".NET MAUI"
           TextColor="White"
           FontSize="18"
           FontAttributes="Bold" />
</Border>
```

Alternatively, the `StrokeShape` property value can be specified using property tag syntax:

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

In this example, a border with rounded top-left and bottom-right corners is drawn around a <xref:Microsoft.Maui.Controls.Label>. The border shape is defined as a <xref:Microsoft.Maui.Controls.Shapes.RoundRectangle> object, whose `CornerRadius` property is set to a `Thickness` value that enables independent control of each corner of the rectangle:

:::image type="content" source="media/border/border.png" alt-text="Border around a Label screenshot.":::

Because the `Stroke` property is of type <xref:Microsoft.Maui.Controls.Brush>, borders can also be drawn using gradients:

```xaml
<Border StrokeThickness="4"
        StrokeShape="RoundRectangle 40,0,0,40"
        Background="#2B0B98"
        Padding="16,8"
        HorizontalOptions="Center">
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

In this example, a border that uses a linear gradient is drawn around a <xref:Microsoft.Maui.Controls.Label>:

:::image type="content" source="media/border/linear-gradient-border.png" alt-text="Linear gradient border around a Label screenshot.":::

## Define the border shape with a string

In XAML, the value of the `StrokeShape` property can be defined using property-tag syntax, or as a `string`. Valid `string` values for the `StrokeShape` property are:

- `Ellipse`
- `Line`, followed by one or two x- and y-coordinate pairs. For example, `Line 10 20` draws a line from (10,20) to (0,0), and `Line 10 20, 100 120` draws a line from (10,20) to (100,120).
- `Path`, followed by path markup syntax data. For example, `Path M 10,100 L 100,100 100,50Z` will draw a triangular border. For more information about path markup syntax, see [Path markup syntax](shapes/path-markup-syntax.md).
- `Polygon`, followed by a collection of x- and y-coordinate pairs. For example, `Polygon 40 10, 70 80, 10 50`.
- `Polyline`, followed by a collection x- and y-coordinate pairs. For example, `Polyline 0,0 10,30 15,0 18,60 23,30 35,30 40,0 43,60 48,30 100,30`.
- `Rectangle`
- `RoundRectangle`, optionally followed by a corner radius. For example, `RoundRectangle 40` or `RoundRectangle 40,0,0,40`.

> [!IMPORTANT]
> While `Line` is a valid `string` value for the `StrokeShape` property, its use is not supported.

`String`-based x- and y-coordinate pairs can be delimited by a single comma and/or one or more spaces. For example, "40,10 70,80" and "40 10, 70 80" are both valid. Coordinate pairs will be converted to `Point` objects that define `X` and `Y` properties, of type `double`.
