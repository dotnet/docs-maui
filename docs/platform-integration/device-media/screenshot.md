---
title: "Screenshot"
description: "Learn how to use the IScreenshot interface in the Microsoft.Maui.Media namespace, to capture of the current displayed screen of the app."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Media", "ScreenShot"]
---

# Screenshot

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Media.IScreenshot> interface. This interface lets you take a capture of the current displayed screen of the app.

The default implementation of the `IScreenshot` interface is available through the <xref:Microsoft.Maui.Media.Screenshot.Default?displayProperty=nameWithType> property. Both the `IScreenshot` interface and `Screenshot` class are contained in the `Microsoft.Maui.Media` namespace.

## Capture a screenshot

To capture a screenshot of the current app, use the <xref:Microsoft.Maui.Media.IScreenshot.CaptureAsync> method. This method returns a <xref:Microsoft.Maui.Media.IScreenshotResult>, which contains information about the capture, such as the width and height of the screenshot. The following example demonstrates a method that captures a screenshot and returns it as an <xref:System.Windows.Media.ImageSource>.

:::code language="csharp" source="../snippets/shared_1/MediaPage.cs" id="screenshot":::
