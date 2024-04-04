---
title: "Frame"
description: "The .NET MAUI Frame is used to wrap a view or layout with a border that can be configured with color, shadow, and other options."
ms.date: 04/12/2022
---

# Frame

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Frame> is used to wrap a view or layout with a border that can be configured with color, shadow, and other options. Frames can be used to create borders around controls but can also be used to create more complex UI.

The <xref:Microsoft.Maui.Controls.Frame> class defines the following properties:

- `BorderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, determines the color of the <xref:Microsoft.Maui.Controls.Frame> border.
- `CornerRadius`, of type `float`, determines the rounded radius of the corner.
- `HasShadow`, of type `bool`, determines whether the frame has a drop shadow.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.Frame> class inherits from <xref:Microsoft.Maui.Controls.ContentView>, which provides a `Content` bindable property. The `Content` property is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of the <xref:Microsoft.Maui.Controls.Frame> class, and therefore does not need to be explicitly set from XAML.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Frame> class existed in Xamarin.Forms and is present in .NET MAUI for users who are migrating their apps from Xamarin.Forms to .NET MAUI. If you're building a new .NET MAUI app it's recommended to use <xref:Microsoft.Maui.Controls.Border> instead, and to set shadows using the `Shadow` bindable property on <xref:Microsoft.Maui.Controls.VisualElement>. For more information, see [Border](border.md) and [Shadow](../shadow.md).

## Create a Frame

A <xref:Microsoft.Maui.Controls.Frame> object typically wraps another control, such as a <xref:Microsoft.Maui.Controls.Label>:

```xaml
<Frame>
  <Label Text="Frame wrapped around a Label" />
</Frame>
```

The appearance of <xref:Microsoft.Maui.Controls.Frame> objects can be customized by setting properties:

```xaml
<Frame BorderColor="Gray"
       CornerRadius="10">
  <Label Text="Frame wrapped around a Label" />
</Frame>
```

The equivalent C# code is:

```csharp
Frame frame = new Frame
{
    BorderColor = Colors.Gray,
    CornerRadius = 10,
    Content = new Label { Text = "Frame wrapped around a Label" }
};
```

The following screenshot shows the example <xref:Microsoft.Maui.Controls.Frame>:

:::image type="content" source="media/frame/frame.png" alt-text="Screenshot of Frame examples.":::

## Create a card with a Frame

Combining a <xref:Microsoft.Maui.Controls.Frame> object with a layout such as a <xref:Microsoft.Maui.Controls.StackLayout> enables the creation of more complex UI.

The following XAML shows how to create a card with a <xref:Microsoft.Maui.Controls.Frame>:

```xaml
<Frame BorderColor="Gray"
       CornerRadius="5"
       Padding="8">
  <StackLayout>
    <Label Text="Card Example"
           FontSize="14"
           FontAttributes="Bold" />
    <BoxView Color="Gray"
             HeightRequest="2"
             HorizontalOptions="Fill" />
    <Label Text="Frames can wrap more complex layouts to create more complex UI components, such as this card!"/>
  </StackLayout>
</Frame>
```

The following screenshot shows the example card:

:::image type="content" source="media/frame/frame-card.png" alt-text="Screenshot of a card created with a Frame.":::

## Round elements

The `CornerRadius` property of the <xref:Microsoft.Maui.Controls.Frame> control is one approach to creating a circle image. The following XAML shows how to create a circle image with a <xref:Microsoft.Maui.Controls.Frame>:

```xaml
<Frame Margin="10"
       BorderColor="Black"
       CornerRadius="50"
       HeightRequest="60"
       WidthRequest="60"
       IsClippedToBounds="True"
       HorizontalOptions="Center"
       VerticalOptions="Center">
  <Image Source="outdoors.jpg"
         Aspect="AspectFill"
         Margin="-20"
         HeightRequest="100"
         WidthRequest="100" />
</Frame>
```

The following screenshot shows the example circle image:

:::image type="content" source="media/frame/circle-image.png" alt-text="Screenshot of a circle image created with a Frame.":::
