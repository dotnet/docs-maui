---
title: "Binding Path"
description: "This article explains how to use .NET MAUI data bindings to access sub-properties and collection members with the Path property of the Binding class."
ms.date: 01/19/2022
---

# Binding Path

A .NET Multi-platform App UI (.NET MAUI) 
In all the previous data-binding examples, the `Path` property of the `Binding` class (or the `Path` property of the `Binding` markup extension) has been set to a single property. It's actually possible to set `Path` to a *sub-property* (a property of a property), or to a member of a collection.
[!INCLUDE [docs under construction](~/includes/preview-note.md)]

For example, suppose your page contains a `TimePicker`:

```xaml
<TimePicker x:Name="timePicker">
```

The `Time` property of `TimePicker` is of type `TimeSpan`, but perhaps you want to create a data binding that references the `TotalSeconds` property of that `TimeSpan` value. Here's the data binding:

```xaml
{Binding Source={x:Reference timePicker},
         Path=Time.TotalSeconds}
```

The `Time` property is of type `TimeSpan`, which has a `TotalSeconds` property. The `Time` and `TotalSeconds` properties are simply connected with a period. The items in the `Path` string always refer to properties and not to the types of these properties.

That example and several others are shown in the **Path Variations** page:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:globe="clr-namespace:System.Globalization;assembly=netstandard"
             x:Class="DataBindingDemos.PathVariationsPage"
             Title="Path Variations"
             x:Name="page">
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style TargetType="Label">
                <Setter Property="FontSize" Value="Large" />
                <Setter Property="HorizontalTextAlignment" Value="Center" />
                <Setter Property="VerticalOptions" Value="CenterAndExpand" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>

    <StackLayout Margin="10, 0">
        <TimePicker x:Name="timePicker" />

        <Label Text="{Binding Source={x:Reference timePicker},
                              Path=Time.TotalSeconds,
                              StringFormat='{0} total seconds'}" />

        <Label Text="{Binding Source={x:Reference page},
                              Path=Content.Children.Count,
                              StringFormat='There are {0} children in this StackLayout'}" />

        <Label Text="{Binding Source={x:Static globe:CultureInfo.CurrentCulture},
                              Path=DateTimeFormat.DayNames[3],
                              StringFormat='The middle day of the week is {0}'}" />

        <Label>
            <Label.Text>
                <Binding Path="DateTimeFormat.DayNames[3]"
                         StringFormat="The middle day of the week in France is {0}">
                    <Binding.Source>
                        <globe:CultureInfo>
                            <x:Arguments>
                                <x:String>fr-FR</x:String>
                            </x:Arguments>
                        </globe:CultureInfo>
                    </Binding.Source>
                </Binding>
            </Label.Text>
        </Label>

        <Label Text="{Binding Source={x:Reference page},
                              Path=Content.Children[1].Text.Length,
                              StringFormat='The second Label has {0} characters'}" />
    </StackLayout>
</ContentPage>
```

In the second `Label`, the binding source is the page itself. The `Content` property is of type `StackLayout`, which has a `Children` property of type `IList<View>`, which has a `Count` property indicating the number of children.

## Paths with Indexers

The binding in the third `Label` in the **Path Variations** pages references the [`CultureInfo`](xref:System.Globalization.CultureInfo) class in the `System.Globalization` namespace:

```xaml
<Label Text="{Binding Source={x:Static globe:CultureInfo.CurrentCulture},
                      Path=DateTimeFormat.DayNames[3],
                      StringFormat='The middle day of the week is {0}'}" />
```

The source is set to the static `CultureInfo.CurrentCulture` property, which is an object of type `CultureInfo`. That class defines a property named `DateTimeFormat` of type [`DateTimeFormatInfo`](xref:System.Globalization.DateTimeFormatInfo) that contains a `DayNames` collection. The index selects the fourth item.

The fourth `Label` does something similar but for the culture associated with France. The `Source` property of the binding is set to `CultureInfo` object with a constructor:

```xaml
<Label>
    <Label.Text>
        <Binding Path="DateTimeFormat.DayNames[3]"
                 StringFormat="The middle day of the week in France is {0}">
            <Binding.Source>
                <globe:CultureInfo>
                    <x:Arguments>
                        <x:String>fr-FR</x:String>
                    </x:Arguments>
                </globe:CultureInfo>
            </Binding.Source>
        </Binding>
    </Label.Text>
</Label>
```

See [Passing Constructor Arguments](~/xamarin-forms/xaml/passing-arguments.md#passing-constructor-arguments) for more details on specifying constructor arguments in XAML.

Finally, the last example is similar to the second, except that it references one of the children of the `StackLayout`:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      Path=Content.Children[1].Text.Length,
                      StringFormat='The first Label has {0} characters'}" />
```

That child is a `Label`, which has a `Text` property of type `String`, which has a `Length` property. The first `Label` reports the `TimeSpan` set in the `TimePicker`, so when that text changes, the final `Label` changes as well.

Here's the program running:

![Path Variations.](binding-path-images/pathvariations.png)

## Debugging Complex Paths

Complex path definitions can be difficult to construct: You need to know the type of each sub-property or the type of items in the collection to correctly add the next sub-property, but the types themselves do not appear in the path. One good technique is to build up the path incrementally and look at the intermediate results. For that last example, you could start with no `Path` definition at all:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      StringFormat='{0}'}" />
```

That displays the type of the binding source, or `DataBindingDemos.PathVariationsPage`. You know `PathVariationsPage` derives from `ContentPage`, so it has a `Content` property:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      Path=Content,
                      StringFormat='{0}'}" />
```

The type of the `Content` property is now revealed to be `Xamarin.Forms.StackLayout`. Add the `Children` property to the `Path` and the type is `Xamarin.Forms.ElementCollection'1[Xamarin.Forms.View]`, which is a class internal to .NET MAUI, but obviously a collection type. Add an index to that and the type is `Xamarin.Forms.Label`. Continue in this way.

As .NET MAUI processes the binding path, it installs a `PropertyChanged` handler on any object in the path that implements the `INotifyPropertyChanged` interface. For example, the final binding reacts to a change in the first `Label` because the `Text` property changes.

If a property in the binding path does not implement `INotifyPropertyChanged`, any changes to that property will be ignored. Some changes could entirely invalidate the binding path, so you should use this technique only when the string of properties and sub-properties never become invalid.
