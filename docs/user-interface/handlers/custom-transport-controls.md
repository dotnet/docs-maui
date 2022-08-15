---
title: "Custom transport controls and positioning bar"
description: "Learn how to implement custom transport controls in a video control, using .NET MAUI."
ms.date: 08/02/2022
---

# Custom transport controls

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-createhandler)

The transport controls of a video player include buttons that play, pause, and stop the video. These buttons are often identified with familiar icons rather than text, and the play and pause buttons are often combined into one button.

By default, the `Video` control displays transport controls supported by each platform. However, when you set the `AreTransportControlsEnabled` property to `false`, these controls are suppressed. You can then control the `Video` control programmatically or supply your own transport controls.

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

In this example, the `Video` control sets the `AreTransportControlsEnabled` property to `false` and defines a `Button` that plays and pauses the video, and a `Button` that stop video playback. Button appearance is defined using unicode characters and their text equivalents, to create buttons that consist of an icon and text:

:::image type="content" source="media/custom-transport-controls/play-stop.png" alt-text="Screenshot of play and pause buttons.":::

When the video is playing, the play button is updated to a pause button:

:::image type="content" source="media/custom-transport-controls/pause-stop.png" alt-text="Screenshot of pause and stop buttons.:::

The UI also includes an `ActivityIndicator` that's displayed while the video is loading. Data triggers are used to enable and disable the `ActivityIndicator` and the buttons, and to switch the first button between play and pause. For more information about data triggers, see [Data triggers](~/fundamentals/triggers.md#data-triggers).

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

Implementing your own transport controls requires the `Video` class to be able to notify it's native views to play, pause, or stop the video, and know the current status of video playback.

## Transport controls

The `Video` class defines three methods named `Play`, `Pause`, and `Stop` that raise a corresponding event, and send a command to the `VideoHandler`:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
    {
        ...
        public event EventHandler<VideoPositionEventArgs> PlayRequested;
        public event EventHandler<VideoPositionEventArgs> PauseRequested;
        public event EventHandler<VideoPositionEventArgs> StopRequested;

        public void Play()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            PlayRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(IVideo.PlayRequested), args);
        }

        public void Pause()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            PauseRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(IVideo.PauseRequested), args);
        }

        public void Stop()
        {
            VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
            StopRequested?.Invoke(this, args);
            Handler?.Invoke(nameof(IVideo.StopRequested), args);
        }
    }
}
```

`VideoPositionEventArgs` simply defines a `Position` property that can be set through its constructor.

The final line in the `Play`, `Pause`, and `Stop` methods sends a command and associated data to the `VideoHandler`. The `CommandMapper` for `VideoHandler` maps command names to Actions that are executed when a command is received. For example, when `VideoHandler` receives the `PlayRequested` command, it executes its `MapPlayRequested` method. The advantage of this approach is that it enables native views to be decoupled from cross-platform controls, because the cross-platform control can send a command to its native views without referencing its handler, and the handler doesn't reference the cross-platform control. This removes the need for native views to subscribe to cross-platform control events. In addition, it allows for easy customisation because the command mapper can be modified by consumers without subclassing. For more information about `CommandMapper`, see [Command mappers](create-handler.md#command-mappers).

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

Each of the three methods also logs the position at which the video was played, pause, or stopped using the data that's sent with the command.

Therefore, the overall mechanism is when the `Play`, `Pause`, or `Stop` method is invoked on the `Video` control, it's native view is instructed to play, pause, or stop, the video and log the position at which the video was played, paused, or stopped. This all happens using a decoupled approach, without native views having to subscribe to cross-platform events.

## Video status

Implementing play, pause, and stop functionality is not sufficient for supporting custom transport controls. Often the play and pause functionality should be implemented with the same button, which changes its appearance to indicate whether the video is currently playing or pause. In addition, the button shouldn't even be enabled if the video has not yet loaded.

These requirements imply that that video player needs to make available a current status indicating if it's playing or paused, or if it's not yet ready to play a video. This can represented by an enumeration:

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
    public class Video : View, IVideo, IVideoController
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

Usually, a read-only bindable property would have a private `set` accessor on the `Status` property to allow it to be set from within the class. However, for a `View` derivative supported by handlers, the property must be set from outside the class but only by the control's handler.

For this reason, another property is defined with the name `IVideoController.Status`. This is an explicit interface implementation, and is made possible by the `IVideoController` interface that the `Video` class implements:

```csharp
public interface IVideoController
{
    VideoStatus Status { get; set; }
    TimeSpan Duration { get; set; }
}
```

This interface makes it possible for a class external to `Video` to set the `Status` property by referencing the `IVideoController` interface. Most importantly, the `Status` property can't be set through a data binding.

> [!NOTE]
> The `Status` property could be set from other classes as well, but it's unlikely to be set inadvertently.

To assist the handlers in keeping the `Status` property updated, the `Video` class defines an `UpdateStatus` event and command:

```csharp
using System.ComponentModel;

namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
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
            Handler?.Invoke(nameof(IVideo.UpdateStatus));
        }
        ...
    }
}
```

The `OnTimerTick` event handler is executed every tenth of a second, which raises the `UpdateStatus` event and invokes the `UpdateStatus` command.

When the `UpdateStatus` command is sent from the `Video` control to it's handler, the handler's command mapper ensures that the `MapUpdateStatus` method is invoked:

```csharp
public static void MapUpdateStatus(IVideoHandler handler, IVideo video, object? args)
{
    handler.PlatformView?.UpdateStatus();
}
```

The `MapUpdateStatus` method in turns calls the `UpdateStatus` method on the handler's `PlatformView` property. The `PlatformView` property, which is of type `MauiVideoPlayer`, represents the native view that provides the video player implementation on each platform.

### Android

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
        IVideo _video;
        ...

        public MauiVideoPlayer(Context context, IVideo video) : base(context)
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

The `IsPlayingProperty` of the `VideoView` is a boolean that indicates if the video is playing or paused. To determine if the `VideoView` can neither play nor pause the video, it's `Prepared` event must be handled. This event is raised when the media source is ready for playback. The event is subscribed to in the `MauiVideoPlayer` constructor, and unsubscribed from in its `Dispose` override. The `UpdateStatus` method then uses the `isPrepared` field and the `VideoView.IsPlaying` property to set the `Status` property on the `IVideo` object by casting it to `IVideoController`.

### iOS and Mac Catalyst

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
        IVideo _video;
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

Two properties of `AVPlayer` must be accessed to set the `Status` property - the `Status` property of type `AVPlayerStatus` and the `TimeControlStatus` property of type `AVPlayerTimeControlStatus`. The `Status` property can then be set on the `IVideo` object by casting it to `IVideoController`.

## Positioning bar

The transport controls implemented by each platform include a position bar. This bar resembles a slider or scroll bar, and shows the current location of the video within its total duration. In addition, the user can manipulate the position bar to move forwards or backwards to a new position in the video.

Implementing your own position bar requires the `Video` class to know the duration of the video, and its current position within that duration.

### Duration

One item of information that the `Video` control needs to support a custom position bar is the duration of the video. The `Video` class defines a read-only bindable property named `Duration`, of type `TimeSpan`. This property is defined as read-only because it should only be set from the control's handler:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
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

Usually, a read-only bindable property would have a private `set` accessor on the `Duration` property to allow it to be set from within the class. However, for a `View` derivative supported by handlers, the property must be set from outside the class but only by the control's handler.

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

This interface makes it possible for a class external to `Video` to set the `Duration` property by referencing the `IVideoController` interface. Most importantly, the `Duration` property can't be set through a data binding.

> [!NOTE]
> The `Duration` property could be set from other classes as well, but it's unlikely to be set inadvertently.

The duration of a video isn't available immediately after the `Source` property of the `Video` control is set. The video must be partially downloaded before the native view can determine it's duration.

#### Android

On Android, the `VideoView.Duration` property reports a valid duration in milliseconds when the `VideoView.Prepared` event is raised. The `MauiVideoPlayer` class uses the `Prepared` event handler to obtain the `Duration` property value:

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
        IVideo _video;
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

#### iOS and Mac Catalyst

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

### Position

The `Video` control also needs a `Position` property that increases from zero to `Duration` as the video plays. The `Video` class implements this property as a bindable property with public `get` and `set` accessors:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
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

The `get` accessor returns the current position of the video as its playing. The `set` accessor responds to user manipulation of the position bar by moving the video position forwards or backwards.

> [!NOTE]
> The property-changed event handler for the `Position` bindable property calls a method named `SetTimeToEnd`, which is described in [Calculating time to end](#calculating-time-to-end).

On Android and iOS the property that obtains the current position only has a `get` accessor. Instead, a `Seek` method is available to set the position. This seems to be a more sensible approach than using a single `Position` property, which has an inherent problem. As a video plays, a `Position` property must be continually updated to reflect the new position. But you don't want most changes of the `Position` property to cause the video player to move to a new position in the video. If that happens, the video player would respond by seeking to the last value of the `Position` property, and the video wouldn't advance.

Despite the difficulties of implementing a `Position` property with `get` and `set` accessors, this approach is used because it can utilize data binding. The `Position` property of the `Video` control can be bound to a `Slider` that's used both to display the position and to seek a new position. However, several precautions are necessary when implementing the `Position` property to avoid feedback loops.

#### Android

On Android, the `VideoView.CurrentPosition` property indicates the current position of the video. The `MauiVideoPlayer` class sets the `Position` property in the `UpdateStatus` method at the sam time as it sets the `Duration` property:

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
        IVideo _video;
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

#### iOS and Mac Catalyst

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
        IVideo _video;
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

### Calculating time to end

Sometimes video players show the time remaining in the video. This value begins at the video's duration when the video begins, and decreases down to zero when the video ends.

The `Video` class includes a read-only `TimeToEnd` property that's calculated based on changes to the `Duration` and `Position` properties:

```csharp
namespace VideoDemos.Controls
{
    public class Video : View, IVideo, IVideoController
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

### Custom positioning bar

A custom positioning bar can be implemented by creating a class that derives from `Slider`, which contains `Duration` and `Position` properties of type `TimeSpan`:

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

The property-changed event handler for the `Duration` property sets the `Maximum` property of the `Slider` to the `TotalSeconds` property of the `TimeSpan` value. Similarly, the property-changed event handler for the `Position` property sets the `Value` property of the `Slider`. This is the mechanism by which the `Slider` tracks the position of `PositionSlider`.

The `PositionSlider` is updated from the underlying `Slider` in only one scenario, which is when the user manipulates the `Slider` to indicate that the video should be advanced or reversed to a new position. This is detected in the `PropertyChanged` handler in the `PositionSlider` constructor. This event handler checks for a change in the `Value` property, and if it's different from the `Position` property, then the `Position` property is set from the `Value` property.

The following example shows the `PositionSlider` being consumed in XAML:

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

The `Position` property of the `Video` object is bound to the `Position` property of the `PositionSlider`, without performance issues, because the `Video.Position` property is changed by the `MauiVideoPlayer.UpdateStatus` method on each platform, which is only called 10 times a second. In addition, two `Label` objects display the `Position` and `TimeToEnd` properties values from the `Video` object.
