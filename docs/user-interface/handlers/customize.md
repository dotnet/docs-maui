---
title: ".NET MAUI control customization with handlers"
description: ".NET MAUI handlers map cross-platform controls to performant native controls on each platform."
ms.date: 06/10/2021
---

# Customize .NET MAUI controls with handlers

.NET Multi-platform App UI (.NET MAUI) provides a collection of controls that can be used to display data, initiate actions, indicate activity, display collections, pick data, and more. By default, *handlers* map these cross-platform controls to native controls on each platform. For example, on iOS a .NET MAUI handler will map a .NET MAUI `Button` to an iOS `UIButton`. On Android, the `Button` will be mapped to a `AppCompatButton`:

:::image type="content" source="customize-images/button-handler.png" alt-text="Button handler architecture." border="false":::

Handlers can be accessed through a control-specific interface provided by .NET MAUI, such as `IButton` for a `Button`. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control. The mapping of the cross-platform control API to the platform API is provided by a *mapper*.

Handlers expose the native control that implements the .NET MAUI cross-platform control via the `NativeView` property. This property can be accessed to set platform properties, invoke platform methods, and subscribe to platform events.

Handlers are global and therefore customization can occur anywhere in your .NET MAUI app. For example, customizing a handler for a control in your `App` class will result in all controls of that type being customized in the app. Similarly, customizing a handler for a control in the first page of your app will also result in all controls of that type in the app being customized.

## Examples

Handlers are typically customized to augment the appearance and behavior of a native platform control. Handlers can be customized per platform by using the `#ifdef` compiler directive, to multi-target code based on the platform. However, you can just as easily use platform-specific folders or files to organize such code.

> [!NOTE]
> Handler customization can range from the simple to the complex. More complex examples will appear in time.

### Customize the background color of all controls

The following example customizes the background color of every control in the app, on Android, to cyan:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

#if __ANDROID__
        Microsoft.Maui.Handlers.ViewHandler.ViewMapper[nameof(IView.Background)] = (h, v) =>
        {
            (h.NativeView as Android.Views.View).SetBackgroundColor(Microsoft.Maui.Graphics.Colors.Cyan.ToNative());
        };
#endif
    }
}
```

### Remove Android Entry underline

The following example removes the underline from all `Entry` controls in the app, on Android:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage, IPage
{
    public MainPage()
    {
        InitializeComponent();
#if __ANDROID__
        Handlers.EntryHandler.EntryMapper[nameof(IEntry.Background)] = (h, v) =>
        {
            (h.NativeView as global::Android.Views.Entry).UnderlineVisible = false;
        };
#endif
    }
}
```

### Customize specific control instances

Handlers for specific control instances can be customized by subclassing the control, and then by modifying the handler for the parent control only when the control is of the subclassed type. For example, to customize specific `Entry` controls on a page that contains multiple `Entry` controls, you should first subclass the `Entry` control:

```csharp
using Microsoft.Maui.Controls;

namespace MyMauiApp
{
    public class MyEntry : Entry
    {
    }
}
```

You can then customize the `EntryHandler` to perform the desired modification to `MyEntry` instances:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Microsoft.Maui.Handlers.EntryHandler.EntryMapper[nameof(IView.Background)] = (handler, view) =>
            {
                if (view is MyEntry)
                {
#if __ANDROID__
                    handler.NativeView.SetBackgroundColor(Colors.Red.ToNative());
#elif __IOS__
                  handler.NativeView.BackgroundColor = Colors.Red.ToNative();
                  handler.NativeView.BorderStyle = UIKit.UITextBorderStyle.Line;
#elif WINDOWS
                  handler.NativeView.Background = Colors.Red.ToNative();
#endif
                }
            };
        }
    }
}
```

Any `MyEntry` instances will then be customized as per the handler modification.
