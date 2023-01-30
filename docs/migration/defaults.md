---
title: "Default values changed"
description: "Changes to default values."
ms.date: 1/20/2023
---

# Default values changed

.NET MAUI levels out default values across colors, padding, margins, spacing, and elsewhere where Xamarin.Forms set arbitrary values. The default style resource included in new .NET MAUI projects provides default values. We advise you take a similar approach with your styles, or inherit from [these provided styles](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles).

Xamarin.Forms included arbitrary default values for padding, margins, spacing, etc. .NET MAUI zeros them out. To preserve the Xamarin.Forms values where projects don't declare explicit values, add implicit styles to your project.

| Property | Xamarin.Forms | .NET MAUI |
| --- | --- | --- |
| Grid.ColumnSpacing | 6 | 0 |
| Grid.RowSpacing | 6 | 0 |
| StackLayout.Spacing | 6 | 0 |

### See also

* [Default styles](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles)
