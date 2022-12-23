---
title: "ListView SelectionMode on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that controls whether items in a ListView can respond to tap gestures."
ms.date: 04/06/2022
---

# ListView SelectionMode on Windows

On Windows, by default the .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ListView> uses the native `ItemClick` event to respond to interaction, rather than the native `Tapped` event. This provides accessibility functionality so that the Windows Narrator and the keyboard can interact with the <xref:Microsoft.Maui.Controls.ListView>. However, it also renders any tap gestures inside the <xref:Microsoft.Maui.Controls.ListView> inoperable.

This .NET MAUI Windows platform-specific controls whether items in a <xref:Microsoft.Maui.Controls.ListView> can respond to tap gestures, and hence whether the native <xref:Microsoft.Maui.Controls.ListView> fires the `ItemClick` or `Tapped` event. It's consumed in XAML by setting the `ListView.SelectionMode` attached property to a value of the `ListViewSelectionMode` enumeration:

```xaml
<ContentPage ...
             xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <ListView ... windows:ListView.SelectionMode="Inaccessible">
            ...
        </ListView>
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

listView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetSelectionMode(ListViewSelectionMode.Inaccessible);
```

The `ListView.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `ListView.SetSelectionMode` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to control whether items in a <xref:Microsoft.Maui.Controls.ListView> can respond to tap gestures, with the `ListViewSelectionMode` enumeration providing two possible values:

- `Accessible` – indicates that the <xref:Microsoft.Maui.Controls.ListView> will fire the native `ItemClick` event to handle interaction, and hence provide accessibility functionality. Therefore, the Windows Narrator and the keyboard can interact with the <xref:Microsoft.Maui.Controls.ListView>. However, items in the <xref:Microsoft.Maui.Controls.ListView> can't respond to tap gestures. This is the default behavior for <xref:Microsoft.Maui.Controls.ListView> objects on Windows.
- `Inaccessible` – indicates that the <xref:Microsoft.Maui.Controls.ListView> will fire the native `Tapped` event to handle interaction. Therefore, items in the <xref:Microsoft.Maui.Controls.ListView> can respond to tap gestures. However, there's no accessibility functionality and hence the Windows Narrator and the keyboard can't interact with the <xref:Microsoft.Maui.Controls.ListView>.

> [!NOTE]
> The `Accessible` and `Inaccessible` selection modes are mutually exclusive, and you will need to choose between an accessible <xref:Microsoft.Maui.Controls.ListView> or a <xref:Microsoft.Maui.Controls.ListView> that can respond to tap gestures.

In addition, the `GetSelectionMode` method can be used to return the current `ListViewSelectionMode`.

The result is that a specified `ListViewSelectionMode` is applied to the <xref:Microsoft.Maui.Controls.ListView>, which controls whether items in the <xref:Microsoft.Maui.Controls.ListView> can respond to tap gestures, and hence whether the native <xref:Microsoft.Maui.Controls.ListView> fires the `ItemClick` or `Tapped` event.
