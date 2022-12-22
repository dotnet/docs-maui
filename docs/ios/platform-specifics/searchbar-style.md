---
title: "SearchBar style on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that controls whether a SearchBar has a background."
ms.date: 04/05/2022
---

# SearchBar style on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific controls whether a <xref:Microsoft.Maui.Controls.SearchBar> has a background. It's consumed in XAML by setting the `SearchBar.SearchBarStyle` bindable property to a value of the `UISearchBarStyle` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <SearchBar ios:SearchBar.SearchBarStyle="Minimal"
                   Placeholder="Enter search term" />
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

SearchBar searchBar = new SearchBar { Placeholder = "Enter search term" };
searchBar.On<iOS>().SetSearchBarStyle(UISearchBarStyle.Minimal);
```

The `SearchBar.On<iOS>` method specifies that this platform-specific will only run on iOS. The `SearchBar.SetSearchBarStyle` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to control whether the <xref:Microsoft.Maui.Controls.SearchBar> has a background. The `UISearchBarStyle` enumeration provides three possible values:

- `Default` indicates that the <xref:Microsoft.Maui.Controls.SearchBar> has the default style. This is the default value of the `SearchBar.SearchBarStyle` bindable property.
- `Prominent` indicates that the <xref:Microsoft.Maui.Controls.SearchBar> has a translucent background, and the search field is opaque.
- `Minimal` indicates that the <xref:Microsoft.Maui.Controls.SearchBar> has no background, and the search field is translucent.

In addition, the `SearchBar.GetSearchBarStyle` method can be used to return the `UISearchBarStyle` that's applied to the <xref:Microsoft.Maui.Controls.SearchBar>.

The result is that a specified `UISearchBarStyle` member is applied to a <xref:Microsoft.Maui.Controls.SearchBar>, which controls whether the <xref:Microsoft.Maui.Controls.SearchBar> has a background:

:::image type="content" source="media/searchbar-style/searchbar-styles.png" alt-text="Screenshot of SearchBar styles, on iOS.":::

The following screenshot shows the `UISearchBarStyle` members applied to <xref:Microsoft.Maui.Controls.SearchBar> objects that have their `BackgroundColor` property set:

:::image type="content" source="media/searchbar-style/searchbar-background-styles.png" alt-text="Screenshot of SearchBar styles with background color, on iOS.":::
