---
title: "XAML Security"
description: "Explore risks and best practices for securely loading and handling dynamic XAML in .NET MAUI"
ms.date: 07/01/2025
---

# XAML Security Considerations in .NET MAUI

This article outlines best practices for securing .NET MAUI applications that use XAML, with a focus on dynamic loading scenarios using APIs like `LoadFromXaml<T>()`.

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> Loading XAML at runtime isn't trim safe and shouldn't be used with full trimming or NativeAOT. It can be made trim safe by annotating all types that could be loaded at runtime with the [`DynamicallyAccessedMembers`](xref:System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute) attribute or the [`DynamicDependency`](xref:System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute) attribute. However, this is very error prone and isn't recommended. In addition, loading XAML at runtime has a significant performance cost. For more information, see [Trim a .NET MAUI app](~/deployment/trimming.md) and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

## Untrusted XAML in .NET MAUI Applications

The nature of XAML capabilities gives the XAML the right to construct objects and set their properties. These capabilities also include accessing type converters, mapping and accessing assemblies in the application domain, using markup extensions, and so on.

In addition to its language-level capabilities, XAML is used for UI definition in many technologies. Loading untrusted XAML might mean loading a malicious spoofing UI.

In .NET MAUI, *untrusted XAML* refers to any markup that wasn't explicitly authored, embedded, or compiled into the app. Examples include:

- XAML loaded from external files, streams, web services, or user input.
- Dynamically generated or modified XAML at runtime.

In contrast, XAML that’s compiled into your app or embedded in signed resource files is considered trusted—safe to the extent your application binary is trusted.

### Risks with `LoadFromXaml<T>()`

While [`LoadFromXaml<T>()`](/dotnet/maui/xaml/runtime-load) is useful for dynamic view loading, it comes with potential threats:

- Unexpected object instantiation or behavior via markup.
- Assigning properties or triggering side effects.
- **Gadget vulnerabilities:**  
  In security research, a *gadget* is an existing framework type that can be misused during deserialization to perform harmful actions (e.g., launching a process, reading files). Attackers can chain gadgets together for more serious exploits.  
  - On **Windows Desktop**, more gadgets are available because trimming is not enabled by default.  
  - On **Mobile and MacCatalyst**, the linker trims unused APIs, reducing but not eliminating gadget risks.

> [!NOTE]
> In general, use of this API is recommended against.

## Best Practices

- **Avoid runtime XAML loading** when possible. Prefer compile-time definitions.
- If dynamic loading is required:
  - **Validate all input**, sanitize markup sources, and limit to known-safe schemas.
  - **Restrict exposed types** and avoid loading arbitrary assemblies.
- Avoid or secure:
  - Custom markup extensions.
  - Bindings or converters that access sensitive resources.
- Place dynamic views inside sandboxed layouts or navigation flows to minimize impact.

## XAML Namespaces & Assembly Mappings in MAUI

MAUI uses XML namespaces in XAML to resolve CLR types. However, trust boundaries between assemblies are not enforced at the markup level.

### Potential Exploits

An untrusted assembly could:

- Spoof a namespace mapping.
- Hijack XAML-parsed type references.
  
### Mitigations

- Use *fully qualified namespaces* with strong assembly names.
- Avoid referencing or executing XAML from untrusted sources.

## Summary

Dynamic XAML introduces flexibility and power—but also risk. Treat *loose XAML* as you would any untrusted code, and apply rigorous validation, binding safeguards, and source control when using APIs like `LoadFromXaml<T>()`.
