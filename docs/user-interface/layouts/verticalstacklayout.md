---
title: ".NET MAUI VerticalStackLayout"
description: "The .NET MAUI VerticalStackLayout organizes child views in a one-dimensional vertical stack."
ms.date: 07/08/2021
---

# .NET MAUI VerticalStackLayout

A `VerticalStackLayout` organizes child views in a one-dimensional vertical stack, and is a more performant alternative to a `StackLayout`. In addition, a `VerticalStackLayout` can be used as a parent layout that contains other child layouts.

The `VerticalStackLayout` defines the following properties:

- `Spacing`, of type `double`, indicates the amount of space between each child view. The default value of this property is 0.

This property is backed by a `BindableProperty` object, which means that it can be the target of data bindings and styled.

The `VerticalStackLayout` class ultimately derives from the `Layout` class, which defines a `Children` property of type `IList<IView>`. The `Children` property is the `ContentProperty` of the `Layout` class, and therefore does not need to be explicitly set from XAML.

<!--
> [!TIP]
> To obtain the best possible layout performance, follow the guidelines at [Optimize layout performance](~/xamarin-forms/deploy-test/performance.md#optimize-layout-performance).
-->

The following XAML shows how to create a `VerticalStackLayout` that contains different child views:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20">
        <Label Text="Primary colors" />
        <BoxView Color="Red" />
        <BoxView Color="Yellow" />
        <BoxView Color="Blue" />
        <Label Text="Secondary colors" />
        <BoxView Color="Green" />
        <BoxView Color="Orange" />
        <BoxView Color="Purple" />
    </VerticalStackLayout>
</ContentPage>
```

This example creates a `VerticalStackLayout` containing `Label` and `BoxView` objects. By default, there is no space between the child views:

<!--

[![Screenshot of a vertically oriented StackLayout](stacklayout-images/vertical.png "Vertically oriented StackLayout")](stacklayout-images/vertical-large.png#lightbox "Vertically oriented StackLayout")

> [!NOTE]
> The value of the `Margin` property represents the distance between an element and its adjacent elements. For more information, see [Margin and Padding](margin-and-padding.md).
-->

## Space between child views

The spacing between child views in a `VerticalStackLayout` can be changed by setting the `Spacing` property to a `double` value:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.VerticalStackLayoutPage">
    <VerticalStackLayout Margin="20"
                         Spacing="10">
        <Label Text="Primary colors" />
        <BoxView Color="Red" />
        <BoxView Color="Yellow" />
        <BoxView Color="Blue" />
        <Label Text="Secondary colors" />
        <BoxView Color="Green" />
        <BoxView Color="Orange" />
        <BoxView Color="Purple" />
    </VerticalStackLayout>
</ContentPage>
```

This example creates a `VerticalStackLayout` containing `Label` and `BoxView` objects that have ten device-independent units of space between the child views:
<!--
[![Screenshot of a StackLayout without any spacing.](stacklayout-images/spacing.png "StackLayout without any spacing."](stacklayout-images/spacing-large.png#lightbox "StackLayout without any spacing")

> [!TIP]
> The `Spacing` property can be set to negative values to make child views overlap.
-->

## Position and size of child views

The size and position of child views within a `VerticalStackLayout` depends upon the values of the child views' `HeightRequest` and `WidthRequest` properties, and the values of their `HorizontalOptions` and `VerticalOptions` properties. In a `VerticalStackLayout`, child views expand to fill the available width when their size isn't explicitly set.

The `HorizontalOptions` and `VerticalOptions` properties of a `VerticalStackLayout`, and its child views, can be set to fields from the `LayoutOptions` struct, which encapsulates two layout preferences:

- *Alignment* determines the position and size of a child view within its parent layout.
- *Expansion* indicates if the child view should use extra space, if it's available.

<!--
> [!TIP]
> Don't set the `HorizontalOptions` and `VerticalOptions` properties of a `VerticalStackLayout` unless you need to. The default values of `LayoutOptions.Fill` and `LayoutOptions.FillAndExpand` allow for the best layout optimization. Changing these properties has a cost and consumes memory, even when setting them back to the default values.
-->

### Alignment

The following XAML example sets alignment preferences on each child view in the `VerticalStackLayout`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.AlignmentPage"
             Title="Alignment demo">
    <VerticalStackLayout Margin="20">
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

In this example, alignment preferences are set on the `Label` objects to control their position within the `VerticalStackLayout`. The `Start`, `Center`, `End`, and `Fill` fields are used to define the alignment of the `Label` objects within the parent `StackLayout`.

<!--
[![Screenshot of a StackLayout with alignment options set.](stacklayout-images/alignment.png "StackLayout with alignment options."](stacklayout-images/alignment-large.png#lightbox "StackLayout with alignment options")

A `VerticalStackLayout` only respects the alignment preferences on child views that are in the opposite direction to the `StackLayout` orientation. Therefore, the `Label` child views within the vertically oriented `VerticalStackLayout` set their `HorizontalOptions` properties to one of the alignment fields:

- `Start`, which positions the `Label` on the left-hand side of the `VerticalStackLayout`.
- `Center`, which centers the `Label` in the `VerticalStackLayout`.
- `End`, which positions the `Label` on the right-hand side of the `VerticalStackLayout`.
- `Fill`, which ensures that the `Label` fills the width of the `VerticalStackLayout`.
-->

### Expansion

The following XAML example sets expansion preferences on each `Label` in the `VerticalStackLayout`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.ExpansionPage"
             Title="Expansion demo">
    <VerticalStackLayout Margin="20">
        <BoxView BackgroundColor="Red"
                 HeightRequest="1" />
        <Label Text="Start"
               BackgroundColor="Gray"
               VerticalOptions="StartAndExpand" />
        <BoxView BackgroundColor="Red"
                 HeightRequest="1" />
        <Label Text="Center"
               BackgroundColor="Gray"
               VerticalOptions="CenterAndExpand" />
        <BoxView BackgroundColor="Red"
                 HeightRequest="1" />
        <Label Text="End"
               BackgroundColor="Gray"
               VerticalOptions="EndAndExpand" />
        <BoxView BackgroundColor="Red"
                 HeightRequest="1" />
        <Label Text="Fill"
               BackgroundColor="Gray"
               VerticalOptions="FillAndExpand" />
        <BoxView BackgroundColor="Red"
                 HeightRequest="1" />
    </VerticalStackLayout>
</ContentPage>
```

In this example, expansion preferences are set on the `Label` objects to control their size within the `VerticalStackLayout`. The `StartAndExpand`, `CenterAndExpand`, `EndAndExpand`, and `FillAndExpand` fields are used to define the alignment preference, and whether the `Label` will occupy more space if available within the parent `VerticalStackLayout`:

<!--
[![Screenshot of a StackLayout with expansion options set.](stacklayout-images/expansion.png "StackLayout with expansion options."](stacklayout-images/expansion-large.png#lightbox "StackLayout with expansion options")

A `VerticalStackLayout` can only expand child views in the direction of its orientation. Therefore, the vertically oriented `VerticalStackLayout` can expand `Label` child views that set their `VerticalOptions` properties to one of the expansion fields. This means that, for vertical alignment, each `Label` occupies the same amount of space within the `StackLayout`. However, only the final `Label`, which sets its `VerticalOptions` property to `FillAndExpand` has a different size.

> [!TIP]
> When using a `VerticalStackLayout`, ensure that only one child view is set to `LayoutOptions.Expands`. This property ensures that the specified child will occupy the largest space that the `StackLayout` can give to it, and it is wasteful to perform these calculations more than once.

> [!IMPORTANT]
> When all the space in a `StackLayout` is used, expansion preferences have no effect.

For more information about alignment and expansion, see [Layout Options in .NET MAUI](layout-options.md).

-->

## Nest VerticalStackLayout objects

A `VerticalStackLayout` can be used as a parent layout that contains other nested child layouts.

The following XAML shows an example of nesting `HorizontalStackLayout` objects in a `VerticalStackLayout`:

```xaml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.CombinedStackLayoutPage"
             Title="Combined StackLayouts demo">
    <VerticalStackLayout Margin="20">
        ...
        <Frame BorderColor="Black"
               Padding="5">
            <HorizontalStackLayout Spacing="15">
                <BoxView Color="Red" />
                <Label Text="Red"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </HorizontalStackLayout>
        </Frame>
        <Frame BorderColor="Black"
               Padding="5">
            <HorizontalStackLayout Spacing="15">
                <BoxView Color="Yellow" />
                <Label Text="Yellow"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </HorizontalStackLayout>
        </Frame>
        <Frame BorderColor="Black"
               Padding="5">
            <HorizontalStackLayout Spacing="15">
                <BoxView Color="Blue" />
                <Label Text="Blue"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </HorizontalStackLayout>
        </Frame>
        ...
    </VerticalStackLayout>
</ContentPage>
```

In this example, the parent `VerticalStackLayout` contains nested `HorizontalStackLayout` objects inside `Frame` objects.

<!--
[![Screenshot of nested StackLayout objects](stacklayout-images/combined.png "Nested StackLayouts")](stacklayout-images/combined-large.png#lightbox "Nested StackLayouts")

> [!IMPORTANT]
> The deeper you nest layout objects, the more the nested layouts will impact performance. For more information, see [Choose the correct layout](~/xamarin-forms/deploy-test/performance.md#choose-the-correct-layout).
-->
