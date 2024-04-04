---
title: "XAML namespaces"
description: ".NET MAUI XAML uses the xmlns XML attribute for namespace declarations. The default namespace specifies that elements defined within the XAML file with no prefix refer to .NET MAUI classes."
ms.date: 10/19/2023
---

# XAML namespaces

XAML uses the `xmlns` XML attribute for namespace declarations. There are two XAML namespace declarations that are always within the root element of a XAML file. The first defines the default namespace:

```xaml
xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
```

The default namespace specifies that elements defined within the XAML file with no prefix refer to .NET Multi-platform App UI (.NET MAUI) classes, such as <xref:Microsoft.Maui.Controls.ContentPage>, <xref:Microsoft.Maui.Controls.Label>, and <xref:Microsoft.Maui.Controls.Button>.

The second namespace declaration uses the `x` prefix:

```xaml
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
```

XAML uses prefixes to declare non-default namespaces, with the prefix being used when referencing types within the namespace. The `x` namespace declaration specifies that elements defined within XAML with a prefix of `x` are used for elements and attributes that are intrinsic to XAML (specifically the 2009 XAML specification).

The following table outlines the `x` constructs supported by .NET MAUI:

::: moniker range="=net-maui-7.0"

|Construct|Description|
|--- |--- |
|`x:Arguments`|Specifies constructor arguments for a non-default constructor, or for a factory method object declaration.|
|`x:Class`|Specifies the namespace and class name for a class defined in XAML. The class name must match the class name of the code-behind file. Note that this construct can only appear in the root element of a XAML file.|
|`x:DataType`|Specifies the type of the object that the XAML element, and it's children, will bind to.|
|`x:FactoryMethod`|Specifies a factory method that can be used to initialize an object.|
|`x:FieldModifier`|Specifies the access level for generated fields for named XAML elements.|
|`x:Key`|Specifies a unique user-defined key for each resource in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. The key's value is used to retrieve the XAML resource, and is typically used as the argument for the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension.|
|`x:Name`|Specifies a runtime object name for the XAML element. Setting `x:Name` is similar to declaring a variable in code.|
|`x:TypeArguments`|Specifies the generic type arguments to the constructor of a generic type.|

For more information about the `x:DataType` attribute, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md). For more information about the `x:FieldModifier` attribute, see [Field modifiers](~/xaml/field-modifiers.md). For more information about the `x:Arguments` and `x:FactoryMethod` attributes, see [Pass arguments](~/xaml/pass-arguments.md). For more information about the `x:TypeArguments` attribute, see [Generics](~/xaml/generics.md).

::: moniker-end

::: moniker range=">=net-maui-8.0"

|Construct|Description|
|--- |--- |
|`x:Arguments`|Specifies constructor arguments for a non-default constructor, or for a factory method object declaration.|
|`x:Class`|Specifies the namespace and class name for a class defined in XAML. The class name must match the class name of the code-behind file. Note that this construct can only appear in the root element of a XAML file.|
|`x:ClassModifier`|Specifies the access level for the generated class in the assembly.|
|`x:DataType`|Specifies the type of the object that the XAML element, and it's children, will bind to.|
|`x:FactoryMethod`|Specifies a factory method that can be used to initialize an object.|
|`x:FieldModifier`|Specifies the access level for generated fields for named XAML elements.|
|`x:Key`|Specifies a unique user-defined key for each resource in a <xref:Microsoft.Maui.Controls.ResourceDictionary>. The key's value is used to retrieve the XAML resource, and is typically used as the argument for the [`StaticResource`](xref:Microsoft.Maui.Controls.Xaml.StaticResourceExtension) markup extension.|
|`x:Name`|Specifies a runtime object name for the XAML element. Setting `x:Name` is similar to declaring a variable in code.|
|`x:TypeArguments`|Specifies the generic type arguments to the constructor of a generic type.|

For more information about the `x:ClassModifier` attribute, see [Class modifiers](~/xaml/class-modifiers.md). For more information about the `x:DataType` attribute, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md). For more information about the `x:FieldModifier` attribute, see [Field modifiers](~/xaml/field-modifiers.md). For more information about the `x:Arguments` and `x:FactoryMethod` attributes, see [Pass arguments](~/xaml/pass-arguments.md). For more information about the `x:TypeArguments` attribute, see [Generics](~/xaml/generics.md).

::: moniker-end

> [!NOTE]
> In addition to the constructs listed above, .NET MAUI also includes markup extensions that can be consumed through the `x` namespace prefix. For more information, see [Consume XAML Markup Extensions](~/xaml/markup-extensions/consume.md).

In XAML, namespace declarations inherit from parent element to child element. Therefore, when defining a namespace in the root element of a XAML file, all elements within that file inherit the namespace declaration.

## Declare namespaces for types

Types can be referenced in XAML by declaring a XAML namespace with a prefix, with the namespace declaration specifying the Common Language Runtime (CLR) namespace name, and optionally an assembly name. This is achieved by defining values for the following keywords within the namespace declaration:

- `clr-namespace:` or `using:` – the CLR namespace declared within the assembly that contains the types to expose as XAML elements. This keyword is required.
- `assembly=` – the assembly that contains the referenced CLR namespace. This value is the name of the assembly, without the file extension. The path to the assembly should be established as a reference in the project that contains the XAML file that will reference the assembly. This keyword can be omitted if the **clr-namespace** value is within the same assembly as the app code that's referencing the types.

> [!NOTE]
> The character separating the `clr-namespace` or `using` token from its value is a colon, whereas the character separating the `assembly` token from its value is an equal sign. The character to use between the two tokens is a semicolon.

The following code example shows a XAML namespace declaration:

```xaml
<ContentPage ... xmlns:local="clr-namespace:MyMauiApp">
  ...
</ContentPage>
```

Alternatively, this can be written as:

```xaml
<ContentPage ... xmlns:local="using:MyMauiApp">
  ...
</ContentPage>
```

The `local` prefix is a convention used to indicate that the types within the namespace are local to the app. Alternatively, if the types are in a different assembly, the assembly name should also be defined in the namespace declaration:

```xaml
<ContentPage ... xmlns:controls="clr-namespace:Controls;assembly=MyControlLibrary" ...>
  ...
</ContentPage>
```

The namespace prefix is then specified when declaring an instance of a type from an imported namespace:

```xaml
<controls:Expander IsExpanded="True">
    ...
</controls:Expander>
```

For information about defining a custom namespace schema, see [Custom namespace schemas](custom-namespace-schemas.md).
