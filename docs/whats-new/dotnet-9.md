---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 10/04/2024
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is to improve product quality. This includes expanding test coverage, end to end scenario testing, and bug fixing. For more information about the product quality improvements in .NET MAUI 9, see the following release notes:

- [.NET MAUI 9 RC1](https://github.com/dotnet/maui/releases/tag/9.0.0-rc.1.24453.9)
- [.NET MAUI 9 Preview 7](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.7.24407.4)
- [.NET MAUI 9 Preview 6](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.6.24327.7)
- [.NET MAUI 9 Preview 5](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.5.24307.10)
- [.NET MAUI 9 Preview 4](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.4.10690)
- [.NET MAUI 9 Preview 3](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.3.10457)
- [.NET MAUI 9 Preview 2](https://github.com/dotnet/maui/releases/tag/9.0.0-preview.2.10293)
- [.NET MAUI 9 Preview 1](https://github.com/dotnet/maui/releases/tag/9.0.100-preview.1.9973)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 9, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds. When you create a new .NET MAUI project the required NuGet packages are automatically added to the project.

## Minimum deployment targets

.NET MAUI 9 requires minimum deployment targets of iOS 12.2, and Mac Catalyst 15.0 (macOS 12.0). Android and Windows minimum deployment targets remain the same. For more information, see [Supported platforms for .NET MAUI apps](../supported-platforms.md).

## New controls

.NET MAUI 9 includes two new controls.

### HybridWebView

<xref:Microsoft.Maui.Controls.HybridWebView> enables hosting arbitrary HTML/JS/CSS content in a web view, and enables communication between the code in the web view (JavaScript) and the code that hosts the web view (C#/.NET). For example, if you have an existing React JS app, you could host it in a cross-platform .NET MAUI native app, and build the back-end of the app using C# and .NET.

To build a .NET MAUI app with <xref:Microsoft.Maui.Controls.HybridWebView> you need:

- The web content of the app, which consists of static HTML, JavaScript, CSS, images, and other files.
- A <xref:Microsoft.Maui.Controls.HybridWebView> control as part of the app's UI. This can be achieved by referencing it in the app's XAML.
- Code in the web content, and in C#/.NET, that uses the <xref:Microsoft.Maui.Controls.HybridWebView> APIs to send messages between the two components.

The entire app, including the web content, is packaged and runs locally on a device, and can be published to applicable app stores. The web content is hosted within a native web view control and runs within the context of the app. Any part of the app can access external web services, but isn't required to.

For more information, see [HybridWebView](~/user-interface/controls/hybridwebview.md).

### Titlebar for Windows

The `TitleBar` control provides the ability to add a custom title bar to your app on Windows:

:::image type="content" source="media/dotnet-9/titlebar-overview.png" alt-text=".NET MAUI Titlebar overview." border="false":::

A `TitleBar` can be set as the value of the `Window.TitleBar` property on any `Window`:

```xaml
<Window.TitleBar>
    <TitleBar x:Name="TeamsTitleBar"
              Title="Hello World"
              Icon="appicon.png"
              HeightRequest="46">
        <TitleBar.Content>
            <Entry x:Name="SearchTitleBar"
                   Placeholder="Search"
                   VerticalOptions="Center"
                   MinimumWidthRequest="300"
                   MaximumWidthRequest="450"
                   HeightRequest="32"/>
        </TitleBar.Content>
    </TitleBar>
</Window.TitleBar>
```

An example of its use in C# is:

```csharp
Window.TitleBar = new TitleBar
{
    Title = "MAUI App",
    Icon = "appicon.png",
    HeightRequest = 46,
    LeadingContent = new AvatarButton()
};
```

A `TitleBar` is highly customizable through its `Content`, `LeadingContent`, and `TrailingContent` properties:

```xaml
<TitleBar Title="My App"
          BackgroundColor="#512BD4"
          HeightRequest="48">
    <TitleBar.Content>
        <SearchBar Placeholder="Search"
                   MaximumWidthRequest="300"
                   HorizontalOptions="FillAndExpand"
                   VerticalOptions="Center" />
    </TitleBar.Content>
    <TitleBar.TrailingContent>
        <ImageButton HeightRequest="36"
                     WidthRequest="36"
                     BorderWidth="0"
                     Background="Transparent">
            <ImageButton.Source>
                <FontImageSource Size="16"
                                 Glyph="&#xE713;"
                                 FontFamily="SegoeMDL2"/>
            </ImageButton.Source>
        </ImageButton>
    </TitleBar.TrailingContent>
</TitleBar>
```

The following screenshot shows the resulting appearance:

:::image type="content" source="media/dotnet-9/titlebar-full.png" alt-text=".NET MAUI Titlebar screenshot.":::

> [!NOTE]
> Mac Catalyst support for the `TitleBar` control will be added in a future release.

## Control enhancements

.NET MAUI 9 includes control enhancements.

### BackButtonBehavior OneWay binding mode

The binding mode for `IsVisible` and `IsEnabled` on a <xref:Microsoft.Maui.Controls.BackButtonBehavior> in a Shell app is now `BindingMode.OneWay` instead of `BindingMode.OneTime`. This enables you to more easily control the behavior of the back button at runtime, with data bindings:

```xaml
<ContentPage ...>    
    <Shell.BackButtonBehavior>
        <BackButtonBehavior Command="{Binding BackCommand}"
                            IsVisible="{Binding IsBackButtonVisible}"
                            IconOverride="back.png" />   
    </Shell.BackButtonBehavior>
    ...
</ContentPage>
```

### BlazorWebView

On iOS and Mac Catalyst 18, .NET MAUI 9 changes the default behavior for hosting content in a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> to `localhost`. The internal `0.0.0.0` address used to host content no longer works and results in the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> not loading any content and rendering as an empty rectangle.

To opt into using the `0.0.0.0` address, add the following code to the `CreateMauiApp` method in *MauiProgram.cs*:

```csharp
// Set this switch to use the LEGACY behavior of always using 0.0.0.0 to host BlazorWebView
AppContext.SetSwitch("BlazorWebView.AppHostAddressAlways0000", true);
```

If you encounter hangs on Android with <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> you should enable an <xref:System.AppContext> switch in the `CreateMauiApp` method in your `MauiProgram` class:

```csharp
AppContext.SetSwitch("BlazorWebView.AndroidFireAndForgetAsync", true);
```

This switch enables <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> to fire and forget the async disposal that occurs, and as a result fixes the majority of the disposal deadlocks that occur on Android. For more information, see [Fix disposal deadlocks on Android](~/user-interface/controls/blazorwebview.md#fix-disposal-deadlocks-on-android).

### CollectionView and CarouselView

.NET MAUI 9 includes two optional new handlers on iOS and Mac Catalyst that bring performance and stability improvements to `CollectionView` and `CarouselView`. These handlers are based on `UICollectionView` APIs.

To opt into using these handlers, add the following code to your `MauiProgram` class:

```csharp
#if IOS || MACCATALYST
builder.ConfigureMauiHandlers(handlers =>
{
    handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
    handlers.AddHandler<Microsoft.Maui.Controls.CarouselView, Microsoft.Maui.Controls.Handlers.Items2.CarouselViewHandler2>();
});
#endif
```

### ContentPage

In .NET MAUI 9, the <xref:Microsoft.Maui.Controls.ContentPage.HideSoftInputOnTapped> property is also supported on Mac Catalyst, as well and Android and iOS.

### Soft keyboard input support

.NET MAUI 9 adds new soft keyboard input support for `Password`, `Date`, and `Time`. These can be enabled on <xref:Microsoft.Maui.Controls.Editor> and <xref:Microsoft.Maui.Controls.Entry> controls:

```xaml
<Entry Keyboard="Date" />
```

### Text alignment

The <xref:Microsoft.Maui.TextAlignment> enumeration adds a `Justify` member that can be used to align text in text controls. For example, you can horizontally align text in a <xref:Microsoft.Maui.Controls.Label> with `HorizontalTextAlignment.Justify`:

```xaml
<Label Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. In facilisis nulla eu felis fringilla vulputate."
       HorizontalTextAlignment="Justify"/>
```

### TimePicker

<xref:Microsoft.Maui.Controls.TimePicker> gains a <xref:Microsoft.Maui.Controls.TimePicker.TimeSelected> event, which is raised when the selected time changes. The <xref:Microsoft.Maui.Controls.TimeChangedEventArgs> object that accompanies the `TimeSelected` event has `NewTime` and `OldTime` properties, which specify the new and old time, respectively.

### WebView

<xref:Microsoft.Maui.Controls.WebView> adds a `ProcessTerminated` event that's raised when a <xref:Microsoft.Maui.Controls.WebView> process ends unexpectedly. The `WebViewProcessTerminatedEventArgs` object that accompanies this event defines platform-specific properties that indicate why the process failed.

## App lifecycle

.NET MAUI 9 adds the following remote notification lifecycle methods on iOS and Mac Catalyst:

- `RegisteredForRemoteNotifications`, which is invoked when the app has successfully registered for remote notifications.
- `ReceivedRemoteNotifications`, which is invoked when a remote notification is received.

The following example shows how to consume these lifecycle methods:

```csharp
using Microsoft.Maui.LifecycleEvents;

namespace PlatformLifecycleDemo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureLifecycleEvents(events =>
            {
#if IOS || MACCATALYST
                events.AddiOS(ios => ios
                    .ReceivedRemoteNotifications((app, dictionary) => LogEvent(nameof(iOSLifecycle.OnReceivedRemoteNotifications)))
                    .RegisteredForRemoteNotifications((app, data) => LogEvent(nameof(iOSLifecycle.OnRegisteredForRemoteNotifications)));
#endif
                static bool LogEvent(string eventName, string type = null)
                {
                    System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                    return true;
                }
            });

        return builder.Build();
    }
}
```

## Compiled bindings in code

Bindings written in code typically use string paths that are resolved at runtime with reflection, and the overhead of doing this varies from platform to platform. .NET MAUI 9 introduces an additional <xref:Microsoft.Maui.Controls.BindableObjectExtensions.SetBinding%2A> extension method that defines bindings using a `Func` argument instead of a  string path:

```csharp
// in .NET 8
MyLabel.SetBinding(Label.TextProperty, "Text");

// in .NET 9
MyLabel.SetBinding(Label.TextProperty, static (Entry entry) => entry.Text);
```

This compiled binding approach provides the following benefits:

- Improved data binding performance by resolving binding expressions at compile-time rather than runtime.
- A better developer troubleshooting experience because invalid bindings are reported as build errors.
- Intellisense while editing.

Not all methods can be used to define a compiled binding. The expression must be a simple property access expression. The following examples show valid and invalid binding expressions:

```csharp
// Valid: Property access
static (PersonViewModel vm) => vm.Name;
static (PersonViewModel vm) => vm.Address?.Street;

// Valid: Array and indexer access
static (PersonViewModel vm) => vm.PhoneNumbers[0];
static (PersonViewModel vm) => vm.Config["Font"];

// Valid: Casts
static (Label label) => (label.BindingContext as PersonViewModel).Name;
static (Label label) => ((PersonViewModel)label.BindingContext).Name;

// Invalid: Method calls
static (PersonViewModel vm) => vm.GetAddress();
static (PersonViewModel vm) => vm.Address?.ToString();

// Invalid: Complex expressions
static (PersonViewModel vm) => vm.Address?.Street + " " + vm.Address?.City;
static (PersonViewModel vm) => $"Name: {vm.Name}";
```

In addition, .NET MAUI 9 adds a <xref:Microsoft.Maui.Controls.Binding.Create%2A?displayProperty=nameWithType> method that sets the binding directly on the object with a `Func`, and returns the binding object instance:

```csharp
// in .NET 8
myEntry.SetBinding(Entry.TextProperty, new MultiBinding
{
    Bindings = new Collection<BindingBase>
    {
        new Binding(nameof(Entry.FontFamily), source: RelativeBindingSource.Self),
        new Binding(nameof(Entry.FontSize), source: RelativeBindingSource.Self),
        new Binding(nameof(Entry.FontAttributes), source: RelativeBindingSource.Self),
    },
    Converter = new StringConcatenationConverter()
});

// in .NET 9
myEntry.SetBinding(Entry.TextProperty, new MultiBinding
{
    Bindings = new Collection<BindingBase>
    {
        Binding.Create(static (Entry entry) => entry.FontFamily, source: RelativeBindingSource.Self),
        Binding.Create(static (Entry entry) => entry.FontSize, source: RelativeBindingSource.Self),
        Binding.Create(static (Entry entry) => entry.FontAttributes, source: RelativeBindingSource.Self),
    },
    Converter = new StringConcatenationConverter()
});
```

> [!IMPORTANT]
> Compiled bindings are required instead of string-based bindings in NativeAOT apps, and in apps with full trimming enabled.

## Compiled bindings in XAML

In .NET MAUI 8, compiled bindings are disabled for any XAML binding expressions that define the `Source` property, and are unsupported on multi-bindings. These restrictions have been removed in .NET MAUI 9.

By default, .NET MAUI doesn't produce build warnings for bindings that don't use compiled bindings, unless you've enabled NativeAOT for your app. However, you can opt into compiled bindings warnings being produced by setting the `$(MauiStrictXamlCompilation)` build property to `true` in your app's project file (*.csproj):

```xml
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
```

## Handler disconnection

When implementing a custom control using handlers, every platform handler implementation is required to implement the `DisconnectHandler` method, to perform any native view cleanup such as unsubscribing from events. However, prior to .NET MAUI 9, the `DisconnectHandler` implementation is intentionally not invoked by .NET MAUI. Instead, you'd have to invoke it yourself when choosing to cleanup a control, such as when navigating backwards in an app.

In .NET MAUI 9, handlers automatically disconnect from their controls when possible, such as when navigating backwards in an app. In some scenarios you might not want this behavior. Therefore, .NET MAUI 9 adds a `HandlerProperties.DisconnectPolicy` attached property for controlling when handlers are disconnected from their controls. This property requires a `HandlerDisconnectPolicy` argument, with the `HandlerDisconnectPolicy` enumeration defining the following values:

- `Automatic`, which indicates that handlers will be disconnected automatically. This is the default value of the `HandlerProperties.DisconnectPolicy` attached property.
- `Manual`, which indicates that handlers will have to be disconnected manually by invoking the `DisconnectHandler` implementation.

The following example shows setting the `HandlerProperties.DisconnectPolicy` attached property:

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

In addition, there's a `DisconnectHandlers` extension method that disconnects handlers from a given `IView`:

```csharp
video.DisconnectHandlers();
```

When disconnecting, the `DisconnectHandlers` method will propagate down the control tree until it completes or arrives at a control that has set a manual policy.

## Multi-window support

.NET MAUI 9 adds the ability to bring a specific window to the front on Mac Catalyst and Windows with the `Application.Current.ActivateWindow` method:

```csharp
Application.Current?.ActivateWindow(windowToActivate);
```

## Native embedding

.NET MAUI 9 includes full APIs for native embedding scenarios, which previously had to be manually added to your project:

```csharp
var mauiApp = MauiProgram.CreateMauiApp();

#if ANDROID
var mauiContext = new MauiContext(mauiApp.Services, window);
#else
var mauiContext = new MauiContext(mauiApp.Services);
#endif

var mauiView = new MyMauiContent();
var nativeView = mauiView.ToPlatform(mauiContext);
```

Alternatively, you can use the `ToPlatformEmbedded` method, passing in the `Window` for the platform on which the app is running:

```csharp
var mauiApp = MauiProgram.CreateMauiApp();
var mauiView = new MyMauiContent();
var nativeView = mauiView.ToPlatformEmbedded(mauiApp, window);
```

In both examples, `nativeView` is a platform-specific version of `mauiView`.

To bootstrap a native embedded app in .NET MAUI 9, call the `UseMauiEmbeddedApp` extension method on your `MauiAppBuilder` object:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiEmbeddedApp<App>();

        return builder.Build();
    }
}
```

## Project templates

.NET MAUI 9 adds a **.NET MAUI Blazor Hybrid and Web App** project template to Visual Studio that creates a solution with a .NET MAUI Blazor Hybrid app with a Blazor Web app, which share common code in a Razor class library project.

The template can also be used from `dotnew new`:

```dotnetcli
dotnet new maui-blazor-web -n AllTheTargets
```

## Resource dictionaries

In .NET MAUI 9, a stand-alone XAML <xref:Microsoft.Maui.Controls.ResourceDictionary> (which isn't backed by a code-behind file) defaults to having its XAML compiled. To opt out of this behavior, specify `<?xaml-comp compile="false" ?>` after the XML header.

## Shell

Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> isn't trim safe and shouldn't be used with full trimming or NativeAOT. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).

## Trimming feature switches

Several areas of .NET MAUI come with trimmer directives, known as feature switches, that make it possible to remove the code for disabled features when `TrimMode=full`, as well as for NativeAOT:

| MSBuild property | Description |
| ---------------- | ----------- |
| `MauiEnableVisualAssemblyScanning` | When set to `true`, .NET MAUI will scan assemblies for types implementing `IVisual` and for `[assembly:Visual(...)]` attributes, and will register these types. By default, this build property is set to `false`. |
| `MauiShellSearchResultsRendererDisplayMemberNameSupported` | When set to `false`, the value of `SearchHandler.DisplayMemberName` will be ignored. Instead, you should provide an `ItemTemplate` to define the appearance of `SearchHandler` results. By default, this build property is set to `true`.|
| `MauiQueryPropertyAttributeSupport` | When set to `false`, `[QueryProperty(...)]` attributes won't be used to set property values when navigating. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface to accept query parameters. By default, this build property is set to `true`. |
| `MauiImplicitCastOperatorsUsageViaReflectionSupport` | When set to `false`, .NET MAUI won't look for implicit cast operators when converting values from one type to another. This can affect bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a `TypeConverter` for your type and attach it to the type using the `[TypeConverter(typeof(MyTypeConverter))]` attribute. By default, this build property is set to `true`.|
| `_MauiBindingInterceptorsSupport` | When set to `false`, .NET MAUI won't intercept any calls to the `SetBinding` methods and won't try to compile them. By default, this build property is set to `true`. |

To consume a feature switch you should put the corresponding MSBuild property into your app's project file (*.csproj), which causes the related code to be trimmed from the .NET MAUI assemblies. Disabling features an app doesn't require can help reduce the app size when combined with the `Full` trimming mode.

## XAML

All classes that implement <xref:Microsoft.Maui.Controls.Xaml.IMarkupExtension>, <xref:Microsoft.Maui.Controls.Xaml.IMarkupExtension`1>, <xref:Microsoft.Maui.Controls.Xaml.IValueProvider>, and <xref:Microsoft.Maui.Controls.IExtendedTypeConverter> need to be annotated with either the <xref:Microsoft.Maui.Controls.Xaml.RequireServiceAttribute> or <xref:Microsoft.Maui.Controls.Xaml.AcceptEmptyServiceProviderAttribute>. This is required due to a XAML compiler optimization introduced in .NET MAUI 9 that enables the generation of more efficient code, which helps reduce the app size and improve runtime performance.

For information about annotating markup extensions with these attributes, see [Service providers](~/xaml/markup-extensions/create.md?view=net-maui-9&preserve-view=true).

## Xcode sync

.NET MAUI 9 includes Xcode sync (`xcsync`), which is a tool that enables you to use Xcode for managing Apple specific files with .NET projects, including asset catalogs, plist files, storyboards, and xib files. The tool has two main commands to generate a temporary Xcode project from a .NET project, and to synchronize changes from the Xcode files back to your .NET project.

You use `dotnet build` with the `xcsync-generate` or `xcsync-sync` commands, to generate or sync these files, and pass in a project file and additional arguments:

```dotnetcli
dotnet build /t:xcsync-generate
    /p:xcSyncProjectFile=<PROJECT>
    /p:xcSyncXcodeFolder=<TARGET_XCODE_DIRECTORY>
    /p:xcSyncTargetFrameworkMoniker=<FRAMEWORK>
    /p:xcSyncVerbosity=<LEVEL>
```

For more information, see [Xcode sync](~/macios/xcsync.md).

## Deprecated APIs

.NET MAUI 9 deprecates some APIs, which will be completely removed in a future release.

### Frame

The `Frame` control is marked as obsolete in .NET MAUI 9, and will be completely removed in a future release. The `Border` control should be used in its place. For more information see [Border](~/user-interface/controls/border.md).

### MainPage

Instead of defining the first page of your app using the `MainPage` property on an `Application` object, you should set the `Page` property on a `Window` to the first page of your app. This is what happens internally in .NET MAUI when you set the `MainPage` property, so there's no behavior change introduced by the `MainPage` property being marked as obsolete.

The following example shows setting the `Page` property on a `Window`, via the `CreateWindow` override:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
```

The `MainPage` property is retained for .NET MAUI 9, but will be completely removed in a future release.

### Compatibility layouts

The compatibility layout classes in the `Microsoft.Maui.Controls.Compatibility` namespace have been obsoleted.

### Legacy measure calls

The following `VisualElement` legacy measure methods have been obsoleted:

- `protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)`.
- `public virtual SizeRequest Measure(double widthConstraint, double heightConstraint, MeasureFlags flags = MeasureFlags.None)` from `VisualElement`.

As a replacement, the following method has been introduced:

- `public size Measure(double widthConstraint, double heightConstraint)`

This `Measure` method returns the minimum size that an element needs in order to be displayed on a device. Margins are excluded from the measurement, but are returned with the size. This is the preferred method to call when measuring a view.

In addition, the `Microsoft.Maui.SizeRequest` struct is obsoleted. Instead, `Microsoft.Maui.Size` should be used.

## .NET for Android

.NET for Android 9, which adds support for API 35, includes work to reduce build times, and to improve the trimability of apps to reduce size and improve performance. For more information about .NET for Android 9, see the following release notes:

- [.NET for Android 9 RC1](https://github.com/dotnet/android/releases/tag/35.0.0-rc.1.80)
- [.NET for Android 9 Preview 7](https://github.com/xamarin/xamarin-android/releases/tag/35.0.0-preview.7.41)
- [.NET for Android 9 Preview 6](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.6.340)
- [.NET for Android 9 Preview 5](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.5.308)
- [.NET for Android 9 Preview 4](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.4.272)
- [.NET for Android 9 Preview 3](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.3.231)
- [.NET for Android 9 Preview 2](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.2.189)
- [.NET for Android 9 Preview 1](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.1.151)

### Asset packs

.NET for Android 9 introduces the ability to place assets into a separate package, known as an *asset pack*. This enables you to upload games and apps that would normally be larger than the basic package size allowed by Google Play. By putting these assets into a separate package you gain the ability to upload a package which is up to 2Gb in size, rather than the basic package size of 200Mb.

> [!IMPORTANT]
> Asset packs can only contain assets. In the case of .NET for Android this means items that have the `AndroidAsset` build action.

.NET MAUI apps define assets via the `MauiAsset` build action. An asset pack can be specified via the `AssetPack` attribute:

```xml
<MauiAsset
    Include="Resources\Raw\**"
    LogicalName="%(RecursiveDir)%(Filename)%(Extension)"
    AssetPack="myassetpack" />
```

> [!NOTE]
> The additional metadata will be ignored by other platforms.

If you have specific items you want to place in an asset pack you can use the `Update` attribute to define the `AssetPack` metadata:

```xml
<MauiAsset Update="Resources\Raw\MyLargeAsset.txt" AssetPack="myassetpack" />
```

Asset packs can have different delivery options, which control when your assets will install on the device:

- Install time packs are installed at the same time as the app. This pack type can be up to 1Gb in size, but you can only have one of them. This delivery type is specified with `InstallTime` metadata.
- Fast follow packs will install at some point shortly after the app has finished installing. The app will be able to start while this type of pack is being installed so you should check it has finished installing before trying to use the assets. This kind of asset pack can be up to 512Mb in size. This delivery type is specified with `FastFollow` metadata.
- On demand packs will never be downloaded to the device unless the app specifically requests it. The total size of all your asset packs can't exceed 2Gb, and you can have up to 50 separate asset packs. This delivery type is specified with `OnDemand` metadata.

In .NET MAUI apps, the delivery type can be specified with the `DeliveryType` attribute on a `MauiAsset`:

```xml
<MauiAsset Update="Resources\Raw\myvideo.mp4" AssetPack="myassetpack" DeliveryType="FastFollow" />
```

For more information about Android asset packs, see [Android asset packs](~/android/asset-packs.md).

### Android 15 support

.NET for Android 9 adds .NET bindings for Android 15 (API 35). To build for these APIs, update the target framework of your project:

```xml
<TargetFramework>net9.0-android35</TargetFramework>
```

### LLVM marshalled methods

Low-level Virtual Machine (LLVM) marshalled methods are now enabled by default in .NET for Android 9 in non-Blazor apps. This has resulted in a [~10% improvement in performance in a test app](https://github.com/xamarin/xamarin-android/pull/8925).

LLVM marshalled methods can be disabled in your project file (*.csproj*):

```xml
<PropertyGroup Condition="'$(TargetFramework)' == 'net9.0-android'">
    <AndroidEnableLLVM>false</AndroidEnableLLVM>
    <AndroidEnableLLVMOptimizations>false</AndroidEnableLLVMOptimizations>
</PropertyGroup>
```

### Trimming enhancements

.NET for Android 9 includes fixes for when using full trimming to reduce app size. Full trimming is usually only enabled for release builds of your app, and can be configured in your project file (*.csproj*):

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release' And '$(TargetFramework)' == 'net9.0-android'">
    <TrimMode>Full</TrimMode>
</PropertyGroup>
```

## .NET for iOS

.NET 9 on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 15.4 for the following platform versions:

- iOS: 17.5
- tvOS: 17.5
- Mac Catalyst: 17.5
- macOS: 14.5

For more information about .NET 9 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 9.0.1xx RC1](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-rc1-9270)
- [.NET 9.0.1xx Preview 7](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview7-9231)
- [.NET 9.0.1xx Preview 6](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview6-9714)
- [.NET 9.0.1xx Preview 5](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview5-9639)
- [.NET 9.0.1xx Preview 4](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview4-9523)
- [.NET 9.0.1xx Preview 3](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview3-9429)
- [.NET 9.0.1xx Preview 2](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview2-9244)
- [.NET 9.0.1xx Preview 1](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-preview1-9088)

### Bindings

.NET for iOS 9 introduces the ability to multi-target versions of .NET for iOS bindings. For example, a library project may need to build for two distinct iOS versions:

```xml
<TargetFrameworks>net9.0-ios17.0;net9.0-ios17.2</TargetFrameworks>
```

This will produce two libraries, one using iOS 17.0 bindings, and one using iOS 17.2 bindings.

> [!IMPORTANT]
> An app project should always target the latest iOS SDK.

### Native AOT for iOS & Mac Catalyst

In .NET for iOS 9, native Ahead of Time (AOT) compilation for iOS and Mac Catalyst takes advantage of full trimming to reduce your app's package size and startup performance. This is a publishing feature that you can use when you're ready to ship your app.

> [!IMPORTANT]
> Your app and it's dependencies must be fully trimmable in order to utilize this feature.

## See also

- [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9/overview).
- [Our Vision for .NET 9](https://devblogs.microsoft.com/dotnet/our-vision-for-dotnet-9/)
