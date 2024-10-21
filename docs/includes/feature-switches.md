---
ms.topic: include
ms.date: 10/21/2024
monikerRange: ">=net-maui-9.0"
---

## Trimming feature switches

.NET MAUI possesses trimmer directives, known as feature switches, that make it possible to preserve the code for features that aren't trim safe. These trimmer directives can be used when the `$(TrimMode)` build property is set to `full`, as well as for NativeAOT:

| MSBuild property | Description |
| ---------------- | ----------- |
| `MauiEnableVisualAssemblyScanning` | When set to `true`, .NET MAUI will scan assemblies for types implementing `IVisual` and for `[assembly:Visual(...)]` attributes, and will register these types. By default, this build property is set to `false`. |
| `MauiShellSearchResultsRendererDisplayMemberNameSupported` | When set to `false`, the value of `SearchHandler.DisplayMemberName` will be ignored. Instead, you should provide an `ItemTemplate` to define the appearance of `SearchHandler` results. By default, this build property is set to `true`.|
| `MauiQueryPropertyAttributeSupport` | When set to `false`, `[QueryProperty(...)]` attributes won't be used to set property values when navigating. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface to accept query parameters. By default, this build property is set to `true`. |
| `MauiImplicitCastOperatorsUsageViaReflectionSupport` | When set to `false`, .NET MAUI won't look for implicit cast operators when converting values from one type to another. This can affect bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a `TypeConverter` for your type and attach it to the type using the `[TypeConverter(typeof(MyTypeConverter))]` attribute. By default, this build property is set to `true`.|
| `_MauiBindingInterceptorsSupport` | When set to `false`, .NET MAUI won't intercept any calls to the `SetBinding` methods and won't try to compile them. By default, this build property is set to `true`. |

These MSBuild properties also have equivalent `AppContext` switches:

- The `MauiEnableVisualAssemblyScanning` MSBuild property has an equivalent `AppContext` switch named `Microsoft.Maui.RuntimeFeature.IsIVisualAssemblyScanningEnabled`.
- The `MauiShellSearchResultsRendererDisplayMemberNameSupported` MSBuild property has an equivalent `AppContext` switch named `Microsoft.Maui.RuntimeFeature.IsShellSearchResultsRendererDisplayMemberNameSupported`.
- The `MauiQueryPropertyAttributeSupport` MSBuild property has an equivalent `AppContext` switch named `Microsoft.Maui.RuntimeFeature.IsQueryPropertyAttributeSupported`.
- The `MauiImplicitCastOperatorsUsageViaReflectionSupport` MSBuild property has an equivalent `AppContext` switch named `Microsoft.Maui.RuntimeFeature.IsImplicitCastOperatorsUsageViaReflectionSupported`.
- The `_MauiBindingInterceptorsSupport` MSBuild property has an equivalent `AppContext` switch named `Microsoft.Maui.RuntimeFeature.AreBindingInterceptorsSupported`.

The easiest way to consume a feature switch is by putting the corresponding MSBuild property into your app's project file (*.csproj), which causes the related code to be trimmed from the .NET MAUI assemblies.
