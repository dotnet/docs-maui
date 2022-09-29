---
title: "Upgrade Troubleshooting"
description: ""
ms.date: 10/01/2022
---

# Overview

When going through the upgrade to .NET 6, you may encounter compiler messages or runtime errors that need to be addressed. This page provides guidance for the most common scenarios.

| Issue    | Tip |
|---------|-------------|
| Layout is missing padding, margin, or spacing    | Add default values to your project based on the .NET MAUI style resource. [See this guide](layout-reference.md) |
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

