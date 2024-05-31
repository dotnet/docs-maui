---
title: "Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app with the project template"
description: "Learn how to manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app using the Visual Studio project template."
ms.date: 05/31/2024
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net8.0-android", "net8.0-ios" ]
---

# Manually upgrade a Xamarin.Forms app to a multi-project .NET MAUI app with the project template

Upgrading a multi-project Xamarin.Forms app to a multi-project .NET Multi-platform App UI (.NET MAUI) app can be accomplished by creating a new multi-project .NET MAUI app and then migrating the code and resources from your Xamarin.Forms apps to the multi-project .NET MAUI app. There are then additional steps to take advantage of changes in .NET MAUI.

To manually migrate a multi-project Xamarin.Forms app to a multi-project .NET MAUI library app, you must:

> [!div class="checklist"]
>
> - Update your Xamarin.Forms app to use Xamarin.Forms 5.
> - Update the app's dependencies to the latest versions.
> - Ensure the app still works.
> - Update namespaces.
> - Address any API changes.
> - Upgrade or replace incompatible dependencies with .NET 8 versions.
> - Compile and test your app.

To simplify the upgrade process, you should create a new multi-project .NET MAUI app of the same name as your Xamarin.Forms app, and then copy in your code, configuration, and resources. This is the approach outlined below.

## Update your Xamarin.Forms app

Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions.

This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.

## Create a new project

In Visual Studio, create a new .NET MAUI multi-project app of the same name as your Xamarin.Forms project. The multi-project template provides a .NET MAUI app for Android, iOS, Mac Catalyst, and WinUI with multiple, separate app project.

> [!NOTE]
> You can remove the the app projects for platforms you don't support.

Opening the project file for the class library will confirm that you have a .NET SDK-style project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net8.0;net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFramework>
        <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
        <SingleProject>true</SingleProject>
        <ImplicitUsings>enable</ImplicitUsings>
        <UseMaui>true</UseMaui>
        <Nullable>enable</Nullable>
    </PropertyGroup>

</Project>
```

Opening a specific platform project should also confirm that you have similar properties set including versioning with a reference to the library project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net8.0-android</TargetFramework>
        <SupportedOSPlatformVersion>21.0</SupportedOSPlatformVersion>
        <OutputType>Exe</OutputType>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
        <UseMaui>true</UseMaui>
    </PropertyGroup>

    <!--
        ...
    -->

    <ItemGroup>
        <ProjectReference Include="..\MauiApp.1\MauiApp.1.csproj" />
    </ItemGroup>

</Project>
```

## Copy files to the .NET MAUI app

All of the cross-platform code from your Xamarin.Forms library project should be copied into your .NET MAUI library project in identically named folders and files.

Custom renderers can either be reused in a .NET MAUI app, or migrated to a .NET MAUI handler. For more information, see [Reuse custom renderers in .NET MAUI](custom-renderers.md) and [Migrate a Xamarin.Forms custom renderer to a .NET MAUI handler](renderer-to-handler.md).

Effects can be reused in a .NET MAUI app. For more information, see [Reuse effects](effects.md).

If there are platform specific services that need to be migrated to .NET MAUI, use the <xref:Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Type)> method to add a transient service of the specified type to the specified <xref:Microsoft.Extensions.DependencyInjection.IServiceCollection>.

Any platform code, resources, and configuration should then be copied to your multi-project .NET MAUI app.

[!INCLUDE [Namespace changes](includes/namespace-changes.md)]

[!INCLUDE [API changes](includes/api-changes.md)]

[!INCLUDE [AssemblyInfo changes](includes/assemblyinfo-changes.md)]

[!INCLUDE [Update app dependencies](includes/update-app-dependencies.md)]

[!INCLUDE [Compile and troubleshoot](includes/compile-troubleshoot.md)]

## See also

- [Porting from .NET Framework to .NET](/dotnet/core/porting/)
- [.NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview)
