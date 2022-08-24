---
title: ".NET MAUI control customization with handlers"
description: "Learn how to customize .NET MAUI handlers, to augment the appearance and behavior of a cross-platform control."
ms.date: 08/15/2022
---

# Customize controls with handlers

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-customizehandler)

Handlers can be customized to augment the appearance and behavior of a cross-platform control beyond the customization that's possible through the control's API. This customization is achieved by modifying the mapper for a handler, with one of the following methods:

- `PrependToMapping`, which modifies the mapper for a handler before the .NET MAUI control mappings have been applied.
- `ModifyMapping`, which modifies an existing mapping.
- `AppendToMapping`, which modifies the mapper for a handler after the .NET MAUI control mappings have been applied.

Each of these methods has an identical signature that requires two arguments:

- A `string`-based key. When modifying one of the mappings provided by .NET MAUI, the key used by .NET MAUI must be specified. The key values used by .NET MAUI control mappings are based on interface and property names, for example `nameof(IEntry.IsPassword)`. The interfaces, and their properties, that abstract each cross-platform control can be found [here](https://github.com/dotnet/maui/tree/main/src/Core/src/Core). Otherwise, this key can be an arbitrary value that doesn't have to correspond to the name of a property exposed by a type. For example, `MyCustomization` can be specified as a key, with any native view modification being performed as the customization.
- An `Action` that represents the method that performs the handler customization. The `Action` specifies two arguments:
  - A `handler` argument that provides an instance of the handler being customized.
  - A `view` argument that provides an instance of the cross-platform control that the handler implements.

> [!IMPORTANT]
> Handler customizations are global and aren't scoped to a specific control instance. Handler customization is allowed to happen anywhere in your app. Once a handler is customized, it affects all controls of that type, everywhere in your app.

Each handler class exposes the native view that implements the cross-platform control via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform control implemented by the handler is exposed via its `VirtualView` property.

Handlers can be customized per platform by using conditional compilation, to multi-target code based on the platform. Alternatively, you can use partial classes to organize your code into platform-specific folders and files. For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

## Customize a control

The .NET MAUI `Entry` is a single-line text input control, that implements the `IEntry` interface. On iOS, the `EntryHandler` maps the `Entry` to an iOS `UITextField` native view. On Android, the `Entry` is mapped to an `AppCompatEditText` native view, and on Windows the `Entry` is mapped to a `TextBox` native view:

:::image type="content" source="media/customize/entry-handler.png" alt-text="Entry handler architecture." border="false":::

The `Entry` property mapper, in the `EntryHandler` class, maps the cross-platform control properties to the native view API. This ensures that when a property is set on an `Entry`, the underlying native view is updated as required. The property mapper can be modified to customize the control on each platform:

```csharp
namespace CustomizeHandlersDemo.Views;

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
            handler.PlatformView.SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
            handler.PlatformView.EditingDidBegin += (s, e) =>
            {
                handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
            };
#elif WINDOWS
            handler.PlatformView.GotFocus += (s, e) =>
            {
                handler.PlatformView.SelectAll();
            };
#endif
        });
    }
}
```

In this example, the `Entry` customization occurs in a page class. Therefore, all `Entry` controls on Android, iOS, and Windows will be customized once an instance of the `CustomizeEntryPage` is created. Customization is performed by accessing the handlers `PlatformView` property, which provides access to the native view that implements the cross-platform control on each platform. Native code then customizes the handler by selecting all of the text in the `Entry` when it gains focus.

For more information about mappers, see [Mappers](index.md#mappers).

## Customize a specific control instance

Handlers are global, and customizing a handler for a control will result in all controls of the same type being customized in your app. However, handlers for specific control instances can be customized by subclassing the control, and then by modifying the handler for the base control type only when the control is of the subclassed type. For example, to customize a specific `Entry` control on a page that contains multiple `Entry` controls, you should first subclass the `Entry` control:

```csharp
namespace CustomizeHandlersDemo.Controls
{
    internal class MyEntry : Entry
    {
    }
}
```

You can then customize the `EntryHandler`, via its property mapper, to perform the desired modification only to `MyEntry` instances:

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
{
    if (view is MyEntry)
    {
#if ANDROID
        handler.PlatformView.SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
        handler.PlatformView.EditingDidBegin += (s, e) =>
        {
            handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
        };
#elif WINDOWS
        handler.PlatformView.GotFocus += (s, e) =>
        {
            handler.PlatformView.SelectAll();
        };
#endif
    }
});
```

If the handler customization is performed in your `App` class, any `MyEntry` instances in the app will be customized as per the handler modification.

## Customize a control using the handler lifecycle

All handler-based .NET MAUI controls support `HandlerChanging` and `HandlerChanged` events. The `HandlerChanged` event is raised when the native view that implements the cross-platform control is available and initialized. The `HandlerChanging` event is raised when the control's handler is about to be removed from the cross-platform control. For more information about handler lifecycle events, see [Handler lifecycle](index.md#handler-lifecycle).

The handler lifecycle can be used to perform handler customization. For example, to subscribe to, and unsubscribe from, native view events you must register event handlers for the `HandlerChanged` and `HandlerChanging` events on the cross-platform control being customized:

```xaml
<Entry HandlerChanged="OnEntryHandlerChanged"
       HandlerChanging="OnEntryHandlerChanging" />
```

Handlers can be customized per platform by using conditional compilation, or by using partial classes to organize your code into platform-specific folders and files. Each approach will be discussed in turn, by customizing an `Entry` so that all of its text is selected when it gains focus.

### Conditional compilation

The code-behind file containing the event handlers for the `HandlerChanged` and `HandlerChanging` events is shown in the following example, which uses conditional compilation:

```csharp
#if ANDROID
using AndroidX.AppCompat.Widget;
#elif IOS || MACCATALYST
using UIKit;
#elif WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
#endif

namespace CustomizeHandlersDemo.Views;

public partial class CustomizeEntryHandlerLifecyclePage : ContentPage
{
    public CustomizeEntryHandlerLifecyclePage()
    {
        InitializeComponent();
    }

    void OnEntryHandlerChanged(object sender, EventArgs e)
    {
#if ANDROID
        ((sender as Entry).Handler.PlatformView as AppCompatEditText).SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
        ((sender as Entry).Handler.PlatformView as UITextField).EditingDidBegin += OnEditingDidBegin;
#elif WINDOWS
        ((sender as Entry).Handler.PlatformView as TextBox).GotFocus += OnGotFocus;
#endif
    }

    void OnEntryHandlerChanging(object sender, HandlerChangingEventArgs e)
    {
        if (e.OldHandler != null)
        {
#if IOS || MACCATALYST
            (e.OldHandler.PlatformView as UITextField).EditingDidBegin -= OnEditingDidBegin;
#elif WINDOWS
            (e.OldHandler.PlatformView as TextBox).GotFocus -= OnGotFocus;
#endif
        }
    }

#if IOS || MACCATALYST                   
    void OnEditingDidBegin(object sender, EventArgs e)
    {
        var nativeView = sender as UITextField;
        nativeView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
    }
#elif WINDOWS
    void OnGotFocus(object sender, RoutedEventArgs e)
    {
        var nativeView = sender as TextBox;
        nativeView.SelectAll();
    }
#endif
}
```

The `HandlerChanged` event is raised after the native view that implements the cross-platform control has been created and initialized. Therefore, its event handler is where native event subscriptions should be performed. This requires casting the `PlatformView` property of the handler to the type, or base type, of the native view so that native events can be accessed. In this example, on iOS, Mac Catalyst, and Windows, the `OnHandlerChanged` event subscribes to native view events that are raised when the native views that implement the `Entry` gain focus.

The `OnEditingDidBegin` and `OnGotFocus` event handlers access the native view for the `Entry` on their respective platforms, and select all text that's in the `Entry`.

The `HandlerChanging` event is raised before the existing handler is removed from the cross-platform control, and before the new handler for the cross-platform control is created. Therefore, its event handler is where native event subscriptions should be removed, and other cleanup should be performed. The `HandlerChangingEventArgs` object that accompanies this event has `OldHandler` and `NewHandler` properties, which will be set to the old and new handlers respectively. In this example, the `OnHandlerChanging` event removes the subscription to the native view events on iOS, Mac Catalyst, and Windows.

### Partial classes

Rather than using conditional compilation, it's also possible to use partial classes to organize your control customization code into platform-specific folders and files. With this approach, your customization code is separated into a cross-platform partial class and a platform-specific partial class. The following example shows the cross-platform partial class:

```csharp
namespace CustomizeHandlersDemo.Views;

public partial class CustomizeEntryPartialMethodsPage : ContentPage
{
    public CustomizeEntryPartialMethodsPage()
    {
        InitializeComponent();
    }

    partial void ChangedHandler(object sender, EventArgs e);
    partial void ChangingHandler(object sender, HandlerChangingEventArgs e);

    void OnEntryHandlerChanged(object sender, EventArgs e) => ChangedHandler(sender, e);
    void OnEntryHandlerChanging(object sender, HandlerChangingEventArgs e) => ChangingHandler(sender, e);
}
```

> [!IMPORTANT]
> The cross-platform partial class shouldn't be placed in any of the *Platforms* child folders of your project.

In this example, the two event handlers call partial methods named `ChangedHandler` and `ChangingHandler`, whose signatures are defined in the cross-platform partial class. The partial method implementations are then defined in the platform-specific partial classes, which should be placed in the correct *Platforms* child folders to ensure that the build system only attempts to build native code when building for the specific platform. For example, the following code shows the `CustomizeEntryPartialMethodsPage` class in the *Platforms* > *Windows* folder of the project:

```csharp
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CustomizeHandlersDemo.Views
{
    public partial class CustomizeEntryPartialMethodsPage : ContentPage
    {
        partial void ChangedHandler(object sender, EventArgs e)
        {
            ((sender as Entry).Handler.PlatformView as TextBox).GotFocus += OnGotFocus;
        }

        partial void ChangingHandler(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
                (e.OldHandler.PlatformView as TextBox).GotFocus -= OnGotFocus;
            }
        }

        void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var nativeView = sender as TextBox;
            nativeView.SelectAll();
        }
    }
}
```

The advantage of this approach is that conditional compilation isn't required, and that the partial methods don't have to be implemented on each platform. If an implementation isn't provided on a platform, then the method and all calls to the method are removed at compile time. For information about partial methods, see [Partial methods](/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods#partial-methods).
