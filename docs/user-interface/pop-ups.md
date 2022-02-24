---
title: "Display pop-ups"
description: ".NET MAUI provides three pop-up-like user interface elements – an alert, an action sheet, and a prompt, that can be used to ask simple questions, guide users through tasks, and display prompts."
ms.date: 02/24/2022
---

# Display pop-ups

Displaying an alert, asking a user to make a choice, or displaying a prompt is a common UI task. .NET Multi-platform App UI (.NET MAUI) has three methods on the `Page` class for interacting with the user via a pop-up: `DisplayAlert`, `DisplayActionSheet`, and `DisplayPromptAsync`. Pop-ups are rendered with native controls on each platform.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

## Display an alert

All .NET MAUI-supported platforms have a modal pop-up to alert the user or ask simple questions of them. To display alerts, use the `DisplayAlert` method on any `Page`. The following example shows a simple message to the user:

```csharp
await DisplayAlert("Alert", "You have been alerted", "OK");
```

The alert is displayed modally:

:::image type="content" source="media/pop-ups/simple-alert.png" alt-text="Screenshot of an alert dialog with one button.":::

This example does not collect information from the user. The alert displays modally and once dismissed the user continues interacting with the app.

The `DisplayAlert` method can also be used to capture a user's response by presenting two buttons and returning a `bool`. To get a response from an alert, supply text for both buttons and `await` the method:

```csharp
bool answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
Debug.WriteLine("Answer: " + answer);
```

:::image type="content" source="media/pop-ups/two-button-alert.png" alt-text="Screenshot of an alert dialog with two buttons.":::

After the user selects one of the options the response will be returned to your code.

The `DisplayAlert` method also has overloads that accept a `FlowDirection` argument that specifies the direction in which UI elements flow within the alert. <!--For more information about flow direction, see [Right-to-left localization](~/fundamentals/localization/right-to-left.md).-->

<!-- > [!WARNING]
> By default on Windows, when an alert is displayed any access keys that are defined on the page behind the alert can still be activated. For more information, see [VisualElement Access Keys on Windows](~/xamarin-forms/platform/windows/visualelement-access-keys.md). -->

## Guide users through tasks

An action sheet presents the user with a set of alternatives for how to proceed with a task. To display an action sheet, use the `DisplayActionSheet` method on any `Page`, passing the message and button labels as strings:

```csharp
string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
Debug.WriteLine("Action: " + action);
```

The `DisplayActionSheet` method returns the string label of the button that was clicked by the user.

The action sheet will be displayed modally:

:::image type="content" source="media/pop-ups/simple-actionsheet.png" alt-text="Screenshot of an action sheet dialog.":::

Action sheets support a destroy button, which is a button that represents destructive behavior. The destroy button can be specified as the third string argument to the `DisplayActionSheet` method, or can be left `null`. The following example specifies a destroy button:

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

The `DisplayActionSheet` method also has an overload that accepts a `FlowDirection` argument that specifies the direction in which UI elements flow within the action sheet. <!-- For more information about flow direction, see [Right-to-left localization](~/fundamentals/localization/right-to-left.md).-->

## Display a prompt

To display a prompt, call the `DisplayPromptAsync` on any `Page`, passing a title and message as `string` arguments:

```csharp
string result = await DisplayPromptAsync("Question 1", "What's your name?");
```

The prompt is displayed modally:

:::image type="content" source="media/pop-ups/simple-prompt.png" alt-text="Screenshot of a modal prompt.":::

If the OK button is tapped, the entered response is returned as a `string`. If the Cancel button is tapped, `null` is returned.

The full argument list for the `DisplayPromptAsync` method is:

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

:::image type="content" source="media/pop-ups/keyboard-prompt.png" alt-text="Screenshot of an optional modal prompt.":::

<!-- > [!WARNING]
> By default on UWP, when a prompt is displayed any access keys that are defined on the page behind the prompt can still be activated. For more information, see [VisualElement Access Keys on Windows](~/xamarin-forms/platform/windows/visualelement-access-keys.md). -->
