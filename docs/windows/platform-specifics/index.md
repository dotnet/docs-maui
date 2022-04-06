---
title: "Windows platform-specifics in .NET MAUI"
description: "Learn how to consume Windows platform-specifics in .NET MAUI apps."
ms.date: 04/06/2022
---

# Windows platform-specifics in .NET MAUI

.NET Multi-platform App UI (.NET MAUI) platform-specifics allow you to consume functionality that's only available on a specific platform, without customizing handlers.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The following platform-specific functionality is provided for .NET MAUI views, pages, and layouts on Windows:

- Setting an access key for a `VisualElement`. For more information, see [VisualElement Access Keys on Windows](visualelement-access-keys.md).
- Disabling legacy color mode on a supported `VisualElement`. For more information, see [VisualElement Legacy Color Mode on Windows](legacy-color-mode.md).

The following platform-specific functionality is provided for .NET MAUI views on Windows:

- Detecting reading order from text content in `Entry`, `Editor`, and `Label` instances. For more information, see [InputView Reading Order on Windows](inputview-reading-order.md).
- Enabling tap gesture support in a `ListView`. For more information, see [ListView SelectionMode on Windows](listview-selectionmode.md).
- Enabling the pull direction of a `RefreshView` to be changed. For more information, see [RefreshView Pull Direction on Windows](refreshview-pulldirection.md).
- Enabling a `SearchBar` to interact with the spell check engine. For more information, see [SearchBar Spell Check on Windows](searchbar-spell-check.md).
- Setting the thread on which a `WebView` hosts its content. For more information, see [WebView Execution Mode on Windows](webview-executionmode.md).
- Enabling a `WebView` to display JavaScript alerts in a UWP message dialog. For more information, see [WebView JavaScript Alerts on Windows](webview-javascript-alert.md).

The following platform-specific functionality is provided for .NET MAUI pages on Windows:

- Collapsing the `FlyoutPage` navigation bar. For more information, see [FlyoutPage Navigation Bar on Windows](flyoutpage-navigation-bar.md).
- Setting toolbar placement options. For more information, see [Page Toolbar Placement on Windows](page-toolbar-placement.md).
- Enabling page icons to be displayed on a `TabbedPage` toolbar. For more information, see [TabbedPage Icons on Windows](tabbedpage-icons.md).

The following platform-specific functionality is provided for the .NET MAUI `Application` class on Windows:

- Specifying the directory in the project that image assets will be loaded from. For more information, see [Default Image Directory on Windows](default-image-directory.md).
