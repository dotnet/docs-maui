---
title: "Handler architecture in .NET MAUI"
description: "Understand the .NET MAUI handler architecture — how handlers map cross-platform controls to native views, how to create custom controls, and how to customize existing controls."
ms.date: 02/10/2026
ms.topic: conceptual
---

# Handler architecture in .NET MAUI

.NET Multi-platform App UI (.NET MAUI) uses a **handler architecture** to map cross-platform controls to native platform views. This guide explains the full architecture: what handlers are, how they work, how to create custom controls, and how to customize existing controls.

## What are handlers

Handlers are the bridge between cross-platform .NET MAUI controls and native platform views. Each .NET MAUI control has a corresponding handler that creates and manages the native control on each platform. For example, a .NET MAUI <xref:Microsoft.Maui.Controls.Button> maps to:

| Platform | Native view |
|---|---|
| Android | `MaterialButton` |
| iOS / Mac Catalyst | `UIButton` |
| Windows | `Microsoft.UI.Xaml.Controls.Button` |

The key architecture is a three-layer pipeline:

```
Virtual View (cross-platform control)  →  Handler  →  Platform View (native control)
```

The cross-platform control is known as the **virtual view**. It defines the public API (properties, events, commands) that developers use. The handler translates that API into calls on the **platform view** — the actual native control rendered on screen. Each handler exposes:

- `VirtualView` — the cross-platform control instance.
- `PlatformView` — the underlying native control.

```csharp
// Access the native Android EditText from an Entry handler
#if ANDROID
var nativeEditText = entry.Handler?.PlatformView as Android.Widget.EditText;
if (nativeEditText is not null)
{
    nativeEditText.SetHintTextColor(Android.Graphics.Color.Gray);
}
#endif
```

> [!IMPORTANT]
> Handlers replaced Xamarin.Forms renderers. If you are migrating from Xamarin.Forms, always use handlers — never use the legacy renderer compatibility infrastructure for new code.

## How handlers work

A handler inherits from `ViewHandler<TVirtualView, TPlatformView>`, where `TVirtualView` is the cross-platform control type and `TPlatformView` is the native view type. The handler uses two mapper dictionaries to keep the virtual view and platform view synchronized.

### Property mappers

A **property mapper** is a dictionary that maps cross-platform property names to static methods. When a property changes on the virtual view, the handler looks up the property name in the mapper and invokes the corresponding method to update the native view.

```csharp
public static IPropertyMapper<IRatingControl, RatingControlHandler> Mapper =
    new PropertyMapper<IRatingControl, RatingControlHandler>(ViewMapper)
    {
        [nameof(IRatingControl.Value)] = MapValue,
        [nameof(IRatingControl.Maximum)] = MapMaximum,
    };
```

Each mapped method receives the handler and virtual view, then updates the platform view:

```csharp
public static void MapValue(RatingControlHandler handler, IRatingControl control)
{
    handler.PlatformView.UpdateValue(control.Value);
}
```

### Command mappers

A **command mapper** works similarly but handles imperative actions (commands) rather than property changes. Commands can carry optional arguments.

```csharp
public static ICommandMapper<IRatingControl, RatingControlHandler> CommandMapper =
    new CommandMapper<IRatingControl, RatingControlHandler>(ViewCommandMapper)
    {
        [nameof(IRatingControl.Reset)] = MapReset,
    };

public static void MapReset(RatingControlHandler handler, IRatingControl control, object? args)
{
    handler.PlatformView.UpdateValue(0);
}
```

### Key handler overrides

When implementing a handler, you typically override:

| Method | Purpose |
|---|---|
| `CreatePlatformView()` | Creates and returns the native view instance. |
| `ConnectHandler()` | Subscribes to native events after the native view is created. |
| `DisconnectHandler()` | Unsubscribes from native events and performs cleanup. |

```csharp
protected override PlatformRatingView CreatePlatformView()
{
    return new PlatformRatingView(Context);
}

protected override void ConnectHandler(PlatformRatingView platformView)
{
    base.ConnectHandler(platformView);
    platformView.ValueChanged += OnPlatformValueChanged;
}

protected override void DisconnectHandler(PlatformRatingView platformView)
{
    platformView.ValueChanged -= OnPlatformValueChanged;
    base.DisconnectHandler(platformView);
}
```

## Creating a custom control with a handler

Building a custom control requires four pieces: a cross-platform control, an interface, a handler, and platform implementations. The following example creates a `RatingControl` that displays a star rating.

### Step 1 — Define the cross-platform control

Create the control class with <xref:Microsoft.Maui.Controls.BindableProperty> declarations:

```csharp
namespace MyApp.Controls;

public class RatingControl : View
{
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(RatingControl), 0.0);

    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(double), typeof(RatingControl), 5.0);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
}
```

Define an interface so the handler can work against an abstraction:

```csharp
namespace MyApp.Controls;

public interface IRatingControl : IView
{
    double Value { get; }
    double Maximum { get; }
}
```

Update the control to implement the interface:

```csharp
public class RatingControl : View, IRatingControl
{
    // ... BindableProperty declarations unchanged ...
}
```

### Step 2 — Create the partial handler class

The handler is a `partial` class. The shared file declares the property mapper and constructor, while platform-specific files provide `CreatePlatformView()` and mapper methods.

**Shared handler (all platforms):**

```csharp
#if ANDROID
using PlatformView = Android.Widget.ProgressBar;
#elif IOS || MACCATALYST
using PlatformView = UIKit.UIProgressView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.ProgressBar;
#endif

namespace MyApp.Handlers;

public partial class RatingControlHandler : ViewHandler<IRatingControl, PlatformView>
{
    public static IPropertyMapper<IRatingControl, RatingControlHandler> Mapper =
        new PropertyMapper<IRatingControl, RatingControlHandler>(ViewMapper)
        {
            [nameof(IRatingControl.Value)] = MapValue,
            [nameof(IRatingControl.Maximum)] = MapMaximum,
        };

    public RatingControlHandler() : base(Mapper)
    {
    }
}
```

### Step 3 — Platform implementations

Each platform file provides `CreatePlatformView()` and the static mapper methods.

**Android** (`Platforms/Android/RatingControlHandler.cs`):

```csharp
namespace MyApp.Handlers;

public partial class RatingControlHandler
{
    protected override Android.Widget.ProgressBar CreatePlatformView()
    {
        return new Android.Widget.ProgressBar(Context, null, Android.Resource.Attribute.ProgressBarStyleHorizontal)
        {
            Max = (int)VirtualView.Maximum,
            Progress = (int)VirtualView.Value,
        };
    }

    public static void MapValue(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Progress = (int)control.Value;
    }

    public static void MapMaximum(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Max = (int)control.Maximum;
    }
}
```

**iOS / Mac Catalyst** (`Platforms/iOS/RatingControlHandler.cs`):

```csharp
namespace MyApp.Handlers;

public partial class RatingControlHandler
{
    protected override UIKit.UIProgressView CreatePlatformView()
    {
        return new UIKit.UIProgressView(UIKit.UIProgressViewStyle.Default);
    }

    public static void MapValue(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Progress = (float)(control.Value / control.Maximum);
    }

    public static void MapMaximum(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Progress = (float)(control.Value / control.Maximum);
    }
}
```

**Windows** (`Platforms/Windows/RatingControlHandler.cs`):

```csharp
namespace MyApp.Handlers;

public partial class RatingControlHandler
{
    protected override Microsoft.UI.Xaml.Controls.ProgressBar CreatePlatformView()
    {
        return new Microsoft.UI.Xaml.Controls.ProgressBar
        {
            Minimum = 0,
            Maximum = VirtualView.Maximum,
            Value = VirtualView.Value,
        };
    }

    public static void MapValue(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Value = control.Value;
    }

    public static void MapMaximum(RatingControlHandler handler, IRatingControl control)
    {
        handler.PlatformView.Maximum = control.Maximum;
    }
}
```

### Step 4 — Register the handler

Register the handler in `MauiProgram.cs` using <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A>:

```csharp
using MyApp.Controls;
using MyApp.Handlers;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<RatingControl, RatingControlHandler>();
            });

        return builder.Build();
    }
}
```

After registration, use the control in XAML:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:MyApp.Controls"
             x:Class="MyApp.MainPage">
    <controls:RatingControl Value="3" Maximum="5" />
</ContentPage>
```

## Customizing existing controls

The most common use of handlers is customizing existing .NET MAUI controls to apply platform-specific tweaks. You do this by modifying a handler's property mapper — no subclassing of the handler is required.

> [!IMPORTANT]
> Handler customizations are global. Once you modify a handler's mapper, the change affects **every** instance of that control type throughout your app.

### AppendToMapping

<xref:Microsoft.Maui.PropertyMapperExtensions.AppendToMapping%2A> runs your customization **after** .NET MAUI's own property mapping. Use it for platform-specific tweaks that build on top of the default behavior.

```csharp
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
```

### PrependToMapping

<xref:Microsoft.Maui.PropertyMapperExtensions.PrependToMapping%2A> runs your customization **before** .NET MAUI's own property mapping. Use it when you need to set platform defaults before .NET MAUI applies its mappings.

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.PrependToMapping("DefaultPlaceholderColor", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.SetHintTextColor(Android.Graphics.Color.LightGray);
#elif IOS || MACCATALYST
    // Set a default attributed placeholder before MAUI applies its own
    handler.PlatformView.AttributedPlaceholder = new Foundation.NSAttributedString(
        handler.PlatformView.Placeholder ?? string.Empty,
        new UIKit.UIStringAttributes { ForegroundColor = UIKit.UIColor.LightGray });
#endif
});
```

### ModifyMapping

<xref:Microsoft.Maui.PropertyMapperExtensions.ModifyMapping%2A> replaces or wraps an existing mapping. The third parameter gives you access to the original mapping action so you can call it conditionally.

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.ModifyMapping(
    nameof(IEntry.Background), (handler, view, action) =>
{
    // Call the default mapping first
    action?.Invoke(handler, view);

    // Then apply additional behavior
#if ANDROID
    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.AliceBlue);
#endif
});
```

### Scoping customization to a specific control instance

Because handler customizations are global, use a type check to limit the customization to a subclassed control:

```csharp
public class BorderlessEntry : Entry { }
```

Then check the type inside the mapping action:

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
{
    if (view is BorderlessEntry)
    {
#if ANDROID
        handler.PlatformView.Background = null;
        handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
    }
});
```

Use the subclassed control in XAML just like the original:

```xaml
<local:BorderlessEntry Placeholder="No border here" />
```

### Global handler customization in MauiProgram.cs

Register all handler customizations during app startup in `MauiProgram.cs` to ensure they apply before any UI is rendered:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureMauiHandlers(handlers =>
            {
                // Register custom handlers
                handlers.AddHandler<RatingControl, RatingControlHandler>();
            });

        // Customize existing handlers after the builder is configured
        EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        return builder.Build();
    }
}
```

> [!NOTE]
> You can place `AppendToMapping` / `PrependToMapping` calls anywhere that runs before the control is displayed, but `MauiProgram.cs` is the conventional and recommended location for global customizations.

## Handler lifecycle

Every handler-based .NET MAUI control raises two lifecycle events.

### HandlerChanging

Raised when a new handler is about to be set, or when the current handler is about to be removed. Use `HandlerChangingEventArgs` to inspect `NewHandler` and `OldHandler`:

```csharp
myControl.HandlerChanging += (s, e) =>
{
    if (e.OldHandler is not null)
    {
        // Unwire native events from the old handler
        // This is your cleanup opportunity
    }

    if (e.NewHandler is not null)
    {
        // A new handler is about to be assigned
    }
};
```

### HandlerChanged

Raised after the handler is fully created and all cross-platform property values have been applied to the native view. This is the safe point to access `PlatformView`:

```csharp
myEntry.HandlerChanged += (s, e) =>
{
    if (myEntry.Handler?.PlatformView is not null)
    {
#if ANDROID
        var editText = (Android.Widget.EditText)myEntry.Handler.PlatformView;
        editText.SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
        var textField = (UIKit.UITextField)myEntry.Handler.PlatformView;
        textField.ClearButtonMode = UIKit.UITextFieldViewMode.WhileEditing;
#endif
    }
};
```

> [!NOTE]
> `HandlerChanging` is always raised before `HandlerChanged`.

## Common handler customization recipes

The following recipes demonstrate frequent platform-specific customizations.

### Remove Entry underline / border

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.BackgroundTintList =
        Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
    handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
#endif
});
```

### Set native button corner radius

```csharp
Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("RoundedButton", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.CornerRadius = 20;
#elif IOS || MACCATALYST
    handler.PlatformView.Layer.CornerRadius = 20;
    handler.PlatformView.ClipsToBounds = true;
#endif
});
```

### Customize Editor placeholder text

```csharp
Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("PlaceholderColor", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.SetHintTextColor(Android.Graphics.Color.Gray);
#elif IOS || MACCATALYST
    // iOS Editor (UITextView) doesn't have a built-in placeholder;
    // use a custom overlay or set text attributes in the delegate.
#endif
});
```

### Access the native navigation bar on iOS

```csharp
Microsoft.Maui.Handlers.NavigationViewHandler.Mapper.AppendToMapping("TransparentNavBar", (handler, view) =>
{
#if IOS || MACCATALYST
    var navController = handler.ViewController?.NavigationController;
    if (navController is not null)
    {
        navController.NavigationBar.SetBackgroundImage(new UIKit.UIImage(), UIKit.UIBarMetrics.Default);
        navController.NavigationBar.ShadowImage = new UIKit.UIImage();
        navController.NavigationBar.Translucent = true;
    }
#endif
});
```

## When to use handlers vs other approaches

| Approach | When to use |
|---|---|
| **Handler customization** (`AppendToMapping` / `PrependToMapping`) | You need platform-specific visual or behavioral changes to an existing control. This is the most common scenario. |
| **Custom control with handler** | No built-in .NET MAUI control fits your requirements, and you need full control over the native view on each platform. |
| **Platform code via `#if`** | You need platform APIs that aren't related to UI controls, such as accessing sensors or platform services. See [Invoke platform code](../../platform-integration/invoke-platform-code.md). |
| **Effects** | Deprecated in .NET MAUI. Always use handlers instead. |

> [!NOTE]
> If you are migrating from Xamarin.Forms, convert all custom renderers and effects to handlers. The handler architecture is more performant and decouples the cross-platform control from its native implementation.

## See also

- [Handlers overview](index.md)
- [Create custom controls with handlers](create.md)
- [Customize controls with handlers](customize.md)
- [Invoke platform code](../../platform-integration/invoke-platform-code.md)
