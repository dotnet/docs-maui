---
title: ".NET MAUI macOS AppKit backend"
description: "Learn how to build native macOS apps using .NET MAUI with the experimental AppKit backend from the dotnet/maui-labs repository."
ms.date: 05/07/2026
---

# .NET MAUI macOS AppKit backend

The macOS AppKit backend lets .NET MAUI applications run as true native macOS apps using AppKit controls (`NSWindow`, `NSButton`, `NSScrollView`, etc.) and standard macOS UI conventions such as the menu bar, toolbar, sidebar flyout, and native dialogs. This is distinct from the Mac Catalyst target that ships with .NET MAUI.

> [!IMPORTANT]
> This backend is experimental and will change between releases. It is not officially supported by Microsoft.

## Prerequisites

- .NET 10 SDK
- macOS 14 (Sonoma) or later
- Xcode command line tools

## Packages

| Package | Description |
|---------|-------------|
| `Microsoft.Maui.Platforms.MacOS` | Core handlers, hosting, and platform services |
| `Microsoft.Maui.Platforms.MacOS.Essentials` | MAUI Essentials implementations (clipboard, preferences, sensors, and more) |
| `Microsoft.Maui.Platforms.MacOS.BlazorWebView` | Blazor Hybrid (`BlazorWebView`) support |

## Quick start

### Option 1: Use the template (recommended)

```bash
# Install the template
dotnet new install Microsoft.Maui.Platforms.MacOS.Templates --prerelease

# Create a new macOS MAUI app
dotnet new maui-macos -n MyApp.MacOS
cd MyApp.MacOS
dotnet run
```

### Option 2: Add to an existing project manually

#### 1. Project file

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-macos</TargetFramework>
    <OutputType>Exe</OutputType>
    <UseMaui>true</UseMaui>
    <SingleProject>true</SingleProject>
    <SupportedOSPlatformVersion>14.0</SupportedOSPlatformVersion>

    <ApplicationTitle>My macOS App</ApplicationTitle>
    <ApplicationId>com.example.myapp</ApplicationId>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
    <PackageReference Include="Microsoft.Maui.Platforms.MacOS" Version="*-*" />
    <PackageReference Include="Microsoft.Maui.Platforms.MacOS.Essentials" Version="*-*" />
  </ItemGroup>

  <ItemGroup>
    <MauiIcon Include="Resources\AppIcon\appicon.png" />
  </ItemGroup>
</Project>
```

#### 2. `Main.cs`

```csharp
using AppKit;

public class MainClass
{
    static void Main(string[] args)
    {
        NSApplication.Init();
        NSApplication.SharedApplication.Delegate = new MauiMacOSApp();
        NSApplication.Main(args);
    }
}
```

#### 3. `MauiMacOSApp.cs`

```csharp
using Foundation;
using Microsoft.Maui.Platforms.MacOS.Platform;

[Register("MauiMacOSApp")]
public class MauiMacOSApp : MacOSMauiApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

#### 4. `MauiProgram.cs`

```csharp
using Microsoft.Maui.Platforms.MacOS.Hosting;
using Microsoft.Maui.Platforms.MacOS.Essentials;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiAppMacOS<App>()
            .AddMacOSEssentials()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}
```

#### 5. `App.cs`

```csharp
public class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(new MainPage());
}
```

## Source code

The source code for this backend is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under `platforms/MacOS/`.

## See also

- [Experimental platform backends overview](index.md)
- [Linux GTK4 backend](linux-gtk4.md)
- [Windows WPF backend](windows-wpf.md)
