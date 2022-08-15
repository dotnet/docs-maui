---
title: "Play video"
description: "Learn how to play web, resource, and file-based videos in a custom .NET MAUI video control."
ms.date: 08/02/2022
---

# Play video

<!-- sample link goes here -->

The `Video` class defines a `Source` property that's used to specify the source of the video file, as well as an `AutoPlay` property. `AutoPlay` defaults to `true`, which means that the video should begin playing automatically after `Source` has been set.

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class Video : View, IVideo
    {
        ...
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(Video), null);

        public static readonly BindableProperty AutoPlayProperty =
            BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(Video), true);

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

The type converter is invoked when the `Source` property is set to a string in XAML. The `ConvertFromInvariantString` method attempts to convert the string to a `Uri` object. If it succeeds, and the scheme is not `file`, then the method returns a `UriVideoSource`. Otherwise it returns a `ResourceVideoSource`.

## Play web video

Videos can be played from a URL source, as shown in the following example:

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

### URI video sources

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
public static void MapSource(IVideoHandler handler, IVideo video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

#### Android

Video is played on Android with a `VideoView`. The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `UriVideoSource`:

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

When processing objects of type `UriVideoSource`, the `SetVideoUri` method of `VideoView` is used to specify the video with an Android `Uri` object created from the string URI.

The `AutoPlay` property has no equivalent on `VideoView`, so the `Start` method is called if a new video has been set.

#### iOS and Mac Catalyst

To play a video on iOS and Mac Catalyst, an object of type `AVAsset` is created to encapsulate the video, and that is used to create an `AVPlayerItem`, which is then handed off to the `AVPlayer` object. The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `UriVideoSource`:

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

When processing objects of type `UriVideoSource`, the static `AVAsset.FromUrl` method is used to specify the video with an iOS `NSUrl` object created from the string URI.

The `AutoPlay` property has no equivalent in the iOS video classes, so the property is examined at the end of the `UpdateSource` method to call the `Play` method on the `AVPlayer` object.

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWIthPlayerItem` is set to `null` in the `Dispose` override:

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

## Play a video resource

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

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, and cleanup resources used by the native views, ensure that the `DisconnectHandler` method is called when the page is navigated away from:

```csharp
void OnContentPageUnloaded(object sender, EventArgs e)
{
    video.Handler?.DisconnectHandler();
}
```

### Resource video sources

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
public static void MapSource(IVideoHandler handler, IVideo video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

### Android

The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `ResourceVideoSource`:

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

When processing objects of type `ResourceVideoSource`, the `SetVideoPath` method of `VideoView` is called with a string argument that combines the app's package name with the video's filename.

The video file is stored in the package's *assets* folder, and requires a content provider to access it. The following code example shows the `VideoProvider` class, which creates an `AssetFileDescriptor` object that provides access to the video file:

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

### iOS and Mac Catalyst

The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `ResourceVideoSource`:

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

When processing objects of type `FileVideoSource`, the `GetUrlForResource` method of `NSBundle` is used to retrieve the file from the app package. The complete path must be divided into a filename, extension, and directory.

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, the `ReplaceCurrentItemWIthPlayerItem` is set to `null` in the `Dispose` override:

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

## Play a video file from the device's library

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

When the `Button` is tapped its `Clicked` event handler is executed, which is shown in the following code example:

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

In some cases on iOS, videos continue playing after the video playback page has been navigated away from. To stop the video, and cleanup resources used by the native views, ensure that the `DisconnectHandler` method is called when the page is navigated away from:

```csharp
void OnContentPageUnloaded(object sender, EventArgs e)
{
    video.Handler?.DisconnectHandler();
}
```

### File video source

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
public static void MapSource(IVideoHandler handler, IVideo video)
{
    handler?.PlatformView.UpdateSource();
}
```

The `MapSource` method in turns calls the `UpdateSource` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

### Android

The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `FileVideoSource`:

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

When processing objects of type `FileVideoSource`, the `SetVideoPath` method of `VideoView` is used to specify the file on the device.

### iOS and Mac Catalyst

The following code example shows how the `UpdateSource` method handles the `Source` property when it's of type `FileVideoSource`:

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
        ...

        public void UpdateSource()
        {
            AVAsset asset = null;
            ...

            else if (_video.Source is FileVideoSource)
            {
                string uri = (_video.Source as FileVideoSource).File;
                if (!string.IsNullOrWhiteSpace(uri))
                    asset = AVAsset.FromUrl(new NSUrl(uri));
            }
            ...
        }
        ...
    }
}
```

When processing objects of type `FileVideoSource`, the static `AVAsset.FromUrl` method is used to specify the video with an iOS `NSUrl` object created from the string URI.
