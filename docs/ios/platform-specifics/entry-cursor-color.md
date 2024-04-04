---
title: "Entry cursor color on iOS"
description: "This article explains how to consume the .NET MAUI iOS platform-specific that sets the cursor color of an Entry."
ms.date: 04/05/2022
---

# Entry Cursor Color on iOS

This .NET Multi-platform App UI (.NET MAUI) iOS platform-specific sets the cursor color of an <xref:Microsoft.Maui.Controls.Entry> to a specified color. It's consumed in XAML by setting the `Entry.CursorColor` bindable property to a <xref:Microsoft.Maui.Graphics.Color>:

```xaml
<ContentPage ...
             xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout>
        <Entry ... ios:Entry.CursorColor="LimeGreen" />
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
...

Entry entry = new Microsoft.Maui.Controls.Entry();
entry.On<iOS>().SetCursorColor(Colors.LimeGreen);
```

The `Entry.On<iOS>` method specifies that this platform-specific will only run on iOS. The `Entry.SetCursorColor` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific` namespace, sets the cursor color to a specified <xref:Microsoft.Maui.Graphics.Color>. In addition, the `Entry.GetCursorColor` method can be used to retrieve the current cursor color.

The result is that the cursor color in a <xref:Microsoft.Maui.Controls.Entry> can be set to a specific <xref:Microsoft.Maui.Graphics.Color>:

:::image type="content" source="media/entry-cursor-color/entry-cursorcolor.png" alt-text="Entry cursor color.":::
