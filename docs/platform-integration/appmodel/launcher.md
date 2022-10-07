---
title: "Launcher"
description: "Learn how to use the .NET MAUI ILauncher interface in the Microsoft.Maui.ApplicationModel namespace, which can open another application by URI."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Launcher

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `ILauncher` interface. This interface enables an application to open a URI by the system. This way of opening is often used when deep linking into another application's custom URI schemes.

The default implementation of the `ILauncher` interface is available through the `Launcher.Default` property. Both the `ILauncher` interface and `Launcher` class are contained in the `Microsoft.Maui.ApplicationModel` namespace.

> [!IMPORTANT]
> To open the browser to a website, use the [Browser](open-browser.md) API instead.

## Get started

To access the launcher functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No setup is required.

# [iOS\macOS](#tab/ios)

Apple requires that you define the schemes you want to use. Add the `LSApplicationQueriesSchemes` key and schemes to the _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist_ files:

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
    <string>lyft</string>  
    <string>fb</string>
</array>
```

The `<string>` elements are the URI schemes preregistered with your app. You can't use schemes outside of this list.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Open another app

To use the Launcher functionality, call the `Launcher.OpenAsync` method and pass in a `string` or `Uri` representing the app to open. Optionally, the `Launcher.CanOpenAsync` method can be used to check if the URI scheme can be handled by an app on the device. The following code demonstrates how to check if a URI scheme is supported or not, and then opens the URI:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open":::

The previous code example can be simplified by using the `TryOpenAsync`, which checks if the URI scheme can be opened, before opening it:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open_try":::

## Open another app via a file

The launcher can also be used to open an app with a selected file. .NET MAUI automatically detects the file type (MIME), and opens the default app for that file type. If more than one app is registered with the file type, an app selection popover is shown to the user.

The following code example writes text to a file, and opens the text file with the launcher:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open_file":::

## Set the launcher location

[!INCLUDE [ios-PresentationSourceBounds](../includes/ios-PresentationSourceBounds.md)]

## Platform differences

This section describes the platform-specific differences with the launcher API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

The `Task` returned from `CanOpenAsync` completes immediately.

# [iOS\macOS](#tab/ios)

The `Task` returned from `CanOpenAsync` completes immediately.

If the target app on the device has never been opened by your application with `OpenAsync`, iOS displays a popover to the user, requesting permission to allow this action.

<!-- TODO: where does this go?
For more information about the iOS implementation, see [TITLE](xref:UIKit.UIApplication.CanOpenUrl*)
-->

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
