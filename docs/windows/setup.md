---
title: "Debugging on Windows"
description: "This article explains how to configure Windows 10 and Windows 11 for .NET MAUI app deployment and debugging."
ms.date: 02/28/2022
ms.custom: updateeachrelease
---

# Deploy and debug your .NET MAUI app on Windows

You can use your local Windows development computer to deploy and debug a .NET Multi-platform App UI (.NET MAUI) app. This article describes how to configure Windows to debug a .NET MAUI app.

## Configure Windows

You must enable Developer Mode in Windows. Both Windows 10 and Windows 11 are supported.

:::image type="content" source="media/setup/developer-mode-win11.png" alt-text="Developer mode settings in Windows 11 for MAUI .NET Windows app.":::

### Windows 11

Developer Mode is enabled in **Settings** app, under **Privacy & security** > **For developers**. To enable Developer Mode in Windows 11:

01. Open the Start Menu.
01. Type **Developer settings** in the search box and select it.
01. Turn on **Developer Mode**.
01. If you receive a warning message about Developer Mode, read it, and select **Yes** if you understand the warning.

### Windows 10

Developer Mode is enabled in **Settings** app, under **Update & Security** > **For developers**. To enable Developer Mode in Windows 10:

01. Open the Start Menu.
01. Search for **Developer settings**, select it.
01. Turn on **Developer Mode**.
01. If you receive a warning message about Developer Mode, read it, and select **Yes** if you understand the warning.

## Target Windows

In Visual Studio, set the **Debug Target** to **Framework (...)** > **net8.0-windows**. There is a version number in the item entry, which may or may not match the following screenshot:

:::image type="content" source="media/setup/vs-target-windows-net8.png" alt-text="Visual Studio debug target set to Windows for .NET MAUI app.":::

<!--
## Start Menu entry

The desktop framework used for a .NET MAUI Windows app is [WinUI 3](/windows/apps/winui/winui3/). When you run and debug in Visual Studio, the app is installed and registered with Windows. You'll see an entry in the start menu for the app, which you can uninstall when you're done working with the project.

The Windows app deployment settings are configured in the project file and the _Platforms\\Windows\\Package.appxmanifest_ file:

- Project file

  In the project file, you can set which icon is displayed in the Start Menu entry by changing the `<MauiIcon>` element:

  ```xml
  <MauiIcon Include="Resources\appicon.svg" ForegroundFile="Resources\appiconfg.svg" Color="#512BD4" />
  ```

  For more information, see [App icons](../user-interface/images/app-icons.md).

- App manifest

  In the _Package.appxmanifest_ file, you can set the display name and description used for the Start Menu entry by changing the `<uap:VisualElements>` element:

  ```xml
        <uap:VisualElements
          DisplayName="My MAUI App"
          Description="MauiApp3"
          ... >
  ```
-->
