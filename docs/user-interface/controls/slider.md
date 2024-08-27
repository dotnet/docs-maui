---
title: "Slider"
description: "The .NET MAUI Slider is a horizontal bar that can be manipulated by a user to select a double value from a continuous range."
ms.date: 08/30/2024
---

# Slider

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Slider> is a horizontal bar that you can manipulate to select a `double` value from a continuous range.

<xref:Microsoft.Maui.Controls.Slider> defines the following properties:

- `Minimum`, of type `double`, is the minimum of the range, with a default value of 0.
- `Maximum`, of type `double`, is the maximum of the range, with a default value of 1.
- `Value`, of type `double`, is the slider's value, which can range between `Minimum` and `Maximum` and has a default value of 0.
- `MinimumTrackColor`, of type <xref:Microsoft.Maui.Graphics.Color>, is the bar color on the left side of the thumb.
- `MaximumTrackColor`, of type <xref:Microsoft.Maui.Graphics.Color>, is the bar color on the right side of the thumb.
- `ThumbColor` of type <xref:Microsoft.Maui.Graphics.Color>, is the thumb color.
- `ThumbImageSource`, of type <xref:Microsoft.Maui.Controls.ImageSource>, is the image to use for the thumb, of type <xref:Microsoft.Maui.Controls.ImageSource>.
- `DragStartedCommand`, of type <xref:System.Windows.Input.ICommand>, which is executed at the beginning of a drag action.
- `DragCompletedCommand`, of type <xref:System.Windows.Input.ICommand>, which is executed at the end of a drag action.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects. The `Value` property has a default binding mode of `BindingMode.TwoWay`, which means that it's suitable as a binding source in an application that uses the Model-View-ViewModel (MVVM) pattern.

> [!NOTE]
> The `ThumbColor` and `ThumbImageSource` properties are mutually exclusive. If both properties are set, the `ThumbImageSource` property will take precedence.

The <xref:Microsoft.Maui.Controls.Slider> coerces the `Value` property so that it is between `Minimum` and `Maximum`, inclusive. If the `Minimum` property is set to a value greater than the `Value` property, the <xref:Microsoft.Maui.Controls.Slider> sets the `Value` property to `Minimum`. Similarly, if `Maximum` is set to a value less than `Value`, then <xref:Microsoft.Maui.Controls.Slider> sets the `Value` property to `Maximum`. Internally, the <xref:Microsoft.Maui.Controls.Slider> ensures that `Minimum` is less than `Maximum`. If `Minimum` or `Maximum` are ever set so that `Minimum` is not less than `Maximum`, an exception is raised. For more information on setting the `Minimum` and `Maximum` properties, see [Precautions](#precautions).

<xref:Microsoft.Maui.Controls.Slider> defines a `ValueChanged` event that's raised when the `Value` changes, either through user manipulation of the <xref:Microsoft.Maui.Controls.Slider> or when the program sets the `Value` property directly. A `ValueChanged` event is also raised when the `Value` property is coerced as described in the previous paragraph. The `ValueChangedEventArgs` object that accompanies the `ValueChanged` event has `OldValue` and `NewValue` properties, of type `double`. At the time the event is raised, the value of `NewValue` is the same as the `Value` property of the <xref:Microsoft.Maui.Controls.Slider> object.

<xref:Microsoft.Maui.Controls.Slider> also defines `DragStarted` and `DragCompleted` events, that are raised at the beginning and end of the drag action. Unlike the `ValueChanged` event, the `DragStarted` and `DragCompleted` events are only raised through user manipulation of the <xref:Microsoft.Maui.Controls.Slider>. When the `DragStarted` event fires, the `DragStartedCommand`, of type <xref:System.Windows.Input.ICommand>, is executed. Similarly, when the `DragCompleted` event fires, the `DragCompletedCommand`, of type <xref:System.Windows.Input.ICommand>, is executed.

> [!WARNING]
> Do not use unconstrained horizontal layout options of `Center`, `Start`, or `End` with <xref:Microsoft.Maui.Controls.Slider>. Keep the default `HorizontalOptions` setting of `Fill`, and don't use a width of `Auto` when putting <xref:Microsoft.Maui.Controls.Slider> in a <xref:Microsoft.Maui.Controls.Grid> layout.

## Create a Slider

The following example shows how to create a <xref:Microsoft.Maui.Controls.Slider>, with two <xref:Microsoft.Maui.Controls.Label> objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="SliderDemos.BasicSliderXamlPage"
             Title="Basic Slider XAML"
             Padding="10, 0">
    <StackLayout>
        <Label x:Name="rotatingLabel"
               Text="ROTATING TEXT"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Slider Maximum="360"
                ValueChanged="OnSliderValueChanged" />
        <Label x:Name="displayLabel"
               Text="(uninitialized)"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </StackLayout>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Slider> is initialized to have a `Maximum` property of 360. The second <xref:Microsoft.Maui.Controls.Label> displays the text "(uninitialized)" until the <xref:Microsoft.Maui.Controls.Slider> is manipulated, which causes the first `ValueChanged` event to be raised.

The code-behind file contains the handler for the `ValueChanged` event:

```csharp
public partial class BasicSliderXamlPage : ContentPage
{
    public BasicSliderXamlPage()
    {
        InitializeComponent();
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        rotatingLabel.Rotation = value;
        displayLabel.Text = String.Format("The Slider value is {0}", value);
    }
}
```

The `ValueChanged` handler of the <xref:Microsoft.Maui.Controls.Slider> uses the `Value` property of the `slider` object to set the `Rotation` property of the first <xref:Microsoft.Maui.Controls.Label> and uses the `String.Format` method with the `NewValue` property of the event arguments to set the `Text` property of the second <xref:Microsoft.Maui.Controls.Label>:

:::image type="content" source="media/slider/slider-basic.png" alt-text=".NET MAUI Slider screenshot.":::

It's also possible for the event handler to obtain the <xref:Microsoft.Maui.Controls.Slider> that is firing the event through the `sender` argument. The `Value` property contains the current value:

```csharp
double value = ((Slider)sender).Value;
```

If the <xref:Microsoft.Maui.Controls.Slider> object were given a name in the XAML file with an `x:Name` attribute (for example, "slider"), then the event handler could reference that object directly:

```csharp
double value = slider.Value;
```

The equivalent C# code for creating a <xref:Microsoft.Maui.Controls.Slider> is:

```csharp
Slider slider = new Slider
{
    Maximum = 360
};
slider.ValueChanged += (sender, args) =>
{
    rotationLabel.Rotation = slider.Value;
    displayLabel.Text = String.Format("The Slider value is {0}", args.NewValue);
};
```

## Data bind a Slider

The `ValueChanged` event handler can be eliminated by using data binding to respond to the <xref:Microsoft.Maui.Controls.Slider> value changing:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="SliderDemos.BasicSliderBindingsPage"
             Title="Basic Slider Bindings"
             Padding="10, 0">
    <StackLayout>
        <Label Text="ROTATING TEXT"
               Rotation="{Binding Source={x:Reference slider},
                                  Path=Value}"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Slider x:Name="slider"
                Maximum="360" />
        <Label x:Name="displayLabel"
               Text="{Binding Source={x:Reference slider},
                              Path=Value,
                              StringFormat='The Slider value is {0:F0}'}"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </StackLayout>
</ContentPage>
```

In this example, the `Rotation` property of the first <xref:Microsoft.Maui.Controls.Label> is bound to the `Value` property of the <xref:Microsoft.Maui.Controls.Slider>, as is the `Text` property of the second <xref:Microsoft.Maui.Controls.Label> with a `StringFormat` specification. When the page first appears, the second <xref:Microsoft.Maui.Controls.Label> displays the text string with the value. To display text without data binding, you'd need to specifically initialize the `Text` property of the <xref:Microsoft.Maui.Controls.Label> or simulate a firing of the `ValueChanged` event by calling the event handler from the class constructor.

## Precautions

The value of the `Minimum` property must always be less than the value of the `Maximum` property. The following example causes the <xref:Microsoft.Maui.Controls.Slider> to raise an exception:

```csharp
// Throws an exception!
Slider slider = new Slider
{
    Minimum = 10,
    Maximum = 20
};
```

The C# compiler generates code that sets these two properties in sequence, and when the `Minimum` property is set to 10, it is greater than the default `Maximum` value of 1. You can avoid the exception in this case by setting the `Maximum` property first:

```csharp
Slider slider = new Slider
{
    Maximum = 20,
    Minimum = 10
};
```

In this example, setting `Maximum` to 20 is not a problem because it is greater than the default `Minimum` value of 0. When `Minimum` is set, the value is less than the `Maximum` value of 20.

The same problem exists in XAML. The properties must be set in an order that ensures that `Maximum` is always greater than `Minimum`:

```xaml
<Slider Maximum="20"
        Minimum="10" ... />
```

You can set the `Minimum` and `Maximum` values to negative numbers, but only in an order where `Minimum` is always less than `Maximum`:

```xaml
<Slider Minimum="-20"
        Maximum="-10" ... />
```

The `Value` property is always greater than or equal to the `Minimum` value and less than or equal to `Maximum`. If `Value` is set to a value outside that range, the value will be coerced to lie within the range, but no exception is raised. For example, the following example won't raise an exception:

```csharp
Slider slider = new Slider
{
    Value = 10
};
```

Instead, the `Value` property is coerced to the `Maximum` value of 1.

A previous example set `Maximum` to 20, and `Minimum` to 10:

```csharp
Slider slider = new Slider
{
    Maximum = 20,
    Minimum = 10
};
```

When `Minimum` is set to 10, then `Value` is also set to 10.

If a `ValueChanged` event handler has been attached at the time that the `Value` property is coerced to something other than its default value of 0, then a `ValueChanged` event is raised:

```xaml
<Slider ValueChanged="OnSliderValueChanged"
        Maximum="20"
        Minimum="10" />
```

When `Minimum` is set to 10, `Value` is also set to 10, and the `ValueChanged` event is raised. This might occur before the rest of the page has been constructed, and the handler might attempt to reference other elements on the page that have not yet been created. You might want to add some code to the `ValueChanged` handler that checks for `null` values of other elements on the page. Or, you can set the `ValueChanged` event handler after the <xref:Microsoft.Maui.Controls.Slider> values have been initialized.
