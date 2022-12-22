---
title: "Android platform-specifics in .NET MAUI"
description: "Learn how to consume Android platform-specifics in .NET MAUI apps."
ms.date: 04/05/2022
---

# Android platform-specifics

.NET Multi-platform App UI (.NET MAUI) platform-specifics allow you to consume functionality that's only available on a specific platform, without customizing handlers.

The following platform-specific functionality is provided for .NET MAUI views on Android:

- Setting the input method editor options for the soft keyboard for an <xref:Microsoft.Maui.Controls.Entry>. For more information, see [Entry input method editor options on Android](entry-ime-options.md).
- Enabling fast scrolling in a <xref:Microsoft.Maui.Controls.ListView>. For more information, see [ListView fast scrolling on Android](listview-fast-scrolling.md).
- Controlling the transition that's used when opening a <xref:Microsoft.Maui.Controls.SwipeView>. For more information, see [SwipeView swipe transition Mode](swipeview-swipetransitionmode.md).
- Controlling whether a <xref:Microsoft.Maui.Controls.WebView> can display mixed content. For more information, see [WebView mixed content on Android](webview-mixed-content.md).
- Enabling zoom on a <xref:Microsoft.Maui.Controls.WebView>. For more information, see [WebView zoom on Android](webview-zoom-controls.md).

The following platform-specific functionality is provided for .NET MAUI pages on Android:

- Disabling transition animations when navigating through pages in a <xref:Microsoft.Maui.Controls.TabbedPage>. For more information, see [TabbedPage page transition animations on Android](tabbedpage-transition-animations.md).
- Enabling swiping between pages in a <xref:Microsoft.Maui.Controls.TabbedPage>. For more information, see [TabbedPage page swiping on Android](tabbedpage-page-swiping.md).
- Setting the toolbar placement and color on a <xref:Microsoft.Maui.Controls.TabbedPage>. For more information, see [TabbedPage toolbar placement on android](tabbedpage-toolbar-placement.md).

The following platform-specific functionality is provided for the .NET MAUI `Application` class on Android:

- Setting the operating mode of a soft keyboard. For more information, see [Soft keyboard input mode on Android](soft-keyboard-input-mode.md).
