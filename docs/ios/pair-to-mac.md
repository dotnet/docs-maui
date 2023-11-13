---
title: "Pair to Mac for iOS development"
description: "Learn how to use Pair to Mac to connect Visual Studio 2022 to a Mac build host. This article discusses how to enable remote login on the Mac, connect to the Mac from Visual Studio 2022, and manually add a Mac build host to the Windows machine."
ms.date: 3/2/2023
---

# Pair to Mac for iOS development

Building native iOS applications using .NET Multi-platform App UI (.NET MAUI) requires access to Apple's build tools, which only run on a Mac. Because of this, Visual Studio 2022 must connect to a network-accessible Mac to build .NET MAUI iOS apps.

Visual Studio 2022's Pair to Mac feature discovers, connects to, authenticates with, and remembers Mac build hosts so that you can work productively on Windows.

Pair to Mac enables the following software development workflow:

- You can write .NET MAUI iOS code in Visual Studio 2022.
- Visual Studio 2022 opens a network connection to a Mac build host and uses the build tools on that machine to compile and sign the iOS app.
- There's no need to run a separate application on the Mac – Visual Studio 2022 invokes Mac builds securely over SSH.
- Visual Studio 2022 is notified of changes as soon as they happen. For example, when an iOS device is plugged into the Mac or becomes available on the network, the iOS Toolbar updates instantly.
- Multiple instances of Visual Studio 2022 can connect to the Mac simultaneously.
- It's possible to use the Windows command-line to build iOS apps.

> [!NOTE]
> Before following the instructions in this article, on a Mac, [install Xcode](https://apps.apple.com/us/app/xcode/id497799835?mt=12). Then manually open Xcode, after installation, so that it can add additional components. In addition, you should also install either the latest [Visual Studio 2022 for Mac](https://visualstudio.microsoft.com/vs/mac/) or [Mono](https://www.mono-project.com/download/stable/#download-mac). In addition, if you have a Mac computer with Apple silicon please ensure that Rosetta is installed.
>
> If you would prefer not to install Visual Studio 2022 for Mac, Visual Studio 2022 can automatically configure the Mac build host. However, you must still install and run Xcode, and install Mono.

## Enable remote login on the Mac

To set up the Mac build host, first enable remote login:

1. On the Mac, open **System Preferences** and go to the **Sharing** pane.

1. Check **Remote Login** in the **Service** list.

    :::image type="content" source="media/pair-to-mac/sharing.png" alt-text="Enabling remote login.":::

    Make sure that it's configured to allow access for **All users**, or that your Mac username or group is included in the list of allowed users.

1. If prompted, configure the macOS firewall. If you have set the macOS firewall to block incoming connections, you may need to allow `mono-sgen` to receive incoming connections. An alert appears to prompt you if so.

1. If it's on the same network as the Windows machine, the Mac should now be discoverable by Visual Studio 2022. If the Mac is still not discoverable, try [manually adding a Mac](#manually-add-a-mac).

## Connect to the Mac from Visual Studio 2022

After enabling remote login on the Mac, connect Visual Studio 2022 to the Mac:

1. In Visual Studio 2022, open an existing .NET MAUI project or create a new one.

1. Open the **Pair to Mac** dialog with the **Pair to Mac** button iOS toolbar:

    :::image type="content" source="media/pair-to-mac/ios-toolbar.png" alt-text="The iOS toolbar, with the Pair to Mac button highlighted.":::

    Alternatively, select **Tools > iOS > Pair to Mac**.

    The **Pair to Mac** dialog displays a list of all previously connected and currently available Mac build hosts:

    :::image type="content" source="media/pair-to-mac/pairtomac.png" alt-text="The Pair to Mac dialog.":::

1. Select a Mac in the list and select **Connect**.

1. Enter your username and password. The first time you connect to any particular Mac, you're prompted to enter your username and password for that machine:

    :::image type="content" source="media/pair-to-mac/auth.png" alt-text="Entering a username and password for the Mac.":::

    > [!TIP]
    > When logging in, use your system username.

    Pair to Mac uses these credentials to create a new SSH connection to the Mac. If it succeeds, a key is added to the **authorized_keys** file on the Mac. Subsequent connections to the same Mac will log in automatically.

1. Pair to Mac automatically configures the Mac. Visual Studio 2022 installs or updates pre-requisites on a connected Mac build host as needed. However, Xcode must still be installed manually.

1. Examine the connection status icon. When Visual Studio 2022 is connected to a Mac, that Mac's item in the **Pair to Mac** dialog displays an icon indicating that it's currently connected:

    :::image type="content" source="media/pair-to-mac/connected.png" alt-text="A connected Mac.":::

    There can be only one connected Mac at a time.

    > [!TIP]
    > Right-clicking any Mac in the **Pair to Mac** list brings up a context menu that allows you to **Connect...**, **Forget this Mac**, or **Disconnect**:
    >
    > :::image type="content" source="media/pair-to-mac/contextmenu.png" alt-text="The Pair to Mac context menus.":::
    >
    > If you choose **Forget this Mac**, your credentials for the selected Mac will be forgotten. To reconnect to that Mac, you will need to re-enter your username and password.

If you've successfully paired to a Mac build host, you're ready to build .NET MAUI iOS apps in Visual Studio 2022. For more information, see [Build your first app](~/get-started/first-app.md?pivots=devices-ios).

If you haven't been able to pair a Mac, try [manually adding a Mac](#manually-add-a-mac).

## Manually add a Mac

If you don't see a specific Mac listed in the **Pair to Mac** dialog, add it manually:

1. Open **System Preferences > Sharing > Remote Login** on your Mac to locate your Mac’s IP address:

    :::image type="content" source="media/pair-to-mac/remote-login.png" alt-text="The Mac's IP address in System Preferences > Sharing.":::

    Alternatively, use the command line. In **Terminal**, issue the following command:

    ```zsh
    ipconfig getifaddr en0
    ```

    Depending on your network configuration, you may need to use an interface name other than `en0`, for example, `en1` or `en2`.

1. In Visual Studio 2022's **Pair to Mac** dialog, select **Add Mac...**:

    :::image type="content" source="media/pair-to-mac/addtomac.png" alt-text="The Add Mac button in the Pair to Mac dialog.":::

1. Enter the Mac's IP address and select **Add**:

    :::image type="content" source="media/pair-to-mac/enteripaddress.png" alt-text="Entering the Mac's IP address.":::

1. Enter your username and password for the Mac:

    :::image type="content" source="media/pair-to-mac/auth.png" alt-text="Enter a username and password.":::

   > [!TIP]
   > When logging in, use your system username.

1. Select **Login** to connect Visual Studio 2022 to the Mac over SSH and add it to the list of known machines.

## Enable automatic connection to known Macs

By default, a connection to previously paired Macs won't be established when Visual Studio starts. However, automatic connection to known Macs can be enabled in Visual Studio by navigating to **Tools > Options > Xamarin > iOS Settings** and ensuring that **Enable auto connection to known Macs** is checked:

:::image type="content" source="media/pair-to-mac/enable-auto-connection.png" alt-text="Enable auto connection to known Macs in Visual Studio.":::

After restarting Visual Studio, it will automatically connect to known Macs on each launch.

## Automatic Mac provisioning

Pair to Mac automatically provisions a Mac with the software necessary for building .NET MAUI iOS apps. This includes .NET and various Xcode-related tools (but not Xcode itself).

> [!IMPORTANT]
>
> - Pair to Mac can't install Xcode. You must manually install it on the Mac build host. It's required for .NET MAUI iOS development.
> - Automatic Mac provisioning requires that remote login is enabled on the Mac, and the Mac must be network-accessible to the Windows machine.
> - Automatic Mac provisioning requires sufficient free space on the Mac to install .NET.

In addition, Pair to Mac performs required software installations and updates to the Mac, when Visual Studio 2022 connects to it.

### Xcode tools and license

Pair to Mac will also check to determine whether Xcode has been installed and its license accepted. While Pair to Mac doesn't install Xcode, it does prompt for license acceptance.

In addition, Pair to Mac will install or update various packages distributed with Xcode. The installation of these packages happens quickly and without a prompt.

### Troubleshooting automatic Mac provisioning

If you encounter any trouble using automatic Mac provisioning, take a look at the Visual Studio 2022 IDE logs, stored in **%LOCALAPPDATA%\Xamarin\Logs\17.0**. These logs may contain error messages to help you better diagnose the failure or get support.

## Build iOS apps from the Windows command-line

Pair to Mac supports building .NET MAUI apps from the command line. Navigate to the folder that holds the source of your .NET MAUI iOS app and execute the following command:

```dotnet
dotnet build -f net8.0-ios -p:ServerAddress={macOS build host IP address} -p:ServerUser={macOS username} -p:ServerPassword={macOS password} -p:TcpPort=58181 -p:_DotNetRootRemoteDirectory=/Users/{macOS username}/Library/Caches/Xamarin/XMA/SDKs/dotnet/
```

The parameters passed to `dotnet` in the above example are:

- `ServerAddress` – the IP address of the Mac build host.
- `ServerUser` – the username to use when logging in to the Mac build host. Use your system username rather than your full name.
- `ServerPassword` – the password to use when logging in to the Mac build host.
- `_DotNetRootRemoteDirectory` - the folder on the Mac build host that contains the .NET SDK.

The first time Pair to Mac logs in to a Mac build host from either Visual Studio 2022 or the command-line, it sets up SSH keys. With these keys, future logins won't require a username or password. Newly created keys are stored in **%LOCALAPPDATA%\Xamarin\MonoTouch**.

If the `ServerPassword` parameter is omitted from a command-line build invocation, Pair to Mac attempts to log in to the Mac build host using the saved SSH keys.

For more information about building iOS apps from the Windows command-line, see [Publish an iOS app using the command line](~/ios/deployment/publish-cli.md).
