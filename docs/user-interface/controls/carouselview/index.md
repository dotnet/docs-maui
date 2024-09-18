---
title: "CarouselView"
description: "The .NET MAUI CarouselView is a view for presenting data in a scrollable layout, where users can swipe to move through a collection of items."
ms.date: 09/30/2024
---

# CarouselView

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-carouselview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.CarouselView> is a view for presenting data in a scrollable layout, where users can swipe to move through a collection of items.

By default, <xref:Microsoft.Maui.Controls.CarouselView> will display its items in a horizontal orientation. A single item will be displayed on screen, with swipe gestures resulting in forwards and backwards navigation through the collection of items. In addition, indicators can be displayed that represent each item in the <xref:Microsoft.Maui.Controls.CarouselView>:

:::image type="content" source="media/populate-data/indicators.png" alt-text="Screenshot of a CarouselView and IndicatorView.":::

By default, <xref:Microsoft.Maui.Controls.CarouselView> provides looped access to its collection of items. Therefore, swiping backwards from the first item in the collection will display the last item in the collection. Similarly, swiping forwards from the last item in the collection will return to the first item in the collection.

<xref:Microsoft.Maui.Controls.CarouselView> shares much of its implementation with <xref:Microsoft.Maui.Controls.CollectionView>. However, the two controls have different use cases. <xref:Microsoft.Maui.Controls.CollectionView> is typically used to present lists of data of any length, whereas <xref:Microsoft.Maui.Controls.CarouselView> is typically used to highlight information in a list of limited length. For more information about <xref:Microsoft.Maui.Controls.CollectionView>, see [CollectionView](~/user-interface/controls/collectionview/index.md).
