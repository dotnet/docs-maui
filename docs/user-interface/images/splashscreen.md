---
title: "Splash screen"
description: "A .NET MAUI splash screen can be displayed on Android and iOS when an app is launched, while the app's initialization process completes."
ms.date: 12/24/2021
---

# Splash screens

<!-- Todo: Move this content into the images doc, once it migrates over? -->

On Android and iOS, .NET Multi-platform App UI (.NET MAUI) apps can display a splash screen while their initialization process completes. The splash screen is displayed immediately when an app is launched, providing immediate feedback to users while app resources are initialized:

:::image type="content" source="media/splashscreen/splashscreens.png" alt-text=".NET MAUI splash screens screenshot." border="false":::

Once the app is ready for interaction, its splash screen is dismissed.

Splash screens are a composite of an image and a background color. The standard platform image formats are supported, including Scalable Vector Graphics (SVG) files.

> [!TIP]
> The SVG format is the recommended image format for .NET MAUI splash screens.

.NET MAUI splash screens can be added to your app project by dragging an image into the _Resources\Images_ folder of the project, and setting the build action of the image to **MauiSplashScreen** in the **Properties** window. This creates a corresponding entry in your project file:

<!-- Todo: The template currently puts the splash screen in Resources, not Resources\Images -->

```xml
<ItemGroup>
  <MauiSplashScreen Include="Resources\Images\splashscreen.svg" />
</ItemGroup>
```

To comply with Android resource naming rules, splash screen files names must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

A background color for your splash screen can also be specified:

```xml
<MauiSplashScreen Include="Resources\Images\splashscreen.svg" Color="#512BD4" />
```

<!-- Valid color values are actually derived from the SKColor struct, rather than Microsoft.Maui.Graphics.Colors. This may change. -->
Color values can be specified in hexadecimal, or as a .NET MAUI color. For example, `Color="Red"` is valid.

At build time, the splash screen image is resized to the correct size for the target platform and device. The splash screen is then added to your app package.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

On Android, the splash screen is added to your app package as **Resourcs/values/maui_colors.xml** and **Resources/drawable/maui_splash_image.xml**. .NET MAUI apps use the `Maui.SplashTheme` by default, which ensures that a splash screen will be displayed if present. Therefore, you should not specify a different theme in your manifest file or in your `MainActivity` class:

<!-- Todo: P11 templates have implicit usings, so no need to include a using directive for Microsoft.Maui -->
```csharp
using Android.App;
using Android.Content.PM;

namespace MyMauiApp
{
      [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
      public class MainActivity : MauiAppCompatActivity
      {
      }
}
```

# [iOS](#tab/ios)

On iOS, the splash screen is added to the app package as a storyboard named `MauiSplash.storyboard`, which is set as value of the `UILaunchStoryboardName` key in the app package's **Info.plist**:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  ...
  <key>UILaunchStoryboardName</key>
  <string>MauiSplash</string>
  ...
</dict>
</plist>
```

Therefore, you shouldn't set the `UILaunchStoryboardName` key in your **Info.plist** file and you shouldn't add a `LaunchScreen.storyboard` to your app.

---

For more advanced splash screen scenarios, per-platform approaches apply.
