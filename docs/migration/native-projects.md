---
title: "Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac projects to .NET"
description: "Learn how to manually upgrade Xamarin native projects to .NET."
ms.date: 02/15/2023
---

# Upgrade Xamarin.Android, Xamarin.iOS, and Xamarin.Mac projects to .NET

To upgrade your Xamarin native projects to .NET, you must:

> [!div class="checklist"]
>
> - Update your project file to be SDK-style.
> - Update or replace incompatible dependencies with .NET 8 versions.
> - Compile and test your app.

For most projects you won't need to change namespaces or undertake other rewrites.

To simplify the upgrade process, we recommend creating a new .NET project of the same type and name as your Xamarin native project, and then copy in your code. This is the approach outlined below.

## Create a new project

In Visual Studio, create a new .NET project of the same type and name as your Xamarin native project. For example, to upgrade from Xamarin.Android to .NET Android select the **Android Application** project template:

:::image type="content" source="media/new-android-app.png" alt-text="Screenshot of selecting the Android app project template in Visual Studio.":::

The new project should be given the same project and package name as your existing project, and should be placed in a new folder. Opening the project file will confirm that you have a .NET SDK-style project:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0-android</TargetFramework>
    <SupportedOSPlatformVersion>21</SupportedOSPlatformVersion>
    <OutputType>Exe</OutputType>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <ApplicationId>com.companyname.AndroidApp2</ApplicationId>
    <ApplicationVersion>1</ApplicationVersion>
    <ApplicationDisplayVersion>1.0</ApplicationDisplayVersion>
  </PropertyGroup>
</Project>
```

> [!IMPORTANT]
> The target framework moniker (TFM) is what denotes the project as using .NET, in this case .NET 8. Valid TFMs for equivalent Xamarin native projects are net8.0-android, net8.0-ios, net8.0-macos, and net8.0-tvos. For information about target frameworks in SDK-style projects, see [Target frameworks in SDK-style projects](/dotnet/standard/frameworks).

Launch the app to confirm that your development environment can build the app.

## Merge files

Copy your code and resource files from the folders of your Xamarin native project to identical folders within your new app. You should overwrite any files of the same name.

If you have other library projects, you should add them to your new solution and [add project references](/visualstudio/ide/managing-references-in-a-project) to them from your new .NET project.

You'll also need to copy some project properties from your Xamarin native project to your new .NET project, for settings like conditional compilation arguments and code signing. Opening the projects side-by-side in separate Visual Studio instances will enable you to compare the project properties. Alternatively, you can migrate the settings by editing the new project file directly. For more information, see [Xamarin.Android project migration](android-projects.md) and [Xamarin Apple project migration](apple-projects.md).

## Update dependencies

Generally, Xamarin native NuGet packages are not compatible with .NET 8 unless they have been recompiled using .NET TFMs. However, .NET Android apps can use NuGet packages targeting the `monoandroid` and `monoandroidXX.X` frameworks.

You can confirm a package is .NET 8 compatible by looking at the **Frameworks** tab on [NuGet](https://nuget.org) for the package you're using, and checking that it lists one of the compatible frameworks shown in the following table:

| Compatible frameworks | Incompatible frameworks |
| --- | --- |
| net8.0-android, monoandroid, monoandroidXX.X | |
| net8.0-ios | monotouch, xamarinios, xamarinios10 |
| net8.0-macos | monomac, xamarinmac, xamarinmac20 |
| net8.0-tvos | xamarintvos |
| | xamarinwatchos |

> [!NOTE]
> .NET Standard libraries that have no dependencies on the incompatible frameworks listed above are still compatible with .NET 8.

If a package on [NuGet](https://nuget.org) indicates compatibility with any of the compatible frameworks above, regardless of also including incompatible frameworks, then the package is compatible. Compatible NuGet packages can be added to your .NET native project using the NuGet package manager in Visual Studio.

If you can't find a .NET 8 compatible version of a NuGet package you should:

- Recompile the package with .NET TFMs, if you own the code.
- Look for a preview release of a .NET 8 version of the package.
- Replace the dependency with a .NET 8 compatible alternative.

For information about migrating Xamarin.Essentials code in a .NET Android or .NET iOS app, see [Migrate Xamarin.Essentials code in .NET Android and .NET iOS apps](native-essentials.md).

## Compile and troubleshoot

Once your dependencies are resolved and your code and resource files are added to your .NET native project, you should build your project. Any errors will guide you towards next steps.

<!-- markdownlint-disable MD032 -->
> [!TIP]
> - Delete all *bin* and *obj* folders from all projects before opening and building projects in Visual Studio, particularly when changing .NET versions.
> - Delete the *Resource.designer.cs* generated file from the Android project.
<!-- markdownlint-enable MD032 -->
