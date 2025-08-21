You can programmatically open and close the selection UI by controlling focus on the picker:

```csharp
// Open the platform picking UI
picker.Focus();

// ... later, close the UI
picker.Unfocus();
```

Platform notes:

- Android shows the dialog when focused and dismisses it when unfocused.
- iOS and Mac Catalyst present the input view when focused; Unfocus ends editing.
- Windows uses a flyout/ComboBox; focus doesn’t always force the drop-down to open in all contexts. Prefer user interaction to open; Unfocus closes if open.

To detect when the picker opens or closes, handle the <xref:Microsoft.Maui.Controls.VisualElement.Focused> and <xref:Microsoft.Maui.Controls.VisualElement.Unfocused> events.
