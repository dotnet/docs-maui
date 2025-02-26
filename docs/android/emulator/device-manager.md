---
title: "Managing Virtual Devices with the Android Device Manager"
description: "This article explains how to use the Android Device Manager to create and configure Android Virtual Devices (AVDs) that emulate physical Android devices, for .NET MAUI. You can use these virtual devices to run and test your app without having to rely on a physical device"
ms.date: 08/27/2024
no-loc: ["user.config"]
---

# Managing virtual devices with the Android Device Manager

This article explains how to use the Android Device Manager to create and configure Android Virtual Devices (AVDs) that emulate physical Android devices. You can use these virtual devices to run and test your app without having to rely on a physical device. For more information about Android virtual devices, see [Create and manage virtual devices](https://developer.android.com/studio/run/managing-avds) on developer.android.com.

> [!IMPORTANT]
> Enable hardware acceleration for the Android devices. For more information, see [Hardware Acceleration for Emulator Performance](hardware-acceleration.md).

<!--
::: zone pivot="windows"
-->

## Android Device Manager on Windows

You use the Android Device Manager to create and configure an Android Virtual Devices (AVD) that run in the [Android Emulator](debug-on-emulator.md). Each AVD is an emulator configuration that simulates a physical Android device. This makes it possible to run and test your app in a variety of configurations that simulate different physical Android devices.

:::image type="content" source="media/device-manager/win/01-devices-dialog-sml.png" alt-text="Screenshot of the Android Device Manager in the Devices tab." lightbox="media/device-manager/win/01-devices-dialog.png":::

## Requirements

To use the Android Device Manager, you'll need the following items:

- Visual Studio 2022: Community, Professional, and Enterprise editions are supported.

- The **Android SDK API Level 30** or later. Be sure to install the Android SDK at its default location if it isn't already installed: _C:\\Program Files (x86)\\Android\\android-sdk_.

- The following packages must be installed:

  - **Android SDK Tools 5.0** or later
  - **Android SDK Platform-Tools 31.0.3** or later
  - **Android SDK Build-Tools 30.0.2** or later
  - **Android Emulator 30.8.4** or later

  These packages should be displayed with **Installed** status as seen in the following screenshot:

  :::image type="content" source="media/device-manager/win/02-sdk-tools.png" alt-text="Installing Android SDK Tools.":::

When you install the **.NET Multi-Platform App UI development** workload in Visual Studio, everything is installed for you. For more information on setting up .NET MAUI with Visual Studio, see [Build your first app](../../get-started/first-app.md).

## Open the device manager

Open the Android Device Manager in Visual Studio from the **Tools** menu by pressing **Tools > Android > Android Device Manager**:

:::image type="content" source="media/device-manager/win/03-tools-menu.png" alt-text="Launching the Device manager from the Tools menu.":::

## Main screen

When you run the Android Device Manager, it presents a screen that displays all currently configured virtual devices. For each virtual device, the **Name**, **OS** (Android Version), **Processor**, **Memory** size, and screen **Resolution** are displayed:

When you select a device in the list, the **Start** button appears on the right. Press the **Start** button to launch the emulator with this virtual device. If the emulator is running with the selected virtual device, the **Start** button changes to a **Stop** button that you can use to halt the emulator.

### Create a new device

To create a new device, press the **New** button:

:::image type="content" source="media/device-manager/win/08-new-button.png" alt-text="New button for creating a new device.":::

The **New Device** window is displayed. To configure the device, follow these steps:

01. Give the device a new name. In the following example, the new device is named _Pixel 3a - API 31_.

    :::image type="content" source="media/device-manager/win/09-device-props-sml.png" alt-text="New Device screen of the Device Manager." lightbox="media/device-manager/win/09-device-props.png":::

01. Select a physical device to emulate by selecting a device in the **Base Device** box.
01. Select a processor type for this virtual device with the **Processor** box.

    It's recommended that you choose **x86_64** and enable [hardware acceleration](hardware-acceleration.md).

01. Select the Android version (API level) with the **OS** box.

    If you select an Android API level that has not yet been installed, the Device Manager will display **A new device will be downloaded** message at the bottom of the screen &ndash; it will download and install the necessary files as it creates the new virtual device.

01. If you want to include Google Play Services APIs in your virtual device, select the **Google APIs** option. To include the Google Play Store app on the virtual device, select the **Google Play Store** option

    > [!NOTE]
    > Google Play Store images are available only for some base device types such as Pixel, Pixel 2, Pixel 3, and Nexus 5. This is indicated by the text `(+ Store)` in the image name.

01. Use the property list to change some of the most commonly modified properties. To make changes to properties, see [Editing Android Virtual Device Properties](device-properties.md).

01. Add any additional properties that you need to explicitly set with the **Add Property** box at the bottom of the window:

    :::image type="content" source="media/device-manager/win/16-add-property-menu.png" alt-text="Add Property pull-down menu.":::

    You can also define a custom property by selecting **Custom...**.

01. Press the **Create** button to create the new device:

    :::image type="content" source="media/device-manager/win/17-create-button.png" alt-text="Create button.":::

    You might get a **License Acceptance** screen when you create the device. Select **Accept** if you agree to the license terms.

01. The Android Device Manager adds the new device to the list of installed virtual devices while displaying a **Creating** progress indicator during device creation:

    :::image type="content" source="media/device-manager/win/19-creating-the-device.png" alt-text="Creation progress Indicator.":::

01. When the creation process is complete, the new device is shown in the list of installed virtual devices with a **Start** button, ready to launch

### Edit device

To edit an existing virtual device, select the device and then press the **Edit** button:

:::image type="content" source="media/device-manager/win/21-edit-button.png" alt-text="Edit button for modifying a device.":::

Pressing **Edit** displays the **Device Editor** window for the selected virtual device.

The **Device Editor** window lists the properties of the virtual device under the **Property** column, with the corresponding values of each property in the **Value** column. When you select a property, a detailed description of that property is displayed on the right.

To change a property, edit its value in the **Value** column. For example, in the following screenshot the `hw.lcd.density` property is being changed to **240**:

:::image type="content" source="media/device-manager/win/23-device-editing-sml.png" alt-text="Device Editing example." lightbox="media/device-manager/win/23-device-editing.png":::

After you've made the necessary configuration changes, press the **Save** button. For more information about changing virtual device properties, see [Editing Android Virtual Device Properties](device-properties.md).

### Additional options

Additional options for working with devices are available from the **Additional Options** (&hellip;) pull-down menu:

:::image type="content" source="media/device-manager/win/24-overflow-menu.png" alt-text="Location of additional options menu.":::

The additional options menu contains the following items:

- **Duplicate and Edit** &ndash; Duplicates the currently selected device and opens it in the **New Device** screen with a new name that's similar to the existing device. For example, selecting **Pixel 3a - API 31** and pressing **Duplicate and Edit** appends a counter to the name: **Pixel 3a - API 31 (1)**.

- **Start with Factory Defaults** &ndash; Starts the device with a cold boot.

- **Start with Kernel Logs** &ndash; Starts the emulator and opens up kernel logs directory.

- **Download System Image** &ndash; Downloads the Android OS system image for the device, if it's not already downloaded.

- **Reveal in Explorer** &ndash; Opens Windows Explorer and navigates to the folder that holds the files for the virtual device.

- **Repair** &ndash; Initiates a repair on the device.

- **Factory Reset** &ndash; Resets the selected device to its default settings, erasing any user changes made to the internal state of the device while it was running. This action also erases the current [Fast Boot](debug-on-emulator.md#fast-boot) snapshot if it exists. This change doesn't alter modifications that you make to the virtual device during creation and editing. A dialog box will appear with the reminder that this reset cannot be undone &ndash; press **Factory Reset** to confirm the reset.

- **Delete** &ndash; Permanently deletes the selected virtual device. A dialog box will appear with the reminder that deleting a device cannot be undone. Press **Delete** if you are certain that you want to delete the device.

## Troubleshooting

The following sections explain how to diagnose and work around problems that may occur when using the Android Device Manager to configure virtual devices.

<!-- uncomment if applicable later

### Android SDK in non-standard location

Typically, the Android SDK is installed at _C:\\Program Files (x86)\\Android\\android-sdk_. If the SDK isn't installed at this location, you may get this error when you launch the Android Device Manager:

:::image type="content" source="media/device-manager/win/29-sdk-error.png" alt-text="Android SDK instance error.":::

To work around this problem, do the following:

01. Open the _%LOCALAPPDATA%\\Xamarin\\Logs\\XamarinDeviceManager_ folder.

    The `%LOCALAPPDATA%` value is an environment variable that typically points to the _C:\\Users\\{your user name}\\AppData\\Local_ folder.

01. Open one of the log files and search for an entry describing **Config file path**. For example:

    :::image type="content" source="media/device-manager/win/31-config-file-path.png" alt-text="Config file path in log file.":::

01. Navigate to this folder and open the _user.config_ in a text editor.

01. In _user.config_, find the `<UserSettings>` element and add an **AndroidSdkPath** attribute to it. Set this attribute to the path where the Android SDK is installed on your computer and save the file. For example, `<UserSettings>` would look like the following if the Android SDK was installed at _C:\\Programs\\Android\\SDK_:

    TODO: Does visual studio handle this some how now?

    ```xml
    <UserSettings SdkLibLastWriteTimeUtcTicks="636409365200000000" AndroidSdkPath="C:\Programs\Android\SDK" />
    ```

After making this change to **user.config**, you should be able to launch the Android Device Manager.

-->

### Wrong version of Android SDK Tools

If you have the wrong Android SDK tools installed, you may see this error dialog on launch:

:::image type="content" source="media/device-manager/win/32-sdk-instance-error.png" alt-text="Screenshot shows the Android SDK instance error dialog box.":::

If you see that error dialog, press **Open SDK Manager** to open the Android SDK Manager. In the Android SDK Manager, go to the **Tools** tab and install the following packages:

- **Android SDK Command-line Tools 5.0** or later
- **Android SDK Platform-Tools 31.0.3** or later
- **Android SDK Build-Tools 30.0.3** or later

### Snapshot disables Wi-Fi on Android Oreo

If you've an AVD configured for Android Oreo with simulated Wi-Fi access, restarting the AVD after a snapshot may cause Wi-Fi access to become disabled.

To work around this problem,

01. Open the **Android Device Manager**.

01. Select the AVD in the Android Device Manager.

01. From the **Additional Options** (&hellip;) menu, select **Reveal in Explorer**.

01. Navigate to the **snapshots > default_boot** folder.

01. Delete the _snapshot.pb_ file:

    :::image type="content" source="media/device-manager/win/33-delete-snapshot.png" alt-text="Location of the snapshot.pb file.":::

01. Restart the AVD.

After these changes are made, the AVD will restart in a state that allows Wi-Fi to work again.
