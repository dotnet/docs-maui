---
title: "Large page titles on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that displays the page title as a large title on the navigation bar of a NavigationPage."
ms.date: 04/05/2022
---

# Large Page Titles on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to display the page title as a large title on the navigation bar of a <xref:Microsoft.Maui.Controls.NavigationPage>, for devices that use iOS 11 or greater. A large title is left aligned and uses a larger font, and transitions to a standard title as the user begins scrolling content, so that the screen real estate is used efficiently. However, in landscape orientation, the title will return to the center of the navigation bar to optimize content layout. It's consumed in XAML by setting the `NavigationPage.PrefersLargeTitles` attached property to a `boolean` value:

```xaml
<NavigationPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
                ...
                ios:NavigationPage.PrefersLargeTitles="true">
  ...
</NavigationPage>
```

Alternatively it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

var navigationPage = new Microsoft.Maui.Controls.NavigationPage(new iOSLargeTitlePageCode());
navigationPage.On<iOS>().SetPrefersLargeTitles(true);
```

The `NavigationPage.On<iOS>` method specifies that this platform-specific will only run on iOS. The `NavigationPage.SetPrefersLargeTitle` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls whether large titles are enabled.

Provided that large titles are enabled on the <xref:Microsoft.Maui.Controls.NavigationPage>, all pages in the navigation stack will display large titles. This behavior can be overridden on pages by setting the `Page.LargeTitleDisplay` attached property to a value of the `LargeTitleDisplayMode` enumeration:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             Title="Large Title"
             ios:Page.LargeTitleDisplay="Never">
  ...
</ContentPage>
```

Alternatively, the page behavior can be overridden from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

public class iOSLargeTitlePageCode : ContentPage
{
    public iOSLargeTitlePageCode
    {
        On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Never);
    }
    ...
}
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetLargeTitleDisplay` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, controls the large title behavior on the <xref:Microsoft.Maui.Controls.Page>, with the `LargeTitleDisplayMode` enumeration providing three possible values:

- `Always` – force the navigation bar and font size to use the large format.
- `Automatic` – use the same style (large or small) as the previous item in the navigation stack.
- `Never` – force the use of the regular, small format navigation bar.

In addition, the `SetLargeTitleDisplay` method can be used to toggle the enumeration values by calling the `LargeTitleDisplay` method, which returns the current `LargeTitleDisplayMode`:

```csharp
switch (On<iOS>().LargeTitleDisplay())
{
    case LargeTitleDisplayMode.Always:
        On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Automatic);
        break;
    case LargeTitleDisplayMode.Automatic:
        On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Never);
        break;
    case LargeTitleDisplayMode.Never:
        On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Always);
        break;
}
```

The result is that a specified `LargeTitleDisplayMode` is applied to the <xref:Microsoft.Maui.Controls.Page>, which controls the large title behavior:

:::image type="content" source="media/page-large-title/large-title.png" alt-text="Page title as a large title.":::
