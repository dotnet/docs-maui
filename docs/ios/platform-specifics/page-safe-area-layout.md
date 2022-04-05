---
title: "Safe area layout guide on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that ensures that page content is positioned on an area of the screen that is safe for all devices that use iOS 11 and greater."
ms.date: 04/05/2022
---

# Safe area layout guide on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to ensure that page content is positioned on an area of the screen that is safe for all devices that use iOS 11 and greater. Specifically, it will help to make sure that content isn't clipped by rounded device corners, the home indicator, or the sensor housing. It's consumed in XAML by setting the `Page.UseSafeArea` attached property to a `boolean` value:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             Title="Safe Area"
             ios:Page.UseSafeArea="true">
    <StackLayout>
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

On<iOS>().SetUseSafeArea(true);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetUseSafeArea` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls whether the safe area layout guide is enabled.

The result is that page content can be positioned on an area of the screen that is safe for all iPhones:

:::image type="content" source="media/page-safe-area/safe-area-layout.png" alt-text="Safe area layout guide.":::

> [!NOTE]
> The safe area defined by Apple is used in .NET MAUI to set the `Page.Padding` property, and will override any previous values of this property that have been set.

The safe area can be customized by retrieving its `Thickness` value with the `Page.SafeAreaInsets` method from the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace. It can then be modified as required and re-assigned to the `Padding` property in the `OnAppearing` override:

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();

    var safeInsets = On<iOS>().SafeAreaInsets();
    safeInsets.Left = 20;
    Padding = safeInsets;
}
```
