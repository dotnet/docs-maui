---
title: "Data templates"
description: ".NET MAUI data templates provide the ability to define the presentation of data on supported controls. Data templates can be chosen at runtime, using a DataTemplateSelector, based on the value of a data-bound property."
ms.date: 02/21/2022
---

# Data templates

.NET Multi-platform App UI (.NET MAUI) data templates provide the ability to define the presentation of data on supported controls.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Consider a `ListView` that displays a collection of `Person` objects. The following example shows the definition of the `Person` class:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
}
```

The `Person` class defines `Name`, `Age`, and `Location` properties, which can be set when a `Person` object is created. A control that displays collections, such as `ListView`, can be used to display `Person` objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataTemplates"
             x:Class=""DataTemplates.WithoutDataTemplatePage>
    <StackLayout>
        <ListView>
            <ListView.ItemsSource>
                <x:Array Type="{x:Type local:Person}">
                    <local:Person Name="Steve" Age="21" Location="USA" />
                    <local:Person Name="John" Age="37" Location="USA" />
                    <local:Person Name="Tom" Age="42" Location="UK" />
                    <local:Person Name="Lucas" Age="29" Location="Germany" />
                    <local:Person Name="Tariq" Age="39" Location="UK" />
                    <local:Person Name="Jane" Age="30" Location="USA" />
                </x:Array>
            </ListView.ItemsSource>
        </ListView>
    </StackLayout>
</ContentPage>
```

In this example, items are added to the `ListView` by initializing its `ItemsSource` property from an array of `Person` objects. `ListView` calls `ToString` when displaying the objects in the collection. However, because there is no `Person.ToString` override, `ToString` returns the type name of each object:

:::image type="content" source="media/datatemplate/no-data-template.png" alt-text="Screenshot of a ListView without a data template.":::

The `Person` object can override the `ToString` method to display meaningful data:

```csharp
public class Person
{
  ...
  public override string ToString ()
  {
    return Name;
  }
}
```

This results in the `ListView` displaying the `Person.Name` property value for each object in the collection:

:::image type="content" source="media/datatemplate/override-tostring.png" alt-text="Screenshot of a ListView that overrides the Person.ToString method.":::

The `Person.ToString` override could return a formatted string consisting of the `Name`, `Age`, and `Location` properties. However, this approach only offers limited control over the appearance of each item of data. For more flexibility, a `DataTemplate` can be created that defines the appearance of the data.

## Create a DataTemplate

A `DataTemplate` is used to specify the appearance of data, and typically uses data binding to display data. A common usage scenario for data templates is when displaying data from a collection of objects in a control such as a `ListView`, `CollectionView`, or `CarouselView`. For example, when a `ListView` is bound to a collection of `Person` objects, the `ListView.ItemTemplate` property can be set to a `DataTemplate` that defines the appearance of each `Person` object in the `ListView`. The `DataTemplate` will contain elements that bind to property values of each `Person` object. For more information about data binding, see [Data binding](~/fundamentals/data-binding/index.md).

A `DataTemplate` that's defined inline in a control is known as an *inline template*. Alternatively, data templates can be defined as a control-level, page-level, or app-level resource. Choosing where to define a `DataTemplate` impacts where it can be used:

- A `DataTemplate` defined at the control level can only be applied to the control.
- A `DataTemplate` defined at the page level can be applied to multiple controls on the page.
- A `DataTemplate` defined at the app level can be applied to valid controls throughout the app.

> [!NOTE]
> Data templates lower in the view hierarchy take precedence over those defined higher up when they share `x:Key` attributes. For example, an app-level data template will be overridden by a page-level data template, and a page-level data template will be overridden by a control-level data template, or an inline data template.

Regardless of where a `DataTemplate` is defined, they can be created inline, with a type, or as a resource.

### Create an inline DataTemplate

The `ListView.ItemTemplate` property can be set to an inline `DataTemplate`. An inline template, which is one that's defined inline in a control, should be used if there's no need to reuse the data template elsewhere. The elements specified in the `DataTemplate` define the appearance of each cell:

```xaml
<ListView>
    <ListView.ItemsSource>
        <x:Array Type="{x:Type local:Person}">
            <local:Person Name="Steve" Age="21" Location="USA" />
            <local:Person Name="John" Age="37" Location="USA" />
            <local:Person Name="Tom" Age="42" Location="UK" />
            <local:Person Name="Lucas" Age="29" Location="Germany" />
            <local:Person Name="Tariq" Age="39" Location="UK" />
            <local:Person Name="Jane" Age="30" Location="USA" />
        </x:Array>
    </ListView.ItemsSource>
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <Grid>
                    ...
                    <Label Text="{Binding Name}" FontAttributes="Bold" />
                    <Label Grid.Column="1" Text="{Binding Age}" />
                    <Label Grid.Column="2" Text="{Binding Location}" HorizontalTextAlignment="End" />
                </Grid>
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

In a `ListView`, the child of an inline `DataTemplate` must be of, or derive from, type `Cell`. In this example, a `ViewCell`, which derives from `Cell` is used. Layout inside the `ViewCell` is managed here by a `Grid`. The `Grid` contains three `Label` instances that bind their `Text` properties to the appropriate properties of each `Person` object in the collection. The following screenshot shows the resulting appearance:

:::image type="content" source="media/datatemplate/data-template-appearance.png" alt-text="Screenshot of a ListView with a data template.":::

### Create a DataTemplate with a type

The `ListView.ItemTemplate` property can be set to a `DataTemplate` that's created from a cell type. The advantage of this approach is that the appearance defined by the cell type can be reused by multiple data templates throughout the app. The following XAML shows an example of this approach:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataTemplates"
             ...>
    <StackLayout>
        ...
        <ListView>
           <ListView.ItemsSource>
                <x:Array Type="{x:Type local:Person}">
                    <local:Person Name="Steve" Age="21" Location="USA" />
                    ...
                </x:Array>
            </ListView.ItemsSource>
            <ListView.ItemTemplate>
                <DataTemplate>
                    <local:PersonCell />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

In this example, the `ListView.ItemTemplate` property is set to a `DataTemplate` that's created from a custom type that defines the cell appearance. The custom type must derive from `ViewCell`:

```xaml
<ViewCell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
          xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
          x:Class="DataTemplates.PersonCell">
     <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="0.5*" />
            <ColumnDefinition Width="0.2*" />
            <ColumnDefinition Width="0.3*" />
        </Grid.ColumnDefinitions>
        <Label Text="{Binding Name}" FontAttributes="Bold" />
        <Label Grid.Column="1" Text="{Binding Age}" />
        <Label Grid.Column="2" Text="{Binding Location}" HorizontalTextAlignment="End" />
    </Grid>
</ViewCell>
```

In this example, within the `ViewCell` layout is managed by a `Grid`. The `Grid` contains three `Label` instances that bind their `Text` properties to the appropriate properties of each `Person` object in the collection.

<!-- > [!NOTE]
> .NET MAUI also includes cell types that can be used to display simple data in `ListView` cells. For more information, see [Cell aAppearance](~/user-interface/controls/listview/customizing-cell-appearance.md). -->

### Create a DataTemplate as a resource

Data templates can be created as reusable objects in a `ResourceDictionary`. This is achieved by giving each `DataTemplate` a unique `x:Key` value, which provides it with a descriptive key in the `ResourceDictionary`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...>
    <ContentPage.Resources>
        <ResourceDictionary>
            <DataTemplate x:Key="personTemplate">
                 <ViewCell>
                    <Grid>
                        ...
                    </Grid>
                </ViewCell>
            </DataTemplate>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout>
        ...
        <ListView ItemTemplate="{StaticResource personTemplate}">
            <ListView.ItemsSource>
                <x:Array Type="{x:Type local:Person}">
                    <local:Person Name="Steve" Age="21" Location="USA" />
                    ...
                </x:Array>
            </ListView.ItemsSource>
        </ListView>
    </StackLayout>
</ContentPage>
```

In this example, the `DataTemplate` is assigned to the `ListView.ItemTemplate` property by using the `StaticResource` markup extension. While the `DataTemplate` is defined in the page's `ResourceDictionary`, it could also be defined at the control level or app level.

## Create a DataTemplateSelector

A `DataTemplateSelector` can be used to choose a `DataTemplate` at runtime based on the value of a data-bound property. This enables multiple data templates to be applied to the same type of object, to customize the appearance of particular objects. A data template selector enables scenarios such as a `ListView` binding to a collection of objects, where the appearance of each object in the `ListView` can be chosen at runtime by the data template selector returning a specific `DataTemplate`.

A data template selector can be implemented by creating a class that inherits from `DataTemplateSelector`. The `OnSelectTemplate` method should then be overridden to return a specific `DataTemplate`:

```csharp
public class PersonDataTemplateSelector : DataTemplateSelector
{
  public DataTemplate ValidTemplate { get; set; }
  public DataTemplate InvalidTemplate { get; set; }

  protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
  {
    return ((Person)item).DateOfBirth.Year >= 1980 ? ValidTemplate : InvalidTemplate;
  }
}
```

In this example, the `OnSelectTemplate` method returns a specific data template based on the value of the `DateOfBirth` property. The data template to return is the value of the `ValidTemplate` property or the `InvalidTemplate` property, which are set when consuming the `PersonDataTemplateSelector`.

An instance of the data template selector class can then be assigned to .NET MAUI control properties such as `ListView.ItemTemplate`.

### Limitations

`DataTemplateSelector` instances have the following limitations:

- The `DataTemplateSelector` subclass must always return the same template for the same data if queried multiple times.
- The `DataTemplateSelector` subclass must not return another `DataTemplateSelector` subclass.
- The `DataTemplateSelector` subclass must not return new instances of a `DataTemplate` on each call. Instead, the same instance must be returned. Failure to do so will create a memory leak and will disable virtualization.
- On Android, there can be no more than 20 different data templates per `ListView`.

### Consume a DataTemplateSelector

A data template selector can be consumed by by declaring it as a resource and assigning it to the `ItemTemplate` property of a control such as a `ListView`.

The following example shows declaring `PersonDataTemplateSelector` as a page level resource:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:local="clr-namespace:Selector"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Selector.MainPage">
    <ContentPage.Resources>
        <DataTemplate x:Key="validPersonTemplate">
            <ViewCell>
                ...
            </ViewCell>
        </DataTemplate>
        <DataTemplate x:Key="invalidPersonTemplate">
            <ViewCell>
                ...
            </ViewCell>
        </DataTemplate>
        <local:PersonDataTemplateSelector x:Key="personDataTemplateSelector"
                                          ValidTemplate="{StaticResource validPersonTemplate}"
                                          InvalidTemplate="{StaticResource invalidPersonTemplate}" />
    </ContentPage.Resources>
    ...
</ContentPage>
```

In this example, the page level `ResourceDictionary` defines two `DataTemplate` objects and a `PersonDataTemplateSelector` object. The `PersonDataTemplateSelector` object sets its `ValidTemplate` and `InvalidTemplate` properties to the `DataTemplate` objects using the `StaticResource` markup extension. While the resources are defined in the page's `ResourceDictionary`, they could also be defined at the control level or app level.

The `PersonDataTemplateSelector` object can be consumed by assigning it to the `ListView.ItemTemplate` property:

```xaml
<ListView x:Name="listView"
          ItemTemplate="{StaticResource personDataTemplateSelector}" />
```

At runtime, the `ListView` calls the `PersonDataTemplateSelector.OnSelectTemplate` method for each of the items in the underlying collection, with the call passing the data object as the `item` parameter. The `DataTemplate` that is returned is then applied to that object.

The following screenshot shows the result of the `ListView` applying the `PersonDataTemplateSelector` to each object in the underlying collection:

:::image type="content" source="media/datatemplate/data-template-selector.png" alt-text="Screenshot of a ListView with a data template selector.":::

Any `Person` object that has a `DateOfBirth` property value greater than or equal to 1980 is displayed in green, with the remaining objects being displayed in red.
