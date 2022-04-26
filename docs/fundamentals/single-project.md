---
title: "Target multiple platforms from .NET MAUI single project"
description: "Learn about the .NET MAUI single project, which brings all the platform-specific experiences across Android, iOS, macOS, and Windows, into one shared project."
ms.date: 04/19/2022
---

# Target multiple platforms from .NET MAUI single project

.NET Multi-platform App UI (.NET MAUI) single project takes the platform-specific development experiences you typically encounter while developing apps and abstracts them into a single shared project that can target Android, iOS, macOS, and Windows.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

.NET MAUI single project provides a simplified and consistent cross-platform development experience, regardless of the platforms being targeted. .NET MAUI single project provides the following features:

- A single shared project that can target Android, iOS, macOS, and Windows.
- A simplified debug target selection for running your .NET MAUI apps.
- Shared resource files within the single project.
- A single app manifest that specifies the app title, id, and version.
- Access to platform-specific APIs and tools when required.
- A single cross-platform app entry point.

.NET MAUI single project is enabled using multi-targeting and the use of SDK-style projects in .NET 6.

## Resource files

Resource management for cross-platform app development has traditionally been problematic. Each platform has its own approach to managing resources, that must be implemented on each platform. For example, each platform has differing image requirements that typically involves creating multiple versions of each image at different resolutions. Therefore, a single image typically has to be duplicated multiple times per platform, at different resolutions, with the resulting images having to use different filename and folder conventions on each platform.

.NET MAUI single project enables resource files to be stored in a single location while being consumed on each platform. This includes fonts, images, the app icon, the splash screen, raw assets, and CSS files for styling .NET MAUI apps.

> [!IMPORTANT]
> Each image resource file is used as a source image, from which images of the required resolutions are generated for each platform at build time.

Resource files should typically be placed in the _Resources_ folder of your .NET MAUI app project, or child folders of the _Resources_ folder, and must have their build action set correctly. The following table shows the build actions for each resource file type:

| Resource | Build action |
| -------- | ------------ |
| App icon | MauiIcon |
| Fonts | MauiFont |
| Images | MauiImage |
| Splash screen | MauiSplashScreen |
| Raw assets | MauiAsset |
| CSS files | MauiCss |

> [!NOTE]
> XAML files are also stored in your .NET MAUI app project, and are automatically assigned the **MauiXaml** build action when created by project and item templates. However, XAML files will not typically be located in the _Resources_ folder of the app project.

When a resource file is added to a .NET MAUI app project, a corresponding entry for the resource is created in the project (.csproj) file, with the exception of CSS files. After adding a resource file, its build action can be set in the **Properties** window. The following screenshot shows a _Resources_ folder containing image and font resources in child folders:

:::image type="content" source="media/single-project/resources.png" alt-text="Image and font resources screenshot.":::

Child folders of the _Resources_ folder can be designated for each resource type by editing the project file for your app:

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

The wildcard character (`*`) indicates that all the files within the folder will be treated as being of the specified resource type. In addition, it's possible to include all files from child folders:

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\**\*" />
</ItemGroup>
```

In this example, the double wildcard character ('**') specifies that the _Images_ folder can contain child folders. Therefore, `<MauiImage Include="Resources\Images\**\*" />` specifies that any files in the _Resources\Images_ folder, or any child folders of the _Images_ folder, will be used as source images from which images of the required resolution are generated for each platform.

Platform-specific resources will override their shared resource counterparts. For example, if you have an Android-specific image located at _Platforms\Android\Resources\drawable-xhdpi\logo.png_, and you also provide a shared _Resources\Images\logo.svg_ image, the Scalable Vector Graphics (SVG) file will be used to generate the required Android images, except for the XHDPI image that already exists as a platform-specific image.

### App icons

An app icon can be added to your app project by dragging an image into the _Resources\Images_ folder of the project, and setting the build action of the icon to **MauiIcon** in the **Properties** window. This creates a corresponding entry in your project file:

```xml
<MauiIcon Include="Resources\Images\appicon.png" />
```

At build time, the app icon is resized to the correct sizes for the target platform and device. The resized app icons are then added to your app package. App icons are resized to multiple resolutions because they have multiple uses, including being used to represent the app on the device, and in the app store.

For more information, see [App icons](~/user-interface/images/app-icons.md).

### Images

Images can be added to your app project by dragging them to the _Resources\Images_ folder of your project,  where their build action will automatically be set to **MauiImage**. This creates a corresponding entry per image in your project file:

```xml
<MauiImage Include="Resources\Images\logo.jpg" />
```

At build time, vector images are resized to the correct resolutions for the target platform and device. The resulting images, whether vector-based or not, are then added to your app package.

For more information, see [Images](~/user-interface/images/images.md).

### Fonts

True type format (TTF) and open type font (OTF) fonts can be added to your app project by dragging them into the _Resources\Fonts_ folder of your project, and setting their build action to **MauiFont** in the **Properties** window. This creates a corresponding entry per font in your project file:

```xml
<MauiFont Include="Resources\Fonts\OpenSans-Regular.ttf" />
```

At build time, the fonts are copied to your app package.

<!-- For more information, see [Fonts](~/user-interface/fonts.md). -->

### Splash screen

A slash screen can be added to your app project by dragging an image into the _Resources\Images_ folder of your project, and setting the build action of the image to **MauiSplashScreen** in the **Properties** window. This creates a corresponding entry in your project file:

```xml
<MauiSplashScreen Include="Resources\Images\splashscreen.svg" />
```

At build time, the splash screen image is resized to the correct size for the target platform and device. The resized splash screen is then added to your app package.

For more information, see [Splash screens](~/user-interface/images/splashscreen.md).

### Raw assets

Raw asset files, such as HTML, JSON, and videos, can be added to your app project by dragging them into the _Resources_ folder of your project (or a sub-folder, such as _Resources\Raw_), and setting their build action to `MauiAsset` in the **Properties** window. This creates a corresponding entry per asset in your project file:

```xml
<MauiAsset Include="Resources\Raw\index.html" />
```

Raw assets can then be consumed by controls, as required:

```xaml
<WebView Source="index.html" />
```

At build time, raw assets are copied to your app package.

### CSS files

.NET MAUI apps can be partially styled with Cascading Style Sheet (CSS) files. CSS files can be added to your app project by dragging them into any folder of your project, and setting their build action to `MauiCss` in the **Properties** window.

CSS files must be loaded by the `StyleSheet` class before being added to a `ResourceDictionary`:

```xaml
<Application ...>
    <Application.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </Application.Resources>
</Application>
```

For more information, see [Style apps with CSS](~/user-interface/styles/css.md).

## App manifest

Each platform uses its own native app manifest file to specify information such as the app title, id, version, and more. .NET MAUI single project enables you to specify this common app data in a single location in the project file (.csproj).

To specify the shared app manifest data for a project, open the shortcut menu for the project in **Solution Explorer**, and then choose **Properties**. The app title, id, and version can then be specified in **MAUI Shared > General**:

:::image type="content" source="media/single-project/manifest.png" alt-text=".NET MAUI app manifest screenshot.":::

At build time the shared app manifest data is merged with platform-specific data in the native app manifest file, to produce the manifest file that ships with the app.

## Platform-specific code

A .NET MAUI app project contains a _Platforms_ folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/single-project/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each platform contain platform-specific resources, and code and that starts the app on each platform:

:::image type="content" source="media/single-project/platform-code.png" alt-text="Platform-specific code screenshot.":::

At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the _Platforms\Android_ folder will be built into the app package, but the files in the other _Platforms_ folders won't be. This approach uses multi-targeting to target multiple platforms from a single project. Multi-targeting can be combined with partial classes and partial methods to invoke native platform functionality from cross-platform code. For more information, see [Invoke platform code](~/platform-integration/invoke-platform-code.md).

In addition to this default multi-targeting approach, .NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into sub-folders of the _Platforms_ folder. For more information, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

Multi-targeting can also be combined with conditional compilation so that code is targeted to specific platforms:

```csharp
#if ANDROID
                  handler.NativeView.SetBackgroundColor(Colors.Red.ToNative());
#elif IOS
                  handler.NativeView.BackgroundColor = Colors.Red.ToNative();
                  handler.NativeView.BorderStyle = UIKit.UITextBorderStyle.Line;
#elif WINDOWS
                  handler.NativeView.Background = Colors.Red.ToNative();
#endif
```

For more information about conditional compilation, see [Conditional compilation](/dotnet/csharp/language-reference/preprocessor-directives#conditional-compilation).

## App entry point

While the _Platforms_ folders contain platform-specific code that starts the app on each platform, .NET MAUI apps have a single cross-platform app entry point. Each platform entry point calls a `CreateMauiApp` method on the static `MauiProgram` class in your app project, and returns a `MauiApp`, which is the entry point for your app. The `CreateMauiApp` method bootstraps the app using the [.NET Generic Host](/dotnet/core/extensions/generic-host). This provides the ability to configure the app, services, and third-party libraries from a single location.

For more information, see [App startup](~/fundamentals/app-startup.md).
