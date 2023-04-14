---
ms.topic: include
ms.date: 04/11/2023
---

## Preserve code

When you use the linker it will sometimes remove code that you might have called dynamically, even indirectly. You can instruct the linker to preserve members by annotating them with the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. This attribute can be used to express a dependency on either a type and subset of members, or at specific members.

> [!IMPORTANT]
> Every member that isn't statically linked by the app is subject to be removed.

The [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute can be applied to constructors, fields, and methods:

```csharp
[DynamicDependency("Helper", "MyType", "MyAssembly")]
static void RunHelper()
{
    var helper = Assembly.Load("MyAssembly").GetType("MyType").GetMethod("Helper");
    helper.Invoke(null, null);
}
```

In this example, the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis) ensures that the `Helper` method is kept. Without the attribute, linking would remove `Helper` from `MyAssembly` or remove `MyAssembly` completely if it's not referenced elsewhere.

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

It's possible to specify assemblies that should be excluded from the linking process, while allowing other assemblies to be linked.

When linking all assemblies, the linker can skip an assembly by setting the `TrimmerRootAssembly` MSBuild property in an `<ItemGroup>` tag in the project file:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="MyAssembly" />
</ItemGroup>
```

> [!NOTE]
> The `.dll` extension isn't required when setting the `TrimmerRootAssembly` MSBuild property.

If the linker skips an assembly, it's considered "rooted" which means that it and all of its statically understood dependencies will be kept. Additional assemblies may be skipped by adding additional `TrimmerRootAssembly` MSBuild properties to the `<ItemGroup>`.

## Preserve assemblies, types, and members

The linker can be passed an XML description file that specifies which assemblies, types, and members need to be retained.

When linking all assemblies, to exclude a member from the linking process set the `TrimmerRootDescriptor` MSBuild property in an `<ItemGroup>` tag in the project file to the XML file that defines the members to exclude:

```xml
<ItemGroup>
  <TrimmerRootDescriptor Include="MyRoots.xml" />
</ItemGroup>
```

The XML file then uses the trimmer [descriptor format](https://github.com/dotnet/linker/blob/main/docs/data-formats.md#descriptor-format) to define which members to exclude from linking:

```xml
<linker>
  <assembly fullname="MyAssembly">
    <type fullname="MyAssembly.MyClass">
      <method name="DynamicallyAccessedMethod" />
    </type>
  </assembly>
</linker>
```

In this example, the XML file specifies a method that's dynamically accessed by the app, which is excluded from linking.

When an assembly, type or member are listed in the XML, the default action is preservation, which means that regardless of whether the linker thinks they are used or not, they will be preserved in the output.
> [!NOTE]
> The preservation tags are ambiguously inclusive. If you don’t provide the next level of detail, it will include all the children. If an assembly is listed without any types, then all the assembly’s types and members will be preserved.

## Mark an assembly as linker safe

If you have a library in your project, or you are a developer of a re-usable library and you want the linker to treat your assembly as linkable, you can mark the assembly as linker safe with the [`AssemblyMetadata`](xref:System.Reflection.AssemblyMetadataAttribute) attribute:

```csharp
[assembly: AssemblyMetadata("IsTrimmable", "True")]
```

Alternatively, you can set `<IsTrimmable>true</IsTrimmable>` in a `<PropertyGroup>` tag in the project file for the assembly. This will mark your assembly as "trimmable" and enable trim warnings for that project. Being "trimmable" means your assembly is considered compatible with trimming and should have no trim warnings when building the assembly. When used in a trimmed app, the assembly will have its unused members removed in the final output.

> [!NOTE]
> If the `IsTrimmable` MSBuild property is set for an assembly, this overrides the `IsTrimmable` attribute. This enables you to opt an assembly into trimming even if it doesn't have the attribute, or to disable trimming of an assembly that has the attribute.

## Disable linking

To disable linking, set `<MtouchLink>None</MtouchLink>` in a `<PropertyGroup>` tag in the project file, or set the linker behavior to **Don't link** in Visual Studio.

> [!WARNING]
> Setting `<PublishTrimmed>false</PublishTrimmed>` in a `<PropertyGroup>` tag in the project file for the assembly doesn't disable linking for .NET MAUI iOS apps.

## See also

- [ILLink](https://github.com/dotnet/linker/tree/main/docs)
