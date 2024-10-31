---
title: "Create custom controls with .NET MAUI handlers"
description: "Learn how to create a .NET MAUI handler, to provide the platform implementations for a cross-platform video control."
ms.date: 10/31/2024
---

# Create a custom control using handlers

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-createhandler)

A standard requirement for apps is the ability to play videos. This article examines how to create a .NET Multi-platform App UI (.NET MAUI) cross-platform `Video` control that uses a handler to map the cross-platform control API to the native views on Android, iOS, and Mac Catalyst that play videos. This control can play video from three sources:

- A URL, which represents a remote video.
- A resource, which is a file embedded in the app.
- A file, from the device's video library.

Video controls require *transport controls*, which are buttons for playing and pausing the video, and a positioning bar that shows the progress through the video and allows the user to move quickly to a different location. The `Video` control can either use the transport controls and positioning bar provided by the platform, or you can supply custom transport controls and a positioning bar. The following screenshots show the control on iOS, with and without custom transport controls:

:::image type="content" source="media/create/play-video.png" alt-text="Screenshot of video playback on iOS."::: :::image type="content" source="media/create/custom-transport-controls.png" alt-text="Screenshot of video playback using custom transport controls on iOS.":::

A more sophisticated video control would have additional features, such as a volume control, a mechanism to interrupt video playback when a call is received, and a way of keeping the screen active during playback.

The architecture of the `Video` control is shown in the following diagram:

:::image type="content" source="media/create/video-handler.png" alt-text="Video handler architecture." border="false":::

The `Video` class provides the cross-platform API for the control. Mapping of the cross-platform API to the native view APIs is performed by the `VideoHandler` class on each platform, which maps the `Video` class to the `MauiVideoPlayer` class. On iOS and Mac Catalyst, the `MauiVideoPlayer` class uses the `AVPlayer` type to provide video playback. On Android, the `MauiVideoPlayer` class uses the `VideoView` type to provide video playback. On Windows, the `MauiVideoPlayer` class uses the `MediaPlayerElement` type to provide video playback.

> [!IMPORTANT]
> .NET MAUI decouples its handlers from its cross-platform controls through interfaces. This enables experimental frameworks such as Comet and Fabulous to provide their own cross-platform controls, that implement the interfaces, while still using .NET MAUI's handlers. Creating an interface for your cross-platform control is only necessary if you need to decouple your handler from its cross-platform control for a similar purpose, or for testing purposes.

The process for creating a cross-platform .NET MAUI custom control, whose platform implementations are provided by handlers, is as follows:

1. Create a class for the cross-platform control, which provides the control's public API. For more information, see [Create the cross-platform control](#create-the-cross-platform-control).
1. Create any required additional cross-platform types.
1. Create a `partial` handler class. For more information, see [Create the handler](#create-the-handler).
1. In the handler class, create a <xref:Microsoft.Maui.PropertyMapper> dictionary, which defines the Actions to take when cross-platform property changes occur. For more information, see [Create the property mapper](#create-the-property-mapper).
1. Optionally, in your handler class, create a <xref:Microsoft.Maui.CommandMapper> dictionary, which defines the Actions to take when the cross-platform control sends instructions to the native views that implement the cross-platform control. For more information, see [Create the command mapper](#create-the-command-mapper).
1. Create `partial` handler classes for each platform that create the native views that implement the cross-platform control. For more information, see [Create the platform controls](#create-the-platform-controls).
1. Register the handler using the <xref:Microsoft.Maui.Hosting.HandlerMauiAppBuilderExtensions.ConfigureMauiHandlers%2A> and <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> methods in your app's `MauiProgram` class. For more information, see [Register the handler](#register-the-handler).

Then, the cross-platform control can be consumed. For more information, see [Consume the cross-platform control](#consume-the-cross-platform-control).

## Create the cross-platform control

To create a cross-platform control, you should create a class that derives from <xref:Microsoft.Maui.Controls.View>:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        public static readonly BindableProperty AreTransportControlsEnabledProperty =
            BindableProperty.Create(nameof(AreTransportControlsEnabled), typeof(bool), typeof(Video), true);

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(Video), null);

        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(Video), true);

        public static readonly BindableProperty IsLoopingProperty =
            BindableProperty.Create(nameof(IsLooping), typeof(bool), typeof(Video), false);            

        public bool AreTransportControlsEnabled
        {
            get { return (bool)GetValue(AreTransportControlsEnabledProperty); }
            set { SetValue(AreTransportControlsEnabledProperty, value); }
        }

        [TypeConverter(typeof(VideoSourceConverter))]
        public VideoSource Source
        {
            get { return (VideoSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public bool AutoPlay
        {
            get { return (bool)GetValue(AutoPlayProperty); }
            set { SetValue(AutoPlayProperty, value); }
        }

        public bool IsLooping
        {
            get { return (bool)GetValue(IsLoopingProperty); }
            set { SetValue(IsLoopingProperty, value); }
        }        
        ...
    }
}
```

The control should provide a public API that will be accessed by its handler, and control consumers. Cross-platform controls should derive from <xref:Microsoft.Maui.Controls.View>, which represents a visual element that's used to place layouts and views on the screen.

## Create the handler

After creating your cross-platform control, you should create a `partial` class for your handler:

```csharp
#if IOS || MACCATALYST
using PlatformView = VideoDemos.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = VideoDemos.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = VideoDemos.Platforms.Windows.MauiVideoPlayer;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using VideoDemos.Controls;
using Microsoft.Maui.Handlers;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler
    {
    }
}
```

The handler class is a partial class whose implementation will be completed on each platform with an additional partial class.

The conditional `using` statements define the `PlatformView` type on each platform. On Android, iOS, Mac Catalyst, and Windows, the native views are provided by the custom `MauiVideoPlayer` class. The final conditional `using` statement defines `PlatformView` to be equal to `System.Object`. This is necessary so that the `PlatformView` type can be used within the handler for usage across all platforms. The alternative would be to have to define the `PlatformView` property once per platform, using conditional compilation.

## Create the property mapper

Each handler typically provides a *property mapper*, which defines what Actions to take when a property change occurs in the cross-platform control. The <xref:Microsoft.Maui.PropertyMapper> type is a `Dictionary` that maps the cross-platform control's properties to their associated Actions.

<xref:Microsoft.Maui.PropertyMapper> is defined in .NET MAUI's <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, and requires two generic arguments to be supplied:

- The class for the cross-platform control, which derives from <xref:Microsoft.Maui.Controls.View>.
- The class for the handler.

The following code example shows the `VideoHandler` class extended with the <xref:Microsoft.Maui.PropertyMapper> definition:

```csharp
public partial class VideoHandler
{
    public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(Video.Source)] = MapSource,
        [nameof(Video.IsLooping)] = MapIsLooping,
        [nameof(Video.Position)] = MapPosition
    };

    public VideoHandler() : base(PropertyMapper)
    {
    }
}
```

The <xref:Microsoft.Maui.PropertyMapper> is a `Dictionary` whose key is a `string` and whose value is a generic `Action`. The `string` represents the cross-platform control's property name, and the `Action` represents a `static` method that requires the handler and cross-platform control as arguments. For example, the signature of the `MapSource` method is `public static void MapSource(VideoHandler handler, Video video)`.

Each platform handler must provide implementations of the Actions, which manipulate the native view APIs. This ensures that when a property is set on a cross-platform control, the underlying native view will be updated as required. The advantage of this approach is that it allows for easy cross-platform control customization, because the property mapper can be modified by cross-platform control consumers without subclassing.

## Create the command mapper

Each handler can also provide a *command mapper*, which defines what Actions to take when the cross-platform control sends commands to native views. Command mappers are similar to property mappers, but allow for additional data to be passed. In this context, a command is an instruction, and optionally its data, that's sent to a native view. The <xref:Microsoft.Maui.CommandMapper> type is a `Dictionary` that maps cross-platform control members to their associated Actions.

<xref:Microsoft.Maui.CommandMapper> is defined in .NET MAUI's <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, and requires two generic arguments to be supplied:

- The class for the cross-platform control, which derives from <xref:Microsoft.Maui.Controls.View>.
- The class for the handler.

The following code example shows the `VideoHandler` class extended with the <xref:Microsoft.Maui.CommandMapper> definition:

```csharp
public partial class VideoHandler
{
    public static IPropertyMapper<Video, VideoHandler> PropertyMapper = new PropertyMapper<Video, VideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(Video.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(Video.Source)] = MapSource,
        [nameof(Video.IsLooping)] = MapIsLooping,
        [nameof(Video.Position)] = MapPosition
    };

    public static CommandMapper<Video, VideoHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(Video.UpdateStatus)] = MapUpdateStatus,
        [nameof(Video.PlayRequested)] = MapPlayRequested,
        [nameof(Video.PauseRequested)] = MapPauseRequested,
        [nameof(Video.StopRequested)] = MapStopRequested
    };

    public VideoHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}
```

The <xref:Microsoft.Maui.CommandMapper> is a `Dictionary` whose key is a `string` and whose value is a generic `Action`. The `string` represents the cross-platform control's command name, and the `Action` represents a `static` method that requires the handler, cross-platform control, and optional data as arguments. For example, the signature of the `MapPlayRequested` method is `public static void MapPlayRequested(VideoHandler handler, Video video, object? args)`.

Each platform handler must provide implementations of the Actions, which manipulate the native view APIs. This ensures that when a command is sent from the cross-platform control, the underlying native view will be manipulated as required. The advantage of this approach is that it removes the need for native views to subscribe to and unsubscribe from cross-platform control events. In addition, it allows for easy customization because the command mapper can be modified by cross-platform control consumers without subclassing.

## Create the platform controls

After creating the mappers for your handler, you must provide handler implementations on all platforms. This can be accomplished by adding partial class handler implementations in the child folders of the *Platforms* folder. Alternatively you could configure your project to support filename-based multi-targeting, or folder-based multi-targeting, or both.

The sample app is configured to support filename-based multi-targeting, so that the handler classes all are located in a single folder:

:::image type="content" source="media/create/handlers-folder.png" alt-text="Screenshot of the files in the Handlers folder of the project.":::

The `VideoHandler` class containing the mappers is named *VideoHandler.cs*. Its platform implementations are in the *VideoHandler.Android.cs*, *VideoHandler.MaciOS.cs*, and *VideoHandler.Windows.cs* files. This filename-based multi-targeting is configured by adding the following XML to the project file, as children of the `<Project>` node:

```xml
<!-- Android -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-android')) != true">
  <Compile Remove="**\*.Android.cs" />
  <None Include="**\*.Android.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- iOS and Mac Catalyst -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-ios')) != true AND $(TargetFramework.StartsWith('net8.0-maccatalyst')) != true">
  <Compile Remove="**\*.MaciOS.cs" />
  <None Include="**\*.MaciOS.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>

<!-- Windows -->
<ItemGroup Condition="$(TargetFramework.Contains('-windows')) != true ">
  <Compile Remove="**\*.Windows.cs" />
  <None Include="**\*.Windows.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
</ItemGroup>
```

For more information about configuring multi-targeting, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

Each platform handler class should be a partial class and derive from the <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, which requires two type arguments:

- The class for the cross-platform control, which derives from <xref:Microsoft.Maui.Controls.View>.
- The type of the native view that implements the cross-platform control on the platform. This should be identical to the type of the `PlatformView` property in the handler.

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Handlers.ViewHandler`2> class provides <xref:Microsoft.Maui.Handlers.ViewHandler`2.VirtualView> and <xref:Microsoft.Maui.Handlers.ViewHandler`2.PlatformView> properties. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.VirtualView> property is used to access the cross-platform control from its handler. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.PlatformView> property, is used to access the native view on each platform that implements the cross-platform control.

Each of the platform handler implementations should override the following methods:

- <xref:Microsoft.Maui.Handlers.ViewHandler`2.CreatePlatformView%2A>, which should create and return the native view that implements the cross-platform control.
- <xref:Microsoft.Maui.Handlers.ViewHandler`2.ConnectHandler%2A>, which should perform any native view setup, such as initializing the native view and performing event subscriptions.
- <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A>, which should perform any native view cleanup, such as unsubscribing from events and disposing objects.

::: moniker range="=net-maui-8.0"

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> method is intentionally not invoked by .NET MAUI. Instead, you must invoke it yourself from a suitable location in your app's lifecycle. For more information, see [Native view cleanup](#native-view-cleanup).

::: moniker-end

::: moniker range=">=net-maui-9.0"

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> method is automatically invoked by .NET MAUI by default, although this behavior can be changed. For more information, see [Handler disconnection](#handler-disconnection).

::: moniker-end

Each platform handler should also implement the Actions that are defined in the mapper dictionaries.

In addition, each platform handler should also provide code, as required, to implement the functionality of the cross-platform control on the platform. Alternatively, this can be provided by an additional type, which is the approach adopted here.

### Android

Video is played on Android with a `VideoView`. However, here, the `VideoView` has been encapsulated in a `MauiVideoPlayer` type to keep the native view separated from its handler. The following example shows the `VideoHandler` partial class for Android, with its three overrides:

```csharp
#nullable enable
using Microsoft.Maui.Handlers;
using VideoDemos.Controls;
using VideoDemos.Platforms.Android;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
    {
        protected override MauiVideoPlayer CreatePlatformView() => new MauiVideoPlayer(Context, VirtualView);

        protected override void ConnectHandler(MauiVideoPlayer platformView)
        {
            base.ConnectHandler(platformView);

            // Perform any control setup here
        }

        protected override void DisconnectHandler(MauiVideoPlayer platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }
        ...
    }
}
```

`VideoHandler` derives from the <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, with the generic `Video` argument specifying the cross-platform control type, and the `MauiVideoPlayer` argument specifying the type that encapsulates the `VideoView` native view.

The <xref:Microsoft.Maui.Handlers.ViewHandler`2.CreatePlatformView%2A> override creates and returns a `MauiVideoPlayer` object. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.ConnectHandler%2A> override is the location to perform any required native view setup. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override is the location to perform any native view cleanup, and so calls the `Dispose` method on the `MauiVideoPlayer` instance.

The platform handler also has to implement the Actions defined in the property mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapAreTransportControlsEnabled(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateTransportControlsEnabled();
    }

    public static void MapSource(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateSource();
    }

    public static void MapIsLooping(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateIsLooping();
    }    

    public static void MapPosition(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdatePosition();
    }
    ...
}
```

Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires handler and cross-platform control instances as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` type.

The platform handler also has to implement the Actions defined in the command mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapUpdateStatus(VideoHandler handler, Video video, object? args)
    {
        handler.PlatformView?.UpdateStatus();
    }

    public static void MapPlayRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PlayRequested(position);
    }

    public static void MapPauseRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PauseRequested(position);
    }

    public static void MapStopRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.StopRequested(position);
    }
    ...
}
```

Each Action is executed in response to a command being sent from the cross-platform control, and is a `static` method that requires handler and cross-platform control instances, and optional data as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` class, after extracting the optional data.

On Android, the `MauiVideoPlayer` class encapsulates the `VideoView` to keep the native view separated from its handler:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        MediaController _mediaController;
        bool _isPrepared;
        Context _context;
        Video _video;

        public MauiVideoPlayer(Context context, Video video) : base(context)
        {
            _context = context;
            _video = video;

            SetBackgroundColor(Color.Black);

            // Create a RelativeLayout for sizing the video
            RelativeLayout relativeLayout = new RelativeLayout(_context)
            {
                LayoutParameters = new CoordinatorLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                {
                    Gravity = (int)GravityFlags.Center
                }
            };

            // Create a VideoView and position it in the RelativeLayout
            _videoView = new VideoView(context)
            {
                LayoutParameters = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            };

            // Add to the layouts
            relativeLayout.AddView(_videoView);
            AddView(relativeLayout);

            // Handle events
            _videoView.Prepared += OnVideoViewPrepared;
        }
        ...
    }
}
```

`MauiVideoPlayer` derives from `CoordinatorLayout`, because the root native view in a .NET MAUI app on Android is `CoordinatorLayout`. While the `MauiVideoPlayer` class could derive from other native Android types, it can be difficult to control native view positioning in some scenarios.

The `VideoView` could be added directly to the `CoordinatorLayout`, and positioned in the layout as required. However, here, an Android `RelativeLayout` is added to the `CoordinatorLayout`, and the `VideoView` is added to the `RelativeLayout`. Layout parameters are set on both the `RelativeLayout` and `VideoView` so that the `VideoView` is centered in the page, and expands to fill the available space while maintaining its aspect ratio.

The constructor also subscribes to the `VideoView.Prepared` event. This event is raised when the video is ready for playback, and is unsubscribed from in the `Dispose` override:

```csharp
public class MauiVideoPlayer : CoordinatorLayout
{
    VideoView _videoView;
    Video _video;
    ...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _videoView.Prepared -= OnVideoViewPrepared;
            _videoView.Dispose();
            _videoView = null;
            _video = null;
        }

        base.Dispose(disposing);
    }
    ...
}
```

In addition to unsubscribing from the `Prepared` event, the `Dispose` override also performs native view cleanup.

> [!NOTE]
> The `Dispose` override is called by the handler's <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override.

The platform transport controls include buttons that play, pause, and stop the video, and are provided by Android's `MediaController` type. If the `Video.AreTransportControlsEnabled` property is set to `true`, a `MediaController` is set as the media player of the `VideoView`. This occurs because when the `AreTransportControlsEnabled` property is set, the handler's property mapper ensures that the `MapAreTransportControlsEnabled` method is invoked, which in turn calls the `UpdateTransportControlsEnabled` method in `MauiVideoPlayer`:

```csharp
public class MauiVideoPlayer : CoordinatorLayout
{
    VideoView _videoView;
    MediaController _mediaController;
    Video _video;
    ...

    public void UpdateTransportControlsEnabled()
    {
        if (_video.AreTransportControlsEnabled)
        {
            _mediaController = new MediaController(_context);
            _mediaController.SetMediaPlayer(_videoView);
            _videoView.SetMediaController(_mediaController);
        }
        else
        {
            _videoView.SetMediaController(null);
            if (_mediaController != null)
            {
                _mediaController.SetMediaPlayer(null);
                _mediaController = null;
            }
        }
    }
    ...
}
```

The transport controls fade out if they're not used but can be restored by tapping on the video.

If the `Video.AreTransportControlsEnabled` property is set to `false`, the `MediaController` is removed as the media player of the `VideoView`. In this scenario, you can then control video playback programmatically or supply your own transport controls. For more information, see [Create custom transport controls](#create-custom-transport-controls).

### iOS and Mac Catalyst

Video is played on iOS and Mac Catalyst with an `AVPlayer` and an `AVPlayerViewController`. However, here, these types are encapsulated in a `MauiVideoPlayer` type to keep the native views separated from their handler. The following example shows the `VideoHandler` partial class for iOS, with its three overrides:

```csharp
using Microsoft.Maui.Handlers;
using VideoDemos.Controls;
using VideoDemos.Platforms.MaciOS;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
    {
        protected override MauiVideoPlayer CreatePlatformView() => new MauiVideoPlayer(VirtualView);

        protected override void ConnectHandler(MauiVideoPlayer platformView)
        {
            base.ConnectHandler(platformView);

            // Perform any control setup here
        }

        protected override void DisconnectHandler(MauiVideoPlayer platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }
        ...
    }
}
```

`VideoHandler` derives from the <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, with the generic `Video` argument specifying the cross-platform control type, and the `MauiVideoPlayer` argument specifying the type that encapsulates the `AVPlayer` and `AVPlayerViewController` native views.

The <xref:Microsoft.Maui.Handlers.ViewHandler`2.CreatePlatformView%2A> override creates and returns a `MauiVideoPlayer` object. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.ConnectHandler%2A> override is the location to perform any required native view setup. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override is the location to perform any native view cleanup, and so calls the `Dispose` method on the `MauiVideoPlayer` instance.

The platform handler also has to implement the Actions defined in the property mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapAreTransportControlsEnabled(VideoHandler handler, Video video)
    {
        handler?.PlatformView.UpdateTransportControlsEnabled();
    }

    public static void MapSource(VideoHandler handler, Video video)
    {
        handler?.PlatformView.UpdateSource();
    }

    public static void MapIsLooping(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateIsLooping();
    }    

    public static void MapPosition(VideoHandler handler, Video video)
    {
        handler?.PlatformView.UpdatePosition();
    }
    ...
}
```

Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires handler and cross-platform control instances as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` type.

The platform handler also has to implement the Actions defined in the command mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapUpdateStatus(VideoHandler handler, Video video, object? args)
    {
        handler.PlatformView?.UpdateStatus();
    }

    public static void MapPlayRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PlayRequested(position);
    }

    public static void MapPauseRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PauseRequested(position);
    }

    public static void MapStopRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.StopRequested(position);
    }
    ...
}
```

Each Action is executed in response to a command being sent from the cross-platform control, and is a `static` method that requires handler and cross-platform control instances, and optional data as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` class, after extracting the optional data.

On iOS and Mac Catalyst, the `MauiVideoPlayer` class encapsulates the `AVPlayer` and `AVPlayerViewController` types to keep the native views separated from their handler:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        AVPlayerViewController _playerViewController;
        Video _video;
        ...

        public MauiVideoPlayer(Video video)
        {
            _video = video;

            _playerViewController = new AVPlayerViewController();
            _player = new AVPlayer();
            _playerViewController.Player = _player;
            _playerViewController.View.Frame = this.Bounds;

#if IOS16_0_OR_GREATER || MACCATALYST16_1_OR_GREATER
            // On iOS 16 and Mac Catalyst 16, for Shell-based apps, the AVPlayerViewController has to be added to the parent ViewController, otherwise the transport controls won't be displayed.
            var viewController = WindowStateManager.Default.GetCurrentUIViewController();

            // If there's no view controller, assume it's not Shell and continue because the transport controls will still be displayed.
            if (viewController?.View is not null)
            {
                // Zero out the safe area insets of the AVPlayerViewController
                UIEdgeInsets insets = viewController.View.SafeAreaInsets;
                _playerViewController.AdditionalSafeAreaInsets = new UIEdgeInsets(insets.Top * -1, insets.Left, insets.Bottom * -1, insets.Right);

                // Add the View from the AVPlayerViewController to the parent ViewController
                viewController.View.AddSubview(_playerViewController.View);
            }
#endif
            // Use the View from the AVPlayerViewController as the native control
            AddSubview(_playerViewController.View);
        }
        ...
    }
}
```

`MauiVideoPlayer` derives from `UIView`, which is the base class on iOS and Mac Catalyst for objects that display content and handle user interaction with that content. The constructor creates an `AVPlayer` object, which manages the playback and timing of a media file, and sets it as the `Player` property value of an `AVPlayerViewController`. The `AVPlayerViewController` displays content from the `AVPlayer` and presents transport controls and other features. The size and location of the control is then set, which ensures that the video is centered in the page and expands to fill the available space while maintaining its aspect ratio. On iOS 16 and Mac Catalyst 16, the `AVPlayerViewController` has to be added to the parent `ViewController` for Shell-based apps, otherwise the transport controls aren't displayed. The native view, which is the view from the `AVPlayerViewController`, is then added to the page.

The `Dispose` method is responsible for performing native view cleanup:

```csharp
public class MauiVideoPlayer : UIView
{
    AVPlayer _player;
    AVPlayerViewController _playerViewController;
    Video _video;
    ...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_player != null)
            {
                DestroyPlayedToEndObserver();
                _player.ReplaceCurrentItemWithPlayerItem(null);
                _player.Dispose();
            }
            if (_playerViewController != null)
                _playerViewController.Dispose();

            _video = null;
        }

        base.Dispose(disposing);
    }
    ...
}
```

In some scenarios, videos continue playing after a video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWithPlayerItem` is set to `null` in the `Dispose` override, and other native view cleanup is performed.

> [!NOTE]
> The `Dispose` override is called by the handler's <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override.

The platform transport controls include buttons that play, pause, and stop the video, and are provided by the `AVPlayerViewController` type. If the `Video.AreTransportControlsEnabled` property is set to `true`, the `AVPlayerViewController` will display its playback controls. This occurs because when the `AreTransportControlsEnabled` property is set, the handler's property mapper ensures that the `MapAreTransportControlsEnabled` method is invoked, which in turn calls the `UpdateTransportControlsEnabled` method in `MauiVideoPlayer`:

```csharp
public class MauiVideoPlayer : UIView
{
    AVPlayerViewController _playerViewController;
    Video _video;
    ...

    public void UpdateTransportControlsEnabled()
    {
        _playerViewController.ShowsPlaybackControls = _video.AreTransportControlsEnabled;
    }
    ...
}
```

The transport controls fade out if they're not used but can be restored by tapping on the video.

If the `Video.AreTransportControlsEnabled` property is set to `false`, the `AVPlayerViewController` doesn't show its playback controls. In this scenario, you can then control video playback programmatically or supply your own transport controls. For more information, see [Create custom transport controls](#create-custom-transport-controls).

### Windows

Video is played on Windows with the `MediaPlayerElement`. However, here, the `MediaPlayerElement` has been encapsulated in a `MauiVideoPlayer` type to keep the native view separated from its handler. The following example shows the `VideoHandler` partial class fo Windows, with its three overrides:

```csharp
#nullable enable
using Microsoft.Maui.Handlers;
using VideoDemos.Controls;
using VideoDemos.Platforms.Windows;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
    {
        protected override MauiVideoPlayer CreatePlatformView() => new MauiVideoPlayer(VirtualView);

        protected override void ConnectHandler(MauiVideoPlayer platformView)
        {
            base.ConnectHandler(platformView);

            // Perform any control setup here
        }

        protected override void DisconnectHandler(MauiVideoPlayer platformView)
        {
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }
        ...
    }
}
```

`VideoHandler` derives from the <xref:Microsoft.Maui.Handlers.ViewHandler`2> class, with the generic `Video` argument specifying the cross-platform control type, and the `MauiVideoPlayer` argument specifying the type that encapsulates the `MediaPlayerElement` native view.

The <xref:Microsoft.Maui.Handlers.ViewHandler`2.CreatePlatformView%2A> override creates and returns a `MauiVideoPlayer` object. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.ConnectHandler%2A> override is the location to perform any required native view setup. The <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override is the location to perform any native view cleanup, and so calls the `Dispose` method on the `MauiVideoPlayer` instance.

The platform handler also has to implement the Actions defined in the property mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapAreTransportControlsEnabled(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateTransportControlsEnabled();
    }

    public static void MapSource(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateSource();
    }

    public static void MapIsLooping(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdateIsLooping();
    }

    public static void MapPosition(VideoHandler handler, Video video)
    {
        handler.PlatformView?.UpdatePosition();
    }
    ...
}
```

Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires handler and cross-platform control instances as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` type.

The platform handler also has to implement the Actions defined in the command mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
{
    ...
    public static void MapUpdateStatus(VideoHandler handler, Video video, object? args)
    {
        handler.PlatformView?.UpdateStatus();
    }

    public static void MapPlayRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PlayRequested(position);
    }

    public static void MapPauseRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PauseRequested(position);
    }

    public static void MapStopRequested(VideoHandler handler, Video video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.StopRequested(position);
    }
    ...
}
```

Each Action is executed in response to a command being sent from the cross-platform control, and is a `static` method that requires handler and cross-platform control instances, and optional data as arguments. In each case, the Action calls a method defined in the `MauiVideoPlayer` class, after extracting the optional data.

On Windows, the `MauiVideoPlayer` class encapsulates the `MediaPlayerElement` to keep the native view separated from its handler:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        ...

        public MauiVideoPlayer(Video video)
        {
            _video = video;
            _mediaPlayerElement = new MediaPlayerElement();
            this.Children.Add(_mediaPlayerElement);
        }
        ...
    }
}
```

`MauiVideoPlayer` derives from <xref:Microsoft.Maui.Controls.Grid>, and the `MediaPlayerElement` is added as a child of the <xref:Microsoft.Maui.Controls.Grid>. This enables the `MediaPlayerElement` to automatically size to fill all available space.

The `Dispose` method is responsible for performing native view cleanup:

```csharp
public class MauiVideoPlayer : Grid, IDisposable
{
    MediaPlayerElement _mediaPlayerElement;
    Video _video;
    bool _isMediaPlayerAttached;
    ...

    public void Dispose()
    {
        if (_isMediaPlayerAttached)
        {
            _mediaPlayerElement.MediaPlayer.MediaOpened -= OnMediaPlayerMediaOpened;
            _mediaPlayerElement.MediaPlayer.Dispose();
        }
        _mediaPlayerElement = null;
    }
    ...
}
```

In addition to unsubscribing from the `MediaOpened` event, the `Dispose` override also performs native view cleanup.

> [!NOTE]
> The `Dispose` override is called by the handler's <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> override.

The platform transport controls include buttons that play, pause, and stop the video, and are provided by the `MediaPlayerElement` type. If the `Video.AreTransportControlsEnabled` property is set to `true`, the `MediaPlayerElement` will display its playback controls. This occurs because when the `AreTransportControlsEnabled` property is set, the handler's property mapper ensures that the `MapAreTransportControlsEnabled` method is invoked, which in turn calls the `UpdateTransportControlsEnabled` method in `MauiVideoPlayer`:

```csharp
public class MauiVideoPlayer : Grid, IDisposable
{
    MediaPlayerElement _mediaPlayerElement;
    Video _video;
    bool _isMediaPlayerAttached;
    ...

    public void UpdateTransportControlsEnabled()
    {
        _mediaPlayerElement.AreTransportControlsEnabled = _video.AreTransportControlsEnabled;
    }
    ...

}
```

If the `Video.AreTransportControlsEnabled` property is set to `false`, the `MediaPlayerElement` doesn't show its playback controls. In this scenario, you can then control video playback programmatically or supply your own transport controls. For more information, see [Create custom transport controls](#create-custom-transport-controls).

## Convert a cross-platform control into a platform control

Any .NET MAUI cross-platform control, that derives from <xref:Microsoft.Maui.Controls.Element>, can be converted to its underlying platform control with the <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> extension method:

- On Android, <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> converts a .NET MAUI control to an Android <xref:Android.Views.View> object.
- On iOS and Mac Catalyst, <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> converts a .NET MAUI control to a <xref:UIKit.UIView> object.
- On Windows, <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> converts a .NET MAUI control to a `FrameworkElement` object.

> [!NOTE]
> The <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> method is in the `Microsoft.Maui.Platform` namespace.

On all platforms, the <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> method requires a <xref:Microsoft.Maui.MauiContext> argument.

The <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> method can convert a cross-platform control to its underlying platform control from platform code, such as in a partial handler class for a platform:

```csharp
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using VideoDemos.Controls;
using VideoDemos.Platforms.Android;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<Video, MauiVideoPlayer>
    {
        ...
        public static void MapSource(VideoHandler handler, Video video)
        {
            handler.PlatformView?.UpdateSource();

            // Convert cross-platform control to its underlying platform control
            MauiVideoPlayer mvp = (MauiVideoPlayer)video.ToPlatform(handler.MauiContext);
            ...
        }
        ...
    }
}
```

In this example, in the `VideoHandler` partial class for Android, the `MapSource` method converts the `Video` instance to a `MauiVideoPlayer` object.

The <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> method can also convert a cross-platform control to its underlying platform control from cross-platform code:

```csharp
using Microsoft.Maui.Platform;

namespace VideoDemos.Views;

public partial class MyPage : ContentPage
{
    ...
    protected override void OnHandlerChanged()
    {
        // Convert cross-platform control to its underlying platform control
#if ANDROID
        Android.Views.View nativeView = video.ToPlatform(video.Handler.MauiContext);
#elif IOS || MACCATALYST
        UIKit.UIView nativeView = video.ToPlatform(video.Handler.MauiContext);
#elif WINDOWS
        Microsoft.UI.Xaml.FrameworkElement nativeView = video.ToPlatform(video.Handler.MauiContext);
#endif
        ...
    }
    ...
}
```

In this example, a cross-platform `Video` control named `video` is converted to its underlying native view on each platform in the <xref:Microsoft.Maui.Controls.Element.OnHandlerChanged> override. This override is called when the native view that implements the cross-platform control is available and initialized. The object returned by the <xref:Microsoft.Maui.Platform.ElementExtensions.ToPlatform%2A> method could be cast to its exact native type, which here is a `MauiVideoPlayer`.

## Play a video

The `Video` class defines a `Source` property, which is used to specify the source of the video file, and an `AutoPlay` property. `AutoPlay` defaults to `true`, which means that the video should begin playing automatically after `Source` has been set. For the definition of these properties, see [Create the cross-platform control](#create-the-cross-platform-control).

The `Source` property is of type `VideoSource`, which is an abstract class that consists of three static methods that instantiate the three classes that derive from `VideoSource`:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    [TypeConverter(typeof(VideoSourceConverter))]
    public abstract class VideoSource : Element
    {
        public static VideoSource FromUri(string uri)
        {
            return new UriVideoSource { Uri = uri };
        }

        public static VideoSource FromFile(string file)
        {
            return new FileVideoSource { File = file };
        }

        public static VideoSource FromResource(string path)
        {
            return new ResourceVideoSource { Path = path };
        }
    }
}
```

The `VideoSource` class includes a `TypeConverter` attribute that references `VideoSourceConverter`:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class VideoSourceConverter : TypeConverter, IExtendedTypeConverter
    {
        object IExtendedTypeConverter.ConvertFromInvariantString(string value, IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Uri uri;
                return Uri.TryCreate(value, UriKind.Absolute, out uri) && uri.Scheme != "file" ?
                    VideoSource.FromUri(value) : VideoSource.FromResource(value);
            }
            throw new InvalidOperationException("Cannot convert null or whitespace to VideoSource.");
        }
    }
}
```

The type converter is invoked when the `Source` property is set to a string in XAML. The `ConvertFromInvariantString` method attempts to convert the string to a `Uri` object. If it succeeds, and the scheme isn't `file`, then the method returns a `UriVideoSource`. Otherwise it returns a `ResourceVideoSource`.

### Play a web video

The `UriVideoSource` class is used to specify a remote video with a URI. It defines a `Uri` property of type `string`:

```csharp
namespace VideoDemos.Controls
{
    public class UriVideoSource : VideoSource
    {
        public static readonly BindableProperty UriProperty =
            BindableProperty.Create(nameof(Uri), typeof(string), typeof(UriVideoSource));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}
```

When the `Source` property is set to a `UriVideoSource`, the handler's property mapper ensures that the `MapSource` method is invoked:

```csharp
public static void MapSource(VideoHandler handler, Video video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

#### Android

Video is played on Android with a `VideoView`. The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `UriVideoSource`:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        bool _isPrepared;
        Video _video;
        ...

        public void UpdateSource()
        {
            _isPrepared = false;
            bool hasSetSource = false;

            if (_video.Source is UriVideoSource)
            {
                string uri = (_video.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                {
                    _videoView.SetVideoURI(Uri.Parse(uri));
                    hasSetSource = true;
                }
            }
            ...

            if (hasSetSource && _video.AutoPlay)
            {
                _videoView.Start();
            }
        }
        ...
    }
}
```

When processing objects of type `UriVideoSource`, the `SetVideoUri` method of `VideoView` is used to specify the video to be played, with an Android `Uri` object created from the string URI.

The `AutoPlay` property has no equivalent on `VideoView`, so the `Start` method is called if a new video has been set.

#### iOS and Mac Catalyst

To play a video on iOS and Mac Catalyst, an object of type `AVAsset` is created to encapsulate the video, and that is used to create an `AVPlayerItem`, which is then handed off to the `AVPlayer` object. The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `UriVideoSource`:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        AVPlayerItem _playerItem;
        Video _video;
        ...

        public void UpdateSource()
        {
            AVAsset asset = null;

            if (_video.Source is UriVideoSource)
            {
                string uri = (_video.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                    asset = AVAsset.FromUrl(new NSUrl(uri));
            }
            ...

            if (asset != null)
                _playerItem = new AVPlayerItem(asset);
            else
                _playerItem = null;

            _player.ReplaceCurrentItemWithPlayerItem(_playerItem);
            if (_playerItem != null && _video.AutoPlay)
            {
                _player.Play();
            }
        }
        ...
    }
}
```

When processing objects of type `UriVideoSource`, the static `AVAsset.FromUrl` method is used to specify the video to be played, with an iOS `NSUrl` object created from the string URI.

The `AutoPlay` property has no equivalent in the iOS video classes, so the property is examined at the end of the `UpdateSource` method to call the `Play` method on the `AVPlayer` object.

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWithPlayerItem` is set to `null` in the `Dispose` override:

```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        if (_player != null)
        {
            _player.ReplaceCurrentItemWithPlayerItem(null);
            ...
        }
        ...
    }
    base.Dispose(disposing);
}
```

#### Windows

Video is played on Windows with a `MediaPlayerElement`. The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `UriVideoSource`:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        bool _isMediaPlayerAttached;
        ...

        public async void UpdateSource()
        {
            bool hasSetSource = false;

            if (_video.Source is UriVideoSource)
            {
                string uri = (_video.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                {
                    _mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(uri));
                    hasSetSource = true;
                }
            }
            ...

            if (hasSetSource && !_isMediaPlayerAttached)
            {
                _isMediaPlayerAttached = true;
                _mediaPlayerElement.MediaPlayer.MediaOpened += OnMediaPlayerMediaOpened;
            }

            if (hasSetSource && _video.AutoPlay)
            {
                _mediaPlayerElement.AutoPlay = true;
            }
        }
        ...
    }
}
```

When processing objects of type `UriVideoSource`, the `MediaPlayerElement.Source` property is set to a `MediaSource` object that initializes a `Uri` with the URI of the video to be played. When the `MediaPlayerElement.Source` has been set, the `OnMediaPlayerMediaOpened` event handler method is registered against the `MediaPlayerElement.MediaPlayer.MediaOpened` event. This event handler is used to set the `Duration` property of the `Video` control.

At the end of the `UpdateSource` method, the `Video.AutoPlay` property is examined and if it's true the `MediaPlayerElement.AutoPlay` property is set to `true` to start video playback.

### Play a video resource

The `ResourceVideoSource` class is used to access video files that are embedded in the app. It defines a `Path` property of type `string`:

```csharp
namespace VideoDemos.Controls
{
    public class ResourceVideoSource : VideoSource
    {
        public static readonly BindableProperty PathProperty =
            BindableProperty.Create(nameof(Path), typeof(string), typeof(ResourceVideoSource));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
    }
}
```

When the `Source` property is set to a `ResourceVideoSource`, the handler's property mapper ensures that the `MapSource` method is invoked:

```csharp
public static void MapSource(VideoHandler handler, Video video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

#### Android

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `ResourceVideoSource`:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        bool _isPrepared;
        Context _context;
        Video _video;
        ...

        public void UpdateSource()
        {
            _isPrepared = false;
            bool hasSetSource = false;
            ...

            else if (_video.Source is ResourceVideoSource)
            {
                string package = Context.PackageName;
                string path = (_video.Source as ResourceVideoSource).Path;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string assetFilePath = "content://" + package + "/" + path;
                    _videoView.SetVideoPath(assetFilePath);
                    hasSetSource = true;
                }
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `ResourceVideoSource`, the `SetVideoPath` method of `VideoView` is used to specify the video to be played, with a string argument combining the app's package name with the video's filename.

A resource video file is stored in the package's *assets* folder, and requires a content provider to access it. The content provider is provided by the `VideoProvider` class, which creates an `AssetFileDescriptor` object that provides access to the video file:

```csharp
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Debug = System.Diagnostics.Debug;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    [ContentProvider(new string[] { "com.companyname.videodemos" })]
    public class VideoProvider : ContentProvider
    {
        public override AssetFileDescriptor OpenAssetFile(Uri uri, string mode)
        {
            var assets = Context.Assets;
            string fileName = uri.LastPathSegment;
            if (fileName == null)
                throw new FileNotFoundException();

            AssetFileDescriptor afd = null;
            try
            {
                afd = assets.OpenFd(fileName);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
            }
            return afd;
        }

        public override bool OnCreate()
        {
            return false;
        }
        ...
    }
}
```

#### iOS and Mac Catalyst

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `ResourceVideoSource`:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        Video _video;
        ...

        public void UpdateSource()
        {
            AVAsset asset = null;
            ...

            else if (_video.Source is ResourceVideoSource)
            {
                string path = (_video.Source as ResourceVideoSource).Path;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    string directory = Path.GetDirectoryName(path);
                    string filename = Path.GetFileNameWithoutExtension(path);
                    string extension = Path.GetExtension(path).Substring(1);
                    NSUrl url = NSBundle.MainBundle.GetUrlForResource(filename, extension, directory);
                    asset = AVAsset.FromUrl(url);
                }
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `ResourceVideoSource`, the `GetUrlForResource` method of `NSBundle` is used to retrieve the file from the app package. The complete path must be divided into a filename, extension, and directory.

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWithPlayerItem` is set to `null` in the `Dispose` override:

```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        if (_player != null)
        {
            _player.ReplaceCurrentItemWithPlayerItem(null);
            ...
        }
        ...
    }
    base.Dispose(disposing);
}
```

#### Windows

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `ResourceVideoSource`:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        ...

        public async void UpdateSource()
        {
            bool hasSetSource = false;

            ...
            else if (_video.Source is ResourceVideoSource)
            {
                string path = "ms-appx:///" + (_video.Source as ResourceVideoSource).Path;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    _mediaPlayerElement.Source = MediaSource.CreateFromUri(new Uri(path));
                    hasSetSource = true;
                }
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `ResourceVideoSource`, the `MediaPlayerElement.Source` property is set to a `MediaSource` object that initializes a `Uri` with the path of the video resource prefixed with `ms-appx:///`.

### Play a video file from the device's library

The `FileVideoSource` class is used to access videos in the device's video library. It defines a `File` property of type `string`:

```csharp
namespace VideoDemos.Controls
{
    public class FileVideoSource : VideoSource
    {
        public static readonly BindableProperty FileProperty =
            BindableProperty.Create(nameof(File), typeof(string), typeof(FileVideoSource));

        public string File
        {
            get { return (string)GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }
    }
}
```

When the `Source` property is set to a `FileVideoSource`, the handler's property mapper ensures that the `MapSource` method is invoked:

```csharp
public static void MapSource(VideoHandler handler, Video video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

#### Android

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `FileVideoSource`:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        bool _isPrepared;
        Video _video;
        ...

        public void UpdateSource()
        {
            _isPrepared = false;
            bool hasSetSource = false;
            ...

            else if (_video.Source is FileVideoSource)
            {
                string filename = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _videoView.SetVideoPath(filename);
                    hasSetSource = true;
                }
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `FileVideoSource`, the `SetVideoPath` method of `VideoView` is used to specify the video file to be played.

#### iOS and Mac Catalyst

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `FileVideoSource`:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        Video _video;
        ...

        public void UpdateSource()
        {
            AVAsset asset = null;
            ...

            else if (_video.Source is FileVideoSource)
            {
                string uri = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(uri))
                    asset = AVAsset.FromUrl(NSUrl.CreateFileUrl(new [] { uri }));
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `FileVideoSource`, the static `AVAsset.FromUrl` method is used to specify the video file to be played, with the `NSUrl.CreateFileUrl` method creating an iOS `NSUrl` object from the string URI.

#### Windows

The following code example shows how the `UpdateSource` method processes the `Source` property when it's of type `FileVideoSource`:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        ...

        public async void UpdateSource()
        {
            bool hasSetSource = false;

            ...
            else if (_video.Source is FileVideoSource)
            {
                string filename = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filename);
                    _mediaPlayerElement.Source = MediaSource.CreateFromStorageFile(storageFile);
                    hasSetSource = true;
                }
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `FileVideoSource`, the video filename is converted to a `StorageFile` object. Then, the `MediaSource.CreateFromStorageFile` method returns a `MediaSource` object that's set as the value of the `MediaPlayerElement.Source` property.

## Loop a video

The `Video` class defines an `IsLooping` property, which enables the control to automatically set the video position to the start after reaching its end. It defaults to `false`, which indicates that videos don't automatically loop.

When the `IsLooping` property is set, the handler's property mapper ensures that the `MapIsLooping` method is invoked:

```csharp
public static void MapIsLooping(VideoHandler handler, Video video)
{
    handler.PlatformView?.UpdateIsLooping();
}  
```

The `MapIsLooping` method in turn calls the `UpdateIsLooping` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

### Android

The following code example shows how the `UpdateIsLooping` method on Android enables video looping:

```csharp
using Android.Content;
using Android.Media;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout, MediaPlayer.IOnPreparedListener
    {
        VideoView _videoView;
        Video _video;
        ...

        public void UpdateIsLooping()
        {
            if (_video.IsLooping)
            {
                _videoView.SetOnPreparedListener(this);
            }
            else
            {
                _videoView.SetOnPreparedListener(null);
            }
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = _video.IsLooping;
        }
        ...
    }
}
```

To enable video looping, the `MauiVideoPlayer` class implements the `MediaPlayer.IOnPreparedListener` interface. This interface defines an `OnPrepared` callback that's invoked when the media source is ready for playback. When the `Video.IsLooping` property is `true`, the `UpdateIsLooping` method sets `MauiVideoPlayer` as the object that provides the `OnPrepared` callback. The callback sets the `MediaPlayer.IsLooping` property to the value of the `Video.IsLooping` property.

### iOS and Mac Catalyst

The following code example shows how the `UpdateIsLooping` method on iOS and Mac Catalyst enables video looping:

```csharp
using System.Diagnostics;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        AVPlayerViewController _playerViewController;
        Video _video;
        NSObject? _playedToEndObserver;
        ...

        public void UpdateIsLooping()
        {
            DestroyPlayedToEndObserver();
            if (_video.IsLooping)
            {
                _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
                _playedToEndObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, PlayedToEnd);
            }
            else
                _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.Pause;
        }

        void PlayedToEnd(NSNotification notification)
        {
            if (_video == null || notification.Object != _playerViewController.Player?.CurrentItem)
                return;

            _playerViewController.Player?.Seek(CMTime.Zero);
        }
        ...
    }
}
```

On iOS and Mac Catalyst, a notification is used to execute a callback when the video has been played to the end. When the `Video.IsLooping` property is `true`, the `UpdateIsLooping` method adds an observer for the `AVPlayerItem.DidPlayToEndTimeNotification` notification, and executes the `PlayedToEnd` method when the notification is received. In turn, this method resumes playback from the beginning of the video. If the `Video.IsLooping` property is `false`, the video pauses at the end of playback.

Because `MauiVideoPlayer` adds an observer for a notification it must also remove the observer when performing native view cleanup. This is accomplished in the `Dispose` override:

```csharp
public class MauiVideoPlayer : UIView
{
    AVPlayer _player;
    AVPlayerViewController _playerViewController;
    Video _video;
    NSObject? _playedToEndObserver;
    ...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_player != null)
            {
                DestroyPlayedToEndObserver();
                ...
            }
            ...
        }

        base.Dispose(disposing);
    }

    void DestroyPlayedToEndObserver()
    {
        if (_playedToEndObserver != null)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_playedToEndObserver);
            DisposeObserver(ref _playedToEndObserver);
        }
    }

    void DisposeObserver(ref NSObject? disposable)
    {
        disposable?.Dispose();
        disposable = null;
    }
    ...
}
```

The `Dispose` override calls the `DestroyPlayedToEndObserver` method that removes the observer for the `AVPlayerItem.DidPlayToEndTimeNotification` notification, and which also invokes the `Dispose` method on the `NSObject`.

### Windows

The following code example shows how the `UpdateIsLooping` method on Windows enables video looping:

```csharp
public void UpdateIsLooping()
{
    if (_isMediaPlayerAttached)
        _mediaPlayerElement.MediaPlayer.IsLoopingEnabled = _video.IsLooping;
}
```

To enable video looping, the `UpdateIsLooping` method sets the `MediaPlayerElement.MediaPlayer.IsLoopingEnabled` property to the value of the `Video.IsLooping` property.

## Create custom transport controls

The transport controls of a video player include buttons that play, pause, and stop the video. These buttons are often identified with familiar icons rather than text, and the play and pause buttons are often combined into one button.

By default, the `Video` control displays transport controls supported by each platform. However, when you set the `AreTransportControlsEnabled` property to `false`, these controls are suppressed. You can then control video playback programmatically or supply your own transport controls.

Implementing your own transport controls requires the `Video` class to be able to notify its native views to play, pause, or stop the video, and know the current status of video playback. The `Video` class defines methods named `Play`, `Pause`, and `Stop` that raise a corresponding event, and send a command to the `VideoHandler`:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        public event EventHandler<VideoPositionEventArgs> PlayRequested;
        public event EventHandler<VideoPositionEventArgs> PauseRequested;
        public event EventHandler<VideoPositionEventArgs> StopRequested;

        public void Play()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            PlayRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(Video.PlayRequested), args);
        }

        public void Pause()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            PauseRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(Video.PauseRequested), args);
        }

        public void Stop()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            StopRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(Video.StopRequested), args);
        }
    }
}
```

The `VideoPositionEventArgs` class defines a `Position` property that can be set through its constructor. This property represents the position at which video playback was started, paused, or stopped.

The final line in the `Play`, `Pause`, and `Stop` methods sends a command and associated data to `VideoHandler`. The <xref:Microsoft.Maui.CommandMapper> for `VideoHandler` maps command names to Actions that are executed when a command is received. For example, when `VideoHandler` receives the `PlayRequested` command, it executes its `MapPlayRequested` method. The advantage of this approach is that it removes the need for native views to subscribe to and unsubscribe from cross-platform control events. In addition, it allows for easy customization because the command mapper can be modified by cross-platform control consumers without subclassing. For more information about <xref:Microsoft.Maui.CommandMapper>, see [Create the command mapper](create.md#create-the-command-mapper).

The `MauiVideoPlayer` implementation on Android, iOS and Mac Catalyst, has `PlayRequested`, `PauseRequested`, and `StopRequested` methods that are executed in response to the `Video` control sending `PlayRequested`, `PauseRequested`, and `StopRequested` commands. Each method invokes a method on its native view to play, pause, or stop the video. For example, the following code shows the `PlayRequested`, `PauseRequested`, and `StopRequested` methods on iOS and Mac Catalyst:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        ...

        public void PlayRequested(TimeSpan position)
        {
            _player.Play();
            Debug.WriteLine($"Video playback from {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
        }

        public void PauseRequested(TimeSpan position)
        {
            _player.Pause();
            Debug.WriteLine($"Video paused at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
        }

        public void StopRequested(TimeSpan position)
        {
            _player.Pause();
            _player.Seek(new CMTime(0, 1));
            Debug.WriteLine($"Video stopped at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
        }
    }
}
```

Each of the three methods logs the position at which the video was played, paused, or stopped, using the data that's sent with the command.

This mechanism ensures that when the `Play`, `Pause`, or `Stop` method is invoked on the `Video` control, its native view is instructed to play, pause, or stop the video and log the position at which the video was played, paused, or stopped. This all happens using a decoupled approach, without native views having to subscribe to cross-platform events.

### Video status

Implementing play, pause, and stop functionality isn't sufficient for supporting custom transport controls. Often the play and pause functionality should be implemented with the same button, which changes its appearance to indicate whether the video is currently playing or paused. In addition, the button shouldn't even be enabled if the video hasn't yet loaded.

These requirements imply that the video player needs to make available a current status indicating if it's playing or paused, or if it's not yet ready to play a video. This status can be represented by an enumeration:

```csharp
public enum VideoStatus
{
    NotReady,
    Playing,
    Paused
}
```

The `Video` class defines a read-only bindable property named `Status` of type `VideoStatus`. This property is defined as read-only because it should only be set from the control's handler:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        private static readonly BindablePropertyKey StatusPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Status), typeof(VideoStatus), typeof(Video), VideoStatus.NotReady);

        public static readonly BindableProperty StatusProperty = StatusPropertyKey.BindableProperty;

        public VideoStatus Status
        {
            get { return (VideoStatus)GetValue(StatusProperty); }
        }

        VideoStatus IVideoController.Status
        {
            get { return Status; }
            set { SetValue(StatusPropertyKey, value); }
        }
        ...
    }
}
```

Usually, a read-only bindable property would have a private `set` accessor on the `Status` property to allow it to be set from within the class. However, for a <xref:Microsoft.Maui.Controls.View> derivative supported by handlers, the property must be set from outside the class but only by the control's handler.

For this reason, another property is defined with the name `IVideoController.Status`. This is an explicit interface implementation, and is made possible by the `IVideoController` interface that the `Video` class implements:

```csharp
public interface IVideoController
{
    VideoStatus Status { get; set; }
    TimeSpan Duration { get; set; }
}
```

This interface makes it possible for a class external to `Video` to set the `Status` property by referencing the `IVideoController` interface. The property can be set from other classes and the handler, but it's unlikely to be set inadvertently. Most importantly, the `Status` property can't be set through a data binding.

To assist the handler implementations in keeping the `Status` property updated, the `Video` class defines an `UpdateStatus` event and command:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        public event EventHandler UpdateStatus;

        IDispatcherTimer _timer;

        public Video()
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        ~Video() => _timer.Tick -= OnTimerTick;

        void OnTimerTick(object sender, EventArgs e)
        {
            UpdateStatus?.Invoke(this, EventArgs.Empty);
            Handler?.Invoke(nameof(Video.UpdateStatus));
        }
        ...
    }
}
```

The `OnTimerTick` event handler is executed every tenth of a second, which raises the `UpdateStatus` event and invokes the `UpdateStatus` command.

When the `UpdateStatus` command is sent from the `Video` control to its handler, the handler's command mapper ensures that the `MapUpdateStatus` method is invoked:

```csharp
public static void MapUpdateStatus(VideoHandler handler, Video video, object? args)
{
    handler.PlatformView?.UpdateStatus();
}
```

The `MapUpdateStatus` method in turns calls the `UpdateStatus` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, encapsulates the native views that provide the video player implementation on each platform.

#### Android

The following code example shows the `UpdateStatus` method on Android sets the `Status` property:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        bool _isPrepared;
        Video _video;
        ...

        public MauiVideoPlayer(Context context, Video video) : base(context)
        {
            _video = video;
            ...
            _videoView.Prepared += OnVideoViewPrepared;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _videoView.Prepared -= OnVideoViewPrepared;
                ...
            }

            base.Dispose(disposing);
        }

        void OnVideoViewPrepared(object sender, EventArgs args)
        {
            _isPrepared = true;
            ((IVideoController)_video).Duration = TimeSpan.FromMilliseconds(_videoView.Duration);
        }

        public void UpdateStatus()
        {
            VideoStatus status = VideoStatus.NotReady;

            if (_isPrepared)
                status = _videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;

            ((IVideoController)_video).Status = status;
            ...
        }
        ...
    }
}
```

The `VideoView.IsPlaying` property is a Boolean that indicates if the video is playing or paused. To determine if the `VideoView` can't play or pause the video, its `Prepared` event must be handled. This event is raised when the media source is ready for playback. The event is subscribed to in the `MauiVideoPlayer` constructor, and unsubscribed from in its `Dispose` override. The `UpdateStatus` method then uses the `isPrepared` field and the `VideoView.IsPlaying` property to set the `Status` property on the `Video` object by casting it to `IVideoController`.

#### iOS and Mac Catalyst

The following code example shows the `UpdateStatus` method on iOS and Mac Catalyst sets the `Status` property:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        Video _video;
        ...

        public void UpdateStatus()
        {
            VideoStatus videoStatus = VideoStatus.NotReady;

            switch (_player.Status)
            {
                case AVPlayerStatus.ReadyToPlay:
                    switch (_player.TimeControlStatus)
                    {
                        case AVPlayerTimeControlStatus.Playing:
                            videoStatus = VideoStatus.Playing;
                            break;

                        case AVPlayerTimeControlStatus.Paused:
                            videoStatus = VideoStatus.Paused;
                            break;
                    }
                    break;
            }
            ((IVideoController)_video).Status = videoStatus;
            ...
        }
        ...
    }
}
```

Two properties of `AVPlayer` must be accessed to set the `Status` property - the `Status` property of type `AVPlayerStatus` and the `TimeControlStatus` property of type `AVPlayerTimeControlStatus`. The `Status` property can then be set on the `Video` object by casting it to `IVideoController`.

#### Windows

The following code example shows the `UpdateStatus` method on Windows sets the `Status` property:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        bool _isMediaPlayerAttached;
        ...

        public void UpdateStatus()
        {
            if (_isMediaPlayerAttached)
            {
                VideoStatus status = VideoStatus.NotReady;

                switch (_mediaPlayerElement.MediaPlayer.CurrentState)
                {
                    case MediaPlayerState.Playing:
                        status = VideoStatus.Playing;
                        break;
                    case MediaPlayerState.Paused:
                    case MediaPlayerState.Stopped:
                        status = VideoStatus.Paused;
                        break;
                }

                ((IVideoController)_video).Status = status;
                _video.Position = _mediaPlayerElement.MediaPlayer.Position;
            }
        }
        ...
    }
}
```

The `UpdateStatus` method uses the value of the `MediaPlayerElement.MediaPlayer.CurrentState` property to determine the value of the `Status` property. The `Status` property can then be set on the `Video` object by casting it to `IVideoController`.

### Positioning bar

The transport controls implemented by each platform include a positioning bar. This bar resembles a slider or scroll bar, and shows the current location of the video within its total duration. Users can manipulate the positioning bar to move forwards or backwards to a new position in the video.

Implementing your own positioning bar requires the `Video` class to know the duration of the video, and its current position within that duration.

#### Duration

One item of information that the `Video` control needs to support a custom positioning bar is the duration of the video. The `Video` class defines a read-only bindable property named `Duration`, of type `TimeSpan`. This property is defined as read-only because it should only be set from the control's handler:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        private static readonly BindablePropertyKey DurationPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Duration), typeof(TimeSpan), typeof(Video), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((Video)bindable).SetTimeToEnd());

        public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
        }

        TimeSpan IVideoController.Duration
        {
            get { return Duration; }
            set { SetValue(DurationPropertyKey, value); }
        }
        ...
    }
}
```

Usually, a read-only bindable property would have a private `set` accessor on the `Duration` property to allow it to be set from within the class. However, for a <xref:Microsoft.Maui.Controls.View> derivative supported by handlers, the property must be set from outside the class but only by the control's handler.

> [!NOTE]
> The property-changed event handler for the `Duration` bindable property calls a method named `SetTimeToEnd`, which is described in [Calculating time to end](#calculating-time-to-end).

For this reason, another property is defined with the name `IVideoController.Duration`. This is an explicit interface implementation, and is made possible by the `IVideoController` interface that the `Video` class implements:

```csharp
public interface IVideoController
{
    VideoStatus Status { get; set; }
    TimeSpan Duration { get; set; }
}
```

This interface makes it possible for a class external to `Video` to set the `Duration` property by referencing the `IVideoController` interface. The property can be set from other classes and the handler, but it's unlikely to be set inadvertently. Most importantly, the `Duration` property can't be set through a data binding.

The duration of a video isn't available immediately after the `Source` property of the `Video` control is set. The video must be partially downloaded before the native view can determine its duration.

##### Android

On Android, the `VideoView.Duration` property reports a valid duration in milliseconds after the `VideoView.Prepared` event has been raised. The `MauiVideoPlayer` class uses the `Prepared` event handler to obtain the `Duration` property value:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        Video _video;
        ...

        void OnVideoViewPrepared(object sender, EventArgs args)
        {
            ...
            ((IVideoController)_video).Duration = TimeSpan.FromMilliseconds(_videoView.Duration);
        }
        ...
    }
}
```

##### iOS and Mac Catalyst

On iOS and Mac Catalyst, the duration of a video is obtained from the `AVPlayerItem.Duration` property, but not immediately after the `AVPlayerItem` is created. It's possible to set an iOS observer for the `Duration` property, but the `MauiVideoPlayer` class obtains the duration in the `UpdateStatus` method that's called 10 times a second:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayerItem _playerItem;
        ...

        TimeSpan ConvertTime(CMTime cmTime)
        {
            return TimeSpan.FromSeconds(Double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);
        }

        public void UpdateStatus()
        {
            ...
            if (_playerItem != null)
            {
                ((IVideoController)_video).Duration = ConvertTime(_playerItem.Duration);
                ...
            }
        }
        ...
    }
}
```

The `ConvertTime` method converts a `CMTime` object to a `TimeSpan` value.

##### Windows

On Windows, the `MediaPlayerElement.MediaPlayer.NaturalDuration` property is a `TimeSpan` value that becomes valid when the `MediaPlayerElement.MediaPlayer.MediaOpened` event has been raised. The `MauiVideoPlayer` class uses the `MediaOpened` event handler to obtain the `NaturalDuration` property value:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        bool _isMediaPlayerAttached;
        ...

        void OnMediaPlayerMediaOpened(MediaPlayer sender, object args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ((IVideoController)_video).Duration = _mediaPlayerElement.MediaPlayer.NaturalDuration;
            });
        }
        ...
    }
}
```

The `OnMediaPlayer` event handler then calls the `MainThread.BeginInvokeOnMainThread` method to set the `Duration` property on the `Video` object, by casting it to `IVideoController`, on the main thread. This is necessary because the `MediaPlayerElement.MediaPlayer.MediaOpened` event is handled on a background thread. For more information about running code on the main thread, see [Create a thread on the .NET MAUI UI thread](~/platform-integration/appmodel/main-thread.md).

#### Position

The `Video` control also needs a `Position` property that increases from zero to `Duration` as the video plays. The `Video` class implements this property as a bindable property with public `get` and `set` accessors:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(Video), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((Video)bindable).SetTimeToEnd());

        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        ...
    }
}
```

The `get` accessor returns the current position of the video as its playing. The `set` accessor responds to user manipulation of the positioning bar by moving the video position forwards or backwards.

> [!NOTE]
> The property-changed event handler for the `Position` bindable property calls a method named `SetTimeToEnd`, which is described in [Calculating time to end](#calculating-time-to-end).

On Android, iOS and Mac Catalyst, the property that obtains the current position only has a `get` accessor. Instead, a `Seek` method is available to set the position. This seems to be a more sensible approach than using a single `Position` property, which has an inherent problem. As a video plays, a `Position` property must be continually updated to reflect the new position. But you don't want most changes of the `Position` property to cause the video player to move to a new position in the video. If that happens, the video player would respond by seeking to the last value of the `Position` property, and the video wouldn't advance.

Despite the difficulties of implementing a `Position` property with `get` and `set` accessors, this approach is used because it can utilize data binding. The `Position` property of the `Video` control can be bound to a <xref:Microsoft.Maui.Controls.Slider> that's used both to display the position and to seek a new position. However, several precautions are necessary when implementing the `Position` property, to avoid feedback loops.

##### Android

On Android, the `VideoView.CurrentPosition` property indicates the current position of the video. The `MauiVideoPlayer` class sets the `Position` property in the `UpdateStatus` method at the same time as it sets the `Duration` property:

```csharp
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using VideoDemos.Controls;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace VideoDemos.Platforms.Android
{
    public class MauiVideoPlayer : CoordinatorLayout
    {
        VideoView _videoView;
        Video _video;
        ...

        public void UpdateStatus()
        {
            ...
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(_videoView.CurrentPosition);
            _video.Position = timeSpan;
        }

        public void UpdatePosition()
        {
            if (Math.Abs(_videoView.CurrentPosition - _video.Position.TotalMilliseconds) > 1000)
            {
                _videoView.SeekTo((int)_video.Position.TotalMilliseconds);
            }
        }
        ...
    }
}
```

Every time the `Position` property is set by the `UpdateStatus` method, the `Position` property fires a `PropertyChanged` event, which causes the property mapper for the handler to call the `UpdatePosition` method. The `UpdatePosition` method should do nothing for most of the property changes. Otherwise, with every change in the video's position it would be moved to same position it just reached. To avoid this feedback loop, the `UpdatePosition` only calls the `Seek` method on the `VideoView` object when the difference between the `Position` property and the current position of the `VideoView` is greater than one second.

##### iOS and Mac Catalyst

On iOS and Mac Catalyst, the `AVPlayerItem.CurrentTime` property indicates the current position of the video. The `MauiVideoPlayer` class sets the `Position` property in the `UpdateStatus` method at the same time as it sets the `Duration` property:

```csharp
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using System.Diagnostics;
using UIKit;
using VideoDemos.Controls;

namespace VideoDemos.Platforms.MaciOS
{
    public class MauiVideoPlayer : UIView
    {
        AVPlayer _player;
        AVPlayerItem _playerItem;
        Video _video;
        ...

        TimeSpan ConvertTime(CMTime cmTime)
        {
            return TimeSpan.FromSeconds(Double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);
        }

        public void UpdateStatus()
        {
            ...
            if (_playerItem != null)
            {
                ...
                _video.Position = ConvertTime(_playerItem.CurrentTime);
            }
        }

        public void UpdatePosition()
        {
            TimeSpan controlPosition = ConvertTime(_player.CurrentTime);
            if (Math.Abs((controlPosition - _video.Position).TotalSeconds) > 1)
            {
                _player.Seek(CMTime.FromSeconds(_video.Position.TotalSeconds, 1));
            }
        }
        ...
    }
}
```

Every time the `Position` property is set by the `UpdateStatus` method, the `Position` property fires a `PropertyChanged` event, which causes the property mapper for the handler to call the `UpdatePosition` method. The `UpdatePosition` method should do nothing for most of the property changes. Otherwise, with every change in the video's position it would be moved to same position it just reached. To avoid this feedback loop, the `UpdatePosition` only calls the `Seek` method on the `AVPlayer` object when the difference between the `Position` property and the current position of the `AVPlayer` is greater than one second.

##### Windows

On Windows, the `MediaPlayerElement.MedaPlayer.Position` property indicates the current position of the video. The `MauiVideoPlayer` class sets the `Position` property in the `UpdateStatus` method at the same time as it sets the `Duration` property:

```csharp
using Microsoft.UI.Xaml.Controls;
using VideoDemos.Controls;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Grid = Microsoft.UI.Xaml.Controls.Grid;

namespace VideoDemos.Platforms.Windows
{
    public class MauiVideoPlayer : Grid, IDisposable
    {
        MediaPlayerElement _mediaPlayerElement;
        Video _video;
        bool _isMediaPlayerAttached;
        ...

        public void UpdateStatus()
        {
            if (_isMediaPlayerAttached)
            {
                ...
                _video.Position = _mediaPlayerElement.MediaPlayer.Position;
            }
        }

        public void UpdatePosition()
        {
            if (_isMediaPlayerAttached)
            {
                if (Math.Abs((_mediaPlayerElement.MediaPlayer.Position - _video.Position).TotalSeconds) > 1)
                {
                    _mediaPlayerElement.MediaPlayer.Position = _video.Position;
                }
            }
        }
        ...
    }
}
```

Every time the `Position` property is set by the `UpdateStatus` method, the `Position` property fires a `PropertyChanged` event, which causes the property mapper for the handler to call the `UpdatePosition` method. The `UpdatePosition` method should do nothing for most of the property changes. Otherwise, with every change in the video's position it would be moved to same position it just reached. To avoid this feedback loop, the `UpdatePosition` only sets the `MediaPlayerElement.MediaPlayer.Position` property when the difference between the `Position` property and the current position of the `MediaPlayerElement` is greater than one second.

#### Calculating time to end

Sometimes video players show the time remaining in the video. This value begins at the video's duration when the video begins, and decreases down to zero when the video ends.

The `Video` class includes a read-only `TimeToEnd` property that's calculated based on changes to the `Duration` and `Position` properties:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideoController
    {
        ...
        private static readonly BindablePropertyKey TimeToEndPropertyKey =
            BindableProperty.CreateReadOnly(nameof(TimeToEnd), typeof(TimeSpan), typeof(Video), new TimeSpan());

        public static readonly BindableProperty TimeToEndProperty = TimeToEndPropertyKey.BindableProperty;

        public TimeSpan TimeToEnd
        {
            get { return (TimeSpan)GetValue(TimeToEndProperty); }
            private set { SetValue(TimeToEndPropertyKey, value); }
        }

        void SetTimeToEnd()
        {
            TimeToEnd = Duration - Position;
        }
        ...
    }
}
```

The `SetTimeToEnd` method is called from the property-changed event handlers of the `Duration` and `Position` properties.

#### Custom positioning bar

A custom positioning bar can be implemented by creating a class that derives from <xref:Microsoft.Maui.Controls.Slider>, which contains `Duration` and `Position` properties of type `TimeSpan`:

```csharp
namespace VideoDemos.Controls
{
    public class PositionSlider : Slider
    {
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration), typeof(TimeSpan), typeof(PositionSlider), new TimeSpan(1),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    double seconds = ((TimeSpan)newValue).TotalSeconds;
                    ((Slider)bindable).Maximum = seconds <= 0 ? 1 : seconds;
                });

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(PositionSlider), new TimeSpan(0),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    double seconds = ((TimeSpan)newValue).TotalSeconds;
                    ((Slider)bindable).Value = seconds;
                });

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue (PositionProperty, value); }
        }

        public PositionSlider()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Value")
                {
                    TimeSpan newPosition = TimeSpan.FromSeconds(Value);
                    if (Math.Abs(newPosition.TotalSeconds - Position.TotalSeconds) / Duration.TotalSeconds > 0.01)
                        Position = newPosition;
                }
            };
        }
    }
}
```

The property-changed event handler for the `Duration` property sets the `Maximum` property of the <xref:Microsoft.Maui.Controls.Slider> to the `TotalSeconds` property of the `TimeSpan` value. Similarly, the property-changed event handler for the `Position` property sets the `Value` property of the <xref:Microsoft.Maui.Controls.Slider>. This is the mechanism by which the <xref:Microsoft.Maui.Controls.Slider> tracks the position of `PositionSlider`.

The `PositionSlider` is updated from the underlying <xref:Microsoft.Maui.Controls.Slider> in only one scenario, which is when the user manipulates the <xref:Microsoft.Maui.Controls.Slider> to indicate that the video should be advanced or reversed to a new position. This is detected in the `PropertyChanged` handler in the `PositionSlider` constructor. This event handler checks for a change in the `Value` property, and if it's different from the `Position` property, then the `Position` property is set from the `Value` property.

## Register the handler

A custom control and its handler must be registered with an app, before it can be consumed. This should occur in the `CreateMauiApp` method in the `MauiProgram` class in your app project, which is the cross-platform entry point for the app:

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

The handler is registered with the <xref:Microsoft.Maui.Hosting.HandlerMauiAppBuilderExtensions.ConfigureMauiHandlers%2A> and <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> method. The first argument to the <xref:Microsoft.Maui.Hosting.MauiHandlersCollectionExtensions.AddHandler%2A> method is the cross-platform control type, with the second argument being its handler type.

## Consume the cross-platform control

After registering the handler with your app, the cross-platform control can be consumed.

### Play a web video

The `Video` control can play a video from a URL, as shown in the following example:

```xaml
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

### Play a video resource

Video files that are embedded in the *Resources\Raw* folder of the app, with a **MauiAsset** build action, can be played by the `Video` control:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             x:Class="VideoDemos.Views.PlayVideoResourcePage"
             Unloaded="OnContentPageUnloaded"
             Title="Play video resource">
    <controls:Video x:Name="video"
                    Source="video.mp4" />
</ContentPage>
```

In this example, the `VideoSourceConverter` class converts the string that represents the filename of the video to a `ResourceVideoSource`. For each platform, the video begins playing almost immediately after the video source is set because the file is in the app package and doesn't need to be downloaded. On each platform, the transport controls fade out if they're not used but can be restored by tapping on the video.

### Play a video file from the device's library

Video files that are stored on the device can be retrieved and then played by the `Video` control:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             x:Class="VideoDemos.Views.PlayLibraryVideoPage"
             Unloaded="OnContentPageUnloaded"
             Title="Play library video">
    <Grid RowDefinitions="*,Auto">
        <controls:Video x:Name="video" />
        <Button Grid.Row="1"
                Text="Show Video Library"
                Margin="10"
                HorizontalOptions="Center"
                Clicked="OnShowVideoLibraryClicked" />
    </Grid>
</ContentPage>
```

When the <xref:Microsoft.Maui.Controls.Button> is tapped its `Clicked` event handler is executed, which is shown in the following code example:

```csharp
async void OnShowVideoLibraryClicked(object sender, EventArgs e)
{
    Button button = sender as Button;
    button.IsEnabled = false;

    var pickedVideo = await MediaPicker.PickVideoAsync();
    if (!string.IsNullOrWhiteSpace(pickedVideo?.FileName))
    {
        video.Source = new FileVideoSource
        {
            File = pickedVideo.FullPath
        };
    }

    button.IsEnabled = true;
}
```

The `Clicked` event handler uses .NET MAUI's `MediaPicker` class to let the user pick a video file from the device. The picked video file is then encapsulated as a `FileVideoSource` object and set as the `Source` property of the `Video` control. For more information about the `MediaPicker` class, see [Media picker](~/platform-integration/device-media/picker.md). For each platform, the video begins playing almost immediately after the video source is set because the file is on the device and doesn't need to be downloaded. On each platform, the transport controls fade out if they're not used but can be restored by tapping on the video.

### Configure the Video control

You can prevent a video from automatically starting by setting the `AutoPlay` property to `false`:

```xaml
<controls:Video x:Name="video"
                Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4"
                AutoPlay="False" />
```

You can suppress the transport controls by setting the `AreTransportControlsEnabled` property to `false`:

```xaml
<controls:Video x:Name="video"
                Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4"
                AreTransportControlsEnabled="False" />
```

If you set `AutoPlay` and `AreTransportControlsEnabled` to `false`, the video won't begin playing and there will be no way to start it playing. In this scenario you'd need to call the `Play` method from the code-behind file, or create your own transport controls.

In addition, you can set a video to loop by setting the `IsLooping` property to `true:`

```xaml
<controls:Video x:Name="video"
                Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4"
                IsLooping="true" />
```

If you set the `IsLooping` property to `true` this ensures that the `Video` control automatically sets the video position to the start after reaching its end.

### Use custom transport controls

The following XAML example shows custom transport controls that play, pause, and stop the video:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             x:Class="VideoDemos.Views.CustomTransportPage"
             Unloaded="OnContentPageUnloaded"
             Title="Custom transport controls">
    <Grid RowDefinitions="*,Auto">
        <controls:Video x:Name="video"
                        AutoPlay="False"
                        AreTransportControlsEnabled="False"
                        Source="https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4" />
        <ActivityIndicator Color="Gray"
                           IsVisible="False">
            <ActivityIndicator.Triggers>
                <DataTrigger TargetType="ActivityIndicator"
                             Binding="{Binding Source={x:Reference video},
                                               Path=Status}"
                             Value="{x:Static controls:VideoStatus.NotReady}">
                    <Setter Property="IsVisible"
                            Value="True" />
                    <Setter Property="IsRunning"
                            Value="True" />
                </DataTrigger>
            </ActivityIndicator.Triggers>
        </ActivityIndicator>
        <Grid Grid.Row="1"
              Margin="0,10"
              ColumnDefinitions="0.5*,0.5*"
              BindingContext="{x:Reference video}">
            <Button Text="&#x25B6;&#xFE0F; Play"
                    HorizontalOptions="Center"
                    Clicked="OnPlayPauseButtonClicked">
                <Button.Triggers>
                    <DataTrigger TargetType="Button"
                                 Binding="{Binding Status}"
                                 Value="{x:Static controls:VideoStatus.Playing}">
                        <Setter Property="Text"
                                Value="&#x23F8; Pause" />
                    </DataTrigger>
                    <DataTrigger TargetType="Button"
                                 Binding="{Binding Status}"
                                 Value="{x:Static controls:VideoStatus.NotReady}">
                        <Setter Property="IsEnabled"
                                Value="False" />
                    </DataTrigger>
                </Button.Triggers>
            </Button>
            <Button Grid.Column="1"
                    Text="&#x23F9; Stop"
                    HorizontalOptions="Center"
                    Clicked="OnStopButtonClicked">
                <Button.Triggers>
                    <DataTrigger TargetType="Button"
                                 Binding="{Binding Status}"
                                 Value="{x:Static controls:VideoStatus.NotReady}">
                        <Setter Property="IsEnabled"
                                Value="False" />
                    </DataTrigger>
                </Button.Triggers>
            </Button>
        </Grid>
    </Grid>
</ContentPage>
```

In this example, the `Video` control sets the `AreTransportControlsEnabled` property to `false` and defines a <xref:Microsoft.Maui.Controls.Button> that plays and pauses the video, and a <xref:Microsoft.Maui.Controls.Button> that stop video playback. Button appearance is defined using unicode characters and their text equivalents, to create buttons that consist of an icon and text:

:::image type="content" source="media/create/play-stop.png" alt-text="Screenshot of play and pause buttons.":::

When the video is playing, the play button is updated to a pause button:

:::image type="content" source="media/create/pause-stop.png" alt-text="Screenshot of pause and stop buttons.":::

The UI also includes an <xref:Microsoft.Maui.Controls.ActivityIndicator> that's displayed while the video is loading. Data triggers are used to enable and disable the <xref:Microsoft.Maui.Controls.ActivityIndicator> and the buttons, and to switch the first button between play and pause. For more information about data triggers, see [Data triggers](~/fundamentals/triggers.md#data-triggers).

The code-behind file defines the event handlers for the button `Clicked` events:

```csharp
public partial class CustomTransportPage : ContentPage
{
    ...
    void OnPlayPauseButtonClicked(object sender, EventArgs args)
    {
        if (video.Status == VideoStatus.Playing)
        {
            video.Pause();
        }
        else if (video.Status == VideoStatus.Paused)
        {
            video.Play();
        }
    }

    void OnStopButtonClicked(object sender, EventArgs args)
    {
        video.Stop();
    }
    ...
}
```

### Custom positioning bar

The following example shows a custom positioning bar, `PositionSlider`, being consumed in XAML:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             x:Class="VideoDemos.Views.CustomPositionBarPage"
             Unloaded="OnContentPageUnloaded"
             Title="Custom position bar">
    <Grid RowDefinitions="*,Auto,Auto">
        <controls:Video x:Name="video"
                        AreTransportControlsEnabled="False"
                        Source="{StaticResource ElephantsDream}" />
        ...
        <Grid Grid.Row="1"
              Margin="10,0"
              ColumnDefinitions="0.25*,0.25*,0.25*,0.25*"
              BindingContext="{x:Reference video}">
            <Label Text="{Binding Path=Position,
                                  StringFormat='{0:hh\\:mm\\:ss}'}"
                   HorizontalOptions="Center"
                   VerticalOptions="Center" />
            ...
            <Label Grid.Column="3"
                   Text="{Binding Path=TimeToEnd,
                                  StringFormat='{0:hh\\:mm\\:ss}'}"
                   HorizontalOptions="Center"
                   VerticalOptions="Center" />
        </Grid>
        <controls:PositionSlider Grid.Row="2"
                                 Margin="10,0,10,10"
                                 BindingContext="{x:Reference video}"
                                 Duration="{Binding Duration}"
                                 Position="{Binding Position}">
            <controls:PositionSlider.Triggers>
                <DataTrigger TargetType="controls:PositionSlider"
                             Binding="{Binding Status}"
                             Value="{x:Static controls:VideoStatus.NotReady}">
                    <Setter Property="IsEnabled"
                            Value="False" />
                </DataTrigger>
            </controls:PositionSlider.Triggers>
        </controls:PositionSlider>
    </Grid>
</ContentPage>
```

The `Position` property of the `Video` object is bound to the `Position` property of the `PositionSlider`, without performance issues, because the `Video.Position` property is changed by the `MauiVideoPlayer.UpdateStatus` method on each platform, which is only called 10 times a second. In addition, two <xref:Microsoft.Maui.Controls.Label> objects display the `Position` and `TimeToEnd` properties values from the `Video` object.

::: moniker range="=net-maui-8.0"

### Native view cleanup

Each platform's handler implementation overrides the <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> implementation, which is used to perform native view cleanup such as unsubscribing from events and disposing objects. However, this override is intentionally not invoked by .NET MAUI. Instead, you must invoke it yourself from a suitable location in your app's lifecycle. This will often be when the page containing the `Video` control is navigated away from, which causes the page's `Unloaded` event to be raised.

An event handler for the page's `Unloaded` event can be registered in XAML:

```xaml
<ContentPage ...
             xmlns:controls="clr-namespace:VideoDemos.Controls"
             Unloaded="OnContentPageUnloaded">
    <controls:Video x:Name="video"
                    ... />
</ContentPage>
```

The event handler for the `Unloaded` event can then invoke the <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> method on its `Handler` instance:

```csharp
void OnContentPageUnloaded(object sender, EventArgs e)
{
    video.Handler?.DisconnectHandler();
}
```

In addition to cleaning up native view resources, invoking the handler's <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> method also ensures that videos stop playing on backwards navigation on iOS.

::: moniker-end

::: moniker range=">=net-maui-9.0"

## Handler disconnection

Each platform's handler implementation overrides the <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> implementation, which is used to perform native view cleanup such as unsubscribing from events and disposing objects. Handlers then automatically disconnect from their controls when possible, such as when navigating backwards in an app.

In some scenarios you might want to control when a handler disconnects from its control, which can be achieved with the [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property. This property requires a <xref:Microsoft.Maui.HandlerDisconnectPolicy> argument, with the <xref:Microsoft.Maui.HandlerDisconnectPolicy> enumeration defining the following values:

- `Automatic`, which indicates that a handler will be disconnected automatically. This is the default value of the [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property.
- `Manual`, which indicates that a handler will have to be disconnected manually by invoking the <xref:Microsoft.Maui.IElementHandler.DisconnectHandler> implementation.

The following example shows setting the [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property:

```xaml
<controls:Video x:Name="video"
                HandlerProperties.DisconnectPolicy="Manual"
                Source="video.mp4"
                AutoPlay="False" />
```

The equivalent C# code is:

```csharp
Video video = new Video
{
    Source = "video.mp4",
    AutoPlay = false
};
HandlerProperties.SetDisconnectPolicy(video, HandlerDisconnectPolicy.Manual);
```

When setting the [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property to `Manual` you must then invoke the handler's <xref:Microsoft.Maui.Handlers.ViewHandler`2.DisconnectHandler%2A> implementation yourself, from a suitable location in your app's lifecycle.

In addition, there's a <xref:Microsoft.Maui.ViewExtensions.DisconnectHandlers%2A> extension method that disconnects handlers from a given <xref:Microsoft.Maui.IView>:

```csharp
video.DisconnectHandlers();
```

When disconnecting, the <xref:Microsoft.Maui.ViewExtensions.DisconnectHandlers%2A> method will propagate down the control tree until it completes or arrives at a control that has set a manual policy.

::: moniker-end
