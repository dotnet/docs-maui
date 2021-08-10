---
title: "Paint images"
description: "The .NET MAUI graphics library includes an ImagePaint class, that paints a graphical object with an image."
ms.date: 07/22/2021
---

# Paint an image

<!-- Sample link goes here -->

The .NET Multi-platform App UI (.NET MAUI) graphics library includes an `ImagePaint` class that's derived from the `Paint` class, which is used to paint a graphical object with an image.

The `ImagePaint` class defines an `Image` property, of type `IImage`, which represents the image to paint. The class also has an `IsTransparent` property that returns `false`.

## Create an ImagePaint object

To paint an object with an image, load the image and assign it to the `Image` property of the `ImagePaint` object.

> [!NOTE]
> Loading an image that's embedded in an assembly requires the image to have its build action set to **Embedded Resource**.

The following example shows how to load an image and fill a rectangle with it:

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
        Image = image.Downsize(100)
    };
    canvas.SetFillPaint(imagePaint, RectangleF.Zero);
    canvas.FillRectangle(0, 0, 265, 300);
}
```

In this example, the image is retrieved from the assembly and loaded as a stream. The image is resized using the `Downsize` method, with the argument specifying that its largest dimension should be set to 100 pixels. For more information about downsizing an image, see [Downsize an image](~/user-interface/graphics/images.md#downsize-an-image).

The `Image` property of the `ImagePaint` object is set to the downsized version of the image, and the `ImagePaint` object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="image-images/imagepaint.png" alt-text="Screenshot of a rectangle, filled with an image.":::

> [!NOTE]
> An `ImagePaint` object can also be created from an `IImage` object by the `AsPaint` extension method.

Alternatively, the `SetFillImage` extension method can be used to simplify the code:

```csharp
if (image != null)
{
    canvas.SetFillImage(image.Downsize(100));
    canvas.FillRectangle(0, 0, 265, 300);
}
```
