---
title: "Use the CLI to publish for Android"
description: "Learn how to package and publish an Android .NET MAUI app with the dotnet publish command."
ms.date: 03/17/2022
---

# Publish a .NET MAUI app for Android with the CLI

> [!div class="op_single_selector"]
>
> - [Publish for Android](publish-cli.md)
> - [Publish for iOS](../../ios/deployment/index.md)
> - [Publish for macOS](../../macos/deployment/index.md)
> - [Publish for Windows](../../windows/deployment/publish-cli.md)

When distributing your .NET Multi-platform App UI (.NET MAUI) app for Android, you generate an _apk_ (Android Package) or an _aab_ (Android App Bundle) file. The _apk_ is used for installing your app to an Android device, and the _aab_ is used to publish your app to an Android store.

With just a few configuration changes to your project, your app can be packaged for distribution.

## Validate package settings

Every Android app specifies a unique package identifier and a version. These identifiers are generally set in the Android app manifest file, which is located in your project folder at _.\\Platforms\\Android\\AndroidManifest.xml_. However, these specific settings are provided by the project file itself. When a .NET MAUI app is built, the final _AndroidManifest.xml_ file is automatically generated using the project file and the original _AndroidManifest.xml_ file.

Your project file must declare `<ApplicationId>` and `<ApplicationVersion>` within a `<PropertyGroup>` node. These items should have been generated for you when the project was created. Just validate that they exist and are set to valid values:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <!-- other settings -->

    <PropertyGroup>
        <ApplicationId>com.companyname.myproject</ApplicationId>
        <ApplicationVersion>1</ApplicationVersion>
    </PropertyGroup>

</Project>
```

> [!TIP]
> Some settings are available in the **Project Properties** editor in Visual Studio to change values. Right-click on the project in the **Solution Explorer** pane and choose **Properties**. For more information, see [Project configuration in .NET MAUI](../../deployment/visual-studio-properties.md).

The following table describes how each project setting maps to the manifest file:

| Project setting | Manifest setting |
| --- | --- |
| `ApplicationId` | The `package` attribute of the `<manifest>` node: `<manifest ... package="com.companyname.myproject>"`. |
| `ApplicationVersion` | The `android:versionCode` attribute of the `<manifest>` node: `<manifest ... android:versionCode="1">`. |

Here's an example of an automatically generated manifest file with the package and version information specified:

```xml
<manifest
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:versionCode="1"
    package="com.hi.companyname.myproject"
    android:versionName="1.0.0">
    <!-- other settings -->
</manifest>
```

For more information about the manifest, see [Google Android App Manifest Overview](https://developer.android.com/guide/topics/manifest/manifest-intro).

## Create a keystore file

Your app package should be signed. You use a keystore file to sign your package. The Java/Android SDKs includes the tools you need to generate a keystore. After generating a keystore file, you'll add it to your project and configure your project file to reference it. The Java SDK should be in your system path so that you can run the _keytool_ tool.

Perform the following steps to create a keystore file:

01. Open a terminal and navigate to the folder of your project.

    > [!TIP]
    > If Visual Studio is open, use the **View** > **Terminal** menu to open a terminal at the location of the solution or project. Navigate to the project folder.

01. Run the _keytool_ tool with the following parameters:

    ```console
    keytool -genkey -v -keystore myapp.keystore -alias key -keyalg RSA -keysize 2048 -validity 10000
    ```

    You'll be prompted to provide and confirm a password, followed by other settings.

    The tool generates a _myapp.keystore_ file, which should be located in the same folder as your project.

## Add a reference to the keystore file

There are project-level settings you must set to sign your Android app with the keystore file. These settings are configured in a `<PropertyGroup>` node:

- `<AndroidKeyStore>` &ndash; Set to `True` to sign the app.
- `<AndroidSigningKeyStore>` &ndash; The keystore file created in the previous section: **myapp.keystore**.
- `<AndroidSigningKeyAlias>` &ndash; The `-alias` parameter value passed to the _keytool_ tool: **key**.
- `<AndroidSigningKeyPass>` &ndash; The password you provided when creating the keystore file.
- `<AndroidSigningStorePass>` &ndash; The password you provided when creating the keystore file.

For security reasons, you don't want to supply a value for `<AndroidSigningKeyPass>` and `<AndroidSigningStorePass>` in the project file. You can provide these values on the command line when you publish the app. An example of providing the password is in the [Publish section](#publish).

```xml
<PropertyGroup Condition="$(TargetFramework.Contains('-android')) and '$(Configuration)' == 'Release'">
    <AndroidKeyStore>True</AndroidKeyStore>
    <AndroidSigningKeyStore>myapp.keystore</AndroidSigningKeyStore>
    <AndroidSigningKeyAlias>key</AndroidSigningKeyAlias>
    <AndroidSigningKeyPass></AndroidSigningKeyPass>
    <AndroidSigningStorePass></AndroidSigningStorePass>
</PropertyGroup>
```

The example `<PropertyGroup>` above adds a condition check, preventing those settings from being processed unless the condition check passes. The condition check looks for two things:

01. The target framework is set to something containing the text `-android`.
01. The build configuration is set to `Release`.

If either of those conditions fail, the settings aren't processed. More importantly, the `<AndroidKeyStore>` setting isn't set, preventing the app from being signed.

## Publish

At this time, publishing is only supported through the .NET command line interface.

To publish your app, open a terminal and navigate to the folder for your .NET MAUI app project. Run the `dotnet publish` command, providing the following parameters:

| Parameter                    | Value                                                                                                                                     |
|------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------|
| `-f` or `--framework`        | The target framework, which is `net6.0-android` or `net7.0-android`.                                                                      |
| `-c` or `--configuration`    | The build configuration, which is `Release`.                                                                                              |
| `/p:AndroidSigningKeyPass`   | This is the value used for the `<AndroidSigningKeyPass>` project setting, the password you provided when you created the keystore file.   |
| `/p:AndroidSigningStorePass` | This is the value used for the `<AndroidSigningStorePass>` project setting, the password you provided when you created the keystore file. |

> [!WARNING]
> Attempting to publish a .NET MAUI solution will result in the `dotnet publish` command attempting to publish each project in the solution individually, which can cause issues when you've added other project types to your solution. Therefore, the `dotnet publish` command should be scoped to your .NET MAUI app project.

::: moniker range="=net-maui-6.0"

For example:

```console
dotnet publish -f:net6.0-android -c:Release /p:AndroidSigningKeyPass=mypassword /p:AndroidSigningStorePass=mypassword
```

Publishing builds the app, and then copies the _aab_ and _apk_ files to the _bin\\Release\\net6.0-android\\publish_ folder. There are two _aab_ files, one unsigned and another signed. The signed variant has **-signed** in the file name.

::: moniker-end

::: moniker range="=net-maui-7.0"

For example:

```console
dotnet publish -f:net7.0-android -c:Release /p:AndroidSigningKeyPass=mypassword /p:AndroidSigningStorePass=mypassword
```

Publishing builds the app, and then copies the _aab_ and _apk_ files to the _bin\\Release\\net7.0-android\\publish_ folder. There are two _aab_ files, one unsigned and another signed. The signed variant has **-signed** in the file name.

::: moniker-end

For more information about the `dotnet publish` command, see [dotnet publish](/dotnet/core/tools/dotnet-publish).

To learn how to upload a signed Android App Bundle to the Google Play Store, see [Upload your app to the Play Console](https://developer.android.com/studio/publish/upload-bundle).

## See also

- [GitHub discussion and feedback: .NET MAUI Android target publishing/archiving](https://github.com/dotnet/maui/issues/4377)
- [Android Developers: About Android App Bundles](https://developer.android.com/guide/app-bundle)
- [Android Developers: Meet Google Play's target API level requirement](https://developer.android.com/google/play/requirements/target-sdk)
