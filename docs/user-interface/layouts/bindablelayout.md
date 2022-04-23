---
title: "BindableLayout"
description: ".NET MAUI bindable layouts enable layout classes to generate their content by binding to a collection of items, with the option to set the appearance of each item with a DataTemplate."
ms.date: 02/22/2022
---

# BindableLayout

.NET Multi-platform App UI (.NET MAUI) bindable layouts enable any layout class that derives from the `Layout` class to generate its content by binding to a collection of items, with the option to set the appearance of each item with a `DataTemplate`.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Bindable layouts are provided by the `BindableLayout` class, which exposes the following attached properties:

- `ItemsSource` – specifies the collection of `IEnumerable` items to be displayed by the layout.
- `ItemTemplate` – specifies the `DataTemplate` to apply to each item in the collection of items displayed by the layout.
- `ItemTemplateSelector` – specifies the `DataTemplateSelector` that will be used to choose a `DataTemplate` for an item at runtime.

> [!NOTE]
> The `ItemTemplate` property takes precedence when both the `ItemTemplate` and `ItemTemplateSelector` properties are set.

In addition, the `BindableLayout` class exposes the following bindable properties:

- `EmptyView` – specifies the `string` or view that will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The default value is `null`.
- `EmptyViewTemplate` – specifies the `DataTemplate` that will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The default value is `null`.

> [!NOTE]
> The `EmptyViewTemplate` property takes precedence when both the `EmptyView` and `EmptyViewTemplate` properties are set.

All of these properties can be attached to the `AbsoluteLayout`, `FlexLayout`, `Grid`, `HorizontalStackLayout`, `StackLayout`, and `VerticalStackLayout` classes, which all derive from the `Layout` class.

When the `BinableLayout.ItemsSource` property is set to a collection of items and attached to a `Layout`-derived class, each item in the collection is added to the `Layout`-derived class for display. The `Layout`-derived class will then update its child views when the underlying collection changes. <!-- For more information about the .NET MAUI layout cycle, see [Creating a Custom Layout](~/user-interface/layouts/custom.md).-->

Bindable layouts should only be used when the collection of items to be displayed is small, and scrolling and selection isn't required. While scrolling can be provided by wrapping a bindable layout in a `ScrollView`, this is not recommended as bindable layouts lack UI virtualization. When scrolling is required, a scrollable view that includes UI virtualization, such as `ListView` or `CollectionView`, should be used. Failure to observe this recommendation can lead to performance issues.

> [!IMPORTANT]
> While it's technically possible to attach a bindable layout to any layout class that derives from the `Layout` class, it's not always practical to do so, particularly for the `AbsoluteLayout` and  `Grid` classes. For example, consider the scenario of wanting to display a collection of data in a `Grid` using a bindable layout, where each item in the collection is an object containing multiple properties. Each row in the `Grid` should display an object from the collection, with each column in the `Grid` displaying one of the object's properties. Because the `DataTemplate` for the bindable layout can only contain a single object, it's necessary for that object to be a layout class containing multiple views that each display one of the object's properties in a specific `Grid` column. While this scenario can be realised with bindable layouts, it results in a parent `Grid` containing a child `Grid` for each item in the bound collection, which is a highly inefficient and problematic use of the `Grid` layout.

## Populate a bindable layout with data

A bindable layout is populated with data by setting its `ItemsSource` property to any collection that implements `IEnumerable`, and attaching it to a `Layout`-derived class:

```xaml
<Grid BindableLayout.ItemsSource="{Binding Items}" />
```

The equivalent C# code is:

```csharp
IEnumerable<string> items = ...;
Grid grid = new Grid();
BindableLayout.SetItemsSource(grid, items);
```

When the `BindableLayout.ItemsSource` attached property is set on a layout, but the `BindableLayout.ItemTemplate` attached property isn't set, every item in the `IEnumerable` collection will be displayed by a `Label` that's created by the `BindableLayout` class.

## Define item appearance

The appearance of each item in the bindable layout can be defined by setting the `BindableLayout.ItemTemplate` attached property to a `DataTemplate`:

```xaml
<StackLayout BindableLayout.ItemsSource="{Binding User.TopFollowers}"
             Orientation="Horizontal"
             ...>
    <BindableLayout.ItemTemplate>
        <DataTemplate>
            <Image Source="{Binding}"
                   Aspect="AspectFill"
                   WidthRequest="44"
                   HeightRequest="44"
                   ... />
        </DataTemplate>
    </BindableLayout.ItemTemplate>
</StackLayout>
```

The equivalent C# code is:

```csharp
DataTemplate imageTemplate = ...;
StackLayout stackLayout = new StackLayout();
BindableLayout.SetItemsSource(stackLayout, viewModel.User.TopFollowers);
BindableLayout.SetItemTemplate(stackLayout, imageTemplate);
```

In this example, every item in the `TopFollowers` collection will be displayed by an `Image` view defined in the `DataTemplate`:

:::image type="content" source="media/bindablelayout/top-followers.png" alt-text=".NET MAUI bindable layout with a DataTemplate.":::

<!-- For more information about data templates, see [Data templates](~/fundamentals/datatemplate.md). -->

## Choose item appearance at runtime

The appearance of each item in the bindable layout can be chosen at runtime, based on the item value, by setting the `BindableLayout.ItemTemplateSelector` attached property to a `DataTemplateSelector`:

```xaml
<FlexLayout BindableLayout.ItemsSource="{Binding User.FavoriteTech}"
            BindableLayout.ItemTemplateSelector="{StaticResource TechItemTemplateSelector}"
            ... />
```

The equivalent C# code is:

```csharp
DataTemplateSelector dataTemplateSelector = new TechItemTemplateSelector { ... };
FlexLayout flexLayout = new FlexLayout();
BindableLayout.SetItemsSource(flexLayout, viewModel.User.FavoriteTech);
BindableLayout.SetItemTemplateSelector(flexLayout, dataTemplateSelector);
```

The following example shows the `TechItemTemplateSelector` class:

```csharp
public class TechItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate { get; set; }
    public DataTemplate MAUITemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return (string)item == ".NET MAUI" ? MAUITemplate : DefaultTemplate;
    }
}
```

The `TechItemTemplateSelector` class defines `DefaultTemplate` and `MAUITemplate` `DataTemplate` properties that are set to different data templates. The `OnSelectTemplate` method returns the `MAUITemplate`, which displays an item in dark red with a heart next to it, when the item is equal to ".NET MAUI". When the item isn't equal to ".NET MAUI", the `OnSelectTemplate` method returns the `DefaultTemplate`, which displays an item using the default color of a `Label`:

:::image type="content" source="media/bindablelayout/favorite-tech.png" alt-text=".NET MAUI bindable layout with a DataTemplateSelector.":::

<!-- For more information about data template selectors, see [Create a DataTemplateSelector](~/fundamentals/datatemplate.md#create-a-datatemplateselector). -->

## Display a string when data is unavailable

The `EmptyView` property can be set to a string, which will be displayed by a `Label` when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The following XAML shows an example of this scenario:

```xaml
<StackLayout BindableLayout.ItemsSource="{Binding UserWithoutAchievements.Achievements}"
             BindableLayout.EmptyView="No achievements">
    ...
</StackLayout>
```

The result is that when the data bound collection is `null`, the string set as the `EmptyView` property value is displayed:

:::image type="content" source="media/bindablelayout/emptyview-string.png" alt-text="Screenshot of a bindable layout string empty view.":::

## Display views when data is unavailable

The `EmptyView` property can be set to a view, which will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. This can be a single view, or a view that contains multiple child views. The following XAML example shows the `EmptyView` property set to a view that contains multiple child views:

```xaml
<StackLayout BindableLayout.ItemsSource="{Binding UserWithoutAchievements.Achievements}">
    <BindableLayout.EmptyView>
        <StackLayout>
            <Label Text="None."
                   FontAttributes="Italic"
                   FontSize="{StaticResource smallTextSize}" />
            <Label Text="Try harder and return later?"
                   FontAttributes="Italic"
                   FontSize="{StaticResource smallTextSize}" />
        </StackLayout>
    </BindableLayout.EmptyView>
    ...
</StackLayout>
```

The result is that when the data bound collection is `null`, the `StackLayout` and its child views are displayed.

:::image type="content" source="media/bindablelayout/emptyview-views.png" alt-text="Screenshot of a bindable layout empty view with multiple views.":::

Similarly, the `EmptyViewTemplate` can be set to a `DataTemplate`, which will be displayed when the `ItemsSource` property is `null`, or when the collection specified by the `ItemsSource` property is `null` or empty. The `DataTemplate` can contain a single view, or a view that contains multiple child views. In addition, the `BindingContext` of the `EmptyViewTemplate` will be inherited from the `BindingContext` of the `BindableLayout`. The following XAML example shows the `EmptyViewTemplate` property set to a `DataTemplate` that contains a single view:

```xaml
<StackLayout BindableLayout.ItemsSource="{Binding UserWithoutAchievements.Achievements}">
    <BindableLayout.EmptyViewTemplate>
        <DataTemplate>
            <Label Text="{Binding Source={x:Reference usernameLabel}, Path=Text, StringFormat='{0} has no achievements.'}" />
        </DataTemplate>
    </BindableLayout.EmptyViewTemplate>
    ...
</StackLayout>
```

The result is that when the data bound collection is `null`, the `Label` in the `DataTemplate` is displayed:

:::image type="content" source="media/bindablelayout/emptyviewtemplate.png" alt-text="Screenshot of a bindable layout empty view template.":::

> [!NOTE]
> The `EmptyViewTemplate` property can't be set via a `DataTemplateSelector`.

## Choose an EmptyView at runtime

Views that will be displayed as an `EmptyView` when data is unavailable, can be defined as `ContentView` objects in a `ResourceDictionary`. The `EmptyView` property can then be set to a specific `ContentView`, based on some business logic, at runtime. The following XAML shows an example of this scenario:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        ...    
        <ContentView x:Key="BasicEmptyView">
            <StackLayout>
                <Label Text="No achievements."
                       FontSize="14" />
            </StackLayout>
        </ContentView>
        <ContentView x:Key="AdvancedEmptyView">
            <StackLayout>
                <Label Text="None."
                       FontAttributes="Italic"
                       FontSize="14" />
                <Label Text="Try harder and return later?"
                       FontAttributes="Italic"
                       FontSize="14" />
            </StackLayout>
        </ContentView>
    </ContentPage.Resources>

    <StackLayout>
        ...
        <Switch Toggled="OnEmptyViewSwitchToggled" />

        <StackLayout x:Name="stackLayout"
                     BindableLayout.ItemsSource="{Binding UserWithoutAchievements.Achievements}">
            ...
        </StackLayout>
    </StackLayout>
</ContentPage>
```

The XAML defines two `ContentView` objects in the page-level `ResourceDictionary`, with the `Switch` object controlling which `ContentView` object will be set as the `EmptyView` property value. When the `Switch` is toggled, the `OnEmptyViewSwitchToggled` event handler executes the `ToggleEmptyView` method:

```csharp
void ToggleEmptyView(bool isToggled)
{
    object view = isToggled ? Resources["BasicEmptyView"] : Resources["AdvancedEmptyView"];
    BindableLayout.SetEmptyView(stackLayout, view);
}
```

The `ToggleEmptyView` method sets the `EmptyView` property of the `stackLayout` object to one of the two `ContentView` objects stored in the `ResourceDictionary`, based on the value of the `Switch.IsToggled` property. Then, when the data bound collection is `null`, the `ContentView` object set as the `EmptyView` property is displayed.
