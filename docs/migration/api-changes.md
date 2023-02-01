---
title: "API changes from Xamarin.Forms"
description: "Learn about the API changes that need to be made when moving from Xamarin.Forms to .NET MAUI."
ms.date: 1/31/2023
---

# API changes from Xamarin.Forms

Namespaces have changed in the move from Xamarin.Forms to .NET Multi-platform App UI (.NET MAUI), and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, do a find and replace for the following namespace changes:

| Xamarin.Forms namespace | .NET MAUI namespace(s) |
| --- | --- |
| `xmlns="http://xamarin.com/schemas/2014/forms"` | `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"` |
| `using Xamarin.Forms` | `using Microsoft.Maui` and `using Microsoft.Maui.Controls` |
| `using Xamarin.Forms.Xaml` | `using Microsoft.Maui.Controls.Xaml` |

The .NET MAUI class library project makes use of implicit `global using` directives. This enables you to remove `using` directives for the `Xamarin.Essentials` namespace, without having to resolve the types from that namespace.

## Colors

`Color` and `Colors` are separate types in .NET MAUI, and can be found in the `Microsoft.Maui.Graphics` namespace:

- `Color` - the type used to express a color
- `Colors` - holds static references to default colors by name, and extension methods to convert types.
- `Color.Default` doesn't exist. A `Color` defaults to `null`.

Default colors on views are `null`.

For more information about color changes, see [Microsoft.Maui.Graphics.Color vs Xamarin.Forms.Color](https://gist.github.com/hartez/593fc3fb87035a3aedc91657e9c15ab3).

## Other API changes

- `Shape`, and it's derivatives, are in `Microsoft.Maui.Controls.Shapes` namespace.
- `Frame.BorderColor=Accent` doesn't exist. Instead, use an explicit color.
- `ToolbarItem.Icon` doesn't exist. Instead, use `ToolbarItem.IconImageSource`.
- `Button.Image` doesn't exist. Instead, use `Button.ImageSource`.
- `Span.ForegroundColor` doesn't exist. Instead, use `Span.TextColor`.
- `OSTheme` doesn't exist. Instead, use `AppTheme`.

## Next step

> [!div class="nextstepaction"]
> [Bootstrap your app](bootstrap.md)
