---
title: "SearchBar"
description: "The .NET MAUI SearchBar is a user input control that is used for initiating a search. The SearchBar control supports placeholder text, query input, execution, and cancellation."
ms.date: 10/19/2023
---

# SearchBar

The .NET Multi-platform App UI (.NET MAUI) <xref:Microsoft.Maui.Controls.SearchBar> is a user input control used to initiating a search. The <xref:Microsoft.Maui.Controls.SearchBar> control supports placeholder text, query input, search execution, and cancellation. The following iOS screenshot shows a <xref:Microsoft.Maui.Controls.SearchBar> query with results displayed in a <xref:Microsoft.Maui.Controls.ListView>:

:::image type="content" source="media/searchbar/searchbar.png" alt-text="Screenshot of a SearchBar.":::

<xref:Microsoft.Maui.Controls.SearchBar> defines the following properties:

::: moniker range="=net-maui-7.0"

- `CancelButtonColor` is a <xref:Microsoft.Maui.Graphics.Color> that defines the color of the cancel button.
- `CursorPosition` is an `int` that determines the position at which the next character will be inserted into the string stored in the `Text` property.
- `FontAttributes` is a `FontAttributes` enum value that determines whether the <xref:Microsoft.Maui.Controls.SearchBar> font is bold, italic, or neither.
- `FontAutoScalingEnabled` is a `bool` which defines whether an app's UI reflects text scaling preferences set in the operating system.
- `FontFamily` is a `string` that determines the font family used by the <xref:Microsoft.Maui.Controls.SearchBar>.
- `FontSize` is a `double` value that represents specific font sizes across platforms.
- `HorizontalTextAlignment` is a `TextAlignment` enum value that defines the horizontal alignment of the query text.
- `IsTextPredictionEnabled` is a `bool` that determines whether text prediction and automatic text correction is enabled.
- `SearchCommand` is an <xref:System.Windows.Input.ICommand> that allows binding user actions, such as finger taps or clicks, to commands defined on a viewmodel.
- `SearchCommandParameter` is an `object` that specifies the parameter that should be passed to the `SearchCommand`.
- `SelectionLength` is an `int` that can be used to return or set the length of text selection within the <xref:Microsoft.Maui.Controls.SearchBar>.
- `VerticalTextAlignment` is a `TextAlignment` enum value that defines the vertical alignment of the query text.

::: moniker-end

::: moniker range=">=net-maui-8.0"

- `CancelButtonColor` is a <xref:Microsoft.Maui.Graphics.Color> that defines the color of the cancel button.
- `HorizontalTextAlignment` is a `TextAlignment` enum value that defines the horizontal alignment of the query text.
- `SearchCommand` is an <xref:System.Windows.Input.ICommand> that allows binding user actions, such as finger taps or clicks, to commands defined on a viewmodel.
- `SearchCommandParameter` is an `object` that specifies the parameter that should be passed to the `SearchCommand`.
- `VerticalTextAlignment` is a `TextAlignment` enum value that defines the vertical alignment of the query text.

::: moniker-end

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

In addition, <xref:Microsoft.Maui.Controls.SearchBar> defines a `SearchButtonPressed` event, which is raised when the search button is clicked, or the enter key is pressed.

<xref:Microsoft.Maui.Controls.SearchBar> derives from the `InputView` class, from which it inherits the following properties:

::: moniker range="=net-maui-7.0"

- `CharacterSpacing`, of type `double`, sets the spacing between characters in the entered text.
- `IsReadOnly`, of type `bool`, defines whether the user should be prevented from modifying text. The default value of this property is `false`.
- `IsSpellCheckEnabled`, of type `bool`, controls whether spell checking is enabled.
- `Keyboard`, of type `Keyboard`, specifies the soft input keyboard that's displayed when entering text.
- `MaxLength`, of type `int`, defines the maximum input length.
- `Placeholder`, of type `string`, defines the text that's displayed when the control is empty.
- `PlaceholderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the placeholder text.
- `Text`, of type `string`, defines the text entered into the control.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the entered text.
- `TextTransform`, of type `TextTransform`, specifies the casing of the entered text.

::: moniker-end

::: moniker range=">=net-maui-8.0"

- `CharacterSpacing`, of type `double`, sets the spacing between characters in the entered text.
- `CursorPosition`, of type `int`, defines the position of the cursor within the editor.
- `FontAttributes`, of type `FontAttributes`, determines text style.
- `FontAutoScalingEnabled`, of type `bool`, defines whether the text will reflect scaling preferences set in the operating system. The default value of this property is `true`.
- `FontFamily`, of type `string`, defines the font family.
- `FontSize`, of type `double`, defines the font size.
- `IsReadOnly`, of type `bool`, defines whether the user should be prevented from modifying text. The default value of this property is `false`.
- `IsSpellCheckEnabled`, of type `bool`, controls whether spell checking is enabled.
- `IsTextPredictionEnabled`, of type `bool`, controls whether text prediction and automatic text correction is enabled.
- `Keyboard`, of type `Keyboard`, specifies the soft input keyboard that's displayed when entering text.
- `MaxLength`, of type `int`, defines the maximum input length.
- `Placeholder`, of type `string`, defines the text that's displayed when the control is empty.
- `PlaceholderColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the placeholder text.
- `SelectionLength`, of type `int`, represents the length of selected text within the control.
- `Text`, of type `string`, defines the text entered into the control.
- `TextColor`, of type <xref:Microsoft.Maui.Graphics.Color>, defines the color of the entered text.
- `TextTransform`, of type `TextTransform`, specifies the casing of the entered text.

::: moniker-end

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

In addition, `InputView` defines a `TextChanged` event, which is raised when the text in the <xref:Microsoft.Maui.Controls.Entry> changes. The `TextChangedEventArgs` object that accompanies the `TextChanged` event has `NewTextValue` and `OldTextValue` properties, which specify the new and old text, respectively.

## Create a SearchBar

To create a search bar, create a <xref:Microsoft.Maui.Controls.SearchBar> object and set its `Placeholder` property to text that instructs the user to enter a search term.

The following XAML example shows how to create a <xref:Microsoft.Maui.Controls.SearchBar>:

```xaml
<SearchBar Placeholder="Search items..." />
```

The equivalent C# code is:

```csharp
SearchBar searchBar = new SearchBar { Placeholder = "Search items..." };
```

[!INCLUDE [Keyboard autoscroll manager](includes/keyboardautomanagerscroll.md)]

## Perform a search with event handlers

A search can be executed using the <xref:Microsoft.Maui.Controls.SearchBar> control by attaching an event handler to one of the following events:

- `SearchButtonPressed`, which is called when the user either clicks the search button or presses the enter key.
- `TextChanged`, which is called anytime the text in the query box is changed. This event is inherited from the `InputView` class.

The following XAML example shows an event handler attached to the `TextChanged` event and uses a <xref:Microsoft.Maui.Controls.ListView> to display search results:

```xaml
<SearchBar TextChanged="OnTextChanged" />
<ListView x:Name="searchResults" >
```

In this example, the `TextChanged` event is set to an event handler named `OnTextChanged`. This handler is located in the code-behind file:

```csharp
void OnTextChanged(object sender, EventArgs e)
{
    SearchBar searchBar = (SearchBar)sender;
    searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);
}
```

In this example, a `DataService` class with a `GetSearchResults` method is used to returnitems that match a query. The <xref:Microsoft.Maui.Controls.SearchBar> control's `Text` property value is passed to the `GetSearchResults` method and the result is used to update the <xref:Microsoft.Maui.Controls.ListView> control's `ItemsSource` property. The overall effect is that search results are displayed in the <xref:Microsoft.Maui.Controls.ListView>.

## Perform a search using a viewmodel

A search can be executed without event handlers by binding the `SearchCommand` property to an <xref:System.Windows.Input.ICommand> implementation. For more information about commanding, see [Commanding](~/fundamentals/data-binding/commanding.md).

The following example shows a viewmodel class that contains an <xref:System.Windows.Input.ICommand> property named `PerformSearch`:

```csharp
public class SearchViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        SearchResults = DataService.GetSearchResults(query);
    });

    private List<string> searchResults = DataService.Fruits;
    public List<string> SearchResults
    {
        get
        {
            return searchResults;
        }
        set
        {
            searchResults = value;
            NotifyPropertyChanged();
        }
    }
}
```

> [!NOTE]
> The viewmodel assumes the existence of a `DataService` class capable of performing searches.

The following XAML example consumes the `SearchViewModel` class:

```xaml
<ContentPage ...>
    <ContentPage.BindingContext>
        <viewmodels:SearchViewModel />
    </ContentPage.BindingContext>
    <StackLayout>
        <SearchBar x:Name="searchBar"
                   SearchCommand="{Binding PerformSearch}"
                   SearchCommandParameter="{Binding Text, Source={x:Reference searchBar}}"/>
        <ListView x:Name="searchResults"
                  ItemsSource="{Binding SearchResults}" />
    </StackLayout>
</ContentPage>
```

In this example, the `BindingContext` is set to an instance of the `SearchViewModel` class. The `SearchBar.SearchCommand` property binds to `PerformSearch` viewmodel property, and the `SearchCommandParameter` property binds to the `SearchBar.Text` property. Similarly, the `ListView.ItemsSource` property is bound to the `SearchResults` property of the viewmodel.

<!--
> [!NOTE]
> On iOS, the `SearchBarRenderer` class contains an overridable `UpdateCancelButton` method. This method controls when the cancel button appears, and can be overridden in a custom renderer.
 -->

::: moniker range=">=net-maui-8.0"

[!INCLUDE [Hide and show the soft input keyboard](includes/soft-input-extensions.md)]

The following example shows how to hide the soft input keyboard on a <xref:Microsoft.Maui.Controls.SearchBar> named `searchBar`, if it's currently showing:

```csharp
if (searchBar.IsSoftInputShowing())
   await searchBar.HideSoftInputAsync(System.Threading.CancellationToken.None);
```

::: moniker-end
