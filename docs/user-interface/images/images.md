---
title: ".NET MAUI images"
description: "Learn how to add images to your .NET MAUI app, and control their resizing."
ms.date: 04/26/2022
---

# Add images to a .NET MAUI app project

Images are a crucial part of app navigation, usability, and branding. However, each platform has differing image requirements that typically involve creating multiple versions of each image at different resolutions. Therefore, a single image typically has to be duplicated multiple times per platform, at different resolutions, with the resulting images having to use different filename and folder conventions on each platform.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In a .NET Multi-platform App UI (.NET MAUI) app project, images can be specified in a single location in your app project, and at build time they are automatically resized to the correct resolutions for the target platform and device, and added to your app package. This avoids having to manually duplicate and name images on a per platform basis. .NET Multi-platform App UI (.NET MAUI) images can use any of the standard platform image formats, including Scalable Vector Graphics (SVG) files. By default, non-vector image formats are not automatically resized by .NET MAUI.

> [!TIP]
> The SVG format is the recommended image format for .NET MAUI images.

Images can be added to your app project by dragging them into the *Resources\Images* folder of the project, where their build action will automatically be set to **MauiImage**. This creates a corresponding entry in your project file:

```xml
<ItemGroup>
    <MauiImage Include="Resources\Images\logo.svg" />
</ItemGroup>
```

> [!NOTE]
> Images can also be added to other folders of your app project. However, in this scenario their build action must be manually set to `MauiImage` in the **properties** window.

To comply with Android resource naming rules, app icon filenames must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

By default, a .NET MAUI image maps to the *drawable-mdpi* resolution on Android, the *@1x* resolution on iOS, and the *scale-100* resolution on Windows. This behavior can be overridden by setting the `BaseSize` attribute to numbers that are divisible by 8:

```xml
<MauiImage Include="Resources\Images\logo.jpg" BaseSize="376,678" />
```

The value of the `BaseSize` attribute represents the baseline density size of the original image, and is effectively the 1.0 scale factor for the image (the size you would typically use in your code to specify the image size) from which all other density sizes are derived. This value will be used to ensure that vector images are correctly resized to different display densities.

To add a tint to your images, which is useful when you have icons or simple images you'd like to render in a different color to the source, set the `TintColor` attribute:

```xml
<MauiImage Include="Resources\Images\logo.svg" TintColor="#66B3FF" />
```

A background color for an image can also be specified:

```xml
 <MauiIcon Include="Resources\Images\appicon.svg" Color="#512BD4" />
```

<!-- Valid color values are actually derived from the SKColor struct, rather than Microsoft.Maui.Graphics.Colors. -->
Color values can be specified in hexadecimal, or as a .NET MAUI color. For example, `Color="Red"` is valid.

At build time, vector images are resized to the correct resolutions for the target platform and device. The resulting images, whether vector-based or not, are then added to your app package.

To stop vector images being resized, set the `Resize` attribute to `false`:

```xml
<MauiImage Include="Resources\Images\logo.png" Resize="false" />
```

> [!NOTE]
> To force a non-vector image to be resized, set the `Resize` attribute to `true`.
