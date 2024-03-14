---
title: "Manually upgrade a Xamarin.Forms app to a single project .NET MAUI app"
description: "Learn how to manually upgrade a Xamarin.Forms app to a single project .NET MAUI app."
ms.date: 08/29/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net8.0-android", "net8.0-ios" ]
---

# Manually upgrade a Xamarin.Forms app to a single project .NET MAUI app

To migrate a Xamarin.Forms app to a single project .NET Multi-platform App UI (.NET MAUI) app, you must:

> [!div class="checklist"]
>
> - Update your Xamarin.Forms app to use Xamarin.Forms 5.
> - Update the app's dependencies to the latest versions.
> - Ensure the app still works.
> - Create a .NET MAUI app.
> - Copy code and configuration from the Xamarin.Forms app to the .NET MAUI app.
> - Copy resources from your Xamarin.Forms app to the .NET MAUI app.
> - Update namespaces.
> - Address any API changes.
> - Upgrade or replace incompatible dependencies with .NET 8 versions.
> - Compile and test your app.

To simplify the upgrade process, you should create a new .NET MAUI app of the same name as your Xamarin.Forms app, and then copy in your code, configuration, and resources. This is the approach outlined below.

## Update your Xamarin.Forms app

Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions.

This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.

## Create a .NET MAUI app

In Visual Studio, create a new .NET MAUI app using the same name as your Xamarin.Forms app:

:::image type="content" source="media/multi-project-to-single-project/create-maui-app.png" alt-text="Screenshot of creating a .NET MAUI app in Visual Studio.":::

Opening the project file will confirm that you have a .NET SDK-style project.

## Copy code to the .NET MAUI app

All of the cross-platform code from your Xamarin.Forms library project should be copied into your .NET MAUI app project in identically named folders and files.

Custom renderers can either be reused in a .NET MAUI app, or migrated to a .NET MAUI handler. For more information, see [Reuse custom renderers in .NET MAUI](custom-renderers.md) and [Migrate a Xamarin.Forms custom renderer to a .NET MAUI handler](renderer-to-handler.md).

Effects can be reused in a .NET MAUI app. For more information, see [Reuse effects](effects.md).

> [!NOTE]
> You can quickly update your `Xamarin.Forms` namespaces to `Microsoft.Maui` by using [Quick actions in Visual Studio](upgrade-assistant.md#quick-actions-in-visual-studio) make sure you have the Upgrade Assistant installed to get started.

### Platform-specific code

A .NET MAUI app project contains a *Platforms* folder, with each child folder representing a platform that .NET MAUI can target:

:::image type="content" source="media/multi-project-to-single-project/platform-folders.png" alt-text="Platform folders screenshot.":::

The folders for each platform contain platform-specific resources, and code that starts the app on each platform:

:::image type="content" source="media/multi-project-to-single-project/platform-code.png" alt-text="Platform-specific code screenshot.":::

Code, and their containing folders, from your Xamarin.Forms head projects should be copied to these folders:

- Code from your Xamarin.Forms Android head project should be copied to the *Platform\Android* folder of your .NET MAUI app project. In addition, copy any custom code from your Xamarin.Forms `MainActivity` and `MainApplication` classes to the same classes in your .NET MAUI app project.
- Code from your Xamarin.Forms iOS head project should be copied to the *Platforms\iOS* folder of your .NET MAUI app project. In addition, copy any custom code from your Xamarin.Forms `AppDelegate` class to the same class in your .NET MAUI app project.

    > [!NOTE]
    > For a list of breaking changes in .NET iOS, see [Breaking changes in .NET iOS](https://github.com/xamarin/xamarin-macios/wiki/Breaking-changes-in-.NET).

- Code from your Xamarin.Forms UWP head project should be copied to the *Platforms\Windows* folder of your .NET MAUI app project. In addition, copy any custom code from your Xamarin.Forms `App` class to the same class in your .NET MAUI app project.

At build time, the build system only includes the code from each folder when building for that specific platform. For example, when you build for Android the files in the *Platforms\Android* folder will be built into the app package, but the files in the other *Platforms* folders won't be. This approach uses multi-targeting to target multiple platforms from a single project. .NET MAUI apps can also be multi-targeted based on your own filename and folder criteria. This enables you to structure your .NET MAUI app project so that you don't have to place your platform code into child-folders of the *Platforms* folder. For more information, see [Configure multi-targeting](~/platform-integration/configure-multi-targeting.md).

## Copy configuration to the .NET MAUI app

Each platform uses its own native app manifest file to specify information such as the app title, ID, version, and more. .NET MAUI single project enables you to specify this common app data in a single location in the project file.

To specify the shared app manifest data for a project, open the shortcut menu for the project in **Solution Explorer**, and then choose **Properties**. The app title, ID, and version can then be specified in **MAUI Shared > General**:

:::image type="content" source="media/multi-project-to-single-project/manifest.png" alt-text=".NET MAUI app manifest screenshot.":::

At build time the shared app manifest data is merged with platform-specific data in the native app manifest file, to produce the manifest file for the app package. For more information, see [Project configuration in .NET MAUI - MAUI Shared](../deployment/visual-studio-properties.md#maui-shared).

The remaining data from your Xamarin.Forms app manifests should be copied to your .NET MAUI app manifest:

- On Android, copy any additional data from the *AndroidManifest.xml* file in your Xamarin.Forms Android head project, to the *Platforms\Android\AndroidManifest.xml* file in your .NET MAUI app project.
- On iOS, copy any additional data from the *Info.plist* file in your Xamarin.Forms iOS head project, to the *Platforms\iOS\Info.plist* file in your .NET MAUI app project. In addition, copy the *Entitlements.plist* file in your Xamarin.Forms iOS head project to the *Platforms\iOS* folder in your .NET MAUI app project.
- On Windows, copy additional data from the *Package.appxmanifest* file in your Xamarin.Forms UWP head project, to the *Platforms\Windows\Package.appxmanifest* file in your .NET MAUI app project.

## Copy resources to the .NET MAUI app

.NET MAUI single project enables resource files to be stored in a single location while being consumed on each platform. This includes fonts, images, the app icon, the splash screen, raw assets, and CSS files for styling .NET MAUI apps.

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
> XAML files are also stored in your .NET MAUI app project, and are automatically assigned the **MauiXaml** build action. However, only XAML resource dictionaries will typically be placed in the _Resources_ folder of the app project.

The following screenshot shows a typical *Resources* folder containing child-folders for each resource type:

:::image type="content" source="media/multi-project-to-single-project/resources.png" alt-text="Image and font resources screenshot.":::

The build action for a resource file will be correctly set, if the resource has been added to the correct *Resources* child folder.

> [!IMPORTANT]
> Platform-specific resources will override their shared resource counterparts. For example, if you have an Android-specific image located at *Platforms\Android\Resources\drawable-xhdpi\logo.png*, and you also provide a shared *Resources\Images\logo.svg* image, the Scalable Vector Graphics (SVG) file will be used to generate the required Android images, except for the XHDPI image that already exists as a platform-specific image.

### App icons

Your Xamarin.Forms app icon should be added to your .NET MAUI app project by dragging the image into the _Resources\AppIcon_ folder of the project, where its build action will automatically be set to **MauiIcon**. At build time, the app icon is resized to the correct sizes for the target platform and device. App icons are resized to multiple resolutions because they have multiple uses, including being used to represent the app on the device, and in the app store.

For more information, see [Add an app icon to a .NET MAUI app project](~/user-interface/images/app-icons.md).

### Splash screen

If your Xamarin.Forms app has a splash screen, it should be added to your .NET MAUI app project by dragging the image into the *Resources\Splash* folder of the project, where its build action will automatically be set to **MauiSplashScreen**. At build time, the splash screen image is resized to the correct size for the target platform and device.

For more information, see [Add a splash screen to a .NET MAUI app project](~/user-interface/images/splashscreen.md).

### Images

Devices have a range of screen sizes and densities and each platform has functionality for displaying density-dependent images. In Xamarin.Forms, density-dependent images are typically placed in head projects and adopt a platform-specific naming convention. There are two approaches that can be taken to migrate these images to .NET MAUI.

The recommended approach is to copy the highest resolution version of each image from your Xamarin.Forms solution to your .NET MAUI app project by dragging it into the *Resources\Images* folder of the project, where its build action will automatically be set to **MauiImage**. It will also be necessary to set the `BaseSize` attribute of each bitmap image, to ensure that resizing occurs. This eliminates the need to have multiple versions of each image, on each platform. At build time, any images will then be resized into multiple density-dependent images that meet platform requirements. For more information, see [Add images to a .NET MAUI app project](~/user-interface/images/images.md).

Alternatively, you could copy density-dependent images from your Xamarin.Forms solution to identically named folders in the *Platforms\\{Platform}* folder of your .NET MAUI app project, and set their build actions to the build actions that are used in your Xamarin.Forms solution. The following table lists example image locations for a Xamarin.Forms solution, and their equivalent location in a .NET MAUI app project:

| Xamarin.Forms image location | .NET MAUI image location | .NET MAUI platform image build action |
| ---------------------------- | ------------------------ | ------------------------------------- |
| *{MyApp.Android}\Resources\drawable-xhdpi\image.png* | *Platforms\Android\Resources\drawable-xhdpi\image.png* | **AndroidResource** |
| *{MyApp.iOS}\image.jpg* | *Platforms\iOS\Resources\image.jpg | **BundleResource** |
| *{MyApp.UWP}\Assets\Images\image.gif* | *Platforms\Windows\Assets\Images\image.gif | **Content** |

Provided that you've adopted the same image naming convention as used in your Xamarin.Forms solution, the appropriate image will be chosen at runtime based on the device's capabilities. The disadvantage of this approach is that you still have multiple versions of each image on each platform.

### Fonts

Any fonts from your Xamarin.Forms solution can be added to your .NET MAUI solution by dragging them into the *Resources\Fonts* folder of your .NET MAUI app project, where their build action will automatically be set to **MauiFont**.

For more information, see [Fonts](~/user-interface/fonts.md).

### CSS files

Any CSS files from your Xamarin.Forms solution can be added to your .NET MAUI solution by dragging them into an identically named folder, and setting their build action to **MauiCss** in the **Properties** window.

For more information about using CSS files in a .NET MAUI app, see [Style apps using Cascading Style Sheets](~/user-interface/styles/css.md).

### Raw assets

Any raw asset files, such as HTML, JSON, and video, should be copied from your Xamarin.Forms solution to your .NET MAUI app project by dragging them into the *Resources\Raw* folder of your project, where their build action will automatically be set to **MauiAsset**.

### Localized resources

In a .NET MAUI app, strings are localized using the same approach as in a Xamarin.Forms app. Therefore, your .NET resource files (*.resx*) should be copied from your Xamarin.Forms solution to an identically named folder in your .NET MAUI solution. Then, the neutral language of your .NET MAUI app must be specified. For more information, see [Specify the app's neutral language](~/fundamentals/localization.md#specify-the-apps-neutral-language).

> [!NOTE]
> .NET resource files don't have to be placed in the *Resources* folder of your .NET MAUI app project.

In a .NET MAUI app, images are localized using the same approach as in a Xamarin.Forms app. Therefore, your localized images, and the folders in which they reside, should be copied from your Xamarin.Forms solution to your .NET MAUI app project:

- On Android, the root folder in your .NET MAUI app project for localized images is *Platforms\Android\Resources*.
- On iOS, the root folder in your .NET MAUI app project for localized images is *Platforms\iOS\Resources*.
- On Windows, the root folder in your .NET MAUI app project for localized images is *Platforms\Windows\Assets\Images*.

Localized images should have their build actions set to the build actions that are used in your Xamarin.Forms solution. For more information, see [Localize images](~/fundamentals/localization.md#localize-images).

In a .NET MAUI app, app names are localized using the same approach as in a Xamarin.Forms app:

- On Android, the localized app name can be stored using a folder-based naming convention in the *Platforms\Android\Resources* folder. App name localization folders and files should be copied to this folder from your Xamarin.Forms solution.
- On iOS, the localized app name is stored using a folder-based naming convention in the *Platforms\iOS\Resources* folder. App name localization folders and files should be copied to this folder from your Xamarin.Forms solution.
- On Windows, the localized app name is stored in the app package manifest.

For more information, see [Localize the app name](~/fundamentals/localization.md#localize-the-app-name). For more information about .NET MAUI app localization, see [Localization](~/fundamentals/localization.md).

[!INCLUDE [Namespace changes](includes/namespace-changes.md)]

[!INCLUDE [API changes](includes/api-changes.md)]

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

[!INCLUDE [Update app dependencies](includes/update-app-dependencies.md)]

[!INCLUDE [Compile and troubleshoot](includes/compile-troubleshoot.md)]
