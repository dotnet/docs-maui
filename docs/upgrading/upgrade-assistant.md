---
title: "Upgrading from Xamarin to .NET with .NET Upgrade Assistant"
description: ".NET Upgrade Assistant handles repetitive tasks to help you upgrade to .NET from .NET Framework."
ms.date: 1/20/2023
---

# .NET Upgrade Assistant Steps

The .NET Upgrade Assistant will help you upgrade Xamarin.Forms projects to .NET MAUI by converting the solution's csproj and performing common code updates.

> Please [file feedback](https://github.com/maddymontaquila/maui-migration-samples/issues/new?assignees=&labels=&template=trial-migration-template.md&title=[MIGRATION]+Your+migration+name+here) so we may continue to improve this tool.

## Getting started

The assistant will make a backup of your solution, but we recommend using source control. When using source control you may add the `--skip-backup` parameter to bypass the backup and speed up the upgrade process.

### Pre-requisites

For best success we recommend your Xamarin.Forms projects are:

* Xamarin.Forms 5.0
* .NET Standarind 2.0 or greater

### Limitations
-   .NET Upgrade Assistant is available for Windows only
-	Must be on Xamarin.Forms 4.8 or higher
-	Only works with Xamarin.Forms .slns (Xamarin.Android and Xamarin.iOS only solutions coming later)
-	Will not work with binding or library projects
-	Gets you _most_ of the way there – you’ll still have to do some manual changes too

### Installation

Install the .NET Upgrade Assistant dotnet tool: 

```
dotnet tool update --global upgrade-assistant
```

### Running the upgrade

Start the upgrade process and follow the interactive instructions:

```
upgrade-assistant upgrade <path to sln or csproj> --non-interactive --entry-point *
```

This command runs the tool in non-interactive mode. It will update all eligible projects in the solution and dependent projects.


### See also

* [Custom renderers in .NET MAUI](using-custom-renderers.md)
* [Default value changes](defaults.md)
* [Layout changes](layout-reference.md)
* [Troubleshooting guide](troubleshooting.md)