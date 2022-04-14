---
title: "App icons"
description: "Learn how to add a .NET MAUI app icon to your app, which is the logo that represents your app in multiple places."
ms.date: 01/14/2022
---

# App icons

Every app has a logo icon that represents it, and that icon typically appears in multiple places. For example, on iOS the app icon appears on the Home screen and throughout the system, such as in Settings, notifications, and search results, and in the App Store. On Android, the app icon appears as a launcher icon and throughout the system, such as on the action bar and in notifications, and in the Google Play Store. On Windows the app icon appears in the app list in the start menu, on the taskbar and task manager, on the app's tile, and in the Microsoft Store.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

A .NET Multi-platform App UI (.NET MAUI) app icon is a composite of an image and a background color. The standard platform image formats are supported, including Scalable Vector Graphics (SVG) files.

> [!TIP]
> The SVG format is the recommended image format for .NET MAUI app icons.

An app icon can be added to your app project by dragging an image into the _Resources\Images_ folder of the project, and setting the build action of the image to `MauiIcon` in the **Properties** window. This creates a corresponding entry in your project file:

```xml
<ItemGroup>
    <MauiIcon Include="Resources\Images\appicon.svg" />
</ItemGroup>
```

To comply with Android resource naming rules, app icon filenames must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

A background color for the app icon can also be specified:

```xml
 <MauiIcon Include="Resources\Images\appicon.svg" Color="#512BD4" />
```

<!-- Valid color values are actually derived from the SKColor struct, rather than Microsoft.Maui.Graphics.Colors. -->
Color values can be specified in hexadecimal, or as a .NET MAUI color. For example, `Color="Red"` is valid.

At build time, the app icon is resized to the correct sizes for the target platform and device. The resized app icons are then added to your app package.

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
