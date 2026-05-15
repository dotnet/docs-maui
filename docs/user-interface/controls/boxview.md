---
title: "BoxView"
description: "The .NET MAUI BoxView draws a simple rectangle or square, of a specified width, height, and color."
ms.date: 08/30/2024
---

# BoxView

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.BoxView> draws a simple rectangle or square, of a specified width, height, and color.

<xref:Microsoft.Maui.Controls.BoxView> defines the following properties:

- `Color`, of type <xref:Microsoft.Maui.Graphics.Color>, which defines the color of the <xref:Microsoft.Maui.Controls.BoxView>.
- `CornerRadius`, of type `CornerRadius`, which defines the corner radius of the <xref:Microsoft.Maui.Controls.BoxView>. This property can be set to a single `double` uniform corner radius value, or a `CornerRadius` structure defined by four `double` values that are applied to the top left, top right, bottom left, and bottom right of the <xref:Microsoft.Maui.Controls.BoxView>.

::: moniker range=">=net-maui-11.0"

- `Fill`, of type <xref:Microsoft.Maui.Controls.Brush>, which defines the brush used to paint the <xref:Microsoft.Maui.Controls.BoxView>. This property accepts any <xref:Microsoft.Maui.Controls.Brush>, including <xref:Microsoft.Maui.Controls.SolidColorBrush>, <xref:Microsoft.Maui.Controls.LinearGradientBrush>, and <xref:Microsoft.Maui.Controls.RadialGradientBrush>.

::: moniker-end

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> Although <xref:Microsoft.Maui.Controls.BoxView> can mimic simple graphics, a better alternative is to use .NET MAUI Shapes or [.NET MAUI Graphics](~/user-interface/graphics/index.md).

## Create a BoxView

To draw a rectangle or square, create a <xref:Microsoft.Maui.Controls.BoxView> object and set its `Color`, <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest>, and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties. Optionally, you can also set its `CornerRadius` property.

The following XAML example shows how to create a <xref:Microsoft.Maui.Controls.BoxView>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:BasicBoxView"
             x:Class="BasicBoxView.MainPage">
    <BoxView Color="CornflowerBlue"
             CornerRadius="10"
             WidthRequest="160"
             HeightRequest="160"
             VerticalOptions="Center"
             HorizontalOptions="Center" />
</ContentPage>
```

In this example, a cornflower blue <xref:Microsoft.Maui.Controls.BoxView> is displayed in the center of the page:

:::image type="content" source="media/boxview/boxview-basic.png" alt-text="Screenshot of a BoxView with rounded corners.":::

The <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties are measured in device-independent units.

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.BoxView> can also be a child of an <xref:Microsoft.Maui.Controls.AbsoluteLayout>. In this case, both the location and size of the <xref:Microsoft.Maui.Controls.BoxView> are set using the `LayoutBounds` attached bindable property.

A <xref:Microsoft.Maui.Controls.BoxView> can also be sized to resemble a line of a specific width and thickness.

::: moniker range=">=net-maui-11.0"

## Fill a BoxView with a brush

Starting in .NET 11, <xref:Microsoft.Maui.Controls.BoxView> exposes a `Fill` property of type <xref:Microsoft.Maui.Controls.Brush>. Setting `Fill` lets you paint a <xref:Microsoft.Maui.Controls.BoxView> with any brush, including gradients, instead of being limited to the solid `Color` property.

The following XAML example fills a <xref:Microsoft.Maui.Controls.BoxView> with a <xref:Microsoft.Maui.Controls.LinearGradientBrush>:

```xaml
<BoxView Opacity="0.5"
         WidthRequest="200"
         HeightRequest="100"
         HasShadow="true"
         HorizontalOptions="Center"
         VerticalOptions="Center">
    <BoxView.Fill>
        <LinearGradientBrush StartPoint="0,0"
                             EndPoint="1,0">
            <GradientStop Color="Purple"
                          Offset="0.0" />
            <GradientStop Color="Orange"
                          Offset="0.5" />
            <GradientStop Color="Red"
                          Offset="1.0" />
        </LinearGradientBrush>
    </BoxView.Fill>
</BoxView>
```

Or a <xref:Microsoft.Maui.Controls.RadialGradientBrush>:

```xaml
<BoxView Opacity="0.5"
         WidthRequest="200"
         HeightRequest="100"
         HasShadow="true"
         HorizontalOptions="Center"
         VerticalOptions="Center">
    <BoxView.Fill>
        <RadialGradientBrush Center="0.5,0.5"
                             Radius="0.5">
            <GradientStop Color="Yellow"
                          Offset="0.0" />
            <GradientStop Color="Green"
                          Offset="1.0" />
        </RadialGradientBrush>
    </BoxView.Fill>
</BoxView>
```

In this example, the <xref:Microsoft.Maui.Controls.BoxView> is painted with a yellow-to-green radial gradient:

:::image type="content" source="media/boxview/boxview-radial-fill.png" alt-text="Screenshot of a BoxView painted with a radial gradient brush.":::

The `Fill` property takes priority over the `Color` property. When both are set, the brush specified by `Fill` is used to paint the <xref:Microsoft.Maui.Controls.BoxView>. If `Fill` is later cleared by setting it to `null`, the <xref:Microsoft.Maui.Controls.BoxView> reverts to rendering with `Color`:

```csharp
box.Color = Colors.Red;
box.Fill = new LinearGradientBrush(/* ... */); // Gradient is rendered.
box.Fill = null;                                // Red is rendered.
```

::: moniker-end
