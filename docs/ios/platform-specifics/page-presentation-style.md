---
title: "Modal page presentation style on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that sets the presentation style of a modal page."
ms.date: 03/14/2025
---

# Modal page presentation style on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific is used to set the presentation style of a modal page, and in addition can be used to display modal pages that have transparent backgrounds. It's consumed in XAML by setting the `Page.ModalPresentationStyle` bindable property to a `UIModalPresentationStyle` enumeration value:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
             ios:Page.ModalPresentationStyle="OverFullScreen">
    ...
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

public class iOSModalFormSheetPageCode : ContentPage
{
    public iOSModalFormSheetPageCode()
    {
        On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.OverFullScreen);
    }
}
```

The `Page.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Page.SetModalPresentationStyle` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, is used to set the modal presentation style on a <xref:Microsoft.Maui.Controls.Page> by specifying one of the following `UIModalPresentationStyle` enumeration values:

- `FullScreen`, which sets the modal presentation style to encompass the whole screen. By default, modal pages are displayed using this presentation style.
- `FormSheet`, which sets the modal presentation style to be centered on and smaller than the screen.
- `Automatic`, which sets the modal presentation style to the default chosen by the system. For most view controllers, `UIKit` maps this to `UIModalPresentationStyle.PageSheet`, but some system view controllers may map it to a different style.
- `OverFullScreen`, which sets the modal presentation style to cover the screen.
- `PageSheet`, which sets the modal presentation style to cover the underlying content.
- `Popover`, which sets the modal presentation style to display content in a popover.

In addition, the `GetModalPresentationStyle` method can be used to retrieve the current value of the `UIModalPresentationStyle` enumeration that's applied to the <xref:Microsoft.Maui.Controls.Page>.

The result is that the modal presentation style on a <xref:Microsoft.Maui.Controls.Page> can be set:

:::image type="content" source="media/page-presentation-style/modal-presentation-style.png" alt-text="Modal presentation styles.":::

> [!NOTE]
> Pages that use this platform-specific to set the modal presentation style must use modal navigation. For more information, see [Perform modal navigation](~/user-interface/pages/navigationpage.md#perform-modal-navigation).
