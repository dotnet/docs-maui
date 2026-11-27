---
title: "Build an Android app with the .NET CLI"
description: "Learn how to create, build, and run a .NET MAUI Android app with the .NET CLI, including interactive device selection."
ms.date: 11/27/2026
monikerRange: ">=net-maui-10.0"
---

# Build an Android app with the .NET CLI

You can create, build, and run .NET Multi-platform App UI (.NET MAUI) Android apps from a terminal on Windows, macOS, or Linux. This guide uses an Android-only project so that you don't need workloads for other platforms.

## Prerequisites

- Install the [.NET SDK](/dotnet/core/install/) for the version of .NET MAUI you want to use. The runtime alone isn't sufficient. Run `dotnet --version` in your working directory to check the selected SDK. A [global.json](/dotnet/core/tools/global-json) file can select a different SDK from the latest installed version.
- Install and configure the Android SDK and Java SDK versions required by your Android workload. If they aren't installed yet, use the `InstallAndroidDependencies` command after creating the project, as shown below.
- Create an [Android emulator](https://developer.android.com/studio/run/managing-avds), or [set up a physical device for debugging](~/android/device/setup.md). Installing a .NET workload doesn't create an Android Virtual Device (AVD).
- Add the Android SDK's *platform-tools* and *emulator* directories to your `PATH` to use `adb` and `emulator` in the commands below.

## Create and build the app

1. Install the .NET MAUI Android workload for the selected SDK:

    ```dotnetcli
    dotnet workload install maui-android
    ```

    If your SDK installation requires administrator permissions, use an elevated terminal on Windows or prefix the command with `sudo` on macOS or Linux. If you've already installed the `maui` workload, you don't need to install `maui-android` separately. For more information, see [dotnet workload install](/dotnet/core/tools/dotnet-workload-install).

1. Create a .NET MAUI app without restoring its dependencies yet, and change to its directory:

    ```dotnetcli
    dotnet new maui -n MyMauiApp --no-restore
    cd MyMauiApp
    ```

1. Open *MyMauiApp.csproj*. To target only Android, replace all of its `TargetFrameworks` elements, including those with a `Condition` attribute, with the following element inside the existing `PropertyGroup`:

    ::: moniker range="net-maui-10.0"

    ```xml
    <TargetFrameworks>net10.0-android</TargetFrameworks>
    ```

    ::: moniker-end

    ::: moniker range=">=net-maui-11.0"

    ```xml
    <TargetFrameworks>net11.0-android</TargetFrameworks>
    ```

    ::: moniker-end

    Use a target framework supported by your installed SDK and workload. If you want to keep the template's other target frameworks instead, install their workloads too; specifying `-f` for a build doesn't prevent restore from evaluating the other frameworks.

1. If the Android SDK and Java SDK aren't already installed, install the project's dependencies:

    ::: moniker range="net-maui-10.0"

    ```dotnetcli
    dotnet build -t:InstallAndroidDependencies -f net10.0-android -p:AndroidSdkDirectory="ANDROID_SDK_PATH" -p:JavaSdkDirectory="JAVA_SDK_PATH" -p:AcceptAndroidSDKLicenses=True
    ```

    ::: moniker-end

    ::: moniker range=">=net-maui-11.0"

    ```dotnetcli
    dotnet build -t:InstallAndroidDependencies -f net11.0-android -p:AndroidSdkDirectory="ANDROID_SDK_PATH" -p:JavaSdkDirectory="JAVA_SDK_PATH" -p:AcceptAndroidSDKLicenses=True
    ```

    ::: moniker-end

    Replace `ANDROID_SDK_PATH` and `JAVA_SDK_PATH` with absolute installation paths on your computer. Setting `AcceptAndroidSDKLicenses=True` accepts the Android SDK licenses on your behalf; review the licenses before running the command. Set the `ANDROID_HOME` and `JAVA_HOME` environment variables to these paths for subsequent commands, and add the Android SDK's *platform-tools* and *emulator* directories to `PATH`. This target doesn't create an AVD. For more information, see [InstallAndroidDependencies](/dotnet/android/building-apps/build-targets#installandroiddependencies).

1. Build the app:

    ```dotnetcli
    dotnet build
    ```

    This command restores dependencies and builds the Android app. It doesn't start an emulator or deploy or launch the app.

## Run the app

::: moniker range=">=net-maui-11.0"

With the .NET 11 SDK and .NET 11 Android workload, build, deploy, and launch the app with:

```dotnetcli
dotnet run --interactive
```

If multiple devices are available, select one from the terminal menu. If only one is available, it's selected automatically. For a project that still targets multiple frameworks, select `net11.0-android` from the framework menu first, or specify it explicitly:

```dotnetcli
dotnet run -f net11.0-android --interactive
```

The Android workload lists online devices first. If any are online, stopped AVDs aren't included in the selection menu. If none are online, configured AVDs are included, and the workload can start the selected emulator. This doesn't create an AVD or install its system image. If startup fails, start the emulator manually and see [Android emulator troubleshooting](~/android/emulator/troubleshooting.md).

The command streams app logs to the terminal. Press <kbd>Ctrl+C</kbd> to stop the app and return to the command prompt.

### Select a device without a prompt

List the available targets:

```dotnetcli
dotnet run -f net11.0-android --list-devices
```

Use an identifier from the output to select a running emulator or connected physical device:

```dotnetcli
dotnet run -f net11.0-android --device emulator-5554
```

You can also select a configured AVD by name, including one that isn't running:

```dotnetcli
dotnet run -f net11.0-android --device MyAndroidAvd
```

Replace `MyAndroidAvd` with a name from `emulator -list-avds`. Quote the name if it contains spaces.

For scripts and CI, specify both `-f` and `--device` to avoid prompts or ambiguity. A bare `dotnet run` doesn't prompt unless you add `--interactive`; it fails when a framework or device choice is required. Device discovery requires both SDK and workload support, not just changing the target framework in the project file.

::: moniker-end

::: moniker range="net-maui-10.0"

Start an emulator or connect a physical device with USB debugging enabled. Then follow [Run with an explicit adb target](#run-with-an-explicit-adb-target). This build-target approach also works with older SDKs.

> [!NOTE]
> `dotnet run` isn't exclusive to .NET 10 and later; earlier Android workloads also implement it. The device-selection experience depends on the SDK and Android workload versions. Recent .NET 10 tooling also supports `--device`, `--list-devices`, and `--interactive`; check `dotnet run --help` and your [Android workload release notes](https://github.com/dotnet/android/releases). The commands below don't require those newer options.

::: moniker-end

On the emulator or device, select the **Click me** button several times and confirm that the displayed count increases.

## Run with an explicit adb target

The `Run` MSBuild target is an alternative when using older tooling or troubleshooting device selection. Start the emulator yourself, or connect and authorize your device, then list the connected targets:

```console
adb devices
```

For example:

```console
List of devices attached
emulator-5554   device
1A2B3C4D5E6F    device
```

Only use targets whose state is `device`. For `unauthorized`, unlock the physical device and accept its USB debugging prompt. For `offline`, wait for the emulator to finish starting or reconnect the device.

Pass the identifier through the public [AdbTarget](/dotnet/android/building-apps/build-properties#adbtarget) MSBuild property:

::: moniker range="net-maui-10.0"

```dotnetcli
dotnet build -t:Run -f net10.0-android -p:AdbTarget=-s%20emulator-5554
```

::: moniker-end

::: moniker range=">=net-maui-11.0"

```dotnetcli
dotnet build -t:Run -f net11.0-android -p:AdbTarget=-s%20emulator-5554
```

::: moniker-end

Replace `emulator-5554` with your target's identifier. `%20` is the MSBuild escape for a space, so the property becomes `-s emulator-5554` without relying on shell-specific handling of embedded quotes.

Alternatively, use `-p:AdbTarget=-e` for the only running emulator, or `-p:AdbTarget=-d` for the only connected physical device. These options fail if more than one matching target is connected. For more information, see [Send commands to a specific device](https://developer.android.com/tools/adb#directingcommands).

## Additional CLI options

Use `-c Release` with a build or run command to select the Release configuration. These commands are for local deployment, not store distribution. For signing and publishing instructions, see [Publish an Android app using the command line](~/android/deployment/publish-cli.md).
