---
title: "FlexLayout"
description: "The .NET MAUI FlexLayout is a layout that stacks or wraps a collection of child views."
ms.date: 12/31/2021
---

# FlexLayout

<!-- Sample link goes here -->

:::image type="content" source="media/flexlayout/layouts.png" alt-text=".NET MAUI FlexLayout." border="false":::

The .NET Multi-platform App UI (.NET MAUI) `FlexLayout` is a layout that can arrange its children horizontally and vertically in a stack, and can also wrap its children if there are too many to fit in a single row or column. In addition, `FlexLayout` can control orientation and alignment, and adapt to different screen sizes. `FlexLayout` is based on the Cascading Style Sheets (CSS) [Flexible Box Layout Module](https://www.w3.org/TR/css-flexbox-1/).

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `FlexLayout` class defines the following properties:

- `AlignContent`, of type `FlexAlignContent`, which determines how the layout engine will distribute space between and around children that have been laid out on multiple lines. The default value of this property is `Stretch`. For more information, see [AlignContent](#aligncontent).
- `AlignItems`, of type `FlexAlignItems`, which indicates how the layout engine will distribute space between and around children along the cross axis. The default value of this property is `Stretch`. For more information, see [AlignItems](#alignitems).
- `Direction`, of type `FlexDirection`, which defines the direction and main axis of children. The default value of this property is `Row`. For more information, see [Direction](#direction).
- `JustifyContent`, of type `FlexJustify`, which specifies how space is distributed between and around children along the main axis. The default value of this property is `Start`. For more information, see [JustifyContent](#justifycontent).
- `Position`, of type `FlexPosition`, which determines whether the position of children are relative to each other, or by using fixed values. The default value of this property is `Relative`.
- `Wrap`, of type `FlexWrap`, which controls whether children are laid out in a single line or in multiple lines. The default value of this property is `NoWrap`. For more information, see [Wrao](#wrap).
- `AlignSelf`, of type `FlexAlignSelf`, which is an attached property that indicates how the layout engine will distribute space between and around children for a specific child along the cross axis. The default value of this property is `Auto`. For more information, see [AlignSelf](#alignself).
- `Basis`, of type `FlexBasis`, which is an attached property that defines the initial main axis dimension of the child. The default value of this property is `Auto`. For more information, see [Basis](#basis).
- `Grow`, of type `float`, which is an attached property that specifies the amount of available space the child should use on the main axis. The default value of this property is 0.0. A validation callback ensures that when the property is set, its value is greater than or equal to 0. For more information, see [Grow](#grow).
- `Order`, of type `int`, which is an attached property that determines whether the child should be laid out before or after other children in the container. The default value of this property is 0. For more information, see [Order](#order).
- `Shrink`, of type `float`, which is an attached property that controls how a child should shrink so that all children can fit inside the container. The default value of this property is 1.0. A validation callback ensures that when the property is set, its value is greater than or equal to 0. For more information, see [Shrink](#shrink).

These properties are backed by `BindableProperty` objects, which means that the properties can be targets of data bindings and styled.

> [!IMPORTANT]
> When items in a `FlexLayout` are arranged in a column, the `FlexLayout` has a vertical *main axis* and a horizontal *cross axis*. When items in a `FlexLayout` are arranged in a row, the `FlexLayout` has a horizontal *main axis* and a vertical *cross axis*.

## Orientation and alignment

The `Direction`, `Wrap`, `JustifyContent`, `AlignItems`, `AlignContent`, and `Position` bindable properties can be set on a `FlexLayout` to control orientation and alignment of all children.

### Direction

The `Direction` property, of type `FlexDirection`, defines the direction and main axis of children. The `FlexDirection` enumeration defines the following members:

- `Column`, which indicates that children should be stacked vertically.
- `ColumnReverse` (or "column-reverse" in XAML), which indicates that children should be stacked vertically in reverse order.
- `Row`, which indicates that children should be stacked horizontally. This is the default value of the `Direction` property.
- `RowReverse` (or "row-reverse" in XAML), which indicates that children should be stacked horizontally in reverse order.

When the `Direction` property is set to `Column`, or `ColumnReverse`, the main-axis will be the y-axis and items will be stacked vertically. When the `Direction` property is set to `Row`, or `RowReverse`, the main-axis will be the x-axis and children will be stacked horizontally.

> [!NOTE]
> In XAML, you can specify the value of this property using the enumeration member names in lowercase, uppercase, or mixed case, or you can use the two additional strings shown in parentheses.

### Wrap

The `Wrap` property, of type `FlexWrap`, controls whether children are laid out in a single line or in multiple lines. The `FlexWrap` enumeration defines the following members:

- `NoWrap`, which indicates that children are laid out in a single line. This is the default value of the `Wrap` property.
- `Wrap`, which indicates that items are laid out in multiple lines if needed.
- `Reverse` (or "wrap-reverse" in XAML), which indicates that items are laid out in multiple lines if needed, in reverse order.

When the `Wrap` property is set to `NoWrap` and the main axis is constrained, and the main axis is not wide or tall enough to fit all the children, the `FlexLayout` attempts to make the items smaller. You can control the shrink factor of children with the `Shrink` attached bindable property.

When the `Wrap` property is set to `Wrap` or `WrapReverse`, the `AlignContent` property can be used to specify how the lines should be distributed.

### JustifyContent

The `JustifyContent` property, of type `FlexJustify`, specifies how space is distributed between and around children along the main axis. The `FlexJustify` enumeration defines the following members:

- `Start` (or "flex-start" in XAML), which indicates that children should be aligned at the start. This is the default value of the `JustifyContent` property.
- `Center`, which indicates that children should be aligned around the center.
- `End` (or "flex-end" in XAML), which indicates that children should be aligned at the end.
- `SpaceBetween` (or "space-between" in XAML), which indicates that children should be evenly distributed, with the first child being at the start and the last child being at the end.
- `SpaceAround` (or "space-around" in XAML), which indicates that children should be evenly distributed, with the first and last children having a half-size space.
- `SpaceEvenly`, which indicates that children should be evenly distributed, with all children having equal space around them.

### AlignItems

The `AlignItems` property, of type `FlexAlignItems`, indicates how the layout engine will distribute space between and around children along the cross axis. The `FlexAlignItems` enumeration defines the following members:

- `Stretch`, which indicates that children should be stretched out. This is the default value of the `AlignItems` property.
- `Center`, which indicates that children should be aligned around the center.
- `Start` (or "flex-start" in XAML), which indicates that children should be aligned at the start.
- `End` (or "flex-end" in XAML), which indicates that children should be aligned at the end.

This is one of two properties that indicates how children are aligned on the cross axis. Within each row, children are stretched or aligned on the start, center, or end of each item.

For any individual child, the `AlignItems` setting can be overridden with the `AlignSelf` attached bindable property.

### AlignContent

The `AlignContent` property, of type `FlexAlignContent`, determines how the layout engine will distribute space between and around children that have been laid out on multiple lines. The `FlexAlignContent` enumeration defines the following members:

- `Stretch`, which indicates that children should be stretched out. This is the default value of the `AlignContent` property.
- `Center`, which indicates that children should be aligned around the center.
- `Start` (or "flex-start" in XAML), which indicates that children should be aligned at the start.
- `End` (or "flex-end" in XAML), which indicates that children should be aligned at the end.
- `SpaceBetween` (or "space-between" in XAML), which indicates that children should be evenly distributed, with the first child being at the start and the last child being at the end.
- `SpaceAround` (or "space-around" in XAML), which indicates that children should be evenly distributed, with the first and last children having a half-size space.
- `SpaceEvenly`, which indicates that children should be evenly distributed, with all children having equal space around them.

The `AlignContent` property has no effect when there is only one row or column.

<!-- ### Position

The `Position` property, of type `FlexPosition`, determines whether the position of children are relative to each other, or by using fixed values. The `FlexPosition` enumeration defines the following members:

- `Relative`, which indicates that the position of children will be relative to each other. This is the default value of the `Position` property.
- `Absolute`, which indicates that the position of children will be determined by fixed position values.

When the `Position` property is set to `Absolute`, the `Left`, `Right`, `Top`, and `Bottom` property values will be used to determine the child's fixed position in its container.

-->

## Child alignment and sizing

The `AlignSelf`, `Order`, `Basis`, `Grow`, and `Shrink` attached bindable properties can be set on children of the `FlexLayout` to control child orientation, alignment, and sizing.

### AlignSelf

The `AlignSelf` property, of type `FlexAlignSelf`, indicates how the layout engine will distribute space between and around children for a specific child along the cross axis. The `FlexAlignSelf` enumeration defines the following members:

- `Auto`, which indicates that a child should be aligned according to the alignment value of its parent. This is the default value of the `AlignSelf` property.
- `Stretch`, which indicates that a child should be stretched out.
- `Center`, which indicates that a child should be aligned around the center.
- `Start` (or "flex-start" in XAML), which indicates that a child should be aligned at the start.
- `End` (or "flex-end" in XAML), which indicates that a child should be aligned at the end.

For any individual child of the `FlexLayout`, this property overrides the `AlignItems` property set on the `FlexLayout`. The default setting of `Auto` means to use the `AlignItems` setting.

In XAML, this property is set on a child without any reference to its `FlexLayout` parent:

```xaml
<Label FlexLayout.AlignSelf="Center"
       ... />
```

The equivalent C# code is:

```csharp
Label label = new Label();
FlexLayout.SetAlignSelf(label, FlexAlignSelf.Center);
```

### Order

The `Order` property, of type `int`, enables you to change the order that children of the `FlexLayout` are arranged. The default value of this property is 0.

Usually, children are arranged in the order in which they are added to the `FlexLayout`. However, this order can be overridden by setting this property to a non-zero integer value on one or more children. The `FlexLayout` then arranges its children based on their `Order` property values. Children with the same `Order` property values are arranged in the order in which they are added to the `FlexLayout`.

### Basis

The `Basis` property, of type `FlexBasis`, defines the amount of space that's allocated to a child on the main axis. The value specified by this property is the size along the main axis of the parent `FlexLayout`. Therefore, this property indicates the width of a child when children are arranged in rows, or the height of a child when children are arranged in columns.

The `FlexBasis` type is a structure that enables size to be specified in device-independent units, or as a percentage of the size of the `FlexLayout`. The default value of the `Basis` property is `Auto`, which means that the child's requested width or height is used.

In XAML, you can use a number for a size in device-independent units:

```xaml
<Label FlexLayout.Basis="40"
       ... />
```

The equivalent C# code is:

```csharp
FlexLayout.SetBasis(label, 40);
```

In XAML, a percentage can be specified as follows:

```xaml
<Label FlexLayout.Basis="25%"
       ... />
```

The equivalent C# code is:

```csharp
FlexLayout.SetBasis(label, new FlexBasis(0.25f, true));
```

The first argument to the `FlexBasis` constructor is a fractional `float` value that must be in the range of 0 to 1. The second argument indicates that the size is relative, rather than absolute.

### Grow

The `Grow` property, of type `float`, specifies the amount of available space the child should use on the main axis. The default value of this property is 0.0, and its value must be greater than or equal to 0.

The `Grow` property is used when the `Wrap` property is set to `NoWrap` and a row of children has a total width less than the width of the `FlexLayout`, or a column of children has a shorter height than the `FlexLayout`. The `Grow` property indicates how to apportion the leftover space among the children. If a single child is given a positive `Grow` value, then that child takes up all the remaining space. Alternatively, the remaining space can also be allocated among two or more children.

### Shrink

The `Shrink` property, of type `float`, controls how a child should shrink so that all children can fit inside the container. The default value of this property is 1.0, and its value must be greater than or equal to 0.

The `Shrink` property is used when the `Wrap` property is set to `NoWrap` and the aggregate width of a row of children is greater than the width of the `FlexLayout`, or the aggregate height of a single column of children is greater than the height of the `FlexLayout`. Normally the `FlexLayout` will display these children by constricting their sizes. The `Shrink` property can indicate which children are given priority in being displayed at their full sizes.

> [!TIP]
> The `Grow` and `Shrink` values can both be set to accommodate situations where the aggregate child sizes might sometimes be less than or sometimes greater than the size of the `FlexLayout`.

## Examples

This section shows/demonstrates common uses of `FlexLayout`.

### Stack

A `FlexLayout` can substitute for a `StackLayout`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="FlexLayoutDemos.Views.SimpleStackPage"
             Title="Simple Stack">    
    <FlexLayout Direction="Column"
                AlignItems="Center"
                JustifyContent="SpaceEvenly">        
        <Label Text="FlexLayout in Action"
               FontSize="Large" />
        <Image Source="seatedmonkey.jpg" />
        <Button Text="Do-Nothing Button" />
        <Label Text="Another Label" />
    </FlexLayout>
</ContentPage>
```

In this example, the `Direction` property is set to `Column`, which causes the children of the `FlexLayout` to be arranged in a single column. The `AlignItems` property is set to `Center`, which causes each child to be horizontally centered. The `JustifyContent` property is set to `SpaceEvenly` which allocates all leftover vertical space equally between all the children, above the first child and below the last child:

:::image type="content" source="media/flexlayout/stack.png" alt-text="Vertically oriented .NET MAUI FlexLayout.":::

> [!NOTE]
> The `AlignSelf` attached property can be used to override the `AlignItems` property for a specific child.

### Wrap items

A `FlexLayout` can wrap its children to additional rows or columns:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="FlexLayoutDemos.Views.PhotoWrappingPage"
             Title="Photo Wrapping">
    <Grid>
        <ScrollView>
            <FlexLayout x:Name="flexLayout"
                        Wrap="Wrap"
                        JustifyContent="SpaceAround" />
        </ScrollView>
        ...
    </Grid>
</ContentPage>
```

In this example, the `Direction` property of the `FlexLayout` is not set, so it has the default setting of `Row`, meaning that the children are arranged in rows and the main axis is horizontal. The `Wrap` property is set to to `Wrap`, which causes children to wrap to the next row if there are too many children to fit on a row. The `JustifyContent` property is set to `SpaceAround` which allocates all leftover space on the main axis so that each child is surrounded by the same amount of space:

:::image type="content" source="media/flexlayout/wrap.png" alt-text="Horizontally wrapping .NET MAUI FlexLayout.":::

The code-behind file for this example accesses a collection of photos and adds them to the `FlexLayout`.

In addition, the `FlexLayout` is a child of a `ScrollView`. Therefore, if there are too many rows to fit on the page, then the `ScrollView` has a default `Orientation` property of `Vertical` and allows vertical scrolling.

### Page layout

There is a standard layout in web design called the [_holy grail_](https://en.wikipedia.org/wiki/Holy_grail_(web_design)) because it's a layout format that is very desirable, but often hard to realize with perfection. The layout consists of a header at the top of the page and a footer at the bottom, both extending to the full width of the page. Occupying the center of the page is the main content, but often with a columnar menu to the left of the content and supplementary information (sometimes called an _aside_ area) at the right. [Section 5.4.1 of the CSS Flexible Box Layout specification](https://www.w3.org/TR/css-flexbox-1/#order-accessibility) describes how the holy grail layout can be realized with a flex box.

The **Holy Grail Layout** page of the **[FlexLayoutDemos](/samples/xamarin/xamarin-forms-samples/userinterface-flexlayoutdemos)** sample shows a simple implementation of this layout using one `FlexLayout` nested in another. Because this page is designed for a phone in portrait mode, the areas to the left and right of the content area are only 50 pixels wide:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="FlexLayoutDemos.HolyGrailLayoutPage"
             Title="Holy Grail Layout">

    <FlexLayout Direction="Column">

        <!-- Header -->
        <Label Text="HEADER"
               FontSize="Large"
               BackgroundColor="Aqua"
               HorizontalTextAlignment="Center" />

        <!-- Body -->
        <FlexLayout FlexLayout.Grow="1">

            <!-- Content -->
            <Label Text="CONTENT"
                   FontSize="Large"
                   BackgroundColor="Gray"
                   HorizontalTextAlignment="Center"
                   VerticalTextAlignment="Center"
                   FlexLayout.Grow="1" />

            <!-- Navigation items-->
            <BoxView FlexLayout.Basis="50"
                     FlexLayout.Order="-1"
                     Color="Blue" />

            <!-- Aside items -->
            <BoxView FlexLayout.Basis="50"
                     Color="Green" />

        </FlexLayout>

        <!-- Footer -->
        <Label Text="FOOTER"
               FontSize="Large"
               BackgroundColor="Pink"
               HorizontalTextAlignment="Center" />
    </FlexLayout>
</ContentPage>
```

Here it is running:

[![The Holy Grail Layout Page.](flex-layout-images/HolyGrailLayout.png "The Holy Grail Layout Page."](flex-layout-images/HolyGrailLayout-Large.png#lightbox)]

The navigation and aside areas are rendered with a `BoxView` on the left and right.

The first `FlexLayout` in the XAML file has a vertical main axis and contains three children arranged in a column. These are the header, the body of the page, and the footer. The nested `FlexLayout` has a horizontal main axis with three children arranged in a row.

Three attached bindable properties are demonstrated in this program:

- The `Order` attached bindable property is set on the first `BoxView`. This property is an integer with a default value of 0. You can use this property to change the layout order. Generally developers prefer the content of the page to appear in markup prior to the navigation items and aside items. Setting the `Order` property on the first `BoxView` to a value less than its other siblings causes it to appear as the first item in the row. Similarly, you can ensure that an item appears last by setting the `Order` property to a value greater than its siblings.

- The `Basis` attached bindable property is set on the two `BoxView` items to give them a width of 50 pixels. This property is of type `FlexBasis`, a structure that defines a static property of type `FlexBasis` named `Auto`, which is the default. You can use `Basis` to  specify a pixel size or a percentage that indicates how much space the item occupies on the main axis. It is called a _basis_ because it specifies an item size that is the basis of all subsequent layout.

- The `Grow` property is set on the nested `Layout` and on the `Label` child representing the content. This property is of type `float` and has a default value of 0. When set to a positive value, all the remaining space along the main axis is allocated to that item and to siblings with positive values of `Grow`. The space is allocated proportionally to the values, somewhat like the star specification in a `Grid`.

    The first `Grow` attached property is set on the nested `FlexLayout`, indicating that this `FlexLayout` is to occupy all the unused vertical space within the outer `FlexLayout`. The second `Grow` attached property is set on the `Label` representing the content, indicating that this content is to occupy all the unused horizontal space within the inner `FlexLayout`.

    There is also a similar `Shrink` attached bindable property that you can use when the size of children exceeds the size of the `FlexLayout` but wrapping is not desired.

### Catalog items with FlexLayout

The **Catalog Items** page in the **[FlexLayoutDemos](/samples/xamarin/xamarin-forms-samples/userinterface-flexlayoutdemos)** sample is similar to [Example 1 in Section 1.1 of the CSS Flex Layout Box specification](https://www.w3.org//TR/css-flexbox-1/#overview) except that it displays a horizontally scrollable series of pictures and descriptions of three monkeys:

[![The Catalog Items Page.](flex-layout-images/CatalogItems.png "The Catalog Items Page."](flex-layout-images/CatalogItems-Large.png#lightbox)]

Each of the three monkeys is a `FlexLayout` contained in a `Frame` that is given an explicit height and width, and which is also a child of a larger `FlexLayout`. In this XAML file, most of the properties of the `FlexLayout` children are specified in styles, all but one of which is an implicit style:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:FlexLayoutDemos"
             x:Class="FlexLayoutDemos.CatalogItemsPage"
             Title="Catalog Items">
    <ContentPage.Resources>
        <Style TargetType="Frame">
            <Setter Property="BackgroundColor" Value="LightYellow" />
            <Setter Property="BorderColor" Value="Blue" />
            <Setter Property="Margin" Value="10" />
            <Setter Property="CornerRadius" Value="15" />
        </Style>

        <Style TargetType="Label">
            <Setter Property="Margin" Value="0, 4" />
        </Style>

        <Style x:Key="headerLabel" TargetType="Label">
            <Setter Property="Margin" Value="0, 8" />
            <Setter Property="FontSize" Value="Large" />
            <Setter Property="TextColor" Value="Blue" />
        </Style>

        <Style TargetType="Image">
            <Setter Property="FlexLayout.Order" Value="-1" />
            <Setter Property="FlexLayout.AlignSelf" Value="Center" />
        </Style>

        <Style TargetType="Button">
            <Setter Property="Text" Value="LEARN MORE" />
            <Setter Property="FontSize" Value="Large" />
            <Setter Property="TextColor" Value="White" />
            <Setter Property="BackgroundColor" Value="Green" />
            <Setter Property="BorderRadius" Value="20" />
        </Style>
    </ContentPage.Resources>

    <ScrollView Orientation="Both">
        <FlexLayout>
            <Frame WidthRequest="300"
                   HeightRequest="480">

                <FlexLayout Direction="Column">
                    <Label Text="Seated Monkey"
                           Style="{StaticResource headerLabel}" />
                    <Label Text="This monkey is laid back and relaxed, and likes to watch the world go by." />
                    <Label Text="  &#x2022; Doesn't make a lot of noise" />
                    <Label Text="  &#x2022; Often smiles mysteriously" />
                    <Label Text="  &#x2022; Sleeps sitting up" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.SeatedMonkey.jpg}"
                           WidthRequest="180"
                           HeightRequest="180" />
                    <Label FlexLayout.Grow="1" />
                    <Button />
                </FlexLayout>
            </Frame>

            <Frame WidthRequest="300"
                   HeightRequest="480">

                <FlexLayout Direction="Column">
                    <Label Text="Banana Monkey"
                           Style="{StaticResource headerLabel}" />
                    <Label Text="Watch this monkey eat a giant banana." />
                    <Label Text="  &#x2022; More fun than a barrel of monkeys" />
                    <Label Text="  &#x2022; Banana not included" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.Banana.jpg}"
                           WidthRequest="240"
                           HeightRequest="180" />
                    <Label FlexLayout.Grow="1" />
                    <Button />
                </FlexLayout>
            </Frame>

            <Frame WidthRequest="300"
                   HeightRequest="480">

                <FlexLayout Direction="Column">
                    <Label Text="Face-Palm Monkey"
                           Style="{StaticResource headerLabel}" />
                    <Label Text="This monkey reacts appropriately to ridiculous assertions and actions." />
                    <Label Text="  &#x2022; Cynical but not unfriendly" />
                    <Label Text="  &#x2022; Seven varieties of grimaces" />
                    <Label Text="  &#x2022; Doesn't laugh at your jokes" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.FacePalm.jpg}"
                           WidthRequest="180"
                           HeightRequest="180" />
                    <Label FlexLayout.Grow="1" />
                    <Button />
                </FlexLayout>
            </Frame>
        </FlexLayout>
    </ScrollView>
</ContentPage>
```

The implicit style for the `Image` includes settings of two attached bindable properties of `Flexlayout`:

```xaml
<Style TargetType="Image">
    <Setter Property="FlexLayout.Order" Value="-1" />
    <Setter Property="FlexLayout.AlignSelf" Value="Center" />
</Style>
```

The `Order` setting of &ndash;1 causes the `Image` element to be displayed first in each of the nested `FlexLayout` views regardless of its position within the children collection. The `AlignSelf` property of `Center` causes the `Image` to be centered within the `FlexLayout`. This overrides the setting of the `AlignItems` property, which has a default value of `Stretch`, meaning that the `Label` and `Button` children are stretched to the full width of the `FlexLayout`.

Within each of the three `FlexLayout` views, a blank `Label` precedes the `Button`, but it has a `Grow` setting of 1. This means that all the extra vertical space is allocated to this blank `Label`, which effectively pushes the `Button` to the bottom.

## CSS styling with FlexLayout

You can use the [CSS styling](~/xamarin-forms/user-interface/styles/css/index.md) feature introduced with .NET MAUI 3.0 in connection with `FlexLayout`. The **CSS Catalog Items** page of the **[FlexLayoutDemos](/samples/xamarin/xamarin-forms-samples/userinterface-flexlayoutdemos)** sample duplicates the layout of the **Catalog Items** page, but with a CSS style sheet for many of the styles:

[![The CSS Catalog Items Page.](flex-layout-images/CssCatalogItems.png "The CSS Catalog Items Page."](flex-layout-images/CssCatalogItems-Large.png#lightbox)]

The original **CatalogItemsPage.xaml** file has five `Style` definitions in its `Resources` section with 15 `Setter` objects. In the **CssCatalogItemsPage.xaml** file, that has been reduced to two `Style` definitions with just four `Setter` objects. These styles supplement the CSS style sheet for properties that the .NET MAUI CSS styling feature currently doesn't support:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:FlexLayoutDemos"
             x:Class="FlexLayoutDemos.CssCatalogItemsPage"
             Title="CSS Catalog Items">
    <ContentPage.Resources>
        <StyleSheet Source="CatalogItemsStyles.css" />

        <Style TargetType="Frame">
            <Setter Property="BorderColor" Value="Blue" />
            <Setter Property="CornerRadius" Value="15" />
        </Style>

        <Style TargetType="Button">
            <Setter Property="Text" Value="LEARN MORE" />
            <Setter Property="BorderRadius" Value="20" />
        </Style>
    </ContentPage.Resources>

    <ScrollView Orientation="Both">
        <FlexLayout>
            <Frame>
                <FlexLayout Direction="Column">
                    <Label Text="Seated Monkey" StyleClass="header" />
                    <Label Text="This monkey is laid back and relaxed, and likes to watch the world go by." />
                    <Label Text="  &#x2022; Doesn't make a lot of noise" />
                    <Label Text="  &#x2022; Often smiles mysteriously" />
                    <Label Text="  &#x2022; Sleeps sitting up" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.SeatedMonkey.jpg}" />
                    <Label StyleClass="empty" />
                    <Button />
                </FlexLayout>
            </Frame>

            <Frame>
                <FlexLayout Direction="Column">
                    <Label Text="Banana Monkey" StyleClass="header" />
                    <Label Text="Watch this monkey eat a giant banana." />
                    <Label Text="  &#x2022; More fun than a barrel of monkeys" />
                    <Label Text="  &#x2022; Banana not included" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.Banana.jpg}" />
                    <Label StyleClass="empty" />
                    <Button />
                </FlexLayout>
            </Frame>

            <Frame>
                <FlexLayout Direction="Column">
                    <Label Text="Face-Palm Monkey" StyleClass="header" />
                    <Label Text="This monkey reacts appropriately to ridiculous assertions and actions." />
                    <Label Text="  &#x2022; Cynical but not unfriendly" />
                    <Label Text="  &#x2022; Seven varieties of grimaces" />
                    <Label Text="  &#x2022; Doesn't laugh at your jokes" />
                    <Image Source="{local:ImageResource FlexLayoutDemos.Images.FacePalm.jpg}" />
                    <Label StyleClass="empty" />
                    <Button />
                </FlexLayout>
            </Frame>
        </FlexLayout>
    </ScrollView>
</ContentPage>
```

The CSS style sheet is referenced in the first line of the `Resources` section:

```xaml
<StyleSheet Source="CatalogItemsStyles.css" />
```

Notice also that two elements in each of the three items include `StyleClass` settings:

```xaml
<Label Text="Seated Monkey" StyleClass="header" />
···
<Label StyleClass="empty" />
```

These refer to selectors in the **CatalogItemsStyles.css** style sheet:

```css
frame {
    width: 300;
    height: 480;
    background-color: lightyellow;
    margin: 10;
}

label {
    margin: 4 0;
}

label.header {
    margin: 8 0;
    font-size: large;
    color: blue;
}

label.empty {
    flex-grow: 1;
}

image {
    height: 180;
    order: -1;
    align-self: center;
}

button {
    font-size: large;
    color: white;
    background-color: green;
}
```

Several `FlexLayout` attached bindable properties are referenced here. In the `label.empty` selector, you'll see the `flex-grow` attribute, which styles an empty `Label` to provide some blank space above the `Button`. The `image` selector contains an `order` attribute and an `align-self` attribute, both of which correspond to `FlexLayout` attached bindable properties.

You've seen that you can set properties directly on the `FlexLayout` and you can set attached bindable properties on the children of a `FlexLayout`. Or, you can set these properties indirectly using traditional XAML-based styles or CSS styles. What's important is to know and understand these properties. These properties are what makes the `FlexLayout` truly flexible.
