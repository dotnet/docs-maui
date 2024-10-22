---
title: "Trim a .NET MAUI app"
description: "Learn about the .NET trimmer, which eliminates unused code from a .NET MAUI app to reduce its size."
ms.date: 10/22/2024
no-loc: [ ILLink ]
monikerRange: ">=net-maui-9.0"
---

# Trim a .NET MAUI app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app with a technique known as trimming. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Trimming behavior

To prevent changes in behavior when trimming apps, .NET provides static analysis of trim compatibility through trim warnings. The trimmer produces trim warnings when it finds code that might not be compatible with trimming. If there are any trim warnings, the app should be thoroughly tested after trimming to ensure that there are no behavior changes. For more information, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings).

Trimming behavior can be configured for your app with the `$(PublishTrimmed)` build property:

```xml
<PublishTrimmed>True</PublishTrimmed>
```

> [!WARNING]
> Enabling trimming for your app's debug configuration could hinder your debugging experience, because it could remove property accessors that enable you to inspect the state of your objects.

## Trimming granularity

Trimming granularity can be controlled by setting the `$(TrimMode)` build property to either `partial` or `full`:

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

## Trimming defaults per platform

By default, Android and Mac Catalyst builds use partial trimming when the build configuration is set to a release build. iOS uses partial trimming for any device builds, regardless of the build configuration, and doesn't use trimming for simulator builds.

## Trimming incompatibilities

The following .NET MAUI features are incompatible with full trimming and will be removed by the trimmer:

- Binding expressions where that binding path is set to a string. Instead, use compiled bindings. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
- Implicit cast operators. This typically affects bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a `TypeConverter` for your type and attach it to the type using the `[TypeConverter(typeof(MyTypeConverter))]` attribute.
- Loading XAML at runtime with the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> extension method. This XAML can be made trim safe by annotating all types that could be loaded at runtime with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. However, this is very error prone and isn't recommended.
- Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute>. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).
- The `SearchHandler.DisplayMemberName` property. Instead, you should provide an <xref:Microsoft.Maui.Controls.ItemsView.ItemTemplate> to define the appearance of <xref:Microsoft.Maui.Controls.SearchHandler> results. For more information, see [Define search results item appearance](~/fundamentals/shell/search.md#define-search-results-item-appearance).

Alternatively, you can use feature switches so that the trimmer preserves the code for these features. For more information, see [Trimming feature switches](#trimming-feature-switches).

For .NET trimming incompatibilities, see [Known trimming incompatibilities](/dotnet/core/deploying/trimming/incompatibilities).

[!INCLUDE [Trimming feature switches](../includes/feature-switches.md)]

[!INCLUDE [Control the trimmer](../includes/linker-control.md)]

## See also

- [Trim self-contained deployments and executables](/dotnet/core/deploying/trimming/trim-self-contained)
