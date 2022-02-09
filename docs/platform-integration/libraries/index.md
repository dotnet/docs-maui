---
title: "Libraries"
description: ".NET MAUI supports NuGet packages, platform channeling, and .NET binding libraries."
ms.date: 02/08/2022
---

# Libraries

In addition to using your own .NET class libraries in .NET MAUI projects, you have a few other methods to include .NET libraries as well as platform libraries written in non-.NET languages.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

|   | Complexity | Notes |
| :---- | :---- | :---- |
| NuGet Packages | Easy | These are ready-to-use .NET libraries |
| Platform Channels | Easy-Moderate | Requires a little platform language knowledge in addition to .NET |
| Minimal Bindings | Moderate-Difficult | Requires platform language knowledge in addition to .NET |
| Full Bindings | Difficult | Requires deep platform language knowledge in addition to .NET |

## NuGet Packages

NuGet libraries are the easiest to consume in your .NET MAUI projects. Browse [NuGet.org](https://www.nuget.org) for anything that is .NET 6 and .NET MAUI compatible. Visual Studio 2022 provides a built-in NuGet Package Manager that can add, remove, and update the packages across your solution's projects. You may also directly edit your ".csproj" file using the PackageReference syntax provided on [NuGet.org](https://www.nuget.org).

> [!NOTE] Should I use Xamarin packages?
> Most Xamarin packages will not be .NET 6 compatible, so check for that first. We recommend only using packages that state .NET MAUI and/or .NET 6 support.

For more information about package compatibility and updating libraries for .NET MAUI, see [NuGet packages](nugets.md).

## Platform Channels

NuGet libraries are the easiest to consume in your .NET MAUI projects. Browse [NuGet.org](https://www.nuget.org) for anything that is .NET 6 and .NET MAUI compatible. Visual Studio 2022 provides a built-in NuGet Package Manager that can add, remove, and update the packages across your solution's projects. You may also directly edit your ".csproj" file using the PackageReference syntax provided on [NuGet.org](https://www.nuget.org).

For more information about this and updating libraries for .NET MAUI, see [NuGet packages](nugets.md).



XAML allows developers to define user interfaces in .NET Multi-platform App UI (.NET MAUI) apps using markup rather than code. XAML is not required in a .NET MAUI app, but it is the recommended approach to developing your UI because it's often more succinct, more visually coherent, and has tooling support. XAML is also well suited for use with the Model-View-ViewModel (MVVM) pattern, where XAML defines the view that is linked to viewmodel code through XAML-based data bindings.

Within a XAML file, you can define user interfaces using all the .NET MAUI views, layouts, and pages, as well as custom classes. The XAML file can be either compiled or embedded in the app package. Either way, the XAML is parsed at build time to locate named objects, and at runtime the objects represented by the XAML are instantiated and initialized.

XAML has several advantages over equivalent code:

- XAML is often more succinct and readable than equivalent code.
- The parent-child hierarchy inherent in XML allows XAML to mimic with greater visual clarity the parent-child hierarchy of user-interface objects.

There are also disadvantages, mostly related to limitations that are intrinsic to markup languages:

- XAML cannot contain code. All event handlers must be defined in a code file.
- XAML cannot contain loops for repetitive processing.
- XAML cannot contain conditional processing. However, a data-binding can reference a code-based binding converter that effectively allows some conditional processing.
- XAML generally cannot instantiate classes that do not define a parameterless constructor, although this restriction can sometimes be overcome.
- XAML generally cannot call methods, although this restriction can sometimes be overcome.

There is no visual designer for producing XAML in .NET MAUI apps. All XAML must be hand-written, but you can use XAML hot reload to view your UI as you edit it.

XAML is basically XML, but XAML has some unique syntax features. The most important are:

- Property elements
- Attached properties
- Markup extensions

These features are *not* XML extensions. XAML is entirely legal XML. But these XAML syntax features use XML in unique ways.
