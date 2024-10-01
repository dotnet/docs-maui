---
ms.topic: include
ms.date: 09/26/2024
monikerRange: ">=net-maui-9.0"
---

# Trim MAUI features

Several areas of .NET MAUI come with trimmer directives, known as feature switches, that make it possible to remove the code for disabled features when `TrimMode=full`, as well as for NativeAOT:

| MSBuild property | AppContext switch | Description |
| ---------------- | ----------------- | ----------- |
| `MauiEnableVisualAssemblyScanning` | `Microsoft.Maui.RuntimeFeature.IsIVisualAssemblyScanningEnabled` | When set to `true`, .NET MAUI will scan assemblies for types implementing `IVisual` and for `[assembly:Visual(...)]` attributes, and will register these types. By default, this build property is set to `false`. |
| `MauiShellSearchResultsRendererDisplayMemberNameSupported` | `Microsoft.Maui.RuntimeFeature.IsShellSearchResultsRendererDisplayMemberNameSupported` | When set to `false`, the value of `SearchHandler.DisplayMemberName` will be ignored. Instead, you should provide an `ItemTemplate` to define the appearance of `SearchHandler` results. By default, this build property is set to `true`.|
| `MauiQueryPropertyAttributeSupport` | `Microsoft.Maui.RuntimeFeature.IsQueryPropertyAttributeSupported` | When set to `false`, `[QueryProperty(...)]` attributes won't be used to set property values when navigating. Instead, you should implement the `IQueryAttributable` interface to accept query parameters. By default, this build property is set to `true`. |
| `MauiImplicitCastOperatorsUsageViaReflectionSupport` | `Microsoft.Maui.RuntimeFeature.IsImplicitCastOperatorsUsageViaReflectionSupported` | When set to `false`, .NET MAUI won't look for implicit cast operators when converting values from one type to another. This can affect bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a `TypeConverter` for your type and attach it to the type using the `[TypeConverter(typeof(MyTypeConverter))]` attribute. By default, this build property is set to `true`.|
| `_MauiBindingInterceptorsSupport` | `Microsoft.Maui.RuntimeFeature.AreBindingInterceptorsSupported` | When set to `false`, .NET MAUI won't intercept any calls to the `SetBinding` methods and won't try to compile them. By default, this build property is set to `true`. |

The easiest way to consume a feature switch is by putting the corresponding MSBuild property into your app's project file (*.csproj), which causes the related code to be trimmed from the .NET MAUI assemblies. Disabling features an app doesn't require can help reduce the app size when combined with the `Full` trimming mode.
