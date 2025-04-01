---
title: "Display pop-ups"
description: ".NET MAUI provides three pop-up-like user interface elements – an alert, an action sheet, and a prompt - that can be used to ask simple questions, guide users through tasks, and display prompts."
ms.date: 04/01/2025
---

# Display pop-ups

::: moniker range="<=net-maui-9.0"

[!INCLUDE [Display a pop-up](../includes/pop-ups-dotnet9.md)]

::: moniker-end

::: moniker range=">=net-maui-10.0"

[!INCLUDE [Display a pop-up](../includes/pop-ups-dotnet10.md)]

::: moniker-end

## Display a prompt

To display a prompt, call the <xref:Microsoft.Maui.Controls.Page.DisplayPromptAsync%2A> on any <xref:Microsoft.Maui.Controls.Page>, passing a title and message as `string` arguments:

```csharp
string result = await DisplayPromptAsync("Question 1", "What's your name?");
```

:::image type="content" source="media/pop-ups/simple-prompt.png" alt-text="Screenshot of a prompt.":::

If the OK button is tapped the entered response is returned as a `string`. If the Cancel button is tapped, `null` is returned.

> [!NOTE]
> On Android, prompts can be dismissed by tapping on the page outside the alert. On desktop platforms, prompts can be dismissed with the escape key.

The full argument list for the <xref:Microsoft.Maui.Controls.Page.DisplayPromptAsync%2A> method is:

- `title`, of type `string`, is the title to display in the prompt.
- `message`, of type `string`, is the message to display in the prompt.
- `accept`, of type `string`, is the text for the accept button. This is an optional argument, whose default value is OK.
- `cancel`, of type `string`, is the text for the cancel button. This is an optional argument, whose default value is Cancel.
- `placeholder`, of type `string`, is the placeholder text to display in the prompt. This is an optional argument, whose default value is `null`.
- `maxLength`, of type `int`, is the maximum length of the user response. This is an optional argument, whose default value is -1.
- `keyboard`, of type `Keyboard`, is the keyboard type to use for the user response. This is an optional argument, whose default value is `Keyboard.Default`.
- `initialValue`, of type `string`, is a pre-defined response that will be displayed, and which can be edited. This is an optional argument, whose default value is an empty `string`.

The following example shows setting some of the optional arguments:

```csharp
string result = await DisplayPromptAsync("Question 2", "What's 5 + 5?", initialValue: "10", maxLength: 2, keyboard: Keyboard.Numeric);
```

This code displays a predefined response of 10, limits the number of characters that can be input to 2, and displays the numeric keyboard for user input:

:::image type="content" source="media/pop-ups/keyboard-prompt.png" alt-text="Screenshot of an optional prompt.":::

> [!WARNING]
> By default on Windows, when a prompt is displayed any access keys that are defined on the page behind the prompt can still be activated. For more information, see [VisualElement access keys on Windows](~/windows/platform-specifics/visualelement-access-keys.md).

## Display a page as a pop-up

.NET MAUI supports modal page navigation. A modal page encourages users to complete a self-contained task that can't be navigated away from until the task is completed or canceled. For example, to display a form as a pop-up that requires users to enter multiple pieces of data, create a <xref:Microsoft.Maui.Controls.ContentPage> that contains the UI for your form and then push it onto the navigation stack as a modal page. For more information, see [Perform modal navigation](~/user-interface/pages/navigationpage.md#perform-modal-navigation).
