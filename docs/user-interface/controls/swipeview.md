---
title: "SwipeView"
description: "The .NET MAUI SwipeView is a container control that wraps around an item of content, and provides context menu items that are revealed by a swipe gesture."
ms.date: 02/15/2022
---

# SwipeView

The .NET Multi-platform App UI (.NET MAUI) `SwipeView` is a container control that wraps around an item of content, and provides context menu items that are revealed by a swipe gesture:

:::image type="content" source="media/swipeview/swipeview-collectionview.png" alt-text="Screenshot of SwipeView swipe items in a CollectionView.":::

`SwipeView` defines the following properties:

- `LeftItems`, of type `SwipeItems`, which represents the swipe items that can be invoked when the control is swiped from the left side.
- `RightItems`, of type `SwipeItems`, which represents the swipe items that can be invoked when the control is swiped from the right side.
- `TopItems`, of type `SwipeItems`, which represents the swipe items that can be invoked when the control is swiped from the top down.
- `BottomItems`, of type `SwipeItems`, which represents the swipe items that can be invoked when the control is swiped from the bottom up.
- `Threshold`, of type `double`, which represents the number of device-independent units that trigger a swipe gesture to fully reveal swipe items.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

In addition, the `SwipeView` inherits the `Content` property from the `ContentView` class. The `Content` property is the content property of the `SwipeView` class, and therefore does not need to be explicitly set.

The `SwipeView` class also defines three events:

- `SwipeStarted` is raised when a swipe starts. The `SwipeStartedEventArgs` object that accompanies this event has a `SwipeDirection` property, of type `SwipeDirection`.
- `SwipeChanging` is raised as the swipe moves. The `SwipeChangingEventArgs` object that accompanies this event has a `SwipeDirection` property, of type `SwipeDirection`, and an `Offset` property of type `double`.
- `SwipeEnded` is raised when a swipe ends. The `SwipeEndedEventArgs` object that accompanies this event has a `SwipeDirection` property, of type `SwipeDirection`, and an `IsOpen` property of type `bool`.

In addition, `SwipeView` includes `Open` and `Close` methods, which programmatically open and close the swipe items, respectively.

<!-- > [!NOTE]
> `SwipeView` has a platform-specific on iOS and Android, that controls the transition that's used when opening a `SwipeView`. For more information, see [SwipeView Swipe Transition Mode on iOS](~//platform-integration/platform-specifics/ios/swipeview-swipetransitionmode.md) and [SwipeView Swipe Transition Mode on Android](~/platform-integration/platform-specifics/android/swipeview-swipetransitionmode.md). -->

## Create a SwipeView

A `SwipeView` must define the content that the `SwipeView` wraps around, and the swipe items that are revealed by the swipe gesture. The swipe items are one or more `SwipeItem` objects that are placed in one of the four `SwipeView` directional collections - `LeftItems`, `RightItems`, `TopItems`, or `BottomItems`.

The following example shows how to instantiate a `SwipeView` in XAML:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems>
            <SwipeItem Text="Favorite"
                       IconImageSource="favorite.png"
                       BackgroundColor="LightGreen"
                       Invoked="OnFavoriteSwipeItemInvoked" />
            <SwipeItem Text="Delete"
                       IconImageSource="delete.png"
                       BackgroundColor="LightPink"
                       Invoked="OnDeleteSwipeItemInvoked" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
    <Grid HeightRequest="60"
          WidthRequest="300"
          BackgroundColor="LightGray">
        <Label Text="Swipe right"
               HorizontalOptions="Center"
               VerticalOptions="Center" />
    </Grid>
</SwipeView>
```

The equivalent C# code is:

```csharp
// SwipeItems
SwipeItem favoriteSwipeItem = new SwipeItem
{
    Text = "Favorite",
    IconImageSource = "favorite.png",
    BackgroundColor = Colors.LightGreen
};
favoriteSwipeItem.Invoked += OnFavoriteSwipeItemInvoked;

SwipeItem deleteSwipeItem = new SwipeItem
{
    Text = "Delete",
    IconImageSource = "delete.png",
    BackgroundColor = Colors.LightPink
};
deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

List<SwipeItem> swipeItems = new List<SwipeItem>() { favoriteSwipeItem, deleteSwipeItem };

// SwipeView content
Grid grid = new Grid
{
    HeightRequest = 60,
    WidthRequest = 300,
    BackgroundColor = Colors.LightGray
};
grid.Add(new Label
{
    Text = "Swipe right",
    HorizontalOptions = LayoutOptions.Center,
    VerticalOptions = LayoutOptions.Center
});

SwipeView swipeView = new SwipeView
{
    LeftItems = new SwipeItems(swipeItems),
    Content = grid
};
```

In this example, the `SwipeView` content is a `Grid` that contains a `Label`:

:::image type="content" source="media/swipeview/swipeview-content.png" alt-text="Screenshot of SwipeView content.":::

The swipe items are used to perform actions on the `SwipeView` content, and are revealed when the control is swiped from the left side:

:::image type="content" source="media/swipeview/swipeview-swipeitems.png" alt-text="Screenshot of SwipeView swipe items.":::

By default, a swipe item is executed when it is tapped by the user. However, this behavior can be changed. For more information, see [Swipe mode](#swipe-mode).

Once a swipe item has been executed the swipe items are hidden and the `SwipeView` content is re-displayed. However, this behavior can be changed. For more information, see [Swipe behavior](#swipe-behavior).

> [!NOTE]
> Swipe content and swipe items can be placed inline, or defined as resources.

## Swipe items

The `LeftItems`, `RightItems`, `TopItems`, and `BottomItems` collections are all of type `SwipeItems`. The `SwipeItems` class defines the following properties:

- `Mode`, of type `SwipeMode`, which indicates the effect of a swipe interaction. For more information about swipe mode, see [Swipe mode](#swipe-mode).
- `SwipeBehaviorOnInvoked`, of type `SwipeBehaviorOnInvoked`, which indicates how a `SwipeView` behaves after a swipe item is invoked. For more information about swipe behavior, see [Swipe behavior](#swipe-behavior).

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

Each swipe item is defined as a `SwipeItem` object that's placed into one of the four `SwipeItems` directional collections. The `SwipeItem` class derives from the `MenuItem` class, and adds the following members:

- A `BackgroundColor` property, of type `Color`, that defines the background color of the swipe item. This property is backed by a bindable property.
- An `Invoked` event, which is raised when the swipe item is executed.

> [!IMPORTANT]
> The `MenuItem` class defines several properties, including `Command`, `CommandParameter`, `IconImageSource`, and `Text`. These properties can be set on a `SwipeItem` object to define its appearance, and to define an `ICommand` that executes when the swipe item is invoked. <!--For more information, see [MenuItem](~/user-interface/menuitem.md).-->

The following example shows two `SwipeItem` objects in the `LeftItems` collection of a `SwipeView`:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems>
            <SwipeItem Text="Favorite"
                       IconImageSource="favorite.png"
                       BackgroundColor="LightGreen"
                       Invoked="OnFavoriteSwipeItemInvoked" />
            <SwipeItem Text="Delete"
                       IconImageSource="delete.png"
                       BackgroundColor="LightPink"
                       Invoked="OnDeleteSwipeItemInvoked" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
</SwipeView>
```

The appearance of each `SwipeItem` is defined by a combination of the `Text`, `IconImageSource`, and `BackgroundColor` properties:

:::image type="content" source="media/swipeview/swipeview-swipeitems.png" alt-text="Screenshot of SwipeView swipe items.":::

When a `SwipeItem` is tapped, its `Invoked` event fires and is handled by its registered event handler. In addition, the `MenuItem.Clicked` event fires. Alternatively, the `Command` property can be set to an `ICommand` implementation that will be executed when the `SwipeItem` is invoked.

> [!NOTE]
> When the appearance of a `SwipeItem` is defined only using the `Text` or `IconImageSource` properties, the content is always centered.

In addition to defining swipe items as `SwipeItem` objects, it's also possible to define custom swipe item views. For more information, see [Custom swipe items](#custom-swipe-items).

## Swipe direction

`SwipeView` supports four different swipe directions, with the swipe direction being defined by the directional `SwipeItems` collection the `SwipeItem` objects are added to. Each swipe direction can hold its own swipe items. For example, the following example shows a `SwipeView` whose swipe items depend on the swipe direction:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems>
            <SwipeItem Text="Delete"
                       IconImageSource="delete.png"
                       BackgroundColor="LightPink"
                       Command="{Binding DeleteCommand}" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <SwipeView.RightItems>
        <SwipeItems>
            <SwipeItem Text="Favorite"
                       IconImageSource="favorite.png"
                       BackgroundColor="LightGreen"
                       Command="{Binding FavoriteCommand}" />
            <SwipeItem Text="Share"
                       IconImageSource="share.png"
                       BackgroundColor="LightYellow"
                       Command="{Binding ShareCommand}" />
        </SwipeItems>
    </SwipeView.RightItems>
    <!-- Content -->
</SwipeView>
```

In this example, the `SwipeView` content can be swiped right or left. Swiping to the right will show the **Delete** swipe item, while swiping to the left will show the **Favorite** and **Share** swipe items.

> [!WARNING]
> Only one instance of a directional `SwipeItems` collection can be set at a time on a `SwipeView`. Therefore, you cannot have two `LeftItems` definitions on a `SwipeView`.

The `SwipeStarted`, `SwipeChanging`, and `SwipeEnded` events report the swipe direction via the `SwipeDirection` property in the event arguments. This property is of type `SwipeDirection`, which is an enumeration consisting of four members:

- `Right` indicates that a right swipe occurred.
- `Left` indicates that a left swipe occurred.
- `Up` indicates that an upwards swipe occurred.
- `Down` indicates that a downwards swipe occurred.

## Swipe threshold

`SwipeView` includes a `Threshold` property, of type `double`, which represents the number of device-independent units that trigger a swipe gesture to fully reveal swipe items.

The following example shows a `SwipeView` that sets the `Threshold` property:

```xaml
<SwipeView Threshold="200">
    <SwipeView.LeftItems>
        <SwipeItems>
            <SwipeItem Text="Favorite"
                       IconImageSource="favorite.png"
                       BackgroundColor="LightGreen" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
</SwipeView>
```

In this example, the `SwipeView` must be swiped for 200 device-independent units before the `SwipeItem` is fully revealed.

## Swipe mode

The `SwipeItems` class has a `Mode` property, which indicates the effect of a swipe interaction. This property should be set to one of the `SwipeMode` enumeration members:

- `Reveal` indicates that a swipe reveals the swipe items. This is the default value of the `SwipeItems.Mode` property.
- `Execute` indicates that a swipe executes the swipe items.

In reveal mode, the user swipes a `SwipeView` to open a menu consisting of one or more swipe items, and must explicitly tap a swipe item to execute it. After the swipe item has been executed the swipe items are closed and the `SwipeView` content is re-displayed. In execute mode, the user swipes a `SwipeView` to open a menu consisting of one more swipe items, which are then automatically executed. Following execution, the swipe items are closed and the `SwipeView` content is re-displayed.

The following example shows a `SwipeView` configured to use execute mode:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems Mode="Execute">
            <SwipeItem Text="Delete"
                       IconImageSource="delete.png"
                       BackgroundColor="LightPink"
                       Command="{Binding DeleteCommand}" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
</SwipeView>
```

In this example, the `SwipeView` content can be swiped right to reveal the swipe item, which is executed immediately. Following execution, the `SwipeView` content is re-displayed.

## Swipe behavior

The `SwipeItems` class has a `SwipeBehaviorOnInvoked` property, which indicates how a `SwipeView` behaves after a swipe item is invoked. This property should be set to one of the `SwipeBehaviorOnInvoked` enumeration members:

- `Auto` indicates that in reveal mode the `SwipeView` closes after a swipe item is invoked, and in execute mode the `SwipeView` remains open after a swipe item is invoked. This is the default value of the `SwipeItems.SwipeBehaviorOnInvoked` property.
- `Close` indicates that the `SwipeView` closes after a swipe item is invoked.
- `RemainOpen` indicates that the `SwipeView` remains open after a swipe item is invoked.

The following example shows a `SwipeView` configured to remain open after a swipe item is invoked:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems SwipeBehaviorOnInvoked="RemainOpen">
            <SwipeItem Text="Favorite"
                       IconImageSource="favorite.png"
                       BackgroundColor="LightGreen"
                       Invoked="OnFavoriteSwipeItemInvoked" />
            <SwipeItem Text="Delete"
                       IconImageSource="delete.png"
                       BackgroundColor="LightPink"
                       Invoked="OnDeleteSwipeItemInvoked" />
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
</SwipeView>
```

## Custom swipe items

Custom swipe items can be defined with the `SwipeItemView` type. The `SwipeItemView` class derives from the `ContentView` class, and adds the following properties:

- `Command`, of type `ICommand`, which is executed when a swipe item is tapped.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

The `SwipeItemView` class also defines an `Invoked` event that's raised when the item is tapped, after the `Command` is executed.

The following example shows a `SwipeItemView` object in the `LeftItems` collection of a `SwipeView`:

```xaml
<SwipeView>
    <SwipeView.LeftItems>
        <SwipeItems>
            <SwipeItemView Command="{Binding CheckAnswerCommand}"
                           CommandParameter="{Binding Source={x:Reference resultEntry}, Path=Text}">
                <StackLayout Margin="10"
                             WidthRequest="300">
                    <Entry x:Name="resultEntry"
                           Placeholder="Enter answer"
                           HorizontalOptions="CenterAndExpand" />
                    <Label Text="Check"
                           FontAttributes="Bold"
                           HorizontalOptions="Center" />
                </StackLayout>
            </SwipeItemView>
        </SwipeItems>
    </SwipeView.LeftItems>
    <!-- Content -->
</SwipeView>
```

In this example, the `SwipeItemView` comprises a `StackLayout` containing an `Entry` and a `Label`. After the user enters input into the `Entry`, the rest of the `SwipeViewItem` can be tapped which executes the `ICommand` defined by the `SwipeItemView.Command` property.

## Open and close a SwipeView programmatically

`SwipeView` includes `Open` and `Close` methods, which programmatically open and close the swipe items, respectively. By default, these methods will animate the `SwipeView` when its opened or closed.

The `Open` method requires an `OpenSwipeItem` argument, to specify the direction the `SwipeView` will be opened from. The `OpenSwipeItem` enumeration has four members:

- `LeftItems`, which indicates that the `SwipeView` will be opened from the left, to reveal the swipe items in the `LeftItems` collection.
- `TopItems`, which indicates that the `SwipeView` will be opened from the top, to reveal the swipe items in the `TopItems` collection.
- `RightItems`, which indicates that the `SwipeView` will be opened from the right, to reveal the swipe items in the `RightItems` collection.
- `BottomItems`, which indicates that the `SwipeView` will be opened from the bottom, to reveal the swipe items in the `BottomItems` collection.

In addition, the `Open` method also accepts an optional `bool` argument that defines whether the `SwipeView` will be animated when it opens.

Given a `SwipeView` named `swipeView`, the following example shows how to open a `SwipeView` to reveal the swipe items in the `LeftItems` collection:

```csharp
swipeView.Open(OpenSwipeItem.LeftItems);
```

The `swipeView` can then be closed with the `Close` method:

```csharp
swipeView.Close();
```

> [!NOTE]
> The `Close` method also accepts an optional `bool` argument that defines whether the `SwipeView` will be animated when it closes.

## Disable a SwipeView

An app may enter a state where swiping an item of content is not a valid operation. In such cases, the `SwipeView` can be disabled by setting its `IsEnabled` property to `false`. This will prevent users from being able to swipe content to reveal swipe items.

In addition, when defining the `Command` property of a `SwipeItem` or `SwipeItemView`, the `CanExecute` delegate of the `ICommand` can be specified to enable or disable the swipe item.
