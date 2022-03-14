---
title: "FlyoutPage"
description: "The .NET MAUI FlyoutPage displays a single view, which is often a layout, and is the most common page type."
ms.date: 03/14/2022
---

# FlyoutPage

:::image type="content" source="media/flyoutpage/pages.png" alt-text=".NET MAUI FlyoutPage." border="false":::

The .NET Multi-platform App UI (.NET MAUI) `FlyoutPage` displays a single view, which is often a layout such as as `Grid` or `StackLayout`, and is the most common page type.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

`FlyoutPage` defines the following properties:

- `Detail`, of type `Page`,
- `Flyout`, of type `Page`,
- `FlyoutLayoutBehavior`, of type `FlyoutLayoutBehavior`,
- `IsGestureEnabled`, of type `bool`, default is `true`.
- `IsPresented`, of type `bool`,

The `IsGestureEnabled`, `IsPresented`, and `FlyoutLayoutBehavior` properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

In addition, `FlyoutPage` inherits `ItemsSource`, `ItemTemplate`, and `SelectedItem` bindable properties from the `MultiPage<T>` class.

Events: `IsPresentedChanged` (no specific args),

`FlyoutLayoutBehavior` defines the following members:

- `Default`,
- `SplitOnLandscape`,
- `Split`
- `PopOver`,
- `SplitOnPortrait`,
