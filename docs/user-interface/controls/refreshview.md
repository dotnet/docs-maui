---
title: "RefreshView"
description: "The .NET MAUI RefreshView is a container control that provides pull to refresh functionality for scrollable content."
ms.date: 02/15/2022
no-loc: [RefreshView]
---

# RefreshView

The .NET Multi-platform App UI (.NET MAUI) `RefreshView` is a container control that provides pull to refresh functionality for scrollable content. Therefore, the child of a `RefreshView` must be a scrollable control, such as `ScrollView`, `CollectionView`, or `ListView`.

[!INCLUDE [docs under construction](~/includes/preview-note.md)]

`RefreshView` defines the following properties:

- `Command`, of type `ICommand`, which is executed when a refresh is triggered.
- `CommandParameter`, of type `object`, which is the parameter that's passed to the `Command`.
- `IsRefreshing`, of type `bool`, which indicates the current state of the `RefreshView`.
- `RefreshColor`, of type `Color`, the color of the progress circle that appears during the refresh.

These properties are backed by `BindableProperty` objects, which means that they can be targets of data bindings, and styled.

<!-- > [!NOTE]
> On Windows, the pull direction of a `RefreshView` can be set with a platform-specific. For more information, see [RefreshView Pull Direction](~/platform-integration/platform-specifics/windows/refreshview-pulldirection.md). -->

## Create a RefreshView

To add a `RefreshView` to a page, create a `RefreshView` object and set its `IsRefreshing` and `Command` properties. Then set its child to a scrollable control.

The following example shows how to instantiate a `RefreshView` in XAML:

```xaml
<RefreshView IsRefreshing="{Binding IsRefreshing}"
             Command="{Binding RefreshCommand}">
    <ScrollView>
        <FlexLayout Direction="Row"
                    Wrap="Wrap"
                    AlignItems="Center"
                    AlignContent="Center"
                    BindableLayout.ItemsSource="{Binding Items}"
                    BindableLayout.ItemTemplate="{StaticResource ColorItemTemplate}" />
    </ScrollView>
</RefreshView>
```

A `RefreshView` can also be created in code:

```csharp
RefreshView refreshView = new RefreshView();
ICommand refreshCommand = new Command(() =>
{
    // IsRefreshing is true
    // Refresh data here
    refreshView.IsRefreshing = false;
});
refreshView.Command = refreshCommand;

ScrollView scrollView = new ScrollView();
FlexLayout flexLayout = new FlexLayout { ... };
scrollView.Content = flexLayout;
refreshView.Content = scrollView;
```

In this example, the `RefreshView` provides pull to refresh functionality to a `ScrollView` whose child is a `FlexLayout`. The `FlexLayout` uses a bindable layout to generate its content by binding to a collection of items, and sets the appearance of each item with a `DataTemplate`. <!-- For more information about bindable layouts, see [Bindable layouts](~/user-interface/layouts/bindable-layouts.md). -->

The value of the `RefreshView.IsRefreshing` property indicates the current state of the `RefreshView`. When a refresh is triggered by the user, this property will automatically transition to `true`. Once the refresh completes, you should reset the property to `false`.

When the user initiates a refresh, the `ICommand` defined by the `Command` property is executed, which should refresh the items being displayed. A refresh visualization is shown while the refresh occurs, which consists of an animated progress circle. The following screenshot shows the progress circle on iOS:

:::image type="content" source="media/refreshview/default-progress-circle.png" alt-text="Screenshot of a RefreshView refreshing data.":::

> [!NOTE]
> Manually setting the `IsRefreshing` property to `true` will trigger the refresh visualization, and will execute the `ICommand` defined by the `Command` property.

## RefreshView appearance

In addition to the properties that `RefreshView` inherits from the `VisualElement` class, `RefreshView` also defines the `RefreshColor` property. This property can be set to define the color of the progress circle that appears during the refresh:

```xaml
<RefreshView RefreshColor="Teal"
             ... />
```

The following Android screenshot shows a `RefreshView` with the `RefreshColor` property:

:::image type="content" source="media/refreshview/teal-progress-circle.png" alt-text="Screenshot of a RefreshView with a teal progress circle.":::

In addition, the `BackgroundColor` property can be set to a `Color` that represents the background color of the progress circle.

> [!NOTE]
> On iOS, the `BackgroundColor` property sets the background color of the `UIView` that contains the progress circle.

## Disable a RefreshView

An app may enter a state where pull to refresh is not a valid operation. In such cases, the `RefreshView` can be disabled by setting its `IsEnabled` property to `false`. This will prevent users from being able to trigger pull to refresh.

Alternatively, when defining the `Command` property, the `CanExecute` delegate of the `ICommand` can be specified to enable or disable the command.
