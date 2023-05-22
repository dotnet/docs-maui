---
title: "Linking a .NET MAUI Android app"
description: "Learn about the .NET Android linker, which is used to eliminate unused code from a .NET MAUI Android app in order to reduce its size."
ms.date: 04/22/2023
no-loc: [ ILLink ]
---

# Linking a .NET MAUI Android app

When it builds your app, .NET Multi-platform App UI (.NET MAUI) can use a linker called *ILLink* to reduce the overall size of the app. ILLink reduces the size by analyzing the intermediate code produced by the compiler. It removes unused methods, properties, fields, events, structs, and classes to produce an app that contains only code and assembly dependencies that are necessary to run the app.

## Linker behavior

The linker enables you to trim your .NET MAUI Android apps. When trimming is enabled, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.

Linker behavior can be configured for each build configuration of your app. By default, trimming is disabled for debug builds and enabled for release builds.

> [!WARNING]
> Enabling the linker for your app's debug configuration may hinder your debugging experience, as it may remove property accessors that enable you to inspect the state of your objects.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that trimming is enabled for the release build configuration:

    :::image type="content" source="media/linking/vs.png" alt-text="Screenshot of the linker behavior for Android in Visual Studio.":::

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

1. In the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > Android > Linker** tab.
1. In the **Project Properties** window, ensure the **Configuration** drop-down is set to **Release** and set the **Linker Behavior** drop-down to your desired linker behavior:

    - *Don't link*. Disabling linking ensures assemblies aren't modified.
    - *Link SDK assemblies only*. In this mode, the linker leaves your assemblies untouched and reduces the size of the SDK assemblies by removing types and members that your app doesn't use.
    - *Link all assemblies*. When it links all assemblies, the linker performs additional optimizations to make your app as small as possible. It modifies the intermediate code for your source code, which may break your app if you use features using an approach that the linker's static analysis can't detect. In these cases, you may need to make adjustments to your source code to make your app work correctly.

    :::image type="content" source="media/linking/vsmac.png" alt-text="Screenshot of the linker behavior for Android in Visual Studio for Mac.":::

    > [!WARNING]
    > Linking all assemblies isn't recommended, and can require significant additional work to ensure that your app still works.

1. In the **Project Properties** window, click the **OK** button.

> [!NOTE]
> Visual Studio for Mac sets the `$(AndroidLinkMode)` MSBuild property in your app's project file. `<AndroidLinkMode>None</AndroidLinkMode>` maps to `<PublishTrimmed>false</PublishTrimmed>` and `<AndroidLinkMode>SdkOnly</AndroidLinkMode>` maps to `<PublishTrimmed>true</PublishTrimmed>`.

----

## Preserve code

When you use the linker, it sometimes remove code that you might have called dynamically, even indirectly. You can instruct the linker to preserve members by annotating them with the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. This attribute can be used to express a dependency on either a type and subset of members, or at specific members.

> [!IMPORTANT]
> Every member in the BCL that cannot be statically determined to be used by the app is subject to be removed.

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

When it links all assemblies, you can tell the linker to skip an assembly by setting the `TrimmerRootAssembly` MSBuild property in an `<ItemGroup>` tag in the project file:

```xml
<ItemGroup>
  <TrimmerRootAssembly Include="MyAssembly" />
</ItemGroup>
```

> [!NOTE]
> The `.dll` extension isn't required when setting the `TrimmerRootAssembly` MSBuild property.

If the linker skips an assembly, it's considered *rooted*, which means that it and all of its statically understood dependencies will be kept. You can skip additional assemblies by adding more `TrimmerRootAssembly` MSBuild properties to the `<ItemGroup>`.

## Preserve assemblies, types, and members

You can pass the linker an XML description file that specifies which assemblies, types, and members need to be retained.

To exclude a member from the linking process when linking all assemblies, set the `TrimmerRootDescriptor` MSBuild property in an `<ItemGroup>` tag in the project file to the XML file that defines the members to exclude:

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

When an assembly, type, or member is listed in the XML, the default action is preservation, which means that regardless of whether the linker thinks it's used or not, it's preserved in the output.

> [!NOTE]
> The preservation tags are ambiguously inclusive. If you don’t provide the next level of detail, it will include all the children. If an assembly is listed without any types, then all the assembly’s types and members will be preserved.

## Mark an assembly as linker safe

If you have a library in your project, or you're a developer of a reusable library and you want the linker to treat your assembly as linkable, you can mark the assembly as linker safe with the [`AssemblyMetadata`](xref:System.Reflection.AssemblyMetadataAttribute) attribute:

```csharp
[assembly: AssemblyMetadata("IsTrimmable", "True")]
```

Alternatively, you can set `<IsTrimmable>true</IsTrimmable>` in a `<PropertyGroup>` tag in the project file for the assembly. This marks your assembly as "trimmable" and enable trim warnings for that project. Being "trimmable" means your assembly is considered compatible with trimming and should have no trim warnings when the assembly is built. When used in a trimmed app, the assembly's unused members will be removed in the final output.

> [!NOTE]
> If the `IsTrimmable` MSBuild property is set for an assembly, this overrides the `IsTrimmable` attribute. This enables you to opt an assembly into trimming even if it doesn't have the attribute, or to disable trimming of an assembly that has the attribute.

## See also

- [ILLink](https://github.com/dotnet/linker/tree/main/docs)
