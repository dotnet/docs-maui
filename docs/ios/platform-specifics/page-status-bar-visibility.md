---
title: "Page status bar visibility on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that sets the visibility of the status bar on a Page."
ms.date: 04/05/2022
---

# Page status bar sisibility on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to set the visibility of the status bar on a `Page`, and it includes the ability to control how the status bar enters or leaves the `Page`. It's consumed in XAML by setting the `Page.PrefersStatusBarHidden` attached property to a value of the `StatusBarHiddenMode` enumeration, and optionally the `Page.PreferredStatusBarUpdateAnimation` attached property to a value of the `UIStatusBarAnimation` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.PrefersStatusBarHidden="True"
             ios:Page.PreferredStatusBarUpdateAnimation="Fade">
  ...
</ContentPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

On<iOS>().SetPrefersStatusBarHidden(StatusBarHiddenMode.True)
         .SetPreferredStatusBarUpdateAnimation(UIStatusBarAnimation.Fade);
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetPrefersStatusBarHidden` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to set the visibility of the status bar on a `Page` by specifying one of the `StatusBarHiddenMode` enumeration values: `Default`, `True`, or `False`. The `StatusBarHiddenMode.True` and `StatusBarHiddenMode.False` values set the status bar visibility regardless of device orientation, and the `StatusBarHiddenMode.Default` value hides the status bar in a vertically compact environment.

The result is that the visibility of the status bar on a `Page` can be set:

:::image type="content" source="media/page-status-bar-visibility/hide-status-bar.png" alt-text="Status Bar Visibility Platform-Specific.":::

> [!NOTE]
> On a `TabbedPage`, the specified `StatusBarHiddenMode` enumeration value will also update the status bar on all child pages. On all other `Page`-derived types, the specified `StatusBarHiddenMode` enumeration value will only update the status bar on the current page.

The `Page.SetPreferredStatusBarUpdateAnimation` method is used to set how the status bar enters or leaves the `Page` by specifying one of the `UIStatusBarAnimation` enumeration values: `None`, `Fade`, or `Slide`. If the `Fade` or `Slide` enumeration value is specified, a 0.25 second animation executes as the status bar enters or leaves the `Page`.
