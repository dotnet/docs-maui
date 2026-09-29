---
title: ".NET MAUI Linux GTK4 backend"
description: "Learn how to build native Linux apps using .NET MAUI with the experimental GTK4 backend from the dotnet/maui-labs repository."
ms.date: 09/29/2026
---

# .NET MAUI Linux GTK4 backend

The Linux GTK4 backend lets .NET MAUI applications run natively on Linux desktops using GTK4 rendering via [GirCore](https://github.com/gircore/gir.core) bindings. Every MAUI control maps to a real GTK4 widget, styled via GTK CSS.

> [!IMPORTANT]
> This backend is experimental and will change between releases. It is not officially supported by Microsoft.

## Prerequisites

| Requirement | Version |
|-------------|---------|
| .NET SDK | 10.0+ |
| GTK 4 libraries | 4.x (system package) |
| WebKitGTK *(Blazor only)* | 6.x (system package) |

### Install GTK4 on Debian / Ubuntu

```bash
sudo apt install libgtk-4-dev libwebkitgtk-6.0-dev \
  gobject-introspection libgirepository1.0-dev \
  gir1.2-gtk-4.0 gir1.2-webkit-6.0 pkg-config
```

### Install GTK4 on Fedora

```bash
sudo dnf install gtk4-devel webkitgtk6.0-devel \
  gobject-introspection-devel pkg-config
```

## Packages

| Package | Description |
|---------|-------------|
| `Microsoft.Maui.Platforms.Linux.Gtk4` | Core handlers, hosting, and platform services |
| `Microsoft.Maui.Platforms.Linux.Gtk4.Essentials` | MAUI Essentials implementations |
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

## Source code

The source code for this backend is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under `platforms/Linux.Gtk4/`.

## See also

- [Experimental platform backends overview](index.md)
- [macOS AppKit backend](macos.md)
- [Windows WPF backend](windows-wpf.md)
