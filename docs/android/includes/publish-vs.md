---
ms.date: 05/09/2023
ms.topic: include
---

To build and sign your app in Visual Studio:

1. In the Visual Studio toolbar, use the **Debug Target** drop-down to select **Android Emulators** and then your chosen emulator:

    :::image type="content" source="../deployment/media/publish/vs/select-android-deployment.png" alt-text="Select an Android deployment target in Visual Studio.":::

1. In the Visual Studio toolbar, use the **Solutions Configuration** drop-down to change from the debug configuration to the release configuration:

    :::image type="content" source="../deployment/media/publish/vs/release-configuration.png" alt-text="Select the release configuration in Visual Studio.":::

1. In **Solution Explorer**, right-click on your .NET MAUI app project and select **Publish...**:

    :::image type="content" source="../deployment/media/publish/vs/publish-menu-item.png" alt-text="Select the publish menu item in Visual Studio.":::

    The **Archive Manager** will open and Visual Studio will begin to archive your app bundle:

    :::image type="content" source="../deployment/media/publish/vs/archive-manager.png" alt-text="Screenshot of the archive manager in Visual Studio.":::

1. In the **Archive Manager**, once archiving has successfully completed, ensure your archive is selected and then select the **Distribute ...** button to begin the process of distributing your app:

    :::image type="content" source="../deployment/media/publish/vs/archive-manager-distribute.png" alt-text="Screenshot of the archive manager in Visual Studio once archiving is complete.":::

    The **Distribute - Select Channel** dialog will appear.
