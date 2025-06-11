---
title: What's new in .NET MAUI for .NET 10
description: Learn about the new features introduced in .NET MAUI for .NET 10.
ms.date: 05/13/2025
---

# What's new in .NET MAUI for .NET 10

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 10 is to improve product quality. For information about what's new in each .NET MAUI in .NET 10 release, see the following release notes:

- [.NET MAUI in .NET 10 Preview 5](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview5/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 4](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview4/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 3](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview3/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 2](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview2/dotnetmaui.md)
- [.NET MAUI in .NET 10 Preview 1](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview1/dotnetmaui.md)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 10, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

## XAML with *implicit & global* XML namespaces (Preview 5)

.NET 10 Preview 5 introduces a cleaner **XML-namespace experience** for .NET MAUI that wipes out nearly all of the boiler-plate `xmlns:` lines you used to copy-paste at the top of every XAML file.

| What changed                         | How it works                                                                                                                                                                 |
|--------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Project-wide “global” namespaces** | `http://schemas.microsoft.com/dotnet/maui/global` is a new xmlns that you can use to aggregate multiple xmlns together. By default, it contains the maui xmlns (sourcegenerated), {YourNamespace}, and {YourNamespace}.Pages |
| **Backward compatible**              | Existing explicit `xmlns:` mappings still compile; add them only when you need to disambiguate duplicate type names.                                                        |
| **Implicit default namespace** (opt-in)      | When opting in, the compiler now injects `http://schemas.microsoft.com/dotnet/2021/maui` automatically, so you can drop root `xmlns` and `xmlns:x` lines.                               |

### Before vs. after

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

### How to adopt

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

### Sample `GlobalXmlns.cs`

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

## Intercept Web Requests

The `HybridWebView` now allows you to intercept when the browser requests a web resource in order to take action before it executes, such as adding a header to the request. The do this, add a listener to the `WebResourceRequested` event.

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

### CollectionView and CarouselView

.NET MAUI in .NET 9 included two optional handlers on iOS and Mac Catalyst that brought performance and stability improvements to <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>. In .NET 10, these are the default handlers for <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>.

### Editor and Entry on Android

On Android, the <xref:Microsoft.Maui.Controls.Editor> and <xref:Microsoft.Maui.Controls.Entry> views change their native views from `AppCompatEditText` to `MauiAppCompatEditText`, which adds support for the `SelectionChanged` event.

### HybridWebView

<xref:Microsoft.Maui.Controls.HybridWebView> gains an <xref:Microsoft.Maui.Controls.HybridWebView.InvokeJavaScriptAsync%2A> overload that invokes a specified JavaScript method without specifying any information about the return type. For more information, see [Invoke JavaScript methods that don't return a value](~/user-interface/controls/hybridwebview.md?view=net-maui-10.0&preserve-view=true#invoke-javascript-methods-that-dont-return-a-value).

By default, any exceptions that are thrown by your JavaScript code will be sent to .NET, where they're re-thrown as .NET exceptions.

### ListView

<xref:Microsoft.Maui.Controls.ListView> has been deprecated, along with <xref:Microsoft.Maui.Controls.EntryCell>, <xref:Microsoft.Maui.Controls.ImageCell>, <xref:Microsoft.Maui.Controls.SwitchCell>, <xref:Microsoft.Maui.Controls.TextCell>, and <xref:Microsoft.Maui.Controls.ViewCell>. Instead, <xref:Microsoft.Maui.Controls.CollectionView> should be used.

> [!NOTE]
> <xref:Microsoft.Maui.Controls.Cell> hasn't been deprecated because it's currently used for source generation. However, it should be considered deprecated.

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

### WebView on Android

When videos are hosted in a <xref:Microsoft.Maui.Controls.WebView> on Android, they can now be played fullscreen by including `allowfullscreen` in the `iframe`.

For more information, see [Play video fullscreen](~/user-interface/controls/webview.md?view=net-maui-10.0&preserve-view=true#play-video-full-screen-on-android).

## MessagingCenter

<xref:Microsoft.Maui.Controls.MessagingCenter> has been made internal in .NET 10. Usage of it in your code can be replaced with `WeakReferenceMessenger` in the [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm) NuGet package. For more information, see [Messenger](/windows/communitytoolkit/mvvm/messenger).

## Shadows

In .NET 10, the `ShadowTypeConverter` class, in the `Microsoft.Maui.Controls` namespace, is now public.

For more information about how shadows can be specified using formatted strings, see [Shadow](~/user-interface/shadow.md).

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

### Text to speech

The <xref:Microsoft.Maui.Media.SpeechOptions> class gains a `Rate` property that controls the speech rate when using <xref:Microsoft.Maui.Media.TextToSpeech> functionality. For more information, see [Text-to-Speech settings](~/platform-integration/device-media/text-to-speech.md?view=net-maui-10.0&preserve-view=true#settings).

### Web authentication

The <xref:Microsoft.Maui.Authentication.WebAuthenticator> gains a <xref:Microsoft.Maui.Authentication.IWebAuthenticator.AuthenticateAsync%2A> method overload that enables an authentication flow to be cancelled programatically with a <xref:System.Threading.CancellationToken> argument.

## Pop-ups

The `DisplayAlert` and `DisplayActionSheet` methods have been deprecated and replaced with the `DisplayAlertAsync` and `DisplayActionSheetAsync` methods.

For more information, see [Display pop-ups](~/user-interface/pop-ups.md?view=net-maui-10.0&preserve-view=true).

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

- [.NET for Android 10 Preview 3](https://github.com/dotnet/android/releases/tag/36.0.0-preview.3.22)
- [.NET for Android 10 Preview 2](https://github.com/dotnet/android/releases/tag/35.99.0-preview.2.205)
- [.NET for Android 10 Preview 1](https://github.com/dotnet/android/releases/tag/35.99.0-preview.1.140)

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
<PropertyGroup>
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

For more information about .NET 10 on iOS, tvOS, Mac Catalyst, and macOS, see the following release notes:

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
