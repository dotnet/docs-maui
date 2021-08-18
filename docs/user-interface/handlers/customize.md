---
title: "Control customization with handlers"
description: ".NET MAUI handlers map cross-platform controls to performant native controls on each platform."
ms.date: 08/18/2021
---

# Customize controls with handlers

<!-- Sample link goes here -->

.NET Multi-platform App UI (.NET MAUI) provides a collection of controls that can be used to display data, initiate actions, indicate activity, display collections, pick data, and more. By default, *handlers* map these cross-platform controls to native controls on each platform. For example, on iOS a .NET MAUI handler maps a .NET MAUI `Button` to an iOS `UIButton`. On Android, the `Button` is mapped to an `AppCompatButton`:

:::image type="content" source="customize-images/button-handler.png" alt-text="Button handler architecture." border="false":::

Handlers can be accessed through a control-specific interface provided by .NET MAUI, such as `IButton` for a `Button`. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control. The mapping of the cross-platform control API to the native control API is provided by a *mapper*.

All handler-based .NET MAUI controls can be customized by modifying the mapper for a control's handler. The syntax for modifying a mapper is as follows:

```csharp
<HandlerType>.<MapperName>[<string>] = (handler, view) =>
{
  // Per-platform handler customization logic goes here
};
```

All handler types are located in the `Microsoft.Maui.Handlers` namespace, and each handler provides a mapper that maps each cross-platform control property to a method that applies the property value to the native control. The `handler` argument to the mapper provides an instance of the handler being customized, and the `view` argument provides an instance of the cross-platform control that the handler implements. For a list of the type names that implement handler-based .NET MAUI controls, see [Handler-based controls](#handler-based-controls).

Handlers expose the cross-platform control implemented by the handler via the `VirtualView` property. In addition, the native control that implements the .NET MAUI cross-platform control is exposed by the handlers' `NativeView` property. This property can be accessed to set native control properties, invoke native control methods, and subscribe to native control events.

> [!IMPORTANT]
> Handlers should only be customized to augment the appearance and behavior of a .NET MAUI control beyond that provided by the control's cross-platform API.

Handlers are global and therefore customization can occur anywhere in your .NET MAUI app. For example, customizing a handler for a control in your `App` class will result in all controls of that type being customized in the app. Similarly, customizing a handler for a control in the first page of your app will also result in all controls of that type in the app being customized.

Handlers can be customized per platform by using the `#if` preprocessor directive, to multi-target code based on the platform. However, you can just as easily use platform-specific folders and files to organize your code. For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

## Handler lifecycle

All handler-based .NET MAUI controls support two handler lifecycle events:

- `HandlerChanged` is fired after the handler for a cross-platform control has been created. This event signals that the native control that implements the cross-platform control is available, and all the property values set on the cross-platform control have been applied to the native control.
- `HandlerChanging` is fired before an existing handler is removed from a cross-platform control, and before a new handler for the cross-platform control has been created. This event signals that the existing native control is about be removed from the cross-platform control, and therefore that any native events should be unwired and other cleanup performed. The `HandlerChangingEventArgs` object that accompanies this event has `OldHandler` and `NewHandler` properties, of type `IElementHandler`.

 before a new handler for a cross-platform control is created

In addition to these events, each control also features an overridable `OnHandlerChanged` method that's invoked when the `HandlerChanged` event fires, and a `OnHandlerChanging` method that's invoked when the `HandlerChanging` event fires.

## Customize a control with a mapper

The .NET MAUI `Entry` is a single-line text input control, that implements the `IEntry` interface. On iOS, the `EntryHandler` maps the `Entry` to an iOS `UITextField`. On Android, the `Entry` is mapped to an `AppCompatEditText`, and on Windows the `Entry` is mapped to a `TextBox`:

:::image type="content" source="customize-images/entry-handler.png" alt-text="Entry handler architecture." border="false":::

The `EntryHandler` class maps the cross-platform control API to the native control API with the `EntryMapper`. This mapper can be modified to customize the control on each platform:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace HandlersDemos.Views
{
    public partial class CustomizeEntryPage : ContentPage
    {
        public CustomizeEntryPage()
        {
            InitializeComponent();

            Microsoft.Maui.Handlers.EntryHandler.EntryMapper["MyCustomization"] = (handler, view) =>
            {
#if __ANDROID__
                handler.NativeView.SetBackgroundColor(Colors.Transparent.ToNative());
#elif __iOS__
                handler.NativeView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                handler.NativeView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
            };
        }
    }
}
```

In this example, the `Entry` customization occurs in a page class. Therefore, all `Entry` controls on Android, iOS, and Windows will be customized once the `CustomizeEntryPage` is displayed. The following customization is performed:

- On Android, the underline is removed from the `Entry`.
- On iOS, the border is removed from the `Entry`.
- On Windows, the thickness of the font in the `Entry` is set to thin.

> [!IMPORTANT]
> The index into a mapper is an arbitrary string and does not have to correspond to a property exposed by a type, or a property on a native control. For example, `["MyCustomization"]` can be specified as a mapper index, with any native control modification being performed as the customization.

## Customize specific control instances

Handlers are global, and therefore customizing a handler for a control will result in all controls of that type being customized in your app. However, handlers for specific control instances can be customized by subclassing the control, and then by modifying the handler for the parent control only when the control is of the subclassed type. For example, to customize a spcific `Entry` control on a page that contains multiple `Entry` controls, you should first subclass the `Entry` control:

```csharp
using Microsoft.Maui.Controls;

namespace HandlersDemos.Controls
{
    public class MyEntry : Entry
    {
    }
}
```

You can then customize the `EntryHandler`, via its mapper, to perform the desired modification to `MyEntry` instances:

```csharp
Microsoft.Maui.Handlers.EntryHandler.EntryMapper["MyCustomization"] = (handler, view) =>
{
    if (view is MyEntry)
    {
#if __ANDROID__
        handler.NativeView.SetBackgroundColor(Colors.Transparent.ToNative());
#elif __iOS__
        handler.NativeView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
        handler.NativeView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
    }
};
```

Any `MyEntry` instances in the app will then be customized as per the handler modification.

## Subscribe to native control events

A handlers `NativeView` property can be accessed to set native control properties, invoke native control methods, and subscribe to native control events. Subscribing to a native control event should occur when the `HandlerChanged` event fires, which indicates that the native control that implements the cross-platform control is available. Similarly, unsubscribing from the same event should occur when the `HandlerChanging` event fires, which indicates that the control's handler is about to be removed from the cross-platform control. For more information about handler lifecycle events, see [Handler lifecycle](#handler-lifecycle).

Therefore, to subscribe to, and unsubscribe from, native control events you must register event handlers for the `HandlerChanged` and `HandlerChanging` events on the cross-platform control being customized:

```xaml
<Entry Placeholder="Entry customized with a native event"
       HandlerChanged="OnHandlerChanged"
       HandlerChanging="OnHandlerChanging" />
```

Handlers can be customized per platform by using the `#if` preprocessor directive, or by using platform-specific folders and files to organize your code. Each approach will be discussed in turn.

### Using preprocessor directives

The code-behind file containing the event handlers for the `HandlerChanged` and `HandlerChanging` events is shown in the following example, which uses preprocessor directives to customize the `Entry` control on Android:

```csharp
using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace HandlersDemos.Views
{
    public partial class CustomizeEntryPage : ContentPage
    {
        public CustomizeEntryPage()
        {
            InitializeComponent();
        }

        void OnHandlerChanged(object sender, EventArgs e)
        {
#if __ANDROID__
            ((sender as Entry).Handler.NativeView as Android.Views.View).FocusChange += OnFocusChange;
#endif
        }

        void OnHandlerChanging(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
#if __ANDROID__
                (e.OldHandler.NativeView as Android.Views.View).FocusChange -= OnFocusChange;
#endif
            }
        }

#if __ANDROID__
        void OnFocusChange(object sender, EventArgs e)
        {
            var nativeView = sender as AndroidX.AppCompat.Widget.AppCompatEditText;

            if (nativeView.IsFocused)
                nativeView.SetBackgroundColor(Colors.LightPink.ToNative());
            else
                nativeView.SetBackgroundColor(Colors.White.ToNative());
        }
#endif        
    }
}
```

The `HandlerChanged` event fires after the handler for a cross-platform control has been created, signalling that the corresponding native control is available for use. Therefore, the event handler for this event is where any native event subscriptions should be performed. Doing so involves casting the `NativeView` property of the handler to the type, or base type, of the native control so that native events can be accessed. In this example, the `OnHandlerChanged` event subscribes to the native control's `FocusChange` event.

The `OnFocusChange` event handler accesses the native control for the `Entry` and sets its background color as the control gains and loses focus.

The `HandlerChanging` event fires before an existing handler is removed from a cross-platform control, before a new handler for a cross-platform control is created. Therefore, the event handler for this event is where any native event subscriptions should be removed, and other cleanup, should be performed. The `HandlerChangingEventArgs` object that accompanies this event has `OldHandler` and `NewHandler` properties, which will be set to the old and new handlers respectively. In this example, the `OnHandlerChanging` event removes the subscription to the native `FocusChange` event.

### Using platform-specific folders and files

Rather than using compiler preprocessor directives to conditionally compile your app, it's also possible to organize your platform code using platform-specific folders and files.

The code-behind file containing the event handlers for the `HandlerChanged` and `HandlerChanging` events is shown in the following example, which partial methods and partial classes to organize code to customize the `Entry` control on Android:

```csharp
using System;
using Microsoft.Maui.Controls;

namespace HandlersDemos.Views
{
    public partial class CustomizeEntryPartialClassPage : ContentPage
    {
        public CustomizeEntryPartialClassPage()
        {
            InitializeComponent();
        }

        partial void ChangedHandler(object sender, EventArgs e);
        partial void ChangingHandler(object sender, HandlerChangingEventArgs e);

        void OnHandlerChanged(object sender, EventArgs e)
        {
            ChangedHandler(sender, e);
        }

        void OnHandlerChanging(object sender, HandlerChangingEventArgs e)
        {
            ChangingHandler(sender, e);
        }
    }
}
```

In this example the two event handlers call partial methods named `ChangedHandler` and `ChangingHandler`, whose signatures are defined in this part of the partial class. The partial methods implementations are defined in another part of the partial class, that's located in the **Platforms** > **Android** folder of the project:

```csharp
using AndroidX.AppCompat.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace HandlersDemos.Views
{
    public partial class CustomizeEntryPartialClassPage : ContentPage
    {
        partial void ChangedHandler(object sender, EventArgs e)
        {
            ((sender as Entry).Handler.NativeView as Android.Views.View).FocusChange += OnFocusChange;
        }

        partial void ChangingHandler(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
                (e.OldHandler.NativeView as Android.Views.View).FocusChange -= OnFocusChange;
            }
        }

        void OnFocusChange(object sender, EventArgs e)
        {
            var nativeView = sender as AppCompatEditText;

            if (nativeView.IsFocused)
                nativeView.SetBackgroundColor(Colors.LightPink.ToNative());
            else
                nativeView.SetBackgroundColor(Colors.White.ToNative());
        }
    }
}
```

The `HandlerChanged` event fires after the handler for a cross-platform control has been created, signalling that the corresponding native control is available for use. Therefore, the event handler for this event is where any native event subscriptions should be performed. Doing so involves casting the `NativeView` property of the handler to the type, or base type, of the native control so that native events can be accessed. In this example, the `OnHandlerChanged` event subscribes to the native control's `FocusChange` event.

The `OnFocusChange` event handler accesses the native control for the `Entry` and sets its background color as the control gains and loses focus.

The `HandlerChanging` event fires before an existing handler is removed from a cross-platform control, before a new handler for a cross-platform control is created. Therefore, the event handler for this event is where any native event subscriptions should be removed, and other cleanup, should be performed. The `HandlerChangingEventArgs` object that accompanies this event has `OldHandler` and `NewHandler` properties, which will be set to the old and new handlers respectively. In this example, the `OnHandlerChanging` event removes the subscription to the native `FocusChange` event.

The advantage of this approach is that the partial methods don't have to be implemented on each platform. Instead, the compiler removes the signature at compile time on any platforms that don't provide an implementation. For information about partial methods, see [Partial methods](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-methods).

## Handler-based controls

The following table lists the names of the types that implement handler-based .NET MAUI controls:

| Control | Interface | Handler | Mapper |
| -- | -- | -- | -- |
| `ActivityIndicator` | `IActivityHandler` | `ActivityIndicatorHandler` | `ActivityIndicatorMapper` |
| `Button` | `IButton` | `ButtonHandler` | `ButtonMapper` |
| `CheckBox` | `ICheckBox` | `CheckBoxHandler` | `CheckBoxMapper` |
| `DatePicker` | `IDatePicker` | `DatePickerHandler` | `DatePickerMapper` |
| `Editor` | `IEditor` | `EditorHandler` | `EditorMapper` |
| `Element` | `IElement` | `ElementHandler` | `ElementMapper` |
| `Entry` | `IEntry` | `EntryHandler` | `EntryMapper` |
| `GraphicsView` | `IGraphicsView` | `GraphicsViewHandler` | `GraphicsViewMapper` |
| `Image` | `IImage` | `ImageHandler` | `ImageMapper` |
| `Label` | `ILabel` | `LabelHandler` | `LabelMapper` |
| `Layout` | `ILayout` | `LayoutHandler` | `LayoutMapper` |
| `NavigationPage` | `INavigationView` | `NavigationPageHandler` | `NavigationPageMapper` |
| `Page` | `IView` | `PageHandler` | `PageMapper` |
| `Picker` | `IPicker` | `PickerHandler` | `PickerMapper` |
| `ProgressBar` | `IProgress` | `ProgressBarHandler` | `ProgressBarMapper` |
| `ScrollView` | `IScrollView` | `ScrollViewHandler` | `ScrollViewMapper` |
| `SearchBar` | `ISearchBar` | `SearchBarHandler` | `SearchBarMapper` |
| `ShapeView` | `IShapeView` | `ShapeViewHandler` | `ShapeViewMapper` |
| `Slider` | `ISlider` | `SliderHandler` | `SliderMapper` |
| `Stepper` | `IStepper` | `StepperHandler` | `StepperMapper` |
| `Switch` | `ISwitch` | `SwitchHandler` | `SwitchMapper` |
| `TimePicker` | `ITimePicker` | `TimePickerHandler` | `TimePickerMapper` |
| `View` | `IView` | `ViewHandler` | `ViewMapper` |
| `Window` | `IWindow` | `WindowHandler` | `WindowMapper` |

> [!IMPORTANT]
> The .NET MAUI cross-platform controls are located in the `Microsoft.Maui.Controls` namespace, while handlers for those controls are located in the `Microsoft.Maui.Handlers` namespace. The control interfaces are located in the `Microsoft.Maui` namespace.

## Renderer-based controls

The following controls are backed by renderers:

- `CarouselView`
- `CollectionView`
- `FlyoutPage`
- `IndicatorView`
- `ListView`
- `Map`
- `RefreshView`
- `SwipeView`
- `TabbedPage`
- `TableView`

For more information about renderers, see [Xamarin.Forms custom renderers](/xamarin/xamarin-forms/app-fundamentals/custom-renderer/).
