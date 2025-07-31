---
ms.date: 05/12/2023
ms.topic: include
ms.custom: sfi-image-nochange
---

<!-- markdownlint-disable MD029 -->
5. In the **Distribute - Select Channel** dialog, select the **Ad Hoc** button:

    :::image type="content" source="../deployment/media/publish/vs/distribution-select-channel-ad-hoc.png" alt-text="Screenshot of selecting a distribution channel in the distribution dialog.":::
    <!-- markdownlint-enable MD029 -->

1. In the **Distribute - Signing Identity** dialog, select the **+** button to create a new signing identity:

    :::image type="content" source="../deployment/media/publish/vs/create-new-ad-hoc-signing-identity.png" alt-text="Screenshot of creating a new signing identity in the distribution dialog.":::

    The **Create Android Keystore** dialog will appear.

    > [!NOTE]
    > Alternatively, an existing signing identity can be used by selecting the **Import** button.

1. In the **Create Android Keystore** dialog, enter the required information to create a new signing identity, known as a *keystore*, and then select the **Create** button:

    - Alias. Enter an identifying name for your key.
    - Password. Create and confirm a secure password for your key.
    - Validity. Set the length of time, in years, that your key will be valid.
    - Full name, organization unit, organization, city or locality, state or province, and country code. This information is not displayed in your app, but is included in your certificate.

    :::image type="content" source="../deployment/media/publish/vs/create-android-keystore.png" alt-text="Screenshot of creating an Android keystore.":::

    A new keystore, which contains a new certificate, will be saved to **C:\Users\{Username}\AppData\Local\Xamarin\Mono for Android\Keystore\{Alias}\{Alias}.keystore**.

    > [!IMPORTANT]
    > The keystore and password isn't saved to your Visual Studio solution. Therefore, ensure you back up this data. If you lose it you'll be unable to sign your app with the same signing identity.  

1. In the **Distribute - Signing Identity** dialog, select your newly created signing identity and select the **Save As** button:

    :::image type="content" source="../deployment/media/publish/vs/save-ad-hoc.png" alt-text="Screenshot of publishing your app for ad-hoc distribution.":::

    The *Archive Manager* displays the publishing process.

1. In the **Save As** dialog, confirm the location and file name for your package is correct and select the **Save** button.
1. In the **Signing Password** dialog, enter your signing identity password and select the **OK** button:

    :::image type="content" source="../deployment/media/publish/vs/keystore-password.png" alt-text="Screenshot of entering your signing identity password.":::

1. In the *Archive Manager*, select the **Open Distribution** button once the publishing process completes:

    :::image type="content" source="../deployment/media/publish/vs/ad-hoc-open-distribution.png" alt-text="Screenshot of opening the folder containing your published Android app.":::

    Visual Studio will open the folder containing the published app.
