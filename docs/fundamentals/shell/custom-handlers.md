---
title: ".NET MAUI Shell custom handlers"
description: "Learn how to customize the appearance and behavior of a .NET MAUI Shell app using platform-specific handlers."
ms.date: 07/28/2026
---

# .NET MAUI Shell custom handlers

::: moniker range=">=net-maui-11.0"

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

.NET MAUI Shell applications are highly customizable through the properties and methods that the various Shell classes expose. However, it's also possible to create custom handlers when more extensive platform-specific customizations are required. Custom handlers can be registered conditionally for a single platform, while allowing the default behavior on other platforms.

> [!NOTE]
> Shell custom handlers are currently supported on Android in .NET MAUI 11 and later. iOS and Mac Catalyst continue to use the legacy `ShellRenderer`.

## Android handler customization

Starting in .NET MAUI 11, Shell on Android uses a handler-based architecture by default. The Android implementation is built around `ShellHandler`, `ShellItemHandler`, and `ShellSectionHandler`. This is the recommended approach for customizing Shell on Android.

### Create a custom Shell handler

The process for creating a custom Shell handler on Android is:

1. Create a subclass of `ShellHandler`, `ShellItemHandler`, or `ShellSectionHandler`.
2. Override the required methods to perform the customization.
3. Register the handler in `MauiProgram.cs`.

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

::: moniker-end
