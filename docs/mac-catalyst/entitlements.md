---
title: "Mac Catalyst entitlements"
description: "Learn how to add entitlements to your .NET MAUI Mac Catalyst app, to request access to specific system resources or user data."
ms.date: 03/28/2023
---

# Mac Catalyst entitlements

On Mac Catalyst, .NET Multi-platform App UI (.NET MAUI) apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. *Entitlements* are used to request the expansion of the sandbox to give your app additional capabilities, such as integration with Siri. Any entitlements used by your app must be specified in the app's *Entitlements.plist* file. For more information about entitlements, see [Entitlements](https://developer.apple.com/documentation/bundleresources/entitlements) on developer.apple.com.

In addition to specifying entitlements, the *Entitlements.plist* file is used to code sign the app. When code signing your app, the entitlements file is combined with information from your Apple Developer Account, and other project information to apply a final set of entitlements to your app.

Entitlements are closely related to the concept of capabilities. They both request the expansion of the sandbox your app runs in, to give it additional capabilities. Entitlements are typically added when developing your app, while capabilities are typically added when code signing your app for distribution. For more information about capabilities, see [Capabilities](capabilities.md).

> [!IMPORTANT]
> An *Entitlements.plist* file isn't linked to an Apple Developer Account. Therefore, when creating a provisioning profile for your app, you should ensure that any entitlements used by your app are also specified as capabilities in its provisioning profile. For more information, see [Capabilities](capabilities.md).

## Add an Entitlements.plist file

To add a new entitlements file to your .NET MAUI app project, add a new XML file named *Entitlements.plist* to the *Platforms/MacCatalyst* folder of your app project. Then add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
</dict>
</plist>
```

## Set entitlements

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

Entitlements can be configured in Visual Studio for Mac by double-clicking the *Entitlements.plist* file to open it in the entitlements editor:

1. In Visual Studio for Mac's **Solution Window**, double-click the *Entitlements.plist* file from the *Platforms > MacCatalyst* folder of your .NET MAUI app project to open it in the entitlements editor. Then, change from the **Source** view to the **Entitlements** view:

    :::image type="content" source="media/entitlements/editor-source.png" alt-text="Visual Studio for Mac iOS entitlements editor source view.":::

1. In the entitlements editor, select and configure any entitlements required for your app:

    :::image type="content" source="media/entitlements/editor-entitlements.png" alt-text="Visual Studio for Mac iOS entitlements editor entitlements view.":::

1. Save the changes to your *Entitlements.plist* file to add the entitlement key/value pairs to the file.

It may also be necessary to set privacy keys in *Info.plist*, for certain entitlements.

## Consume entitlements

[!INCLUDE [Visual Studio for Mac end of life](~/includes/vsmac-eol.md)]

A .NET MAUI Mac Catalyst app must be configured to consume the entitlements defined in the *Entitlements.plist* file:

1. In Visual Studio for Mac's **Solution Window**, right-click on your .NET MAUI app project and select **Properties**.
1. In the **Project Properties** window, select the **Build > MacCatalyst > Bundle Signing** tab and click the **...** button next to the **Custom Entitlements** field:

    :::image type="content" source="media/entitlements/set-custom-entitlements.png" alt-text="Visual Studio for Mac bundle signing properties.":::

1. In the dialog, navigate to the folder containing your *Entitlements.plist* file, select the file, and click the **Open** button.
1. In the **Project Properties** window, the **Custom Entitlements** field will be populated with your entitlements file:

    :::image type="content" source="media/entitlements/custom-entitlements-set.png" alt-text="Visual Studio for Mac custom entitlements field set.":::

1. In the **Project Properties** window, click the **OK** button to close the window.

> [!IMPORTANT]
> The custom entitlements field must be set separately for each build configuration for your app.

[!INCLUDE [Entitlements key reference](../macios/includes/entitlements-reference.md)]
