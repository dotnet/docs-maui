---
title: "Register and consume the custom control"
description: ""
ms.date: 08/02/2022
---

## Register the handler

A custom control and its handler must be registered with an app, before it can be consumed in the app. This should occur in the `CreateMauiApp` in the `MauiProgram` class in your app project, which is the cross-platform entry point for the app:

```csharp
using VideoDemos.Controls;
using VideoDemos.Handlers;

namespace VideoDemos;

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
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(Video), typeof(VideoHandler));
            });

        return builder.Build();
    }
}
```

The handler is registered with the `ConfigureMauiHandlers` and `AddHandler` method. The first argument to the `AddHandler` is the custom cross-platform control type, with the second argument being it's handler type.

## Consume the cross-platform control

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             x:Class="VideoDemos.Views.PlayWebVideoPage"
             Unloaded="OnContentPageUnloaded"
             Title="Play web video">
    <controls:Video x:Name="video"
                    Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4" />
</ContentPage>
```

In this example, the `VideoSourceConverter` class converts the string that represents the URI to a `UriVideoSource`. The video then begins loading and starts playing once a sufficient quantity of data has been downloaded and buffered. On each platform, the transport controls fade out if they're not used but can be restored by tapping on the video.

You can prevent the video from automatically starting by setting the `AutoPlay` property to `false`:

```xaml
<controls:Video x:Name="video"
                Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4"
                AutoPlay="False" />
```

Similarly, you can suppress the transport controls by setting the `AreTransportControlsEnabled` property to `false`:

```xaml
<controls:Video x:Name="video"
                Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4"
                AreTransportControlsEnabled="False" />
```

If you set `AutoPlay` and `AreTransportControlsEnabled` to `false`, the video won't begin playing and there'll be no way to start it playing. In this scenario you'd need to call the `Play` method from the code-behind file, or create your own transport controls.

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, and cleanup resources used by the native views, ensure that the `DisconnectHandler` method is called when the page is navigated away from:

```csharp
void OnContentPageUnloaded(object sender, EventArgs e)
{
    video.Handler?.DisconnectHandler();
}
```
