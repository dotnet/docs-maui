---
title: "StackLayout"
description: "A .NET MAUI StackLayout organizes child views in a one-dimensional stack, either horizontally or vertically."
ms.date: 12/28/2021
---

# StackLayout

<!-- Sample link goes here -->

:::image type="content" source="media/stacklayout/layouts.png" alt-text=".NET MAUI StackLayout." border="false":::

A .NET Multi-platform App UI (.NET MAUI) `StackLayout` organizes child views in a one-dimensional stack, either horizontally or vertically. By default, a `StackLayout` is oriented vertically. In addition, a `StackLayout` can be used as a parent layout that contains other child layouts.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `StackLayout` class defines the following properties:

- `Orientation`, of type `StackOrientation`, represents the direction in which child views are positioned. The default value of this property is `Vertical`.
- `Spacing`, of type `double`, indicates the amount of space between each child view. The default value of this property is 0.

These properties are backed by `BindableProperty` objects, which means that the properties can be targets of data bindings and styled.

<!-- > [!TIP]
> To obtain the best possible layout performance, follow the guidelines at [Optimize layout performance](~/xamarin-forms/deploy-test/performance.md#optimize-layout-performance). -->

## Vertical orientation

The following XAML shows how to create a vertically oriented `StackLayout` that contains different child views:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.XAML.VerticalStackLayoutPage"
             Title="Vertical StackLayout demo">
    <StackLayout Margin="20">
        <Label Text="Primary colors" />
        <BoxView Color="Red"
                 HeightRequest="40" />
        <BoxView Color="Yellow"
                 HeightRequest="40" />
        <BoxView Color="Blue"
                 HeightRequest="40" />
        <Label Text="Secondary colors" />
        <BoxView Color="Green"
                 HeightRequest="40" />
        <BoxView Color="Orange"
                 HeightRequest="40" />
        <BoxView Color="Purple"
                 HeightRequest="40" />
    </StackLayout>
</ContentPage>
```

This example creates a vertical `StackLayout` containing `Label` and `BoxView` objects. By default, there's no space between the child views:

:::image type="content" source="media/stacklayout/vertical.png" alt-text="Vertically oriented .NET MAUI StackLayout.":::

The equivalent C# code is:

```csharp
public class VerticalStackLayoutPage : ContentPage
{
    public VerticalStackLayoutPage()
    {
        Title = "Vertical StackLayout demo";

        StackLayout stackLayout = new StackLayout { Margin = new Thickness(20) };

        stackLayout.Add(new Label { Text = "Primary colors" });
        stackLayout.Add(new BoxView { Color = Colors.Red, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Yellow, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Blue, HeightRequest = 40 });
        stackLayout.Add(new Label { Text = "Secondary colors" });
        stackLayout.Add(new BoxView { Color = Colors.Green, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Orange, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Purple, HeightRequest = 40 });

        Content = stackLayout;
    }
}
```

> [!NOTE]
> The value of the `Margin` property represents the distance between an element and its adjacent elements. <!--For more information, see [Margin and Padding](margin-and-padding.md).-->

## Horizontal orientation

The following XAML shows how to create a horizontally oriented `StackLayout` by setting its `Orientation` property to `Horizontal`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.XAML.HorizontalStackLayoutPage"
             Title="Horizontal StackLayout demo">
    <StackLayout Margin="20"
                 Orientation="Horizontal"
                 HorizontalOptions="Center">
        <BoxView Color="Red"
                 WidthRequest="40" />
        <BoxView Color="Yellow"
                 WidthRequest="40" />
        <BoxView Color="Blue"
                 WidthRequest="40" />
        <BoxView Color="Green"
                 WidthRequest="40" />
        <BoxView Color="Orange"
                 WidthRequest="40" />
        <BoxView Color="Purple"
                 WidthRequest="40" />
    </StackLayout>
</ContentPage>
```

This example creates a horizontal `StackLayout` containing `BoxView` objects, with no space between the child views:

:::image type="content" source="media/stacklayout/horizontal.png" alt-text="Horizontally oriented .NET MAUI StackLayout.":::

The equivalent C# code is:

```csharp
public class HorizontalStackLayoutPage : ContentPage
{
    public HorizontalStackLayoutPage()
    {
        Title = "Horizontal StackLayout demo";

        StackLayout stackLayout = new StackLayout
        {
            Margin = new Thickness(20),
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center
        };

        stackLayout.Add(new BoxView { Color = Colors.Red, WidthRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Yellow, WidthRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Blue, WidthRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Green, WidthRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Orange, WidthRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Purple, WidthRequest = 40 });

        Content = stackLayout;
    }
}
```

## Space between child views

The spacing between child views in a `StackLayout` can be changed by setting the `Spacing` property to a `double` value:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.XAML.StackLayoutSpacingPage"
             Title="StackLayout Spacing demo">
    <StackLayout Margin="20"
                 Spacing="6">
        <Label Text="Primary colors" />
        <BoxView Color="Red"
                 HeightRequest="40" />
        <BoxView Color="Yellow"
                 HeightRequest="40" />
        <BoxView Color="Blue"
                 HeightRequest="40" />
        <Label Text="Secondary colors" />
        <BoxView Color="Green"
                 HeightRequest="40" />
        <BoxView Color="Orange"
                 HeightRequest="40" />
        <BoxView Color="Purple"
                 HeightRequest="40" />
    </StackLayout>
</ContentPage>
```

This example creates a vertical `StackLayout` containing `Label` and `BoxView` objects that have six device-independent units of vertical space between them:

:::image type="content" source="media/stacklayout/spacing.png" alt-text="Vertically oriented .NET MAUI StackLayout with spacing between child views.":::

> [!TIP]
> The `Spacing` property can be set to negative values to make child views overlap.

The equivalent C# code is:

```csharp
public class StackLayoutSpacingPage : ContentPage
{
    public StackLayoutSpacingPage()
    {
        Title = "StackLayout Spacing demo";

        StackLayout stackLayout = new StackLayout
        {
            Margin = new Thickness(20),
            Spacing = 6
        };

        stackLayout.Add(new Label { Text = "Primary colors" });
        stackLayout.Add(new BoxView { Color = Colors.Red, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Yellow, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Blue, HeightRequest = 40 });
        stackLayout.Add(new Label { Text = "Secondary colors" });
        stackLayout.Add(new BoxView { Color = Colors.Green, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Orange, HeightRequest = 40 });
        stackLayout.Add(new BoxView { Color = Colors.Purple, HeightRequest = 40 });

        Content = stackLayout;
    }
}
```

## Position and size of child views

The size and position of child views within a `StackLayout` depends upon the values of the child views' `HeightRequest` and `WidthRequest` properties, and the values of their `HorizontalOptions` and `VerticalOptions` properties. In a vertical `StackLayout`, child views expand to fill the available width when their size isn't explicitly set. Similarly, in a horizontal `StackLayout`, child views expand to fill the available height when their size isn't explicitly set.

The `HorizontalOptions` and `VerticalOptions` properties of a `StackLayout`, and its child views, can be set to fields from the `LayoutOptions` struct, which encapsulates an *alignment* layout preference. This layout preference determines the position and size of a child view within its parent layout.

The following XAML example sets alignment preferences on each child view in the `StackLayout`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.XAML.AlignmentPage"
             Title="Alignment demo">
    <StackLayout Margin="20"
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
    </StackLayout>
</ContentPage>
```

In this example, alignment preferences are set on the `Label` objects to control their position within the `StackLayout`. The `Start`, `Center`, `End`, and `Fill` fields are used to define the alignment of the `Label` objects within the parent `StackLayout`:

:::image type="content" source="media/stacklayout/alignment.png" alt-text=".NET MAUI StackLayout with alignment options specified.":::

A `StackLayout` only respects the alignment preferences on child views that are in the opposite direction to the `StackLayout` orientation. Therefore, the `Label` child views within the vertically oriented `StackLayout` set their `HorizontalOptions` properties to one of the alignment fields:

- `Start`, which positions the `Label` on the left-hand side of the `StackLayout`.
- `Center`, which centers the `Label` in the `StackLayout`.
- `End`, which positions the `Label` on the right-hand side of the `StackLayout`.
- `Fill`, which ensures that the `Label` fills the width of the `StackLayout`.

The equivalent C# code is:

```csharp
public class AlignmentPage : ContentPage
{
    public AlignmentPage()
    {
        Title = "Alignment demo";

        StackLayout stackLayout = new StackLayout
        {
            Margin = new Thickness(20),
            Spacing = 6
        };

        stackLayout.Add(new Label { Text = "Start", BackgroundColor = Colors.Gray, HorizontalOptions = LayoutOptions.Start });
        stackLayout.Add(new Label { Text = "Center", BackgroundColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center });
        stackLayout.Add(new Label { Text = "End", BackgroundColor = Colors.Gray, HorizontalOptions = LayoutOptions.End });
        stackLayout.Add(new Label { Text = "Fill", BackgroundColor = Colors.Gray, HorizontalOptions = LayoutOptions.Fill });

        Content = stackLayout;
    }
}
```

<!-- For more information about alignment, see [Layout Options in .NET MAUI](layout-options.md). -->

## Nested StackLayout objects

A `StackLayout` can be used as a parent layout that contains nested child `StackLayout` objects, or other child layouts.

The following XAML shows an example of nesting `StackLayout` objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StackLayoutDemos.Views.XAML.CombinedStackLayoutPage"
             Title="Combined StackLayouts demo">
    <StackLayout Margin="20">
        ...
        <Frame BorderColor="Black"
               Padding="5">
            <StackLayout Orientation="Horizontal"
                         Spacing="15">
                <BoxView Color="Red"
                         WidthRequest="40" />
                <Label Text="Red"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </StackLayout>
        </Frame>
        <Frame BorderColor="Black"
               Padding="5">
            <StackLayout Orientation="Horizontal"
                         Spacing="15">
                <BoxView Color="Yellow"
                         WidthRequest="40" />
                <Label Text="Yellow"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </StackLayout>
        </Frame>
        <Frame BorderColor="Black"
               Padding="5">
            <StackLayout Orientation="Horizontal"
                         Spacing="15">
                <BoxView Color="Blue"
                         WidthRequest="40" />
                <Label Text="Blue"
                       FontSize="Large"
                       VerticalOptions="Center" />
            </StackLayout>
        </Frame>
        ...
    </StackLayout>
</ContentPage>
```

In this example, the parent `StackLayout` contains nested `StackLayout` objects inside `Frame` objects. The parent `StackLayout` is oriented vertically, while the child `StackLayout` objects are oriented horizontally:

:::image type="content" source="media/stacklayout/nested.png" alt-text="Nested .NET MAUI StackLayouts.":::

> [!IMPORTANT]
> The deeper you nest `StackLayout` objects and other layouts, the more the nested layouts will impact performance. <!--For more information, see [Choose the correct layout](~/xamarin-forms/deploy-test/performance.md#choose-the-correct-layout).-->

The equivalent C# code is:

```csharp
public class CombinedStackLayoutPage : ContentPage
{
    public CombinedStackLayoutPage()
    {
        Title = "Combined StackLayouts demo";

        Frame frame1 = new Frame
        {
            BorderColor = Colors.Black,
            Padding = new Thickness(5)
        };
        StackLayout frame1StackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 15
        };
        frame1StackLayout.Add(new BoxView { Color = Colors.Red, WidthRequest = 40 });
        frame1StackLayout.Add(new Label { Text = "Red", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), VerticalOptions = LayoutOptions.Center });
        frame1.Content = frame1StackLayout;

        Frame frame2 = new Frame
        {
            BorderColor = Colors.Black,
            Padding = new Thickness(5)
        };
        StackLayout frame2StackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 15
        };
        frame2StackLayout.Add(new BoxView { Color = Colors.Yellow, WidthRequest = 40 });
        frame2StackLayout.Add(new Label { Text = "Yellow", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), VerticalOptions = LayoutOptions.Center });
        frame2.Content = frame2StackLayout;

        Frame frame3 = new Frame
        {
            BorderColor = Colors.Black,
            Padding = new Thickness(5)
        };
        StackLayout frame3StackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 15
        };
        frame3StackLayout.Add(new BoxView { Color = Colors.Blue, WidthRequest = 40 });
        frame3StackLayout.Add(new Label { Text = "Blue", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), VerticalOptions = LayoutOptions.Center });
        frame3.Content = frame3StackLayout;

        ...

        StackLayout stackLayout = new StackLayout { Margin = new Thickness(20) };
        stackLayout.Add(new Label { Text = "Primary colors" });
        stackLayout.Add(frame1);
        stackLayout.Add(frame2);
        stackLayout.Add(frame3);
        stackLayout.Add(new Label { Text = "Secondary colors" });
        stackLayout.Add(frame4);
        stackLayout.Add(frame5);
        stackLayout.Add(frame6);

        Content = stackLayout;
    }
}
```
