---
ms.topic: include
ms.date: 08/30/2023
---

### AssemblyInfo changes

Properties that are typically set in an *AssemblyInfo.cs* file are now available in your SDK-style project. We recommend migrating them from *AssemblyInfo.cs* to your project file in every project, and removing the *AssemblyInfo.cs* file.

<!-- Todo: Feels like more is required here -->

Optionally, you can keep the *AssemblyInfo.cs* file and set the `GenerateAssemblyInfo` property in your project file to `false`:

```xml
<PropertyGroup>
  <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
</PropertyGroup>
```

For more information about the `GenerateAssemblyInfo` property, see [GenerateAssemblyInfo](/dotnet/core/project-sdk/msbuild-props#generateassemblyinfo).
