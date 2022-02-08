---
title: "Basic animation"
description: "The .NET MAUI ViewExtensions class, in the Microsoft.Maui.Controls namespace, provides extension methods that can be used to create and cancel basic animations."
ms.date: 02/08/2022
---

# Basic animation

.NET Multi-platform App UI (.NET MAUI) animation classes target different properties of visual elements, with a typical basic animation progressively changing a property from one value to another over a period of time.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Basic animations can be created with extension methods provided by the `ViewExtensions` class, in the `Microsoft.Maui.Controls` namespace, which operate on `VisualElement` objects:

- `CancelAnimations` cancels any animations.
- `FadeTo` animates the `Opacity` property of a `VisualElement`.
- `RelScaleTo` applies an animated incremental increase or decrease to the `Scale` property of a `VisualElement`.
- `RotateTo` animates the `Rotation` property of a `VisualElement`.
- `RelRotateTo` applies an animated incremental increase or decrease to the `Rotation` property of a `VisualElement`.
- `RotateXTo` animates the `RotationX` property of a `VisualElement`.
- `RotateYTo` animates the `RotationY` property of a `VisualElement`.
- `ScaleTo` animates the `Scale` property of a `VisualElement`.
- `ScaleXTo` animates the `ScaleX` property of a `VisualElement`.
- `ScaleYTo` animates the `ScaleY` property of a `VisualElement`.
- `TranslateTo` animates the `TranslationX` and `TranslationY` properties of a `VisualElement`.

By default, each animation will take 250 milliseconds. However, a duration for each animation can be specified when creating the animation.

> [!NOTE]
> The `ViewExtensions` class also provides a `LayoutTo` extension method. However, this method is intended to be used by layouts to animate transitions between layout states that contain size and position changes.

The animation extension methods in the `ViewExtensions` class are all asynchronous and return a `Task<bool>` object. The return value is `false` if the animation completes, and `true` if the animation is cancelled. Therefore, when animation operations are combined with the `await` operator it becomes possible to create sequential animations with subsequent animation methods executing after the previous method has completed. For more information, see [Compound animations](#compound-animations).

If there's a requirement to let an animation complete in the background, then the `await` operator can be omitted. In this scenario, the animation extension methods will quickly return after initiating the animation, with the animation occurring in the background. This operation can be taken advantage of when creating composite animations. For more information, see [Composite animations](#composite-animations).

## Single animations

Each extension method in the `ViewExtensions` class implements a single animation operation that progressively changes a property from one value to another value over a period of time.

### Rotation

Rotation is performed with the `RotateTo` method, which progressively changes the `Rotation` property of an element:

```csharp
await image.RotateTo(360, 2000);
image.Rotation = 0;
```

In this example, an `Image` instance is rotated up to 360 degrees over 2 seconds (2000 milliseconds). The `RotateTo` method obtains the current `Rotation` property value of the element for the start of the animation, and then rotates from that value to its first argument (360). Once the animation is complete, the image's `Rotation` property is reset to 0. This ensures that the `Rotation` property doesn't remain at 360 after the animation concludes, which would prevent additional rotations.

The following screenshot shows the rotation in progress:

![Rotation Animation.](simple-images/rotateto.png)

> [!NOTE]
> In addition to the `RotateTo` method, there are also `RotateXTo` and `RotateYTo` methods that animate the `RotationX` and `RotationY` properties, respectively.

### Relative rotation

Relative rotation is performed with the `RelRotateTo` method, which progressively changes the `Rotation` property of an element:

```csharp
await image.RelRotateTo(360, 2000);
```

In this example, an `Image` instance is rotated 360 degrees from its starting position over 2 seconds (2000 milliseconds). The `RelRotateTo` method obtains the current `Rotation` property value of the element for the start of the animation, and then rotates from that value to the value plus its first argument (360). This ensures that each animation will always be a 360 degrees rotation from the starting position. Therefore, if a new animation is invoked while an animation is already in progress, it will start from the current position and may end at a position that is not an increment of 360 degrees.

### Scaling

Scaling is performed with the `ScaleTo` method, which progressively changes the `Scale` property of an element:

```csharp
await image.ScaleTo(2, 2000);
```

In this example, an `Image` instance is scaled up to twice its size over 2 seconds (2000 milliseconds). The `ScaleTo` method obtains the current `Scale` property value of the element for the start of the animation, and then scales from that value to its first argument. This has the effect of expanding the size of the image to twice its size.

> [!NOTE]
> In addition to the `ScaleTo` method, there are also `ScaleXTo` and `ScaleYTo` methods that animate the `ScaleX` and `ScaleY` properties, respectively.

### Relative scaling

Relative scaling is performed with the `RelScaleTo` method, which progressively changes the `Scale` property of an element:

```csharp
await image.RelScaleTo(2, 2000);
```

In this example, an `Image` instance is scaled up to twice its size over 2 seconds (2000 milliseconds). The `RelScaleTo` method obtains the current `Scale` property value of the element for the start of the animation, and then scales from that value to the value plus its first argument. This ensures that each animation will always be a scaling of 2 from the starting position.

### Scaling and rotation with anchors

The `AnchorX` and `AnchorY` properties of a visual element set the center of scaling or rotation for the `Rotation` and `Scale` properties. Therefore, their values also affect the `RotateTo` and `ScaleTo` methods.

Given an `Image` that has been placed at the center of a layout, the following code example demonstrates rotating the image around the center of the layout by setting its `AnchorY` property:

```csharp
double radius = Math.Min(absoluteLayout.Width, absoluteLayout.Height) / 2;
image.AnchorY = radius / image.Height;
await image.RotateTo(360, 2000);
```

To rotate the `Image` instance around the center of the layout, the `AnchorX` and `AnchorY` properties must be set to values that are relative to the width and height of the `Image`. In this example, the center of the `Image` is defined to be at the center of the layout, and so the default `AnchorX` value of 0.5 does not require changing. However, the `AnchorY` property is redefined to be a value from the top of the `Image` to the center point of the layout. This ensures that the `Image` makes a full rotation of 360 degrees around the center point of the layout.

### Translation

Translation is performed with the `TranslateTo` method, which progressively changes the `TranslationX` and `TranslationY` properties of an element:

```csharp
await image.TranslateTo(-100, -100, 1000);
```

In this example, the `Image` instance is translated horizontally and vertically over 1 second (1000 milliseconds). The `TranslateTo` method simultaneously translates the image 100 device-independent units to the left, and 100 device-independent units upwards. This is because the first and second arguments are both negative numbers. Providing positive numbers would translate the image to the right, and down.

> [!IMPORTANT]
> If an element is initially laid out off screen and then translated onto the screen, after translation the element's input layout remains off screen and the user can't interact with it. Therefore, it's recommended that a view should be laid out in its final position, and then any required translations performed.

### Fading

Fading is performed with the `FadeTo` method, which progressively changes the `Opacity` property of an element:

```csharp
image.Opacity = 0;
await image.FadeTo(1, 4000);
```

In this example, the `Image` instance fades in over 4 seconds (4000 milliseconds). The `FadeTo` method obtains the current `Opacity` property value of the element for the start of the animation, and then fades in from that value to its first argument.

## Compound animations

A compound animation is a sequential combination of animations, and can be created with the `await` operator:

```csharp
await image.TranslateTo(-100, 0, 1000);    // Move image left
await image.TranslateTo(-100, -100, 1000); // Move image diagonally up and left
await image.TranslateTo(100, 100, 2000);   // Move image diagonally down and right
await image.TranslateTo(0, 100, 1000);     // Move image left
await image.TranslateTo(0, 0, 1000);       // Move image up
```

In this example, the `Image` instance is translated over 6 seconds (6000 milliseconds). The translation of the `Image` uses five animations, with the `await` operator indicating that each animation executes sequentially. Therefore, subsequent animation methods execute after the previous method has completed.

## Composite animations

A composite animation is a combination of animations where two or more animations run simultaneously. Composite animations can be created by combining awaited and non-awaited animations:

```csharp
image.RotateTo(360, 4000);
await image.ScaleTo(2, 2000);
await image.ScaleTo(1, 2000);
```

In this example, the `Image` instance is scaled and simultaneously rotated over 4 seconds (4000 milliseconds). The scaling of the `Image` uses two sequential animations that occur at the same time as the rotation. The `RotateTo` method executes without an `await` operator and returns immediately, with the first `ScaleTo` animation then beginning. The `await` operator on the first `ScaleTo` method delays the second `ScaleTo` method until the first `ScaleTo` method has completed. At this point the `RotateTo` animation is half completed and the `Image` will be rotated 180 degrees. During the final 2 seconds (2000 milliseconds), the second `ScaleTo` animation and the `RotateTo` animation both complete.

### Run multiple animations concurrently

The `Task.WhenAny` and `Task.WhenAll` methods can be used to run multiple asynchronous methods concurrently, and therefore can create composite animations. Both methods return a `Task` object and accept a collection of methods that each return a `Task` object. The `Task.WhenAny` method completes when any method in its collection completes execution, as demonstrated in the following code example:

```csharp
await Task.WhenAny<bool>
(
  image.RotateTo(360, 4000),
  image.ScaleTo(2, 2000)
);
await image.ScaleTo(1, 2000);
```

In this example, the `Task.WhenAny` method contains two tasks. The first task rotates an `Image` instance over 4 seconds (4000 milliseconds), and the second task scales the image over 2 seconds (2000 milliseconds). When the second task completes, the `Task.WhenAny` method call completes. However, even though the `RotateTo` method is still running, the second `ScaleTo` method can begin.

The `Task.WhenAll` method completes when all the methods in its collection have completed, as demonstrated in the following code example:

```csharp
// 10 minute animation
uint duration = 10 * 60 * 1000;
await Task.WhenAll
(
  image.RotateTo(307 * 360, duration),
  image.RotateXTo(251 * 360, duration),
  image.RotateYTo(199 * 360, duration)
);
```

In this example, the `Task.WhenAll` method contains three tasks, each of which executes over 10 minutes. Each `Task` makes a different number of 360 degree rotations – 307 rotations for `RotateTo`, 251 rotations for `RotateXTo`, and 199 rotations for `RotateYTo`. These values are prime numbers, therefore ensuring that the rotations aren't synchronized and hence won't result in repetitive patterns.

## Canceling animations

An app can cancel one or more animations with a call to the `CancelAnimations` extension method:

```csharp
image.CancelAnimations();
```

In this example, all animations that are running on the `Image` instance are immediately canceled.
