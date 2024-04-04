---
title: "Entry input method editor options on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that sets the input method editor options for the soft keyboard for an Entry."
ms.date: 04/05/2022
---

# Entry input method editor options on Android

This .NET Multi-platform App UI (.NET MAUI) Android platform-specific sets the input method editor (IME) options for the soft keyboard for an <xref:Microsoft.Maui.Controls.Entry>. This includes setting the user action button in the bottom corner of the soft keyboard, and the interactions with the <xref:Microsoft.Maui.Controls.Entry>. It's consumed in XAML by setting the `Entry.ImeOptions` attached property to a value of the `ImeFlags` enumeration:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout ...>
        <Entry ... android:Entry.ImeOptions="Send" />
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

entry.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetImeOptions(ImeFlags.Send);
```

The `Entry.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `Entry.SetImeOptions` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to set the input method action option for the soft keyboard for the <xref:Microsoft.Maui.Controls.Entry>, with the `ImeFlags` enumeration providing the following values:

- `Default` – indicates that no specific action key is required, and that the underlying control will produce its own if it can. This will either be `Next` or `Done`.
- `None` – indicates that no action key will be made available.
- `Go` – indicates that the action key will perform a "go" operation, taking the user to the target of the text they typed.
- `Search` – indicates that the action key performs a "search" operation, taking the user to the results of searching for the text they have typed.
- `Send` – indicates that the action key will perform a "send" operation, delivering the text to its target.
- `Next` – indicates that the action key will perform a "next" operation, taking the user to the next field that will accept text.
- `Done` – indicates that the action key will perform a "done" operation, closing the soft keyboard.
- `Previous` – indicates that the action key will perform a "previous" operation, taking the user to the previous field that will accept text.
- `ImeMaskAction` – the mask to select action options.
- `NoPersonalizedLearning` – indicates that the spellchecker will neither learn from the user, nor suggest corrections based on what the user has previously typed.
- `NoFullscreen` – indicates that the UI should not go fullscreen.
- `NoExtractUi` – indicates that no UI will be shown for extracted text.
- `NoAccessoryAction` – indicates that no UI will be displayed for custom actions.

The result is that a specified `ImeFlags` value is applied to the soft keyboard for the <xref:Microsoft.Maui.Controls.Entry>, which sets the input method editor options:

:::image type="content" source="media/entry-ime-options/entry-imeoptions.png" alt-text="Entry input method editor platform-specific.":::
