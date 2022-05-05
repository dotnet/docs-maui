---
title: "Clipboard"
description: "Learn how to use the .NET MAUI Clipboard class in the Microsoft.Maui.ApplicationModel.DataTransfer namespace, which lets you copy and paste text to the system clipboard"
ms.date: 05/04/2022
no-loc: ["Microsoft.Maui", "Microsoft.Maui.ApplicationModel.DataTransfer"]
---

# Clipboard

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `Clipboard` class. With this class, you can copy and paste text to and from the system clipboard.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

The `Clipboard` class is available in the `Microsoft.Maui.ApplicationModel.DataTransfer` namespace.

> [!TIP]
> Access to the clipboard must be done on the main user interface thread. For more information on how to invoke methods on the main user interface thread, see [MainThread](../appmodel/main-thread.md).

## Using Clipboard

The clipboard is accessed through default implementation of the `IClipboard` interface, available from the `Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default` property. Access to the clipboard is limited to string data. You can check if the clipboard contains data, set or clear the data, and read the data. The `ClipboardContentChanged` event is raised whenever the clipboard data changes.

The following code example demonstrates using a button to set the clipboard data:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_set":::

The following code example demonstrates using a button to read the clipboard data. The code first checks if the clipboard has data, read that data, and then uses a `null` value with `SetTextAsync` to clear the clipboard:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_read":::

## Clear the clipboard

You can clear the clipboard by passing `null` to the `SetTextAsync` method, as the following code example demonstrates:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_clear":::

## Detecting clipboard changes

The `IClipboard` interface provides the `ClipboardContentChanged` event. When this event is raised, the clipboard content has changed. The following code example adds a handler to the event when the content page is loaded:

:::code language="csharp" source="../snippets/shared_1/DataPage.xaml.cs" id="clipboard_event":::

## API

- [Clipboard source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Clipboard)
<!-- - [Clipboard API documentation](xref:Microsoft.Maui.Essentials.Clipboard)-->
