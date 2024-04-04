---
ms.topic: include
ms.date: 11/30/2022
---

### Create an App ID

An App ID is required to identify the app that you are distributing. An App ID is similar to a reverse-DNS string, that uniquely identifies an app, and should be identical to the bundle identifier for your app. You can use the same App ID that you used when deploying your app to a device for testing.

There are two types of App ID:

- Wildcard. A wildcard App ID allows you to use a single App ID to match multiple apps, and typically takes the form `com.domainname.*`. A wildcard App ID can be used to distribute multiple apps, and should be used for apps that do not enable app-specific capabilities.
- Explicit. An explicit App ID is unique to a single app, and typically takes the form `com.domainname.myid`. An explicit App ID allows the distribution of one app, with a matching bundle identifier. Explicit App IDs are typically used for apps that enable app-specific capabilities such as Apple Pay, or Game Center. For more information about capabilities, see [Capabilities](~/ios/capabilities.md).

To create a new App ID:

1. In your Apple Developer Account, navigate to **Certificates, IDs & Profiles**.
1. On the **Certificates, Identifiers & Profiles** page, select the **Identifiers** tab.
1. On the **Identifiers** page, click the **+** button to create a new App ID.
1. On the **Register a new identifier** page, select the **App IDs** radio button before clicking the **Continue** button:

    :::image type="content" source="../deployment/media/publish/create-app-id.png" alt-text="Create an App ID.":::

1. On the **Register a new identifier** page, select **App** before clicking the **Continue** button:

    :::image type="content" source="../deployment/media/publish/register-identifier.png" alt-text="Register an App ID.":::

1. On the **Register an App ID** page, enter a description, and select either the **Explicit** or **Wildcard** Bundle ID radio button. Then, enter the Bundle ID for your app in reverse DS format:

    :::image type="content" source="../deployment/media/publish/specify-bundle-id.png" alt-text="Specify the bundle identifier for the app.":::

    <!-- markdownlint-disable MD032 -->
    > [!IMPORTANT]
    > The Bundle ID you enter must correspond to the **Bundle identifier** in the *Info.plist* file in your app project.
    >
    > The bundle identifier for a .NET MAUI app is stored in the project file as the **Application ID** property:
    > - In Visual Studio, in **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.
    > - In Visual Studio for Mac, in the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > App Info** tab. The **Application ID** field lists the bundle identifier.
    >
    > When the value of the **Application ID** field is updated, the value of the **Bundle identifier** in the **Info.plist** will be automatically updated.
    <!-- markdownlint-enable MD032 -->

1. On the **Register an App ID** page, select any capabilities that the app uses. Any capabilities must be configured both on this page and in the *Entitlements.plist* file in your app project. For more information see [Capabilities](~/ios/capabilities.md) and [Entitlements](~/ios/entitlements.md).
1. On the **Register an App ID** page, click the **Continue** button.
1. On the **Confirm your App ID** page, click the **Register** button.
