---
title: ".NET MAUI Graphics: Image paint"
description: "The .NET MAUI ImagePaint class paints an area with an image."
ms.date: 07/22/2021
---

# .NET MAUI Graphics: Image paint

The .NET Multi-platform App UI (MAUI) `ImagePaint` class derives from the `Paint` class, and is used to paint an area with an image.

The `ImagePaint` class defines an `Image` property, of type `IImage`, which represents the image to paint. The class also has an `IsTransparent` property that returns `false`.

## Create an ImagePaint object

An image must be loaded before an area can be painted with it. Once the image has been retrieved, it should be assigned to the `Image` property of the `ImagePaint` object.

> [!NOTE]
> Loading an image that's embedded in an assembly requires the image to have its build action set to **Embedded Resource**.

The following example shows how to load an image and fill an area with it:

```csharp
IImage image;
var assembly = GetType().GetTypeInfo().Assembly;
using (var stream = assembly.GetManifestResourceStream("MyMauiApp.Resources.Images.dotnet_bot.png"))
{
    image = GraphicsPlatform.CurrentService.LoadImageFromStream(stream);
}

if (image != null)
{
    ImagePaint imagePaint = new ImagePaint
    {
        Image = image.Downsize(100) // Dispose the original image
    };
    canvas.SetFillPaint(imagePaint, RectangleF.Zero);
    canvas.FillRectangle(0, 0, 265, 300);
}
```

In this example, the image is retrieved from the assembly and loaded as a stream. The image is resized using the `Downsize` method, with the argument specifying that its largest dimension should be set to 100 pixels.

> [!IMPORTANT]
> The `Downsize` method automatically disposes its source image. This can be disabled by specifying `false` to its `dispose` argument.

The `Image` property of the `ImagePaint` object is set to the downsized version of the image, and the `ImagePaint` object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="image-images/imagepaint.png" alt-text="Screenshot of a rectangle, filled with an image.":::

> [!NOTE]
> An `ImagePaint` object can also be created from an `IImage`` object by the `AsPaint` extension method.

Alternatively, the `SetFillImage` extension method can be used to simplify the code to paint an area with an image:

```csharp
if (image != null)
{
    canvas.SetFillImage(image.Downsize(100));
    canvas.FillRectangle(0, 0, 265, 300);
}
```
