---
author: adegeo
ms.author: adegeo
ms.date: 03/20/2024
ms.topic: include
no-loc: ["MainPage.xaml", "MainPage.xaml.cs", "NotePage.xaml", "NotePage.xaml.cs", "AboutPage.xaml", "AboutPage.xaml.cs", "AppShell.xaml", "AppShell.xaml.cs", "Note.cs", "AllNotes"]
---

This portion of the tutorial adds the final piece of the app, a view to display all of the notes previously created.

## Multiple notes and navigation

Currently the **note** view displays a single note. To display multiple notes, create a new view and model: **AllNotes**.

01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Views":::** folder and select **Add** > **New Item...**
01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _AllNotesPage.xaml_, and then select **Add**.
01. In the **Solution Explorer** pane, right-click on the **:::no-loc text="Models":::** folder and select **Add** > **Class...**
01. Name the class _AllNotes.cs_ and press **Add**.

## Code the AllNotes model

The new model will represent the data required to display multiple notes. This data will be a property that represents a collection of notes. The collection will be an `ObservableCollection` which is a specialized collection. When a control lists multiple items, such as a <xref:Microsoft.Maui.Controls.ListView>, is bound to an `ObservableCollection`, the two work together to automatically keep the list of items in sync with the collection. If the list adds an item, the collection is updated. If the collection adds an item, the control is automatically updated with a new item.

01. In the **Solution Explorer** pane, open the _Models\\AllNotes.cs_ file.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/allnotes/Models/AllNotes.cs":::

The previous code declares a collection, named `Notes`, and uses the `LoadNotes` method to load notes from the device. This method uses LINQ extensions to load, transform, and sort the data into the `Notes` collection.

## Design the AllNotes page

Next, the view needs to be designed to support the **AllNotes** model.

01. In the **Solution Explorer** pane, open the _Views\\AllNotesPage.xaml_ file.
01. Replace the code with the following markup:

    :::code language="xaml" source="../snippets/allnotes/Views/AllNotesPage.xaml":::

The previous XAML introduces a few new concepts:

- The `ContentPage.ToolbarItems` property contains a `ToolbarItem`. The buttons defined here are usually display at the top of the app, along the page title. Depending on the platform, though, it may be in a different position. When one of these buttons is pressed, the `Clicked` event is raised, just like a normal button.

  The `ToolbarItem.IconImageSource` property sets the icon to display on the button. The icon can be any image resource defined by the project, however, in this example, a `FontImage` is used. A `FontImage` can use a single glyph from a font as an image.

- The <xref:Microsoft.Maui.Controls.CollectionView> control displays a collection of items, and in this case, is bound to the model's `Notes` property. The way each item is presented by the collection view is set through the  `CollectionView.ItemsLayout` and `CollectionView.ItemTemplate` properties.

  For each item in the collection, the `CollectionView.ItemTemplate` generates the declared XAML. The `BindingContext` of that XAML becomes the collection item itself, in this case, each individual note. The template for the note uses two labels, which are bound to the note's `Text` and `Date` properties.

- The <xref:Microsoft.Maui.Controls.CollectionView> handles the `SelectionChanged` event, which is raised when an item in the collection view is selected.

The code-behind for the view needs to be written to load the notes and handle the events.

01. In the **Solution Explorer** pane, open the _Views/AllNotesPage.xaml.cs_ file.
01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/allnotes/Views/AllNotesPage.xaml.cs":::

This code uses the constructor to set the `BindingContext` of the page to the model.

The `OnAppearing` method is overridden from the base class. This method is automatically called whenever the page is shown, such as when the page is navigated to. The code here tells the model to load the notes. Because the <xref:Microsoft.Maui.Controls.CollectionView> in the **AllNotes view** is bound to the **AllNotes model's** `Notes` property, which is an `ObservableCollection`, whenever the notes are loaded, the <xref:Microsoft.Maui.Controls.CollectionView> is automatically updated.

The `Add_Clicked` handler introduces another new concept, navigation. Because the app is using .NET MAUI Shell, you can navigate to pages by calling the `Shell.Current.GoToAsync` method. Notice that the handler is declared with the `async` keyword, which allows the use of the `await` keyword when navigating. This handler navigates to the `NotePage`.

The last piece of code in the previous snippet is the `notesCollection_SelectionChanged` handler. This method takes the currently selected item, a **:::no-loc text="Note":::** model, and uses its information to navigate to the `NotePage`. <xref:Microsoft.Maui.Controls.Shell.GoToAsync%2A> uses a URI string for navigation. In this case, a string is constructed that uses a query string parameter to set a property on the destination page. The interpolated string representing the URI ends up looking similar to the following string:

```text
NotePage?ItemId=path\on\device\XYZ.notes.txt
```

The `ItemId=` parameter is set to the file name on the device where the note is stored.

Visual Studio may be indicating that the `NotePage.ItemId` property doesn't exist, which it doesn't. The next step is modifying the **:::no-loc text="Note"::: view** to load the model based on the `ItemId` parameter that you'll create.

## Query string parameters

The **:::no-loc text="Note"::: view** needs to support the query string parameter, `ItemId`. Create it now:

01. In the **Solution Explorer** pane, open the _Views/NotePage.xaml.cs_ file.
01. Add the `QueryProperty` attribute to the `class` keyword, providing the name of the query string property, and the class property it maps to, `ItemId` and `ItemId` respectively:

    :::code language="csharp" source="../snippets/allnotes/Views/NotePage.xaml.cs" id="query_prop":::

01. Add a new `string` property named `ItemId`. This property calls the `LoadNote` method, passing the value of the property, which in turn, should be the file name of the note:

    :::code language="csharp" source="../snippets/allnotes/Views/NotePage.xaml.cs" id="itemid":::

01. Replace the `SaveButton_Clicked` and `DeleteButton_Clicked` handlers with the following code:

    :::code language="csharp" source="../snippets/allnotes/Views/NotePage.xaml.cs" id="buttons":::

    The buttons are now `async`. After they're pressed, the page navigates back to the previous page by using a URI of `..`.

01. Delete the `_fileName` variable from top of the code, as it's no longer used by the class.

## Modify the app's visual tree

The `AppShell` is still loading the single note page, instead, it needs to load the **AllPages view**. Open the _AppShell.xaml_ file and change the first <xref:Microsoft.Maui.Controls.ShellContent> entry to point to the `AllNotesPage` instead of `NotePage`:

:::code language="xaml" source="../snippets/allnotes/AppShell.xaml" highlight="12":::

If you run the app now, you'll notice it crashes if you press the **Add** button, complaining that it can't navigate to `NotesPage`. Every page that can be navigated to from another page, needs to be registered with the navigation system. The `AllNotesPage` and `AboutPage` pages are automatically registered with the navigation system by being declared in the <xref:Microsoft.Maui.Controls.TabBar>.

Register the `NotesPage` with the navigation system:

01. In the **Solution Explorer** pane, open the _AppShell.xaml.cs_ file.
01. Add a line to the constructor that registers the navigation route:

    :::code language="csharp" source="../snippets/allnotes/AppShell.xaml.cs" highlight="9":::

The `Routing.RegisterRoute` method takes two parameters:

- The first parameter is the string name of the URI you want to register, in this case the resolved name is `"NotePage"`.
- The second parameter is the type of page to load when `"NotePage"` is navigated to.

Now you can run your app. Try adding new notes, navigating back and forth between notes, and deleting notes.

If you want to download a copy of the completed project to compare your code with, download [this project](https://github.com/dotnet/maui-samples/raw/main/8.0/Tutorials/CreateNetMauiApp/app_after.zip).
