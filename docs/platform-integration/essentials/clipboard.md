---
title: "Clipboard"
description: "Learn how to use the .NET MAUIClipboard class in the Microsoft.Maui.Essentials namespace, which lets you copy and paste text to the system clipboard"
ms.date: 08/16/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Clipboard

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) Essentials `Clipboard` class. With this class, you can copy and paste text to and from the system clipboard.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

## Using Clipboard

To check if the clipboard contains text, which a user would normally use to paste into an app:

```csharp
bool hasText = Clipboard.HasText;
```

To set text to the clipboard:

```csharp
await Clipboard.SetTextAsync("Hello World");
```

To read text from the clipboard:

```csharp
string text = await Clipboard.GetTextAsync();
```

Whenever the clipboard content changes, an event is triggered:

```csharp
public class ClipboardTest
{
    public ClipboardTest()
    {
        // Register for clipboard changes, be sure to unsubscribe when needed
        Clipboard.ClipboardContentChanged += OnClipboardContentChanged;
    }

    void OnClipboardContentChanged(object sender, EventArgs e)
    {
        Console.WriteLine($"Last clipboard change occurred at {DateTime.UtcNow:T}");
    }
}
```

> [!TIP]
> Access to the clipboard must be done on the main user interface thread. For more information on how to invoke methods on the main user interface thread, see [MainThread](main-thread.md).

## API

- [Clipboard source code](https://github.com/dotnet/maui/tree/main/src/Essentials/src/Clipboard)
<!-- - [Clipboard API documentation](xref:Microsoft.Maui.Essentials.Clipboard)-->
