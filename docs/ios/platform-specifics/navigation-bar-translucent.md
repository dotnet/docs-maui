---
title: "NavigationPage bar translucency on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that changes the transparency of the navigation bar in a NavigationPage."
ms.date: 04/05/2022
---

# NavigationPage bar translucency on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to change the transparency of the navigation bar on a <xref:Microsoft.Maui.Controls.NavigationPage>, and is consumed in XAML by setting the `NavigationPage.IsNavigationBarTranslucent` attached property to a `boolean` value:

```xaml
<NavigationPage ...
                xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
                BackgroundColor="Blue"
                ios:NavigationPage.IsNavigationBarTranslucent="true">
  ...
</NavigationPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

// Assume the app has a single window
(App.Current.Windows[0].Page as Microsoft.Maui.Controls.NavigationPage).BackgroundColor = Colors.Blue;
(App.Current.Windows[0].Page as Microsoft.maui.Controls.NavigationPage).On<iOS>().EnableTranslucentNavigationBar();
```

The `NavigationPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `NavigationPage.EnableTranslucentNavigationBar` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to make the navigation bar translucent. In addition, the <xref:Microsoft.Maui.Controls.NavigationPage> class in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace also has a `DisableTranslucentNavigationBar` method that restores the navigation bar to its default state, and a `SetIsNavigationBarTranslucent` method which can be used to toggle the navigation bar transparency by calling the `IsNavigationBarTranslucent` method:

```csharp
// Assume the app has a single window
(App.Current.Windows[0].Page as Microsoft.Maui.Controls.NavigationPage)
  .On<iOS>()
  .SetIsNavigationBarTranslucent(!(App.Current.Windows[0].Page as Microsoft.Maui.Controls.NavigationPage).On<iOS>().IsNavigationBarTranslucent());
```

The result is that the transparency of the navigation bar can be changed:

:::image type="content" source="media/navigation-bar-translucent/translucent-navigation-bar.png" alt-text="Translucent Navigation Bar Platform-Specific.":::
