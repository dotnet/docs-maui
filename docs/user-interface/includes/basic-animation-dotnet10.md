---
ms.topic: include
ms.date: 04/03/2025
---

The .NET Multi-platform App UI (.NET MAUI) animation classes target different properties of visual elements, with a typical basic animation progressively changing a property from one value to another over a period of time.

Basic animations can be created with extension methods provided by the <xref:Microsoft.Maui.Controls.ViewExtensions> class, which operate on <xref:Microsoft.Maui.Controls.VisualElement> objects:

- <xref:Microsoft.Maui.Controls.ViewExtensions.CancelAnimations%2A> cancels any animations.
- <xref:Microsoft.Maui.Controls.ViewExtensions.FadeTo%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.Opacity> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.RelScaleToAsync%2A> applies an animated incremental increase or decrease to the <xref:Microsoft.Maui.Controls.VisualElement.Scale> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.RelRotateToAsync%2A> applies an animated incremental increase or decrease to the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.RotateXToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.RotationX> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.RotateYToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.RotationY> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.Scale> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleXToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.ScaleX> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleYToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.ScaleY> property of a <xref:Microsoft.Maui.Controls.VisualElement>.
- <xref:Microsoft.Maui.Controls.ViewExtensions.TranslateToAsync%2A> animates the <xref:Microsoft.Maui.Controls.VisualElement.TranslationX> and <xref:Microsoft.Maui.Controls.VisualElement.TranslationY> properties of a <xref:Microsoft.Maui.Controls.VisualElement>.

By default, each animation will take 250 milliseconds. However, a duration for each animation can be specified when creating the animation.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.ViewExtensions> class also provides a <xref:Microsoft.Maui.Controls.ViewExtensions.LayoutToAsync%2A> extension method. However, this method is intended to be used by layouts to animate transitions between layout states that contain size and position changes.

The animation extension methods in the <xref:Microsoft.Maui.Controls.ViewExtensions> class are all asynchronous and return a `Task<bool>` object. The return value is `false` if the animation completes, and `true` if the animation is cancelled. Therefore, when animation operations are combined with the `await` operator it becomes possible to create sequential animations with subsequent animation methods executing after the previous method has completed. For more information, see [Compound animations](#compound-animations).

If there's a requirement to let an animation complete in the background, then the `await` operator can be omitted. In this scenario, the animation extension methods will quickly return after initiating the animation, with the animation occurring in the background. This operation can be taken advantage of when creating composite animations. For more information, see [Composite animations](#composite-animations).

[!INCLUDE [Android animation system settings](../includes/animation-android.md)]

## Single animations

Each extension method in the <xref:Microsoft.Maui.Controls.ViewExtensions> class implements a single animation operation that progressively changes a property from one value to another value over a period of time.

### Rotation

Rotation is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> method, which progressively changes the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property of an element:

```csharp
await image.RotateToAsync(360, 2000);
image.Rotation = 0;
```

In this example, an <xref:Microsoft.Maui.Controls.Image> instance is rotated up to 360 degrees over 2 seconds (2000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> method obtains the current <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property value of the element for the start of the animation, and then rotates from that value to its first argument (360). Once the animation is complete, the image's <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property is reset to 0. This ensures that the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property doesn't remain at 360 after the animation concludes, which would prevent additional rotations.

> [!NOTE]
> In addition to the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> method, there are also <xref:Microsoft.Maui.Controls.ViewExtensions.RotateXToAsync%2A> and <xref:Microsoft.Maui.Controls.ViewExtensions.RotateYToAsync%2A> methods that animate the `RotationX` and `RotationY` properties, respectively.

### Relative rotation

Relative rotation is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.RelRotateToAsync%2A> method, which progressively changes the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property of an element:

```csharp
await image.RelRotateToAsync(360, 2000);
```

In this example, an <xref:Microsoft.Maui.Controls.Image> instance is rotated 360 degrees from its starting position over 2 seconds (2000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.RelRotateToAsync%2A> method obtains the current <xref:Microsoft.Maui.Controls.VisualElement.Rotation> property value of the element for the start of the animation, and then rotates from that value to the value plus its first argument (360). This ensures that each animation will always be a 360 degrees rotation from the starting position. Therefore, if a new animation is invoked while an animation is already in progress, it will start from the current position and may end at a position that is not an increment of 360 degrees.

### Scaling

Scaling is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method, which progressively changes the `Scale` property of an element:

```csharp
await image.ScaleToAsync(2, 2000);
```

In this example, an <xref:Microsoft.Maui.Controls.Image> instance is scaled up to twice its size over 2 seconds (2000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method obtains the current <xref:Microsoft.Maui.Controls.VisualElement.Scale> property value of the element for the start of the animation, and then scales from that value to its first argument. This has the effect of expanding the size of the image to twice its size.

> [!NOTE]
> In addition to the <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method, there are also <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleXToAsync%2A> and <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleYToAsync%2A> methods that animate the `ScaleX` and `ScaleY` properties, respectively.

### Relative scaling

Relative scaling is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.RelScaleToAsync%2A> method, which progressively changes the <xref:Microsoft.Maui.Controls.VisualElement.Scale> property of an element:

```csharp
await image.RelScaleToAsync(2, 2000);
```

In this example, an <xref:Microsoft.Maui.Controls.Image> instance is scaled up to twice its size over 2 seconds (2000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.RelScaleTo%2A> method obtains the current <xref:Microsoft.Maui.Controls.VisualElement.Scale> property value of the element for the start of the animation, and then scales from that value to the value plus its first argument. This ensures that each animation will always be a scaling of 2 from the starting position.

### Scaling and rotation with anchors

The `AnchorX` and `AnchorY` properties of a visual element set the center of scaling or rotation for the <xref:Microsoft.Maui.Controls.VisualElement.Rotation> and <xref:Microsoft.Maui.Controls.VisualElement.Scale> properties. Therefore, their values also affect the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> and <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> methods.

Given an <xref:Microsoft.Maui.Controls.Image> that has been placed at the center of a layout, the following code example demonstrates rotating the image around the center of the layout by setting its `AnchorY` property:

```csharp
double radius = Math.Min(absoluteLayout.Width, absoluteLayout.Height) / 2;
image.AnchorY = radius / image.Height;
await image.RotateToAsync(360, 2000);
```

To rotate the <xref:Microsoft.Maui.Controls.Image> instance around the center of the layout, the <xref:Microsoft.Maui.Controls.VisualElement.AnchorX> and <xref:Microsoft.Maui.Controls.VisualElement.AnchorY> properties must be set to values that are relative to the width and height of the <xref:Microsoft.Maui.Controls.Image>. In this example, the center of the <xref:Microsoft.Maui.Controls.Image> is defined to be at the center of the layout, and so the default <xref:Microsoft.Maui.Controls.VisualElement.AnchorX> value of 0.5 does not require changing. However, the <xref:Microsoft.Maui.Controls.VisualElement.AnchorY> property is redefined to be a value from the top of the <xref:Microsoft.Maui.Controls.Image> to the center point of the layout. This ensures that the <xref:Microsoft.Maui.Controls.Image> makes a full rotation of 360 degrees around the center point of the layout.

### Translation

Translation is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.TranslateToAsync%2A> method, which progressively changes the <xref:Microsoft.Maui.Controls.VisualElement.TranslationX> and <xref:Microsoft.Maui.Controls.VisualElement.TranslationY> properties of an element:

```csharp
await image.TranslateToAsync(-100, -100, 1000);
```

In this example, the <xref:Microsoft.Maui.Controls.Image> instance is translated horizontally and vertically over 1 second (1000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.TranslateToAsync%2A> method simultaneously translates the image 100 device-independent units to the left, and 100 device-independent units upwards. This is because the first and second arguments are both negative numbers. Providing positive numbers would translate the image to the right, and down. For more information about device-independent units, see [Device-independent units](../device-independent-units.md).

> [!IMPORTANT]
> If an element is initially laid out off screen and then translated onto the screen, after translation the element's input layout remains off screen and the user can't interact with it. Therefore, it's recommended that a view should be laid out in its final position, and then any required translations performed.

### Fading

Fading is performed with the <xref:Microsoft.Maui.Controls.ViewExtensions.FadeToAsync%2A> method, which progressively changes the <xref:Microsoft.Maui.Controls.VisualElement.Opacity> property of an element:

```csharp
image.Opacity = 0;
await image.FadeToAsync(1, 4000);
```

In this example, the <xref:Microsoft.Maui.Controls.Image> instance fades in over 4 seconds (4000 milliseconds). The <xref:Microsoft.Maui.Controls.ViewExtensions.FadeToAsync%2A> method obtains the current <xref:Microsoft.Maui.Controls.VisualElement.Opacity> property value of the element for the start of the animation, and then fades in from that value to its first argument.

## Compound animations

A compound animation is a sequential combination of animations, and can be created with the `await` operator:

```csharp
await image.TranslateToAsync(-100, 0, 1000);    // Move image left
await image.TranslateToAsync(-100, -100, 1000); // Move image diagonally up and left
await image.TranslateToAsync(100, 100, 2000);   // Move image diagonally down and right
await image.TranslateToAsync(0, 100, 1000);     // Move image left
await image.TranslateToAsync(0, 0, 1000);       // Move image up
```

In this example, the <xref:Microsoft.Maui.Controls.Image> instance is translated over 6 seconds (6000 milliseconds). The translation of the <xref:Microsoft.Maui.Controls.Image> uses five animations, with the `await` operator indicating that each animation executes sequentially. Therefore, subsequent animation methods execute after the previous method has completed.

## Composite animations

A composite animation is a combination of animations where two or more animations run simultaneously. Composite animations can be created by combining awaited and non-awaited animations:

```csharp
image.RotateToAsync(360, 4000);
await image.ScaleToAsync(2, 2000);
await image.ScaleToAsync(1, 2000);
```

In this example, the <xref:Microsoft.Maui.Controls.Image> instance is scaled and simultaneously rotated over 4 seconds (4000 milliseconds). The scaling of the <xref:Microsoft.Maui.Controls.Image> uses two sequential animations that occur at the same time as the rotation. The <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> method executes without an `await` operator and returns immediately, with the first <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> animation then beginning. The `await` operator on the first <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method delays the second <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method until the first <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method has completed. At this point the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> animation is half completed and the <xref:Microsoft.Maui.Controls.Image> will be rotated 180 degrees. During the final 2 seconds (2000 milliseconds), the second <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> animation and the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> animation both complete.

### Run multiple animations concurrently

The `Task.WhenAny` and `Task.WhenAll` methods can be used to run multiple asynchronous methods concurrently, and therefore can create composite animations. Both methods return a `Task` object and accept a collection of methods that each return a `Task` object. The `Task.WhenAny` method completes when any method in its collection completes execution, as demonstrated in the following code example:

```csharp
await Task.WhenAny<bool>
(
  image.RotateToAsync(360, 4000),
  image.ScaleToAsync(2, 2000)
);
await image.ScaleToAsync(1, 2000);
```

In this example, the `Task.WhenAny` method contains two tasks. The first task rotates an <xref:Microsoft.Maui.Controls.Image> instance over 4 seconds (4000 milliseconds), and the second task scales the image over 2 seconds (2000 milliseconds). When the second task completes, the `Task.WhenAny` method call completes. However, even though the <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A> method is still running, the second <xref:Microsoft.Maui.Controls.ViewExtensions.ScaleToAsync%2A> method can begin.

The `Task.WhenAll` method completes when all the methods in its collection have completed, as demonstrated in the following code example:

```csharp
// 10 minute animation
uint duration = 10 * 60 * 1000;
await Task.WhenAll
(
  image.RotateToAsync(307 * 360, duration),
  image.RotateXToAsync(251 * 360, duration),
  image.RotateYToAsync(199 * 360, duration)
);
```

In this example, the `Task.WhenAll` method contains three tasks, each of which executes over 10 minutes. Each `Task` makes a different number of 360 degree rotations – 307 rotations for <xref:Microsoft.Maui.Controls.ViewExtensions.RotateToAsync%2A>, 251 rotations for <xref:Microsoft.Maui.Controls.ViewExtensions.RotateXToAsync%2A>, and 199 rotations for <xref:Microsoft.Maui.Controls.ViewExtensions.RotateYToAsync%2A>. These values are prime numbers, therefore ensuring that the rotations aren't synchronized and hence won't result in repetitive patterns.
