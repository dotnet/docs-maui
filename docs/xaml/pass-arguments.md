---
title: "Pass arguments"
description: "In .NET MAUI XAML attributes can be used to pass arguments to non-default constructors, to call factory methods, and to specify the type of a generic argument."
ms.date: 01/24/2022
---

# Pass arguments

It's often necessary to instantiate objects with constructors that require arguments, or by calling a static creation method. This can be achieved in .NET Multi-platform App UI (.NET MAUI) XAML by using the `x:Arguments` and `x:FactoryMethod` attributes:

- The `x:Arguments` attribute is used to specify constructor arguments for a non-default constructor, or for a factory method object declaration. For more information, see [Pass constructor arguments](#pass-constructor-arguments).
- The `x:FactoryMethod` attribute is used to specify a factory method that can be used to initialize an object. For more information, see [Call factory methods](#call-factory-methods).

In addition, the `x:TypeArguments` attribute can be used to specify the generic type arguments to the constructor of a generic type. For more information, see [Specify a generic type argument](#specify-a-generic-type-argument).

Arguments can be passed to constructors and factory methods using the following .NET MAUI XAML language primitives:

- `x:Array`, which corresponds to [`Array`](xref:System.Array).
- `x:Boolean`, which corresponds to [`Boolean`](xref:System.Boolean).
- `x:Byte`, which corresponds to [`Byte`](xref:System.Byte).
- `x:Char`, which corresponds to [`Char`](xref:System.Char).
- `x:DateTime`, which corresponds to [`DateTime`](xref:System.DateTime).
- `x:Decimal`, which corresponds to [`Decimal`](xref:System.Decimal).
- `x:Double`, which corresponds to [`Double`](xref:System.Double).
- `x:Int16`, which corresponds to [`Int16`](xref:System.Int16).
- `x:Int32`, which corresponds to [`Int32`](xref:System.Int32).
- `x:Int64`, which corresponds to [`Int64`](xref:System.Int64).
- `x:Object`, which corresponds to the [`Object`](xref:System.Object).
- `x:Single`, which corresponds to [`Single`](xref:System.Single).
- `x:String`, which corresponds to [`String`](xref:System.String).
- `x:TimeSpan`, which corresponds to [`TimeSpan`](xref:System.TimeSpan).

With the exception of `x:DateTime`, the other language primitives are in the XAML 2009 specification.

> [!NOTE]
> The `x:Single` language primitive can be used to pass `float` arguments.

## Pass constructor arguments

Arguments can be passed to a non-default constructor using the `x:Arguments` attribute. Each constructor argument must be delimited within an XML element that represents the type of the argument.

The following example demonstrates using the `x:Arguments` attribute with three different <xref:Microsoft.Maui.Graphics.Color> constructors:

```xaml
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
    <BoxView.Color>
        <Color>
            <x:Arguments>
                <x:Single>0.9</x:Single>
            </x:Arguments>
        </Color>
    </BoxView.Color>
</BoxView>
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
    <BoxView.Color>
        <Color>
            <x:Arguments>
                <x:Single>0.25</x:Single>
                <x:Single>0.5</x:Single>
                <x:Single>0.75</x:Single>
            </x:Arguments>
        </Color>
    </BoxView.Color>
</BoxView>
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
    <BoxView.Color>
        <Color>
            <x:Arguments>
                <x:Single>0.8</x:Single>
                <x:Single>0.5</x:Single>
                <x:Single>0.2</x:Single>
                <x:Single>0.5</x:Single>
            </x:Arguments>
        </Color>
    </BoxView.Color>
</BoxView>
```

The number of elements within the `x:Arguments` tag, and the types of these elements, must match one of the <xref:Microsoft.Maui.Graphics.Color> constructors. The <xref:Microsoft.Maui.Graphics.Color> constructor with a single parameter requires a grayscale `float` value from 0 (black) to 1 (white). The <xref:Microsoft.Maui.Graphics.Color> constructor with three parameters requires `float` red, green, and blue values ranging from 0 to 1. The <xref:Microsoft.Maui.Graphics.Color> constructor with four parameters adds a `float` alpha channel as the fourth parameter.

## Call factory methods

Factory methods can be called in .NET MAUI XAML by specifying the method's name using the `x:FactoryMethod` attribute, and its arguments using the `x:Arguments` attribute. A factory method is a `public static` method that returns objects or values of the same type as the class or structure that defines the methods.

The <xref:Microsoft.Maui.Graphics.Color> class defines a number of factory methods, and the following example demonstrates calling three of them:

```xaml
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
  <BoxView.Color>
    <Color x:FactoryMethod="FromRgba">
      <x:Arguments>
        <x:Byte>192</x:Byte>
        <x:Byte>75</x:Byte>
        <x:Byte>150</x:Byte>
        <x:Byte>128</x:Byte>
      </x:Arguments>
    </Color>
  </BoxView.Color>
</BoxView>
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
  <BoxView.Color>
    <Color x:FactoryMethod="FromHsla">
      <x:Arguments>
        <x:Double>0.23</x:Double>
        <x:Double>0.42</x:Double>
        <x:Double>0.69</x:Double>
        <x:Double>0.7</x:Double>
      </x:Arguments>
    </Color>
  </BoxView.Color>
</BoxView>
<BoxView HeightRequest="150"
         WidthRequest="150"
         HorizontalOptions="Center">
  <BoxView.Color>
    <Color x:FactoryMethod="FromHex">
      <x:Arguments>
        <x:String>#FF048B9A</x:String>
      </x:Arguments>
    </Color>
  </BoxView.Color>
</BoxView>
```

The number of elements within the `x:Arguments` tag, and the types of these elements, must match the arguments of the factory method being called. The `FromRgba` factory method requires four `byte` arguments, which represent the red, green, blue, and alpha values, ranging from 0 to 255 respectively. The `FromHsla` factory method requires four `float` arguments, which represent the hue, saturation, luminosity, and alpha values, ranging from 0 to 1 respectively. The `FromHex` factory method requires a `string` argument that represents the hexadecimal (A)RGB color.

## Specify a generic type argument

Generic type arguments for the constructor of a generic type can be specified using the `x:TypeArguments` attribute, as demonstrated in the following example:

```xaml
<StackLayout>
    <StackLayout.Margin>
        <OnPlatform x:TypeArguments="Thickness">
          <On Platform="iOS" Value="0,20,0,0" />
          <On Platform="Android" Value="5, 10" />
        </OnPlatform>
    </StackLayout.Margin>
</StackLayout>
```

The `OnPlatform` class is a generic class and must be instantiated with an `x:TypeArguments` attribute that matches the target type. In the `On` class, the `Platform` attribute can accept a single `string` value, or multiple comma-delimited `string` values. In this example, the `StackLayout.Margin` property is set to a platform-specific `Thickness`.

For more information about generic type arguments, see [Generics in XAML](generics.md).
