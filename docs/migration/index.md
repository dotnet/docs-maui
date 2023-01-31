---
title: "Upgrade from Xamarin to .NET"
description: "Learn how to upgrade Xamarin apps to .NET starting with .NET 6"
ms.date: 1/31/2023
---

# Upgrade from Xamarin to .NET

Xamarin projects can run on .NET, starting with .NET 6, after completing an upgrade process. This series of articles describe the process for migrating your Xamarin projects to .NET.

> [!IMPORTANT]
> - All projects DO need to become "SDK style".
> - Projects do NOT need to be rewritten.
> - Multi-project solutions do NOT need to become "single project".

The .NET Upgrade Assistant is a command-line tool that can be run to help you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI). After running the tool, in most cases the app will require additional effort to complete the migration. For more information, see [Upgrade a Xamarin.Forms project to .NET MAUI](upgrade-assistant.md).

Alternatively, you may choose to manually upgrade your apps. To upgrade your Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET, you'll have to update the projects to be SDK-style projects and then update your dependencies to .NET 6+. For more information, see [Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](xamarin-projects.md).

You don't need to rewrite your Xamarin.Forms apps to move them to .NET MAUI. However, you will need to make a small amount of code changes to each app. Similarly, you can use single-project features without merging all of your Xamarin.Forms projects into one project.

To migrate a Xamarin.Forms app to .NET MAUI, you'll need to do the following:

- Convert the projects from .NET Framework to .NET SDK style.
- Update namespaces.
- Update any incompatible NuGet packages.
- Address any breaking API changes.
- Run the converted app and verify that it functions correctly.

For more information, see [Upgrade a Xamarin.Forms app to .NET MAUI](forms-projects.md).
