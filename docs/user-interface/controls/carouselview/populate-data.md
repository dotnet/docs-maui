---
title: "Populate a CarouselView with data"
description: "A .NET MAUI CarouselView is populated with data by setting its ItemsSource property to any collection that implements IEnumerable."
ms.date: 09/30/2024
---

# Populate a CarouselView with data

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-carouselview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.CarouselView> includes the following properties that define the data to be displayed, and its appearance:

- `ItemsSource`, of type `IEnumerable`, specifies the collection of items to be displayed, and has a default value of `null`.
- `ItemTemplate`, of type <xref:Microsoft.Maui.Controls.DataTemplate>, specifies the template to apply to each item in the collection of items to be displayed.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings.

<xref:Microsoft.Maui.Controls.CarouselView> defines a `ItemsUpdatingScrollMode` property that represents the scrolling behavior of the <xref:Microsoft.Maui.Controls.CarouselView> when new items are added to it. For more information about this property, see [Control scroll position when new items are added](scrolling.md#control-scroll-position-when-new-items-are-added).

<xref:Microsoft.Maui.Controls.CarouselView> supports incremental data virtualization as the user scrolls. For more information, see [Load data incrementally](#load-data-incrementally).

## Populate a CarouselView with data

A <xref:Microsoft.Maui.Controls.CarouselView> is populated with data by setting its `ItemsSource` property to any collection that implements `IEnumerable`. By default, <xref:Microsoft.Maui.Controls.CarouselView> displays items horizontally.

> [!IMPORTANT]
> If the <xref:Microsoft.Maui.Controls.CarouselView> is required to refresh as items are added, removed, or changed in the underlying collection, the underlying collection should be an `IEnumerable` collection that sends property change notifications, such as `ObservableCollection`.

<xref:Microsoft.Maui.Controls.CarouselView> can be populated with data by using data binding to bind its `ItemsSource` property to an `IEnumerable` collection. In XAML, this is achieved with the `Binding` markup extension:

```xaml
<CarouselView ItemsSource="{Binding Monkeys}" />
```

The equivalent C# code is:

```csharp
CarouselView carouselView = new CarouselView();
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);
```

In this example, the `ItemsSource` property data binds to the `Monkeys` property of the connected viewmodel.

> [!NOTE]
> Compiled bindings can be enabled to improve data binding performance in .NET MAUI applications. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).

For information on how to change the <xref:Microsoft.Maui.Controls.CarouselView> orientation, see [Specify CarouselView layout](layout.md). For information on how to define the appearance of each item in the <xref:Microsoft.Maui.Controls.CarouselView>, see [Define item appearance](#define-item-appearance). For more information about data binding, see [Data binding](~/fundamentals/data-binding/index.md).

## Define item appearance

The appearance of each item in the <xref:Microsoft.Maui.Controls.CarouselView> can be defined by setting the `CarouselView.ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<CarouselView ItemsSource="{Binding Monkeys}">
    <CarouselView.ItemTemplate>
        <DataTemplate>
            <StackLayout>
                <Border Stroke="DarkGray"
                        StrokeShape="RoundRectangle 5"
                        Margin="20"
                        Padding="20"
                        HeightRequest="360"
                        HorizontalOptions="Center"
                        VerticalOptions="Center">            
                    <StackLayout>
                        <Label Text="{Binding Name}"
                               FontAttributes="Bold"
                               FontSize="18"
                               HorizontalOptions="Center"
                               VerticalOptions="Center" />
                        <Image Source="{Binding ImageUrl}"
                               Aspect="AspectFill"
                               HeightRequest="150"
                               WidthRequest="150"
                               HorizontalOptions="Center" />
                        <Label Text="{Binding Location}"
                               HorizontalOptions="Center" />
                        <Label Text="{Binding Details}"
                               FontAttributes="Italic"
                               HorizontalOptions="Center"
                               MaxLines="5"
                               LineBreakMode="TailTruncation" />
                    </StackLayout>
                </Border>
            </StackLayout>
        </DataTemplate>
    </CarouselView.ItemTemplate>
</CarouselView>
```

The equivalent C# code is:

```csharp
CarouselView carouselView = new CarouselView();
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);

carouselView.ItemTemplate = new DataTemplate(() =>
{
    Label nameLabel = new Label { ... };
    nameLabel.SetBinding(Label.TextProperty, static (Monkey monkey) => monkey.Name);

    Image image = new Image { ... };
    image.SetBinding(Image.SourceProperty, static (Monkey monkey) => monkey.ImageUrl);

    Label locationLabel = new Label { ... };
    locationLabel.SetBinding(Label.TextProperty, static (Monkey monkey) => monkey.Location);

    Label detailsLabel = new Label { ... };
    detailsLabel.SetBinding(Label.TextProperty, static (Monkey monkey) => monkey.Details);

    StackLayout stackLayout = new StackLayout();
    stackLayout.Add(nameLabel);
    stackLayout.Add(image);
    stackLayout.Add(locationLabel);
    stackLayout.Add(detailsLabel);

    Border border = new Border { ... };
    StackLayout rootStackLayout = new StackLayout();
    rootStackLayout.Add(border);

    return rootStackLayout;
});
```

The elements specified in the <xref:Microsoft.Maui.Controls.DataTemplate> define the appearance of each item in the <xref:Microsoft.Maui.Controls.CarouselView>. In the example, layout within the <xref:Microsoft.Maui.Controls.DataTemplate> is managed by a <xref:Microsoft.Maui.Controls.StackLayout>, and the data is displayed with an <xref:Microsoft.Maui.Controls.Image> object, and three <xref:Microsoft.Maui.Controls.Label> objects, that all bind to properties of the `Monkey` class:

```csharp
public class Monkey
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Details { get; set; }
    public string ImageUrl { get; set; }
}
```

The following screenshot shows an example of templating each item:

:::image type="content" source="media/populate-data/datatemplate.png" alt-text="Screenshot of a CarouselView where each item is templated.":::

For more information about data templates, see [Data templates](~/fundamentals/datatemplate.md).

## Choose item appearance at runtime

The appearance of each item in the <xref:Microsoft.Maui.Controls.CarouselView> can be chosen at runtime, based on the item value, by setting the `CarouselView.ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplateSelector> object:

```xaml
<ContentPage ...
             xmlns:controls="clr-namespace:CarouselViewDemos.Controls"
             x:Class="CarouselViewDemos.Views.HorizontalLayoutDataTemplateSelectorPage">
    <ContentPage.Resources>
        <DataTemplate x:Key="AmericanMonkeyTemplate">
            ...
        </DataTemplate>

        <DataTemplate x:Key="OtherMonkeyTemplate">
            ...
        </DataTemplate>

        <controls:MonkeyDataTemplateSelector x:Key="MonkeySelector"
                                             AmericanMonkey="{StaticResource AmericanMonkeyTemplate}"
                                             OtherMonkey="{StaticResource OtherMonkeyTemplate}" />
    </ContentPage.Resources>

    <CarouselView ItemsSource="{Binding Monkeys}"
                  ItemTemplate="{StaticResource MonkeySelector}" />
</ContentPage>
```

The equivalent C# code is:

```csharp
CarouselView carouselView = new CarouselView
{
    ItemTemplate = new MonkeyDataTemplateSelector { ... }
};
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);
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

The `MonkeyDataTemplateSelector` class defines `AmericanMonkey` and `OtherMonkey` <xref:Microsoft.Maui.Controls.DataTemplate> properties that are set to different data templates. The `OnSelectTemplate` override returns the `AmericanMonkey` template when the monkey name contains "America". When the monkey name doesn't contain "America", the `OnSelectTemplate` override returns the `OtherMonkey` template, which displays its data grayed out:

:::image type="content" source="media/populate-data/datatemplateselector.png" alt-text="Screenshot of a CarouselView runtime item template selection.":::

For more information about data template selectors, see [Create a DataTemplateSelector](~/fundamentals/datatemplate.md#create-a-datatemplateselector).

> [!IMPORTANT]
> When using <xref:Microsoft.Maui.Controls.CarouselView>, never set the root element of your <xref:Microsoft.Maui.Controls.DataTemplate> objects to a <xref:Microsoft.Maui.Controls.ViewCell>. This will result in an exception being thrown because <xref:Microsoft.Maui.Controls.CarouselView> has no concept of cells.

## Display indicators

Indicators, that represent the number of items and current position in a <xref:Microsoft.Maui.Controls.CarouselView>, can be displayed next to the <xref:Microsoft.Maui.Controls.CarouselView>. This can be accomplished with the <xref:Microsoft.Maui.Controls.IndicatorView> control:

```xaml
<StackLayout>
    <CarouselView ItemsSource="{Binding Monkeys}"
                  IndicatorView="indicatorView">
        <CarouselView.ItemTemplate>
            <!-- DataTemplate that defines item appearance -->
        </CarouselView.ItemTemplate>
    </CarouselView>
    <IndicatorView x:Name="indicatorView"
                   IndicatorColor="LightGray"
                   SelectedIndicatorColor="DarkGray"
                   HorizontalOptions="Center" />
</StackLayout>
```

In this example, the <xref:Microsoft.Maui.Controls.IndicatorView> is rendered beneath the <xref:Microsoft.Maui.Controls.CarouselView>, with an indicator for each item in the <xref:Microsoft.Maui.Controls.CarouselView>. The <xref:Microsoft.Maui.Controls.IndicatorView> is populated with data by setting the `CarouselView.IndicatorView` property to the <xref:Microsoft.Maui.Controls.IndicatorView> object. Each indicator is a light gray circle, while the indicator that represents the current item in the <xref:Microsoft.Maui.Controls.CarouselView> is dark gray:

:::image type="content" source="media/populate-data/indicators.png" alt-text="Screenshot of a CarouselView and IndicatorView.":::

> [!IMPORTANT]
> Setting the `CarouselView.IndicatorView` property results in the `IndicatorView.Position` property binding to the `CarouselView.Position` property, and the `IndicatorView.ItemsSource` property binding to the `CarouselView.ItemsSource` property.

For more information about indicators, see [IndicatorView](~/user-interface/controls/indicatorview.md).

## Context menus

<xref:Microsoft.Maui.Controls.CarouselView> supports context menus for items of data through the <xref:Microsoft.Maui.Controls.SwipeView>, which reveals the context menu with a swipe gesture. The <xref:Microsoft.Maui.Controls.SwipeView> is a container control that wraps around an item of content, and provides context menu items for that item of content. Therefore, context menus are implemented for a <xref:Microsoft.Maui.Controls.CarouselView> by creating a <xref:Microsoft.Maui.Controls.SwipeView> that defines the content that the <xref:Microsoft.Maui.Controls.SwipeView> wraps around, and the context menu items that are revealed by the swipe gesture. This is achieved by adding a <xref:Microsoft.Maui.Controls.SwipeView> to the <xref:Microsoft.Maui.Controls.DataTemplate> that defines the appearance of each item of data in the <xref:Microsoft.Maui.Controls.CarouselView>:

```xaml
<CarouselView x:Name="carouselView"
              ItemsSource="{Binding Monkeys}">
    <CarouselView.ItemTemplate>
        <DataTemplate>
            <StackLayout>
                    <Border Stroke="DarkGray"
                            StrokeShape="RoundRectangle 5"
                            Margin="20"
                            Padding="20"
                            HeightRequest="360"
                            HorizontalOptions="Center"
                            VerticalOptions="Center">
                        <SwipeView>
                            <SwipeView.TopItems>
                                <SwipeItems>
                                    <SwipeItem Text="Favorite"
                                               IconImageSource="favorite.png"
                                               BackgroundColor="LightGreen"
                                               Command="{Binding Source={x:Reference carouselView}, Path=BindingContext.FavoriteCommand}"
                                               CommandParameter="{Binding}" />
                                </SwipeItems>
                            </SwipeView.TopItems>
                            <SwipeView.BottomItems>
                                <SwipeItems>
                                    <SwipeItem Text="Delete"
                                               IconImageSource="delete.png"
                                               BackgroundColor="LightPink"
                                               Command="{Binding Source={x:Reference carouselView}, Path=BindingContext.DeleteCommand}"
                                               CommandParameter="{Binding}" />
                                </SwipeItems>
                            </SwipeView.BottomItems>
                            <StackLayout>
                                <!-- Define item appearance -->
                            </StackLayout>
                        </SwipeView>
                    </Border>
            </StackLayout>
        </DataTemplate>
    </CarouselView.ItemTemplate>
</CarouselView>
```

The equivalent C# code is:

```csharp
CarouselView carouselView = new CarouselView();
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);

carouselView.ItemTemplate = new DataTemplate(() =>
{
    StackLayout stackLayout = new StackLayout();
    Border border = new Border { ... };

    SwipeView swipeView = new SwipeView();
    SwipeItem favoriteSwipeItem = new SwipeItem
    {
        Text = "Favorite",
        IconImageSource = "favorite.png",
        BackgroundColor = Colors.LightGreen
    };
    favoriteSwipeItem.SetBinding(MenuItem.CommandProperty, Binding.Create(static (CarouselView cv) => (cv.BindingContext as MonkeysViewModel).FavoriteCommand, source: carouselView));
    favoriteSwipeItem.SetBinding(MenuItem.CommandParameterProperty, static (CarouselView cv) => cv.CurrentItem);

    SwipeItem deleteSwipeItem = new SwipeItem
    {
        Text = "Delete",
        IconImageSource = "delete.png",
        BackgroundColor = Colors.LightPink
    };
    deleteSwipeItem.SetBinding(MenuItem.CommandProperty, Binding.Create(static (CarouselView cv) => (cv.BindingContext as MonkeysViewModel).DeleteCommand, source: carouselView));
    deleteSwipeItem.SetBinding(MenuItem.CommandParameterProperty, static (CarouselView cv) => cv.CurrentItem);

    swipeView.TopItems = new SwipeItems { favoriteSwipeItem };
    swipeView.BottomItems = new SwipeItems { deleteSwipeItem };

    StackLayout swipeViewStackLayout = new StackLayout { ... };
    swipeView.Content = swipeViewStackLayout;
    border.Content = swipeView;
    stackLayout.Add(border);

    return stackLayout;
});
```

In this example, the <xref:Microsoft.Maui.Controls.SwipeView> content is a <xref:Microsoft.Maui.Controls.StackLayout> that defines the appearance of each item that's surrounded by a <xref:Microsoft.Maui.Controls.Border> in the <xref:Microsoft.Maui.Controls.CarouselView>. The swipe items are used to perform actions on the <xref:Microsoft.Maui.Controls.SwipeView> content, and are revealed when the control is swiped from the bottom and from the top:

:::image type="content" source="media/populate-data/swipeview-bottom.png" alt-text="Screenshot of a CarouselView bottom context menu item.":::
:::image type="content" source="media/populate-data/swipeview-top.png" alt-text="Screenshot of a CarouselView top context menu item.":::

<xref:Microsoft.Maui.Controls.SwipeView> supports four different swipe directions, with the swipe direction being defined by the directional `SwipeItems` collection the `SwipeItems` objects are added to. By default, a swipe item is executed when it's tapped by the user. In addition, once a swipe item has been executed the swipe items are hidden and the <xref:Microsoft.Maui.Controls.SwipeView> content is re-displayed. However, these behaviors can be changed.

For more information about the <xref:Microsoft.Maui.Controls.SwipeView> control, see [SwipeView](~/user-interface/controls/swipeview.md).

## Pull to refresh

<xref:Microsoft.Maui.Controls.CarouselView> supports pull to refresh functionality through the <xref:Microsoft.Maui.Controls.RefreshView>, which enables the data being displayed to be refreshed by pulling down on the items. The <xref:Microsoft.Maui.Controls.RefreshView> is a container control that provides pull to refresh functionality to its child, provided that the child supports scrollable content. Therefore, pull to refresh is implemented for a <xref:Microsoft.Maui.Controls.CarouselView> by setting it as the child of a <xref:Microsoft.Maui.Controls.RefreshView>:

```xaml
<RefreshView IsRefreshing="{Binding IsRefreshing}"
             Command="{Binding RefreshCommand}">
    <CarouselView ItemsSource="{Binding Animals}">
        ...
    </CarouselView>
</RefreshView>
```

The equivalent C# code is:

```csharp
RefreshView refreshView = new RefreshView();
ICommand refreshCommand = new Command(() =>
{
    // IsRefreshing is true
    // Refresh data here
    refreshView.IsRefreshing = false;
});
refreshView.Command = refreshCommand;

CarouselView carouselView = new CarouselView();
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (AnimalsViewModel vm) => vm.Animals);
refreshView.Content = carouselView;
// ...
```

When the user initiates a refresh, the <xref:System.Windows.Input.ICommand> defined by the `Command` property is executed, which should refresh the items being displayed. A refresh visualization is shown while the refresh occurs, which consists of an animated progress circle:

:::image type="content" source="media/populate-data/pull-to-refresh.png" alt-text="Screenshot of CarouselView pull-to-refresh.":::

The value of the `RefreshView.IsRefreshing` property indicates the current state of the <xref:Microsoft.Maui.Controls.RefreshView>. When a refresh is triggered by the user, this property will automatically transition to `true`. Once the refresh completes, you should reset the property to `false`.

For more information about <xref:Microsoft.Maui.Controls.RefreshView>, see [RefreshView](~/user-interface/controls/refreshview.md).

## Load data incrementally

<xref:Microsoft.Maui.Controls.CarouselView> supports incremental data virtualization as the user scrolls. This enables scenarios such as asynchronously loading a page of data from a web service, as the user scrolls. In addition, the point at which more data is loaded is configurable so that users don't see blank space, or are stopped from scrolling.

<xref:Microsoft.Maui.Controls.CarouselView> defines the following properties to control incremental loading of data:

- `RemainingItemsThreshold`, of type `int`, the threshold of items not yet visible in the list at which the `RemainingItemsThresholdReached` event will be fired.
- `RemainingItemsThresholdReachedCommand`, of type <xref:System.Windows.Input.ICommand>, which is executed when the `RemainingItemsThreshold` is reached.
- `RemainingItemsThresholdReachedCommandParameter`, of type `object`, which is the parameter that's passed to the `RemainingItemsThresholdReachedCommand`.

<xref:Microsoft.Maui.Controls.CarouselView> also defines a `RemainingItemsThresholdReached` event that is fired when the <xref:Microsoft.Maui.Controls.CarouselView> is scrolled far enough that `RemainingItemsThreshold` items have not been displayed. This event can be handled to load more items. In addition, when the `RemainingItemsThresholdReached` event is fired, the `RemainingItemsThresholdReachedCommand` is executed, enabling incremental data loading to take place in a viewmodel.

The default value of the `RemainingItemsThreshold` property is -1, which indicates that the `RemainingItemsThresholdReached` event will never be fired. When the property value is 0, the `RemainingItemsThresholdReached` event will be fired when the final item in the `ItemsSource` is displayed. For values greater than 0, the `RemainingItemsThresholdReached` event will be fired when the `ItemsSource` contains that number of items not yet scrolled to.

> [!NOTE]
> <xref:Microsoft.Maui.Controls.CarouselView> validates the `RemainingItemsThreshold` property so that its value is always greater than or equal to -1.

The following XAML example shows a <xref:Microsoft.Maui.Controls.CarouselView> that loads data incrementally:

```xaml
<CarouselView ItemsSource="{Binding Animals}"
              RemainingItemsThreshold="2"
              RemainingItemsThresholdReached="OnCarouselViewRemainingItemsThresholdReached"
              RemainingItemsThresholdReachedCommand="{Binding LoadMoreDataCommand}">
    ...
</CarouselView>
```

The equivalent C# code is:

```csharp
CarouselView carouselView = new CarouselView
{
    RemainingItemsThreshold = 2
};
carouselView.RemainingItemsThresholdReached += OnCollectionViewRemainingItemsThresholdReached;
carouselView.SetBinding(ItemsView.ItemsSourceProperty, static (AnimalsViewModel vm) => vm.Animals);
```

In this code example, the `RemainingItemsThresholdReached` event fires when there are 2 items not yet scrolled to, and in response executes the `OnCollectionViewRemainingItemsThresholdReached` event handler:

```csharp
void OnCollectionViewRemainingItemsThresholdReached(object sender, EventArgs e)
{
    // Retrieve more data here and add it to the CollectionView's ItemsSource collection.
}
```

> [!NOTE]
> Data can also be loaded incrementally by binding the `RemainingItemsThresholdReachedCommand` to an <xref:System.Windows.Input.ICommand> implementation in the viewmodel.
