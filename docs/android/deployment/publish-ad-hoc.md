---
title: "Publish a .NET MAUI Android app for ad-hoc distribution"
description: "Learn how to publish a .NET MAUI Android app for ad-hoc distribution."
ms.date: 05/10/2023
---

# Publish an Android app for ad-hoc distribution

https://learn.microsoft.com/en-us/xamarin/android/deploy-test/signing/?tabs=windows

Ad-Hoc – Saves a signed AAB to disk that can be sideloaded to Android devices. Continue to Signing the App Package to learn how to create an Android signing identity, create a new signing certificate for Android applications, and publish an ad hoc version of the app to disk. This is a good way to create an APK for testing.

This section explains how to create an Android signing identity, create a new signing certificate for Android applications, and publish the archived app ad hoc to disk. The resulting AAB can be sideloaded into Android devices without going through an app store.

## Change package format

By default, the package format for .NET MAUI Android release builds is AAB. To publish a .NET MAUI Android app using ad-hoc publishing requires that you first change the package format to APK:

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that the value of the **Release** field is set to **apk**:

    :::image type="content" source="media/publish/vs/ad-hoc-change-package-format.png" alt-text="Screenshot of changing the package format of a .NET MAUI Android app to APK.":::

## Publish

[!INCLUDE [Publish](../includes/publish-vs.md)]

<!-- markdownlint-disable MD029 -->
4. In the **Distribute - Select Channel** dialog, select the **Ad Hoc** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-ad-hoc.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select the **+** button to create a new signing identity:

    :::image type="content" source="media/publish/vs/create-new-ad-hoc-signing-identity.png" alt-text="Screenshot of creating a new signing identity in the distribution dialog.":::

    The **Create Android Keystore** dialog will appear.

    > [!NOTE]
    > Alternatively, an existing signing identity can be used by selecting the **Import** button.

1. In the **Create Android Keystore** dialog, enter the required information to create a new signing identity, known as a *keystore*, and then select the **Create** button:

    :::image type="content" source="media/publish/vs/create-android-keystore.png" alt-text="Screenshot of creating an Android keystore.":::

    A new keystore, which contains a new certificate, will be saved to **C:\Users\{Username}\AppData\Local\Xamarin\Mono for Android\Keystore\{Alias}\{Alias}.keystore**.

    > [!IMPORTANT]
    > The keystore and password isn't saved to your Visual Studio solution. Therefore, ensure you back up this data. If you lose it you'll be unable to sign your app with the same signing identity.

1. In the **Distribute - Signing Identity** dialog, select your newly created signing identity and select the **Save As** button:

    :::image type="content" source="media/publish/vs/save-ad-hoc.png" alt-text="Screenshot of publishing your app for ad-hoc distribution.":::

    The *Archive Manager* displays the publishing process.

1. In the **Save As** dialog, confirm the location and file name for your *.AAB* file is correct and select the **Save** button.
1. In the **Signing Password** dialog, enter the signing identity password and select the **OK** button:

    :::image type="content" source="media/publish/vs/keystore-password.png" alt-text="Screenshot of entering your signing identity password.":::

1. In the *Archive Manager**, select the **Open Distribution** button once the publishing process completes:

    :::image type="content" source="media/publish/vs/ad-hoc-open-distribution.png" alt-text="Screenshot of opening the folder containing your published Android app.":::

    Visual Studio will open the folder containing the published *.AAB* file.

The app can then be distributed to Android devices through a website, or email. When users browse to a download link from their Android-powered device, the file is downloaded and Android automatically starts installing it on the device. For more information, see [Alternative distribution options](https://developer.android.com/distribute/marketing-tools/alternative-distribution) on developer.android.com.
