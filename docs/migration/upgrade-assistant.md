---
title: "Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant"
description: "Learn how to use .NET Upgrade Assistant to migrate a Xamarin.Forms app to .NET MAUI."
ms.date: 2/10/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps" ]
---

# Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant

The .NET Upgrade Assistant is a command-line tool that will help you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI) by converting the solution's project file and by performing common code updates. Specifically, the tool will:

- Converts the Xamarin.Forms class library project, Xamarin.iOS project, and Xamarin.Android project to SDK-style projects.
- Updates the target framework in project files to net7.0-android and net7.0-ios, as required.
- Sets `<UseMaui>true</UseMaui>` in project files.
- Adds additional project properties, and removes project properties that aren't required.
- Adds and removes various NuGet packages. This includes removing the Xamarin.Forms and Xamarin.Essentials NuGet packages, replacing the Xamarin.CommunityToolkit NuGet package with the .NET MAUI Community Toolkit, and ensuring that .NET MAUI compatible versions of the SkiaSharp NuGet packages are used where required.
- Replaces Xamarin namespaces in XAML and code, with the exception of Xamarin.Forms.Maps.

After running the tool, the app will require additional effort to complete the migration.

> [!NOTE]
> The .NET Upgrade Assistant doesn't support upgrading UWP projects, iOS extension projects, or binding projects.

For more information about .NET Upgrade Assistant, including the other app types it can convert, see [Overview of the .NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview).

> [!IMPORTANT]
> The .NET Upgrade Assistant for .NET MAUI is still under development. Please [file feedback](https://github.com/maddymontaquila/maui-migration-samples/issues/new?assignees=&labels=&template=trial-migration-template.md&title=[MIGRATION]+Your+migration+name+here) so we can continue to improve this tool.

## Get started

.NET Upgrade Assistant is currently only available for Windows, and only works with Xamarin.Forms projects. It doesn't work with Xamarin native projects, or binding or library projects. To use it, your Xamarin.Forms project must use Xamarin.Forms 4.8 or higher. However, for best success we recommend that your Xamarin.Forms project uses Xamarin.Forms 5.0, and .NET Standard 2.0 or higher.

.NET Upgrade Assistant will make a backup of your solution, but we recommend using source control. When using source control you may add the `--skip-backup` parameter to bypass the backup and speed up the upgrade process.

## Installation

Install the .NET Upgrade Assistant globally with the following command:

```dotnetcli
dotnet tool update -g upgrade-assistant
```

Similarly, because the .NET Upgrade Assistant is installed as a .NET tool, it can be easily updated by running:

```dotnetcli
dotnet tool update -g upgrade-assistant
```

> [!IMPORTANT]
> Installing this tool may fail if you've configued additional NuGet feed source. Use the `--ignore-failed-sources` parameter to treat those failures as warnings instead of errors:
>
> ```dotnetcli
> dotnet tool install -g --ignore-failed-sources upgrade-assistant
> ```

## Run upgrade-assistant

Open a terminal and navigate to the folder where the target project or solution is located. Run the `upgrade-assistant upgrade` command, passing in the name of the project or solution you're upgrading:

```dotnetcli
upgrade-assistant upgrade <sln or csproj> --non-interactive --entry-point *
```

This command runs the tool in non-interactive mode. It will update all eligible projects in the solution and dependent projects.
