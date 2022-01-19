---
title: "Binding Value Converters"
description: "This article explains how to cast or convert values within a .NET MAUI data binding by implementing a value converter (which is also known as a binding converter, or binding value converter)."
ms.date: 01/19/2022
---

# Binding Value Converters

A .NET Multi-platform App UI (.NET MAUI)

Data bindings usually transfer data from a source property to a target property, and in some cases from the target property to the source property. This transfer is straightforward when the source and target properties are of the same type, or when one type can be converted to the other type through an implicit conversion. When that is not the case, a type conversion must take place.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In the [**String Formatting**](string-formatting.md) article, you saw how you can use the `StringFormat` property of a data binding to convert any type into a string. For other types of conversions, you need to write some specialized code in a class that implements the `IValueConverter` interface. (The Universal Windows Platform contains a similar class named [`IValueConverter`](/uwp/api/Windows.UI.Xaml.Data.IValueConverter/) in the `Windows.UI.Xaml.Data` namespace, but this `IValueConverter` is in the `.NET MAUI` namespace.) Classes that implement `IValueConverter` are called *value converters*, but they are also often referred to as *binding converters* or *binding value converters*.

## The IValueConverter Interface

Suppose you want to define a data binding where the source property is of type `int` but the target property is a `bool`. You want this data binding to produce a `false` value when the integer source is equal to 0, and `true` otherwise.  

You can do this with a class that implements the `IValueConverter` interface:

```csharp
public class IntToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 1 : 0;
    }
}
```

You set an instance of this class to the `Converter` property of the `Binding` class or to the `Converter` property of the `Binding` markup extension. This class becomes part of the data binding.

The `Convert` method is called when data moves from the source to the target in `OneWay` or `TwoWay` bindings. The `value` parameter is the object or value from the data-binding source. The method must return a value of the type of the data-binding target. The method shown here casts the `value` parameter to an `int` and then compares it with 0 for a `bool` return value.

The `ConvertBack` method is called when data moves from the target to the source in `TwoWay` or `OneWayToSource` bindings. `ConvertBack` performs the opposite conversion: It assumes the `value` parameter is a `bool` from the target, and converts it to an `int` return value for the source.

If the data binding also includes a `StringFormat` setting, the value converter is invoked before the result is formatted as a string.

The **Enable Buttons** page in the [**Data Binding Demos**](/samples/xamarin/xamarin-forms-samples/databindingdemos) sample demonstrates how to use this value converter in a data binding. The `IntToBoolConverter` is instantiated in the page's resource dictionary. It is then referenced with a `StaticResource` markup extension to set the `Converter` property in two data bindings. It is very common to share data converters among multiple data bindings on the page:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.EnableButtonsPage"
             Title="Enable Buttons">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:IntToBoolConverter x:Key="intToBool" />
        </ResourceDictionary>
    </ContentPage.Resources>

    <StackLayout Padding="10, 0">
        <Entry x:Name="entry1"
               Text=""
               Placeholder="enter search term"
               VerticalOptions="CenterAndExpand" />

        <Button Text="Search"
                HorizontalOptions="Center"
                VerticalOptions="CenterAndExpand"
                IsEnabled="{Binding Source={x:Reference entry1},
                                    Path=Text.Length,
                                    Converter={StaticResource intToBool}}" />

        <Entry x:Name="entry2"
               Text=""
               Placeholder="enter destination"
               VerticalOptions="CenterAndExpand" />

        <Button Text="Submit"
                HorizontalOptions="Center"
                VerticalOptions="CenterAndExpand"
                IsEnabled="{Binding Source={x:Reference entry2},
                                    Path=Text.Length,
                                    Converter={StaticResource intToBool}}" />
    </StackLayout>
</ContentPage>
```

If a value converter is used in multiple pages of your application, you can instantiate it in the resource dictionary in the **App.xaml** file.

The **Enable Buttons** page demonstrates a common need when a `Button` performs an operation based on text that the user types into an `Entry` view. If nothing has been typed into the `Entry`, the `Button` should be disabled. Each `Button` contains a data binding on its `IsEnabled` property. The data-binding source is the `Length` property of the `Text` property of the corresponding `Entry`. If that `Length` property is not 0, the value converter returns `true` and the `Button` is enabled:

:::image type="content" source="media/converters/enablebuttons.png" alt-text="Enable buttons.":::

Notice that the `Text` property in each `Entry` is initialized to an empty string. The `Text` property is `null` by default, and the data binding will not work in that case.

Some value converters are written specifically for particular applications, while others are generalized. If you know that a value converter will only be used in `OneWay` bindings, then the `ConvertBack` method can simply return `null`.

The `Convert` method shown above implicitly assumes that the `value` argument is of type `int` and the return value must be of type `bool`. Similarly, the `ConvertBack` method assumes that the `value` argument is of type `bool` and the return value is `int`. If that is not the case, a runtime exception will occur.

You can write value converters to be more generalized and to accept several different types of data. The `Convert` and `ConvertBack` methods can use the `as` or `is` operators with the `value` parameter, or can call `GetType` on that parameter to determine its type, and then do something appropriate. The expected type of each method's return value is given by the `targetType` parameter. Sometimes, value converters are used with data bindings of different target types; the value converter can use the `targetType` argument to perform a conversion for the correct type.

If the conversion being performed is different for different cultures, use the `culture` parameter for this purpose. The `parameter` argument to `Convert` and `ConvertBack` is discussed later in this article.

## Binding Converter Properties

Value converter classes can have properties and generic parameters. This particular value converter converts a `bool` from the source to an object of type `T` for the target:

```csharp
public class BoolToObjectConverter<T> : IValueConverter
{
    public T TrueObject { set; get; }

    public T FalseObject { set; get; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? TrueObject : FalseObject;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((T)value).Equals(TrueObject);
    }
}
```

The **Switch Indicators** page demonstrates how it can be used to display the value of a `Switch` view. Although it's common to instantiate value converters as resources in a resource dictionary, this page demonstrates an alternative: Each value converter is instantiated between `Binding.Converter` property-element tags. The `x:TypeArguments` indicates the generic argument, and `TrueObject` and `FalseObject` are both set to objects of that type:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.SwitchIndicatorsPage"
             Title="Switch Indicators">
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style TargetType="Label">
                <Setter Property="FontSize" Value="18" />
                <Setter Property="VerticalOptions" Value="Center" />
            </Style>

            <Style TargetType="Switch">
                <Setter Property="VerticalOptions" Value="Center" />
            </Style>
        </ResourceDictionary>
    </ContentPage.Resources>

    <StackLayout Padding="10, 0">
        <StackLayout Orientation="Horizontal"
                     VerticalOptions="CenterAndExpand">
            <Label Text="Subscribe?" />
            <Switch x:Name="switch1" />
            <Label>
                <Label.Text>
                    <Binding Source="{x:Reference switch1}"
                             Path="IsToggled">
                        <Binding.Converter>
                            <local:BoolToObjectConverter x:TypeArguments="x:String"
                                                         TrueObject="Of course!"
                                                         FalseObject="No way!" />
                        </Binding.Converter>
                    </Binding>
                </Label.Text>
            </Label>
        </StackLayout>

        <StackLayout Orientation="Horizontal"
                     VerticalOptions="CenterAndExpand">
            <Label Text="Allow popups?" />
            <Switch x:Name="switch2" />
            <Label>
                <Label.Text>
                    <Binding Source="{x:Reference switch2}"
                             Path="IsToggled">
                        <Binding.Converter>
                            <local:BoolToObjectConverter x:TypeArguments="x:String"
                                                         TrueObject="Yes"
                                                         FalseObject="No" />
                        </Binding.Converter>
                    </Binding>
                </Label.Text>
                <Label.TextColor>
                    <Binding Source="{x:Reference switch2}"
                             Path="IsToggled">
                        <Binding.Converter>
                            <local:BoolToObjectConverter x:TypeArguments="Color"
                                                         TrueObject="Green"
                                                         FalseObject="Red" />
                        </Binding.Converter>
                    </Binding>
                </Label.TextColor>
            </Label>
        </StackLayout>

        <StackLayout Orientation="Horizontal"
                     VerticalOptions="CenterAndExpand">
            <Label Text="Learn more?" />
            <Switch x:Name="switch3" />
            <Label FontSize="18"
                   VerticalOptions="Center">
                <Label.Style>
                    <Binding Source="{x:Reference switch3}"
                             Path="IsToggled">
                        <Binding.Converter>
                            <local:BoolToObjectConverter x:TypeArguments="Style">
                                <local:BoolToObjectConverter.TrueObject>
                                    <Style TargetType="Label">
                                        <Setter Property="Text" Value="Indubitably!" />
                                        <Setter Property="FontAttributes" Value="Italic, Bold" />
                                        <Setter Property="TextColor" Value="Green" />
                                    </Style>                                    
                                </local:BoolToObjectConverter.TrueObject>

                                <local:BoolToObjectConverter.FalseObject>
                                    <Style TargetType="Label">
                                        <Setter Property="Text" Value="Maybe later" />
                                        <Setter Property="FontAttributes" Value="None" />
                                        <Setter Property="TextColor" Value="Red" />
                                    </Style>
                                </local:BoolToObjectConverter.FalseObject>
                            </local:BoolToObjectConverter>
                        </Binding.Converter>
                    </Binding>
                </Label.Style>
            </Label>
        </StackLayout>
    </StackLayout>
</ContentPage>
```

In the last of the three `Switch` and `Label` pairs, the generic argument is set to `Style`, and entire `Style` objects are provided for the values of `TrueObject` and `FalseObject`. These override the implicit style for `Label` set in the resource dictionary, so the properties in that style are explicitly assigned to the `Label`. Toggling the `Switch` causes the corresponding `Label` to reflect the change:

:::image type="content" source="media/converters/switchindicators.png" alt-text="Switch indicators.":::

It's also possible to use [`Triggers`](~/xamarin-forms/app-fundamentals/triggers.md) to implement similar changes in the user-interface based on other views.

## Binding Converter Parameters

The `Binding` class defines a `ConverterParameter` property, and the `Binding` markup extension also defines a `ConverterParameter` property. If this property is set, then the value is passed to the `Convert` and `ConvertBack` methods as the `parameter` argument. Even if the instance of the value converter is shared among several data bindings, the `ConverterParameter` can be different to perform somewhat different conversions.

The use of `ConverterParameter` is demonstrated with a color-selection program. In this case, the `RgbColorViewModel` has three properties of type `double` named `Red`, `Green`, and `Blue` that it uses to construct a `Color` value:

```csharp
public class RgbColorViewModel : INotifyPropertyChanged
{
    Color color;
    string name;

    public event PropertyChangedEventHandler PropertyChanged;

    public double Red
    {
        set
        {
            if (color.R != value)
            {
                Color = new Color(value, color.G, color.B);
            }
        }
        get
        {
            return color.R;
        }
    }

    public double Green
    {
        set
        {
            if (color.G != value)
            {
                Color = new Color(color.R, value, color.B);
            }
        }
        get
        {
            return color.G;
        }
    }

    public double Blue
    {
        set
        {
            if (color.B != value)
            {
                Color = new Color(color.R, color.G, value);
            }
        }
        get
        {
            return color.B;
        }
    }

    public Color Color
    {
        set
        {
            if (color != value)
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Red"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Green"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Blue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));

                Name = NamedColor.GetNearestColorName(color);
            }
        }
        get
        {
            return color;
        }
    }

    public string Name
    {
        private set
        {
            if (name != value)
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        get
        {
            return name;
        }
    }
}
```

The `Red`, `Green`, and `Blue` properties range between 0 and 1. However, you might prefer that the components be displayed as two-digit hexadecimal values.

To display these as hexadecimal values in XAML, they must be multiplied by 255, converted to an integer, and then formatted with a specification of "X2" in the `StringFormat` property. The first two tasks (multiplying by 255 and converting to an integer) can be handled by the value converter. To make the value converter as generalized as possible, the multiplication factor can be specified with the `ConverterParameter` property, which means that it enters the `Convert` and `ConvertBack` methods as the `parameter` argument:

```csharp
public class DoubleToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)Math.Round((double)value * GetParameter(parameter));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value / GetParameter(parameter);
    }

    double GetParameter(object parameter)
    {
        if (parameter is double)
            return (double)parameter;

        else if (parameter is int)
            return (int)parameter;

        else if (parameter is string)
            return double.Parse((string)parameter);

        return 1;
    }
}
```

The `Convert` converts from a `double` to `int` while multiplying by the `parameter` value; the `ConvertBack` divides the integer `value` argument by `parameter` and returns a `double` result. (In the program shown below, the value converter is used only in connection with string formatting, so `ConvertBack` is not used.)

The type of the `parameter` argument is likely to be different depending on whether the data binding is defined in code or XAML. If the `ConverterParameter` property of `Binding` is set in code, it's likely to be set to a numeric value:

```csharp
binding.ConverterParameter = 255;
```

The `ConverterParameter` property is of type `Object`, so the C# compiler interprets the literal 255 as an integer, and sets the property to that value.

In XAML, however, the `ConverterParameter` is likely to be set like this:

```xaml
<Label Text="{Binding Red,
                      Converter={StaticResource doubleToInt},
                      ConverterParameter=255,
                      StringFormat='Red = {0:X2}'}" />
```

The 255 looks like a number, but because `ConverterParameter` is of type `Object`, the XAML parser treats the 255 as a string.

For that reason, the value converter shown above includes a separate `GetParameter` method that handles cases for `parameter` being of type `double`, `int`, or `string`.  

The **RGB Color Selector** page instantiates `DoubleToIntConverter` in its resource dictionary following the definition of two implicit styles:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.RgbColorSelectorPage"
             Title="RGB Color Selector">
    <ContentPage.Resources>
        <ResourceDictionary>
            <Style TargetType="Slider">
                <Setter Property="VerticalOptions" Value="CenterAndExpand" />
            </Style>

            <Style TargetType="Label">
                <Setter Property="HorizontalTextAlignment" Value="Center" />
            </Style>

            <local:DoubleToIntConverter x:Key="doubleToInt" />
        </ResourceDictionary>
    </ContentPage.Resources>

    <StackLayout>
        <StackLayout.BindingContext>
            <local:RgbColorViewModel Color="Gray" />
        </StackLayout.BindingContext>

        <BoxView Color="{Binding Color}"
                 VerticalOptions="FillAndExpand" />

        <StackLayout Margin="10, 0">
            <Label Text="{Binding Name}" />

            <Slider Value="{Binding Red}" />
            <Label Text="{Binding Red,
                                  Converter={StaticResource doubleToInt},
                                  ConverterParameter=255,
                                  StringFormat='Red = {0:X2}'}" />

            <Slider Value="{Binding Green}" />
            <Label Text="{Binding Green,
                                  Converter={StaticResource doubleToInt},
                                  ConverterParameter=255,
                                  StringFormat='Green = {0:X2}'}" />

            <Slider Value="{Binding Blue}" />
            <Label>
                <Label.Text>
                    <Binding Path="Blue"
                             StringFormat="Blue = {0:X2}"
                             Converter="{StaticResource doubleToInt}">
                        <Binding.ConverterParameter>
                            <x:Double>255</x:Double>
                        </Binding.ConverterParameter>
                    </Binding>
                </Label.Text>
            </Label>
        </StackLayout>
    </StackLayout>
</ContentPage>    
```

The values of the `Red` and `Green` properties are displayed with a `Binding` markup extension. The `Blue` property, however, instantiates the `Binding` class to demonstrate how an explicit `double` value can be set to `ConverterParameter` property.

Here's the result:

:::image type="content" source="media/converters/rgbcolorselector.png" alt-text="RGB color selector.":::
