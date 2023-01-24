---
title: "Upgrade troubleshooting"
description: "Upgrade tips for troubleshooting."
ms.date: 1/20/2023
---

# Upgrade troubleshooting

When going through the upgrade to .NET 6, you may encounter compiler messages or runtime errors that need to be addressed. This page provides guidance for the most common scenarios.

General tips:

* Delete all bin and obj folders from all projects before opening and building projects in Visual Studio, especially when changing .NET versions.
* Delete the 'ResourceDesigner' generated file from the Android project.

| Issue    | Tip |
|---------|-------------|
| Layout is missing padding, margin, or spacing    | Add default values to your project based on the .NET MAUI style resource. [See this guide][0] |
| `Color.Red` and similar cannot be found | Named colors are now in `Microsoft.Maui.Graphics.Colors` |
| Color cannot be found | Color and Colors are now in `Microsoft.Maui.Graphics` |
| `Color.Default` does not exist | Use `ClearValue` instead. [Details](https://github.com/dotnet/upgrade-assistant/issues/592) |
| `Frame.BorderColor=Accent` does not exist | User an explicit color |
| `ToolbarItem.Icon` does not exist | Use `ToolbarItem.IconImageSource` |
| `Button.Image` does not exist | Use `Button.ImageSource` |
| `Span.ForegroundColor` does not exist | Use `Span.TextColor` |
| `OSTheme` does not exist | Use `AppTheme` |
| `Xamarin.Essentials` does not exist | Remove the namespace and resolve types individually |
| `Xamarin.Forms` does not exist | Replace with `Microsoft.Maui` or `Microsoft.Maui.Controls` depending on the type used |
| `Xamarin.Forms.Xaml` does not exist | Replace with `Microsoft.Maui.Controls.Xaml` |
| `CollectionView` does not scroll | Check the container layout and the measured size of the `CollectionView`. By default the control will take up as much space as the container allows. A `Grid` will constrain children at its own size, however a `StackLayout` will allow children to take up space beyond its bounds. |

### See also:

* [Layout reference][0]
* [Default values][1]

[0]: layout-reference.md
[1]: defaults.md
