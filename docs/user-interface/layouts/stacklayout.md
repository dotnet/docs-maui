---
title: "StackLayout"
description: "The .NET MAUI StackLayout organizes child views in a one-dimensional stack, either horizontally or vertically."
ms.date: 12/28/2021
---

# StackLayout

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-stacklayout)

:::image type="content" source="media/stacklayout/layouts.png" alt-text=".NET MAUI StackLayout." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.StackLayout> organizes child views in a one-dimensional stack, either horizontally or vertically. By default, a <xref:Microsoft.Maui.Controls.StackLayout> is oriented vertically. In addition, a <xref:Microsoft.Maui.Controls.StackLayout> can be used as a parent layout that contains other child layouts.

The <xref:Microsoft.Maui.Controls.StackLayout> class defines the following properties:

- `Orientation`, of type `StackOrientation`, represents the direction in which child views are positioned. The default value of this property is `Vertical`.
- `Spacing`, of type `double`, indicates the amount of space between each child view. The default value of this property is 0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings and styled.

## Vertical orientation

The following XAML shows how to create a vertically oriented <xref:Microsoft.Maui.Controls.StackLayout> that contains different child views:

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

This example creates a vertical <xref:Microsoft.Maui.Controls.StackLayout> containing <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.BoxView> objects. By default, there's no space between the child views:

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
> The value of the `Margin` property represents the distance between an element and its adjacent elements. For more information, see [Position controls](~/user-interface/align-position.md#position-controls).

## Horizontal orientation

The following XAML shows how to create a horizontally oriented <xref:Microsoft.Maui.Controls.StackLayout> by setting its `Orientation` property to `Horizontal`:

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

This example creates a horizontal <xref:Microsoft.Maui.Controls.StackLayout> containing <xref:Microsoft.Maui.Controls.BoxView> objects, with no space between the child views:

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

The spacing between child views in a <xref:Microsoft.Maui.Controls.StackLayout> can be changed by setting the `Spacing` property to a `double` value:

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

This example creates a vertical <xref:Microsoft.Maui.Controls.StackLayout> containing <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.BoxView> objects that have six device-independent units of vertical space between them:

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

The size and position of child views within a <xref:Microsoft.Maui.Controls.StackLayout> depends upon the values of the child views' <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> and <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> properties, and the values of their `HorizontalOptions` and `VerticalOptions` properties. In a vertical <xref:Microsoft.Maui.Controls.StackLayout>, child views expand to fill the available width when their size isn't explicitly set. Similarly, in a horizontal <xref:Microsoft.Maui.Controls.StackLayout>, child views expand to fill the available height when their size isn't explicitly set.

The `HorizontalOptions` and `VerticalOptions` properties of a <xref:Microsoft.Maui.Controls.StackLayout>, and its child views, can be set to fields from the `LayoutOptions` struct, which encapsulates an *alignment* layout preference. This layout preference determines the position and size of a child view within its parent layout.

The following XAML example sets alignment preferences on each child view in the <xref:Microsoft.Maui.Controls.StackLayout>:

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

In this example, alignment preferences are set on the <xref:Microsoft.Maui.Controls.Label> objects to control their position within the <xref:Microsoft.Maui.Controls.StackLayout>. The `Start`, `Center`, `End`, and `Fill` fields are used to define the alignment of the <xref:Microsoft.Maui.Controls.Label> objects within the parent <xref:Microsoft.Maui.Controls.StackLayout>:

:::image type="content" source="media/stacklayout/alignment.png" alt-text=".NET MAUI StackLayout with alignment options specified.":::

A <xref:Microsoft.Maui.Controls.StackLayout> only respects the alignment preferences on child views that are in the opposite direction to the <xref:Microsoft.Maui.Controls.StackLayout> orientation. Therefore, the <xref:Microsoft.Maui.Controls.Label> child views within the vertically oriented <xref:Microsoft.Maui.Controls.StackLayout> set their `HorizontalOptions` properties to one of the alignment fields:

- `Start`, which positions the <xref:Microsoft.Maui.Controls.Label> on the left-hand side of the <xref:Microsoft.Maui.Controls.StackLayout>.
- `Center`, which centers the <xref:Microsoft.Maui.Controls.Label> in the <xref:Microsoft.Maui.Controls.StackLayout>.
- `End`, which positions the <xref:Microsoft.Maui.Controls.Label> on the right-hand side of the <xref:Microsoft.Maui.Controls.StackLayout>.
- `Fill`, which ensures that the <xref:Microsoft.Maui.Controls.Label> fills the width of the <xref:Microsoft.Maui.Controls.StackLayout>.

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

For more information about alignment, see [Align views in layouts](~/user-interface/align-position.md#align-views-in-layouts).

## Nested StackLayout objects

A <xref:Microsoft.Maui.Controls.StackLayout> can be used as a parent layout that contains nested child <xref:Microsoft.Maui.Controls.StackLayout> objects, or other child layouts.

The following XAML shows an example of nesting <xref:Microsoft.Maui.Controls.StackLayout> objects:

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
                       FontSize="18"
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
                       FontSize="18"
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
                       FontSize="18"
                       VerticalOptions="Center" />
            </StackLayout>
        </Frame>
        ...
    </StackLayout>
</ContentPage>
```

In this example, the parent <xref:Microsoft.Maui.Controls.StackLayout> contains nested <xref:Microsoft.Maui.Controls.StackLayout> objects inside <xref:Microsoft.Maui.Controls.Frame> objects. The parent <xref:Microsoft.Maui.Controls.StackLayout> is oriented vertically, while the child <xref:Microsoft.Maui.Controls.StackLayout> objects are oriented horizontally:

:::image type="content" source="media/stacklayout/nested.png" alt-text="Nested .NET MAUI StackLayouts.":::

> [!IMPORTANT]
> The deeper you nest <xref:Microsoft.Maui.Controls.StackLayout> objects and other layouts, the more layout calculations will be performed which may impact performance. For more information, see [Choose the correct layout](~/deployment/performance.md#choose-the-correct-layout).

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
        frame1StackLayout.Add(new Label { Text = "Red", FontSize = 22, VerticalOptions = LayoutOptions.Center });
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
        frame2StackLayout.Add(new Label { Text = "Yellow", FontSize = 22, VerticalOptions = LayoutOptions.Center });
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
        frame3StackLayout.Add(new Label { Text = "Blue", FontSize = 22, VerticalOptions = LayoutOptions.Center });
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
