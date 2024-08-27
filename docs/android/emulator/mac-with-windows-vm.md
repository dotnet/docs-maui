---
title: "Connect to an Android emulator on a Mac from inside a Windows VM"
description: "Learn how to connect to an Android emulator running on a Mac from inside a Windows virtual machine."
ms.date: 08/27/2024
---

# Connect to an Android emulator on a Mac from inside a Windows virtual machine

The Android emulator provides versatile networking capabilities that can be used for different purposes, including connecting to an emulator running on a Mac from inside a Windows virtual machine (VM). This is useful when running Windows in Parallels on a Mac. In this scenario it's required to use the emulator on the Mac since it can't run in Parallels. For information about installing the Android emulator on a Mac, see [Installation](~/get-started/installation.md?tabs=visual-studio-code).

> [!IMPORTANT]
> The address 127.0.0.1 on your development machine corresponds to the emulator's loopback interface.

There are two main approaches for connecting to an Android emulator on a Mac from inside a Windows VM:

1. Using `nc` to perform packet forwarding. For more information, see [Use nc to perform packet forwarding](#use-nc-to-perform-packet-forwarding).
1. Using `ssh` port forwarding. For more information, see [Use ssh port forwarding](#use-ssh-port-forwarding).

In both cases, the Android Debug Bridge (ADB) is used to connect to the emulator. ADB is a command-line tool that's bundled with the Android SDK Platform Tools package, that lets you communicate with a device. The `adb` command facilitates a variety of device actions, including connecting to devices. For more information about `adb`, see [Android Debug Bridge (adb)](https://developer.android.com/tools/adb) on developer.android.com.

## Use nc to perform packet forwarding

To connect to the Android Emulator running on a Mac from a Windows VM, by using packet forwarding, use the following steps:

1. On your Mac, start the Android emulator.
1. On your Mac, open **Terminal**.
1. In **Terminal**, kill the `adb` server:

    ```zsh
    adb kill-server
    ```

1. In **Terminal**, identify the ports used by the emulator:

    ```zsh
    lsof -iTCP -sTCP:LISTEN -P | grep 'emulator\|qemu'

    emulator6 94105 macuser   20u  IPv4 0xa8dacfb1d4a1b51f      0t0  TCP localhost:5555 (LISTEN)
    emulator6 94105 macuser   21u  IPv4 0xa8dacfb1d845a51f      0t0  TCP localhost:5554 (LISTEN)
    ```

    The emulator uses a pair of sequential TCP ports - an even-numbered port for console connections, and an odd-numbered port for `adb` connections. In the output above the emulator listens for `adb` on port 5555 and listens for console connections on port 5554.

1. In **Terminal**, use `nc` to forward inbound TCP packets received externally on port 5555 (or any other port) to the odd-numbered port on the loopback interface, and to forward the outbound packets back the other way:

    ```zsh
    cd /tmp
    mkfifo backpipe
    nc -kl 5555 0<backpipe | nc 127.0.0.1 5555 > backpipe
    ```

    In this example, `127.0.0.1 5555` denotes the odd-numbered port on the loopback interface.

    Provided that the `nc` command keeps running in a Terminal window, packets will be forwarded as expected. Once you've finished using the emulator, you can stop `nc` performing packet forwarding by pressing <kbd>CTRL+C</kbd> in the Terminal window.

1. In your Windows VM, open **Command Prompt**.
1. In **Command Prompt**, connect to the emulator:

    ```cmd
    adb connect ip-address-of-the-mac:5555
    ```

    Replace `ip-address-of-the-mac` in this example with the IP address of your Mac, and 5555 with the port you've used from the previous step.

    > [!NOTE]
    > Command line access to `adb` can be obtained in Visual Studio through the **Tools > Android > Android Adb Command Prompt...** menu item.  

When the connection is completed Visual Studio will display the emulator as a debug target under **Android Local Devices**, which you can use to deploy your app to the emulator.

## Use ssh port forwarding

Provided that **Remote Login** is enabled on your Mac, you can use `ssh` port forwarding to connect to the emulator.

> [!IMPORTANT]
> `ssh` port forwarding requires you to have installed a SSH client in your Windows VM. One option is to install [Git for Windows](https://git-for-windows.github.io/). The `ssh` command will then be available in the **Git Bash** command prompt.

To connect to the Android Emulator running on a Mac from a Windows virtual machine, by using `ssh` port forwarding, use the following steps:

1. On your Mac, start the Android emulator.
1. On your Mac, open **Terminal**.
1. In **Terminal**, kill the `adb` server:

    ```zsh
    adb kill-server
    ```

1. In **Terminal**, identify the ports used by the emulator:

    ```zsh
    lsof -iTCP -sTCP:LISTEN -P | grep 'emulator\|qemu'

    emulator6 94105 macuser   20u  IPv4 0xa8dacfb1d4a1b51f      0t0  TCP localhost:5555 (LISTEN)
    emulator6 94105 macuser   21u  IPv4 0xa8dacfb1d845a51f      0t0  TCP localhost:5554 (LISTEN)
    ```

    The emulator uses a pair of sequential TCP ports - an even-numbered port for console connections, and an odd-numbered port for `adb` connections. In the output above the emulator listens for `adb` on port 5555 and listens for console connections on port 5554.

1. In your Windows VM, open **Command Prompt**.
1. In **Command Prompt**, run `ssh` to setup two-way port forwarding between a local port on Windows and the odd-numbered emulator port on the Mac's loopback interface:

    ```cmd
    ssh -L localhost:15555:127.0.0.1:5555 mac-username@ip-address-of-the-mac
    ```

    In this example, `localhost:15555` denotes the local port on Windows and `127.0.0.1 5555` denotes the odd-numbered port on the loopback interface.

    Replace `mac-username` with your Mac username, which can be obtained with the `whoami` command, and `ip-address-of-the-mac` with the IP address of your Mac.

1. In **Command Prompt**, connect to the emulator using the local port:

    ```cmd
    adb connect localhost:15555
    ```

    In this example, `localhost:15555` denotes the local port on Windows

    > [!NOTE]
    > Command line access to `adb` can be obtained in Visual Studio through the **Tools > Android > Android Adb Command Prompt...** menu item.  

    <!-- > [!CAUTION]
    > If you use port 5555 for the local port, `adb` will think that the emulator is running locally on Windows. This doesn't cause any issues in Visual Studio, but in Visual Studio for Mac it causes the app to exit immediately after launch. -->

When the connection is completed Visual Studio will display the emulator as a debug target under **Android Local Devices**, which you can use to deploy your app to the emulator.
