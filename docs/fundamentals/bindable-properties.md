---
title: "Bindable properties"
description: ".NET MAUI bindable properties provide a property system that supports data binding, styles, tempaltes, and values set through parent-child relationships."
ms.date: 10/05/2023
---

# Bindable properties

.NET Multi-platform App UI (.NET MAUI) bindable properties extend Common Language Runtime (CLR) property functionality by backing a property with a <xref:Microsoft.Maui.Controls.BindableProperty> type, instead of with a field. The purpose of bindable properties is to provide a property system that supports data binding, styles, templates, and values set through parent-child relationships. In addition, bindable properties can provide default values, validation of property values, and callbacks that monitor property changes.

In .NET MAUI apps, properties should be implemented as bindable properties to support one or more of the following features:

- Acting as a valid *target* property for data binding. For more information about target properties, see [Basic bindings](~/fundamentals/data-binding/basic-bindings.md).
- Setting the property through a style.
- Providing a default property value that's different from the default for the type of the property.
- Validating the value of the property.
- Monitoring property changes.

Examples of .NET MAUI bindable properties include `Label.Text`, `Button.BorderRadius`, and `StackLayout.Orientation`. Each bindable property has a corresponding `public static readonly` field of type <xref:Microsoft.Maui.Controls.BindableProperty> that is exposed on the same class and that is the identifier of the bindable property. For example, the corresponding bindable property identifier for the `Label.Text` property is `Label.TextProperty`.

## Create a bindable property

The process for creating a bindable property is as follows:

1. Create a <xref:Microsoft.Maui.Controls.BindableProperty> instance with one of the `BindableProperty.Create` method overloads.
1. Define property accessors for the <xref:Microsoft.Maui.Controls.BindableProperty> instance.

All <xref:Microsoft.Maui.Controls.BindableProperty> instances must be created on the UI thread. This means that only code that runs on the UI thread can get or set the value of a bindable property. However, <xref:Microsoft.Maui.Controls.BindableProperty> instances can be accessed from other threads by marshaling to the UI thread. For more information, see [Run code on the UI thread](~/platform-integration/appmodel/main-thread.md#run-code-on-the-ui-thread).

### Create a property

To create a <xref:Microsoft.Maui.Controls.BindableProperty> instance, the containing class must derive from the <xref:Microsoft.Maui.Controls.BindableObject> class. However, the <xref:Microsoft.Maui.Controls.BindableObject> class is high in the class hierarchy, so the majority of classes used for UI functionality support bindable properties.

A bindable property can be created by declaring a `public static readonly` property of type <xref:Microsoft.Maui.Controls.BindableProperty>. The bindable property should be set to the returned value of one of the `BindableProperty.Create` method overloads. The declaration should be within the body of <xref:Microsoft.Maui.Controls.BindableObject> derived class, but outside of any member definitions.

At a minimum, an identifier must be specified when creating a <xref:Microsoft.Maui.Controls.BindableProperty>, along with the following parameters:

- The name of the <xref:Microsoft.Maui.Controls.BindableProperty>.
- The type of the property.
- The type of the owning object.
- The default value for the property. This ensures that the property always returns a particular default value when it is unset, and it can be different from the default value for the type of the property. The default value will be restored when the `ClearValue` method is called on the bindable property.

> [!IMPORTANT]
> The naming convention for bindable properties is that the bindable property identifier must match the property name specified in the `Create` method, with "Property" appended to it.

The following code shows an example of a bindable property, with an identifier and values for the four required parameters:

```csharp
public static readonly BindableProperty IsExpandedProperty =
  BindableProperty.Create ("IsExpanded", typeof(bool), typeof(Expander), false);
```

This creates a <xref:Microsoft.Maui.Controls.BindableProperty> instance named `IsExpandedProperty`, of type `bool`. The property is owned by the `Expander` class, and has a default value of `false`.

> [!NOTE]
> `Expander` is a control in .NET MAUI Community Toolkit. For more information, see [Expander](/dotnet/communitytoolkit/maui/views/expander).

Optionally, when creating a <xref:Microsoft.Maui.Controls.BindableProperty> instance, the following parameters can be specified:

- The binding mode. This is used to specify the direction in which property value changes will propagate. In the default binding mode, changes will propagate from the *source* to the *target*. For more information, see [Basic bindings](~/fundamentals/data-binding/basic-bindings.md).
- A validation delegate that will be invoked when the property value is set. For more information, see [Validation callbacks](#validation-callbacks).
- A property changed delegate that will be invoked when the property value has changed. For more information, see [Detect property changes](#detect-property-changes).
- A property changing delegate that will be invoked when the property value will change. This delegate has the same signature as the property changed delegate.
- A coerce value delegate that will be invoked when the property value has changed. For more information, see [Coerce value callbacks](#coerce-value-callbacks).
- A `Func` that's used to initialize a default property value. For more information, see [Create a default value with a Func](#create-a-default-value-with-a-func).

### Create accessors

Property accessors are required to use property syntax to access a bindable property. The `Get` accessor should return the value that's contained in the corresponding bindable property. This can be achieved by calling the `GetValue` method, passing in the bindable property identifier on which to get the value, and then casting the result to the required type. The `Set` accessor should set the value of the corresponding bindable property. This can be achieved by calling the `SetValue` method, passing in the bindable property identifier on which to set the value, and the value to set.

The following code example shows accessors for the `IsExpanded` bindable property:

```csharp
public bool IsExpanded
{
    get => (bool)GetValue(IsExpandedProperty);
    set => SetValue(IsExpandedProperty, value);
}
```

## Consume a bindable property

Once a bindable property has been created, it can be consumed from XAML or code. In XAML, this is achieved by declaring a namespace with a prefix, with the namespace declaration indicating the CLR namespace name, and optionally, an assembly name. For more information, see [XAML Namespaces](~/xaml/namespaces/index.md).

The following code example demonstrates a XAML namespace for a custom type that contains a bindable property, which is defined within the same assembly as the application code that's referencing the custom type:

```xaml
<ContentPage ... xmlns:local="clr-namespace:DataBindingDemos" ...>
  ...
</ContentPage>
```

The namespace declaration is used when setting the `IsExpanded` bindable property, as demonstrated in the following XAML code example:

```xaml
<Expander IsExpanded="true">
    ...
</Expander>
```

The equivalent C# code is shown in the following code example:

```csharp
Expander expander = new Expander
{
    IsExpanded = true
};
```

## Advanced scenarios

When creating a <xref:Microsoft.Maui.Controls.BindableProperty> instance, there are a number of optional parameters that can be set to enable advanced bindable property scenarios. This section explores these scenarios.

### Detect property changes

A `static` property-changed callback method can be registered with a bindable property by specifying the `propertyChanged` parameter for the `BindableProperty.Create` method. The specified callback method will be invoked when the value of the bindable property has changed.

The following code example shows how the `IsExpanded` bindable property registers the `OnIsExpandedChanged` method as a property-changed callback method:

```csharp
public static readonly BindableProperty IsExpandedProperty =
    BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(Expander), false, propertyChanged: OnIsExpandedChanged);
...

static void OnIsExpandedChanged (BindableObject bindable, object oldValue, object newValue)
{
  // Property changed implementation goes here
}
```

In the property-changed callback method, the <xref:Microsoft.Maui.Controls.BindableObject> parameter is used to denote which instance of the owning class has reported a change, and the values of the two `object` parameters represent the old and new values of the bindable property.

### Validation callbacks

A `static` validation callback method can be registered with a bindable property by specifying the `validateValue` parameter for the `BindableProperty.Create` method. The specified callback method will be invoked when the value of the bindable property is set.

The following code example shows how the `Angle` bindable property registers the `IsValidValue` method as a validation callback method:

```csharp
public static readonly BindableProperty AngleProperty =
    BindableProperty.Create("Angle", typeof(double), typeof(MainPage), 0.0, validateValue: IsValidValue);
...

static bool IsValidValue(BindableObject view, object value)
{
    double result;
    double.TryParse(value.ToString(), out result);
    return (result >= 0 && result <= 360);
}
```

Validation callbacks are provided with a value, and should return `true` if the value is valid for the property, otherwise `false`. An exception will be raised if a validation callback returns `false`, which you should handle. A typical use of a validation callback method is constraining the values of integers or doubles when the bindable property is set. For example, the `IsValidValue` method checks that the property value is a `double` within the range 0 to 360.

### Coerce value callbacks

A `static` coerce value callback method can be registered with a bindable property by specifying the `coerceValue` parameter for the `BindableProperty.Create` method. The specified callback method will be invoked when the value of the bindable property is about to change, so that you can adjust the new value before it's applied.

> [!IMPORTANT]
> In addition to being triggered by the bindable property engine, you can invoke coerce-value callbacks from code. The <xref:Microsoft.Maui.Controls.BindableObject> type has a `CoerceValue` method that can be called to force a reevaluation of the value of its <xref:Microsoft.Maui.Controls.BindableProperty> argument, by invoking its coerce value callback.

Coerce value callbacks are used to force a reevaluation of a bindable property when the value of the property is about to change. For example, a coerce value callback can be used to ensure that the value of one bindable property is not greater than the value of another bindable property.

The following code example shows how the `Angle` bindable property registers the `CoerceAngle` method as a coerce value callback method:

```csharp
public static readonly BindableProperty AngleProperty =
    BindableProperty.Create("Angle", typeof(double), typeof(MainPage), 0.0, coerceValue: CoerceAngle);
public static readonly BindableProperty MaximumAngleProperty =
    BindableProperty.Create("MaximumAngle", typeof(double), typeof(MainPage), 360.0, propertyChanged: ForceCoerceValue);
...

static object CoerceAngle(BindableObject bindable, object value)
{
    MainPage page = bindable as MainPage;
    double input = (double)value;

    if (input > page.MaximumAngle)
    {
        input = page.MaximumAngle;
    }

    return input;
}

static void ForceCoerceValue(BindableObject bindable, object oldValue, object newValue)
{
    bindable.CoerceValue(AngleProperty);
}
```

The `CoerceAngle` method checks the value of the `MaximumAngle` property, and if the `Angle` property value is greater than it, it coerces the value to the `MaximumAngle` property value. In addition, when the `MaximumAngle` property changes the coerce value callback is invoked on the `Angle` property by calling the `CoerceValue` method.

### Create a default value with a Func

A `Func` can be used to initialize the default value of a bindable property, as demonstrated in the following example:

```csharp
public static readonly BindableProperty DateProperty =
    BindableProperty.Create ("Date", typeof(DateTime), typeof(MyPage), default(DateTime), BindingMode.TwoWay, defaultValueCreator: bindable => DateTime.Today);
```

The `defaultValueCreator` parameter is set to a `Func` that returns a `DateTime` that represents today's date.
