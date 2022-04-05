---
title: "VisualElement drop shadows on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables a drop shadow on a VisualElement."
ms.date: 04/05/2022
---

# VisualElement drop shadows on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to enable a drop shadow on a `VisualElement`. It's consumed in XAML by setting the `VisualElement.IsShadowEnabled` attached property to `true`, along with a number of additional optional attached properties that control the drop shadow:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout Margin="20">
        <BoxView ...
                 ios:VisualElement.IsShadowEnabled="true"
                 ios:VisualElement.ShadowColor="Purple"
                 ios:VisualElement.ShadowOpacity="0.7"
                 ios:VisualElement.ShadowRadius="12">
            <ios:VisualElement.ShadowOffset>
                <Size>
                    <x:Arguments>
                        <x:Double>10</x:Double>
                        <x:Double>10</x:Double>
                    </x:Arguments>
                </Size>
            </ios:VisualElement.ShadowOffset>
         </BoxView>
        ...
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

var boxView = new BoxView { Color = Colors.Aqua, WidthRequest = 100, HeightRequest = 100 };
boxView.On<iOS>()
       .SetIsShadowEnabled(true)
       .SetShadowColor(Colors.Purple)
       .SetShadowOffset(new Size(10,10))
       .SetShadowOpacity(0.7)
       .SetShadowRadius(12);
```

The `VisualElement.On<iOS>` method specifies that this platform-specific will only run on iOS. The `VisualElement.SetIsShadowEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether a drop shadow is enabled on the `VisualElement`. In addition, the following methods can be invoked to control the drop shadow:

- `SetShadowColor` – sets the color of the drop shadow.
- `SetShadowOffset` – sets the offset of the drop shadow. The offset changes the direction the shadow is cast, and is specified as a `Size` value. The `Size` structure values are expressed in device-independent units, with the first value being the distance to the left (negative value) or right (positive value), and the second value being the distance above (negative value) or below (positive value). The default value of this property is (0.0, 0.0), which results in the shadow being cast around every side of the `VisualElement`.
- `SetShadowOpacity` – sets the opacity of the drop shadow, with the value being in the range 0.0 (transparent) to 1.0 (opaque). The default opacity value is 0.5.
- `SetShadowRadius` – sets the blur radius used to render the drop shadow. The default radius value is 10.0.

> [!NOTE]
> The state of a drop shadow can be queried by calling the `GetIsShadowEnabled`, `GetShadowColor`, `GetShadowOffset`, `GetShadowOpacity`, and `GetShadowRadius` methods.

The result is that a drop shadow can be enabled on a `VisualElement`:

:::image type="content" source="media/drop-shadow/drop-shadow.png" alt-text="Drop shadow enabled.":::
