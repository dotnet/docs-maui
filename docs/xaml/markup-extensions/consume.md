---
title: "Consume XAML markup extensions"
description: ".NET MAUI XAML markup extensions enhance the power and flexibility of XAML by allowing element attributes to be set from a variety of sources."
ms.date: 01/27/2022
---

# Consume XAML markup extensions

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/xaml-markupextensions)

.NET Multi-platform App UI (.NET MAUI) XAML markup extensions help enhance the power and flexibility of XAML by allowing element attributes to be set from a variety of sources.

For example, you typically set the `Color` property of <xref:Microsoft.Maui.Controls.BoxView> like this:

```xaml
<BoxView Color="Blue" />
```

However, you might prefer instead to set the `Color` attribute from a value stored in a resource dictionary, or from the value of a static property of a class that you've created, or from a property of type `Color` of another element on the page, or constructed from separate hue, saturation, and luminosity values. All these options are possible using XAML markup extensions.

A markup extension is a different way to express an attribute of an element. .NET MAUI XAML markup extensions are usually identifiable by an attribute value that is enclosed in curly braces:

```xaml
<BoxView Color="{StaticResource themeColor}" />
```

Any attribute value in curly braces is *always* a XAML markup extension. However, XAML markup extensions can also be referenced without the use of curly braces.

> [!NOTE]
> Several XAML markup extensions are part of the XAML 2009 specification. These appear in XAML files with the customary `x` namespace prefix, and are commonly referred to with this prefix.

In addition to the markup extensions discussed in this article, the following markup extensions are included in .NET MAUI and discussed in other articles:

- `StaticResource` - reference objects from a resource dictionary. For more information, see [Resource dictionaries**](~/fundamentals/resource-dictionaries.md).
- `DynamicResource` - respond to changes in objects in a resource dictionary. For more information, see [Dynamic styles**](~/user-interface/styles/xaml.md#dynamic-styles).
- `Binding` - establish a link between properties of two objects. For more information, see [Data binding**](~/fundamentals/data-binding/index.md).
- `TemplateBinding` - performs data binding from a control template. For more information, see [Control templates](~/fundamentals/controltemplate.md).
- `RelativeSource` - sets the binding source relative to the position of the binding target. For more information, see [Relative bindings](~/fundamentals/data-binding/relative-bindings.md).
<!-- - `ConstraintExpression` - relates the position and size of a child in a `RelativeLayout` to its parent, or a sibling. For more information, see [RelativeLayout](~/user-interface/layouts/relativelayout.md).-->

## x:Static markup extension

The `x:Static` markup extension is supported by the `StaticExtension` class. The class has a single property named `Member` of type `string` that you set to the name of a public constant, static property, static field, or enumeration member.

One way to use `x:Static` is to first define a class with some constants or static variables, such as this `AppConstants` class:

```csharp
static class AppConstants
{
    public static double NormalFontSize = 18;
}
```

The following XAML demonstrates the most verbose approach to instantiating the `StaticExtension` class between `Label.FontSize` property-element tags:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:sys="clr-namespace:System;assembly=netstandard"
             xmlns:local="clr-namespace:MarkupExtensions"
             x:Class="MarkupExtensions.StaticDemoPage"
             Title="x:Static Demo">
    <StackLayout Margin="10, 0">
        <Label Text="Label No. 1">
            <Label.FontSize>
                <x:StaticExtension Member="local:AppConstants.NormalFontSize" />
            </Label.FontSize>
        </Label>
        ···
    </StackLayout>
</ContentPage>
```

The XAML parser also allows the `StaticExtension` class to be abbreviated as `x:Static`:

```xaml
<Label Text="Label No. 2">
    <Label.FontSize>
        <x:Static Member="local:AppConstants.NormalFontSize" />
    </Label.FontSize>
</Label>
```

This syntax can be simplified even further by putting the `StaticExtension` class and the member setting in curly braces. The resulting expression is set directly to the `FontSize` attribute:

```xaml
<Label Text="Label No. 3"
       FontSize="{x:StaticExtension Member=local:AppConstants.NormalFontSize}" />
```

In this example, there are *no* quotation marks within the curly braces. The `Member` property of `StaticExtension` is no longer an XML attribute. It is instead part of the expression for the markup extension.

Just as you can abbreviate `x:StaticExtension` to `x:Static` when you use it as an object element, you can also abbreviate it in the expression within curly braces:

```xaml
<Label Text="Label No. 4"
       FontSize="{x:Static Member=local:AppConstants.NormalFontSize}" />
```

The `StaticExtension` class has a `ContentProperty` attribute referencing the property `Member`, which marks this property as the class's default content property. For XAML markup extensions expressed with curly braces, you can eliminate the `Member=` part of the expression:

```xaml
<Label Text="Label No. 5"
       FontSize="{x:Static local:AppConstants.NormalFontSize}" />
```

This is the most common form of the `x:Static` markup extension.

The root tag of the XAML example also contains an XML namespace declaration for the .NET `System` namespace. This allows the <xref:Microsoft.Maui.Controls.Label> font size to be set to the static field `Math.PI`. That results in rather small text, so the `Scale` property is set to `Math.E`:

```xaml
<Label Text="&#x03C0; &#x00D7; E sized text"
       FontSize="{x:Static sys:Math.PI}"
       Scale="{x:Static sys:Math.E}"
       HorizontalOptions="Center" />
```

The following screenshot shows the XAML output:

:::image type="content" source="media/consume/staticdemo.png" alt-text="x:Static demo.":::

## x:Reference markup extension

The `x:Reference` markup extension is supported by the `ReferenceExtension` class. The class has a single property named `Name` of type `string` that you set to the name of an element on the page that has been given a name with `x:Name`. This `Name` property is the content property of `ReferenceExtension`, so `Name=` is not required when `x:Reference` appears in curly braces. The `x:Reference` markup extension is used exclusively with data bindings. For more information about data bindings, see [Data binding](~/fundamentals/data-binding/index.md).

The following XAML example shows two uses of `x:Reference` with data bindings, the first where it's used to set the `Source` property of the `Binding` object, and the second where it's used to set the `BindingContext` property for two data bindings:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MarkupExtensions.ReferenceDemoPage"
             x:Name="page"
             Title="x:Reference Demo">    
    <StackLayout Margin="10, 0">        
        <Label Text="{Binding Source={x:Reference page},
                              StringFormat='The type of this page is {0}'}"
               FontSize="18"
               VerticalOptions="Center"
               HorizontalTextAlignment="Center" />
        <Slider x:Name="slider"
                Maximum="360"
                VerticalOptions="Center" />
        <Label BindingContext="{x:Reference slider}"
               Text="{Binding Value, StringFormat='{0:F0}&#x00B0; rotation'}"
               Rotation="{Binding Value}"
               FontSize="24"
               HorizontalOptions="Center"
               VerticalOptions="Center" />        
    </StackLayout>
</ContentPage>
```

In this example, both `x:Reference` expressions use the abbreviated version of the `ReferenceExtension` class name and eliminate the `Name=` part of the expression. In the first example, the `x:Reference` markup extension is embedded in the `Binding` markup extension and the `Source` and `StringFormat` properties are separated by commas.

The following screenshot shows the XAML output:

:::image type="content" source="media/consume/referencedemo.png" alt-text="x:Reference demo.":::

## x:Type markup extension

The `x:Type` markup extension is the XAML equivalent of the C# [`typeof`](/dotnet/csharp/language-reference/keywords/typeof/) keyword. It's supported by the `TypeExtension` class, which defines a property named `TypeName` of type `string` that should be set to a class or structure name. The `x:Type` markup extension returns the [`Type`](xref:System.Type) object of that class or structure. `TypeName` is the content property of `TypeExtension`, so `TypeName=` is not required when `x:Type` appears with curly braces.

The `x:Type` markup extension is commonly used with the `x:Array` markup extension. For more information, see [x:Array markup extension](#xarray-markup-extension).

The following XAML example demonstrates using the `x:Type` markup extension to instantiate .NET MAUI objects and add them to a <xref:Microsoft.Maui.Controls.StackLayout>. The XAML consists of three <xref:Microsoft.Maui.Controls.Button> elements with their `Command` properties set to a `Binding` and the `CommandParameter` properties set to types of three .NET MAUI views:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MarkupExtensions.TypeDemoPage"
             Title="x:Type Demo">    
    <StackLayout x:Name="stackLayout"
                 Padding="10, 0">        
        <Button Text="Create a Slider"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                Command="{Binding CreateCommand}"
                CommandParameter="{x:Type Slider}" />
        <Button Text="Create a Stepper"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                Command="{Binding CreateCommand}"
                CommandParameter="{x:Type Stepper}" />
        <Button Text="Create a Switch"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                Command="{Binding CreateCommand}"
                CommandParameter="{x:Type Switch}" />
    </StackLayout>
</ContentPage>
```

The code-behind file defines and initializes the `CreateCommand` property:

```csharp
public partial class TypeDemoPage : ContentPage
{
    public ICommand CreateCommand { get; private set; }

    public TypeDemoPage()
    {
        InitializeComponent();

        CreateCommand = new Command<Type>((Type viewType) =>
        {
            View view = (View)Activator.CreateInstance(viewType);
            view.VerticalOptions = LayoutOptions.Center;
            stackLayout.Add(view);
        });

        BindingContext = this;
    }
}
```

When a <xref:Microsoft.Maui.Controls.Button> is pressed a new instance of the `CommandParameter` argument is created and added to the <xref:Microsoft.Maui.Controls.StackLayout>. The three <xref:Microsoft.Maui.Controls.Button> objects then share the page with dynamically created views:

:::image type="content" source="media/consume/typedemo.png" alt-text="x:Type demo.":::

## x:Array markup extension

The `x:Array` markup extension enables you to define an array in markup. It is supported by the `ArrayExtension` class, which defines two properties:

- `Type` of type `Type`, which indicates the type of the elements in the array. This property should be set to an `x:Type` markup extension.
- `Items` of type `IList`, which is a collection of the items themselves. This is the content property of `ArrayExtension`.

The `x:Array` markup extension itself never appears in curly braces. Instead, `x:Array` start and end tags delimit the list of items.

The following XAML example shows how to use `x:Array` to add items to a <xref:Microsoft.Maui.Controls.ListView> by setting the `ItemsSource` property to an array:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MarkupExtensions.ArrayDemoPage"
             Title="x:Array Demo Page">
    <ListView Margin="10">
        <ListView.ItemsSource>
            <x:Array Type="{x:Type Color}">
                <Color>Aqua</Color>
                <Color>Black</Color>
                <Color>Blue</Color>
                <Color>Fuchsia</Color>
                <Color>Gray</Color>
                <Color>Green</Color>
                <Color>Lime</Color>
                <Color>Maroon</Color>
                <Color>Navy</Color>
                <Color>Olive</Color>
                <Color>Pink</Color>
                <Color>Purple</Color>
                <Color>Red</Color>
                <Color>Silver</Color>
                <Color>Teal</Color>
                <Color>White</Color>
                <Color>Yellow</Color>
            </x:Array>
        </ListView.ItemsSource>
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <BoxView Color="{Binding}"
                             Margin="3" />
                </ViewCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
</ContentPage>     
```

In this example, the <xref:Microsoft.Maui.Controls.ViewCell> creates a simple <xref:Microsoft.Maui.Controls.BoxView> for each color entry:

:::image type="content" source="media/consume/arraydemo.png" alt-text="x:Array demo.":::

> [!NOTE]
> When defining arrays of common types like strings or numbers, use the XAML language primitives tags listed in [Pass arguments](~/xaml/pass-arguments.md).

## x:Null markup extension

The `x:Null` markup extension is supported by the `NullExtension` class. It has no properties and is simply the XAML equivalent of the C# [`null`](/dotnet/csharp/language-reference/keywords/null/) keyword.

The following XAML example shows how to use the `x:Null` markup extension:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MarkupExtensions.NullDemoPage"
             Title="x:Null Demo">
    <ContentPage.Resources>
        <Style TargetType="Label">
            <Setter Property="FontSize" Value="48" />
            <Setter Property="FontFamily" Value="OpenSansRegular" />
        </Style>
    </ContentPage.Resources>

    <StackLayout Padding="10, 0">
        <Label Text="Text 1" />
        <Label Text="Text 2" />
        <Label Text="Text 3"
               FontFamily="{x:Null}" />
        <Label Text="Text 4" />
        <Label Text="Text 5" />
    </StackLayout>
</ContentPage>      
```

In this example, an implicit `Style` is defined for <xref:Microsoft.Maui.Controls.Label> that includes a `Setter` that sets the `FontFamily` property to a specific font. However, the third <xref:Microsoft.Maui.Controls.Label> avoids using the font defined in the implicit style by setting its `FontFamily` to `x:Null`:

:::image type="content" source="media/consume/nulldemo.png" alt-text="x:Null demo.":::

## OnPlatform markup extension

The `OnPlatform` markup extension enables you to customize UI appearance on a per-platform basis. It provides the same functionality as the `OnPlatform` and `On` classes, but with a more concise representation.

The `OnPlatform` markup extension is supported by the `OnPlatformExtension` class, which defines the following properties:

- `Default` of type `object`, that you set to a default value to be applied to the properties that represent platforms.
- `Android` of type `object`, that you set to a value to be applied on Android.
- `iOS` of type `object`, that you set to a value to be applied on iOS.
- `MacCatalyst` of type `object`, that you set to a value to be applied on Mac Catalyst.
- `Tizen` of type `object`, that you set to a value to be applied on the Tizen platform.
- `WinUI` of type `object`, that you set to a value to be applied on WinUI.
- `Converter` of type `IValueConverter`, that can be set to an `IValueConverter` implementation.
- `ConverterParameter` of type `object`, that can be set to a value to pass to the `IValueConverter` implementation.

> [!NOTE]
> The XAML parser allows the `OnPlatformExtension` class to be abbreviated as `OnPlatform`.

The `Default` property is the content property of `OnPlatformExtension`. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Default=` part of the expression provided that it's the first argument. If the `Default` property isn't set, it will default to the `BindableProperty.DefaultValue` property value, provided that the markup extension is targeting a `BindableProperty`.

> [!IMPORTANT]
> The XAML parser expects that values of the correct type will be provided to properties consuming the `OnPlatform` markup extension. If type conversion is necessary, the `OnPlatform` markup extension will attempt to perform it using the default converters provided by Xamarin.Forms. However, there are some type conversions that can't be performed by the default converters and in these cases the `Converter` property should be set to an `IValueConverter` implementation.

The **OnPlatform Demo** page shows how to use the `OnPlatform` markup extension:

```xaml
<BoxView Color="{OnPlatform Yellow, iOS=Red, Android=Green}"
         WidthRequest="{OnPlatform 250, iOS=200, Android=300}"  
         HeightRequest="{OnPlatform 250, iOS=200, Android=300}"
         HorizontalOptions="Center" />
```

In this example, all three `OnPlatform` expressions use the abbreviated version of the `OnPlatformExtension` class name. The three `OnPlatform` markup extensions set the `Color`, `WidthRequest`, and `HeightRequest` properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on iOS and Android. The markup extensions also provide default values for these properties on the platforms that aren't specified, while eliminating the `Default=` part of the expression.

## OnIdiom markup extension

The `OnIdiom` markup extension enables you to customize UI appearance based on the idiom of the device the application is running on. It's supported by the `OnIdiomExtension` class, which defines the following properties:

- `Default` of type `object`, that you set to a default value to be applied to the properties that represent device idioms.
- `Phone` of type `object`, that you set to a value to be applied on phones.
- `Tablet` of type `object`, that you set to a value to be applied on tablets.
- `Desktop` of type `object`, that you set to a value to be applied on desktop platforms.
- `TV` of type `object`, that you set to a value to be applied on TV platforms.
- `Watch` of type `object`, that you set to a value to be applied on Watch platforms.
- `Converter` of type `IValueConverter`, that can be set to an `IValueConverter` implementation.
- `ConverterParameter` of type `object`, that can be set to a value to pass to the `IValueConverter` implementation.

> [!NOTE]
> The XAML parser allows the `OnIdiomExtension` class to be abbreviated as `OnIdiom`.

The `Default` property is the content property of `OnIdiomExtension`. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Default=` part of the expression provided that it's the first argument.

> [!IMPORTANT]
> The XAML parser expects that values of the correct type will be provided to properties consuming the `OnIdiom` markup extension. If type conversion is necessary, the `OnIdiom` markup extension will attempt to perform it using the default converters provided by .NET MAUI. However, there are some type conversions that can't be performed by the default converters and in these cases the `Converter` property should be set to an `IValueConverter` implementation.

The following XAML example shows how to use the `OnIdiom` markup extension:

```xaml
<BoxView Color="{OnIdiom Yellow, Phone=Red, Tablet=Green, Desktop=Blue}"
         WidthRequest="{OnIdiom 100, Phone=200, Tablet=300, Desktop=400}"
         HeightRequest="{OnIdiom 100, Phone=200, Tablet=300, Desktop=400}"
         HorizontalOptions="Center" />
```

In this example, all three `OnIdiom` expressions use the abbreviated version of the `OnIdiomExtension` class name. The three `OnIdiom` markup extensions set the `Color`, `WidthRequest`, and `HeightRequest` properties of the <xref:Microsoft.Maui.Controls.BoxView> to different values on the phone, tablet, and desktop idioms. The markup extensions also provide default values for these properties on the idioms that aren't specified, while eliminating the `Default=` part of the expression.

## DataTemplate markup extension

The `DataTemplate` markup extension enables you to convert a type into a `DataTemplate`. It's supported by the `DataTemplateExtension` class, which defines a `TypeName` property, of type `string`, that is set to the name of the type to be converted into a `DataTemplate`. The `TypeName` property is the content property of `DataTemplateExtension`. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `TypeName=` part of the expression.

> [!NOTE]
> The XAML parser allows the `DataTemplateExtension` class to be abbreviated as `DataTemplate`.

A typical usage of this markup extension is in a Shell application, as shown in the following example:

```xaml
<ShellContent Title="Monkeys"
              Icon="monkey.png"
              ContentTemplate="{DataTemplate views:MonkeysPage}" />
```

In this example, `MonkeysPage` is converted from a <xref:Microsoft.Maui.Controls.ContentPage> to a `DataTemplate`, which is set as the value of the `ShellContent.ContentTemplate` property. This ensures that `MonkeysPage` is only created when navigation to the page occurs, rather than at application startup.

For more information about Shell apps, see [Shell](~/fundamentals/shell/index.md).

## FontImage markup extension

The `FontImage` markup extension enables you to display a font icon in any view that can display an `ImageSource`. It provides the same functionality as the `FontImageSource` class, but with a more concise representation.

The `FontImage` markup extension is supported by the `FontImageExtension` class, which defines the following properties:

- `FontFamily` of type `string`, the font family to which the font icon belongs.
- `Glyph` of type `string`, the unicode character value of the font icon.
- `Color` of type `Color`, the color to be used when displaying the font icon.
- `Size` of type `double`, the size, in device-independent units, of the rendered font icon. The default value is 30. In addition, this property can be set to a named font size.

> [!NOTE]
> The XAML parser allows the `FontImageExtension` class to be abbreviated as `FontImage`.

The `Glyph` property is the content property of `FontImageExtension`. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Glyph=` part of the expression provided that it's the first argument.

The following XAML example shows how to use the `FontImage` markup extension:

```xaml
<Image BackgroundColor="#D1D1D1"
       Source="{FontImage &#xf30c;, FontFamily=Ionicons, Size=44}" />
```

In this example, the abbreviated version of the `FontImageExtension` class name is used to display an XBox icon, from the Ionicons font family, in an <xref:Microsoft.Maui.Controls.Image>:

:::image type="content" source="media/consume/fontimagedemo.png" alt-text="Screenshot of the FontImage markup extension.":::

While the unicode character for the icon is `\uf30c`, it has to be escaped in XAML and so becomes `&#xf30c;`.

For information about displaying font icons by specifying the font icon data in a `FontImageSource` object, see [Display font icons](~/user-interface/fonts.md#display-font-icons).

## AppThemeBinding markup extension

The `AppThemeBinding` markup extension enables you to specify a resource to be consumed, such as an image or color, based on the current system theme.

<!-- > [!IMPORTANT]
> The `AppThemeBinding` markup extension has minimum operating system requirements. For more information, see [Respond to system theme changes in .NET MAUI applications](~/xamarin-forms/user-interface/theming/system-theme-changes.md). -->

The `AppThemeBinding` markup extension is supported by the `AppThemeBindingExtension` class, which defines the following properties:

- `Default`, of type `object`, that you set to the resource to be used by default.
- `Light`, of type `object`, that you set to the resource to be used when the device is using its light theme.
- `Dark`, of type `object`, that you set to the resource to be used when the device is using its dark theme.
- `Value`, of type `object`, that returns the resource that's currently being used by the markup extension.

> [!NOTE]
> The XAML parser allows the `AppThemeBindingExtension` class to be abbreviated as `AppBindingTheme`.

The `Default` property is the content property of `AppThemeBindingExtension`. Therefore, for XAML markup expressions expressed with curly braces, you can eliminate the `Default=` part of the expression provided that it's the first argument.

The following XAML example shows how to use the `AppThemeBinding` markup extension:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MarkupExtensions.AppThemeBindingDemoPage"
             Title="AppThemeBinding Demo">
    <ContentPage.Resources>
        <Style x:Key="labelStyle"
               TargetType="Label">
            <Setter Property="TextColor"
                    Value="{AppThemeBinding Black, Light=Blue, Dark=Teal}" />
        </Style>
    </ContentPage.Resources>

    <StackLayout Margin="20">
        <Label Text="This text is green in light mode, and red in dark mode."
               TextColor="{AppThemeBinding Light=Green, Dark=Red}" />
        <Label Text="This text is black by default, blue in light mode, and teal in dark mode."
               Style="{StaticResource labelStyle}" />
    </StackLayout>
</ContentPage>
```

In this example, the text color of the first <xref:Microsoft.Maui.Controls.Label> is set to green when the device is using its light theme, and is set to red when the device is using its dark theme. The second <xref:Microsoft.Maui.Controls.Label> has its `TextColor` property set through a `Style`. This `Style` sets the text color of the <xref:Microsoft.Maui.Controls.Label> to black by default, to blue when the device is using its light theme, and to teal when the device is using its dark theme:

:::image type="content" source="media/consume/appthemebindingdemo.png" alt-text="AppThemeBinding demo.":::
