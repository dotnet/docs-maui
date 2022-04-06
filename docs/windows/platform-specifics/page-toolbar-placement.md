---
title: "Page toolbar placement on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that changes the placement of a toolbar on a Page."
ms.date: 04/06/2022
---

# Page toolbar placement on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific is used to change the placement of a toolbar on a `Page`, and is consumed in XAML by setting the `Page.ToolbarPlacement` attached property to a value of the `ToolbarPlacement` enumeration:

```xaml
<TabbedPage ...
            xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls"
            windows:Page.ToolbarPlacement="Bottom">
  ...
</TabbedPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

page.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetToolbarPlacement(ToolbarPlacement.Bottom);
```

The `Page.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `Page.SetToolbarPlacement` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to set the toolbar placement, with the `ToolbarPlacement` enumeration providing three values: `Default`, `Top`, and `Bottom`.

The result is that the specified toolbar placement is applied to the `Page` object:

:::image type="content" source="media/page-toolbar-placement/toolbar-placement.png" alt-text="Toolbar Placement Platform-Specific.":::
