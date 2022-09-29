---
title: "Using Custom Renderers in .NET MAUI"
description: ""
ms.date: 10/01/2022
---

# Using Custom Renderers in .NET MAUI

While there are many benefits to using the new handler-mapper pattern, it's still possible to use the [custom renderer](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/custom-renderer/) pattern familiar to Xamarin.Forms developers.

To demonstrate using custom renderers in .NET MAUI, let's consider this Xamarin.Forms control `PressableView`. The control simply exposes pressed and released events based on the platform-specific gestures. The custom renderer implementation is composed of 3 files:

- `PressableView.cs` - the cross-platform class that extends `ContentView`
- `PressableViewRenderer.cs` - the Android implementation
- `PressableViewRenderer.cs` - the iOS implementation

To use this in .NET MAUI you will:

1. Add the files into the appropriate location in your .NET MAUI project(s)
2. Modify the "usings" and files
3. Configure the renderers in `MauiProgram.cs`

[Sample on GitHub](https://github.com/davidortinau/CustomRendererSample)

## Add the Files

If you're using the .NET MAUI multi-targeted project, the cross-platform file can be moved to anywhere outside the Platforms folder, and the platform-specific implementation files should be moved to the corresponding Platform folder.

![MoveRendererFiles](https://user-images.githubusercontent.com/41873/166120451-2833eb95-2a71-4caa-bd29-3f7e8b53f470.png)

On the other hand if you're solution has separate projects per-platform, then you would move the platform-specific implementation files into the corresponding projects.

## Modify Usings and Files

Any reference to the `Xamarin.Forms.*` namespaces need to be removed, and then you can resolve the related types to `Microsoft.Maui.*`. This needs to be done in all files you've added to the .NET MAUI project(s).

Remove any `ExportRenderer` directives as they won't be needed in .NET MAUI, such as:

```csharp
[assembly: ExportRenderer(typeof(PressableView), typeof(PressableViewRenderer))]
```

## Configure Renderers

Open `MauiProgram.cs` in your .NET MAUI project, add `UseMauiCompatibility()` with `using Microsoft.Maui.Controls.Compatibility.Hosting`, and then configure each renderer using conditional compilation for each platform.

```csharp
public static MauiApp CreateMauiApp()
{
	var builder = MauiApp.CreateBuilder();
	builder
		.UseMauiApp<App>()
		.UseMauiCompatibility()
                .ConfigureMauiHandlers((handlers) =>{
#if ANDROID
			handlers.AddCompatibilityRenderer(typeof(PressableView),typeof(XamarinCustomRenderer.Droid.Renderers.PressableViewRenderer));
#endif

#if IOS
                        handlers.AddCompatibilityRenderer(typeof(PressableView), typeof(XamarinCustomRenderer.iOS.Renderers.PressableViewRenderer));
#endif
        });

	return builder.Build();
}
```

## Conclusion

That's it! You can now use your custom renderer in .NET MAUI just like any other custom control.

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:c="clr-namespace:XamarinCustomRenderer.Controls"
             x:Class="MauiCustomRenderer.MainPage">

    <Grid BackgroundColor="#f1f1f1">
        <c:PressableView Pressed="Handle_Pressed"
                         Released="Handle_Released"
                         HorizontalOptions="Center"
                         VerticalOptions="Center">
            <Grid
                BackgroundColor="#202020"
                HorizontalOptions="Center"
                VerticalOptions="Center">
                <Label Text="Press Me"
                        FontSize="16"
                        TextColor="White"
                        Margin="24,20"
                        HorizontalTextAlignment="Center" />
            </Grid>
        </c:PressableView>
    </Grid>

</ContentPage>
```
