---
title: "VisualElement blur on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that applies blur to a VisualElement."
ms.date: 04/05/2022
---

# VisualElement blur on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to blur the content layered beneath it, and can be applied to any `VisualElement`. It's consumed in XAML by setting the `VisualElement.BlurEffect` attached property to a value of the `BlurEffectStyle` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
  ...
  <Image Source="monkeyface.png"
         ios:VisualElement.BlurEffect="ExtraLight" />
  ...
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

image.On<iOS>().UseBlurEffect(BlurEffectStyle.ExtraLight);
```

The `Image.On<iOS>` method specifies that this platform-specific will only run on iOS. The `VisualElement.UseBlurEffect` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to apply the blur effect, with the `BlurEffectStyle` enumeration providing four values:

- `None`
- `ExtraLight`
- `Light`
- `Dark`

The result is that a specified `BlurEffectStyle` is applied to the `Image`:

:::image type="content" source="media/applying-blur/blur-effect.png" alt-text="Blur Effect Platform-Specific.":::

> [!NOTE]
> When adding a blur effect to a `VisualElement`, touch events will still be received by the `VisualElement`.
