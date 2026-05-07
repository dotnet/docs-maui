---
title: ".NET MAUI Windows WPF backend"
description: "Learn how to build WPF-based Windows apps using .NET MAUI with the experimental WPF backend from the dotnet/maui-labs repository."
ms.date: 05/07/2026
---

# .NET MAUI Windows WPF backend

The Windows WPF backend is an alternative .NET MAUI backend for Windows that targets WPF (Windows Presentation Foundation) instead of the standard WinUI 3 backend. It uses the platform-agnostic MAUI NuGet packages and provides custom handler implementations that bridge MAUI's layout and rendering system to WPF controls.

> [!IMPORTANT]
> This backend is experimental and will change between releases. It is not officially supported by Microsoft.

## Prerequisites

- .NET 10 SDK
- Windows 10 or later

## Packages

| Package | Description |
|---------|-------------|
| `Microsoft.Maui.Platforms.Windows.WPF` | Core handlers, hosting, and platform services |
| `Microsoft.Maui.Platforms.Windows.WPF.Essentials` | MAUI Essentials implementations |

## Quick start

### Option 1: Use the template (recommended)

```bash
# Install the template
dotnet new install Microsoft.Maui.Platforms.Windows.WPF.Templates --prerelease

# Create a new WPF MAUI app
dotnet new maui-wpf -n MyApp.WPF
cd MyApp.WPF
dotnet run
```

### Option 2: Add to an existing project manually

#### 1. Create the project

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0-windows</TargetFramework>
    <OutputType>WinExe</OutputType>
    <UseMaui>true</UseMaui>
    <UseWPF>true</UseWPF>
    <EnableDefaultXamlItems>false</EnableDefaultXamlItems>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Maui.Controls" Version="10.0.31" />
    <PackageReference Include="Microsoft.Maui.Platforms.Windows.WPF" Version="*-*" />
    <PackageReference Include="Microsoft.Maui.Platforms.Windows.WPF.Essentials" Version="*-*" />
  </ItemGroup>
</Project>
```

#### 2. `App.xaml`

```xml
<Microsoft.Maui.Platforms.Windows.WPF:MauiWPFApplication
    x:Class="MyApp.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:Microsoft.Maui.Platforms.Windows.WPF="clr-namespace:Microsoft.Maui.Platform.WPF;assembly=Microsoft.Maui.Platforms.Windows.WPF">
</Microsoft.Maui.Platforms.Windows.WPF:MauiWPFApplication>
```

#### 3. `App.xaml.cs`

```csharp
using Microsoft.Maui.Platform.WPF;

namespace MyApp;

public partial class App : MauiWPFApplication
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
```

#### 4. `MauiProgram.cs`

```csharp
using Microsoft.Maui.Platform.WPF.Hosting;
using Microsoft.Maui.Essentials.WPF;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiAppWPF<App>()
            .UseWPFEssentials()
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

The source code for this backend is available in the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository under `platforms/Windows.WPF/`.

## See also

- [Experimental platform backends overview](index.md)
- [macOS AppKit backend](macos.md)
- [Linux GTK4 backend](linux-gtk4.md)
