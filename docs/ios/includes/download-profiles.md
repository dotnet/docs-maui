---
ms.date: 02/24/2023
ms.topic: include
---

## Download provisioning profiles in Visual Studio

After you create a distribution provisioning profile in your Apple Developer Account, Visual Studio can download it so that it's available for signing your app.

<!-- markdownlint-disable MD025 -->
# [Visual Studio](#tab/vs)
<!-- markdownlint-enable MD025 -->

1. In Visual Studio, go to **Tools > Options > Xamarin > Apple Accounts**.
1. In the **Apple Developer Accounts** dialog, select your team and click **View Details**.
1. In the **Details** dialog, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio to refresh the list.
1. In the **Details** dialog, click **Download All Profiles**.

The provisioning profiles are downloaded on Windows, and exported to your Mac build host if the IDE is paired to it. For more information, see [Pair to Mac for iOS development](~/ios/pair-to-mac.md).

<!-- markdownlint-disable MD025 -->
# [Visual Studio for Mac](#tab/vsmac)
<!-- markdownlint-enable MD025 -->

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

1. In Visual Studio for Mac, go to **Visual Studio > Preferences > Publishing > Apple Developer Account**.
1. In the **Apple Developer Accounts** window, select your team and click **View Details**.
1. In the **Details** window, verify that the new profile appears in the **Provisioning Profiles** list. You may need to restart Visual Studio for Mac to refresh the list.
1. In the **Details** dialog, click **Download All Profiles**.

The development provisioning profile is now available for use.

---
