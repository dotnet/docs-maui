---
title: "ListView"
description: "The .NET MAUI ListView displays a scrollable vertical list of selectable data items. ListView supports displaying headers and footers, grouped data, pull-to-refresh, and context menu items."
ms.date: 08/30/2024
---

# ListView

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-listview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ListView> displays a scrollable vertical list of selectable data items. While <xref:Microsoft.Maui.Controls.ListView> manages the appearance of the list, the appearance of each item in the list is defined by a <xref:Microsoft.Maui.Controls.DataTemplate> that uses a <xref:Microsoft.Maui.Controls.Cell> to display items. .NET MAUI includes cell types to display combinations of text and images, and you can also define custom cells that display any content you want. <xref:Microsoft.Maui.Controls.ListView> also includes support for displaying headers and footers, grouped data, pull-to-refresh, and context menu items.

The <xref:Microsoft.Maui.Controls.ListView> class derives from the `ItemsView<Cell>` class, from which it inherits the following properties:

- `ItemsSource`, of type `IEnumerable`, specifies the collection of items to be displayed, and has a default value of `null`.
- `ItemTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the template to apply to each item in the collection of items to be displayed.

<xref:Microsoft.Maui.Controls.ListView> defines the following properties:

- `Footer`, of type `object`, specifies the string or view that will be displayed at the end of the list.
- `FooterTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to use to format the `Footer`.
- `GroupHeaderTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, defines the <xref:Microsoft.Maui.Controls.DataTemplate> used to define the appearance of the header of each group. This property is mutually exclusive with the `GroupDisplayBinding` property. Therefore, setting this property will set `GroupDisplayBinding` to `null`.
- `HasUnevenRows`, of type `bool`, indicates whether items in the list can have rows of different heights. The default value of this property is `false`.
- `Header`, of type `object`, specifies the string or view that will be displayed at the start of the list.
- `HeaderTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to use to format the `Header`.
- `HorizontalScrollBarVisibility`, of type `ScrollBarVisibility`, indicates when the horizontal scroll bar will be visible.
- `IsGroupingEnabled`, of type `bool`, indicates whether the underlying data should be displayed in groups. The default value of this property is `false`.
- `IsPullToRefreshEnabled`, of type `bool`, indicates whether the user can swipe down to cause the <xref:Microsoft.Maui.Controls.ListView> to refresh its data. The default value of this property is `false`.
- `IsRefreshing`, of type `bool`, indicates whether the <xref:Microsoft.Maui.Controls.ListView> is currently refreshing. The default value of this property is `false`.
- `RefreshCommand`, of type <xref:System.Windows.Input.ICommand>, represents the command that will be executed when a refresh is triggered.
- `RefreshControlColor`, of type <xref:Microsoft.Maui.Graphics.Color>, determines the color of the refresh visualization that's shown while a refresh occurs.
- `RowHeight`, of type `int`, determines the height of each row when `HasUnevenRows` is `false`.
- `SelectedItem`, of type `object`, represents the currently selected item in the <xref:Microsoft.Maui.Controls.ListView>.
- `SelectionMode`, of type `ListViewSelectionMode`, indicates whether items can be selected in the <xref:Microsoft.Maui.Controls.ListView> or not. The default value of this property is `Single`.
- `SeparatorColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the bar that separates items in the list.
- `SeparatorVisibility`, of type `SeparatorVisibility`, defines whether separators are visible between items.
- `VerticalScrollBarVisibility`, of type `ScrollBarVisibility`, indicates when the vertical scroll bar will be visible.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

In addition, <xref:Microsoft.Maui.Controls.ListView> defines the following properties that aren't backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects:

- `GroupDisplayBinding`, of type `BindingBase`, the binding to use for displaying the group header. This property is mutually exclusive with the `GroupHeaderTemplate` property. Therefore, setting this property will set `GroupHeaderTemplate` to `null`.
- `GroupShortNameBinding`, of type `BindingBase`, the binding for the name to display in grouped jump lists.
- `CachingStrategy`, of type `ListViewCachingStrategy`, defines the cell reuse strategy of the <xref:Microsoft.Maui.Controls.ListView>. This is a read-only property.

<xref:Microsoft.Maui.Controls.ListView> defines the following events:

- `ItemAppearing`, which is raised when the visual representation of an item is being added to the visual layout of the <xref:Microsoft.Maui.Controls.ListView>. The `ItemVisibilityEventArgs` object that accompanies this event defines `Item` and `Index` properties.
- `ItemDisappearing`, which is raised when the visual representation of an item is being removed from the visual layout of the <xref:Microsoft.Maui.Controls.ListView>. The `ItemVisibilityEventArgs` object that accompanies this event defines `Item` and `Index` properties.
- `ItemSelected`, which is raised when a new item in the list is selected. The `SelectedItemChangedEventArgs` object that accompanies this event defines `SelectedItem` and `SelectedItemIndex` properties.
- `ItemTapped`, which raised when an item in the <xref:Microsoft.Maui.Controls.ListView> is tapped. The `ItemTappedEventArgs` object that accompanies this event defines `Group`, `Item`, and `ItemIndex` properties.
- `Refreshing`, which is raised when a pull to refresh operation is triggered on the <xref:Microsoft.Maui.Controls.ListView>.
- `Scrolled`, . The `ScrolledEventArgs` object that accompanies this event defines `ScrollX` and `ScrollY` properties.
- `ScrollToRequested` . The `ScrollToRequestedEventArgs` object that accompanies this event defines <xref:Microsoft.Maui.Controls.Element>, `Mode`, `Position`, `ScrollX`, `ScrollY`, and `ShouldAnimate` properties.

## Populate a ListView with data

A <xref:Microsoft.Maui.Controls.ListView> is populated with data by setting its `ItemsSource` property to any collection that implements `IEnumerable`.

> [!IMPORTANT]
> If the <xref:Microsoft.Maui.Controls.ListView> is required to refresh as items are added, removed, or changed in the underlying collection, the underlying collection should be an `IEnumerable` collection that sends property change notifications, such as `ObservableCollection`.

<xref:Microsoft.Maui.Controls.ListView> can be populated with data by using data binding to bind its `ItemsSource` property to an `IEnumerable` collection. In XAML, this is achieved with the `Binding` markup extension:

```xaml
<ListView ItemsSource="{Binding Monkeys}" />
```

The equivalent C# code is:

```csharp
ListView listView = new ListView();
listView.SetBinding(ItemsView.ItemsSourceProperty,  static (MonkeysViewModel vm) => vm.Monkeys);
```

In this example, the `ItemsSource` property data binds to the `Monkeys` property of the connected viewmodel.

> [!NOTE]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).

For more information about data binding, see [Data binding](~/fundamentals/data-binding/index.md).

## Define item appearance

The appearance of each item in the <xref:Microsoft.Maui.Controls.ListView> can be defined by setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<ListView ItemsSource="{Binding Monkeys}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid Padding="10">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto" />
                        <RowDefinition Height="Auto" />
                    </Grid.RowDefinitions>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto" />
                        <ColumnDefinition Width="Auto" />
                    </Grid.ColumnDefinitions>
                    <Image Grid.RowSpan="2"
                           Source="{Binding ImageUrl}"
                           Aspect="AspectFill"
                           HeightRequest="60"
                           WidthRequest="60" />
                    <Label Grid.Column="1"
                           Text="{Binding Name}"
                           FontAttributes="Bold" />
                    <Label Grid.Row="1"
                           Grid.Column="1"
                           Text="{Binding Location}"
                           FontAttributes="Italic"
                           VerticalOptions="End" />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The elements specified in the <xref:Microsoft.Maui.Controls.DataTemplate> define the appearance of each item in the list, and the child of the <xref:Microsoft.Maui.Controls.DataTemplate> must be a <xref:Microsoft.Maui.Controls.Cell> object. In the example, layout within the <xref:Microsoft.Maui.Controls.DataTemplate> is managed by a <xref:Microsoft.Maui.Controls.Grid>. The <xref:Microsoft.Maui.Controls.Grid> contains an <xref:Microsoft.Maui.Controls.Image> object, and two <xref:Microsoft.Maui.Controls.Label> objects, that all bind to properties of the `Monkey` class:

```csharp
public class Monkey
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Details { get; set; }
    public string ImageUrl { get; set; }
}
```

The following screenshot shows the result of templating each item in the list:

:::image type="content" source="media/listview/datatemplate.png" alt-text="Screenshot of ListView where each item is templated.":::

For more information about data templates, see [Data templates](~/fundamentals/datatemplate.md).

### Cells

The appearance of each item in a <xref:Microsoft.Maui.Controls.ListView> is defined by a <xref:Microsoft.Maui.Controls.DataTemplate>, and the <xref:Microsoft.Maui.Controls.DataTemplate> must reference a <xref:Microsoft.Maui.Controls.Cell> class to display items. Each cell represents an item of data in the <xref:Microsoft.Maui.Controls.ListView>. .NET MAUI includes the following built-in cells:

- <xref:Microsoft.Maui.Controls.TextCell>, which displays primary and secondary text on separate lines.
- <xref:Microsoft.Maui.Controls.ImageCell>, which displays an image with primary and secondary text on separate lines.
- <xref:Microsoft.Maui.Controls.SwitchCell>, which displays text and a switch that can be switched on or off.
- <xref:Microsoft.Maui.Controls.EntryCell>, which displays a label and text that's editable.
- <xref:Microsoft.Maui.Controls.ViewCell>, which is a custom cell whose appearance is defined by a <xref:Microsoft.Maui.Controls.View>. This cell type should be used when you want to fully define the appearance of each item in a <xref:Microsoft.Maui.Controls.ListView>.

Typically, <xref:Microsoft.Maui.Controls.SwitchCell> and <xref:Microsoft.Maui.Controls.EntryCell> will only be used in a <xref:Microsoft.Maui.Controls.TableView> and won't be used in a <xref:Microsoft.Maui.Controls.ListView>. For more information about <xref:Microsoft.Maui.Controls.SwitchCell> and <xref:Microsoft.Maui.Controls.EntryCell>, see [TableView](tableview.md).

#### Text cell

A <xref:Microsoft.Maui.Controls.TextCell> displays primary and secondary text on separate lines. <xref:Microsoft.Maui.Controls.TextCell> defines the following properties:

- `Text`, of type `string`, defines the primary text to be displayed.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, represents the color of the primary text.
- `Detail`, of type `string`, defines the secondary text to be displayed.
- `DetailColor`, of type <xref:Microsoft.Maui.Graphics.Color>, indicates the color of the secondary text.
- `Command`, of type <xref:System.Windows.Input.ICommand>, defines the command that's executed when the cell is tapped.
- `CommandParameter`, of type `object`, represents the parameter that's passed to the command.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The following example shows using a <xref:Microsoft.Maui.Controls.TextCell> to define the appearance of items in a <xref:Microsoft.Maui.Controls.ListView>:

```xaml
<ListView ItemsSource="{Binding Food}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <TextCell Text="{Binding Name}"
                      Detail="{Binding Description}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The following screenshot shows the resulting cell appearance:

:::image type="content" source="media/listview/textcell.png" alt-text="Screenshot of ListView where each item is a TextCell.":::

#### Image cell

An <xref:Microsoft.Maui.Controls.ImageCell> displays an image with primary and secondary text on separate lines. <xref:Microsoft.Maui.Controls.ImageCell> inherits the properties from <xref:Microsoft.Maui.Controls.TextCell>, and defines the <xref:Microsoft.Maui.Controls.ImageSource> property, of type <xref:Microsoft.Maui.Controls.ImageSource>, which specifies the image to be displayed in the cell. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means it can be the target of data bindings, and be styled.

The following example shows using an <xref:Microsoft.Maui.Controls.ImageCell> to define the appearance of items in a <xref:Microsoft.Maui.Controls.ListView>:

```xaml
<ListView ItemsSource="{Binding Food}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ImageCell ImageSource="{Binding Image}"
                       Text="{Binding Name}"
                       Detail="{Binding Description}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The following screenshot shows the resulting cell appearance:

:::image type="content" source="media/listview/imagecell.png" alt-text="Screenshot of ListView where each item is an ImageCell.":::

#### View cell

A <xref:Microsoft.Maui.Controls.ViewCell> is a custom cell whose appearance is defined by a <xref:Microsoft.Maui.Controls.View>. <xref:Microsoft.Maui.Controls.ViewCell> defines a <xref:Microsoft.Maui.Controls.View> property, of type <xref:Microsoft.Maui.Controls.View>, which defines the view that represents the content of the cell. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means it can be the target of data bindings, and be styled.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.View> property is the content property of the <xref:Microsoft.Maui.Controls.ViewCell> class, and therefore does not need to be explicitly set from XAML.

The following example shows using a <xref:Microsoft.Maui.Controls.ViewCell> to define the appearance of items in a <xref:Microsoft.Maui.Controls.ListView>:

```xaml
<ListView ItemsSource="{Binding Monkeys}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid Padding="10">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto" />
                        <RowDefinition Height="Auto" />
                    </Grid.RowDefinitions>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto" />
                        <ColumnDefinition Width="Auto" />
                    </Grid.ColumnDefinitions>
                    <Image Grid.RowSpan="2"
                           Source="{Binding ImageUrl}"
                           Aspect="AspectFill"
                           HeightRequest="60"
                           WidthRequest="60" />
                    <Label Grid.Column="1"
                           Text="{Binding Name}"
                           FontAttributes="Bold" />
                    <Label Grid.Row="1"
                           Grid.Column="1"
                           Text="{Binding Location}"
                           FontAttributes="Italic"
                           VerticalOptions="End" />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

Inside the <xref:Microsoft.Maui.Controls.ViewCell>, layout can be managed by any .NET MAUI layout. In this example, layout is managed by a <xref:Microsoft.Maui.Controls.Grid>. The <xref:Microsoft.Maui.Controls.Grid> contains an <xref:Microsoft.Maui.Controls.Image> object, and two <xref:Microsoft.Maui.Controls.Label> objects, that all bind to properties of the `Monkey` class.

The following screenshot shows the result of templating each item in the list:

:::image type="content" source="media/listview/datatemplate.png" alt-text="Screenshot of ListView where each item is templated with a ViewCell.":::

## Choose item appearance at runtime

The appearance of each item in the <xref:Microsoft.Maui.Controls.ListView> can be chosen at runtime, based on the item value, by setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplateSelector> object:

```xaml
<ContentPage ...
             xmlns:templates="clr-namespace:ListViewDemos.Templates">
    <ContentPage.Resources>
        <DataTemplate x:Key="AmericanMonkeyTemplate">
            <ViewCell>
                ...
            </ViewCell>
        </DataTemplate>

        <DataTemplate x:Key="OtherMonkeyTemplate">
            <ViewCell>
                ...
            </ViewCell>
        </DataTemplate>

        <templates:MonkeyDataTemplateSelector x:Key="MonkeySelector"
                                             AmericanMonkey="{StaticResource AmericanMonkeyTemplate}"
                                             OtherMonkey="{StaticResource OtherMonkeyTemplate}" />
    </ContentPage.Resources>

    <Grid Margin="20">
        <ListView ItemsSource="{Binding Monkeys}"
                  ItemTemplate="{StaticResource MonkeySelector}" />
    </Grid>
</ContentPage>
```

The `ItemTemplate` property is set to a `MonkeyDataTemplateSelector` object. The following example shows the `MonkeyDataTemplateSelector` class:

```csharp
public class MonkeyDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate AmericanMonkey { get; set; }
    public DataTemplate OtherMonkey { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((Monkey)item).Location.Contains("America") ? AmericanMonkey : OtherMonkey;
    }
}
```

The `MonkeyDataTemplateSelector` class defines `AmericanMonkey` and `OtherMonkey` <xref:Microsoft.Maui.Controls.DataTemplate> properties that are set to different data templates. The `OnSelectTemplate` override returns the `AmericanMonkey` template, which displays the monkey name and location in teal, when the monkey name contains "America". When the monkey name doesn't contain "America", the `OnSelectTemplate` override returns the `OtherMonkey` template, which displays the monkey name and location in silver:

:::image type="content" source="media/listview/datatemplateselector.png" alt-text="Screenshot of ListView runtime item template selection.":::

For more information about data template selectors, see [Create a DataTemplateSelector](~/fundamentals/datatemplate.md#create-a-datatemplateselector).

## Respond to item selection

By default, <xref:Microsoft.Maui.Controls.ListView> selection is enabled. However, this behavior can be changed by setting the `SelectionMode` property. The `ListViewSelectionMode` enumeration defines the following members:

- `None` – indicates that items cannot be selected.
- `Single` – indicates that a single item can be selected, with the selected item being highlighted. This is the default value.

<xref:Microsoft.Maui.Controls.ListView> defines an `ItemSelected` event that's raised when the `SelectedItem` property changes, either due to the user selecting an item from the list, or when an app sets the property. The `SelectedItemChangedEventArgs` object that accompanies this event has `SelectedItem` and `SelectedItemIndex` properties.

When the `SelectionMode` property is set to `Single`, a single item in the <xref:Microsoft.Maui.Controls.ListView> can be selected. When an item is selected, the `SelectedItem` property will be set to the value of the selected item. When this property changes, the `ItemSelected` event is raised.

The following example shows a <xref:Microsoft.Maui.Controls.ListView> that can respond to single item selection:

```xaml
<ListView ItemsSource="{Binding Monkeys}"
          ItemSelected="OnItemSelected">
    ...
</ListView>
```

In this example, the `OnItemSelected` event handler is executed when the `ItemSelected` event fires, with the event handler retrieving the selected item:

```csharp
void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
{
    Monkey item = args.SelectedItem as Monkey;
}
```

The following screenshot shows single item selection in a <xref:Microsoft.Maui.Controls.ListView>:

:::image type="content" source="media/listview/selection.png" alt-text="Screenshot of a ListView with a selection.":::

### Clear the selection

The `SelectedItem` property can be cleared by setting it, or the object it binds to, to `null`.

### Disable selection

<xref:Microsoft.Maui.Controls.ListView> selection is enabled by default. However, it can be disabled by setting the `SelectionMode` property to `None`:

```xaml
<ListView ...
          SelectionMode="None" />
```

When the `SelectionMode` property is set to `None`, items in the <xref:Microsoft.Maui.Controls.ListView> cannot be selected, the `SelectedItem` property will remain `null`, and the `ItemSelected` event will not be fired.

## Cache data

<xref:Microsoft.Maui.Controls.ListView> is a powerful view for displaying data, but it has some limitations. Scrolling performance can suffer when using custom cells, especially when they contain deeply nested view hierarchies or use certain layouts that require complex measurement. Fortunately, there are techniques you can use to avoid poor performance.

A <xref:Microsoft.Maui.Controls.ListView> is often used to display much more data than fits onscreen. For example, a music app might have a library of songs with thousands of entries. Creating an item for every entry would waste valuable memory and perform poorly. Creating and destroying rows constantly would require the app to instantiate and cleanup objects constantly, which would also perform poorly.

To conserve memory, the native <xref:Microsoft.Maui.Controls.ListView> equivalents for each platform have built-in features for reusing rows. Only the cells visible on screen are loaded in memory and the content is loaded into existing cells. This pattern prevents the app from instantiating thousands of objects, saving time and memory.

.NET MAUI permits <xref:Microsoft.Maui.Controls.ListView> cell reuse through the `ListViewCachingStrategy` enumeration, which defines the following members:

- `RetainElement`, specifies that the <xref:Microsoft.Maui.Controls.ListView> will generate a cell for each item in the list.
- `RecycleElement`, specifies that the <xref:Microsoft.Maui.Controls.ListView> will attempt to minimize its memory footprint and execution speed by recycling list cells.
- `RecycleElementAndDataTemplate`, as `RecycleElement` while also ensuring that when a <xref:Microsoft.Maui.Controls.ListView> uses a <xref:Microsoft.Maui.Controls.DataTemplateSelector>, <xref:Microsoft.Maui.Controls.DataTemplate> objects are cached by the type of item in the list.

<!-- > [!NOTE]
> Windows ignores the `RetainElement` caching strategy, because it always uses caching to improve performance. Therefore, by default it behaves as if the `RecycleElement` caching strategy is applied. -->

### Retain elements

The `RetainElement` caching strategy specifies that the <xref:Microsoft.Maui.Controls.ListView> will generate a cell for each item in the list, and is the default <xref:Microsoft.Maui.Controls.ListView> behavior. It should be used in the following circumstances:

- Each cell has a large number of bindings (20-30+).
- The cell template changes frequently.
- Testing reveals that the `RecycleElement` caching strategy results in a reduced execution speed.

It's important to recognize the consequences of the `RetainElement` caching strategy when working with custom cells. Any cell initialization code will need to run for each cell creation, which may be multiple times per second. In this circumstance, layout techniques that were fine on a page, like using multiple nested <xref:Microsoft.Maui.Controls.Grid> objects, become performance bottlenecks when they're set up and destroyed in real time as the user scrolls.

### Recycle elements

The `RecycleElement` caching strategy specifies that the <xref:Microsoft.Maui.Controls.ListView> will attempt to minimize its memory footprint and execution speed by recycling list cells. This mode doesn't always offer a performance improvement, and testing should be performed to determine any improvements. However, it's the preferred choice, and should be used in the following circumstances:

- Each cell has a small to moderate number of bindings.
- Each cell's `BindingContext` defines all of the cell data.
- Each cell is largely similar, with the cell template unchanging.

During virtualization the cell will have its binding context updated, and so if an app uses this mode it must ensure that binding context updates are handled appropriately. All data about the cell must come from the binding context or consistency errors may occur. This problem can be avoided by using data binding to display cell data. Alternatively, cell data should be set in the `OnBindingContextChanged` override, rather than in the custom cell's constructor, as shown in the following example:

```csharp
public class CustomCell : ViewCell
{
    Image image = null;

    public CustomCell()
    {
        image = new Image();
        View = image;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        var item = BindingContext as ImageItem;
        if (item != null)
        {
            image.Source = item.ImageUrl;
        }
    }
}
```

<!-- On iOS and Android, if cells use custom renderers, they must ensure that property change notification is correctly implemented. When cells are reused their property values will change when the binding context is updated to that of an available cell, with `PropertyChanged` events being raised. -->

#### Recycle elements with a DataTemplateSelector

When a <xref:Microsoft.Maui.Controls.ListView> uses a <xref:Microsoft.Maui.Controls.DataTemplateSelector> to select a <xref:Microsoft.Maui.Controls.DataTemplate>, the `RecycleElement` caching strategy does not cache <xref:Microsoft.Maui.Controls.DataTemplate> objects. Instead, a <xref:Microsoft.Maui.Controls.DataTemplate> is selected for each item of data in the list.

> [!NOTE]
> The `RecycleElement` caching strategy requires that when a <xref:Microsoft.Maui.Controls.DataTemplateSelector> is asked to select a <xref:Microsoft.Maui.Controls.DataTemplate> that each <xref:Microsoft.Maui.Controls.DataTemplate> must return the same <xref:Microsoft.Maui.Controls.ViewCell> type. For example, given a <xref:Microsoft.Maui.Controls.ListView> with a <xref:Microsoft.Maui.Controls.DataTemplateSelector> that can return either `MyDataTemplateA` (where `MyDataTemplateA` returns a <xref:Microsoft.Maui.Controls.ViewCell> of type `MyViewCellA`), or `MyDataTemplateB` (where `MyDataTemplateB` returns a <xref:Microsoft.Maui.Controls.ViewCell> of type `MyViewCellB`), when `MyDataTemplateA` is returned it must return `MyViewCellA` or an exception will be thrown.

### Recycle elements with DataTemplates

The `RecycleElementAndDataTemplate` caching strategy builds on the `RecycleElement` caching strategy by additionally ensuring that when a <xref:Microsoft.Maui.Controls.ListView> uses a <xref:Microsoft.Maui.Controls.DataTemplateSelector> to select a <xref:Microsoft.Maui.Controls.DataTemplate>, <xref:Microsoft.Maui.Controls.DataTemplate> objects are cached by the type of item in the list. Therefore, <xref:Microsoft.Maui.Controls.DataTemplate> objects are selected once per item type, instead of once per item instance.

> [!NOTE]
> The `RecycleElementAndDataTemplate` caching strategy requires that <xref:Microsoft.Maui.Controls.DataTemplate> objects returned by the <xref:Microsoft.Maui.Controls.DataTemplateSelector> must use the <xref:Microsoft.Maui.Controls.DataTemplate> constructor that takes a `Type`.

### Set the caching strategy

The <xref:Microsoft.Maui.Controls.ListView> caching strategy can be defined by in XAML by setting the `CachingStrategy` attribute:

```xaml
<ListView CachingStrategy="RecycleElement">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
              ...
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

In C#, the caching strategy is set via a constructor overload:

```csharp
ListView listView = new ListView(ListViewCachingStrategy.RecycleElement);
```

#### Set the caching strategy in a subclassed ListView

Setting the `CachingStrategy` attribute from XAML on a subclassed <xref:Microsoft.Maui.Controls.ListView> will not produce the desired behavior, because there's no `CachingStrategy` property on <xref:Microsoft.Maui.Controls.ListView>. The solution to this issue is to specify a constructor on the subclassed <xref:Microsoft.Maui.Controls.ListView> that accepts a `ListViewCachingStrategy` parameter and passes it to the base class:

```csharp
public class CustomListView : ListView
{
    public CustomListView (ListViewCachingStrategy strategy) : base (strategy)
    {
    }
    ...
}
```

Then the `ListViewCachingStrategy` enumeration value can be specified from XAML by using the `x:Arguments` attribute:

```xaml
<local:CustomListView>
    <x:Arguments>
        <ListViewCachingStrategy>RecycleElement</ListViewCachingStrategy>
    </x:Arguments>
</local:CustomListView>
```

## Headers and footers

<xref:Microsoft.Maui.Controls.ListView> can present a header and footer that scroll with the items in the list. The header and footer can be strings, views, or <xref:Microsoft.Maui.Controls.DataTemplate> objects.

<xref:Microsoft.Maui.Controls.ListView> defines the following properties for specifying the header and footer:

- `Header`, of type `object`, specifies the string, binding, or view that will be displayed at the start of the list.
- `HeaderTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to use to format the `Header`.
- `Footer`, of type `object`, specifies the string, binding, or view that will be displayed at the end of the list.
- `FooterTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the <xref:Microsoft.Maui.Controls.DataTemplate> to use to format the `Footer`.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings.

### Display strings in the header and footer

The `Header` and `Footer` properties can be set to `string` values, as shown in the following example:

```xaml
<ListView ItemsSource="{Binding Monkeys}"
          Header="Monkeys"
          Footer="2022">
    ...
</ListView>
```

The following screenshot shows the resulting header:

:::image type="content" source="media/listview/header-text.png" alt-text="Screenshot of a ListView string header.":::

### Display views in the header and footer

The `Header` and `Footer` properties can each be set to a view. This can be a single view, or a view that contains multiple child views. The following example shows the `Header` and `Footer` properties each set to a <xref:Microsoft.Maui.Controls.Grid> object that contains a <xref:Microsoft.Maui.Controls.Label> object:

```xaml
<ListView ItemsSource="{Binding Monkeys}">
    <ListView.Header>
        <Grid BackgroundColor="LightGray">
            <Label Margin="10,0,0,0"
                   Text="Monkeys"
                   FontSize="12"
                   FontAttributes="Bold" />
        </Grid>
    </ListView.Header>
    <ListView.Footer>
        <Grid BackgroundColor="LightGray">
            <Label Margin="10,0,0,0"
                   Text="Friends of Monkey"
                   FontSize="12"
                   FontAttributes="Bold" />
        </Grid>
    </ListView.Footer>
    ...
</ListView>
```

The following screenshot shows the resulting header:

:::image type="content" source="media/listview/header-view.png" alt-text="Screenshot of CollectionView header and footer using views.":::

### Display a templated header and footer

The `HeaderTemplate` and `FooterTemplate` properties can be set to <xref:Microsoft.Maui.Controls.DataTemplate> objects that are used to format the header and footer. In this scenario, the `Header` and `Footer` properties must bind to the current source for the templates to be applied, as shown in the following example:

```xaml
<ListView ItemsSource="{Binding Monkeys}"
          Header="{Binding .}"
          Footer="{Binding .}">
    <ListView.HeaderTemplate>
        <DataTemplate>
            <Grid BackgroundColor="LightGray">
                <Label Margin="10,0,0,0"
                       Text="Monkeys"
                       FontSize="12"
                       FontAttributes="Bold" />
            </Grid>
        </DataTemplate>
    </ListView.HeaderTemplate>
    <ListView.FooterTemplate>
        <DataTemplate>
            <Grid BackgroundColor="LightGray">
                <Label Margin="10,0,0,0"
                       Text="Friends of Monkey"
                       FontSize="12"
                       FontAttributes="Bold" />
            </Grid>
        </DataTemplate>
    </ListView.FooterTemplate>
    ...
</ListView>
```

## Control item separators

By default, separators are displayed between <xref:Microsoft.Maui.Controls.ListView> items on iOS and Android. This behavior can be changed by setting the `SeparatorVisibility` property, of type `SeparatorVisibility`, to `None`:

```xaml
<ListView ...
          SeparatorVisibility="None" />
```

In addition, when the separator is enabled, it's color can be set with the `SeparatorColor` property:

```xaml
<ListView ...
          SeparatorColor="Blue" />
```

## Size items

By default, all items in a <xref:Microsoft.Maui.Controls.ListView> have the same height, which is derived from the contents of the <xref:Microsoft.Maui.Controls.DataTemplate> that defines the appearance of each item. However, this behavior can be changed with the `HasUnevenRows` and `RowHeight` properties. By default, the `HasUnevenRows` property is `false`.

The `RowHeight` property can be set to an `int` that represents the height of each item in the <xref:Microsoft.Maui.Controls.ListView>, provided that `HasUnevenRows` is `false`. When `HasUnevenRows` is set to `true`, each item in the <xref:Microsoft.Maui.Controls.ListView> can have a different height. The height of each item will be derived from the contents of the item's <xref:Microsoft.Maui.Controls.DataTemplate>, and so each item will be sized to its content.

Individual <xref:Microsoft.Maui.Controls.ListView> items can be programmatically resized at runtime by changing layout related properties of elements within the <xref:Microsoft.Maui.Controls.DataTemplate>, provided that the `HasUnevenRows` property is `true`. The following example changes the height of an <xref:Microsoft.Maui.Controls.Image> object when it's tapped:

```csharp
void OnImageTapped(object sender, EventArgs args)
{
    Image image = sender as Image;
    ViewCell viewCell = image.Parent.Parent as ViewCell;

    if (image.HeightRequest < 250)
    {
      image.HeightRequest = image.Height + 100;
      viewCell.ForceUpdateSize();
    }
}
```

In this example, the `OnImageTapped` event handler is executed in response to an <xref:Microsoft.Maui.Controls.Image> object being tapped. The event handler updates the height of the <xref:Microsoft.Maui.Controls.Image> and the `Cell.ForceUpdateSize` method updates the cell's size, even when it isn't currently visible.

> [!WARNING]
> Overuse of dynamic item sizing can cause <xref:Microsoft.Maui.Controls.ListView> performance to degrade.

## Right-to-left layout

<xref:Microsoft.Maui.Controls.ListView> can layout its content in a right-to-left flow direction by setting its `FlowDirection` property to `RightToLeft`. However, the `FlowDirection` property should ideally be set on a page or root layout, which causes all the elements within the page, or root layout, to respond to the flow direction:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="ListViewDemos.RightToLeftListPage"
             Title="Right to left list"
             FlowDirection="RightToLeft">
    <Grid Margin="20">
        <ListView ItemsSource="{Binding Monkeys}">
            ...
        </ListView>
    </Grid>
</ContentPage>
```

The default `FlowDirection` for an element with a parent is `MatchParent`. Therefore, the <xref:Microsoft.Maui.Controls.ListView> inherits the `FlowDirection` property value from the <xref:Microsoft.Maui.Controls.Grid>, which in turn inherits the `FlowDirection` property value from the <xref:Microsoft.Maui.Controls.ContentPage>.

## Display grouped data

Large data sets can often become unwieldy when presented in a continually scrolling list. In this scenario, organizing the data into groups can improve the user experience by making it easier to navigate the data.

Data must be grouped before it can be displayed. This can be accomplished by creating a list of groups, where each group is a list of items. The list of groups should be an `IEnumerable<T>` collection, where `T` defines two pieces of data:

- A group name.
- An `IEnumerable` collection that defines the items belonging to the group.

The process for grouping data, therefore, is to:

- Create a type that models a single item.
- Create a type that models a single group of items.
- Create an `IEnumerable<T>` collection, where `T` is the type that models a single group of items. This collection is a collection of groups, which stores the grouped data.
- Add data to the `IEnumerable<T>` collection.

### Example

When grouping data, the first step is to create a type that models a single item. The following example shows the `Animal` class:

```csharp
public class Animal
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Details { get; set; }
    public string ImageUrl { get; set; }
}
```

The `Animal` class models a single item. A type that models a group of items can then be created. The following example shows the `AnimalGroup` class:

```csharp
public class AnimalGroup : List<Animal>
{
    public string Name { get; private set; }

    public AnimalGroup(string name, List<Animal> animals) : base(animals)
    {
        Name = name;
    }
}
```

The `AnimalGroup` class inherits from the `List<T>` class and adds a `Name` property that represents the group name.

An `IEnumerable<T>` collection of groups can then be created:

```csharp
public List<AnimalGroup> Animals { get; private set; } = new List<AnimalGroup>();
```

This code defines a collection named `Animals`, where each item in the collection is an `AnimalGroup` object. Each `AnimalGroup` object comprises a name, and a `List<Animal>` collection that defines the `Animal` objects in the group.

Grouped data can then be added to the `Animals` collection:

```csharp
Animals.Add(new AnimalGroup("Bears", new List<Animal>
{
    new Animal
    {
        Name = "American Black Bear",
        Location = "North America",
        Details = "Details about the bear go here.",
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/08/01_Schwarzbär.jpg"
    },
    new Animal
    {
        Name = "Asian Black Bear",
        Location = "Asia",
        Details = "Details about the bear go here.",
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b7/Ursus_thibetanus_3_%28Wroclaw_zoo%29.JPG/180px-Ursus_thibetanus_3_%28Wroclaw_zoo%29.JPG"
    },
    // ...
}));

Animals.Add(new AnimalGroup("Monkeys", new List<Animal>
{
    new Animal
    {
        Name = "Baboon",
        Location = "Africa & Asia",
        Details = "Details about the monkey go here.",
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg"
    },
    new Animal
    {
        Name = "Capuchin Monkey",
        Location = "Central & South America",
        Details = "Details about the monkey go here.",
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg"
    },
    new Animal
    {
        Name = "Blue Monkey",
        Location = "Central and East Africa",
        Details = "Details about the monkey go here.",
        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg"
    },
    // ...
}));
```

This code creates two groups in the `Animals` collection. The first `AnimalGroup` is named `Bears`, and contains a `List<Animal>` collection of bear details. The second `AnimalGroup` is named `Monkeys`, and contains a `List<Animal>` collection of monkey details.

<xref:Microsoft.Maui.Controls.ListView> will display grouped data, provided that the data has been grouped correctly, by setting the `IsGroupingEnabled` property to `true`:

```xaml
<ListView ItemsSource="{Binding Animals}"
          IsGroupingEnabled="True">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid Padding="10">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto" />
                        <RowDefinition Height="Auto" />
                    </Grid.RowDefinitions>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto" />
                        <ColumnDefinition Width="Auto" />
                    </Grid.ColumnDefinitions>
                    <Image Grid.RowSpan="2"
                           Source="{Binding ImageUrl}"
                           Aspect="AspectFill"
                           HeightRequest="60"
                           WidthRequest="60" />
                    <Label Grid.Column="1"
                           Text="{Binding Name}"
                           FontAttributes="Bold" />
                    <Label Grid.Row="1"
                           Grid.Column="1"
                           Text="{Binding Location}"
                           FontAttributes="Italic"
                           VerticalOptions="End" />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The equivalent C# code is:

```csharp
ListView listView = new ListView
{
    IsGroupingEnabled = true
};
listView.SetBinding(ItemsView.ItemsSourceProperty, static (AnimalsViewModel vm) => vm.Animals);
// ...
```

The appearance of each item in the <xref:Microsoft.Maui.Controls.ListView> is defined by setting its `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>. For more information, see [Define item appearance](#define-item-appearance).

The following screenshot shows the <xref:Microsoft.Maui.Controls.ListView> displaying grouped data:

:::image type="content" source="media/listview/grouped-data.png" alt-text="Screenshot of grouped data in a ListView.":::

> [!NOTE]
> By default, <xref:Microsoft.Maui.Controls.ListView> will display the group name in the group header. This behavior can be changed by customizing the group header.

### Customize the group header

The appearance of each group header can be customized by setting the `ListView.GroupHeaderTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<ListView ItemsSource="{Binding Animals}"
          IsGroupingEnabled="True">
    <ListView.GroupHeaderTemplate>
        <DataTemplate>
            <ViewCell>
                <Label Text="{Binding Name}"
                       BackgroundColor="LightGray"
                       FontSize="18"
                       FontAttributes="Bold" />
            </ViewCell>
        </DataTemplate>
    </ListView.GroupHeaderTemplate>
    ...
</ListView>
```

In this example, each group header is set to a <xref:Microsoft.Maui.Controls.Label> that displays the group name, and that has other appearance properties set. The following screenshot shows the customized group header:

:::image type="content" source="media/listview/customized-header.png" alt-text="Screenshot of a customized group header in a ListView.":::

> [!IMPORTANT]
> The `GroupHeaderTemplate` property is mutually exclusive with the `GroupDisplayBinding` property. Therefore, both properties should not be set.

### Group without templates

<xref:Microsoft.Maui.Controls.ListView> can display correctly grouped data without setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<ListView ItemsSource="{Binding Animals}"
          IsGroupingEnabled="true" />
```

In this scenario, meaningful data can be displayed by overriding the `ToString` method in the type that models a single item, and the type that models a single group of items.

## Control scrolling

<xref:Microsoft.Maui.Controls.ListView> defines two `ScrollTo` methods, that scroll items into view. One of the overloads scrolls the specified item into view, while the other scrolls the specified item in the specified group into view. Both overloads have additional arguments that allow the exact position of the item after the scroll has completed to be specified, and whether to animate the scroll.

<xref:Microsoft.Maui.Controls.ListView> defines a `ScrollToRequested` event that is fired when one of the `ScrollTo` methods is invoked. The `ScrollToRequestedEventArgs` object that accompanies the `ScrollToRequested` event has many properties, including `ShouldAnimate`, <xref:Microsoft.Maui.Controls.Element>, `Mode`, and `Position`. Some of these properties are set from the arguments specified in the `ScrollTo` method calls.

In addition, <xref:Microsoft.Maui.Controls.ListView> defines a `Scrolled` event that is fired to indicate that scrolling occurred. The `ScrolledEventArgs` object that accompanies the `Scrolled` event has `ScrollX` and `ScrollY` properties.

### Detect scrolling

<xref:Microsoft.Maui.Controls.ListView> defines a `Scrolled` event which is fired to indicate that scrolling occurred. The `ItemsViewScrolledEventArgs` class, which represents the object that accompanies the `Scrolled` event, defines the following properties:

- `ScrollX`, of type `double`, represents the X position of the scroll
- `ScrollY`, of type `double`, represents the Y position of the scroll.

The following XAML example shows a <xref:Microsoft.Maui.Controls.ListView> that sets an event handler for the `Scrolled` event:

```xaml
<ListView Scrolled="OnListViewScrolled">
    ...
</ListView>
```

The equivalent C# code is:

```csharp
ListView listView = new ListView();
listView.Scrolled += OnListViewScrolled;
```

In this code example, the `OnListViewScrolled` event handler is executed when the `Scrolled` event fires:

```csharp
void OnListViewScrolled(object sender, ScrolledEventArgs e)
{
    // Custom logic
}
```

> [!IMPORTANT]
> The `Scrolled` event is fired for user initiated scrolls, and for programmatic scrolls.

### Scroll an item into view

The `ScrollTo` method scrolls the specified item into view. Given a <xref:Microsoft.Maui.Controls.ListView> object named `listView`, the following example shows how to scroll the Proboscis Monkey item into view:

```csharp
MonkeysViewModel viewModel = BindingContext as MonkeysViewModel;
Monkey monkey = viewModel.Monkeys.FirstOrDefault(m => m.Name == "Proboscis Monkey");
listView.ScrollTo(monkey, ScrollToPosition.MakeVisible, true);
```

Alternatively, an item in grouped data can be scrolled into view by specifying the item and the group. The following example shows how to scroll the Proboscis Monkey item in the Monkeys group into view:

```csharp
GroupedAnimalsViewModel viewModel = BindingContext as GroupedAnimalsViewModel;
AnimalGroup group = viewModel.Animals.FirstOrDefault(a => a.Name == "Monkeys");
Animal monkey = group.FirstOrDefault(m => m.Name == "Proboscis Monkey");
listView.ScrollTo(monkey, group, ScrollToPosition.MakeVisible, true);
```

> [!NOTE]
> The `ScrollToRequested` event is fired when the `ScrollTo` method is invoked.

### Disable scroll animation

A scrolling animation is displayed when scrolling an item into view. However, this animation can be disabled by setting the `animated` argument of the `ScrollTo` method to `false`:

```csharp
listView.ScrollTo(monkey, position: ScrollToPosition.MakeVisible, animate: false);
```

### Control scroll position

When scrolling an item into view, the exact position of the item after the scroll has completed can be specified with the `position` argument of the `ScrollTo` methods. This argument accepts a `ScrollToPosition` enumeration member.

#### MakeVisible

The `ScrollToPosition.MakeVisible` member indicates that the item should be scrolled until it's visible in the view:

```csharp
listView.ScrollTo(monkey, position: ScrollToPosition.MakeVisible, animate: true);
```

#### Start

The `ScrollToPosition.Start` member indicates that the item should be scrolled to the start of the view:

```csharp
listView.ScrollTo(monkey, position: ScrollToPosition.Start, animate: true);
```

#### Center

The `ScrollToPosition.Center` member indicates that the item should be scrolled to the center of the view:

```csharp
listView.ScrollTo(monkey, position: ScrollToPosition.Center, animate: true);
```

#### End

The `ScrollToPosition.End` member indicates that the item should be scrolled to the end of the view:

```csharp
listView.ScrollTo(monkey, position: ScrollToPosition.End, animate: true);
```

### Scroll bar visibility

<xref:Microsoft.Maui.Controls.ListView> defines `HorizontalScrollBarVisibility` and `VerticalScrollBarVisibility` properties, which are backed by bindable properties. These properties get or set a `ScrollBarVisibility` enumeration value that represents when the horizontal, or vertical, scroll bar is visible. The `ScrollBarVisibility` enumeration defines the following members:

- `Default` indicates the default scroll bar behavior for the platform, and is the default value for the `HorizontalScrollBarVisibility` and `VerticalScrollBarVisibility` properties.
- `Always` indicates that scroll bars will be visible, even when the content fits in the view.
- `Never` indicates that scroll bars will not be visible, even if the content doesn't fit in the view.

## Add context menus

<xref:Microsoft.Maui.Controls.ListView> supports context menus items, which are defined as  <xref:Microsoft.Maui.Controls.MenuItem> objects that are added to the `ViewCell.ContextActions` collection in the <xref:Microsoft.Maui.Controls.DataTemplate> for each item:

```xaml
<ListView x:Name="listView"
          ItemsSource="{Binding Monkeys}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <ViewCell.ContextActions>
                    <MenuItem Text="Favorite"
                              Command="{Binding Source={x:Reference listView}, Path=BindingContext.FavoriteCommand}"
                              CommandParameter="{Binding}" />
                    <MenuItem Text="Delete"
                              Command="{Binding Source={x:Reference listView}, Path=BindingContext.DeleteCommand}"
                              CommandParameter="{Binding}" />
                </ViewCell.ContextActions>

                ...
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

The  <xref:Microsoft.Maui.Controls.MenuItem> objects are revealed when an item in the <xref:Microsoft.Maui.Controls.ListView> is right-clicked:

:::image type="content" source="media/listview/contextmenuitems.png" alt-text="Screenshot of CollectionView context menu items.":::

For more information about menu items, see [Display menu items](~/user-interface/menuitem.md).

## Pull to refresh

<xref:Microsoft.Maui.Controls.ListView> supports pull to refresh functionality, which enables the data being displayed to be refreshed by pulling down on the list of items.

To enable pull to refresh, set the `IsPullToRefreshEnabled` property to `true`. When a refresh is triggered, <xref:Microsoft.Maui.Controls.ListView> raises the `Refreshing` event, and the `IsRefreshing` property will be set to `true`. The code required to refresh the contents of the <xref:Microsoft.Maui.Controls.ListView> should then be executed by the handler for the `Refreshing` event, or by the <xref:System.Windows.Input.ICommand> implementation the `RefreshCommand` executes. Once the <xref:Microsoft.Maui.Controls.ListView> is refreshed, the `IsRefreshing` property should be set to `false`, or the `EndRefresh` method should be called on the <xref:Microsoft.Maui.Controls.ListView>, to indicate that the refresh is complete.

The following example shows a <xref:Microsoft.Maui.Controls.ListView> that uses pull to refresh:

```xaml
<ListView ItemsSource="{Binding Animals}"
          IsPullToRefreshEnabled="true"
          RefreshCommand="{Binding RefreshCommand}"
          IsRefreshing="{Binding IsRefreshing}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                ...
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

In this example, when the user initiates a refresh, the <xref:System.Windows.Input.ICommand> defined by the `RefreshCommand` property is executed, which should refresh the items being displayed. A refresh visualization is shown while the refresh occurs, which consists of an animated progress circle. The value of the `IsRefreshing` property indicates the current state of the refresh operation. When a refresh is triggered, this property will automatically transition to `true`. Once the refresh completes, you should reset the property to `false`.
