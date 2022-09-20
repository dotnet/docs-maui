---
title: "Install Visual Studio to develop cross-platform apps using .NET MAUI"
description: "Learn how to install Visual Studio 2022 and Visual Studio 2022 for Mac, to develop native, cross-platform apps using .NET MAUI."
ms.date: 09/20/2022
---

# Installation

Developing native, cross-platform .NET Multi-platform App UI (.NET MAUI) apps requires Visual Studio 2022 or Visual Studio 2022 for Mac 17.4 Preview.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vswin)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET MAUI apps on Windows, install Visual Studio 2022 17.3 or greater by following the steps below.

## Prerequisites

- Visual Studio 2022 17.3 or greater, with the .NET Multi-platform App UI development workload installed. For information about supported operating systems, hardware, supported languages, and additional requirements and guidance, see [Visual Studio 2022 System Requirements](/visualstudio/releases/2022/system-requirements).

To build .NET MAUI apps for iOS, you'll need:

- A Mac that is compatible with the latest version of Xcode. See Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/)
- The latest version of [Xcode](https://developer.apple.com/xcode).
- An Apple ID. If you don't have an Apple ID already, you can create a new one at https://appleid.apple.com. An Apple ID is required to deploy apps to devices, and to submit apps to the Apple Store.

## Installation

1. To create .NET MAUI apps, you'll need the latest version of Visual Studio 2022:

    - [Download Visual Studio 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    - [Download Visual Studio 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    - [Download Visual Studio 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

1. Either install Visual Studio, or modify your existing installation, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/first-app/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

    Status screens will appear that show the progress of your Visual Studio 2022 installation.

After your Visual Studio 2022 installation is complete, select the **Launch** button to start developing cross-platform apps using .NET MAUI.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET MAUI apps on macOS, install Visual Studio 2022 for Mac 17.4 Preview by following the steps below.

## Prerequisites

- Visual Studio 2022 for Mac 17.4 Preview, with the .NET, .NET MAUI, Android, and iOS workloads installed. For information about supported operating systems, hardware, supported languages, and additional requirements and guidance, see [Visual Studio 2022 for Mac System Requirements](/visualstudio/releases/2022/mac-system-requirements).

To build .NET MAUI apps for iOS or macOS, you'll need:

- A Mac that is compatible with the latest version of Xcode. See Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/)
- The latest version of [Xcode](https://developer.apple.com/xcode).
- An Apple ID. If you don't have an Apple ID already, you can create a new one at [Apple ID](https://appleid.apple.com). An Apple ID is required to deploy apps to devices, and to submit apps to the Apple Store.

## Installation

1. To create .NET MAUI apps, you'll need the latest Visual Studio for Mac 17.4 Preview:

    > [!div class="button"]
    > [Download Visual Studio 2022 for Mac 17.4 Preview](https://aka.ms/installer/preview)

1. Either install Visual Studio 2022 for Mac 17.4 Preview, or modify your existing installation, and ensure that the following workloads are installed:

    - .NET
    - .NET MAUI
    - Android
    - iOS

    :::image type="content" source="media/installation/vsmac/maui-workloads.png" alt-text="Visual Studio for Mac .NET MAUI workloads.":::

    :::image type="content" source="media/installation/vsmac/legacy-workloads.png" alt-text="Visual Studio for Mac legacy workloads.":::

    <!-- The legacy workloads shouldn't be required in 17.4 P2 -->

If you have network trouble while installing in a corporate environment, review the [installing behind a firewall or proxy](#install-visual-studio-for-mac-behind-a-firewall-or-proxy-server) instructions.

> [!NOTE]
> If you chose not to install a platform or tool during the original installation (by unselecting it in step #6), you must run the installer again if you wish to add the components later.

## Install Visual Studio for Mac behind a firewall or proxy server

To install Visual Studio 2022 for Mac Preview behind a firewall, certain endpoints must be made accessible in order to allow downloads of the required tools and updates for your software. For more information about configuring your network to allow access to the required endpoints, see [Install and use Visual Studio for Mac behind a firewall or proxy server](/visualstudio/mac/install-behind-a-firewall-or-proxy-server?view=vsmac-2022).

---

## Next steps

To learn how to create and run your first .NET MAUI app in Visual Studio 2022 on Windows, or Visual Studio 2022 for Mac 17.4 Preview, .

> [!div class="nextstepaction"]
> [Build your first app](first-app.md)
