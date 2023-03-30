---
ms.topic: include
ms.date: 03/28/2023
---

### Create an App ID with an app service

An App ID is similar to a reverse-DNS string, that uniquely identifies an app, and is required to identify the app that you are distributing. The App ID should be identical to the bundle identifier for your app.

<!-- markdownlint-disable MD032 -->
> [!IMPORTANT]
> The bundle identifier for a .NET MAUI app is stored in the project file as the **Application ID** property:
> - In Visual Studio, in **Solution Explorer** right-click on your .NET MAUI app project and select **Properties**. Then, navigate to the **MAUI Shared > General** tab. The **Application ID** field lists the bundle identifier.
> - In Visual Studio for Mac, in the **Solution Window**, right-click on your .NET MAUI app project and select **Properties**. Then, in the **Project Properties** window, select the **Build > App Info** tab. The **Application ID** field lists the bundle identifier.
>
> When the value of the **Application ID** field is updated, the value of the **Bundle identifier** in the **Info.plist** will be automatically updated.
<!-- markdownlint-enable MD032 -->

There are two types of App ID - explicit and wildcard. An explicit App ID is unique to a single app, and typically takes the form `com.domainname.myid`. An explicit App ID allows the installation of one app, with a matching bundle identifier, to a device. Explicit App IDs are required for apps that enable app-specific capabilities.

An explicit App ID can be created with the following steps:

1. In a web browser, go to the [Identifiers](https://developer.apple.com/account/resources/identifiers/list) section of your Apple Developer Account and click the **+** button.
1. In the **Register a new identifier** page, select **App IDs** and click the **Continue** button.
1. In the **Register a new identifier** page, select the **App** type and click the **Continue** button.
1. In the **Register an App ID** page, provide a **Description** and set the **Bundle ID** to **Explicit**. Then, enter an App ID in the format `com.domainname.myid`:

    :::image type="content" source="~/ios/media/capabilities/register-app-id.png" alt-text="Screenshot of new App ID registration page with required fields populated.":::

1. In the **Register an App ID** page, enable your required capabilities under the **Capabilities** and **App Services** tabs:

    :::image type="content" source="~/ios/media/capabilities/enable-capabilities.png" alt-text="Screenshot of enabled capabilities.":::

1. In the **Register an App ID** page, click the **Continue** button.
1. In the **Confirm your App ID** page, review the information and then click the **Register** button. Provided that your App ID successfully registers, you'll be returned to the Identifiers section of your Apple Developer Account.
1. In the **Identifiers** page, click on the App ID you just created.
1. In the **Edit your App ID Configuration** page, any of your enabled capabilities that require additional setup will have a **Configure** button:

    :::image type="content" source="~/ios/media/capabilities/configure-capabilities.png" alt-text="Screenshot of editing capabilities.":::

    Click any **Configure** buttons to configure your enabled capabilities. For more information, see [Configure app capabilities](https://developer.apple.com/help/account/configure-app-capabilities/configure-apple-pay) on developer.apple.com.
