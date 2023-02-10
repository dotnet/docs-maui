---
title: "Use custom renderers in .NET MAUI"
description: "Learn how to adapt Xamarin.Forms custom renderers to work in a .NET MAUI app."
ms.date: 1/31/2023
---

# Use custom renderers in .NET MAUI

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/custom-renderers/)

While there are many benefits to using .NET Multi-platform App UI (.NET MAUI) handlers to customize and create controls, it's still possible to use Xamarin.Forms custom renderers in .NET MAUI apps. For more information about custom renderers, see [Xamarin.Forms custom renderers](/xamarin/xamarin-forms/app-fundamentals/custom-renderer/).

To demonstrate using custom renderers in .NET MAUI, consider a Xamarin.Forms control named `PressableView`. This control exposes `Pressed` and `Released` events based on platform-specific gestures. The custom renderer implementation is composed of 3 files:

- `PressableView.cs` - the cross-platform class that extends `ContentView`.
- `PressableViewRenderer.cs` - the Android implementation.
- `PressableViewRenderer.cs` - the iOS implementation.

To use the `PressableView` in a .NET MAUI app, you will have to:

1. Add the code into the appropriate location in your .NET MAUI project(s).
1. Modify the `using` statements and other code.
1. Configure the renderers in your `MauiProgram` class.

## Add the code

If you're using a .NET MAUI multi-targeted project, the cross-platform file can be moved to anywhere outside the *Platforms* folder, and the platform-specific implementation files should be moved to the corresponding *Platform* folder:

:::image type="content" source="media/move-renderer-files.png" alt-text="Move your renderer files.":::

However, if your solution has separate projects per-platform, then you should move the platform-specific implementation files into the corresponding projects.

## Modify using statements and other code

Any reference to the `Xamarin.Forms.*` namespaces need to be removed, and then you can resolve the related types to `Microsoft.Maui.*`. This needs to occur in all files you've added to the .NET MAUI project(s).

You should also remove any `ExportRenderer` attributes as they won't be needed in .NET MAUI. For example, the following should be removed:

```csharp
[assembly: ExportRenderer(typeof(PressableView), typeof(PressableViewRenderer))]
```

## Register renderers

In your .NET MAUI app project, open *MauiProgram.cs* and add a `using` statement for the `Microsoft.Maui.Controls.Compatibility.Hosting` namespace. Then, call `UseMauiCompatibility` on the `MauiAppBuilder` object in the `CreateMauiApp` method, and configure each renderer using conditional compilation per platform:

```csharp
using Microsoft.Maui.Controls.Compatibility.Hosting;
...

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers((handlers) =>
            {
#if ANDROID
                handlers.AddHandler(typeof(PressableView), typeof(XamarinCustomRenderer.Droid.Renderers.PressableViewRenderer));
#elif IOS
                handlers.AddHandler(typeof(PressableView), typeof(XamarinCustomRenderer.iOS.Renderers.PressableViewRenderer));
#endif
            });

        return builder.Build();
    }
}
```

## Consume the custom renderer

The custom renderer can then be consumed in a .NET MAUI app as a custom control:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:XamarinCustomRenderer.Controls"
             x:Class="MauiCustomRenderer.MainPage">
    <Grid BackgroundColor="#f1f1f1">
        <controls:PressableView Pressed="Handle_Pressed"
                                Released="Handle_Released"
                                HorizontalOptions="Center"
                                VerticalOptions="Center">
            <Grid BackgroundColor="#202020"
                  HorizontalOptions="Center"
                  VerticalOptions="Center">
                <Label Text="Press Me"
                       FontSize="16"
                       TextColor="White"
                       Margin="24,20"
                       HorizontalTextAlignment="Center" />
            </Grid>
        </controls:PressableView>
    </Grid>
</ContentPage>
```
