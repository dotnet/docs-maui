---
title: "ViewCell context actions on Android"
description: "This article explains how to consume the .NET MAUI Android platform-specific that enables ViewCell context actions legacy mode."
ms.date: 04/05/2022
---

# ViewCell context actions on Android

By default in .NET Multi-platform App UI (.NET MAUI), when a `ViewCell` in an Android application defines context actions for each item in a `ListView`, the context actions menu is updated when the selected item in the `ListView` changes. However, in previous versions of .NET MAUI the context actions menu was not updated, and this behavior is referred to as the `ViewCell` legacy mode. This legacy mode can result in incorrect behavior if a `ListView` uses a `DataTemplateSelector` to set its `ItemTemplate` from `DataTemplate` objects that define different context actions.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

This .NET MAUI Android platform-specific enables the `ViewCell` context actions menu legacy mode, for backwards compatibility, so that the context actions menu is not updated when the selected item in a `ListView` changes. It's consumed in XAML by setting the `ViewCell.IsContextActionsLegacyModeEnabled` bindable property to `true`:

```xaml
<ContentPage ...
             xmlns:android="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;assembly=Microsoft.Maui.Controls">
    <StackLayout Margin="20">
        <ListView ItemsSource="{Binding Items}">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell android:ViewCell.IsContextActionsLegacyModeEnabled="true">
                        <ViewCell.ContextActions>
                            <MenuItem Text="{Binding Item1Text}" />
                            <MenuItem Text="{Binding Item2Text}" />
                        </ViewCell.ContextActions>
                        <Label Text="{Binding Text}" />
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>
```

Alternatively, it can be consumed from C# using the fluent API:

```csharp
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
...

viewCell.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetIsContextActionsLegacyModeEnabled(true);
```

The `ViewCell.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>` method specifies that this platform-specific will only run on Android. The `ViewCell.SetIsContextActionsLegacyModeEnabled` method, in the `Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific` namespace, is used to enable the `ViewCell` context actions menu legacy mode, so that the context actions menu is not updated when the selected item in a `ListView` changes. In addition, the `ViewCell.GetIsContextActionsLegacyModeEnabled` method can be used to return whether the context actions legacy mode is enabled.

The following screenshot shows `ViewCell` context actions legacy mode enabled:

:::image type="content" source="media/viewcell-context-actions/legacy-mode-enabled.png" alt-text="Screenshot of ViewCell legacy mode enabled, on Android.":::

In this mode, the displayed context action menu items are identical for cell 1 and cell 2, despite different context menu items being defined for cell 2.

The following screenshot shows `ViewCell` context actions legacy mode disabled, which is the default .NET MAUI behavior:

:::image type="content" source="media/viewcell-context-actions/legacy-mode-disabled.png" alt-text="Screenshot of ViewCell legacy mode disabled, on Android.":::

In this mode, the correct context action menu items are displayed for cell 1 and cell 2.
