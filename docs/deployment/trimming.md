---
title: "Trim a .NET MAUI app"
description: "Learn about the .NET trimmer, which eliminates unused code from a .NET MAUI app to reduce its size."
ms.date: 12/20/2024
no-loc: [ ILLink ]
monikerRange: ">=net-maui-9.0"
---

# Trim a .NET MAUI app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app with a technique known as trimming. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

To prevent changes in behavior when trimming apps, .NET provides static analysis of trim compatibility through trim warnings. The trimmer produces trim warnings when it finds code that might not be compatible with trimming. If there are any trim warnings they should be fixed and the app should be thoroughly tested after trimming to ensure that there are no behavior changes. For more information, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings).

## Trimming behavior

Trimming behavior can be controlled by setting the `$(TrimMode)` build property to either `partial` or `full`:

```xml
<PropertyGroup>
  <TrimMode>full</TrimMode>
</PropertyGroup>
```

> [!IMPORTANT]
> The `$(TrimMode)` build property shouldn't be conditioned by build configuration. This is because features switches are enabled or disabled based on the value of the `$(TrimMode)` build property, and the same features should be enabled or disabled in all build configurations so that your code behaves identically.

The `full` trim mode removes any code that's not used by your app. The `partial` trim mode trims the base class library (BCL), assemblies for the underlying platforms (such as *Mono.Android.dll* and *Microsoft.iOS.dll*), and any other assemblies that have opted into trimming with the `$(TrimmableAsssembly)` build item:

```xml
<ItemGroup>
  <TrimmableAssembly Include="MyAssembly" />
</ItemGroup>
```

This is equivalent to setting `[AssemblyMetadata("IsTrimmable", "True")]` when building the assembly.

In a .NET MAUI app, it's not necessary to set the `$(PublishTrimmed)` build property to `true` in your app's project file, because this is set by default.

> [!NOTE]
> Don't set the `$(TrimMode)` build property when using Native AOT deployment. Native AOT deployment automatically performs full trimming of your app. For more information, see [Native AOT deployment on iOS and Mac Catalyst](nativeaot.md).

For more trimming options, see [Trimming options](/dotnet/core/deploying/trimming/trimming-options).

## Trimming defaults

By default, Android and Mac Catalyst builds use partial trimming when the build configuration is set to a release build. iOS uses partial trimming for any device builds, regardless of the build configuration, and doesn't use trimming for simulator builds.

## Trimming incompatibilities

[!INCLUDE [Trimming incompatibilities](includes/trimming-incompatibilities.md)]

Alternatively, you can use feature switches so that the trimmer preserves the code for these features. For more information, see [Trimming feature switches](#trimming-feature-switches).

For .NET trimming incompatibilities, see [Known trimming incompatibilities](/dotnet/core/deploying/trimming/incompatibilities).

### Define a TypeConverter to replace an implicit conversion operator

It's not possible to rely on implicit conversion operators when assigning a value of an incompatible type to a property in XAML, or when two properties of different types use a data binding, when full trimming is enabled. This is because the implicit operator methods could be removed by the trimmer if they aren't used in your C# code. For more information about implicit conversion operators, see [User-defined explicit and implicit conversion operators](/dotnet/csharp/language-reference/operators/user-defined-conversion-operators).

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

With full trimming enabled, the implicit conversion operators between `SizeRequest` and `Size` could be removed by the trimmer if they aren't used in your C# code.

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

## Trimming feature switches

[!INCLUDE [Trimming feature switches](includes/feature-switches.md)]

[!INCLUDE [Control the trimmer](../includes/linker-control.md)]

## See also

- [Trim self-contained deployments and executables](/dotnet/core/deploying/trimming/trim-self-contained)
