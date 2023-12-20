---
title: "Migrate a custom renderer to a .NET MAUI handler"
description: "Learn how to migrate a Xamarin.Forms custom renderer to a .NET MAUI handler."
ms.date: 04/11/2023
---

# Migrate a Xamarin.Forms custom renderer to a .NET MAUI handler

In Xamarin.Forms, custom renderers can be used to customize the appearance and behavior of a control, and create new cross-platform controls. Each custom renderer has a reference to the cross-platform control and often relies on `INotifyPropertyChanged` to send property change notifications. Rather than using custom renderers, .NET Multi-platform App UI (.NET MAUI) introduces a new concept called a *handler*.

Handlers offer many performance improvements over custom renderers. In Xamarin.Forms, the `ViewRenderer` class creates a parent element. For example, on Android, a `ViewGroup` is created which is used for auxiliary positioning tasks. In .NET MAUI, the `ViewHandler` class doesn't create a parent element, which helps to reduce the size of the visual hierarchy and improve your app's performance. Handlers also decouple platform controls from the framework. The platform control only needs to handle the needs of the framework. This is not only more efficient, but it's much easier to extend or override when required. Handlers are also suitable for reuse by other frameworks such as [Comet](https://github.com/dotnet/Comet) and [Fabulous](https://github.com/fabulous-dev/Fabulous). For more information about handlers, see [Handlers](~/user-interface/handlers/index.md).

In Xamarin.Forms, the `OnElementChanged` method in a custom renderer creates the platform control, initializes default values, subscribes to events, and handles the Xamarin.Forms element the renderer was attached to (`OldElement`) and the element that the renderer is attached to (`NewElement`). In addition, a single `OnElementPropertyChanged` method defines the operations to invoke when a property change occurs in the cross-platform control. .NET MAUI simplifies this approach, so that every property change is handled by a separate method, and so that code to create the platform control, perform control setup, and perform control cleanup, is separated into distinct methods.

The process for migrating a Xamarin.Forms custom control that's backed by custom renderers on each platform to a .NET MAUI custom control that's backed by a handler on each platform is as follows:

1. Create a class for the cross-platform control, which provides the control's public API. For more information, see [Create the cross-platform control](#create-the-cross-platform-control).
1. Create a `partial` handler class. For more information, see [Create the handler](#create-the-handler).
1. In the handler class, create a `PropertyMapper` dictionary, which defines the Actions to take when cross-platform property changes occur. For more information, see [Create the property mapper](#create-the-property-mapper).
1. Create `partial` handler classes for each platform that create the native views that implement the cross-platform control. For more information, see [Create the platform controls](#create-the-platform-controls).
1. Register the handler using the `ConfigureMauiHandlers` and `AddHandler` methods in your app's `MauiProgram` class. For more information, see [Register the handler](#register-the-handler).

Then, the cross-platform control can be consumed. For more information, see [Consume the cross-platform control](#consume-the-cross-platform-control).

Alternatively, custom renderers that customize Xamarin.Forms controls can be converted so that they modify .NET MAUI handlers. For more information, see [Customize controls with handlers](~/user-interface/handlers/customize.md).

## Create the cross-platform control

To create a cross-platform control, you should create a class that derives from <xref:Microsoft.Maui.Controls.View>:

```csharp
namespace MyMauiControl.Controls
{
    public class CustomEntry : View
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry), null);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomEntry), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
    }
}
```

The control should provide a public API that will be accessed by its handler, and control consumers. Cross-platform controls should derive from <xref:Microsoft.Maui.Controls.View>, which represents a visual element that's used to place layouts and views on the screen.

## Create the handler

After creating your cross-platform control, you should create a `partial` class for your handler:

```csharp
#if IOS || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTextField;
#elif ANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatEditText;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using MyMauiControl.Controls;
using Microsoft.Maui.Handlers;

namespace MyMauiControl.Handlers
{
    public partial class CustomEntryHandler
    {
    }
}
```

The handler class is a partial class whose implementation will be completed on each platform with an additional partial class.

The conditional `using` statements define the `PlatformView` type on each platform. The final conditional `using` statement defines `PlatformView` to be equal to `System.Object`. This is necessary so that the `PlatformView` type can be used within the handler for usage across all platforms. The alternative would be to have to define the `PlatformView` property once per platform, using conditional compilation.

## Create the property mapper

Each handler typically provides a *property mapper*, which defines what Actions to take when a property change occurs in the cross-platform control. The `PropertyMapper` type is a `Dictionary` that maps the cross-platform control's properties to their associated Actions.

> [!NOTE]
> The property mapper is the replacement for the `OnElementPropertyChanged` method in a Xamarin.Forms custom renderer.

`PropertyMapper` is defined in .NET MAUI's generic `ViewHandler` class, and requires two generic arguments to be supplied:

- The class for the cross-platform control, which derives from <xref:Microsoft.Maui.Controls.View>.
- The class for the handler.

The following code example shows the `CustomEntryHandler` class extended with the `PropertyMapper` definition:

```csharp
public partial class CustomEntryHandler
{
    public static PropertyMapper<CustomEntry, CustomEntryHandler> PropertyMapper = new PropertyMapper<CustomEntry, CustomEntryHandler>(ViewHandler.ViewMapper)
    {
        [nameof(CustomEntry.Text)] = MapText,
        [nameof(CustomEntry.TextColor)] = MapTextColor
    };

    public CustomEntryHandler() : base(PropertyMapper)
    {
    }
}
```

The `PropertyMapper` is a `Dictionary` whose key is a `string` and whose value is a generic `Action`. The `string` represents the cross-platform control's property name, and the `Action` represents a `static` method that requires the handler and cross-platform control as arguments. For example, the signature of the `MapText` method is `public static void MapText(CustomEntryHandler handler, CustomEntry view)`.

Each platform handler must provide implementations of the Actions, which manipulate the native view APIs. This ensures that when a property is set on a cross-platform control, the underlying native view will be updated as required. The advantage of this approach is that it allows for easy cross-platform control customization, because the property mapper can be modified by cross-platform control consumers without subclassing. For more information, see [Customize controls with handlers](~/user-interface/handlers/customize.md).

## Create the platform controls

After creating the mappers for your handler, you must provide handler implementations on all platforms. This can be accomplished by adding partial class handler implementations in the child folders of the *Platforms* folder. Alternatively you could configure your project to support filename-based multi-targeting, or folder-based multi-targeting, or both.

Filename-based multi-targeting is configured by adding the following XML to the project file, as children of the `<Project>` node:

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

Each platform handler class should be a partial class and derive from the generic `ViewHandler` class, which requires two type arguments:

- The class for the cross-platform control, which derives from <xref:Microsoft.Maui.Controls.View>.
- The type of the native view that implements the cross-platform control on the platform. This should be identical to the type of the `PlatformView` property in the handler.

> [!IMPORTANT]
> The `ViewHandler` class provides `VirtualView` and `PlatformView` properties. The `VirtualView` property is used to access the cross-platform control from its handler. The `PlatformView` property, is used to access the native view on each platform that implements the cross-platform control.

Each of the platform handler implementations should override the following methods:

- `CreatePlatformView`, which should create and return the native view that implements the cross-platform control.
- `ConnectHandler`, which should perform any native view setup, such as initializing the native view and performing event subscriptions.
- `DisconnectHandler`, which should perform any native view cleanup, such as unsubscribing from events and disposing objects. This method is intentionally not invoked by .NET MAUI. Instead, you must invoke it yourself from a suitable location in your app's lifecycle. For more information, see [Native view cleanup](#native-view-cleanup).

> [!NOTE]
> The `CreatePlatformView`, `ConnectHandler`, and `DisconnectHandler` overrides are the replacements for the `OnElementChanged` method in a Xamarin.Forms custom renderer.

Each platform handler should also implement the Actions that are defined in the mapper dictionaries. In addition, each platform handler should also provide code, as required, to implement the functionality of the cross-platform control on the platform. Alternatively, for more complex controls this can be provided by an additional type.

The following example shows the `CustomEntryHandler` implementation on Android:

```csharp
#nullable enable
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using MyMauiControl.Controls;

namespace MyMauiControl.Handlers
{
    public partial class CustomEntryHandler : ViewHandler<CustomEntry, AppCompatEditText>
    {
        protected override AppCompatEditText CreatePlatformView() => new AppCompatEditText(Context);

        protected override void ConnectHandler(AppCompatEditText platformView)
        {
            base.ConnectHandler(platformView);

            // Perform any control setup here
        }

        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            // Perform any native view cleanup here
            platformView.Dispose();
            base.DisconnectHandler(platformView);
        }

        public static void MapText(CustomEntryHandler handler, CustomEntry view)
        {
            handler.PlatformView.Text = view.Text;
            handler.PlatformView?.SetSelection(handler.PlatformView?.Text?.Length ?? 0);
        }

        public static void MapTextColor(CustomEntryHandler handler, CustomEntry view)
        {
            handler.PlatformView?.SetTextColor(view.TextColor.ToPlatform());
        }
    }
}
```

`CustomEntryHandler` derives from the `ViewHandler` class, with the generic `CustomEntry` argument specifying the cross-platform control type, and the `AppCompatEditText` argument specifying the type of native control.

The `CreatePlatformView` override creates and returns an `AppCompatEditText` object. The `ConnectHandler` override is the location to perform any required native view setup. The `DisconnectHandler` override is the location to perform any native view cleanup, and so calls the `Dispose` method on the `AppCompatEditText` instance.

The handler also implements the Actions defined in the property mapper dictionary. Each Action is executed in response to a property changing on the cross-platform control, and is a `static` method that requires handler and cross-platform control instances as arguments. In each case, the Action calls methods defined on the native control.

## Register the handler

A custom control and its handler must be registered with an app, before it can be consumed. This should occur in the `CreateMauiApp` method in the `MauiProgram` class in your app project, which is the cross-platform entry point for the app:

```csharp
using Microsoft.Extensions.Logging;
using MyMauiControl.Controls;
using MyMauiControl.Handlers;

namespace MyMauiControl;

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
        handlers.AddHandler(typeof(CustomEntry), typeof(CustomEntryHandler));
      });

#if DEBUG
    builder.Logging.AddDebug();
#endif

    return builder.Build();
  }
}
```

The handler is registered with the `ConfigureMauiHandlers` and `AddHandler` method. The first argument to the `AddHandler` method is the cross-platform control type, with the second argument being its handler type.

> [!NOTE]
> This registration approach avoids Xamarin.Forms' assembly scanning, which is slow and expensive.

## Consume the cross-platform control

After registering the handler with your app, the cross-platform control can then be consumed:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:MyMauiControl.Controls"
             x:Class="MyMauiControl.MainPage">
    <Grid>
        <controls:CustomEntry Text="Hello world"
                              TextColor="Blue" />
    </Grid>
</ContentPage>
```

### Native view cleanup

Each platform's handler implementation overrides the `DisconnectHandler` implementation, which is used to perform native view cleanup such as unsubscribing from events and disposing objects. However, this override is intentionally not invoked by .NET MAUI. Instead, you must invoke it yourself from a suitable location in your app's lifecycle. This could be when the page containing the control is navigated away from, which causes the page's `Unloaded` event to be raised.

An event handler for the page's `Unloaded` event can be registered in XAML:

```xaml
<ContentPage ...
             xmlns:controls="clr-namespace:MyMauiControl.Controls"
             Unloaded="ContentPage_Unloaded">
    <Grid>
        <controls:CustomEntry x:Name="customEntry"
                              ... />
    </Grid>
</ContentPage>
```

The event handler for the `Unloaded` event can then invoke the `DisconnectHandler` method on its `Handler` instance:

```csharp
void ContentPage_Unloaded(object sender, EventArgs e)
{
    customEntry.Handler?.DisconnectHandler();
}
```

## See also

- [Create a custom control using handler](~/user-interface/handlers/create.md)
