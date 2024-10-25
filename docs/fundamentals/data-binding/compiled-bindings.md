---
title: "Compiled bindings"
description: "Compiled bindings can be used to improve data binding performance in .NET MAUI applications."
ms.date: 10/08/2024
---

# Compiled bindings

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

.NET Multi-platform App UI (.NET MAUI) data bindings have two main issues:

1. There's no compile-time validation of binding expressions. Instead, bindings are resolved at runtime. Therefore, any invalid bindings aren't detected until runtime when the application doesn't behave as expected or error messages appear.
1. They aren't cost efficient. Bindings are resolved at runtime using general-purpose object inspection (reflection), and the overhead of doing this varies from platform to platform.

Compiled bindings improve data binding performance in .NET MAUI applications by resolving binding expressions at compile-time rather than runtime. In addition, this compile-time validation of binding expressions enables a better developer troubleshooting experience because invalid bindings are reported as build errors.

::: moniker range=">=net-maui-9.0"

> [!IMPORTANT]
> Compiled bindings are required instead of string-based bindings in NativeAOT apps, and in apps with full trimming enabled. For more information, see [Trim a .NET MAUI app](~/deployment/trimming.md) and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

## Compiled bindings in XAML

To use compiled bindings in XAML, set an `x:DataType` attribute on a <xref:Microsoft.Maui.Controls.VisualElement> to the type of the object that the <xref:Microsoft.Maui.Controls.VisualElement> and its children will bind to. It's recommended to set the `x:DataType` attribute at the same level in the view hierarchy as the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> is set. However, this attribute can be re-defined at any location in a view hierarchy.

> [!IMPORTANT]
> Compiled bindings require the use of XAML compilation, which is enabled by default in .NET MAUI. If you've disabled XAML compilation, you'll need to enable it. For more information, see [XAML Compilation](~/xaml/xamlc.md).

To use compiled bindings in XAML, the `x:DataType` attribute must be set to a string literal, or a type using the `x:Type` markup extension. At XAML compile time, any invalid binding expressions will be reported as build errors. However, the XAML compiler will only report a build error for the first invalid binding expression that it encounters. Any valid binding expressions that are defined on the <xref:Microsoft.Maui.Controls.VisualElement> or its children will be compiled, regardless of whether the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> is set in XAML or code. Compiling a binding expression generates compiled code that will get a value from a property on the *source*, and set it on the property on the *target* that's specified in the markup. In addition, depending on the binding expression, the generated code may observe changes in the value of the *source* property and refresh the *target* property, and may push changes from the *target* back to the *source*.

::: moniker range="=net-maui-8.0"

> [!IMPORTANT]
> Compiled bindings are disabled for any XAML binding expressions that define the `Source` property. This is because the `Source` property is always set using the `x:Reference` markup extension, which can't be resolved at compile time.
>
> In addition, compiled bindings in XAML are currently unsupported on multi-bindings.

By default, .NET MAUI doesn't produce build warnings for XAML bindings that don't use compiled bindings. However, you can opt into compiled bindings warnings being produced by setting the `$(MauiStrictXamlCompilation)` build property to `true` in your app's project file (*.csproj):

```xml
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
```

::: moniker-end

::: moniker range=">=net-maui-9.0"

By default, .NET MAUI produces build warnings for XAML bindings that don't use compiled bindings.

::: moniker-end

For more information about XAML compiled bindings warnings, see [XAML compiled bindings warnings](#xaml-compiled-bindings-warnings).

### Use compiled bindings in XAML

The following example demonstrates using compiled bindings between .NET MAUI views and viewmodel properties:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.CompiledColorSelectorPage"
             x:DataType="local:HslColorViewModel"
             Title="Compiled Color Selector">
    <ContentPage.BindingContext>
        <local:HslColorViewModel Color="Sienna" />
    </ContentPage.BindingContext>
    ...
    <StackLayout>
        <BoxView Color="{Binding Color}"
                 ... />
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

The <xref:Microsoft.Maui.Controls.ContentPage> instantiates the `HslColorViewModel` and initializes the `Color` property within property element tags for the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> property. The <xref:Microsoft.Maui.Controls.ContentPage> also defines the `x:DataType` attribute as the viewmodel type, indicating that any binding expressions in the <xref:Microsoft.Maui.Controls.ContentPage> view hierarchy will be compiled. This can be verified by changing any of the binding expressions to bind to a non-existent viewmodel property, which will result in a build error. While this example sets the `x:DataType` attribute to a string literal, it can also be set to a type with the `x:Type` markup extension. For more information about the `x:Type` markup extension, see [x:Type Markup Extension](~/xaml/markup-extensions/consume.md#xtype-markup-extension).

> [!IMPORTANT]
> The `x:DataType` attribute can be re-defined at any point in a view hierarchy.

The <xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Label> elements, and <xref:Microsoft.Maui.Controls.Slider> views inherit the binding context from the <xref:Microsoft.Maui.Controls.ContentPage>. These views are all binding targets that reference source properties in the viewmodel. For the `BoxView.Color` property, and the `Label.Text` property, the data bindings are `OneWay` – the properties in the view are set from the properties in the viewmodel. However, the `Slider.Value` property uses a `TwoWay` binding. This allows each <xref:Microsoft.Maui.Controls.Slider> to be set from the viewmodel, and also for the viewmodel to be set from each <xref:Microsoft.Maui.Controls.Slider>.

When the example is first run, the <xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Label> elements, and <xref:Microsoft.Maui.Controls.Slider> elements are all set from the viewmodel based on the initial `Color` property set when the viewmodel was instantiated. As the sliders are manipulated, the <xref:Microsoft.Maui.Controls.BoxView> and <xref:Microsoft.Maui.Controls.Label> elements are updated accordingly:

:::image type="content" source="media/compiled-bindings/compiledcolorselector.png" alt-text="Compiled color selector.":::

For more information about this color selector, see [ViewModels and property-change notifications](binding-mode.md#viewmodels-and-property-change-notifications).

### Use compiled bindings in XAML in a DataTemplate

Bindings in a <xref:Microsoft.Maui.Controls.DataTemplate> are interpreted in the context of the object being templated. Therefore, when using compiled bindings in a <xref:Microsoft.Maui.Controls.DataTemplate>, the <xref:Microsoft.Maui.Controls.DataTemplate> needs to declare the type of its data object using the `x:DataType` attribute. Failure to do this could result in the <xref:Microsoft.Maui.Controls.DataTemplate> inheriting an incorrect `x:DataType` from its parent scope:

```xaml
<ContentPage ...
             x:DataType="local:AnimalsPageViewModel">
    <!-- Binding to AnimalsPageViewModel.Animals -->
    <CollectionView ItemsSource="{Binding Animals}">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <!-- incorrect: compiler thinks you want to bind to AnimalsPageViewModel.Name -->  
                <Label Text="{Binding Name}" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage>
```

The following example demonstrates correctly setting the `x:DataType` on a <xref:Microsoft.Maui.Controls.DataTemplate>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.CompiledColorListPage"
             Title="Compiled Color List">
    <Grid>
        ...
        <ListView x:Name="colorListView"
                  ItemsSource="{x:Static local:NamedColor.All}"
                  ... >
            <ListView.ItemTemplate>
                <DataTemplate x:DataType="local:NamedColor">
                    <ViewCell>
                        <StackLayout Orientation="Horizontal">
                            <BoxView Color="{Binding Color}"
                                     ... />
                            <Label Text="{Binding FriendlyName}"
                                   ... />
                        </StackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
        <!-- The BoxView doesn't use compiled bindings -->
        <BoxView Color="{Binding Source={x:Reference colorListView}, Path=SelectedItem.Color}"
                 ... />
    </Grid>
</ContentPage>
```

The `ListView.ItemsSource` property is set to the static `NamedColor.All` property. The `NamedColor` class uses .NET reflection to enumerate all the static public fields in the <xref:Microsoft.Maui.Graphics.Colors> class, and to store them with their names in a collection that is accessible from the static `All` property. Therefore, the <xref:Microsoft.Maui.Controls.ListView> is filled with all of the `NamedColor` instances. For each item in the <xref:Microsoft.Maui.Controls.ListView>, the binding context for the item is set to a `NamedColor` object. The <xref:Microsoft.Maui.Controls.BoxView> and <xref:Microsoft.Maui.Controls.Label> elements in the <xref:Microsoft.Maui.Controls.ViewCell> are bound to `NamedColor` properties.

The <xref:Microsoft.Maui.Controls.DataTemplate> defines the `x:DataType` attribute to be the `NamedColor` type, indicating that any binding expressions in the <xref:Microsoft.Maui.Controls.DataTemplate> view hierarchy will be compiled. This can be verified by changing any of the binding expressions to bind to a non-existent `NamedColor` property, which will result in a build error.  While this example sets the `x:DataType` attribute to a string literal, it can also be set to a type with the `x:Type` markup extension. For more information about the `x:Type` markup extension, see [x:Type Markup Extension](~/xaml/markup-extensions/consume.md#xtype-markup-extension).

When the example is first run, the <xref:Microsoft.Maui.Controls.ListView> is populated with `NamedColor` instances. When an item in the <xref:Microsoft.Maui.Controls.ListView> is selected, the `BoxView.Color` property is set to the color of the selected item in the <xref:Microsoft.Maui.Controls.ListView>:

:::image type="content" source="media/compiled-bindings/compiledcolorlist.png" alt-text="Compiled color list.":::

Selecting other items in the <xref:Microsoft.Maui.Controls.ListView> updates the color of the <xref:Microsoft.Maui.Controls.BoxView>.

::: moniker range=">=net-maui-9.0"

### Compile bindings that define the `Source` property

Prior to .NET MAUI 9, the XAML compiler would skip compilation of bindings that define the `Source` property instead of the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext>. From .NET MAUI 9, these bindings can be compiled to take advantage of better runtime performance. However, this optimization isn't enabled by default to avoid breaking existing app code. To enable this optimization, set the `$(MauiEnableXamlCBindingWithSourceCompilation)` build property to `true` in your app's project file:

```xml
<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>
```

Then, ensure that all your bindings are annotated with the correct `x:DataType` and that they don't inherit incorrect data types from their parent scope:

```xaml
<HorizontalStackLayout BindingContext="{x:Reference slider}" x:DataType="Slider">
  <Label Text="{Binding Value}" />
  <Label Text="{Binding Text, Source={x:Reference entry}, x:DataType=Entry}" />
</HorizontalStackLayout>
```

::: moniker-end

### Combine compiled bindings with classic bindings in XAML

Binding expressions are only compiled for the view hierarchy that the `x:DataType` attribute is defined on. Conversely, any views in a hierarchy on which the `x:DataType` attribute is not defined will use classic bindings. It's therefore possible to combine compiled bindings and classic bindings on a page. For example, in the previous section the views within the <xref:Microsoft.Maui.Controls.DataTemplate> use compiled bindings, while the <xref:Microsoft.Maui.Controls.BoxView> that's set to the color selected in the <xref:Microsoft.Maui.Controls.ListView> does not.

Careful structuring of `x:DataType` attributes can therefore lead to a page using compiled and classic bindings. Alternatively, the `x:DataType` attribute can be re-defined at any point in a view hierarchy to `null` using the `x:Null` markup extension. Doing this indicates that any binding expressions within the view hierarchy will use classic bindings. The following example demonstrates this approach:

```xaml
<StackLayout x:DataType="local:HslColorViewModel">
    <StackLayout.BindingContext>
        <local:HslColorViewModel Color="Sienna" />
    </StackLayout.BindingContext>
    <BoxView Color="{Binding Color}"
             VerticalOptions="FillAndExpand" />
    <StackLayout x:DataType="{x:Null}"
                 Margin="10, 0">
        <Label Text="{Binding Name}" />
        <Slider Value="{Binding Hue}" />
        <Label Text="{Binding Hue, StringFormat='Hue = {0:F2}'}" />
        <Slider Value="{Binding Saturation}" />
        <Label Text="{Binding Saturation, StringFormat='Saturation = {0:F2}'}" />
        <Slider Value="{Binding Luminosity}" />
        <Label Text="{Binding Luminosity, StringFormat='Luminosity = {0:F2}'}" />
    </StackLayout>
</StackLayout>   
```

The root <xref:Microsoft.Maui.Controls.StackLayout> sets the `x:DataType` attribute to be the `HslColorViewModel` type, indicating that any binding expression in the root <xref:Microsoft.Maui.Controls.StackLayout> view hierarchy will be compiled. However, the inner <xref:Microsoft.Maui.Controls.StackLayout> redefines the `x:DataType` attribute to `null` with the `x:Null` markup expression. Therefore, the binding expressions within the inner <xref:Microsoft.Maui.Controls.StackLayout> use classic bindings. Only the <xref:Microsoft.Maui.Controls.BoxView>, within the root <xref:Microsoft.Maui.Controls.StackLayout> view hierarchy, uses compiled bindings.

For more information about the `x:Null` markup expression, see [x:Null Markup Extension](~/xaml/markup-extensions/consume.md#xnull-markup-extension).

### XAML compiled bindings warnings

The following table lists the compiler warnings for compiled bindings, and how to resolve them:

::: moniker range="=net-maui-8.0"

| Code | Message | Fix |
| ---- | ------- | --- |
| `XC0022` | Binding could be compiled to improve runtime performance if `x:DataType` is specified. | Add `x:DataType` to your XAML to specify the type of the current <xref:Microsoft.Maui.Controls.BindableObject.BindingContext>. It's best practice to add `x:DataType` to all elements where the binding context changes. |
| `XC0023` | Binding could be compiled to improve runtime performance if `x:DataType` is not explicitly `null`. | Replace `x:DataType="{x:Null}"` with the right type.  |

::: moniker-end

::: moniker range=">=net-maui-9.0"

| Code | Message |
| ---- | ------- |
| `XC0022` | Binding could be compiled to improve runtime performance if `x:DataType` is specified. <br><br>To fix this warning, add `x:DataType` to your XAML to specify the type of the current <xref:Microsoft.Maui.Controls.BindableObject.BindingContext>. It's best practice to add `x:DataType` to all elements where the binding context changes. |
| `XC0023` | Binding could be compiled to improve runtime performance if `x:DataType` is not explicitly `null`. <br><br>To fix this warning, replace `x:DataType="{x:Null}"` with the right type. |
| `XC0024` | Binding might be compiled incorrectly since the `x:DataType` annotation comes from an outer scope. Make sure you annotate all `DataTemplate` XAML elements with the correct `x:DataType`. <br><br>To fix this warning, ensure that all `DataTemplate` elements are annotated with the correct `x:DataType`. |
| `XC0025` | Binding was not compiled because it has an explicitly set `Source` property and compilation of bindings with `Source` is not enabled. Consider enabling this optimization by setting the `<MauiEnableXamlCBindingWithSourceCompilation>true</MauiEnableXamlCBindingWithSourceCompilation>` in your project file and make sure the correct `x:DataType` is specified for this binding. <br><br>To fix this warning, enable the `$(MauiEnableXamlCBindingWithSourceCompilation)` build property in your project file, and annotate all your bindings with the appropriate `x:DataType`. |

::: moniker-end

To ensure these warnings aren't ignored, consider changing specific warnings to build errors with the `$(WarningsAsErrors)` build property:

```xml
<WarningsAsErrors>$(WarningsAsErrors);XC0022;XC0023</WarningsAsErrors>
```

To ignore these warnings, use the `$(NoWarn)` build property with specific warning codes:

```xml
<NoWarn>$(NoWarn);XC0022;XC0023</NoWarn>
```

::: moniker range="=net-maui-8.0"

> [!IMPORTANT]
> `XC0022` and `XC0023` warnings will always be suppressed unless the `$(MauiStrictXamlCompilation)` build property is set to `true`.

::: moniker-end

If you set the `$(TreatWarningsAsErrors)` build property to `true` in your app's project file, but you want to ignore certain XAML compiler warnings, use either the `$(NoWarn)` build property to silence these warnings or the `$(WarningsNotAsErrors)` build property to reduce the severity of some specific codes.

::: moniker range=">=net-maui-9.0"

By default, .NET MAUI produces build warnings for XAML bindings that don't use compiled bindings. You can opt into compiled bindings warnings being treated as errors by setting the `$(MauiStrictXamlCompilation)` and `$(TreatWarningsAsErrors)` build properties to `true` in your app's project file (*.csproj):

```xml
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
<MauiStrictXamlCompilation>true</MauiStrictXamlCompilation>
```

> [!NOTE]
> By default, the `$(MauiStrictXamlCompilation)` build property is `false` unless you are publishing your app using full trimming or NativeAOT.

::: moniker-end

::: moniker range=">=net-maui-9.0"

## Compiled bindings in code

Bindings written in code typically use string paths that are resolved at runtime with reflection. However, the <xref:Microsoft.Maui.Controls.BindableObjectExtensions.SetBinding%2A> extension method also has an overload that defines bindings using a `Func` argument instead of a string path:

```csharp
MyLabel.SetBinding(Label.TextProperty, static (Entry entry) => entry.Text);
```

Not all methods can be used to define a compiled binding. The expression must be a simple property access expression. The following examples show valid and invalid binding expressions:

```csharp
// Valid: Property access
static (PersonViewModel vm) => vm.Name;
static (PersonViewModel vm) => vm.Address?.Street;

// Valid: Array and indexer access
static (PersonViewModel vm) => vm.PhoneNumbers[0];
static (PersonViewModel vm) => vm.Config["Font"];

// Valid: Casts
static (Label label) => (label.BindingContext as PersonViewModel).Name;
static (Label label) => ((PersonViewModel)label.BindingContext).Name;

// Invalid: Method calls
static (PersonViewModel vm) => vm.GetAddress();
static (PersonViewModel vm) => vm.Address?.ToString();

// Invalid: Complex expressions
static (PersonViewModel vm) => vm.Address?.Street + " " + vm.Address?.City;
static (PersonViewModel vm) => $"Name: {vm.Name}";
```

In addition, the <xref:Microsoft.Maui.Controls.Binding.Create%2A?displayProperty=nameWithType> method sets the binding directly on the object with a `Func`, and returns the binding object instance:

```csharp
myEntry.SetBinding(Entry.TextProperty, new MultiBinding
{
    Bindings = new Collection<BindingBase>
    {
        Binding.Create(static (Entry entry) => entry.FontFamily, source: RelativeBindingSource.Self),
        Binding.Create(static (Entry entry) => entry.FontSize, source: RelativeBindingSource.Self),
        Binding.Create(static (Entry entry) => entry.FontAttributes, source: RelativeBindingSource.Self),
    },
    Converter = new StringConcatenationConverter()
});
```

These compiled binding approaches provide the following benefits:

- Improved data binding performance by resolving binding expressions at compile-time rather than runtime.
- A better developer troubleshooting experience because invalid bindings are reported as build errors.
- Intellisense while editing.

::: moniker-end

## Performance

Compiled bindings improve data binding performance, with the performance benefit varying:

- A compiled binding that uses property-change notification (i.e. a `OneWay`, `OneWayToSource`, or `TwoWay` binding) is resolved approximately 8 times quicker than a classic binding.
- A compiled binding that doesn't use property-change notification (i.e. a `OneTime` binding) is resolved approximately 20 times quicker than a classic binding.
- Setting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> on a compiled binding that uses property change notification (i.e. a `OneWay`, `OneWayToSource`, or `TwoWay` binding) is approximately 5 times quicker than setting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> on a classic binding.
- Setting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> on a compiled binding that doesn't use property change notification (i.e. a `OneTime` binding) is approximately 7 times quicker than setting the <xref:Microsoft.Maui.Controls.BindableObject.BindingContext> on a classic binding.

These performance differences can be magnified on mobile devices, dependent upon the platform being used, the version of the operating system being used, and the device on which the application is running.
