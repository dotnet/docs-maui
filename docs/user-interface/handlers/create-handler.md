---
title: "Create custom controls with .NET MAUI handlers"
description: ""
ms.date: 08/02/2022
---

# Create custom controls

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-createhandler)

https://github.com/dotnet/maui/wiki/Porting-Custom-Renderers-to-Handlers

Each handler class exposes the native view that implements the cross-platform view via its `PlatformView` property. This property can be accessed to set native view properties, invoke native view methods, and subscribe to native view events. In addition, the cross-platform view implemented by the handler is exposed via its `VirtualView` property.

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

1. Create an interface for your cross-platform control, that implements `IView`. For more information, see [Cross-platform control interface](#cross-platform-control-interface).
1. Create a class for your custom cross-platform control, that derives from `View` and that implements your control interface. For more information, see [Cross-platform control](#cross-platform-control).
1. Optionally, create any required additional cross-platform types.
1. Create an interface for your handler, that implements `IViewHandler`. For more information, see [Handler interface](#handler-interface).
1. Create a `partial` handler class, that implements your handler interface. For more information, see [Handler class](#handler-class).
1. In your handler class, create the `PropertyMapper` dictionary, which defines the actions to take when cross-platform property changes occur. For more information, see [Property mappers](#property-mappers).
1. Optionally, in your handler class, create the `CommandMapper` dictionary, which defines the actions to take when the cross-platform control sends instructions to the native views that implement the control. For more information, see [Command mappers](#command-mappers).
1. Create `partial` handler classes for each platform, create the native views that implement the cross-platform control. For more information, see [Create platform controls](create-platform-controls.md).
1. Register the handler using the `ConfigureMauiHandlers` and `AddHandler` methods in your app's `MauiProgram` class. For more information, see [Register the handler](register-and-consume.md).

Then, the cross-platform control can be consumed. For more information, see [Consume the cross-platform control](register-and-consume.md#consume-the-cross-platform-control).

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

The handler class is a partial class whose implementation will be completed on each platform in an additional partial class. It implements the `VirtualView` and `PlatformView` properties that are defined in the interface, using [expression-bodied members](/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members) to return `VirtualView` and `PlatformView` properties that are defined in .NET MAUI's generic `ViewHandler` class. This will be discussed further in [Create platform controls](create-platform-controls.md).

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
        [nameof(IVideo.Position)] = MapPosition
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
        [nameof(IVideo.Position)] = MapPosition
    };

    public static CommandMapper<IVideo, IVideoHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(IVideo.UpdateStatus)] = MapUpdateStatus,
        [nameof(IVideo.PlayRequested)] = MapPlayRequested,
        [nameof(IVideo.PauseRequested)] = MapPauseRequested,
        [nameof(IVideo.StopRequested)] = MapStopRequested
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
