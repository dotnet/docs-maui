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
