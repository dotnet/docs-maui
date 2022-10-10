---
title: "Screenshot"
description: "Learn how to use the IScreenshot interface in the Microsoft.Maui.Media namespace, to capture of the current displayed screen of the app."
ms.date: 09/02/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "ScreenShot"]
---

# Screenshot

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IScreenshot` interface. This interface lets you take a capture of the current displayed screen of the app.

The default implementation of the `IScreenshot` interface is available through the `Screenshot.Default` property. Both the `IScreenshot` interface and `Screenshot` class are contained in the `Microsoft.Maui.Media` namespace.

## Capture a screenshot

To capture a screenshot of the current app, use the `CaptureAsync` method. This method returns a `IScreenshotResult`, which contains information about the capture, such as the width and height of the screenshot. `IScreenshotResult` also includes a `Stream` property that's used to convert the screenshot into an image object for use by your app. The following example demonstrates a method that captures a screenshot and returns it as an `ImageSource`.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="screenshot":::

## Limitations

Not all views support being captured at a screen level, such as an OpenGL view.
