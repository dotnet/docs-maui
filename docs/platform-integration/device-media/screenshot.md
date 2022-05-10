---
title: "Screenshot"
description: "Learn how to take a screenshot of the app, using .NET MAUI. The Screenshot class in Microsoft.Maui.Essentials namespace is used to capture of the current displayed screen of the app."
ms.date: 08/27/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Screenshot

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Screenshot` class. This class lets you take a capture of the current displayed screen of the app.

## Get started

[!INCLUDE [get-started](../includes/get-started.md)]

[!INCLUDE [essentials-namespace](../includes/essentials-namespace.md)]

## Using Screenshot

To capture a screenshot of the current app, use the `ScreenShot.CaptureAsync` method. This method returns a `ScreenshotResult`, which contains information about the capture, such as the width and height of the screenshot. `ScreenshotResult` also includes a `Stream` property that is used to convert the screenshot into an image object for use by your app. The following example demonstrates a method that captures a screenshot and returns it as an `ImageSource`.

```csharp
async Task<ImageSource> CaptureScreenshot()
{
    ScreenshotResult screenshot = await Screenshot.CaptureAsync();
    Stream stream = await screenshot.OpenReadAsync();

    return ImageSource.FromStream(() => stream);
}
```

## Limitations

Not all views support being captured at a screen level, such as an OpenGL view.
