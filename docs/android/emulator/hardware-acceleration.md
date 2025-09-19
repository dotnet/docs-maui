---
title: "Enable hardware acceleration for the Android emulator (Hyper-V & AEHD)"
description: "Learn how to enable hardware acceleration features to maximize Android emulator performance for a .NET MAUI app."
ms.date: 08/27/2024
ms.topic: how-to
---

# How to enable hardware acceleration with Android emulators (Hyper-V & AEHD)

With Visual Studio, you can easily test and debug your .NET MAUI app for Android in emulators for situations where an Android device isn't available. However, if hardware acceleration isn't available or enabled, the emulator will run very slowly. You can significantly improve the performance of the emulator by enabling hardware acceleration and by using virtual device images that are appropriate for your processor architecture. For more information, see [Configure hardware acceleration for the Android Emulator](https://developer.android.com/studio/run/emulator-acceleration) on developer.android.com.

The emulator provides versatile networking capabilities that can be used for different purposes, including connecting to an emulator running on a Mac from inside a Windows virtual machine (VM). For more information, see [Connect to an Android emulator on a Mac from inside a Windows virtual machine](mac-with-windows-vm.md).

On macOS, the Android emulator uses the built-in Hypervisor.Framework. For more information, see [Hypervisor](https://developer.apple.com/documentation/hypervisor) on developer.apple.com.

## Accelerate Android emulators on Windows

The following virtualization technologies are available for accelerating the Android emulator on Windows:

- The Windows Hypervisor Platform (WHPX). [Hyper-V](/virtualization/hyper-v-on-windows/) is a virtualization feature of Windows that makes it possible to run virtualized computer systems on a physical host computer.
- The Android Emulator hypervisor driver (AEHD).

> [!NOTE]
> The Intel Hardware Accelerated Execution Manager (HAXM) is deprecated from emulator 33.x.x.x, and has been replaced by AEHD on Intel processors. For information about using HAXM on emulator 32.x.x.x and lower, see [Configure VM acceleration using Intel HAXM on Windows](https://developer.android.com/studio/run/emulator-acceleration#vm-windows-haxm-intel) on developer.android.com.

For the best experience on Windows, it's recommended you use WHPX to accelerate the Android emulator. If WHPX isn't available on your computer, then AEHD can be used. The Android emulator automatically uses hardware acceleration if the following criteria are met:

- Hardware acceleration is available and enabled on your development computer.
- The emulator is running a system image created for an **x86-64** or **x86**-based virtual device.

> [!IMPORTANT]
> A Virtual Machine (VM) accelerated emulator can run inside another VM, including Microsoft Dev Box, provided that nested virtualization is enabled in the VM.

### Windows ARM device limitations

The Android emulator currently requires x64 processor architecture and is **not supported on Windows ARM devices**. This limitation affects devices such as:

- Surface Pro X
- Surface Pro 9 (5G/ARM variant)
- Other Windows devices with ARM64 processors (Snapdragon, etc.)

If you're using a Windows ARM device, consider the following alternatives:

- Use a physical Android device for testing via USB debugging. For more information, see [Set up Android device for debugging](~/android/device/setup.md).
- Use cloud-based testing services or remote development environments with x64 architecture.
- Consider using Windows Subsystem for Android (WSA) if available, though this has different limitations and capabilities compared to the Android emulator.

For the latest information about Android emulator requirements, see [Android Emulator requirements](https://developer.android.com/studio/run/emulator#requirements) on developer.android.com.

For information about launching and debugging with the Android emulator, see [Debugging on the Android Emulator](debug-on-emulator.md).

## Accelerate with Hyper-V

Before enabling Hyper-V, read the following section to verify that your computer supports Hyper-V.

### Verify support for Hyper-V

Hyper-V runs on the Windows Hypervisor Platform. To use the Android emulator with Hyper-V, your computer must meet the following criteria to support the Windows Hypervisor Platform:

- Your computer hardware must meet the following requirements:

  - Intel processors with support for Virtualization Technology (VT-x), Extended Page Tables (EPT), and Unrestricted Guest (UG) features. VT-x must be enabled in your computer's BIOS.
  - AMD processors: AMD Ryzen processor recommended. Virtualization or SVM must be enabled in your computer's BIOS.

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
- 64-bit Windows 11 or Windows 10.
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

#### AEHD 2.1 and higher

After installation, confirm that the driver is operating correctly using the following command:

```cmd
sc query aehd
```

If the driver is operating correctly, the status message will include the following information:

```
SERVICE_NAME: aehd
       ...
       STATE              : 4  RUNNING
       ...
```

The following error message means that the virtualization extension isn't enabled in your BIOS or that Hyper-V isn't disabled:

```
SERVICE_NAME: aehd
       ...
       STATE              : 1  STOPPED
       WIN32_EXIT_CODE    : 4294967201 (0xffffffa1)
       ...
```

After AEHD is installed and running, you'll be able to run your accelerated Android emulator.

#### AEHD 2.0 and lower

After installation, confirm that the driver is operating correctly using the following command:

```cmd
sc query gvm
```

If the driver is operating correctly, the status message will include the following information:

```
SERVICE_NAME: gvm
       ...
       STATE              : 4  RUNNING
       ...
```

The following error message means that the virtualization extension isn't enabled in your BIOS or that Hyper-V isn't disabled:

```
SERVICE_NAME: gvm
       ...
       STATE              : 1  STOPPED
       WIN32_EXIT_CODE    : 4294967201 (0xffffffa1)
       ...
```

After AEHD is installed and running, you'll be able to run your accelerated Android emulator.

### Uninstall AEHD

To uninstall AEHD, use the following commands at a command line with administrator privileges:

- AEHD 2.1 and higher

    ```cmd
    sc stop aehd
    sc delete aehd
    ```

- AEHD 2.0 and lower

    ```cmd
    sc stop gvm
    sc delete gvm
    ```

> [!IMPORTANT]
> Shut down any x86 emulators before uninstalling AEHD for AMD.

## Troubleshoot

For help with troubleshooting hardware acceleration issues, see the Android emulator [Troubleshooting](troubleshooting.md#hardware-acceleration-issues) guide.
