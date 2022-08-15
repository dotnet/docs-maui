---
title: "Create platform controls"
description: ""
ms.date: 08/02/2022
---


# Create platform controls

<!-- sample link goes here -->

.NET Multi-platform App UI (.NET MAUI)

After creating the property mapper and optional command mapper for your handler, you must provide handler implementations on all platforms. This can be accomplished by adding partial class handler implementations in the child folders of the *Platforms* folder. Alternatively you could configure your project to support filename-based multi-targeting, or folder-based multi-targeting, or both. For more information, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

The sample application is configured to support filename-based multi-targeting, so that the handler classes all are located in a single folder:

:::image type="content" source="media/create-platform-controls/handlers-folder.png" alt-text="Screenshot of the files in the Handlers folder of the project.":::

The `VideoHandler` class containing the mappers is named *VideoHandler.cs*. Then its platform implementations are in the *VideoHandler.Android.cs*, *VideoHandler.iOS.cs*, and *VideoHandler.Windows.cs* files.

Each platform handler class should be a partial class and derive from the generic `ViewHandler` class, that requires two type arguments:

- The interface for the cross-platform control, that implements `IView`.
- The type of the native view that implements cross-platform control on this platform. This should be identical to the type of the `PlatformView` property in the handler interface.

Each of the platform handler implementations should override the following methods:

- `CreatePlatformView`, which should create and return the native view that implements the cross-platform control.
- `ConnectHandler`, which should perform any native view setup, such as initializing the native view and performing event subscriptions.
- `DisconnectHandler`, which should perform any native view cleanup, such as unsubscribing from events and disposing objects.

> [!IMPORTANT]
> The `DisconnectHandler` method is intentionally not invoked by .NET MAUI. Instead, you must invoke it yourself from a suitable location in your app's lifecycle. For more information, see []().

In addition, each platform handler implementation should implement the Actions that are defined in the property mapper and command mapper dictionaries.

Each platform handler implementation should also provide additional code, as required, to implement the functionality of the cross-platform control on the platform. Alternatively, this can be provided by an additional type, which is the approach adopted here.

### Android

Video is played on Android with a `VideoView`. However, here, the `VideoView` has been encapsulated in a `MauiVideoPlayer` type to keep the native view separated from its handler. The following example shows the `VideoHandler` partial class for Android, with its three overrides:

```csharp
using Microsoft.Maui.Handlers;
using VideoDemos.Controls;
using VideoDemos.Platforms.Android;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
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

`VideoHandler` derives from the `ViewHandler` class, with the generic `IVideo` argument specifying the interface for the cross-platform control, and the `MauiVideoPlayer` argument specifying the type of the native view that encapsulates the `VideoView` control.

The `CreatePlatformView` override creates and returns a `MauiVideoPlayer` object. The `ConnectHandler` override is the location to perform any required native view setup instance. The `DisconnectHandler` override is the location to perform any native view cleanup, and calls the `Dispose` method of the `MauiVideoPlayer` instance.

The platform handler also has to implement the Actions defined in the property mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
{
    ...
    public static void MapAreTransportControlsEnabled(IVideoHandler handler, IVideo video)
    {
        handler.PlatformView?.UpdateTransportControlsEnabled();
    }

    public static void MapSource(IVideoHandler handler, IVideo video)
    {
        handler.PlatformView?.UpdateSource();
    }

    public static void MapPosition(IVideoHandler handler, IVideo video)
    {
        handler.PlatformView?.UpdatePosition();
    }
    ...
}
```

Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires the handler interface and cross-platform control interface as arguments. In each case, each Action calls a method defined in the `MauiVideoPlayer` type.

The platform handler also has to implement the Actions defined in the command mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
{
    ...
    public static void MapUpdateStatus(IVideoHandler handler, IVideo video, object? args)
    {
        handler.PlatformView?.UpdateStatus();
    }

    public static void MapPlayRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PlayRequested(position);
    }

    public static void MapPauseRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PauseRequested(position);
    }

    public static void MapStopRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.StopRequested(position);
    }
    ...
}
```

Each Action is executed in response to a command being sent from the cross-platform control, and is a `static` method that requires the handler interface, cross-platform control interface, and optional data as arguments. In each case, each Action calls a method defined in the `MauiVideoPlayer` class, after extracting the optional data.

On Android, the `MauiVideoPlayer` class encapsulates the `VideoView` to keep the native video control separated from its handler:

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
        MediaController _mediaController;    // Used to display transport controls
        bool _isPrepared;
        Context _context;
        IVideo _video;

        public MauiVideoPlayer(Context context, IVideo video) : base(context)
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

            // Create a ViedoView and position it in the RelativeLayout
            _videoView = new VideoView(context)
            {
                LayoutParameters = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            };

            // Add the views to the layouts
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
    IVideo _video;
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
> The `Dispose` override is called by the handler's `DisconnectHandler` override.

The platform transport controls include buttons that play, pause, and stop the video, and are provided by Android's `MediaController` type. Provided that the `AreTransportControlsEnabled` property of the `Video` control is set to `true`, a `MediaController` is set as the media player of the `VideoView`. This occurs because when the `AreTransportControlsEnabled` property is set, the handler's property mapper ensures that the `MapAreTransportControlsEnabled` method is invoked, which in turn calls the `UpdateTransportControlsEnabled` method in `MauiVideoPlayer`:

```csharp
public class MauiVideoPlayer : CoordinatorLayout
{
    VideoView _videoView;
    MediaController _mediaController;    // Used to display transport controls
    IVideo _video;
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

If the `AreTransportControlsEnabled` property of the `Video` control is set to `false`, the `MediaController` is removed as the media player of the `VideoView`. In this scenario, you can then control video playback programmatically or supply your own transport controls. For more information, see []().

### iOS

Video is played on iOS and Mac Catalyst with an `AVPlayer` and an `AVPlayerViewController`. However, here, these types are encapsulated in a `MauiVideoPlayer` type to keep the native views separated from their handler. The following example shows the `VideoHandler` partial class for iOS, with its three overrides:

```csharp
using Microsoft.Maui.Handlers;
using VideoDemos.Controls;
using VideoDemos.Platforms.MaciOS;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
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

`VideoHandler` derives from the `ViewHandler` class, with the generic `IVideo` argument specifying the interface for the cross-platform control, and the `MauiVideoPlayer` argument specifying the type of the native view that encapsulates the `AVPlayer` and `AVPlayerViewController` controls.

The `CreatePlatformView` override creates and returns a `MauiVideoPlayer` object. The `ConnectHandler` override is the location to perform any required native view setup instance. The `DisconnectHandler` override is the location to perform any native view cleanup, and calls the `Dispose` method of the `MauiVideoPlayer` instance.

The platform handler also has to implement the Actions defined in the property mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
{
    ...
    public static void MapAreTransportControlsEnabled(IVideoHandler handler, IVideo video)
    {
        handler?.PlatformView.UpdateTransportControlsEnabled();
    }

    public static void MapSource(IVideoHandler handler, IVideo video)
    {
        handler?.PlatformView.UpdateSource();
    }

    public static void MapPosition(IVideoHandler handler, IVideo video)
    {
        handler?.PlatformView.UpdatePosition();
    }
    ...
}
```

Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires the handler interface and cross-platform control interface as arguments. In each case, each Action calls a method defined in the `MauiVideoPlayer` type.

The platform handler also has to implement the Actions defined in the command mapper dictionary:

```csharp
public partial class VideoHandler : ViewHandler<IVideo, MauiVideoPlayer>
{
    ...
    public static void MapUpdateStatus(IVideoHandler handler, IVideo video, object? args)
    {
        handler.PlatformView?.UpdateStatus();
    }

    public static void MapPlayRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PlayRequested(position);
    }

    public static void MapPauseRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.PauseRequested(position);
    }

    public static void MapStopRequested(IVideoHandler handler, IVideo video, object? args)
    {
        if (args is not VideoPositionEventArgs)
            return;

        TimeSpan position = ((VideoPositionEventArgs)args).Position;
        handler.PlatformView?.StopRequested(position);
    }
    ...
}
```

Each Action is executed in response to a command being sent from the cross-platform control, and is a `static` method that requires the handler interface, cross-platform control interface, and optional data as arguments. In each case, each Action calls a method defined in the `MauiVideoPlayer` class, after extracting the optional data.

On iOS and Mac Catalyst, the `MauiVideoPlayer` class encapsulates the `AVPlayer` and `AVPlayerViewController` types to keep the native video controls separated from their handler:

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
        IVideo _video;
        ...

        public MauiVideoPlayer(IVideo video)
        {
            _video = video;

            // Create AVPlayerViewController
            _playerViewController = new AVPlayerViewController();

            // Set Player property to AVPlayer
            _player = new AVPlayer();
            _playerViewController.Player = _player;

            // Use the View from the controller as the native control
            _playerViewController.View.Frame = this.Bounds;
            AddSubview(_playerViewController.View);
        }
        ...
    }
}
```

`MauiVideoPlayer` derives from `UIView`, which is the base class for objects that display content and handler user interaction with that content. The constructor creates an `AVPlayer` object, which manages the playback and timing of a media file, and sets it as the `Player` property value of an `AVPlayerViewController`. The `AVPlayerViewController` displays content from the `AVPlayer` and presents transport controls and other features. The size and location of the control is then set which ensures that the video is centered in the page, and expands to fill the available space while maintaining its aspect ratio. The native control, which is the view from the `AVPlayerViewController` is then added to the page.

The `Dispose` method is responsible for performing native view cleanup:

```csharp
public class MauiVideoPlayer : UIView
{
    AVPlayer _player;
    AVPlayerViewController _playerViewController;
    IVideo _video;
    ...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_player != null)
            {
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

In some scenarios, videos continue playing after a video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWIthPlayerItem` is set to `null` in the `Dispose` override, and other native view cleanup is performed.

> [!NOTE]
> The `Dispose` override is called by the handler's `DisconnectHandler` override.

The platform transport controls include buttons that play, pause, and stop the video, and are provided by the `AVPlayerViewController` type. Provided that the `AreTransportControlsEnabled` property of the `Video` control is set to `true`, the `AVPlayerViewController` will display its playback controls. This occurs because when the `AreTransportControlsEnabled` property is set, the handler's property mapper ensures that the `MapAreTransportControlsEnabled` method is invoked, which in turn calls the `UpdateTransportControlsEnabled` method in `MauiVideoPlayer`:

```csharp
public class MauiVideoPlayer : UIView
{
    AVPlayerViewController _playerViewController;
    IVideo _video;
    ...

    public void UpdateTransportControlsEnabled()
    {
        _playerViewController.ShowsPlaybackControls = _video.AreTransportControlsEnabled;
    }
    ...
}
```

The transport controls fade out if they're not used but can be restored by tapping on the video.

If the `AreTransportControlsEnabled` property of the `Video` control is set to `false`, the `AVPlayerViewController` doesn't show its playback controls. In this scenario, you can then control video playback programmatically or supply your own transport controls. For more information, see []().

### Windows

WinUI 3 currently lacks a control capable of playing video. However, it's still necessary to provide a handler implementation on Windows that overrides the `CreatePlatformView` method and that provides methods for the Actions defined in the property mapper and command mapper dictionaries. In all cases, these methods can throw `PlatformNotSupportedException` exceptions:

```csharp
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using VideoDemos.Controls;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : ViewHandler<IVideo, FrameworkElement>
    {
        protected override FrameworkElement CreatePlatformView() => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapAreTransportControlsEnabled(IVideoHandler handler, IVideo video) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapSource(IVideoHandler handler, IVideo video) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapPosition(IVideoHandler handler, IVideo video) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapUpdateStatus(IVideoHandler handler, IVideo video, object? arg) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapPlayRequested(IVideoHandler handler, IVideo video, object? arg) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapPauseRequested(IVideoHandler handler, IVideo video, object? arg) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
        public static void MapStopRequested(IVideoHandler handler, IVideo video, object? arg) => throw new PlatformNotSupportedException("No MediaElement control on Windows.");
    }
}
```

A type must still be specified that represents the native view for the `Video` control, and this is provided here by the `FrameworkElement` class, which is a base type for WinUI controls.
