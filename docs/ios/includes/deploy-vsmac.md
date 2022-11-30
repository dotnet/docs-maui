1. In the **Solution Window**, double-click the **Info.plist** file from the **Platforms > iOS** folder of your .NET MAUI app project to open it in the editor.
1. In the **Info.plist** editor, change from the **Source** view to the **Application** view:

    :::image type="content" source="~/ios/device-provisioning/media/automatic-provisioning/vsmac/source-view.png" alt-text="Screenshot of source view in Info.plist editor in Visual Studio for Mac.":::

1. In the **Deployment Info** section of the **Application** view, set the deployment target to the minimum supported iOS version for your app, using the **Deployment target** drop-down:

    :::image type="content" source="~/ios/device-provisioning/media/automatic-provisioning/vsmac/deployment-target.png" alt-text="Screenshot of deployment target in Info.plist editor in Visual Studio for Mac.":::

    > [!IMPORTANT]
    > The deployment target value must be identical to the **Minimum iOS version** value that's defined in the **Project Properties** dialog under the **Build > Target Platforms** tab.

1. Ensure that your iOS device is connected to Visual Studio for Mac via USB or WiFi.
1. In The Visual Studio for Mac toolbar, ensure that the debug target is set to your connected iOS device, and then press the **Play** button to launch the app on your device.
