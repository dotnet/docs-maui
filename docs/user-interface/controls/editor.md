---
title: "Editor"
description: "The .NET MAUI Editor allows you to enter and edit multiple lines of text."
ms.date: 08/30/2024
---

# Editor

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Editor> allows you to enter and edit multiple lines of text.

<xref:Microsoft.Maui.Controls.Editor> defines the following properties:

- `AutoSize`, of type `EditorAutoSizeOption`, defines whether the editor will change size to accommodate user input. By default, the editor doesn't auto size.
- `HorizontalTextAlignment`, of type <xref:Microsoft.Maui.TextAlignment>, defines the horizontal alignment of the text.
- `VerticalTextAlignment`, of type <xref:Microsoft.Maui.TextAlignment>, defines the vertical alignment of the text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

In addition, <xref:Microsoft.Maui.Controls.Editor> defines a `Completed` event, which is raised when the user finalizes text in the <xref:Microsoft.Maui.Controls.Editor> with the return key.

<xref:Microsoft.Maui.Controls.Editor> derives from the <xref:Microsoft.Maui.Controls.InputView> class, from which it inherits the following properties:

- `CharacterSpacing`, of type `double`, sets the spacing between characters in the text content, including both the user-entered or displayed text and the placeholder text.
- `CursorPosition`, of type `int`, defines the position of the cursor within the editor.
- `FontAttributes`, of type `FontAttributes`, determines text style.
- `FontAutoScalingEnabled`, of type `bool`, defines whether the text will reflect scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily`, of type `string`, defines the font family.
- `FontSize`, of type `double`, defines the font size.
- `IsReadOnly`, of type `bool`, defines whether the user should be prevented from modifying text. The default value of this property is `false`.
- `IsSpellCheckEnabled`, of type `bool`, controls whether spell checking is enabled.
- `IsTextPredictionEnabled`, of type `bool`, controls whether text prediction and automatic text correction is enabled.
- `Keyboard`, of type `Keyboard`, specifies the soft input keyboard that's displayed when entering text.
- `MaxLength`, of type `int`, defines the maximum input length.
- `Placeholder`, of type `string`, defines the text that's displayed when the control is empty.
- `PlaceholderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the placeholder text.
- `SelectionLength`, of type `int`, represents the length of selected text within the control.
- `Text`, of type `string`, defines the text entered into the control.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the entered text.
- `TextTransform`, of type `TextTransform`, specifies the casing of the entered text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

In addition, <xref:Microsoft.Maui.Controls.InputView> defines a `TextChanged` event, which is raised when the text in the <xref:Microsoft.Maui.Controls.Editor> changes. The `TextChangedEventArgs` object that accompanies the `TextChanged` event has `NewTextValue` and `OldTextValue` properties, which specify the new and old text, respectively.

For information about specifying fonts on an <xref:Microsoft.Maui.Controls.Editor>, see [Fonts](~/user-interface/fonts.md).

## Create an Editor

The following example shows how to create an <xref:Microsoft.Maui.Controls.Editor>:

```xaml
<Editor x:Name="editor"
        Placeholder="Enter your response here"
        HeightRequest="250"
        TextChanged="OnEditorTextChanged"
        Completed="OnEditorCompleted" />
```

The equivalent C# code is:

```csharp
Editor editor = new Editor { Placeholder = "Enter text", HeightRequest = 250 };
editor.TextChanged += OnEditorTextChanged;
editor.Completed += OnEditorCompleted;
```

The following screenshot shows the resulting <xref:Microsoft.Maui.Controls.Editor> on Android:

:::image type="content" source="media/editor/editor.png" alt-text="Screenshot of a basic Editor on Android.":::

[!INCLUDE [Keyboard autoscroll manager](includes/keyboardautomanagerscroll.md)]

Entered text can be accessed by reading the `Text` property, and the `TextChanged` and `Completed` events signal that the text has changed or been completed.

The `TextChanged` event is raised when the text in the <xref:Microsoft.Maui.Controls.Editor> changes, and the `TextChangedEventArgs` provide the text before and after the change via the `OldTextValue` and `NewTextValue` properties:

```csharp
void OnEditorTextChanged(object sender, TextChangedEventArgs e)
{
    string oldText = e.OldTextValue;
    string newText = e.NewTextValue;
    string myText = editor.Text;
}
```

The `Completed` event is only raised on Windows when the user has ended input by pressing the <kbd>Tab</kbd> key on the keyboard, or by focusing another control. The handler for the event is a generic event handler:

```csharp
void OnEditorCompleted(object sender, EventArgs e)
{
   string text = ((Editor)sender).Text;
}
```

## Set character spacing

Character spacing can be applied to an <xref:Microsoft.Maui.Controls.Editor> by setting the `CharacterSpacing` property to a `double` value:

```xaml
<Editor ...
        CharacterSpacing="10" />
```

The result is that characters in the text displayed by the <xref:Microsoft.Maui.Controls.Editor> are spaced `CharacterSpacing` device-independent units apart.

> [!NOTE]
> The `CharacterSpacing` property value is applied to the text displayed by the `Text` and `Placeholder` properties.

## Limit input length

The `MaxLength` property can be used to limit the input length that's permitted for the <xref:Microsoft.Maui.Controls.Editor>. This property should be set to a positive integer:

```xaml
<Editor ... MaxLength="10" />
```

A `MaxLength` property value of 0 indicates that no input will be allowed, and a value of `int.MaxValue`, which is the default value for an <xref:Microsoft.Maui.Controls.Editor>, indicates that there is no effective limit on the number of characters that may be entered.

## Auto-size an Editor

An <xref:Microsoft.Maui.Controls.Editor> can be made to auto-size to its content by setting the `Editor.AutoSize` property to `TextChanges`, which is a value of the `EditorAutoSizeOption` enumeration. This enumeration has two values:

- `Disabled` indicates that automatic resizing is disabled, and is the default value.
- `TextChanges` indicates that automatic resizing is enabled.

This can be accomplished as follows:

```xaml
<Editor Text="Enter text here"
        AutoSize="TextChanges" />
```

When auto-resizing is enabled, the height of the <xref:Microsoft.Maui.Controls.Editor> will increase when the user fills it with text, and the height will decrease as the user deletes text. This can be used to ensure that <xref:Microsoft.Maui.Controls.Editor> objects in a <xref:Microsoft.Maui.Controls.DataTemplate> in a <xref:Microsoft.Maui.Controls.CollectionView> size correctly.

> [!IMPORTANT]
> An <xref:Microsoft.Maui.Controls.Editor> will not auto-size if the <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> property has been set.

## Transform text

An <xref:Microsoft.Maui.Controls.Editor> can transform the casing of its text, stored in the `Text` property, by setting the `TextTransform` property to a value of the `TextTransform` enumeration. This enumeration has four values:

- `None` indicates that the text won't be transformed.
- `Default` indicates that the default behavior for the platform will be used. This is the default value of the `TextTransform` property.
- `Lowercase` indicates that the text will be transformed to lowercase.
- `Uppercase` indicates that the text will be transformed to uppercase.

The following example shows transforming text to uppercase:

```xaml
<Editor Text="This text will be displayed in uppercase."
        TextTransform="Uppercase" />
```

## Customize the keyboard

The keyboard that's presented when users interact with an <xref:Microsoft.Maui.Controls.Editor> can be set programmatically via the `Keyboard` property, to one of the following properties from the `Keyboard` class:

- `Chat` – used for texting and places where emoji are useful.
- `Default` – the default keyboard.
- `Email` – used when entering email addresses.
- `Numeric` – used when entering numbers.
- `Plain` – used when entering text, without any `KeyboardFlags` specified.
- `Telephone` – used when entering telephone numbers.
- `Text` – used when entering text.
- `Url` – used for entering file paths & web addresses.

The following example shows setting the `Keyboard` property:

```xaml
<Editor Keyboard="Chat" />
```

The `Keyboard` class also has a `Create` factory method that can be used to customize a keyboard by specifying capitalization, spellcheck, and suggestion behavior. `KeyboardFlags` enumeration values are specified as arguments to the method, with a customized `Keyboard` being returned. The `KeyboardFlags` enumeration contains the following values:

- `None` – no features are added to the keyboard.
- `CapitalizeSentence` – indicates that the first letter of the first word of each entered sentence will be automatically capitalized.
- `Spellcheck` – indicates that spellcheck will be performed on entered text.
- `Suggestions` – indicates that word completions will be offered on entered text.
- `CapitalizeWord` – indicates that the first letter of each word will be automatically capitalized.
- `CapitalizeCharacter` – indicates that every character will be automatically capitalized.
- `CapitalizeNone` – indicates that no automatic capitalization will occur.
- `All` – indicates that spellcheck, word completions, and sentence capitalization will occur on entered text.

The following XAML code example shows how to customize the default `Keyboard` to offer word completions and capitalize every entered character:

```xaml
<Editor>
    <Editor.Keyboard>
        <Keyboard x:FactoryMethod="Create">
            <x:Arguments>
                <KeyboardFlags>Suggestions,CapitalizeCharacter</KeyboardFlags>
            </x:Arguments>
        </Keyboard>
    </Editor.Keyboard>
</Editor>
```

The equivalent C# code is:

```csharp
Editor editor = new Editor();
editor.Keyboard = Keyboard.Create(KeyboardFlags.Suggestions | KeyboardFlags.CapitalizeCharacter);
```

[!INCLUDE [Hide and show the soft input keyboard](includes/soft-input-extensions.md)]

The following example shows how to hide the soft input keyboard on an <xref:Microsoft.Maui.Controls.Editor> named `editor`, if it's currently showing:

```csharp
if (editor.IsSoftInputShowing())
    await editor.HideSoftInputAsync(System.Threading.CancellationToken.None);
```

## Enable and disable spell checking

The `IsSpellCheckEnabled` property controls whether spell checking is enabled. By default, the property is set to `true`. As the user enters text, misspellings are indicated.

However, for some text entry scenarios, such as entering a username, spell checking provides a negative experience and so should be disabled by setting the `IsSpellCheckEnabled` property to `false`:

```xaml
<Editor ... IsSpellCheckEnabled="false" />
```

> [!NOTE]
> When the `IsSpellCheckEnabled` property is set to `false`, and a custom keyboard isn't being used, the native spell checker will be disabled. However, if a `Keyboard` has been set that disables spell checking, such as `Keyboard.Chat`, the `IsSpellCheckEnabled` property is ignored. Therefore, the property cannot be used to enable spell checking for a `Keyboard` that explicitly disables it.

## Enable and disable text prediction

The `IsTextPredictionEnabled` property controls whether text prediction and automatic text correction is enabled. By default, the property is set to `true`. As the user enters text, word predictions are presented.

However, for some text entry scenarios, such as entering a username, text prediction and automatic text correction provides a negative experience and should be disabled by setting the `IsTextPredictionEnabled` property to `false`:

```xaml
<Editor ... IsTextPredictionEnabled="false" />
```

> [!NOTE]
> When the `IsTextPredictionEnabled` property is set to `false`, and a custom keyboard isn't being used, text prediction and automatic text correction is disabled. However, if a `Keyboard` has been set that disables text prediction, the `IsTextPredictionEnabled` property is ignored. Therefore, the property cannot be used to enable text prediction for a `Keyboard` that explicitly disables it.

## Prevent text entry

Users can be prevented from modifying the text in an <xref:Microsoft.Maui.Controls.Editor> by setting the `IsReadOnly` property, which has a default value of `false`, to `true`:

```xaml
<Editor Text="This is a read-only Editor"
        IsReadOnly="true" />
```

> [!NOTE]
> The `IsReadonly` property does not alter the visual appearance of an <xref:Microsoft.Maui.Controls.Editor>, unlike the `IsEnabled` property that also changes the visual appearance of the <xref:Microsoft.Maui.Controls.Editor> to gray.

## Line endings (Windows only)

When the user adds multiple lines in the control, the `Text` property will contain only "\r" for the line endings and not "\r\n" which is normal in Windows. There is no property to change this behavior, but the text can be altered to include this specific line ending if needed.

```csharp
#if WINDOWS
if (EditorText.Text.Contains("\r"))
{
    var textWithWindowsLineBreaks = EditorText.Text.Replace("\r", Environment.NewLine);

    // Do something with the text that has Windows line breaks
}
#endif

```
> [!NOTE]
> Do not assign the updated text to the `Text` property of the control. The original line ending ("\r") will be restored if this happens.
