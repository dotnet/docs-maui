---
title: What's new in .NET MAUI for .NET 10
description: Learn about the new features introduced in .NET MAUI for .NET 10.
ms.date: 11/11/2025
---

# What's new in .NET MAUI for .NET 10

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 10 is to improve product quality. For information about what's new in each .NET MAUI in .NET 10 release, see the following release notes:

- [.NET MAUI in .NET 10 Release Candidate 1](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 7](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview7/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 6](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview6/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 5](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview5/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 4](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview4/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 3](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview3/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 2](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview2/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 1](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview1/dotnetmaui.md)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 10, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

## .NET Aspire integration

.NET MAUI for .NET 10 includes a new project template that creates a .NET Aspire service defaults project for .NET MAUI. This provides a set of extension methods that connect telemetry and service discovery to your app.

To connect telemetry and service discovery to your app, modify the `CreateMauiApp` method in your `MauiProgram` class to invoke the `AddServiceDefaults` method, from the .NET Aspire service defaults project, on the `MauiAppBuilder` object:

```csharp
builder.AddServiceDefaults();
```

The `AddServiceDefaults` method performs the following tasks:

- Configures OpenTelemetry metrics and tracing.
- Adds service discovery functionality.
- Configures <xref:System.Net.Http.HttpClient> to work with service discovery.

> [!IMPORTANT]
> The .NET Aspire service defaults project is designed for sharing the *Extensions.cs* file and its functionality. Don't include other shared functionality or models in this project. Instead, use a shared class library project for those purposes.

For more information, see [.NET Aspire service defaults](/dotnet/aspire/fundamentals/service-defaults).

## Animation

The `FadeTo`, `LayoutTo`, `RelRotateTo`, `RelScaleTo`, `RotateTo`, `RotateXTo`, `RotateYTo`, `ScaleTo`, `ScaleXTo`, `ScaleYTo`, and `TranslateTo` methods have been deprecated and replaced with the `FadeToAsync`, `LayoutToAsync`, `RelRotateToAsync`, `RelScaleToAsync`, `RotateToAsync`, `RotateXToAsync`, `RotateYToAsync`, `ScaleToAsync`, `ScaleXToAsync`, `ScaleYToAsync`, and `TranslateToAsync` methods.

For more information, see [Basic animation](~/user-interface/animation/basic.md?view=net-maui-10.0&preserve-view=true).

## Controls

.NET MAUI in .NET 10 includes control enhancements and deprecations.

### Accelerator

Removed from Microsoft.Maui.Controls. Use `KeyboardAccelerator`.

### ClickGestureRecognizer

Removed. Use `TapGestureRecognizer`.

### CollectionView and CarouselView

.NET MAUI in .NET 9 included two optional handlers on iOS and Mac Catalyst that brought performance and stability improvements to <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>. In .NET 10, these are the default handlers for <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>.

### Compatibility.Layout

This is now removed from templates and releases. Use .NET MAUI layouts.

### Editor and Entry on Android

On Android, the <xref:Microsoft.Maui.Controls.Editor> and <xref:Microsoft.Maui.Controls.Entry> views change their native views from `AppCompatEditText` to `MauiAppCompatEditText`, which adds support for the `SelectionChanged` event.

### HybridWebView

<xref:Microsoft.Maui.Controls.HybridWebView> gains an <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> overload that invokes a specified JavaScript method without specifying any information about the return type. For more information, see [Invoke JavaScript methods that don't return a value](~/user-interface/controls/hybridwebview.md?view=net-maui-10.0&preserve-view=true#invoke-javascript-methods-that-dont-return-a-value).

By default, any exceptions that are thrown by your JavaScript code will be sent to .NET, where they're re-thrown as .NET exceptions.

In addition, you can customize initialization and access platform web views to tweak settings when the native view is ready (for example, use `RunAfterInitialize` on Windows or adjust `WKWebView` configuration on iOS/Mac Catalyst). For details, see [Customize initialization and access platform web views](~/user-interface/controls/hybridwebview.md?view=net-maui-10.0&preserve-view=true#customize-initialization-and-access-platform-web-views).

#### New Events

Adds initialization events to `HybridWebView` following the same pattern as `BlazorWebView`, enabling platform-specific customization before and after initialization.

- `WebViewInitializing` - Fired before `WebView` creation, allows configuration of platform-specific initialization parameters
- `WebViewInitialized` - Fired after `WebView` creation, provides access to the native `WebView` instance and settings

### Intercept Web Requests

You can now intercept and respond to web requests made from `BlazorWebView` and `HybridWebView`. This allows for scenarios such as modifying headers, redirecting requests, or supplying local responses.

```csharp
webView.WebResourceRequested += (s, e) =>
{
    if (e.Uri.ToString().Contains("api/secure"))
    {
        e.Handled = true;
        e.SetResponse(200, "OK", "application/json", GetCustomStream());
    }
};
```

Another example from XAML:

```xml
<HybridWebView WebResourceRequested="HybridWebView_WebResourceRequested" />
```

```csharp
private void HybridWebView_WebResourceRequested(object sender, HybridWebViewWebResourceRequestedEventArgs e)
{
  // NOTES:
  // * This method MUST be synchronous, as it is called from the WebView's thread.
  // * This method MUST return a response (even if it is not yet complete), otherwise the 
  //   WebView may freeze or return a error response.
  // * The response must be set using the SetResponse method of the event args.

  // Only handle requests for the specific image URL
  if (!e.Uri.ToString().Contains("sample-image.png"))
    return;

  // Prevent the default behavior of the web view
  e.Handled = true;

  // Return the stream or task of stream that contains the content
  // NOTE: the method is NOT awaited, the WebView will continue to load the content
  e.SetResponse(200, "OK", "image/png", GetStreamAsync());
  }
```

### ListView

<xref:Microsoft.Maui.Controls.ListView> has been deprecated, along with <xref:Microsoft.Maui.Controls.EntryCell>, <xref:Microsoft.Maui.Controls.ImageCell>, <xref:Microsoft.Maui.Controls.SwitchCell>, <xref:Microsoft.Maui.Controls.TextCell>, and <xref:Microsoft.Maui.Controls.ViewCell>. Instead, <xref:Microsoft.Maui.Controls.CollectionView> should be used.

> [!NOTE]
> <xref:Microsoft.Maui.Controls.Cell> hasn't been deprecated because it's currently used for source generation. However, it should be considered deprecated.

### Page

The `IsBusy` property is marked obsolete. In .NET 10 we recommend using <xref:Microsoft.Maui.Controls.ActivityIndicator> (or a similar UX) for page or view-specific busy states and avoiding long-lived global busy indicators. For guidance, see [ContentPage](~/user-interface/pages/contentpage.md?view=net-maui-10.0&preserve-view=true).

### Picker

Programmatically control picker state with new Open/Close API.

In addition, <xref:Microsoft.Maui.Controls.DatePicker> and <xref:Microsoft.Maui.Controls.TimePicker> support nullable selection in .NET 10. This enables clearing values and binding to nullable types:

- <xref:Microsoft.Maui.Controls.DatePicker.Date> is now <xref:System.DateTime?>; <xref:Microsoft.Maui.Controls.DatePicker.MinimumDate> and <xref:Microsoft.Maui.Controls.DatePicker.MaximumDate> are also nullable.
- <xref:Microsoft.Maui.Controls.TimePicker.Time> is now <xref:System.TimeSpan?>.

For examples and platform notes, see [DatePicker](~/user-interface/controls/datepicker.md?view=net-maui-10.0&preserve-view=true) and [TimePicker](~/user-interface/controls/timepicker.md?view=net-maui-10.0&preserve-view=true).

## RefreshView

Added `IsRefreshEnabled` property to be distinct from `IsEnabled` and make the behavior consistent across platforms.

```xml
<RefreshView IsRefreshEnabled="false">
    <!-- Login form remains usable -->
    <StackLayout>
        <Entry Placeholder="Username" />
        <Entry Placeholder="Password" />
        <Button Text="Login" />
    </StackLayout>
</RefreshView>
```

### SearchBar

<xref:Microsoft.Maui.Controls.SearchBar> gains a `SearchIconColor` bindable property that sets the color of the search icon:

```xaml
<SearchBar Placeholder="Search items..."
           SearchIconColor="Blue" />
```

<xref:Microsoft.Maui.Controls.SearchBar> also gains a `ReturnType` bindable property, of type <xref:Microsoft.Maui.ReturnType>, that specifies the appearance of the return button. The default value of this property is `Search`.

For more information, see [SearchBar](~/user-interface/controls/searchbar.md?view=net-maui-10.0&preserve-view=true).

### Switch

<xref:Microsoft.Maui.Controls.Switch> gains an `OffColor` bindable property that sets the color of the switch when it's in the off state:

```xaml
<Switch OffColor="Red"
        OnColor="Green" />
```

For more information, see [Switch](~/user-interface/controls/switch.md?view=net-maui-10.0&preserve-view=true).

### TableView

<xref:Microsoft.Maui.Controls.TableView> has been deprecated. Instead, <xref:Microsoft.Maui.Controls.CollectionView> should be used.

### Vibration and HapticFeedback

Added `IsSupported` to check platform support.

### WebView on Android

When videos are hosted in a <xref:Microsoft.Maui.Controls.WebView> on Android, they can now be played fullscreen by including `allowfullscreen` in the `iframe`.

For more information, see [Play video fullscreen](~/user-interface/controls/webview.md?view=net-maui-10.0&preserve-view=true#play-video-full-screen-on-android).

You can also programmatically enable or disable JavaScript execution on Android with a .NET 10 platform-specific API. For details and a code example, see [WebView](~/user-interface/controls/webview.md?view=net-maui-10.0&preserve-view=true).

### SearchHandler

<xref:Microsoft.Maui.Controls.SearchHandler> adds APIs to programmatically show or hide the soft keyboard, making it easier to control focus and user input flow:

- `ShowSoftInputAsync`
- `HideSoftInputAsync`

For examples and platform notes, see [Search](~/fundamentals/shell/search.md?view=net-maui-10.0&preserve-view=true). If you use reflection-based properties such as `DisplayMemberName`, review the [trimming guidance](~/deployment/trimming.md?view=net-maui-10.0&preserve-view=true) for feature switches and linker configuration.

## Shell

.NET 10 introduces a toggle to control the shell navigation bar visibility animation. Use `Shell.NavBarVisibilityAnimationEnabled` to enable or disable the animation when the navigation bar shows or hides. See [Shell appearance and behavior](~/fundamentals/shell/index.md?view=net-maui-10.0&preserve-view=true).

## Diagnostics

We've added comprehensive diagnostics and metrics tracking for .NET MAUI applications, focusing on layout performance monitoring with an extensible architecture for future observability needs.

**Core Diagnostics Infrastructure:**

- **ActivitySource**: `"Microsoft.Maui"` - Tracks layout operations with detailed timing
- **Metrics**: `"Microsoft.Maui"` - Records counters and histograms for performance analysis
- **Feature Switch**: `System.Diagnostics.Metrics.Meter.IsSupported` - Runtime enable/disable for AOT/trimming

**Layout Performance Tracking:**

- Instruments `IView.Measure()` and `IView.Arrange()` operations
- Records timing data and operation counts with rich contextual tags
- Zero-allocation struct-based instrumentation using `using` pattern

| Metric Name | Type | Description |
|-------------|------|-------------|
| `maui.layout.measure_count` | Counter | Number of measure operations |
| `maui.layout.measure_duration` | Histogram | Time spent measuring (ns) |
| `maui.layout.arrange_count` | Counter | Number of arrange operations |
| `maui.layout.arrange_duration` | Histogram | Time spent arranging (ns) |

See [pull request #31058](https://github.com/dotnet/maui/pull/31058) for more details.

## Window

Added ability to enable/disable the minimize and maximize buttons on Windows.

## MessagingCenter

<xref:Microsoft.Maui.Controls.MessagingCenter> has been made internal in .NET 10. Usage of it in your code can be replaced with `WeakReferenceMessenger` in the [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm) NuGet package. For more information, see [Messenger](/windows/communitytoolkit/mvvm/messenger).

## Platform features

.NET MAUI's platform features have received some updates in .NET 10.

### Display a modal page as a popover on iOS and Mac Catalyst

.NET MAUI for .NET 10 adds a platform-specific that displays a modal page as a popover on iOS and Mac Catalyst. It's consumed by setting the `Page.ModalPopoverSourceView` bindable property to a `View` that defines the source of the modal, the `Page.ModalPopoverRect` bindable property to a <xref:System.Drawing.Rectangle> that defines the rectangle within the view from which the popover will originate, and the `Page.ModalPresentationStyle` bindable property to `Popover`:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

public partial class PopoverPage : ContentPage
{
    public PopoverPage(View modal, Rectangle rectangle)
    {
        InitializeComponent();
        On<iOS>().SetModalPopoverView(modal);
        On<iOS>().SetModalPopoverRect(rectangle);
        On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.Popover);
    }
}
```

Then, navigate to the modal page with the `Navigation.PushModalAsync` method:

```csharp
Page modalPage = new PopoverPage(originButton, Rectangle.Empty);
await Navigation.PushModalAsync(modalPage);
```

For more information, see [Display a modal page as a popover on iOS and Mac Catalyst](~/ios/platform-specifics/page-popover.md).

### Geolocation

The <xref:Microsoft.Maui.Devices.Sensors.Geolocation> class gains a read-only `IsEnabled` property that can be used to determine if location services have been enabled on the device.

### iOS and Mac Catalyst compatibility AccessibilityExtensions

The following iOS compatibility `AccessibilityExtensions` extension methods, in the `Microsoft.Maui.Controls.Compatibility.Platform.iOS`, have been deprecated:

- `SetAccessibilityHint`
- `SetAccessibilityLabel`
- `SetAccessibilityHint`
- `SetAccessibilityLabel`

Instead, the `Microsoft.Maui.Platform.UpdateSemantics` method should be used.

### iOS and Mac Catalyst MauiWebViewNavigationDelegate overrides

The following `MauiWebViewNavigationDelegate` methods, in the `Microsoft.Maui.Platform` namespace, are now overridable:

- `DecidePolicy`
- `DidFailNavigation`
- `DidFailProvisionalNavigation`
- `DidFinishNavigation`

### MediaPicker

#### EXIF

The `MediaPicker` now automatically handles EXIF information when working with images:

- **Auto-rotate images**: Images are automatically rotated based on their EXIF orientation data
- **Preserve EXIF information**: Original EXIF metadata is preserved when using MediaPicker

This ensures that images appear correctly oriented regardless of how they were captured or stored on the device.

#### Pick by size

The `MediaPicker` has been extended with support for selecting multiple files and compressing images directly from the API using `MaximumWidth` and `MaximumHeight` parameters.

```csharp
var result = await MediaPicker.PickMultipleAsync(new MediaPickerOptions
{
    MaximumWidth = 1024,
    MaximumHeight = 768
});
```

### Text to speech

The <xref:Microsoft.Maui.Media.SpeechOptions> class gains a `Rate` property that controls the speech rate when using <xref:Microsoft.Maui.Media.TextToSpeech> functionality. For more information, see [Text-to-Speech settings](~/platform-integration/device-media/text-to-speech.md?view=net-maui-10.0&preserve-view=true#settings).

### Web authentication

The <xref:Microsoft.Maui.Authentication.WebAuthenticator> gains a <xref:Microsoft.Maui.Authentication.IWebAuthenticator.AuthenticateAsync%2A> method overload that enables an authentication flow to be cancelled programatically with a <xref:System.Threading.CancellationToken> argument.

## Pop-ups

The `DisplayAlert` and `DisplayActionSheet` methods have been deprecated and replaced with the `DisplayAlertAsync` and `DisplayActionSheetAsync` methods.

For more information, see [Display pop-ups](~/user-interface/pop-ups.md?view=net-maui-10.0&preserve-view=true).

## SafeArea Enhancements

This release introduces significant improvements to SafeArea management:

- **Enhanced SafeAreaEdges control**: Improved `SafeAreaEdges` property with refined `SafeAreaEdges` enum for precise safe area behavior control
- **iOS SafeArea fixes**: Resolved issues with SafeArea management on iOS, including extra bottom space in ScrollView when using SafeAreaEdges
- **Improved defaults**: Fixed safe area defaults to provide more consistent behavior across platforms

The `SafeAreaEdges` property is available on these controls:

- **Layout**: Base layout class (inherited by `Grid`, `StackLayout`, `AbsoluteLayout`, `FlexLayout`, etc.)
- **ContentView**: Content view container
- **ContentPage**: Main page type
- **Border**: Border control
- **ScrollView**: Scrollable content container

The `SafeAreaEdges` enum provides granular control over safe area behavior:

```csharp
public enum SafeAreaEdges
{
    None = 0,          // Edge-to-edge content (no safe area padding)
    SoftInput = 1,     // Always pad for keyboard/soft input
    Container = 2,     // Flow under keyboard, stay out of bars/notch  
    Default = 4,       // Platform default behavior
    All = int.MaxValue // Obey all safe area insets
}

// Usage examples
<ContentPage SafeAreaEdges="Container">
    <!-- Content flows under keyboard but respects bars/notch -->
</ContentPage>

<ScrollView SafeAreaEdges="None">
    <!-- Edge-to-edge scrolling content -->
</ScrollView>

<Grid SafeAreaEdges="SoftInput">
    <!-- Grid respects keyboard but not other safe areas -->
</Grid>
```

For more information, see [Safe area layout](~/user-interface/safe-area.md).

## Secondary Toolbar Items

iOS and macOS now support secondary toolbar items, providing better alignment with platform conventions:

- **Modern iOS pattern**: Uses iOS 13+ APIs with pull-down menu design following iOS Human Interface Guidelines
- **Automatic detection**: Toolbar items with `Order="Secondary"` are automatically grouped into a secondary menu
- **Priority ordering**: Items are ordered within the secondary menu based on their `Priority` property
- **Platform positioning**: Menu appears on the far right (or left when RTL is enabled)
- **Customizable icon**: Developers can override the default ellipsis icon through a protected virtual method

```xml
<ContentPage.ToolbarItems>
    <!-- Primary toolbar items appear directly in the toolbar -->
    <ToolbarItem Text="Save" Order="Primary" Priority="0" />
    <ToolbarItem Text="Edit" Order="Primary" Priority="1" />
    
    <!-- Secondary toolbar items appear in the overflow menu -->
    <ToolbarItem Text="Share" Order="Secondary" Priority="0" />
    <ToolbarItem Text="Delete" Order="Secondary" Priority="1" />
    <ToolbarItem Text="Settings" Order="Secondary" Priority="2" />
</ContentPage.ToolbarItems>
```

The secondary items are grouped into a pull-down menu with the system ellipsis icon (`UIImage.GetSystemImage("ellipsis.circle")`) by default. When the menu is opened and an item's properties change, the menu automatically rebuilds and closes to reflect the updates.

## XAML

### Source Generator

> [!NOTE]
> The below are the instructions for .NET MAUI 10 RC1 and newer. Before RC1 enabling source generation was different. Please update to RC1 and use the below instructions. Any other code you have implemented to enable source generation can now be removed.

.NET MAUI now includes a source generator for XAML that improves build performance and enables better tooling support. This generator creates strongly-typed code for your XAML files at compile time, reducing runtime overhead and providing better IntelliSense support.

The source generator decorates generated types with the `[Generated]` attribute for better tooling integration and debugging support.

To enable XAML source generation, add the below property to the project file of your .NET MAUI project. Make sure to add it in a `PropertyGroup`, which can be a new one or an existing one in your csproj file.

```xml
<PropertyGroup>
  <MauiXamlInflator>SourceGen</MauiXamlInflator>
</PropertyGroup>
```

### Implicit and Global XML namespaces

.NET 10 Preview 5 introduces a cleaner **XML-namespace experience** for .NET MAUI that wipes out nearly all of the boiler-plate `xmlns:` lines you used to copy-paste at the top of every XAML file.

| What changed                         | How it works                                                                                                                                                                 |
|--------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Project-wide “global” namespaces** | `http://schemas.microsoft.com/dotnet/maui/global` is a new xmlns that you can use to aggregate multiple xmlns together. By default, it contains the maui xmlns (sourcegenerated), {YourNamespace}, and {YourNamespace}.Pages |
| **Backward compatible**              | Existing explicit `xmlns:` mappings still compile; add them only when you need to disambiguate duplicate type names.                                                        |
| **Implicit default namespace** (opt-in)      | When opting in, the compiler now injects `http://schemas.microsoft.com/dotnet/2021/maui` automatically, so you can drop root `xmlns` and `xmlns:x` lines.                               |

#### Before vs. after

```xml
<!-- .NET 8 style -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        xmlns:models="clr-namespace:MyApp.Models"
        xmlns:controls="clr-namespace:MyApp.Controls"
        x:Class="MyApp.MainPage">
    <controls:TagView x:DataType="models:Tag" />
</ContentPage>
````

```xml
<!-- .NET 10 Preview 5 -->
<ContentPage 
    xmlns="http://schemas.microsoft.com/dotnet/maui/global"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MyApp.MainPage">
    <TagView x:DataType="Tag" />
</ContentPage>
```

Changes:

- No need to declare the `xmlns:models` or `xmlns:controls` because they are declared globally in a `GlobalXmlns.cs` file
- No prefixes required for `TagView` or `Tag`

```xml
<!-- .NET 10 Preview 5 plus opt-in -->
<ContentPage x:Class="MyApp.MainPage">
    <TagView x:DataType="Tag" />
</ContentPage>
```

Changes:

- No need to declare `xmlns` or `xmlns:x` since they are implicitly added to the global namespace

#### How to adopt

Create a new project to see the streamlined experience, or upgrade your existing project with the details below.

1. **Upgrade** the project to `net10.0-` target frameworks.
2. **Add** one assembly-level file (e.g. `GlobalXmlns.cs`) that maps your CLR namespaces.
3. **Delete** redundant `xmlns:` lines and prefixes from your XAML.
   IntelliSense and Hot Reload keep working—just with much cleaner markup.

```xml
<PropertyGroup>
    <DefineConstants>$(DefineConstants);MauiAllowImplicitXmlnsDeclaration</DefineConstants>
    <EnablePreviewFeatures>true</EnablePreviewFeatures>
</PropertyGroup>
```

#### Sample `GlobalXmlns.cs`

```csharp
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "MyApp.Views")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "MyApp.Controls")]
[assembly: XmlnsDefinition(
    "http://schemas.microsoft.com/dotnet/maui/global",
    "MyApp.Converters")]
```

If you prefer to continue using the xmlns prefixes in your XAML, you can provide default prefixes for them in the `GlobalXmlns.cs` as well.

```csharp
using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

[assembly: XmlnsPrefix("MyApp.Controls","controls")]
```

Use them as before.

```xml
<ContentPage x:Class="MyApp.MainPage">
    <controls:TagView x:DataType="Tag" />
</ContentPage>
```

> ✨ **Tip:** You can register third-party libraries here too!

## XAML markup extensions

The <xref:Microsoft.Maui.Controls.Xaml.FontImageExtension> XAML markup extension has been deprecated. Instead, the <xref:Microsoft.Maui.Controls.FontImageSource> type should be used:

```xaml
<Button Text="Press me"
        Background="Transparent"
        TextColor="{AppThemeBinding Light=Black, Dark=White}"
        ImageSource="{FontImageSource Glyph=MyGlyph, Color={AppThemeBinding Light=Black, Dark=White}, FontFamily=FontAwesome, Size=18}" />
```

For convenience, property element syntax can also be used:

```xaml
<Button Text="Press me"
        Background="Transparent"
        TextColor="{AppThemeBinding Light=Black, Dark=White}" >
        <Button.ImageSource>
            <FontImageSource Glyph="MyGlyph"
                             Color="{AppThemeBinding Light=Black, Dark=White}"
                             FontFamily="FontAwesome"
                             Size="18" />
        </Button.ImageSource>
</Button>
```

For more information, see [Display font icons](~/user-interface/fonts.md#display-font-icons).

## .NET for Android

.NET for Android in .NET 10 adds support for API 36 and JDK 21, and includes work to reduce build times and improve performance. For more information about .NET for Android in .NET 10, see the following release notes:

- [.NET for Android 10 Release Candidate 1](https://github.com/dotnet/android/releases/tag/36.0.0-rc.1.285)
- [.NET for Android 10 Preview 7](https://github.com/dotnet/android/releases/tag/36.0.0-preview.7.229)
- [.NET for Android 10 Preview 6](https://github.com/dotnet/android/releases/tag/36.0.0-preview.6.169)
- [.NET for Android 10 Preview 5](https://github.com/dotnet/android/releases/tag/36.0.0-preview.5.116)
- [.NET for Android 10 Preview 4](https://github.com/dotnet/android/releases/tag/36.0.0-preview.4.80)
- [.NET for Android 10 Preview 3](https://github.com/dotnet/android/releases/tag/36.0.0-preview.3.22)
- [.NET for Android 10 Preview 2](https://github.com/dotnet/android/releases/tag/35.99.0-preview.2.205)
- [.NET for Android 10 Preview 1](https://github.com/dotnet/android/releases/tag/35.99.0-preview.1.140)

### (Experimental) CoreCLR

Enables Android apps to run on the CoreCLR runtime (instead of Mono). To use it, add the following to your project file for Android builds:

```xml
<!-- Use CoreCLR on Android -->
<PropertyGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'android'">
    <UseMonoRuntime>false</UseMonoRuntime>
</PropertyGroup>
```

Please try this in your applications and report any issues; when filing feedback, state that you are using UseMonoRuntime=false. Expect that application size is currently larger than with Mono and that debugging and some runtime diagnostics are not fully functional yet; these areas are actively being improved. This is an experimental feature and not intended for production use.

A detailed list of Android changes can be found on the [dotnet/android GitHub releases](https://github.com/dotnet/android/releases/).

### Android 16 (Baklava) beta 3 bindings

Google has released [Beta 3](https://android-developers.googleblog.com/2025/03/the-third-beta-of-android-16.html) of the Android 16 (API 36) SDK. While still classed as a preview, the ABIs have been declared stable and Google's final release of Android 16 is expected this summer.

API 36 is now the default in .NET 10, which means that API 36 will be used when the `$(TargetFramework)` build property is set to `net10.0-android`.

This will cause issues when using Visual Studio and Visual Studio Code with .NET 10, because they don't yet know about API 36, which will trigger an [XA5207](/dotnet/android/messages/xa5207) error. To fix this, you'll need to manually install the API36 *platform.jar* into your Android SDK directory or wait for a forthcoming update to Visual Studio.

### Recommended minimum supported Android API

The .NET for Android project templates have been updated to specify 24 (Nougat) as the default `$(SupportedOSPlatformVersion)` instead of 21 (Lollipop). This prevents you from hitting "desugaring" runtime crashes when using Java default interface methods.

While API 21 is still supported in .NET 10, it's recommended to update existing projects to API 24 to avoid unexpected runtime errors.

### `dotnet run` support

Previously, the `dotnet run` command wasn't supported for .NET for Android projects because it didn't accept parameters needed to specify which Android device or emulator to use.

In .NET 10, .NET for Android projects can be run using the `dotnet run` command:

```dotnetcli
// Run on the only attached Android physical device
dotnet run -p:AdbTarget=-d

// Run on the only running Android emulator
dotnet run -p:AdbTarget=-e

// Run on the specified Android physical device or emulator
dotnet run -p:AdbTarget="-s emulator-5554"
```

The `$(AdbTarget)` property is passed to `adb`. For more information, see [Issue shell commands](https://developer.android.com/tools/adb#shellcommands) on developer.android.com.

### Marshal methods enabled by default

In .NET 9, a [new way](https://github.com/dotnet/android/pull/7351) of creating the marshal methods needed for Java calling into C# code provided startup performance improvements. However, in .NET 9 they were off by default.

In .NET 10, they are enabled by default. Problems with these marshal methods often manifest as a hang at startup. If you're getting a hang on startup on .NET 10 previews that you didn't see on .NET 9, try disabling marshal methods by setting the `$(AndroidEnableMarshalMethods)` MSBuild property to `false` in your project file:

```xml
<PropertyGroup>
    <AndroidEnableMarshalMethods>false</AndroidEnableMarshalMethods>
</PropertyGroup>
```

If this fixes the hang, please file an [issue](https://github.com/dotnet/android/issues).

### `ArtifactFilename` metadata for `@(AndroidMavenLibrary)` item

[`@(AndroidMavenLibrary)`](/dotnet/android/binding-libs/binding-java-libs/binding-java-maven-library) was added in .NET 9 and allows a Java library to be automatically downloaded from Maven to be bound. Generally, this library is named `{artifact.Id}-{artifact.Version}.[jar|aar]`. However, this doesn't follow a standard and could be arbitrarily different.

In .NET 10 you can add the `ArtifactFilename` metadata to the `@(AndroidMavenLibrary)` MSBuild item to allow an alternative filename:

```xml
<ItemGroup>
    <AndroidMavenLibrary Include="com.facebook.react:react-android" Version="0.76.0" ArtifactFilename="react-android-0.76.0-release.aar" />
</ItemGroup>
```

### System.IO.Compression used for *.apk* creation

Historically, [dotnet/android-libzipsharp](https://github.com/dotnet/android-libzipsharp) was used to process ZIP archives and create *.aab* and *.apk* files.

In .NET 10:

- For command line `dotnet build` invocations, <xref:System.IO.Compression.ZipArchive> is used to create *.aab* and *.apk* files. This should result in faster build times.
- For Visual Studio builds, [dotnet/android-libzipsharp](https://github.com/dotnet/android-libzipsharp) is still used because the .NET Framework version of <xref:System.IO.Compression> can't be used.

### Visual Studio design time builds no longer invoke `aapt2`

In order to speed up design time builds, `aapt2` is no longer invoked. Instead, the `.aar` files and underlying Android resources are parsed directly. This reduces the time of a design time build for some unit tests from over 2s to under 600ms.

### Building with JDK 21

.NET for Android projects can now be built with JDK 21.

### `generator` output avoids potential System.Reflection.Emit usage

App startup and overall performance has been optimized by removing codepaths that may hit `System.Reflection.Emit` from "Java calling into C#" codepaths.

### `ApplicationAttribute.ManageSpaceActivity` no longer throws an `InvalidCastException`

Setting the `ApplicationAttribute.ManageSpaceActivity` property doesn't result in an XAGJS7007 error in .NET 10.

## .NET for iOS

.NET 10 on iOS, tvOS, Mac Catalyst, and macOS supports the following platform versions:

- iOS: 18.2
- tvOS: 18.2
- Mac Catalyst: 18.2
- macOS: 15.2

Preview 7 release includes Xcode 26 Beta 4 support for targeting .NET 9. We will include builds for .NET 10 in a subsequent release. To use these new bindings, target `net9.0-ios26` and/or `net9.0-maccatalyst26` and include `<NoWarn>$(NoWarn);XCODE_26_0_PREVIEW</NoWarn>` in your project file.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0-ios26</TargetFramework>
    <NoWarn>$(NoWarn);XCODE_26_0_PREVIEW</NoWarn>
    <!-- rest of your configuration -->
  </PropertyGroup>
</Project>
```

> **Note** Previously there was an issue with .NET MAUI `Shell` that prevents it from rendering on iOS 26. This has been fixed for .NET MAUI 9 Service Release 11 and .NET MAUI 10 RC1.

For more information about .NET 10 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

- [.NET 10.0.1xx Release Candidate 1](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-rc.1-10727)
- [.NET 10.0.1xx Preview 7](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview7-10601)
- [.NET 10.0.1xx Preview 6](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview6-10451)
- [.NET 10.0.1xx Preview 5](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview5-10622)
- [.NET 10.0.1xx Preview 4](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview4-10471)
- [.NET 10.0.1xx Preview 3](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview3-10695)
- [.NET 10.0.1xx Preview 2](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview2-10552)
- [.NET 10.0.1xx Preview 1](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview1-10322)

For information about known issues, see [Known issues in .NET 10](https://github.com/dotnet/macios/wiki/Known-issues-in-.NET10).

### Trimmer enabled in more configurations

The trimmer is now enabled in the following configurations:

- iOS Simulator/arm64 (all configurations)
- tvOS Simulator/arm64 (all configurations)
- Mac Catalyst/arm64 (all configurations)

### Trimmer warnings enabled by default

Trimmer warnings were previously suppressed, because the base class library produced trimmer warnings which means that it wasn't possible for you to fix all the trimmer warnings. However, in .NET 9 all the iOS trimmer warnings were fixed, and so trimmer warnings are now enabled by default. To disable trimmer warnings, set the `$(SuppressTrimAnalysisWarnings)` MSBuild property to `true` in your project file:

```xml
<PropertyGroup>
    <SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings>
</PropertyGroup>
```

### Bundling original resources in libraries

Library projects can have different types of bundle resources, such as storyboards, xibs, property lists, images, CoreML models, and texture atlases, and they're bundled into the compiled library as embedded resources.

Processing these resources, such as compiling storyboards or xibs, or optimizing property lists and images, is done before embedding but this complicates library builds because the processing:

- Needs to run on a Mac, because compiling storyboards/xibs can only be done on a Mac.
- Needs Apple's tool chain.
- Makes it impossible to perform decision-making based on the original resources when building the app.

Therefore, opt-in support for embedding the original resource in libraries was added in .NET 9, and it's now opt-out in .NET 10. To opt out of this behavior, set the `$(BundleOriginalResources)` MSBuild property to `false` in your project file:

```xml
<PropertyGroup>
    <BundleOriginalResources>false</BundleOriginalResources>
</PropertyGroup>
```

### Build binding projects on Windows

Binding projects are now built entirely on Windows, and so there's no need for a remote Mac. This makes building binding projects on Windows significantly faster.

### NSUrlSessionHandler no longer sets the TLS minimum supported protocol version for the session

Previously, <xref:Foundation.NSUrlSessionHandler> would initialize the <xref:Foundation.NSUrlSessionConfiguration.TLSMinimumSupportedProtocol?displayProperty=nameWithType> value from the <xref:System.Net.ServicePointManager.SecurityProtocol?displayProperty=nameWithType> property, but the <xref:System.Net.ServicePointManager> class is now deprecated. Therefore, you'll have to set the <xref:Foundation.NSUrlSessionConfiguration.TLSMinimumSupportedProtocol?displayProperty=nameWithType> value before creating the <xref:Foundation.NSUrlSessionHandler>:

```csharp
var sessionConfiguration = NSUrlSessionConfiguration.DefaultSessionConfiguration;
sessionConfiguration.TlsMinimumSupportedProtocolVersion = TlsProtocolVersion.Tls13;
var handler = new NSUrlSessionHandler(sessionConfiguration);
```

### NSUrlSessionHandler.BypassBackgroundSessionCheck property is ignored

The `NSUrlSessionHandler.BypassBackgroundSessionCheck` property exists for an old issue in the Mono runtime. This workaround is no longer required and so the property is ignored.

## See also

- [.NET MAUI roadmap](https://github.com/dotnet/maui/wiki/Roadmap)
- [What's new in .NET 10](/dotnet/core/whats-new/dotnet-10/overview)
