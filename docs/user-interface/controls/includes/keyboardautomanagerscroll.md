---
ms.topic: include
ms.date: 10/21/2023
---

> [!NOTE]
> On iOS, the soft input keyboard can cover a text input field when the field is near the bottom of the screen, making it difficult to enter text. However, in a .NET MAUI iOS app, pages automatically scroll when the soft input keyboard would cover a text entry field, so that the field is above the soft input keyboard. The `KeyboardAutoManagerScroll.Disconnect` method, in the `Microsoft.Maui.Platform` namespace, can be called to disable this default behavior. The `KeyboardAutoManagerScroll.Connect` method can be called to re-enable the behavior after it's been disabled.
