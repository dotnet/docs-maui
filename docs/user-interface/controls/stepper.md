---
title: "Stepper"
description: "The .NET MAUI Stepper allows you to select a numeric value from a range of values. It consists of two buttons labeled with minus and plus signs. Manipulating the two buttons changes the selected value incrementally."
ms.date: 08/30/2024
---

# Stepper

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Stepper> enables a numeric value to be selected from a range of values. It consists of two buttons labeled with minus and plus signs. These buttons can be manipulated by the user to incrementally select a `double` value from a range of values.

The <xref:Microsoft.Maui.Controls.Stepper> defines four properties of type `double`:

- `Increment` is the amount to change the selected value by, with a default value of 1.
- `Minimum` is the minimum of the range, with a default value of 0.
- `Maximum` is the maximum of the range, with a default value of 100.
- `Value` is the stepper's value, which can range between `Minimum` and `Maximum` and has a default value of 0.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects. The `Value` property has a default binding mode of `BindingMode.TwoWay`, which means that it's suitable as a binding source in an application that uses the Model-View-ViewModel (MVVM) pattern.

The <xref:Microsoft.Maui.Controls.Stepper> coerces the `Value` property so that it is between `Minimum` and `Maximum`, inclusive. If the `Minimum` property is set to a value greater than the `Value` property, the <xref:Microsoft.Maui.Controls.Stepper> sets the `Value` property to `Minimum`. Similarly, if `Maximum` is set to a value less than `Value`, then <xref:Microsoft.Maui.Controls.Stepper> sets the `Value` property to `Maximum`. Internally, the <xref:Microsoft.Maui.Controls.Stepper> ensures that `Minimum` is less than `Maximum`. If `Minimum` or `Maximum` are ever set so that `Minimum` is not less than `Maximum`, an exception is raised. For more information on setting the `Minimum` and `Maximum` properties, see [Precautions](#precautions).

<xref:Microsoft.Maui.Controls.Stepper> defines a `ValueChanged` event that's raised when the `Value` changes, either through user manipulation of the <xref:Microsoft.Maui.Controls.Stepper> or when the application sets the `Value` property directly. A `ValueChanged` event is also raised when the `Value` property is coerced as previously described. The `ValueChangedEventArgs` object that accompanies the `ValueChanged` event has `OldValue` and `NewValue`, of type `double`. At the time the event is raised, the value of `NewValue` is the same as the `Value` property of the <xref:Microsoft.Maui.Controls.Stepper> object.

## Create a Stepper

The following example shows how to create a <xref:Microsoft.Maui.Controls.Stepper>, with two <xref:Microsoft.Maui.Controls.Label> objects:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StepperDemo.BasicStepperXAMLPage"
             Title="Basic Stepper XAML">
    <StackLayout Margin="20">
        <Label x:Name="_rotatingLabel"
               Text="ROTATING TEXT"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Stepper Maximum="360"
                 Increment="30"
                 HorizontalOptions="Center"
                 ValueChanged="OnStepperValueChanged" />
        <Label x:Name="_displayLabel"
               Text="(uninitialized)"
               HorizontalOptions="Center"
               VerticalOptions="Center" />        
    </StackLayout>
</ContentPage>
```

In this example, the <xref:Microsoft.Maui.Controls.Stepper> is initialized to have a `Maximum` property of 360, and an `Increment` property of 30. Manipulating the <xref:Microsoft.Maui.Controls.Stepper> changes the selected value incrementally between `Minimum` to `Maximum` based on the value of the `Increment` property. The second <xref:Microsoft.Maui.Controls.Label> displays the text "(uninitialized)" until the <xref:Microsoft.Maui.Controls.Stepper> is manipulated, which causes the first `ValueChanged` event to be raised.

The code-behind file contains the handler for the `ValueChanged` event:

```csharp
public partial class BasicStepperXAMLPage : ContentPage
{
    public BasicStepperXAMLPage()
    {
        InitializeComponent();
    }

    void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        double value = e.NewValue;
        _rotatingLabel.Rotation = value;
        _displayLabel.Text = string.Format("The Stepper value is {0}", value);
    }
}
```

The `ValueChanged` handler of the <xref:Microsoft.Maui.Controls.Stepper> uses the `Value` property of the `stepper` object to set the `Rotation` property of the first <xref:Microsoft.Maui.Controls.Label>  and uses the `string.Format` method with the `NewValue` property of the event arguments to set the `Text` property of the second <xref:Microsoft.Maui.Controls.Label>:

:::image type="content" source="media/stepper/stepper-basic.png" alt-text=".NET MAUI Stepper screenshot.":::

It's also possible for the event handler to obtain the <xref:Microsoft.Maui.Controls.Stepper> that is firing the event through the `sender` argument. The `Value` property contains the current value:

```csharp
double value = ((Stepper)sender).Value;
```

If the <xref:Microsoft.Maui.Controls.Stepper> object were given a name in the XAML file with an `x:Name` attribute (for example, "stepper"), then the event handler could reference that object directly:

```csharp
double value = stepper.Value;
```

The equivalent C# code for creating a <xref:Microsoft.Maui.Controls.Stepper> is:

```csharp
Stepper stepper = new Stepper
{
    Maximum = 360,
    Increment = 30,
    HorizontalOptions = LayoutOptions.Center
};
stepper.ValueChanged += (sender, e) =>
{
    rotationLabel.Rotation = stepper.Value;
    displayLabel.Text = string.Format("The Stepper value is {0}", e.NewValue);
};
```

## Data bind a Stepper

The `ValueChanged` event handler can be eliminated by using data binding to respond to the <xref:Microsoft.Maui.Controls.Stepper> value changing:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="StepperDemo.BasicStepperBindingsPage"
             Title="Basic Stepper Bindings">
    <StackLayout Margin="20">
        <Label Text="ROTATING TEXT"
               Rotation="{Binding x:DataType='Stepper', Source={x:Reference _stepper}, Path=Value}"
               FontSize="18"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
        <Stepper x:Name="_stepper"
                 Maximum="360"
                 Increment="30"
                 HorizontalOptions="Center" />
        <Label Text="{Binding x:DataType='Stepper', Source={x:Reference _stepper}, Path=Value, StringFormat='The Stepper value is {0:F0}'}"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </StackLayout>
</ContentPage>
```

In this example, the `Rotation` property of the first <xref:Microsoft.Maui.Controls.Label> is bound to the `Value` property of the <xref:Microsoft.Maui.Controls.Stepper>, as is the `Text` property of the second <xref:Microsoft.Maui.Controls.Label> with a `StringFormat` specification. When the page first appears, the second <xref:Microsoft.Maui.Controls.Label> displays the text string with the value. To display text without data binding, you'd need to specifically initialize the `Text` property of the <xref:Microsoft.Maui.Controls.Label> or simulate a firing of the `ValueChanged` event by calling the event handler from the class constructor.

## Precautions

The value of the `Minimum` property must always be less than the value of the `Maximum` property. The following code example causes the <xref:Microsoft.Maui.Controls.Stepper> to raise an exception:

```csharp
// Throws an exception!
Stepper stepper = new Stepper
{
    Minimum = 180,
    Maximum = 360
};
```

The C# compiler generates code that sets these two properties in sequence, and when the `Minimum` property is set to 180, it is greater than the default `Maximum` value of 100. You can avoid the exception in this case by setting the `Maximum` property first:

```csharp
Stepper stepper = new Stepper
{
    Maximum = 360,
    Minimum = 180
};
```

In this example, setting `Maximum` to 360 is not a problem because it is greater than the default `Minimum` value of 0. When `Minimum` is set, the value is less than the `Maximum` value of 360.

The same problem exists in XAML. Set the properties in an order that ensures that `Maximum` is always greater than `Minimum`:

```xaml
<Stepper Maximum="360"
         Minimum="180" ... />
```

You can set the `Minimum` and `Maximum` values to negative numbers, but only in an order where `Minimum` is always less than `Maximum`:

```xaml
<Stepper Minimum="-360"
         Maximum="-180" ... />
```

The `Value` property is always greater than or equal to the `Minimum` value and less than or equal to `Maximum`. If `Value` is set to a value outside that range, the value will be coerced to lie within the range, but no exception is raised. For example, this code won't raise an exception:

```csharp
Stepper stepper = new Stepper
{
    Value = 180
};
```

Instead, the `Value` property is coerced to the `Maximum` value of 100.

A previous example set `Maximum` to 360 and `Minimum` to 180:

```csharp
Stepper stepper = new Stepper
{
    Maximum = 360,
    Minimum = 180
};
```

When `Minimum` is set to 180, then `Value` is also set to 180.

If a `ValueChanged` event handler has been attached at the time that the `Value` property is coerced to something other than its default value of 0, then a `ValueChanged` event is raised:

```xaml
<Stepper ValueChanged="OnStepperValueChanged"
         Maximum="360"
         Minimum="180" />
```

When `Minimum` is set to 180, `Value` is also set to 180, and the `ValueChanged` event is raised. This might occur before the rest of the page has been constructed, and the handler might attempt to reference other elements on the page that have not yet been created. You might want to add some code to the `ValueChanged` handler that checks for `null` values of other elements on the page. Or, you can set the `ValueChanged` event handler after the <xref:Microsoft.Maui.Controls.Stepper> values have been initialized.
