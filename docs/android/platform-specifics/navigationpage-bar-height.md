---
title: "NavigationPage bar height on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that sets the height of the navigation bar on a NavigationPage."
ms.date: 04/05/2022
---

# NavigationPage bar height on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific sets the height of the navigation bar on a `NavigationPage`. It's consumed in XAML by setting the `NavigationPage.BarHeight` bindable property to an integer value:

```xaml
<NavigationPage ...
                xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;assembly=Microsoft.Maui.Controls"
                android:NavigationPage.BarHeight="450">
    ...
</NavigationPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;
...

public class AndroidNavigationPageCS : Microsoft.Maui.Controls.NavigationPage
{
    public AndroidNavigationPageCS()
    {
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetBarHeight(450);
    }
}
```

The `NavigationPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on app compat Android. The `NavigationPage.SetBarHeight` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat` namespace, is used to set the height of the navigation bar on a `NavigationPage`. In addition, the `NavigationPage.GetBarHeight` method can be used to return the height of the navigation bar in the `NavigationPage`.

The result is that the height of the navigation bar on a `NavigationPage` can be set:

:::image type="content" source="media/navigationpage-bar-height/navigationpage-barheight.png" alt-text="NavigationPage navigation bar height.":::
