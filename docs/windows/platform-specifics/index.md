---
title: "Windows platform-specifics in .NET MAUI"
description: "Learn how to consume Windows platform-specifics in .NET MAUI apps."
ms.date: 04/06/2022
---

# Windows platform-specifics

.NET Multi-platform App UI (.NET MAUI) platform-specifics allow you to consume functionality that's only available on a specific platform, without customizing handlers.

The following platform-specific functionality is provided for .NET MAUI views, pages, and layouts on Windows:

- Setting an access key for a <xref:Microsoft.Maui.Controls.VisualElement>. For more information, see [VisualElement Access Keys on Windows](visualelement-access-keys.md).

The following platform-specific functionality is provided for .NET MAUI views on Windows:

- Detecting reading order from text content in <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, and <xref:Microsoft.Maui.Controls.Label> instances. For more information, see [InputView Reading Order on Windows](inputview-reading-order.md).
- Enabling tap gesture support in a <xref:Microsoft.Maui.Controls.ListView>. For more information, see [ListView SelectionMode on Windows](listview-selectionmode.md).
- Enabling the pull direction of a <xref:Microsoft.Maui.Controls.RefreshView> to be changed. For more information, see [RefreshView Pull Direction on Windows](refreshview-pulldirection.md).
- Enabling a <xref:Microsoft.Maui.Controls.SearchBar> to interact with the spell check engine. For more information, see [SearchBar Spell Check on Windows](searchbar-spell-check.md).

The following platform-specific functionality is provided for the .NET MAUI `Application` class on Windows:

- Specifying the directory in the project that image assets will be loaded from. For more information, see [Default Image Directory on Windows](default-image-directory.md).
