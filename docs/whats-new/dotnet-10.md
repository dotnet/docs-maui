---
title: What's new in .NET MAUI for .NET 10
description: Learn about the new features introduced in .NET MAUI for .NET 10.
ms.date: 02/18/2025
---

# What's new in .NET MAUI for .NET 10

The focus of .NET Multi-platform App UI (.NET MAUI) in .NET 10 is to improve product quality. For more information about the product quality improvements in .NET MAUI in .NET 10, see the following release notes:

- [.NET MAUI in .NET 10 Preview 2](https://github.com/dotnet/maui/releases/tag/10.0.0-preview.2.XXXXX.X)
- [.NET MAUI in .NET 10 Preview 1](https://github.com/dotnet/maui/releases/tag/10.0.0-preview.1.25122.6)

> [!IMPORTANT]
> Due to working with external dependencies, such as Xcode or Android SDK Tools, the .NET MAUI support policy differs from the [.NET and .NET Core support policy](https://dotnet.microsoft.com/platform/support/policy/maui). For more information, see [.NET MAUI support policy](https://dotnet.microsoft.com/platform/support/policy/maui).

In .NET 10, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

## Control enhancements

.NET MAUI in .NET 10 includes control enhancements.

### CollectionView and CarouselView

.NET MAUI in .NET 9 included two optional handlers on iOS and Mac Catalyst that brought performance and stability improvements to <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>. In .NET 10, these are the default handlers for <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.CarouselView>.

### HybridWebView

<xref:Microsoft.Maui.Controls.HybridWebView> gains a `InvokeJavaScriptAsync` method that invokes a specified JavaScript method without requiring a generic argument, or a return type argument.

### SearchBar

<xref:Microsoft.Maui.Controls.SearchBar> gains a `SearchIconColor` bindable property that sets the color of the search icon:

```xaml

```

### Switch

<xref:Microsoft.Maui.Controls.Switch> gains an `OffColor` bindable property that sets the color of the switch when it's in the off state:

```xaml
<Switch OffColor="Red"
        OnColor="Green" />
```

When the `OffColor` bindable property isn't set, the default color is used for the off state.

## MessagingCenter

`MessagingCenter` has been made internal in .NET 10. It can be replaced with `WeakReferenceMessenger` in the [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm) NuGet package. For more information, see [Messenger](/windows/communitytoolkit/mvvm/messenger).

## Shadows

.NET 10 includes a `ShadowTypeConverter` type for converting a formatted string to a `Shadow` on a <xref:Microsoft.Maui.Controls.VisualElement>. The converter supports three different approaches to initializing a `Shadow`:

- color, offset X, offset Y

    ```xaml
    <VerticalStackLayout BackgroundColor="#fff" Shadow="#000000 4 4" />
    ```

- offset X, offset Y, radius, color

    ```xaml
    <VerticalStackLayout BackgroundColor="#fff" Shadow="4 4 16 #000000" />
    ```

- offset X, offset Y, radius, color, opacity

    ```xaml
    <VerticalStackLayout BackgroundColor="#fff" Shadow="4 4 16 #000000 0.5" />
    ```

## Platform integration

The `SpeechOptions` class, in the `Microsoft.Maui.Media` class, gains a `Rate` property that controls the speech rate when using `TextToSpeech` functionality. For more information, see [Text-to-Speech settings](~/platform-integration/device-media/text-to-speech?view=net-maui-10.0&preserve-view=true#settings).

## Platforms

### Android

The signature of the `ToSpannableString` extension method on Android, in the `Microsoft.Maui.Controls.Platform.FormattedStringExensions` class, has changed

### iOS and Mac Catalyst

#### Compatibility AccessibilityExtensions

The following iOS compatibility `AccessibilityExtensions` extension methods, in the `Microsoft.Maui.Controls.Compatibility.Platform.iOS`, have been obsoleted:

- `SetAccessibilityHint`
- `SetAccessibilityLabel`
- `SetAccessibilityHint`
- `SetAccessibilityLabel`

Instead, the <xref:Microsoft.Maui.Platform.UpdateSemantics> method should be used.

#### MauiWebViewNavigationDelegate overrides

The following `MauiWebViewNavigationDelegate` methods, in the `Microsoft.Maui.Platform` namespace, are now overridable:

- `DecidePolicy`
- `DidFailNavigation`
- `DidFailProvisionalNavigation`
- `DidFinishNavigation`

#### Style modal dialogs as popovers

.NET MAUI for .NET 10 adds a platform-specific that displays a modal page as a popover on iOS and Mac Catalyst. It's consumed by setting the `Page.ModalPopoverSourceView` bindable property to a `View` that defines the source of the modal, the `Page.ModalPopoverRect` bindable property to a `Rectangle` that defines the rectangle within the source of the modal, and the `Page.ModalPresentationStyle` bindable property to `Popover`:

```csharp
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

## XAML markup extensions

The <xref:Microsoft.Maui.Controls.Xaml.FontImageExtension> XAML markup extension has been deprecated. Instead, the `FontImageSource` type should be used:

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

## .NET for Android

.NET for Android in .NET 10 adds support for API 36 and JDK 21, and includes work to reduce build times and improve performance. For more information about .NET for Android in .NET 10, see the following release notes:

- [.NET for Android 10 Preview 1](https://github.com/dotnet/android/releases/tag/35.99.0-preview.1.140)

### Android 16 (Baklava) beta 1 bindings

Google has released [Beta 1](https://android-developers.googleblog.com/2025/01/first-beta-android16.html) of the Android 16 (API-36) SDK.  Support has been adding for using these preview APIs.

To target the Android 16 preview API:

- Use the Android SDK Manager to download the Android 16 (Baklava) platform.
- Update your project's `TargetFramework` to `net10.0-android36`.

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

- [.NET 10.0.1xx Preview 1](https://github.com/dotnet/macios/releases/tag/dotnet-10.0.1xx-preview1-10322)

For information about known issues, see [Known issues in .NET 10](https://github.com/dotnet/macios/wiki/Known-issues-in-.NET10).

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

## See also

- [.NET MAUI roadmap](https://github.com/dotnet/maui/wiki/Roadmap)
- [What's new in .NET 10](/dotnet/core/whats-new/dotnet-10/overview)
