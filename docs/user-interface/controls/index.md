---
title: "Controls"
description: "The user interface of a .NET MAUI app is constructed from pages, layouts, and views."
ms.date: 01/14/2022
---

# Controls

The user interface of a .NET Multi-platform App UI (.NET MAUI) app is constructed of objects that map to the native controls of each target platform.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The main control groups used to create the user interface of a .NET MAUI app are pages, layouts, and views. A .NET MAUI page generally occupies the full screen or window. The page usually contains a layout, which contains views and possibly other layouts. Pages, layouts, and views derive from the `VisualElement` class. This class provides a variety of properties, methods, and events that are useful in derived classes.

> [!NOTE]
> `ListView` and `TableView` also support the use of cells. Cells are specialized elements used for items in a table, that describe how each item should be rendered.

## Layouts

.NET MAUI layouts are used to compose user-interface controls into visual structures. Layout classes typically contain logic to set the position and size of child elements.

.NET MAUI contains the following layouts:

| Layout | Description |
| --- | --- |
| `AbsoluteLayout` | `AbsoluteLayout` positions child elements at specific locations relative to its parent. For more information, see [AbsoluteLayout](~/user-interface/layouts/absolutelayout.md). |
| `FlexLayout`| `FlexLayout` enables its children to be stacked or wrapped with different alignment and orientation options. `FlexLayout` is based on the CSS Flexible Box Layout Module, known as *flex layout* or *flex-box*. For more information, see [FlexLayout](~/user-interface/layouts/flexlayout.md). |
| `GridLayout` | `GridLayout` positions its child elements in a grid of rows and columns. For more information, see [GridLayout](~/user-interface/layouts/gridlayout.md). |
| `HorizontalStackLayout` | `HorizontalStackLayout` positions child elements in a horizontal stack. For more information, see [HorizontalStackLayout](~/user-interface/layouts/horizontalstacklayout.md). |
| `StackLayout` | `StackLayout` positions child elements in either a vertical or horizontal stack. For more information, see [StackLayout](~/user-interface/layouts/stacklayout.md). |
| `VerticalStackLayout` | `VerticalStackLayout` positions child elements in a vertical stack. For more information, see [VerticalStackLayout](~/user-interface/layouts/verticalstacklayout.md). |

## Views

.NET MAUI views are the UI objects such as labels, buttons, and sliders that are commonly known as *controls* or *widgets* in other environments.

.NET MAUI contains the following views:

| View | Description |
| --- | --- |
| `BlazorWebView` | `BlazorWebView` enables you to host a Blazor web app in your .NET MAUI app. For more information, see [BlazorWebView](~/user-interface/controls/blazorwebview.md). |
| `Border` | `Border` is a container control that draws a border, background, or both, around another control. For more information, see [Border](~/user-interface/controls/border.md). |
| `ContentView` | `ContentView` is a control that enables the creation of custom, reusable controls. For more information, see [ContentView](~/user-interface/controls/contentview.md). |
| `Frame` | `Frame` is used to wrap a view or layout with a border that can be configured with color, shadow, and other options. For more information, see [Frame](~/user-interface/controls/frame.md). |
| `GraphicsView` | `GraphicsView` is a graphics canvas on which 2D graphics can be drawn using types from the `Microsoft.Maui.Graphics` namespace. For more information, see [GraphicsView](~/user-interface/controls/graphicsview.md). |
| `SearchBar` | `SearchBar` is a user input control used to initiate a search. For more information, see [SearchBar](~/user-interface/controls/searchbar.md). |
