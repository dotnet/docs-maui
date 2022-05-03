---
title: "Migrate your app from Xamarin.Forms to .NET MAUI"
description: "Learn how to migrate your Xamarin.Forms app to .NET MAUI."
ms.date: 05/02/2022
---

# Migrate your app from Xamarin.Forms

You don't need to rewrite your Xamarin.Forms apps to move them to .NET Multi-platform App UI (.NET MAUI). However, you will need to make a small amount of code changes to each app. Similarly, you can use single-project features without merging all of your Xamarin.Forms projects into one project.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

To migrate a Xamarin.Forms app to .NET 6 and update the code to .NET MAUI, you'll need to do the following:

- Convert the projects from .NET Framework to .NET SDK style.
- Update namespaces.
- Update any incompatible NuGet packages.
- Address any breaking API changes.
- Run the converted app and verify that it functions correctly.

For more information, see [Migrating from Xamarin.Forms](https://github.com/dotnet/maui/wiki/Migrating-from-Xamarin.Forms-(Preview)) on the .NET MAUI Wiki.
