---
title: "Attached properties"
description: ".NET MAUI attached properties enable an object to assign a value for a property that its own class doesn't define."
ms.date: 01/24/2022
---

# Attached properties

.NET Multi-platform App UI (.NET MAUI) attached properties enable an object to assign a value for a property that its own class doesn't define. For example, child elements can use attached properties to inform their parent element of how they are to be presented in the user interface. The <xref:Microsoft.Maui.Controls.Grid> layout enables the row and column of a child to be specified by setting the `Grid.Row` and `Grid.Column` attached properties. `Grid.Row` and `Grid.Column` are attached properties because they are set on elements that are children of a <xref:Microsoft.Maui.Controls.Grid>, rather than on the <xref:Microsoft.Maui.Controls.Grid> itself.

Bindable properties should be implemented as attached properties in the following scenarios:

- When there's a need to have a property setting mechanism available for classes other than the defining class.
- When the class represents a service that needs to be easily integrated with other classes.

For more information about bindable properties, see [Bindable properties](bindable-properties.md).

## Create an attached property

The process for creating an attached property is as follows:

1. Create a <xref:Microsoft.Maui.Controls.BindableProperty> instance with one of the `CreateAttached` method overloads.
1. Provide `static` `Get`*PropertyName* and `Set`*PropertyName* methods as accessors for the attached property.

### Create a property

When creating an attached property for use on other types, the class where the property is created does not have to derive from <xref:Microsoft.Maui.Controls.BindableObject>. However, the *target* property for accessors should be of, or derive from, <xref:Microsoft.Maui.Controls.BindableObject>. For more information about target properties, see [Basic bindings](~/fundamentals/data-binding/basic-bindings.md).

An attached property can be created by declaring a `public static readonly` property of type <xref:Microsoft.Maui.Controls.BindableProperty>. The bindable property should be set to the returned value of one of the `BindableProperty.CreateAttached` method overloads. The declaration should be within the body of the owning class, but outside of any member definitions.

> [!IMPORTANT]
> The naming convention for attached properties is that the attached property identifier must match the property name specified in the `CreateAttached` method, with "Property" appended to it.

The following code shows an example of an attached property:

```csharp
public class MyControl
{
    public static readonly BindableProperty HasShadowProperty =
        BindableProperty.CreateAttached("HasShadow", typeof(bool), typeof(Shadow), false);
}
```

This creates an attached property named `HasShadowProperty`, of type `bool`. The property is owned by the `MyControl` class, and has a default value of `false`. Ownership, in this case, means that the property will be accessed in XAML using the format `Type.Property`, for example `MyControl.HasShadow`.

For more information about creating bindable properties, including parameters that can be specified during creation, see [Create a bindable property](bindable-properties.md#consume-a-bindable-property).

### Create accessors

Static `Get`*PropertyName* and `Set`*PropertyName* methods are required as accessors for the attached property, otherwise the property system will be unable to use the attached property. The `Get`*PropertyName* accessor should conform to the following signature:

```csharp
public static valueType GetPropertyName(BindableObject target)
```

The `Get`*PropertyName* accessor should return the value that's contained in the corresponding <xref:Microsoft.Maui.Controls.BindableProperty> field for the attached property. This can be achieved by calling the `GetValue` method, passing in the bindable property identifier on which to get the value, and then casting the resulting value to the required type.

The `Set`*PropertyName* accessor should conform to the following signature:

```csharp
public static void SetPropertyName(BindableObject target, valueType value)
```

The `Set`*PropertyName* accessor should set the value of the corresponding <xref:Microsoft.Maui.Controls.BindableProperty> field for the attached property. This can be achieved by calling the `SetValue` method, passing in the bindable property identifier on which to set the value, and the value to set.

For both accessors, the *target* object should be of, or derive from, <xref:Microsoft.Maui.Controls.BindableObject>. For more information about target objects, see [Basic bindings](~/fundamentals/data-binding/basic-bindings.md).

The following code example shows accessors for the `HasShadow` attached property:

```csharp
public class MyControl
{
    public static bool GetHasShadow(BindableObject view)
    {
        return (bool)view.GetValue (HasShadowProperty);
    }

    public static void SetHasShadow(BindableObject view, bool value)
    {
        view.SetValue(HasShadowProperty, value);
    }  
}
```

### Consume an attached property

Once an attached property has been created, it can be consumed from XAML or code. In XAML, this is achieved by declaring a namespace with a prefix, with the namespace declaration indicating the Common Language Runtime (CLR) namespace name, and optionally an assembly name. For more information, see [XAML Namespaces](~/xaml/namespaces/index.md).

The following example demonstrates a XAML namespace for a custom type that contains an attached property, which is defined within the same assembly as the app code that's referencing the custom type:

```xaml
<ContentPage ... xmlns:local="clr-namespace:ShadowDemo" ...>
  ...
</ContentPage>
```

The namespace declaration is then used when setting the attached property on a specific control, as demonstrated in the following XAML:

```xaml
<Label Text="Label with shadow" local:MyControl.HasShadow="true" />
```

The equivalent C# code is shown in the following code example:

```csharp
Label label = new Label { Text = "Label with shadow" };
MyControl.SetHasShadow (label, true);
```

### Consume an attached property with a style

Attached properties can also be added to a control by a style. The following XAML code example shows an *explicit* style for <xref:Microsoft.Maui.Controls.Label> controls that uses the `HasShadow` attached property:

```xaml
<Style x:Key="ShadowStyle" TargetType="Label">
  <Style.Setters>
    <Setter Property="local:MyControl.HasShadow" Value="true" />
  </Style.Setters>
</Style>
```

The <xref:Microsoft.Maui.Controls.Style> can be applied to a <xref:Microsoft.Maui.Controls.Label> by setting its <xref:Microsoft.Maui.Controls.NavigableElement.Style> property to the <xref:Microsoft.Maui.Controls.Style> instance using the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension, as demonstrated in the following code example:

```xaml
<Label Text="Label with shadow" Style="{StaticResource ShadowStyle}" />
```

For more information about styles, see [Styles](~/user-interface/styles/xaml.md).

## Advanced scenarios

When creating an attached property, there are some optional parameters that can be set to enable advanced attached property scenarios. This includes detecting property changes, validating property values, and coercing property values. For more information, see [Advanced scenarios](bindable-properties.md#advanced-scenarios).
