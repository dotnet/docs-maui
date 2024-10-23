---
title: "Native AOT on iOS and Mac Catalyst"
description: "Learn how to reduce your app size and achieve faster startup time with NativeAOT on iOS and Mac Catalyst."
ms.date: 10/23/2024
monikerRange: ">=net-maui-9.0"
---

# Native AOT on iOS and Mac Catalyst

Native ahead-of-time (AOT) deployment produces a .NET Multi-platform App UI (.NET MAUI) app on iOS and Mac Catalyst that's been ahead-of-time compiled to native code. Publishing and deploying a Native AOT app produces the following benefits:

- Reduced app package size, typically up to 2.5x smaller.
- Faster startup time, typically up to 2x faster.
- Faster build time.

Native AOT performs static program analysis, full trimming of your app, which is aggressive in removing code that's not AOT compatible, and ahead-of-time code generation. This introduces limitations on usage of certain aspects of the .NET runtime, and any AOT incompatible constructs in your app might be removed at build time which can result in crashes at runtime. It's therefore important to test an app thoroughly when moving to the Native AOT deployment model. The AOT deployed app must be tested to verify that functionality hasn't changed from the untrimmed app. For more information about limitations, see [Limitations](#limitations).

Native AOT should only be used in scenarios where app size and performance are important. However, it will require you to adapt your apps to Native AOT requirements. This means ensuring they are fully trimming and AOT compatible. When Native AOT deployment is enabled, the build system analyzes your code, and all its dependencies, to verify if it's suitable for full trimming and AOT compilation. If incompatibilities are detected, trimming and AOT warnings are produced. A single trimming or AOT warning means that the app isn't compatible with Native AOT deployment, and that it might not work correctly. Therefore, when building an app for Native AOT deployment you should review and correct all trimming and AOT warnings. For more information, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings) and [Introduction to AOT warnings](/dotnet/core/deploying/native-aot/fixing-warnings).

> [!NOTE]
> There may be cases where fixing trimming and AOT warnings isn't possible, such as when they occur for third-party libraries. WHAT DO THEY DO NOW?

The following chart shows the resulting app package size for a `dotnet new maui` app on iOS and Mac Catalyst across different deployment models:

:::image type="content" source="media/nativeaot/app-package-size.png" alt-text="Chart showing app package size across different deployment models." border="false":::

The preceding chart shows that Native AOT produces more than 2x smaller apps for both iOS and Mac Catalyst compared to the default deployment model.

The following chart shows the average startup time for a `dotnet new maui` app on iOS and Mac Catalyst on Mono and Native AOT:

:::image type="content" source="media/nativeaot/average-startup-time.png" alt-text="Chart showing average app startup time on Mono and Native AOT." border="false":::

The preceding chart shows that Native AOT has up to 2x faster startup times on iOS devices and 1.2x faster startup time on Mac Catalyst, compared to the default deployment model.

The following chart shows the average build time for a `dotnet new maui` app on iOS and Mac Catalyst:

:::image type="content" source="media/nativeaot/average-build-time.png" alt-text="Chart showing average app build time on Mono and Native AOT." border="false":::

The preceding chart shows that Native AOT has up to 2.8x faster build times on iOS devices compared to the default deployment model. For Mac Catalyst, build times are comparable for arm64 single RID apps, but are slightly slower for universal apps when compared to the default deployment model.

> [!IMPORTANT]
> In many scenarios Native AOT will produce smaller and faster apps. However, in some scenarios Native AOT might not produce smaller and faster apps. Therefore, it's important to test and profile your app to determine the result of enabling Native AOT.

## Publish using Native AOT

The Native AOT deployment model is enabled with the `$(PublishAot)` MSBuild property, and the `dotnet publish` command. The following example shows how to modify a project file to enable Native AOT on iOS and Mac Catalyst:

```xml
<PropertyGroup>
  <!-- enable trimming and AOT analyzers on all platforms -->
  <IsAotCompatible>true</IsAotCompatible>

  <!-- select platforms to use with NativeAOT -->
  <PublishAot Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">true</PublishAot>
  <PublishAot Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">true</PublishAot>
</PropertyGroup>
```

Setting the `$(IsAotCompatible)` MSBuild property to `true`, for all platforms, will enable trimming and AOT analyzers. These analyzers will help you identify code that's not compatible with trimming or AOT.

Conditionally setting `$(PublishAot)` to `true`, for iOS and Mac Catalyst, enables Native AOT compilation during publish and enables dynamic code usage analysis during build. Native AOT analysis includes all of the app's code and the libraries the app depends on.

> [!WARNING]
> The `$(PublishAot)` build property shouldn't be conditioned by build configuration. This is because trimming features switches are enabled or disabled based on the value of the `$(PublishAot)` build property, and the same features should be enabled or disabled in all build configurations so that your code behaves identically.

The only way to verify that a Native AOT app works correctly is to publish it using `dotnet publish` and verify that there are no trimming or AOT warnings produced by your code and its dependencies. In particular, `dotnet build -t:Publish` isn't equivalent to `dotnet publish`.

Use the following `dotnet publish` command to publish your app on iOS and Mac Catalyst using Native AOT:

```dotnetcli
# for iOS
dotnet publish -f net9.0-ios -r ios-arm64

# for MacCatalyst
dotnet publish -f net9.0-maccatalyst -r maccatalyst-arm64
dotnet publish -f net9.0-maccatalyst -r maccatalyst-x64

# for universal MacCatalyst apps i.e. when <RuntimeIdentifiers>maccatalyst-x64;maccatalyst-arm64</RuntimeIdentifiers> is set in the project
dotnet publish -f net9.0-maccatalyst
```

> [!TIP]
> Publish apps frequently to discover trimming or AOT issues early in the development lifecycle.

## Limitations

Native AOT isn't suitable for all apps, and should only be used in scenarios where app size and performance are important. Making an app AOT compatible can require a lot of work. In addition to the [.NET limitations of Native AOT deployment](/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8#limitations-of-native-aot-deployment), Native AOT deployment for .NET MAUI has additional limitations.

> [!IMPORTANT]
> The Mono interpreter isn't compatible with Native AOT deployment, and therefore the `$(UseInterpreter)` and `$(MtouchInterpreter)` MSBuild properties have no effect when using Native AOT. For more information about the Mono interpreter, see [Mono interpreter on iOS and Mac Catalyst](~/macios/interpreter.md).

Third-party libraries your apps depend on might not be AOT compatible. The only way to ensure that a library is trimming and AOT compatible is to publish your app using Native AOT deployment and the `dotnet publish` command, and see if the Native AOT compiler produces any warnings. For information about making your own libraries AOT compatible, see [How to make libraries compatible with native AOT](https://devblogs.microsoft.com/dotnet/creating-aot-compatible-libraries/).

### Reflection and dynamic code

Native AOT limits the use of reflection in your code and its dependencies, and it can become necessary to use annotations to help the Native AOT compiler understand reflection patterns. When the compiler encounters a reflection pattern it can't statically analyze, and hence can't build the app, it produces trim warnings. Native AOT also prevents you from using dynamic code in your app. For example, compiling <xref:System.Linq.Expressions> won't work as expected, and it isn't possible to load and execute assemblies at runtime. When the compiler encounters a dynamic pattern it can't ahead-of-time compile, it will produce an AOT warning.

In .NET MAUI app this means that:

- All XAML needs to be ahead-of-time compiled. Therefore, ensure that you haven't disabled XAML compilation, and that all bindings are compiled. For more information, see [XAML compilation](~/xaml/xamlc.md) and [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
- All binding expressions must use compiled bindings, rather than a binding path that's set to a string. For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
- Implicit conversion operators might not be called. In a .NET MAUI app this typically affects bindings between properties with different types, and setting a property value of a bindable object with a value of a different type. Instead, you should define a <xref:System.ComponentModel.TypeConverter> for your type and attach it to the type using the <xref:System.ComponentModel.TypeConverterAttribute>. For more information, see [Define a TypeConverter to replace an implicit conversion operator](trimming.md#define-a-typeconverter-to-replace-an-implicit-conversion-operator)
- It's not possible to parse XAML at runtime with the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method. While this can be made trim safe by annotating all types that could be loaded at runtime with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute this is very error prone and isn't recommended.
- Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> won't work. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).
- The `SearchHandler.DisplayMemberName` property might not work. Instead, you should provide an <xref:Microsoft.Maui.Controls.ItemsView.ItemTemplate> to define the appearance of <xref:Microsoft.Maui.Controls.SearchHandler> results. For more information, see [Define search results item appearance](~/fundamentals/shell/search.md#define-search-results-item-appearance).

For more information about trim warnings, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings). For more information about AOT warnings, see [Introduction to AOT warnings](/dotnet/core/deploying/native-aot/fixing-warnings).

## Adapt an app to Native AOT deployment

Use the following checklist to help you adapt your app to Native AOT deployment requirements:

> [!div class="checklist"]
>
> - Ensure that all XAML is compiled:
>   - Remove all `[XamlCompilation(XamlCompilationOptions.Skip)]` usage.
>   - Remove all `<?xaml-comp compile="false" ?>` usage.
> - Remove all calls to the <xref:Microsoft.Maui.Controls.Xaml.Extensions.LoadFromXaml%2A> method.
> - Ensure that all data bindings are compiled:
>   - Ensure that all XAML data bindings are annotated with `x:DataType`.
>   - Ensure that all code data bindings replace all string-based bindings with lambda-based bindings.
> For more information, see [Compiled bindings](~/fundamentals/data-binding/compiled-bindings.md).
> - Replace all `[QueryProperty(...)]` usage with an implementation of the `IQueryAttributable` interface. For more information, see [Process navigation data using a single method](~/fundamentals/shell/navigation.md#process-navigation-data-using-a-single-method).
> - Replace all `SearchHandler.DisplayMemberName` usage with an <xref:Microsoft.Maui.Controls.ItemsView.ItemTemplate>. For more information, see [Define search results item appearance](~/fundamentals/shell/search.md#define-search-results-item-appearance).
> - Replace all implicit conversion operators for types used in XAML with a <xref:System.ComponentModel.TypeConverter>, and it attach it to your type using the <xref:System.ComponentModel.TypeConverterAttribute>:
>   - When converting from type `A` to type `B`, either the `ConvertTo` method on a type converter associated with `A` will be used or the `ConvertFrom` method on a type converter associated with `B` will be used.
>   - When both source and target types have an associated type converter, either of them can be used.
> For more information, see [Define a TypeConverter to replace an implicit conversion operator](trimming.md#define-a-typeconverter-to-replace-an-implicit-conversion-operator)
> - Compile all regular expressions using source generators. For more information, see [.NET regular expression source generators](/dotnet/standard/base-types/regular-expression-source-generators).
> - Ensure that JSON serialization and deserialization uses a source generated context. For more information, see [Minimal APIs and JSON payloads](/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0#minimal-apis-and-json-payloads).
> - Review and correct any trimming or AOT warnings. For more information, see [Introduction to trim warnings](/dotnet/core/deploying/trimming/fixing-warnings) and [Introduction to AOT warnings](/dotnet/core/deploying/native-aot/fixing-warnings).
