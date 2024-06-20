---
title: "InputView reading order on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that enables the reading order of bidirectional text to be detected dynamically."
ms.date: 04/06/2022
---

# InputView reading order on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific enables the reading order (left-to-right or right-to-left) of bidirectional text in <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, and <xref:Microsoft.Maui.Controls.Label> objects to be detected dynamically. It's consumed in XAML by setting the `InputView.DetectReadingOrderFromContent` (for <xref:Microsoft.Maui.Controls.Entry> and <xref:Microsoft.Maui.Controls.Editor> objects) or `Label.DetectReadingOrderFromContent` attached property to a `boolean` value:

```xaml
<ContentPage ...
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <Editor ... windows:InputView.DetectReadingOrderFromContent="true" />
        ...
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

editor.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetDetectReadingOrderFromContent(true);
```

The `Editor.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `InputView.SetDetectReadingOrderFromContent` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to control whether the reading order is detected from the content in the <xref:Microsoft.Maui.Controls.InputView>. In addition, the `InputView.SetDetectReadingOrderFromContent` method can be used to toggle whether the reading order is detected from the content by calling the `InputView.GetDetectReadingOrderFromContent` method to return the current value:

```csharp
editor.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetDetectReadingOrderFromContent(!editor.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().GetDetectReadingOrderFromContent());
```

The result is that <xref:Microsoft.Maui.Controls.Entry>, <xref:Microsoft.Maui.Controls.Editor>, and <xref:Microsoft.Maui.Controls.Label> objects can have the reading order of their content detected dynamically:

:::image type="content" source="media/inputview-reading-order/editor-readingorder.png" alt-text="InputView detecting reading order from content platform-specific.":::

> [!NOTE]
> Unlike setting the `FlowDirection` property, the logic for views that detect the reading order from their text content will not affect the alignment of text within the view. Instead, it adjusts the order in which blocks of bidirectional text are laid out.
