---
title: "Add images to a .NET MAUI app project"
description: "Learn how to add images to your .NET MAUI app, and control their resizing."
ms.date: 04/26/2022
---

# Add images to a .NET MAUI app project

Images are a crucial part of app navigation, usability, and branding. However, each platform has differing image requirements that typically involve creating multiple versions of each image at different resolutions. Therefore, a single image typically has to be duplicated multiple times per platform, at different resolutions, with the resulting images having to use different filename and folder conventions on each platform.

In a .NET Multi-platform App UI (.NET MAUI) app project, images can be specified in a single location in your app project, and at build time they can be automatically resized to the correct resolution for the target platform and device, and added to your app package. This avoids having to manually duplicate and name images on a per platform basis. By default, bitmap (non-vector) image formats, including animated GIFs, are not automatically resized by .NET MAUI.

.NET MAUI images can use any of the standard platform image formats, including Scalable Vector Graphics (SVG) files.

> [!IMPORTANT]
> .NET MAUI converts SVG files to PNG files. Therefore, when adding an SVG file to your .NET MAUI app project, it should be referenced from XAML or C# with a .png extension. The only reference to the SVG file should be in your project file.

An image can be added to your app project by dragging it into the *Resources\Images* folder of the project, where its build action will automatically be set to **MauiImage**. This creates a corresponding entry in your project file:

```xml
<ItemGroup>
    <MauiImage Include="Resources\Images\logo.svg" />
</ItemGroup>
```

> [!NOTE]
> Images can also be added to other folders of your app project. However, in this scenario their build action must be manually set to **MauiImage** in the **Properties** window.

To comply with Android resource naming rules, image filenames must be lowercase, start and end with a letter character, and contain only alphanumeric characters or underscores. For more information, see [App resources overview](https://developer.android.com/guide/topics/resources/providing-resources) on developer.android.com.

The base size of the image can be specified by setting the `BaseSize` attribute to values that are divisible by 8:

```xml
<MauiImage Include="Resources\Images\logo.jpg" BaseSize="376,678" />
```

The value of the `BaseSize` attribute represents the baseline density of the image, and is effectively the 1.0 scale factor for the image (the size you would typically use in your code to specify the image size) from which all other density sizes are derived. This value will be used to ensure that images are correctly resized to different display densities. If you don't specify a `BaseSize` for a bitmap image, the image isn't resized. If you don't specify a `BaseSize` value for a vector image, the dimensions specified in the SVG are assumed to be the base size. To stop vector images being resized, set the `Resize` attribute to `false`:

```xml
<MauiImage Include="Resources\Images\logo.svg" Resize="false" />
```

To add a tint to your images, which is useful when you have icons or simple images you'd like to render in a different color to the source, set the `TintColor` attribute:

```xml
<MauiImage Include="Resources\Images\logo.svg" TintColor="#66B3FF" />
```

A background color for an image can also be specified:

```xml
<MauiImage Include="Resources\Images\logo.svg" Color="#512BD4" />
```

<!-- Valid color values are actually derived from the SKColor struct, rather than Microsoft.Maui.Graphics.Colors. -->
Color values can be specified in hexadecimal, or as a .NET MAUI color. For example, `Color="Red"` is valid.

At build time, images can be resized to the correct resolutions for the target platform and device. The resulting images are then added to your app package.
