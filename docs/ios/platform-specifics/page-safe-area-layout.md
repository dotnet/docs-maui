---
title: "Enable the safe area layout guide on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that enables the safe area layout guide."
ms.date: 02/16/2023
---

# Enable the safe area layout guide on iOS

By default, .NET Multi-platform App UI (.NET MAUI) apps automatically position page content on an area of the screen that is safe for all devices. This is known as the safe area layout guide, and ensures that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on some iPhone models.

This iOS platform-specific enables the safe area layout guide, and is consumed in XAML by setting the `Page.UseSafeArea` attached property to `true`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.UseSafeArea="True">
    <StackLayout>
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

On<iOS>().SetUseSafeArea(true);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetUseSafeArea` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls whether the safe area layout guide is disabled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.Layout> class defines a <xref:Microsoft.Maui.Controls.Layout.IgnoreSafeArea> property that ensures that content is positioned on an area of the screen that is safe for all iOS devices. This property can be set to `true` on any layout class, such as a <xref:Microsoft.Maui.Controls.Grid> or <xref:Microsoft.Maui.Controls.StackLayout>, to perform the equivalent of this platform-specific.
