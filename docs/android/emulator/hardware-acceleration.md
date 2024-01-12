---
title: "Enable hardware acceleration for the Android emulator (Hyper-V & AEHD)"
description: "Learn how to enable hardware acceleration features to maximize Android emulator performance for a .NET MAUI app."
ms.date: 05/19/2023
ms.topic: how-to
---

# How to enable hardware acceleration with Android emulators (Hyper-V & AEHD)

With Visual Studio, you can easily test and debug your .NET MAUI app for Android in emulators for situations where an Android device isn't available. However, if hardware acceleration isn't available or enabled, the emulator will run very slowly. You can significantly improve the performance of the emulator by enabling hardware acceleration and by using virtual device images that are appropriate for your processor architecture. For more information, see [Configure hardware acceleration for the Android Emulator](https://developer.android.com/studio/run/emulator-acceleration) on developer.android.com.

The emulator provides versatile networking capabilities that can be used for different purposes, including connecting to an emulator running on a Mac from inside a Windows virtual machine (VM). For more information, see [Connect to an Android emulator on a Mac from inside a Windows virtual machine](mac-with-windows-vm.md).

## Accelerate Android emulators on Windows

The following virtualization technologies are available for accelerating the Android emulator:

- The Windows Hypervisor Platform (WHPX). [Hyper-V](/virtualization/hyper-v-on-windows/) is a virtualization feature of Windows that makes it possible to run virtualized computer systems on a physical host computer.
- The Android Emulator hypervisor driver (AEHD).

> [!NOTE]
> The Intel Hardware Accelerated Execution Manager (HAXM) is deprecated from emulator 33.x.x.x, and has been replaced by AEHD on Intel processors. For information about using HAXM on emulator 32.x.x.x and lower, see [Configure VM acceleration using Intel HAXM on Windows](https://developer.android.com/studio/run/emulator-acceleration#vm-windows-haxm-intel) on developer.android.com.

For the best experience on Windows, it's recommended you use WHPX to accelerate the Android emulator. If WHPX isn't available on your computer, then AEHD can be used. The Android emulator automatically uses hardware acceleration if the following criteria are met:

- Hardware acceleration is available and enabled on your development computer.
- The emulator is running a system image created for an **x86-64** or **x86**-based virtual device.

> [!IMPORTANT]
> A Virtual Machine (VM) accelerated emulator can run inside another VM, including Microsoft Dev Box, provided that nested virtualization is enabled in the VM.

For information about launching and debugging with the Android emulator, see [Debugging on the Android Emulator](debug-on-emulator.md).

## Accelerate with Hyper-V

Before enabling Hyper-V, read the following section to verify that your computer supports Hyper-V.

### Verify support for Hyper-V

Hyper-V runs on the Windows Hypervisor Platform. To use the Android emulator with Hyper-V, your computer must meet the following criteria to support the Windows Hypervisor Platform:

- Your computer hardware must meet the following requirements:

  - A 64-bit Intel or AMD Ryzen CPU with Second Level Address Translation (SLAT).
  - CPU support for VM Monitor Mode Extension (VT-c on Intel CPUs).
  - Minimum of 4-GB memory.

- In your computer's BIOS, the following items must be enabled:

  - Virtualization Technology (may have a different label depending on motherboard manufacturer).
  - Hardware Enforced Data Execution Prevention.

- Your computer must be running the Enterprise, Pro, or Education versions of Windows 11 or Windows 10 Version 1909 or later.

To verify that your computer hardware and software is compatible with Hyper-V, open a command prompt and type the following command:

```cmd
systeminfo
```

If all listed Hyper-V requirements have a value of **Yes**, then your computer can support Hyper-V. For example:

:::image type="content" source="media/hardware-acceleration/win/systeminfo-small.png" alt-text="Example of systeminfo output when checking Hyper-V support for .NET MAUI." lightbox="media/hardware-acceleration/win/systeminfo.png":::

If the Hyper-V result indicates that a hypervisor is currently running, Hyper-V is already enabled.

> [!IMPORTANT]
> If Windows is running inside a virtual machine, nested virtualization must be enabled in the host hypervisor.

### Enable Hyper-V acceleration in Windows

If your computer meets the above criteria, use the following steps to accelerate the Android emulator with Hyper-V:

1. Enter **windows features** in the Windows search box and select **Turn Windows features on or off** in the search results. In the **Windows Features** dialog, enable both **Hyper-V** and **Windows Hypervisor Platform**:

    :::image type="content" source="media/hardware-acceleration/win/windows-features.png" alt-text="Enabling Hyper-V and Windows Hypervisor Platform for .NET MAUI.":::

    After making these changes, reboot your computer.

    > [!IMPORTANT]
    > On Windows 10 October 2018 Update (RS5) and higher, you only need to enable Hyper-V, as it will use Windows Hypervisor Platform (WHPX) automatically.

1. Make sure that the virtual device you [created in the Android Device Manager](device-manager.md) is an **x86-64** or **x86**-based system image. If you use an Arm-based system image, the virtual device won't be accelerated and will run slowly.

After Hyper-V is enabled, you'll be able to run your accelerated Android emulator.

## Accelerate with AEHD

If your computer doesn't support Hyper-V, you should use AEHD to accelerate the Android emulator. Before you can install and use AEHD, read the following section to verify that your computer supports AEHD.

### Verify support for AEHD

Your computer must meet the following criteria to support AEHD:

- An Intel or AMD processor with virtualization extension, which must be enabled in your BIOS.
- 64-bit Windows 11, Windows 10, Windows 8, or Windows 7.
- Hyper-V must be turned off.

> [!NOTE]
> Several features in Windows enable Hyper-V implicitly. For more information, see [Double-check when disabling Hyper-V](https://developer.android.com/studio/run/emulator-acceleration#disable-hyper-v) on developer.android.com.

### Enable AEHD acceleration in Windows

If your computer meets the above criteria, use the following steps to accelerate the Android emulator with AEHD:

1. In Visual Studio, select the **Tools > Android > Android SDK Manager...** menu item.
1. In the **Android SDKs and Tools** window, select the **Tools** tab.
1. In the **Tools tab**, expand **Extras**, tick the checkbox for the **Android Emulator Hypervisor Driver (installer)** item, and then select the **Apply Changes** button:

    :::image type="content" source="media/hardware-acceleration/win/aehd.png" alt-text="Installing AEHD through the Android SDK manager in Visual Studio.":::

    > [!NOTE]
    > Alternatively, AEHD can be downloaded and installed from [GitHub](https://github.com/google/android-emulator-hypervisor-driver/releases). After unpacking the driver package, run `silent_install.bat` at a command line with administrator privileges.

1. Make sure that the virtual device you [created in the Android Device Manager](device-manager.md) is an **x86-64** or **x86**-based system image. If you use an Arm-based system image, the virtual device won't be accelerated and will run slowly.

After installation, confirm that the driver is operating correctly using the following command:

```cmd
sc query aehd
```

After AEHD is installed and running, you'll be able to run your accelerated Android emulator.

## Troubleshoot

For help with troubleshooting hardware acceleration issues, see the Android emulator [Troubleshooting](troubleshooting.md#hardware-acceleration-issues) guide.

<!--

TODO: The Mac stuff hasn't been rewritten/touched.

::: zone-end
::: zone pivot="macos"

## Accelerating Android emulators on macOS

The following virtualization technologies are available for accelerating the Android emulator:

1. **Apple's Hypervisor Framework**. [Hypervisor](https://developer.apple.com/documentation/hypervisor) is a feature of macOS 10.10 and later that makes it possible to run virtual machines on a Mac.

2. **Intel's Hardware Accelerated Execution Manager (HAXM)**. [HAXM](https://software.intel.com/articles/intel-hardware-accelerated-execution-manager-intel-haxm) is a virtualization engine for computers running Intel CPUs.

It is recommended that you use the Hypervisor Framework to accelerate the Android emulator. If the Hypervisor Framework isn't available on your Mac, then HAXM can be used. The Android emulator will automatically make use of hardware acceleration if the following criteria are met:

- Hardware acceleration is available and enabled on the development computer.

- The emulator is running a system image created for an **x86**-based virtual device.

> [!IMPORTANT]
>
> You can't run a VM-accelerated emulator inside another VM, such as a VM hosted by VirtualBox, VMware, or Docker. You must run the Android emulator [directly on your system hardware](https://developer.android.com/studio/run/emulator-acceleration.html#extensions).

For information about launching and debugging with the Android emulator, see [Debugging on the Android Emulator](~/android/deploy-test/debugging/debug-on-emulator.md).

## Accelerating with the Hypervisor Framework

To use the Android emulator with the Hypervisor Framework, your Mac must meet the following criteria:

- Your Mac must be running macOS 10.10 or later.

- Your Mac's CPU must be able to support the Hypervisor Framework.

If your Mac meets these criteria, the Android emulator will automatically use the Hypervisor Framework for acceleration. If you aren't sure if Hypervisor Framework is supported on your Mac, see the [Troubleshooting guide](troubleshooting.md?tabs=vsmac#hypervisor-issues) for ways to verify that your Mac supports Hypervisor.

If the Hypervisor Framework isn't supported by your Mac, you can use HAXM to accelerate the Android emulator (described next).

## Accelerating with HAXM

If your Mac doesn't support the Hypervisor framework (or you're using a version of macOS earlier than 10.10), you can use **Intel's Hardware Accelerated Execution Manager** ([HAXM](https://software.intel.com/articles/intel-hardware-accelerated-execution-manager-intel-haxm)) to speed up the Android emulator.

Before using the Android emulator with HAXM for the first time, it's a good idea to verify that HAXM is installed and available for the Android emulator to use.

### Verifying HAXM support

You can check to see if HAXM is already installed by using the following steps:

01. Open a Terminal and enter the following command:

    ```bash
    ~/Library/Developer/Xamarin/android-sdk-macosx/tools/emulator -accel-check
    ```

    This command assumes that the Android SDK is installed at the default location of **~/Library/Developer/Xamarin/android-sdk-macosx**; if not, modify the above path for the location of the Android SDK on your Mac.

01. If HAXM is installed, the above command will return a message similar to the following result:

    > HAXM version 7.2.0 (3) is installed and usable.

    If HAXM is *not* installed, a message similar to the following output is returned:

    > HAXM is not installed on this machine (/dev/HAX is missing).

If HAXM isn't installed, use the steps in the next section to install HAXM.

### Installing HAXM

HAXM installation packages for macOS are available from the [Intel Hardware Accelerated Execution Manager](https://software.intel.com/android/articles/intel-hardware-accelerated-execution-manager) page. Use the following steps to download and install HAXM:

01. From the Intel website, download the latest [HAXM virtualization engine](https://software.intel.com/android/articles/intel-hardware-accelerated-execution-manager/) installer for macOS.

01. Run the HAXM installer. Accept the default values in the installer dialogs.

## Troubleshooting

For help with troubleshooting hardware acceleration issues, see the Android emulator [Troubleshooting](troubleshooting.md?tabs=vsmac#accel-issues-mac) guide.

::: zone-end
-->
