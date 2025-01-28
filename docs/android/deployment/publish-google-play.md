---
title: "Publish a .NET MAUI Android app for Google Play distribution"
description: "Learn how to publish a .NET MAUI Android app for Google Play distribution."
ms.date: 07/18/2024
---

# Publish an Android app for Google Play distribution

> [!div class="op_single_selector"]
>
> - [Publish for ad-hoc distribution](publish-ad-hoc.md)
> - [Publish using the command line](publish-cli.md)

The most common approach to distributing Android apps to users is through the Google Play. The first time an app is submitted to Google Play it must be submitted through the Google Play Console. Subsequent versions of the app can be submitted through Visual Studio. In both cases, a Google Play Developer account is required. Apps submitted to Google Play require approval from Google.

To distribute a .NET Multi-platform App UI (.NET MAUI) Android app, you'll need to sign it with a key from your keystore, prior to upload to Google Play. Keystores are binary files that serve as repositories of certificates and private keys.

Google Play requires that you submit your app as an *Android App Bundle* (AAB). Google Play uses your app bundle to generate and serve optimized Android Packages (APK) for each device configuration, so that only the code and resources that are needed for a specific device are downloaded to run your app. For more information about Android App Bundles, see [About Android App Bundles](https://developer.android.com/guide/app-bundle) on developer.android.com.

The process for distributing a .NET MAUI Android app through Google Play is as follows:

1. Create a Google Play Developer account. For more information, see [Create a Google Play Developer account](#create-a-google-play-developer-account).
1. Create your app in Google Play Console. For more information, see [Create your app in Google Play Console](#create-your-app-in-google-play-console).
1. Setup your app in Google Play Console. For more information, see [Setup your app in Google Play Console](#setup-your-app-in-google-play-console).
1. Ensure your app uses the correct package format. For more information, see [Ensure correct package format](#ensure-correct-package-format).
1. Build and sign your app in Visual Studio, and then distribute it through Google Play Console. For more information, see [Distribute your app through Google Play Console](#distribute-your-app-through-google-play-console).

Then, subsequent versions of your app can be published through Visual Studio. For more information, see [Distribute your app through Visual Studio](#distribute-your-app-through-visual-studio).

## Create a Google Play Developer account

To publish Android apps on Google Play, you'll need to create a Google Play Developer account:

1. Using your Google Account, sign up for a [Google Play Developer account](https://play.google.com/apps/publish/signup).
1. Enter information about your developer identity.
1. During the sign-up process, you'll need to review and accept the [Google Play Developer Distribution Agreement](https://play.google.com/about/developer-distribution-agreement.html).
1. Pay the one-time $25 registration fee.
1. Verify your identity by following the instructions in your verification email.

> [!IMPORTANT]
> Identity verification must be complete before you can publish apps through Google Play. In addition, new personal account holders will have to verify that they have access to a real Android device. For more information, see [Device verification requirements for new developer accounts](https://support.google.com/googleplay/android-developer/answer/14316361?hl=en) on support.google.com.

Once your Google Play Developer account has been created you'll be able to begin the process to publish an app to Google Play.

For more information, see [Register for a Google Play Developer account](https://support.google.com/googleplay/android-developer/answer/6112435?hl=en-GB#zippy=) on support.google.com.

## Create your app in Google Play Console

After you've created your Google Play Developer account, you'll need to create an app in Google Play Console:

1. Login to your [Google Play Developer account](https://play.google.com/apps/publish).
1. In **Google Play Console**, on the **All apps** tab, select the **Create app** button:

    :::image type="content" source="media/publish/vs/google-play-create-new-app.png" alt-text="Screenshot of the all apps page in Google Play.":::

1. In the **Create app** page, enter your app details and select the **Create app** button:

    :::image type="content" source="media/publish/vs/google-play-create-app.png" alt-text="Screenshot of creating a new app in Google Play.":::

For more information about creating an app in Google Play Console, see [Create and set up your app](https://support.google.com/googleplay/android-developer/answer/9859152) on support.google.com.

## Setup your app in Google Play Console

After creating your app, you should set it up. Your app's dashboard will guide you through all the most important steps.

To start setting up your app, select **Dashboard** at the left menu. Under your app's details at the top of the page you'll find different categories and tasks relating to app setup and release. You must complete the mandatory tasks before you can launch your app on Google Play. When you complete a task you'll see a green tick mark and strikethrough text. The progress bar at the top of the section will also be updated. For more information, see [Set up your app on the app dashboard](https://support.google.com/googleplay/android-developer/answer/9859454) on support.google.com.

## Ensure correct package format

To publish a .NET MAUI Android app for Google Play distribution requires that your app package format is AAB, which is the default package format for release builds. To verify that your app's package format is set correctly:

1. In Visual Studio's **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that the value of the **Release** field is set to **bundle**:

    :::image type="content" source="media/publish/vs/google-play-ensure-package-format.png" alt-text="Screenshot of changing the package format of a .NET MAUI Android app to APK.":::Publish

## Distribute your app through Google Play Console

The first time an AAB is submitted to Google Play, it must be manually uploaded through the Google Play Console. This enables Google Play to match the signature of the key on all future bundles to the original key used for the first version of the app. In order to upload the app through the Google Play Console, it must first be built and signed in Visual Studio.

[!INCLUDE [Publish](../includes/publish-vs.md)]

[!INCLUDE [Publish ad-hoc](../includes/publish-ad-hoc.md)]

The published app can then be released to Google Play via the Google Play Console. To do this you must first create a release in Google Play Console. A release is a combination of one or more app versions that you'll prepare in order to launch an app, or roll out an update. You can create a release on the following tracks:

- Internal testing. Internal testing releases are available to up to 100 testers that you can choose.
- Closed testing. Closed testing releases are available to a limited number of testers that you choose, who can test a pre-release version of your app and submit feedback.
- Open testing. Open testing releases are available to testers on Google Play. Users can join tests from your Store listing.
- Production. Production releases are available to all Google Play users in your chosen countries.

For more information about creating a release in Google Play Console, see [Prepare and roll out a release](https://support.google.com/googleplay/android-developer/answer/9859348) on support.google.com.

> [!IMPORTANT]
> App's submitted to Google Play typically undergo a review process. For more information, see [Prepare your app for review](https://support.google.com/googleplay/android-developer/answer/9859455) and [Publish your app](https://support.google.com/googleplay/android-developer/answer/9859751) on support.google.com.

## Distribute your app through Visual Studio

An AAB must have already been submitted to Google Play, and have passed review, before you can distribute it from Visual Studio. If you attempt to distribute an AAB from Visual Studio that hasn't first been uploaded from the Play Console, you'll receive the following error:

> Google Play requires you to manually upload your first package (APK/AAB) for this app. You can use an ad-hoc package for this.

When this error occurs, manually upload an AAB via the Google Play Console. Subsequent releases of the app can then be published through Visual Studio. However, you must change the version code of the app for each upload, otherwise the following error will occur:

> An AAB with version code (1) has already been uploaded.

To resolve this error, rebuild the app with a different version number and then resubmit it to Google Play via Visual Studio.

> [!NOTE]
> The app's version number can be updated by increasing the value of the `ApplicationVersion` integer property in the app's project file.

Uploading your app from Visual Studio to Google Play first requires you to setup API access in the Google Play Console.

### Enable Google API access

The Google Play Developer Publishing API enables Visual Studio to upload new versions of an app to Google Play. Before Visual Studio can start making API calls, you'll need to set up API access in your Google Play Developer account. This involves linking your Google Play Developer account to a Google Cloud project, and configuring access to the Google Play Developer Publishing API with an OAuth client.

To enable Google API access:

1. Login to your [Google Play Developer account](https://play.google.com/apps/publish).
1. In **Google Play Console**, expand the **Setup** item and select **API access**. Then in the **API access** page, select the **Choose a project to link** button:

    :::image type="content" source="media/publish/vs/api-access-choose-project.png" alt-text="Screenshot of API access page in Google Play Console.":::

    To use Google Play Developer APIs you'll need a Google Cloud project that must be linked to your Google Play Developer account.

    > [!NOTE]
    > A Google Play Developer account can only be linked to a single Google Cloud project. Therefore, if you publish multiple apps from one Google Play Developer account, they all must share the same Google Cloud project.

1. In the **API access** page, select the **Create a new Google Cloud project** radio button followed by the **Save** button:

    :::image type="content" source="media/publish/vs/api-access-create-google-cloud-project.png" alt-text="Screenshot of selecting the create a new Google Cloud project radio button.":::

    A new Google Cloud project will be created and linked to your Google Play Developer account.

1. In the **API access** page, in the **OAuth clients** section, select **Configure OAuth consent screen**:

    :::image type="content" source="media/publish/vs/api-access-configure-oauth-consent.png" alt-text="Screenshot of API access page with linked Google Cloud project.":::

1. In the **OAuth consent screen** page, select your required user type radio button and then select the **CREATE** button:

    :::image type="content" source="media/publish/vs/oauth-consent-user-type.png" alt-text="Screenshot of selecting the user type for the OAuth consent screen.":::

    For more information about the user types, see [User type](https://support.google.com/cloud/answer/10311615#user-type) on support.google.com.

1. In the **Edit app registration** page, complete the fields marked as required and then select the **SAVE AND CONTINUE** button:

    :::image type="content" source="media/publish/vs/oauth-consent-edit-app-registration1.png" alt-text="Screenshot of editing the OAuth consent screen app registration data.":::
    :::image type="content" source="media/publish/vs/oauth-consent-edit-app-registration2.png" alt-text="Screenshot of editing the OAuth consent screen developer contact information.":::

1. In the **Edit app registration** page, select the **ADD OR REMOVE SCOPES** button:

    :::image type="content" source="media/publish/vs/oauth-consent-add-scopes.png" alt-text="Screenshot of adding scopes for the OAuth consent screen.":::

1. In the **Update selected scopes** flyout, select the `./auth/androidpublisher` scope in the table and then select the **UPDATE** button:

    :::image type="content" source="media/publish/vs/oauth-consent-scopes.png" alt-text="Screenshot of adding the android publisher scope for the OAuth consent screen.":::

    > [!NOTE]
    > The other scopes in the table can be optionally selected as they are standard scopes.

1. In the **Edit app registration** page, select the **Credentials** tab:

    :::image type="content" source="media/publish/vs/oauth-consent-credentials.png" alt-text="Screenshot of selecting the credentials tab.":::

1. In the **Credentials** page, select the **+ CREATE CREDENTIALS** button and then the **OAuth client ID** item:

    :::image type="content" source="media/publish/vs/credentials-create.png" alt-text="Screenshot of selecting the create credentials button.":::

1. In the **Create OAuth client ID** page, choose **Desktop app** in the **Application type** drop-down, enter **Visual Studio** in the **Name** field, and then select the **CREATE** button:

    :::image type="content" source="media/publish/vs/credentials-create-oauth.png" alt-text="Screenshot of creating OAuth credentials.":::

1. In the **OAuth client created** dialog, select the **DOWNLOAD JSON** data button to download your client ID and client secret. This will be required later. Then click the **OK** button to dismiss the dialog.

For more information about enabling Google API access to your Google Play Developer account, see [Getting Started](https://developers.google.com/android-publisher/getting_started) on developers.google.com. For more information about setting up your OAuth consent screen, see [Setting up your OAuth consent screen](https://support.google.com/cloud/answer/10311615) on support.google.com.

### Upload your app through Visual Studio

[!INCLUDE [Publish](../includes/publish-vs.md)]

<!-- markdownlint-disable MD029 -->
5. In the **Distribute - Select Channel** dialog, select the **Google Play** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-google-play.png" alt-text="Screenshot of selecting the Google Play distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select the signing identity you created when building the app for distribution through the Google Play Console, and then select the **Continue** button:

    :::image type="content" source="media/publish/vs/signing-identity-continue.png" alt-text="Screenshot of selecting your newly created signing identity.":::

#### Add your Google Play Developer account details

To add your Google Play Developer account to Visual Studio:

1. In the **Distribute - Google Play Account** dialog, select the **+** button to add your Google Play Developer account details:

    :::image type="content" source="media/publish/vs/distribution-add-google-play-account.png" alt-text="Screenshot of adding a Google Play Developer account in the distribution dialog.":::


1. In the **Register Google API Access** dialog, enter a description and your OAuth client ID and client secret, and then click the **Register** button:

    :::image type="content" source="media/publish/vs/register-google-api-access.png" alt-text="Screenshot of registering your OAuth client ID and client secret in Visual Studio.":::

    > [!NOTE]
    > The account description makes it possible to register more than one Google Play Developer account and upload apps to different Google Play Developer accounts.

    A web browser will open.

1. In the web browser, sign into your Google Play Developer account. After you sign in, a message may be displayed telling you that Google hasn't verified the app. If this occurs select the **Continue** button:

    :::image type="content" source="media/publish/vs/google-sign-in-continue.png" alt-text="Screenshot of Google sign in saying the app hasn't been verified.":::

1. In the web browser, select the **Continue** button to authorize the app:

    :::image type="content" source="media/publish/vs/google-api-access-authorize.png" alt-text="Screenshot of authorizing Google API access.":::

    The web browser will receive a verification code.

    > [!IMPORTANT]
    > Don't close the web browser.

1. In Visual Studio, in the **Distribute - Google Play Account** dialog, select **Continue**:

    :::image type="content" source="media/publish/vs/distribution-google-api-access-authorized.png" alt-text="Screenshot of authorized Google API access in Visual Studio.":::

<!-- Don't delete this heading - the Visual Studio IDE links to it -->
#### Select a track to upload your app to

To select the Google Play track to upload your app to:

1. In the **Distribute - Google Play Track** dialog, select the track to upload your app to. Google Play offers five tracks for uploading your app:

    - Internal should be used for quickly distributing your app for internal testing and quality assurance checks.
    - Alpha should be used for uploading an early version of your app to a small group of testers.
    - Beta should be used for uploading an early version of your app to a larger group of testers.
    - Production should be used for full distribution to the Google Play store.
    - Custom should be used for testing pre-release versions of your app with specific users by creating a list of testers by email address.

    > [!IMPORTANT]
    > If you don't see the custom track, ensure you've created a release for that track in the Google Play Console. For more information, see [Prepare and roll out a release](https://support.google.com/googleplay/android-developer/answer/9859348?hl=en&visit_id=638192315525080840-296240211&rd=1) on support.google.com.

    Select the track to upload your app to and then select the **Upload** button:

    :::image type="content" source="media/publish/vs/distribution-select-google-play-track.png" alt-text="Screenshot of selecting a Google Play Track prior to uploading your app.":::

    For more information about Google Play testing, see [Set up an open, closed, or internal test](https://support.google.com/googleplay/android-developer/answer/9845334?hl=en&visit_id=638192315525080840-296240211&rd=1) on support.google.com.

1. Visual Studio will begin publishing your app to Google Play. In the **Signing Password** dialog, enter your password you created for the signing identity and select the **OK** button:

    :::image type="content" source="media/publish/vs/enter-password.png" alt-text="Screenshot of entering your password for your signing identity in Visual Studio.":::

    Visual Studio will sign your app bundle and upload it to Google Play.

    > [!IMPORTANT]
    > App's submitted to Google Play typically undergo a review process. For more information, see [Prepare your app for review](https://support.google.com/googleplay/android-developer/answer/9859455) and [Publish your app](https://support.google.com/googleplay/android-developer/answer/9859751) on support.google.com.
