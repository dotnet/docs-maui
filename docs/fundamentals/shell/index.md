---
title: ".NET MAUI Shell overview"
description: ".NET MAUI Shell provides the fundamental features that most apps require, including a common navigation user experience, a URI-based navigation scheme, and an integrated search handler."
ms.date: 08/30/2024
---

# .NET MAUI Shell overview

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

.NET Multi-platform App UI (.NET MAUI) Shell reduces the complexity of app development by providing the fundamental features that most apps require, including:

- A single place to describe the visual hierarchy of an app.
- A common navigation user experience.
- A URI-based navigation scheme that permits navigation to any page in the app.
- An integrated search handler.

## App visual hierarchy

In a .NET MAUI Shell app, the visual hierarchy of the app is described in a class that subclasses the <xref:Microsoft.Maui.Controls.Shell> class. This class can consist of three main hierarchical objects:

1. <xref:Microsoft.Maui.Controls.FlyoutItem> or <xref:Microsoft.Maui.Controls.TabBar>. A <xref:Microsoft.Maui.Controls.FlyoutItem> represents one or more items in the flyout, and should be used when the navigation pattern for the app requires a flyout. A <xref:Microsoft.Maui.Controls.TabBar> represents the bottom tab bar, and should be used when the navigation pattern for the app begins with bottom tabs and doesn't require a flyout. For more information about flyout items, see [.NET MAUI Shell flyout](flyout.md). For more information about tab bars, see [.NET MAUI Shell tabs](tabs.md).
1. <xref:Microsoft.Maui.Controls.Tab>, which represents grouped content, navigable by bottom tabs. For more information, see [.NET MAUI Shell tabs](tabs.md).
1. <xref:Microsoft.Maui.Controls.ShellContent>, which represents the <xref:Microsoft.Maui.Controls.ContentPage> objects for each tab. For more information, see [.NET MAUI Shell pages](pages.md).

These objects don't represent any user interface, but rather the organization of the app's visual hierarchy. Shell will take these objects and produce the navigation user interface for the content.

> [!NOTE]
> Pages are created on demand in Shell apps, in response to navigation.

For more information, see [Create a .NET MAUI Shell app](create.md).

## Navigation user experience

The navigation experience provided by .NET MAUI Shell is based on flyouts and tabs. The top level of navigation in a Shell app is either a flyout or a bottom tab bar, depending on the navigation requirements of the app. The following example shows an app where the top level of navigation is a flyout:

:::image type="content" source="media/flyout.png" alt-text="Screenshot of a Shell flyout.":::

In this example, some flyout items are duplicated as tab bar items. However, there are also items that can only be accessed from the flyout. Selecting a flyout item results in the bottom tab that represents the item being selected and displayed:

:::image type="content" source="media/cats.png" alt-text="Screenshot of Shell bottom tabs.":::

> [!NOTE]
> When the flyout isn't open the bottom tab bar can be considered to be the top level of navigation in the app.

Each tab on the tab bar displays a <xref:Microsoft.Maui.Controls.ContentPage>. However, if a bottom tab contains more than one page, the pages are navigable by the top tab bar:

:::image type="content" source="media/dogs.png" alt-text="Screenshot of Shell top tabs.":::

Within each tab, additional <xref:Microsoft.Maui.Controls.ContentPage> objects that are known as detail pages, can be navigated to:

:::image type="content" source="media/dogdetails.png" alt-text="Screenshot of Shell page navigation.":::

Shell uses a URI-based navigation experience that uses routes to navigate to any page in the app, without having to follow a set navigation hierarchy. In addition, it also provides the ability to navigate backwards without having to visit all of the pages on the navigation stack. For more information, see [.NET MAUI Shell navigation](navigation.md).

## Search

.NET MAUI Shell includes integrated search functionality that's provided by the <xref:Microsoft.Maui.Controls.SearchHandler> class. Search capability can be added to a page by adding a subclassed <xref:Microsoft.Maui.Controls.SearchHandler> object to it. This results in a search box being added at the top of the page. When data is entered into the search box, the search suggestions area is populated with data:

:::image type="content" source="media/search.png" alt-text="Screenshot of Shell search.":::

Then, when a result is selected from the search suggestions area, custom logic can be executed such as navigating to a detail page.

For more information, see [.NET MAUI Shell search](search.md).
