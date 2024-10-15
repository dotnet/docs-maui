---
title: "TitleBar"
description: "Learn how to use the .NET MAUI TitleBar, which provides the ability to add a custom title bar to your app on Windows."
ms.date: 10/15/2024
monikerRange: ">=net-maui-9.0"
---

# TitleBar

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-titlebar)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.TitleBar> is a view that lets you to add a custom title bar to a <xref:Microsoft.Maui.Controls.Window> to match the personality of your app. The following diagram shows the components of the <xref:Microsoft.Maui.Controls.TitleBar>:

:::image type="content" source="media/titlebar/titlebar-overview.png" alt-text=".NET MAUI Titlebar overview." border="false":::

> [!IMPORTANT]
> <xref:Microsoft.Maui.Controls.TitleBar> is only available on Windows. Mac Catalyst support will be added in a future release.

<xref:Microsoft.Maui.Controls.TitleBar> defines the following properties:

- <xref:Microsoft.Maui.Controls.TitleBar.Content>, of type <xref:Microsoft.Maui.IView>, which specifies the control for the content that's centered in the title bar, and is allocated the space between the leading and trailing content.
- <xref:Microsoft.Maui.Controls.TitleBar.DefaultTemplate>, of type <xref:Microsoft.Maui.Controls.ControlTemplate>, which represents the default template for the title bar.
- <xref:Microsoft.Maui.Controls.TitleBar.ForegroundColor>, of type <xref:Microsoft.Maui.Graphics.Color>, which specifies the foreground colour of the title bar, and is used as the color for the title and subtitle text.
- <xref:Microsoft.Maui.Controls.TitleBar.Icon>, of type <xref:Microsoft.Maui.Controls.ImageSource>, which represents an optional 16x16px icon image for the title bar.
- <xref:Microsoft.Maui.Controls.TitleBar.LeadingContent>, of type <xref:Microsoft.Maui.IView>, which specifies the control for the content that follows the icon.
- <xref:Microsoft.Maui.Controls.TitleBar.PassthroughElements>, of type `IList<IView>`, which represents a list of elements that should prevent dragging in the title bar region and instead directly handle input.
- <xref:Microsoft.Maui.Controls.TitleBar.Subtitle>, of type `string`, which specifies the subtitle text of the title bar. This is usually secondary information about the app or window.
- <xref:Microsoft.Maui.Controls.TitleBar.Title>, of type `string`, which specifies the title text of the title bar. This is usually the name of the app or indicates the purpose of the window.
- <xref:Microsoft.Maui.Controls.TitleBar.TrailingContent>, of type <xref:Microsoft.Maui.IView>, which specifies the control that follows the `Content` control.

These properties, with the exception of <xref:Microsoft.Maui.Controls.TitleBar.DefaultTemplate> and <xref:Microsoft.Maui.Controls.TitleBar.PassthroughElements>, are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be styled, and be the target of data bindings.

> [!IMPORTANT]
> Views set as the value of the <xref:Microsoft.Maui.Controls.TitleBar.Content>, <xref:Microsoft.Maui.Controls.TitleBar.LeadingContent>, and <xref:Microsoft.Maui.Controls.TitleBar.TrailingContent> properties will block all input to the title bar region and will directly handle input.

The standard title bar height is 32px, but can be set to a larger value. For information about designing your title bar on Windows, see [Title bar](/windows/apps/design/basics/titlebar-design).

## Create a TitleBar

To add a title bar to a window, set the <xref:Microsoft.Maui.Controls.Window.TitleBar?displayProperty=nameWithType> property to a <xref:Microsoft.Maui.Controls.TitleBar> object.

The following XAML example shows how to add a <xref:Microsoft.Maui.Controls.TitleBar> to a <xref:Microsoft.Maui.Controls.Window>:

```xaml
<Window xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        xmlns:local="clr-namespace:TitleBarDemo"
        x:Class="TitleBarDemo.MainWindow">
    ...
    <Window.TitleBar>
        <TitleBar Title="{Binding Title}"
                  Subtitle="{Binding Subtitle}"
                  IsVisible="{Binding ShowTitleBar}"
                  BackgroundColor="#512BD4"
                  ForegroundColor="White"                  
                  HeightRequest="48">
            <TitleBar.Content>
                <SearchBar Placeholder="Search"
                           PlaceholderColor="White"
                           MaximumWidthRequest="300"
                           HorizontalOptions="Fill"
                           VerticalOptions="Center" />
            </TitleBar.Content>            
        </TitleBar>
    </Window.TitleBar>
</Window>
```

A <xref:Microsoft.Maui.Controls.TitleBar> can also be defined in C# and added to a <xref:Microsoft.Maui.Controls.Window>:

```csharp
Window window = new Window
{
    TitleBar = new TitleBar
    {
        Icon = "titlebar_icon.png"
        Title = "My App",
        Subtitle = "Demo"
        Content = new SearchBar { ... }      
    }
};
```

A <xref:Microsoft.Maui.Controls.TitleBar> is highly customizable through its <xref:Microsoft.Maui.Controls.TitleBar.Content>, <xref:Microsoft.Maui.Controls.TitleBar.LeadingContent>, and <xref:Microsoft.Maui.Controls.TitleBar.TrailingContent> properties:

```xaml
<TitleBar Title="My App"
          BackgroundColor="#512BD4"
          HeightRequest="48">
    <TitleBar.Content>
        <SearchBar Placeholder="Search"
                   MaximumWidthRequest="300"
                   HorizontalOptions="FillAndExpand"
                   VerticalOptions="Center" />
    </TitleBar.Content>
    <TitleBar.TrailingContent>
        <ImageButton HeightRequest="36"
                     WidthRequest="36"
                     BorderWidth="0"
                     Background="Transparent">
            <ImageButton.Source>
                <FontImageSource Size="16"
                                 Glyph="&#xE713;"
                                 FontFamily="SegoeMDL2"/>
            </ImageButton.Source>
        </ImageButton>
    </TitleBar.TrailingContent>
</TitleBar>
```

The following screenshot shows the resulting appearance:

:::image type="content" source="media/titlebar/titlebar-full.png" alt-text=".NET MAUI Titlebar screenshot.":::

> [!NOTE]
> The title bar can be hidden by setting the <xref:Microsoft.Maui.Controls.VisualElement.IsVisible> property, which causes the window content to be displayed in the title bar region.

## TitleBar visual states

<xref:Microsoft.Maui.Controls.TitleBar> defines the following visual states that can be used to initiate a visual change to the <xref:Microsoft.Maui.Controls.TitleBar>:

- `IconVisible`
- `IconCollapsed`
- `TitleVisible`
- `TitleCollapsed`
- `SubtitleVisible`
- `SubtitleCollapsed`
- `LeadingContentVisible`
- `LeadingContentCollapsed`
- `ContentVisible`
- `ContentCollapsed`
- `TrailingContentVisible`
- `TrailingContentCollapsed`
- `TitleBarTitleActive`
- `TitleBarTitleInactive`

The following XAML example shows how to define a visual state for the `TitleBarTitleActive` and `TitleBarTitleInactive` states:

```xaml
<TitleBar ...>
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroupList>
            <VisualStateGroup x:Name="TitleActiveStates">
                <VisualState x:Name="TitleBarTitleActive">
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor" Value="Transparent" />
                        <Setter Property="ForegroundColor" Value="Black" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="TitleBarTitleInactive">
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor" Value="White" />
                        <Setter Property="ForegroundColor" Value="Gray" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    <VisualStateManager.VisualStateGroups>
</TitleBar>
```

In this example, the visual state sets the `BackgroundColor` and `ForegroundColor` properties to specific colors based on whether the title bar is active or inactive.

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).
