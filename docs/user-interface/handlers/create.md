---
title: "Create custom controls with .NET MAUI handlers"
description: ""
ms.date: 08/02/2022
---

# Create custom controls

.NET Multi-platform App UI (.NET MAUI)

A standard requirement for apps is the ability to play videos. This article discuses how to write a handler for Android, iOS, Mac Catalyst, and Windows for a .NET MAUI custom control named `Video`. This control can play video from three sources:

- A URL, which represents a remote video.
- A resource, which is a file embedded in the app.
- A file, from the device's video library.

Video controls require *transport controls*, which are buttons for playing and pausing the video, and a positioning bar that shows the progress through the video and allows the user to move quickly to a different location. `Video` can either use the transport controls and positioning bar provided by the platform, or you can supply custom transport controls and a positioning bar.

SCREENSHOT GOES HERE

A more sophisticated video control would have additional features, such as a volume control, a mechanism to interrupt video playback when a call is received, and a way of keeping the screen active during playback.

> [!NOTE]
> Unlike Android, iOS, and Mac Catalyst, WinUI 3 currently lacks a control capable of playing video. Therefore the Windows implementation of the `Video` control currently throws a `PlatformNotSupportedException.`

Explain that it's a video control implemented on everything but Windows.

The process for creating a cross-platform .NET MAUI custom control, whose platform implementations are provided by a handler is as follows:

1. Create an interface for your cross-platform control, that implements `IView`. For more information, see []().
1. Create a class for your custom cross-platform control, that derives from `View` and that implements your control interface. For more information, see []().
1. Optionally, create any required additional cross-platform types.
1. Create an interface for your handler, that implements `IViewHandler`. For more information, see []().
1. Create a `partial` handler class, that implements your handler interface. For more information, see []().
1. In your handler class, create the `PropertyMapper` dictionary, which defines the actions to take when cross-platform property changes occur. For more information, see []().
1. Optionally, in your handler class, create the `CommandMapper` dictionary, which defines the actions to take when the cross-platform control sends instructions to the native views that implement the control. For more information, see []().
1. Create `partial` handler classes for each platform, create the native views that implement the cross-platform control. For more information, see []().
1. Register the handler using the `ConfigureMauiHandlers` and `AddHandler` methods in your app's `MauiProgram` class. For more information, see [Register the handler](#register-the-handler).

Then, the cross-platform control can be consumed. For more information, see []().

## Cross-platform control interface

Before creating your custom cross-platform control type, you must first create its interface. This can be achieved by creating an interface that implements `IView`:

```csharp
public interface IVideo : IView
{
    bool AreTransportControlsEnabled { get; }
    VideoSource Source { get; }
    bool AutoPlay { get; }
    VideoStatus Status { get; }
    TimeSpan Duration { get; }
    TimeSpan Position { get; set; }
    TimeSpan TimeToEnd { get; }

    event EventHandler UpdateStatus;
    event EventHandler<VideoPositionEventArgs> PlayRequested;
    event EventHandler<VideoPositionEventArgs> PauseRequested;
    event EventHandler<VideoPositionEventArgs> StopRequested;
}
```

Within the interface, you should define the public API of your custom control that will be accessed by its handler.

> [!NOTE]
> Handlers are accessed through their control-specific interface. This avoids the cross-platform control having to reference its handler, and the handler having to reference the cross-platform control.

## Cross-platform control

After defining your custom control's interface, you should create the type for your custom control. This type should derive from `View`, and implement your control's interface:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
    {
        public static readonly BindableProperty AreTransportControlsEnabledProperty =
            BindableProperty.Create(nameof(AreTransportControlsEnabled), typeof(bool), typeof(Video), true);

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(Video), null);

        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(Video), true);

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
        ...
    }
}
```

## Handler interface

After creating the type for your custom control, you should create an interface for your handler. This can be achieved by creating an interface that implements `IViewHandler`:

```csharp
#if IOS || MACCATALYST
using PlatformView = VideoDemos.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = VideoDemos.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using VideoDemos.Controls;

namespace VideoDemos.Handlers
{
    public interface IVideoHandler : IViewHandler
    {
        new IVideo VirtualView { get; }
        new PlatformView PlatformView { get; }
    }
}
```

The interface should define read-only `VirtualView` and `PlatformView` properties. The `VirtualView` property, of type `IVideo`, is used to access the cross-platform control from its handler. The `PlatformView` property, of type `PlatformView`, is used to access the native view that implements the cross-platform control. The conditional `using` statements define the `PlatformView` type on each platform. On Android, iOS, and Mac Catalyst, the native views are provided by the custom `MauiVideoPlayer` class. On Windows, which currently lacks a video player control, there is no video player implementation. However, a native view must be specified for compilation purposes, and this is provided by the `FrameworkElement` class.

The final conditional `using` statement defines `PlatformView` to be equal to `System.Object`. This is necessary so that the `PlatformView` type can be used within the interface for usage across all platforms. The alternative would be to have to define the `PlatformView` property once per platform, using conditional compilation.

> [!NOTE]
> The `new` keyword on the `VirtualView` and `PlatformView` properties tells the compiler that the interface definition hides the definition contained in the interface being extended.

## Handler class

After creating an interface for your handler, you should create a `partial` type for your handler that implements its interface:

```csharp
#if __IOS__ || MACCATALYST
using PlatformView = VideoDemos.Platforms.MaciOS.MauiVideoPlayer;
#elif ANDROID
using PlatformView = VideoDemos.Platforms.Android.MauiVideoPlayer;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0 && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using VideoDemos.Controls;
using Microsoft.Maui.Handlers;

namespace VideoDemos.Handlers
{
    public partial class VideoHandler : IVideoHandler
    {
        IVideo IVideoHandler.VirtualView => VirtualView;
        PlatformView IVideoHandler.PlatformView => PlatformView;
    }
}
```

The handler class is a partial class whose implementation will be completed on each platform in an additional partial class. It implements the `VirtualView` and `PlatformView` properties that are defined in the interface, using [expression-bodied members](/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members) to return `VirtualView` and `PlatformView` properties that are defined in .NET MAUI's generic `ViewHandler` class. This will be discussed further in [Implement the handler per platform](#implement-the-handler-per-platform).

The conditional `using` statements are identical to those defined in the handler interface, and define the native view that implements the cross-platform control on each platform. As with the interface, the final conditional `using` statement defines `PlatformView` to be equal to `System.Object`. This is necessary so that the `PlatformView` type can be used within the class for usage across all platforms. The alternative would be to have to define the `PlatformView` property once per platform, using conditional compilation.

## Property mappers

Each handler typically provides a *property mapper*, which defines what actions to take when a property change occurs in the cross-platform control. `PropertyMapper` is a `Dictionary` that maps the cross-platform control's interface properties to their associated Actions.

`PropertyMapper` is defined in .NET MAUI's generic `ViewHandler` class, and requires two generic arguments to be supplied:

- The interface for the cross-platform control, that implements `IView`.
- The interface for the handler, that implements `IViewHandler`.

The following code example shows the `VideoHandler` class extended with the `PropertyMapper` definition:

```csharp
public partial class VideoHandler : IVideoHandler
{
    public static IPropertyMapper<IVideo, IVideoHandler> PropertyMapper = new PropertyMapper<IVideo, IVideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(IVideo.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(IVideo.Source)] = MapSource,
        ...
    };

    IVideo IVideoHandler.VirtualView => VirtualView;
    PlatformView IVideoHandler.PlatformView => PlatformView;

    public VideoHandler() : base(PropertyMapper)
    {
    }
}
```

The `PropertyMapper` is a `Dictionary` whose key is a `string` and whose value is a generic `Action`. The `string` represents the property name, accessed via it's cross-platform interface, and the `Action` represents a `static` method that requires the handler interface and cross-platform control interface as arguments. For example, the signature of the `MapSource` method is `public static void MapSource(IVideoHandler handler, IVideo video)`.

Each platform handler implementation will then provide implementations of the Actions, which manipulate the native view API. The overall effect is that when a property is set on a cross-platform control, the underlying native view will be updated as required. The advantage of this approach is that it enables native views to be decoupled from cross-platform controls, because the cross-platform control doesn't reference its handler, and the handler doesn't reference the cross-platform control. In addition, it allows for easy customisation because the property mapper can be modified by consumers without subclassing.

## Command mappers

Each handler can also provide a *command mapper*, which provide a way for cross-platform controls to send instructions to native views on each platform. They're similar to property mappers, but allow for additional data to be passed. In this context a command is an instruction, and optionally its data, that's sent to a native view. `CommandMapper` is a `Dictionary` that maps cross-platform control interface members to their associated Actions.

`CommandMapper` is defined in .NET MAUI's generic `ViewHandler` class, and requires two generic arguments to be supplied:

- The interface for the cross-platform control, that implements `IView`.
- The interface for the handler, that implements `IViewHandler`.

The following code example shows the `VideoHandler` class extended with the `CommandMapper` definition:

```csharp
public partial class VideoHandler : IVideoHandler
{
    public static IPropertyMapper<IVideo, IVideoHandler> PropertyMapper = new PropertyMapper<IVideo, IVideoHandler>(ViewHandler.ViewMapper)
    {
        [nameof(IVideo.AreTransportControlsEnabled)] = MapAreTransportControlsEnabled,
        [nameof(IVideo.Source)] = MapSource,
        ...
    };

    public static CommandMapper<IVideo, IVideoHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(IVideo.UpdateStatus)] = MapUpdateStatus,
        [nameof(IVideo.PlayRequested)] = MapPlayRequested,
        ...
    };

    IVideo IVideoHandler.VirtualView => VirtualView;
    PlatformView IVideoHandler.PlatformView => PlatformView;

    public VideoHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}
```

The `CommandMapper` is a `Dictionary` whose key is a `string` and whose value is a generic `Action`. The `string` represents the command name, accessed via it's cross-platform interface, and the `Action` represents a `static` method that requires the handler interface, cross-platform control interface, and optional data as arguments. For example, the signature of the `MapPlayRequested` method is `public static void MapPlayRequested(IVideoHandler handler, IVideo video, object? args)`.

Each platform handler implementation will then provide implementations of the Actions, which manipulate the native view API. The overall effect is that when a command is sent from the cross-platform control, the underlying native view will be manipulated as required. The advantage of this approach is that it enables native views to be decoupled from cross-platform controls, because the cross-platform control can send a command to its native views without referencing its handler, and the handler doesn't reference the cross-platform control. This removes the need for native views to subscribe to cross-platform control events. In addition, it allows for easy customisation because the command mapper can be modified by consumers without subclassing.

## Implement the handler per platform

Implement `CreatePlatformView`, `ConnectHandler`, `DisconnectHandler` and the actions defined in the property mapper and command mapper.

### Android

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
    }
}
```

On Android, the `VideoView` class provides video playback functionality. However,

On Android, the `MauiVideoPlayer` class implements the video control, which derives from `CoordinatorLayout`. The ability to play video is provided by the `AVPlayerViewController` and `AVPlayer` types:


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
            else if (_video.Source is FileVideoSource)
            {
                string filename = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    _videoView.SetVideoPath(filename);
                    hasSetSource = true;
                }
            }
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

            if (hasSetSource && _video.AutoPlay)
            {
                _videoView.Start();
            }
        }

        public void UpdatePosition()
        {
            if (Math.Abs(_videoView.CurrentPosition - _video.Position.TotalMilliseconds) > 1000)
            {
                _videoView.SeekTo((int)_video.Position.TotalMilliseconds);
            }
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
            {
                status = _videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;
            }

            ((IVideoController)_video).Status = status;

            // Set Position property
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(_videoView.CurrentPosition);
            _video.Position = timeSpan;
        }

        public void PlayRequested(TimeSpan position)
        {
            _videoView.Start();
            System.Diagnostics.Debug.WriteLine($"Video playback from {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
        }

        public void PauseRequested(TimeSpan position)
        {
            _videoView.Pause();
            System.Diagnostics.Debug.WriteLine($"Video paused at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
        }

        public void StopRequested(TimeSpan position)
        {
            // Stops and releases the media player
            _videoView.StopPlayback();
            System.Diagnostics.Debug.WriteLine($"Video stopped at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");

            // Ensure the video can be played again
            _videoView.Resume();
        }
    }
}
```

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

        public override int Delete(Uri uri, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }

        public override string GetType(Uri uri)
        {
            throw new NotImplementedException();
        }

        public override Uri Insert(Uri uri, ContentValues values)
        {
            throw new NotImplementedException();
        }

        public override bool OnCreate()
        {
            return false;
        }

        public override ICursor Query(Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public override int Update(Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }
    }
}
```

### iOS

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
    }
}
```

On iOS, the `MauiVideoPlayer` class implements the video control, which derives from `UIView`. The ability to play video is provided by the `AVPlayerViewController` and `AVPlayer` types:

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
        AVPlayerViewController _playerViewController;
        IVideo _video;

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

        public void UpdateTransportControlsEnabled()
        {
            _playerViewController.ShowsPlaybackControls = _video.AreTransportControlsEnabled;
        }

        public void UpdateSource()
        {
            AVAsset asset = null;

            if (_video.Source is UriVideoSource)
            {
                string uri = (_video.Source as UriVideoSource).Uri;
                if (!string.IsNullOrWhiteSpace(uri))
                    asset = AVAsset.FromUrl(new NSUrl(uri));
            }
            else if (_video.Source is FileVideoSource)
            {
                string uri = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(uri))
                    asset = AVAsset.FromUrl(new NSUrl(uri));
            }
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

        public void UpdatePosition()
        {
            TimeSpan controlPosition = ConvertTime(_player.CurrentTime);
            if (Math.Abs((controlPosition - _video.Position).TotalSeconds) > 1)
            {
                _player.Seek(CMTime.FromSeconds(_video.Position.TotalSeconds, 1));
            }
        }

        TimeSpan ConvertTime(CMTime cmTime)
        {
            return TimeSpan.FromSeconds(Double.IsNaN(cmTime.Seconds) ? 0 : cmTime.Seconds);
        }

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

            if (_playerItem != null)
            {
                ((IVideoController)_video).Duration = ConvertTime(_playerItem.Duration);
                _video.Position = ConvertTime(_playerItem.CurrentTime);
            }
        }

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



### Windows

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

----

https://github.com/dotnet/maui/wiki/Porting-Custom-Renderers-to-Handlers

Each handler class exposes the native view that implements the cross-platform view via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform view implemented by the handler is exposed via its `VirtualView` property.
