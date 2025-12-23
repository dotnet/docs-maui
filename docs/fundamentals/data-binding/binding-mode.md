---
title: "Binding mode"
description: "In .NET MAUI, every bindable property has a default binding mode, which indicates the binding mode in effect when that property is a data-binding target. The binding mode controls the flow of information between source and target, and is specified with a member of the BindingMode enumeration. "
ms.date: 02/27/2025
---

# Binding mode

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-databinding)

Every .NET Multi-platform App UI (.NET MAUI) bindable property has a default binding mode that is set when the bindable property is created, and which is available from the `DefaultBindingMode` property of the <xref:Microsoft.Maui.Controls.BindableProperty> object. This default binding mode indicates the mode in effect when that property is a data-binding target. The default binding mode for most properties such as `Rotation`, `Scale`, and `Opacity` is `OneWay`. When these properties are data-binding targets, then the target property is set from the source.

The following example shows a data binding defined on a <xref:Microsoft.Maui.Controls.Slider>:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DataBindingDemos.ReverseBindingPage"
             Title="Reverse Binding">
    <StackLayout Padding="10, 0">
        <Label x:Name="label"
               Text="TEXT"
               FontSize="80"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Slider x:Name="slider"
                x:DataType="Label"
                VerticalOptions="Center"
                Value="{Binding Source={x:Reference label},
                                Path=Opacity}" />
    </StackLayout>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Label> is the data-binding source, and the <xref:Microsoft.Maui.Controls.Slider> is the target. The binding references the `Opacity` property of the <xref:Microsoft.Maui.Controls.Label>, which has a default value of 1. Therefore, the <xref:Microsoft.Maui.Controls.Slider> is initialized to the value 1 from the initial `Opacity` value of <xref:Microsoft.Maui.Controls.Label>. This is shown in the following screenshot:

:::image type="content" source="media/binding-mode/reversebinding.png" alt-text="Reverse binding.":::

In addition, the <xref:Microsoft.Maui.Controls.Slider> continues to work. This is because the default binding mode for the `Value` property of <xref:Microsoft.Maui.Controls.Slider> is `TwoWay`. This means that when the `Value` property is a data-binding target, then the target is set from the source but the source is also set from the target. When a `TwoWay` binding is initialized, the target is set from the source first, which allows the <xref:Microsoft.Maui.Controls.Slider> to be set from the initial `Opacity` value.

> [!NOTE]
> Bindable properties don't signal a property change unless the property actually changes. This prevents an infinite loop.

If the default binding mode on the target property is not suitable for a particular data binding, it's possible to override it by setting the `Mode` property of `Binding` (or the `Mode` property of the `Binding` markup extension) to one of the members of the `BindingMode` enumeration:

- `Default`
- `TwoWay` — data goes both ways between source and target. When initialized, the target is set from the source first.
- `OneWay` — data goes from source to target
- `OneWayToSource` — data goes from target to source
- `OneTime` — data goes from source to target, but only when the `BindingContext` changes

## Two-way bindings

Most bindable properties have a default binding mode of `OneWay` but some properties have a default binding mode of `TwoWay`, including the following:

- `Date` property of <xref:Microsoft.Maui.Controls.DatePicker>
- `Text` property of <xref:Microsoft.Maui.Controls.Editor>, <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.SearchBar>, and <xref:Microsoft.Maui.Controls.EntryCell>
- `IsRefreshing` property of <xref:Microsoft.Maui.Controls.ListView>
- `SelectedItem` property of `MultiPage`
- `SelectedIndex` and `SelectedItem` properties of <xref:Microsoft.Maui.Controls.Picker>
- `Value` property of <xref:Microsoft.Maui.Controls.Slider> and <xref:Microsoft.Maui.Controls.Stepper>
- `IsToggled` property of <xref:Microsoft.Maui.Controls.Switch>
- `On` property of <xref:Microsoft.Maui.Controls.SwitchCell>
- `Time` property of <xref:Microsoft.Maui.Controls.TimePicker>

These properties are defined as `TwoWay` because when data bindings are used with the Model-View-ViewModel (MVVM) pattern, the viewmodel class is the data-binding source, and the view, which consists of views such as <xref:Microsoft.Maui.Controls.Slider>, are data-binding targets. MVVM bindings resemble the example above, because it's likely that you want each view on the page to be initialized with the value of the corresponding property in the viewmodel, but changes in the view should also affect the viewmodel property. When initialized, a `TwoWay` binding sets the target from the source first, and then subsequent changes flow in both directions.

## One-way-to-source bindings

Read-only bindable properties have a default binding mode of `OneWayToSource`. For example, the `SelectedItem` property of <xref:Microsoft.Maui.Controls.ListView> has a default binding mode of `OneWayToSource`. This is because a binding on the `SelectedItem` property should result in setting the binding source.

## One-time bindings

Target properties with a binding mode of `OneTime` are updated only when the binding context changes. For bindings on these target properties, this simplifies the binding infrastructure because it is not necessary to monitor changes in the source properties.

Several properties have a default binding mode of `OneTime`, including the `IsTextPredictionEnabled` property of <xref:Microsoft.Maui.Controls.Entry>.

## Viewmodels and property-change notifications

When using a viewmodel in a data-binding, the viewmodel is the data-binding source. The viewmodel doesn't define bindable properties, but it does implement a notification mechanism that allows the binding infrastructure to be notified when the value of a property changes. This notification mechanism is the [`INotifyPropertyChanged`](xref:System.ComponentModel.INotifyPropertyChanged) interface, which defines a single event named [`PropertyChanged`](xref:System.ComponentModel.INotifyPropertyChanged.PropertyChanged). A class that implements this interface typically fires the event when one of its public properties changes value. The event does not need to be raised if the property never changes. The `INotifyPropertyChanged` interface is also implemented by <xref:Microsoft.Maui.Controls.BindableObject> and a `PropertyChanged` event is raised whenever a bindable property changes value.

In the following example, data bindings allow you to select a color using three <xref:Microsoft.Maui.Controls.Slider> elements for the hue, saturation, and luminosity:

```csharp
public class HslColorViewModel : INotifyPropertyChanged
{
    Color color;
    string name;
    float hue;
    float saturation;
    float luminosity;

    public event PropertyChangedEventHandler PropertyChanged;

    public float Hue
    {
        get
        {
            return hue;
        }
        set
        {
            if (hue != value)
            {
                Color = Color.FromHsla(value, saturation, luminosity);
            }
        }
    }

    public float Saturation
    {
        get
        {
            return saturation;
        }
        set
        {
            if (saturation != value)
            {
                Color = Color.FromHsla(hue, value, luminosity);
            }
        }        
    }

    public float Luminosity
    {
        get
        {
            return luminosity;
        }
        set
        {
            if (luminosity != value)
            {
                Color = Color.FromHsla(hue, saturation, value);
            }
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (color != value)
            {
                color = value;
                hue = color.GetHue();
                saturation = color.GetSaturation();
                luminosity = color.GetLuminosity();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Saturation"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Luminosity"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));

                Name = NamedColor.GetNearestColorName(color);
            }
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
        private set
        {
            if (name != value)
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
    }
}
```

In this example, the `HslColorViewModel` class defines `Hue`, `Saturation`, `Luminosity`, `Color`, and `Name` properties. When any one of the three color components changes value, the `Color` property is recalculated, and `PropertyChanged` events are raised for all four properties. When the `Color` property changes, the static `GetNearestColorName` method in the `NamedColor` class obtains the closest named color and sets the `Name` property.

When a viewmodel is set as a binding source, the binding infrastructure attaches a handler to the `PropertyChanged` event. In this way, the binding can be notified of changes to properties, and can then set the target properties from the changed values. However, when a target property (or the `Binding` definition on a target property) has a `BindingMode` of `OneTime`, it is not necessary for the binding infrastructure to attach a handler on the `PropertyChanged` event. The target property is updated only when the `BindingContext` changes and not when the source property itself changes.

The following XAML consumes the `HslColorViewModel`:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:DataBindingDemos"
             x:Class="DataBindingDemos.SimpleColorSelectorPage"
             x:DataType="local:HslColorViewModel">
    <ContentPage.BindingContext>
        <local:HslColorViewModel Color="MediumTurquoise" />
    </ContentPage.BindingContext>

    <ContentPage.Resources>
        <Style TargetType="Slider">
            <Setter Property="VerticalOptions" Value="CenterAndExpand" />
        </Style>
    </ContentPage.Resources>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <BoxView Color="{Binding Color}"
                 Grid.Row="0" />
        <StackLayout Grid.Row="1"
                     Margin="10, 0">
            <Label Text="{Binding Name}"
                   HorizontalTextAlignment="Center" />
            <Slider Value="{Binding Hue}" />
            <Slider Value="{Binding Saturation}" />
            <Slider Value="{Binding Luminosity}" />
        </StackLayout>
    </Grid>
</ContentPage>
```

In this example, the `HslColorViewModel` is instantiated, and `Color` property set, and set as the page's `BindingContext`. The <xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Label>, and three <xref:Microsoft.Maui.Controls.Slider> views inherit the binding context from the <xref:Microsoft.Maui.Controls.ContentPage>. These views are all binding targets that reference source properties in the viewmodel. For the `Color` property of the <xref:Microsoft.Maui.Controls.BoxView>, and the `Text` property of the <xref:Microsoft.Maui.Controls.Label>, the data bindings are `OneWay` - the properties in the view are set from the properties in the viewmodel. The `Value` property of the <xref:Microsoft.Maui.Controls.Slider>, however, uses a `TwoWay` binding mode. This enables each <xref:Microsoft.Maui.Controls.Slider> to be set from the viewmodel, and also for the viewmodel to be set from each <xref:Microsoft.Maui.Controls.Slider>.

When the example is first run, the <xref:Microsoft.Maui.Controls.BoxView>, <xref:Microsoft.Maui.Controls.Label>, and three <xref:Microsoft.Maui.Controls.Slider> elements are all set from the viewmodel based on the initial `Color` property set when the viewmodel was instantiated:

:::image type="content" source="media/binding-mode/simplecolorselector.png" alt-text="Simple color selector.":::

As you manipulate the sliders, the <xref:Microsoft.Maui.Controls.BoxView> and <xref:Microsoft.Maui.Controls.Label> are updated accordingly.

## Overriding the binding mode

The binding mode for a target property can be overridden by setting the `Mode` property of `Binding` (or the `Mode` property of the `Binding` markup extension) to one of the members of the `BindingMode` enumeration.

However, setting the `Mode` property doesn't always produce the expected result. For example, in the following example setting the `Mode` property to `TwoWay` doesn't work as you might expect:

```xaml
<Label Text="TEXT"
       FontSize="40"
       HorizontalOptions="Center"
       VerticalOptions="CenterAndExpand"
       Scale="{Binding x:DataType='Slider',
                       Source={x:Reference slider},
                       Path=Value,
                       Mode=TwoWay}" />
```

In this example, it might be expected that the <xref:Microsoft.Maui.Controls.Slider> would be initialized to the initial value of the `Scale` property, which is 1, but that doesn't happen. When a `TwoWay` binding is initialized, the target is set from the source first, which means that the `Scale` property is set to the <xref:Microsoft.Maui.Controls.Slider> default value of 0. When the `TwoWay` binding is set on the <xref:Microsoft.Maui.Controls.Slider>, then the <xref:Microsoft.Maui.Controls.Slider> is initially set from the source.

Alternatively, you can set the binding mode to `OneWayToSource`:

```xaml
<Label Text="TEXT"
       FontSize="40"
       HorizontalOptions="Center"
       VerticalOptions="CenterAndExpand"
       Scale="{Binding x:DataType='Slider',
                       Source={x:Reference slider},
                       Path=Value,
                       Mode=OneWayToSource}" />
```

Now the <xref:Microsoft.Maui.Controls.Slider> is initialized to 1 (the default value of `Scale`) but manipulating the <xref:Microsoft.Maui.Controls.Slider> doesn't affect the `Scale` property.

> [!NOTE]
> The <xref:Microsoft.Maui.Controls.VisualElement> class also defines `ScaleX` and `ScaleY` properties, which can scale the <xref:Microsoft.Maui.Controls.VisualElement> differently in the horizontal and vertical directions.

A very useful application of overriding the default binding mode with a `TwoWay` binding mode involves the `SelectedItem` property of <xref:Microsoft.Maui.Controls.ListView>. The default binding mode is `OneWayToSource`. When a data binding is set on the `SelectedItem` property to reference a source property in a viewmodel, then that source property is set from the <xref:Microsoft.Maui.Controls.ListView> selection. However, in some circumstances, you might also want the <xref:Microsoft.Maui.Controls.ListView> to be initialized from the viewmodel.

> [!IMPORTANT]
> The default binding mode can vary from control to control, and is set when the bindable property is created. It's available from the `DefaultBindingMode` property of the <xref:Microsoft.Maui.Controls.BindableProperty> object.
