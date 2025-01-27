---
title: "Generics"
description: ".NET MAUI XAML provides support for consuming generic CLR types by specifying the generic constraints as type arguments."
ms.date: 01/27/2025
---

# Generics

.NET Multi-platform App UI (.NET MAUI) XAML provides support for consuming generic CLR types by specifying the generic constraints as type arguments. This support is provided by the `x:TypeArguments` directive, which passes the constraining type arguments of a generic to the constructor of the generic type.

Type arguments are specified as a string, and are typically prefixed, such as `sys:String` and `sys:Int32`. Prefixing is required because the typical types of CLR generic constraints come from libraries that are not mapped to the default .NET MAUI namespaces. However, the XAML 2009 built-in types such as `x:String` and `x:Int32`, can also be specified as type arguments, where `x` is the XAML language namespace for XAML 2009. For more information about the XAML 2009 built-in types, see [XAML 2009 Language Primitives](/dotnet/desktop-wpf/xaml-services/types-for-primitives#xaml-2009-language-primitives).

> [!IMPORTANT]
> Defining generic types in .NET MAUI XAML, with the `x:TypeArguments` directive, is unsupported.

Multiple type arguments can be specified by using a comma delimiter. In addition, if a generic constraint uses generic types, the nested constraint type arguments should be contained in parentheses.

> [!NOTE]
> The `x:Type` markup extension supplies a Common Language Runtime (CLR) type reference for a generic type, and has a similar function to the `typeof` operator in C#. For more information, see [x:Type markup extension](~/xaml/markup-extensions/consume.md#xtype-markup-extension).

Specifying generic types in .NET MAUI XAML, with the `x:DataType` and `x:Type` directives is supported. For more information, see [Compile bindings that specify a generic type](~/fundamentals/data-binding/compiled-bindings.md#compile-bindings-that-specify-a-generic-type) and [x:Type markup extension](~/xaml/markup-extensions/consume.md#x-type-markup-extension).

## Single primitive type argument

A single primitive type argument can be specified as a prefixed string argument using the `x:TypeArguments` directive:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:scg="clr-namespace:System.Collections.Generic;assembly=netstandard"
             ...>
    <CollectionView>
        <CollectionView.ItemsSource>
            <scg:List x:TypeArguments="x:String">
                <x:String>Baboon</x:String>
                <x:String>Capuchin Monkey</x:String>
                <x:String>Blue Monkey</x:String>
                <x:String>Squirrel Monkey</x:String>
                <x:String>Golden Lion Tamarin</x:String>
                <x:String>Howler Monkey</x:String>
                <x:String>Japanese Macaque</x:String>
            </scg:List>
        </CollectionView.ItemsSource>
    </CollectionView>
</ContentPage>
```

In this example, `System.Collections.Generic` is defined as the `scg` XAML namespace. The `CollectionView.ItemsSource` property is set to a `List<T>` that's instantiated with a `string` type argument, using the XAML 2009 built-in `x:String` type. The `List<string>` collection is initialized with multiple `string` items.

Alternatively, but equivalently, the `List<T>` collection can be instantiated with the CLR `String` type:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:scg="clr-namespace:System.Collections.Generic;assembly=netstandard"
             xmlns:sys="clr-namespace:System;assembly=netstandard"
             ...>
    <CollectionView>
        <CollectionView.ItemsSource>
            <scg:List x:TypeArguments="sys:String">
                <sys:String>Baboon</sys:String>
                <sys:String>Capuchin Monkey</sys:String>
                <sys:String>Blue Monkey</sys:String>
                <sys:String>Squirrel Monkey</sys:String>
                <sys:String>Golden Lion Tamarin</sys:String>
                <sys:String>Howler Monkey</sys:String>
                <sys:String>Japanese Macaque</sys:String>
            </scg:List>
        </CollectionView.ItemsSource>
    </CollectionView>
</ContentPage>
```

## Single object type argument

A single object type argument can be specified as a prefixed string argument using the `x:TypeArguments` directive:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:models="clr-namespace:GenericsDemo.Models"
             xmlns:scg="clr-namespace:System.Collections.Generic;assembly=netstandard"
             ...>
    <CollectionView>
        <CollectionView.ItemsSource>
            <scg:List x:TypeArguments="models:Monkey">
                <models:Monkey Name="Baboon"
                               Location="Africa and Asia"
                               ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg" />
                <models:Monkey Name="Capuchin Monkey"
                               Location="Central and South America"
                               ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg" />
                <models:Monkey Name="Blue Monkey"
                               Location="Central and East Africa"
                               ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg" />
            </scg:List>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
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
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

In this example, `GenericsDemo.Models` is defined as the `models` XAML namespace, and `System.Collections.Generic` is defined as the `scg` XAML namespace. The `CollectionView.ItemsSource` property is set to a `List<T>` that's instantiated with a `Monkey` type argument. The `List<Monkey>` collection is initialized with multiple `Monkey` items, and a <xref:Microsoft.Maui.Controls.DataTemplate> that defines the appearance of each `Monkey` object is set as the `ItemTemplate` of the <xref:Microsoft.Maui.Controls.CollectionView>.

## Multiple type arguments

Multiple type arguments can be specified as prefixed string arguments, delimited by a comma, using the `x:TypeArguments` directive. When a generic constraint uses generic types, the nested constraint type arguments are contained in parentheses:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:models="clr-namespace:GenericsDemo.Models"
             xmlns:scg="clr-namespace:System.Collections.Generic;assembly=netstandard"
             ...>
    <CollectionView>
        <CollectionView.ItemsSource>
            <scg:List x:TypeArguments="scg:KeyValuePair(x:String,models:Monkey)">
                <scg:KeyValuePair x:TypeArguments="x:String,models:Monkey">
                    <x:Arguments>
                        <x:String>Baboon</x:String>
                        <models:Monkey Location="Africa and Asia"
                                       ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg" />
                    </x:Arguments>
                </scg:KeyValuePair>
                <scg:KeyValuePair x:TypeArguments="x:String,models:Monkey">
                    <x:Arguments>
                        <x:String>Capuchin Monkey</x:String>
                        <models:Monkey Location="Central and South America"
                                       ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg" />   
                    </x:Arguments>
                </scg:KeyValuePair>
                <scg:KeyValuePair x:TypeArguments="x:String,models:Monkey">
                    <x:Arguments>
                        <x:String>Blue Monkey</x:String>
                        <models:Monkey Location="Central and East Africa"
                                       ImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg" />
                    </x:Arguments>
                </scg:KeyValuePair>
            </scg:List>
        </CollectionView.ItemsSource>
        <CollectionView.ItemTemplate>
            <DataTemplate>
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
                           Source="{Binding Value.ImageUrl}"
                           Aspect="AspectFill"
                           HeightRequest="60"
                           WidthRequest="60" />
                    <Label Grid.Column="1"
                           Text="{Binding Key}"
                           FontAttributes="Bold" />
                    <Label Grid.Row="1"
                           Grid.Column="1"
                           Text="{Binding Value.Location}"
                           FontAttributes="Italic"
                           VerticalOptions="End" />
                </Grid>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage    
```

In this example, `GenericsDemo.Models` is defined as the `models` XAML namespace, and `System.Collections.Generic` is defined as the `scg` XAML namespace. The `CollectionView.ItemsSource` property is set to a `List<T>` that's instantiated with a `KeyValuePair<TKey, TValue>` constraint, with the inner constraint type arguments `string` and `Monkey`. The `List<KeyValuePair<string,Monkey>>` collection is initialized with multiple `KeyValuePair` items, using the non-default `KeyValuePair` constructor, and a <xref:Microsoft.Maui.Controls.DataTemplate> that defines the appearance of each `Monkey` object is set as the `ItemTemplate` of the <xref:Microsoft.Maui.Controls.CollectionView>. For information on passing arguments to a non-default constructor, see [Pass constructor arguments](pass-arguments.md#pass-constructor-arguments).
