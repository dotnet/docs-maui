---
title: ".NET MAUI Linux GTK4 backend"
description: "Learn how to build native Linux apps using .NET MAUI with the experimental GTK4 backend from the dotnet/maui-labs repository."
ms.date: 05/07/2026
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
using Microsoft.Maui.Platforms.Linux.Gtk4.Hosting;
using Microsoft.Maui.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiAppLinuxGtk4<App>();

        return builder.Build();
    }
}
```

## Source code

The source code for this backend is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under `platforms/Linux.Gtk4/`.

## See also

- [Experimental platform backends overview](index.md)
- [macOS AppKit backend](macos.md)
- [Windows WPF backend](windows-wpf.md)
