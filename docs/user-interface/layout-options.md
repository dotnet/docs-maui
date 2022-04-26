---
title: "Align .NET MAUI views in layouts"
description: "Learn how to align .NET MAUI views in a layout, with the HorizontalOptions and VerticalOptions properties."
ms.date: 04/26/2022
---

# Align .NET MAUI views in layouts

Every .NET Multi-platform App UI (.NET MAUI) class that derives from `View`, which includes views and layouts, has `HorizontalOptions` and `VerticalOptions` properties, of type `LayoutOptions`. The `LayoutOptions` structure encapsulates a view's preferred alignment, which determines its position and size within its parent layout when the parent layout contains unused space (that is, the parent layout is larger than the combined size of all its children).

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The alignment of a `View`, relative to its parent, can be controlled by setting the `HorizontalOptions` or `VerticalOptions` property of the `View` to one of the public fields from the `LayoutOptions` structure. The public fields are as `Start`, `Center`, `End`, and `Fill`.

The `Start`, `Center`, `End`, and `Fill` fields are used to define the view's alignment within the parent layout:

- For horizontal alignment, `Start` positions the `View` on the left hand side of the parent layout, and for vertical alignment, it positions the `View` at the top of the parent layout.
- For horizontal and vertical alignment, `Center` horizontally or vertically centers the `View`.
- For horizontal alignment, `End` positions the `View` on the right hand side of the parent layout, and for vertical alignment, it positions the `View` at the bottom of the parent layout.
- For horizontal alignment, `Fill` ensures that the `View` fills the width of the parent layout, and for vertical alignment, it ensures that the `View` fills the height of the parent layout.

> [!NOTE]
> The default value of a view's `HorizontalOptions` and `VerticalOptions` properties is `LayoutOptions.Fill`.

A `StackLayout` only respects the `Start`, `Center`, `End`, and `Fill` `LayoutOptions` fields on child views that are in the opposite direction to the `StackLayout` orientation. Therefore, child views within a vertically oriented `StackLayout` can set their `HorizontalOptions` properties to one of the `Start`, `Center`, `End`, or `Fill` fields. Similarly, child views within a horizontally oriented `StackLayout` can set their `VerticalOptions` properties to one of the `Start`, `Center`, `End`, or `Fill` fields.

A `StackLayout` does not respect the `Start`, `Center`, `End`, and `Fill` `LayoutOptions` fields on child views that are in the same direction as the `StackLayout` orientation. Therefore, a vertically oriented `StackLayout` ignores the `Start`, `Center`, `End`, or `Fill` fields if they are set on the `VerticalOptions` properties of child views. Similarly, a horizontally oriented `StackLayout` ignores the `Start`, `Center`, `End`, or `Fill` fields if they are set on the `HorizontalOptions` properties of child views.

> [!IMPORTANT]
> `LayoutOptions.Fill` generally overrides size requests specified using the  `HeightRequest` and `WidthRequest` properties.

The following XAML example demonstrates a vertically oriented `StackLayout` where each child `Label` sets its `HorizontalOptions` property to one of the four alignment fields from the `LayoutOptions` structure:

```xaml
<StackLayout>
  ...
  <Label Text="Start" BackgroundColor="Gray" HorizontalOptions="Start" />
  <Label Text="Center" BackgroundColor="Gray" HorizontalOptions="Center" />
  <Label Text="End" BackgroundColor="Gray" HorizontalOptions="End" />
  <Label Text="Fill" BackgroundColor="Gray" HorizontalOptions="Fill" />
</StackLayout>
```

The following screenshot shows the resulting alignment of each `Label`:

:::image type="content" source="media/layout-options/alignment.png" alt-text="Screenshot of alignment layout options.":::
