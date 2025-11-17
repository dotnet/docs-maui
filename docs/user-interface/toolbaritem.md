---
title: "Display toolbar items"
description: "Learn how to add toolbar items, which are a special type of button, to the app's navigation bar."
ms.date: 08/18/2025
---

# Display toolbar items

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.ToolbarItem> class is a special type of button that can be added to a <xref:Microsoft.Maui.Controls.Page> object's <xref:Microsoft.Maui.Controls.Page.ToolbarItems> collection. Because the <xref:Microsoft.Maui.Controls.Shell> class derives from <xref:Microsoft.Maui.Controls.Page>, <xref:Microsoft.Maui.Controls.ToolbarItem> objects can also be added to the `ToolbarItems` collection of a <xref:Microsoft.Maui.Controls.Shell> object. Each <xref:Microsoft.Maui.Controls.ToolbarItem> object will appear as a button in the app's navigation bar. A <xref:Microsoft.Maui.Controls.ToolbarItem> object can have an icon and appear as a primary or secondary item. The <xref:Microsoft.Maui.Controls.ToolbarItem> class inherits from <xref:Microsoft.Maui.Controls.MenuItem>.

The following screenshot shows a <xref:Microsoft.Maui.Controls.ToolbarItem> object in the navigation bar on iOS:

:::image type="content" source="media/toolbaritem/ios.png" alt-text="Screenshot of a toolbar item in the navigation bar on iOS.":::

The <xref:Microsoft.Maui.Controls.ToolbarItem> class defines the following properties:

- <xref:Microsoft.Maui.Controls.ToolbarItem.Order>, of type <xref:Microsoft.Maui.Controls.ToolbarItemOrder>, determines whether the <xref:Microsoft.Maui.Controls.ToolbarItem>  object displays in the primary or secondary menu.
- <xref:Microsoft.Maui.Controls.ToolbarItem.Priority>, of type `int`, determines the display order of items in a <xref:Microsoft.Maui.Controls.Page.ToolbarItems> collection.

The <xref:Microsoft.Maui.Controls.ToolbarItem> class inherits the following typically used properties from the <xref:Microsoft.Maui.Controls.MenuItem> class:

- <xref:Microsoft.Maui.Controls.MenuItem.Command>, of type <xref:System.Windows.Input.ICommand>, allows binding user actions, such as finger taps or clicks, to commands defined on a viewmodel.
- <xref:Microsoft.Maui.Controls.MenuItem.CommandParameter>, of type `object`, specifies the parameter that should be passed to the `Command`.
- <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource>, of type <xref:Microsoft.Maui.Controls.ImageSource>, that determines the display icon on a <xref:Microsoft.Maui.Controls.ToolbarItem>  object.
- <xref:Microsoft.Maui.Controls.MenuItem.Text>, of type `string`, determines the display text on a <xref:Microsoft.Maui.Controls.ToolbarItem>  object.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings.

> [!NOTE]
> An alternative to creating a toolbar from <xref:Microsoft.Maui.Controls.ToolbarItem> objects is to set the <xref:Microsoft.Maui.Controls.NavigationPage.TitleViewProperty> attached property to a layout class that contains multiple views. For more information, see [Display views in the navigation bar](~/user-interface/pages/navigationpage.md#display-views-in-the-navigation-bar).

## Create a ToolbarItem

To create a toolbar item, create a <xref:Microsoft.Maui.Controls.ToolbarItem> object and set its properties to define its appearance and behavior. The following example shows how to create a <xref:Microsoft.Maui.Controls.ToolbarItem> with minimal properties set, and add it to a <xref:Microsoft.Maui.Controls.ContentPage>'s <xref:Microsoft.Maui.Controls.Page.ToolbarItems> collection:

```xaml
<ContentPage.ToolbarItems>
    <ToolbarItem Text="Add item"
                 IconImageSource="add.png" />
</ContentPage.ToolbarItems>
```

This example results in a <xref:Microsoft.Maui.Controls.ToolbarItem> object that has text and an icon. However, the appearance of a <xref:Microsoft.Maui.Controls.ToolbarItem> varies across platforms.

A <xref:Microsoft.Maui.Controls.ToolbarItem> can also be created in code and added to the <xref:Microsoft.Maui.Controls.Page.ToolbarItems> collection:

```csharp
ToolbarItem item = new ToolbarItem
{
    Text = "Add item",
    IconImageSource = ImageSource.FromFile("add.png")
};

// "this" refers to a Page object
this.ToolbarItems.Add(item);
```

> [!NOTE]
> Images can be stored in a single location in your app project. For more information, see [Add images to a .NET MAUI project](~/user-interface/images/images.md).

## Define button behavior

The <xref:Microsoft.Maui.Controls.ToolbarItem> class inherits the <xref:Microsoft.Maui.Controls.MenuItem.Clicked> event from the <xref:Microsoft.Maui.Controls.MenuItem> class. An event handler can be attached to the <xref:Microsoft.Maui.Controls.MenuItem.Clicked> event to react to taps or clicks on <xref:Microsoft.Maui.Controls.ToolbarItem> objects:

```xaml
<ToolbarItem ...
             Clicked="OnItemClicked" />
```

An event handler can also be attached in code:

```csharp
ToolbarItem item = new ToolbarItem { ... };
item.Clicked += OnItemClicked;
```

These examples reference an `OnItemClicked` event handler, which is shown in the following example:

```csharp
void OnItemClicked(object sender, EventArgs e)
{
    ToolbarItem item = (ToolbarItem)sender;
    messageLabel.Text = $"You clicked the \"{item.Text}\" toolbar item.";
}
```

> [!NOTE]
> <xref:Microsoft.Maui.Controls.ToolbarItem> objects can also use the <xref:Microsoft.Maui.Controls.MenuItem.Command> and <xref:Microsoft.Maui.Controls.MenuItem.CommandParameter> properties to react to user input without event handlers.

## Enable or disable a ToolbarItem at runtime

To enable or disable a <xref:Microsoft.Maui.Controls.ToolbarItem> at runtime, bind its <xref:Microsoft.Maui.Controls.MenuItem.Command> property to an <xref:System.Windows.Input.ICommand> implementation, and ensure that its `canExecute` delegate enables and disables the <xref:System.Windows.Input.ICommand> as appropriate.

> [!IMPORTANT]
> Don't bind the `IsEnabled` property to another property when using the `Command` property to enable or disable the <xref:Microsoft.Maui.Controls.ToolbarItem>.

## Primary and secondary toolbar items

The <xref:Microsoft.Maui.Controls.ToolbarItemOrder> enum has `Default`, `Primary`, and `Secondary` values.

When the <xref:Microsoft.Maui.Controls.ToolbarItem.Order> property is set to `Primary`, the <xref:Microsoft.Maui.Controls.ToolbarItem> object appears in the navigation bar on all platforms. <xref:Microsoft.Maui.Controls.ToolbarItem> objects are prioritized over the page title, which will be truncated to make room for the items.

::: moniker range="<=net-maui-9.0"

When the <xref:Microsoft.Maui.Controls.ToolbarItem.Order> property is set to `Secondary`, behavior varies across platforms. On iOS and Mac Catalyst, `Secondary` toolbar items appear as a horizontal list. On Android and Windows, the `Secondary` items menu appears as three dots that can be tapped:

:::image type="content" source="media/toolbaritem/android-dots.png" alt-text="Screenshot of secondary toolbar ellipsis on Android.":::

Tapping the three dots reveals items in a vertical list:

:::image type="content" source="media/toolbaritem/android-secondaries.png" alt-text="Screenshot of secondary toolbar items in vertical list on Android.":::

::: moniker-end

::: moniker range=">=net-maui-10.0"

When the <xref:Microsoft.Maui.Controls.ToolbarItem.Order> property is set to `Secondary`, behavior varies across platforms. On iOS and Mac Catalyst, `Secondary` toolbar items are grouped into a pull‑down menu, shown under a system ellipsis icon in the navigation bar. Items within this menu are ordered by their <xref:Microsoft.Maui.Controls.ToolbarItem.Priority> value. On Android and Windows, the `Secondary` items menu appears as three dots that can be tapped:

:::image type="content" source="media/toolbaritem/android-dots.png" alt-text="Screenshot of secondary toolbar ellipsis on Android.":::

Tapping the three dots reveals items in a vertical list:

:::image type="content" source="media/toolbaritem/android-secondaries.png" alt-text="Screenshot of secondary toolbar items in vertical list on Android.":::

::: moniker-end

> [!WARNING]
> Icon behavior in <xref:Microsoft.Maui.Controls.ToolbarItem> objects that have their <xref:Microsoft.Maui.Controls.ToolbarItem.Order> property set to `Secondary` can be inconsistent across platforms. Avoid setting the <xref:Microsoft.Maui.Controls.MenuItem.IconImageSource> property on items that appear in the secondary menu.

::: moniker range=">=net-maui-10.0"

### Example: order secondary items by priority (iOS and Mac Catalyst)

On iOS and Mac Catalyst, secondary items are shown in a pull‑down menu ordered by their `Priority` (lower values appear first):

```xaml
<ContentPage.ToolbarItems>
    <ToolbarItem Text="Settings" Order="Secondary" Priority="0" />
    <ToolbarItem Text="Feedback" Order="Secondary" Priority="1" />
    <ToolbarItem Text="About" Order="Secondary" Priority="2" />
    <ToolbarItem Text="Help" Order="Secondary" Priority="3" />
    <ToolbarItem Text="Sign out" Order="Secondary" Priority="100" />
</ContentPage.ToolbarItems>
```

> [!TIP]
> Keep labels short so they fit comfortably in the pull‑down. Avoid icons for `Secondary` items due to platform inconsistency.

::: moniker-end
