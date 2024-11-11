---
title: "Debugging on Windows"
description: "This article explains how to configure Windows 10 and Windows 11 for .NET MAUI app deployment and debugging."
ms.date: 11/07/2024
---

# Deploy and debug your .NET MAUI app on Windows

You can use your local Windows development computer to deploy and debug a .NET Multi-platform App UI (.NET MAUI) app. This article describes how to configure Windows to debug a .NET MAUI app.

::: moniker range="=net-maui-8.0"

When debugging and deploying a new .NET MAUI project to Windows, the default behavior is to deploy a packaged app. For information about packaged apps, see [Windows apps: packaging, deployment, and process](/windows/apps/get-started/intro-pack-dep-proc).

[!INCLUDE [Configure Windows for packaged app deployment](includes/configure-packaged.md)]

[!INCLUDE [Target Windows](includes/target-windows.md)]

[!INCLUDE [Convert a packaged .NET MAUI Windows app to unpackaged](includes/convert-packaged-to-unpackaged.md)]

[!INCLUDE [Convert an unpackaged .NET MAUI Windows app to packaged](includes/convert-unpackaged-to-packaged.md)]

::: moniker-end

::: moniker range=">=net-maui-9.0"

When debugging and deploying a new .NET MAUI project to Windows, the default behavior is to deploy an unpackaged app. For information about unpackaged apps, see [Windows apps: packaging, deployment, and process](/windows/apps/get-started/intro-pack-dep-proc).

[!INCLUDE [Target Windows](includes/target-windows.md)]

[!INCLUDE [Convert an unpackaged .NET MAUI Windows app to packaged](includes/convert-unpackaged-to-packaged.md)]

[!INCLUDE [Convert a packaged .NET MAUI Windows app to unpackaged](includes/convert-packaged-to-unpackaged.md)]

[!INCLUDE [Configure Windows for packaged app deployment](includes/configure-packaged.md)]

::: moniker-end

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
