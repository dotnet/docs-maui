---
title: "Display menu items"
description: "Learn how to create menu items for menus such as ListView item context menus and Shell app flyout menus."
ms.date: 08/02/2023
---

# Display menu items

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.MenuItem> class can be used to define menu items for menus such as <xref:Microsoft.Maui.Controls.ListView> item context menus and Shell app flyout menus.

The following screenshots show <xref:Microsoft.Maui.Controls.MenuItem> objects in a  <xref:Microsoft.Maui.Controls.ListView> context menu on iOS and Android:

IMAGE GOES HERE

The <xref:Microsoft.Maui.Controls.MenuItem> class defines the following properties:

- <xref:Microsoft.Maui.Controls.MenuItem.Command), of type `ICommand`, allows binding user actions, such as finger taps or clicks, to commands defined on a viewmodel.
- <xref:Microsoft.Maui.Controls.MenuItem.CommandParameter), of type `object`, specifies the parameter that should be passed to the `Command`.
- <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource), of type <xref:Microsoft.Maui.Controls.ImageSource>, defines the display icon.
- <xref:Microsoft.Maui.Controls.MenuItem.IsDestructive), of type `bool`, indicates whether the <xref:Microsoft.Maui.Controls.MenuItem> removes its associated UI element from the list.
- <xref:Microsoft.Maui.Controls.MenuItem.IsEnabled), of type `bool`, indicates whether this object responds to user input.
- <xref:Microsoft.Maui.Controls.MenuItem.Text), of type `string`, specifies the display text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings.

## Create a MenuItem

<xref:Microsoft.Maui.Controls.MenuItem> objects can be used within a context menu on a  <xref:Microsoft.Maui.Controls.ListView> object's items. The most common pattern is to create <xref:Microsoft.Maui.Controls.MenuItem> objects within a `ViewCell` instance, which is used as the `DataTemplate` object for the  <xref:Microsoft.Maui.Controls.ListView>s `ItemTemplate`. When the  <xref:Microsoft.Maui.Controls.ListView> object is populated it will create each item using the `DataTemplate`, exposing the <xref:Microsoft.Maui.Controls.MenuItem> choices when the context menu is activated for an item.

The following example shows <xref:Microsoft.Maui.Controls.MenuItem> instantiation within the context of a  <xref:Microsoft.Maui.Controls.ListView> object:

```xaml
<ListView>
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <ViewCell.ContextActions>
                    <MenuItem Text="Context Menu Option" />
                </ViewCell.ContextActions>
                <Label Text="{Binding .}" />
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

A <xref:Microsoft.Maui.Controls.MenuItem> can also be created in code:

```csharp
// A function returns a ViewCell instance that
// is used as the template for each list item
DataTemplate dataTemplate = new DataTemplate(() =>
{
    // A Label displays the list item text
    Label label = new Label();
    label.SetBinding(Label.TextProperty, ".");

    // A ViewCell serves as the DataTemplate
    ViewCell viewCell = new ViewCell
    {
        View = label
    };

    // Add a MenuItem instance to the ContextActions
    MenuItem menuItem = new MenuItem
    {
        Text = "Context Menu Option"
    };
    viewCell.ContextActions.Add(menuItem);

    // The function returns the custom ViewCell
    // to the DataTemplate constructor
    return viewCell;
});

// Finally, the dataTemplate is provided to
// the ListView object
ListView listView = new ListView
{
    ...
    ItemTemplate = dataTemplate
};
```

## Define MenuItem behavior with events

The <xref:Microsoft.Maui.Controls.MenuItem> class exposes a `Clicked` event. An event handler can be attached to this event to react to taps or clicks on the <xref:Microsoft.Maui.Controls.MenuItem> instance in XAML:

```xaml
<MenuItem ...
          Clicked="OnItemClicked" />
```

An event handler can also be attached in code:

```csharp
MenuItem item = new MenuItem { ... }
item.Clicked += OnItemClicked;
```

Previous examples referenced an `OnItemClicked` event handler. The following code shows an example implementation:

```csharp
void OnItemClicked(object sender, EventArgs e)
{
    // The sender is the menuItem
    MenuItem menuItem = sender as MenuItem;

    // Access the list item through the BindingContext
    var contextItem = menuItem.BindingContext;

    // Do something with the contextItem here
}
```

## Define MenuItem behavior with MVVM

The <xref:Microsoft.Maui.Controls.MenuItem> class supports the Model-View-ViewModel (MVVM) pattern through [`BindableProperty`](xref:Xamarin.Forms.BindableProperty) objects and the `ICommand` interface. The following XAML shows <xref:Microsoft.Maui.Controls.MenuItem> instances bound to commands defined on a viewmodel:

```xaml
<ContentPage.BindingContext>
    <viewmodels:ListPageViewModel />
</ContentPage.BindingContext>

<StackLayout>
    <Label Text="{Binding Message}" ... />
    <ListView ItemsSource="{Binding Items}">
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <ViewCell.ContextActions>
                        <MenuItem Text="Edit"
                                    IconImageSource="icon.png"
                                    Command="{Binding Source={x:Reference contentPage}, Path=BindingContext.EditCommand}"
                                    CommandParameter="{Binding .}"/>
                        <MenuItem Text="Delete"
                                    Command="{Binding Source={x:Reference contentPage}, Path=BindingContext.DeleteCommand}"
                                    CommandParameter="{Binding .}"/>
                    </ViewCell.ContextActions>
                    <Label Text="{Binding .}" />
                </ViewCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
</StackLayout>
```

In the previous example, two <xref:Microsoft.Maui.Controls.MenuItem> objects are defined with their `Command` and `CommandParameter` properties bound to commands on the viewmodel. The viewmodel contains the commands referenced in the XAML:

```csharp
public class ListPageViewModel : INotifyPropertyChanged
{
    ...

    public ICommand EditCommand => new Command<string>((string item) =>
    {
        Message = $"Edit command was called on: {item}";
    });

    public ICommand DeleteCommand => new Command<string>((string item) =>
    {
        Message = $"Delete command was called on: {item}";
    });
}
```

The sample application includes a `DataService` class used to get a list of items for populating the  <xref:Microsoft.Maui.Controls.ListView> objects. A viewmodel is instantiated, with items from the `DataService` class, and set as the `BindingContext` in the code-behind:

```csharp
public MenuItemXamlMvvmPage()
{
    InitializeComponent();
    BindingContext = new ListPageViewModel(DataService.GetListItems());
}
```

## MenuItem icons

> [!WARNING]
> <xref:Microsoft.Maui.Controls.MenuItem> objects only display icons on Android. On other platforms, only the text specified by the `Text` property will be displayed.

Icons are specified using the `IconImageSource` property. If an icon is specified, the text specified by the `Text` property will not be displayed. The following screenshot shows a <xref:Microsoft.Maui.Controls.MenuItem> with an icon on Android:

!["Screenshot of MenuItem icon on Android"](menuitem-images/menuitem-android-icon.png "Screenshot of MenuItem icon on Android")

For more information on using images in Xamarin.Forms, see [Images in Xamarin.Forms](~/xamarin-forms/user-interface/images.md).

## Enable or disable a MenuItem at runtime

To enable or disable a <xref:Microsoft.Maui.Controls.MenuItem> at runtime, bind its `Command` property to an `ICommand` implementation, and ensure that a `canExecute` delegate enables and disables the `ICommand` as appropriate.

> [!IMPORTANT]
> Do not bind the `IsEnabled` property to another property when using the `Command` property to enable or disable the <xref:Microsoft.Maui.Controls.MenuItem>.

The following example shows a <xref:Microsoft.Maui.Controls.MenuItem> whose `Command` property binds to an `ICommand` named `MyCommand`:

```xaml
<MenuItem Text="My menu item"
          Command="{Binding MyCommand}" />
```

The `ICommand` implementation requires a `canExecute` delegate that returns the value of a `bool` property to enable and disable the <xref:Microsoft.Maui.Controls.MenuItem>:

```csharp
public class MyViewModel : INotifyPropertyChanged
{
    bool isMenuItemEnabled = false;
    public bool IsMenuItemEnabled
    {
        get { return isMenuItemEnabled; }
        set
        {
            isMenuItemEnabled = value;
            MyCommand.ChangeCanExecute();
        }
    }

    public Command MyCommand { get; private set; }

    public MyViewModel()
    {
        MyCommand = new Command(() =>
        {
            // Execute logic here
        },
        () => IsMenuItemEnabled);
    }
}
```

In this example, the <xref:Microsoft.Maui.Controls.MenuItem> is disabled until the `IsMenuItemEnabled` property is set. When this occurs, the `Command.ChangeCanExecute` method is called which causes the `canExecute` delegate for `MyCommand` to be re-evaluated.

## Cross-platform context menu behavior

Context menus are accessed and displayed differently on each platform.

On Android, the context menu is activated by long-press on a list item. The context menu replaces the title and navigation bar area and <xref:Microsoft.Maui.Controls.MenuItem> options are displayed as horizontal buttons.

IMAGE GOES HERE

On iOS, the context menu is activated by swiping on a list item. The context menu is displayed on the list item and `MenuItems` are displayed as horizontal buttons.

IMAGE GOES HERE

On UWP, the context menu is activated by right-clicking on a list item. The context menu is displayed near the cursor as a vertical list.

IMAGE GOES HERE
