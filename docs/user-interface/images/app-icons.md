---
title: "Add an app icon to a .NET MAUI app project"
description: "Learn how to add a .NET MAUI app icon to your app, which is the logo that represents your app in multiple places."
ms.date: 04/26/2022
---

# Add an app icon to a .NET MAUI app project

Every app has a logo icon that represents it, and that icon typically appears in multiple places. For example, on iOS the app icon appears on the Home screen and throughout the system, such as in Settings, notifications, and search results, and in the App Store. On Android, the app icon appears as a launcher icon and throughout the system, such as on the action bar and in notifications, and in the Google Play Store. On Windows the app icon appears in the app list in the start menu, on the taskbar and task manager, on the app's tile, and in the Microsoft Store.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In a .NET Multi-platform App UI (.NET MAUI) app project, an app icon can be specified in a single location in your app project, and at build time it can be automatically resized to the correct resolution for the target platform and device, and added to your app package. This avoids having to manually duplicate and name the app icon on a per platform basis. By default, bitmap (non-vector) image formats are not automatically resized by .NET MAUI.

A .NET MAUI app icon can use any of the standard platform image formats, including Scalable Vector Graphics (SVG) files.

> [!IMPORTANT]
> .NET MAUI converts SVG files to PNG files. Therefore, when adding an SVG file to your .NET MAUI app project, it should be referenced from XAML or C# with a .png extension.

An app icon can be added to your app project by dragging an image into the _Resources\Images_ folder of the project, and setting the build action of the image to `MauiIcon` in the **Properties** window. This creates a corresponding entry in your project file:

```xml
<ItemGroup>
    <MauiIcon Include="Resources\Images\appicon.svg" />
</ItemGroup>
```

> [!NOTE]
> An app icons can also be added to other folders of your app project.

To comply with Android resource naming rules, app icon filenames must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

The base size of the app icon can be specified by setting the `BaseSize` attribute to values that are divisible by 8:

```xml
<MauiIcon Include="Resources\Images\appicon.jpg" BaseSize="64,64" />
```

The value of the `BaseSize` attribute represents the baseline density of the image, and is effectively the 1.0 scale factor for the image from which all other density sizes are derived. This value will be used to ensure that the app icon is correctly resized to different display densities. If you don't specify a `BaseSize` for a bitmap-based app icon, the image isn't resized. If you don't specify a `BaseSize` value for a vector-based app icon, the dimensions specified in the SVG are assumed to be the base size. To stop vector images being resized, set the `Resize` attribute to `false`:

```xml
<MauiIcon Include="Resources\Images\appicon.svg" Resize="false" />
```

To add a tint to your app icon, which is useful when you have an icon or simple image you'd like to render in a different color to the source, set the `TintColor` attribute:

```xml
<MauiIcon Include="Resources\Images\appicon.svg" TintColor="#66B3FF" />
```

A background color for the app icon can also be specified:

```xml
<MauiIcon Include="Resources\Images\appicon.svg" Color="#512BD4" />
```

<!-- Valid color values are actually derived from the SKColor struct, rather than Microsoft.Maui.Graphics.Colors. -->
Color values can be specified in hexadecimal, or as a .NET MAUI color. For example, `Color="Red"` is valid.

At build time, the app icon can be resized to the correct resolutions for the target platform and device. The resulting app icon is then added to your app package.

<!-- markdownlint-disable MD025 -->

# [Android](#tab/android)

.NET MAUI supports creating an adaptive launcher icon on Android 8.0 and higher, from the app icon. Adaptive launcher icons can display as a variety of shapes across different device models, including circular and square. For more information about adaptive icons, see [Adaptive icons](https://developer.android.com/guide/practices/ui_guidelines/icon_design_adaptive) on developer.android.com.

Adaptive launcher icons are defined using a background layer and a foreground layer, and an optional scaling value:

```xml
<MauiIcon Include="Resources\Images\appiconback.svg" ForegroundFile="Resources\Images\appiconfore.svg" ForegroundScale="0.65" Color="#512BD4" />
```

The `ForegroundFile` attribute defines the main content of the icon. The `Include` attribute is used to specify the background layer, and is typically set to a color or pattern. In addition, the `ForegroundScale` attribute can be optionally specified to change the scaling of the foreground layer being rendered over the background layer.

> [!IMPORTANT]
> The `Color` attribute can still be specified when producing an adaptive launcher icon on Android so that a background color is generated for the app icon on other platforms.

On Android, drawable XML resources and bitmaps are generated from the app icon. An adaptive launcher icon is generated when the `ForegroundFile` attribute is defined.

# [iOS](#tab/ios)

On iOS, an asset catalog icon set is generated from the app icon.

---
