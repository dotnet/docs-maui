---
title: "Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant"
description: "Learn how to use .NET Upgrade Assistant to migrate a Xamarin.Forms app to .NET MAUI."
ms.date: 02/15/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net7.0-android", "net7.0-ios" ]
---

# Upgrade a Xamarin.Forms app to .NET MAUI with the .NET Upgrade Assistant

The .NET Upgrade Assistant is a command-line tool that helps you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI) by converting the solution's project file and by performing common code updates. Specifically, the tool will:

- Convert the Xamarin.Forms class library project, Xamarin.iOS project, and Xamarin.Android project to SDK-style projects.
- Update the target framework in project files to net7.0-android and net7.0-ios, as required.
- Set `<UseMaui>true</UseMaui>` in project files.
- Add additional project properties, and remove project properties that aren't required.
- Add and remove specific NuGet packages:
  - Remove the Xamarin.Forms and Xamarin.Essentials NuGet packages.
  - Replace the Xamarin.CommunityToolkit NuGet package with the .NET MAUI Community Toolkit NuGet package.
  - Replace Xamarin.Forms compatible versions of the SkiaSharp NuGet packages with .NET MAUI compatible versions, if used.
- Remove references to the `Xamarin.Essentials` namespace, and replace the `Xamarin.Forms` namespace with the `Microsoft.Maui` and `Microsoft.Maui.Controls` namespaces.

After you run the tool, additional effort will be required to complete the migration.

> [!NOTE]
> The .NET Upgrade Assistant for .NET MAUI doesn't support upgrading UWP projects, iOS extension projects, or binding projects.

For more information about .NET Upgrade Assistant, including the other app types it can convert, see [Overview of the .NET Upgrade Assistant](/dotnet/core/porting/upgrade-assistant-overview).

## Get started

.NET Upgrade Assistant is available for Windows and Mac, and only works with Xamarin.Forms projects. To use it, your Xamarin.Forms project must use Xamarin.Forms 4.8 or higher. However, for best success we recommend that your Xamarin.Forms project uses Xamarin.Forms 5.0, and .NET Standard 2.0 or higher.

> [!IMPORTANT]
> The .NET Upgrade Assistant for .NET MAUI is still under development. Please [file feedback](https://github.com/dotnet/upgrade-assistant/issues/new?assignees=&labels=&projects=&template=20_bug_report.md) <b> with an 'area:Maui' label </b> so we can continue to improve this tool.

.NET Upgrade Assistant backs up your solution, but we recommend using source control. If you're using source control, you can add the `--skip-backup` parameter to bypass the backup and speed up the upgrade process.

## Installation on the CLI

Install the .NET Upgrade Assistant globally with the following command:

```dotnetcli
dotnet tool install -g upgrade-assistant
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

## Installation on VS

Visual Studio 2022 version 17.6.0 or later is highly recommended to run the upgrade assistant. 

1. Open Visual Studio -> Extensions -> Manage Extensions.
2. Search for .NET Upgrade Assistant and select Download.
3. Once the extension has been downloaded, close Visual Studio. This should kick off the installation of the extension; select modify.

You can update the upgrade assistant by:
1. Opening Visual Studio -> Extensions -> Manage Extensions -> Updates.
2. If you see .NET Upgrade Assistant here, then click Update.


## Run upgrade-assistant

Open a terminal and navigate to the folder where the target project or solution is located. Run the `upgrade-assistant upgrade` command, passing in the name of the project or solution you're upgrading:

```dotnetcli
upgrade-assistant upgrade
```

This command runs the tool updates all eligible files within the chosen project and dependent projects.
The upgrade assistant will prompt you with two styles of upgrades:
* <b>In-place</b> : This option upgrades your project without making a copy.

* <b>Side-by-side</b> : This option copies your project and upgrades the copy, leaving the original project alone.

## Next steps

> [!div class="nextstepaction"]
> [Manual migration](forms-projects.md#namespace-changes)
