---
title: "Upgrade manually to a multi-project app"
description: "Learn about the two approaches to manually upgrading a Xamarin.Forms app to a multi-project .NET MAUI app."
ms.date: 05/31/2024
no-loc: [ "Xamarin.Forms" ]
---

# Upgrade manually to a multi-project app

A Xamarin.Forms app can be manually upgraded to a multi-project .NET Multi-platform App UI (.NET MAUI) app with two approaches:

- By creating a new .NET MAUI app using the multi-project template.

  With this approach you create a new multi-project .NET MAUI app, and then migrate the code and resources from your Xamarin.Forms app to the multi-project .NET MAUI app. For more information, see [Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app with the project template](multi-project-to-multi-project-with-template.md).

- By migrating a Xamarin.Forms library project to a .NET MAUI library project.

  With this approach you create a new .NET MAUI library project that replaces your Xamarin.Forms library project, and then update your Xamarin.Forms platform projects to be SDK-style projects and reference the .NET MAUI library project. You'll then need to enable .NET MAUI support in each platform project, update each platform project's entry point class, and then configure the bootstrapping of your .NET MAUI app. For more information, see [Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app](multi-project-to-multi-project.md).

Using either multi-project template approach for your .NET MAUI app can more easily provide one-to-one mappings as you incrementally upgrade individual components of your project.

> [!IMPORTANT]
> Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions. This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.
