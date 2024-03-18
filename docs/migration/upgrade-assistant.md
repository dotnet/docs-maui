---
title: "Upgrade a Xamarin.Forms app to a .NET MAUI app with the .NET Upgrade Assistant"
description: "Learn how to use .NET Upgrade Assistant to migrate a Xamarin.Forms app to a multi-project .NET MAUI app."
ms.date: 08/21/2023
no-loc: [ "Xamarin.Forms", "Xamarin.Essentials", "Xamarin.CommunityToolkit", ".NET MAUI Community Toolkit", "SkiaSharp", "Xamarin.Forms.Maps", "Microsoft.Maui", "Microsoft.Maui.Controls", "net8.0-android", "net8.0-ios" ]
---

# Upgrade a Xamarin.Forms app to a .NET MAUI app with the .NET Upgrade Assistant

The .NET Upgrade Assistant helps you upgrade Xamarin.Forms projects to .NET Multi-platform App UI (.NET MAUI) by converting the solution's project file and by performing common code updates. Specifically, the tool will:

- Convert the Xamarin.Forms class library project, Xamarin.iOS project, and Xamarin.Android project to SDK-style projects.
- Update the target framework in project files to net8.0-android and net8.0-ios, as required.
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

.NET Upgrade Assistant is available as a Visual Studio extension on Windows, and as a CLI tool for Windows and Mac. To use it, your Xamarin.Forms project must use Xamarin.Forms 4.8 or higher. However, for best success we recommend that your Xamarin.Forms project uses Xamarin.Forms 5.0, and .NET Standard 2.0 or higher.

> [!IMPORTANT]
> The .NET Upgrade Assistant for .NET MAUI is still under development. Please [file feedback](https://github.com/dotnet/upgrade-assistant/issues/new?assignees=&labels=&projects=&template=20_bug_report.md) with an **'area:Maui' label** so we can continue to improve this tool.

.NET Upgrade Assistant will prompt you for the type of upgrade to perform:

- **In-place**: This option upgrades your project without making a copy.
- **Side-by-side**: This option copies your project and upgrades the copy, leaving the original project alone. Currently, a .NET MAUI side-by-side upgrade requires the creation of new projects to upgrade each project head. Attempting to upgrade a project head into an existing project will likely cause errors and is not a supported experience at this time.

Before upgrading your Xamarin.Forms app to .NET MAUI, you should first update your Xamarin.Forms app to use Xamarin.Forms 5 and ensure that it still runs correctly. In addition, you should update the dependencies that your app uses to the latest versions.

This will help to simplify the rest of the migration process, as it will minimize the API differences between Xamarin.Forms and .NET MAUI, and will ensure that you are using .NET compatible versions of your dependencies if they exist.

## Quick actions in Visual Studio

.NET Upgrade Assistant has a quick action for converting Xamarin.Forms namespaces to the appropriate .NET MAUI namespaces, that's displayed as a light bulb:

:::image type="content" source="media/ua-light-bulb.png" alt-text="Screenshot of previewing the application of a light bulb suggestion.":::

The quick action will make the necessary replacements across the file.

> [!NOTE]
> Quick action light bulbs will appear as you migrate the files from your Xamarin.Forms project into a .NET MAUI project.

## Installation

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vswin)
<!-- markdownlint-enable MD025 -->

Visual Studio 2022 version 17.6.0 or later is highly recommended to run the upgrade assistant. To install the upgrade assistant extension:

1. Open Visual Studio and select the **Extensions > Manage Extensions** menu item.
1. In the **Manage Extensions** dialog, search for and download the .NET Upgrade Assistant.
1. Once the extension has been downloaded, close Visual Studio. The extension will then be installed.

Extensions are updated automatically when a new version is available on Visual Studio Marketplace. For more information, see [Automatic extension updates](/visualstudio/ide/finding-and-using-visual-studio-extensions#automatic-extension-updates).

<!-- markdownlint-disable MD025 -->
# [CLI](#tab/cli)
<!-- markdownlint-enable MD025 -->

Install the .NET Upgrade Assistant globally with the following command:

```dotnetcli
dotnet tool install -g upgrade-assistant
```

You can update .NET Upgrade Assistant with the following command:

```dotnetcli
dotnet tool update -g upgrade-assistant
```

> [!IMPORTANT]
> Installing this tool may fail if you've configued additional NuGet feed source. Use the `--ignore-failed-sources` parameter to treat those failures as warnings instead of errors:
>
> ```dotnetcli
> dotnet tool install -g --ignore-failed-sources upgrade-assistant
> ```

---

## Run the upgrade assistant

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vswin)
<!-- markdownlint-enable MD025 -->

Right-click on the project in Solution Explorer and select **Upgrade**.

<!-- markdownlint-disable MD025 -->
# [CLI](#tab/cli)
<!-- markdownlint-enable MD025 -->

Open a terminal and navigate to the folder where the target project or solution is located. Run the `upgrade-assistant upgrade` command:

```dotnetcli
upgrade-assistant upgrade
```

This command runs the tool updates all eligible files within the chosen project and dependent projects.

<!-- .NET Upgrade Assistant backs up your solution, but we recommend using source control. If you're using source control, you can add the `--skip-backup` parameter to bypass the backup and speed up the upgrade process. -->

---

## Next steps

> [!div class="nextstepaction"]
> [Manual migration](multi-project-to-multi-project.md#namespace-changes)
