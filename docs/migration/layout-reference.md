---
title: "Layout changes from Xamarin.Forms"
description: "Describing the layout changes between Xamarin.Forms and .NET MAUI."
ms.date: 1/20/2023
---

# Layout changes from Xamarin.Forms

You may notice when running your upgraded application for the first time in .NET MAUI that layouts aren't exactly the same. Some of this is the result of changes to [default values][0]. This document describes additional changes and recommendations related to layouts.

| Layout  | Xamarin.Forms  | .NET MAUI  | Recommendation |
|---|---|---|---|
| All | In certain cases sizing requests would not be honored | Sizing requests are honored | |
| Grid | Columns and Rows can be inferred from the markup | Columns and Rows must be explicitly declared | Add `ColumnDefinitions` and `RowDefinitions` |
| HorizontalStackLayout  |   | "AndExpand" has no effect  | |
| RelativeLayout | | Requires the compatibility namespace | Use `Grid` instead, or add the xmlns and update tags (see below) |
| StackLayout  | Children can fill space in the stacking direction  | Children are stacked and will go beyond available space | If you need child views to fill space, change to a `Grid` |
| VerticalStackLayout  |   | "AndExpand" has no effect  | |

## General

Controls generally honors explicit size requests. If you ask for a control to be 200 points wide, then .NET MAUI will oblige you and make that control 200 points wide, even if the container is only 100 points wide.

## Grid

The biggest change in Grid behavior between Xamarin.Forms and MAUI.Controls is that Grids don't automagically add missing rows/columns for you anymore. For example, in Forms you could do this:

```xml
<Grid>
    <Label Text="Hello"/>
    <Label Grid.Row="1" Text="World"/>
</Grid>
```

And even though you didn't declare that there were two rows in the Grid, Forms would guess that's what you wanted and add the second row for you. MAUI.Controls doesn't do that; you have to explicitly say you want `RowDefinitions="Auto,Auto"`. This was changed for performance reasons.

However, MAUI.Controls _does_ still assume the zeroth row/column for you. That is, if you don't declare any `RowDefinitions` or `ColumnDefinitions`, then the default is `RowDefinitions="*"` and `ColumnDefinitions="*"`

## RelativeLayout

We advise using `Grid` instead of `RelativeLayout` whenever possible. To continue with `RelativeLayout`, you'll need to add the compatibility namespace and update your XAML tags if using XAML.

```xml
xmlns:cmp="clr-namespace:Microsoft.Maui.Controls.Compatibility;assembly=Microsoft.Maui.Controls"

<cmp:RelativeLayout></cmp:RelativeLayout>
```

## StackLayout

There are a few differences between the stack layouts in MAUI (StackLayout, VerticalStackLayout, and HorizontalStackLayout) and the StackLayout in Xamarin.Forms. The first is that the MAUI stack layouts are _very_ simple; they stack their child views in a single direction until all of them have been stacked. They will keep going until the last child has been stacked, even if that takes them beyond the available space in the stacking direction. MAUI stack layouts simply arrange controls in a particular direction; they do not subdivide a space.

This is in contrast to the Xamarin.Forms StackLayout, which changes its behavior based on circumstances and the presence of any "AndExpand" layout options (e.g., `FillAndExpand` or `CenterAndExpand`). Sometimes the Forms StackLayout subdivides the space, expanding to (or stopping at) the edge of its container; in other cases, it expands beyond its container.

This brings us to the second main difference: the MAUI VerticalStackLayout and HorizontalStackLayout do not recognize the "AndExpand" layout options. If they see a child with layout options that include "AndExpand", they simply treat it as if the "AndExpand" weren't there - e.g., `FillAndExpand` becomes `Fill`.

For simplicity of migration from Forms to MAUI, the MAUI.Controls StackLayout _does_ honor "AndExpand", at least for the time being. All of the "AndExpand" options have been marked `Obsolete`. If you want to avoid the warning about the obsolete properties, you should convert your layouts which use "AndExpand" options to the appropriate layout type. The suggested process is as follows:

1. If your layout is anything other than a StackLayout, remove all uses of "AndExpand". Just as in Xamarin.Forms, the "AndExpand" options have no effect on any layout other than StackLayout. If your layout wasn't a StackLayout, "AndExpand" was never doing anything.

2. Remove any "AndExpand" properties which are orthogonal to the stacking direction. For example, if you have a StackLayout with an Orientation of `Vertical`, and it has a child with a `HorizontalAligment="CenterAndExpand"` - that "AndExpand" does nothing. You can just remove it.

3. If you have any remaining "AndExpand" properties on a StackLayout, you should convert that StackLayout to a Grid; the Grid is designed to subdivide a space, and will provide the layout that "AndExpand" provided in Xamarin.Forms. For example,

```xml
<StackLayout>
    <Label Text="howdy"/>
    <Image VerticalOptions="FillAndExpand" src="dotnetbot.png"/>
</StackLayout>
```

can be converted to

```xml
<Grid RowDefinitions="Auto, *">
    <Label Text="howdy"/>
    <Image Grid.Row="1" src="dotnetbot.png"/>
</StackLayout>
```

Anything that was marked "AndExpand" should go in its own row or column with a size of "*".

### See also:

* [Default value and name changes][0]

[0]: defaults.md
