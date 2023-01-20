---
title: "TabbedPage page transition animations on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that disables transition animations when navigating through pages in a TabbedPage."
ms.date: 04/05/2022
---

# TabbedPage page transition animations on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific is used to disable transition animations when navigating through pages, either programmatically or when using the tab bar, in a <xref:Microsoft.Maui.Controls.TabbedPage>. It's consumed in XAML by setting the `TabbedPage.IsSmoothScrollEnabled` bindable property to `false`:

```xaml
<TabbedPage ...
            xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls"
            android:TabbedPage.IsSmoothScrollEnabled="false">
    ...
</TabbedPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetIsSmoothScrollEnabled(false);
```

> [!NOTE]
> This platform-specific has no effect on tabs in Shell-based apps.

The `TabbedPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `TabbedPage.SetIsSmoothScrollEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to control whether transition animations will be displayed when navigating between pages in a <xref:Microsoft.Maui.Controls.TabbedPage>. In addition, the <xref:Microsoft.Maui.Controls.TabbedPage> class in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace also has the following methods:

- `IsSmoothScrollEnabled`, which is used to retrieve whether transition animations will be displayed when navigating between pages in a <xref:Microsoft.Maui.Controls.TabbedPage>.
- `EnableSmoothScroll`, which is used to enable transition animations when navigating between pages in a <xref:Microsoft.Maui.Controls.TabbedPage>.
- `DisableSmoothScroll`, which is used to disable transition animations when navigating between pages in a <xref:Microsoft.Maui.Controls.TabbedPage>.
