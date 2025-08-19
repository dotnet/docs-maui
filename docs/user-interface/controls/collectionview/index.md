---
title: "CollectionView"
description: "The .NET MAUI CollectionView displays a scrollable list of selectable data items, using different layout specifications."
ms.date: 08/19/2025
---

# CollectionView

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-collectionview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.CollectionView> is a view for presenting lists of data using different layout specifications. It aims to provide a more flexible, and performant alternative to <xref:Microsoft.Maui.Controls.ListView>.

The following screenshot shows a <xref:Microsoft.Maui.Controls.CollectionView> that uses a two-column vertical grid and allows multiple selections:

:::image type="content" source="media/verticalgrid-multipleselection.png" alt-text="Screenshot of a CollectionView vertical grid layout.":::

<xref:Microsoft.Maui.Controls.CollectionView> should be used for presenting lists of data that require scrolling or selection. A bindable layout can be used when the data to be displayed doesn't require scrolling or selection. For more information, see [BindableLayout](~/user-interface/layouts/bindablelayout.md).

::: moniker range=">=net-maui-10.0"

> [!NOTE]
> On iOS and Mac Catalyst, the optimized handlers that were optional in .NET 9 are the default handlers for <xref:Microsoft.Maui.Controls.CollectionView> in .NET 10, providing improved performance and stability.

::: moniker-end

## CollectionView and ListView differences

While the <xref:Microsoft.Maui.Controls.CollectionView> and <xref:Microsoft.Maui.Controls.ListView> APIs are similar, there are some notable differences:

- <xref:Microsoft.Maui.Controls.CollectionView> has a flexible layout model, which allows data to be presented vertically or horizontally, in a list or a grid.
- <xref:Microsoft.Maui.Controls.CollectionView> supports single and multiple selection.
- <xref:Microsoft.Maui.Controls.CollectionView> has no concept of cells. Instead, a data template is used to define the appearance of each item of data in the list.
- <xref:Microsoft.Maui.Controls.CollectionView> automatically utilizes the virtualization provided by the underlying native controls.
- <xref:Microsoft.Maui.Controls.CollectionView> reduces the API surface of <xref:Microsoft.Maui.Controls.ListView>. Many properties and events from <xref:Microsoft.Maui.Controls.ListView> are not present in <xref:Microsoft.Maui.Controls.CollectionView>.
- <xref:Microsoft.Maui.Controls.CollectionView> does not include built-in separators.
- <xref:Microsoft.Maui.Controls.CollectionView> will throw an exception if its `ItemsSource` is updated off the UI thread.

## Move from ListView to CollectionView

<xref:Microsoft.Maui.Controls.ListView> implementations can be migrated to <xref:Microsoft.Maui.Controls.CollectionView> implementations with the help of the following table:

| Concept | ListView API | CollectionView |
|---|---|---|
| Data | `ItemsSource` | A <xref:Microsoft.Maui.Controls.CollectionView> is populated with data by setting its `ItemsSource` property. For more information, see [Populate a CollectionView with data](populate-data.md#populate-a-collectionview-with-data). |
| Item appearance | `ItemTemplate` | The appearance of each item in a <xref:Microsoft.Maui.Controls.CollectionView> can be defined by setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>. For more information, see [Define item appearance](populate-data.md#define-item-appearance). |
| Cells | <xref:Microsoft.Maui.Controls.TextCell>, <xref:Microsoft.Maui.Controls.ImageCell>, <xref:Microsoft.Maui.Controls.ViewCell> | <xref:Microsoft.Maui.Controls.CollectionView> has no concept of cells, and therefore no concept of disclosure indicators. Instead, a data template is used to define the appearance of each item of data in the list. |
| Row separators | `SeparatorColor`, `SeparatorVisibility` | <xref:Microsoft.Maui.Controls.CollectionView> does not include built-in separators. These can be provided, if desired, in the item template. |
| Selection | `SelectionMode`, `SelectedItem` | <xref:Microsoft.Maui.Controls.CollectionView> supports single and multiple selection. For more information, see [Configure CollectionView item selection](selection.md). |
| Row height | `HasUnevenRows`, `RowHeight` | In a <xref:Microsoft.Maui.Controls.CollectionView>, the row height of each item is determined by the `ItemSizingStrategy` property. For more information, see [Item sizing](layout.md#item-sizing).|
| Caching | `CachingStrategy` | <xref:Microsoft.Maui.Controls.CollectionView> automatically uses the virtualization provided by the underlying native controls. |
| Headers and footers | `Header`, `HeaderElement`, `HeaderTemplate`, `Footer`, `FooterElement`, `FooterTemplate` | <xref:Microsoft.Maui.Controls.CollectionView> can present a header and footer that scroll with the items in the list, via the `Header`, `Footer`, `HeaderTemplate`, and `FooterTemplate` properties. For more information, see [Headers and footers](layout.md#headers-and-footers). |
| Grouping | `GroupDisplayBinding`, `GroupHeaderTemplate`, `GroupShortNameBinding`, `IsGroupingEnabled` | <xref:Microsoft.Maui.Controls.CollectionView> displays correctly grouped data by setting its `IsGrouped` property to `true`. Group headers and group footers can be customized by setting the `GroupHeaderTemplate` and `GroupFooterTemplate` properties to  <xref:Microsoft.Maui.Controls.DataTemplate> objects. For more information, see [Display grouped data in a CollectionView](grouping.md). |
| Pull to refresh | `IsPullToRefreshEnabled`, `IsRefreshing`, `RefreshAllowed`, `RefreshCommand`, `RefreshControlColor`, `BeginRefresh()`, `EndRefresh()` | Pull to refresh functionality is supported by setting a <xref:Microsoft.Maui.Controls.CollectionView> as the child of a <xref:Microsoft.Maui.Controls.RefreshView>. For more information, see [Pull to refresh](populate-data.md#pull-to-refresh). |
| Context menu items | `ContextActions` | Context menu items are supported by setting a <xref:Microsoft.Maui.Controls.SwipeView> as the root view in the <xref:Microsoft.Maui.Controls.DataTemplate> that defines the appearance of each item of data in the <xref:Microsoft.Maui.Controls.CollectionView>. For more information, see [Context menus](populate-data.md#context-menus). |
| Scrolling | `ScrollTo()` | <xref:Microsoft.Maui.Controls.CollectionView> defines `ScrollTo` methods, which scroll items into view. For more information, see [Control scrolling in a CollectionView](scrolling.md). |
