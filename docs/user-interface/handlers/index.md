---
title: ".NET MAUI handlers"
description: "Learn about .NET MAUI handlers, which map cross-platform controls to performant native controls on each platform."
ms.date: 01/13/2025
---

# Handlers

.NET Multi-platform App UI (.NET MAUI) provides a collection of cross-platform controls that can be used to display data, initiate actions, indicate activity, display collections, pick data, and more. Each control has an interface representation that abstracts the control. Cross-platform controls that implement these interfaces are known as *virtual views*. *Handlers* map these virtual views to controls on each platform, which are known as *native views*. Handlers are also responsible for instantiating the underlying native view, and mapping the cross-platform control API to the native view API. For example, on iOS a handler maps a .NET MAUI <xref:Microsoft.Maui.Controls.Button> to an iOS `UIButton`. On Android, the <xref:Microsoft.Maui.Controls.Button> is mapped to a `MaterialButton`:

:::image type="content" source="media/overview/button-handler.png" alt-text="Button handler architecture." border="false":::

.NET MAUI handlers are accessed through their control-specific interface, such as `IButton` for a <xref:Microsoft.Maui.Controls.Button>. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control.

Each handler class exposes the native view for the cross-platform control via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform control implemented by the handler is exposed via its `VirtualView` property.

When you create a cross-platform control whose implementation is provided on each platform by native views, you should implement a handler that maps the cross-platform control API to the native view APIs. For more information, see [Create custom controls with handlers](create.md).

You can also customize handlers to augment the appearance and behavior of existing cross-platform controls beyond the customization that's possible through the control's API. This handler customization modifies the native views for the cross-platform control. Handlers are global, and customizing a handler for a control will result in all controls of the same type being customized in your app. For more information, see [Customize .NET MAUI controls with handlers](customize.md).

## Mappers

A key concept of .NET MAUI handlers is mappers. Each handler typically provides a *property mapper*, and sometimes a *command mapper*, that maps the cross-platform control's API to the native view's API.

A *property mapper* defines what Actions to take when a property change occurs in the cross-platform control. It's a `Dictionary` that maps the cross-platform control's properties to their associated Actions. Each platform handler then provides implementations of the Actions, which manipulate the native view API. This ensures that when a property is set on a cross-platform control, the underlying native view is updated as required.

A *command mapper* defines what Actions to take when the cross-platform control sends commands to native views. They're similar to property mappers, but allow for additional data to be passed. A command in this context doesn't mean an <xref:System.Windows.Input.ICommand> implementation. Instead, a command is just an instruction, and optionally its data, that's sent to a native view. The command mapper is a `Dictionary` that maps the cross-platform control's command to their associated Actions. Each handler then provides implementations of the Actions, which manipulate the native view API. This ensures that when a cross-platform control sends a command to its native view, the native view is updated as required. For example, when a <xref:Microsoft.Maui.Controls.ScrollView> is scrolled, the `ScrollViewHandler` uses a command mapper to invoke an Action that accepts a scroll position argument. The Action then instructs the underlying native view to scroll to that position.

The advantage of using *mappers* to update native views is that native views can be decoupled from cross-platform controls. This removes the need for native views to subscribe to and unsubscribe from cross-platform control events. It also allows for easy customization because mappers can be modified without subclassing.

## Handler lifecycle

All handler-based .NET MAUI controls support two handler lifecycle events:

- `HandlerChanging` is raised when a new handler is about to be created for a cross-platform control, and when an existing handler is about to be removed from a cross-platform control. The `HandlerChangingEventArgs` object that accompanies this event has `NewHandler` and `OldHandler` properties, of type `IElementHandler`. When the `NewHandler` property isn't `null`, the event indicates that a new handler is about to be created for a cross-platform control. When the `OldHandler` property isn't `null`, the event indicates that the existing native control is about be removed from the cross-platform control, and therefore any native events should be unwired and other cleanup performed.
- `HandlerChanged` is raised after the handler for a cross-platform control has been created. This event indicates that the native control that implements the cross-platform control is available, and all the property values set on the cross-platform control have been applied to the native control.

> [!NOTE]
> The `HandlerChanging` event is raised on a cross-platform control before the `HandlerChanged` event.

In addition to these events, each cross-platform control also has an overridable `OnHandlerChanging` method that's invoked when the `HandlerChanging` event is raised, and a `OnHandlerChanged` method that's invoked when the `HandlerChanged` event is raised.

## View handlers

The following table lists the types that implement views in .NET MAUI:

| View | Interface | Handler | Property Mapper | Command Mapper |
| -- | -- | -- | -- | -- |
| <xref:Microsoft.Maui.Controls.ActivityIndicator> | <xref:Microsoft.Maui.IActivityIndicator> | <xref:Microsoft.Maui.Handlers.ActivityIndicatorHandler> | <xref:Microsoft.Maui.Handlers.ActivityIndicatorHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ActivityIndicatorHandler.CommandMapper> |
| <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> | <xref:Microsoft.AspNetCore.Components.WebView.Maui.IBlazorWebView> | <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler> | <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler.BlazorWebViewMapper> | |
| <xref:Microsoft.Maui.Controls.Border> | <xref:Microsoft.Maui.IBorderView> | <xref:Microsoft.Maui.Handlers.BorderHandler> | <xref:Microsoft.Maui.Handlers.BorderHandler.Mapper> | <xref:Microsoft.Maui.Handlers.BorderHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.BoxView> | <xref:Microsoft.Maui.IShapeView>, <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Button> | <xref:Microsoft.Maui.IButton> | <xref:Microsoft.Maui.Handlers.ButtonHandler> | <xref:Microsoft.Maui.Handlers.ButtonHandler.ImageButtonMapper>, <xref:Microsoft.Maui.Handlers.ButtonHandler.TextButtonMapper>, <xref:Microsoft.Maui.Handlers.ButtonHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ButtonHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.CarouselView> | | <xref:Microsoft.Maui.Controls.Handlers.Items.CarouselViewHandler> | <xref:Microsoft.Maui.Controls.Handlers.Items.CarouselViewHandler.Mapper> | |
| <xref:Microsoft.Maui.Controls.Cell> | | `CellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.CheckBox> | <xref:Microsoft.Maui.ICheckBox> | <xref:Microsoft.Maui.Handlers.CheckBoxHandler> | <xref:Microsoft.Maui.Handlers.CheckBoxHandler.Mapper> | <xref:Microsoft.Maui.Handlers.CheckBoxHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.CollectionView> |  | <xref:Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler> | <<xref:Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper> | |
| <xref:Microsoft.Maui.Controls.ContentView> | <xref:Microsoft.Maui.IContentView> | <xref:Microsoft.Maui.Handlers.ContentViewHandler> | <xref:Microsoft.Maui.Handlers.ContentViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ContentViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.DatePicker> | <xref:Microsoft.Maui.IDatePicker> | <xref:Microsoft.Maui.Handlers.DatePickerHandler> | <xref:Microsoft.Maui.Handlers.DatePickerHandler.Mapper> | <xref:Microsoft.Maui.Handlers.DatePickerHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Editor> | <xref:Microsoft.Maui.IEditor> | <xref:Microsoft.Maui.Handlers.EditorHandler> | <xref:Microsoft.Maui.Handlers.EditorHandler.Mapper> | <xref:Microsoft.Maui.Handlers.EditorHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Ellipse> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Entry> | <xref:Microsoft.Maui.IEntry> | <xref:Microsoft.Maui.Handlers.EntryHandler> | <xref:Microsoft.Maui.Handlers.EntryHandler.Mapper> | <xref:Microsoft.Maui.Handlers.EntryHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.EntryCell> | | `EntryCellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Frame> | | `FrameRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.GraphicsView> | <xref:Microsoft.Maui.IGraphicsView> | <xref:Microsoft.Maui.Handlers.GraphicsViewHandler> | <xref:Microsoft.Maui.Handlers.GraphicsViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.GraphicsViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Image> | <xref:Microsoft.Maui.IImage> | <xref:Microsoft.Maui.Handlers.ImageHandler> | <xref:Microsoft.Maui.Handlers.ImageHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ImageHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.ImageButton> | <xref:Microsoft.Maui.IImageButton> | <xref:Microsoft.Maui.Handlers.ImageButtonHandler> | <xref:Microsoft.Maui.Handlers.ImageButtonHandler.ImageMapper>, <xref:Microsoft.Maui.Handlers.ImageButtonHandler.Mapper> | |
| <xref:Microsoft.Maui.Controls.ImageCell> | | `ImageCellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.IndicatorView> | <xref:Microsoft.Maui.IIndicatorView> | <xref:Microsoft.Maui.Handlers.IndicatorViewHandler> | <xref:Microsoft.Maui.Handlers.IndicatorViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.IndicatorViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Label> | <xref:Microsoft.Maui.ILabel> | <xref:Microsoft.Maui.Handlers.LabelHandler> | <xref:Microsoft.Maui.Handlers.LabelHandler.Mapper> | <xref:Microsoft.Maui.Handlers.LabelHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Line> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.LineHandler> | <xref:Microsoft.Maui.Controls.Handlers.LineHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.ListView> | | `ListViewRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Maps.Map> | <xref:Microsoft.Maui.Maps.IMap> | <xref:Microsoft.Maui.Maps.Handlers.MapHandler> | <xref:Microsoft.Maui.Maps.Handlers.MapHandler.Mapper> | <xref:Microsoft.Maui.Maps.Handlers.MapHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Path> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.PathHandler> | <xref:Microsoft.Maui.Controls.Handlers.PathHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Picker> | <xref:Microsoft.Maui.IPicker> | <xref:Microsoft.Maui.Handlers.PickerHandler> | <xref:Microsoft.Maui.Handlers.PickerHandler.Mapper> | <xref:Microsoft.Maui.Handlers.PickerHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Polygon> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.PolygonHandler> | <xref:Microsoft.Maui.Controls.Handlers.PolygonHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Polyline> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.PolylineHandler> | <xref:Microsoft.Maui.Controls.Handlers.PolylineHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.ProgressBar> | <xref:Microsoft.Maui.IProgress> | <xref:Microsoft.Maui.Handlers.ProgressBarHandler> | <xref:Microsoft.Maui.Handlers.ProgressBarHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ProgressBarHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.RadioButton> | <xref:Microsoft.Maui.IRadioButton> | <xref:Microsoft.Maui.Handlers.RadioButtonHandler> | <xref:Microsoft.Maui.Handlers.RadioButtonHandler.Mapper> | <xref:Microsoft.Maui.Handlers.RadioButtonHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.Rectangle> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.RectangleHandler> | <xref:Microsoft.Maui.Controls.Handlers.RectangleHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.RefreshView> | <xref:Microsoft.Maui.IRefreshView> | <xref:Microsoft.Maui.Handlers.RefreshViewHandler> | <xref:Microsoft.Maui.Handlers.RefreshViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.RefreshViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shapes.RoundRectangle> | <xref:Microsoft.Maui.Graphics.IShape> | <xref:Microsoft.Maui.Controls.Handlers.RoundRectangleHandler> | <xref:Microsoft.Maui.Controls.Handlers.RoundRectangleHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ShapeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.ScrollView> | <xref:Microsoft.Maui.IScrollView> | <xref:Microsoft.Maui.Handlers.ScrollViewHandler> | <xref:Microsoft.Maui.Handlers.ScrollViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.ScrollViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.SearchBar> | <xref:Microsoft.Maui.ISearchBar> | <xref:Microsoft.Maui.Handlers.SearchBarHandler> | <xref:Microsoft.Maui.Handlers.SearchBarHandler.Mapper> | <xref:Microsoft.Maui.Handlers.SearchBarHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Slider> | <xref:Microsoft.Maui.ISlider> | <xref:Microsoft.Maui.Handlers.SliderHandler> | <xref:Microsoft.Maui.Handlers.SliderHandler.Mapper> | <xref:Microsoft.Maui.Handlers.SliderHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Stepper> | <xref:Microsoft.Maui.IStepper> | <xref:Microsoft.Maui.Handlers.StepperHandler> | <xref:Microsoft.Maui.Handlers.StepperHandler.Mapper> | <xref:Microsoft.Maui.Handlers.StepperHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.SwipeView> | <xref:Microsoft.Maui.ISwipeView> | <xref:Microsoft.Maui.Handlers.SwipeViewHandler> | <xref:Microsoft.Maui.Handlers.SwipeViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.SwipeViewHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Switch> | <xref:Microsoft.Maui.ISwitch> | <xref:Microsoft.Maui.Handlers.SwitchHandler> | <xref:Microsoft.Maui.Handlers.SwitchHandler.Mapper> | <xref:Microsoft.Maui.Handlers.SwitchHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.SwitchCell> | | `SwitchCellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.TableView> | | `TableViewRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.TextCell> | | `TextCellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.TimePicker> | <xref:Microsoft.Maui.ITimePicker> | <xref:Microsoft.Maui.Handlers.TimePickerHandler> | <xref:Microsoft.Maui.Handlers.TimePickerHandler.Mapper> | <xref:Microsoft.Maui.Handlers.TimePickerHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.ViewCell> | | `ViewCellRenderer` | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.WebView> | <xref:Microsoft.Maui.IWebView> | <xref:Microsoft.Maui.Handlers.WebViewHandler> | <xref:Microsoft.Maui.Handlers.WebViewHandler.Mapper> | <xref:Microsoft.Maui.Handlers.WebViewHandler.CommandMapper> |

## Page handlers

The following table lists the types that implement pages in .NET MAUI:

| Page | Android Handler | iOS/Mac Catalyst Handler | Windows Handler | Property Mapper | Command Mapper |
| -- | -- | -- | -- | -- | -- |
| <xref:Microsoft.Maui.Controls.ContentPage> | <xref:Microsoft.Maui.Handlers.PageHandler> | <xref:Microsoft.Maui.Handlers.PageHandler> | <xref:Microsoft.Maui.Handlers.PageHandler> | <xref:Microsoft.Maui.Handlers.PageHandler.Mapper> | <xref:Microsoft.Maui.Handlers.PageHandler.CommandMapper> |
| <xref:Microsoft.Maui.Controls.FlyoutPage> | <xref:Microsoft.Maui.Handlers.FlyoutViewHandler> | PhoneFlyoutPageRenderer | <xref:Microsoft.Maui.Handlers.FlyoutViewHandler> | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.NavigationPage> | <xref:Microsoft.Maui.Handlers.NavigationViewHandler> | NavigationRenderer | <xref:Microsoft.Maui.Handlers.NavigationViewHandler> | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.TabbedPage> | <xref:Microsoft.Maui.Handlers.TabbedViewHandler> | TabbedRenderer | <xref:Microsoft.Maui.Handlers.TabbedViewHandler> | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |
| <xref:Microsoft.Maui.Controls.Shell> | `ShellHandler` | ShellRenderer | ShellRenderer | `Mapper` | <xref:Microsoft.Maui.CommandMapper> |

<!--
xrefs not used on:

1. Mapper and CommandMapper because the properties are in different files (handlers vs compatibility renderers).
1. Renderer classes because they are platform-specific, and the API docs only exist for the xplat layer.
1. No API doc for ShellHandler.

-->
