---
title: ".NET MAUI HorizontalStackLayout"
description: "Learn how the .NET MAUI HorizontalStackLayout organizes child views in a one-dimensional horizontal stack."
ms.date: 09/30/2024
---

# HorizontalStackLayout

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.HorizontalStackLayout> organizes child views in a one-dimensional horizontal stack, and is a more performant alternative to a <xref:Microsoft.Maui.Controls.StackLayout>. In addition, a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> can be used as a parent layout that contains other child layouts.

The <xref:Microsoft.Maui.Controls.HorizontalStackLayout> defines the following properties:

- `Spacing`, of type `double`, indicates the amount of space between each child view. The default value of this property is 0.

This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings and styled.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> that contains different child views:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.HorizontalStackLayoutPage">
    <HorizontalStackLayout Margin="20">
       <Rectangle Fill="Red"
                  HeightRequest="30"
                  WidthRequest="30" />
       <Label Text="Red"
              FontSize="18" />
    </HorizontalStackLayout>
</ContentPage>
```

This example creates a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> containing a <xref:Microsoft.Maui.Controls.Shapes.Rectangle> and a <xref:Microsoft.Maui.Controls.Label> object. By default, there is no space between the child views:

:::image type="content" source="media/horizontalstacklayout/basic.png" alt-text="HorizontalStackLayout displaying two views screenshot.":::

> [!NOTE]
> The value of the `Margin` property represents the distance between an element and its adjacent elements. For more information, see [Position controls](~/user-interface/align-position.md#position-controls).

## Space between child views

The spacing between child views in a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> can be changed by setting the `Spacing` property to a `double` value:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.HorizontalStackLayoutPage">
    <HorizontalStackLayout Margin="20"
                           Spacing="10">
       <Rectangle Fill="Red"
                  HeightRequest="30"
                  WidthRequest="30" />
       <Label Text="Red"
              FontSize="18" />
    </HorizontalStackLayout>
</ContentPage>
```

This example creates a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> containing a <xref:Microsoft.Maui.Controls.Shapes.Rectangle> and a <xref:Microsoft.Maui.Controls.Label> object, that have ten device-independent units of space between them:

:::image type="content" source="media/horizontalstacklayout/spacing.png" alt-text="HorizontalStackLayout displaying two spaced views screenshot.":::

> [!TIP]
> The `Spacing` property can be set to negative values to make child views overlap.

## Position and size child views

The size and position of child views within a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> depends upon the values of the child views' <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> and <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> properties, and the values of their `VerticalOptions` properties. In a <xref:Microsoft.Maui.Controls.HorizontalStackLayout>, child views expand to fill the available height when their size isn't explicitly set.

The `VerticalOptions` properties of a <xref:Microsoft.Maui.Controls.HorizontalStackLayout>, and its child views, can be set to fields from the `LayoutOptions` struct, which encapsulates an *alignment* layout preference. This layout preference determines the position and size of a child view within its parent layout.

The following XAML example sets alignment preferences on each child view in the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.HorizontalStackLayoutPage">
    <HorizontalStackLayout Margin="20"
                           HeightRequest="200">
        <Label Text="Start"
               BackgroundColor="Gray"
               VerticalOptions="Start" />
        <Label Text="Center"
               BackgroundColor="Gray"
               VerticalOptions="Center" />
        <Label Text="End"
               BackgroundColor="Gray"
               VerticalOptions="End" />
        <Label Text="Fill"
               BackgroundColor="Gray"
               VerticalOptions="Fill" />
    </HorizontalStackLayout>
</ContentPage>
```

In this example, alignment preferences are set on the <xref:Microsoft.Maui.Controls.Label> objects to control their position within the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>. The `Start`, `Center`, `End`, and `Fill` fields are used to define the alignment of the <xref:Microsoft.Maui.Controls.Label> objects within the parent <xref:Microsoft.Maui.Controls.HorizontalStackLayout>:

:::image type="content" source="media/horizontalstacklayout/alignment.png" alt-text="HorizontalStackLayout displaying aligned views screenshot.":::

A <xref:Microsoft.Maui.Controls.HorizontalStackLayout> only respects the alignment preferences on child views that are in the opposite direction to the orientation of the layout. Therefore, the <xref:Microsoft.Maui.Controls.Label> child views within the <xref:Microsoft.Maui.Controls.HorizontalStackLayout> set their `VerticalOptions` properties to one of the alignment fields:

- `Start`, which positions the <xref:Microsoft.Maui.Controls.Label> at the start of the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>.
- `Center`, which vertically centers the <xref:Microsoft.Maui.Controls.Label> in the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>.
- `End`, which positions the <xref:Microsoft.Maui.Controls.Label> at the end of the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>.
- `Fill`, which ensures that the <xref:Microsoft.Maui.Controls.Label> fills the height of the <xref:Microsoft.Maui.Controls.HorizontalStackLayout>.

For more information about alignment, see [Align and position .NET MAUI controls](~/user-interface/align-position.md).

## Nest HorizontalStackLayout objects

A <xref:Microsoft.Maui.Controls.HorizontalStackLayout> can be used as a parent layout that contains other nested child layouts.

The following XAML shows an example of nesting <xref:Microsoft.Maui.Controls.VerticalStackLayout> objects in a <xref:Microsoft.Maui.Controls.HorizontalStackLayout>:

```xaml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.HorizontalStackLayoutPage">
    <HorizontalStackLayout Margin="20"
                           Spacing="6">
        <Label Text="Primary colors:" />
        <VerticalStackLayout Spacing="6">
            <Rectangle Fill="Red"
                       WidthRequest="30"
                       HeightRequest="30" />
            <Rectangle Fill="Yellow"
                       WidthRequest="30"
                       HeightRequest="30" />
            <Rectangle Fill="Blue"
                       WidthRequest="30"
                       HeightRequest="30" />
        </VerticalStackLayout>
        <Label Text="Secondary colors:" />
        <VerticalStackLayout Spacing="6">
            <Rectangle Fill="Green"
                       WidthRequest="30"
                       HeightRequest="30" />
            <Rectangle Fill="Orange"
                       WidthRequest="30"
                       HeightRequest="30" />
            <Rectangle Fill="Purple"
                       WidthRequest="30"
                       HeightRequest="30" />
        </VerticalStackLayout>
    </HorizontalStackLayout>
</ContentPage>
```

In this example, the parent <xref:Microsoft.Maui.Controls.HorizontalStackLayout> contains two nested <xref:Microsoft.Maui.Controls.VerticalStackLayout> objects:

:::image type="content" source="media/horizontalstacklayout/nested.png" alt-text="HorizontalStackLayout displaying two nested HorizontalStackLayout objects screenshot.":::

> [!IMPORTANT]
> The deeper you nest layout objects the more layout calculations will be performed, which may impact performance. For more information, see [Choose the correct layout](~/deployment/performance.md#choose-the-correct-layout).
