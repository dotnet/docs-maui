---
title: "Linking a .NET MAUI iOS app"
description: "Learn about the .NET iOS linker, which is used to eliminate unused code from a .NET MAUI iOS app in order to reduce its size."
ms.date: 04/11/2023
no-loc: [ Objective-C, ILLink ]
---

# Linking a .NET MAUI iOS app

When building your app in release configuration, .NET Multi-platform App UI (.NET MAUI) uses a linker called *ILLink* to reduce the overall size of the app. ILLink does this by analysing the intermediate code produced by the compiler, removing unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Linker behavior

The linker supports three modes for .NET MAUI iOS apps:

- *Don't link*. Disabling linking will ensure assemblies aren't modified.
- *Link SDK assemblies only*. In this mode, the linker will leave your assemblies untouched, and will reduce the size of the SDK assemblies by removing types and members that your app doesn't use.
- *Link all assemblies*. When linking all assemblies, the linker will perform additional optimizations to make your app as small as possible. It will modify the intermediate code for your source code, which may break your app if you use features using an approach that the linker's static analysis can't detect. In these cases, adjustments to your source code may be required to make your app work correctly.

Linker behavior can be configured for each build configuration of your app.

> [!WARNING]
> Enabling the linker for debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **iOS > Build** tab and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vs.png" alt-text="Screenshot of the linker behavior for iOS in Visual Studio.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > iOS > Builg** tab.
1. In the **Project Properties** window, ensure the **Configuration** drop-down is set to **Release** and set the **Linker behavior** drop-down to your desired linker behavior:

    :::image type="content" source="media/linking/vsmac.png" alt-text="Screenshot of the linker behavior for iOS in Visual Studio for Mac.":::

1. In the **Project Properties** window, click the **OK** button.

----

## Preserve code

When you use the linker it will sometimes remove code that you might have called dynamically, even indirectly. To cover these cases the linker provides several features and options to allow you greater control over its behavior.

> [!IMPORTANT]
> Every member that isn't statically linked by the app is subject to be removed.

### RequiresUnreferencedCode attribute

The [`RequiresUnreferencedCode`](xref:System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute) attribute indicates that the annotated member has dependencies that aren't statically discoverable, and therefore shouldn't be removed by the linker. The attribute is typically used in scenarios where code uses reflection, dynamic code generation, or native interop, and therefore has dependencies that aren't visible to the compiler at build-time.

By adding the [`RequiresUnreferencedCode`](xref:System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute) attribute, you're indicating that the member shouldn't be removed by the linker, even if it appears to be unreferenced.

The [`RequiresUnreferencedCode`](xref:System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute) attribute can be applied to classes, constructors, and methods. For more information, see [RequiresUnreferencedCode](/dotnet/core/deploying/trimming/fixing-warnings#requiresunreferencedcode).

### DynamicallyAccessedMembers attribute

The [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute is used to indicate that the annotated member is dynamically accessed at runtime, and therefore shouldn't be removed by the linker. The attribute is typically used in scenarios where code uses reflection or other dynamic runtime mechanisms to access members of a type.

By adding the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute, you're indicating that the member shouldn't be removed by the linker, even if it appears to be unreferenced.

The [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute can be applied to classes, fields, generic parameters, interfaces, methods, parameters, properties, return values, and structs. For more information, see [DynamicallyAccessedMembers](/dotnet/core/deploying/trimming/fixing-warnings#dynamicallyaccessedmembers).

### DynamicDependency attribute

You can instruct the linker to preserve members by annotating them with the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis) attribute. This results in the referenced members being kept whenever the member with the attribute is kept.

> [!WARNING]
> Use the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis) attribute only as a last resort when the other approaches aren't viable. It's preferable to express the reflection behavior of your code using the [`RequiresUnreferencedCode`](xref:System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute) attribute or the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute.

The attribute can be applied to constructors, fields, and methods:

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

## Skip assemblies

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

## Skip members

It's possible to specify individual methods that should be excluded from the linking process, while allowing the rest of the assembly to be linked.

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
