---
title: "Solid color brushes"
description: "The .NET MAUI SolidColorBrush class paints an area with a solid color."
ms.date: 09/30/2024
---

# Solid color brushes

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-brushes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.SolidColorBrush> class derives from the <xref:Microsoft.Maui.Controls.Brush> class, and is used to paint an area with a solid color. There are a variety of approaches to specifying the color of a <xref:Microsoft.Maui.Controls.SolidColorBrush>. For example, you can specify its color with a <xref:Microsoft.Maui.Graphics.Color> value or by using one of the predefined <xref:Microsoft.Maui.Controls.SolidColorBrush> objects provided by the <xref:Microsoft.Maui.Controls.Brush> class.

The <xref:Microsoft.Maui.Controls.SolidColorBrush> class defines the `Color` property, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the color of the brush. This property is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.SolidColorBrush> class, and therefore does not need to be explicitly set from XAML. In addition, this property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.SolidColorBrush> class also has an `IsEmpty` method that returns a `bool` that represents whether the brush has been assigned a color.

## Create a SolidColorBrush

There are three main techniques for creating a <xref:Microsoft.Maui.Controls.SolidColorBrush>. You can create a <xref:Microsoft.Maui.Controls.SolidColorBrush> from a <xref:Microsoft.Maui.Graphics.Color>, use a predefined brush, or create a <xref:Microsoft.Maui.Controls.SolidColorBrush> using hexadecimal notation.

### Use a predefined Color

.NET MAUI includes a type converter that creates a <xref:Microsoft.Maui.Controls.SolidColorBrush> from a <xref:Microsoft.Maui.Graphics.Color> value. In XAML, this enables a <xref:Microsoft.Maui.Controls.SolidColorBrush> to be created from a predefined <xref:Microsoft.Maui.Graphics.Color> value:

```xaml
<Frame Background="DarkBlue"
       BorderColor="LightGray"
       HasShadow="True"
       CornerRadius="12"
       HeightRequest="120"
       WidthRequest="120" />
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Frame> is painted with a dark blue <xref:Microsoft.Maui.Controls.SolidColorBrush>:

:::image type="content" source="media/solidcolor/predefined-color.png" alt-text="Screenshot of a Frame painted with a predefined color.":::

Alternatively, the <xref:Microsoft.Maui.Graphics.Color> value can be specified using property tag syntax:

```xaml
<Frame BorderColor="LightGray"
       HasShadow="True"
       CornerRadius="12"
       HeightRequest="120"
       WidthRequest="120">
       <Frame.Background>
           <SolidColorBrush Color="DarkBlue" />
       </Frame.Background>
</Frame>
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Frame> is painted with a <xref:Microsoft.Maui.Controls.SolidColorBrush> whose color is specified by setting the `SolidColorBrush.Color` property.

### Use a predefined Brush

The <xref:Microsoft.Maui.Controls.Brush> class defines a set of commonly used <xref:Microsoft.Maui.Controls.SolidColorBrush> objects. The following example uses one of these predefined <xref:Microsoft.Maui.Controls.SolidColorBrush> objects:

```xaml
<Frame Background="{x:Static Brush.Indigo}"
       BorderColor="LightGray"
       HasShadow="True"
       CornerRadius="12"
       HeightRequest="120"
       WidthRequest="120" />       
```

The equivalent C# code is:

```csharp
Frame frame = new Frame
{
    Background = Brush.Indigo,
    BorderColor = Colors.LightGray,
    // ...
};
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Frame> is painted with an indigo <xref:Microsoft.Maui.Controls.SolidColorBrush>:

:::image type="content" source="media/solidcolor/predefined-brush.png" alt-text="Screenshot of a Frame painted with a predefined SolidColorBrush.":::

For a list of predefined <xref:Microsoft.Maui.Controls.SolidColorBrush> objects provided by the <xref:Microsoft.Maui.Controls.Brush> class, see [Solid color brushes](#solid-color-brushes).

### Use hexadecimal notation

<xref:Microsoft.Maui.Controls.SolidColorBrush> objects can also be created using hexadecimal notation. With this approach, a color is specified in terms of the amount of red, green, and blue to combine into a single color. The main format for specifying a color using hexadecimal notation is `#rrggbb`, where:

- `rr` is a two-digit hexadecimal number specifying the relative amount of red.
- `gg` is a two-digit hexadecimal number specifying the relative amount of green.
- `bb` is a two-digit hexadecimal number specifying the relative amount of blue.

In addition, a color can be specified as `#aarrggbb` where `aa` specifies the alpha value, or transparency, of the color. This approach enables you to create colors that are partially transparent.

The following example sets the color value of a <xref:Microsoft.Maui.Controls.SolidColorBrush> using hexadecimal notation:

```xaml
<Frame Background="#FF9988"
       BorderColor="LightGray"
       HasShadow="True"
       CornerRadius="12"
       HeightRequest="120"
       WidthRequest="120" />
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Frame> is painted with a salmon-colored <xref:Microsoft.Maui.Controls.SolidColorBrush>:

:::image type="content" source="media/solidcolor/hex.png" alt-text="Screenshot of a Frame painted with a SolidColorBrush created with hexadecimal notation.":::

For other ways of describing color, see [Colors](~/user-interface/graphics/colors.md).

## Solid color brushes

For convenience, the <xref:Microsoft.Maui.Controls.Brush> class provides a set of commonly used <xref:Microsoft.Maui.Controls.SolidColorBrush> objects, such as `AliceBlue` and `YellowGreen`. The following image shows the color of each predefined brush, its name, and its hexadecimal value:

:::image type="content" source="media/solidcolor/solidcolorbrushes.png" alt-text="Color table including a color swatch, color name, and hexadecimal value." lightbox="media/solidcolor/solidcolorbrushes-large.png" border="false":::
