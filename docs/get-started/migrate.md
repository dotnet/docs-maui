---
title: "Migrate your app from Xamarin.Forms to .NET MAUI"
description: "Migrate your Xamarin.Forms app to .NET MAUI with the .NET upgrade assistant."
ms.date: 06/07/2021
---

# Migrate your app from Xamarin.Forms

You don't need to rewrite your Xamarin.Forms apps to move them to .NET Multi-platform App UI (.NET MAUI). However, you need to make a small amount of code changes to each app. Similarly, you can use single-project features without merging all of your Xamarin.Forms projects into one project.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The process for migrating a Xamarin.Forms app to .NET MAUI is expected to be:

1. Use the [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview) for .NET MAUI to migrate your Xamarin.Forms projects to .NET MAUI single project, and perform well-known code namespace changes.
1. Update any dependencies to .NET 6 and .NET MAUI compatible versions.
1. Register any compatibility services or renderers.
1. Build and fix any issues.
1. Run the converted app and verify that it functions correctly.

> [!WARNING]
> Do not currently migrate your production apps to .NET MAUI.

## Port an app example

This example ports the [Button Demos](/samples/xamarin/xamarin-forms-samples/userinterface-buttondemos/) sample. The process is as follows:

1. Create a new, blank, multi-targeted .NET MAUI project:

    ```dotnetcli
    dotnet new maui -n ButtonDemos
    ```

1. Restore the dependencies for the newly created project:

    ```dotnetcli
    cd ButtonDemos
    dotnet restore
    ```

    > [!NOTE]
    > If you are unable to restore project dependencies, ensure that you have the latest .NET MAUI preview installed.

1. Copy the [code files](https://github.com/xamarin/xamarin-forms-samples/tree/main/UserInterface/ButtonDemos/ButtonDemos/ButtonDemos) (except *App.xaml*) into the newly created project.
1. In the newly created project, replace the following namespaces:

    | Old namespace | New namespace |
    | --- | --- |
    | `xmlns="http://xamarin.com/schemas/2014/forms"` | `xmlns="http://schemas.microsoft.com/dotnet/2021/maui"` |
    | `using Xamarin.Forms` | `using Microsoft.Maui` **AND** `using Microsoft.Maui.Controls` |
    | `using Xamarin.Forms.Xaml` | `using Microsoft.Maui.Controls.Xaml` |

1. Run the app on your chosen platform:

    ```dotnetcli
    dotnet build -t:Run -f net6.0-android
    dotnet build -t:Run -f net6.0-ios
    dotnet build -t:Run -f net6.0-maccatalyst
    ```

1. Examine any run-time errors, some of which will be due to incomplete handler availability.
