---
title: "Define an EmptyView for a CollectionView"
description: "In .NET MAUI CollectionView, an empty view can be specified that provides feedback to the user when no data is available for display. The empty view can be a string, a view, or multiple views."
ms.date: 01/18/2022
---

# Define an EmptyView for a CollectionView

The .NET Multi-platform App UI (.NET MAUI) `CollectionView` defines the following properties that can be used to provide user feedback when there's no data to display:

- `EmptyView`, of type `object`, the string, binding, or view that will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The default value is `null`.
- `EmptyViewTemplate`, of type `DataTemplate`, the template to use to format the specified `EmptyView`. The default value is `null`.

These properties are backed by `BindableProperty` objects, which means that the properties can be targets of data bindings.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The main usage scenarios for setting the `EmptyView` property are displaying user feedback when a filtering operation on a `CollectionView` yields no data, and displaying user feedback while data is being retrieved from a web service.

> [!NOTE]
> The `EmptyView` property can be set to a view that includes interactive content if required.

<!-- For more information about data templates, see [Data Templates](~/fundamentals/templates/data-templates/introduction.md). -->

## Display a string when data is unavailable

The `EmptyView` property can be set to a string, which will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The following XAML shows an example of this scenario:

```xaml
<CollectionView ItemsSource="{Binding EmptyMonkeys}"
                EmptyView="No items to display" />
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    EmptyView = "No items to display"
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty, "EmptyMonkeys");
```

The result is that, because the data bound collection is `null`, the string set as the `EmptyView` property value is displayed:

:::image type="content" source="media/emptyview/null-itemssource.png" alt-text="Screenshot of a CollectionView vertical list with a text empty view.":::

## Display views when data is unavailable

The `EmptyView` property can be set to a view, which will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. This can be a single view, or a view that contains multiple child views. The following XAML example shows the `EmptyView` property set to a view that contains multiple child views:

```xaml
<StackLayout Margin="20">
    <SearchBar x:Name="searchBar"
               SearchCommand="{Binding FilterCommand}"
               SearchCommandParameter="{Binding Source={x:Reference searchBar}, Path=Text}"
               Placeholder="Filter" />
    <CollectionView ItemsSource="{Binding Monkeys}">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                ...
            </DataTemplate>
        </CollectionView.ItemTemplate>
        <CollectionView.EmptyView>
            <ContentView>
                <StackLayout HorizontalOptions="CenterAndExpand"
                             VerticalOptions="CenterAndExpand">
                    <Label Text="No results matched your filter."
                           Margin="10,25,10,10"
                           FontAttributes="Bold"
                           FontSize="18"
                           HorizontalOptions="Fill"
                           HorizontalTextAlignment="Center" />
                    <Label Text="Try a broader filter?"
                           FontAttributes="Italic"
                           FontSize="12"
                           HorizontalOptions="Fill"
                           HorizontalTextAlignment="Center" />
                </StackLayout>
            </ContentView>
        </CollectionView.EmptyView>
    </CollectionView>
</StackLayout>
```

In this example, what looks like a redundant has been added as the root element of the `EmptyView`. This is because internally, the `EmptyView` is added to a native container that doesn't provide any context for .NET MAUI layout. Therefore, to position the views that comprise your `EmptyView`, you must add a root layout, whose child is a layout that can position itself within the root layout.

The equivalent C# code is:

```csharp
StackLayout stackLayout = new StackLayout();
stackLayout.Add(new Label { Text = "No results matched your filter.", ... } );
stackLayout.Add(new Label { Text = "Try a broader filter?", ... } );

SearchBar searchBar = new SearchBar { ... };
CollectionView collectionView = new CollectionView
{
    EmptyView = new ContentView
    {
        Content = stackLayout
    }
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty, "Monkeys");
```

When the `SearchBar` executes the `FilterCommand`, the collection displayed by the `CollectionView` is filtered for the search term stored in the `SearchBar.Text` property. If the filtering operation yields no data, the `StackLayout` set as the `EmptyView` property value is displayed:

:::image type="content" source="media/emptyview/filter-multiple-views.png" alt-text="Screenshot of a CollectionView vertical list with a custom empty view.":::

## Display a templated custom type when data is unavailable

The `EmptyView` property can be set to a custom type, whose template is displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The `EmptyViewTemplate` property can be set to a `DataTemplate` that defines the appearance of the `EmptyView`. The following XAML shows an example of this scenario:

```xaml
<StackLayout Margin="20">
    <SearchBar x:Name="searchBar"
               SearchCommand="{Binding FilterCommand}"
               SearchCommandParameter="{Binding Source={x:Reference searchBar}, Path=Text}"
               Placeholder="Filter" />
    <CollectionView ItemsSource="{Binding Monkeys}">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                ...
            </DataTemplate>
        </CollectionView.ItemTemplate>
        <CollectionView.EmptyView>
            <views:FilterData Filter="{Binding Source={x:Reference searchBar}, Path=Text}" />
        </CollectionView.EmptyView>
        <CollectionView.EmptyViewTemplate>
            <DataTemplate>
                <Label Text="{Binding Filter, StringFormat='Your filter term of {0} did not match any records.'}"
                       Margin="10,25,10,10"
                       FontAttributes="Bold"
                       FontSize="18"
                       HorizontalOptions="Fill"
                       HorizontalTextAlignment="Center" />
            </DataTemplate>
        </CollectionView.EmptyViewTemplate>
    </CollectionView>
</StackLayout>
```

The equivalent C# code is:

```csharp
SearchBar searchBar = new SearchBar { ... };
CollectionView collectionView = new CollectionView
{
    EmptyView = new FilterData { Filter = searchBar.Text },
    EmptyViewTemplate = new DataTemplate(() =>
    {
        return new Label { ... };
    })
};
```

The `FilterData` type defines a `Filter` property, and a corresponding `BindableProperty`:

```csharp
public class FilterData : BindableObject
{
    public static readonly BindableProperty FilterProperty = BindableProperty.Create(nameof(Filter), typeof(string), typeof(FilterData), null);

    public string Filter
    {
        get { return (string)GetValue(FilterProperty); }
        set { SetValue(FilterProperty, value); }
    }
}
```

The `EmptyView` property is set to a `FilterData` object, and the `Filter` property data binds to the `SearchBar.Text` property. When the `SearchBar` executes the `FilterCommand`, the collection displayed by the `CollectionView` is filtered for the search term stored in the `Filter` property. If the filtering operation yields no data, the `Label` defined in the `DataTemplate`, that's set as the `EmptyViewTemplate` property value, is displayed:

:::image type="content" source="media/emptyview/emptyviewtemplate.png" alt-text="Screenshot of a CollectionView vertical list with an empty view template.":::

> [!NOTE]
> When displaying a templated custom type when data is unavailable, the `EmptyViewTemplate` property can be set to a view that contains multiple child views.

## Choose an EmptyView at runtime

Views that will be displayed as an `EmptyView` when data is unavailable, can be defined as `ContentView` objects in a `ResourceDictionary`. The `EmptyView` property can then be set to a specific `ContentView`, based on some business logic, at runtime. The following XAML shows an example of this scenario:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="CollectionViewDemos.Views.EmptyViewSwapPage"
             Title="EmptyView (swap)">
    <ContentPage.Resources>
        <ContentView x:Key="BasicEmptyView">
            <StackLayout>
                <Label Text="No items to display."
                       Margin="10,25,10,10"
                       FontAttributes="Bold"
                       FontSize="18"
                       HorizontalOptions="Fill"
                       HorizontalTextAlignment="Center" />
            </StackLayout>
        </ContentView>
        <ContentView x:Key="AdvancedEmptyView">
            <StackLayout>
                <Label Text="No results matched your filter."
                       Margin="10,25,10,10"
                       FontAttributes="Bold"
                       FontSize="18"
                       HorizontalOptions="Fill"
                       HorizontalTextAlignment="Center" />
                <Label Text="Try a broader filter?"
                       FontAttributes="Italic"
                       FontSize="12"
                       HorizontalOptions="Fill"
                       HorizontalTextAlignment="Center" />
            </StackLayout>
        </ContentView>
    </ContentPage.Resources>

    <StackLayout Margin="20">
        <SearchBar x:Name="searchBar"
                   SearchCommand="{Binding FilterCommand}"
                   SearchCommandParameter="{Binding Source={x:Reference searchBar}, Path=Text}"
                   Placeholder="Filter" />
        <StackLayout Orientation="Horizontal">
            <Label Text="Toggle EmptyViews" />
            <Switch Toggled="OnEmptyViewSwitchToggled" />
        </StackLayout>
        <CollectionView x:Name="collectionView"
                        ItemsSource="{Binding Monkeys}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    ...
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```

This XAML defines two `ContentView` objects in the page-level `ResourceDictionary`, with the `Switch` object controlling which `ContentView` object will be set as the `EmptyView` property value. When the `Switch` is toggled, the `OnEmptyViewSwitchToggled` event handler executes the `ToggleEmptyView` method:

```csharp
void ToggleEmptyView(bool isToggled)
{
    collectionView.EmptyView = isToggled ? Resources["BasicEmptyView"] : Resources["AdvancedEmptyView"];
}
```

The `ToggleEmptyView` method sets the `EmptyView` property of the `collectionView` object to one of the two `ContentView` objects stored in the `ResourceDictionary`, based on the value of the `Switch.IsToggled` property. When the `SearchBar` executes the `FilterCommand`, the collection displayed by the `CollectionView` is filtered for the search term stored in the `SearchBar.Text` property. If the filtering operation yields no data, the `ContentView` object set as the `EmptyView` property is displayed:

:::image type="content" source="media/emptyview/swap.png" alt-text="Screenshot of a CollectionView vertical list with swapped empty views.":::

<!-- For more information about resource dictionaries, see [Resource Dictionaries](~/xaml/resource-dictionaries.md). -->

## Choose an EmptyViewTemplate at runtime

The appearance of the `EmptyView` can be chosen at runtime, based on its value, by setting the `CollectionView.EmptyViewTemplate` property to a `DataTemplateSelector` object:

```xaml
<ContentPage ...
             xmlns:controls="clr-namespace:CollectionViewDemos.Controls">
    <ContentPage.Resources>
        <DataTemplate x:Key="AdvancedTemplate">
            ...
        </DataTemplate>

        <DataTemplate x:Key="BasicTemplate">
            ...
        </DataTemplate>

        <controls:SearchTermDataTemplateSelector x:Key="SearchSelector"
                                                 DefaultTemplate="{StaticResource AdvancedTemplate}"
                                                 OtherTemplate="{StaticResource BasicTemplate}" />
    </ContentPage.Resources>

    <StackLayout Margin="20">
        <SearchBar x:Name="searchBar"
                   SearchCommand="{Binding FilterCommand}"
                   SearchCommandParameter="{Binding Source={x:Reference searchBar}, Path=Text}"
                   Placeholder="Filter" />
        <CollectionView ItemsSource="{Binding Monkeys}"
                        EmptyView="{Binding Source={x:Reference searchBar}, Path=Text}"
                        EmptyViewTemplate="{StaticResource SearchSelector}" />
    </StackLayout>
</ContentPage>
```

The equivalent C# code is:

```csharp
SearchBar searchBar = new SearchBar { ... };
CollectionView collectionView = new CollectionView
{
    EmptyView = searchBar.Text,
    EmptyViewTemplate = new SearchTermDataTemplateSelector { ... }
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty, "Monkeys");
```

The `EmptyView` property is set to the `SearchBar.Text` property, and the `EmptyViewTemplate` property is set to a `SearchTermDataTemplateSelector` object.

When the `SearchBar` executes the `FilterCommand`, the collection displayed by the `CollectionView` is filtered for the search term stored in the `SearchBar.Text` property. If the filtering operation yields no data, the `DataTemplate` chosen by the `SearchTermDataTemplateSelector` object is set as the `EmptyViewTemplate` property and displayed.

The following example shows the `SearchTermDataTemplateSelector` class:

```csharp
public class SearchTermDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate { get; set; }
    public DataTemplate OtherTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        string query = (string)item;
        return query.ToLower().Equals("xamarin") ? OtherTemplate : DefaultTemplate;
    }
}
```

The `SearchTermTemplateSelector` class defines `DefaultTemplate` and `OtherTemplate` `DataTemplate` properties that are set to different data templates. The `OnSelectTemplate` override returns `DefaultTemplate`, which displays a message to the user, when the search query isn't equal to "xamarin". When the search query is equal to "xamarin", the `OnSelectTemplate` override returns `OtherTemplate`, which displays a basic message to the user:

:::image type="content" source="media/emptyview/datatemplateselector.png" alt-text="Screenshot of a CollectionView runtime empty view template selection.":::

<!-- For more information about data template selectors, see [Create a DataTemplateSelector](~/fundamentals/templates/data-templates/selector.md). -->
