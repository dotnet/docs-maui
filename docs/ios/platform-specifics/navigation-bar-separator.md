---
title: "NavigationPage bar separator on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that hides the separator line and shadow that is at the bottom of the navigation bar on a NavigationPage."
ms.date: 04/05/2022
---

# NavigationPage bar separator on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific hides the separator line and shadow that is at the bottom of the navigation bar on a `NavigationPage`. It's consumed in XAML by setting the `NavigationPage.HideNavigationBarSeparator` bindable property to `false`:

```xaml
<NavigationPage ...
                xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
                ios:NavigationPage.HideNavigationBarSeparator="true">

</NavigationPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

public class iOSTitleViewNavigationPageCode : Microsoft.Maui.Controls.NavigationPage
{
    public iOSTitleViewNavigationPageCode()
    {
        On<iOS>().SetHideNavigationBarSeparator(true);
    }
}
```

The `NavigationPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `NavigationPage.SetHideNavigationBarSeparator` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether the navigation bar separator is hidden. In addition, the `NavigationPage.HideNavigationBarSeparator` method can be used to return whether the navigation bar separator is hidden.

The result is that the navigation bar separator on a `NavigationPage` can be hidden:

:::image type="content" source="media/navigation-bar-separator/navigationpage-hideseparatorbar.png" alt-text="NavigationPage navigation bar hidden.":::
