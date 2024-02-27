---
title: ".NET MAUI control customization with handlers"
description: "Learn how to customize .NET MAUI handlers, to augment the appearance and behavior of a cross-platform control."
ms.date: 08/15/2022
---

# Customize controls with handlers

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-customizehandler)

Handlers can be customized to augment the appearance and behavior of a cross-platform control beyond the customization that's possible through the control's API. This customization, which modifies the native views for the cross-platform control, is achieved by modifying the mapper for a handler with one of the following methods:

- <xref:Microsoft.Maui.PropertyMapperExtensions.PrependToMapping%2A>, which modifies the mapper for a handler before the .NET MAUI control mappings have been applied.
- <xref:Microsoft.Maui.PropertyMapperExtensions.ModifyMapping%2A>, which modifies an existing mapping.
- <xref:Microsoft.Maui.PropertyMapperExtensions.AppendToMapping%2A>, which modifies the mapper for a handler after the .NET MAUI control mappings have been applied.

Each of these methods has an identical signature that requires two arguments:

- A `string`-based key. When modifying one of the mappings provided by .NET MAUI, the key used by .NET MAUI must be specified. The key values used by .NET MAUI control mappings are based on interface and property names, for example `nameof(IEntry.IsPassword)`. The interfaces, and their properties, that abstract each cross-platform control can be found [here](https://github.com/dotnet/maui/tree/main/src/Core/src/Core). This is the key format that should be used if you want your handler customization to run every time a property changes. Otherwise, this key can be an arbitrary value that doesn't have to correspond to the name of a property exposed by a type. For example, `MyCustomization` can be specified as a key, with any native view modification being performed as the customization. However, a consequence of this key format is your handler customization will only run when the mapper for a handler is modified.
- An <xref:System.Action> that represents the method that performs the handler customization. The <xref:System.Action> specifies two arguments:
  - A `handler` argument that provides an instance of the handler being customized.
  - A `view` argument that provides an instance of the cross-platform control that the handler implements.

> [!IMPORTANT]
> Handler customizations are global and aren't scoped to a specific control instance. Handler customization is allowed to happen anywhere in your app. Once a handler is customized, it affects all controls of that type, everywhere in your app.

Each handler class exposes the native view for the cross-platform control via its <xref:Microsoft.Maui.IElementHandler.PlatformView> property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform control implemented by the handler is exposed via its <xref:Microsoft.Maui.IElementHandler.VirtualView> property.

Handlers can be customized per platform by using conditional compilation, to multi-target code based on the platform. Alternatively, you can use partial classes to organize your code into platform-specific folders and files. For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

## Customize a control

The .NET MAUI <xref:Microsoft.Maui.Controls.Entry> view is a single-line text input control, that implements the <xref:Microsoft.Maui.IEntry> interface. The <xref:Microsoft.Maui.Handlers.EntryHandler> maps the <xref:Microsoft.Maui.Controls.Entry> view to the following native views for each platform:

- **iOS/Mac Catalyst**: `UITextField`
- **Android**: `AppCompatEditText`
- **Windows**: `TextBox`

The following diagrams shows how the <xref:Microsoft.Maui.Controls.Entry> view is mapped to its native views via the <xref:Microsoft.Maui.Handlers.EntryHandler>:

:::image type="content" source="media/customize/entry-handler.png" alt-text="Entry handler architecture." border="false":::

The <xref:Microsoft.Maui.Controls.Entry> property mapper, in the <xref:Microsoft.Maui.Handlers.EntryHandler> class, maps the cross-platform control properties to the native view API. This ensures that when a property is set on an <xref:Microsoft.Maui.Controls.Entry>, the underlying native view is updated as required.

The property mapper can be modified to customize <xref:Microsoft.Maui.Controls.Entry> on each platform:

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

In this example, the <xref:Microsoft.Maui.Controls.Entry> customization occurs in a page class. Therefore, all <xref:Microsoft.Maui.Controls.Entry> controls on Android, iOS, and Windows will be customized once an instance of the `CustomizeEntryPage` is created. Customization is performed by accessing the handlers <xref:Microsoft.Maui.IElementHandler.PlatformView> property, which provides access to the native view that maps to the cross-platform control on each platform. Native code then customizes the handler by selecting all of the text in the <xref:Microsoft.Maui.Controls.Entry> when it gains focus.

For more information about mappers, see [Mappers](index.md#mappers).

## Customize a specific control instance

Handlers are global, and customizing a handler for a control will result in all controls of the same type being customized in your app. However, handlers for specific control instances can be customized by subclassing the control, and then by modifying the handler for the base control type only when the control is of the subclassed type. For example, to customize a specific <xref:Microsoft.Maui.Controls.Entry> control on a page that contains multiple <xref:Microsoft.Maui.Controls.Entry> controls, you should first subclass the <xref:Microsoft.Maui.Controls.Entry> control:

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

All handler-based .NET MAUI controls support <xref:Microsoft.Maui.Controls.Element.HandlerChanging> and <xref:Microsoft.Maui.Controls.Element.HandlerChanged> events. The <xref:Microsoft.Maui.Controls.Element.HandlerChanged> event is raised when the native view that implements the cross-platform control is available and initialized. The <xref:Microsoft.Maui.Controls.Element.HandlerChanging> event is raised when the control's handler is about to be removed from the cross-platform control. For more information about handler lifecycle events, see [Handler lifecycle](index.md#handler-lifecycle).

The handler lifecycle can be used to perform handler customization. For example, to subscribe to, and unsubscribe from, native view events you must register event handlers for the <xref:Microsoft.Maui.Controls.Element.HandlerChanged> and <xref:Microsoft.Maui.Controls.Element.HandlerChanging> events on the cross-platform control being customized:

```xaml
<Entry HandlerChanged="OnEntryHandlerChanged"
       HandlerChanging="OnEntryHandlerChanging" />
```

Handlers can be customized per platform by using conditional compilation, or by using partial classes to organize your code into platform-specific folders and files. Each approach will be discussed in turn, by customizing an <xref:Microsoft.Maui.Controls.Entry> so that all of its text is selected when it gains focus.

### Conditional compilation

The code-behind file containing the event handlers for the <xref:Microsoft.Maui.Controls.Element.HandlerChanged> and <xref:Microsoft.Maui.Controls.Element.HandlerChanging> events is shown in the following example, which uses conditional compilation:

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
        Entry entry = sender as Entry;
#if ANDROID
        (entry.Handler.PlatformView as AppCompatEditText).SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
        (entry.Handler.PlatformView as UITextField).EditingDidBegin += OnEditingDidBegin;
#elif WINDOWS
        (entry.Handler.PlatformView as TextBox).GotFocus += OnGotFocus;
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

The <xref:Microsoft.Maui.Controls.Element.HandlerChanged> event is raised after the native view that implements the cross-platform control has been created and initialized. Therefore, its event handler is where native event subscriptions should be performed. This requires casting the <xref:Microsoft.Maui.IElementHandler.PlatformView> property of the handler to the type, or base type, of the native view so that native events can be accessed. In this example, on iOS, Mac Catalyst, and Windows, the `OnEntryHandlerChanged` event subscribes to native view events that are raised when the native views that implement the <xref:Microsoft.Maui.Controls.Entry> gain focus.

The `OnEditingDidBegin` and `OnGotFocus` event handlers access the native view for the <xref:Microsoft.Maui.Controls.Entry> on their respective platforms, and select all text that's in the <xref:Microsoft.Maui.Controls.Entry>.

The <xref:Microsoft.Maui.Controls.Element.HandlerChanging> event is raised before the existing handler is removed from the cross-platform control, and before the new handler for the cross-platform control is created. Therefore, its event handler is where native event subscriptions should be removed, and other cleanup should be performed. The <xref:Microsoft.Maui.Controls.HandlerChangingEventArgs> object that accompanies this event has <xref:Microsoft.Maui.Controls.HandlerChangingEventArgs.OldHandler> and <xref:Microsoft.Maui.Controls.HandlerChangingEventArgs.NewHandler> properties, which will be set to the old and new handlers respectively. In this example, the `OnEntryHandlerChanging` event removes the subscription to the native view events on iOS, Mac Catalyst, and Windows.

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
            Entry entry = sender as Entry;
            (entry.Handler.PlatformView as TextBox).GotFocus += OnGotFocus;
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

For information about the organization of the *Platforms* folder in a .NET MAUI project, see [Partial classes and methods](~/platform-integration/invoke-platform-code.md#partial-classes-and-methods). For information about how to configure multi-targeting so that you don't have to place platform code into sub-folders of the *Platforms* folder, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).
