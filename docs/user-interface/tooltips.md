---
title: "Display tooltips"
description: "Learn how to display a tooltip, which displays information about the view's purpose, when the user rests the pointer on the view."
ms.date: 09/30/2024
---

# Display tooltips

A .NET Multi-platform App UI (.NET MAUI) tooltip is a small rectangular popup that displays a brief description of a view's purpose, when the user rests the pointer on the [view](~/user-interface/controls/index.md#views). Tooltips display automatically, typically when the user hovers the pointer over the associated view:

- On Android, tooltips are displayed when users long press the view. Tooltips remain visible for a few seconds after the long press is released.
- On iOS, to show a tooltip your app must be an iPhone or iPad app running on a Mac with Apple silicon. Providing this criteria is met, tooltips are displayed when the pointer is positioned over the view for a few seconds, and remain visible until the pointer moves away from the view. Tooltips on iOS require the use of iOS 15.0+. For more information about using iPhone and iPad apps on a Mac with Apple silicon, see [Use iPhone and iPad apps on Mac with Apple silicon](https://support.apple.com/guide/app-store/fird2c7092da/mac).
- On Mac Catalyst, tooltips are displayed when the pointer is positioned over the view for a few seconds. Tooltips remain visible until the pointer moves away from the view. Tooltips on Mac Catalyst require the use of Mac Catalyst 15.0+.
- On Windows, tooltips are displayed when the pointer hovers over the view. Tooltips remain visible for a few seconds, or until the pointer stops hovering over the view.

Tooltips are defined by setting the `ToolTipProperties.Text` attached property, on a view, to a `string`:

```xaml
<Button Text="Save"
        ToolTipProperties.Text="Click to Save your data" />
```

The equivalent C# code is:

```csharp
Button button = new Button { Text = "Save" };
ToolTipProperties.SetText(button, "Click to Save your data");
```

For more information about attached properties, see [Attached properties](~/fundamentals/attached-properties.md).

By default, a tooltip is displayed centered above the pointer:

:::image type="content" source="media/tooltips/tooltip.png" alt-text="Screenshot of tooltip above a button.":::

> [!NOTE]
> Configuring tooltip placement is currently unsupported.
