---
title: "Publish a .NET MAUI Android app for Google Play distribution"
description: "Learn how to publish a .NET MAUI Android app for Google Play distribution."
ms.date: 05/10/2023
---

# Publish an Android app for Google Play distribution

<!--
https://learn.microsoft.com/en-us/xamarin/android/deploy-test/publishing/publishing-to-google-play/?tabs=windows
https://developer.android.com/studio/publish/app-signing
https://developercommunity.visualstudio.com/t/cannot-publish-Maui-App-with-Visual-Stud/10118589?entry=problem
https://goforgoldman.com/posts/maui-app-deploy/
-->

Google Play – Publishes a signed APK to Google Play. Continue to Publishing to Google Play to learn how to sign and publish an AAB in the Google Play store.

Android requires that *.AAB* files are

Process:

- Create a Google Play Developer account
- Sign your bundle
- Deploy to Google Play
- Deploy from Visual Studio
- Release to production

## Keystores

Keystores are binary files that serve as repositories of certificates and private keys.

## Create a Google Play Developer account

To publish Android apps on Google Play, you'll need to create a Google Play Developer account:

1. Using your Google Account, sign up for a [Google Play Developer account](https://play.google.com/apps/publish/signup).
1. Enter information about your developer identity
1. During the sign-up process, you'll need to review and accept the [Google Play Developer Distribution Agreement](https://play.google.com/about/developer-distribution-agreement.html).
1. Pay the one-time $25 registration fee.
1. Verify your identity by following the instructions in your verification email.

> [!IMPORTANT]
> Identity verification must be complete before you can publish apps through Google Play.

Once your Google Play Developer Account has been created you'll be able to publish apps to Google Play.

For more information, see [Register for a Google Play Developer account](https://support.google.com/googleplay/android-developer/answer/6112435?hl=en-GB#zippy=) on support.google.com.

## Create your app in Google Play Console

After you've created your Google Play Developer account, you'll need to create an app and set it up using Google Play Console:

1. Login to your [Google Play Developer account](https://play.google.com/apps/publish).
1. In **Google Play Console**, on the **All apps** tab, select the **Create app** button:

    :::image type="content" source="media/publish/vs/google-play-create-new-app.png" alt-text="Screenshot of the all apps page in Google Play.":::

1. In the **Create app** page, enter your app details and select the **Create app** button:

    :::image type="content" source="media/publish/vs/google-play-create-app.png" alt-text="Screenshot of creating a new app in Google Play.":::

    For more information about creating an app in Google Play Console, see [Create and set up your app](https://support.google.com/googleplay/android-developer/answer/9859152) on support.google.com.

## Setup your app in Google Play Console

After creating your app, you should set it up. Your app's dashboard will guide you through all the most important steps.

To start setting up your app, select **Dashboard** at the left menu. Under your app's details at the top of the page you'll find different categories and tasks relating to app setup and release. You must complete the mandatory tasks before you can launch your app on Google Play. When you complete a task you'll see a green tick mark and strikethrough text. The progress bar at the top of the section will also be updated. For more information, see [Set up your app on the app dashboard](https://support.google.com/googleplay/android-developer/answer/9859454) on support.google.com.

## Ad hoc publish

INCLUDE FILE

## Prepare a release

A release is a combination of one or more app versions that you'll prepare to launch an app, or roll out an update. You can create a release on the following tracks:

- Open testing. Open testing releases are available to testers on Google Play. Users can join tests from your Store listing.
- Closed testing. Closed testing releases are available to a limited number of testers that you choose, who can test a pre-release version of your app and submit feedback.
- Internal testing. Internal testing releases are available to up to 100 testers that you can choose.
- Production. Production releases are available to all Google Play users in your chosen countries.

For more information about creating a release, see [Prepare and roll out a release](https://support.google.com/googleplay/android-developer/answer/9859348) on support.google.com.

## Deploy from VS

Once you've uploaded your initial build you can sign and distribute your AAB directly from Visual Studio. This requires you to setup API access in the Google Play Console.

### Google API access

1. Login to your [Google Play Developer account](https://play.google.com/apps/publish).
1. In **Google Play Console**, expand the **Setup** item and select **API access**. Then in the **API access** page, select the **Choose a project to link** button:

    :::image type="content" source="media/publish/vs/api-access-choose-project.png" alt-text="Screenshot of API access page in Google Play Console.":::

  To use Google Play Developer APIs you'll need a Google Cloud project that must be linked to your Play Console developer account.

  > [!NOTE]
  > A Google Cloud project can only be linked to one developer account.

1. In the **API access** page, select the **Create a new Google Cloud project** radio button followed by the **Save** button:

    :::image type="content" source="media/publish/vs/api-access-create-google-cloud-project.png" alt-text="Screenshot of selecting the create a new Google Cloud project radio button.":::

  A new Google Cloud project will be created and linked to your Play Console developer account.

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

1. In the **Create OAuth client ID** page, choose **Desktop app** in the **Application type** drop down, enter **Visual Studio** in the **Name** field, and then select the **CREATE** button:

    :::image type="content" source="media/publish/vs/credentials-create-oauth.png" alt-text="Screenshot of creating OAuth credentials.":::

1. In the **OAuth client created** dialog, select the **DOWNLOAD JSON** data button to download your client ID and client secret. Then click the **OK** button to dismiss the dialog.

## Ensure correct package format

To publish a .NET MAUI Android app for Google Play distribution requires that your app package format is AAB, which is the default package format for release builds. To verify that your package format is set correctly:

1. In **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **Android > Options** tab and ensure that the value of the **Release** field is set to **bundle**:

    :::image type="content" source="media/publish/vs/google-play-ensure-package-format.png" alt-text="Screenshot of ensuring the bundle package format is setchanging the package format of a .NET MAUI Android app to APK.":::

## Publish

1. In the Visual Studio toolbar, use the **Solutions Configuration** drop-down to change from the debug configuration to the release configuration:

    :::image type="content" source="../deployment/media/publish/vs/release-configuration.png" alt-text="Select the release configuration in Visual Studio.":::

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Publish...**:

    :::image type="content" source="../deployment/media/publish/vs/publish-menu-item.png" alt-text="Select the publish menu item in Visual Studio.":::

    The **Archive Manager** will open and Visual Studio will begin to archive your app bundle:

    :::image type="content" source="../deployment/media/publish/vs/archive-manager.png" alt-text="Screenshot of the archive manager in Visual Studio.":::

    The archiving process signs the app with the certificate and provisioning profiles that you specified in the **iOS Bundle Signing** tab, for the selected solution configuration.

1. In the **Archive Manager**, once archiving has successfully completed, ensure your archive is selected and then select the **Distribute ...** button to begin the process of distributing your app:

    :::image type="content" source="../deployment/media/publish/vs/archive-manager-distribute.png" alt-text="Screenshot of the archive manager in Visual Studio once archiving is complete.":::

    The **Distribute - Select Channel** dialog will appear.

1. In the **Distribute - Select Channel** dialog, select the **Google Play** button:

    :::image type="content" source="media/publish/vs/distribution-select-channel-google-play.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::

1. In the **Distribute - Signing Identity** dialog, select the **+** button to add your signing identity:

    :::image type="content" source="media/publish/vs/distribution-add-signing-identity.png" alt-text="Screenshot of adding a signing identity in the distribution dialog.":::

    The **Create Android Keystore** dialog will appear.

    > [!NOTE]
    > An existing signing identity can be used by selecting the **Import** button.

1. In the **Create Android Keystore** dialog, provide the following information for your signing identity, known as a *keystore*, and then select the **Create** button:

    - Alias. Enter an identifying name for your key.
    - Password. Create and confirm a secure password for your key.
    - Validity. Set the length of time, in years, that your key will be valid.
    - Full name, organization unit, organization, city or locality, state or province, and country code. This information is not displayed in your app, but is included in your certificate.

    :::image type="content" source="media/publish/vs/create-android-keystore.png" alt-text="Screenshot of creating an Android keystore.":::

    A new keystore, which contains a new certificate, will be saved to **C:\Users\{Username}\AppData\Local\Xamarin\Mono for Android\Keystore\{Alias}\{Alias}.keystore**.

    > [!IMPORTANT]
    > The keystore and password isn't saved to your Visual Studio solution. Therefore, ensure you back up this data. If you lose it you'll be unable to sign your app with the same signing identity.  

1. In the **Distribute - Signing Identity** dialog, select your signing identity and select the **Continue** button:

    :::image type="content" source="media/publish/vs/signing-identity-continue.png" alt-text="Screenshot of selecting your newly created signing identity.":::

1. In the **Distribute - Google Play Account** dialog, select the **+** button to add your Google Play Account details:

    :::image type="content" source="media/publish/vs/distribution-add-google-play-account.png" alt-text="Screenshot of adding a Google Play account in the distribution dialog.":::

1. In the **Register Google API Access** dialog, enter a description and your OAuth client ID and client secret, and then click the **Register** button:

    :::image type="content" source="media/publish/vs/register-google-api-access.png" alt-text="Screenshot of registering your OAuth client ID and client secret in Visual Studio.":::

    > [!NOTE]
    > The account description makes it possible to register more than one Google Play account and upload apps to different Google Play accounts.

    A web browser will open.

1. In the web browser, sign into your Google Play Developer account. After you sign in, a message may be displayed telling you that Google hasn't verified the app. Select the **Continue** button:

    :::image type="content" source="media/publish/vs/google-sign-in-continue.png" alt-text="Screenshot of Google sign in saying the app hasn't been verified.":::

1. In the web browser, select the **Continue** button to authorize the app:

    :::image type="content" source="media/publish/vs/google-api-access-authorize.png" alt-text="Screenshot of authorizing Google API access.":::

  The web browser will receive a verification code.

  > [!IMPORTANT]
  > Don't close the web browser.

1. In Visual Studio, in the **Distribute - Google Play Account** dialog, select **Continue**:

    :::image type="content" source="media/publish/vs/distribution-google-api-access-authorized.png" alt-text="Screenshot of authorized Google API access in Visual Studio.":::

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

## Manually upload to Google Play

The first time an AAB is submitted to Google Play, it must be manually uploaded through the Google Play Console. This enables Google Play to match the signature of the key on all future bundles to the original key used for the first version of the app.

Steps:

1. Create an ad-hoc AAB. Although the app will be distributed via Google Play it must be manually uploaded the first time (otherwise error).


## Upload to Google Play from Visual Studio
