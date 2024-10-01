---
title: "Easing functions"
description: ".NET MAUI includes an Easing class that enables you to specify a transfer function that controls how animations speed up or slow down as they're running."
ms.date: 09/30/2024
---

# Easing functions

.NET Multi-platform App UI (.NET MAUI) includes an <xref:Microsoft.Maui.Easing> class that enables you to specify a transfer function that controls how animations speed up or slow down as they're running.

The <xref:Microsoft.Maui.Easing> class defines a number of easing functions that can be consumed by animations:

- The <xref:Microsoft.Maui.Easing.BounceIn> easing function bounces the animation at the beginning.
- The <xref:Microsoft.Maui.Easing.BounceOut> easing function bounces the animation at the end.
- The <xref:Microsoft.Maui.Easing.CubicIn> easing function slowly accelerates the animation.
- The <xref:Microsoft.Maui.Easing.CubicInOut> easing function accelerates the animation at the beginning, and decelerates the animation at the end.
- The <xref:Microsoft.Maui.Easing.CubicOut> easing function quickly decelerates the animation.
- The <xref:Microsoft.Maui.Easing.Linear> easing function uses a constant velocity, and is the default easing function.
- The <xref:Microsoft.Maui.Easing.SinIn> easing function smoothly accelerates the animation.
- The <xref:Microsoft.Maui.Easing.SinInOut> easing function smoothly accelerates the animation at the beginning, and smoothly decelerates the animation at the end.
- The <xref:Microsoft.Maui.Easing.SinOut> easing function smoothly decelerates the animation.
- The <xref:Microsoft.Maui.Easing.SpringIn> easing function causes the animation to very quickly accelerate towards the end.
- The <xref:Microsoft.Maui.Easing.SpringOut> easing function causes the animation to quickly decelerate towards the end.

The `In` and `Out` suffixes indicate if the effect provided by the easing function is noticeable at the beginning of the animation, at the end, or both.

In addition, custom easing functions can be created. For more information, see [Custom easing functions](#custom-easing-functions).

## Consume an easing function

The animation extension methods in the <xref:Microsoft.Maui.Controls.ViewExtensions> class allow an easing function to be specified as the final method argument:

```csharp
await image.TranslateTo(0, 200, 2000, Easing.BounceIn);
await image.ScaleTo(2, 2000, Easing.CubicIn);
await image.RotateTo(360, 2000, Easing.SinInOut);
await image.ScaleTo(1, 2000, Easing.CubicOut);
await image.TranslateTo(0, -200, 2000, Easing.BounceOut);
```

By specifying an easing function for an animation, the animation velocity becomes non-linear and produces the effect provided by the easing function. Omitting an easing function when creating an animation causes the animation to use the default <xref:Microsoft.Maui.Easing.Linear> easing function, which produces a linear velocity.

For more information about using the animation extension methods in the <xref:Microsoft.Maui.Controls.ViewExtensions> class, see [Basic animation](basic.md). Easing functions can also be consumed by the <xref:Microsoft.Maui.Controls.Animation> class. For more information, see [Custom animation](custom.md).

## Custom easing functions

There are three main approaches to creating a custom easing function:

1. Create a method that takes a `double` argument, and returns a `double` result.
1. Create a `Func<double, double>`.
1. Specify the easing function as the argument to the <xref:Microsoft.Maui.Easing> constructor.

In all three cases, the custom easing function should return a value between 0 and 1.

### Custom easing method

A custom easing function can be defined as a method that takes a `double` argument, and returns a `double` result:

```csharp
double CustomEase (double t)
{
  return t == 0 || t == 1 ? t : (int)(5 * t) / 5.0;
}

await image.TranslateTo(0, 200, 2000, (Easing)CustomEase);
```

In this example, the `CustomEase` method truncates the incoming value to the values 0, 0.2, 0.4, 0.6, 0.8, and 1. Therefore, the <xref:Microsoft.Maui.Controls.Image> instance is translated in discrete jumps, rather than smoothly.

### Custom easing func

A custom easing function can also be defined as a `Func<double, double>`:

```csharp
Func<double, double> CustomEaseFunc = t => 9 * t * t * t - 13.5 * t * t + 5.5 * t;
await image.TranslateTo(0, 200, 2000, CustomEaseFunc);
```

In this example, the `CustomEaseFunc` represents an easing function that starts off fast, slows down and reverses course, and then reverses course again to accelerate quickly towards the end. Therefore, while the overall movement of the <xref:Microsoft.Maui.Controls.Image> instance is downwards, it also temporarily reverses course halfway through the animation.

### Custom easing constructor

A custom easing function can also be defined as the argument to the <xref:Microsoft.Maui.Easing> constructor:

```csharp
await image.TranslateTo(0, 200, 2000, new Easing (t => 1 - Math.Cos (10 * Math.PI * t) * Math.Exp (-5 * t)));
```

In this example, the custom easing function is specified as a lambda function argument to the <xref:Microsoft.Maui.Easing> constructor, and uses the `Math.Cos` method to create a slow drop effect that's dampened by the `Math.Exp` method. Therefore, the <xref:Microsoft.Maui.Controls.Image> instance is translated so that it appears to drop to its final position.
