---
title: "BoxView"
description: "The .NET MAUI BoxView draws a simple rectangle or square, of a specified width, height, and color."
ms.date: 02/10/2022
---

# BoxView

The .NET Multi-platform App UI (.NET MAUI) `BoxView` draws a simple rectangle or square, of a specified width, height, and color.

`BoxView` defines the following properties:

- `Color`, of type `Color`, which defines the color of the `BoxView`.
- `CornerRadius`, of type `CornerRadius`, which defines the corner radius of the `BoxView`. This property can be set to a single `double` uniform corner radius value, or a `CornerRadius` structure defined by four `double` values that are applied to the top left, top right, bottom left, and bottom right of the `BoxView`.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

> [!NOTE]
> Although `BoxView` can mimic simple graphics, a better alternative is to use .NET MAUI Shapes or [.NET MAUI Graphics](~/user-interface/graphics/index.md).

## Create a BoxView

To draw a rectangle or square, create a `BoxView` object and set its `Color`, `WidthRequest`, and `HeightRequest` properties. Optionally, you can also set its `CornerRadius` property.

The following XAML example shows how to create a `BoxView`:

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

In this example, a cornflower blue `BoxView` is displayed in the center of the page:

:::image type="content" source="media/boxview/boxview-basic.png" alt-text="Screenshot of a BoxView with rounded corners.":::

The `WidthRequest` and `HeightRequest` properties are measured in device-independent units.

> [!NOTE]
> A `BoxView` can also be a child of an `AbsoluteLayout`. In this case, both the location and size of the `BoxView` are set using the `LayoutBounds` attached bindable property.

A `BoxView` can also be sized to resemble a line of a specific width and thickness.
