---
title: "Display pop-ups"
description: ".NET MAUI provides three pop-up-like user interface elements – an alert, an action sheet, and a prompt - that can be used to ask simple questions, guide users through tasks, and display prompts."
ms.date: 12/22/2023
---

# Display pop-ups

Displaying an alert, asking a user to make a choice, or displaying a prompt is a common UI task. .NET Multi-platform App UI (.NET MAUI) has three methods on the <xref:Microsoft.Maui.Controls.Page> class for interacting with the user via a pop-up: <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A>, <xref:Microsoft.Maui.Controls.Page.DisplayActionSheet%2A>, and <xref:Microsoft.Maui.Controls.Page.DisplayPromptAsync%2A>. Pop-ups are rendered with native controls on each platform.

## Display an alert

All .NET MAUI-supported platforms have a pop-up to alert the user or ask simple questions of them. To display alerts, use the <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A> method on any <xref:Microsoft.Maui.Controls.Page>. The following example shows a simple message to the user:

```csharp
await DisplayAlert("Alert", "You have been alerted", "OK");
```

:::image type="content" source="media/pop-ups/simple-alert.png" alt-text="Screenshot of an alert dialog with one button.":::

Once the alert is dismissed the user continues interacting with the app.

> [!NOTE]
> On Android, alerts can be dismissed by tapping on the page outside the alert. On desktop platforms, alerts can be dismissed with the escape key.

The <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A> method can also be used to capture a user's response by presenting two buttons and returning a `bool`. To get a response from an alert, supply text for both buttons and `await` the method:

```csharp
bool answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
Debug.WriteLine("Answer: " + answer);
```

:::image type="content" source="media/pop-ups/two-button-alert.png" alt-text="Screenshot of an alert dialog with two buttons.":::

After the user selects one of the options the response will be returned as a `bool`.

The <xref:Microsoft.Maui.Controls.Page.DisplayAlert%2A> method also has overloads that accept a `FlowDirection` argument that specifies the direction in which UI elements flow within the alert. For more information about flow direction, see [Right to left localization](~/fundamentals/localization.md#right-to-left-localization).

> [!WARNING]
> By default on Windows, when an alert is displayed any access keys that are defined on the page behind the alert can still be activated. For more information, see [VisualElement access keys on Windows](~/windows/platform-specifics/visualelement-access-keys.md).

## Guide users through tasks

An action sheet presents the user with a set of alternatives for how to proceed with a task. To display an action sheet, use the <xref:Microsoft.Maui.Controls.Page.DisplayActionSheet%2A> method on any <xref:Microsoft.Maui.Controls.Page>, passing the message and button labels as strings:

```csharp
string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
Debug.WriteLine("Action: " + action);
```

:::image type="content" source="media/pop-ups/simple-actionsheet.png" alt-text="Screenshot of an action sheet dialog.":::

After the user taps one of the buttons, the button label will be returned as a `string`.

> [!NOTE]
> Action sheets can be dismissed on touch platforms, and Mac Catalyst, by tapping on the page outside the action sheet. On Windows, action sheets can be dismissed with the escape key and by clicking on the page outside the action sheet.

Action sheets also support a destroy button, which is a button that represents destructive behavior. The destroy button can be specified as the third string argument to the <xref:Microsoft.Maui.Controls.Page.DisplayActionSheet%2A> method, or can be left `null`. The following example specifies a destroy button:

```csharp
async void OnActionSheetCancelDeleteClicked(object sender, EventArgs e)
{
  string action = await DisplayActionSheet("ActionSheet: SavePhoto?", "Cancel", "Delete", "Photo Roll", "Email");
  Debug.WriteLine("Action: " + action);
}
```

:::image type="content" source="media/pop-ups/actionsheet-destroy-button.png" alt-text="Screenshot of an action sheet with a destroy button.":::

> [!NOTE]
> On iOS, the destroy button is rendered differently to the other buttons in the action sheet.

The <xref:Microsoft.Maui.Controls.Page.DisplayActionSheet%2A> method also has an overload that accepts a `FlowDirection` argument that specifies the direction in which UI elements flow within the action sheet. For more information about flow direction, see [Right to left localization](~/fundamentals/localization.md#right-to-left-localization).

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
