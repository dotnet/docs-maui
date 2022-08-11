---
title: "Create platform controls"
description: ""
ms.date: 08/02/2022
---

https://github.com/xamarin/docs-archive/blob/master/Docs/Video%20Player/player-creation.md

# Create platform controls

.NET Multi-platform App UI (.NET MAUI)

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
