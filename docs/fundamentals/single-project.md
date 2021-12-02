---
title: ".NET MAUI single project"
description: "Learn about the .NET MAUI single project, which brings all the platform-specific experiences across Android, iOS, macOS, and Windows, into one shared project."
ms.date: 11/30/2021
---

# Single project

.NET Multi-platform App UI (.NET MAUI) single project takes all the platform-specific development experiences you typically encounter while developing apps and abstracts them into a single shared project that can target Android, iOS, macOS, and Windows.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

.NET MAUI single project provides:

- A single shared project that can target Android, iOS, macOS, and Windows.
- A simplified and consistent cross-platform development experience, regardless of the platforms being targeted.
- Access to platform-specific APIs and tools when required.

.NET MAUI single project is enabled using multi-targeting and the use of SDK-style projects in .NET 6.

## Resources

Resource management for cross-platform development has traditionally been problematic. Each platform has its own approach to managing resources, which must be understood by developers. For example, each platform has differing image requirements that typically involve supplying multiple versions of each image at different resolutions.

.NET MAUI single project enables fonts, images, an app icon, a splash screen, raw assets, CSS files, and XAML files to be stored in a single location while being consumed on each platform. Each resource type should be placed in the _Resources_ folder of your .NET MAUI app, and must have its build action set correctly. The following table shows the build actions for each resource type:

| Resource | Build action |
| -------- | ------------ |
| App icons | **MauiIcon** |
| Fonts | **MauiFont** |
| Images | **MauiImage** |
| Splash screen | **MauiSplashScreen** |
| Raw assets | **MauiAsset** |
| CSS | **MauiCss** |
| XAML | **MauiXaml** |

> [!NOTE]
> After adding a resource file, its build action can be set in the **Properties** window.

Alternatively, child folders of the _Resources_ folder can be designated for each resource type by editing the project (.csproj) file for your app:

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\*" />

    <!-- Fonts -->
    <MauiFont Include="Resources\Fonts\*" />

    <!-- Assets -->
    <MauiAsset Include="Resources\Assets\*" />
</ItemGroup>
```

The wildcard character (`*`) indicates that all the files within the folder will be treated as being of the specified resource type. In addition, if you want to includes files from sub-folders too, this can be configured using two wildcard characters (`**`):

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\**\*" />
</ItemGroup>
```

In this example, `<MauiImage Include="Resources\Images\**\*" />` specifies that any files in the _Resources\Images_ folder, or any child folders of the _Images_ folder, can be consumed on each platform as images.

Platform-specific resources will override their shared resource counterparts. For example, if you have an Android-specific file located at _Platforms\Android\Resources\drawable-xhdpi\logo.png_, and you also provide a shared _Resources\Images\logo.svg_ file, the SVG file will be used to generate the required Android images, except for the XHDPI image which already exists as a platform-specific image.

### App icons

An app icon can be added to the single project by dragging an image into the _Resources\Images_ folder of your project, and setting the build action of the icon to **MauiIcon** in the **Properties** window. This creates a corresponding entry in the .csproj file for your project:

```xml
<MauiIcon Include="Resources\Images\appicon.png" />
```

At build time, the app icon is resized to the correct sizes for the target platform and device. The resized app icons are then added to your app package. App icons are resized to multiple resolutions, because they have multiple purposes, including being used to represent the app on the device, and in the app store.

<!-- For more information, see [App icons](~/user-interface/app-icons.md). -->

### Images

Images can be added to the single project by dragging them to the _Resources\Images_ folder of your project, and settings it their build action to **MauiImage** in the **Properties** window. This creating a corresponding entry per image in the .csproj file for your project:

```xml
<MauiImage Include="Resources\Images\logo.jpg" />
```

At build time, images are resized to the correct resolutions for the target platform and device. The resized images are then added to your app package.

<!-- For more information, see [Images](~/user-interface/images.md). -->

### Fonts

By default, .NET MAUI apps use the Open Sans font on each platform. However, True type format (TTF) and open type font (OTF) fonts can be added to your app and referenced by filename or alias.

A font can be added to the single project by dragging it into the _Resources\Fonts_ folder of your project, and setting its build action to **MauiFont** in the **Properties** window. This creates a corresponding entry in the .csproj file for your project:

```xml
<MauiFont Include="Resources\Fonts\MyFont.ttf" />
```

For more information, see [Fonts](~/user-interface/fonts.md).

### Splash screen

On Android and iOS, .NET MAUI apps can display a splash screen while their initialization process completes. The splash screen is displayed immediately when an app is launched, providing immediate feedback to users while app resources are initialized. Once the app is ready for interaction, the splash screen is dismissed.

A .NET MAUI splash screen can be added to the single project by dragging an image into the _Resources\Images_ folder of your project, and setting the build action of the image to **MauiSplashScreen** in the **Properties** window. This creates a corresponding entry in the .csproj file for your project:

```xml
<MauiSplashScreen Include="Resources\Images\splashscreen.svg" />
```

At build time, the splash screen image is resized to the correct size for the target platform and device. The resized splash screen is then added to your app package.

For more information, see [Splash screens](~/user-interface/splashscreen.md).

### Raw assets

Raw assets, such as HTML, JSON, and videos, can be added to the single project by dragging them into the _Resources_ folder of your project (or a sub-folder, such as _Resources\Assets_), and setting their build action to `MauiAsset` in the **Properties** window. This creates a corresponding entry per asset in the .csproj file for your project:

```xml
<MauiAsset Include="Resources\Assets\index.html" />
```

The raw asset can then be consumed by controls, as required:

```xaml
<WebView Source="index.html" />

```
At build time, the raw asset is copied to your app package.

### CSS files

.NET MAUI apps can be styled using Cascading Style Sheets (CSS). CSS files can be added to the single project by dragging them into the _Resources_ folder of your project, and setting their build action to `MauiCss` in the **Properties** window. This creates a corresponding entry per CSS file in the .csproj file for your project:

```xml
<MauiCss Include="Resources\styles.css" />
```

CSS files must be loaded by the `StyleSheet` class before being added to a `ResourceDictionary`:

```xaml
<Application ...>
    <Application.Resources>
        <StyleSheet Source="styles.css" />
    </Application.Resources>
</Application>
```

<!-- For more information, see [](). -->

### XAML files

XAML files are automatically assigned the **MauiXaml** build action when created by project and item templates, and do not have to reside in the _Resources_ folder of the app project.

## App startup

.NET MAUI apps are bootstrapped using the [.NET Generic Host](/dotnet/core/extensions/generic-host). This enables apps to be initialized from a single location, and provides the ability to configure the app, services, and third-party libraries.

For more information, see [App startup](~/fundamentals/app-startup.md).

## Platform-specific code

The project for a .NET MAUI app contains a _Platforms_ folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/single-project/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each target platform contain platform-specific code that starts the app on each platform, plus any additional platform code you add. At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the _Platforms_ > _Android_ folder will be built into the app package, but the files in the other _Platforms_ folders won't be. This approach uses a feature called multi-targeting to target multiple platforms from a single project.

Multi-targeting can be combined with partial classes and partial methods to invoke native platform functionality from cross-platform code. For more information, see [Invoke platform code](~/platform-integration/invoke-platform-code.md)

In addition to this default multi-targeting approach, .NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into sub-folders of the _Platforms_ folder. For more information, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

Multi-targeting can also be combined with conditional compilation so that code is targeted to specific platforms:

```csharp
#if __ANDROID__
                  handler.NativeView.SetBackgroundColor(Colors.Red.ToNative());
#elif __IOS__
                  handler.NativeView.BackgroundColor = Colors.Red.ToNative();
                  handler.NativeView.BorderStyle = UIKit.UITextBorderStyle.Line;
#elif WINDOWS
                  handler.NativeView.Background = Colors.Red.ToNative();
#endif
```

For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).
