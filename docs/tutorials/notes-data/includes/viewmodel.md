---
author: adegeo
ms.author: adegeo
ms.date: 12/03/2022
ms.topic: include
no-loc: ["communitytoolkit", "CommunityToolkit.Mvvm", "AllNotes", "Notes", "About"]
---

Before adding view models to the project, add reference to the MVVM Community Toolkit. This library is available on NuGet, and provides types and systems that help implement the MVVM pattern.

01. In the **Solution Explorer** pane of Visual Studio, right-click on the **Notes** project > **Manage NuGet Packages**.
01. Select the **Browse** tab.
01. Search for **communitytoolkit mvvm** and select the `CommunityToolkit.Mvvm` package, which should be the first result.
01. Make sure that version **8.0.0** is selected.
01. Next, select **Install** and accept any prompts that are displayed.

    :::image type="content" source="../media/viewmodel/nuget.png" alt-text="Searching for the CommunityToolkit.Mvvm package in NuGet.":::

Now you're ready to start updating the project by adding view models.

## Decouple with view models

The view-to-viewmodel relationship relies heavily on the binding system provided by .NET MAUI. The app is already using binding in the views to display a list of notes and to present the text and date of a single note. The app logic is currently provided by the view's code-behind and is directly tied to the view. For example, when a user is editing a note and presses the **Save** button, the `Clicked` event for the button is raised. Then, the code-behind for the event handler saves the note text to a file and navigates to the previous screen.

Having app logic in the code-behind of a view can become an issue when the view changes. For example if the button is replaced with a different input control, or the name of a control is changed, event handlers may become invalid. Regardless of how the view is designed, the purpose of the view is to invoke some sort of app logic and to present information to the user. For this app, it's saving the note and then navigating back to the previous screen.

The viewmodel gives the app a specific place to code the app logic regardless of how the UI is designed, and regardless of how the data is being loaded or saved. The viewmodel is the glue that represents and interacts with the data model on behalf of the view.

The view models are stored in a _ViewModels_ folder.

01. Find the **Solution Explorer** pane of Visual Studio.
01. Right-click on the **Notes** project and select **Add** > **New Folder**. Name the folder **ViewModels**.
01. Right-click on the **ViewModels** folder > **Add** > **Class** and name it **About.cs**.
01. Add two more classes using the previous step:
    - **Note.cs**
    - **Notes.cs**

Your project structure should look like the following image:

:::image type="content" source="../media/viewmodel/class-structure-1.png" alt-text="Solution explorer showing MVVM folders.":::

## About viewmodel and About view

The **About view** displays some data on the screen and optionally navigates to a website with more information. Since this view doesn't have any data to change, like with an entry control or selecting items from a list, it's a good candidate to demonstrate adding a viewmodel. For the **About viewmodel**, there isn't a backing model.

Create the **About view model**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\About.cs**.
01. Paste in the following code:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/About.cs":::

The previous code snippet contains some properties that represent information about the app, such as the name and version. This snippet is exactly the same as the **About model** you deleted earlier. However, this viewmodel contains a new concept, the `ShowMoreInfoCommand` command property.

Commands are bindable actions that invoke code, and are a great place to put app logic. In this example, the `ShowMoreInfoCommand` points to the `ShowMoreInfo` method, which opens the web browser to a specific page. You'll learn more about the command system in the next section.

### About view

The **About view** needs to be changed slightly to hook it up to the viewmodel that was created in the previous section. Modify the _Views\\AboutPage.xaml_ file:

- Update the `xmlns:models` XML namespace to `xmlns:viewModels` and target the `Notes.ViewModels` .NET namespace.
- Change the `ContentPage.BindingContext` property to a new instance of the `About` viewmodel.
- Remove the button's `Clicked` event handler and use the `Command` property.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AboutPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel/csharp/Notes/Views/AboutPage.xaml" highlight="4,7,19":::

    The previous code snippet highlights the lines that have changed in this version of the view.

Notice that the button is using the `Command` property. Many controls support a `Command` property that is invoked when the user interacts with the control. When used with a button, the command is invoked when a user presses the button, similar to how the `Clicked` event handler is invoked, except that you can bind `Command` to the viewmodel.

In this view, when the user presses the button, the `Command` is invoked. The `Command` is bound to the `ShowMoreInfoCommand` property in the viewmodel, and when invoked, runs the code in the `ShowMoreInfo` method, which opens the web browser to a specific page.

### Clean up the About code-behind

The `ShowMoreInfo` button isn't using the event handler, so the `LearnMore_Clicked` code should be removed from the _Views\\AboutPage.xaml.cs_ file. Delete that code. The class should only contain the constructor:

:::code language="csharp" source="../snippets/viewmodel/csharp/Notes/Views/AboutPage.xaml.cs":::

## Notes viewmodel and AllNotes view

The **AllNotes view** is going to use the **Notes viewmodel**. Currently though, it's binding directly to the model, which was deleted at the start of this tutorial. The goal in updating the **AllNotes view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel. Again, the benefit being that the view can change its design with little effect to your code.

### Notes viewmodel

Based on what the **AllNotes view** is going to display and what interactions the user will do, the **Notes viewmodel** must provide the following items:

- A collection of notes.
- A command to handle navigating to a note.
- A command to create a new note.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\Notes.cs**.
01. Replace the code in this file with the following code:

    ```csharp
    using CommunityToolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    
    namespace Notes.ViewModels;
    
    internal class Notes
    {
    }
    ```

    This code is the blank `Notes` viewmodel where you'll add properties and commands to support the `AllNotes` view. Notice that the `CommunityToolkit.Mvvm.Input` namespace is being imported. This namespace provides some command-types that invoke methods asynchronously.

01. In the `Notes` class code, add the following properties:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Notes.cs" id="properties":::

    The `AllNotes` property is an `ObservableCollection` that stores all of the notes loaded from the device. The two commands will be used by the view to trigger the actions of creating a new note or selecting an existing note.

01. Add a parameterless constructor to the class, which initializes the commands and loads the notes from the model:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Notes.cs" id="ctor":::

    Notice that the `AllNotes` collection uses the `Models.Note.LoadAll` method fill the observable collection with notes. The `LoadAll` method returns the notes as the `Models.Note` type, but the observable collection is a collection of `ViewModel.Note` types. The code uses the `Select` Linq extension to create viewmodel instances from the note models returned from `LoadAll`.

01. Create the methods targeted by the commands:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Notes.cs" id="commands":::

    Notice that the `NewNoteAsync` method doesn't take a parameter while the `SelectNoteAsync` does. Commands can optionally have a single parameter that is provided when the command is invoked. For the `SelectNoteAsync` method, you want to provide the note that's being selected.

    The code for `SelectNoteAsync` that navigates to an existing note is relatively the same as the previous tutorial, with just a slight difference in the query attribute. Instead of mapping a property name to a query string attribute, the query string is just being set directly: `load={note.Identifier}`. The query string data will be read differently in the **Note viewmodel** than it was in the previous tutorial.

Don't worry that the code won't compile, you'll fix it later. The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Notes.cs" id="full":::

### AllNotes view

Now that the viewmodel has been created, update the **AllNotes view** to point to the viewmodel properties. In the _Views\\AllNotesPage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the toolbar button's `Clicked` event and use the `Command` property.
- Change the `CollectionView` to use commanding to react to when the selected item changes.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel/csharp/Notes/Views/AllNotesPage.xaml" id="full" highlight="4,8,15,22-23":::

The `CollectionView` supports commanding with the `SelectionChangedCommand` and `SelectionChangedCommandParameter` properties. In the updated XAML, the `SelectionChangedCommand` property is bound to the viewmodel's `SelectNodeCommand`, which means the command is invoked when the selected item changes. When the command is invoked, the `SelectionChangedCommandParameter` property value is passed to the command. The XAML value for the property uses `{RelativeSource Self}` binding to bind directly to the `CollectionView.SelectedItem` property. Meaning, when the command is invoked, the item that's currently selected in the collection view is passed to the command as a parameter.

:::code language="xaml" source="../snippets/viewmodel/csharp/Notes/Views/AllNotesPage.xaml" id="CollectionView" highlight="5-6":::

The toolbar no longer uses the `Clicked` event and instead uses a command.

### Clean up the AllNotes code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\AllNotesPage.xaml.cs_ file and replace all the code with the following snippet:

:::code language="csharp" source="../snippets/viewmodel/csharp/Notes/Views/AllNotesPage.xaml.cs":::

## Note viewmodel and Note view

The final viewmodel-view pair is the **Note viewmodel** and **Note view**. Again, the goal in updating the **Note view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel.

### Note viewmodel

Based on what the **Note view** requires, the **Note viewmodel** needs to provide the following items:

- The text of the note.
- The date/time the note was created or last updated.
- A command to that saves the note.
- A command to that deletes the note.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\Note.cs**.
01. Replace the code in this file with the following snippet:

    ```csharp
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using System.Windows.Input;
    
    namespace Notes.ViewModels;
    
    internal class Note : ObservableObject, IQueryAttributable
    {
        private Models.Note _note;

    }
    ```

    This code is the blank `Note` viewmodel where you'll add properties and commands to support the `Note` view. Notice that the `CommunityToolkit.Mvvm.ComponentModel` namespace is being imported. This namespace provides the `ObservableObject` used as the base class. You'll learn more about `ObservableObject` in the next step.

    The `Models.Note` model is being stored as a private field, the next step is to map public properties to the model.

01. Add the following properties to the class:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="properties":::

    The `Date` and `Identifier` properties are simple properties that just retrieve the corresponding values from the model.

    > [!TIP]
    > For properties, the `=>` syntax creates a `get`-only property where the statement to the right of `=>` must evaluate to a value to return.

    The `Text` property first checks if the value being set is a different value. If the value is different, it's set to the property and the `OnPropertyChanged` method is called.

    The `OnPropertyChanged` method is provided by the `ObservableObject` base class. This method uses the name of the calling code, in this case the property name of **Text**, and raises the `ObservableObject.PropertyChanged` event. This event supplies the name of the property to any event subscribers. The binding system provided by .NET MAUI recognizes this event, and updates any related bindings in the UI. In the example of this code, when the `Text` property changes, the event is raised, and any UI element that is bound to the `Text` property is notified that the property changed.

01. Add the following command-properties to the class, which are the commands that the view can bind to:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="commands":::

01. Add the following constructors to the class:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="ctor":::

    These two constructors are used to either create the viewmodel as an empty note, or to load the viewmodel from the supplied model. The constructors also use the `SetupCommands` method, which you'll add next.

01. Create the `SetupCommands` method, along with the methods that the commands will reference:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="command_methods":::

    These commands invoke the specified action on the underlying model, and navigate to the previous page.

01. Finally, add the following methods to the class:

    :::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="methods":::

    The `ApplyQueryAttributes` method satisfies the `IQueryAttributable` interface that is declared on the class. When a page or the binding context of a page implements this interface, the query parameters used in navigation are passed to the `ApplyQueryAttributes` method. This viewmodel is used as the binding context for the **Note view**. When the **Note view** is navigated to, that binding context (this viewmodel) is passed the query string parameters used during navigation.

    This code checks if the `load` key was provided in the `query` dictionary. If this key is found, the value should be the identifier (the file name) of the note to load. That note is loaded and set as the underlying model object of this viewmodel instance.

    The `RefreshProperties` method is a helper method to ensure that any subscribers bound to this object are notified that the `Text` and `Date` properties have changed. Since the underlying model (the `_note` field) is changed when the note is loaded during navigation, the `Text` and `Date` properties aren't actually set to new values. Since these properties aren't directly set, any bindings attached to those properties wouldn't be notified because `OnPropertyChanged` isn't called for each property. `RefreshProperties` ensures bindings to these properties are refreshed.

The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel/csharp/Notes/ViewModels/Note.cs" id="full":::

### Note view

Now that the viewmodel has been created, update the **Note view**. In the _Views\\NotePage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the delete and save button `Clicked` event handlers and replace them with commands.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\NotePage.xaml** to open the XAML editor.
01. Paste in the following code:

:::code language="xaml" source="../snippets/viewmodel/csharp/Notes/Views/NotePage.xaml" highlight="4,7-9,18,21":::

Previously, this view didn't declare a binding context, as it was supplied by the code-behind of the page itself. Setting the binding context directly in the XAML provides two things:

- At run-time, when the page is navigated to, it displays a blank note. This is because the default constructor for the note viewmodel is invoked, not the constructor that loads a note viewmodel from a model.

- The intellisense in the XAML editor shows the available properties as soon as you start typing `{Binding` syntax. The syntax is also validated and alerts you of an invalid value. Try changing the binding syntax for the `SaveCommand` to `Save123Command`. If you hover the mouse cursor over the text, you'll notice that a tooltip is displayed informing you that **Save123Command** isn't found. This notification isn't considered an error because bindings are dynamic, it's really a small warning that may help you notice when you typed the wrong property.

### Clean up the Note code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\NotePage.xaml.cs_ file and replace all the code with the following snippet:

:::code language="csharp" source="../snippets/viewmodel/csharp/Notes/Views/NotePage.xaml.cs":::

## Run the app

You can now run the app. Notice that when you add or delete a note, the **AllNotes view** isn't updated. If you restart the app, you'll see the list is updated. Obviously you want the list immediately updated when a note is added or deleted. You'll fix this problem in the next step of the tutorial by introducing a notification system.
