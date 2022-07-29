---
author: adegeo
ms.author: adegeo
ms.date: 07/29/2022
ms.topic: include
---

This portion of the tutorial introduces the concepts of views, models, and in-app navigation.

Currently the project has three pages: `MainPage`, `NotePage`, and `AboutPage`. The pages represent a view of the data model. The `NotePage` is a "view" that displays a "note data model" and the `AboutPage` is a "view" that displays "app information data model." Both of these views have their models hardcoded, or embedded in them, and you'll need to separate the view from the model.

What is the benefit of separating the view from the model? It allows you to alter the view without worrying about impacting the model. This is accomplished using data binding, something that will be presented later in this tutorial. For now, though, lets restructure the project.

## Separate the view and model

Refactor the existing code to separate the model from the view. The next few steps will organize the code so that views and models are defined separate from each other.

01. Delete _MainPage.xaml_ and _MainPage.xaml.cs_ from your project, they're no longer needed. In the **Solution Explorer** pane, find the entry for **MainPage.xaml** and right-click it and select **Delete**.

    > [!TIP]
    > Deleting the _MainPage.xaml_ item should also delete the _MainPage.xaml.cs_ item too. If it didn't delete the file, right-click on it and select **Delete**.

01. Right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **Models**.
01. Right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **Views**.
01. Find the **NotePage.xaml** item and drag it to the **Views** folder. The **NotePage.xaml.cs** should move with it.

    > [!IMPORTANT]
    > When you move a file, Visual Studio usually prompts you with a warning about how the move operation may take a long time. This shouldn't be a problem here, press **OK** if you see this warning.
    >
    > Visual Studio may also ask you if you want to adjust the namespace of the moved file. You may select **Yes** here, but the next steps will change the namespace.

01. Find the **AboutPage.xaml** item and drag it to the **Views** folder. The **AboutPage.xaml.cs** should move with it.

### Update the view namespace

Now that the views have been moved to the **Views** folder, you'll need to alter the namespaces to match. The namespace for the XAML and code-behind files of the pages is set to `Notes`. This needs to be updated to `Notes.Views`.

01. In the **Solution Explorer** pane, expand both **NotePage.xaml** and **AboutPage.xaml** to reveal the code-behind files:

    :::image type="content" source="../media/navigation/vs-expand-views.png" alt-text="The Notes project with both the Views folder and the page views expanded.":::

01. Double-click on the **NotePage.xaml.cs** item to open the code editor. Change the namespace to `Notes.Views`:

    ```csharp
    namespace Notes.Views;
    ```

01. Repeat the previous step for the **AboutPage.xaml.cs** item.
01. Double-click on the **NotePage.xaml** item to open the XAML editor. The old namespace is referenced through the `x:Class` attribute, which defines which class type is the code-behind for the XAML. This entry isn't just the namespace, but the namespace with the type. Change the `x:Class` value to `Notes.Views.NotePage`:

    ```xaml
    x:Class="Notes.Views.NotePage"
    ```

01. Repeat the previous step for the **AboutPage.xaml** item, but set the `x:Class` value to `Notes.Views.AboutPage`.

### Fix the namespace reference in Shell

The _AppShell.xaml_ contains two tabs, one for the NotesPage and another for AboutPage. Now that those two pages were moved to a new namespace, the type mapping in the XAML is now invalid. In the **Solution Explorer** pane, double-click on the **AppShell.xaml** entry to open it in the XAML editor. It should look like the following snippet:

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="Notes.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Notes"
    WidthRequest="400"
    Shell.FlyoutBehavior="Disabled">

    <TabBar>
        <ShellContent
            Title="Notes"
            ContentTemplate="{DataTemplate local:NotePage}" />
        
        <ShellContent
            Title="About"
            ContentTemplate="{DataTemplate local:AboutPage}" />
    </TabBar>

</Shell>
```

A .NET namespace is imported into the XAML through an XML namespace declaration. In the previous XAML markup, it's the `xmlns:local="clr-namespace:Notes"` attribute in the root element: `<Shell>`. The format of declaring an XML namespace to import a .NET namespace is `xmlns:{XML namespace name}="clr-namespace:{.NET namespace}`. So the previous declaration maps the XML namespace of `local` to the .NET namespace of `Notes`. It's common practice to map the name `local` to the root namespace of your project.

Edit the `local` XML namespace declaration and set the .NET namespace to `Notes.Views`. Your XAML should now look like the following snippet:

```xaml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="Notes.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:Notes.Views"
    WidthRequest="400"
    Shell.FlyoutBehavior="Disabled">

    <TabBar>
        <ShellContent
            Title="Notes"
            ContentTemplate="{DataTemplate local:NotePage}" />
        
        <ShellContent
            Title="About"
            ContentTemplate="{DataTemplate local:AboutPage}" />
    </TabBar>

</Shell>
```

You should now be able to run the app without any compiler errors, and everything should still work as before.

## Define the model

Currently the model is the data that is embedded in the note and about views. We'll create new classes to represent that data. First, the model to represent a note page's data:

01. In the **Solution Explorer** pane, right-click on the **Models** folder and select **Add** > **Class...**.
01. Name the class **Note.cs** and press **Add**.

Now you'll need to create a model to represent the about page's data. Repeat the previous steps, but name the class **About.cs**.

Open **Note.cs** and replace the code with the following snippet:

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Models/Note.cs":::

Open **About.cs** and replace the code with the following snippet:

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Models/About.cs":::

## Update About page

The about page will be the quickest to update and you'll be able to run the app and see how it loads data from the model.

01. Open the _Views\\AboutPage.xaml_ file.
01. Replace the content with the following snippet:

    :::code language="xaml" source="../snippets/navigation/csharp/Notes/Views/AboutPage.xaml" highlight="4,6-8,14,15,18":::

Let's look at the changed lines:

- `xmlns:models="clr-namespace:Notes.Models"`

  This line maps the `Notes.Models` .NET namespace to the `models` XML namespace.

- The `BindingContext` property of the `ContentPage` was set to an object using object markup instead of an XML attribute. The `BindingContext` was set to an instance of the `Note.Models.About` class, using the XML namespace and object of `models:About`.

  > [!IMPORTANT]
  > Until now, properties have been set using an XML attribute. This works great for simple values, such as a `Label.FontSize` property. But if the property value is more complex, you must use XAML markup to create the object. Consider the following example of a creating a label with its `FontSize` property set:
  >
  > ```xaml
  > <Label FontSize="22" />
  > ```
  >
  > The same `FontSize` property can be set using XAML markup:
  >
  > ```xaml
  > <Label>
  >     <Label.FontSize>
  >         22
  >     </Label.FontSize>
  > </Label>
  > ```

- Three `<Label>` controls had their `Text` property value changed from a hardcoded string to binding syntax: `{Binding PATH}`.

  `{Binding}` syntax is processed at run-time, allowing value returned from the binding to be dynamic. The `PATH` value following `{Binding` is the property path to bind to. The property comes from the current control's `BindingContext` property. With the `<Label>` control, `BindingContext` is unset. When the context is unset, .NET MAUI checks the parent control for a context, and so on, walking up the control hierarchy looking for a valid context.

  Eventually the `ContentPage` as the root object is discovered with a set `BindingContext`.

The final change to the about page is updating the button click that opens a web page. The URL was hardcoded in the code, but the URL should come from the model that is in the `BindingContext` property.

01. Open the _Views\\AboutPage.xaml.cs_ file.
01. Replace the `LearnMore_Clicked` method with the following code:

    :::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/AboutPage.xaml.cs" id="learn_more" highlight="3":::

If you look at the highlighted line, the first thing the code does is check if the `BindingContext` is a `Models.About` type, and if it is, assigns it to the `about` variable. The next line inside of the `if` statement opens the browser to the URL provided by the `about.MoreInfoUrl` property.

Run the app and you should see that it runs exactly the same as before. Try changing the about model's values and see how the UI and URL opened by the browser also change.

## Update Note page

The previous section bound the **about** page view to the **about** model, and now you'll do the same, binding the **note** view to the **note** model.

First, update the view XAML. Change the `<Editor>` control, binding the `Text` property to the note model's `Text` property: `<Editor ... Text="{Binding Text}"`:

:::code language="xaml" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml" highlight="9":::

The modifications for the code-behind are more complicated than the XAML. The current code is loading the file content in the constructor, and then setting it directly to the `TextEditor.Text` property:

```csharp
public NotePage()
{
    InitializeComponent();

    if (File.Exists(_fileName))
        TextEditor.Text = File.ReadAllText(_fileName);
}
```

Instead of loading the note in the constructor, create a new `LoadNote` method. This method does the following items in order:

01. Accept a file name parameter.
01. Create a new note model and set the file name.
01. If the file exists, load its content into the model.
01. If the file exists, update the model with the date the file was created.
01. Set the `BindingContext` to the model.

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml.cs" id="load_note_by_file":::

Update the constructor to call `LoadNote`. The file name for the note should be a randomly generated name to be created in the app data directory of the device.

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml.cs" id="load_note_ctor" highlight="8":::

You can remove the `_fileName` variable from top of the code, as it's no longer used.

## Multiple notes and navigation

Currently the **Note** view displays a single note, and there isn't a view that represents multiple notes. To display multiple notes, create a new view and model: **AllNotes**.

01. In the **Solution Explorer** pane, right-click on the **Views** folder and select **Add** > **New Item...**
01. In the **Add New Item** dialog, select **.NET MAUI** in the template list on the left-side of the window. Next, select the **.NET MAUI ContentPage (XAML)** template. Name the file _AllNotesPage.xaml_, and then select **Add**.
01. In the **Solution Explorer** pane, right-click on the **Models** folder and select **Add** > **Class...**
01. Name the class **AllNotes.cs** and press **Add**.

### Code the AllNotes model

First, the model should be created to represent the data required to display multiple notes. This data would be a property that represents a collection of notes. The collection will be an `ObservableCollection` which .NET MAUI controls understand as a collection of objects that can change. When an `ObservableCollection` is bound to a list control, the control adds and removes items automatically to stay in sync with the collection.

Paste the following code in the _Models\\AllNotes.cs_ class:

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Models/AllNotes.cs":::

The constructor of the class calls the `LoadNotes` method. This method uses Linq extensions to load, transform, and sort the data into the `Notes` collection.

### Design the AllNotes page

Next, the view needs to be designed for the **AllNotes** model.

Paste the following code in the _Views\\AllNotesPage.xaml_ page:

:::code language="xaml" source="../snippets/navigation/csharp/Notes/Views/AllNotesPage.xaml":::

The previous XAML introduces a few new concepts:

- The `ContentPage.ToolbarItems` property contains a `ToolbarItem`. The buttons defined here usually display at the top of the app, along the page title. Depending on the device, though, it may be in a different position. When one of these buttons is pressed, the `Clicked` event is raised, just like a normal button.
- The `CollectionView` control displays a collection of items, and in this case, is bound to the model's `Notes` property. The way each item is presented by the collection view is set through the  `CollectionView.ItemsLayout` and `CollectionView.ItemTemplate` properties.

  For each item in the collection, the `CollectionView.ItemTemplate` generates the declared XAML. The `BindingContext` of that XAML becomes the item itself, in this case, each individual note. The template for the note uses two labels, which are bound to the note's `Text` and `Date` properties.

The code-behind for the view needs to be written. Open the _Views\\AlLNotesPage.xaml.cs_ file and paste the following code:

:::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/AllNotesPage.xaml.cs":::

This code uses the constructor to set the `BindingContext` of the page to the model. The `OnAppearing` method is overridden from the base class. This method is automatically called whenever the page is shown, such as when the page is first loaded or navigated to. The code here tells the model to load the notes. Because the `CollectionView` in the **AllNotes view** is bound to the **AllNotes model's** `Notes` property, which is an `ObservableCollection`, whenever the notes are loaded, the `CollectionView` is automatically updated.

The `Add_Clicked` handler introduces another new concept, navigation. Because the app is using the .NET MAUI Shell, you can navigate to new pages by calling the `Shell.Current.GoToAsync` method. Notice that the handler was also modified to use the `async` keyword, which allows the use of the `await` keyword when navigating. This handler navigates to the `NotePage`.

The last piece of code in the previous snippet is the `notesCollection_SelectionChanged` handler. This method takes the currently selected item, a **Note** model, and uses its information to navigate to the `NotePage` while providing a parameter, the file name of the note. `GoToAsync` uses a string, representing a URI, for navigation. In this case, `nameof(NotePage)` is used to generate a string value of `"NotePage"`. The URI can also contain query string parameters, which are key-value pairs representing date passed to the destination page. The interpolated string representing the URI ends up looking similar to `NotePage?ItemId=path\on\device\XYZ.notes.txt`, where the `ItemId` parameter is set to the file name of the note.

The next step is modifying the **Note view** to load the model based on the `ItemId` parameter.

### Query string parameters

The **Note view** needs to support the query string parameter, `ItemId`. To handle this, open the _Views\\NotePage.xaml.cs_ and perform the following changes:

01. Add the `QueryProperty` attribute to the class, providing the name of the query string property, and the class property it maps to, `ItemId` and `ItemId` respectively:

    :::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml.cs" id="query_prop":::

    > [!TIP]
    > The names for both the query string property and the class property are kept in sync by using the `nameof(ItemId)` expression. If the class property name changes, the code will error in all of the `nameof(ItemId)` references, preventing you from accidentally disconnecting a query string parameter from its backing class property.

01. Add a new `string` property named `ItemId`. This property calls the `LoadNote` method, passing the value of the property, which in turn, should be the file name of the note:

    :::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml.cs" id="itemid":::

01. Replace the `SaveButton_Clicked` and `DeleteButton_Clicked` handlers with the following code:

    :::code language="csharp" source="../snippets/navigation/csharp/Notes/Views/NotePage.xaml.cs" id="buttons":::

    The buttons are now `async`. After they're pressed, the page navigates back to the previous page by using a URI of `..`.

## Modify the Shell

The Shell is still loading the single note page, instead, it needs to load the **AllPages view**. Open the _AppShell.xaml_ file and change the first `ShellContent` entry to point to the `AllNotesPage` instead of `NotePage`:

:::code language="xaml" source="../snippets/navigation/csharp/Notes/AppShell.xaml" highlight="13":::

If you run the app now, you'll notice it crashes if you press the **Add** button, complaining that it can't navigate to `NotesPage`. Every page that can be navigated to by the shell, except the first page, which is automatically registered, needs to be registered with the Shell. Open the _AppShell.xaml.cs_ file and add a line to the constructor that registers the navigation route.

:::code language="csharp" source="../snippets/navigation/csharp/Notes/AppShell.xaml.cs" highlight="9":::

The `Routing.RegisterRoute` method takes two parameters:

- The first parameter is the string name of the URI you want to register, in this case the resolved name is `"NotePage"`.
- The second parameter is the type of page to load when `"NotePage"` is navigated to.

Now you can run your app. Try adding new notes, navigating back and forth between notes, and deleting notes.
