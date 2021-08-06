---
title: "Essentials: Clipboard"
description: "Describes the Clipboard class in the Microsoft.Maui.Essentials namespace, which lets you copy and paste text to the system clipboard"
ms.date: 08/05/2021
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Essentials"]
---

# Clipboard

The `Clipboard` class lets you copy and paste text to the system clipboard between applications.

## Get started

[!INCLUDE [get-started](includes/get-started.md)]

## Using Clipboard

[!INCLUDE [essentials-namespace](includes/essentials-namespace.md)]

To check if the clipboard has text currently ready to be pasted:

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

Whenever any of the clipboard's content has changed, an event is triggered:

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
