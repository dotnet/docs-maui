---
title: "NavigationPage"
description: "The .NET MAUI NavigationPage is used to perform hierarchical navigation through a stack of last-in, first-out (LIFO) pages."
ms.date: 09/30/2024
---

# NavigationPage

:::image type="content" source="media/navigationpage/pages.png" alt-text=".NET MAUI NavigationPage." border="false":::

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.NavigationPage> provides a hierarchical navigation experience where you're able to navigate through pages, forwards and backwards, as desired. <xref:Microsoft.Maui.Controls.NavigationPage> provides navigation as a last-in, first-out (LIFO) stack of <xref:Microsoft.Maui.Controls.Page> objects.

<xref:Microsoft.Maui.Controls.NavigationPage> defines the following properties:

- `BarBackground`, of type <xref:Microsoft.Maui.Controls.Brush>, specifies the background of the navigation bar as a <xref:Microsoft.Maui.Controls.Brush>.
- `BarBackgroundColor`, of type <xref:Microsoft.Maui.Graphics.Color>, specifies the background color of the navigation bar.
- `BackButtonTitle`, of type `string`, represents the text to use for the back button. This is an attached property.
- `BarTextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, specifies the color of the text on the navigation bar.
- `CurrentPage`, of type <xref:Microsoft.Maui.Controls.Page>, represents the page that's on top of the navigation stack. This is a read-only property.
- `HasNavigationBar`, of type `bool`, represents whether a navigation bar is present on the <xref:Microsoft.Maui.Controls.NavigationPage>. The default value of this property is `true`. This is an attached property.
- `HasBackButton`, of type `bool`, represents whether the navigation bar includes a back button. The default value of this property is `true`. This is an attached property.
- `IconColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the background color of the icon in the navigation bar. This is an attached property.
- `RootPage`, of type <xref:Microsoft.Maui.Controls.Page>, represents the root page of the navigation stack. This is a read-only property.
- `TitleIconImageSource`, of type <xref:Microsoft.Maui.Controls.ImageSource>, defines the icon that represents the title on the navigation bar. This is an attached property.
- `TitleView`, of type <xref:Microsoft.Maui.Controls.View>, defines the view that can be displayed in the navigation bar. This is an attached property.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The <xref:Microsoft.Maui.Controls.NavigationPage> class also defines three events:

- `Pushed` is raised when a page is pushed onto the navigation stack.
- `Popped` is raised when a page is popped from the navigation stack.
- `PoppedToRoot` is raised when the last non-root page is popped from the navigation stack.

All three events receive `NavigationEventArgs` objects that define a read-only <xref:Microsoft.Maui.Controls.Page> property, which retrieves the page that was popped from the navigation stack, or the newly visible page on the stack.

> [!WARNING]
> <xref:Microsoft.Maui.Controls.NavigationPage> is incompatible with .NET MAUI Shell apps, and an exception will be thrown if you attempt to use <xref:Microsoft.Maui.Controls.NavigationPage> in a Shell app. For more information about Shell apps, see [Shell](~/fundamentals/shell/index.md).

## Perform modeless navigation

.NET MAUI supports modeless page navigation. A modeless page stays on screen and remains available until you navigate to another page.

A <xref:Microsoft.Maui.Controls.NavigationPage> is typically used to navigate through a stack of <xref:Microsoft.Maui.Controls.ContentPage> objects. When one page navigates to another, the new page is pushed on the stack and becomes the active page:

:::image type="content" source="media/navigationpage/pushing.png" alt-text="Pushing a page to the navigation stack." border="false":::

When the second page returns back to the first page, a page is popped from the stack, and the new topmost page then becomes active:

:::image type="content" source="media/navigationpage/popping.png" alt-text="Popping a page from the navigation stack." border="false":::

A <xref:Microsoft.Maui.Controls.NavigationPage> consists of a navigation bar, with the active page being displayed below the navigation bar. The following diagram shows the main components of the navigation bar:

:::image type="content" source="media/navigationpage/components.png" alt-text="NavigationPage components." border="false":::

An optional icon can be displayed between the back button and the title.

Navigation methods are exposed by the `Navigation` property on any <xref:Microsoft.Maui.Controls.Page> derived types. These methods provide the ability to push pages onto the navigation stack, to pop pages from the stack, and to manipulate the stack.

> [!TIP]
> It's recommended that a <xref:Microsoft.Maui.Controls.NavigationPage> should only be populated with <xref:Microsoft.Maui.Controls.ContentPage> objects.

### Create the root page

::: moniker range="=net-maui-8.0"

An app that is structured around multiple pages always has a *root* page, which is the first page added to the navigation stack. This is accomplished by creating a <xref:Microsoft.Maui.Controls.NavigationPage> object whose constructor argument is the root page of the app, and setting the resulting object as the value of the `App.MainPage` property:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage());
    }
}
```

::: moniker-end

::: moniker range=">=net-maui-9.0"

An app that is structured around multiple pages always has a *root* page, which is the first page added to the navigation stack. This is accomplished by creating a <xref:Microsoft.Maui.Controls.NavigationPage> object whose constructor argument is the root page of the app, and setting the resulting object as the root page of a <xref:Microsoft.Maui.Controls.Window>:

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage()));
    }    
}
```

::: moniker-end

> [!NOTE]
> The `RootPage` property of a <xref:Microsoft.Maui.Controls.NavigationPage> provides access to the first page in the navigation stack.

### Push pages to the navigation stack

A page can be navigated to by calling the `PushAsync` method on the `Navigation` property of the current page:

```csharp
await Navigation.PushAsync(new DetailsPage());
```

In this example, the `DetailsPage` object is pushed onto the navigation stack, where it becomes the active page.

> [!NOTE]
> The `PushAsync` method has an override that includes a `bool` argument that specifies whether to display a page transition during navigation. The `PushAsync` method that lacks the `bool` argument enables the page transition by default.

### Pop pages from the navigation stack

The active page can be popped from the navigation stack by pressing the *Back* button on a device, regardless of whether this is a physical button on the device or an on-screen button.

To programmatically return to the previous page, the `PopAsync` method should be called on the `Navigation` property of the current page:

```csharp
await Navigation.PopAsync();
```

In this example, the current page is removed from the navigation stack, with the new topmost page becoming the active page.

> [!NOTE]
> The `PopAsync` method has an override that includes a `bool` argument that specifies whether to display a page transition during navigation. The `PopAsync` method that lacks the `bool` argument enables the page transition by default.

In addition, the `Navigation` property of each page also exposes a `PopToRootAsync` method that pops all but the root page off the navigation stack, therefore making the app's root page the active page.

## Manipulate the navigation stack

The `Navigation` property of a <xref:Microsoft.Maui.Controls.Page> exposes a `NavigationStack` property from which the pages in the navigation stack can be obtained. While .NET MAUI maintains access to the navigation stack, the `Navigation` property provides the `InsertPageBefore` and `RemovePage` methods for manipulating the stack by inserting pages or removing them.

The `InsertPageBefore` method inserts a specified page in the navigation stack before an existing specified page, as shown in the following diagram:

:::image type="content" source="media/navigationpage/insert-page-before.png" alt-text="Inserting a page in the navigation stack." border="false":::

The `RemovePage` method removes the specified page from the navigation stack, as shown in the following diagram:

:::image type="content" source="media/navigationpage/remove-page.png" alt-text="Removing a page from the navigation stack." border="false":::

Together, these methods enable a custom navigation experience, such as replacing a login page with a new page following a successful login.

## Perform modal navigation

.NET MAUI supports modal page navigation. A modal page encourages users to complete a self-contained task that cannot be navigated away from until the task is completed or cancelled.

A modal page can be any of the page types supported by .NET MAUI. To display a page modally, the app should push it onto the modal stack, where it will become the active page:

:::image type="content" source="media/navigationpage/pushing-modal.png" alt-text="Pushing a page to the modal stack." border="false":::

To return to the previous page the app should pop the current page from the modal stack, and the new topmost page becomes the active page:

:::image type="content" source="media/navigationpage/popping-modal.png" alt-text="Popping a page from the modal stack." border="false":::

Modal navigation methods are exposed by the `Navigation` property on any <xref:Microsoft.Maui.Controls.Page> derived types. These methods provide the ability to push pages onto the modal stack, and pop pages from the modal stack. The `Navigation` property also exposes a `ModalStack` property from which pages in the modal stack can be obtained. However, there is no concept of performing modal stack manipulation, or popping to the root page in modal navigation. This is because these operations are not universally supported on the underlying platforms.

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.NavigationPage> object is not required for performing modal page navigation.

### Push pages to the modal stack

A page can be modally navigated to by calling the `PushModalAsync` method on the `Navigation` property of the current page:

```csharp
await Navigation.PushModalAsync(new DetailsPage());
```

In this example, the `DetailsPage` object is pushed onto the modal stack, where it becomes the active page.

> [!NOTE]
> The `PushModalAsync` method has an override that includes a `bool` argument that specifies whether to display a page transition during navigation. The `PushModalAsync` method that lacks the `bool` argument enables the page transition by default.

### Pop pages from the modal stack

The active page can be popped from the modal stack by pressing the *Back* button on a device, regardless of whether this is a physical button on the device or an on-screen button.

To programmatically return to the original page, the `PopModalAsync` method should be called on the `Navigation` property of the current page:

```csharp
await Navigation.PopModalAsync();
```

In this example, the current page is removed from the modal stack, with the new topmost page becoming the active page.

> [!NOTE]
> The `PopModalAsync` method has an override that includes a `bool` argument that specifies whether to display a page transition during navigation. The `PopModalAsync` method that lacks the `bool` argument enables the page transition by default.

### Disable the back button

On Android, you can always return to the previous page by pressing the standard *Back* button on the device. If the modal page requires a self-contained task to be completed before leaving the page, the app must disable the *Back* button. This can be accomplished by overriding the `Page.OnBackButtonPressed` method on the modal page.

## Pass data during navigation

Sometimes it's necessary for a page to pass data to another page during navigation. Two standard techniques for accomplishing this are passing data through a page constructor, and by setting the new page's `BindingContext` to the data.

### Pass data through a page constructor

The simplest technique for passing data to another page during navigation is through a page constructor argument:

```csharp
Contact contact = new Contact
{
    Name = "Jane Doe",
    Age = 30,
    Occupation = "Developer",
    Country = "USA"
};
...
await Navigation.PushModalAsync(new DetailsPage(contact));
```

In this example, a `Contact` object is passed as a constructor argument to `DetailPage`. The `Contact` object can then be displayed by `DetailsPage`.

### Pass data through a BindingContext

An alternative approach for passing data to another page during navigation is by setting the new page's `BindingContext` to the data:

```csharp
Contact contact = new Contact
{
    Name = "Jane Doe",
    Age = 30,
    Occupation = "Developer",
    Country = "USA"
};

await Navigation.PushAsync(new DetailsPage
{
    BindingContext = contact  
});
```

The advantage of passing navigation data via a page's `BindingContext` is that the new page can use data binding to display the data:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp"
             x:Class="MyMauiApp.DetailsPage"
             Title="Details"
             x:DataType="local:Contact">
    <StackLayout>
        <Label Text="{Binding Name}" />
        <Label Text="{Binding Occupation}" />
    </StackLayout>
</ContentPage>
```

For more information about data binding, see [Data binding](~/fundamentals/data-binding/index.md).

## Display views in the navigation bar

Any .NET MAUI <xref:Microsoft.Maui.Controls.View> can be displayed in the navigation bar of a <xref:Microsoft.Maui.Controls.NavigationPage>. This is accomplished by setting the `NavigationPage.TitleView` attached property to a <xref:Microsoft.Maui.Controls.View>. This attached property can be set on any <xref:Microsoft.Maui.Controls.Page>, and when the <xref:Microsoft.Maui.Controls.Page> is pushed onto a <xref:Microsoft.Maui.Controls.NavigationPage>, the <xref:Microsoft.Maui.Controls.NavigationPage> will respect the value of the property.

The following example shows how to set the `NavigationPage.TitleView` attached property:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="NavigationPageTitleView.TitleViewPage">
    <NavigationPage.TitleView>
        <Slider HeightRequest="44"
                WidthRequest="300" />
    </NavigationPage.TitleView>
    ...
</ContentPage>
```

The equivalent C# code is:

```csharp
Slider titleView = new Slider { HeightRequest = 44, WidthRequest = 300 };
NavigationPage.SetTitleView(this, titleView);
```

In this example, a <xref:Microsoft.Maui.Controls.Slider> is displayed in the navigation bar of the <xref:Microsoft.Maui.Controls.NavigationPage>, to control zooming.

> [!IMPORTANT]
> Many views won't appear in the navigation bar unless the size of the view is specified with the <xref:Microsoft.Maui.Controls.VisualElement.WidthRequest> and <xref:Microsoft.Maui.Controls.VisualElement.HeightRequest> properties.

Because the <xref:Microsoft.Maui.Controls.Layout> class derives from the <xref:Microsoft.Maui.Controls.View> class, the `TitleView` attached property can be set to display a layout class that contains multiple views. However, this can result in clipping if the view displayed in the navigation bar is larger than the default size of the navigation bar. However, on Android, the height of the navigation bar can be changed by setting the `NavigationPage.BarHeight` bindable property to a `double` representing the new height. <!--For more information, see [Set the navigation bar height on a NavigationPage](~/platform-integration/android/navigationpage-bar-height.md).-->

Alternatively, an extended navigation bar can be suggested by placing some of the content in the navigation bar, and some in a view at the top of the page content that you color match to the navigation bar. In addition, on iOS the separator line and shadow that's at the bottom of the navigation bar can be removed by setting the `NavigationPage.HideNavigationBarSeparator` bindable property to `true`. <!--For more information, see [Hiding the Navigation Bar Separator on a NavigationPage](~/platform-integration/ios/navigation-bar-separator.md).-->

> [!TIP]
> The `BackButtonTitle`, `Title`, `TitleIconImageSource`, and `TitleView` properties can all define values that occupy space on the navigation bar. While the navigation bar size varies by platform and screen size, setting all of these properties will result in conflicts due to the limited space available. Instead of attempting to use a combination of these properties, you may find that you can better achieve your desired navigation bar design by only setting the `TitleView` property.
