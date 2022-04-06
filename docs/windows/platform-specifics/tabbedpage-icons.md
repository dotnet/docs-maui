---
title: "TabbedPage icons on Windows"
description: "This article explains how to consume the .NET MAUI Windows platform-specific that enables page icons to be displayed on a TabbedPage toolbar."
ms.date: 04/06/2022
---

# TabbedPage icons on Windows

This .NET Multi-platform App UI (.NET MAUI) Windows platform-specific enables page icons to be displayed on a `TabbedPage` toolbar, and provides the ability to optionally specify the icon size. It's consumed in XAML by setting the `TabbedPage.HeaderIconsEnabled` attached property to `true`, and by optionally setting the `TabbedPage.HeaderIconsSize` attached property to a `Size` value:

```xaml
<TabbedPage ...
            xmlns:windows="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;assembly=Microsoft.Maui.Controls"
            windows:TabbedPage.HeaderIconsEnabled="true">
    <windows:TabbedPage.HeaderIconsSize>
        <Size>
            <x:Arguments>
                <x:Double>24</x:Double>
                <x:Double>24</x:Double>
            </x:Arguments>
        </Size>
    </windows:TabbedPage.HeaderIconsSize>
    <ContentPage Title="Todo" IconImageSource="todo.png">
        ...
    </ContentPage>
    <ContentPage Title="Reminders" IconImageSource="reminders.png">
        ...
    </ContentPage>
    <ContentPage Title="Contacts" IconImageSource="contacts.png">
        ...
    </ContentPage>
</TabbedPage>
```

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
...

public class WindowsTabbedPageIconsCode : Microsoft.Maui.Controls.TabbedPage
{
    public WindowsTabbedPageIconsCode()
    {
        On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetHeaderIconsEnabled(true);
        On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>().SetHeaderIconsSize(new Size(24, 24));

        Children.Add(new ContentPage { Title = "Todo", IconImageSource = "todo.png" });
        Children.Add(new ContentPage { Title = "Reminders", IconImageSource = "reminders.png" });
        Children.Add(new ContentPage { Title = "Contacts", IconImageSource = "contacts.png" });
    }
}
```

The `TabbedPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Windows>` method specifies that this platform-specific will only run on Windows. The `TabbedPage.SetHeaderIconsEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace, is used to turn header icons on or off. The `TabbedPage.SetHeaderIconsSize` method optionally specifies the header icon size with a `Size` value.

In addition, the `TabbedPage` class in the `Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific` namespace also has a `EnableHeaderIcons` method that enables header icons, a `DisableHeaderIcons` method that disables header icons, and a `IsHeaderIconsEnabled` method that returns a `boolean` value that indicates whether header icons are enabled.

The result is that page icons can be displayed on a `TabbedPage` toolbar, with the icon size being optionally set to a desired size:

:::image type="content" source="media/tabbedpage-icons/tabbedpage-icons.png" alt-text="TabbedPage icons enabled platform-specific.":::
