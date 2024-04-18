---
title: "String formatting"
description: ".NET MAUI apps can use data bindings to format and display objects as strings. This is achieved by setting the StringFormat of the Binding to a standard .NET formatting string with a placeholder."
ms.date: 01/19/2022
---

# String formatting

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

In a .NET Multi-platform App UI (.NET MAUI) app, it's sometimes convenient to use data bindings to display the string representation of an object or value. For example, you might want to use a <xref:Microsoft.Maui.Controls.Label> to display the current value of a <xref:Microsoft.Maui.Controls.Slider>. In this data binding, the <xref:Microsoft.Maui.Controls.Slider> is the source, and the target is the `Text` property of the <xref:Microsoft.Maui.Controls.Label>.

String formatting in code is typically accomplished with the static [`String.Format`](xref:System.String.Format(System.String,System.Object)) method. The formatting string includes formatting codes specific to various types of objects, and you can include other text along with the values being formatted. See [Formatting Types in .NET](/dotnet/standard/base-types/formatting-types/) for more information on string formatting.

String formatting can also be accomplished with data bindings by setting the `StringFormat` property of `Binding` (or the `StringFormat` property of the `Binding` markup extension) to a standard .NET formatting string with a placeholder:

```xaml
<Slider x:Name="slider" />
<Label Text="{Binding Source={x:Reference slider},
                      Path=Value,
                      StringFormat='The slider value is {0:F2}'}" />
```

In XAML the formatting string is delimited by single-quote characters to help the XAML parser avoid treating the curly braces as another XAML markup extension. In this example, the formatting specification of `F2` causes the value to be displayed with two decimal places.

> [!NOTE]
> Using the `StringFormat` property only makes sense when the target property is of type `string`, and the binding mode is `OneWay` or `TwoWay`. For two-way bindings, the `StringFormat` is only applicable for values passing from the source to the target.

The following example demonstrates several uses of the `StringFormat` property:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:sys="clr-namespace:System;assembly=netstandard"
             x:Class="DataBindingDemos.StringFormattingPage"
             Title="String Formatting">

    <ContentPage.Resources>
        <Style TargetType="Label">
            <Setter Property="HorizontalTextAlignment" Value="Center" />
        </Style>
        <Style TargetType="BoxView">
            <Setter Property="Color" Value="Blue" />
            <Setter Property="HeightRequest" Value="2" />
            <Setter Property="Margin" Value="0, 5" />
        </Style>        
    </ContentPage.Resources>

    <StackLayout Margin="10">
        <Slider x:Name="slider" />
        <Label Text="{Binding Source={x:Reference slider},
                              Path=Value,
                              StringFormat='The slider value is {0:F2}'}" />
        <BoxView />
        <TimePicker x:Name="timePicker" />
        <Label Text="{Binding Source={x:Reference timePicker},
                              Path=Time,
                              StringFormat='The TimeSpan is {0:c}'}" />
        <BoxView />                              
        <Entry x:Name="entry" />
        <Label Text="{Binding Source={x:Reference entry},
                              Path=Text,
                              StringFormat='The Entry text is &quot;{0}&quot;'}" />
        <BoxView />
        <StackLayout BindingContext="{x:Static sys:DateTime.Now}">
            <Label Text="{Binding}" />
            <Label Text="{Binding Path=Ticks,
                                  StringFormat='{0:N0} ticks since 1/1/1'}" />
            <Label Text="{Binding StringFormat='The {{0:MMMM}} specifier produces {0:MMMM}'}" />
            <Label Text="{Binding StringFormat='The long date is {0:D}'}" />
        </StackLayout>
        <BoxView />        
        <StackLayout BindingContext="{x:Static sys:Math.PI}">
            <Label Text="{Binding}" />
            <Label Text="{Binding StringFormat='PI to 4 decimal points = {0:F4}'}" />
            <Label Text="{Binding StringFormat='PI in scientific notation = {0:E7}'}" />
        </StackLayout>
    </StackLayout>
</ContentPage>
```

In this example, the bindings on the <xref:Microsoft.Maui.Controls.Slider> and <xref:Microsoft.Maui.Controls.TimePicker> show the use of format specifications particular to `double` and `TimeSpan` data types. The `StringFormat` that displays the text from the <xref:Microsoft.Maui.Controls.Entry> view demonstrates how to specify double quotation marks in the formatting string with the use of the `&quot;` HTML entity.

The next section in the XAML file is a <xref:Microsoft.Maui.Controls.StackLayout> with a `BindingContext` set to an `x:Static` markup extension that references the static `DateTime.Now` property. The first binding has no properties:

```xaml
<Label Text="{Binding}" />
```

This simply displays the `DateTime` value of the `BindingContext` with default formatting. The second binding displays the `Ticks` property of `DateTime`, while the other two bindings display the `DateTime` itself with specific formatting.

> [!NOTE]
> If you need to display left or right curly braces in your formatting string, use a pair of them. For example, `StringFormat='{{0:MMMM}}'`.

The last section sets the `BindingContext` to the value of `Math.PI` and displays it with default formatting and two different types of numeric formatting:

:::image type="content" source="media/string-formatting/stringformatting.png" alt-text="String formatting.":::

## ViewModels and string formatting

When you're using <xref:Microsoft.Maui.Controls.Label> and `StringFormat` to display the value of a view that is also the target of a viewmodel, you can either define the binding from the view to the <xref:Microsoft.Maui.Controls.Label> or from the viewmodel to the <xref:Microsoft.Maui.Controls.Label>. In general, the second approach is best because it verifies that the bindings between the view and viewmodel are working.

This approach is shown in the following example:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.BetterColorSelectorPage"
             Title="Better Color Selector">
    <ContentPage.BindingContext>
        <local:HslColorViewModel Color="Sienna" />
    </ContentPage.BindingContext>

    <ContentPage.Resources>
        <ResourceDictionary>
            <Style TargetType="Slider">
                <Setter Property="VerticalOptions" Value="Center" />
            </Style>

            <Style TargetType="Label">
                <Setter Property="HorizontalTextAlignment" Value="Center" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>

    <StackLayout Margin="20">
        <BoxView Color="{Binding Color}"
                 HeightRequest="100"
                 WidthRequest="100"
                 HorizontalOptions="Center" />
        <StackLayout Margin="10, 0">
            <Label Text="{Binding Name}" />
            <Slider Value="{Binding Hue}" />
            <Label Text="{Binding Hue, StringFormat='Hue = {0:F2}'}" />
            <Slider Value="{Binding Saturation}" />
            <Label Text="{Binding Saturation, StringFormat='Saturation = {0:F2}'}" />
            <Slider Value="{Binding Luminosity}" />
            <Label Text="{Binding Luminosity, StringFormat='Luminosity = {0:F2}'}" />
        </StackLayout>
    </StackLayout>
</ContentPage>    
```

In this example, there are three pairs of <xref:Microsoft.Maui.Controls.Slider> and <xref:Microsoft.Maui.Controls.Label> elements that are bound to the same source property in the `HslColorViewModel` object. Each <xref:Microsoft.Maui.Controls.Label> that accompanies a <xref:Microsoft.Maui.Controls.Slider> has a `StringFormat` property to display each <xref:Microsoft.Maui.Controls.Slider> value:

:::image type="content" source="media/string-formatting/bettercolorselector.png" alt-text="Better color selector.":::
