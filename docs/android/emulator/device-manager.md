---
title: "Managing Virtual Devices with the Android Device Manager"
description: "This article explains how to use the Android Device Manager to create and configure Android Virtual Devices (AVDs) that emulate physical Android devices, for .NET MAUI. You can use these virtual devices to run and test your app without having to rely on a physical device"
ms.date: 02/23/2022
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

If you have the wrong Android SDK tools installed, installed, you may see this error dialog on launch:

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

<!--

TODO: The Mac stuff hasn't been rewritten/touched.

::: zone-end
::: zone pivot="macos"

> [!NOTE]
> If you are using a Mac with an Apple chip, such as the M1, you'll need to install the [Android Emulator for M1 preview from GitHub](https://github.com/google/android-emulator-m1-preview/releases).

## Android Device Manager on macOS

This article explains how to use the Android Device Manager to create, duplicate, customize, and launch Android virtual devices.

:::image type="content" source="media/device-manager/mac/01-devices-dialog-sml.png" alt-text="Screenshot of the Android Device Manager in the Devices tab." lightbox="media/device-manager/mac/01-devices-dialog.png":::

You use the Android Device Manager to create and configure *Android Virtual Devices* (AVDs) that run in the [Android Emulator](debug-on-emulator.md). Each AVD is an emulator configuration that simulates a physical Android device. This makes it possible to run and test your app in a variety of configurations that simulate different physical Android devices.

## Requirements

To use the Android Device Manager, you'll need the following items:

- Visual Studio for Mac 7.6 or later.

- The Android SDK must be installed (see [Setting up the Android SDK for Xamarin.Android](~/android/get-started/installation/android-sdk.md)).

- The following packages must be installed (via the [Android SDK Manager](~/android/get-started/installation/android-sdk.md)):

  - **SDK tools version 26.1.1** or later
  - **Android SDK Platform-Tools 28.0.1** or later
  - **Android SDK Build-Tools 26.0.3** or later

  These packages should be displayed with **Installed** status as seen in the following screenshot:

  :::image type="content" source="media/device-manager/mac/02-sdk-tools-sml.png" alt-text="Installing Android SDK Tools." lightbox="media/device-manager/mac/02-sdk-tools.png":::

## Launching the device manager

Launch the Android Device Manager by pressing **Tools > Device Manager**:

:::image type="content" source="media/device-manager/mac/03-tools-menu-sml.png" alt-text="Launching the Device manager from the Tools menu." lightbox="media/device-manager/mac/03-tools-menu.png":::

If the following error dialog is presented on launch, see the [Troubleshooting](#troubleshooting) section for workaround instructions:

:::image type="content" source="media/device-manager/mac/04-sdk-instance-error.png" alt-text="Screenshot shows the Android SDK instance error dialog box for troubleshooting information.":::

## Main screen

When you first launch the Android Device Manager, it presents a screen that displays all currently-configured virtual devices. For each virtual device, the **Name**, **OS** (Android Version), **Processor**, **Memory** size, and screen **Resolution** are displayed:

:::image type="content" source="media/device-manager/mac/05-devices-list-sml.png" alt-text="List of installed devices and their parameters." lightbox="media/device-manager/mac/05-devices-list.png":::

When you select a device in the list, the **Play** button appears on the right. You can press the **Play** button to launch the emulator with this virtual device:

:::image type="content" source="media/device-manager/mac/06-start-button-sml.png" alt-text="Play button for a device image." lightbox="media/device-manager/mac/06-start-button.png":::

After the emulator starts with the selected virtual device, the **Play** button changes to a **Stop** button that you can use to halt the emulator:

:::image type="content" source="media/device-manager/mac/07-stop-button-sml.png" alt-text="Stop button for the running device." lightbox="media/device-manager/mac/07-stop-button.png":::

When you stop the emulator, you may get a prompt asking if you want to save the current state for the next quick boot:

:::image type="content" source="media/device-manager/mac/08-save-for-quick-boot-m76.png" alt-text="Save current state for Quick Boot dialog.":::

Saving the current state will make the emulator boot faster when this virtual device is launched again. For more information about Quick Boot, see [Quick Boot](debug-on-emulator.md#quick-boot).

### New device

To create a new device, press the **New Device** button (located in the upper left-hand area of the screen):

:::image type="content" source="media/device-manager/mac/09-new-button-sml.png" alt-text="New button for creating a new device." lightbox="media/device-manager/mac/09-new-button.png":::

Pressing **New Device** launches the **New Device** screen:

:::image type="content" source="media/device-manager/mac/10-new-device-editor-sml.png" alt-text="New Device screen of the Device Manager." lightbox="media/device-manager/mac/10-new-device-editor.png":::

Use the following steps to configure a new device in the **New Device** screen:

01. Give the device a new name. In the following example, the new device is named **Pixel_API_27**:

    :::image type="content" source="media/device-manager/mac/11-device-name-m76-sml.png" alt-text="Naming the new device." lightbox="media/device-manager/mac/11-device-name-m76.png":::

01. Select a physical device to emulate by pressing the **Base Device** pull-down menu:

    :::image type="content" source="media/device-manager/mac/12-device-menu-m76-sml.png" alt-text="Selecting the physical device to emulate." lightbox="media/device-manager/mac/12-device-menu-m76.png":::

01. Select a processor type for this virtual device by pressing the **Processor** pull-down menu. Selecting **x86** will provide the best performance because it enables the emulator to take advantage of [hardware acceleration](hardware-acceleration.md). The **x86_64** option will also make use of hardware acceleration, but it runs slightly slower than **x86** (**x86_64** is normally used for testing 64-bit apps):

    :::image type="content" source="media/device-manager/mac/13-processor-type-menu-m76-sml.png" alt-text="Selecting the processor type." lightbox="media/device-manager/mac/13-processor-type-menu-m76.png":::

01. Select the Android version (API level) by pressing the **OS** pull-down menu. For example, select **Oreo 8.1 - API 27** to create a virtual device for API level 27:

    :::image type="content" source="media/device-manager/mac/14-android-screenshot-m76-sml.png" alt-text="Selecting the Android version." lightbox="media/device-manager/mac/14-android-screenshot-m76.png":::

    If you select an Android API level that has not yet been installed, the Device Manager will display **A new device will be downloaded** message at the bottom of the screen &ndash; it will download and install the necessary files as it creates the new virtual device:

    :::image type="content" source="media/device-manager/mac/15-automatic-download-m76.png" alt-text="A new device image will be downloaded.":::

01. If you want to include Google Play Services APIs in your virtual device, enable the **Google APIs** option. To include the Google Play Store app, enable the **Google Play Store** option:

    :::image type="content" source="media/device-manager/mac/16-google-play-services-m76-sml.png" alt-text="Selecting Google Play Services and Google Play Store." lightbox="media/device-manager/mac/16-google-play-services-m76.png":::

    Note that Google Play Store images are available only for some base device types such as Pixel, Pixel 2, Nexus 5, and Nexus 5X.

01. Edit any properties that you need to modify. To make changes to properties, see [Editing Android Virtual Device Properties](device-properties.md).

01. Add any additional properties that you need to explicitly set. The **New Device** screen lists only the most commonly-modified properties, but you can press the **Add Property** pull-down menu (at the bottom) to add additional properties:

    :::image type="content" source="media/device-manager/mac/17-add-property-menu-m76-sml.png" alt-text="Add Property pull-down menu." lightbox="media/device-manager/mac/17-add-property-menu-m76.png":::

    You can also define a custom property by pressing **Custom...** at the top of this property list.

01. Press the **Create** button (lower right-hand corner) to create the new device:

    :::image type="content" source="media/device-manager/mac/18-create-button-m76.png" alt-text="Create button.":::

01. The Android Device Manager adds the new device to the list of installed virtual devices while displaying a **Creating** progress indicator during device creation:

    :::image type="content" source="media/device-manager/mac/19-creating-the-device-m76-sml.png" alt-text="Creation progress indicator." lightbox="media/device-manager/mac/19-creating-the-device-m76.png":::

01. When the creation process is complete, the new device is shown in the list of installed virtual devices with a **Start** button, ready to launch:

    :::image type="content" source="media/device-manager/mac/20-created-device-m76-sml.png" alt-text="Newly-created device ready to launch." lightbox="media/device-manager/mac/20-created-device-m76.png":::

### Edit device

To edit an existing virtual device, select the **Additional Options** pull-down menu (gear icon) and select **Edit**:

:::image type="content" source="media/device-manager/mac/21-edit-button-m76-sml.png" alt-text="Edit menu selection for modifying a new device." lightbox="media/device-manager/mac/21-edit-button-m76.png":::

Pressing **Edit** launches the Device Editor for the selected virtual device:

:::image type="content" source="media/device-manager/mac/22-device-editor-sml.png" alt-text="Device Editor screen." lightbox="media/device-manager/mac/22-device-editor.png":::

The **Device Editor** screen lists the properties of the virtual device under the **Property** column, with the corresponding values of each property in the **Value** column. When you select a property, a detailed description of that property is displayed on the right.

To change a property, edit its value in the **Value** column. For example, in the following screenshot the `hw.lcd.density` property is being changed from **480** to **240**:

:::image type="content" source="media/device-manager/mac/23-device-editing-sml.png" alt-text="Device Editing example." lightbox="media/device-manager/mac/23-device-editing.png":::

After you've made the necessary configuration changes, press the **Save** button. For more information about changing virtual device properties, see [Editing Android Virtual Device Properties](device-properties.md).

### Additional options

Additional options for working with a device are available from the pull-down menu located to the left of the **Play** button:

:::image type="content" source="media/device-manager/mac/24-overflow-menu-sml.png" alt-text="Location of additional options menu." lightbox="media/device-manager/mac/24-overflow-menu.png":::

The additional options menu contains the following items:

- **Edit** &ndash; Opens the currently-selected device in the device editor as described earlier.

- **Duplicate and Edit** &ndash; Duplicates the currently-selected device and opens it in the **New Device** screen with a different unique name. For example, selecting **Pixel 2 API 28** and pressing **Duplicate and Edit** appends a counter to the name:

  :::image type="content" source="media/device-manager/mac/25-dupe-and-edit-sml.png" alt-text="Duplicate and Edit screen." lightbox="media/device-manager/mac/25-dupe-and-edit.png":::

- **Reveal in Finder** &ndash; Opens a macOS Finder window in the folder that holds the files for the virtual device. For example, selecting **Pixel 2 API 28** and pressing **Reveal in Finder** opens a window like the following example:

  :::image type="content" source="media/device-manager/mac/26-reveal-in-finder-sml.png" alt-text="Results of pressing Reveal in Finder." lightbox="media/device-manager/mac/26-reveal-in-finder.png":::

- **Factory Reset** &ndash; Resets the selected device to its default settings, erasing any user changes made to the internal state of the device while it was running (this also erases the current [Quick Boot](debug-on-emulator.md#quick-boot) snapshot, if any). This change doesn't alter modifications that you make to the virtual device during creation and editing. A dialog box will appear with the reminder that this reset cannot be undone. Press **Factory Reset** to confirm the reset.

  :::image type="content" source="media/device-manager/mac/27-factory-reset-m76.png" alt-text="Factory reset dialog.":::

- **Delete** &ndash; Permanently deletes the selected virtual device. A dialog box will appear with the reminder that deleting a device cannot be undone. Press **Delete** if you are certain that you want to delete the device.

  :::image type="content" source="media/device-manager/mac/28-delete-device-m76.png" alt-text="Delete device dialog.":::

## Troubleshooting

The following sections explain how to diagnose and work around problems that may occur when using the Android Device Manager to configure virtual devices.

### Wrong version of Android SDK Tools

TODO: Set the correct version numbers

If Android SDK tools 26.1.1 or later isn't installed, you may see this error dialog on launch:

:::image type="content" source="media/device-manager/mac/29-sdk-instance-error.png" alt-text="Android SDK instance error dialog.":::

If you see this error dialog, press **OK** to open the Android SDK Manager. In the Android SDK Manager, go to the **Tools** tab and install the following packages:

- **Android SDK Tools 26.1.1** or later
- **Android SDK Platform-Tools 28.0.1** or later
- **Android SDK Build-Tools 26.0.3** or later

### Snapshot disables WiFi on Android Oreo

If you've an AVD configured for Android Oreo with simulated Wi-Fi access, restarting the AVD after a snapshot may cause Wi-Fi access to become disabled.

To work around this problem,

01. Select the AVD in the Android Device Manager.

01. From the **Additional Options** (&hellip;) menu, select **Reveal in Finder**.

01. Navigate to **snapshots > default_boot**.

01. Delete the **snapshot.pb** file:

    :::image type="content" source="media/device-manager/mac/30-delete-snapshot-sml.png" alt-text="Location of the snapshot.pb file." lightbox="media/device-manager/mac/30-delete-snapshot.png":::

01. Restart the AVD.

After these changes are made, the AVD will restart in a state that allows Wi-Fi to work again.

::: zone-end

-->
