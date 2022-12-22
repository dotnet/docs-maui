---
title: ".NET MAUI handlers"
description: "Learn about .NET MAUI handlers, which map cross-platform controls to performant native controls on each platform."
ms.date: 08/15/2022
---

# Handlers

.NET Multi-platform App UI (.NET MAUI) provides a collection of cross-platform controls that can be used to display data, initiate actions, indicate activity, display collections, pick data, and more. Each control has an interface representation, that abstracts the control. Cross-platform controls that implement these interfaces are known as *virtual views*. *Handlers* map these virtual views to controls on each platform, which are known as *native views*. Handlers are also responsible for instantiating the underlying native view, and mapping the cross-platform control API to the native view API. For example, on iOS a handler maps a .NET MAUI `Button` to an iOS `UIButton`. On Android, the `Button` is mapped to an `AppCompatButton`:

:::image type="content" source="media/overview/button-handler.png" alt-text="Button handler architecture." border="false":::

.NET MAUI handlers are accessed through their control-specific interface, such as `IButton` for a `Button`. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control.

Each handler class exposes the native view for the cross-platform control via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform control implemented by the handler is exposed via its `VirtualView` property.

When creating a cross-platform control, whose implementation is provided on each platform by native views, a handler should be implemented that maps the cross-platform control API to the native view APIs. For more information, see [Create custom controls with handlers](create.md).

Handlers can also be customized to augment the appearance and behavior of existing cross-platform controls beyond the customization that's possible through the control's API. This handler customization modifies the native views for the cross-platform control. Handlers are global, and customizing a handler for a control will result in all controls of the same type being customized in your app. For more information, see [Customize .NET MAUI controls with handlers](customize.md).

## Mappers

A key concept of .NET MAUI handlers is mappers. Each handler typically provides a *property mapper*, and sometimes a *command mapper*, that maps the cross-platform control's API to the native view's API.

A *property mapper* defines what Actions to take when a property change occurs in the cross-platform control. It's a `Dictionary` that maps the cross-platform control's properties to their associated Actions. Each platform handler then provides implementations of the Actions, which manipulate the native view API. This ensures that when a property is set on a cross-platform control, the underlying native view is updated as required.

A *command mapper* defines what Actions to take when the cross-platform control sends commands to native views. They're similar to property mappers, but allow for additional data to be passed. Commands, in this context, doesn't mean `ICommand` implementations. Instead, a command is just an instruction, and optionally its data, that's sent to a native view. The command mapper is a `Dictionary` that maps the cross-platform control's command to their associated Actions. Each handler then provides implementations of the Actions, which manipulate the native view API. This ensures that when a cross-platform control sends a command to its native view, the native view is updated as required. For example, when a `ScrollView` is scrolled, the `ScrollViewHandler` uses a command mapper to invoke an Action that accepts a scroll position argument. The Action then instructs the underlying native view to scroll to that position.

The advantage of using *mappers* to update native views is that native views can be decoupled from cross-platform controls. This removes the need for native views to subscribe to and unsubscribe from cross-platform control events. It also allows for easy customization because mappers can be modified without subclassing.

## Handler lifecycle

All handler-based .NET MAUI controls support two handler lifecycle events:

- `HandlerChanging` is raised when a new handler is about to be created for a cross-platform control, and when an existing handler is about to be removed from a cross-platform control. The `HandlerChangingEventArgs` object that accompanies this event has `NewHandler` and `OldHandler` properties, of type `IElementHandler`. When the `NewHandler` property isn't `null`, the event indicates that a new handler is about to be created for a cross-platform control. When the `OldHandler` property isn't `null`, the event indicates that the existing native control is about be removed from the cross-platform control, and therefore any native events should be unwired and other cleanup performed.
- `HandlerChanged` is raised after the handler for a cross-platform control has been created. This event indicates that the native control that implements the cross-platform control is available, and all the property values set on the cross-platform control have been applied to the native control.

> [!NOTE]
> The `HandlerChanging` event is raised on a cross-platform control before the `HandlerChanged` event.

In addition to these events, each cross-platform control also has an overridable `OnHandlerChanging` method that's invoked when the `HandlerChanging` event is raised, and a `OnHandlerChanged` method that's invoked when the `HandlerChanged` event is raised.

## Handler-based views

The following table lists the types that implement handler-based views in .NET MAUI:

| View | Interface | Handler | Property Mapper | Command Mapper |
| -- | -- | -- | -- | -- |
| <xref:Microsoft.Maui.Controls.ActivityIndicator> | `IActivityIndicator` | `ActivityIndicatorHandler` | `Mapper` | `CommandMapper` |
| <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> | `IBlazorWebView` | `BlazorWebViewHandler` | `BlazorWebViewMapper` | |
| <xref:Microsoft.Maui.Controls.Border> | `IBorderView` | `BorderHandler` | `Mapper` | `CommandMapper` |
| `Button` | `IButton` | `ButtonHandler` | `ImageButtonMapper`. `TextButtonMapper`, `Mapper` | `CommandMapper` |
| <xref:Microsoft.Maui.Controls.CarouselView> | | `CarouselViewHandler` | `Mapper` | |
| `CheckBox` | `ICheckBox` | `CheckBoxHandler` | `Mapper` | `CommandMapper` |
| <xref:Microsoft.Maui.Controls.CollectionView> |  | `CollectionViewHandler` | `Mapper` | |
| <xref:Microsoft.Maui.Controls.ContentView> | `IContentView` | `ContentViewHandler` | `Mapper` | `CommandMapper` |
| `DatePicker` | `IDatePicker` | `DatePickerHandler` | `Mapper` | `CommandMapper` |
| `Editor` | `IEditor` | `EditorHandler` | `Mapper` | `CommandMapper` |
| `Ellipse` | | `ShapeViewHandler` | `Mapper` | `CommandMapper` |
| `Entry` | `IEntry` | `EntryHandler` | `Mapper` | `CommandMapper` |
| `GraphicsView` | `IGraphicsView` | `GraphicsViewHandler` | `Mapper` | `CommandMapper` |
| `Image` | `IImage` | `ImageHandler` | `Mapper` | `CommandMapper` |
| `ImageButton` | `IImageButton` | `ImageButtonHandler` | `ImageMapper`, `Mapper` | |
| `IndicatorView` | `IIndicatorView` | `IndicatorViewHandler` | `Mapper` | `CommandMapper` |
| `Label` | `ILabel` | `LabelHandler` | `Mapper` | `CommandMapper` |
| `Line` | | `LineHandler` | `Mapper` | |
| `Map` | `IMap` | `MapHandler` | `Mapper` | `CommandMapper` |
| `Path` | | `PathHandler` | `Mapper` | |
| `Picker` | `IPicker` | `PickerHandler` | `Mapper` | `CommandMapper` |
| `Polygon` | | `PolygonHandler` | `Mapper` | |
| `Polyline` | | `PolylineHandler` | `Mapper` | |
| `ProgressBar` | `IProgress` | `ProgressBarHandler` | `Mapper` | `CommandMapper` |
| `RadioButton` | `IRadioButton` | `RadioButtonHandler` | `Mapper` | `CommandMapper` |
| `Rectangle` | | `RectangleHandler` | `Mapper` | |
| `RefreshView` | `IRefreshView` | `RefreshViewHandler` | `Mapper` | `CommandMapper` |
| `RoundRectangle` | | `RoundRectangleHandler` | `Mapper` | |
| `ScrollView` | `IScrollView` | `ScrollViewHandler` | `Mapper` | `CommandMapper` |
| `SearchBar` | `ISearchBar` | `SearchBarHandler` | `Mapper` | `CommandMapper` |
| `Slider` | `ISlider` | `SliderHandler` | `Mapper` | `CommandMapper` |
| `Stepper` | `IStepper` | `StepperHandler` | `Mapper` | `CommandMapper` |
| `SwipeView` | `ISwipeView` | `SwipeViewHandler` | `Mapper` | `CommandMapper` |
| `Switch` | `ISwitch` | `SwitchHandler` | `Mapper` | `CommandMapper` |
| `TimePicker` | `ITimePicker` | `TimePickerHandler` | `Mapper` | `CommandMapper` |
| `WebView` | `IWebView` | `WebViewHandler` | `Mapper` | `CommandMapper` |

All handlers are in the `Microsoft.Maui.Handlers` namespace, with the following exceptions:

- `CarouselViewHandler` and `CollectionViewHandler` are in the `Microsoft.Maui.Controls.Handlers.Items` namespace.
- `LineHandler`, `PathHandler`, `PolygonHandler`, `PolylineHandler`, `RectangleHandler`, and `RoundRectangleHandler` are in the `Microsoft.Maui.Controls.Handlers` namespace.
- `MapHandler` is in the `Microsoft.Maui.Maps.Handlers` namespace.

The interfaces listed in the table above are in the `Microsoft.Maui` namespace.

<!-- Remove the text above once there are API docs that can be linked into -->
