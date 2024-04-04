---
title: "VerticalStackLayout"
description: "Learn how the .NET MAUI VerticalStackLayout organizes child views in a one-dimensional vertical stack."
ms.date: 12/06/2021
---

# VerticalStackLayout

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.VerticalStackLayout> organizes child views in a one-dimensional vertical stack, and is a more performant alternative to a <xref:Microsoft.Maui.Controls.StackLayout>. In addition, a <xref:Microsoft.Maui.Controls.VerticalStackLayout> can be used as a parent layout that contains other child layouts.

The <xref:Microsoft.Maui.Controls.VerticalStackLayout> defines the following properties:

- `Spacing`, of type `double`, indicates the amount of space between each child view. The default value of this property is 0.

This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings and styled.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.VerticalStackLayout> that contains different child views:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20">
        <Label Text="Primary colors" />
        <Rectangle Fill="Red"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Yellow"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Blue"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Label Text="Secondary colors" />
        <Rectangle Fill="Green"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Orange"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Purple"
                   HeightRequest="30"
                   WidthRequest="300" />
    </VerticalStackLayout>
</ContentPage>
```

This example creates a <xref:Microsoft.Maui.Controls.VerticalStackLayout> containing <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.Shapes.Rectangle> objects. By default, there is no space between the child views:

:::image type="content" source="media/verticalstacklayout/basic.png" alt-text="VerticalStackLayout displaying different child views screenshot.":::

> [!NOTE]
> The value of the `Margin` property represents the distance between an element and its adjacent elements. For more information, see [Position controls](~/user-interface/align-position.md#position-controls).

## Space between child views

The spacing between child views in a <xref:Microsoft.Maui.Controls.VerticalStackLayout> can be changed by setting the `Spacing` property to a `double` value:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20"
                         Spacing="10">
        <Label Text="Primary colors" />
        <Rectangle Fill="Red"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Yellow"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Blue"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Label Text="Secondary colors" />
        <Rectangle Fill="Green"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Orange"
                   HeightRequest="30"
                   WidthRequest="300" />
        <Rectangle Fill="Purple"
                   HeightRequest="30"
                   WidthRequest="300" />
    </VerticalStackLayout>
</ContentPage>
```

This example creates a <xref:Microsoft.Maui.Controls.VerticalStackLayout> containing <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.Shapes.Rectangle> objects that have ten device-independent units of space between the child views:

:::image type="content" source="media/verticalstacklayout/spacing.png" alt-text="VerticalStackLayout displaying different child views with spacing screenshot.":::

> [!TIP]
> The `Spacing` property can be set to negative values to make child views overlap.

## Position and size child views

The size and position of child views within a <xref:Microsoft.Maui.Controls.VerticalStackLayout> depends upon the values of the child views' <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> and <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> properties, and the values of their `HorizontalOptions` properties. In a <xref:Microsoft.Maui.Controls.VerticalStackLayout>, child views expand to fill the available width when their size isn't explicitly set.

The `HorizontalOptions` properties of a <xref:Microsoft.Maui.Controls.VerticalStackLayout>, and its child views, can be set to fields from the `LayoutOptions` struct, which encapsulates an *alignment* layout preference. This layout preference determines the position and size of a child view within its parent layout.

> [!TIP]
> Don't set the `HorizontalOptions` property of a <xref:Microsoft.Maui.Controls.VerticalStackLayout> unless you need to. The default value of `LayoutOptions.Fill` allows for the best layout optimization. Changing this property has a cost and consumes memory, even when setting it back to its default value.

The following XAML example sets alignment preferences on each child view in the <xref:Microsoft.Maui.Controls.VerticalStackLayout>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20"
                         Spacing="6">
        <Label Text="Start"
               BackgroundColor="Gray"
               HorizontalOptions="Start" />
        <Label Text="Center"
               BackgroundColor="Gray"
               HorizontalOptions="Center" />
        <Label Text="End"
               BackgroundColor="Gray"
               HorizontalOptions="End" />
        <Label Text="Fill"
               BackgroundColor="Gray"
               HorizontalOptions="Fill" />
    </VerticalStackLayout>
</ContentPage>
```

In this example, alignment preferences are set on the <xref:Microsoft.Maui.Controls.Label> objects to control their position within the <xref:Microsoft.Maui.Controls.VerticalStackLayout>. The `Start`, `Center`, `End`, and `Fill` fields are used to define the alignment of the <xref:Microsoft.Maui.Controls.Label> objects within the parent <xref:Microsoft.Maui.Controls.VerticalStackLayout>:

:::image type="content" source="media/verticalstacklayout/alignment.png" alt-text="VerticalStackLayout displaying aligned child views screenshot.":::

A <xref:Microsoft.Maui.Controls.VerticalStackLayout> only respects the alignment preferences on child views that are in the opposite direction to the orientation of the layout. Therefore, the <xref:Microsoft.Maui.Controls.Label> child views within the <xref:Microsoft.Maui.Controls.VerticalStackLayout> set their `HorizontalOptions` properties to one of the alignment fields:

- `Start`, which positions the <xref:Microsoft.Maui.Controls.Label> on the left-hand side of the <xref:Microsoft.Maui.Controls.VerticalStackLayout>.
- `Center`, which centers the <xref:Microsoft.Maui.Controls.Label> in the <xref:Microsoft.Maui.Controls.VerticalStackLayout>.
- `End`, which positions the <xref:Microsoft.Maui.Controls.Label> on the right-hand side of the <xref:Microsoft.Maui.Controls.VerticalStackLayout>.
- `Fill`, which ensures that the <xref:Microsoft.Maui.Controls.Label> fills the width of the <xref:Microsoft.Maui.Controls.VerticalStackLayout>.

<!--
For more information about alignment, see [Layout Options in .NET MAUI](layout-options.md).
-->

## Nest VerticalStackLayout objects

A <xref:Microsoft.Maui.Controls.VerticalStackLayout> can be used as a parent layout that contains other nested child layouts.

The following XAML shows an example of nesting <xref:Microsoft.Maui.Controls.HorizontalStackLayout> objects in a <xref:Microsoft.Maui.Controls.VerticalStackLayout>:

```xaml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20"
                         Spacing="6">
       <Label Text="Primary colors" />
       <Frame BorderColor="Black"
              Padding="5">
           <HorizontalStackLayout Spacing="15">
               <Rectangle Fill="Red"
                          HeightRequest="30"
                          WidthRequest="30" />
               <Label Text="Red"
                      FontSize="18" />
           </HorizontalStackLayout>
       </Frame>
       <Frame BorderColor="Black"
              Padding="5">
           <HorizontalStackLayout Spacing="15">
               <Rectangle Fill="Yellow"
                          HeightRequest="30"
                          WidthRequest="30" />
               <Label Text="Yellow"
                      FontSize="18" />
           </HorizontalStackLayout>
       </Frame>
       <Frame BorderColor="Black"
              Padding="5">
           <HorizontalStackLayout Spacing="15">
               <Rectangle Fill="Blue"
                          HeightRequest="30"
                          WidthRequest="30" />
               <Label Text="Blue"
                      FontSize="18" />
           </HorizontalStackLayout>
       </Frame>
    </VerticalStackLayout>
</ContentPage>
```

In this example, the parent <xref:Microsoft.Maui.Controls.VerticalStackLayout> contains nested <xref:Microsoft.Maui.Controls.HorizontalStackLayout> objects inside <xref:Microsoft.Maui.Controls.Frame> objects:

:::image type="content" source="media/verticalstacklayout/nested.png" alt-text="VerticalStackLayout displaying nested HorizontalStackLayout objects screenshot.":::

> [!IMPORTANT]
> The deeper you nest layout objects the more layout calculations will be performed, which may impact performance. For more information, see [Choose the correct layout](~/deployment/performance.md#choose-the-correct-layout).
