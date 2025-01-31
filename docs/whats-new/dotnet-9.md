---
title: What's new in .NET MAUI for .NET 9
description: Learn about the new features introduced in .NET MAUI for .NET 9.
ms.date: 01/31/2025
---

# What's new in .NET MAUI for .NET 9

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 9 is to improve product quality. This includes expanding test coverage, end to end scenario testing, and bug fixing. For more information about the product quality improvements in .NET MAUI 9, see the following release notes:

- [.NET MAUI 9](https://github.com/dotnet/maui/releases/tag/9.0.0)
- [.NET MAUI 9 RC2](https://github.com/dotnet/maui/releases/tag/9.0.0-rc.2.24503.2)
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

Compatibility with Xcode 16, which includes SDK support for iOS 18, iPadOS 18, tvOS 18, and macOS 15, is required when building with .NET MAUI 9. Xcode 16 requires a Mac running macOS 14.5 or later.

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

The <xref:Microsoft.Maui.Controls.TitleBar> control provides the ability to add a custom title bar to your app on Windows:

:::image type="content" source="media/dotnet-9/titlebar-overview.png" alt-text=".NET MAUI Titlebar overview." border="false":::

A <xref:Microsoft.Maui.Controls.TitleBar> can be set as the value of the <xref:Microsoft.Maui.Controls.Window.TitleBar?displayProperty=nameWithType> property on any <xref:Microsoft.Maui.Controls.TitleBar>:

```xaml
<Window.TitleBar>
    <TitleBar x:Name="TeamsTitleBar"
              Title="Hello World"
              Icon="appicon.png"
              HeightRequest="46">
        <TitleBar.Content>
            <SearchBar Placeholder="Search"
                       PlaceholderColor="White"
                       MaximumWidthRequest="300"
                       HorizontalOptions="Fill"
                       VerticalOptions="Center" />
        </TitleBar.Content>
    </TitleBar>
</Window.TitleBar>
```

An example of its use in C# is:

```csharp
Window window = new Window
{
    TitleBar = new TitleBar
    {
        Icon = "titlebar_icon.png"
        Title = "My App",
        Subtitle = "Demo"
        Content = new SearchBar { ... }      
    }
};
```

A <xref:Microsoft.Maui.Controls.TitleBar> is highly customizable through its <xref:Microsoft.Maui.Controls.TitleBar.Content>, <xref:Microsoft.Maui.Controls.TitleBar.LeadingContent>, and <xref:Microsoft.Maui.Controls.TitleBar.TrailingContent> properties:

```xaml
<TitleBar Title="My App"
          BackgroundColor="#512BD4"
          HeightRequest="48">
    <TitleBar.Content>
        <SearchBar Placeholder="Search"
                   MaximumWidthRequest="300"
                   HorizontalOptions="Fill"
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

For more information, see [TitleBar](~/user-interface/controls/titlebar.md).

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

The default behavior for hosting content in a <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> has changed to `0.0.0.1`. The internal `0.0.0.0` address used to host content no longer works and results in the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> not loading any content and rendering as an empty rectangle.

To opt into using the `0.0.0.0` address, add the following code to the `CreateMauiApp` method in *MauiProgram.cs*:

```csharp
// Set this switch to use the LEGACY behavior of always using 0.0.0.0 to host BlazorWebView
AppContext.SetSwitch("BlazorWebView.AppHostAddressAlways0000", true);
```

By default, <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> now fires and forgets the async disposal of the underlying `WebViewManager`. This reduces the potential for disposal deadlocks to occur on Android.

> [!WARNING]
> This fire-and-forget default behavior means that disposal can return before all objects are disposed, which can cause behavioral changes in your app. The items that are disposed are partially Blazor's own internal types, but also app-defined types such as scoped services used within the <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> portion of your app.

To opt out of this behavior, you should configure your app to block on dispose via an <xref:System.AppContext> switch in the `CreateMauiApp` method in your `MauiProgram` class:

```csharp
AppContext.SetSwitch("BlazorWebView.AndroidFireAndForgetAsync", false);
```

If your app is configured to block on dispose via this switch, <xref:Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView> performs async-over-sync disposal, which means that it blocks the thread until the async disposal is complete. However, this can cause deadlocks if the disposal needs to run code on the same thread (because the thread is blocked while waiting).

### Buttons on iOS

<xref:Microsoft.Maui.Controls.Button> controls on iOS now respect spacing, padding, border width, and margins more accurately than in previous releases. A large image in a <xref:Microsoft.Maui.Controls.Button> will now be resized to the maximum size, taking into account the spacing, padding, border width, and margins. However, if a <xref:Microsoft.Maui.Controls.Button> contains text and an image it might not be possible to fit all the content inside the button, and so you should size your image manually to achieve your desired layout.

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

### Focus behavior on Windows

In .NET MAUI 8 on Windows, the <xref:Microsoft.Maui.Controls.VisualElement.Focused?displayProperty=nameWithType> event, the <xref:Microsoft.Maui.Controls.VisualElement.Unfocused?displayProperty=nameWithType> event, and the <xref:Microsoft.Maui.Controls.VisualElement.IsFocused?displayProperty=nameWithType> property are applied to an element and its children:

```xaml
<VerticalStackLayout Focus="OnFocused">
    <Entry />
    <Editor />
</VerticalStackLayout>
```

In this example on .NET 8, the `OnFocused` event handler is executed on Windows when the <xref:Microsoft.Maui.Controls.VerticalStackLayout>, <xref:Microsoft.Maui.Controls.Entry>, or <xref:Microsoft.Maui.Controls.Editor> gains focus.

.NET MAUI 9 changes this behavior on Windows to be identical to the other platforms. Therefore, the <xref:Microsoft.Maui.Controls.VisualElement.Focused?displayProperty=nameWithType> event, the <xref:Microsoft.Maui.Controls.VisualElement.Unfocused?displayProperty=nameWithType> event, and the <xref:Microsoft.Maui.Controls.VisualElement.IsFocused?displayProperty=nameWithType> property only apply to an element. Therefore, when the previous example runs on .NET 9, the `OnFocused` event handler isn't executed because only input controls can gain focus.

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

> [!WARNING]
> A CS0272 compiler error will occur if the set accessor for a property or indexer is inaccessible. If this occurs, increase the accessibility of the accessor.

In addition, .NET MAUI 9 adds a <xref:Microsoft.Maui.Controls.BindingBase.Create%2A?displayProperty=nameWithType> method that sets the binding directly on the object with a `Func`, and returns the binding object instance:

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

For more information about compiled bindings in code, see [Compiled bindings in code](~/fundamentals/data-binding/compiled-bindings.md?view=net-maui-9.0&preserve-view=true#compiled-bindings-in-code).

## Compiled bindings in XAML

In .NET MAUI 8, compiled bindings are disabled for any XAML binding expressions that define the `Source` property, and are unsupported on multi-bindings. These restrictions have been removed in .NET MAUI 9. For information about compiling XAML binding expressions that define the `Source` property, see [Compile bindings that define the `Source` property](~/fundamentals/data-binding/compiled-bindings.md?view=net-maui-9.0&preserve-view=true#compile-bindings-that-define-the-source-property).

By default, .NET MAUI 9 produces build warnings for bindings that don't use compiled bindings. For more information about XAML compiled bindings warnings, see [XAML compiled bindings warnings](~/fundamentals/data-binding/compiled-bindings.md?view=net-maui-9.0&preserve-view=true#xaml-compiled-bindings-warnings).

## Dependency injection

In a Shell app, you no longer need to register your pages with the dependency injection container unless you want to influence the lifetime of the page relative to the container with the [`AddSingleton`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton%2A), [`AddTransient`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient%2A), or [`AddScoped`](xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped%2A) methods. For more information about these methods, see [Dependency lifetime](~/fundamentals/dependency-injection.md#dependency-lifetime).

## Handler disconnection

When implementing a custom control using handlers, every platform handler implementation is required to implement the <xref:Microsoft.Maui.IElementHandler.DisconnectHandler> method, to perform any native view cleanup such as unsubscribing from events. However, prior to .NET MAUI 9, the <xref:Microsoft.Maui.IElementHandler.DisconnectHandler> implementation is intentionally not invoked by .NET MAUI. Instead, you'd have to invoke it yourself when choosing to cleanup a control, such as when navigating backwards in an app.

In .NET MAUI 9, handlers automatically disconnect from their controls when possible, such as when navigating backwards in an app. In some scenarios you might not want this behavior. Therefore, .NET MAUI 9 adds a [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property for controlling when handlers are disconnected from their controls. This property requires a <xref:Microsoft.Maui.HandlerDisconnectPolicy> argument, with the enumeration defining the following values:

- `Automatic`, which indicates that handlers will be disconnected automatically. This is the default value of the [`HandlerProperties.DisconnectPolicy`](xref:Microsoft.Maui.Controls.HandlerProperties.DisconnectPolicyProperty) attached property.
- `Manual`, which indicates that handlers will have to be disconnected manually by invoking the <xref:Microsoft.Maui.IElementHandler.DisconnectHandler> implementation.

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

In addition, there's a <xref:Microsoft.Maui.ViewExtensions.DisconnectHandlers%2A> extension method that disconnects handlers from a given <xref:Microsoft.Maui.IView>:

```csharp
video.DisconnectHandlers();
```

When disconnecting, the <xref:Microsoft.Maui.ViewExtensions.DisconnectHandlers%2A> method will propagate down the control tree until it completes or arrives at a control that has set a manual policy.

## Multi-window support

.NET MAUI 9 adds the ability to bring a specific window to the front on Mac Catalyst and Windows with the `Application.Current.ActivateWindow` method:

```csharp
Application.Current?.ActivateWindow(windowToActivate);
```

## Native AOT deployment

In .NET MAUI 9 you can opt into Native AOT deployment on iOS and Mac Catalyst. Native AOT deployment produces a .NET MAUI app that's been ahead-of-time (AOT) compiled to native code. This produces the following benefits:

- Reduced app package size, typically up to 2.5x smaller.
- Faster startup time, typically up to 2x faster.
- Faster build time.

For more information, see [Native AOT deployment on iOS and Mac Catalyst](~/deployment/nativeaot.md).

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

For more information, see [Native embedding](~/platform-integration/native-embedding.md?view=net-maui-9&preserve-view=true).

## Project templates

The **.NET MAUI App** project template includes the ability to create a fully functional todo app, using controls from the Syncfusion Toolkit for .NET MAUI to visualize data and persist it to a local database based on SQLite. To create this todo app, create a new project in Visual Studio using the **.NET MAUI App** project template, and then check the **Include sample content** checkbox in the **Additional information** window:

:::image type="content" source="media/dotnet-9/syncfusion-sample-pages.png" alt-text="Screenshot of how to add SyncFusion sample pages to your .NET MAUI app project.":::

The todo app can also be created from the .NET CLI with the `--sample-content` or `-sc` option:

```dotnetcli
dotnet new maui --sample-content -n MyProject
```

.NET MAUI 9 also adds a **.NET MAUI Blazor Hybrid and Web App** project template to Visual Studio that creates a solution with a .NET MAUI Blazor Hybrid app with a Blazor Web app, which share common code in a Razor class library project.

The template can also be used from the .NET CLI:

```dotnetcli
dotnet new maui-blazor-web -n MyProject
```

## Resource dictionaries

In .NET MAUI 9, a stand-alone XAML <xref:Microsoft.Maui.Controls.ResourceDictionary> (which isn't backed by a code-behind file) defaults to having its XAML compiled. To opt out of this behavior, specify `<?xaml-comp compile="false" ?>` after the XML header.

## Trimming

Full trimming is now supported by setting the `$(TrimMode)` MSBuild property to `full`. For more information, see [Trim a .NET MAUI app](~/deployment/trimming.md).

### Trimming incompatibilities

[!INCLUDE [Trimming incompatibilities](../deployment/includes/trimming-incompatibilities.md)]

### Trimming feature switches

[!INCLUDE [Trimming feature switches](../deployment/includes/feature-switches.md)]

## Windows app deployment

When debugging and deploying a new .NET MAUI project to Windows, the default behavior in .NET MAUI 9 is to deploy an unpackaged app. For more information, see [Deploy and debug your .NET MAUI app on Windows](~/windows/setup.md).

## XAML compiler error codes

In .NET MAUI 9, the XAML compiler error codes have changed their prefix from `XFC` to `XC`. Ensure that you update the `$(WarningsAsErrors)`, `$(WarningsNotAsErrors)`, and `$(NoWarn)` build properties in your app's project files, if used, to reference the new prefix.

## XAML markup extensions

All classes that implement <xref:Microsoft.Maui.Controls.Xaml.IMarkupExtension>, <xref:Microsoft.Maui.Controls.Xaml.IMarkupExtension`1>, <xref:Microsoft.Maui.Controls.Xaml.IValueProvider>, and <xref:Microsoft.Maui.Controls.IExtendedTypeConverter> need to be annotated with either the <xref:Microsoft.Maui.Controls.Xaml.RequireServiceAttribute> or <xref:Microsoft.Maui.Controls.Xaml.AcceptEmptyServiceProviderAttribute>. This is required due to a XAML compiler optimization introduced in .NET MAUI 9 that enables the generation of more efficient code, which helps reduce the app size and improve runtime performance.

For information about annotating markup extensions with these attributes, see [Service providers](~/xaml/markup-extensions/create.md?view=net-maui-9&preserve-view=true#service-providers).

## Xcode sync

.NET MAUI 9 includes Xcode sync (`xcsync`), which is a tool that enables you to use Xcode for managing Apple specific files with .NET projects, including asset catalogs, plist files, storyboards, and xib files. The tool has two main commands to generate a temporary Xcode project from a .NET project, and to synchronize changes from the Xcode files back to your .NET project.

> [!IMPORTANT]
> xcsync is currently in preview.

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

The <xref:Microsoft.Maui.Controls.Frame> control is marked as obsolete in .NET MAUI 9, and will be completely removed in a future release. The <xref:Microsoft.Maui.Controls.Border> control should be used in its place.

When replacing a <xref:Microsoft.Maui.Controls.Frame> with a <xref:Microsoft.Maui.Controls.Border>, the <xref:Microsoft.Maui.Controls.Frame.BorderColor?displayProperty=nameWithType> property value should become the <xref:Microsoft.Maui.Controls.Border.Stroke?displayProperty=nameWithType> property value, and the <xref:Microsoft.Maui.Controls.Frame.CornerRadius?displayProperty=nameWithType> property value should become part of the <xref:Microsoft.Maui.Controls.Border.StrokeShape?displayProperty=nameWithType> property value. In addition, it may be necessary to duplicate the `Margin` value as the `Padding` value.

The following example shows equivalent <xref:Microsoft.Maui.Controls.Frame> and <xref:Microsoft.Maui.Controls.Border> elements in XAML:

```xaml
<Frame BorderColor="DarkGray"
       CornerRadius="5"
       Margin="20"
       HeightRequest="360"
       HorizontalOptions="Center"
       VerticalOptions="Center" />

<Border Stroke="DarkGray"
        StrokeShape="RoundRectangle 5"
        Margin="20"
        Padding="20"
        HeightRequest="360"
        HorizontalOptions="Center"
        VerticalOptions="Center" />
```

For more information see [Border](~/user-interface/controls/border.md).

### MainPage

Instead of defining the first page of your app using the <xref:Microsoft.Maui.Controls.Application.MainPage> property on an <xref:Microsoft.Maui.Controls.Application> object, you should set the <xref:Microsoft.Maui.Controls.Window.Page> property on a <xref:Microsoft.Maui.Controls.Window> to the first page of your app. This is what happens internally in .NET MAUI when you set the <xref:Microsoft.Maui.Controls.Application.MainPage> property, so there's no behavior change introduced by the <xref:Microsoft.Maui.Controls.Application.MainPage> property being marked as obsolete.

The following example shows setting the <xref:Microsoft.Maui.Controls.Window.Page> property on a <xref:Microsoft.Maui.Controls.Window>, via the `CreateWindow` override:

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

Code that accesses the `Application.Current.MainPage` property should now access the `Application.Current.Windows[0].Page` property for apps with a single window. For apps with multiple windows, use the `Application.Current.Windows` collection to identify the correct window and then access the `Page` property. In addition, each element features a `Window` property, that's accessible when the element is part of the current window, from which the `Page` property can be accessed (`Window.Page`). Platform code can retrieve the app's <xref:Microsoft.Maui.IWindow> object with the `Microsoft.Maui.Platform.GetWindow` extension method.

While the <xref:Microsoft.Maui.Controls.Application.MainPage> property is retained in .NET MAUI 9 it will be completely removed in a future release.

### Compatibility layouts

The compatibility layout classes in the <xref:Microsoft.Maui.Controls.Compatibility> namespace have been obsoleted.

### Legacy measure calls

The following <xref:Microsoft.Maui.Controls.VisualElement> measure methods have been obsoleted:

- <xref:Microsoft.Maui.Controls.VisualElement.OnMeasure%2A?displayProperty=nameWithType>
- <xref:Microsoft.Maui.Controls.VisualElement.Measure(System.Double,System.Double,Microsoft.Maui.Controls.MeasureFlags)?displayProperty=nameWithType>

These are legacy measure methods that don't function correctly with .NET MAUI layout expectations.

As a replacement, the <xref:Microsoft.Maui.Controls.VisualElement.Measure(System.Double,System.Double)?displayProperty=nameWithType> method has been introduced. This method returns the minimum size that an element needs in order to be displayed on a device. Margins are excluded from the measurement, but are returned with the size. This is the preferred method to call when measuring a view.

In addition, the <xref:Microsoft.Maui.SizeRequest> struct is obsoleted. Instead, <xref:Microsoft.Maui.Graphics.Size> should be used.

## Upgrade from .NET 8 to .NET 9

To upgrade your .NET MAUI projects from .NET 8 to .NET 9, first install .NET 9 and the .NET MAUI workload with [Visual Studio 17.12+](https://visualstudio.microsoft.com/vs/), or with [Visual Studio Code and the .NET MAUI extension and .NET and the .NET MAUI workloads](~/get-started/installation.md?tabs=visual-studio-code), or with the [standalone installer](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) and the `dotnet workload install maui` command.

### Update project file

To update your .NET MAUI app from .NET 8 to .NET 9 open the app's project file (*.csproj*) and change the Target Framework Monikers (TFMs) from 8 to 9. If you're using a TFM such as `net8.0-ios15.2` be sure to match the platform version or remove it entirely. The following example shows the TFMs for a .NET 8 project:

```xml
<TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst;net8.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
```

The following example shows the TFMs for a .NET 9 project:

```xml
<TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst;net9.0-tizen</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net9.0-windows10.0.19041.0</TargetFrameworks>
```

If your app's project file references a .NET 8 version of the [`Microsoft.Maui.Controls`](https://www.nuget.org/packages/Microsoft.Maui.Controls/) NuGet package, either directly or through the `$(MauiVersion)` build property, update this to a .NET 9 version. Then, remove the package reference for the [`Microsoft.Maui.Controls.Compatibility`](https://www.nuget.org/packages/Microsoft.Maui.Controls.Compatibility/) NuGet package, provided that your app doesn't use any types from this package. In addition, update the package reference for the [`Microsoft.Extensions.Logging.Debug`](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug/) NuGet package to the latest .NET 9 release.

If your app targets iOS or Mac Catalyst, update the `$(SupportedOSPlatformVersion)` build properties for these platforms to 15.0:

```xml
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">15.0</SupportedOSPlatformVersion>
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">15.0</SupportedOSPlatformVersion>
```

When debugging and deploying a new .NET MAUI project to Windows, the default behavior in .NET 9 is to deploy an unpackaged app. To adopt this behavior, see [Convert a packaged .NET MAUI Windows app to unpackaged](~/windows/setup.md#convert-a-packaged-net-maui-windows-app-to-unpackaged).

Prior to building your upgraded app for the first time, delete the `bin` and `obj` folders. Any build errors and warnings will guide you towards next steps.

### Update XAML compiler error codes

XAML compiler error codes have changed their prefix from `XFC` to `XC`, so update the `$(WarningsAsErrors)`, `$(WarningsNotAsErrors)`, and `$(NoWarn)` build properties in your app's project file, if used, to reference the new prefix.

### Address new XAML compiler warnings for compiled bindings

Build warnings will be produced for bindings that don't use compiled bindings, and these will need to be addressed. For more information, see [XAML compiled bindings warnings](~/fundamentals/data-binding/compiled-bindings.md?view=net-maui-9.0&preserve-view=true#xaml-compiled-bindings-warnings).

### Update XAML markup extensions

XAML markup extensions will need to be annotated with either the <xref:Microsoft.Maui.Controls.Xaml.RequireServiceAttribute> or <xref:Microsoft.Maui.Controls.Xaml.AcceptEmptyServiceProviderAttribute>. This is required due to a XAML compiler optimization that enables the generation of more efficient code, which helps reduce the app size and improve runtime performance. For more information, see [Service providers](~/xaml/markup-extensions/create.md?view=net-maui-9&preserve-view=true#service-providers).

### Address deprecated APIs

.NET MAUI 9 deprecates some APIs, which will be completely removed in a future release. Therefore, address any build warnings about deprecated APIs. For more information, see [Deprecated APIs](#deprecated-apis).

### Adopt compiled bindings that set the Source property

You can opt into compiling bindings that set the `Source` property, to take advantage of better runtime performance. For more information, see [Compile bindings that define the `Source` property](~/fundamentals/data-binding/compiled-bindings.md?view=net-maui-9.0&preserve-view=true#compile-bindings-that-define-the-source-property).

### Adopt compiled bindings in C\#

You can opt into compiling binding expressions that are declared in code, to take advantage of better runtime performance. For more information, see [Compiled bindings in code](~/fundamentals/data-binding/compiled-bindings.md#compiled-bindings-in-code).

### Adopt full trimming

You can adopt into using full trimming, to reduce the overall size of your app, by setting the `$(TrimMode)` MSBuild property to `full`. For more information, see [Trim a .NET MAUI app](~/deployment/trimming.md).

### Adopt NativeAOT deployment on supported platforms

You can opt into Native AOT deployment on iOS and Mac Catalyst. Native AOT deployment produces a .NET MAUI app that's been ahead-of-time (AOT) compiled to native code. For more information, see [Native AOT deployment on iOS and Mac Catalyst](~/deployment/nativeaot.md).

## .NET for Android

.NET for Android in .NET 9, which adds support for API 35, includes work to reduce build times, and to improve the trimability of apps to reduce size and improve performance. For more information about .NET for Android in .NET 9, see the following release notes:

- [.NET for Android 9](https://github.com/dotnet/android/releases/tag/35.0.7)
- [.NET for Android 9 RC2](https://github.com/dotnet/android/releases/tag/35.0.0-rc.2.152)
- [.NET for Android 9 RC1](https://github.com/dotnet/android/releases/tag/35.0.0-rc.1.80)
- [.NET for Android 9 Preview 7](https://github.com/xamarin/xamarin-android/releases/tag/35.0.0-preview.7.41)
- [.NET for Android 9 Preview 6](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.6.340)
- [.NET for Android 9 Preview 5](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.5.308)
- [.NET for Android 9 Preview 4](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.4.272)
- [.NET for Android 9 Preview 3](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.3.231)
- [.NET for Android 9 Preview 2](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.2.189)
- [.NET for Android 9 Preview 1](https://github.com/xamarin/xamarin-android/releases/tag/34.99.0-preview.1.151)

### Asset packs

.NET for Android in .NET 9 introduces the ability to place assets into a separate package, known as an *asset pack*. This enables you to upload games and apps that would normally be larger than the basic package size allowed by Google Play. By putting these assets into a separate package you gain the ability to upload a package which is up to 2Gb in size, rather than the basic package size of 200Mb.

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

.NET for Android in .NET 9 adds .NET bindings for Android 15 (API 35). To build for these APIs, update the target framework of your project to `net9.0-android`:

```xml
<TargetFramework>net9.0-android</TargetFramework>
```

> [!NOTE]
> You can also specify `net9.0-android35` as a target framework, but the number 35 will probably change in future .NET releases to match newer Android OS releases.

### 64-bit architectures by default

.NET for Android in .NET 9 no longer builds the following runtime identifiers (RIDs) by default:

* `android-arm`
* `android-x86`

This should improve build times and reduce the size of Android `.apk` files. Note that Google Play supports splitting up app bundles per architecture.

If you need to build for these architectures, you can add them to your project file (*.csproj*):

```xml
<RuntimeIdentifiers>android-arm;android-arm64;android-x86;android-x64</RuntimeIdentifiers>
```

Or in a multi-targeted project:

```xml
<RuntimeIdentifiers Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">android-arm;android-arm64;android-x86;android-x64</RuntimeIdentifiers>
```

### Android marshal methods

Improvements to Android marshal methods in .NET 9 has made the feature work more reliably in applications but is not yet the default. Enabling this feature has resulted in a [~10% improvement in performance in a test app](https://github.com/dotnet/android/commit/a9706b6ef0429250ecaf1e500d77cd19e94e2eb5).

Android marshal methods can be enabled in your project file (*.csproj*) via the `$(AndroidEnableMarshalMethods)` property:

```xml
<PropertyGroup>
    <AndroidEnableMarshalMethods>true</AndroidEnableMarshalMethods>
</PropertyGroup>
```

For specific details about the feature, see the [feature documentation](https://github.com/dotnet/android/blob/main/Documentation/guides/internals/JavaJNI_Interop.md#marshal-methods) or [implementation](https://github.com/dotnet/android/commit/8bc7a3e84f95e70fe12790ac31ecd97957771cb2) on GitHub.

### Trimming enhancements

In .NET 9, the Android API assemblies (*Mono.Android.dll*, *Java.Interop.dll*) are now fully trim-compatible. To opt into full trimming, set the `$(TrimMode)` property in your project file (*.csproj*):

```xml
<PropertyGroup>
    <TrimMode>Full</TrimMode>
</PropertyGroup>
```

This also enables trimming analyzers, so that warnings are introduced for any problematic C# code.

For more information, see [Trimming granularity](/dotnet/core/deploying/trimming/trimming-options#trimming-granularity).

## .NET for iOS

.NET 9 on iOS, tvOS, Mac Catalyst, and macOS uses Xcode 16.0 for the following platform versions:

- iOS: 18.0
- tvOS: 18.0
- Mac Catalyst: 18.0
- macOS: 15.0

For more information about .NET 9 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 9](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-xcode16.0-9617)
- [.NET 9.0.1xx RC2](https://github.com/xamarin/xamarin-macios/releases/tag/dotnet-9.0.1xx-rc2-9600)
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

### Trimming enhancements

In .NET 9, the iOS and Mac Catalyst assemblies (*Microsoft.iOS.dll*, *Microsoft.MacCatalyst.dll* etc.) are now fully trim-compatible. To opt into full trimming, set the `$(TrimMode)` property in your project file (*.csproj*):

```xml
<PropertyGroup>
    <TrimMode>Full</TrimMode>
</PropertyGroup>
```

This also enables trimming analyzers, so that warnings are introduced for any problematic C# code.

For more information, see [Trimming granularity](/dotnet/core/deploying/trimming/trimming-options#trimming-granularity).

### Native AOT for iOS & Mac Catalyst

In .NET for iOS 9, native Ahead of Time (AOT) compilation for iOS and Mac Catalyst takes advantage of full trimming to reduce your app's package size and startup performance. NativeAOT builds upon full trimming, by also opting into a new runtime.

> [!IMPORTANT]
> Your app and it's dependencies must be fully trimmable in order to utilize this feature.

NativeAOT requires applications to be built with zero trimmer warnings, in order to prove the application will work correctly at runtime.

## See also

- [What's new in .NET 9](/dotnet/core/whats-new/dotnet-9/overview).
- [Our Vision for .NET 9](https://devblogs.microsoft.com/dotnet/our-vision-for-dotnet-9/)
