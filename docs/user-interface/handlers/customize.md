---
title: ".NET MAUI control customization with handlers"
description: "Learn how to customize .NET MAUI handlers, which map cross-platform controls to performant native controls on each platform."
ms.date: 05/12/2022
---

# Customize .NET MAUI controls with handlers

.NET Multi-platform App UI (.NET MAUI) provides a collection of controls that can be used to display data, initiate actions, indicate activity, display collections, pick data, and more. Each control has an interface representation, that abstracts the control. Cross-platform controls that implement these interfaces are known as *virtual views*. *Handlers* map these virtual views to native controls on each platform, and are responsible for creating the underlying native control, and mapping their properties to the cross-platform control. For example, on iOS a .NET MAUI handler maps a .NET MAUI `Button` to an iOS `UIButton` control. On Android, the `Button` is mapped to an `AppCompatButton` control:

:::image type="content" source="media/customize/button-handler.png" alt-text="Button handler architecture." border="false":::

Handlers are accessed through their control-specific interface, such as `IButton` for a `Button`. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control. Each handler provides a *mapper* that maps the cross-platform control API to the native control API.

Handlers can be customized to augment the appearance and behavior of a cross-platform control beyond the customization that's possible through the control's API. This customization is achieved by modifying the mapper for a handler, with one of the following methods:

- `PrependToMapping`, which modifies the mapper for a handler before the .NET MAUI control mappings have been applied.
- `ModifyMapping`, which modifies an existing mapping.
- `AppendToMapping`, which modifies the mapper for a handler after the .NET MAUI control mappings have been applied.

Each of these methods has an identical signature that requires two arguments:

- A `string`-based key. When modifying one of the mappings provided by .NET MAUI, the key used by .NET MAUI must be specified. The key values used by .NET MAUI control mappings are based on interface and property names, for example `nameof(IEntry.IsPassword)`. The interfaces, and their properties, that abstract each cross-platform control can be found [here](https://github.com/dotnet/maui/tree/main/src/Core/src/Core). Otherwise, this key can be an arbitrary value that doesn't have to correspond to the name of a property exposed by a type. For example, `MyCustomization` can be specified as a key, with any native control modification being performed as the customization.
- An `Action` that represents the method that performs the handler customization. The `Action` specifies two arguments:
  - A `handler` argument that provides an instance of the handler being customized.
  - A `view` argument that provides an instance of the cross-platform control that the handler implements.

> [!IMPORTANT]
> Handler customizations are global and aren't scoped to a specific control instance. Handler customization is allowed to happen anywhere in your app. Once a handler is customized, it affects all controls of that type, everywhere in your app.

Each handler class exposes the native control that implements the cross-platform control via its `PlatformView` property. This property can be accessed to set native control properties, invoke native control methods, and subscribe to native control events. In addition, the cross-platform control implemented by the handler is exposed via its `VirtualView` property.

Handlers can be customized per platform by using compiler preprocessor directives, to multi-target code based on the platform. Alternatively, you can use partial classes to organize your code into platform-specific folders and files. For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

For a list of the type names that implement handler-based .NET MAUI views, see [Handler-based views](#handler-based-views).

## Customize a control with a mapper

The .NET MAUI `Entry` is a single-line text input control, that implements the `IEntry` interface. On iOS, the `EntryHandler` maps the `Entry` to an iOS `UITextField` control. On Android, the `Entry` is mapped to an `AppCompatEditText` control, and on Windows the `Entry` is mapped to a `TextBox` control:

:::image type="content" source="media/customize/entry-handler.png" alt-text="Entry handler architecture." border="false":::

The `Entry` mapper, in the `EntryHandler` class, maps the cross-platform control API to the native control API. This mapper can be modified to customize the control on each platform:

```csharp
using Microsoft.Maui.Platform;

namespace CustomizeHandlersDemo;

public partial class CustomizeEntryPage : ContentPage
{
    public CustomizeEntryPage()
    {
        InitializeComponent();

        ModifyEntry();
    }

    void ModifyEntry()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif iOS
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
        });
    }
}
```

In this example, the `Entry` customization occurs in a page class. Therefore, all `Entry` controls on Android, iOS, and Windows will be customized once an instance of the `CustomizeEntryPage` is created. The following customization is performed by using compiler preprocessing directives:

- On Android, the background color of the `Entry` is set to transparent.
- On iOS, the border is removed from the `Entry`.
- On Windows, the thickness of the font in the `Entry` is set to thin.

## Customize specific control instances

Handlers are global, and customizing a handler for a control will result in all controls of the same type being customized in your app. However, handlers for specific control instances can be customized by subclassing the control, and then by modifying the handler for the base control type only when the control is of the subclassed type. For example, to customize a specific `Entry` control on a page that contains multiple `Entry` controls, you should first subclass the `Entry` control:

```csharp
namespace CustomizeHandlersDemo;

public class MyEntry : Entry
{
}
```

You can then customize the `EntryHandler`, via its mapper, to perform the desired modification to `MyEntry` instances:

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(IView.Background), (handler, view) =>
{
    if (view is MyEntry)
    {
#if ANDROID
        handler.PlatformView.SetBackgroundColor(Colors.Red.ToPlatform());
#elif IOS
        handler.PlatformView.BackgroundColor = Colors.Red.ToPlatform();
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.Line;
#elif WINDOWS
        handler.PlatformView.Background = Colors.Red.ToPlatform();
#endif
    }
});
```

Any `MyEntry` instances in the app will then be customized as per the handler modification.

## Handler lifecycle

All handler-based .NET MAUI controls support two handler lifecycle events:

- `HandlerChanging` is raised when a new handler is about to be created for a cross-platform control, and when an existing handler is about to be removed from a cross-platform control. The `HandlerChangingEventArgs` object that accompanies this event has `NewHandler` and `OldHandler` properties, of type `IElementHandler`. When the `NewHandler` property isn't `null`, the event indicates that a new handler is about to be created for a cross-platform control. When the `OldHandler` property isn't `null`, the event indicates that the existing native control is about be removed from the cross-platform control, and therefore that any native events should be unwired and other cleanup performed.
- `HandlerChanged` is raised after the handler for a cross-platform control has been created. This event indicates that the native control that implements the cross-platform control is available, and all the property values set on the cross-platform control have been applied to the native control.

> [!NOTE]
> The `HandlerChanging` event is raised on a cross-platform control before the `HandlerChanged` event.

In addition to these events, each cross-platform control also has an overridable `OnHandlerChanged` method that's invoked when the `HandlerChanged` event is raised, and a `OnHandlerChanging` method that's invoked when the `HandlerChanging` event is raised.

## Subscribe to native control events

A handlers `PlatformView` property can be accessed to set native control properties, invoke native control methods, and subscribe to native control events. Subscribing to a native control event should occur when the `HandlerChanged` event is raised, which indicates that the native control that implements the cross-platform control is available and initialized. Similarly, unsubscribing from the native event should occur when the `HandlerChanging` event is raised, which indicates that the control's handler is about to be removed from the cross-platform control. For more information about handler lifecycle events, see [Handler lifecycle](#handler-lifecycle).

To subscribe to, and unsubscribe from, native control events you must register event handlers for the `HandlerChanged` and `HandlerChanging` events on the cross-platform control being customized:

```xaml
<Entry HandlerChanged="OnHandlerChanged"
       HandlerChanging="OnHandlerChanging" />
```

Handlers can be customized per platform by using compiler preprocessor directives, or by using partial classes to organize your code into platform-specific folders and files. Each approach will be discussed in turn, by customizing an `Entry` on Android.

### Using preprocessor directives

The code-behind file containing the event handlers for the `HandlerChanged` and `HandlerChanging` events is shown in the following example, which uses preprocessor directives:

```csharp
using Microsoft.Maui.Platform;

namespace CustomizeHandlersDemo;

public partial class CustomizeEntryPage : ContentPage
{
    public CustomizeEntryPage()
    {
        InitializeComponent();
    }

    void OnHandlerChanged(object sender, EventArgs e)
    {
#if ANDROID
        ((sender as Entry).Handler.PlatformView as Android.Views.View).FocusChange += OnFocusChange;
#endif
    }

    void OnHandlerChanging(object sender, HandlerChangingEventArgs e)
    {
        if (e.OldHandler != null)
        {
#if ANDROID
            (e.OldHandler.PlatformView as Android.Views.View).FocusChange -= OnFocusChange;
#endif
        }
    }

#if ANDROID
    void OnFocusChange(object sender, EventArgs e)
    {
        var nativeView = sender as AndroidX.AppCompat.Widget.AppCompatEditText;

        if (nativeView.IsFocused)
            nativeView.SetBackgroundColor(Colors.LightPink.ToPlatform());
        else
            nativeView.SetBackgroundColor(Colors.Transparent.ToPlatform());
    }
#endif        
}
```

The `HandlerChanged` event is raised after the native control that implements the cross-platform control has been created and initialized. Therefore, its event handler is where native event subscriptions should be performed. This requires casting the `PlatformView` property of the handler to the type, or base type, of the native control so that native events can be accessed. In this example, the `OnHandlerChanged` event subscribes to the native control's `FocusChange` event.

The `OnFocusChange` event handler accesses the native control for the `Entry` and sets its background color as the control gains and loses focus.

The `HandlerChanging` event is raised before the existing handler is removed from the cross-platform control, and before the new handler for the cross-platform control is created. Therefore, its event handler is where native event subscriptions should be removed, and other cleanup should be performed. The `HandlerChangingEventArgs` object that accompanies this event has `OldHandler` and `NewHandler` properties, which will be set to the old and new handlers respectively. In this example, the `OnHandlerChanging` event removes the subscription to the native `FocusChange` event.

### Using partial classes

Rather than using compiler preprocessor directives to conditionally compile your app, it's also possible use partial classes to organize your control customization code into platform-specific folders and files. With this approach, your customization code is separated into a cross-platform partial class, and a platform-specific partial class. The following example shows the cross-platform partial class:

```csharp
namespace CustomizeHandlersDemo;

public partial class CustomizeEntryPage : ContentPage
{
    public CustomizePartialEntryPage()
    {
        InitializeComponent();
    }

    partial void ChangedHandler(object sender, EventArgs e);
    partial void ChangingHandler(object sender, HandlerChangingEventArgs e);

    void OnHandlerChanged(object sender, EventArgs e) => ChangedHandler(sender, e);

    void OnHandlerChanging(object sender, HandlerChangingEventArgs e) => ChangingHandler(sender, e);
}
```

In this example, the two event handlers call partial methods named `ChangedHandler` and `ChangingHandler`, whose signatures are defined in the cross-platform partial class. The partial method implementations are then defined in the platform-specific partial class, that's located in the **Platforms** > **Android** folder of the project:

```csharp
using Microsoft.Maui.Platform;

namespace CustomizeHandlersDemo;

public partial class CustomizeEntryPage : ContentPage
{
    partial void ChangedHandler(object sender, EventArgs e)
    {
        ((sender as Entry).Handler.PlatformView as Android.Views.View).FocusChange += OnFocusChange;
    }

    partial void ChangingHandler(object sender, HandlerChangingEventArgs e)
    {
        if (e.OldHandler != null)
        {
            (e.OldHandler.PlatformView as Android.Views.View).FocusChange -= OnFocusChange;
        }
    }

    void OnFocusChange(object sender, EventArgs e)
    {
        var nativeView = sender as AndroidX.AppCompat.Widget.AppCompatEditText;

        if (nativeView.IsFocused)
        {
            nativeView.SetBackgroundColor(Colors.LightPink.ToPlatform());
        }
        else
        {
            nativeView.SetBackgroundColor(Colors.Transparent.ToPlatform());
        }
    }
}
```

The advantage of this approach is that compiler preprocessing directives aren't required, and that the partial methods don't have to be implemented on each platform. If an implementation isn't provided on a platform, then the method and all calls to the method are removed at compile time. For information about partial methods, see [Partial methods](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-methods).

## Handler-based views

The following table lists the types that implement handler-based views in .NET MAUI:

| View | Interface | Handler | Mapper |
| -- | -- | -- | -- |
| `ActivityIndicator` | `IActivityIndicator` | `ActivityIndicatorHandler` | `Mapper` |
| `BlazorWebView` | `IBlazorWebView` | `BlazorWebViewHandler` | `BlazorWebViewMapper` |
| `Border` | `IBorderView` | `BorderHandler` | `Mapper` |
| `Button` | `IButton` | `ButtonHandler` | `Mapper` |
| `CarouselView` | | `CarouselViewHandler` | `Mapper` |
| `CheckBox` | `ICheckBox` | `CheckBoxHandler` | `Mapper` |
| `CollectionView` |  | `CollectionViewHandler` | `Mapper` |
| `ContentView` | `IContentView` | `ContentViewHandler` | `Mapper` |
| `DatePicker` | `IDatePicker` | `DatePickerHandler` | `Mapper` |
| `Editor` | `IEditor` | `EditorHandler` | `Mapper` |
| `Ellipse` | | `ShapeViewHandler` | `Mapper` |
| `Entry` | `IEntry` | `EntryHandler` | `Mapper` |
| `GraphicsView` | `IGraphicsView` | `GraphicsViewHandler` | `Mapper` |
| `Image` | `IImage` | `ImageHandler` | `Mapper` |
| `ImageButton` | `IImageButton` | `ImageButtonHandler` | `Mapper` |
| `IndicatorView` | `IIndicatorView` | `IndicatorViewHandler` | `Mapper` |
| `Label` | `ILabel` | `LabelHandler` | `Mapper` |
| `Line` | | `LineHandler` | `Mapper` |
| `Path` | | `PathHandler` | `Mapper` |
| `Picker` | `IPicker` | `PickerHandler` | `Mapper` |
| `Polygon` | | `PolygonHandler` | `Mapper` |
| `Polyline` | | `PolylineHandler` | `Mapper` |
| `ProgressBar` | `IProgress` | `ProgressBarHandler` | `Mapper` |
| `RadioButton` | `IRadioButton` | `RadioButtonHandler` | `Mapper` |
| `Rectangle` | | `RectangleHandler` | `Mapper` |
| `RefreshView` | `IRefreshView` | `RefreshViewHandler` | `Mapper` |
| `RoundRectangle` | | `RoundRectangleHandler` | `Mapper` |
| `ScrollView` | `IScrollView` | `ScrollViewHandler` | `Mapper` |
| `SearchBar` | `ISearchBar` | `SearchBarHandler` | `Mapper` |
| `Slider` | `ISlider` | `SliderHandler` | `Mapper` |
| `Stepper` | `IStepper` | `StepperHandler` | `Mapper` |
| `SwipeView` | `ISwipeView` | `SwipeViewHandler` | `Mapper` |
| `Switch` | `ISwitch` | `SwitchHandler` | `Mapper` |
| `TimePicker` | `ITimePicker` | `TimePickerHandler` | `Mapper` |
| `WebView` | `IWebView` | `WebViewHandler` | `Mapper` |

All handlers are in the `Microsoft.Maui.Handlers` namespace, with the following exceptions:

- `CarouselViewHandler` and `CollectionViewHandler` are in the `Microsoft.Maui.Controls.Handlers.Items` namespace.
- `LineHandler`, `PathHandler`, `PolygonHandler`, `PolylineHandler`, `RectangleHandler`, and `RoundRectangleHandler` are in the `Microsoft.Maui.Controls.Handlers` namespace.

The interfaces in the table above are in the `Microsoft.Maui` namespace.

<!-- Remove the text above once their are API docs that can be linked into -->

## Renderer-based views

The following legacy Xamarin.Forms views are backed by renderers, rather than handlers, and use a different customization approach:

- `BoxView`
- `Frame`
- `ListView`
- `TableView`
