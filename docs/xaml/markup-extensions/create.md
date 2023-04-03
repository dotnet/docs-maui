---
title: "Create XAML markup extensions"
description: "This article explains how to define your own custom .NET MAUI XAML markup extensions. A XAML markup extension is a class that implements the IMarkupExtension or IMarkupExtension<T> interface."
ms.date: 01/27/2022
---

# Create XAML markup extensions

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/xaml-markupextensions)

At the developer level, a .NET Multi-platform App UI (.NET MAUI) XAML markup extension is a class that implements the `IMarkupExtension` or `IMarkupExtension<T>` interface. It's also possible to define your own custom XAML markup extensions by deriving from `IMarkupExtension` or `IMarkupExtension<T>`. Use the generic form if the markup extension obtains a value of a particular type. This is the case with several of the .NET MAUI markup extensions:

- `TypeExtension` derives from `IMarkupExtension<Type>`
- `ArrayExtension` derives from `IMarkupExtension<Array>`
- `DynamicResourceExtension` derives from `IMarkupExtension<DynamicResource>`
- `BindingExtension` derives from `IMarkupExtension<BindingBase>`

The two `IMarkupExtension` interfaces define only one method each, named `ProvideValue`:

```csharp
public interface IMarkupExtension
{
    object ProvideValue(IServiceProvider serviceProvider);
}

public interface IMarkupExtension<out T> : IMarkupExtension
{
    new T ProvideValue(IServiceProvider serviceProvider);
}
```

Since `IMarkupExtension<T>` derives from `IMarkupExtension` and includes the `new` keyword on `ProvideValue`, it contains both `ProvideValue` methods.

Often, XAML markup extensions define properties that contribute to the return value, and the `ProvideValue` method has a single argument of type `IServiceProvider`. For more information about service providers, see [Service providers](#service-providers).

## Create a markup extension

The following XAML markup extension demonstrates how to create your own markup extension. It allows you to construct a <xref:Microsoft.Maui.Graphics.Color> value using hue, saturation, and luminosity components. It defines four properties for the four components of the color, including an alpha component that is initialized to 1. The class derives from `IMarkupExtension<Color>` to indicate a <xref:Microsoft.Maui.Graphics.Color> return value:

```csharp
public class HslColorExtension : IMarkupExtension<Color>
{
    public float H { get; set; }
    public float S { get; set; }
    public float L { get; set; }
    public float A { get; set; } = 1.0f;

    public Color ProvideValue(IServiceProvider serviceProvider)
    {
        return Color.FromHsla(H, S, L, A);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<Color>).ProvideValue(serviceProvider);
    }
}
```

Because `IMarkupExtension<T>` derives from `IMarkupExtension`, the class must contain two `ProvideValue` methods, one that returns a <xref:Microsoft.Maui.Graphics.Color> and another that returns an `object`, but the second method can call the first method.

## Consume a markup extension

The following XAML demonstrates a variety of approaches that can be used to invoke the `HslColorExtension` to specify the color for a <xref:Microsoft.Maui.Controls.BoxView>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MarkupExtensions"
             x:Class="MarkupExtensions.HslColorDemoPage"
             Title="HSL Color Demo">
    <ContentPage.Resources>
        <Style TargetType="BoxView">
            <Setter Property="WidthRequest" Value="80" />
            <Setter Property="HeightRequest" Value="80" />
            <Setter Property="HorizontalOptions" Value="Center" />
            <Setter Property="VerticalOptions" Value="Center" />
        </Style>
    </ContentPage.Resources>

    <StackLayout>
        <BoxView>
            <BoxView.Color>
                <local:HslColorExtension H="0" S="1" L="0.5" A="1" />
            </BoxView.Color>
        </BoxView>
        <BoxView>
            <BoxView.Color>
                <local:HslColor H="0.33" S="1" L="0.5" />
            </BoxView.Color>
        </BoxView>
        <BoxView Color="{local:HslColorExtension H=0.67, S=1, L=0.5}" />
        <BoxView Color="{local:HslColor H=0, S=0, L=0.5}" />
        <BoxView Color="{local:HslColor A=0.5}" />
    </StackLayout>
</ContentPage>
```

In this example, when `HslColorExtension` is an XML tag the four properties are set as attributes, but when it appears between curly braces, the four properties are separated by commas without quotation marks. The default values for `H`, `S`, and `L` are 0, and the default value of `A` is 1, so those properties can be omitted if you want them set to default values. The last example shows an example where the luminosity is 0, which normally results in black, but the alpha channel is 0.5, so it is half transparent and appears gray against the white background of the page:

:::image type="content" source="media/create/hslcolordemo.png" alt-text="HSL color demo.":::

## Service providers

By using the `IServiceProvider` argument to `ProvideValue`, XAML markup extensions can get access to data about the XAML file in which they're being used. For example, the `IProvideValueTarget` service enables you to retrieve data about the object the markup extension is applied to:

```csharp
IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
```

The `IProvideValueTarget` interface defines two properties, `TargetObject` and `TargetProperty`. When this information is obtained in the `HslColorExtension` class, `TargetObject` is the <xref:Microsoft.Maui.Controls.BoxView> and `TargetProperty` is the `Color` property of <xref:Microsoft.Maui.Controls.BoxView>. This is the property on which the XAML markup extension has been set.
