---
title: "Display menu items"
description: "Learn how to create menu items for menus such as ListView item context menus and Shell app flyout menus."
ms.date: 09/30/2024
---

# Display menu items

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.MenuItem> class can be used to define menu items for menus such as <xref:Microsoft.Maui.Controls.ListView> item context menus and Shell app flyout menus.

The following screenshots show <xref:Microsoft.Maui.Controls.MenuItem> objects in a <xref:Microsoft.Maui.Controls.ListView> context menu on Android:

:::image type="content" source="media/menuitem/text-context-menu-android.png" alt-text="Screenshot of menu items in a ListView context menu on Android.":::

The <xref:Microsoft.Maui.Controls.MenuItem> class defines the following properties:

- <xref:Microsoft.Maui.Controls.MenuItem.Command>, of type <xref:System.Windows.Input.ICommand>, allows binding user actions, such as finger taps or clicks, to commands defined on a viewmodel.
- <xref:Microsoft.Maui.Controls.MenuItem.CommandParameter>, of type `object`, specifies the parameter that should be passed to the `Command`.
- <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource>, of type <xref:Microsoft.Maui.Controls.ImageSource>, defines the menu item icon.
- <xref:Microsoft.Maui.Controls.MenuItem.IsDestructive>, of type `bool`, indicates whether the <xref:Microsoft.Maui.Controls.MenuItem> removes its associated UI element from the list.
- <xref:Microsoft.Maui.Controls.MenuItem.IsEnabled>, of type `bool`, indicates whether the menu item responds to user input.
- <xref:Microsoft.Maui.Controls.MenuItem.Text>, of type `string`, specifies the menu item text.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings.

## Create a MenuItem

To create a menu item, for example as a context menu on a <xref:Microsoft.Maui.Controls.ListView> object's items, create a <xref:Microsoft.Maui.Controls.MenuItem> object within a <xref:Microsoft.Maui.Controls.ViewCell> object that's used as the <xref:Microsoft.Maui.Controls.DataTemplate> object for the <xref:Microsoft.Maui.Controls.ListView>s `ItemTemplate`. When the <xref:Microsoft.Maui.Controls.ListView> object is populated it will create each item using the <xref:Microsoft.Maui.Controls.DataTemplate>, exposing the <xref:Microsoft.Maui.Controls.MenuItem> choices when the context menu is activated for an item.

The following example shows how to create a <xref:Microsoft.Maui.Controls.MenuItem> within the context of a <xref:Microsoft.Maui.Controls.ListView> object:

```xaml
<ListView>
    <ListView.ItemTemplate>
        <DataTemplate>
            <ViewCell>
                <ViewCell.ContextActions>
                    <MenuItem Text="Context menu option" />
                </ViewCell.ContextActions>
                <Label Text="{Binding .}" />
            </ViewCell>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

This example will result in a <xref:Microsoft.Maui.Controls.MenuItem> object that has text. However, the appearance of a <xref:Microsoft.Maui.Controls.MenuItem> varies across platforms.

A <xref:Microsoft.Maui.Controls.MenuItem> can also be created in code:

```csharp
// Return a ViewCell instance that is used as the template for each list item
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

    // Add a MenuItem to the ContextActions
    MenuItem menuItem = new MenuItem
    {
        Text = "Context Menu Option"
    };
    viewCell.ContextActions.Add(menuItem);

    // Return the custom ViewCell to the DataTemplate constructor
    return viewCell;
});

ListView listView = new ListView
{
    ...
    ItemTemplate = dataTemplate
};
```

A context menu in a <xref:Microsoft.Maui.Controls.ListView> is activated and displayed differently on each platform. On Android, the context menu is activated by long-press on a list item. The context menu replaces the title and navigation bar area and <xref:Microsoft.Maui.Controls.MenuItem> options are displayed as horizontal buttons. On iOS, the context menu is activated by swiping on a list item. The context menu is displayed on the list item and `MenuItems` are displayed as horizontal buttons. On Windows, the context menu is activated by right-clicking on a list item. The context menu is displayed near the cursor as a vertical list.

<!-- No MenuItems in this scenario on Mac Catalyst -->

## Define MenuItem behavior

The <xref:Microsoft.Maui.Controls.MenuItem> class defines a <xref:Microsoft.Maui.Controls.MenuItem.Clicked> event. An event handler can be attached to this event to react to taps or clicks on <xref:Microsoft.Maui.Controls.MenuItem> objects:

```xaml
<MenuItem ...
          Clicked="OnItemClicked" />
```

An event handler can also be attached in code:

```csharp
MenuItem item = new MenuItem { ... };
item.Clicked += OnItemClicked;
```

These examples reference an `OnItemClicked` event handler, which is shown in the following example:

```csharp
void OnItemClicked(object sender, EventArgs e)
{
    MenuItem menuItem = sender as MenuItem;

    // Access the list item through the BindingContext
    var contextItem = menuItem.BindingContext;

    // Do something with the contextItem here
}
```

## Define MenuItem appearance

Icons are specified using the <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource> property. If an icon is specified, the text specified by the <xref:Microsoft.Maui.Controls.MenuItem.Text> property won't be displayed. The following screenshot shows a <xref:Microsoft.Maui.Controls.MenuItem> with an icon on Android:

:::image type="content" source="media/menuitem/icon-context-menu-android.png" alt-text="Screenshot of menu items, with an icon, in a ListView context menu on Android.":::

<xref:Microsoft.Maui.Controls.MenuItem> objects only display icons on Android. On other platforms, only the text specified by the <xref:Microsoft.Maui.Controls.MenuItem.Text> property will be displayed.

> [!NOTE]
> Images can be stored in a single location in your app project. For more information, see [Add images to a .NET MAUI project](~/user-interface/images/images.md).

## Enable or disable a MenuItem at runtime

To enable or disable a <xref:Microsoft.Maui.Controls.MenuItem> at runtime, bind its `Command` property to an <xref:System.Windows.Input.ICommand> implementation, and ensure that a `canExecute` delegate enables and disables the <xref:System.Windows.Input.ICommand> as appropriate.

> [!IMPORTANT]
> Don't bind the `IsEnabled` property to another property when using the `Command` property to enable or disable the <xref:Microsoft.Maui.Controls.MenuItem>.
