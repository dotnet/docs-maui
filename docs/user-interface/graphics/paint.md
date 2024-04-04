---
title: "Paint graphical objects"
description: ".NET MAUI graphics includes several paint classes, that enable graphical objects to be painted with solid colors, gradients, images, and patterns."
ms.date: 06/19/2023
---

# Paint graphical objects

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-graphicsview)

.NET Multi-platform App UI (.NET MAUI) graphics includes the ability to paint graphical objects with solid colors, gradients, repeating images, and patterns.

The <xref:Microsoft.Maui.Graphics.Paint> class is an abstract class that paints an object with its output. Classes that derive from <xref:Microsoft.Maui.Graphics.Paint> describe different ways of painting an object. The following list describes the different paint types available in .NET MAUI graphics:

- <xref:Microsoft.Maui.Graphics.SolidPaint>, which paints an object with a solid color. For more information, see [Paint a solid color](#paint-a-solid-color).
- <xref:Microsoft.Maui.Graphics.ImagePaint>, which paints an object with an image. For more information, see [Paint an image](#paint-an-image).
- <xref:Microsoft.Maui.Graphics.PatternPaint>, which paints an object with a pattern. For more information, see [Paint a pattern](#paint-a-pattern).
- <xref:Microsoft.Maui.Graphics.GradientPaint>, which paints an object with a gradient. For more information, see [Paint a gradient](#paint-a-gradient).

Instances of these types can be painted on an <xref:Microsoft.Maui.Graphics.ICanvas>, typically by using the <xref:Microsoft.Maui.Graphics.ICanvas.SetFillPaint%2A> method to set the paint as the fill of a graphical object.

The <xref:Microsoft.Maui.Graphics.Paint> class also defines <xref:Microsoft.Maui.Graphics.Paint.BackgroundColor>, and <xref:Microsoft.Maui.Graphics.Paint.ForegroundColor> properties, of type <xref:Microsoft.Maui.Graphics.Color>, that can be used to optionally define background and foreground colors for a <xref:Microsoft.Maui.Graphics.Paint> object.

## Paint a solid color

The <xref:Microsoft.Maui.Graphics.SolidPaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.Paint> class, is used to paint a graphical object with a solid color.

The <xref:Microsoft.Maui.Graphics.SolidPaint> class defines a <xref:Microsoft.Maui.Graphics.SolidPaint.Color> property, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the color of the paint. The class also has an <xref:Microsoft.Maui.Graphics.SolidPaint.IsTransparent> property that returns a `bool` that represents whether the color has an alpha value of less than 1.

### Create a SolidPaint object

The color of a <xref:Microsoft.Maui.Graphics.SolidPaint> object is typically specified through its constructor, using a <xref:Microsoft.Maui.Graphics.Color> argument:

```csharp
SolidPaint solidPaint = new SolidPaint(Colors.Silver);

RectF solidRectangle = new RectF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```

The <xref:Microsoft.Maui.Graphics.SolidPaint> object is specified as the first argument to the <xref:Microsoft.Maui.Graphics.ICanvas.SetFillPaint%2A> method. Therefore, a filled rounded rectangle is painted with a silver <xref:Microsoft.Maui.Graphics.SolidPaint> object:

:::image type="content" source="media/paint/solidpaint.png" alt-text="Screenshot of a rounded rectangle, filled with a silver SolidPaint object.":::

Alternatively, the color can be specified with the <xref:Microsoft.Maui.Graphics.SolidPaint.Color> property:

```csharp
SolidPaint solidPaint = new SolidPaint
{
    Color = Colors.Silver
};

RectF solidRectangle = new RectF(100, 100, 200, 100);
canvas.SetFillPaint(solidPaint, solidRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(solidRectangle, 12);
```

## Paint an image

The <xref:Microsoft.Maui.Graphics.ImagePaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.Paint> class, is used to paint a graphical object with an image.

The <xref:Microsoft.Maui.Graphics.ImagePaint> class defines an <xref:Microsoft.Maui.Graphics.ImagePaint.Image> property, of type <xref:Microsoft.Maui.Graphics.IImage>, which represents the image to paint. The class also has an <xref:Microsoft.Maui.Graphics.ImagePaint.IsTransparent> property that returns `false`.

### Create an ImagePaint object

To paint an object with an image, load the image and assign it to the <xref:Microsoft.Maui.Graphics.ImagePaint.Image> property of the <xref:Microsoft.Maui.Graphics.ImagePaint> object.

> [!NOTE]
> Loading an image that's embedded in an assembly requires the image to have its build action set to **Embedded Resource**.

The following example shows how to load an image and fill a rectangle with it:

::: moniker range="=net-maui-7.0"

```csharp
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;
#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
#if IOS || ANDROID || MACCATALYST
    // PlatformImage isn't currently supported on Windows.
    image = PlatformImage.FromStream(stream);
#elif WINDOWS
    image = new W2DImageLoadingService().FromStream(stream);
#endif
}

if (image != null)
{
    ImagePaint imagePaint = new ImagePaint
    {
        Image = image.Downsize(100)
    };
    canvas.SetFillPaint(imagePaint, RectF.Zero);
    canvas.FillRectangle(0, 0, 240, 300);
}
```

::: moniker-end

::: moniker range=">=net-maui-8.0"

```csharp
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;
using Microsoft.Maui.Graphics.Platform;

IImage image;
Assembly assembly = GetType().GetTypeInfo().Assembly;
using (Stream stream = assembly.GetManifestResourceStream("GraphicsViewDemos.Resources.Images.dotnet_bot.png"))
{
    image = PlatformImage.FromStream(stream);
}

if (image != null)
{
    ImagePaint imagePaint = new ImagePaint
    {
        Image = image.Downsize(100)
    };
    canvas.SetFillPaint(imagePaint, RectF.Zero);
    canvas.FillRectangle(0, 0, 240, 300);
}
```

::: moniker-end

In this example, the image is retrieved from the assembly and loaded as a stream. The image is resized using the <xref:Microsoft.Maui.Graphics.IImage.Downsize%2a> method, with the argument specifying that its largest dimension should be set to 100 pixels. For more information about downsizing an image, see [Downsize an image](~/user-interface/graphics/images.md#downsize-an-image).

The <xref:Microsoft.Maui.Graphics.ImagePaint.Image> property of the <xref:Microsoft.Maui.Graphics.ImagePaint> object is set to the downsized version of the image, and the <xref:Microsoft.Maui.Graphics.ImagePaint> object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="media/paint/imagepaint.png" alt-text="Screenshot of a rectangle, filled with an image.":::

> [!NOTE]
> An <xref:Microsoft.Maui.Graphics.ImagePaint> object can also be created from an <xref:Microsoft.Maui.Graphics.IImage> object by the `AsPaint` extension method.

Alternatively, the <xref:Microsoft.Maui.Graphics.ImageExtensions.SetFillImage%2A> extension method can be used to simplify the code:

```csharp
if (image != null)
{
    canvas.SetFillImage(image.Downsize(100));
    canvas.FillRectangle(0, 0, 240, 300);
}
```

## Paint a pattern

The <xref:Microsoft.Maui.Graphics.PatternPaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.Paint> class, is used to paint a graphical object with a pattern.

The <xref:Microsoft.Maui.Graphics.PatternPaint> class defines a <xref:Microsoft.Maui.Graphics.PatternPaint.Pattern> property, of type <xref:Microsoft.Maui.Graphics.IPattern>, which represents the pattern to paint. The class also has an <xref:Microsoft.Maui.Graphics.PatternPaint.IsTransparent> property that returns a `bool` that represents whether the background or foreground color of the paint has an alpha value of less than 1.

### Create a PatternPaint object

To paint an area with a pattern, create the pattern and assign it to the <xref:Microsoft.Maui.Graphics.PatternPaint.Pattern> property of a <xref:Microsoft.Maui.Graphics.PatternPaint> object.

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
canvas.SetFillPaint(patternPaint, RectF.Zero);
canvas.FillRectangle(10, 10, 250, 250);
```

In this example, the pattern is a 10x10 area that contains a diagonal line from (0,0) to (10,10), and a diagonal line from (0,10) to (10,0). The <xref:Microsoft.Maui.Graphics.PatternPaint.Pattern> property of the <xref:Microsoft.Maui.Graphics.PatternPaint> object is set to the pattern, and the <xref:Microsoft.Maui.Graphics.PatternPaint> object is set as the paint to fill an object with. A rectangle is then drawn that's filled with the paint:

:::image type="content" source="media/paint/patternpaint.png" alt-text="Screenshot of a rectangle, filled with a silver pattern.":::

> [!NOTE]
> A <xref:Microsoft.Maui.Graphics.PatternPaint> object can also be created from a `PicturePattern` object by the `AsPaint` extension method.

## Paint a gradient

The <xref:Microsoft.Maui.Graphics.GradientPaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.Paint> class, is an abstract base class that describes a gradient, which is composed of gradient steps. A <xref:Microsoft.Maui.Graphics.GradientPaint> paints a graphical object with multiple colors that blend into each other along an axis. Classes that derive from <xref:Microsoft.Maui.Graphics.GradientPaint> describe different ways of interpreting gradients stops, and .NET MAUI graphics provides the following gradient paints:

- <xref:Microsoft.Maui.Graphics.LinearGradientPaint>, which paints an object with a linear gradient. For more information, see [Paint a linear gradient](#paint-a-linear-gradient).
- <xref:Microsoft.Maui.Graphics.RadialGradientPaint>, which paints an object with a radial gradient. For more information, see [Paint a radial gradient](#paint-a-radial-gradient).

The <xref:Microsoft.Maui.Graphics.GradientPaint> class defines the <xref:Microsoft.Maui.Graphics.GradientPaint.GradientStops> property, of type <xref:Microsoft.Maui.Graphics.PaintGradientStop>, which represents the brush's gradient stops, each of which specifies a color and an offset along the gradient axis.

### Gradient stops

Gradient stops are the building blocks of a gradient, and specify the colors in the gradient and their location along the gradient axis. Gradient stops are specified using <xref:Microsoft.Maui.Graphics.PaintGradientStop> objects.

The <xref:Microsoft.Maui.Graphics.PaintGradientStop> class defines the following properties:

- <xref:Microsoft.Maui.Graphics.PaintGradientStop.Color>, of type <xref:Microsoft.Maui.Graphics.Color>, which represents the color of the gradient stop.
- <xref:Microsoft.Maui.Graphics.PaintGradientStop.Offset>, of type `float`, which represents the location of the gradient stop within the gradient vector. Valid values are in the range 0.0-1.0. The closer this value is to 0, the closer the color is to the start of the gradient. Similarly, the closer this value is to 1, the closer the color is to the end of the gradient.

> [!IMPORTANT]
> The coordinate system used by gradients is relative to a bounding box for the graphical object. 0 indicates 0 percent of the bounding box, and 1 indicates 100 percent of the bounding box. Therefore, (0.5,0.5) describes a point in the middle of the bounding box, and (1,1) describes a point at the bottom right of the bounding box.

Gradient stops can be added to a <xref:Microsoft.Maui.Graphics.GradientPaint> object with the <xref:Microsoft.Maui.Graphics.GradientPaint.AddOffset%2A> method.

The following example creates a diagonal <xref:Microsoft.Maui.Graphics.LinearGradientPaint> with four colors:

```csharp
LinearGradientPaint linearGradientPaint = new LinearGradientPaint
{
    StartColor = Colors.Yellow,
    EndColor = Colors.Green,
    StartPoint = new Point(0, 0),
    EndPoint = new Point(1, 1)
};

linearGradientPaint.AddOffset(0.25f, Colors.Red);
linearGradientPaint.AddOffset(0.75f, Colors.Blue);

RectF linearRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(linearGradientPaint, linearRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(linearRectangle, 12);                                                     
```

The color of each point between gradient stops is interpolated as a combination of the color specified by the two bounding gradient stops. The following diagram shows the gradient stops from the previous example:

:::image type="content" source="media/paint/gradient-stops.png" alt-text="Screenshot of a rounded rectangle, filled with a diagonal LinearGradientPaint." border="false":::

In this diagram, the circles mark the position of gradient stops, and the dashed line shows the gradient axis. The first gradient stop specifies the color yellow at an offset of 0.0. The second gradient stop specifies the color red at an offset of 0.25. The points between these two gradient stops gradually change from yellow to red as you move from left to right along the gradient axis. The third gradient stop specifies the color blue at an offset of 0.75. The points between the second and third gradient stops gradually change from red to blue. The fourth gradient stop specifies the color lime green at offset of 1.0. The points between the third and fourth gradient stops gradually change from blue to lime green.

## Paint a linear gradient

The <xref:Microsoft.Maui.Graphics.LinearGradientPaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.GradientPaint> class, paints a graphical object with a linear gradient. A linear gradient blends two or more colors along a line known as the gradient axis. <xref:Microsoft.Maui.Graphics.PaintGradientStop> objects are used to specify the colors in the gradient and their positions. For more information about <xref:Microsoft.Maui.Graphics.PaintGradientStop> objects, see [Paint a gradient](#paint-a-gradient).

The <xref:Microsoft.Maui.Graphics.LinearGradientPaint> class defines the following properties:

- <xref:Microsoft.Maui.Graphics.LinearGradientPaint.StartPoint>, of type <xref:Microsoft.Maui.Graphics.Point>, which represents the starting two-dimensional coordinates of the linear gradient. The class constructor initializes this property to (0,0).
- <xref:Microsoft.Maui.Graphics.LinearGradientPaint.EndPoint>, of type <xref:Microsoft.Maui.Graphics.Point>, which represents the ending two-dimensional coordinates of the linear gradient. The class constructor initializes this property to (1,1).

### Create a LinearGradientPaint object

A linear gradient's gradient stops are positioned along the gradient axis. The orientation and size of the gradient axis can be changed using the <xref:Microsoft.Maui.Graphics.LinearGradientPaint.StartPoint> and <xref:Microsoft.Maui.Graphics.LinearGradientPaint.EndPoint> properties. By manipulating these properties, you can create horizontal, vertical, and diagonal gradients, reverse the gradient direction, condense the gradient spread, and more.

The <xref:Microsoft.Maui.Graphics.LinearGradientPaint.StartPoint> and <xref:Microsoft.Maui.Graphics.LinearGradientPaint.EndPoint> properties are relative to the graphical object being painted. (0,0) represents the top-left corner of the object being painted, and (1,1) represents the bottom-right corner of the object being painted. The following diagram shows the gradient axis for a diagonal linear gradient brush:

:::image type="content" source="media/paint/lineargradient-axis.png" alt-text="The gradient axis for diagonal linear gradient." border="false":::

In this diagram, the dashed line shows the gradient axis, which highlights the interpolation path of the gradient from the start point to the end point.

#### Create a horizontal linear gradient

To create a horizontal linear gradient, create a <xref:Microsoft.Maui.Graphics.LinearGradientPaint> object and set its <xref:Microsoft.Maui.Graphics.GradientPaint.StartColor> and <xref:Microsoft.Maui.Graphics.GradientPaint.EndColor> properties. Then, set its <xref:Microsoft.Maui.Graphics.LinearGradientPaint.EndPoint> to (1,0).

The following example shows how to create a horizontal <xref:Microsoft.Maui.Graphics.LinearGradientPaint>:

```csharp
LinearGradientPaint linearGradientPaint = new LinearGradientPaint
{
    StartColor = Colors.Yellow,
    EndColor = Colors.Green,
    // StartPoint is already (0,0)
    EndPoint = new Point(1, 0)
};

RectF linearRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(linearGradientPaint, linearRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(linearRectangle, 12);
```

In this example, the rounded rectangle is painted with a linear gradient that interpolates horizontally from yellow to green:

:::image type="content" source="media/paint/lineargradient-horizontal.png" alt-text="Screenshot of a rounded rectangle, filled with a horizontal linear gradient.":::

#### Create a vertical linear gradient

To create a vertical linear gradient, create a <xref:Microsoft.Maui.Graphics.LinearGradientPaint> object and set its <xref:Microsoft.Maui.Graphics.GradientPaint.StartColor> and <xref:Microsoft.Maui.Graphics.GradientPaint.EndColor> properties. Then, set its <xref:Microsoft.Maui.Graphics.LinearGradientPaint.EndPoint> to (0,1).

The following example shows how to create a vertical <xref:Microsoft.Maui.Graphics.LinearGradientPaint>:

```csharp
LinearGradientPaint linearGradientPaint = new LinearGradientPaint
{
    StartColor = Colors.Yellow,
    EndColor = Colors.Green,
    // StartPoint is already (0,0)
    EndPoint = new Point(0, 1)
};

RectF linearRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(linearGradientPaint, linearRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(linearRectangle, 12);
```

In this example, the rounded rectangle is painted with a linear gradient that interpolates vertically from yellow to green:

:::image type="content" source="media/paint/lineargradient-vertical.png" alt-text="Screenshot of a rounded rectangle, filled with a vertical linear gradient.":::

#### Create a diagonal linear gradient

To create a diagonal linear gradient, create a <xref:Microsoft.Maui.Graphics.LinearGradientPaint> object and set its <xref:Microsoft.Maui.Graphics.GradientPaint.StartColor> and <xref:Microsoft.Maui.Graphics.GradientPaint.EndColor> properties.

The following example shows how to create a diagonal <xref:Microsoft.Maui.Graphics.LinearGradientPaint>:

```csharp
LinearGradientPaint linearGradientPaint = new LinearGradientPaint
{
    StartColor = Colors.Yellow,
    EndColor = Colors.Green,
    // StartPoint is already (0,0)
    // EndPoint is already (1,1)
};

RectF linearRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(linearGradientPaint, linearRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(linearRectangle, 12);
```

In this example, the rounded rectangle is painted with a linear gradient that interpolates diagonally from yellow to green:

:::image type="content" source="media/paint/lineargradient-diagonal.png" alt-text="Screenshot of a rounded rectangle, filled with a diagonal linear gradient.":::

## Paint a radial gradient

The <xref:Microsoft.Maui.Graphics.RadialGradientPaint> class, that's derived from the <xref:Microsoft.Maui.Graphics.GradientPaint> class, paints a graphical object with a radial gradient. A radial gradient blends two or more colors across a circle. <xref:Microsoft.Maui.Graphics.PaintGradientStop> objects are used to specify the colors in the gradient and their positions. For more information about <xref:Microsoft.Maui.Graphics.PaintGradientStop> objects, see [Paint a gradient](#paint-a-gradient).

The <xref:Microsoft.Maui.Graphics.RadialGradientPaint> class defines the following properties:

- <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Center>, of type <xref:Microsoft.Maui.Graphics.Point>, which represents the center point of the circle for the radial gradient. The class constructor initializes this property to (0.5,0.5).
- <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Radius>, of type `double`, which represents the radius of the circle for the radial gradient. The class constructor initializes this property to 0.5.

### Create a RadialGradientPaint object

A radial gradient's gradient stops are positioned along a gradient axis defined by a circle. The gradient axis radiates from the center of the circle to its circumference. The position and size of the circle can be changed using the <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Center> and <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Radius> properties. The circle defines the end point of the gradient. Therefore, a gradient stop at 1.0 defines the color at the circle's circumference. A gradient stop at 0.0 defines the color at the center of the circle.

To create a radial gradient, create a <xref:Microsoft.Maui.Graphics.RadialGradientPaint> object and set its <xref:Microsoft.Maui.Graphics.GradientPaint.StartColor> and <xref:Microsoft.Maui.Graphics.GradientPaint.EndColor> properties. Then, set its <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Center> and <xref:Microsoft.Maui.Graphics.RadialGradientPaint.Radius> properties.

The following example shows how to create a centered <xref:Microsoft.Maui.Graphics.RadialGradientPaint>:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue
    // Center is already (0.5,0.5)
    // Radius is already 0.5
};

RectF radialRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the center of the rectangle:

:::image type="content" source="media/paint/radialgradient-center.png" alt-text="Screenshot of a rounded rectangle, filled with a centered radial gradient.":::

The following example moves the center of the radial gradient to the top-left corner of the rectangle:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue,
    Center = new Point(0.0, 0.0)
    // Radius is already 0.5
};

RectF radialRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the top-left of the rectangle:

:::image type="content" source="media/paint/radialgradient-top-left.png" alt-text="Screenshot of a rounded rectangle, filled with a top-left radial gradient.":::

The following example moves the center of the radial gradient to the bottom-right corner of the rectangle:

```csharp
RadialGradientPaint radialGradientPaint = new RadialGradientPaint
{
    StartColor = Colors.Red,
    EndColor = Colors.DarkBlue,
    Center = new Point(1.0, 1.0)
    // Radius is already 0.5
};

RectF radialRectangle = new RectF(10, 10, 200, 100);
canvas.SetFillPaint(radialGradientPaint, radialRectangle);
canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
canvas.FillRoundedRectangle(radialRectangle, 12);
```

In this example, the rounded rectangle is painted with a radial gradient that interpolates from red to dark blue. The center of the radial gradient is positioned in the bottom-right of the rectangle:

:::image type="content" source="media/paint/radialgradient-bottom-right.png" alt-text="Screenshot of a rounded rectangle, filled with a bottom-right radial gradient.":::
