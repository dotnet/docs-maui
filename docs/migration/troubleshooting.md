---
title: "Troubleshooting Xamarin.Forms to .NET MAUI app migration"
description: "Learn how to troubleshoot issues when migrating your apps from Xamarin.Forms to .NET MAUI."
ms.date: 1/31/2023
---

# Troubleshooting Xamarin.Forms to .NET MAUI app migration

When upgrading a Xamarin.Forms apps to .NET Multi-platform App UI (.NET MAUI), you may encounter build or runtime errors that need to be addressed.

<!-- markdownlint-disable MD032 -->
> [!TIP]
> - Delete all *bin* and *obj* folders from all projects before opening and building projects in Visual Studio, particularly when changing .NET versions.
> - Delete the 'ResourceDesigner' generated file from the Android project.
<!-- markdownlint-enable MD032 -->

The following table provides guidance for overcoming common build or runtime errors:

| Issue | Tip |
| ----- | --- |
| Layout is missing padding, margin, or spacing. | Add default values to your project based on the .NET MAUI style resource. For more information, see [Default value changes from Xamarin.Forms](defaults.md). |
| `Color.Red` and similar can't be found | Named colors are now in `Microsoft.Maui.Graphics.Colors`. |
| `Color` and `Colors` can't be found | `Color` and `Colors` are now in the `Microsoft.Maui.Graphics` namespace. |
| `Color.Default` doesn't exist. | Use `Colors.Black` instead. |
| `Frame.BorderColor=Accent` doesn't exist | Use an explicit color. |
| `ToolbarItem.Icon` doesn't exist. | Use `ToolbarItem.IconImageSource`. |
| `Button.Image` doesn't exist. | Use `Button.ImageSource`. |
| `Span.ForegroundColor` doesn't exist. | Use `Span.TextColor`. |
| `OSTheme` doesn't exist. | Use `AppTheme`. |
| `Xamarin.Essentials` namespace doesn't exist. | Remove the namespace and resolve types individually. |
| `Xamarin.Forms` namespace doesn't exist. | Replace with the `Microsoft.Maui` or `Microsoft.Maui.Controls` namespace, depending on the type used. |
| `Xamarin.Forms.Xaml` namespace doesn't. | Replace with the `Microsoft.Maui.Controls.Xaml` namespace. |
| `CollectionView` doesn't scroll. | Check the container layout and the measured size of the `CollectionView`. By default the control will take up as much space as the container allows. A `Grid` will constrain children at its own size. However a `StackLayout` will enable children to take up space beyond its bounds. |

For more information about color changes, see [Microsoft.Maui.Graphics.Color vs Xamarin.Forms.Color](https://gist.github.com/hartez/593fc3fb87035a3aedc91657e9c15ab3).
