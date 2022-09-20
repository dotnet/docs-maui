---
title: "Install Visual Studio to develop cross-platform apps using .NET MAUI"
description: "Learn how to install Visual Studio 2022 and Visual Studio 2022 for Mac, to develop native, cross-platform apps using .NET MAUI."
ms.date: 09/20/2022
---

# Installation

In this article, you'll learn how to install Visual Studio 2022 on Windows, or Visual Studio 2022 for Mac 17.4 Preview, so that you can develop native, cross-platform apps using .NET Multi-platform App UI (.NET MAUI).

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vswin)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET Multi-platform App UI (.NET MAUI) apps on Windows, install Visual Studio 2022 17.3 or greater by following the steps below. For information about supported operating systems, hardware, supported languages, and additional requirements and guidance, see [Visual Studio 2022 System Requirements](/visualstudio/releases/2022/system-requirements).

For information about what's new in Visual Studio 2022, see the [release notes](/visualstudio/releases/2022/release-notes).

## Prerequisites

To build .NET MAUI apps for iOS, you'll need:

- A Mac that is compatible with the latest version of Xcode. See Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/)
- The latest version of [Xcode](https://developer.apple.com/xcode). It may be possible to [use an older version of Xcode](/xamarin/ios/troubleshooting/questions/old-version-xcode) if your Mac isn't compatible with the latest version.
- An Apple ID. If you don't have an Apple ID already, you can create a new one at https://appleid.apple.com. It's necessary to have an Apple ID for deploying apps to devices.

## Installation

1. To create .NET MAUI apps, you'll need the latest version of Visual Studio 2022:

    > [!div class="button"]
    > [Download Visual Studio 2022 Community](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Community&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    > [!div class="button"]
    > [Download Visual Studio 2022 Professional](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Professional&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

    > [!div class="button"]
    > [Download Visual Studio 2022 Enterprise](https://c2rsetup.officeapps.live.com/c2r/downloadVS.aspx?sku=Enterprise&channel=Release&Version=VS2022&source=VSLandingPage&add=Microsoft.VisualStudio.Workload.CoreEditor&add=Microsoft.VisualStudio.Workload.NetCrossPlat;includeRecommended&cid=2303)

1. Either install Visual Studio, or modify your existing installation, and install the .NET Multi-platform App UI development workload with its default optional installation options:

    :::image type="content" source="media/first-app/vs/vs-workloads.png" alt-text="Visual Studio workloads for .NET MAUI.":::

    Status screens will appear that show the progress of your Visual Studio 2022 installation.

After your Visual Studio 2022 installation is complete, select the **Launch** button to start developing cross-platform apps using .NET MAUI.

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

To start developing native, cross-platform .NET Multi-platform App UI (.NET MAUI) apps on macOS, install Visual Studio 2022 for Mac 17.4 Preview by following the steps below. For information about supported operating systems, hardware, supported languages, and additional requirements and guidance, see [Visual Studio 2022 for Mac System Requirements](/visualstudio/releases/2022/mac-system-requirements).

For information about what's new in Visual Studio 2022 for Mac Preview, see the [release notes](/visualstudio/releases/2022/mac-release-notes).

## Prerequisites

To build .NET MAUI apps for iOS or macOS, you'll need:

- A Mac that is compatible with the latest version of Xcode. See Apple's [minimum requirements documentation](https://developer.apple.com/support/xcode/)
- The latest version of [Xcode](https://developer.apple.com/xcode). It may be possible to [use an older version of Xcode](/xamarin/ios/troubleshooting/questions/old-version-xcode) if your Mac isn't compatible with the latest version.
- An Apple ID. If you don't have an Apple ID already, you can create a new one at https://appleid.apple.com. It's necessary to have an Apple ID for deploying apps to devices.

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

> [!div class="nextstepaction"]
> [Build your first app](first-app.md)
