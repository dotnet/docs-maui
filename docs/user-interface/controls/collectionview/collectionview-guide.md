---
title: "CollectionView guide for .NET MAUI"
description: "A comprehensive guide to using CollectionView in .NET MAUI — the recommended control for displaying lists of data with virtualization, flexible layouts, selection, grouping, and empty views."
ms.date: 02/10/2026
ms.topic: conceptual
---

# CollectionView guide for .NET MAUI

## Overview and when to use CollectionView

<xref:Microsoft.Maui.Controls.CollectionView> is the recommended control for displaying lists of data in .NET MAUI. It replaces <xref:Microsoft.Maui.Controls.ListView>, which is obsolete and should not be used in new projects.

Key advantages of `CollectionView` over `ListView`:

- **Flexible layouts** — vertical lists, horizontal lists, vertical grids, and horizontal grids.
- **No cells required** — uses <xref:Microsoft.Maui.Controls.DataTemplate> directly instead of `ViewCell`.
- **Single and multiple selection** — built-in support via `SelectionMode`.
- **Headers, footers, and empty views** — declarative support without custom renderers.
- **Virtualization** — efficient recycling of item views for large data sets.

> [!WARNING]
> **Never place `CollectionView` inside a `StackLayout` or `VerticalStackLayout`.** These layouts give `CollectionView` infinite space, which disables virtualization and causes severe performance degradation or blank rendering. Always use `Grid` or another constrained layout as the parent.

> [!WARNING]
> Updating `ItemsSource` from a background thread throws an exception. Always update the collection on the UI thread using `MainThread.BeginInvokeOnMainThread`.

> [!TIP]
> For small collections (≤20 items), consider using `BindableLayout` instead. It doesn't virtualize but is simpler to set up and avoids the overhead of a full `CollectionView`.

## Populating with data

### Basic ItemsSource binding

Bind `ItemsSource` to an `ObservableCollection<T>` or any `IEnumerable` in your view model. Define the appearance of each item with an `ItemTemplate`.

In XAML:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                x:DataType="vm:MonkeysViewModel">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Grid Padding="10" ColumnDefinitions="Auto,*" ColumnSpacing="10">
                <Image Source="{Binding ImageUrl}"
                       HeightRequest="60" WidthRequest="60"
                       Aspect="AspectFill" />
                <VerticalStackLayout Grid.Column="1" VerticalOptions="Center">
                    <Label Text="{Binding Name}" FontAttributes="Bold" />
                    <Label Text="{Binding Location}" FontSize="12" TextColor="Gray" />
                </VerticalStackLayout>
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

In C#:

```csharp
var collectionView = new CollectionView
{
    ItemsSource = viewModel.Monkeys,
    ItemTemplate = new DataTemplate(() =>
    {
        var grid = new Grid
        {
            Padding = 10,
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = 10
        };

        var image = new Image { HeightRequest = 60, WidthRequest = 60, Aspect = Aspect.AspectFill };
        image.SetBinding(Image.SourceProperty, nameof(Monkey.ImageUrl));

        var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
        nameLabel.SetBinding(Label.TextProperty, nameof(Monkey.Name));

        var locationLabel = new Label { FontSize = 12, TextColor = Colors.Gray };
        locationLabel.SetBinding(Label.TextProperty, nameof(Monkey.Location));

        var stack = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center };
        stack.Children.Add(nameLabel);
        stack.Children.Add(locationLabel);

        grid.Add(image);
        grid.Add(stack, 1);

        return grid;
    })
};
```

> [!IMPORTANT]
> Always use `x:DataType` on both the page and the `DataTemplate` for compiled bindings. This enables compile-time validation and improves performance.

> [!WARNING]
> Never use `ViewCell` as the `DataTemplate` root. `CollectionView` does not use cells — place your layout element (such as `Grid`) directly inside the `DataTemplate`.

### DataTemplateSelector

Use a <xref:Microsoft.Maui.Controls.DataTemplateSelector> to choose different templates based on item data:

```csharp
public class AnimalTemplateSelector : DataTemplateSelector
{
    public DataTemplate MonkeyTemplate { get; set; }
    public DataTemplate CatTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((Animal)item).Type == "Monkey" ? MonkeyTemplate : CatTemplate;
    }
}
```

Reference it in XAML:

```xaml
<ContentPage.Resources>
    <DataTemplate x:Key="monkeyTemplate" x:DataType="model:Animal">
        <Grid Padding="10">
            <Label Text="{Binding Name}" FontAttributes="Bold" />
        </Grid>
    </DataTemplate>
    <DataTemplate x:Key="catTemplate" x:DataType="model:Animal">
        <Grid Padding="10" BackgroundColor="LightGoldenrodYellow">
            <Label Text="{Binding Name}" FontAttributes="Italic" />
        </Grid>
    </DataTemplate>
    <local:AnimalTemplateSelector x:Key="animalSelector"
                                  MonkeyTemplate="{StaticResource monkeyTemplate}"
                                  CatTemplate="{StaticResource catTemplate}" />
</ContentPage.Resources>

<CollectionView ItemsSource="{Binding Animals}"
                ItemTemplate="{StaticResource animalSelector}" />
```

### Pull-to-refresh

Wrap `CollectionView` in a <xref:Microsoft.Maui.Controls.RefreshView> to enable pull-to-refresh:

```xaml
<RefreshView IsRefreshing="{Binding IsRefreshing}"
             Command="{Binding RefreshCommand}">
    <CollectionView ItemsSource="{Binding Items}">
        <CollectionView.ItemTemplate>
            <DataTemplate x:DataType="model:Item">
                <Label Text="{Binding Title}" Padding="10" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</RefreshView>
```

In C#:

```csharp
var refreshView = new RefreshView
{
    Content = collectionView
};
refreshView.SetBinding(RefreshView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
refreshView.SetBinding(RefreshView.CommandProperty, nameof(ViewModel.RefreshCommand));
```

### Incremental loading

Use `RemainingItemsThreshold` and `RemainingItemsThresholdReachedCommand` to load more items as the user scrolls toward the end of the list:

```xaml
<CollectionView ItemsSource="{Binding Items}"
                RemainingItemsThreshold="5"
                RemainingItemsThresholdReachedCommand="{Binding LoadMoreCommand}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Item">
            <Label Text="{Binding Title}" Padding="10" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

When fewer than 5 items remain below the visible area, `LoadMoreCommand` executes. In your view model:

```csharp
[RelayCommand]
private async Task LoadMoreAsync()
{
    var nextPage = await dataService.GetItemsAsync(currentPage + 1);
    foreach (var item in nextPage)
    {
        Items.Add(item);
    }
    currentPage++;
}
```

## Layout options

`CollectionView` supports four layout modes controlled by the `ItemsLayout` property.

### Vertical list (default)

Items are arranged in a single-column vertical list. This is the default when no `ItemsLayout` is specified.

```xaml
<CollectionView ItemsSource="{Binding Items}"
                ItemsLayout="VerticalList" />
```

```csharp
var collectionView = new CollectionView
{
    ItemsSource = viewModel.Items,
    ItemsLayout = LinearItemsLayout.Vertical
};
```

### Horizontal list

Items are arranged in a single-row horizontal list.

```xaml
<CollectionView ItemsSource="{Binding Items}"
                ItemsLayout="HorizontalList" />
```

```csharp
var collectionView = new CollectionView
{
    ItemsSource = viewModel.Items,
    ItemsLayout = LinearItemsLayout.Horizontal
};
```

### Vertical grid

Items are arranged in a multi-column vertical grid. Set `Span` to control the number of columns.

```xaml
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Vertical"
                         Span="2"
                         VerticalItemSpacing="5"
                         HorizontalItemSpacing="5" />
    </CollectionView.ItemsLayout>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Item">
            <Frame Padding="10" Margin="5">
                <Label Text="{Binding Title}" />
            </Frame>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

```csharp
var collectionView = new CollectionView
{
    ItemsSource = viewModel.Items,
    ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
    {
        VerticalItemSpacing = 5,
        HorizontalItemSpacing = 5
    }
};
```

### Horizontal grid

Items are arranged in a multi-row horizontal grid. Set `Span` to control the number of rows.

```xaml
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Horizontal" Span="3" />
    </CollectionView.ItemsLayout>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Item">
            <Frame Padding="10" Margin="5" WidthRequest="150">
                <Label Text="{Binding Title}" />
            </Frame>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

```csharp
var collectionView = new CollectionView
{
    ItemsSource = viewModel.Items,
    ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Horizontal)
};
```

### Item sizing strategy

The `ItemSizingStrategy` property controls how items are measured:

| Strategy | Description | When to use |
|---|---|---|
| `MeasureAllItems` | Measures every item individually. This is the default. | Variable-height items. |
| `MeasureFirstItem` | Measures only the first item and applies its size to all items. | Uniform-height items — provides better scroll performance. |

```xaml
<CollectionView ItemsSource="{Binding Items}"
                ItemSizingStrategy="MeasureFirstItem" />
```

```csharp
collectionView.ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
```

## Headers and footers

`CollectionView` supports headers and footers via the `Header`, `Footer`, `HeaderTemplate`, and `FooterTemplate` properties.

### As a string

```xaml
<CollectionView ItemsSource="{Binding Items}"
                Header="Monkeys of the World"
                Footer="End of list" />
```

### As a View

```xaml
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.Header>
        <StackLayout BackgroundColor="LightGray" Padding="10">
            <Label Text="Monkeys" FontSize="18" FontAttributes="Bold" />
            <Label Text="A collection of primates" FontSize="12" />
        </StackLayout>
    </CollectionView.Header>
    <CollectionView.Footer>
        <Label Text="© 2025 Monkey Database" Padding="10" HorizontalTextAlignment="Center" />
    </CollectionView.Footer>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Label Text="{Binding Name}" Padding="10" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

### As a DataTemplate

Use `HeaderTemplate` or `FooterTemplate` when the header or footer content is data-bound:

```xaml
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.HeaderTemplate>
        <DataTemplate>
            <Label Text="{Binding HeaderText}" FontSize="18" FontAttributes="Bold" Padding="10" />
        </DataTemplate>
    </CollectionView.HeaderTemplate>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Item">
            <Label Text="{Binding Title}" Padding="10" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

In C#:

```csharp
collectionView.Header = "Monkeys of the World";
collectionView.Footer = new Label
{
    Text = "End of list",
    Padding = 10,
    HorizontalTextAlignment = TextAlignment.Center
};
```

## Selection

`CollectionView` provides built-in selection support through the `SelectionMode` property.

### SelectionMode

| Mode | Description | Properties |
|---|---|---|
| `None` | No item can be selected. This is the default. | — |
| `Single` | One item at a time can be selected. | `SelectedItem` |
| `Multiple` | Multiple items can be selected. | `SelectedItems` |

### Single selection

Bind `SelectedItem` with `Mode=TwoWay` to keep the view model in sync:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Single"
                SelectedItem="{Binding SelectedMonkey, Mode=TwoWay}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Label Text="{Binding Name}" Padding="10" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

```csharp
var collectionView = new CollectionView
{
    SelectionMode = SelectionMode.Single
};
collectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ViewModel.Monkeys));
collectionView.SetBinding(CollectionView.SelectedItemProperty, nameof(ViewModel.SelectedMonkey), BindingMode.TwoWay);
```

### SelectionChanged event and command

Handle selection changes with the `SelectionChanged` event or `SelectionChangedCommand`:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Single"
                SelectionChangedCommand="{Binding MonkeySelectedCommand}"
                SelectionChangedCommandParameter="{Binding SelectedItem, Source={RelativeSource Self}}">
```

Or in code-behind:

```csharp
collectionView.SelectionChanged += (sender, args) =>
{
    var selected = args.CurrentSelection.FirstOrDefault() as Monkey;
    if (selected != null)
    {
        // Navigate or update details
    }
};
```

### Visual feedback for selected items

Use `VisualStateManager` with the `Selected` visual state to style selected items:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Single">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Grid Padding="10">
                <VisualStateManager.VisualStateGroups>
                    <VisualStateGroup Name="CommonStates">
                        <VisualState Name="Normal" />
                        <VisualState Name="Selected">
                            <VisualState.Setters>
                                <Setter Property="BackgroundColor"
                                        Value="{AppThemeBinding Light=LightSkyBlue, Dark=DarkSlateBlue}" />
                            </VisualState.Setters>
                        </VisualState>
                    </VisualStateGroup>
                </VisualStateManager.VisualStateGroups>
                <Label Text="{Binding Name}" />
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

To programmatically clear the selection, set `SelectedItem` to `null` or call `SelectedItems.Clear()`.

## Empty views

When `ItemsSource` is empty or `null`, `CollectionView` can display an empty state using the `EmptyView` property.

### As a string

```xaml
<CollectionView ItemsSource="{Binding FilteredItems}"
                EmptyView="No results found." />
```

### As a custom View

```xaml
<CollectionView ItemsSource="{Binding FilteredItems}">
    <CollectionView.EmptyView>
        <VerticalStackLayout HorizontalOptions="Center" VerticalOptions="Center">
            <Image Source="no_results.png" HeightRequest="100" />
            <Label Text="No items match your search"
                   FontSize="16"
                   HorizontalTextAlignment="Center" />
        </VerticalStackLayout>
    </CollectionView.EmptyView>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Item">
            <Label Text="{Binding Title}" Padding="10" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

### EmptyViewTemplate

Use `EmptyViewTemplate` for dynamic empty states that are data-bound:

```xaml
<CollectionView ItemsSource="{Binding FilteredItems}"
                EmptyView="{Binding EmptyState}">
    <CollectionView.EmptyViewTemplate>
        <DataTemplate x:DataType="vm:EmptyStateModel">
            <VerticalStackLayout HorizontalOptions="Center" VerticalOptions="Center" Spacing="10">
                <Image Source="{Binding ImageSource}" HeightRequest="100" />
                <Label Text="{Binding Message}" FontSize="16" HorizontalTextAlignment="Center" />
                <Button Text="{Binding ActionText}" Command="{Binding ActionCommand}" />
            </VerticalStackLayout>
        </DataTemplate>
    </CollectionView.EmptyViewTemplate>
</CollectionView>
```

In C#:

```csharp
collectionView.EmptyView = "No results found.";
```

## Grouping

`CollectionView` can display grouped data by setting `IsGrouped="true"` and providing `GroupHeaderTemplate` and optionally `GroupFooterTemplate`.

### Step 1: Define a group model

Create a group class that extends `List<T>`:

```csharp
public class AnimalGroup : List<Animal>
{
    public string Name { get; }
    public int Count => this.Count();

    public AnimalGroup(string name, List<Animal> animals)
        : base(animals)
    {
        Name = name;
    }
}
```

### Step 2: Create grouped data in the view model

```csharp
public ObservableCollection<AnimalGroup> GroupedAnimals { get; } = new()
{
    new AnimalGroup("Primates", new List<Animal>
    {
        new Animal { Name = "Capuchin" },
        new Animal { Name = "Gorilla" }
    }),
    new AnimalGroup("Felines", new List<Animal>
    {
        new Animal { Name = "Lion" },
        new Animal { Name = "Tiger" }
    })
};
```

### Step 3: Configure CollectionView

```xaml
<CollectionView ItemsSource="{Binding GroupedAnimals}"
                IsGrouped="True">
    <CollectionView.GroupHeaderTemplate>
        <DataTemplate x:DataType="model:AnimalGroup">
            <Label Text="{Binding Name}"
                   FontAttributes="Bold"
                   FontSize="16"
                   Padding="10"
                   BackgroundColor="LightGray" />
        </DataTemplate>
    </CollectionView.GroupHeaderTemplate>
    <CollectionView.GroupFooterTemplate>
        <DataTemplate x:DataType="model:AnimalGroup">
            <Label Text="{Binding Count, StringFormat='{0} animals'}"
                   FontSize="12"
                   Padding="10,0,10,10"
                   TextColor="Gray" />
        </DataTemplate>
    </CollectionView.GroupFooterTemplate>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Animal">
            <Label Text="{Binding Name}" Padding="20,5" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

In C#:

```csharp
var collectionView = new CollectionView
{
    IsGrouped = true,
    ItemsSource = viewModel.GroupedAnimals,
    GroupHeaderTemplate = new DataTemplate(() =>
    {
        var label = new Label
        {
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            Padding = 10,
            BackgroundColor = Colors.LightGray
        };
        label.SetBinding(Label.TextProperty, nameof(AnimalGroup.Name));
        return label;
    }),
    ItemTemplate = new DataTemplate(() =>
    {
        var label = new Label { Padding = new Thickness(20, 5) };
        label.SetBinding(Label.TextProperty, nameof(Animal.Name));
        return label;
    })
};
```

## Scrolling

### ScrollTo by index

Scroll to a specific item index:

```csharp
// Scroll to the 10th item with animation
collectionView.ScrollTo(9, animate: true);

// Scroll to the 5th item in the 2nd group
collectionView.ScrollTo(4, 1, animate: true);
```

### ScrollTo by item

Scroll to a specific item object:

```csharp
collectionView.ScrollTo(selectedMonkey, animate: true);

// Scroll to an item within a group
collectionView.ScrollTo(selectedAnimal, animalGroup, animate: true);
```

The `ScrollToPosition` parameter controls where the item appears on screen:

| Value | Description |
|---|---|
| `MakeVisible` | Scrolls the minimum amount to make the item visible. This is the default. |
| `Start` | Scrolls the item to the start of the visible area. |
| `Center` | Scrolls the item to the center of the visible area. |
| `End` | Scrolls the item to the end of the visible area. |

```csharp
collectionView.ScrollTo(item, position: ScrollToPosition.Center, animate: true);
```

### ScrollBarVisibility

Control scroll bar visibility on each axis:

```xaml
<CollectionView ItemsSource="{Binding Items}"
                VerticalScrollBarVisibility="Never"
                HorizontalScrollBarVisibility="Always" />
```

### Snap points

Snap points control how items align after a scroll gesture completes.

```xaml
<CollectionView ItemsSource="{Binding Items}">
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Horizontal"
                           SnapPointsType="MandatorySingle"
                           SnapPointsAlignment="Center" />
    </CollectionView.ItemsLayout>
</CollectionView>
```

| SnapPointsType | Description |
|---|---|
| `None` | No snapping. This is the default. |
| `Mandatory` | Snaps to the nearest snap point. |
| `MandatorySingle` | Snaps to the nearest snap point, one item at a time. |

| SnapPointsAlignment | Description |
|---|---|
| `Start` | Snap points align to the leading edge of items. |
| `Center` | Snap points align to the center of items. |
| `End` | Snap points align to the trailing edge of items. |

### ItemsUpdatingScrollMode

The `ItemsUpdatingScrollMode` property controls scroll behavior when new items are added:

| Value | Description |
|---|---|
| `KeepItemsInView` | Adjusts the scroll offset to keep the first visible item in view. This is the default. |
| `KeepScrollOffset` | Maintains the current scroll offset. |
| `KeepLastItemInView` | Adjusts the scroll offset to keep the last visible item in view — useful for chat-style lists. |

```xaml
<CollectionView ItemsSource="{Binding Messages}"
                ItemsUpdatingScrollMode="KeepLastItemInView" />
```

## Context actions with SwipeView

Wrap item content in a <xref:Microsoft.Maui.Controls.SwipeView> to add swipe-based context actions such as delete or favorite:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <SwipeView>
                <SwipeView.RightItems>
                    <SwipeItems>
                        <SwipeItem Text="Delete"
                                   BackgroundColor="Red"
                                   Command="{Binding Source={RelativeSource AncestorType={x:Type vm:MonkeysViewModel}}, Path=DeleteCommand}"
                                   CommandParameter="{Binding}" />
                    </SwipeItems>
                </SwipeView.RightItems>
                <Grid Padding="10" ColumnDefinitions="Auto,*" ColumnSpacing="10">
                    <Image Source="{Binding ImageUrl}"
                           HeightRequest="60" WidthRequest="60" />
                    <Label Grid.Column="1"
                           Text="{Binding Name}"
                           VerticalOptions="Center" />
                </Grid>
            </SwipeView>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

In C#:

```csharp
var itemTemplate = new DataTemplate(() =>
{
    var swipeView = new SwipeView();

    var deleteItem = new SwipeItem
    {
        Text = "Delete",
        BackgroundColor = Colors.Red
    };
    deleteItem.SetBinding(SwipeItem.CommandProperty,
        new Binding(nameof(MonkeysViewModel.DeleteCommand),
            source: new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(MonkeysViewModel))));
    deleteItem.SetBinding(SwipeItem.CommandParameterProperty, ".");

    swipeView.RightItems = new SwipeItems { deleteItem };

    var grid = new Grid
    {
        Padding = 10,
        ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Auto),
            new ColumnDefinition(GridLength.Star)
        },
        ColumnSpacing = 10
    };

    var image = new Image { HeightRequest = 60, WidthRequest = 60 };
    image.SetBinding(Image.SourceProperty, nameof(Monkey.ImageUrl));

    var label = new Label { VerticalOptions = LayoutOptions.Center };
    label.SetBinding(Label.TextProperty, nameof(Monkey.Name));

    grid.Add(image);
    grid.Add(label, 1);

    swipeView.Content = grid;
    return swipeView;
});
```

## Performance tips

Follow these guidelines to maximize `CollectionView` performance:

1. **Always use compiled bindings** — set `x:DataType` on the `DataTemplate` to enable compile-time binding resolution.
2. **Use `MeasureFirstItem`** for uniform-height items to avoid measuring every item individually.
3. **Never place `CollectionView` in `StackLayout`** — use `Grid` or a constrained layout to preserve virtualization.
4. **Keep the `DataTemplate` visual tree flat** — prefer a single `Grid` over nested `StackLayout` containers. Fewer layout passes mean faster rendering.
5. **Use `ObservableCollection<T>`** — changes to the collection are reflected automatically without resetting the entire view.
6. **Avoid complex bindings in templates** — use simple property bindings instead of multi-bindings or converters where possible.
7. **Set a fixed `HeightRequest` or `WidthRequest`** on item templates when the size is known, to reduce layout calculations.

> [!NOTE]
> If you're migrating from `ListView`, note that `ListView.CachingStrategy` does not apply to `CollectionView`. Virtualization in `CollectionView` is always enabled when the parent layout provides constrained space.

## See also

- [CollectionView overview](index.md)
- [CollectionView data](populate-data.md)
- [CollectionView layout](layout.md)
- [CollectionView selection](selection.md)
- [CollectionView empty views](emptyview.md)
- [CollectionView scrolling](scrolling.md)
- [CollectionView grouping](grouping.md)
