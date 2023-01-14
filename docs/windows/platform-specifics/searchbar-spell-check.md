---
title: "SearchBar spell check on Windows"
description: "This article explains how to consume the.NET MAUI Windows platform-specific that enables a SearchBar to interact with the spell check engine."
ms.date: 04/06/2022
---

# SearchBar spell check on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific enables a <xref:Microsoft.Maui.Controls.SearchBar> to interact with the spell check engine. It's consumed in XAML by setting the `SearchBar.IsSpellCheckEnabled` attached property to a `boolean` value:

```xaml
<ContentPage ...
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <SearchBar ... windows:SearchBar.IsSpellCheckEnabled="true" />
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

searchBar.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetIsSpellCheckEnabled(true);
```

The `SearchBar.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `SearchBar.SetIsSpellCheckEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, turns the spell checker on and off. In addition, the `SearchBar.SetIsSpellCheckEnabled` method can be used to toggle the spell checker by calling the `SearchBar.GetIsSpellCheckEnabled` method to return whether the spell checker is enabled:

```csharp
searchBar.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetIsSpellCheckEnabled(!searchBar.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetIsSpellCheckEnabled());
```

The result is that text entered into the <xref:Microsoft.Maui.Controls.SearchBar> can be spell checked, with incorrect spellings being indicated to the user:

:::image type="content" source="media/searchbar-spell-check/searchbar-spellcheck.png" alt-text="SearchBar spell check platform-specific.":::

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.SearchBar> class in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace also has `EnableSpellCheck` and `DisableSpellCheck` methods that can be used to enable and disable the spell checker on the <xref:Microsoft.Maui.Controls.SearchBar>, respectively.
