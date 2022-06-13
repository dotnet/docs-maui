---
title: "ProgressBar"
description: "The .NET MAUI ProgressBar indicates to users that the app is progressing through a lengthy activity."
ms.date: 02/08/2022
---

# ProgressBar

The .NET Multi-platform App UI (.NET MAUI) `ProgressBar` indicates to users that the app is progressing through a lengthy activity. The progress bar is a horizontal bar that is filled to a percentage represented by a `double` value.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The appearance of a `ProgressBar` is platform-dependent, and the following screenshot shows a `ProgressBar` on iOS and Android:

:::image type="content" source="media/progressbar/progressbars-default.png" alt-text="Screenshot of ProgressBar on iOS and Android.":::

`ProgressBar` defines two properties:

- `Progress` is a `double` value that represents the current progress as a value from 0 to 1. `Progress` values less than 0 will be clamped to 0, values greater than 1 will be clamped to 1. The default value of this property is 0.
- `ProgressColor` is a `Color` values that defines the color of the `ProgressBar`.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

`ProgressBar` also defines a `ProgressTo` method that animates the bar from its current value to a specified value. For more information, see [Animate a ProgressBar](#animate-a-progressbar).

## Create a ProgressBar

To indicate progress through a lengthy activity, create a `ProgressBar` object and set its properties to define its appearance.

The following XAML example shows how to display a `ProgressBar`:

```xaml
<ProgressBar Progress="0.5" />
```

The equivalent C# code is:

```csharp
ProgressBar progressBar = new ProgressBar { Progress = 0.5 };
```

> [!WARNING]
> Do not use unconstrained horizontal layout options such as `Center`, `Start`, or `End` with `ProgressBar`. Keep the default `HorizontalOptions` value of `Fill`. <!-- and don't use a width of `Auto` when putting a `ProgressBar` in a `Grid` layout.-->

The following XAML example shows how to change the color of a `ProgressBar`:

```xaml
<ProgressBar Progress="0.5"
             ProgressColor="Orange" />
```

The equivalent C# code is:

```csharp
ProgressBar progressBar = new ProgressBar
{
    Progress = 0.5,
    ProgressColor = Colors.Orange
};
```

## Animate a ProgressBar

The `ProgressTo` method animates the `ProgressBar` from its current `Progress` value to a provided value over time. The method accepts a `double` progress value, a `uint` duration in milliseconds, an `Easing` enum value and returns a `Task<bool>`. The following example demonstrates how to animate a `ProgressBar`:

```csharp
// animate to 75% progress over 500 milliseconds with linear easing
await progressBar.ProgressTo(0.75, 500, Easing.Linear);
```

For more information about the `Easing` enumeration, see [Easing functions](~/user-interface/animation/easing.md).
