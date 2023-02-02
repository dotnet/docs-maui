---
title: "Upgrade from Xamarin to .NET"
description: "Learn how to upgrade Xamarin apps to .NET starting with .NET 6"
ms.date: 1/31/2023
---

# Upgrade from Xamarin to .NET

Xamarin projects can run on .NET, starting with .NET 6, after completing an upgrade process. This series of articles describe the process for migrating your Xamarin projects to .NET.

<!-- markdownlint-disable MD032 -->
> [!IMPORTANT]
> To migrate an app from Xamarin to .NET:
> - All projects DO need to become "SDK-style".
> - Projects DON'T need to be rewritten.
> - Multi-project solutions DON'T need to become "single project".
<!-- markdownlint-enable MD025 -->

## Upgrade Xamarin native projects to .NET

To upgrade your Xamarin native projects to .NET, you'll first have to update the projects to be SDK-style projects and then update your dependencies to .NET 6+. For more information, see [Update Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md).

## Automatically upgrade Xamarin.Forms projects to .NET MAUI

The .NET Upgrade Assistant is a command-line tool that can be run to help you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI). After running the tool, in most cases the app will require additional effort to complete the migration. For more information, see [Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant](upgrade-assistant.md).

## Manually upgrade Xamarin.Forms projects to .NET MAUI

Manually upgrading a Xamarin.Forms project to .NET MAUI is a two-step process:

1. Update your Xamarin native projects, in your Xamarin.Forms solution, to .NET. For more information, see [Update Xamarin.Android, Xamarin.iOS, and Xamarin.Mac apps to .NET](native-projects.md).
1. Update your Xamarin.Forms library project to .NET Multi-platform App UI (.NET MAUI). For more information, see [Manually upgrade a Xamarin.Forms app to .NET MAUI](forms-projects.md).
