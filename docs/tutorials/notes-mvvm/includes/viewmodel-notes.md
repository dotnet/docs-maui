---
author: adegeo
ms.author: adegeo
ms.date: 12/12/2022
ms.topic: include
no-loc: ["communitytoolkit", "CommunityToolkit.Mvvm", "AllNotes", "Notes", "About"]
---

The **AllNotes view** is going to use the **Notes viewmodel**. Currently though, it's binding directly to the model, which was deleted at the start of this tutorial. The goal in updating the **AllNotes view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel. Again, the benefit being that the view can change its design with little effect to your code.

## Notes viewmodel

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

    :::code language="csharp" source="../snippets/viewmodel-notes/csharp/Notes/ViewModels/Notes.cs" id="properties":::

    The `AllNotes` property is an `ObservableCollection` that stores all of the notes loaded from the device. The two commands will be used by the view to trigger the actions of creating a new note or selecting an existing note.

01. Add a parameterless constructor to the class, which initializes the commands and loads the notes from the model:

    :::code language="csharp" source="../snippets/viewmodel-notes/csharp/Notes/ViewModels/Notes.cs" id="ctor":::

    Notice that the `AllNotes` collection uses the `Models.Note.LoadAll` method fill the observable collection with notes. The `LoadAll` method returns the notes as the `Models.Note` type, but the observable collection is a collection of `ViewModel.Note` types. The code uses the `Select` Linq extension to create viewmodel instances from the note models returned from `LoadAll`.

01. Create the methods targeted by the commands:

    :::code language="csharp" source="../snippets/viewmodel-notes/csharp/Notes/ViewModels/Notes.cs" id="commands":::

    Notice that the `NewNoteAsync` method doesn't take a parameter while the `SelectNoteAsync` does. Commands can optionally have a single parameter that is provided when the command is invoked. For the `SelectNoteAsync` method, you want to provide the note that's being selected.

    The code for `SelectNoteAsync` that navigates to an existing note is relatively the same as the previous tutorial, with just a slight difference in the query attribute. Instead of mapping a property name to a query string attribute, the query string is just being set directly: `load={note.Identifier}`. The query string data will be read differently in the **Note viewmodel** than it was in the previous tutorial.

Don't worry that the code won't compile, you'll fix it later. The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel-notes/csharp/Notes/ViewModels/Notes.cs" id="full":::

## AllNotes view

Now that the viewmodel has been created, update the **AllNotes view** to point to the viewmodel properties. In the _Views\\AllNotesPage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the toolbar button's `Clicked` event and use the `Command` property.
- Change the `CollectionView` to use commanding to react to when the selected item changes.

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel-notes/csharp/Notes/Views/AllNotesPage.xaml" id="full" highlight="4,8,13,21-22":::

The `CollectionView` supports commanding with the `SelectionChangedCommand` and `SelectionChangedCommandParameter` properties. In the updated XAML, the `SelectionChangedCommand` property is bound to the viewmodel's `SelectNodeCommand`, which means the command is invoked when the selected item changes. When the command is invoked, the `SelectionChangedCommandParameter` property value is passed to the command. The XAML value for the property uses `{RelativeSource Self}` binding to bind directly to the `CollectionView.SelectedItem` property. Meaning, when the command is invoked, the item that's currently selected in the collection view is passed to the command as a parameter.

:::code language="xaml" source="../snippets/viewmodel-notes/csharp/Notes/Views/AllNotesPage.xaml" id="CollectionView" highlight="5-6":::

The toolbar no longer uses the `Clicked` event and instead uses a command.

## Clean up the AllNotes code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\AllNotesPage.xaml.cs_ file and replace all the code with the following snippet:

:::code language="csharp" source="../snippets/viewmodel-notes/csharp/Notes/Views/AllNotesPage.xaml.cs":::
