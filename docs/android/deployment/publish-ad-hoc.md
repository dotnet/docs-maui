---
title: "Publish a .NET MAUI Android app for ad-hoc distribution"
description: "Learn how to publish a .NET MAUI Android app for ad-hoc distribution."
ms.date: 05/15/2023
---

# Publish an Android app for ad-hoc distribution

> [!div class="op_single_selector"]
>
> - [Publish for Google Play distribution](publish-google-play.md)
> - [Publish using the command line](publish-cli.md)

When distributing Android apps outside Google Play, and other marketplaces, *ad-hoc* distribution enables you to make the app available for download on a website or server. Android requires that apps created for ad-hoc distribution use the Android Package (APK) format.

To distribute a .NET Multi-platform App UI (.NET MAUI) Android app, you'll need to sign it with a key from your keystore. Keystores are binary files that serve as repositories of certificates and private keys.

The process for publishing a .NET MAUI Android app for ad-hoc distribution is as follows:

1. Ensure your app uses the correct package format. For more information, see [Ensure correct package format](#ensure-correct-package-format).
1. Build and sign your app in Visual Studio. For more information, see [Distribute your app through Visual Studio](#distribute-your-app-through-visual-studio).

## Ensure correct package format

By default, the package format for .NET MAUI Android release builds is AAB. To publish a .NET MAUI Android app for ad-hoc distribution requires that you first change the package format to APK:

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that the value of the **Release** field is set to **apk**:

    :::image type="content" source="media/publish/vs/ad-hoc-change-package-format.png" alt-text="Screenshot of changing the package format of a .NET MAUI Android app to APK.":::

## Distribute your app through Visual Studio

[!INCLUDE [Publish](../includes/publish-vs.md)]

[!INCLUDE [Publish ad-hoc](../includes/publish-ad-hoc.md)]

The app can then be distributed to Android devices through a website or server. When users browse to a download link from their Android device, the file is downloaded. Android will automatically start installing it on the device, provided that the user has configured their settings to allow the installation of apps from unknown sources. For more information about opting into allowing apps from unknown sources, see [User opt-in for unknown apps and sources](https://developer.android.com/studio/publish#publishing-unknown) on developer.android.com.
