---
title: "Reuse custom renderers in .NET MAUI"
description: "Learn how to adapt Xamarin.Forms custom renderers to work in a .NET MAUI app."
ms.date: 04/13/2023
---

# Reuse custom renderers in .NET MAUI

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/custom-renderers/)

While there are many benefits to using .NET Multi-platform App UI (.NET MAUI) handlers to customize and create controls, it's still possible to use Xamarin.Forms custom renderers in .NET MAUI apps. For more information about custom renderers, see [Xamarin.Forms custom renderers](/xamarin/xamarin-forms/app-fundamentals/custom-renderer/).

## Shimmed renderers

.NET MAUI provides shimmed renderers that enable easy re-use of Xamarin.Forms custom renderers, provided that the renderer derives from `FrameRenderer`, `ListViewRenderer`, `ShellRenderer` on iOS and Android, `TableViewRenderer`, and `VisualElementRenderer`.

The process for migrating a Xamarin.Forms custom renderer that derives from `FrameRenderer`, `ListViewRenderer`, `ShellRenderer`, `TableViewRenderer`, and `VisualElementRenderer` to a .NET MAUI shimmed renderer is to:

1. Add the custom renderer code into the appropriate location in your .NET MAUI project(s). For more information, see [Add the code](#add-the-code).
1. Modify the `using` directives and remove `ExportRenderer` attributes. For more information, see [Modify using directives and other code](#modify-using-directives-and-other-code).
1. Register the renderers. For more information, see [Register renderers](#register-renderers).
1. Consume the renderers. For more information, see [Consume the custom renderers](#consume-the-custom-renderers).

To demonstrate using custom renderers in .NET MAUI, consider a Xamarin.Forms control named `PressableView`. This control exposes `Pressed` and `Released` events based on platform-specific gestures. The custom renderer implementation is composed of 3 files:

- `PressableView.cs` - the cross-platform class that extends `ContentView`.
- `PressableViewRenderer.cs` - the Android implementation, which derives from `VisualElementRenderer`.
- `PressableViewRenderer.cs` - the iOS implementation, which derives from `VisualElementRenderer`.

> [!NOTE]
> An alternative to using a Xamarin.Forms custom renderer in .NET MAUI is to migrate the custom renderer to a .NET MAUI handler. For more information, see [Migrate a Xamarin.Forms custom renderer to a .NET MAUI handler](renderer-to-handler.md).

### Add the code

If you're using a .NET MAUI multi-targeted project, the cross-platform file can be moved to anywhere outside the *Platforms* folder, and the platform-specific implementation files should be moved to the corresponding *Platform* folder:

:::image type="content" source="media/move-renderer-files.png" alt-text="Move your renderer files.":::

If your solution has separate projects per-platform, then you should move the platform-specific implementation files into the corresponding projects.

### Modify using directives and other code

Any reference to the `Xamarin.Forms.*` namespaces need to be removed, and then you can resolve the related types to `Microsoft.Maui.*`. This needs to occur in all files you've added to the .NET MAUI project(s).

You should also remove any `ExportRenderer` attributes as they won't be needed in .NET MAUI. For example, the following should be removed:

```csharp
[assembly: ExportRenderer(typeof(PressableView), typeof(PressableViewRenderer))]
```

### Register renderers

The cross-platform control and its renderers must be registered with an app before it can be consumed. This should occur in the `CreateMauiApp` method in the `MauiProgram` class in your app project, which is the cross-platform entry point for the app:

```csharp
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

The renderers are registered with the <xref:Microsoft.Maui.Hosting.HandlerMauiAppBuilderExtensions.ConfigureMauiHandlers%2A> and <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> method. This first argument to the <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> method is the cross-platform control type, with the second argument being its renderer type.

> [!IMPORTANT]
> Only renderers that derive from `FrameRenderer`, `ListViewRenderer`, `NavigationRenderer` on iOS, `ShellRenderer` on iOS and Android, `TabbedRenderer` on iOS, `TableViewRenderer`, and `VisualElementRenderer` can be registered with the <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> method.

### Consume the custom renderers

The custom renderer can be consumed in a .NET MAUI app as a custom control:

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
