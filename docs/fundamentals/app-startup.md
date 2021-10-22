---
title: ".NET MAUI app startup"
description: ".NET MAUI apps are bootstrapped using HostBuilder from the Microsoft.Extensions library, enabling apps to be initialized from a single location."
ms.date: 09/14/2021
---

# App startup

.NET Multi-platform App UI (.NET MAUI) apps are bootstrapped using the [.NET Generic Host](/dotnet/core/extensions/generic-host). This enables apps to be initialized from a single location, and provides the ability to configure fonts, services, and third-party libraries.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Each platform entry point calls a `CreateMauiApp` method on the static `MauiProgram` class that creates and returns a `MauiApp`, the entry point for your app.

The `MauiProgram` class must at a minimum provide an app to run:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace MyMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();

            return builder.Build();
        }
    }  
}
```

The `App` class derives from the `Application` class:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

public class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
```

In the example above, `MainPage` is a `ContentPage` that defines the UI for the initial page of the app. The `Application` creates a `Window` within which to run the application and display views. You can customize this behavior by overriding the `CreateWindow` method.

## Register fonts

Fonts can be added to your app and referenced by filename or alias. This is accomplished by invoking the `ConfigureFonts` method on the `MauiAppBuilder` object. Then, on the `IFontCollection` object, call the `AddFont` method to add the required font:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace MyMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}
```

In the example above, the first argument to the `AddFont` method is the font filename, while the second argument represents an optional alias by which the font can be referenced when consuming it.

Any custom fonts consumed by an app must be included in your .csproj file. This can be accomplished by referencing their filenames, or by using a wildcard:

```xml
<ItemGroup>
   <MauiFont Include="Resources\Fonts\*" />
</ItemGroup>
```

> [!NOTE]
> Fonts added to the project through the Solution Explorer in Visual Studio will automatically be included in the .csproj file.

The font can then be consumed by referencing its name, without the file extension:

```xaml
<!-- Use font name -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSans-Regular" />
```

Alternatively, it can be consumed by referencing its alias:

```xaml
<!-- Use font alias -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSansRegular" />
```

<!-- ## Configure Essentials

```csharp
builder
    .UseMauiApp<App>()
    .ConfigureEssentials(essentials =>
    {
        essentials
            .UseVersionTracking()
            .UseMapServiceToken("YOUR-KEY-HERE");
    });
``` -->

## Register handlers

To register your own handlers, call the `ConfigureMauiHandlers` method on the `MauiAppBuilder` object. Then, on the `IMauiHandlersCollection` object, call the `AddHandler` method to add the required handler:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace MyMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()        
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(MyEntry), typeof(MyEntryHandler));
                });     
            return builder.Build();    
        }
    }
}
```

In this example, the `MyEntryHandler` handler is registered against the `MyEntry` control. Therefore, any instances of the `MyEntry` control will be handled by the `MyEntryHandler`.

## Register renderers

To use controls backed by .NET MAUI handlers, with specific controls backed by Xamarin.Forms renderers, call the `ConfigureMauiHandlers` method on the `MauiAppBuilder` object. Then, on the `IMauiHandlersCollection` object, call the `AddCompatibilityRenderer` method to add the required renderer:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;

namespace MyMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                #if __ANDROID__
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddCompatibilityRenderer(typeof(Microsoft.Maui.Controls.BoxView),
                        typeof(Microsoft.Maui.Controls.Compatibility.Platform.Android.BoxRenderer));
                    handlers.AddCompatibilityRenderer(typeof(Microsoft.Maui.Controls.Frame),
                        typeof(Microsoft.Maui.Controls.Compatibility.Platform.Android.FastRenderers.FrameRenderer));
                });
                #elif __IOS__
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddCompatibilityRenderer(typeof(Microsoft.Maui.Controls.BoxView),
                        typeof(Microsoft.Maui.Controls.Compatibility.Platform.iOS.BoxRenderer));
                    handlers.AddCompatibilityRenderer(typeof(Microsoft.Maui.Controls.Frame),
                        typeof(Microsoft.Maui.Controls.Compatibility.Platform.iOS.FrameRenderer));
                });
                #endif   

            return builder.Build();         
        }
    }
}
```

In this example, all controls in the app will be backed by handlers, aside from the `BoxView` and `Frame` controls that will be backed by Xamarin.Forms renderers.
