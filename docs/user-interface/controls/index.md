---
title: "Controls"
description: "The user interface of a .NET MAUI app is constructed from pages, layouts, and views."
ms.date: 09/26/2022
---

# Controls

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-controlgallery)

The user interface of a .NET Multi-platform App UI (.NET MAUI) app is constructed of objects that map to the native controls of each target platform.

The main control groups used to create the user interface of a .NET MAUI app are pages, layouts, and views. A .NET MAUI page generally occupies the full screen or window. The page usually contains a layout, which contains views and possibly other layouts. Pages, layouts, and views derive from the `VisualElement` class. This class provides a variety of properties, methods, and events that are useful in derived classes.

> [!NOTE]
> `ListView` and `TableView` also support the use of cells. Cells are specialized elements used for items in a table, that describe how each item should be rendered.

## Pages

.NET MAUI apps consist of one or more pages. A page usually occupies all of the screen, or window, and each page typically contains at least one layout.

.NET MAUI contains the following pages:

| Page | Description |
| --- | --- |
| <xref:Microsoft.Maui.Controls.ContentPage> | <xref:Microsoft.Maui.Controls.ContentPage> displays a single view, and is the most common page type. For more information, see [ContentPage](~/user-interface/pages/contentpage.md). |
| `FlyoutPage` | `FlyoutPage` is a page that manages two related pages of information – a flyout page that presents items, and a detail page that presents details about items on the flyout page. For more information, see [FlyoutPage](~/user-interface/pages/flyoutpage.md). |
| `NavigationPage` | `NavigationPage` provides a hierarchical navigation experience where you're able to navigate through pages, forwards and backwards, as desired. For more information, see [NavigationPage](~/user-interface/pages/navigationpage.md). |
| `TabbedPage` | `TabbedPage` consists of a series of pages that are navigable by tabs across the top or bottom of the page, with each tab loading the page content. For more information, see [TabbedPage](~/user-interface/pages/tabbedpage.md). |

## Layouts

.NET MAUI layouts are used to compose user-interface controls into visual structures, and each layout typically contains multiple views. Layout classes typically contain logic to set the position and size of child elements.

.NET MAUI contains the following layouts:

| Layout | Description |
| --- | --- |
| `AbsoluteLayout` | `AbsoluteLayout` positions child elements at specific locations relative to its parent. For more information, see [AbsoluteLayout](~/user-interface/layouts/absolutelayout.md). |
| `BindableLayout` | `BindableLayout` enables any layout class to generate its content by binding to a collection of items, with the option to set the appearance of each item. For more information, see [BindableLayout](~/user-interface/layouts/bindablelayout.md).
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
| `CarouselView` | `CarouselView` displays a scrollable list of data items, where users swipe to move through the collection. For more information, see [CarouselView](~/user-interface/controls/carouselview/index.md). |
| `CheckBox` | `CheckBox` enables you to select a boolean value using a type of button that can either be checked or empty. For more information, see [CheckBox](~/user-interface/controls/checkbox.md). |
| `CollectionView` | `CollectionView` displays a scrollable list of selectable data items, using different layout specifications. For more information, see [CollectionView](~/user-interface/controls/collectionview/index.md). |
| `ContentView` | `ContentView` is a control that enables the creation of custom, reusable controls. For more information, see [ContentView](~/user-interface/controls/contentview.md). |
| `DatePicker` | `DatePicker` enables you to select a date with the platform date picker. For more information, see [DatePicker](~/user-interface/controls/datepicker.md). |
| `Editor` | `Editor` enables you to enter and edit multiple lines of text. For more information, see [Editor](~/user-interface/controls/editor.md). |
| `Ellipse` | `Ellipse` displays an ellipse or circle. For more information, see [Ellipse](~/user-interface/controls/shapes/ellipse.md). |
| `Entry` | `Entry` enables you to enter and edit a single line of text. For more information, see [Entry](~/user-interface/controls/entry.md). |
| `Frame` | `Frame` is used to wrap a view or layout with a border that can be configured with color, shadow, and other options. For more information, see [Frame](~/user-interface/controls/frame.md). |
| `GraphicsView` | `GraphicsView` is a graphics canvas on which 2D graphics can be drawn using types from the `Microsoft.Maui.Graphics` namespace. For more information, see [GraphicsView](~/user-interface/controls/graphicsview.md). |
| `Image` | `Image` displays an image that can be loaded from a local file, a URI, an embedded resource, or a stream. For more information, see [Image](~/user-interface/controls/image.md). |
| `ImageButton` | `ImageButton` displays an image and responds to a tap or click that direct an app to carry out a task. For more information, see [ImageButton](~/user-interface/controls/imagebutton.md). |
| `IndicatorView` | `IndicatorView` displays indicators that represent the number of items in a `CarouselView`. For more information, see [IndicatorView](~/user-interface/controls/indicatorview.md). |
| `Label` | `Label` displays single-line and multi-line text. For more information, see [Label](~/user-interface/controls/label.md).
| `Line` | `Line` displays a line from a start point to an end point. For more information, see [Line](~/user-interface/controls/shapes/line.md). |
| `ListView` | `ListView` displays a scrollable list of selectable data items. For more information, see [ListView](~/user-interface/controls/listview.md). |
| `Map` | `Map` displays a map, and requires the **Microsoft.Maui.Controls.Maps** NuGet package to be installed in your app. |
| `Path` | `Path` display curves and complex shapes. For more information, see [Path](~/user-interface/controls/shapes/path.md). |
| `Picker` | `Picker` displays a short list of items, from which an item can be selected. For more information, see [Picker](~/user-interface/controls/picker.md). |
| `Polygon` | `Polygon` displays a polygon. For more information, see [Polygon](~/user-interface/controls/shapes/polygon.md). |
| `Polyline` | `Polyline` displays a series of connected straight lines. For more information, see [Polyline](~/user-interface/controls/shapes/polyline.md). |
| `ProgressBar` | `ProgressBar` uses an animation to show that the app is progressing through a lengthy activity. For more information, see [ProgressBar](~/user-interface/controls/progressbar.md). |
| `RadioButton` | `RadioButton` is a type of button that allows the selection of one option from a set. For more information, see [RadioButton](~/user-interface/controls/radiobutton.md). |
| `Rectangle` | `Rectangle` displays a rectangle or square. For more information, see [Rectangle](~/user-interface/controls/shapes/rectangle.md). |
| `RefreshView` | `RefreshView` is a container control that provides pull-to-refresh functionality for scrollable content. For more information, see [RefreshView](~/user-interface/controls/refreshview.md). |
| `RoundRectangle` | `RoundRectangle` displays a rectangle or square with rounded corners. For more information, see [Rectangle](~/user-interface/controls/shapes/rectangle.md). |
| `ScrollView` | `ScrollView` provides scrolling of its content, which is typically a layout. For more information, see [ScrollView](~/user-interface/controls/scrollview.md). |
| `SearchBar` | `SearchBar` is a user input control used to initiate a search. For more information, see [SearchBar](~/user-interface/controls/searchbar.md). |
| `Slider` | `Slider` enables you to select a `double` value from a continuous range. For more information, see [Slider](~/user-interface/controls/slider.md). |
| `Stepper` | `Stepper` enables you to select a `double` value from a range of incremental values. For more information, see [Stepper](~/user-interface/controls/stepper.md). |
| `SwipeView` | `SwipeView` is a container control that wraps around an item of content, and provides context menu items that are revealed by a swipe gesture. For more information, see [SwipeView](~/user-interface/controls/swipeview.md). |
| `Switch` | `Switch` enables you to select a boolean value using a type of button that can either be on or off. For more information, see [Switch](~/user-interface/controls/switch.md). |
| `TableView` | `TableView` displays a table of scrollable items that can be grouped into sections. For more information, see [TableView](~/user-interface/controls/tableview.md). |
| `TimePicker` | `TimePicker` enables you to select a time with the platform time picker. For more information, see [TimePicker](~/user-interface/controls/timepicker.md). |
| `WebView` | `WebView` displays web pages or local HTML content. For more information, see [WebView](~/user-interface/controls/webview.md). |
