---
title: "Manually upgrade a multi-project Xamarin app to a single project .NET MAUI app"
description: "Learn how to manually upgrade a multi-project Xamarin.Forms app to a single project .NET MAUI app."
ms.date: 08/29/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net7.0-android", "net7.0-ios" ]
---

# Manually upgrade a multi-project Xamarin app to a single project .NET MAUI app

To migrate a multi-project Xamarin.Forms app to a single-project .NET Multi-platform App UI (.NET MAUI) app, you must:

> [!div class="checklist"]
>
> - Update your Xamarin.Forms app to use Xamarin.Forms 5.
> - Update the app's dependencies to the latest versions.
> - Ensure the app still works.
> - Review NuGet compatibility.
> - Create a .NET MAUI app.
> - Copy code and configuration from the Xamarin.Forms app to the .NET MAUI app.
> - Copy resources from the Xamarin.Forms app to the .NET MAUI app.
> - Update namespaces.
> - Address any API changes.
> - Upgrade or replace incompatible dependencies with .NET 6+ versions.
> - Compile and test your app.

To simplify the upgrade process, you should create a new .NET MAUI app of the same name as your Xamarin.Forms app, and then copy in your code and configuration. This is the approach outlined below.

https://github.com/davidortinau/SmartHotel360-Mobile/blob/upgrade-step-1/Documents/UpgradeDiary/UpgradingSmartHotel360.md

## Update your Xamarin.Forms app

Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions.

This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.

## Create a .NET MAUI app

In Visual Studio, create a new .NET MAUI app using the same name as your Xamarin.Forms app:

:::image type="content" source="media/multi-project-to-single-project.png" alt-text="Screenshot of creating a .NET MAUI app in Visual Studio.":::

Opening the project file will confirm that you have a .NET SDK-style project.

## Copy code to the .NET MAUI app

Migrate effects/custom renderers.

For a list of breaking changes for .NET iOS, see [Breaking changes in .NET](https://github.com/xamarin/xamarin-macios/wiki/Breaking-changes-in-.NET).

## Copy configuration to the .NET MAUI app

Manifest, plist, appx manifest.

## Copy resources to the .NET MAUI app

Images, fonts, other resources, localization files.

[!INCLUDE [Namespace changes](includes/namespace-changes.md)]

[!INCLUDE [API changes](includes/api-changes.md)]

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

[!INCLUDE [Update app dependencies](includes/update-app-dependencies.md)]

[!INCLUDE [Compile and troubleshoot](includes/compile-troubleshoot.md)]

Obsolete APIs will still work but should be replaced for future proofing.
