---
title: "Home indicator visibility on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that sets the visibility of the home indicator on a Page."
ms.date: 04/05/2022
---

# Home indicator visibility on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific sets the visibility of the home indicator on a `Page`. It's consumed in XAML by setting the `Page.PrefersHomeIndicatorAutoHidden` bindable property to a `boolean`:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.PrefersHomeIndicatorAutoHidden="true">
    ...
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetPrefersHomeIndicatorAutoHidden` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls the visibility of the home indicator. In addition, the `Page.PrefersHomeIndicatorAutoHidden` method can be used to retrieve the visibility of the home indicator.

The result is that the visibility of the home indicator on a `Page` can be controlled:

:::image type="content" source="media/page-home-indicator/home-indicator-visibility.png" alt-text="Screenshot of home indicator visibility on an iOS page.":::

> [!NOTE]
> This platform-specific can be applied to `ContentPage`, `FlyoutPage`, `NavigationPage`, and `TabbedPage` objects.
