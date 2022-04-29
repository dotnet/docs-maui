---
title: "Build an iOS app on macOS with .NET CLI"
description: "Learn how to create and run a .NET MAUI app on iOS using .NET CLI on macOS."
ms.date: 04/29/2022
---

# Build an iOS app on macOS with .NET CLI

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

In this tutorial, you'll learn how to create and run a .NET Multi-platform App UI (.NET MAUI) app on iOS using .NET CLI on macOS:

> [!NOTE]
> Visual Studio 2022 for Mac, with .NET MAUI support, is currently available as an early [preview release](https://visualstudio.microsoft.com/vs/mac/preview/).

1. To create .NET MAUI apps, you'll need to download and run the [installer](https://github.com/dotnet/installer/blob/main/README.md#installers-and-binaries) for the latest .NET 6 runtime.

    You'll also need to download and install the latest version of [Xcode 13](https://xcodereleases.com), which is also available from the App Store app on your Mac.

1. On your Mac, open **Terminal** and check that you have the latest .NET 6 runtime installed:

    ```zsh
    dotnet --version
    ```

1. In **Terminal**, install the latest public build of .NET MAUI:

    ```zsh
    sudo dotnet workload install maui --source https://api.nuget.org/v3/index.json
    ```

    This command will install the latest released version of .NET MAUI, including the required platform SDKs.

1. In **Terminal**, create a new .NET MAUI app using .NET CLI:

    ```zsh
    dotnet new maui -n "MyMauiApp"
    ```

1. In **Terminal**, change directory to *MyMauiApp*, and build and run the app:

    ```zsh
    cd MyMauiApp
    dotnet build -t:Run -f net6.0-ios
    ```

    The `dotnet build` command will restore the project the dependencies, build the app, and launch it in the default simulator.

1. In the default simulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/cli/default-simulator.png" alt-text=".NET MAUI app running in the default iOS simulator.":::

## Launch the app on a specific simulator

A .NET MAUI iOS app can be launched on a specific iOS simulator by providing its unique device id (UDID):

1. On your Mac, open *Xcode*, select the **Windows > Devices and Simulators** menu item, and then the **Simulators** tab.

    :::image type="content" source="media/cli/xcode-simulators.png" alt-text="Screenshot of Xcode simulators tab.":::

1. Right-click on your chosen simulator, and select **Copy Identifier** to copy the UDID to the clipboard.

    :::image type="content" source="media/cli/xcode-copy-identifier.png" alt-text="Screenshot of copy identifier menu option in Xcode.":::

    Alternatively, you can retrieve a list of UDID values by executing the `simctl list` command:

    ```zsh
    /Applications/Xcode.app/Contents/Developer/usr/bin/simctl list
    ```

1. In **Terminal**, build the app and run it on your chosen simulator by specifying the `_DeviceName` MSBuild property using the `-p` [MSBuild option](/dotnet/core/tools/dotnet-build#msbuild):

    ```zsh
    dotnet build -t:Run -f net6.0-ios -p:_DeviceName=:v2:udid=insert_UDID_here
    ```

    For example, use the following command to build the app and run it on the iPhone 13 Pro simulator:

    ```zsh
    dotnet build -t:Run -f net6.0-ios -p:_DeviceName=:v2:udid=E25BBE37-69BA-4720-B6FD-D54C97791E79
    ```

1. In your chosen simulator, press the **Click me** button several times and observe that the count of the number of button clicks is incremented.

    :::image type="content" source="media/cli/chosen-simulator.png" alt-text=".NET MAUI app running in iPhone 13 Pro simulator.":::
