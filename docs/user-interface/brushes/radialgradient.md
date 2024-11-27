---
title: "Radial gradient brushes"
description: "The .NET MAUI RadialGradientBrush class paints an area with a radial gradient."
ms.date: 09/30/2024
---

# Radial gradient brushes

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-brushes)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.RadialGradientBrush> class derives from the <xref:Microsoft.Maui.Controls.GradientBrush> class, and paints an area with a radial gradient, which blends two or more colors across a circle. <xref:Microsoft.Maui.Controls.GradientStop> objects are used to specify the colors in the gradient and their positions. For more information about <xref:Microsoft.Maui.Controls.GradientStop> objects, see [Gradients](gradient.md).

The <xref:Microsoft.Maui.Controls.RadialGradientBrush> class defines the following properties:

- `Center`, of type `Point`, which represents the center point of the circle for the radial gradient. The default value of this property is (0.5,0.5).
- `Radius`, of type `double`, which represents the radius of the circle for the radial gradient. The default value of this property is 0.5.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.RadialGradientBrush> class also has an `IsEmpty` method that returns a `bool` that represents whether the brush has been assigned any <xref:Microsoft.Maui.Controls.GradientStop> objects.

> [!NOTE]
> Radial gradients can also be created with the `radial-gradient()` CSS function.

## Create a RadialGradientBrush

A radial gradient brush's gradient stops are positioned along a gradient axis defined by a circle. The gradient axis radiates from the center of the circle to its circumference. The position and size of the circle can be changed using the brush's `Center` and `Radius` properties. The circle defines the end point of the gradient. Therefore, a gradient stop at 1.0 defines the color at the circle's circumference. A gradient stop at 0.0 defines the color at the center of the circle.

To create a radial gradient, create a <xref:Microsoft.Maui.Controls.RadialGradientBrush> object and set its `Center` and `Radius` properties. Then, add two or more <xref:Microsoft.Maui.Controls.GradientStop> objects to the `RadialGradientBrush.GradientStops` collection, that specify the colors in the gradient and their positions.

The following XAML example shows a <xref:Microsoft.Maui.Controls.RadialGradientBrush> that's set as the `Background` of a <xref:Microsoft.Maui.Controls.Border>:

```xaml
<Border Stroke="LightGray"
        StrokeShape="RoundRectangle 12"
        HeightRequest="120"
        WidthRequest="120">
    <Border.Background>
        <!-- Center defaults to (0.5,0.5)
             Radius defaults to (0.5) -->
        <RadialGradientBrush>
            <GradientStop Color="Red"
                          Offset="0.1" />
            <GradientStop Color="DarkBlue"
                          Offset="1.0" />
        </RadialGradientBrush>
    </Border.Background>
</Border>
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Border> is painted with a <xref:Microsoft.Maui.Controls.RadialGradientBrush> that interpolates from red to dark blue. The center of the radial gradient is positioned in the center of the <xref:Microsoft.Maui.Controls.Border>:

:::image type="content" source="media/radialgradient/center.png" alt-text="Screenshot of a Border painted with a centered RadialGradientBrush.":::

The following XAML example moves the center of the radial gradient to the top-left corner of the <xref:Microsoft.Maui.Controls.Border>:

```xaml
<!-- Radius defaults to (0.5) -->
<RadialGradientBrush Center="0.0,0.0">
    <GradientStop Color="Red"
                  Offset="0.1" />
    <GradientStop Color="DarkBlue"
                  Offset="1.0" />
</RadialGradientBrush>
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Border> is painted with a <xref:Microsoft.Maui.Controls.RadialGradientBrush> that interpolates from red to dark blue. The center of the radial gradient is positioned in the top-left of the <xref:Microsoft.Maui.Controls.Border>:

:::image type="content" source="media/radialgradient/top-left.png" alt-text="Screenshot of a Border painted with a top-left RadialGradientBrush.":::

The following XAML example moves the center of the radial gradient to the bottom-right corner of the <xref:Microsoft.Maui.Controls.Border>:

```xaml
<!-- Radius defaults to (0.5) -->
<RadialGradientBrush Center="1.0,1.0">
    <GradientStop Color="Red"
                  Offset="0.1" />
    <GradientStop Color="DarkBlue"
                  Offset="1.0" />
</RadialGradientBrush>            
```

In this example, the background of the <xref:Microsoft.Maui.Controls.Border> is painted with a <xref:Microsoft.Maui.Controls.RadialGradientBrush> that interpolates from red to dark blue. The center of the radial gradient is positioned in the bottom-right of the <xref:Microsoft.Maui.Controls.Border>:

:::image type="content" source="media/radialgradient/bottom-right.png" alt-text="Screenshot of a Border painted with a bottom-right RadialGradientBrush.":::
