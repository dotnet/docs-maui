---
title: "Blazor WebView debugging with CDP (experimental)"
description: "Learn how to debug Blazor Hybrid content in .NET MAUI apps using the DevFlow CDP bridge and Chrome DevTools Protocol."
ms.date: 04/03/2026
---

# Blazor WebView debugging with CDP (experimental)

DevFlow includes a Blazor WebView CDP (Chrome DevTools Protocol) bridge that enables browser-style debugging of Blazor Hybrid content in MAUI apps. The bridge is powered by Chobitsu and works across platforms.

> [!IMPORTANT]
> DevFlow is an **experimental** package from the [dotnet/maui-labs](https://github.com/dotnet/maui-labs) repository. It is **not** covered by the [.NET MAUI Support Policy](https://dotnet.microsoft.com/platform/support/policy/maui). APIs may change between releases. These packages are provided as-is with no guarantees of support, servicing, or updates.

## How it works

The `Microsoft.Maui.DevFlow.Blazor` package injects a CDP bridge into `BlazorWebView`. This allows Chrome DevTools-style inspection of the web content without requiring browser-specific debugging setup. The bridge communicates using the Chrome DevTools Protocol, giving you access to DOM inspection, JavaScript debugging, network monitoring, and console output for Blazor Hybrid content.

## Setup

Add the `Microsoft.Maui.DevFlow.Blazor` NuGet package to your project:

```xml
<PackageReference Include="Microsoft.Maui.DevFlow.Blazor" />
```

Register the Blazor CDP bridge in `MauiProgram.cs` alongside the DevFlow agent:

```csharp
var builder = MauiApp.CreateBuilder();
builder
    .UseMauiApp<App>()
    // Register the DevFlow agent
    // Register the Blazor CDP bridge
    ;
```

## Use the CDP bridge

Once the app is running with the Blazor CDP bridge registered, use the `cdp` command to connect:

```console
maui devflow cdp
```

This connects to the `BlazorWebView` instance via CDP, enabling Chrome DevTools-style debugging of the web content rendered in your MAUI app.

## Platform-specific setup

For platform-specific WebView debugging configuration, see:

- [Android setup](setup-android.md)
- [Apple platforms setup](setup-apple.md)
- [Windows setup](setup-windows.md)

## See also

- [DevFlow overview](index.md)
