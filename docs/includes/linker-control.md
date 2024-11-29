---
ms.topic: include
ms.date: 05/23/2023
---

## Preserve code

When you use the trimmer, it sometimes removes code that you might have called dynamically, even indirectly. You can instruct the trimmer to preserve members by annotating them with the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. This attribute can be used to express a dependency on either a type and subset of members, or at specific members.

> [!IMPORTANT]
> Every member in the BCL that can't be statically determined to be used by the app is subject to be removed.

The [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute can be applied to constructors, fields, and methods:

```csharp
[DynamicDependency("Helper", "MyType", "MyAssembly")]
static void RunHelper()
{
    var helper = Assembly.Load("MyAssembly").GetType("MyType").GetMethod("Helper");
    helper.Invoke(null, null);
}
```

In this example, the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis) ensures that the `Helper` method is kept. Without the attribute, trimming would remove `Helper` from `MyAssembly` or remove `MyAssembly` completely if it's not referenced elsewhere.

The attribute specifies the member to keep via a `string` or via the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute. The type and assembly are either implicit in the attribute context, or explicitly specified in the attribute (by `Type`, or by `string`s for the type and assembly name).

The type and member strings use a variation of the C# documentation comment ID string [format](/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format), without the member prefix. The member string shouldn't include the name of the declaring type, and may omit parameters to keep all members of the specified name. The following examples show valid uses:

```csharp
[DynamicDependency("Method()")]
[DynamicDependency("Method(System,Boolean,System.String)")]
[DynamicDependency("MethodOnDifferentType()", typeof(ContainingType))]
[DynamicDependency("MemberName")]
[DynamicDependency("MemberOnUnreferencedAssembly", "ContainingType", "UnreferencedAssembly")]
[DynamicDependency("MemberName", "Namespace.ContainingType.NestedType", "Assembly")]
// generics
[DynamicDependency("GenericMethodName``1")]
[DynamicDependency("GenericMethod``2(``0,``1)")]
[DynamicDependency("MethodWithGenericParameterTypes(System.Collections.Generic.List{System.String})")]
[DynamicDependency("MethodOnGenericType(`0)", "GenericType`1", "UnreferencedAssembly")]
[DynamicDependency("MethodOnGenericType(`0)", typeof(GenericType<>))]
```

## Preserve assemblies

It's possible to specify assemblies that should be excluded from the trimming process, while allowing other assemblies to be trimmed. This approach can be useful when you can't easily use the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute, or don't control the code that's being trimmed.

When it trims all assemblies, you can tell the trimmer to skip an assembly by setting an `TrimmerRootAssembly` MSBuild item in the project file:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="MyAssembly" />
</ItemGroup>
```

> [!NOTE]
> The `.dll` extension isn't required when setting the `TrimmerRootAssembly` MSBuild property.

If the trimmer skips an assembly, it's considered *rooted*, which means that it and all of its statically understood dependencies are kept. You can skip additional assemblies by adding more `TrimmerRootAssembly` MSBuild properties to the `<ItemGroup>`.

## Preserve assemblies, types, and members

You can pass the trimmer an XML description file that specifies which assemblies, types, and members need to be retained.

To exclude a member from the trimming process when trimming all assemblies, set the `TrimmerRootDescriptor` MSBuild item in the project file to the XML file that defines the members to exclude:

```xml
<ItemGroup>
  <TrimmerRootDescriptor Include="MyRoots.xml" />
</ItemGroup>
```

The XML file then uses the trimmer [descriptor format](https://github.com/dotnet/runtime/blob/main/docs/tools/illink/data-formats.md#descriptor-format) to define which members to exclude:

```xml
<linker>
  <assembly fullname="MyAssembly">
    <type fullname="MyAssembly.MyClass">
      <method name="DynamicallyAccessedMethod" />
    </type>
  </assembly>
</linker>
```

In this example, the XML file specifies a method that's dynamically accessed by the app, which is excluded from trimming.

When an assembly, type, or member is listed in the XML, the default action is preservation, which means that regardless of whether the trimmer thinks it's used or not, it's preserved in the output.

> [!NOTE]
> The preservation tags are ambiguously inclusive. If you don’t provide the next level of detail, it will include all the children. If an assembly is listed without any types, then all the assembly’s types and members will be preserved.

## Mark an assembly as trim safe

If you have a library in your project, or you're a developer of a reusable library and you want the trimmer to treat your assembly as trimmable, you can mark the assembly as trim safe by adding the `IsTrimmable` MSBuild property to the project file for the assembly:

```xml
<PropertyGroup>
    <IsTrimmable>true</IsTrimmable>
</PropertyGroup>
```

This marks your assembly as "trimmable" and enables trim warnings for that project. Being "trimmable" means your assembly is considered compatible with trimming and should have no trim warnings when the assembly is built. When used in a trimmed app, the assembly's unused members are removed in the final output.

::: moniker range=">=net-maui-9.0"

When using Native AOT deployment, setting the `IsAotCompatible` MSBuild property to `true` also assigns a value of `true` to the `IsTrimmable` property, and enables additional AOT analyzer build properties. For more information about AOT analyzers, see [AOT-compatibility analyzers](/dotnet/core/deploying/native-aot#aot-compatibility-analyzers). For more information about Native AOT deployment for .NET MAUI, see [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

Setting the `IsTrimmable` MSBuild property to `true` in your project file inserts the [`AssemblyMetadata`](xref:System.Reflection.AssemblyMetadataAttribute) attribute into your assembly:

```csharp
[assembly: AssemblyMetadata("IsTrimmable", "True")]
```

Alternatively, you can add the [`AssemblyMetadata`](xref:System.Reflection.AssemblyMetadataAttribute) attribute into your assembly without having added the `IsTrimmable` MSBuild property to the project file for your assembly.

> [!NOTE]
> If the `IsTrimmable` MSBuild property is set for an assembly, this overrides the `AssemblyMetadata("IsTrimmable", "True")` attribute. This enables you to opt an assembly into trimming even if it doesn't have the attribute, or to disable trimming of an assembly that has the attribute.

### Suppress analysis warnings

When the trimmer is enabled, it removes IL that's not statically reachable. Apps that use reflection or other patterns that create dynamic dependencies may be broken as a result. To warn about such patterns, when marking an assembly as trim safe, library authors should set the `SuppressTrimAnalysisWarnings` MSBuild property to `false`:

```xml
<PropertyGroup>
  <SuppressTrimAnalysisWarnings>false</SuppressTrimAnalysisWarnings>
</PropertyGroup>
```

Not suppressing trim analysis warnings will include warnings about the entire app, including your own code, library code, and SDK code.

### Show detailed warnings

Trim analysis produces at most one warning for each assembly that comes from a `PackageReference`, indicating that the assembly's internals aren't compatible with trimming. As a library author, when you mark an assembly as trim safe, you should enable individual warnings for all assemblies by setting the `TrimmerSingleWarn` MSBuild property to `false`:

```xml
<PropertyGroup>
  <TrimmerSingleWarn>false</TrimmerSingleWarn>
</PropertyGroup>
```

This setting shows all detailed warnings, instead of collapsing them to a single warning per assembly.
