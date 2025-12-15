---
title: "TabbedPage"
description: "The .NET MAUI TabbedPage consists of a series of pages that are navigable by tabs across the top or bottom of the page, with each tab loading the page content."
ms.date: 11/28/2025
---

# TabbedPage

:::image type="content" source="media/tabbedpage/pages.png" alt-text=".NET MAUI TabbedPage." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.TabbedPage> maintains a collection of children of type <xref:Microsoft.Maui.Controls.Page>, only one of which is fully visible at a time. Each child is identified by a series of tabs across the top or bottom of the page. Typically, each child will be a <xref:Microsoft.Maui.Controls.ContentPage> and when its tab is selected the page content is displayed.

<xref:Microsoft.Maui.Controls.TabbedPage> defines the following properties:

- `BarBackground`, of type <xref:Microsoft.Maui.Controls.Brush>, defines the background of the tab bar.
- `BarBackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the background color of the tab bar.
- `BarTextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, represents the color of the text on the tab bar.
- `SelectedTabColor`, of type <xref:Microsoft.Maui.Graphics.Color>, indicates the color of a tab when it's selected.
- `UnselectedTabColor`, of type <xref:Microsoft.Maui.Graphics.Color>, represents the color of a tab when it's unselected.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The title of a tab is defined by the <xref:Microsoft.Maui.Controls.Page.Title?displayProperty=nameWithType> property of the child page, and the tab icon is defined by the <xref:Microsoft.Maui.Controls.Page.IconImageSource?displayProperty=nameWithType> property of the child page.

In a <xref:Microsoft.Maui.Controls.TabbedPage>, each <xref:Microsoft.Maui.Controls.Page> object is created when the <xref:Microsoft.Maui.Controls.TabbedPage> is constructed. This can lead to a poor user experience, particularly if the <xref:Microsoft.Maui.Controls.TabbedPage> is the root page of your app. However, .NET MAUI Shell enables pages accessed through a tab bar to be created on demand, in response to navigation. For more information about Shell apps, see [Shell](~/fundamentals/shell/index.md).

> [!WARNING]
> <xref:Microsoft.Maui.Controls.TabbedPage> is incompatible with the .NET MAUI Shell visual hierarchy, and an exception will be thrown if you attempt to add <xref:Microsoft.Maui.Controls.TabbedPage> to the Shell visual hierarchy. This includes registering a <xref:Microsoft.Maui.Controls.TabbedPage> as a route with `Routing.RegisterRoute` and navigating to it with `Shell.Current.GoToAsync`. However, it's valid to display a <xref:Microsoft.Maui.Controls.TabbedPage> as a modal page in a Shell app by using `Navigation.PushModalAsync`, because modal pages exist in a separate navigation space. If you need tab-based navigation as part of the Shell visual hierarchy, use Shell tabs instead. For more information, see [Shell tabs](~/fundamentals/shell/tabs.md) and [Perform modal navigation](~/user-interface/pages/navigationpage.md#perform-modal-navigation).

## Create a TabbedPage

Two approaches can be used to create a <xref:Microsoft.Maui.Controls.TabbedPage>:

- Populate the <xref:Microsoft.Maui.Controls.TabbedPage> with a collection of child <xref:Microsoft.Maui.Controls.Page> objects, such as a collection of <xref:Microsoft.Maui.Controls.ContentPage> objects. For more information, see [Populate a TabbedPage with a Page collection](#populate-a-tabbedpage-with-a-page-collection).
- Assign a collection to the `ItemsSource` property and assign a <xref:Microsoft.Maui.Controls.DataTemplate> to the `ItemTemplate` property to return pages for objects in the collection. For more information, see [Populate a TabbedPage with a DataTemplate](#populate-a-tabbedpage-with-a-datatemplate).

> [!IMPORTANT]
> A <xref:Microsoft.Maui.Controls.TabbedPage> should only be populated with <xref:Microsoft.Maui.Controls.NavigationPage> and <xref:Microsoft.Maui.Controls.ContentPage> objects.

Regardless of the approach taken, the location of the tab bar in a <xref:Microsoft.Maui.Controls.TabbedPage> is platform-dependent:

- On iOS, the list of tabs appears at the bottom of the screen, and the page content is above. Each tab consists of a title and an icon. In portrait orientation, tab bar icons appear above tab titles. In landscape orientation, icons and titles appear side by side. In addition, a regular or compact tab bar may be displayed, depending on the device and orientation. If there are more than five tabs, a **More** tab will appear, which can be used to access the additional tabs.
- On Android, the list of tabs appears at the top of the screen, and the page content is below. Each tab consists of a title and an icon. However, the tabs can be moved to the bottom of the screen with a platform-specific. If there are more than five tabs, and the tab list is at the bottom of the screen, a *More* tab will appear that can be used to access the additional tabs. For information about moving the tabs to the bottom of the screen, see [TabbedPage toolbar placement on Android](~/android/platform-specifics/tabbedpage-toolbar-placement.md).
- On Windows, the list of tabs appears at the top of the screen, and the page content is below. Each tab consists of a title. <!--However, icons can be added to each tab with a platform-specific. For more information, see [TabbedPage Icons on Windows](~/platform/windows/tabbedpage-icons.md).-->

### Populate a TabbedPage with a Page collection

A <xref:Microsoft.Maui.Controls.TabbedPage> can be populated with a collection of child <xref:Microsoft.Maui.Controls.Page> objects, which will typically be <xref:Microsoft.Maui.Controls.ContentPage> objects. This is achieved by adding <xref:Microsoft.Maui.Controls.ContentPage> objects as children of the <xref:Microsoft.Maui.Controls.TabbedPage>:

```xaml
<TabbedPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:local="clr-namespace:TabbedPageWithNavigationPage"
            x:Class="TabbedPageWithNavigationPage.MainPage">
    <local:TodayPage />
    <local:SchedulePage />
    <local:SettingsPage />
</TabbedPage>
```

<xref:Microsoft.Maui.Controls.Page> objects that are added as child elements of <xref:Microsoft.Maui.Controls.TabbedPage> are added to the `Children` collection. The `Children` property of the `MultiPage<T>` class, from which <xref:Microsoft.Maui.Controls.TabbedPage> derives, is the [`ContentProperty`](xref:Microsoft.Maui.Controls.ContentPropertyAttribute) of `MultiPage<T>`. Therefore, in XAML it's not necessary to explicitly assign the <xref:Microsoft.Maui.Controls.Page> objects to the `Children` property.

The following screenshot shows the appearance of the resulting tab bar on the <xref:Microsoft.Maui.Controls.TabbedPage>:

:::image type="content" source="media/tabbedpage/tabbar.png" alt-text=".NET MAUI tab bar on a TabbedPage.":::

The page content for a tab appears when the tab is selected.

### Populate a TabbedPage with a DataTemplate

<xref:Microsoft.Maui.Controls.TabbedPage> inherits `ItemsSource`, `ItemTemplate`, and `SelectedItem` bindable properties from the `MultiPage<T>` class. These properties enable you to generate <xref:Microsoft.Maui.Controls.TabbedPage> children dynamically, by setting the `ItemsSource` property to an `IEnumerable` collection of objects with public properties suitable for data bindings, and by setting the `ItemTemplate` property to a <xref:Microsoft.Maui.Controls.DataTemplate> with a page type as the root element.

The following example shows generating <xref:Microsoft.Maui.Controls.TabbedPage> children dynamically:

```xaml
<TabbedPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:local="clr-namespace:TabbedPageDemo"
            x:Class="TabbedPageDemo.MainPage"
            ItemsSource="{x:Static local:MonkeyDataModel.All}"
            x:DataType="local:Monkey">
    <TabbedPage.ItemTemplate>
        <DataTemplate>
            <ContentPage Title="{Binding Name}"
                         IconImageSource="monkeyicon.png">
                <StackLayout Padding="5, 25">
                    <Label Text="{Binding Name}"
                           FontAttributes="Bold"
                           FontSize="18"
                           HorizontalOptions="Center" />
                    <Image Source="{Binding PhotoUrl}"
                           HorizontalOptions="Center"
                           WidthRequest="200"
                           HeightRequest="200" />
                    <StackLayout Padding="50, 10">
                        <StackLayout Orientation="Horizontal">
                            <Label Text="Family: "
                                   FontAttributes="Bold" />
                            <Label Text="{Binding Family}" />
                        </StackLayout>
                        ...
                    </StackLayout>
                </StackLayout>
            </ContentPage>
        </DataTemplate>
    </TabbedPage.ItemTemplate>
</TabbedPage>
```

In this example, each tab consists of a <xref:Microsoft.Maui.Controls.ContentPage> object that uses <xref:Microsoft.Maui.Controls.Image> and <xref:Microsoft.Maui.Controls.Label> objects to display data for the tab:

:::image type="content" source="media/tabbedpage/tabbedpage.png" alt-text="Screenshot of a .NET MAUI TabbedPage.":::

## Navigate within a tab

Navigation can be performed within a tab, provided that the <xref:Microsoft.Maui.Controls.ContentPage> object is wrapped in a <xref:Microsoft.Maui.Controls.NavigationPage> object:

```xaml
<TabbedPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:local="clr-namespace:TabbedPageWithNavigationPage"
            x:Class="TabbedPageWithNavigationPage.MainPage">
    <local:TodayPage />
    <NavigationPage Title="Schedule"
                    IconImageSource="schedule.png">
        <x:Arguments>
            <local:SchedulePage />
        </x:Arguments>
    </NavigationPage>
</TabbedPage>
```

In this example, the <xref:Microsoft.Maui.Controls.TabbedPage> is populated with two <xref:Microsoft.Maui.Controls.Page> objects. The first child is a <xref:Microsoft.Maui.Controls.ContentPage> object, and the second child is a <xref:Microsoft.Maui.Controls.NavigationPage> object containing a <xref:Microsoft.Maui.Controls.ContentPage> object.

When a <xref:Microsoft.Maui.Controls.ContentPage> is wrapped in a <xref:Microsoft.Maui.Controls.NavigationPage>, forwards page navigation can be performed by calling the `PushAsync` method on the `Navigation` property of the <xref:Microsoft.Maui.Controls.ContentPage> object:

```csharp
await Navigation.PushAsync(new UpcomingAppointmentsPage());
```

For more information about performing navigation using the <xref:Microsoft.Maui.Controls.NavigationPage> class, see [NavigationPage](navigationpage.md).

> [!WARNING]
> While a <xref:Microsoft.Maui.Controls.NavigationPage> can be placed in a  <xref:Microsoft.Maui.Controls.TabbedPage>, it's not recommended to place a <xref:Microsoft.Maui.Controls.TabbedPage> into a <xref:Microsoft.Maui.Controls.NavigationPage>.
