---
title: ".NET MAUI Shell navigation"
description: "Learn how .NET MAUI Shell apps can utilize a URI-based navigation experience that permits navigation to any page in the app, without having to follow a set navigation hierarchy."
ms.date: 10/08/2024
---

# .NET MAUI Shell navigation

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/fundamentals-shell)

.NET Multi-platform App UI (.NET MAUI) Shell includes a URI-based navigation experience that uses routes to navigate to any page in the app, without having to follow a set navigation hierarchy. In addition, it also provides the ability to navigate backwards without having to visit all of the pages on the navigation stack.

The <xref:Microsoft.Maui.Controls.Shell> class defines the following navigation-related properties:

- <xref:Microsoft.Maui.Controls.BackButtonBehavior>, of type <xref:Microsoft.Maui.Controls.BackButtonBehavior>, an attached property that defines the behavior of the back button.
- `CurrentItem`, of type `ShellItem`, the currently selected item.
- `CurrentPage`, of type <xref:Microsoft.Maui.Controls.Page>, the currently presented page.
- `CurrentState`, of type `ShellNavigationState`, the current navigation state of the <xref:Microsoft.Maui.Controls.Shell>.
- `Current`, of type <xref:Microsoft.Maui.Controls.Shell>, a type-casted alias for `Application.Current.MainPage`.

The <xref:Microsoft.Maui.Controls.BackButtonBehavior>, `CurrentItem`, and `CurrentState` properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that these properties can be targets of data bindings.

Navigation is performed by invoking the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method, from the <xref:Microsoft.Maui.Controls.Shell> class. When navigation is about to be performed, the `Navigating` event is fired, and the `Navigated` event is fired when navigation completes.

> [!NOTE]
> Navigation can still be performed between pages in a Shell app by using the `Navigation` property. For more information, see [Perform modeless navigation](~/user-interface/pages/navigationpage.md#perform-modeless-navigation).

## Routes

Navigation is performed in a Shell app by specifying a URI to navigate to. Navigation URIs can have three components:

- A *route*, which defines the path to content that exists as part of the Shell visual hierarchy.
- A *page*. Pages that don't exist in the Shell visual hierarchy can be pushed onto the navigation stack from anywhere within a Shell app. For example, a details page won't be defined in the Shell visual hierarchy, but can be pushed onto the navigation stack as required.
- One or more *query parameters*. Query parameters are parameters that can be passed to the destination page while navigating.

When a navigation URI includes all three components, the structure is: //route/page?queryParameters

### Register routes

Routes can be defined on <xref:Microsoft.Maui.Controls.FlyoutItem>, <xref:Microsoft.Maui.Controls.TabBar>, <xref:Microsoft.Maui.Controls.Tab>, and <xref:Microsoft.Maui.Controls.ShellContent> objects, through their `Route` properties:

```xaml
<Shell ...>
    <FlyoutItem ...
                Route="animals">
        <Tab ...
             Route="domestic">
            <ShellContent ...
                          Route="cats" />
            <ShellContent ...
                          Route="dogs" />
        </Tab>
        <ShellContent ...
                      Route="monkeys" />
        <ShellContent ...
                      Route="elephants" />  
        <ShellContent ...
                      Route="bears" />
    </FlyoutItem>
    <ShellContent ...
                  Route="about" />                  
    ...
</Shell>
```

> [!NOTE]
> All items in the Shell hierarchy have a route associated with them. If you don't set a route, one is generated at runtime. However, generated routes are not guaranteed to be consistent across different app sessions.

The above example creates the following route hierarchy, which can be used in programmatic navigation:

```
animals
  domestic
    cats
    dogs
  monkeys
  elephants
  bears
about
```

To navigate to the <xref:Microsoft.Maui.Controls.ShellContent> object for the `dogs` route, the absolute route URI is `//animals/domestic/dogs`. Similarly, to navigate to the <xref:Microsoft.Maui.Controls.ShellContent> object for the `about` route, the absolute route URI is `//about`.

> [!WARNING]
> An `ArgumentException` will be thrown on app startup if a duplicate route is detected. This exception will also be thrown if two or more routes at the same level in the hierarchy share a route name.

### Register detail page routes

In the <xref:Microsoft.Maui.Controls.Shell> subclass constructor, or any other location that runs before a route is invoked, additional routes can be explicitly registered for any detail pages that aren't represented in the Shell visual hierarchy. This is accomplished with the `Routing.RegisterRoute` method:

```csharp
Routing.RegisterRoute("monkeydetails", typeof(MonkeyDetailPage));
Routing.RegisterRoute("beardetails", typeof(BearDetailPage));
Routing.RegisterRoute("catdetails", typeof(CatDetailPage));
Routing.RegisterRoute("dogdetails", typeof(DogDetailPage));
Routing.RegisterRoute("elephantdetails", typeof(ElephantDetailPage));
```

This example registers detail pages, that aren't defined in the <xref:Microsoft.Maui.Controls.Shell> subclass, as routes. These detail pages can then be navigated to using URI-based navigation, from anywhere within the app. The routes for such pages are known as *global routes*.

> [!WARNING]
> An `ArgumentException` will be thrown if the `Routing.RegisterRoute` method attempts to register the same route to two or more different types.

Alternatively, pages can be registered at different route hierarchies if required:

```csharp
Routing.RegisterRoute("monkeys/details", typeof(MonkeyDetailPage));
Routing.RegisterRoute("bears/details", typeof(BearDetailPage));
Routing.RegisterRoute("cats/details", typeof(CatDetailPage));
Routing.RegisterRoute("dogs/details", typeof(DogDetailPage));
Routing.RegisterRoute("elephants/details", typeof(ElephantDetailPage));
```

This example enables contextual page navigation, where navigating to the `details` route from the page for the `monkeys` route displays the `MonkeyDetailPage`. Similarly, navigating to the `details` route from the page for the `elephants` route displays the `ElephantDetailPage`. For more information, see [Contextual navigation](#contextual-navigation).

> [!NOTE]
> Pages whose routes have been registered with the `Routing.RegisterRoute` method can be deregistered with the `Routing.UnRegisterRoute` method, if required.

## Perform navigation

To perform navigation, a reference to the <xref:Microsoft.Maui.Controls.Shell> subclass must first be obtained. This reference can be obtained by casting the `App.Current.MainPage` property to a <xref:Microsoft.Maui.Controls.Shell> object, or through the `Shell.Current` property. Navigation can then be performed by calling the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method on the <xref:Microsoft.Maui.Controls.Shell> object. This method navigates to a `ShellNavigationState` and returns a `Task` that will complete once the navigation animation has completed. The `ShellNavigationState` object is constructed by the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method, from a `string`, or a `Uri`, and it has its `Location` property set to the `string` or `Uri` argument.

> [!IMPORTANT]
> When a route from the Shell visual hierarchy is navigated to, a navigation stack isn't created. However, when a page that's not in the Shell visual hierarchy is navigated to, a navigation stack is created.

The current navigation state of the <xref:Microsoft.Maui.Controls.Shell> object can be retrieved through the `Shell.Current.CurrentState` property, which includes the URI of the displayed route in the `Location` property.

### Absolute routes

Navigation can be performed by specifying a valid absolute URI as an argument to the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method:

```csharp
await Shell.Current.GoToAsync("//animals/monkeys");
```

This example navigates to the page for the `monkeys` route, with the route being defined on a <xref:Microsoft.Maui.Controls.ShellContent> object. The <xref:Microsoft.Maui.Controls.ShellContent> object that represents the `monkeys` route is a child of a <xref:Microsoft.Maui.Controls.FlyoutItem> object, whose route is `animals`.

### Relative routes

Navigation can also be performed by specifying a valid relative URI as an argument to the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method. The routing system will attempt to match the URI to a <xref:Microsoft.Maui.Controls.ShellContent> object. Therefore, if all the routes in an app are unique, navigation can be performed by only specifying the unique route name as a relative URI.

The following relative route formats are supported:

| Format | Description |
| --- | --- |
| *route* | The route hierarchy will be searched for the specified route, upwards from the current position. The matching page will be pushed to the navigation stack. |
| /*route* | The route hierarchy will be searched from the specified route, downwards from the current position. The matching page will be pushed to the navigation stack. |
| //*route* | The route hierarchy will be searched for the specified route, upwards from the current position. The matching page will replace the navigation stack. |
| ///*route* | The route hierarchy will be searched for the specified route, downwards from the current position. The matching page will replace the navigation stack. |

The following example navigates to the page for the `monkeydetails` route:

```csharp
await Shell.Current.GoToAsync("monkeydetails");
```

In this example, the `monkeyDetails` route is searched for up the hierarchy until the matching page is found. When the page is found, it's pushed to the navigation stack.

#### Contextual navigation

Relative routes enable contextual navigation. For example, consider the following route hierarchy:

```
monkeys
  details
bears
  details
```

When the registered page for the `monkeys` route is displayed, navigating to the `details` route will display the registered page for the `monkeys/details` route. Similarly, when the registered page for the `bears` route is displayed, navigating to the `details` route will display the registered page for the `bears/details` route. For information on how to register the routes in this example, see [Register page routes](#register-detail-page-routes).

### Backwards navigation

Backwards navigation can be performed by specifying ".." as the argument to the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method:

```csharp
await Shell.Current.GoToAsync("..");
```

Backwards navigation with ".." can also be combined with a route:

```csharp
await Shell.Current.GoToAsync("../route");
```

In this example, backwards navigation is performed, and then navigation to the specified route.

> [!IMPORTANT]
> Navigating backwards and into a specified route is only possible if the backwards navigation places you at the current location in the route hierarchy to navigate to the specified route.

Similarly, it's possible to navigate backwards multiple times, and then navigate to a specified route:

```csharp
await Shell.Current.GoToAsync("../../route");
```

In this example, backwards navigation is performed twice, and then navigation to the specified route.

In addition, data can be passed through query properties when navigating backwards:

```csharp
await Shell.Current.GoToAsync($"..?parameterToPassBack={parameterValueToPassBack}");
```

In this example, backwards navigation is performed, and the query parameter value is passed to the query parameter on the previous page.

> [!NOTE]
> Query parameters can be appended to any backwards navigation request.

For more information about passing data when navigating, see [Pass data](#pass-data).

### Invalid routes

The following route formats are invalid:

| Format | Explanation |
| --- | --- |
| //*page* or ///*page* | Global routes currently can't be the only page on the navigation stack. Therefore, absolute routing to global routes is unsupported. |

Use of these route formats results in an `Exception` being thrown.

> [!WARNING]
> Attempting to navigate to a non-existent route results in an `ArgumentException` exception being thrown.

### Debugging navigation

Some of the Shell classes are decorated with the `DebuggerDisplayAttribute`, which specifies how a class or field is displayed by the debugger. This can help to debug navigation requests by displaying data related to the navigation request. For example, the following screenshot shows the `CurrentItem` and `CurrentState` properties of the `Shell.Current` object:

:::image type="content" source="media/navigation/debugger.png" alt-text="Screenshot of debugger.":::

In this example, the `CurrentItem` property, of type <xref:Microsoft.Maui.Controls.FlyoutItem>, displays the title and route of the <xref:Microsoft.Maui.Controls.FlyoutItem> object. Similarly, the `CurrentState` property, of type `ShellNavigationState`, displays the URI of the displayed route within the Shell app.

### Navigation stack

The <xref:Microsoft.Maui.Controls.Tab> class defines a `Stack` property, of type `IReadOnlyList<Page>`, which represents the current navigation stack within the <xref:Microsoft.Maui.Controls.Tab>. The class also provides the following overridable navigation methods:

- `GetNavigationStack`, returns `IReadOnlyList<Page>`, the current navigation stack.
- `OnInsertPageBefore`, that's called when `INavigation.InsertPageBefore` is called.
- `OnPopAsync`, returns `Task<Page>`, and is called when `INavigation.PopAsync` is called.
- `OnPopToRootAsync`, returns `Task`, and is called when `INavigation.OnPopToRootAsync` is called.
- `OnPushAsync`, returns `Task`, and is called when `INavigation.PushAsync` is called.
- `OnRemovePage`, that's called when `INavigation.RemovePage` is called.

The following example shows how to override the `OnRemovePage` method:

```csharp
public class MyTab : Tab
{
    protected override void OnRemovePage(Page page)
    {
        base.OnRemovePage(page);

        // Custom logic
    }
}
```

In this example, `MyTab` objects should be consumed in your Shell visual hierarchy instead of <xref:Microsoft.Maui.Controls.Tab> objects.

## Navigation events

The <xref:Microsoft.Maui.Controls.Shell> class defines the `Navigating` event, which is fired when navigation is about to be performed, either due to programmatic navigation or user interaction. The `ShellNavigatingEventArgs` object that accompanies the `Navigating` event provides the following properties:

| Property | Type | Description |
|---|---|---|
| `Current` | `ShellNavigationState` | The URI of the current page. |
| `Source` | `ShellNavigationSource` | The type of navigation that occurred. |
| `Target` | `ShellNavigationState` | The URI representing where the navigation is destined. |
| `CanCancel`  | `bool` | A value indicating if it's possible to cancel the navigation. |
| `Cancelled`  | `bool` | A value indicating if the navigation was canceled. |

In addition, the `ShellNavigatingEventArgs` class provides a `Cancel` method that can be used to cancel navigation, and a `GetDeferral` method that returns a `ShellNavigatingDeferral` token that can be used to complete navigation. For more information about navigation deferral, see [Navigation deferral](#navigation-deferral).

The <xref:Microsoft.Maui.Controls.Shell> class also defines the `Navigated` event, which is fired when navigation has completed. The `ShellNavigatedEventArgs` object that accompanies the `Navigated` event provides the following properties:

| Property | Type | Description |
|---|---|---|
| `Current`  | `ShellNavigationState` | The URI of the current page. |
| `Previous` | `ShellNavigationState` | The URI of the previous page. |
| `Source` | `ShellNavigationSource` | The type of navigation that occurred. |

> [!IMPORTANT]
> The `OnNavigating` method is called when the `Navigating` event fires. Similarly, the `OnNavigated` method is called when the `Navigated` event fires. Both methods can be overridden in your <xref:Microsoft.Maui.Controls.Shell> subclass to intercept navigation requests.

The `ShellNavigatedEventArgs` and `ShellNavigatingEventArgs` classes both have `Source` properties, of type `ShellNavigationSource`. This enumeration provides the following values:

- `Unknown`
- `Push`
- `Pop`
- `PopToRoot`
- `Insert`
- `Remove`
- `ShellItemChanged`
- `ShellSectionChanged`
- `ShellContentChanged`

Therefore, navigation can be intercepted in an `OnNavigating` override and actions can be performed based on the navigation source. For example, the following code shows how to cancel backwards navigation if the data on the page is unsaved:

```csharp
protected override void OnNavigating(ShellNavigatingEventArgs args)
{
    base.OnNavigating(args);

    // Cancel any back navigation.
    if (args.Source == ShellNavigationSource.Pop)
    {
        args.Cancel();
    }
}
```

## Navigation deferral

Shell navigation can be intercepted and completed or canceled based on user choice. This can be achieved by overriding the `OnNavigating` method in your <xref:Microsoft.Maui.Controls.Shell> subclass, and by calling the `GetDeferral` method on the `ShellNavigatingEventArgs` object. This method returns a `ShellNavigatingDeferral` token that has a `Complete` method, which can be used to complete the navigation request:

```csharp
public MyShell : Shell
{
    // ...
    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        ShellNavigatingDeferral token = args.GetDeferral();

        var result = await DisplayActionSheet("Navigate?", "Cancel", "Yes", "No");
        if (result != "Yes")
        {
            args.Cancel();
        }
        token.Complete();
    }    
}
```

In this example, an action sheet is displayed that invites the user to complete the navigation request, or cancel it. Navigation is canceled by invoking the `Cancel` method on the `ShellNavigatingEventArgs` object. Navigation is completed by invoking the `Complete` method on the `ShellNavigatingDeferral` token that was retrieved by the `GetDeferral` method on the `ShellNavigatingEventArgs` object.

> [!WARNING]
> The <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method will throw a `InvalidOperationException` if a user tries to navigate while there is a pending navigation deferral.

## Pass data

Primitive data can be passed as string-based query parameters when performing URI-based programmatic navigation. This is achieved by appending `?` after a route, followed by a query parameter id, `=`, and a value:

```csharp
async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    string elephantName = (e.CurrentSelection.FirstOrDefault() as Animal).Name;
    await Shell.Current.GoToAsync($"elephantdetails?name={elephantName}");
}
```

This example retrieves the currently selected elephant in the <xref:Microsoft.Maui.Controls.CollectionView>, and navigates to the `elephantdetails` route, passing `elephantName` as a query parameter.

### Pass multiple use object-based navigation data

Multiple use object-based navigation data can be passed with a <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> overload that specifies an `IDictionary<string, object>` argument:

```csharp
async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    Animal animal = e.CurrentSelection.FirstOrDefault() as Animal;
    var navigationParameter = new Dictionary<string, object>
    {
        { "Bear", animal }
    };
    await Shell.Current.GoToAsync($"beardetails", navigationParameter);
}
```

This example retrieves the currently selected bear in the <xref:Microsoft.Maui.Controls.CollectionView>, as an `Animal`. The `Animal` object is added to a `Dictionary` with the key `Bear`. Then, navigation to the `beardetails` route is performed, with the `Dictionary` being passed as a navigation parameter.

Any data that's passed as an `IDictionary<string, object>` argument is retained in memory for the lifetime of the page, and isn't released until the page is removed from the navigation stack. This can be problematic, as shown in the following scenario:

1. `Page1` navigates to `Page2` using the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method, passing in an object called `MyData`. `Page2` then receives `MyData` as a query parameter.
1. `Page2` navigates to `Page3` using the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method, without passing any data.
1. `Page3` navigates backwards with the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method. `Page2` then receives `MyData` again as a query parameter.

While this is desirable in many scenarios, if it isn't desired you should clear the `IDictionary<string, object>` argument with the `Clear` method after it's first been received by a page.

### Pass single use object-based navigation data

Single use object-based navigation data can be passed with a <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> overload that specifies a <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> argument. A <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object is intended for single use navigation data that's cleared after navigation has occurred. The following example shows navigating while passing single use data:

```csharp
async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    Animal animal = e.CurrentSelection.FirstOrDefault() as Animal;
    var navigationParameter = new ShellNavigationQueryParameters
    {
        { "Bear", animal }
    };
    await Shell.Current.GoToAsync($"beardetails", navigationParameter);
}
```

This example retrieves the currently selected bear in the <xref:Microsoft.Maui.Controls.CollectionView>, as an `Animal` that's added to the <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object. Then, navigation to the `beardetails` route is performed, with the <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object being passed as a navigation parameter. After navigation has occurred the data in the <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object is cleared.

## Receive navigation data

There are two approaches to receiving navigation data:

1. The class that represents the page being navigated to, or the class for the page's `BindingContext`, can be decorated with a <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> for each query parameter. For more information, see [Process navigation data using query property attributes](#process-navigation-data-using-query-property-attributes).
1. The class that represents the page being navigated to, or the class for the page's `BindingContext`, can implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface. For more information, see [Process navigation data using a single method](#process-navigation-data-using-a-single-method).

### Process navigation data using query property attributes

Navigation data can be received by decorating the receiving class with a <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> for each string-based query parameter, object-based navigation parameter, or <xref:Microsoft.Maui.Controls.ShellNavigationQueryParameters> object:

```csharp
[QueryProperty(nameof(Bear), "Bear")]
public partial class BearDetailPage : ContentPage
{
    Animal bear;
    public Animal Bear
    {
        get => bear;
        set
        {
            bear = value;
            OnPropertyChanged();
        }
    }

    public BearDetailPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
```

In this example the first argument for the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> specifies the name of the property that will receive the data, with the second argument specifying the parameter id. Therefore, the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> in the above example specifies that the `Bear` property will receive the data passed in the `Bear` navigation parameter in the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method call.

> [!IMPORTANT]
> String-based query parameter values that are received via the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> are automatically URL decoded.

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> isn't trim safe and shouldn't be used with full trimming or NativeAOT. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Process navigation data using a single method](#process-navigation-data-using-a-single-method), [Trim a .NET MAUI app](~/deployment/trimming.md), and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

### Process navigation data using a single method

Navigation data can be received by implementing the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on the receiving class. The <xref:Microsoft.Maui.Controls.IQueryAttributable> interface specifies that the implementing class must implement the `ApplyQueryAttributes` method. This method has a `query` argument, of type `IDictionary<string, object>`, that contains any data passed during navigation. Each key in the dictionary is a query parameter id, with its value corresponding to the object that represents the data. The advantage of using this approach is that navigation data can be processed using a single method, which can be useful when you have multiple items of navigation data that require processing as a whole.

The following example shows a view model class that implements the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface:

```csharp
public class MonkeyDetailViewModel : IQueryAttributable, INotifyPropertyChanged
{
    public Animal Monkey { get; private set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Monkey = query["Monkey"] as Animal;
        OnPropertyChanged("Monkey");
    }
    ...
}
```

In this example, the `ApplyQueryAttributes` method retrieves the object that corresponds to the `Monkey` key in the `query` dictionary, which was passed as an argument to the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method call.

> [!IMPORTANT]
> String-based query parameter values that are received via the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface aren't automatically URL decoded.

#### Pass and process multiple items of data

Multiple string-based query parameters can be passed by connecting them with `&`. For example, the following code passes two data items:

```csharp
async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    string elephantName = (e.CurrentSelection.FirstOrDefault() as Animal).Name;
    string elephantLocation = (e.CurrentSelection.FirstOrDefault() as Animal).Location;
    await Shell.Current.GoToAsync($"elephantdetails?name={elephantName}&location={elephantLocation}");
}
```

This code example retrieves the currently selected elephant in the <xref:Microsoft.Maui.Controls.CollectionView>, and navigates to the `elephantdetails` route, passing `elephantName` and `elephantLocation` as query parameters.

To receive multiple items of data, the class that represents the page being navigated to, or the class for the page's `BindingContext`, can be decorated with a <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> for each string-based query parameter:

```csharp
[QueryProperty(nameof(Name), "name")]
[QueryProperty(nameof(Location), "location")]
public partial class ElephantDetailPage : ContentPage
{
    public string Name
    {
        set
        {
            // Custom logic
        }
    }

    public string Location
    {
        set
        {
            // Custom logic
        }
    }
    ...    
}
```

In this example, the class is decorated with a <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> for each query parameter. The first <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> specifies that the `Name` property will receive the data passed in the `name` query parameter, while the second <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> specifies that the `Location` property will receive the data passed in the `location` query parameter. In both cases, the query parameter values are specified in the URI in the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method call.

::: moniker range=">=net-maui-9.0"

> [!WARNING]
> Receiving navigation data using the <xref:Microsoft.Maui.Controls.QueryPropertyAttribute> isn't trim safe and shouldn't be used with full trimming or NativeAOT. Instead, you should implement the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on types that need to accept query parameters. For more information, see [Trim a .NET MAUI app](~/deployment/trimming.md) and [Native AOT deployment](~/deployment/nativeaot.md).

::: moniker-end

Alternatively, navigation data can be processed by a single method by implementing the <xref:Microsoft.Maui.Controls.IQueryAttributable> interface on the class that represents the page being navigated to, or the class for the page's `BindingContext`:

```csharp
public class ElephantDetailViewModel : IQueryAttributable, INotifyPropertyChanged
{
    public Animal Elephant { get; private set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        string name = HttpUtility.UrlDecode(query["name"].ToString());
        string location = HttpUtility.UrlDecode(query["location"].ToString());
        ...        
    }
    ...
}
```

In this example, the `ApplyQueryAttributes` method retrieves the value of the `name` and `location` query parameters from the URI in the <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> method call.

> [!NOTE]
> String-based query parameters and object-based navigation parameters can be simultaneously passed when performing route-based navigation.

## Back button behavior

Back button appearance and behavior can be redefined by setting the <xref:Microsoft.Maui.Controls.BackButtonBehavior> attached property to a <xref:Microsoft.Maui.Controls.BackButtonBehavior> object. The <xref:Microsoft.Maui.Controls.BackButtonBehavior> class defines the following properties:

- `Command`, of type <xref:System.Windows.Input.ICommand>, which is executed when the back button is pressed.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `IconOverride`, of type <xref:Microsoft.Maui.Controls.ImageSource>, the icon used for the back button.
- `IsEnabled`, of type `boolean`, indicates whether the back button is enabled. The default value is `true`.
- `IsVisible`, of type `boolean`, indicates whether the back button is visible. The default value is `true`.
- `TextOverride`, of type `string`, the text used for the back button.

::: moniker range="=net-maui-8.0"

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings. Each <xref:Microsoft.Maui.Controls.BindableProperty> has a `OneTime` binding mode, which means that data goes from the source to the target but only when the `BindingContext` changes.

::: moniker-end

::: moniker range=">=net-maui-9.0"

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings. The `Command`, `CommandParameter`, `IconOveride`, and `TextOveride` <xref:Microsoft.Maui.Controls.BindableProperty> objects have `OneTime` binding modes, which means that data goes from the source to the target but only when the `BindingContext` changes. The `IsEnabled` and `IsVisible` <xref:Microsoft.Maui.Controls.BindableProperty> objects have `OneWay` binding modes, which means that data goes from the source to the target.

::: moniker-end

The following code shows an example of redefining back button appearance and behavior:

```xaml
<ContentPage ...>    
    <Shell.BackButtonBehavior>
        <BackButtonBehavior Command="{Binding BackCommand}"
                            IconOverride="back.png" />   
    </Shell.BackButtonBehavior>
    ...
</ContentPage>
```

The `Command` property is set to an <xref:System.Windows.Input.ICommand> to be executed when the back button is pressed, and the `IconOverride` property is set to the icon that's used for the back button:

:::image type="content" source="media/navigation/back-button.png" alt-text="Screenshot of a Shell back button icon override.":::
