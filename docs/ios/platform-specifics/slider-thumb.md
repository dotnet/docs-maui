---
title: "Slider thumb tap on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables the Slider.Value property to be set by tapping on the Slider bar."
ms.date: 04/05/2022
---

# Slider thumb tap on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific enables the `Slider.Value` property to be set by tapping on a position on the <xref:Microsoft.Maui.Controls.Slider> bar, rather than by having to drag the <xref:Microsoft.Maui.Controls.Slider> thumb. It's consumed in XAML by setting the `Slider.UpdateOnTap` bindable property to `true`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <Slider ... ios:Slider.UpdateOnTap="true" />
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

var slider = new Microsoft.Maui.Controls.Slider();
slider.On<iOS>().SetUpdateOnTap(true);
```

The `Slider.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Slider.SetUpdateOnTap` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether a tap on the <xref:Microsoft.Maui.Controls.Slider> bar will set the `Slider.Value` property. In addition, the `Slider.GetUpdateOnTap` method can be used to return whether a tap on the <xref:Microsoft.Maui.Controls.Slider> bar will set the `Slider.Value` property.

The result is that a tap on the <xref:Microsoft.Maui.Controls.Slider> bar can move the <xref:Microsoft.Maui.Controls.Slider> thumb and set the `Slider.Value` property:

:::image type="content" source="media/slider-thumb/slider-updateontap.png" alt-text="Slider Update on Tap enabled.":::
