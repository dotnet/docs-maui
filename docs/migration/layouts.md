---
title: "Layout changes from Xamarin.Forms"
description: "Learn about the layout changes between Xamarin.Forms and .NET MAUI."
ms.date: 1/31/2023
---

# Layout changes from Xamarin.Forms

You may notice when running your upgraded app in .NET Multi-platform App UI (.NET MAUI) that layout behavior is different. Some of this is the result of changes to layout spacing values. For more information, see [Default value changes from Xamarin.Forms](#default-layout-value-changes-from-xamarin-forms).

There are additional changes and recommendations for layouts in .NET MAUI:

| Layout  | Xamarin.Forms  | .NET MAUI  | Recommendation |
|---|---|---|---|
| All | In certain cases sizing requests would not be honored. | Sizing requests are honored. | |
| `Grid` | Columns and rows can be inferred from XAML. | Columns and rows must be explicitly declared. | Add `ColumnDefinitions` and `RowDefinitions`. |
| `HorizontalStackLayout` |   | "*AndExpand" has no effect. | |
| `RelativeLayout` | | Requires the compatibility namespace. | Use `Grid` instead, or add the `xmlns` and update tags. |
| `StackLayout` | Children can fill space in the stacking direction. | Children are stacked and will go beyond available space. | If you need child views to fill space, change to a `Grid`. |
| `VerticalStackLayout` |  | "*AndExpand" has no effect. | |

.NET MAUI controls generally honour explicit size requests. If you ask a control to be 200 device-independent units wide, then .NET MAUI will make that control 200 units wide, even if the control's container is only 100 units wide.

## Default layout value changes from Xamarin.Forms

Xamarin.Forms uses arbitrary default values for some property values, such as padding, margins, and spacing. .NET Multi-platform App UI (.NET MAUI) changes these arbitrary property values to zero.

The .NET MAUI project template includes resource dictionaries that provide default styles for most controls. It's recommended you take a similar approach in your apps, by modifying or inheriting from these [resource dictionaries](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles).

To preserve the Xamarin.Forms default values in projects that don't set explicit values, add implicit styles to your project. For more information about implicit styles, see [Implicit styles](~/user-interface/styles/xaml.md#implicit-styles).

The following table lists the property values that have changed betwen Xamarin.Forms and .NET MAUI:

| Property | Xamarin.Forms value | .NET MAUI value |
| --- | --- | --- |
| `Grid.ColumnSpacing` | 6 | 0 |
| `Grid.RowSpacing` | 6 | 0 |
| `StackLayout.Spacing` | 6 | 0 |

## Grid

The biggest change in `Grid` behavior between Xamarin.Forms and .NET MAUI is that grids don't automatically add missing rows and columns for you. For example, in Xamarin.Forms you could add controls to a `Grid` without specifying their row behavior:

```xml
<Grid>
    <Label Text="Hello"/>
    <Label Grid.Row="1" Text="World"/>
</Grid>
```

In Xamarin.Forms, despite not declaring that the `Grid` contains two rows, a second row would be automatically added for you. .NET MAUI doesn't do this. Instead, you have to explicitly specify how many rows are in the `Grid` with the `RowDefinitions` property.

> [!IMPORTANT]
> By default, .NET MAUI creates a `Grid` with one column and one row. Therefore, it's not necessary to set the `ColumnDefinitions` and `RowDefinitions` properties if this is your intention.

## StackLayout

There are several differences between the stack layouts in .NET MAUI (`StackLayout`, `VerticalStackLayout`, and `HorizontalStackLayout`) and the `StackLayout` in Xamarin.Forms.

The main difference is that .NET MAUI stack layouts are very simple. They stack their child views in a single direction until all of them have been stacked. They will keep going until the last child has been stacked, even if that takes them beyond the available space in the stacking direction. Therefore, .NET MAUI stack layouts arrange controls in a particular direction. They do not subdivide a space. This is completely different to the Xamarin.Forms `StackLayout`, which changes its layout behavior based on circumstances and the presence of any "*AndExpand" layout options, such as `FillAndExpand` or `CenterAndExpand`. The Xamarin.Forms `StackLayout` sometimes subdivides the space, expanding to or stopping at the edge of its container. In other cases, it expands beyond its container.

The new stack layouts in .NET MAUI, `HorizontalStackLayout` and `VerticalStackLayout`, do not recognize the "*AndExpand" layout options. If they encounter a child with such layout options, they simply treat it as if the "AndExpand" weren't there. For example, `FillAndExpand` becomes `Fill`. However, for simplicity of migration from Xamarin.Forms, the .NET MAUI `StackLayout` does honor the "*AndExpand" layout options, although they've been marked as obsolete. To avoid warnings about using obsolete members, you should convert your layouts that use "*AndExpand" layout options to the appropriate layout type. The can be achieved as follows:

1. If your layout is anything other than a `StackLayout`, remove all uses of "AndExpand". Just as in Xamarin.Forms, in .NET MAUI the "AndExpand" layout options have no effect on any layout other than `StackLayout`.
1. Remove any "AndExpand" properties which are orthogonal to the stacking direction. For example, if you have a `StackLayout` with an `Orientation` of `Vertical`, and it has a child with a `HorizontalAligment="CenterAndExpand"` - that layout options has no effect and can be removed.
1. If you have any remaining "AndExpand" properties on a `StackLayout`, you should convert that `StackLayout` to a `Grid`. A `Grid` is designed to subdivide a space, and will provide the layout that "AndExpand" provided in Xamarin.Forms. The following example shows a Xamarin.Forms `StackLayout` that uses an "AndExpand" property:

    ```xaml
    <StackLayout>
        <Label Text="Hello world!"/>
        <Image VerticalOptions="FillAndExpand" Source="dotnetbot.png"/>
    </StackLayout>
    ```

    This can be converted to a `Grid` in .NET MAUI:

    ```xaml
    <Grid RowDefinitions="Auto, *">
        <Label Text="Hello world!"/>
        <Image Grid.Row="1" Source="dotnetbot.png"/>
    </StackLayout>
    ```

    When performing this conversion, anything that was marked "AndExpand" in the `StackLayout` should go in its own row or column with a size of `*` in the `Grid`.

## RelativeLayout

Use of `RelativeLayout` is not recommended in .NET MAUI. Instead, use a `Grid` wherever possible.

If you absolutely require `RelativeLayout`, it can be found in the `Microsoft.Maui.Controls.Compatibility` namespace:

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
