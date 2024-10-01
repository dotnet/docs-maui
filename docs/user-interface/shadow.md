---
title: "Shadow"
description: "Learn how to use the .NET MAUI Shadow class, which paints a shadow around a control."
ms.date: 09/30/2024
---

# Shadow

The .NET Multi-platform App UI (.NET MAUI) `Shadow` class paints a shadow around a layout or view. The <xref:Microsoft.Maui.Controls.VisualElement> class has a `Shadow` bindable property, of type `Shadow`, that enables a shadow to be added to any layout or view.

The `Shadow` class defines the following properties:

- `Radius`, of type `float`, defines the radius of the blur used to generate the shadow. The default value of this property is 10.
- `Opacity`, of type `float`, indicates the opacity of the shadow. The default value of this property is 1.
- `Brush`, of type <xref:Microsoft.Maui.Controls.Brush>, represents the brush used to colorize the shadow.
- `OffSet`, of type `Point`, specifies the offset for the shadow, which represents the position of the light source that creates the shadow.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!IMPORTANT]
> The `Brush` property only currently supports a <xref:Microsoft.Maui.Controls.SolidColorBrush>.

## Create a Shadow

To add a shadow to a control, set the control's `Shadow` property to a `Shadow` object whose properties define its appearance.

The following XAML example shows how to add a shadow to an <xref:Microsoft.Maui.Controls.Image>:

```xaml
<Image Source="dotnet_bot.png"
       WidthRequest="250"
       HeightRequest="310">
    <Image.Shadow>
        <Shadow Brush="Black"
                Offset="20,20"
                Radius="40"
                Opacity="0.8" />
    </Image.Shadow>
</Image>
```

In this example, a black shadow is painted around the outline of the image, with its offset specifying that it appears at the right and bottom of the image:

:::image type="content" source="media/shadow/image.png" alt-text="Screenshot of an image with a shadow applied.":::

Shadows can also be added to clipped objects, as shown in the following example:

```xaml
<Image Source="https://aka.ms/campus.jpg"
       Aspect="AspectFill"
       HeightRequest="220"
       WidthRequest="220"
       HorizontalOptions="Center">
    <Image.Clip>
        <EllipseGeometry Center="220,250"
                         RadiusX="220"
                         RadiusY="220" />
    </Image.Clip>
    <Image.Shadow>
        <Shadow Brush="Black"
                Offset="10,10"
                Opacity="0.8" />
    </Image.Shadow>
</Image>
```

In this example, a black shadow is painted around the outline of the <xref:Microsoft.Maui.Controls.Shapes.EllipseGeometry> that clips the image:

:::image type="content" source="media/shadow/clipped-image.png" alt-text="Screenshot of a clipped image with a shadow applied.":::

For more information about clipping an element, see [Clip with a Geometry](~/user-interface/controls/shapes/geometries.md#clip-with-a-geometry).

<!-- Todo: Only currently supported on Android

## Create a Shadow gradient

The color of a shadow is defined using a <xref:Microsoft.Maui.Controls.Brush>. Therefore, gradient shadows can also be added to controls:

```xaml
<RoundRectangle HeightRequest="200"
                WidthRequest="300"
                CornerRadius="40"
                Stroke="#C49B33"
                StrokeThickness="10"
                Fill="#2B0B98">
    <RoundRectangle.Shadow>
        <Shadow Radius="60"
                Offset="40,40"
                Opacity="0.75">
            <Shadow.Brush>
                <LinearGradientBrush EndPoint="0,1">
                    <GradientStop Color="Gray"
                                  Offset="0.1" />
                    <GradientStop Color="Black"
                                  Offset="1.0" />
                </LinearGradientBrush>
            </Shadow.Brush>
        </Shadow>
    </RoundRectangle.Shadow>
</RoundRectangle>
```

In this example, a linear gradient shadow is added to the round rectangle, with the gradient interpolating vertically from gray to black:

:::image type="content" source="media/shadow/roundrectangle.png" alt-text="Screenshot of a round rectangle with a shadow applied.":::

For more information about brushes, see [Brushes](). -->
