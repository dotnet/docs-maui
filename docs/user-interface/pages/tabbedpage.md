---
title: "TabbedPage"
description: "The .NET MAUI TabbedPage displays a single view, which is often a layout, and is the most common page type."
ms.date: 03/14/2022
---

# TabbedPage

:::image type="content" source="media/tabbedpage/pages.png" alt-text=".NET MAUI TabbedPage." border="false":::

The .NET Multi-platform App UI (.NET MAUI) `TabbedPage` displays a single view, which is often a layout such as as `Grid` or `StackLayout`, and is the most common page type.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

`TabbedPage` defines the following properties:

- `BarBackground`, of type `Brush`,
- `BarBackgroundColor`, of type `Color`,
- `BarTextColor`, of type `Color`,
- `SelectedTabColor`, of type `Color`,
- `UnselectedTabColor`, of type `Color`,


These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

In addition, `TabbedPage` inherits `ItemsSource`, `ItemTemplate`, and `SelectedItem` bindable properties from the `MultiPage<T>` class.
