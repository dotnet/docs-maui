---
title: "Clipboard"
description: "Learn how to use the .NET MAUI IClipboard interface in the Microsoft.Maui.ApplicationModel.DataTransfer namespace, which lets you copy and paste text to the system clipboard."
ms.date: 02/02/2023
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.DataTransfer"]
---

# Clipboard

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/platformintegration-essentials)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard> interface. With this interface, you can copy and paste text to and from the system clipboard.

The default implementation of the `IClipboard` interface is available through the <xref:Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default?displayProperty=nameWithType> property. Both the `IClipboard` interface and `Clipboard` class are contained in the `Microsoft.Maui.ApplicationModel.DataTransfer` namespace.

> [!TIP]
> Access to the clipboard must be done on the main user interface thread. For more information on how to invoke methods on the main user interface thread, see [MainThread](../appmodel/main-thread.md).

## Using Clipboard

 Access to the clipboard is limited to string data. You can check if the clipboard contains data, set or clear the data, and read the data. The <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard.ClipboardContentChanged> event is raised whenever the clipboard data changes.

The following code example demonstrates using a button to set the clipboard data:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_set":::

The following code example demonstrates using a button to read the clipboard data. The code first checks if the clipboard has data, read that data, and then uses a `null` value with <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard.SetTextAsync%2A> to clear the clipboard:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_read":::

## Clear the clipboard

You can clear the clipboard by passing `null` to the <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard.SetTextAsync%2A> method, as the following code example demonstrates:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_clear":::

## Detecting clipboard changes

The <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard> interface provides the <xref:Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard.ClipboardContentChanged> event. When this event is raised, the clipboard content has changed. The following code example adds a handler to the event when the content page is loaded:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_event":::
