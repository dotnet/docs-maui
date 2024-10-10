---
title: "iOS platform-specifics in .NET MAUI"
description: "Learn how to consume iOS platform-specifics in .NET MAUI apps."
ms.date: 10/10/2024
---

# iOS platform-specifics

.NET Multi-platform App UI (.NET MAUI) platform-specifics allow you to consume functionality that's only available on a specific platform, without customizing handlers.

The following platform-specific functionality is provided for .NET MAUI views on iOS:

- Setting the <xref:Microsoft.Maui.Controls.Cell> background color. For more information, see [Cell background color on iOS](cell-background-color.md).
- Controlling when item selection occurs in a <xref:Microsoft.Maui.Controls.DatePicker>. For more information, see [DatePicker item selection on iOS](datepicker-selection.md).
- Ensuring that inputted text fits into an <xref:Microsoft.Maui.Controls.Entry> by adjusting the font size. For more information, see [Entry font size on iOS](entry-font-size.md).
- Setting the cursor color in a <xref:Microsoft.Maui.Controls.Entry>. For more information, see [Entry cursor color on iOS](entry-cursor-color.md).
- Controlling whether <xref:Microsoft.Maui.Controls.ListView> header cells float during scrolling. For more information, see [ListView group header style on iOS](listview-group-header-style.md).
- Controlling whether row animations are disabled when the <xref:Microsoft.Maui.Controls.ListView> items collection is being updated. For more information, see [ListView row animations on iOS](listview-row-animations.md).
- Setting the separator style on a <xref:Microsoft.Maui.Controls.ListView>. For more information, see [ListView separator style on iOS](listview-separator-style.md).
- Controlling when item selection occurs in a <xref:Microsoft.Maui.Controls.Picker>. For more information, see [Picker item selection on iOS](picker-selection.md).
- Controlling whether a <xref:Microsoft.Maui.Controls.SearchBar> has a background. For more information, see [SearchBar style on iOS](searchbar-style.md).
- Enabling the `Slider.Value` property to be set by tapping on a position on the <xref:Microsoft.Maui.Controls.Slider> bar, rather than by having to drag the <xref:Microsoft.Maui.Controls.Slider> thumb. For more information, see [Slider thumb tap on iOS](slider-thumb.md).
- Controlling the transition that's used when opening a <xref:Microsoft.Maui.Controls.SwipeView>. For more information, see [SwipeView swipe transition mode](swipeview-swipetransitionmode.md).
- Controlling when item selection occurs in a <xref:Microsoft.Maui.Controls.TimePicker>. For more information, see [TimePicker item selection on iOS](timepicker-selection.md).

The following platform-specific functionality is provided for .NET MAUI pages on iOS:

- Controlling whether the detail page of a <xref:Microsoft.Maui.Controls.FlyoutPage> has shadow applied to it, when revealing the flyout page. For more information, see [FlyoutPage shadow](flyoutpage-shadow.md).
- Controlling whether the navigation bar is translucent. For more information, see [Navigation bar translucency on iOS](navigation-bar-translucent.md).
- Controlling whether the page title is displayed as a large title in the page navigation bar. For more information, see [Large page titles on iOS](page-large-title.md).
- Disabling the safe area layout guide, which ensures that page content is positioned on an area of the screen that is safe for all iOS devices. For more information, see [Disable the safe area layout guide on iOS](page-safe-area-layout.md).
- Setting the visibility of the homage indicator on a <xref:Microsoft.Maui.Controls.Page>. For more information, see [Home indicator visibility on iOS](page-home-indicator.md).
- Setting the status bar visibility on a <xref:Microsoft.Maui.Controls.Page>. For more information, see [Page status bar visibility on iOS](page-status-bar-visibility.md).
- Setting the presentation style of modal pages. For more information, see [Modal page presentation style](page-presentation-style.md).
- Setting the translucency mode of the tab bar on a <xref:Microsoft.Maui.Controls.TabbedPage>. For more information, see [TabbedPage translucent TabBar on iOS](tabbedpage-translucent-tabbar.md).

The following platform-specific functionality is provided for .NET MAUI layouts on iOS:

- Controlling whether a <xref:Microsoft.Maui.Controls.ScrollView> handles a touch gesture or passes it to its content. For more information, see [ScrollView content touches on iOS](scrollview-content-touches.md).

The following platform-specific functionality is provided for the .NET MAUI <xref:Microsoft.Maui.Controls.Application> class on iOS:

- Enabling a <xref:Microsoft.Maui.Controls.PanGestureRecognizer> in a scrolling view to capture and share the pan gesture with the scrolling view. For more information, see [Simultaneous pan gesture recognition on iOS](application-pan-gesture.md).
