---
title: "Paint graphical objects"
description: ".NET MAUI graphics includes several paint classes, that enable graphical objects to be painted with solid colors, gradients, images, and patterns."
ms.date: 12/16/2021
---

# Paint graphical objects

<!-- Sample link goes here -->

.NET Multi-platform App UI (.NET MAUI) graphics includes the ability to paint graphical objects with solid colors, gradients, repeating images, and patterns.

The `Paint` class is an abstract class that paints an object with its output. Classes that derive from `Paint` describe different ways of painting an object. The following list describes the different paint types available in .NET MAUI graphics:

- `SolidPaint`, which paints an object with a solid color. For more information, see [Solid color paint](#paint-a-solid-color).
- `GradientPaint`, which paints an object with a gradient. For more information, see [Gradient paint](gradient.md).
- `ImagePaint`, which paints an object with an image. For more information, see [Image paint](#paint-an-image).
- `PatternPaint`, which paints an object with a pattern. For more information, see [Pattern paint](#paint-a-pattern).

Instances of these types can be painted on an `ICanvas`, typically by using the `SetFillPaint` method to set the paint as the fill of a graphical object.

The `Paint` class also defines `BackgroundColor`, and `ForegroundColor` properties, of type `Color`, that can be used to optionally define background and foreground colors for a `Paint` object.

## Paint a solid color

The `SolidPaint` class, that's derived from the `Paint` class, is used to paint a graphical object with a solid color.

The `SolidPaint` class defines a `Color` property, of type `Color`, which represents the color of the paint. The class also has an `IsTransparent` property that returns a `bool` that represents whether the color has an alpha value of less than 1.

### Create a SolidPaint object

The color of a `SolidPaint` object is typically specified through its constructor, using a `Color` argument:

```csharp
SolidPaint solidPaint = new SolidPaint(Colors.Silver);

RectangleF solidRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```

The `SolidPaint` object is specified as the first argument to the `SetFillPaint` method. Therefore, a filled rounded rectangle is painted with a silver `SolidPaint` object:

:::image type="content" source="media/paint/solidpaint.png" alt-text="Screenshot of a rounded rectangle, filled with a silver SolidPaint object.":::

Alternatively, the color can be specified with the `Color` property:

```csharp
SolidPaint solidPaint = new SolidPaint
{
    Color = Colors.Silver
};

RectangleF solidRectangle = new RectangleF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```

## Paint an image

The `ImagePaint` class, that's derived from the `Paint` class, is used to paint a graphical object with an image.

The `ImagePaint` class defines an `Image` property, of type `IImage`, which represents the image to paint. The class also has an `IsTransparent` property that returns `false`.

### Create an ImagePaint object

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

:::image type="content" source="media/paint/imagepaint.png" alt-text="Screenshot of a rectangle, filled with an image.":::

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

## Paint a pattern

The `PatternPaint` class, that's derived from the `Paint` class, is used to paint a graphical object with a pattern.

The `PatternPaint` class defines a `Pattern` property, of type `IPattern`, which represents the pattern to paint. The class also has an `IsTransparent` property that returns a `bool` that represents whether the background or foreground color of the paint has an alpha value of less than 1.

### Create a PatternPaint object

To paint an area with a pattern, create the pattern and assign it to the `Pattern` property of a `PatternPaint` object.

The following example shows how to create a pattern and fill an object with it:

```csharp
IPattern pattern;

// Create a 10x10 template for the pattern
using (PictureCanvas picture = new PictureCanvas(0, 0, 10, 10))
{
    picture.StrokeColor = Colors.Silver;
    picture.DrawLine(0, 0, 10, 10);
    picture.DrawLine(0, 10, 10, 0);
    pattern = new PicturePattern(picture.Picture, 10, 10);
}

// Fill the rectangle with the 10x10 pattern
PatternPaint patternPaint = new PatternPaint
{
    Pattern = pattern
};
canvas.SetFillPaint(patternPaint, RectangleF.Zero);
canvas.FillRectangle(10, 10, 250, 250);
```

In this example, the pattern is a 10x10 area that contains a diagonal line from (0,0) to (10,10), and a diagonal line from (0,10) to (10,0). The `Pattern` property of the `PatternPaint` object is set to the pattern, and the `PatternPaint` object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="media/paint/patternpaint.png" alt-text="Screenshot of a rectangle, filled with a silver pattern.":::

> [!NOTE]
> A `PatternPaint` object can also be created from a `PicturePattern` object by the `AsPaint` extension method.
