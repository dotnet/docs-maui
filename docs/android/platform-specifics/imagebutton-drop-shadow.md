---
title: "ImageButton drop shadows on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that enables a drop shadow on a ImageButton."
ms.date: 04/05/2022
---

# ImageButton drop shadows on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific is used to enable a drop shadow on a `ImageButton`. It's consumed in XAML by setting the `ImageButton.IsShadowEnabled` bindable property to `true`, along with a number of additional optional bindable properties that control the drop shadow:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout Margin="20">
       <ImageButton ...
                    Source="logo.png"
                    BackgroundColor="GhostWhite"
                    android:ImageButton.IsShadowEnabled="true"
                    android:ImageButton.ShadowColor="Gray"
                    android:ImageButton.ShadowRadius="12">
            <android:ImageButton.ShadowOffset>
                <Size>
                    <x:Arguments>
                        <x:Double>10</x:Double>
                        <x:Double>10</x:Double>
                    </x:Arguments>
                </Size>
            </android:ImageButton.ShadowOffset>
        </ImageButton>
        ...
    </StackLayout>
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

var imageButton = new Xamarin.Forms.ImageButton { Source = "logo.png", BackgroundColor = Colors.GhostWhite, ... };
imageButton.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
           .SetIsShadowEnabled(true)
           .SetShadowColor(Colors.Gray)
           .SetShadowOffset(new Size(10, 10))
           .SetShadowRadius(12);
```

> [!IMPORTANT]
> A drop shadow is drawn as part of the `ImageButton` background, and the background is only drawn if the `BackgroundColor` property is set. Therefore, a drop shadow will not be drawn if the `ImageButton.BackgroundColor` property isn't set.

The `ImageButton.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `ImageButton.SetIsShadowEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to control whether a drop shadow is enabled on the `ImageButton`. In addition, the following methods can be invoked to control the drop shadow:

- `SetShadowColor` – sets the color of the drop shadow.
- `SetShadowOffset` – sets the offset of the drop shadow. The offset changes the direction the shadow is cast, and is specified as a `Size` value. The `Size` structure values are expressed in device-independent units, with the first value being the distance to the left (negative value) or right (positive value), and the second value being the distance above (negative value) or below (positive value). The default value of this property is (0.0, 0.0), which results in the shadow being cast around every side of the `ImageButton`.
- `SetShadowRadius`– sets the blur radius used to render the drop shadow. The default radius value is 10.0.

> [!NOTE]
> The state of a drop shadow can be queried by calling the `GetIsShadowEnabled`, `GetShadowColor`, `GetShadowOffset`, and `GetShadowRadius` methods.

The result is that a drop shadow can be enabled on a `ImageButton`:

:::image type="content" source="media/imagebutton-drop-shadow/imagebutton-drop-shadow.png" alt-text="ImageButton with drop shadow.":::
