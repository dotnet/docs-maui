---
title: "Publish a .NET MAUI Android app for ad-hoc distribution"
description: "Learn how to publish a .NET MAUI Android app for ad-hoc distribution."
ms.date: 05/10/2023
---

# Publish an Android app for ad-hoc distribution

<!--
https://learn.microsoft.com/en-us/xamarin/android/deploy-test/signing/?tabs=windows
https://developer.android.com/studio/publish#publishing-website
-->

Ad-Hoc – Saves a signed AAB to disk that can be sideloaded to Android devices. Continue to Signing the App Package to learn how to create an Android signing identity, create a new signing certificate for Android applications, and publish an ad hoc version of the app to disk. This is a good way to create an APK for testing.

This section explains how to create an Android signing identity, create a new signing certificate for Android applications, and publish the archived app ad hoc to disk. The resulting AAB can be sideloaded into Android devices without going through an app store.

## Ensure correct package format

By default, the package format for .NET MAUI Android release builds is AAB. To publish a .NET MAUI Android app for side loading requires that you first change the package format to APK:

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that the value of the **Release** field is set to **apk**:

    :::image type="content" source="media/publish/vs/ad-hoc-change-package-format.png" alt-text="Screenshot of changing the package format of a .NET MAUI Android app to APK.":::

## Publish

[!INCLUDE [Publish](../includes/publish-vs.md)]

[!INCLUDE [Publish ad-hoc](../includes/publish-ad-hoc.md)]

The app can then be distributed to Android devices through a website, or email. When users browse to a download link from their Android-powered device, the file is downloaded and Android automatically starts installing it on the device. For more information, see [Alternative distribution options](https://developer.android.com/distribute/marketing-tools/alternative-distribution) on developer.android.com.
