---
title: "Home indicator visibility on iOS"
description: "This article explains how to consume the iOS platform-specific that sets the visibility of the home indicator on a page."
ms.date: 10/10/2024
---

# Home indicator visibility on iOS

This iOS platform-specific sets the visibility of the home indicator on a <xref:Microsoft.Maui.Controls.Page>. It's consumed in XAML by setting the `Page.PrefersHomeIndicatorAutoHidden` bindable property to a `boolean`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.PrefersHomeIndicatorAutoHidden="true">
    ...
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The
<xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetPrefersHomeIndicatorAutoHidden%2A?displayProperty=nameWithType> method, in the <xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific> namespace, controls the visibility of the home indicator. In addition, the <xref:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.PrefersHomeIndicatorAutoHidden%2A?displayProperty=nameWithType> method can be used to retrieve the visibility of the home indicator.

The result is that the visibility of the home indicator on a <xref:Microsoft.Maui.Controls.Page> can be controlled:

:::image type="content" source="media/page-home-indicator/home-indicator-visibility.png" alt-text="Home indicator visibility on an iOS page.":::

> [!NOTE]
> This platform-specific can be applied to <xref:Microsoft.Maui.Controls.ContentPage>, <xref:Microsoft.Maui.Controls.FlyoutPage>, <xref:Microsoft.Maui.Controls.NavigationPage>, <xref:Microsoft.Maui.Controls.Shell>, and <xref:Microsoft.Maui.Controls.TabbedPage> objects.
