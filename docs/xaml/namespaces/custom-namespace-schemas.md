---
title: "Custom namespace schemas"
description: "A .NET MAUI XAML custom namespace schema can be defined with the XmlnsDefinitionAttribute class, which specifies a mapping between a custom URL and one or more CLR namespaces. The custom namespace schema can then be used in XAML namespace declarations."
ms.date: 01/26/2022
---

# Custom namespace schemas

Types in a .NET Multi-platform App UI (.NET MAUI) library can be referenced in XAML by declaring an XML namespace for the library, with the namespace declaration specifying the Common Language Runtime (CLR) namespace name and an assembly name:

```xaml
<ContentPage ...
             xmlns:controls="clr-namespace:MyCompany.Controls;assembly=MyCompany.Controls">
    ...
</ContentPage>
```

However, specifying a CLR namespace and assembly name in a `xmlns` definition can be awkward and error prone. In addition, multiple XML namespace declarations may be required if the library contains types in multiple namespaces.

An alternative approach is to define a custom namespace schema, such as `http://mycompany.com/schemas/controls`, that maps to one or more CLR namespaces. This enables a single XML namespace declaration to reference all the types in an assembly, even if they are in different namespaces. It also enables a single XML namespace declaration to reference types in multiple assemblies.

For more information about XAML namespaces, see [XAML namespaces](index.md).

## Define a custom namespace schema

Imagine a library that exposes some simple controls, such as `CircleButton`:

```csharp
namespace MyCompany.Controls
{
    public class CircleButton : Button
    {
        ...
    }
}
```

All the controls in the library reside in the `MyCompany.Controls` namespace. These controls can be exposed to a calling assembly through a custom namespace schema.

A custom namespace schema is defined with the `XmlnsDefinitionAttribute` class, which specifies the mapping between a XAML namespace and one or more CLR namespaces. The `XmlnsDefinitionAttribute` takes two arguments: the XAML namespace name, and the CLR namespace name. The XAML namespace name is stored in the `XmlnsDefinitionAttribute.XmlNamespace` property, and the CLR namespace name is stored in the `XmlnsDefinitionAttribute.ClrNamespace` property.

> [!NOTE]
> The `XmlnsDefinitionAttribute` class also has a property named `AssemblyName`, which can be optionally set to the name of the assembly. This is only required when a CLR namespace referenced from a `XmlnsDefinitionAttribute` is in a external assembly.

The `XmlnsDefinitionAttribute` should be defined at the assembly level in the project that contains the CLR namespaces that will be mapped in the custom namespace schema:

```csharp
using MyCompany.Controls;

[assembly: XmlnsDefinition("http://mycompany.com/schemas/controls", "MyCompany.Controls")]
```

This code creates a custom namespace schema that maps the `http://mycompany.com/schemas/controls` URL to the `MyCompany.Controls` CLR namespace.

> [!IMPORTANT]
> You should instruct the linker to preserve types in the assembly that are mapped through the custom namespace schema, or preserve the entire assembly. For more information, see [Preserve code](~/android/linking.md#preserve-code).

The custom namespace schema can then be used for type resolution in XAML files.

## Consume a custom namespace schema

To consume types from the custom namespace schema, the XAML compiler requires that there's a code reference from the assembly that consumes the types, to the assembly that defines the types. This can be accomplished by adding a class containing an `Init` method to the assembly that defines the types that will be consumed through XAML:

```csharp
namespace MyCompany.Controls
{
    public static class Controls
    {
        public static void Init()
        {
        }
    }
}
```

The `Init` method can then be called from the assembly that consumes types from the custom namespace schema:

```csharp
using MyCompany.Controls;

namespace CustomNamespaceSchemaDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            Controls.Init();
            InitializeComponent();
        }
    }
}
```

> [!WARNING]
> Failure to include such a code reference will result in the XAML compiler being unable to locate the assembly containing the custom namespace schema types.

To consume the `CircleButton` control, a XAML namespace is declared, with the namespace declaration specifying the custom namespace schema URL:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="http://mycompany.com/schemas/controls"
             x:Class="CustomNamespaceSchemaDemo.MainPage">
    <StackLayout>
        ...
        <controls:CircleButton Text="+"
                               BackgroundColor="Fuchsia"
                               BorderColor="Black"
                               CircleDiameter="100" />
        <controls:CircleButton Text="-"
                               BackgroundColor="Teal"
                               BorderColor="Silver"
                               CircleDiameter="70" />
        ...
    </StackLayout>
</ContentPage>
```

`CircleButton` instances can then be added to the <xref:Microsoft.Maui.Controls.ContentPage> by declaring them with the `controls` namespace prefix.

To find the custom namespace schema types, .NET MAUI will search referenced assemblies for `XmlnsDefinitionAttribute` instances. If the `xmlns` attribute for an element in a XAML file matches the `XmlNamespace` property value in a `XmlnsDefinitionAttribute`, .NET MAUI will attempt to use the `XmlnsDefinitionAttribute.ClrNamespace` property value for resolution of the type. If type resolution fails, .NET MAUI will continue to attempt type resolution based on any additional matching `XmlnsDefinitionAttribute` instances.

The result is that two `CircleButton` instances are displayed:

:::image type="content" source="media/custom-namespace-schemas/circle-buttons.png" alt-text="Screenshot of two circle buttons.":::
