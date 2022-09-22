---
title: "Entitlements and capabilities"
description: "Learn how to use entitlements and capabilities in a .NET MAUI iOS app to request access to specific system resources or user data."
ms.date: 05/09/2022
---

# Entitlements and capabilities

In iOS, apps run in a sandbox that provides a set of rules that limit access between the app and system resources or user data. *Entitlements* are used to request the expansion of the sandbox to give your app additional capabilities. Any entitlements used by your app must be specified in the app's entitlements file. For more information about entitlements, see [Entitlements](https://developer.apple.com/documentation/bundleresources/entitlements).

Apple provides *capabilities*, also known as *app services*, as a means of extending functionality and widening the scope of what iOS apps can do. Capabilities allow you to add a deeper integration with platform features to your app, such as integration with Siri. For more information about capabilities, see [Capabilities](https://developer.apple.com/documentation/xcode/capabilities).

To extend the capabilities of your app, an entitlement must be provided in your app's *Entitlements.plist* file. Entitlements are a key/value pair, and generally only one entitlement is required per capability. In addition to specifying entitlements, the *Entitlements.plist* file is used to sign the app.

> [!IMPORTANT]
> An *Entitlements.plist* file isn't linked to an Apple Developer Account. Therefore, any entitlements used by an app must also be specified when creating a provisioning profile for an app.

## Add an Entitlements.plist file

To add a new entitlements file to your .NET Multi-platform App UI (.NET MAUI) app project, add a new XML file named *Entitlements.plist* to the *Platforms\\iOS\\* folder of your app project. Then add the following XML to the file:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
</dict>
</plist>
```

## Set entitlements

Entitlements can be configured in Visual Studio by double-clicking the *Entitlements.plist* file to open it in the iOS Entitlements editor:

:::image type="content" source="media/entitlements/editor.png" alt-text="Visual Studio iOS entitlements editors.":::

When a .NET MAUI iOS app uses entitlements, the *Entitlements.plist* file must be referenced from the `<CodesignEntitlements>` node within a `<PropertyGroup>`. For more information, see [Add code signing data to your app project](overview.md#add-code-signing-data-to-your-app-project).
