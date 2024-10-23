---
ms.topic: include
ms.date: 10/23/2024
monikerRange: ">=net-maui-9.0"
---

## Trimming incompatibilities

The following .NET MAUI features are incompatible with full trimming and will be removed by the trimmer:

- Binding expressions where that binding path is set to a string. Instead, use compiled bindings. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
- Implicit conversion operators. In a .NET MAUI app this typically affects bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a <xref:System.ComponentModel.TypeConverter> for your type and attach it to the type using the <xref:System.ComponentModel.TypeConverterAttribute>. For more information, see [Define a TypeConverter to replace an implicit conversion operator](#define-a-typeconverter-to-replace-an-implicit-conversion-operator).
- Loading XAML at runtime with the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> extension method. This XAML can be made trim safe by annotating all types that could be loaded at runtime with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. However, this is very error prone and isn't recommended.
- Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute>. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).
- The `SearchHandler.DisplayMemberName` property. Instead, you should provide an <xref:Microsoft.Maui.Controls.ItemsView.ItemTemplate> to define the appearance of <xref:Microsoft.Maui.Controls.SearchHandler> results. For more information, see [Define search results item appearance](~/fundamentals/shell/search.md#define-search-results-item-appearance).
