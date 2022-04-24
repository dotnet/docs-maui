---
title: "Share"
description: "Learn how to use the .NET MAUI Share class, which can share data, such as web links, to other applications on the device."
ms.date: 09/13/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Share

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Share` class. This class provides an API to send date, such as text or web links, to the devices share function.

When a share request is made, the device displays a share window, prompting the user to choose an app to share with:

:::image type="content" source="images/share.png" alt-text="Share from your app to a different app":::

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To access the **Share** functionality, the following platform-specific setup is required:

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No setup is required.

# [iOS](#tab/ios)

If your application is going to share media files, such as photos and videos, you must add the following keys to your _Info.plist_ file:

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

```csharp
public async Task ShareText(string text)
{
    await Share.RequestAsync(new ShareTextRequest
    {
        Text = text,
        Title = "Share Text"
    });
}

public async Task ShareUri(string uri)
{
    await Share.RequestAsync(new ShareTextRequest
    {
        Uri = uri,
        Title = "Share Web Link"
    });
}
```

## Share a file

You can also share files to other applications on the device. .NET MAUI automatically detects the file type (MIME) and requests a share. However, operating systems may restrict which types of files can be shared.

The following code example writes a text file to the device, and then requests a share:

```csharp
string fn = "Attachment.txt";
string file = Path.Combine(FileSystem.CacheDirectory, fn);

File.WriteAllText(file, "Hello World");

await Share.RequestAsync(new ShareFileRequest
{
    Title = "Share text file",
    File = new ShareFile(file)
});
```

## Share multiple files

Sharing multiple files is slightly different from sharing a single file. Instead of using the `File` property of the share request, use the `Files` property:

```csharp
string file1 = Path.Combine(FileSystem.CacheDirectory, "Attachment1.txt");
string file2 = Path.Combine(FileSystem.CacheDirectory, "Attachment2.txt");
            
File.WriteAllText(file1, "Content 1");
File.WriteAllText(file2, "Content 2");

await Share.RequestAsync(new ShareMultipleFilesRequest
{
    Title = "Share multiple files",
    Files = new List<ShareFile> { new ShareFile(file1), new ShareFile(file2) }
});
```

## Presentation location

[!INCLUDE [ios-PresentationSourceBounds](includes/ios-PresentationSourceBounds.md)]

## Platform differences

This section describes the platform-specific differences with the share API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

- The `Subject` property is used for the desired subject of a message.

# [iOS](#tab/ios)

- The `Subject` property isn't used.

# [Windows](#tab/windows)

- The `Title` property will default to the application name if not set.
- The `Subject` property isn't used.

-----
<!-- markdownlint-enable MD024 -->
<!-- markdownlint-enable MD025 -->

## API

- [Share source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Share)
<!-- - [Share API documentation](xref:Microsoft.Maui.Essentials.Share)-->
