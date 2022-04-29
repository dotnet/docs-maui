---
title: "Launcher"
description: "Learn how to use the .NET MAUI Launcher class in the Microsoft.Maui.Essentials namespace, which can open another application by URI."
ms.date: 08/20/2019
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Launcher

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Launcher` class. This class enables an application to open a URI by the system. This is often used when deep linking into another application's custom URI schemes. If you're looking to open the browser to a website, use the [Browser](open-browser.md) API instead.

## Get started

[!INCLUDE [get-started](../essentials/includes/get-started.md)]

[!INCLUDE [essentials-namespace](../essentials/includes/essentials-namespace.md)]

To access the launcher functionality, the following platform-specific setup is required.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

No setup is required.

# [iOS](#tab/ios)

In iOS 9 and greater, Apple restricts what schemes an application can query for. To specify which schemes you would like to use, you must specify `LSApplicationQueriesSchemes` in your _Info.plist_ file:

```xml
<key>LSApplicationQueriesSchemes</key>
<array>
    <string>lyft</string>  
    <string>fb</string>
</array>
```

The `<string>` elements are the URI schemes preregistered with your app. You can't query for schemes outside of this list.

# [Windows](#tab/windows)

No setup is required.

-----
<!-- markdownlint-enable MD025 -->

## Open another app

To use the Launcher functionality, call the `Launcher.OpenAsync` method and pass in a `string` or `Uri` representing the app to open. Optionally, the `Launcher.CanOpenAsync` method can be used to check if the URI scheme can be handled by an app on the device. The following code demonstrates how to check if a URI scheme is supported or not, and then opens the URI:

```csharp
public class LauncherTest
{
    public async Task OpenRideShareAsync()
    {
        var supportsUri = await Launcher.CanOpenAsync("lyft://");

        if (supportsUri)
            await Launcher.OpenAsync("lyft://ridetype?id=lyft_line");
    }
}
```

The previous code example can be simplified by using the `TryOpenAsync`, which checks if the URI scheme can be opened, before opening it:

```csharp
public class LauncherTest
{
    public async Task<bool> OpenRideShareAsync() =>
        await Launcher.TryOpenAsync("lyft://ridetype?id=lyft_line");
}
```

## Open another app via a file

The launcher can also be used to open an app with a selected file. .NET MAUI automatically detects the file type (MIME), and opens the default app for that file type. If more than one app is registered with the file type, an app selection popover is shown to the user.

The following code example writes text to a file, and opens the text file with the launcher:

```csharp
public class LauncherTest
{
    public async Task OpenTextFile()
    {
        string popoverTitle = "Read text file";
        string name = "File.txt";
        string file = System.IO.Path.Combine(FileSystem.CacheDirectory, name);

        System.IO.File.WriteAllText(file, "Hello World");

        await Launcher.OpenAsync(new OpenFileRequest(popoverTitle, new ReadOnlyFile(file)));
    }
}
```

## Set the launcher location

[!INCLUDE [ios-PresentationSourceBounds](includes/ios-PresentationSourceBounds.md)]

## Platform differences

This section describes the platform-specific differences with the launcher API.

<!-- markdownlint-disable MD025 -->
<!-- markdownlint-disable MD024 -->
# [Android](#tab/android)

The `Task` returned from `CanOpenAsync` completes immediately.

# [iOS](#tab/ios)

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

## API

- [Launcher source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Launcher)
<!-- - [Launcher API documentation](xref:Microsoft.Maui.Essentials.Launcher)-->
