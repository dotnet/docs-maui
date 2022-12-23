---
title: "ListView group header style on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls whether ListView header cells float during scrolling."
ms.date: 04/05/2022
---

# ListView group header style on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific controls whether <xref:Microsoft.Maui.Controls.ListView> header cells float during scrolling. It's consumed in XAML by setting the `ListView.GroupHeaderStyle` bindable property to a value of the `GroupHeaderStyle` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout Margin="20">
        <ListView ... ios:ListView.GroupHeaderStyle="Grouped">
            ...
        </ListView>
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

listView.On<iOS>().SetGroupHeaderStyle(GroupHeaderStyle.Grouped);
```

The `ListView.On<iOS>` method specifies that this platform-specific will only run on iOS. The `ListView.SetGroupHeaderStyle` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether <xref:Microsoft.Maui.Controls.ListView> header cells float during scrolling. The `GroupHeaderStyle` enumeration provides two possible values:

- `Plain` – indicates that header cells float when the <xref:Microsoft.Maui.Controls.ListView> is scrolled (default).
- `Grouped` – indicates that header cells do not float when the <xref:Microsoft.Maui.Controls.ListView> is scrolled.

In addition, the `ListView.GetGroupHeaderStyle` method can be used to return the `GroupHeaderStyle` that's applied to the <xref:Microsoft.Maui.Controls.ListView>.

The result is that a specified `GroupHeaderStyle` value is applied to the <xref:Microsoft.Maui.Controls.ListView>, which controls whether header cells float during scrolling:

:::image type="content" source="media/listview-group-header-style/group-header-styles.png" alt-text="Screenshot of floating and non-floating ListView header cells, on iOS.":::
