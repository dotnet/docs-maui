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

## Minimal Bindings

This approach to bindings glues the platform library to .NET in the same way as full bindings, but only accounts for the smallest API surface area required for your needs. 

## Full Bindings

A full binding approach is best when you want to deliver a complete .NET library to developers that requires no platform language knowledge. In order to do deliver this seamless experience to developer, you'll need to have deep knowledge of both .NET and the platform language and interop techniques. 