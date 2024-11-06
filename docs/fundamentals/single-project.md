---
title: "Target multiple platforms from .NET MAUI single project"
description: "Learn about the .NET MAUI single project, which brings all the platform-specific experiences across Android, iOS, macOS, Tizen, and Windows, into one shared project."
ms.date: 05/17/2022
---

# Target multiple platforms from .NET MAUI single project

.NET Multi-platform App UI (.NET MAUI) single project takes the platform-specific development experiences you typically encounter while developing apps and abstracts them into a single shared project that can target Android, iOS, macOS, and Windows.

.NET MAUI single project provides a simplified and consistent cross-platform development experience, regardless of the platforms being targeted. .NET MAUI single project provides the following features:

- A single shared project that can target Android, iOS, macOS, Tizen, and Windows.
- A simplified debug target selection for running your .NET MAUI apps.
- Shared resource files within the single project.
- A single app manifest that specifies the app title, ID, and version.
- Access to platform-specific APIs and tools when required.
- A single cross-platform app entry point.

.NET MAUI single project is enabled using multi-targeting and the use of SDK-style projects.

## Resource files

Resource management for cross-platform app development has traditionally been problematic, because each platform has its own approach to managing resources. For example, each platform has differing image requirements that typically involves creating multiple versions of each image at different resolutions. Therefore, a single image typically has to be duplicated multiple times at different resolutions, with the resulting images having to use different filename and folder conventions on each platform.

.NET MAUI single project enables resource files to be stored in a single location while being consumed on each platform. This includes fonts, images, the app icon, the splash screen, raw assets, and CSS files for styling .NET MAUI apps. Each image resource file is used as a source image, from which images of the required resolutions are generated for each platform at build time.

> [!NOTE]
> iOS Asset Catalogs are currently unsupported in .NET MAUI single projects.

Resource files should typically be placed in the _Resources_ folder of your .NET MAUI app project, or child folders of the _Resources_ folder, and must have their build action set correctly. The following table shows the build actions for each resource file type:

| Resource      | Build action     |
|---------------|------------------|
| App icon      | MauiIcon         |
| Fonts         | MauiFont         |
| Images        | MauiImage        |
| Splash screen | MauiSplashScreen |
| Raw assets    | MauiAsset        |
| CSS files     | MauiCss          |

> [!NOTE]
> XAML files are also stored in your .NET MAUI app project, and are automatically assigned the **MauiXaml** build action when created by project and item templates. However, only XAML resource dictionaries will typically be placed in the _Resources_ folder of the app project.

When a resource file is added to a .NET MAUI app project, a corresponding entry for the resource is created in the project file, with the exception of CSS files. The following screenshot shows a typical *Resources* folder containing child-folders for each resource type:

:::image type="content" source="media/single-project/resources.png" alt-text="Image and font resources screenshot.":::

The build action for a resource file will be correctly set, if the resource has been added to the correct *Resources* child folder.

Child folders of the *Resources* folder can be designated for each resource type by editing the project file for your app:

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\*" />

    <!-- Fonts -->
    <MauiFont Include="Resources\Fonts\*" />

    <!-- Raw assets -->
    <MauiAsset Include="Resources\Raw\*" />
</ItemGroup>
```

The wildcard character (`*`) indicates that all the files within the folder will be treated as being of the specified resource type. In addition, it's possible to include all files from child folders:

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\**\*" />
</ItemGroup>
```

In this example, the double wildcard character ('**') specifies that the *Images* folder can contain child folders. Therefore, `<MauiImage Include="Resources\Images\**\*" />` specifies that any files in the *Resources\Images* folder, or any child folders of the *Images* folder, will be used as source images from which images of the required resolution are generated for each platform.

Platform-specific resources will override their shared resource counterparts. For example, if you have an Android-specific image located at *Platforms\Android\Resources\drawable-xhdpi\logo.png*, and you also provide a shared *Resources\Images\logo.svg* image, the Scalable Vector Graphics (SVG) file will be used to generate the required Android images, except for the XHDPI image that already exists as a platform-specific image.

### App icons

An app icon can be added to your app project by dragging an image into the _Resources\AppIcon_ folder of the project, where its build action will automatically be set to **MauiIcon**. This creates a corresponding entry in your project file:

```xml
<MauiIcon Include="Resources\AppIcon\appicon.svg" />
```

At build time, the app icon will be resized to the correct sizes for the target platform and device. The resized app icons are then added to your app package. App icons are resized to multiple resolutions because they have multiple uses, including being used to represent the app on the device, and in the app store.

For more information, see [Add an app icon to a .NET MAUI app project](~/user-interface/images/app-icons.md).

### Images

An image can be added to your app project by dragging it into the *Resources\Images* folder of the project, where its build action will automatically be set to **MauiImage**. This creates a corresponding entry in your project file:

```xml
<MauiImage Include="Resources\Images\logo.svg" />
```

At build time, images can be resized to the correct resolutions for the target platform and device. The resulting images are then added to your app package.

For more information, see [Add images to a .NET MAUI app project](~/user-interface/images/images.md).

### Fonts

A true type format (TTF) or open type font (OTF) font can be added to your app project by dragging it into the *Resources\Fonts* folder of your project, where its build action will automatically be set to **MauiFont**. This creates a corresponding entry per font in your project file:

```xml
<MauiFont Include="Resources\Fonts\OpenSans-Regular.ttf" />
```

At build time, the fonts are copied to your app package.

For more information, see [Fonts](~/user-interface/fonts.md).

### Splash screen

A splash screen can be added to your app project by dragging an image into the *Resources\Splash* folder of the project, where its build action will automatically be set to **MauiSplashScreen**. This creates a corresponding entry in your project file:

```xml
<ItemGroup>
  <MauiSplashScreen Include="Resources\Splash\splashscreen.svg" />
</ItemGroup>
```

At build time, the splash screen image is resized to the correct size for the target platform and device. The resized splash screen is then added to your app package.

For more information, see [Add a splash screen to a .NET MAUI app project](~/user-interface/images/splashscreen.md).

### Raw assets

A raw asset file, such as HTML, JSON, and video, can be added to your app project by dragging it into the *Resources\Raw* folder of your project, where its build action will automatically be set to **MauiAsset**. This creates a corresponding entry per asset in your project file:

```xml
<MauiAsset Include="Resources\Raw\index.html" />
```

Raw assets can then be consumed by controls, as required:

```xaml
<WebView Source="index.html" />
```

At build time, raw assets are copied to your app package. For information about disabling asset packaging, see [Disable asset file packaging](~/troubleshooting.md#disable-asset-file-packaging).

### CSS files

.NET MAUI apps can be partially styled with Cascading Style Sheet (CSS) files. CSS files can be added to your app project by dragging them into any folder of your project, and setting their build action to `MauiCss` in the **Properties** window.

CSS files must be loaded by the `StyleSheet` class before being added to a <xref:Microsoft.Maui.Controls.ResourceDictionary>:

```xaml
<Application ...>
    <Application.Resources>
        <StyleSheet Source="/Resources/styles.css" />
    </Application.Resources>
</Application>
```

For more information, see [Style apps with CSS](~/user-interface/styles/css.md).

## App manifest

Each platform uses its own native app manifest file to specify information such as the app title, ID, version, and more. .NET MAUI single project enables you to specify this common app data in a single location in the project file.

To specify the shared app manifest data for a project, open the shortcut menu for the project in **Solution Explorer**, and then choose **Properties**. The app title, ID, and version can then be specified in **MAUI Shared > General**:

:::image type="content" source="media/single-project/manifest.png" alt-text=".NET MAUI app manifest screenshot.":::

At build time the shared app manifest data is merged with platform-specific data in the native app manifest file, to produce the manifest file for the app package. For more information, see [Project configuration in .NET MAUI - MAUI Shared](../deployment/visual-studio-properties.md#maui-shared).

## Platform-specific code

A .NET MAUI app project contains a *Platforms* folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/single-project/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each platform contain platform-specific resources, and code that starts the app on each platform:

:::image type="content" source="media/single-project/platform-code.png" alt-text="Platform-specific code screenshot.":::

At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the *Platforms\Android* folder will be built into the app package, but the files in the other *Platforms* folders won't be. This approach uses multi-targeting to target multiple platforms from a single project. Multi-targeting can be combined with partial classes and partial methods to invoke native platform functionality from cross-platform code. For more information, see [Invoke platform code](~/platform-integration/invoke-platform-code.md).

In addition to this default multi-targeting approach, .NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into child-folders of the *Platforms* folder. For more information, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

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

While the *Platforms* folders contain platform-specific code that starts the app on each platform, .NET MAUI apps have a single cross-platform app entry point. Each platform entry point calls a `CreateMauiApp` method on the static `MauiProgram` class in your app project, and returns a `MauiApp`, which is the entry point for your app.

The `MauiProgram` class must at a minimum provide an app to run:

```csharp
namespace MyMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        return builder.Build();
    }
}  
```

The `App` class derives from the `Application` class:

::: moniker range="=net-maui-8.0"

```csharp
namespace MyMauiApp;

public class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
```

In the preceding example, the `MainPage` property is set to the `AppShell` object. `AppShell` is a subclassed <xref:Microsoft.Maui.Controls.Shell> class that describes the visual hierarchy of the app. For more information, see [Create a .NET MAUI Shell app](shell/create.md).

::: moniker-end

::: moniker range=">=net-maui-9.0"

```csharp
namespace MyMauiApp;

public class App : Application
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

In the preceding example, a new <xref:Microsoft.Maui.Controls.Window> is created whose initial content is set to the `AppShell` object. `AppShell` is a subclassed <xref:Microsoft.Maui.Controls.Shell> class that describes the visual hierarchy of the app. For more information, see [Create a .NET MAUI Shell app](shell/create.md).

::: moniker-end
