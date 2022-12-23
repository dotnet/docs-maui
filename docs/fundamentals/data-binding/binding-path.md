---
title: "Binding path"
description: ".NET MAUI data bindings can be used to access sub-properties and collection members with the Path property of the Binding class."
ms.date: 01/19/2022
---

# Binding path

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

In .NET Multi-platform App UI (.NET MAUI), the `Path` property of the `Binding` class (or the `Path` property of the `Binding` markup extension) can be set to a single property, to a *sub-property* (a property of a property), or to a member of a collection.

For example, suppose a page contains a <xref:Microsoft.Maui.Controls.TimePicker>:

```xaml
<TimePicker x:Name="timePicker">
```

The `Time` property of <xref:Microsoft.Maui.Controls.TimePicker> is of type `TimeSpan`, and it has a `TotalSeconds` property. A data binding can be created that references the `TotalSeconds` property of that `TimeSpan` value:

```xaml
{Binding Source={x:Reference timePicker},
         Path=Time.TotalSeconds}
```

The `Time` and `TotalSeconds` properties are simply connected with a period.

> [!NOTE]
> The items in the `Path` string always refer to properties and not to the types of these properties.

The following XAML shows multiple examples of binding to sub-properties:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:globe="clr-namespace:System.Globalization;assembly=netstandard"
             x:Class="DataBindingDemos.PathVariationsPage"
             Title="Path Variations"
             x:Name="page">
    <ContentPage.Resources>
        <Style TargetType="Label">
            <Setter Property="FontSize" Value="18" />
            <Setter Property="HorizontalTextAlignment" Value="Center" />
            <Setter Property="VerticalOptions" Value="Center" />
        </Style>
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

In the second <xref:Microsoft.Maui.Controls.Label>, the binding source is the page itself. The `Content` property is of type <xref:Microsoft.Maui.Controls.StackLayout>, which has a `Children` property of type `IList<View>`, which has a `Count` property indicating the number of children.

## Paths with indexers

In the example above, the binding in the third <xref:Microsoft.Maui.Controls.Label> references the [`CultureInfo`](xref:System.Globalization.CultureInfo) class in the `System.Globalization` namespace:

```xaml
<Label Text="{Binding Source={x:Static globe:CultureInfo.CurrentCulture},
                      Path=DateTimeFormat.DayNames[3],
                      StringFormat='The middle day of the week is {0}'}" />
```

The source is set to the static `CultureInfo.CurrentCulture` property, which is an object of type `CultureInfo`. That class defines a property named `DateTimeFormat` of type [`DateTimeFormatInfo`](xref:System.Globalization.DateTimeFormatInfo) that contains a `DayNames` collection. The index selects the fourth item.

The fourth <xref:Microsoft.Maui.Controls.Label> does something similar but for the culture associated with France. The `Source` property of the binding is set to `CultureInfo` object with a constructor:

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

For more information about specifying constructor arguments in XAML, see [Pass constructor arguments](~/xaml/pass-arguments.md#pass-constructor-arguments).

The last <xref:Microsoft.Maui.Controls.Label> is similar to the second, except that it references one of the children of the <xref:Microsoft.Maui.Controls.StackLayout>:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      Path=Content.Children[1].Text.Length,
                      StringFormat='The first Label has {0} characters'}" />
```

That child is a <xref:Microsoft.Maui.Controls.Label>, which has a `Text` property of type `String`, which has a `Length` property. The first <xref:Microsoft.Maui.Controls.Label> reports the `TimeSpan` set in the <xref:Microsoft.Maui.Controls.TimePicker>, so when that text changes, the final <xref:Microsoft.Maui.Controls.Label> changes as well:

:::image type="content" source="media/binding-path/pathvariations.png" alt-text="Path variations.":::

## Debug complex paths

Complex path definitions can be difficult to construct. You need to know the type of each sub-property or the type of items in the collection to correctly add the next sub-property, but the types themselves do not appear in the path. One technique is to build up the path incrementally and look at the intermediate results. For that last example, you could start with no `Path` definition at all:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      StringFormat='{0}'}" />
```

That displays the type of the binding source, or `DataBindingDemos.PathVariationsPage`. You know `PathVariationsPage` derives from <xref:Microsoft.Maui.Controls.ContentPage>, so it has a `Content` property:

```xaml
<Label Text="{Binding Source={x:Reference page},
                      Path=Content,
                      StringFormat='{0}'}" />
```

The type of the `Content` property is now revealed to be `Microsoft.Maui.Controls.StackLayout`. Add the `Children` property to the `Path` and the type is also `Microsoft.Maui.Controls.StackLayout`. Add an index to that and the type is `Microsoft.Maui.Controls.Label`. Continue in this way.

As .NET MAUI processes the binding path, it installs a `PropertyChanged` handler on any object in the path that implements the `INotifyPropertyChanged` interface. For example, the final binding reacts to a change in the first <xref:Microsoft.Maui.Controls.Label> because the `Text` property changes. If a property in the binding path does not implement `INotifyPropertyChanged`, any changes to that property will be ignored. Some changes could entirely invalidate the binding path, so you should use this technique only when the string of properties and sub-properties never become invalid.
