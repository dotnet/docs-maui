---
title: "Images"
description: ".NET MAUI graphics includes functionality to load, save, resize, and downsize images."
ms.date: 04/22/2022
---

# Images

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-graphicsview)

.NET Multi-platform App UI (.NET MAUI) graphics includes functionality to load, save, resize, and downsize images. Supported image formats are dependent on the underlying platform.

Images are represented by the `IImage` type, which defines the following properties:

- `Width`, of type `float`, that defines the width of an image.
- `Height`, of type `float`, that defines the height of an image.

An optional `ImageFormat` argument can be specified when loading and saving images. The `ImageFormat` enumeration defines `Png`, `Jpeg`, `Gif`, `Tiff`, and `Bmp` members. However, this argument is only used when the image format is supported by the underlying platform.

> [!NOTE]
> .NET MAUI contains two different `IImage` interfaces. `Microsoft.Maui.Graphics.IImage` is used for image display, manipulation, and persistence in when displaying graphics in a <xref:Microsoft.Maui.Controls.GraphicsView>. `Microsoft.Maui.IImage` is the interface that abstracts the <xref:Microsoft.Maui.Controls.Image> control.

## Load an image

Image loading functionality is provided by the `PlatformImage` class. Images can be loaded from a stream by the `FromStream` method, or from a byte array using the `PlatformImage` constructor.

The following example shows how to load an image:

```csharp
using Microsoft.Maui.Graphics.Platform;
...

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
    image = PlatformImage.FromStream(stream);
}

if (image != null)
{
    canvas.DrawImage(image, 10, 10, image.Width, image.Height);
}
```

> [!WARNING]
> The `PlatformImage` type isn't supported on Windows.

In this example, the image is retrieved from the assembly, loaded as a stream, and displayed.

> [!IMPORTANT]
> Loading an image that's embedded in an assembly requires the image to have its build action set to **Embedded Resource** rather than **MauiImage**.

## Resize an image

Images can be resized using the `IImage.Resize` method, which requires `width` and `height` arguments, of type `float`, which represent the target dimensions of the image. The `Resize` method also accepts two optional arguments:

- A `ResizeMode` argument, that controls how the image will be resized to fit its target dimensions.
- A `bool` argument that controls whether the source image will be disposed after performing the resize operation. This argument defaults to `false`, indicating that the source image won't be disposed.

The `ResizeMode` enumeration defines the following members, which specify how to resize the image to the target size:

- `Fit`, which letterboxes the image so that it fits its target size.
- `Bleed`, which clips the image so that it fits its target size, while preserving its aspect ratio.
- `Stretch`, which stretches the image so it fills the available space. This can result in a change in the image aspect ratio.

The following example shows how to resize an image:

```csharp
using Microsoft.Maui.Graphics.Platform;
...

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
    image = PlatformImage.FromStream(stream);
}

if (image != null)
{
    IImage newImage = image.Resize(100, 60, ResizeMode.Stretch, true);
    canvas.DrawImage(newImage, 10, 10, newImage.Width, newImage.Height);
}
```

> [!WARNING]
> The `PlatformImage` type isn't supported on Windows.

In this example, the image is retrieved from the assembly and loaded as a stream. The image is resized using the `Resize` method, with its arguments specifying the new size, and that it should be stretched to fill the available space. In addition, the source image is disposed. The resized image is then drawn at actual size at (10,10).

## Downsize an image

Images can be downsized by one of the `IImage.Downsize` overloads. The first overload requires a single `float` value that represents the maximum width or height of the image, and downsizes the image while maintaining its aspect ratio. The second overload requires two `float` arguments, that represent the maximum width and maximum height of the image.

The `Downsize` overloads also accept an optional `bool` argument that controls whether the source image should be disposed after performing the downsizing operation. This argument defaults to `false`, indicating that the source image won't be disposed.

The following example shows how to downsize an image:

```csharp
using Microsoft.Maui.Graphics.Platform;
...

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
    image = PlatformImage.FromStream(stream);
}

if (image != null)
{
    IImage newImage = image.Downsize(100, true);
    canvas.DrawImage(newImage, 10, 10, newImage.Width, newImage.Height);
}
```

> [!WARNING]
> The `PlatformImage` type isn't supported on Windows.

In this example, the image is retrieved from the assembly and loaded as a stream. The image is downsized using the `Downsize` method, with the argument specifying that its largest dimension should be set to 100 pixels. In addition, the source image is disposed. The downsized image is then drawn at actual size at (10,10).

## Save an image

Images can be saved by the `IImage.Save` and `IImage.SaveAsync` methods. Each method saves the `IImage` to a `Stream`, and enables optional `ImageFormat` and quality values to be specified.

The following example shows how to save an image:

```csharp
using Microsoft.Maui.Graphics.Platform;
...

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
    image = PlatformImage.FromStream(stream);
}

// Save image to a memory stream
if (image != null)
{
    IImage newImage = image.Downsize(150, true);
    using (MemoryStream memStream = new MemoryStream())
    {
        newImage.Save(memStream);
    }
}
```

> [!WARNING]
> The `PlatformImage` type isn't supported on Windows.

In this example, the image is retrieved from the assembly and loaded as a stream. The image is downsized using the `Downsize` method, with the argument specifying that its largest dimension should be set to 150 pixels. In addition, the source image is disposed. The downsized image is then saved to a stream.
