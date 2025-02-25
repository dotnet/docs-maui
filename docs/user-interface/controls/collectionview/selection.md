---
title: "Configure CollectionView item selection"
description: "By default, CollectionView selection is disabled. However, single and multiple selection can be enabled."
ms.date: 09/30/2024
---

# Configure CollectionView item selection

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-collectionview)

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.CollectionView> defines the following properties that control item selection:

- `SelectionMode`, of type `SelectionMode`, the selection mode.
- `SelectedItem`, of type `object`, the selected item in the list. This property has a default binding mode of `TwoWay`, and has a `null` value when no item is selected.
- `SelectedItems`, of type `IList<object>`, the selected items in the list. This property has a default binding mode of `OneWay`, and has a `null` value when no items are selected.
- `SelectionChangedCommand`, of type <xref:System.Windows.Input.ICommand>, which is executed when the selected item changes.
- `SelectionChangedCommandParameter`, of type `object`, which is the parameter that's passed to the `SelectionChangedCommand`.

All of these properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that the properties can be targets of data bindings.

By default, <xref:Microsoft.Maui.Controls.CollectionView> selection is disabled. However, this behavior can be changed by setting the `SelectionMode` property value to one of the `SelectionMode` enumeration members:

- `None` – indicates that items can't be selected. This is the default value.
- `Single` – indicates that a single item can be selected, with the selected item being highlighted.
- `Multiple` – indicates that multiple items can be selected, with the selected items being highlighted.

<xref:Microsoft.Maui.Controls.CollectionView> defines a `SelectionChanged` event that is fired when the `SelectedItem` property changes, either due to the user selecting an item from the list, or when an application sets the property. In addition, this event is also fired when the `SelectedItems` property changes. The `SelectionChangedEventArgs` object that accompanies the `SelectionChanged` event has two properties, both of type `IReadOnlyList<object>`:

- `PreviousSelection` – the list of items that were selected, before the selection changed.
- `CurrentSelection` – the list of items that are selected, after the selection change.

In addition, <xref:Microsoft.Maui.Controls.CollectionView> has a `UpdateSelectedItems` method that updates the `SelectedItems` property with a list of selected items, while only firing a single change notification.

## Single selection

When the `SelectionMode` property is set to `Single`, a single item in the <xref:Microsoft.Maui.Controls.CollectionView> can be selected. When an item is selected, the `SelectedItem` property is set to the value of the selected item. When this property changes, the `SelectionChangedCommand` is executed (with the value of the `SelectionChangedCommandParameter` being passed to the <xref:System.Windows.Input.ICommand>), and the `SelectionChanged` event fires.

The following XAML example shows a <xref:Microsoft.Maui.Controls.CollectionView> that can respond to single item selection:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Single"
                SelectionChanged="OnCollectionViewSelectionChanged">
    ...
</CollectionView>
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    SelectionMode = SelectionMode.Single
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty,  static (MonkeysViewModel vm) => vm.Monkeys);
collectionView.SelectionChanged += OnCollectionViewSelectionChanged;
```

In this code example, the `OnCollectionViewSelectionChanged` event handler is executed when the `SelectionChanged` event fires, with the event handler retrieving the previously selected item, and the current selected item:

```csharp
void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    string previous = (e.PreviousSelection.FirstOrDefault() as Monkey)?.Name;
    string current = (e.CurrentSelection.FirstOrDefault() as Monkey)?.Name;
    ...
}
```

> [!IMPORTANT]
> The `SelectionChanged` event can be fired by changes that occur as a result of changing the `SelectionMode` property.

The following screenshot shows single item selection in a <xref:Microsoft.Maui.Controls.CollectionView>:

:::image type="content" source="media/selection/single-selection.png" alt-text="Screenshot of a CollectionView vertical list with single selection.":::

## Multiple selection

When the `SelectionMode` property is set to `Multiple`, multiple items in the <xref:Microsoft.Maui.Controls.CollectionView> can be selected. When items are selected, the `SelectedItems` property is set to the selected items. When this property changes, the `SelectionChangedCommand` is executed (with the value of the `SelectionChangedCommandParameter` being passed to the <xref:System.Windows.Input.ICommand>, and the `SelectionChanged` event fires.

The following XAML example shows a <xref:Microsoft.Maui.Controls.CollectionView> that can respond to multiple item selection:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Multiple"
                SelectionChanged="OnCollectionViewSelectionChanged">
    ...
</CollectionView>
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    SelectionMode = SelectionMode.Multiple
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty,  static (MonkeysViewModel vm) => vm.Monkeys);
collectionView.SelectionChanged += OnCollectionViewSelectionChanged;
```

In this code example, the `OnCollectionViewSelectionChanged` event handler is executed when the `SelectionChanged` event fires, with the event handler retrieving the previously selected items, and the current selected items:

```csharp
void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var previous = e.PreviousSelection;
    var current = e.CurrentSelection;
    ...
}
```

> [!IMPORTANT]
> The `SelectionChanged` event can be fired by changes that occur as a result of changing the `SelectionMode` property.

The following screenshot shows multiple item selection in a <xref:Microsoft.Maui.Controls.CollectionView>:

:::image type="content" source="media/selection/multiple-selection.png" alt-text="Screenshot of a CollectionView vertical list with multiple selection.":::

## Single preselection

When the `SelectionMode` property is set to `Single`, a single item in the <xref:Microsoft.Maui.Controls.CollectionView> can be preselected by setting the `SelectedItem` property to the item. The following XAML example shows a <xref:Microsoft.Maui.Controls.CollectionView> that preselects a single item:

```xaml
<CollectionView ItemsSource="{Binding Monkeys}"
                SelectionMode="Single"
                SelectedItem="{Binding SelectedMonkey}">
    ...
</CollectionView>
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    SelectionMode = SelectionMode.Single
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty,  static (MonkeysViewModel vm) => vm.Monkeys);
collectionView.SetBinding(SelectableItemsView.SelectedItemProperty,  static (MonkeysViewModel vm) => vm.SelectedMonkey);
```

> [!NOTE]
> The `SelectedItem` property has a default binding mode of `TwoWay`.

The `SelectedItem` property data binds to the `SelectedMonkey` property of the connected view model, which is of type `Monkey`. By default, a `TwoWay` binding is used so that if the user changes the selected item, the value of the `SelectedMonkey` property is set to the selected `Monkey` object. The `SelectedMonkey` property is defined in the `MonkeysViewModel` class, and is set to the fourth item of the `Monkeys` collection:

```csharp
public class MonkeysViewModel : INotifyPropertyChanged
{
    ...
    public ObservableCollection<Monkey> Monkeys { get; private set; }

    Monkey selectedMonkey;
    public Monkey SelectedMonkey
    {
        get
        {
            return selectedMonkey;
        }
        set
        {
            if (selectedMonkey != value)
            {
                selectedMonkey = value;
            }
        }
    }

    public MonkeysViewModel()
    {
        ...
        selectedMonkey = Monkeys.Skip(3).FirstOrDefault();
    }
    ...
}
```

Therefore, when the <xref:Microsoft.Maui.Controls.CollectionView> appears, the fourth item in the list is preselected:

:::image type="content" source="media/selection/single-pre-selection.png" alt-text="Screenshot of a CollectionView vertical list with single preselection.":::

## Multiple preselection

When the `SelectionMode` property is set to `Multiple`, multiple items in the <xref:Microsoft.Maui.Controls.CollectionView> can be preselected. The following XAML example shows a <xref:Microsoft.Maui.Controls.CollectionView> that enables the preselection of multiple items:

```xaml
<CollectionView x:Name="collectionView"
                ItemsSource="{Binding Monkeys}"
                SelectionMode="Multiple"
                SelectedItems="{Binding SelectedMonkeys}">
    ...
</CollectionView>
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    SelectionMode = SelectionMode.Multiple
};
collectionView.SetBinding(ItemsView.ItemsSourceProperty,  static (MonkeysViewModel vm) => vm.Monkeys);
collectionView.SetBinding(SelectableItemsView.SelectedItemsProperty,  static (MonkeysViewModel vm) => vm.SelectedMonkeys);
```

> [!NOTE]
> The `SelectedItems` property has a default binding mode of `OneWay`.

The `SelectedItems` property data binds to the `SelectedMonkeys` property of the connected view model, which is of type `ObservableCollection<object>`. The `SelectedMonkeys` property is defined in the `MonkeysViewModel` class, and is set to the second, fourth, and fifth items in the `Monkeys` collection:

```csharp
namespace CollectionViewDemos.ViewModels
{
    public class MonkeysViewModel : INotifyPropertyChanged
    {
        ...
        ObservableCollection<object> selectedMonkeys;
        public ObservableCollection<object> SelectedMonkeys
        {
            get
            {
                return selectedMonkeys;
            }
            set
            {
                if (selectedMonkeys != value)
                {
                    selectedMonkeys = value;
                }
            }
        }

        public MonkeysViewModel()
        {
            ...
            SelectedMonkeys = new ObservableCollection<object>()
            {
                Monkeys[1], Monkeys[3], Monkeys[4]
            };
        }
        ...
    }
}
```

Therefore, when the <xref:Microsoft.Maui.Controls.CollectionView> appears, the second, fourth, and fifth items in the list are preselected:

:::image type="content" source="media/selection/multiple-pre-selection.png" alt-text="Screenshot of a CollectionView vertical list with multiple preselection.":::

## Clear selections

The `SelectedItem` and `SelectedItems` properties can be cleared by setting them, or the objects they bind to, to `null`. When either of these properties are cleared, the `SelectionChanged` event is raised with an empty `CurrentSelection` property, and the `SelectionChangedCommand` is executed.

## Handle reselection

A common scenario is that users will select an item in the `CollectionView` and then navigate to another page. When they navigate back the item is still selected, which will result in them being unable to reselect the given item. To enable reselection you should  clear the item selection on the `CollectionView`:

```xaml
<CollectionView ...
                SelectionChanged="OnCollectionViewSelectionChanged" />
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView();
collectionView.SelectionChanged += OnCollectionViewSelectionChanged;
```

The following example shows the event handler code for the `SelectionChanged` event:

```csharp
void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var cv = (CollectionView)sender;
    if (cv.SelectedItem == null)
        return;

    cv.SelectedItem = null;
}
```

## Change selected item color

<xref:Microsoft.Maui.Controls.CollectionView> has a `Selected` <xref:Microsoft.Maui.Controls.VisualState> that can be used to initiate a visual change to the selected item in the <xref:Microsoft.Maui.Controls.CollectionView>. A common use case for this <xref:Microsoft.Maui.Controls.VisualState> is to change the background color of the selected item, which is shown in the following XAML example:

```xaml
<ContentPage ...>
    <ContentPage.Resources>
        <Style TargetType="Grid">
            <Setter Property="VisualStateManager.VisualStateGroups">
                <VisualStateGroupList>
                    <VisualStateGroup x:Name="CommonStates">
                        <VisualState x:Name="Normal" />
                        <VisualState x:Name="Selected">
                            <VisualState.Setters>
                                <Setter Property="BackgroundColor"
                                        Value="LightSkyBlue" />
                            </VisualState.Setters>
                        </VisualState>
                    </VisualStateGroup>
                </VisualStateGroupList>
            </Setter>
        </Style>
    </ContentPage.Resources>
    <Grid Margin="20">
        <CollectionView ItemsSource="{Binding Monkeys}"
                        SelectionMode="Single">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Grid Padding="10">
                        ...
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
</ContentPage>
```

> [!IMPORTANT]
> The <xref:Microsoft.Maui.Controls.Style> that contains the `Selected` <xref:Microsoft.Maui.Controls.VisualState> must have a `TargetType` property value that's the type of the root element of the <xref:Microsoft.Maui.Controls.DataTemplate>, which is set as the `ItemTemplate` property value.

The equivalent C# code for the style containing the visual state is:

```csharp
using static Microsoft.Maui.Controls.VisualStateManager;
...

Setter backgroundColorSetter = new() { Property = BackgroundColorProperty, Value = Colors.LightSkyBlue };
VisualState stateSelected = new() { Name = CommonStates.Selected, Setters = { backgroundColorSetter } };
VisualState stateNormal = new() { Name = CommonStates.Normal };
VisualStateGroup visualStateGroup = new() { Name = nameof(CommonStates), States = { stateSelected, stateNormal } };
VisualStateGroupList visualStateGroupList = new() { visualStateGroup };
Setter vsgSetter = new() { Property = VisualStateGroupsProperty, Value = visualStateGroupList };
Style style = new(typeof(Grid)) { Setters = { vsgSetter } };

// Add the style to the resource dictionary
Resources.Add(style);
```

In this example, the `Style.TargetType` property value is set to <xref:Microsoft.Maui.Controls.Grid> because the root element of the `ItemTemplate` is a <xref:Microsoft.Maui.Controls.Grid>. The `Selected` <xref:Microsoft.Maui.Controls.VisualState> specifies that when an item in the <xref:Microsoft.Maui.Controls.CollectionView> is selected, the `BackgroundColor` of the item is set to `LightSkyBlue`:

:::image type="content" source="media/selection/single-selection-color.png" alt-text="Screenshot of a CollectionView vertical list with a custom single selection color.":::

For more information about visual states, see [Visual states](~/user-interface/visual-states.md).

## Disable selection

<xref:Microsoft.Maui.Controls.CollectionView> selection is disabled by default. However, if a <xref:Microsoft.Maui.Controls.CollectionView> has selection enabled, it can be disabled by setting the `SelectionMode` property to `None`:

```xaml
<CollectionView ...
                SelectionMode="None" />
```

The equivalent C# code is:

```csharp
CollectionView collectionView = new CollectionView
{
    ...
    SelectionMode = SelectionMode.None
};
```

When the `SelectionMode` property is set to `None`, items in the <xref:Microsoft.Maui.Controls.CollectionView> can't be selected, the `SelectedItem` property remains `null`, and the `SelectionChanged` event isn't fired.

> [!NOTE]
> When an item has been selected and the `SelectionMode` property is changed from `Single` to `None`, the `SelectedItem` property will be set to `null` and the `SelectionChanged` event will be fired with an empty `CurrentSelection` property.
