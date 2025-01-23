---
title: "Picker"
description: "The .NET MAUI Picker displays a short list of items, from which the user can select an item."
ms.date: 08/30/2024
---

# Picker

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.Picker> displays a short list of items, from which the user can select an item.

<xref:Microsoft.Maui.Controls.Picker> defines the following properties:

- `CharacterSpacing`, of type `double`, is the spacing between characters of the item displayed by the <xref:Microsoft.Maui.Controls.Picker>.
- `FontAttributes` of type `FontAttributes`, which defaults to `FontAtributes.None`.
- `FontAutoScalingEnabled`, of type `bool`, which determines whether the text respects scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily` of type `string`, which defaults to `null`.
- `FontSize` of type `double`, which defaults to -1.0.
- `HorizontalTextAlignment`, of type <xref:Microsoft.Maui.TextAlignment>, is the horizontal alignment of the text displayed by the <xref:Microsoft.Maui.Controls.Picker>.
- `ItemsSource` of type `IList`, the source list of items to display, which defaults to `null`.
- `SelectedIndex` of type `int`, the index of the selected item, which defaults to -1.
- `SelectedItem` of type `object`, the selected item, which defaults to `null`.
- `ItemDisplayBinding`, of type <xref:Microsoft.Maui.Controls.BindingBase>, selects the property that will be displayed for each object in the list of items, if the `ItemSource` is a complex object. For more information, see [Populate a Picker with data using data binding](#populate-a-picker-with-data-using-data-binding).
- `TextColor` of type <xref:Microsoft.Maui.Graphics.Color>, the color used to display the text.
- `TextTransform`, of type `TextTransform`, which defines whether to transform the casing of text.
- `Title` of type `string`, which defaults to `null`.
- `TitleColor` of type <xref:Microsoft.Maui.Graphics.Color>, the color used to display the `Title` text.
- `VerticalTextAlignment`, of type <xref:Microsoft.Maui.TextAlignment>, is the vertical alignment of the text displayed by the <xref:Microsoft.Maui.Controls.Picker>.

All of the properties, with the exception of `ItemDisplayBinding`, are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be styled, and the properties can be targets of data bindings. The `SelectedIndex` and `SelectedItem` properties have a default binding mode of `BindingMode.TwoWay`, which means that they can be targets of data bindings in an application that uses the Model-View-ViewModel (MVVM) pattern. For information about setting font properties, see [Fonts](~/user-interface/fonts.md).

A <xref:Microsoft.Maui.Controls.Picker> doesn't show any data when it's first displayed. Instead, the value of its `Title` property is shown as a placeholder, as shown in the following iOS screenshot:

:::image type="content" source="media/picker/picker-initial.png" alt-text="Screenshot of initial Picker display.":::

When the <xref:Microsoft.Maui.Controls.Picker> gains focus, its data is displayed and the user can select an item:

:::image type="content" source="media/picker/picker-selection.png" alt-text="Screenshot of selecting an item in a Picker.":::

The <xref:Microsoft.Maui.Controls.Picker> fires a `SelectedIndexChanged` event when the user selects an item. Following selection, the selected item is displayed by the <xref:Microsoft.Maui.Controls.Picker>:

:::image type="content" source="media/picker/picker-after-selection.png" alt-text="Screenshot of Picker after selection.":::

There are two techniques for populating a <xref:Microsoft.Maui.Controls.Picker> with data:

- Setting the `ItemsSource` property to the data to be displayed. This is the recommended technique for adding data to a <xref:Microsoft.Maui.Controls.Picker>. For more information, see [Set the ItemsSource property](#set-the-itemssource-property).
- Adding the data to be displayed to the `Items` collection. For more information, see [Add data to the Items collection](#add-data-to-the-items-collection).

## Set the ItemsSource property

A <xref:Microsoft.Maui.Controls.Picker> can be populated with data by setting its `ItemsSource` property to an `IList` collection. Each item in the collection must be of, or derived from, type `object`. Items can be added in XAML by initializing the `ItemsSource` property from an array of items:

```xaml
<Picker x:Name="picker"
        Title="Select a monkey">
  <Picker.ItemsSource>
    <x:Array Type="{x:Type x:String}">
      <x:String>Baboon</x:String>
      <x:String>Capuchin Monkey</x:String>
      <x:String>Blue Monkey</x:String>
      <x:String>Squirrel Monkey</x:String>
      <x:String>Golden Lion Tamarin</x:String>
      <x:String>Howler Monkey</x:String>
      <x:String>Japanese Macaque</x:String>
    </x:Array>
  </Picker.ItemsSource>
</Picker>
```

> [!NOTE]
> The `x:Array` element requires a `Type` attribute indicating the type of the items in the array.

The equivalent C# code is:

```csharp
var monkeyList = new List<string>();
monkeyList.Add("Baboon");
monkeyList.Add("Capuchin Monkey");
monkeyList.Add("Blue Monkey");
monkeyList.Add("Squirrel Monkey");
monkeyList.Add("Golden Lion Tamarin");
monkeyList.Add("Howler Monkey");
monkeyList.Add("Japanese Macaque");

Picker picker = new Picker { Title = "Select a monkey" };
picker.ItemsSource = monkeyList;
```

### Respond to item selection

A <xref:Microsoft.Maui.Controls.Picker> supports selection of one item at a time. When a user selects an item, the `SelectedIndexChanged` event fires, the `SelectedIndex` property is updated to an integer representing the index of the selected item in the list, and the `SelectedItem` property is updated to the `object` representing the selected item. The `SelectedIndex` property is a zero-based number indicating the item the user selected. If no item is selected, which is the case when the <xref:Microsoft.Maui.Controls.Picker> is first created and initialized, `SelectedIndex` will be -1.

> [!NOTE]
> Item selection behavior in a <xref:Microsoft.Maui.Controls.Picker> can be customized on iOS with a platform-specific. For more information, see [Picker item selection on iOS](~/ios/platform-specifics/picker-selection.md).

The following XAML example shows how to retrieve the `SelectedItem` property value from the <xref:Microsoft.Maui.Controls.Picker>:

```xaml
<Label Text="{Binding Source={x:Reference picker}, Path=SelectedItem}" />
```

The equivalent C# code is:

```csharp
Label monkeyNameLabel = new Label();
monkeyNameLabel.SetBinding(Label.TextProperty, Binding.Create(static (Picker picker) => picker.SelectedItem, source: picker));
```

In addition, an event handler can be executed when the `SelectedIndexChanged` event fires:

```csharp
void OnPickerSelectedIndexChanged(object sender, EventArgs e)
{
  var picker = (Picker)sender;
  int selectedIndex = picker.SelectedIndex;

  if (selectedIndex != -1)
  {
    monkeyNameLabel.Text = (string)picker.ItemsSource[selectedIndex];
  }
}
```

In this example, the event handler obtains the `SelectedIndex` property value, and uses the value to retrieve the selected item from the `ItemsSource` collection. This is functionally equivalent to retrieving the selected item from the `SelectedItem` property. Each item in the `ItemsSource` collection is of type `object`, and so must be cast to a `string` for display.

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.Picker> can be initialized to display a specific item by setting the `SelectedIndex` or `SelectedItem` properties. However, these properties must be set after initializing the `ItemsSource` collection.

### Populate a Picker with data using data binding

A <xref:Microsoft.Maui.Controls.Picker> can be also populated with data by using data binding to bind its `ItemsSource` property to an `IList` collection. In XAML this is achieved with the `Binding` markup extension:

```xaml
<Picker Title="Select a monkey"
        ItemsSource="{Binding Monkeys}"
        ItemDisplayBinding="{Binding Name}" />
```

The equivalent C# code is shown below:

```csharp
Picker picker = new Picker { Title = "Select a monkey" };
picker.SetBinding(Picker.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);
picker.ItemDisplayBinding = Binding.Create(static (Monkey monkey) => monkey.Name);
```

In this example, the `ItemsSource` property data binds to the `Monkeys` property of the binding context, which returns an `IList<Monkey>` collection. The following code example shows the `Monkey` class, which contains four properties:

```csharp
public class Monkey
{
  public string Name { get; set; }
  public string Location { get; set; }
  public string Details { get; set; }
  public string ImageUrl { get; set; }
}
```

When binding to a list of objects, the <xref:Microsoft.Maui.Controls.Picker> must be told which property to display from each object. This is achieved by setting the `ItemDisplayBinding` property to the required property from each object. In the code examples above, the <xref:Microsoft.Maui.Controls.Picker> is set to display each `Monkey.Name` property value.

#### Respond to item selection

Data binding can be used to set an object to the `SelectedItem` property value when it changes:

```xaml
<Picker Title="Select a monkey"
        ItemsSource="{Binding Monkeys}"
        ItemDisplayBinding="{Binding Name}"
        SelectedItem="{Binding SelectedMonkey}" />
<Label Text="{Binding SelectedMonkey.Name}" ... />
<Label Text="{Binding SelectedMonkey.Location}" ... />
<Image Source="{Binding SelectedMonkey.ImageUrl}" ... />
<Label Text="{Binding SelectedMonkey.Details}" ... />
```

The equivalent C# code is:

```csharp
Picker picker = new Picker { Title = "Select a monkey" };
picker.SetBinding(Picker.ItemsSourceProperty, static (MonkeysViewModel vm) => vm.Monkeys);
picker.SetBinding(Picker.SelectedItemProperty, static (MonkeysViewModel vm) => vm.SelectedMonkey);
picker.ItemDisplayBinding = Binding.Create(static (Monkey monkey) => monkey.Name);

Label nameLabel = new Label { ... };
nameLabel.SetBinding(Label.TextProperty, static (MonkeysViewModel vm) => vm.SelectedMonkey.Name);

Label locationLabel = new Label { ... };
locationLabel.SetBinding(Label.TextProperty, static (MonkeysViewModel vm) => vm.SelectedMonkey.Location);

Image image = new Image { ... };
image.SetBinding(Image.SourceProperty, static (MonkeysViewModel vm) => vm.SelectedMonkey.ImageUrl);

Label detailsLabel = new Label();
detailsLabel.SetBinding(Label.TextProperty, static (MonkeysViewModel vm) => vm.SelectedMonkey.Details);
```

The `SelectedItem` property data binds to the `SelectedMonkey` property of the binding context, which is of type `Monkey`. Therefore, when the user selects an item in the <xref:Microsoft.Maui.Controls.Picker>, the `SelectedMonkey` property will be set to the selected `Monkey` object. The `SelectedMonkey` object data is displayed in the user interface by <xref:Microsoft.Maui.Controls.Label> and <xref:Microsoft.Maui.Controls.Image> views.

> [!NOTE]
> The `SelectedItem` and `SelectedIndex` properties both support two-way bindings by default.

## Add data to the Items collection

An alternative process for populating a <xref:Microsoft.Maui.Controls.Picker> with data is to add the data to be displayed to the read-only `Items` collection, which is of type `IList<string>`. Each item in the collection must be of type `string`. Items can be added in XAML by initializing the `Items` property with a list of `x:String` items:

```xaml
<Picker Title="Select a monkey">
  <Picker.Items>
    <x:String>Baboon</x:String>
    <x:String>Capuchin Monkey</x:String>
    <x:String>Blue Monkey</x:String>
    <x:String>Squirrel Monkey</x:String>
    <x:String>Golden Lion Tamarin</x:String>
    <x:String>Howler Monkey</x:String>
    <x:String>Japanese Macaque</x:String>
  </Picker.Items>
</Picker>
```

The equivalent C# code is:

```csharp
Picker picker = new Picker { Title = "Select a monkey" };
picker.Items.Add("Baboon");
picker.Items.Add("Capuchin Monkey");
picker.Items.Add("Blue Monkey");
picker.Items.Add("Squirrel Monkey");
picker.Items.Add("Golden Lion Tamarin");
picker.Items.Add("Howler Monkey");
picker.Items.Add("Japanese Macaque");
```

In addition to adding data using the `Items.Add` method, data can also be inserted into the collection by using the `Items.Insert` method.

### Respond to item selection

A <xref:Microsoft.Maui.Controls.Picker> supports selection of one item at a time. When a user selects an item, the `SelectedIndexChanged` event fires, and the `SelectedIndex` property is updated to an integer representing the index of the selected item in the list. The `SelectedIndex` property is a zero-based number indicating the item that the user selected. If no item is selected, which is the case when the <xref:Microsoft.Maui.Controls.Picker> is first created and initialized, `SelectedIndex` will be -1.

> [!NOTE]
> Item selection behavior in a <xref:Microsoft.Maui.Controls.Picker> can be customized on iOS with a platform-specific. For more information, see [Picker item selection on iOS](~/ios/platform-specifics/picker-selection.md).

The following code example shows the `OnPickerSelectedIndexChanged` event handler method, which is executed when the `SelectedIndexChanged` event fires:

```csharp
void OnPickerSelectedIndexChanged(object sender, EventArgs e)
{
  var picker = (Picker)sender;
  int selectedIndex = picker.SelectedIndex;

  if (selectedIndex != -1)
  {
    monkeyNameLabel.Text = picker.Items[selectedIndex];
  }
}
```

This method obtains the `SelectedIndex` property value, and uses the value to retrieve the selected item from the `Items` collection. Because each item in the `Items` collection is a `string`, they can be displayed by a <xref:Microsoft.Maui.Controls.Label> without requiring a cast.

> [!NOTE]
> A <xref:Microsoft.Maui.Controls.Picker> can be initialized to display a specific item by setting the `SelectedIndex` property. However, the `SelectedIndex` property must be set after initializing the `Items` collection.
