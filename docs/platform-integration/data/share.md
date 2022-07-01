---
title: "Share"
description: "Learn how to use the .NET MAUI Share class, which can share data, such as web links, to other applications on the device."
ms.date: 05/23/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.DataTransfer"]
---

# Share

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `Share` class. This class provides an API to send data, such as text or web links, to the devices share function.

The `Share` class is available in the `Microsoft.Maui.ApplicationModel.DataTransfer` namespace.

When a share request is made, the device displays a share window, prompting the user to choose an app to share with:

:::image type="content" source="media/share/share.png" alt-text="Share from your app to a different app":::

## Get started

To access the **Share** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No setup is required.

# [iOS\macOS](#tab/ios)

If your application is going to share media files, such as photos and videos, you must add the following keys to your _Platforms/iOS/Info.plist_ and _Platforms/MacCatalyst/Info.plist files:

```xml
<key>NSPhotoLibraryAddUsageDescription</key>
<string>This app needs access to the photo gallery to save photos and videos.</string>
<key>NSPhotoLibraryUsageDescription</key>
<string>This app needs access to the photo gallery to save photos and videos.</string>
```

The `<string>` elements represent the text shown to your users when permission is requested. Make sure that you change the text to something specific to your application.

# [Windows](#tab/windows)

No setup is required.

-----

## Share text and links

The share functionality works by calling the `RequestAsync` method with a data payload that includes information to share to other applications. `Text` and `Uri` can be mixed and each platform will handle filtering based on content.

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="share_text_uri":::

## Share a file

You can also share files to other applications on the device. .NET MAUI automatically detects the file type (MIME) and requests a share. However, operating systems may restrict which types of files can be shared.

The following code example writes a text file to the device, and then requests a share:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="share_file":::

## Share multiple files

Sharing multiple files is slightly different from sharing a single file. Instead of using the `File` property of the share request, use the `Files` property:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="share_file_multiple":::

## Presentation location

[!INCLUDE [ios-PresentationSourceBounds](../includes/ios-PresentationSourceBounds.md)]

## Platform differences

This section describes the platform-specific differences with the share API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

- The `Subject` property is used for the desired subject of a message.

# [iOS\macOS](#tab/ios)

- The `Subject` property isn't used.

# [Windows](#tab/windows)

- The `Title` property will default to the application name if not set.
- The `Subject` property isn't used.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->
