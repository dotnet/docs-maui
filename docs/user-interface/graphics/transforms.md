---
title: ".NET MAUI Graphics: Transforms"
description: "The .NET MAUI 2D graphics library supports translation, scaling, and rotation transforms."
ms.date: 07/26/2021
---

# .NET MAUI Graphics: Transforms

The .NET Multi-platform App UI (MAUI) 2D graphics library supports traditional graphics transforms, which are implement as methods of the `ICanvas` object. Mathematically, transforms alter the coordinates and sizes that you specify in `ICanvas` drawing methods as the graphical objects are rendered.

`Microsoft.Maui.Graphics` supports the following transforms:

- *Translate* to shift coordinates from one location to another.
- *Scale* to increase or decrease coordinates and sizes.
- *Rotate* to rotate coordinates around a point.

These transforms are known as *affine* transforms. Affine transforms always preserve parallel lines and never cause a coordinate or size to become infinite.

> [!IMPORTANT]
> The .NET MAUI `VisualElement` class also supports the following transform properties: `TranslationX`, `TranslationY`, `Scale`, `Rotation`, `RotationX`, and `RotationY`.  However, there are several differences between `Microsoft.Maui.Graphics` transforms and `Microsoft.Maui.Controls.VisualElement` transforms:
>
> - `Microsoft.Maui.Graphics` transforms are methods, while `VisualElement` transforms are properties. Therefore, `Microsoft.Maui.Graphics` transforms perform an operation while `VisualElement` transforms set a state. `Microsoft.Maui.Graphics` transforms apply to subsequently drawn graphics objects, but not to graphics objects that are drawn before the transform is applied. In contrast, a `VisualElement` transform applies to a previously rendered element as soon as the property is set. Therefore, `Microsoft.Maui.Graphics` transforms are cumulative as methods are called, while `VisualElement` transforms are replaced when the property is set with another value.
> - `Microsoft.Maui.Graphics` transforms are applied to the `ICanvas` object, while `VisualElement` transforms are applied to `VisualElement` derived object.
> - `Microsoft.Maui.Graphics` transforms are relative to the upper-left corner of the `ICanvas`, while `VisualElement` transforms are relative to the upper-left corner of the `VisualElement` to which they are applied.

## Translate transform

The translate, or translation, transform shifts graphical objects in the horizontal and vertical directions. Translation can be considered unnecessary because the same result can be accomplished by changing the coordinates of the drawing method you're using. However, when displaying a path, all the coordinates are encapsulated in the path, and so it's easier to apply a translate transform to shift the entire path.

The `Translate` method requires `x` and `y` `float` arguments that cause subsequently drawn graphic objects to be shifted horizontally and vertically. Negative `x` values move an object to the left, while positive values move an object to the right. Negative `y` values move an object up, while positive values move an object down.

A common use of the translate transform is for rendering a graphical object that has been originally created using coordinates that are convenient for drawing. The following example creates a `PathF` object for an 11-pointed star:

```csharp
PathF path = new PathF();
for (int i = 0; i < 11; i++)
{
    double angle = 5 * i * 2 * Math.PI / 11;
    PointF point = new PointF(100 * (float)Math.Sin(angle), -100 * (float)Math.Cos(angle));

    if (i == 0)
        path.MoveTo(point);
    else
        path.LineTo(point);
}

canvas.FillColor = Colors.Red;
canvas.Translate(150, 150);
canvas.FillPath(path);
```

The center of the star is at (0,0), and the points of he star are on a circle surrounding that point. Each point is a combination of sine and cosine values of an angle that increases by 5/11ths of 360 degrees. The radius of the circle is set as 100. If the `PathF` object is displayed without any transforms, the center of the star will be positioned at the upper-left corner of the `ICanvas`, and only a quarter of it will be visible. Therefore, a translate transform is used to shift the star horizontally and vertically to (150,150):

:::image type="content" source="transform-images/translate.png" alt-text="Screenshot of an 11-pointed star.":::

## Scale transform

The scale transform changes the size of a graphical object, and can also often cause coordinates to move as a graphical object is made larger.

The `Scale` method requires `x` and `y` `float` arguments that let you specify different values for horizontal and vertical scaling, otherwise known as *anisotropic* scaling. The values of `x` and `y` have a big impact on the resulting scaling:

- Values between 0 and 1 decrease the width and height of the scaled object.
- Values greater than 1 increase the width and height of the scaled object.
- Values of 1 indicate that the object is not scaled.
<!--
- Negative values flip the scaled object horizontally and vertically.
- Values between 0 and -1 flip the scaled object and decrease its width and height.
- Values less than -1 flip the object and increase its width and height.
- Values of -1 flip the scaled object but do not change its horizontal or vertical size.
-->

The following example demonstrates the `Scale` method:

```csharp
canvas.StrokeColor = Colors.Red;
canvas.StrokeSize = 4;
canvas.StrokeDashPattern = new float[] { 2, 2 };
canvas.FontColor = Colors.Blue;
canvas.FontSize = 18;

canvas.DrawRoundedRectangle(50, 50, 80, 20, 5);
canvas.DrawString(".NET MAUI", 50, 50, 80, 20, HorizontalAlignment.Left, VerticalAlignment.Top);

canvas.Scale(2, 2);
canvas.DrawRoundedRectangle(50, 100, 80, 20, 5);
canvas.DrawString(".NET MAUI", 50, 100, 80, 20, HorizontalAlignment.Left, VerticalAlignment.Top);
```

In this example, ".NET MAUI" is displayed inside a rounded rectangle stroked with a dashed line. The same objects drawn after the `Scale` call increase in size proportionally:

:::image type="content" source="transform-images/scale.png" alt-text="Screenshot of unscaled and scaled text.":::

The text and the rounded rectangle are both subject to the same scaling factors.

> [!NOTE]
> Anisotropic scaling causes the stroke size to become different for lines aligned with the horizontal and vertical axes.

Order matters when you combine `Translate` and `Scale` calls. If the `Translate` call comes after the `Scale` call, the translation factors are scaled by the scaling factors. If the `Translate` call comes before the `Scale` call, the translation factors are not scaled.

## Rotate transform

https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/shapes/path-transforms
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/rotate

The rotate transform rotates a graphical object around a point. Rotation is clockwise for increasing angles. Negative angles and angles greater than 360 degrees are allowed.

There are two `Rotate` overloads. The first requires a `degrees` argument, of type `float`, that defines the rotation angle, and centers the rotation around the upper-left corner of the canvas (0,0). The following example demonstrates this `Rotate` method:

```csharp
canvas.FontColor = Colors.Blue;
canvas.FontSize = 18;

canvas.Rotate(45);
canvas.DrawString(".NET MAUI", 50, 50, HorizontalAlignment.Left);
```

In this example, ".NET MAUI" is rotated 45 degrees clockwise:

:::image type="content" source="transform-images/rotate.png" alt-text="Screenshot of rotated text.":::

Alternatively, graphical objects can be rotated centered around a specific point. This requires the `Rotate` overload that accepts `degrees`, `x`, and `y` `float` arguments:

```csharp
canvas.FontColor = Colors.Blue;
canvas.FontSize = 18;

canvas.Rotate(45, dirtyRect.Width / 2, dirtyRect.Height / 2);
canvas.DrawString(".NET MAUI", dirtyRect.Width / 2, dirtyRect.Height / 2, HorizontalAlignment.Left);
```

In this example, `.NET MAUI` is rotated 45 degrees around the center of the canvas.

## Combine transforms

The easiest way to combine transforms is to begin with global transforms, followed by local transforms. For example, translation, scaling, and rotation can be combined to draw an analog clock.

The clock can be drawn using an arbitrary coordinate system based on a circle that's centered at (0,0) with a radius of 100. Translation and scaling expand and center the circle on the page, and rotation can then be used to draw the minute and hour marks of the clock and to rotate the hands:

```csharp
canvas.StrokeLineCap = LineCap.Round;
canvas.FillColor = Colors.Gray;

// Translation and scaling
canvas.Translate(dirtyRect.Width / 2f, dirtyRect.Height / 2f);
float scale = Math.Min(dirtyRect.Width / 200f, dirtyRect.Height / 200f);
canvas.Scale(scale, scale);

// Hour and minute marks
for (int angle = 0; angle < 360; angle += 6)
{
    canvas.FillCircle(0, -90, angle % 30 == 0 ? 4 : 2);
    canvas.Rotate(6);
}

DateTime now = DateTime.Now;

// Hour hand
canvas.StrokeSize = 20;
canvas.SaveState();
canvas.Rotate(30 * now.Hour + now.Minute / 2f);
canvas.DrawLine(0, 0, 0, -50);
canvas.RestoreState();

// Minute hand
canvas.StrokeSize = 10;
canvas.SaveState();
canvas.Rotate(6 * now.Minute + now.Second / 10f);
canvas.DrawLine(0, 0, 0, -70);
canvas.RestoreState();

// Second hand
canvas.StrokeSize = 2;
canvas.SaveState();
canvas.Rotate(6 * now.Second);
canvas.DrawLine(0, 10, 0, -80);
canvas.RestoreState();
```

In this example, the `Translate` and `Scale` calls apply globally to the clock, and so are called before the `Rotate` method.

There are 60 marks of two different sizes that are drawn in a circle around the clock. The `FillCircle` call draws that circle at (0,-90), which relative to the center of the clock corresponds to 12:00. The `Rotate` call increments the rotation angle by 6 degrees after every tick mark. The `angle` variable is used solely to determine if a large circle or a small circle is drawn. Finally, the current time is obtained and rotation degrees are calculated for the hour, minute, and second hands. Each hand is drawn in the 12:00 position so that the rotation angle is relative to that.

## Concatenate transforms (aka Matrix transforms?)
