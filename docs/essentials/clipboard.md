---
title: "Xamarin.Essentials: Clipboard"
description: "This document describes the Clipboard class in Xamarin.Essentials, which lets you copy and paste text to the system clipboard between applications."
author: jamesmontemagno
ms.author: jamont
ms.date: 01/06/2020
ms.custom: video
no-loc: [Xamarin.Forms, Xamarin.Essentials]
---

# Xamarin.Essentials: Clipboard

The **Clipboard** class lets you copy and paste text to the system clipboard between applications.

## Get started

[!include[](~/essentials/includes/get-started.md)]

## Using Clipboard

Add a reference to Xamarin.Essentials in your class:

```csharp
using Xamarin.Essentials;
```

To check if the **Clipboard** has text currently ready to be pasted:

```csharp
var hasText = Clipboard.HasText;
```

To set text to the **Clipboard**:

```csharp
await Clipboard.SetTextAsync("Hello World");
```

To read text from the **Clipboard**:

```csharp
var text = await Clipboard.GetTextAsync();
```

Whenever any of the clipboard's content has changed an event is triggered:

```csharp
public class ClipboardTest
{
    public ClipboardTest()
    {
        // Register for clipboard changes, be sure to unsubscribe when needed
        Clipboard.ClipboardContentChanged += OnClipboardContentChanged;
    }

    void OnClipboardContentChanged(object sender, EventArgs    e)
    {
        Console.WriteLine($"Last clipboard change at {DateTime.UtcNow:T}";);
    }
}
```

> [!TIP]
> Access to the Clipboard must be done on the main user interface thread. See the [MainThread](~/essentials/main-thread.md) API to see how to invoke methods on the main user interface thread.

## API

- [Clipboard source code](https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/Clipboard)
- [Clipboard API documentation](xref:Xamarin.Essentials.Clipboard)

## Related Video

> [!Video https://channel9.msdn.com/Shows/XamarinShow/Clipboard-XamarinEssentials-API-of-the-Week/player]

[!include[](~/essentials/includes/xamarin-show-essentials.md)]
