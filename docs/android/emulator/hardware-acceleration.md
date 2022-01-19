---
title: "Enable hardware acceleration for the Android emulator (Hyper-V & HAXM)"
description: "When developing a .NET MAUI app, you'll want to use your computer's hardware acceleration features to maximize Android emulator performance."
ms.date: 12/02/2021
---

# Hardware acceleration for better emulator performance (Hyper-V & HAXM)

This article explains how to use your computer's hardware acceleration features to maximize Android emulator performance.

With Visual Studio, you can easily test and debug your .NET MAUI app for Android in situations where an Android device isn't available. However, if hardware acceleration isn't available or enabled, the emulator will run too slow. You can drastically improve the performance of the emulator by enabling hardware acceleration and using special x86 virtual device images.

<!-- TODO: What are the virtualization extensions on the new M1 ARM processors for Mac?
-->

| Scenario                                  | HAXM        | WHPX    | Hypervisor.Framework |
|-------------------------------------------|-------------|---------|----------------------|
| You have an Intel Processor               | X           | X       | X                    |
| You have an AMD Processor                 |             | X       |                      |
| You want to support Hyper-V               |             | X       |                      |
| You want to support nested Virtualization |             | Limited |                      |
| You want to use technologies like Docker  | (with WSL2) | X       | X                    |

<!--
::: zone pivot="windows"
-->

## Accelerating Android emulators on Windows

The following virtualization technologies are available for accelerating the Android emulator:

01. **Microsoft's Hyper-V and the Windows Hypervisor Platform (WHPX)**.

    [Hyper-V](/virtualization/hyper-v-on-windows/) is a virtualization feature of Windows that makes it possible to run virtualized computer systems on a physical host computer.

01. **Intel's Hardware Accelerated Execution Manager (HAXM)**.

    HAXM is a virtualization engine for computers running Intel CPUs.

For the best experience on Windows, it's recommended you use WHPX to accelerate the Android emulator. If WHPX isn't available on your computer, then HAXM can be used. The Android emulator automatically uses hardware acceleration if the following criteria are met:

- Hardware acceleration is available and enabled on your development computer.

- The emulator is running a system image created for an **x86**-based virtual device.

> [!IMPORTANT]
> You can't run a VM-accelerated emulator inside another VM, such as a VM hosted by VirtualBox, VMware, or Docker (unless using WSL2). You must run the Android emulator [directly on your system hardware](https://developer.android.com/studio/run/emulator-acceleration.html#extensions).

For information about launching and debugging with the Android emulator, see [Debugging on the Android Emulator](../deployment/debug-on-emulator.md).

## Accelerating with Hyper-V

Before enabling Hyper-V, read the following section to verify that your computer supports Hyper-V.

### Verifying support for Hyper-V

Hyper-V runs on the Windows Hypervisor Platform. To use the Android emulator with Hyper-V, your computer must meet the following criteria to support the Windows Hypervisor Platform:

- Your computer hardware must meet the following requirements:

  - A 64-bit Intel or AMD Ryzen CPU with Second Level Address Translation (SLAT).
  - CPU support for VM Monitor Mode Extension (VT-c on Intel CPUs).
  - Minimum of 4-GB memory.

- In your computer's BIOS, the following items must be enabled:

  - Virtualization Technology (may have a different label depending on motherboard manufacturer).
  - Hardware Enforced Data Execution Prevention.

- Your computer must be running Windows 11 or Windows 10 Version 1909 or later.

To verify that your computer hardware and software is compatible with Hyper-V, open a command prompt and type the following command:

```cmd
systeminfo
```

If all listed Hyper-V requirements have a value of **Yes**, then your computer can support Hyper-V. For example:

:::image type="content" source="media/hardware-acceleration/win/02-systeminfo-w158.png" alt-text="Example of systeminfo output when checking Hyper-V support for .NET MAUI." lightbox="media/hardware-acceleration/win/02-systeminfo-w158-sml.png":::

### Enabling Hyper-V acceleration

If your computer meets the above criteria, use the following steps to accelerate the Android emulator with Hyper-V:

01. Enter **windows features** in the Windows search box and select **Turn Windows features on or off** in the search results. In the **Windows Features** dialog, enable both **Hyper-V** and **Windows Hypervisor Platform**:

    :::image type="content" source="media/hardware-acceleration/win/03-hyper-v-settings-w158.png" alt-text="Enabling Hyper-V and Windows Hypervisor Platform for .NET MAUI.":::

    After making these changes, reboot your computer.

> [!IMPORTANT]
> On Windows 10 October 2018 Update (RS5) and higher, you only need to enable Hyper-V, as it will use Windows Hypervisor Platform (WHPX) automatically.

01. Make sure that the virtual device you [created in the Android Device Manager](device-manager.md) is an **x86**-based system image. If you use an Arm-based system image, the virtual device won't be accelerated and will run slowly.

After Hyper-V is enabled, you'll be able to run your accelerated Android emulator.

## Accelerating with HAXM

> [!IMPORTANT]
> HAXM is only supported on Intel CPUs.

If your computer doesn't support Hyper-V, you may use HAXM to accelerate the Android emulator. To use HAXM, [disable Device Guard](troubleshooting.md#disabling-device-guard).

### Verifying HAXM support

To determine if your hardware supports HAXM, follow the steps in [Does My Processor Support Intel Virtualization Technology?](https://www.intel.com/content/www/us/en/support/processors/000005486.html). If your hardware supports HAXM, you can check to see if HAXM is already installed by using the following steps:

01. Open a command prompt window and enter the following command:

    ```cmd
    sc query intelhaxm
    ```

01. Examine the output to see if the HAXM process is running. If it is, you should see output listing the `intelhaxm` state as `RUNNING`. For example:

    :::image type="content" source="media/hardware-acceleration/win/05-sc_query-w158.png" alt-text="Output from sc query command when HAXM is available.":::

    If `STATE` isn't set to `RUNNING`, then HAXM isn't installed.

If your computer can support HAXM but HAXM isn't installed, use the steps in the next section to install HAXM.

### Installing HAXM

HAXM install packages for Windows are available from the [Intel Hardware Accelerated Execution Manager](https://github.com/intel/haxm/releases) GitHub releases page. Use the following steps to download and install HAXM:

01. From the Intel website, download the latest [HAXM virtualization engine](https://github.com/intel/haxm/releases) installer for Windows. The advantage of downloading the HAXM installer directly from the Intel website is that you can be assured of using the latest version.

01. Run **intelhaxm-android.exe** to start the HAXM installer. Accept the default values in the installer dialogs.

When you [create a virtual device](device-manager.md), be sure to select an **x86**-based system image. If you use an ARM-based system image, the virtual device will not be accelerated and will run slowly.

## Troubleshooting

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

## Related Links

- [Run Apps on the Android Emulator](https://developer.android.com/studio/run/emulator)
