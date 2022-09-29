---
title: "Default Values Changed"
description: ""
ms.date: 10/01/2022
---

# Overview

.NET MAUI levels out default values across colors, padding, margins, spacing, and elsewhere where Xamarin.Forms set arbitrary values. The default style resource included in new .NET MAUI projects provides default values. We advise you take a similar approach with your styles, or inherit from [these provided styles](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles).

## Colors

`Color` and `Colors` are separate types in .NET MAUI based on `Microsoft.Maui.Graphics`. Default colors on views are null. The [provided style resource](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles) assigns starting values.

* `Color` - the type used to express a color
* `Colors` - holds static references to default colors by name, and extension methods to convert types

# Other Default Values

Xamarin.Forms included arbitrary default values for padding, margins, spacing, etc. .NET MAUI zeros them out. To preserve the Xamarin.Forms values where projects don't declare explicit values, add implicit styles to your project.

| Property | Xamarin.Forms | .NET MAUI |
| --- | --- | --- |
| Button.Padding | | NaN |
| IndicatorView.Spacing | [?](https://cs.github.com/xamarin/Xamarin.Forms/blob/022cf08cbbe24141d1db19c314854afc64f6c097/Xamarin.Forms.Core/IndicatorView.cs#L119) | 4 |
| Grid.ColumnSpacing | 6 | 0 |
| Grid.RowSpacing | 6 | 0 |
| StackLayout.Spacing | 6 | 0 |

### See also

* [Default styles](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles)