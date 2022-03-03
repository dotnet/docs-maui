---
title: "Debugging on Windows"
description: "This article explains how to configure Windows 10 and Windows 11 for .NET MAUI app deployment and debugging."
ms.date: 02/28/2022
---

You can use your local Windows development computer to deploy and debug a .NET MAUI app. This article describes how to configure Windows to debug a .NET MAUI app.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

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

In Visual Studio, set the **Debug Target** to **Framework (...)** > **net6.0-windows10.0.19041**:

:::image type="content" source="media/setup/vs-target-windows.png" alt-text="Visual Studio debug target set to Windows for .NET MAUI app.":::

## Start Menu entry

The desktop framework used for a MAUI .NET Windows app is [WinUI 3](/windows/apps/winui/winui3/). When you run and debug in Visual Studio, the app is installed and registered with Windows. You'll see an entry in the start menu for the app. This is different from other Windows UI frameworks, which typically don't install the app.

The Windows app deployment settings are configured in the _Platforms\\Windows\\Package.appxmanifest_ file, such as the entry name and description. Some settings, such as the app icon, are set in the project file.
