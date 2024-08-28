---
title: "Mac Catalyst entitlements"
description: "Learn how to add entitlements to your .NET MAUI Mac Catalyst app, to request access to specific system resources or user data."
ms.date: 08/28/2024
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

Entitlements can be configured in Visual Studio Code by double-clicking the *Entitlements.plist* file to open it in the editor and then entering the required XML for the entitlement. For more information see [Key reference](#key-reference).

> [!NOTE]
> It may also be necessary to set privacy keys in *Info.plist*, for certain entitlements.

## Consume entitlements

A .NET MAUI Mac Catalyst app must be configured to consume the entitlements defined in the *Entitlements.plist* file. This can be achieved by adding the `$(CodesignEntitlements)` build property to a property group in your app's *.csproj* file:

```xml
<PropertyGroup Condition="'$(Configuration)|$(TargetFramework)|$(Platform)'=='Debug|net8.0-maccatalyst|AnyCPU'">
  <CodesignEntitlements>Platforms\MacCatalyst\Entitlements.plist</CodesignEntitlements>
</PropertyGroup>
```

> [!IMPORTANT]
> The `$(CodesignEntitlements)` build property must be set separately for each build configuration for your app.

Alternatively, you can specify the entitlements file via the CLI when building and publishing your app. For more information, see [Publish a .NET MAUI Mac Catalyst app](~/mac-catalyst/deployment/index.md).

[!INCLUDE [Entitlements key reference](../macios/includes/entitlements-reference.md)]
