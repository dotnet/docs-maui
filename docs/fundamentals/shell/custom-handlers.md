---
title: ".NET MAUI Shell custom handlers"
description: "Learn how to customize the appearance and behavior of a .NET MAUI Shell app using platform-specific handlers."
ms.date: 10/08/2026
ai-usage: ai-assisted
---

# .NET MAUI Shell custom handlers

::: moniker range=">=net-maui-11.0"

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

.NET MAUI Shell applications are highly customizable through the properties and methods that the various Shell classes expose. However, it's also possible to create custom handlers when more extensive platform-specific customizations are required. Custom handlers can be registered conditionally for a single platform, while allowing the default behavior on other platforms.

> [!NOTE]
> Shell uses handlers by default on Android in .NET MAUI 11 and later. On iOS and Mac Catalyst, the compatibility `ShellRenderer` remains the default, but you can opt in to the Shell handler implementation.

## Android handler customization

Starting in .NET MAUI 11, Shell on Android uses a handler-based architecture by default. The Android implementation is built around `ShellHandler`, `ShellItemHandler`, and `ShellSectionHandler`. This is the recommended approach for customizing Shell on Android.

### Create a custom Shell handler

The process for creating a custom Shell handler on Android is:

1. Create a subclass of `ShellHandler`, `ShellItemHandler`, or `ShellSectionHandler`.
1. Override the required methods to perform the customization.
1. Register the handler in `MauiProgram.cs`.

The following handler classes expose overridable members on Android:

| Handler | Overridable members |
| --- | --- |
| `ShellHandler` | `CreateShellItemRenderer`, `CreateShellFlyoutContentRenderer`, `CreateFragmentForPage`, `CreateShellSectionRenderer`, `CreateTrackerForToolbar`, `CreateToolbarAppearanceTracker`, `CreateTabLayoutAppearanceTracker`, `CreateBottomNavViewAppearanceTracker` |
| `ShellItemHandler` | `OnTabReselected`, `OnSectionChanged`, `CreateMoreBottomSheet` |
| `ShellSectionHandler` | `OnCreateNavigationAnimation` |

### Android example

The following code example shows a subclassed `ShellHandler`, for Android, that sets a background image on the toolbar of the Shell application and changes the title text color:

```csharp
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform;

namespace MyApp.Platforms.Android;

public class MyShellHandler : ShellHandler
{
    protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
    {
        return new MyShellToolbarAppearanceTracker(this);
    }
}
```

The `MyShellHandler` class overrides the `CreateToolbarAppearanceTracker` method, and returns an instance of the `MyShellToolbarAppearanceTracker` class. The `MyShellToolbarAppearanceTracker` class, which derives from the `ShellToolbarAppearanceTracker` class, is shown in the following example:

```csharp
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Platform;

namespace MyApp.Platforms.Android;

public class MyShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
{
    public MyShellToolbarAppearanceTracker(IShellContext context) : base(context)
    {
    }

    public override void SetAppearance(
        Toolbar toolbar,
        IShellToolbarTracker toolbarTracker,
        ShellAppearance appearance)
    {
        base.SetAppearance(toolbar, toolbarTracker, appearance);

        var context = toolbar.Context;
        if (context is not null)
        {
            var resId = context.Resources?.GetIdentifier(
                "dotnet_bot", "drawable", context.PackageName) ?? 0;
            if (resId != 0)
            {
                toolbar.SetBackgroundResource(resId);
            }
        }

        toolbar.BackgroundTintList = null;
        toolbar.SetTitleTextColor(global::Android.Graphics.Color.Red);
    }
}
```

The `MyShellToolbarAppearanceTracker` class overrides the `SetAppearance` method, and modifies the toolbar by setting a background image on it and changing the title text color.

### Register the Android handler

Register the custom handler in `MauiProgram.cs` using `ConfigureMauiHandlers`:

```csharp
using Microsoft.Maui.Controls;

var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    .ConfigureMauiHandlers(handlers =>
    {
#if ANDROID
        handlers.AddHandler<Shell, MyApp.Platforms.Android.MyShellHandler>();
#endif
    });
```

## iOS and Mac Catalyst handler customization

In .NET MAUI 11, enable the Shell handler implementation on iOS and Mac Catalyst by setting the `UseiOSShellHandler` MSBuild property to `true` in your app's project file:

```xml
<PropertyGroup>
    <UseiOSShellHandler>true</UseiOSShellHandler>
</PropertyGroup>
```

This property sets the `Microsoft.Maui.RuntimeFeature.IsiOSShellHandlerEnabled` <xref:System.AppContext> switch, which defaults to `false`. When enabled, the built-in registration adds the complete Shell handler hierarchy:

| Control | Handler | Responsibility |
| --- | --- | --- |
| `Shell` | `ShellHandler` | Root view controller, flyout presentation, and item transitions. |
| `ShellItem` | `ShellItemHandler` | Tab-bar controller and section selection. |
| `ShellSection` | `ShellSectionHandler` | Navigation controller and navigation stack. |
| `ShellContent` | `ShellContentHandler` | Forward content changes to the owning section handler. |

All four handler types are in the `Microsoft.Maui.Controls.Handlers` namespace. You don't need to register them manually when you use the project property.

To return to the compatibility renderer with the default registrations, remove the property or set it to `false`. Existing custom `ShellRenderer`, `ShellItemRenderer`, and `ShellSectionRenderer` implementations remain supported. If you manually register custom handlers, also remove or replace those registrations when returning to the renderer.

### Create a custom Apple Shell handler

Subclass `ShellHandler` and override its protected virtual factory methods to customize appearance trackers or other Shell components:

| Factory method | Customization |
| --- | --- |
| `CreateNavBarAppearanceTracker` | Navigation-bar appearance. |
| `CreateTabBarAppearanceTracker` | Tab-bar appearance. |
| `CreatePageRendererTracker` | Page navigation-bar and toolbar tracking. |
| `CreateShellFlyoutContentRenderer` | Flyout content. |
| `CreateShellItemRenderer` | Item handler integration. |
| `CreateShellSectionRenderer` | Section handler integration. |
| `CreateShellItemTransition` | Transitions between Shell items. |
| `CreateShellSearchResultsRenderer` | Search results presentation. |

Some factory names and return types retain renderer terminology for compatibility. The default item and section factories resolve `ShellItemHandler` and `ShellSectionHandler` from the handler registry and adapt them to the compatibility interfaces. Register a subclass for `ShellItem` or `ShellSection` to customize a child handler.

Unlike the compatibility renderers, `ShellHandler`, `ShellItemHandler`, and `ShellSectionHandler` own native view controllers instead of inheriting from them. When migrating a renderer customization, move tracker factory overrides to `ShellHandler`, and adapt code that depends on native view-controller overrides.

For example, the following classes add a border to the tab bar. Compile this code only for iOS and Mac Catalyst:

```csharp
#if IOS || MACCATALYST
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace MyApp.Platforms.Apple;

public class MyShellHandler : ShellHandler
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
    {
        return new MyShellTabBarAppearanceTracker();
    }
}

public class MyShellTabBarAppearanceTracker : ShellTabBarAppearanceTracker
{
    public override void SetAppearance(
        UITabBarController controller,
        ShellAppearance appearance)
    {
        base.SetAppearance(controller, appearance);

        controller.TabBar.Layer.BorderColor = UIColor.Red.CGColor;
        controller.TabBar.Layer.BorderWidth = 1;
    }
}
#endif
```

### Register the Apple handler

After enabling `UseiOSShellHandler`, register your custom handler after `UseMauiApp` in `MauiProgram.cs`:

```csharp
using Microsoft.Maui.Controls;

var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    .ConfigureMauiHandlers(handlers =>
    {
#if IOS || MACCATALYST
        handlers.AddHandler<Shell, MyApp.Platforms.Apple.MyShellHandler>();
#endif
    });
```

This replaces only the root Shell handler registration. The project property supplies the other three built-in registrations.

::: moniker-end
