---
title: "Screenshot"
description: "Learn how to use the Screenshot class in the Microsoft.Maui.Media namespace, to capture of the current displayed screen of the app."
ms.date: 05/11/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "ScreenShot"]
---

# Screenshot

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IScreenshot` interface. This interfaces lets you take a capture of the current displayed screen of the app. The `IScreenshot` interface is exposed through the `Screenshot.Default` property.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `Screenshot` and `IScreenshot` types are available in the `Microsoft.Maui.Media` namespace.

## Capture a screenshot

To capture a screenshot of the current app, use the `CaptureAsync` method. This method returns a `IScreenshotResult`, which contains information about the capture, such as the width and height of the screenshot. `IScreenshotResult` also includes a `Stream` property that is used to convert the screenshot into an image object for use by your app. The following example demonstrates a method that captures a screenshot and returns it as an `ImageSource`.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="screenshot":::

## Limitations

Not all views support being captured at a screen level, such as an OpenGL view.
