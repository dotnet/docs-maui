---
title: "ProgressBar"
description: "The .NET MAUI ProgressBar indicates to users that the app is progressing through a lengthy activity."
ms.date: 08/30/2024
---

# ProgressBar

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ProgressBar> indicates to users that the app is progressing through a lengthy activity. The progress bar is a horizontal bar that is filled to a percentage represented by a `double` value.

The appearance of a <xref:Microsoft.Maui.Controls.ProgressBar> is platform-dependent, and the following screenshot shows a <xref:Microsoft.Maui.Controls.ProgressBar> on Android:

:::image type="content" source="media/progressbar/progressbar-default.png" alt-text="Screenshot of ProgressBar on Android.":::

<xref:Microsoft.Maui.Controls.ProgressBar> defines two properties:

- `Progress` is a `double` value that represents the current progress as a value from 0 to 1. `Progress` values less than 0 will be clamped to 0, values greater than 1 will be clamped to 1. The default value of this property is 0.
- `ProgressColor` is a <xref:Microsoft.Maui.Graphics.Color> values that defines the color of the <xref:Microsoft.Maui.Controls.ProgressBar>.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

<xref:Microsoft.Maui.Controls.ProgressBar> also defines a `ProgressTo` method that animates the bar from its current value to a specified value. For more information, see [Animate a ProgressBar](#animate-a-progressbar).

## Create a ProgressBar

To indicate progress through a lengthy activity, create a <xref:Microsoft.Maui.Controls.ProgressBar> object and set its properties to define its appearance.

The following XAML example shows how to display a <xref:Microsoft.Maui.Controls.ProgressBar>:

```xaml
<ProgressBar Progress="0.5" />
```

The equivalent C# code is:

```csharp
ProgressBar progressBar = new ProgressBar { Progress = 0.5 };
```

> [!WARNING]
> Do not use unconstrained horizontal layout options such as `Center`, `Start`, or `End` with <xref:Microsoft.Maui.Controls.ProgressBar>. Keep the default `HorizontalOptions` value of `Fill`. <!-- and don't use a width of `Auto` when putting a <xref:Microsoft.Maui.Controls.ProgressBar> in a <xref:Microsoft.Maui.Controls.Grid> layout.-->

The following XAML example shows how to change the color of a <xref:Microsoft.Maui.Controls.ProgressBar>:

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

The `ProgressTo` method animates the <xref:Microsoft.Maui.Controls.ProgressBar> from its current `Progress` value to a provided value over time. The method accepts a `double` progress value, a `uint` duration in milliseconds, an <xref:Microsoft.Maui.Easing> enum value and returns a `Task<bool>`. The following example demonstrates how to animate a <xref:Microsoft.Maui.Controls.ProgressBar>:

```csharp
// animate to 75% progress over 500 milliseconds with linear easing
await progressBar.ProgressTo(0.75, 500, Easing.Linear);
```

For more information about the <xref:Microsoft.Maui.Easing> enumeration, see [Easing functions](~/user-interface/animation/easing.md).
