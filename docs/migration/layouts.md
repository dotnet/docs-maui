---
title: "Layout behavior changes from Xamarin.Forms"
description: "Learn about the layout behavior changes between Xamarin.Forms and .NET MAUI."
ms.date: 02/15/2023
---

# Layout behavior changes from Xamarin.Forms

You may notice when running your upgraded .NET Multi-platform App UI (.NET MAUI) app that layout behavior is different. Some of this is the result of changes to layout spacing values. For more information, see [Default value changes from Xamarin.Forms](#default-layout-value-changes-from-xamarinforms).

The following table shows additional behavior changes between layouts in Xamarin.Forms and .NET MAUI:

| Layout  | Xamarin.Forms | .NET MAUI  | Recommendation |
|---|---|---|---|
| All | In certain cases sizing requests aren't honored. | Sizing requests are honored. | |
| <xref:Microsoft.Maui.Controls.Grid> | Columns and rows can be inferred from XAML. | Columns and rows must be explicitly declared. | Add <xref:Microsoft.Maui.Controls.Grid.ColumnDefinitions> and <xref:Microsoft.Maui.Controls.Grid.RowDefinitions>. |
| <xref:Microsoft.Maui.Controls.HorizontalStackLayout> |   | `*AndExpand` has no effect. | |
| <xref:Microsoft.Maui.Controls.Compatibility.RelativeLayout>  | | Requires the compatibility namespace. | Use <xref:Microsoft.Maui.Controls.Grid> instead, or add the `xmlns` for the compatibility namespace. |
| <xref:Microsoft.Maui.Controls.StackLayout> | Children can fill space in the stacking direction. | Children are stacked and will go beyond available space. | If you need child views to fill space, change to a <xref:Microsoft.Maui.Controls.Grid>. |
| <xref:Microsoft.Maui.Controls.VerticalStackLayout> |  | `*AndExpand` has no effect. | |

.NET MAUI controls generally honour explicit size requests. If you ask a control to be 200 device-independent units wide, then .NET MAUI will make that control 200 units wide, even if the control's container is only 100 units wide.

## Default layout value changes from Xamarin.Forms

Xamarin.Forms uses arbitrary default values for some property values, such as padding, margins, and spacing. .NET MAUI changes these arbitrary property values to zero.

To preserve the Xamarin.Forms default values in projects that don't set explicit values, add implicit styles to your project. For more information about implicit styles, see [Implicit styles](~/user-interface/styles/xaml.md#implicit-styles).

> [!NOTE]
> The .NET MAUI project template includes resource dictionaries that provide default styles for most controls. It's recommended you take a similar approach in your apps, by modifying or inheriting from these [resource dictionaries](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles).

The following table lists the layout property values that have changed between Xamarin.Forms and .NET MAUI:

| Property | Xamarin.Forms value | .NET MAUI value |
| --- | --- | --- |
| `Grid.ColumnSpacing` | 6 | 0 |
| `Grid.RowSpacing` | 6 | 0 |
| `StackLayout.Spacing` | 6 | 0 |

The following styles retain the Xamarin.Forms defaults:

```xml
<!-- Forms defaults -->
<Style TargetType="Grid">
    <Setter Property="ColumnSpacing" Value="6"/>
    <Setter Property="RowSpacing" Value="6"/>
</Style>
<Style TargetType="StackLayout">
    <Setter Property="Spacing" Value="6"/>
</Style>
<Style TargetType="Frame">
    <Setter Property="Padding" Value="{OnPlatform 20,iOS=19}"/>
</Style>
```

## Frame

<xref:Microsoft.Maui.Controls.Frame> has been replaced in .NET MAUI by <xref:Microsoft.Maui.Controls.Border>. However, it's included to ease migration from Xamarin.Forms. .NET MAUI layout correctly measures <xref:Microsoft.Maui.Controls.Frame> `Padding` across all platforms, whereas Xamarin.Forms had some discrepancies across platforms. This may result in your apps not looking the same in .NET MAUI. The example above accounts for this if you're using default values.

## Grid

The biggest change in <xref:Microsoft.Maui.Controls.Grid> behavior between Xamarin.Forms and .NET MAUI is that grids don't automatically add missing rows and columns for you. For example, in Xamarin.Forms you could add controls to a <xref:Microsoft.Maui.Controls.Grid> without specifying their row behavior:

```xml
<Grid>
    <Label Text="Hello"/>
    <Label Grid.Row="1" Text="World"/>
</Grid>
```

In Xamarin.Forms, despite not declaring that the <xref:Microsoft.Maui.Controls.Grid> contains two rows, a second row would be automatically added for you. .NET MAUI doesn't do this. Instead, you have to explicitly specify how many rows are in the <xref:Microsoft.Maui.Controls.Grid> with the `RowDefinitions` property.

> [!IMPORTANT]
> By default, .NET MAUI creates a <xref:Microsoft.Maui.Controls.Grid> with one column and one row. Therefore, it's not necessary to set the `ColumnDefinitions` and `RowDefinitions` properties if this is your intention.

## StackLayout

There are several differences between the stack layouts in .NET MAUI (<xref:Microsoft.Maui.Controls.StackLayout>, <xref:Microsoft.Maui.Controls.VerticalStackLayout>, and <xref:Microsoft.Maui.Controls.HorizontalStackLayout>) and the <xref:Xamarin.Forms.StackLayout> in Xamarin.Forms.

The main difference is that .NET MAUI stack layouts are very simple. They stack their child views in a single direction until all of them have been stacked. They will keep going until the last child has been stacked, even if that takes them beyond the available space in the stacking direction. Therefore, .NET MAUI stack layouts arrange controls in a particular direction. They do not subdivide a space. This is completely different to the Xamarin.Forms <xref:Xamarin.Forms.StackLayout>, which changes its layout behavior based on circumstances and the presence of any `*AndExpand` layout options, such as `FillAndExpand` or `CenterAndExpand`. The Xamarin.Forms <xref:Xamarin.Forms.StackLayout> sometimes subdivides the space, expanding to or stopping at the edge of its container. In other cases, it expands beyond its container.

The new stack layouts in .NET MAUI, <xref:Microsoft.Maui.Controls.HorizontalStackLayout> and <xref:Microsoft.Maui.Controls.VerticalStackLayout>, do not recognize the `*AndExpand` layout options. If they encounter a child with such layout options, they simply treat it as if the `AndExpand` wasn't there. For example, `FillAndExpand` becomes `Fill`. However, for simplicity of migration from Xamarin.Forms, the .NET MAUI <xref:Microsoft.Maui.Controls.StackLayout> does honor the `*AndExpand` layout options, although they've been marked as obsolete. To avoid warnings about using obsolete members, you should convert your layouts that use `*AndExpand` layout options to the appropriate layout type. The can be achieved as follows:

1. If your layout is anything other than a <xref:Microsoft.Maui.Controls.StackLayout>, remove all uses of `AndExpand`. Just as in Xamarin.Forms, in .NET MAUI the `AndExpand` layout options have no effect on any layout other than <xref:Microsoft.Maui.Controls.StackLayout>.
1. Remove any `AndExpand` properties which are orthogonal to the stacking direction. For example, if you have a <xref:Microsoft.Maui.Controls.StackLayout> with an `Orientation` of `Vertical`, and it has a child with a `HorizontalAligment="CenterAndExpand"` - that layout options has no effect and can be removed.
1. If you have any remaining `AndExpand` properties on a <xref:Microsoft.Maui.Controls.StackLayout>, you should convert that <xref:Microsoft.Maui.Controls.StackLayout> to a <xref:Microsoft.Maui.Controls.Grid>. A <xref:Microsoft.Maui.Controls.Grid> is designed to subdivide a space, and will provide the layout that `AndExpand` provided in Xamarin.Forms. The following example shows a Xamarin.Forms <xref:Xamarin.Forms.StackLayout> that uses an `AndExpand` property:

    ```xaml
    <StackLayout>
        <Label Text="Hello world!"/>
        <Image VerticalOptions="FillAndExpand" Source="dotnetbot.png"/>
    </StackLayout>
    ```

    This can be converted to a <xref:Microsoft.Maui.Controls.Grid> in .NET MAUI:

    ```xaml
    <Grid RowDefinitions="Auto, *">
        <Label Text="Hello world!"/>
        <Image Grid.Row="1" Source="dotnetbot.png"/>
    </Grid>
    ```

    When performing this conversion, anything that was marked `AndExpand` in the <xref:Xamarin.Forms.StackLayout> should go in its own row or column with a size of `*` in the <xref:Microsoft.Maui.Controls.Grid>.

> [!IMPORTANT]
> A <xref:Microsoft.Maui.Controls.StackLayout> continues in its stacking direction until it runs out of content. It does not subdivide its container along that axis. If you want to limit your content to a constrained space in a direction, you should use another layout such as a <xref:Microsoft.Maui.Controls.Grid>.

## RelativeLayout

Use of <xref:Microsoft.Maui.Controls.Compatibility.RelativeLayout> is not recommended in .NET MAUI. Instead, use a <xref:Microsoft.Maui.Controls.Grid> wherever possible.

If you absolutely require a <xref:Microsoft.Maui.Controls.Compatibility.RelativeLayout>, it can be found in the `Microsoft.Maui.Controls.Compatibility` namespace:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:compat="clr-namespace:Microsoft.Maui.Controls.Compatibility;assembly=Microsoft.Maui.Controls"
             x:Class="MyMauiApp.MyPage"
             Title="MyPage">
    <compat:RelativeLayout>
        <!-- Your code goes here -->
    </compat:RelativeLayout>
</ContentPage>
```

## ScrollView

While <xref:Microsoft.Maui.Controls.ScrollView> often isn't considered to be a layout, it can be thought of as a layout as it's used to scroll its child content. In Xamarin.Forms, <xref:Xamarin.Forms.ScrollView> doesn't behave consistently when stacking. It has some arbitrary limits on minimum size that depend partially on its content, and it will sometimes compress to enable other items to fit on the page inside a <xref:Xamarin.Forms.StackLayout> in ways that are inconsistent and sometimes surprising.

In .NET MAUI, the <xref:Microsoft.Maui.Controls.ScrollView> expands to whatever size it wants to be unless otherwise constrained. This means that inside of a <xref:Microsoft.Maui.Controls.VerticalStackLayout>, which can expand infinitely, a <xref:Microsoft.Maui.Controls.ScrollView> will expand to its full content height and doesn't scroll. This behavior can be confusing if you're a Xamarin.Forms user.
