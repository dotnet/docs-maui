---
title: "Launcher"
description: "Learn how to use the .NET MAUI ILauncher interface in the Microsoft.Maui.ApplicationModel namespace, which can open another application by URI."
ms.date: 03/24/2025
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel"]
---

# Launcher

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.ILauncher> interface. This interface enables an application to open a URI by the system. This way of opening an application is often used when deep linking into another application's custom URI schemes.

The default implementation of the `ILauncher` interface is available through the <xref:Microsoft.Maui.ApplicationModel.Launcher.Default?displayProperty=nameWithType> property. Both the `ILauncher` interface and `Launcher` class are contained in the `Microsoft.Maui.ApplicationModel` namespace.

> [!IMPORTANT]
> To open the browser to a website, use the [Browser](open-browser.md) API instead.

## Get started

To access the launcher functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

If you want to use deep links to open other Android apps you should define an intent filter in your app. This can be achieved by adding the following XML to the _Platforms/Android/AndroidManifest.xml_ file:

```xml
<activity android:name="appName" android:exported="true">
    <intent-filter>
       <action android:name="android.intent.action.VIEW" />
       <category android:name="android.intent.category.DEFAULT" />
       <category android:name="android.intent.category.BROWSABLE" />
       <data android:scheme="lyft"/>
       <data android:scheme="fb"/>
       </intent-filter>
</activity>
```

The `<data>` elements are the URI schemes pre-registered with your app. You can't use schemes that aren't defined in the intent filter.

To make your app browsable from other apps declare a `<data>` element with the `android:scheme` attribute:

```xml
<data android:scheme="appName"/>
```

# [iOS/Mac Catalyst](#tab/macios)

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

To use the Launcher functionality, call the <xref:Microsoft.Maui.ApplicationModel.ILauncher.OpenAsync%2A?displayProperty=nameWithType> method and pass in a <xref:System.String> or <xref:System.Uri> representing the app to open. Optionally, the <xref:Microsoft.Maui.ApplicationModel.ILauncher.CanOpenAsync(System.Uri)?displayProperty=nameWithType> method can be used to check if the URI scheme can be handled by an app on the device. The following code demonstrates how to check if a URI scheme is supported or not, and then opens the URI:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open":::

The previous code example can be simplified by using the <xref:Microsoft.Maui.ApplicationModel.ILauncher.TryOpenAsync(System.Uri)>, which checks if the URI scheme can be opened, before opening it:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open_try":::

## Open another app via a file

The launcher can also be used to open an app with a selected file. .NET MAUI automatically detects the file type (MIME), and opens the default app for that file type. If more than one app is registered with the file type, an app selection popover is shown to the user.

The following code example writes text to a file, and opens the text file with the launcher:

:::code language="csharp" source="../snippets/shared_1/AppModelPage.xaml.cs" id="launcher_open_file":::

### Control file locations

[!INCLUDE [android-fileproviderpaths](../includes/android-fileproviderpaths.md)]

## Set the launcher location

[!INCLUDE [ios-PresentationSourceBounds](../includes/ios-PresentationSourceBounds.md)]

## Platform differences

This section describes the platform-specific differences with the launcher API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

The <xref:System.Threading.Tasks.Task> returned from <xref:Microsoft.Maui.ApplicationModel.Launcher.CanOpenAsync%2A> completes immediately.

# [iOS/Mac Catalyst](#tab/macios)

The <xref:System.Threading.Tasks.Task> returned from <xref:Microsoft.Maui.ApplicationModel.Launcher.CanOpenAsync%2A> completes immediately.

If the target app on the device has never been opened by your application with <xref:Microsoft.Maui.ApplicationModel.Launcher.OpenAsync%2A>, iOS displays a popover to the user, requesting permission to allow this action.

For more information about the iOS implementation, see <xref:UIKit.UIApplication.CanOpenUrl%2A>.

# [Windows](#tab/windows)

No platform differences.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
