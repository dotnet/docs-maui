---
title: "CarouselView"
description: "The .NET MAUI CarouselView is a view for presenting data in a scrollable layout, where users can swipe to move through a collection of items."
ms.date: 01/18/2022
---

# CarouselView

The .NET Multi-platform App UI (.NET MAUI) `CarouselView` is a view for presenting data in a scrollable layout, where users can swipe to move through a collection of items.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

By default, `CarouselView` will display its items in a horizontal orientation. A single item will be displayed on screen, with swipe gestures resulting in forwards and backwards navigation through the collection of items. In addition, indicators can be displayed that represent each item in the `CarouselView`:

:::image type="content" source="media/populate-data/indicators.png" alt-text="Screenshot of a CarouselView and IndicatorView.":::

By default, `CarouselView` provides looped access to its collection of items. Therefore, swiping backwards from the first item in the collection will display the last item in the collection. Similarly, swiping forwards from the last item in the collection will return to the first item in the collection.

`CarouselView` shares much of its implementation with `CollectionView`. However, the two controls have different use cases. `CollectionView` is typically used to present lists of data of any length, whereas `CarouselView` is typically used to highlight information in a list of limited length. For more information about `CollectionView`, see [CollectionView](~/user-interface/controls/collectionview/index.md).
