---
title: "FlyoutPage navigation bar on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that collapses the navigation bar on a FlyoutPage."
ms.date: 04/06/2022
---

# FlyoutPage navigation bar on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific is used to collapse the navigation bar on a `FlyoutPage`, and is consumed in XAML by setting the `FlyoutPage.CollapseStyle` and `FlyoutPage.CollapsedPaneWidth` attached properties:

```xaml
<FlyoutPage ...
            xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls"
            windows:FlyoutPage.CollapseStyle="Partial"
            windows:FlyoutPage.CollapsedPaneWidth="48">
  ...
</FlyoutPage>

```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

page.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetCollapseStyle(CollapseStyle.Partial).CollapsedPaneWidth(148);
```

The `FlyoutPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `Page.SetCollapseStyle` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to specify the collapse style, with the `CollapseStyle` enumeration providing two values: `Full` and `Partial`. The `FlyoutPage.CollapsedPaneWidth` method is used to specify the width of a partially collapsed navigation bar.

The result is that a specified `CollapseStyle` is applied to the `FlyoutPage` instance, with the width also being specified:

:::image type="content" source="media/flyoutpage-navigation-bar/collapsed-navigation-bar.png" alt-text="Collapsed Navigation Bar Platform-Specific.":::
