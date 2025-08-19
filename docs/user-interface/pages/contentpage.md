---
title: "ContentPage"
description: "The .NET MAUI ContentPage displays a single view, which is often a layout, and is the most common page type."
ms.date: 08/19/2025
---

# ContentPage

:::image type="content" source="media/contentpage/pages.png" alt-text=".NET MAUI ContentPage." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ContentPage> displays a single view, which is often a layout such as as <xref:Microsoft.Maui.Controls.Grid> or <xref:Microsoft.Maui.Controls.StackLayout>, and is the most common page type.

<xref:Microsoft.Maui.Controls.ContentPage> defines the following properties:

::: moniker range="=net-maui-8.0"

- <xref:Microsoft.Maui.Controls.ContentPage.Content> property, of type <xref:Microsoft.Maui.Controls.View>, which defines the view that represents the page's content.
- <xref:Microsoft.Maui.Controls.ContentPage.HideSoftInputOnTapped>, of type `bool`, which indicates whether tapping anywhere on the page will cause the soft input keyboard to hide if it's visible on Android and iOS.

::: moniker-end

::: moniker range=">=net-maui-9.0"

- <xref:Microsoft.Maui.Controls.ContentPage.Content> property, of type <xref:Microsoft.Maui.Controls.View>, which defines the view that represents the page's content.
- <xref:Microsoft.Maui.Controls.ContentPage.HideSoftInputOnTapped>, of type `bool`, which indicates whether tapping anywhere on the page will cause the soft input keyboard to hide if it's visible on Android, iOS, and Mac Catalyst.

::: moniker-end

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be the target of data bindings, and styled.

In addition, <xref:Microsoft.Maui.Controls.ContentPage> inherits `Title`, `IconImageSource`, `BackgroundImageSource`, `IsBusy`, and `Padding` bindable properties from the <xref:Microsoft.Maui.Controls.Page> class.

> [!NOTE]
> The `Content` property is the content property of the <xref:Microsoft.Maui.Controls.ContentPage> class, and therefore does not need to be explicitly set from XAML.

.NET MAUI apps typically contain multiple pages that derive from <xref:Microsoft.Maui.Controls.ContentPage>, and navigation between these pages can be performed. For more information about page navigation, see [NavigationPage](navigationpage.md).

A <xref:Microsoft.Maui.Controls.ContentPage> can be templated with a control template. For more information, see [Control templates](~/fundamentals/controltemplate.md).

## Page.IsBusy

The <xref:Microsoft.Maui.Controls.Page.IsBusy> property sets a framework-managed busy state for the current page, which triggers a platform-level indicator:

- Android: An overlay with an indeterminate progress indicator is shown above the current window.
- iOS: The legacy network activity indicator on the status bar is toggled on iOS versions that still display it; on newer versions no system indicator may be visible.
- Windows: No visual indicator is shown by default.

Important behavior notes:

- The busy indicator is global per window. If multiple pages set `IsBusy` to `true`, the indicator remains until all set it back to `false`.
- The busy state is raised only while the page is visible; when a page disappears, the framework clears its busy state.
- `IsBusy` is intended for broad, app-level work. For page-specific loading UX, prefer an in-page <xref:Microsoft.Maui.Controls.ActivityIndicator> bound to a view model property.

Example with MVVM binding to a view model property:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.SamplePage"
             IsBusy="{Binding IsBusy}">
    <Grid>
        <!-- Page content -->
        <ActivityIndicator IsRunning="{Binding IsBusy}"
                           IsVisible="{Binding IsBusy}" />
    </Grid>
    
    <!-- Alternatively, rely only on ActivityIndicator for custom visuals
         and keep Page.IsBusy unset for full control over UX. -->
</ContentPage>
```

::: moniker range=">=net-maui-10.0"
> [!TIP]
> In .NET 10, `IsBusy` continues to function as described. It uses internal messaging to coordinate with platform alert managers and may not show a visual indicator on some platforms. When you need consistent, branded UX, use <xref:Microsoft.Maui.Controls.ActivityIndicator> or custom overlays instead of relying on `IsBusy` visuals.
::: moniker-end

For more information, see [ActivityIndicator](~/user-interface/controls/activityindicator.md).

## Create a ContentPage

To add a <xref:Microsoft.Maui.Controls.ContentPage> to a .NET MAUI app:

1. In **Solution Explorer** right-click on your project or folder in your project, and select **New Item...**.
1. In the **Add New Item** dialog, expand **Installed > C# Items**, select **.NET MAUI**, and select the **.NET MAUI ContentPage (XAML)** item template, enter a suitable page name, and click the **Add** button:

    :::image type="content" source="media/contentpage/item-template.png" alt-text=".NET MAUI ContentPage item template.":::

Visual Studio then creates a new <xref:Microsoft.Maui.Controls.ContentPage>-derived page, which will be similar to the following example:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MyPage"
             Title="MyPage"
             BackgroundColor="White">
    <StackLayout>
        <Label Text="Welcome to .NET MAUI!"
                VerticalOptions="Center"
                HorizontalOptions="Center" />
        <!-- Other views go here -->
    </StackLayout>
</ContentPage>
```

The child of a <xref:Microsoft.Maui.Controls.ContentPage> is typically a layout, such as <xref:Microsoft.Maui.Controls.Grid> or <xref:Microsoft.Maui.Controls.StackLayout>, with the layout typically containing multiple views. However, the child of the <xref:Microsoft.Maui.Controls.ContentPage> can be a view that displays a collection, such as <xref:Microsoft.Maui.Controls.CollectionView>.

> [!NOTE]
> The value of the `Title` property will be shown on the navigation bar, when the app performs navigation using a <xref:Microsoft.Maui.Controls.NavigationPage>. For more information, see [NavigationPage](navigationpage.md).
