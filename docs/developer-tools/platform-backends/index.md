---
title: ".NET MAUI experimental platform backends"
description: "Learn about the experimental .NET MAUI platform backends for macOS (AppKit), Linux (GTK4), and Windows (WPF) available from the dotnet/maui-labs repository."
ms.date: 05/07/2026
---

# .NET MAUI experimental platform backends

The [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository hosts experimental .NET MAUI backends for three additional platforms: **macOS** (AppKit), **Linux** (GTK4), and **Windows** (WPF). These backends let you run your existing .NET MAUI applications natively on platforms beyond the standard iOS, Android, Mac Catalyst, and WinUI targets.

> [!IMPORTANT]
> These platform backends are experimental and will change between releases. They are not officially supported by Microsoft.

## Available backends

| Backend | NuGet package | Template short name | Platform |
|---------|--------------|--------------------|-|
| macOS AppKit | `Microsoft.Maui.Platforms.MacOS` | `maui-macos` | macOS 14+ (AppKit) |
| Linux GTK4 | `Microsoft.Maui.Platforms.Linux.Gtk4` | `maui-linux-gtk4` | Linux (GTK4) |
| Windows WPF | `Microsoft.Maui.Platforms.Windows.WPF` | `maui-wpf` | Windows (WPF) |

## Quick start with templates

Each platform backend provides a `dotnet new` template to get started quickly. Install the template package for your target platform and then scaffold a new project.

### macOS AppKit

```bash
dotnet new install Microsoft.Maui.Platforms.MacOS.Templates --prerelease
dotnet new maui-macos -n MyApp.MacOS
```

### Linux GTK4

```bash
dotnet new install Microsoft.Maui.Platforms.Linux.Gtk4.Templates --prerelease
dotnet new maui-linux-gtk4 -n MyApp.Linux
```

### Windows WPF

```bash
dotnet new install Microsoft.Maui.Platforms.Windows.WPF.Templates --prerelease
dotnet new maui-wpf -n MyApp.WPF
```

## Source code

All platform backends are developed in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under the `platforms/` directory.

## Support and feedback

These backends are community-supported. To report issues or request features, file an issue in the [dotnet/maui-labs issue tracker](https://github.com/dotnet/maui-labs/issues).

## See also

- [macOS AppKit backend](macos.md)
- [Linux GTK4 backend](linux-gtk4.md)
- [Windows WPF backend](windows-wpf.md)
