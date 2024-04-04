---
title: "Layouts"
description: ".NET MAUI layout classes allow you to arrange and group UI controls in your app."
ms.date: 05/13/2022
---

# Layouts

:::image type="content" source="media/layouts.png" alt-text=".NET MAUI layout classes." border="false":::

.NET Multi-platform App UI (.NET MAUI) layout classes allow you to arrange and group UI controls in your application. Choosing a layout class requires knowledge of how the layout positions its child elements, and how the layout sizes its child elements. In addition, it may be necessary to nest layouts to create your desired layout.

## StackLayout

A <xref:Microsoft.Maui.Controls.StackLayout> organizes elements in a one-dimensional stack, either horizontally or vertically. The `Orientation` property specifies the direction of the elements, and the default orientation is `Vertical`. <xref:Microsoft.Maui.Controls.StackLayout> is typically used to arrange a subsection of the UI on a page.

The following XAML shows how to create a vertical <xref:Microsoft.Maui.Controls.StackLayout> containing three <xref:Microsoft.Maui.Controls.Label> objects:

```xaml
<StackLayout Margin="20,35,20,25">
    <Label Text="The StackLayout has its Margin property set, to control the rendering position of the StackLayout." />
    <Label Text="The Padding property can be set to specify the distance between the StackLayout and its children." />
    <Label Text="The Spacing property can be set to specify the distance between views in the StackLayout." />
</StackLayout>
```

In a <xref:Microsoft.Maui.Controls.StackLayout>, if an element's size is not explicitly set, it expands to fill the available width, or height if the `Orientation` property is set to `Horizontal`.

A <xref:Microsoft.Maui.Controls.StackLayout> is often used as a parent layout, which contains other child layouts. However, a <xref:Microsoft.Maui.Controls.StackLayout> should not be used to reproduce a <xref:Microsoft.Maui.Controls.Grid> layout by using a combination of <xref:Microsoft.Maui.Controls.StackLayout> objects. The following code shows an example of this bad practice:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Details.HomePage"
             Padding="0,20,0,0">
    <StackLayout>
        <StackLayout Orientation="Horizontal">
            <Label Text="Name:" />
            <Entry Placeholder="Enter your name" />
        </StackLayout>
        <StackLayout Orientation="Horizontal">
            <Label Text="Age:" />
            <Entry Placeholder="Enter your age" />
        </StackLayout>
        <StackLayout Orientation="Horizontal">
            <Label Text="Occupation:" />
            <Entry Placeholder="Enter your occupation" />
        </StackLayout>
        <StackLayout Orientation="Horizontal">
            <Label Text="Address:" />
            <Entry Placeholder="Enter your address" />
        </StackLayout>
    </StackLayout>
</ContentPage>
```

This is wasteful because unnecessary layout calculations are performed. Instead, the desired layout can be better achieved by using a <xref:Microsoft.Maui.Controls.Grid>.

For more information, see [StackLayout](stacklayout.md).

## HorizontalStackLayout

A <xref:Microsoft.Maui.Controls.HorizontalStackLayout> organizes child views in a one-dimensional horizontal stack, and is a more performant alternative to a <xref:Microsoft.Maui.Controls.StackLayout>. <xref:Microsoft.Maui.Controls.HorizontalStackLayout> is typically used to arrange a subsection of the UI on a page.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.HorizontalStackLayout> containing different child views:

```xaml
<HorizontalStackLayout Margin="20">
   <Rectangle Fill="Red"
              HeightRequest="30"
              WidthRequest="30" />
   <Label Text="Red"
          FontSize="18" />
</HorizontalStackLayout>
```

In a <xref:Microsoft.Maui.Controls.HorizontalStackLayout>, if an element's size is not explicitly set, it expands to fill the available height.

For more information, see [HorizontalStackLayout](horizontalstacklayout.md).

## VerticalStackLayout

A <xref:Microsoft.Maui.Controls.VerticalStackLayout> organizes child views in a one-dimensional vertical stack, and is a more performant alternative to a <xref:Microsoft.Maui.Controls.StackLayout>. <xref:Microsoft.Maui.Controls.VerticalStackLayout> is typically used to arrange a subsection of the UI on a page.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.VerticalStackLayout> containing three <xref:Microsoft.Maui.Controls.Label> objects:

```xaml
<VerticalStackLayout Margin="20,35,20,25">
    <Label Text="The VericalStackLayout has its Margin property set, to control the rendering position of the VerticalStackLayout." />
    <Label Text="The Padding property can be set to specify the distance between the VerticalStackLayout and its children." />
    <Label Text="The Spacing property can be set to specify the distance between views in the VerticalStackLayout." />
</VerticalStackLayout>
```

In a <xref:Microsoft.Maui.Controls.VerticalStackLayout>, if an element's size is not explicitly set, it expands to fill the available width.

For more information, see [VerticalStackLayout](verticalstacklayout.md).

## Grid

A <xref:Microsoft.Maui.Controls.Grid> is used for displaying elements in rows and columns, which can have proportional or absolute sizes. A grid's rows and columns are specified with the `RowDefinitions` and `ColumnDefinitions` properties.

To position elements in specific <xref:Microsoft.Maui.Controls.Grid> cells, use the `Grid.Column` and `Grid.Row` attached properties. To make elements span across multiple rows and columns, use the `Grid.RowSpan` and `Grid.ColumnSpan` attached properties.

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.Grid> layout should not be confused with tables, and is not intended to present tabular data.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.Grid> with two rows and two columns:

```xaml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="50" />
        <RowDefinition Height="50" />
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition />
    </Grid.ColumnDefinitions>    
    <Label Text="Column 0, Row 0"
           WidthRequest="200" />
    <Label Grid.Column="1"
           Text="Column 1, Row 0" />
    <Label Grid.Row="1"
           Text="Column 0, Row 1" />
    <Label Grid.Column="1"
           Grid.Row="1"
           Text="Column 1, Row 1" />
</Grid>
```

In this example, sizing works as follows:

- Each row has an explicit height of 50 device-independent units.
- The width of the first column is set to `Auto`, and is therefore as wide as required for its children. In this case, it's 200 device-independent units wide to accommodate the width of the first <xref:Microsoft.Maui.Controls.Label>.

Space can be distributed within a column or row by using auto sizing, which lets columns and rows size to fit their content. This is achieved by setting the height of a `RowDefinition`, or the width of a `ColumnDefinition`, to `Auto`. Proportional sizing can also be used to distribute available space among the rows and columns of the grid by weighted proportions. This is achieved by setting the height of a `RowDefinition`, or the width of a `ColumnDefinition`, to a value that uses the `*` operator.

> [!CAUTION]
> Try to ensure that as few rows and columns as possible are set to `Auto` size. Each auto-sized row or column will cause the layout engine to perform additional layout calculations. Instead, use fixed size rows and columns if possible. Alternatively, set rows and columns to occupy a proportional amount of space with the `GridUnitType.Star` enumeration value.

For more information, see [Grid](grid.md).

## FlexLayout

A <xref:Microsoft.Maui.Controls.FlexLayout> is similar to a <xref:Microsoft.Maui.Controls.StackLayout> in that it displays child elements either horizontally or vertically in a stack. However, a <xref:Microsoft.Maui.Controls.FlexLayout> can also wrap its children if there are too many to fit in a single row or column, and also enables more granular control of the size, orientation, and alignment of its child elements.

The following XAML shows how to create a <xref:Microsoft.Maui.Controls.FlexLayout> that displays its views in a single column:

```xaml
<FlexLayout Direction="Column"
            AlignItems="Center"
            JustifyContent="SpaceEvenly">
    <Label Text="FlexLayout in Action" />
    <Button Text="Button" />
    <Label Text="Another Label" />
</FlexLayout>
```

In this example, layout works as follows:

- The `Direction` property is set to `Column`, which causes the children of the <xref:Microsoft.Maui.Controls.FlexLayout> to be arranged in a single column of items.
- The `AlignItems` property is set to `Center`, which causes each item to be horizontally centered.
- The `JustifyContent` property is set to `SpaceEvenly`, which allocates all leftover vertical space equally between all the items, and above the first item, and below the last item.

For more information, see [FlexLayout](flexlayout.md).

## AbsoluteLayout

An <xref:Microsoft.Maui.Controls.AbsoluteLayout> is used to position and size elements using explicit values, or values relative to the size of the layout. The position is specified by the upper-left corner of the child relative to the upper-left corner of the <xref:Microsoft.Maui.Controls.AbsoluteLayout>.

An <xref:Microsoft.Maui.Controls.AbsoluteLayout> should be regarded as a special-purpose layout to be used only when you can impose a size on children, or when the element's size doesn't affect the positioning of other children. A standard use of this layout is to create an overlay, which covers the page with other controls, perhaps to protect the user from interacting with the normal controls on the page.

> [!IMPORTANT]
> The `HorizontalOptions` and `VerticalOptions` properties have no effect on children of an <xref:Microsoft.Maui.Controls.AbsoluteLayout>.

Within an <xref:Microsoft.Maui.Controls.AbsoluteLayout>, the `AbsoluteLayout.LayoutBounds` attached property is used to specify the horizontal position, vertical position, width and height of an element. In addition, the `AbsoluteLayout.LayoutFlags` attached property specifies how the layout bounds will be interpreted.

The following XAML shows how to arrange elements in an <xref:Microsoft.Maui.Controls.AbsoluteLayout>:

```xaml
<AbsoluteLayout Margin="40">
    <BoxView Color="Red"
             AbsoluteLayout.LayoutFlags="PositionProportional"
             AbsoluteLayout.LayoutBounds="0.5, 0, 100, 100"
             Rotation="30" />
    <BoxView Color="Green"
             AbsoluteLayout.LayoutFlags="PositionProportional"
             AbsoluteLayout.LayoutBounds="0.5, 0, 100, 100"
             Rotation="60" />
    <BoxView Color="Blue"
             AbsoluteLayout.LayoutFlags="PositionProportional"
             AbsoluteLayout.LayoutBounds="0.5, 0, 100, 100" />
</AbsoluteLayout>
```

In this example, layout works as follows:

- Each <xref:Microsoft.Maui.Controls.BoxView> is given an explicit size of 100x100, and is displayed in the same position, horizontally centered.
- The red <xref:Microsoft.Maui.Controls.BoxView> is rotated 30 degrees, and the green <xref:Microsoft.Maui.Controls.BoxView> is rotated 60 degrees.
- On each <xref:Microsoft.Maui.Controls.BoxView>, the `AbsoluteLayout.LayoutFlags` attached property is set to `PositionProportional`, indicating that the position is proportional to the remaining space after width and height are accounted for.

> [!CAUTION]
> Avoid using the `AbsoluteLayout.AutoSize` property whenever possible, as it will cause the layout engine to perform additional layout calculations.

For more information, see [AbsoluteLayout](absolutelayout.md).
<!--
## RelativeLayout

A `RelativeLayout` is used to position and size elements relative to properties of the layout or sibling elements. By default, an element is positioned in the upper left corner of the layout. A `RelativeLayout` can be used to create UIs that scale proportionally across device sizes.

Within a `RelativeLayout`, positions and sizes are specified as constraints. Constraints have `Factor` and `Constant` properties, which can be used to define positions and sizes as multiples (or fractions) of properties of other objects, plus a constant. In addition, constants can be negative.

> [!NOTE]
> A `RelativeLayout` supports positioning elements outside of its own bounds.

The following XAML shows how to arrange elements in a `RelativeLayout`:

```xaml
<RelativeLayout>
    <BoxView Color="Blue"
             HeightRequest="50"
             WidthRequest="50"
             RelativeLayout.XConstraint="{ConstraintExpression Type=RelativeToParent, Property=Width, Factor=0}"
             RelativeLayout.YConstraint="{ConstraintExpression Type=RelativeToParent, Property=Height, Factor=0}" />
    <BoxView Color="Red"
             HeightRequest="50"
             WidthRequest="50"
             RelativeLayout.XConstraint="{ConstraintExpression Type=RelativeToParent, Property=Width, Factor=.85}"
             RelativeLayout.YConstraint="{ConstraintExpression Type=RelativeToParent, Property=Height, Factor=0}" />
    <BoxView x:Name="pole"
             Color="Gray"
             WidthRequest="15"
             RelativeLayout.HeightConstraint="{ConstraintExpression Type=RelativeToParent, Property=Height, Factor=.75}"
             RelativeLayout.XConstraint="{ConstraintExpression Type=RelativeToParent, Property=Width, Factor=.45}"
             RelativeLayout.YConstraint="{ConstraintExpression Type=RelativeToParent, Property=Height, Factor=.25}" />
    <BoxView Color="Green"
             RelativeLayout.HeightConstraint="{ConstraintExpression Type=RelativeToParent, Property=Height, Factor=.10, Constant=10}"
             RelativeLayout.WidthConstraint="{ConstraintExpression Type=RelativeToParent, Property=Width, Factor=.2, Constant=20}"
             RelativeLayout.XConstraint="{ConstraintExpression Type=RelativeToView, ElementName=pole, Property=X, Constant=15}"
             RelativeLayout.YConstraint="{ConstraintExpression Type=RelativeToView, ElementName=pole, Property=Y, Constant=0}" />
</RelativeLayout>
```

In this example, layout works as follows:

- The blue <xref:Microsoft.Maui.Controls.BoxView> is given an explicit size of 50x50 device-independent units. It's placed in the upper left corner of the layout, which is the default position.
- The red <xref:Microsoft.Maui.Controls.BoxView> is given an explicit size of 50x50 device-independent units. It's placed in the upper right corner of the layout.
- The gray <xref:Microsoft.Maui.Controls.BoxView> is given an explicit width of 15 device-independent units, and it's height is set to be 75% of the height of its parent.
- The green <xref:Microsoft.Maui.Controls.BoxView> isn't given an explicit size. Its position is set relative to the <xref:Microsoft.Maui.Controls.BoxView> named `pole`.

> [!WARNING]
> Avoid using a `RelativeLayout` whenever possible. It will result in the CPU having to perform significantly more work.

For more information, see [RelativeLayout](relativelayout.md). -->

## BindableLayout

A <xref:Microsoft.Maui.Controls.BindableLayout> enables any layout class that derives from the <xref:Microsoft.Maui.Controls.Layout> class to generate its content by binding to a collection of items, with the option to set the appearance of each item with a <xref:Microsoft.Maui.Controls.DataTemplate>.

A bindable layout is populated with data by setting its `ItemsSource` property to any collection that implements `IEnumerable`, and attaching it to a <xref:Microsoft.Maui.Controls.Layout>-derived class. The appearance of each item in the bindable layout can be defined by setting the `BindableLayout.ItemTemplate` attached property to a <xref:Microsoft.Maui.Controls.DataTemplate>.

The following XAML shows how to bind a <xref:Microsoft.Maui.Controls.StackLayout> to a collection of items, and define their appearance with a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<StackLayout BindableLayout.ItemsSource="{Binding User.TopFollowers}"
             Orientation="Horizontal">
    <BindableLayout.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding}"
                   Aspect="AspectFill"
                   WidthRequest="44"
                   HeightRequest="44" />
        </DataTemplate>
    </BindableLayout.ItemTemplate>
</StackLayout>
```

Bindable layouts should only be used when the collection of items to be displayed is small, and scrolling and selection isn't required.

For more information, see [BindableLayout](bindablelayout.md).

## Custom layouts

In .NET MAUI, the layout classes derive from the abstract <xref:Microsoft.Maui.Controls.Layout> class. This class delegates cross-platform layout and measurement to a layout manager class. Each layout manager class implements the <xref:Microsoft.Maui.Layouts.ILayoutManager> interface, which specifies that <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> and <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementations must be provided:

- The <xref:Microsoft.Maui.Layouts.ILayoutManager.Measure%2A> implementation calls <xref:Microsoft.Maui.IView.Measure%2A?displayProperty=nameWithType> on each view in the layout, and returns the total size of the layout given the constraints.
- The <xref:Microsoft.Maui.Layouts.ILayoutManager.ArrangeChildren%2A> implementation determines where each view should be placed within the bounds of the layout, and calls <xref:Microsoft.Maui.IView.Arrange%2A> on each view with its appropriate bounds. The return value is the actual size of the layout.

.NET MAUI's layouts have pre-defined layout managers to handle their layout. However, sometimes it's necessary to organize page content using a layout that isn't provided by .NET MAUI. This can be achieved by writing your own custom layout. For more information, see [Custom layouts](custom.md).

## Input transparency

Each visual element has an `InputTransparent` bindable property that's used to define whether the element can receive input. Its default value is `false`, ensuring that the element can receive input. When this property is `true` on an element, the element won't receive any input. Instead, input will be passed to any elements that are visually behind the element.

The <xref:Microsoft.Maui.Controls.Layout> class, from which all layouts derive, has a `CascadeInputTransparent` bindable property that controls whether child elements inherit the input transparency of the layout. Its default value is `true`, ensuring that setting the `InputTransparent` property to `true` on a layout class will result in all elements within the layout not receiving any input.
