---
title: "Trim a .NET MAUI app"
description: "Learn about the .NET trimmer, which eliminates unused code from a .NET MAUI app to reduce its size."
ms.date: 10/22/2024
no-loc: [ ILLink ]
monikerRange: ">=net-maui-9.0"
---

# Trim a .NET MAUI app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app with a technique known as trimming. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

To prevent changes in behavior when trimming apps, .NET provides static analysis of trim compatibility through trim warnings. The trimmer produces trim warnings when it finds code that might not be compatible with trimming. If there are any trim warnings, the app should be thoroughly tested after trimming to ensure that there are no behavior changes. For more information, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings).

## Trimming behavior

Trimming behavior can be controlled by setting the `$(TrimMode)` build property to either `partial` or `full`:

```xml
<TrimMode>full</TrimMode>
```

The `full` trim mode removes any code that's not used by your app. The `partial` trim mode trims SDK assemblies, and any other assemblies that have opted into trimming with the `$(TrimmableAsssembly)` MSBuild item:

```xml
<ItemGroup>
  <TrimmableAssembly Include="MyAssembly" />
</ItemGroup>
```

> [!NOTE]
> This is equivalent to setting `[AssemblyMetadata("IsTrimmable", "True")]` when building the assembly.

For more trimming options, see [Trimming options](/dotnet/core/deploying/trimming/trimming-options).

## Trimming defaults

By default, Android and Mac Catalyst builds use partial trimming when the build configuration is set to a release build. iOS uses partial trimming for any device builds, regardless of the build configuration, and doesn't use trimming for simulator builds.

## Trimming incompatibilities

The following .NET MAUI features are incompatible with full trimming and will be removed by the trimmer:

- Binding expressions where that binding path is set to a string. Instead, use compiled bindings. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
- Implicit conversion operators. In a .NET MAUI app this typically affects bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a <xref:System.ComponentModel.TypeConverter> for your type and attach it to the type using the <xref:System.ComponentModel.TypeConverterAttribute>. For more information, see [Define a TypeConverter to replace an implicit conversion operator](#define-a-typeconverter-to-replace-an-implicit-conversion-operator).
- Loading XAML at runtime with the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> extension method. This XAML can be made trim safe by annotating all types that could be loaded at runtime with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. However, this is very error prone and isn't recommended.
- Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute>. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).
- The `SearchHandler.DisplayMemberName` property. Instead, you should provide an <xref:Microsoft.Maui.Controls.ItemsView.ItemTemplate> to define the appearance of <xref:Microsoft.Maui.Controls.SearchHandler> results. For more information, see [Define search results item appearance](~/fundamentals/shell/search.md#define-search-results-item-appearance).

Alternatively, you can use feature switches so that the trimmer preserves the code for these features. For more information, see [Trimming feature switches](#trimming-feature-switches).

For .NET trimming incompatibilities, see [Known trimming incompatibilities](/dotnet/core/deploying/trimming/incompatibilities).

### Define a TypeConverter to replace an implicit conversion operator

Implicit conversion operators in a .NET MAUI app are incompatible with full trimming and will be removed by the trimmer. In a .NET MAUI app these typically affect bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. For more information about implicit conversion operators, see [User-defined explicit and implicit conversion operators](/dotnet/csharp/language-reference/operators/user-defined-conversion-operators).

For example, consider the following type that defines implicit conversion operators between `SizeRequest` and `Size`:

```csharp
namespace MyMauiApp;

public struct SizeRequest : IEquatable<SizeRequest>
{
    public Size Request { get; set; }
    public Size Minimum { get; set; }

    public SizeRequest(Size request, Size minimum)
    {
        Request = request;
        Minimum = minimum;
    }

    public SizeRequest(Size request)
    {
        Request = request;
        Minimum = request;
    }

    public override string ToString()
    {
        return string.Format("{{Request={0} Minimum={1}}}", Request, Minimum);
    }

    public bool Equals(SizeRequest other) => Request.Equals(other.Request) && Minimum.Equals(other.Minimum);

    public static implicit operator SizeRequest(Size size) => new SizeRequest(size);
    public static implicit operator Size(SizeRequest size) => size.Request;
    public override bool Equals(object? obj) => obj is SizeRequest other && Equals(other);
    public override int GetHashCode() => Request.GetHashCode() ^ Minimum.GetHashCode();
    public static bool operator ==(SizeRequest left, SizeRequest right) => left.Equals(right);
    public static bool operator !=(SizeRequest left, SizeRequest right) => !(left == right);
}
```

With full trimming enabled, the implicit conversion operators between `SizeRequest` and `Size` will be removed by the trimmer.

Instead, you should define a <xref:System.ComponentModel.TypeConverter> for your type and attach it to the type using the <xref:System.ComponentModel.TypeConverterAttribute>:

```csharp
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MyMauiApp;

[TypeConverter(typeof(SizeRequestTypeConverter))]
public struct SizeRequest : IEquatable<SizeRequest>
{
    public Size Request { get; set; }
    public Size Minimum { get; set; }

    public SizeRequest(Size request, Size minimum)
    {
        Request = request;
        Minimum = minimum;
    }

    public SizeRequest(Size request)
    {
        Request = request;
        Minimum = request;
    }

    public override string ToString()
    {
        return string.Format("{{Request={0} Minimum={1}}}", Request, Minimum);
    }

    public bool Equals(SizeRequest other) => Request.Equals(other.Request) && Minimum.Equals(other.Minimum);

    public static implicit operator SizeRequest(Size size) => new SizeRequest(size);
    public static implicit operator Size(SizeRequest size) => size.Request;
    public override bool Equals(object? obj) => obj is SizeRequest other && Equals(other);
    public override int GetHashCode() => Request.GetHashCode() ^ Minimum.GetHashCode();
    public static bool operator ==(SizeRequest left, SizeRequest right) => left.Equals(right);
    public static bool operator !=(SizeRequest left, SizeRequest right) => !(left == right);

    private sealed class SizeRequestTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(Size);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            => value switch
            {
                Size size => (SizeRequest)size,
                _ => throw new NotSupportedException()
            };

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
            => destinationType == typeof(Size);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is SizeRequest sizeRequest)
            {
                if (destinationType == typeof(Size))
                    return (Size)sizeRequest;
            }
            throw new NotSupportedException();
        }
    }
}
```

[!INCLUDE [Trimming feature switches](../includes/feature-switches.md)]

[!INCLUDE [Control the trimmer](../includes/linker-control.md)]

## See also

- [Trim self-contained deployments and executables](/dotnet/core/deploying/trimming/trim-self-contained)