---
author: adegeo
ms.author: adegeo
ms.date: 03/21/2024
ms.topic: include
no-loc: ["Notes viewmodel", "AllNotes view", "ViewModels", "NotesViewModel.cs", "AllNotesPage.xaml", "AllNotesPage.xaml.cs"]
---

The final viewmodel-view pair is the **Notes viewmodel** and **AllNotes view**. Currently though, the view is binding directly to the model, which was deleted at the start of this tutorial. The goal in updating the **AllNotes view** is to move as much functionality as possible out of the XAML code-behind and put it in the viewmodel. Again, the benefit being that the view can change its design with little effect to your code.

## Notes viewmodel

Based on what the **AllNotes view** is going to display and what interactions the user will do, the **Notes viewmodel** must provide the following items:

- A collection of notes.
- A command to handle navigating to a note.
- A command to create a new note.
- Update the list of notes when one is created, deleted, or changed.

Create the **Notes viewmodel**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **ViewModels\\NotesViewModel.cs**.
01. Replace the code in this file with the following code:

    ```csharp
    using CommunityToolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    namespace Notes.ViewModels;

    internal class NotesViewModel: IQueryAttributable
    {
    }
    ```

    This code is the blank `NotesViewModel` where you'll add properties and commands to support the `AllNotes` view.

01. In the `NotesViewModel` class code, add the following properties:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NotesViewModel.cs" id="properties":::

    The `AllNotes` property is an `ObservableCollection` that stores all of the notes loaded from the device. The two commands will be used by the view to trigger the actions of creating a note or selecting an existing note.

01. Add a parameterless constructor to the class, which initializes the commands and loads the notes from the model:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NotesViewModel.cs" id="ctor":::

    Notice that the `AllNotes` collection uses the `Models.Note.LoadAll` method to fill the observable collection with notes. The `LoadAll` method returns the notes as the `Models.Note` type, but the observable collection is a collection of `ViewModels.NoteViewModel` types. The code uses the `Select` Linq extension to create viewmodel instances from the note models returned from `LoadAll`.

01. Create the methods targeted by the commands:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NotesViewModel.cs" id="commands":::

    Notice that the `NewNoteAsync` method doesn't take a parameter while the `SelectNoteAsync` does. Commands can optionally have a single parameter that is provided when the command is invoked. For the `SelectNoteAsync` method, the parameter represents the note that's being selected.

01. Finally, implement the `IQueryAttributable.ApplyQueryAttributes` method:

    :::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NotesViewModel.cs" id="query":::

    The **Note viewmodel** created in the previous tutorial step, used navigation when the note was saved or deleted. The viewmodel navigated back to the **AllNotes view**, which this viewmodel is associated with. This code detects if the query string contains either the `deleted` or `saved` key. The value of the key is the unique identifier of the note.

    If the note was **deleted**, that note is matched in the `AllNotes` collection by the provided identifier, and removed.

    There are two possible reasons a note is **saved**. The note was either just created or an existing note was changed. If the note is already in the `AllNotes` collection, it's a note that was updated. In this case, the note instance in the collection just needs to be refreshed. If the note is missing from the collection, it's a new note and must be added to the collection.

The code for the class should look like the following snippet:

:::code language="csharp" source="../snippets/viewmodel-shared/ViewModels/NotesViewModel.cs" id="full":::

## AllNotes view

Now that the viewmodel has been created, update the **AllNotes view** to point to the viewmodel properties. In the _Views\\AllNotesPage.xaml_ file, apply the following changes:

- Add the `xmlns:viewModels` XML namespace that targets the `Notes.ViewModels` .NET namespace.
- Add a `BindingContext` to the page.
- Remove the toolbar button's `Clicked` event and use the `Command` property.
- Change the `CollectionView` to bind its `ItemSource` to `AllNotes`.
- Change the `CollectionView` to use commanding to react to when the selected item changes.

Update the **AllNotes view**:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml**.
01. Paste in the following code:

    :::code language="xaml" source="../snippets/viewmodel-shared/Views/AllNotesPage.xaml" id="full" highlight="3,7,12,17,20-21":::

The toolbar no longer uses the `Clicked` event and instead uses a command.

The `CollectionView` supports commanding with the `SelectionChangedCommand` and `SelectionChangedCommandParameter` properties. In the updated XAML, the `SelectionChangedCommand` property is bound to the viewmodel's `SelectNoteCommand`, which means the command is invoked when the selected item changes. When the command is invoked, the `SelectionChangedCommandParameter` property value is passed to the command.

Look at the binding used for the `CollectionView`:

:::code language="xaml" source="../snippets/viewmodel-shared/Views/AllNotesPage.xaml" id="collection_view" highlight="5-6":::

The `SelectionChangedCommandParameter` property uses `Source={RelativeSource Self}` binding. The `Self` references the current object, which is the `CollectionView`. Notice that the binding path is the `SelectedItem` property. When the command is invoked by changing the selected item, the `SelectNoteCommand` command is invoked and the selected item is passed to the command as a parameter.

## Clean up the AllNotes code-behind

Now that the interaction with the view has changed from event handlers to commands, open the _Views\\AllNotesPage.xaml.cs_ file and replace all the code with a class that only contains the constructor:

01. In the **Solution Explorer** pane of Visual Studio, double-click on **Views\\AllNotesPage.xaml.cs**.

    > [!TIP]
    > You may need to expand **Views\\AllNotesPage.xaml** to show the file.

01. Replace the code with the following snippet:

    :::code language="csharp" source="../snippets/viewmodel-shared/Views/AllNotesPage.xaml.cs":::

## Run the app

You can now run the app, and everything is working. However, there are two problems with how the app behaves:

- If you select a note, which opens the editor, press **Save**, and then try to select the same note, it doesn't work.
- Whenever a note is changed or added, the list of notes isn't reordered to show the latest notes at the top.

These two problems are fixed in the next tutorial step.
