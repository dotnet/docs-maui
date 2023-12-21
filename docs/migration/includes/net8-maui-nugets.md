---
ms.topic: include
ms.date: 12/21/2023
---

In .NET 8, .NET MAUI ships as a .NET workload and multiple NuGet packages. The advantage of this approach is that it enables you to easily pin your projects to specific versions, while also enabling you to easily preview unreleased or experimental builds.

You should add the following explicit package references to an `<ItemGroup>` in your project file:

```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="$(MauiVersion)" />
<PackageReference Include="Microsoft.Maui.Controls.Compatibility" Version="$(MauiVersion)" />
```

The `$(MauiVersion)` variable is referenced from the version of .NET MAUI you've installed. You can override this by adding the `$(MauiVersion)` build property to your project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <UseMaui>True</UseMaui>
        <MauiVersion>8.0.3</MauiVersion>
    </PropertyGroup>
</Project>
```
