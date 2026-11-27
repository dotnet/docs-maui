---
title: ".NET MAUI Linux GTK4 backend"
description: "Learn how to build native Linux apps using .NET MAUI with the experimental GTK4 backend from the dotnet/maui-labs repository."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# .NET MAUI Linux GTK4 backend

The Linux GTK4 backend lets .NET MAUI applications run natively on Linux desktops using GTK4 rendering via [GirCore](https://github.com/gircore/gir.core) bindings. Every MAUI control maps to a real GTK4 widget, styled via GTK CSS.

> [!IMPORTANT]
> This backend is experimental and will change between releases. It is not officially supported by Microsoft.

## Prerequisites

| Requirement | Version |
|-------------|---------|
| .NET SDK | 10.0+ |
| GTK 4 libraries | 4.12+ (system package) |
| WebKitGTK *(`WebView` and Blazor Hybrid only)* | 6.x (system package) |

The backend checks the GTK version at startup and throws `PlatformNotSupportedException` if GTK 4.12 or later isn't installed.

### Install GTK4 on Debian / Ubuntu

```bash
sudo apt install libgtk-4-dev libwebkitgtk-6.0-dev \
  gobject-introspection libgirepository1.0-dev \
  gir1.2-gtk-4.0 gir1.2-webkit-6.0 pkg-config
```

### Install GTK4 on Fedora

Fedora package names can differ between releases. If a package isn't found, search your release's repositories for the GTK 4, WebKitGTK 6.0, and GObject Introspection development packages.

```bash
sudo dnf install gtk4-devel webkitgtk6.0-devel \
  gobject-introspection-devel pkg-config
```

## Packages

| Package | Description |
|---------|-------------|
| `Microsoft.Maui.Platforms.Linux.Gtk4` | Core handlers, hosting, and platform services |
| `Microsoft.Maui.Platforms.Linux.Gtk4.Essentials` | MAUI Essentials implementations |
| `Microsoft.Maui.Platforms.Linux.Gtk4.BlazorWebView` | Blazor Hybrid (`BlazorWebView`) support through WebKitGTK |
| `Microsoft.Maui.DevFlow.Agent.Gtk` | DevFlow agent for GTK/Linux apps |

Use the current `Microsoft.Maui.Platforms.Linux.Gtk4` and `Microsoft.Maui.DevFlow.Agent.Gtk` packages. The superseded `Platform.Maui.Linux.Gtk4*` packages are no longer recommended. Don't mix the two backends in the same project.

## Quick start

### Option 1: Use the template (recommended)

```bash
# Install the template
dotnet new install Microsoft.Maui.Platforms.Linux.Gtk4.Templates --prerelease

# Create a new Linux MAUI app
dotnet new maui-linux-gtk4 -n MyApp.Linux
cd MyApp.Linux
dotnet run
```

### Option 2: Add to an existing project manually

Add the NuGet packages:

```bash
dotnet add package Microsoft.Maui.Platforms.Linux.Gtk4 --prerelease
dotnet add package Microsoft.Maui.Platforms.Linux.Gtk4.Essentials --prerelease   # optional
dotnet add package Microsoft.Maui.DevFlow.Agent.Gtk --prerelease                # optional, for DevFlow
```

Then set up your entry point:

**Program.cs**

```csharp
using Microsoft.Maui.Platforms.Linux.Gtk4.Platform;
using Microsoft.Maui.Hosting;

public class Program : GtkMauiApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);
    }
}
```

**MauiProgram.cs**

```csharp
using Microsoft.Maui.DevFlow.Agent;
using Microsoft.Maui.Platforms.Linux.Gtk4.Hosting;
using Microsoft.Maui.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiAppLinuxGtk4<App>();

#if DEBUG
        builder.AddMauiDevFlowAgent();
#endif

        return builder.Build();
    }
}
```

If you added the `Microsoft.Maui.Platforms.Linux.Gtk4.Essentials` package, register its services before calling `builder.Build()`. Adding the package alone doesn't replace the default MAUI Essentials implementations.

```csharp
using Microsoft.Maui.Platforms.Linux.Gtk4.Essentials.Hosting;

// In CreateMauiApp, before builder.Build():
builder.AddLinuxGtk4Essentials();
```

The project template includes the Essentials package and this registration by default.

## Supported features

Each .NET MAUI control maps to a native GTK4 widget. The backend includes handlers for:

| Category | Controls |
|----------|----------|
| Basic controls | `Label`, `Button`, `Entry`, `Editor`, `CheckBox`, `Switch`, `Slider`, `Stepper`, `ProgressBar`, `ActivityIndicator`, `Image`, `ImageButton`, `BoxView`, and `RadioButton` |
| Input and selection | `Picker`, `DatePicker`, `TimePicker`, and `SearchBar` |
| Collections | `CollectionView`, `ListView`, `TableView`, `CarouselView`, `SwipeView`, `RefreshView`, and `IndicatorView` |
| Layouts | `StackLayout`, `Grid`, `FlexLayout`, `AbsoluteLayout`, `ScrollView`, `ContentView`, `Border`, and `Frame` |
| Pages and navigation | `ContentPage`, `NavigationPage`, `TabbedPage`, `FlyoutPage`, and `Shell` |
| Shapes | `Rectangle`, `Ellipse`, `Line`, `Path`, `Polygon`, and `Polyline` |
| Other | `GraphicsView` and `WebView` (WebKitGTK) |

The backend also supports gestures, animations, transforms, visual states, control templates, formatted text, alerts and modal pages, menu bars, context menus, Blazor Hybrid, and automatic light and dark theme detection.

## Styling with GTK CSS

The backend styles native widgets with GTK CSS. Font sizes use logical pixels (`px`), which match .NET MAUI device-independent font sizes.

Each handler mapper updates its own CSS fragment for a widget. Font, color, spacing, background, and border updates don't overwrite one another. If you write a custom handler that derives from `GtkViewHandler`, follow these rules:

- Pass the complete CSS fragment to `UpdateCss` or `UpdateCssWithSelector` on every update. Pass `null` or an empty string to remove that fragment's styles.
- The optional `property` parameter defaults to the calling method name. If a shared helper updates independent properties, pass a distinct `property` value for each property.
- When fragments set the same CSS property with the same specificity, the most recently updated fragment wins.

The older `ApplyCss` and `ApplyCssWithSelector` methods remain available for existing custom handlers. They share one fragment per widget and selector, so use `UpdateCss` or `UpdateCssWithSelector` for new code.

## DevFlow agent startup

With the current GTK4 backend, registering the agent with `AddMauiDevFlowAgent()` is all that's needed. The agent starts automatically after the first GTK window is created; you don't need to call `StartDevFlowAgent()` manually.

For advanced scenarios that require an explicit startup hook, override `OnStarted()` in your `GtkMauiApplication`:

```csharp
protected override void OnStarted()
{
    base.OnStarted();
#if DEBUG
    this.StartDevFlowAgent();
#endif
}
```

If the agent doesn't start, verify that `builder.AddMauiDevFlowAgent()` is called during `MauiAppBuilder` setup and that the app uses `UseMauiAppLinuxGtk4<App>()` or the equivalent current GTK backend setup. If using the explicit hook, call `this.StartDevFlowAgent()` from `GtkMauiApplication.OnStarted()`, not from `CreateMauiApp()`.

## Limitations

SecureStorage isn't supported on Linux GTK4. All storage operations (`GetAsync`, `SetAsync`, `Remove`, and `RemoveAll`) throw `PlatformNotSupportedException`. If your app needs secure storage on Linux, implement your own `Microsoft.Maui.Storage.ISecureStorage` and register it in `MauiProgram.cs`.

Register your implementation before you call `AddLinuxGtk4Essentials()`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Platforms.Linux.Gtk4.Essentials.Hosting;
using Microsoft.Maui.Storage;

// MySecureStorage is your app's ISecureStorage implementation.
builder.Services.AddSingleton<ISecureStorage, MySecureStorage>();
builder.AddLinuxGtk4Essentials();
```

`AddLinuxGtk4Essentials()` keeps an existing `ISecureStorage` registration. After `builder.Build()`, both injected `ISecureStorage` instances and static calls such as `SecureStorage.Default.GetAsync("api_token")` use your implementation. Store secrets with a secure system service. Don't store secrets as plain text, and don't store an encryption key next to the encrypted data.

## Source code

The source code for this backend is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under `platforms/Linux.Gtk4/`.

## See also

- [Experimental platform backends overview](index.md)
- [macOS AppKit backend](macos.md)
- [Windows WPF backend](windows-wpf.md)
