---
title: "Upgrading Xamarin.Forms - Name Changes"
description: "Search and replace namespaces and name changes."
ms.date: 1/31/2023
---

# Upgrading from Xamarin.Forms - Name Changes

A few namespaces have been changed in .NET MAUI, and Xamarin.Essentials features are now part of .NET MAUI. To make namespace updates, do a find and replace for the following namespace changes:

| Old namespace | New namespace |
| --- | --- |
| `xmlns="http://xamarin.com/schemas/2014/forms"` | `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"` |
| `using Xamarin.Forms` | `using Microsoft.Maui` **AND** `using Microsoft.Maui.Controls` |
| `using Xamarin.Forms.Xaml` | `using Microsoft.Maui.Controls.Xaml` |

For `Xamarin.Essentials` remove the namespace and resolve each type individually. If your project makes extensive use of `Xamarin.Essentials` consider adding those namespaces as [global using](/dotnet/csharp/language-reference/keywords/using-directive) statements.

## Colors

`Color` and `Colors` are separate types in .NET MAUI based on `Microsoft.Maui.Graphics`. Default colors on views are null. The [provided style resource](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles) assigns starting values.

* `Color` - the type used to express a color
* `Colors` - holds static references to default colors by name, and extension methods to convert types
* `Color.Default` DOES NOT EXIST. A `Color` defaults to `null`. if you can instead (GitHub [Issue](https://github.com/dotnet/upgrade-assistant/issues/592))
  * Refer to .NET MAUI default [styles](https://github.com/dotnet/maui/tree/main/src/Templates/src/templates/maui-mobile/Resources/Styles)

## Additional name changes

- `Shapes` is in `Microsoft.Maui.Controls`
- `Frame`: `BorderColor`= "Accent" DOES NOT EXIST
- `ToolbarItem`: `Icon` == `IconImageSource`
- `Button`: `Image` == `ImageSource`
- `Span` `ForegroundColor` DOES NOT EXIST
- `OSTheme` == `AppTheme`

## Next step

> [!div class="nextstepaction"]
> [Configure .NET MAUI](forms-configuremaui.md)
