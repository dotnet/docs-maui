---
title: "Controls"
description: "The user interface of a .NET MAUI app is constructed from pages, layouts, and views."
ms.date: 02/11/2022
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
| `Grid` | `Grid` positions its child elements in a grid of rows and columns. For more information, see [Grid](~/user-interface/layouts/grid.md). |
| `HorizontalStackLayout` | `HorizontalStackLayout` positions child elements in a horizontal stack. For more information, see [HorizontalStackLayout](~/user-interface/layouts/horizontalstacklayout.md). |
| `StackLayout` | `StackLayout` positions child elements in either a vertical or horizontal stack. For more information, see [StackLayout](~/user-interface/layouts/stacklayout.md). |
| `VerticalStackLayout` | `VerticalStackLayout` positions child elements in a vertical stack. For more information, see [VerticalStackLayout](~/user-interface/layouts/verticalstacklayout.md). |

## Views

.NET MAUI views are the UI objects such as labels, buttons, and sliders that are commonly known as *controls* or *widgets* in other environments.

.NET MAUI contains the following views:

| View | Description |
| --- | --- |
| `ActivityIndicator` | `ActivityIndicator` uses an animation to show that the app is engaged in a lengthy activity, without giving any indication of progress. For more information, see [ActivityIndicator](~/user-interface/controls/activityindicator.md). |
| `BlazorWebView` | `BlazorWebView` enables you to host a Blazor web app in your .NET MAUI app. For more information, see [BlazorWebView](~/user-interface/controls/blazorwebview.md). |
| `Border` | `Border` is a container control that draws a border, background, or both, around another control. For more information, see [Border](~/user-interface/controls/border.md). |
| `BoxView` | `BoxView` draws a rectangle or square, of a specified width, height, and color. For more information, see [BoxView](~/user-interface/controls/boxview.md). |
| `Button` | `Button` displays text and responds to a tap or click that directs an app to carry out a task. For more information, see [Button](~/user-interface/controls/button.md). |
| `CheckBox` | `CheckBox` enables you to select a boolean value using a type of button that can either be checked or empty. For more information, see [CheckBox](~/user-interface/controls/checkbox.md). |
| `ContentView` | `ContentView` is a control that enables the creation of custom, reusable controls. For more information, see [ContentView](~/user-interface/controls/contentview.md). |
| `DatePicker` | `DatePicker` enables you to select a date with the platform date picker. For more information, see [DatePicker](~/user-interface/controls/datepicker.md). |
| `Frame` | `Frame` is used to wrap a view or layout with a border that can be configured with color, shadow, and other options. For more information, see [Frame](~/user-interface/controls/frame.md). |
| `GraphicsView` | `GraphicsView` is a graphics canvas on which 2D graphics can be drawn using types from the `Microsoft.Maui.Graphics` namespace. For more information, see [GraphicsView](~/user-interface/controls/graphicsview.md). |
| `ImageButton` | `ImageButton` displays an image and responds to a tap or click that direct an app to carry out a task. For more information, see [ImageButton](~/user-interface/controls/imagebutton.md). |
| `IndicatorView` | `IndicatorView` displays indicators that represent the number of items in a `CarouselView`. For more information, see [IndicatorView](~/user-interface/controls/indicatorview.md). |
| `Picker` | `Picker` displays a short list of items, from which an item can be selected. For more information, see [Picker](~/user-interface/controls/picker.md). |
| `ProgressBar` | `ProgressBar` uses an animation to show that the app is progressing through a lengthy activity. For more information, see [ProgressBar](~/user-interface/controls/progressbar.md). |
| `RadioButton` | `RadioButton` is a type of button that allows the selection of one option from a set. For more information, see [RadioButton](~/user-interface/controls/radiobutton.md). |
| `SearchBar` | `SearchBar` is a user input control used to initiate a search. For more information, see [SearchBar](~/user-interface/controls/searchbar.md). |
| `Slider` | `Slider` enables you to select a `double` value from a continuous range. For more information, see [Slider](~/user-interface/controls/slider.md). |
| `Stepper` | `Stepper` enables you to select a `double` value from a range of incremental values. For more information, see [Stepper](~/user-interface/controls/stepper.md). |
| `SwipeView` | `SwipeView` is a container control that wraps around an item of content, and provides context menu items that are revealed by a swipe gesture. For more information, see [SwipeView](~/user-interface/controls/swipeview.md). |
| `Switch` | `Switch` enables you to select a boolean value using a type of button that can either be on or off. For more information, see [Switch](~/user-interface/controls/switch.md). |
| `TimePicker` | `TimePicker` enables you to select a time with the platform time picker. For more information, see [TimePicker](~/user-interface/controls/timepicker.md). |
